package com.palitri.openiot.construction.framework.board.transmission.bluetooth;

import android.app.Activity;
import android.bluetooth.BluetoothAdapter;
import android.bluetooth.BluetoothDevice;
import android.content.Intent;
import android.util.Log;

import java.util.Set;

public class BluetoothManager
{
    private final BluetoothAdapter mmAdapter;

    public BluetoothManager()
    {
         mmAdapter = BluetoothAdapter.getDefaultAdapter();
    }

    public BluetoothAdapter getAdapter()
    {
        return mmAdapter;
    }

    public boolean IsBluetoothCapable()
    {
        return mmAdapter != null;
    }

    public boolean IsEnabled()
    {
        return mmAdapter.isEnabled();
    }

    public static void requestEnable(Activity requestingActivity, int activityResultCode)
    {
        Intent enableBtIntent = new Intent(BluetoothAdapter.ACTION_REQUEST_ENABLE);
        requestingActivity.startActivityForResult(enableBtIntent, activityResultCode);
    }

    public void enable()
    {
        this.mmAdapter.enable();
    }

    public void disable()
    {
        this.mmAdapter.disable();
    }

    public Set<BluetoothDevice> getPairedDevices()
    {
        try {
            return this.mmAdapter.getBondedDevices();
        }
        catch (Exception ex)
        {
            Log.e("BluetoothManager", "getPairedDevices: " + ex.getMessage());
            return null;
        }
    }

    public BluetoothDevice getPairedDeviceByName(String name)
    {
        Set<BluetoothDevice> pairedDevices = getPairedDevices();

        for (BluetoothDevice device : pairedDevices)
        {
            String deviceName = device.getName();
            if (deviceName.equals(name))
                return device;
        }

        return null;

//        return this.getPairedDevices().stream()
//                .filter((device) -> device.getName().equals(name))
//                .findFirst()
//                .orElse(null);
    }

    public BluetoothDevice getPairedDeviceByAddress(String mac)
    {
        Set<BluetoothDevice> pairedDevices = getPairedDevices();

        for (BluetoothDevice device : pairedDevices)
        {
            if (device.getAddress().equals(mac))
                return device;
        }

        return null;
    }

    public String getName()
    {
        return this.mmAdapter != null ? this.mmAdapter.getName() : "";
    }


}
