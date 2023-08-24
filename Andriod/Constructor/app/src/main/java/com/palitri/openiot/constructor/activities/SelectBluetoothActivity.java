package com.palitri.openiot.constructor.activities;

import android.bluetooth.BluetoothDevice;
import android.os.Bundle;
import android.widget.ArrayAdapter;
import android.widget.ListView;
import android.widget.TextView;

import com.palitri.openiot.constructor.R;
import com.palitri.openiot.constructor.arrayadapters.BasicItemArrayAdapter;

import java.util.ArrayList;

public class SelectBluetoothActivity extends ActivityBase {

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_select_device);

        getSupportActionBar().setDisplayShowHomeEnabled(true);
        getSupportActionBar().setIcon(R.drawable.ic_baseline_bluetooth_24);
        getSupportActionBar().setDisplayShowTitleEnabled(true);
        getSupportActionBar().setTitle("Devices");

        ListView listParams = findViewById(R.id.list_devices);

        ArrayList<BluetoothDevice> bluetoothDevices = new ArrayList<BluetoothDevice>(this.getBoard().bluetoothManager.getPairedDevices());

        ArrayAdapter adapter =  new BasicItemArrayAdapter<BluetoothDevice>(this, bluetoothDevices) {
            @Override
            public void SetView(TextView view, BluetoothDevice bluetoothDevice) {
                view.setText(bluetoothDevice.getName());
            }

            @Override
            public void OnItemClick(BluetoothDevice bluetoothDevice) {
                SelectBluetoothActivity.this.board.ConnectToBoard(bluetoothDevice.getName());
                SelectBluetoothActivity.this.finish();
            }
        };

        listParams.setAdapter(adapter);
        adapter.notifyDataSetChanged();
    }
}