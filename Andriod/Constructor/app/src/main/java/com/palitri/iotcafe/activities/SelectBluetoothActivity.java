package com.palitri.iotcafe.activities;

import android.bluetooth.BluetoothDevice;
import android.os.Bundle;
import android.view.View;
import android.widget.ArrayAdapter;
import android.widget.LinearLayout;
import android.widget.ListView;
import android.widget.TextView;

import com.palitri.openiot.construction.framework.composite.CompositeBoardResult;
import com.palitri.iotcafe.R;
import com.palitri.iotcafe.arrayadapters.BasicItemArrayAdapter;

import java.util.ArrayList;

public class SelectBluetoothActivity extends ActivityBase {

    public LinearLayout contents;
    public View waitIconView;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);

        setContentView(R.layout.activity_select_device);

        getSupportActionBar().setDisplayShowHomeEnabled(true);
        getSupportActionBar().setIcon(R.drawable.ic_baseline_bluetooth_24);
        getSupportActionBar().setDisplayShowTitleEnabled(true);
        getSupportActionBar().setTitle("Devices");

        this.waitIconView = findViewById(R.id.view_wait);
        this.contents = findViewById(R.id.layout_contents);

        this.showWaitingScreen(true);

        ListView listParams = findViewById(R.id.list_devices);

        ArrayList<BluetoothDevice> bluetoothDevices = new ArrayList<BluetoothDevice>(this.getBoard().bluetoothManager.getPairedDevices());

        ArrayAdapter adapter =  new BasicItemArrayAdapter<BluetoothDevice>(this, bluetoothDevices) {
            @Override
            public void SetView(TextView view, BluetoothDevice bluetoothDevice) {
                view.setText(bluetoothDevice.getName());
            }

            @Override
            public void OnItemClick(BluetoothDevice bluetoothDevice) {
                SelectBluetoothActivity.this.showWaitingScreen(true);

                new Thread() {
                    @Override
                    public void run() {
                        CompositeBoardResult result = SelectBluetoothActivity.this.board.ConnectToBoard(bluetoothDevice.getName());
                        if (result == CompositeBoardResult.Ok)
                            SelectBluetoothActivity.this.FinishActivity(null, ActivityResult_OK);

                        SelectBluetoothActivity.this.FinishActivity(null, ActivityResult_Error);
                    }
                }.start();
            }
        };

        listParams.setAdapter(adapter);
        adapter.notifyDataSetChanged();

        this.showWaitingScreen(false);
    }

    public void showWaitingScreen(boolean isWaiting)
    {
        SelectBluetoothActivity.this.runOnUiThread(new Runnable() {
            public void run() {
                SelectBluetoothActivity.this.waitIconView.setVisibility(isWaiting ? View.VISIBLE :  View.GONE);
                SelectBluetoothActivity.this.contents.setVisibility(isWaiting ? View.GONE :  View.VISIBLE);
            }
        });
    }


}