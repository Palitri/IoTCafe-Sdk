package com.palitri.openiot.construction.framework.board.api;

import com.palitri.openiot.construction.framework.board.transmission.ITransmissionChannel;
import com.palitri.openiot.construction.framework.board.protocol.OpenIoTProtocol;
import com.palitri.openiot.construction.framework.tools.threading.ContinuousThread;

public class OpenIotBoard extends OpenIoTProtocol {

    public ContinuousThread transmissionThread;
    public ITransmissionChannel transmissionChannel;

    public OpenIotBoard(ITransmissionChannel transmissionChannel)
    {
        super();

        this.transmissionChannel = transmissionChannel;
    }

    @Override
    public int Read(byte[] data, int offset, int size)
    {
        return this.transmissionChannel.read(data, offset, size);
    }

    @Override
    public void Write(byte[] data, int offset, int size) {
        this.transmissionChannel.write(data, offset, size);
    }

    public boolean Open()
    {
        if (!this.transmissionChannel.isOpened())
            if (!this.transmissionChannel.open())
                return false;

        if (this.transmissionThread == null) {
            this.transmissionThread = new ContinuousThread()
            {
                @Override
                public boolean action() {
                    OpenIotBoard.this.ProcessInput();
                    return true;
                }
            };
            this.transmissionThread.start();
        }

        return true;
    }

    public void Close()
    {
        if (this.transmissionThread != null)
            this.transmissionThread.terminate(true);
        this.transmissionThread = null;

        if (this.transmissionChannel.isOpened())
            this.transmissionChannel.close();
    }
}
