using Palitri.OpenIoT.Board.Protocol;
using Palitri.OpenIoT.Tools.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenIoT.Lib.Tools.Protocol
{
    public class OpenIoTCommandsWriter
    {
        private byte[] buffer;
        private int size;

        private int commandSizePos;

        public int Size => this.size;
        public byte[] Data => this.buffer;



        public OpenIoTCommandsWriter()
        {
            this.buffer = new byte[256];

            this.Reset();
        }

        public void Send(OpenIoTProtocol openIoT)
        {
            openIoT.SendCommand(OpenIoTProtocol.CommandCode_ExecuteCommand, this.buffer, this.size);
        }

        public void Reset()
        {
            this.size = 0;
        }

        public void BeginCommand(int peripheralId, int commandCode)
        {
            this.WriteUInt8((byte)peripheralId);
            this.WriteUInt8((byte)commandCode);
            this.commandSizePos = this.size;
            this.WriteUInt8(0);
        }

        public void EndCommand()
        {
            this.buffer[this.commandSizePos] = (byte)(this.size - this.commandSizePos - 1);
        }

        public void WriteUInt8(byte value)
        {
            this.buffer[this.size++] = value;
        }

        public void WriteInt16(short value)
        {
            this.size += ByteUtils.FromInt16(value, this.buffer, this.size);
        }

        public void WriteInt32(int value)
        {
            this.size += ByteUtils.FromInt32(value, this.buffer, this.size);
        }

        public void WriteFloat32(float value)
        {
            this.size += ByteUtils.FromFloat(value, this.buffer, this.size);
        }
    }
}
