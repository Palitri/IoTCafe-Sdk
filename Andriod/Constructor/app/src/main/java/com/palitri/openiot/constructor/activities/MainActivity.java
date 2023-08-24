package com.palitri.openiot.constructor.activities;

import androidx.annotation.NonNull;
import androidx.annotation.Nullable;
import androidx.annotation.RequiresApi;
import androidx.appcompat.app.AlertDialog;
import androidx.core.content.ContextCompat;

import android.Manifest;
import android.content.DialogInterface;
import android.content.Intent;
import android.content.pm.PackageManager;
import android.os.Build;
import android.os.Bundle;
import android.view.Menu;
import android.view.MenuItem;
import android.view.SubMenu;
import android.view.View;
import android.widget.ArrayAdapter;
import android.widget.ListView;
import android.widget.Toast;

import com.palitri.openiot.construction.framework.board.models.BoardProperty;
import com.palitri.openiot.construction.framework.board.protocol.events.IOpenIoTProtocolEvents;
import com.palitri.openiot.construction.framework.board.protocol.events.OpenIoTEventsHandler;
import com.palitri.openiot.construction.framework.board.transmission.bluetooth.BluetoothManager;
import com.palitri.openiot.construction.framework.composite.CompositeBoardResult;
import com.palitri.openiot.construction.framework.tools.utils.ByteUtils;
import com.palitri.openiot.construction.framework.tools.utils.StringUtils;
import com.palitri.openiot.construction.framework.web.models.Preset;
import com.palitri.openiot.constructor.R;
import com.palitri.openiot.constructor.arrayadapters.PropertiesArrayAdapter;
import com.palitri.openiot.constructor.dialogs.StringInputDialog;

import java.util.HashMap;

public class MainActivity extends ActivityBase {
    private static final int ActivityResultCode_RequestEnableBluetooth = 1011;
    private static final int ActivityResultCode_SelectProject = 1012;
    private static final int ActivityResultCode_SelectBluetooth = 1013;
    private static final int ActivityResultCode_SelectPreset = 1014;
    private static final int ActivityResultCode_SetDeviceName = 1015;

    public ListView listParams;
    public View waitIconView;

    private ArrayAdapter propertiesArrayAdapter;

    @RequiresApi(api = Build.VERSION_CODES.M)
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);

        //requestWindowFeature(Window.FEATURE_CUSTOM_TITLE);
        setContentView(R.layout.activity_main);
        //getWindow().setFeatureInt(Window.FEATURE_CUSTOM_TITLE, R.layout.titlebar);

        getSupportActionBar().setDisplayShowHomeEnabled(true);
        getSupportActionBar().setIcon(R.drawable.ic_openiot_logo_32);
        getSupportActionBar().setDisplayShowTitleEnabled(false);
        //getSupportActionBar().setTitle("");


        this.waitIconView = findViewById(R.id.view_wait_icon);
        this.listParams = findViewById(R.id.list_params);

        this.setWaitMode(true);

        this.SetEventsListener();
//        this.board.boardDevice.Open();
//        this.board.requestProjectUploadSequence();

        if (ContextCompat.checkSelfPermission(this, Manifest.permission.BLUETOOTH_CONNECT) != PackageManager.PERMISSION_GRANTED) {
            requestPermissions(new String[] { Manifest.permission.BLUETOOTH_CONNECT }, 10101);
        }

    }

    public void setWaitMode(boolean isWaiting)
    {
        this.waitIconView.setVisibility(isWaiting ? View.VISIBLE :  View.GONE);
        this.listParams.setVisibility(isWaiting ? View.GONE :  View.VISIBLE);
    }

    @Override
    protected void onDestroy() {
        this.board.eventHandlers.remove(this);

        //this.board.boardDevice.Close();

        super.onDestroy();
    }

    @Override
    protected void onStart() {
        super.onStart();

        if (!this.board.isBluetoothCapable())
        {
            Toast.makeText(getApplicationContext(), getResources().getText(R.string.bluetooth_enable_not_supported), Toast.LENGTH_LONG).show();
            this.requestAppClose();
            return;
        }

        if (!this.board.isBluetoothEnabled())
        {
            this.startActivityEnableBluetooth();
            return;
        }

        if (!this.board.IsConnected())
        {
            if (this.board.ConnectToBoard() != CompositeBoardResult.Ok)
                this.startActivitySelectBluetooth();
            return;
        }
    }

    @Override
    protected void onStop() {
        super.onStop();
    }

    @Override
    public boolean onCreateOptionsMenu(Menu menu) {
        // Inflate the menu; this adds items to the action bar if it is present.
        getMenuInflater().inflate(R.menu.main_menu, menu);

        return true;
    }

    @Override
    public boolean onPrepareOptionsMenu(Menu menu) {
        MenuItem presetsMenuItem = menu.findItem(R.id.menu_item_presets);
        SubMenu presetsSubmenu = presetsMenuItem.getSubMenu();

        int presetsCount = this.board.presets.size();
        for (int i = 0; i < presetsCount; i++)
            presetsSubmenu.add(1, i, 0, this.board.presets.get(i).name);

        return super.onPrepareOptionsMenu(menu);
    }

    @Override
    public boolean onOptionsItemSelected(@NonNull MenuItem item) {
        int groupId = item.getGroupId();

        if (groupId == 1)
        {
            int presetIndex = item.getItemId();
            Preset preset = this.board.presets.get(presetIndex);
            this.board.ApplyPreset(preset);
        }
        else {
            switch (item.getItemId()) {
                case R.id.menu_item_projects_list: {
                    this.startActivitySelectProject();
                    break;
                }

                case R.id.menu_item_update_credentials: {
                    this.startActivity(new Intent(MainActivity.this, LoginActivity.class));
                    break;
                }

                case R.id.menu_item_presets_list: {
                    this.startActivitySelectPreset();

                    break;
                }

                case R.id.menu_item_name: {
                    this.board.boardDevice.requestDeviceName();
                    break;
                }

                case R.id.menu_item_info: {
                    this.board.boardDevice.requestBoardInfo();
                    break;
                }

                case R.id.menu_item_bluetooth_list: {
                    this.startActivitySelectBluetooth();
                    break;
                }
            }
        }

        return super.onOptionsItemSelected(item);
    }

    @Override
    protected void onActivityResult(int requestCode, int resultCode, @Nullable Intent data)
    {
        super.onActivityResult(requestCode, resultCode, data);

        switch (requestCode) {
            case ActivityResultCode_RequestEnableBluetooth:
                this.finishedActivityEnableBluetooth(resultCode == RESULT_OK);
                break;

            case ActivityResultCode_SelectProject:
                this.finishedActivitySelectProject(resultCode == RESULT_OK);
                break;

            case ActivityResultCode_SelectBluetooth:
                this.finishedActivitySelectBluetooth(resultCode == RESULT_OK);
                break;

            case ActivityResultCode_SelectPreset:
                this.finishedActivitySelectPreset(resultCode == RESULT_OK);
                break;

            case ActivityResultCode_SetDeviceName:
                this.finishedActivitySetDeviceName(resultCode == RESULT_OK);
                break;
        }
    }

//    // Register the permissions callback, which handles the user's response to the
//// system permissions dialog. Save the return value, an instance of
//// ActivityResultLauncher, as an instance variable.
//    private ActivityResultLauncher<String> requestPermissionLauncher =
//            registerForActivityResult(new RequestPermission(), isGranted -> {
//                if (isGranted) {
//                    // Permission is granted. Continue the action or workflow in your
//                    // app.
//                } else {
//                    // Explain to the user that the feature is unavailable because the
//                    // features requires a permission that the user has denied. At the
//                    // same time, respect the user's decision. Don't link to system
//                    // settings in an effort to convince the user to change their
//                    // decision.
//                }
//            });

    private void startActivityEnableBluetooth()
    {
        if (ContextCompat.checkSelfPermission(this, Manifest.permission.BLUETOOTH_CONNECT) == PackageManager.PERMISSION_GRANTED)
        {
            this.board.requestBluetoothEnable(this, ActivityResultCode_RequestEnableBluetooth);
        }
        else
        {
            //requestPermissionLauncher.launch(Manifest.permission.BLUETOOTH_CONNECT);
        }
}

    private void finishedActivityEnableBluetooth(boolean success)
    {
    }


    private void startActivitySelectBluetooth() {
        Intent intent = new Intent(this, SelectBluetoothActivity.class);
        this.startActivityForResult(intent, ActivityResultCode_SelectBluetooth);
    }

    private void finishedActivitySelectBluetooth(boolean success) {
    }

    private void startActivitySelectProject()
    {
        this.startActivityForResult(new Intent(this, SelectProjectActivity.class), ActivityResultCode_SelectProject);
    }

    private void finishedActivitySelectProject(boolean success)
    {
        if (success) {
            this.invalidateOptionsMenu();
            this.setWaitMode(true);
            this.board.requestProjectUploadSequence();
        }
    }



    private void startActivitySelectPreset()
    {
        this.startActivityForResult(new Intent(this, SelectPresetActivity.class), ActivityResultCode_SelectPreset);
    }

    private void finishedActivitySelectPreset(boolean success)
    {
        this.invalidateOptionsMenu();
    }


    private void startActivitySetDeviceName(String deviceName)
    {
        new StringInputDialog(this, deviceName, (String) getResources().getText(R.string.title_set_device_name))
        {
            @Override
            public void setValue(String value) {
                MainActivity.this.board.boardDevice.requestSetDeviceName(value);
                super.setValue(value);
            }
        };
    }

    private void finishedActivitySetDeviceName(boolean success)
    {

    }


    private void requestAppClose()
    {
        this.finishAffinity();

    }

    // Bluetooth stuff


    private void showBluetoothActivationDialog()
    {
        final MainActivity activity = this;

        final AlertDialog.Builder dlgAlert = new AlertDialog.Builder(this);
        dlgAlert.setMessage(getResources().getText(R.string.bluetooth_enable_question));
        dlgAlert.setTitle(getResources().getText(R.string.bluetooth_enable_title));
        dlgAlert.setCancelable(false);
        dlgAlert.setPositiveButton(getResources().getText(R.string.button_enable),
                new DialogInterface.OnClickListener()
                {
                    @Override
                    public void onClick(DialogInterface dialogInterface, int i)
                    {
                        BluetoothManager.requestEnable(activity, ActivityResultCode_RequestEnableBluetooth);
                    }
                });
        dlgAlert.setNegativeButton(getResources().getText(R.string.button_cancel),
                new DialogInterface.OnClickListener()
                {
                    @Override
                    public void onClick(DialogInterface dialog, int which)
                    {
                        Intent homeIntent = new Intent(Intent.ACTION_MAIN);
                        homeIntent.addCategory( Intent.CATEGORY_HOME );
                        homeIntent.setFlags(Intent.FLAG_ACTIVITY_CLEAR_TOP);
                        startActivity(homeIntent);
                        activity.requestAppClose();
                    }
                });
        dlgAlert.create().show();
    }

//    private void initiateBoardConnection() {
//        if (this.getConstructorApplication().getBoard() != null)
//            return;
//
//        if (!this.board.bluetoothManager.IsBluetoothCapable()) {
//            Toast.makeText(getApplicationContext(), getResources().getText(R.string.bluetooth_enable_not_supported), Toast.LENGTH_LONG).show();
//            this.requestAppClose();
//            return;
//        }
//
//        if (!this.board.bluetoothManager.IsEnabled())
//            this.startActivityEnableBluetooth();
//        else
//            this.createBoardWithBluetooth();
//    }

//    private void createBoardWithBluetooth()
//    {
//
//    }

//    private void requestBoardPingWithTime()
//    {
//        java.util.Date time = new java.util.Date();
//        this.board.boardDevice.requestPing(ByteUtils.fromInt64(time.getTime()));
//    }



    // IOpenCNCBoardEvents

    public void SetEventsListener()
    {
        this.board.eventHandlers.add(new OpenIoTEventsHandler()
        {
            @Override
            public void onPingBack(Object sender, byte[] data) {
                if (data != null && data.length >= 8) {
                    long timeNow = new java.util.Date().getTime();
                    long timePing = ByteUtils.toInt64(data, 0);
                    long timeRoundtrip = timeNow - timePing;

                    MainActivity.this.runOnUiThread(new Runnable() {
                        public void run() { Toast.makeText(MainActivity.this, String.valueOf(timeRoundtrip), Toast.LENGTH_SHORT).show(); }
                    });
                }
            }

            @Override
            public void onAllPropertiesInfoReceived(Object sender) {
                final MainActivity activity = MainActivity.this;

                activity.runOnUiThread(new Runnable() {
                    public void run() {
                        activity.propertiesArrayAdapter = new PropertiesArrayAdapter(activity, activity.board.visibleProperties);
                        activity.listParams.setAdapter(activity.propertiesArrayAdapter);
                        activity.propertiesArrayAdapter.notifyDataSetChanged();

                        activity.setWaitMode(false);
                    }
                });
            }

            @Override
            public void onSubscribedPropertyValueChanged(Object sender, BoardProperty p, Object oldValue) {
                MainActivity.this.runOnUiThread(new Runnable() {
                    public void run() {
                        if (MainActivity.this.propertiesArrayAdapter != null)
                            MainActivity.this.propertiesArrayAdapter.notifyDataSetChanged();
                    }
                });
            }

            @Override
            public void onInfoReceived(Object sender, final String info) {
                MainActivity.this.runOnUiThread(new Runnable() {
                    public void run() {
                        Toast.makeText(MainActivity.this, info, Toast.LENGTH_SHORT).show();
                    }
                });
            }

            @Override
            public void onReset(Object sender) {
                MainActivity.this.runOnUiThread(new Runnable() {
                    public void run() {
                        Toast.makeText(MainActivity.this, getResources().getText(R.string.event_logic_reset), Toast.LENGTH_SHORT).show();
                    }
                });
            }

            @Override
            public void onSchemeLogicUploaded(Object sender) {
                MainActivity.this.runOnUiThread(new Runnable() {
                    public void run() {
                        Toast.makeText(MainActivity.this, getResources().getText(R.string.event_scheme_logic_uploaded), Toast.LENGTH_SHORT).show();
                    }
                });
            }

            @Override
            public void onProgramLogicUploaded(Object sender) {
                MainActivity.this.runOnUiThread(new Runnable() {
                    public void run() {
                        Toast.makeText(MainActivity.this, getResources().getText(R.string.event_program_logic_uploaded), Toast.LENGTH_SHORT).show();
                    }
                });
            }

            @Override
            public void onPropertiesChangedSubscriptionReset(Object sender) {
                MainActivity.this.runOnUiThread(new Runnable() {
                    public void run() { Toast.makeText(MainActivity.this, getResources().getText(R.string.event_properties_subscription_reset), Toast.LENGTH_SHORT).show(); }
                });
            }

            @Override
            public void onDeviceNameReceived(Object sender, String name) {
                MainActivity.this.runOnUiThread(new Runnable() {
                    public void run() {
                        MainActivity.this.startActivitySetDeviceName(name);
                    }
                });
            }
        });
    }
}