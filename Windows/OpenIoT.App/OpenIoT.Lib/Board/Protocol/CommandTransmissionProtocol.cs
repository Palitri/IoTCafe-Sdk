using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenIoT.Lib.Board.Protocol
{
    public abstract class CommandTransmissionProtocol : ChunkTransmissionProtocol
    {
        public abstract bool OnReceiveCommand(byte command, byte[] data, int dataSize);
        
        public override void OnReceiveChunk(byte[] data, int dataSize)
        {
            int offset = 0;
            while (offset < dataSize)
            {
                byte commandCode = data[offset++];
                byte commandSize = data[offset++];

                byte[] commandData = new byte[commandSize];
                Array.Copy(data, offset, commandData, 0, commandSize);
                offset += commandSize;

                this.OnReceiveCommand(commandCode, commandData, commandSize);
            }
        }

        public void SendCommand(byte command, byte[] data = null)
        {
            int dataSize = data == null ? 0 : data.Length;

            byte[] chunk = new byte[dataSize + 2];
            chunk[0] = command;
            chunk[1] = (byte)dataSize;
            if (dataSize > 0)
                Array.Copy(data, 0, chunk, 2, dataSize);

            this.SendChunk(chunk, dataSize + 2);
        }
    }
}