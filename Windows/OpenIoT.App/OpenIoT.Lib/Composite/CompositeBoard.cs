using OpenIoT.Lib.Board.Api;
using OpenIoT.Lib.Board.Models;
using OpenIoT.Lib.Board.Protocol;
using OpenIoT.Lib.Board.Protocol.Events;
using OpenIoT.Lib.Board.Scanner;
using OpenIoT.Lib.Board.Transmission;
using OpenIoT.Lib.Board.Transmission.Com;
using OpenIoT.Lib.SoftwarePeripherals;
using OpenIoT.Lib.Tools.Persistence;
using OpenIoT.Lib.Tools.Threading;
using OpenIoT.Lib.Tools.Utils;
using OpenIoT.Lib.Web.Models;
using OpenIoT.Lib.Web.Models.Configurations.Presets;
using OpenIoT.Lib.Web.Models.Configurations.Project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace OpenIoT.Lib.Composite
{
    public delegate void OnConnected(CompositeBoard sender, OpenIoTBoard board);
    public delegate void OnDisconnected(CompositeBoard sender);

    public class CompositeBoard : OpenIoTProtocolEventsHandler, IDisposable
    {
        public OpenIoTBoard boardDevice;
        //public BluetoothManager bluetoothManager = null;
        //public ITransmissionChannel transmissionChannel;

        public Project project;
        public PresetsCollection presets;

        public List<CompositeProperty> properties;
        public List<CompositeProperty> visibleProperties;

        public List<IPropertyTransmissionProtocolEvents> eventHandlers;

        public CompositeBoardPersistence persistence;

        private ContinuousThread? backgroundThread;

        public OnConnected OnConnected;
        public OnDisconnected OnDisconnected;

        public BoardScanInfo info;

        public bool IsConnected { get; private set; }

//        public bool AutoConnect { get { return this.backgroundThread.IsRunning(); } set { this.backgroundThread.Start(); } }

        public CompositeBoard()
        {
            this.IsConnected = false;

            //this.bluetoothManager = new BluetoothManager();

            this.project = new Project();

            this.properties = new List<CompositeProperty>();

            this.presets = new PresetsCollection();

            this.persistence = new CompositeBoardPersistence(new DefaultPersistence("OpenIoTPreferences.json"));

            //this.RestoreFromPersistence();

            this.boardDevice = new OpenIoTBoard();

            this.backgroundThread = new ContinuousThread(this.AutoConnectPortTask, 100);
            //this.backgroundThread.Start();

            this.eventHandlers = new List<IPropertyTransmissionProtocolEvents>();

            this.info = new BoardScanInfo();

            this.LoadProject(this.persistence.GetProject(), this.persistence.GetPresets());
        }

        public void AddEventHandler(IOpenIoTProtocolEvents eventHandler)
        {
            this.eventHandlers.Add(eventHandler);
            this.boardDevice.EventHandlers.Add(eventHandler);
        }

        public void RemoveEventHandler(IOpenIoTProtocolEvents eventHandler)
        {
            this.eventHandlers.Remove(eventHandler);
            this.boardDevice.EventHandlers.Remove(eventHandler);
        }

        public void onConnected(OpenIoTBoard board)
        {
            if (this.OnConnected != null)
                this.OnConnected.Invoke(this, board);
        }

        public void onDisconnected()
        {
            if (this.OnDisconnected != null)
                this.OnDisconnected.Invoke(this);
        }

        public void ConnectToPort(string portName)
        {
            this.SetBoard(new OpenIoTBoard(new ComTransmissionChannel(portName)));
        }

        public void SetBoard(OpenIoTBoard boardDevice)
        {
            this.boardDevice = boardDevice;
            this.boardDevice.EventHandlers.Add(this);
            this.boardDevice.EventHandlers.AddRange(this.eventHandlers);
            this.boardDevice.Open();

            this.IsConnected = true;
            this.onConnected(boardDevice);
        }

        public void DisconnectFromBoard()
        {
            if (this.boardDevice != null)
                this.boardDevice.Close();

            this.IsConnected = false;
            
            this.onDisconnected();
        }

        public void LoadProject(Project project, PresetsCollection projectPresets)
        {
            if (project != null)
            {
                this.project = project;
                this.persistence.SetProject(this.project);

                this.CreateSoftwarePeripherals(this.project);
            }

            if (projectPresets != null)
            {
                this.presets = projectPresets;
                this.persistence.SetPresets(this.presets);
            }
        }

        public SoftwarePeripheralsManager SoftwarePeripherals { get; set; }

        public void CreateSoftwarePeripherals(Project project)
        {
            if (this.SoftwarePeripherals == null)
                return;

            foreach (Peripheral peripheral in project.BoardConfig.Peripherals)
                this.SoftwarePeripherals.AddPeripheral(peripheral);
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

        public void ApplyPreset(Preset preset)
        {
            List<BoardProperty> updateProperties = new List<BoardProperty>();

            foreach (PresetProperty presetProperty in preset.Config.Properties)
            {
                foreach (CompositeProperty property in this.properties)
                {
                    if (presetProperty.ScriptId.Equals(property.ParentPeripheral.ScriptId + "." + property.PeripheralProperty.ScriptId))
                    {
                        updateProperties.Add(new BoardProperty(property.BoardProperty, presetProperty.Value));
                    }
                }
            }

            this.boardDevice.RequestPropertiesUpdate(updateProperties.ToArray());
        }

        public void requestProjectUploadSequence()
        {
            //this.boardDevice.resetLogic();
            //this.boardDevice.requestPropertiesChangedSubscriptionReset();
            this.boardDevice.requestSetProjectDetails(this.project.ProjectId, this.project.Name, this.project.UserId);
        }

        public void RequestVisiblePropertiesChangeSubscription()
        {
            int visiblePropertiesCount = this.visibleProperties.Count;
            byte[] subscriptionPropertiesIndices = new byte[visiblePropertiesCount];
            for (int i = 0; i < visiblePropertiesCount; i++)
                subscriptionPropertiesIndices[i] = (byte)this.visibleProperties[i].BoardProperty.index;

            this.boardDevice.requestPropertiesChangedSubscription(subscriptionPropertiesIndices);
        }

        private List<CompositeProperty> MergeDeviceAndConfigurationProperties()
        {
            Dictionary<int, BoardProperty> boardPropertiesMap = new Dictionary<int, BoardProperty>();
            foreach (BoardProperty boardProperty in this.boardDevice.properties)
                boardPropertiesMap.Add(boardProperty.semantic, boardProperty);

            List<CompositeProperty> result = new List<CompositeProperty>();

            foreach (PeripheralProperty configProperty in this.project.BoardConfig.Properties)
            {
                if (boardPropertiesMap.ContainsKey(configProperty.Semantic))
                    result.Add(new CompositeProperty()
                        {
                            BoardProperty = boardPropertiesMap[configProperty.Semantic],
                            PeripheralProperty = configProperty,
                            ParentPeripheral = this.project.BoardConfig.GetPropertyPeripheral(configProperty.Semantic),
                            Board = this
                        }
                    ); 
            }

            return result;
        }

        public bool AutoConnectPortTask()
        {
            try
            {

                if (this.IsConnected)
                {
                    if (!this.boardDevice.transmissionChannel.IsOpen())
                        this.DisconnectFromBoard();

                    Thread.Sleep(500);
                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        private List<CompositeProperty> GetVisibleProperties()
        {
            return this.properties.Where(p => p.PeripheralProperty.Visible).ToList();
        }

        public Preset SnapshotProjectPreset(string name)
        {
            Preset result = new Preset();
            result.Name = name;
            result.ProjectId = this.project.ProjectId;

            foreach (CompositeProperty property in this.visibleProperties)
            {
                if ((property.BoardProperty.flags & BoardPropertyFlags.Write) != 0)
                    result.Config.Properties.Add(new PresetProperty()
                {
                    Type = (PeripheralPropertyType)property.BoardProperty.type,
                    Value = property.BoardProperty.value,
                    ScriptId = property.ParentPeripheral.ScriptId + "." + property.PeripheralProperty.ScriptId,
                });
            }

            return result;
        }

        public String GenerateUniquePresetName(String suggestedName)
        {
            if (StringUtils.IsNullOrEmpty(suggestedName))
                suggestedName = this.project == null ? "Preset" : this.project.Name + " Preset";

            String result = suggestedName;
            int index = 1;
            bool found = false;
            while (!found)
            {
                found = true;
                foreach (Preset preset in this.presets)
                {
                    if (preset.Name.Equals(result))
                    {
                        result = suggestedName + " " + index++;
                        found = false;
                        break;
                    }
                }
            }

            return result;
        }

        #region OpenIoT Events

        public override void onAllPropertiesInfoReceived(object sender)
        {
            this.properties = this.MergeDeviceAndConfigurationProperties();
            this.visibleProperties = this.GetVisibleProperties();

            this.RequestVisiblePropertiesChangeSubscription();
        }

        public override void onDevicePropertiesSet(object sender, Dictionary<int, byte[]> properties)
        {
            if (properties.ContainsKey(OpenIoTProtocol.DevicePropertyId_ProjectUid))
            {
                this.boardDevice.uploadSchemeLogic(this.project.GetCompiledSchemeCode());
            }
        }

        public override void onSchemeLogicUploaded(object sender)
        {
            this.boardDevice.uploadProgramLogic(this.project.GetCompiledScriptCode());
        }

        public override void onProgramLogicUploaded(object sender)
        {
            this.boardDevice.requestProperties();
        }

        #endregion

        public void Dispose()
        {
            this.backgroundThread.Terminate(true);
        }

    }
}
