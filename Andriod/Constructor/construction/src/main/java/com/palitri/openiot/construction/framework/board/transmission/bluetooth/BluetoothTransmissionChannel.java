package com.palitri.openiot.construction.framework.board.transmission.bluetooth;

import android.bluetooth.BluetoothAdapter;
import android.bluetooth.BluetoothDevice;
import android.bluetooth.BluetoothSocket;
import android.util.Log;

import com.palitri.openiot.construction.framework.board.transmission.ITransmissionChannel;

import java.io.IOException;
import java.io.InputStream;
import java.io.OutputStream;
import java.util.UUID;
import java.util.concurrent.locks.Lock;
import java.util.concurrent.locks.ReentrantLock;

public class BluetoothTransmissionChannel implements ITransmissionChannel {
    private BluetoothSocket mmSocket;
    private final BluetoothDevice mmDevice;
    private final BluetoothAdapter mmBluetoothAdapter;

    private InputStream mmInputStream;
    private OutputStream mmOutputStream;

    private static final String TAG = "BT_DEBUG_TAG";

    private boolean _isOpened;

    private Lock lock;

    public BluetoothTransmissionChannel(BluetoothDevice device, BluetoothAdapter bluetoothAdapter) {
        mmDevice = device;
        mmBluetoothAdapter = bluetoothAdapter;

        lock = new ReentrantLock();

        this.open();
    }

    private boolean createSocket() {
        InputStream inputStream = null;
        OutputStream outputStream = null;
        BluetoothSocket socket = null;

        this._isOpened = false;

        try {
            UUID uuid = mmDevice.getUuids()[0].getUuid();
            socket = mmDevice.createRfcommSocketToServiceRecord(uuid);
        } catch (IOException e) {
            Log.e(TAG, "Error creating socket for bluetooth device", e);
            return false;
        }

        try {
            inputStream = socket.getInputStream();
        } catch (IOException e) {
            Log.e(TAG, "Error occurred when creating socket input stream", e);
            return false;
        }
        try {
            outputStream = socket.getOutputStream();
        } catch (IOException e) {
            Log.e(TAG, "Error occurred when creating socket output stream", e);
            return false;
        }

        this.mmInputStream = inputStream;
        this.mmOutputStream = outputStream;
        this.mmSocket = socket;

        return true;
    }

    private boolean closeSocket()
    {
        try {
            this.mmInputStream.close();
            this.mmInputStream = null;

            this.mmOutputStream.close();
            this.mmOutputStream = null;

            this.mmSocket.close();
            this.mmSocket = null;
        } catch (IOException e) {
            e.printStackTrace();
            return false;
        }

        return true;
    }

    public boolean open() {
        if (!this._isOpened) {
            mmBluetoothAdapter.cancelDiscovery();

            if (!this.createSocket())
                return false;

            try {
                mmSocket.connect();
                this._isOpened = true;
            } catch (IOException connectException) {
                try {
                    mmSocket.close();
                } catch (IOException closeException) {
                    Log.e(TAG, "Could not close the client socket", closeException);
                }
                return false;
            }
        }

        return this._isOpened;
    }

    public boolean close() {
        if (this._isOpened) {
            this._isOpened = !this.closeSocket();
        }

        return this._isOpened;
    }

    public boolean isOpened() { return this._isOpened; }

    public void write(byte[] bytes, int offset, int length) {
        try {
            mmOutputStream.write(bytes, offset, length);
        } catch (Exception e) {
            Log.e(TAG, "Error occurred when sending data", e);
        }
    }

    public int read(byte[] buffer, int offset, int length)
    {
        try
        {
            if (mmInputStream.available() > 0)
                return mmInputStream.read(buffer, offset, length);
        }
        catch (IOException e)
        {
            this._isOpened = false;
            Log.d(TAG, "Input stream was disconnected", e);
            return 0;
        }

        return 0;
    }
}
