package com.palitri.openiot.construction.framework.board.transmission.bluetooth;

import android.Manifest;
import android.app.Activity;
import android.bluetooth.BluetoothAdapter;
import android.bluetooth.BluetoothDevice;
import android.content.Context;
import android.content.Intent;
import android.content.pm.PackageManager;
import android.util.Log;

import androidx.core.app.ActivityCompat;
import androidx.core.content.ContextCompat;

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

    public static boolean isPermitted(Context context, String permission)
    {
        return ContextCompat.checkSelfPermission(context, permission) == PackageManager.PERMISSION_GRANTED;
    }

    public static boolean requiresPermissionRationale(Activity activity, String permission)
    {
        return ActivityCompat.shouldShowRequestPermissionRationale(activity, permission);
    }

    public static void requestPermission(Activity activity, String permission, int requestCode)
    {
        ActivityCompat.requestPermissions(activity, new String[] { permission }, requestCode);
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
