package com.palitri.openiot.construction.framework.board.protocol;

import com.palitri.openiot.construction.framework.tools.utils.ByteUtils;

public abstract class CommandTransmissionProtocol extends ChunkTransmissionProtocol {
    public abstract boolean OnReceiveCommand(byte command, byte[] data, int dataSize);

    @Override
    public void OnReceiveChunk(byte[] data, int dataSize)
    {
        int offset = 0;
        while (offset < dataSize)
        {
            byte commandCode = data[offset++];
            int commandSize = ByteUtils.unsign(data[offset++]);

            byte[] commandData = new byte[commandSize];
            System.arraycopy(data, offset, commandData, 0, commandSize);
            offset += commandSize;

            this.OnReceiveCommand(commandCode, commandData, commandSize);
        }
    }

    public void SendCommand(int command, byte[] data)
    {
        int dataSize = data == null ? 0 : data.length;

        byte[] chunk = new byte[dataSize + 2];
        chunk[0] = (byte)command;
        chunk[1] = (byte)dataSize;
        if (dataSize > 0)
            System.arraycopy(data, 0, chunk, 2, dataSize);

        this.SendChunk(chunk, dataSize + 2);
    }

    public void SendCommand(int command)
    {
        this.SendCommand(command, null);
    }
}