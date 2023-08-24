package com.palitri.openiot.construction.framework.board.transmission;

public interface ITransmissionChannel
{
    void write(byte[] buffer, int offset, int length);
    int read(byte[] buffer, int offset, int bufferLength);

    boolean open();
    boolean close();

    boolean isOpened();
}
