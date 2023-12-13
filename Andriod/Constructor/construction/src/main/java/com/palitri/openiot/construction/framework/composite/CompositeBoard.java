package com.palitri.openiot.construction.framework.composite;

import android.app.Activity;
import android.bluetooth.BluetoothDevice;
import android.content.Context;
import android.util.Log;

import com.palitri.openiot.construction.framework.board.api.OpenIotBoard;
import com.palitri.openiot.construction.framework.board.models.BoardProperty;
import com.palitri.openiot.construction.framework.board.models.BoardPropertyFlags;
import com.palitri.openiot.construction.framework.board.protocol.OpenIoTProtocol;
import com.palitri.openiot.construction.framework.board.protocol.events.OpenIoTEventsHandler;
import com.palitri.openiot.construction.framework.board.transmission.bluetooth.BluetoothTransmissionChannel;
import com.palitri.openiot.construction.framework.board.protocol.events.IOpenIoTProtocolEvents;
import com.palitri.openiot.construction.framework.board.transmission.ITransmissionChannel;
import com.palitri.openiot.construction.framework.tools.persistence.DefaultPersistence;
import com.palitri.openiot.construction.framework.board.transmission.bluetooth.BluetoothManager;
import com.palitri.openiot.construction.framework.tools.utils.StringUtils;
import com.palitri.openiot.construction.framework.web.models.PresetsCollection;
import com.palitri.openiot.construction.framework.web.models.Project;
import com.palitri.openiot.construction.framework.web.models.configurations.project.PeripheralPropertyType;
import com.palitri.openiot.construction.framework.web.models.configurations.project.PeripheralProperty;
import com.palitri.openiot.construction.framework.web.models.Preset;
import com.palitri.openiot.construction.framework.web.models.configurations.presets.PresetProperty;

import java.util.ArrayList;
import java.util.HashMap;

public class CompositeBoard extends OpenIoTEventsHandler {
    private static final String TAG = "CompositeBoard";

    private boolean isWaitingResponse = false;

    public OpenIotBoard boardDevice;
    public BluetoothManager bluetoothManager = null;
    public ITransmissionChannel transmissionChannel;

    public Project project;
    public PresetsCollection presets;

    public ArrayList<CompositeProperty> properties;
    public ArrayList<CompositeProperty> visibleProperties;

    public ArrayList<IOpenIoTProtocolEvents> eventHandlers;

    public CompositeBoardPersistence persistence;

    public CompositeBoard(Context context)
    {
        this.bluetoothManager = new BluetoothManager();

        this.project = new Project();

        this.properties = new ArrayList<CompositeProperty>();

        this.presets = new PresetsCollection();

        this.eventHandlers = new ArrayList<IOpenIoTProtocolEvents>();

        this.persistence = new CompositeBoardPersistence(new DefaultPersistence(context, "OpenIoTPreferences"));

        this.RestoreFromPersistence();
    }

    public void AddEventHandler(IOpenIoTProtocolEvents eventHandler)
    {
        this.eventHandlers.add(eventHandler);
        this.boardDevice.eventHandlers.add(eventHandler);
    }

    public void RemoveEventHandler(IOpenIoTProtocolEvents eventHandler)
    {
        this.eventHandlers.remove(eventHandler);
        this.boardDevice.eventHandlers.remove(eventHandler);
    }

    public CompositeBoardResult ConnectToBoard()
    {
        return this.ConnectToBoard(this.persistence.GetDeviceName());
    }

    public CompositeBoardResult ConnectToBoard(String deviceName)
    {
        if (StringUtils.IsNullOrEmpty(deviceName))
            return CompositeBoardResult.DeviceNotSet;

        BluetoothDevice bluetoothDevice = this.bluetoothManager.getPairedDeviceByName(deviceName);
        if (bluetoothDevice == null)
            return CompositeBoardResult.DeviceNotFound;

        if (this.boardDevice != null) {
            this.boardDevice.Close();
            this.boardDevice = null;
        }

        this.transmissionChannel = new BluetoothTransmissionChannel(bluetoothDevice, this.bluetoothManager.getAdapter());

        this.boardDevice = new OpenIotBoard(transmissionChannel);
        this.boardDevice.eventHandlers.add(this);
        this.boardDevice.eventHandlers.addAll(this.eventHandlers);

        if (!this.boardDevice.Open())
        {
            return CompositeBoardResult.DeviceCannotConnect;
        }

        if (!this.RequestResponse()) {
            this.boardDevice.Close();
            return CompositeBoardResult.DeviceNotResponding;
        }

        this.persistence.SetDeviceName(deviceName);

        return CompositeBoardResult.Ok;
    }

    public void DisconnectFromBoard()
    {
        if (this.boardDevice != null)
            this.boardDevice.Close();
    }

    public boolean IsConnected()
    {
        return this.boardDevice != null && this.transmissionChannel != null && this.transmissionChannel.isOpened();
    }

    public boolean IsProjectLoaded()
    {
        return this.project != null;
    }

    public boolean isBluetoothCapable()
    {
        return this.bluetoothManager.IsBluetoothCapable();
    }

    public boolean isBluetoothEnabled()
    {
        return this.bluetoothManager.IsEnabled();
    }

    public void requestBluetoothEnable(Activity requestingActivity, int activityResultCode)
    {
        BluetoothManager.requestEnable(requestingActivity, activityResultCode);
    }

    public void ClearLoadedProject()
    {
        this.project = null;
        this.persistence.SetProject(null);

        this.presets.clear();
        this.persistence.SetPresets(null);
    }

    public void LoadProject(Project project, PresetsCollection projectPresets)
    {
        if (project != null)
        {
            this.project = project;
            this.persistence.SetProject(this.project);
        }

        if (projectPresets != null) {
            this.presets = projectPresets;
            this.persistence.SetPresets(this.presets);
        }
    }

    public void RestoreFromPersistence()
    {
        Project project = this.persistence.GetProject();
        if (project != null)
            this.project = project;

        PresetsCollection projectPresets = this.persistence.GetPresets();
        if (projectPresets != null)
            this.presets = projectPresets;
    }

    public String GenerateUniquePresetName(String suggestedName)
    {
        if (StringUtils.IsNullOrEmpty(suggestedName))
            suggestedName = this.project == null ? "Preset" : this.project.name + " Preset";

        String result = suggestedName;
        int index = 1;
        boolean found = false;
        while (!found) {
            found = true;
            for (Preset preset : this.presets) {
                if (preset.name.equals(result)) {
                    result = suggestedName + " " + index++;
                    found = false;
                    break;
                }
            }
        }

        return result;
    }

    public void ApplyPreset(Preset preset)
    {
        ArrayList<BoardProperty> updateProperties = new ArrayList<BoardProperty>();

        for (PresetProperty presetProperty : preset.config.properties)
        {
            for (CompositeProperty property : this.properties)
            {
                if (presetProperty.scriptId.equals(property.parentPeripheral.scriptId + "." + property.peripheralProperty.scriptId))
                {
                    updateProperties.add(new BoardProperty(property.boardProperty, presetProperty.value));
                }
            }
        }

        this.boardDevice.RequestPropertiesUpdate(updateProperties.toArray(new BoardProperty[updateProperties.size()]));
    }


    public void requestProjectUploadSequence()
    {
        //this.boardDevice.resetLogic();
        //this.boardDevice.requestPropertiesChangedSubscriptionReset();
        //this.boardDevice.uploadSchemeLogic(this.project.getCompiledSchemeCode());
        this.boardDevice.requestSetProjectDetails(this.project.projectId, this.project.name);
    }

    public void requestVisiblePropertiesChangeSubscription()
    {
        int visiblePropertiesCount = this.visibleProperties.size();
        byte[] subscriptionPropertiesIndices = new byte[visiblePropertiesCount];
        for (int i = 0; i < visiblePropertiesCount; i++)
            subscriptionPropertiesIndices[i] = (byte)this.visibleProperties.get(i).boardProperty.index;

        this.boardDevice.requestPropertiesChangedSubscription(subscriptionPropertiesIndices);
    }

    private ArrayList<CompositeProperty> MergeDeviceAndConfigurationProperties()
    {
        final HashMap<Integer, BoardProperty> boardPropertiesMap = new HashMap<Integer, BoardProperty>();
        for (BoardProperty boardProperty: this.boardDevice.properties)
            boardPropertiesMap.put(boardProperty.semantic, boardProperty);

        ArrayList<CompositeProperty> result = new ArrayList<CompositeProperty>();

        for (final PeripheralProperty configProperty: this.project.boardConfig.properties)
        {
            if (boardPropertiesMap.containsKey(configProperty.semantic))
                result.add(new CompositeProperty()
                {{
                    boardProperty = boardPropertiesMap.get(configProperty.semantic);
                    peripheralProperty = configProperty;
                    parentPeripheral = CompositeBoard.this.project.boardConfig.GetPropertyPeripheral(configProperty.semantic);
                }});
        }

        return result;
    }

    private ArrayList<CompositeProperty> GetVisibleProperties()
    {
        //return this.properties.stream().filter((p) -> p.configurationProperty.visible).toArray(); // Requires API 24 (Android 7)

        ArrayList<CompositeProperty> result = new ArrayList<CompositeProperty>();

        for (CompositeProperty p: this.properties)
            if (p.peripheralProperty.visible)
                result.add(p);

        return result;
    }

    public Preset SnapshotProjectPreset(String name)
    {
        Preset result = new Preset();
        result.name = name;
        result.projectId = this.project.projectId;

        for (final CompositeProperty  property : this.visibleProperties)
        {
            if ((property.boardProperty.flags & BoardPropertyFlags.Write) != 0)
                result.config.properties.add(new PresetProperty()
                {{
                    type = PeripheralPropertyType.FromValue(property.boardProperty.type.value);
                    value = property.boardProperty.value;
                    scriptId = property.parentPeripheral.scriptId + "." + property.peripheralProperty.scriptId;
                }});
        }

        return result;
    }



    public boolean RequestResponse()
    {
        this.isWaitingResponse = true;
        this.boardDevice.requestBoardInfo();

        try {
            Thread thread = new Thread() {
                @Override
                public void run() {
                    for (int i = 0; i < 10; i++) {
                        if (CompositeBoard.this.isWaitingResponse) {
                            try {
                                Thread.sleep(100);
                            } catch (InterruptedException e) {
                                break;
                            }
                        }
                    }
                }
            };

            thread.start();
            thread.join();
        } catch (InterruptedException e) {
            e.printStackTrace();
        }


        return !this.isWaitingResponse;
    }


    // ICompositeBoardEvents >>>

    @Override
    public void onAllPropertiesInfoReceived(Object sender) {
        Log.d(TAG, "onAllPropertiesInfoReceived");

        this.properties = this.MergeDeviceAndConfigurationProperties();
        this.visibleProperties = this.GetVisibleProperties();

        this.requestVisiblePropertiesChangeSubscription();
    }

    @Override
    public void onDevicePropertiesSet(Object sender, HashMap<Integer, byte[]> properties)
    {
        if (properties.containsKey(OpenIoTProtocol.DevicePropertyId_ProjectUid))
        {
            this.boardDevice.uploadSchemeLogic(this.project.getCompiledSchemeCode());
        }
    }

    @Override
    public void onSchemeLogicUploaded(Object sender) {
        Log.d(TAG, "onSchemeLogicUploaded");
        this.boardDevice.uploadProgramLogic(this.project.getCompiledScriptCode());
    }

    @Override
    public void onProgramLogicUploaded(Object sender) {
        Log.d(TAG, "onProgramLogicUploaded");
        this.boardDevice.requestProperties();
    }

    @Override
    public void onInfoReceived(Object sender, String info) {
        this.isWaitingResponse = false;
    }
    // ICompositeBoardEvents <<<

}
