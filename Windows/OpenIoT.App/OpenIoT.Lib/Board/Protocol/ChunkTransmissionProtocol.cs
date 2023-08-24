using OpenIoT.Lib.Tools.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenIoT.Lib.Board.Protocol
{
    public abstract class ChunkTransmissionProtocol
    {
        static private int chunkIdSize = 2;
        static private int chunkLengthSize = 1;
        static private int chunkCRCSize = 2;
        static private int chunkFooterSize = chunkIdSize + chunkLengthSize + chunkCRCSize;
        static private int chunkMaxDataSize = 255;
        static private int chunkMaxTotalSize = chunkMaxDataSize + chunkFooterSize;
        static private byte[] chunkId = { (byte)0xE6, 0x71 };

        private int readBufferPosition, detectedFooterOffset;
        private byte[] readBuffer = new byte[chunkMaxTotalSize];
        private byte[] chunkData = new byte[chunkFooterSize - chunkIdSize];

        private byte[] writeBuffer = new byte[ChunkTransmissionProtocol.chunkMaxTotalSize];

        public abstract int Read(byte[] data, int offset, int size);
        public abstract void Write(byte[] data, int offset, int size);

        public abstract void OnReceiveChunk(byte[] data, int dataSize);

        public ChunkTransmissionProtocol()
            : base()
        {
        }


        public int SendChunk(byte[] data, int dataSize)
        {
            int sendDataSize = Math.Min(dataSize, ChunkTransmissionProtocol.chunkMaxDataSize);

            int crc16 = MathUtils.CRC16(data, dataSize);

            ArrayUtils.Copy(data, 0, writeBuffer, 0, sendDataSize);
            writeBuffer[sendDataSize + 0] = ChunkTransmissionProtocol.chunkId[0];
            writeBuffer[sendDataSize + 1] = ChunkTransmissionProtocol.chunkId[1];
            writeBuffer[sendDataSize + 2] = (byte)sendDataSize;
            writeBuffer[sendDataSize + 3] = (byte)crc16;
            writeBuffer[sendDataSize + 4] = (byte)(crc16 >> 8);

            this.Write(writeBuffer, 0, sendDataSize + ChunkTransmissionProtocol.chunkFooterSize);

            return sendDataSize + ChunkTransmissionProtocol.chunkFooterSize;
        }

        public void Feed(byte[] data, int dataSize)
        {
            for (int i = 0; i < dataSize; i++)
            {
                byte dataByte = data[i];

                this.readBuffer[this.readBufferPosition] = dataByte;
                this.readBufferPosition++;

                if (this.readBufferPosition > chunkMaxTotalSize)
                    this.readBufferPosition = 0;

                if (this.detectedFooterOffset < ChunkTransmissionProtocol.chunkIdSize)
                {
                    if (dataByte == ChunkTransmissionProtocol.chunkId[this.detectedFooterOffset])
                        this.detectedFooterOffset++;
                    else
                        this.detectedFooterOffset = 0;
                }
                else if (this.detectedFooterOffset < ChunkTransmissionProtocol.chunkFooterSize)
                {
                    this.chunkData[this.detectedFooterOffset - ChunkTransmissionProtocol.chunkIdSize] = dataByte;
                    this.detectedFooterOffset++;

                    if (this.detectedFooterOffset == ChunkTransmissionProtocol.chunkFooterSize)
                    {
                        this.detectedFooterOffset = 0;

                        byte payloadSize = this.chunkData[0];
                        short payloadCrc16 = ByteUtils.ToInt16(this.chunkData, 1);

                        int payloadPosition = this.readBufferPosition - ChunkTransmissionProtocol.chunkFooterSize - payloadSize;
                        byte[] chunkData = new byte[ChunkTransmissionProtocol.chunkMaxDataSize];
                        if (payloadPosition >= 0)
                        {
                            ArrayUtils.Copy(this.readBuffer, payloadPosition, chunkData, 0, payloadSize);
                            short actualCrc16 = MathUtils.CRC16(chunkData, payloadSize);
                            if (actualCrc16 == payloadCrc16)
                            {
                                this.OnReceiveChunk(chunkData, payloadSize);

                                this.readBufferPosition = 0;
                            }
                        }
                        else
                        {
                            ArrayUtils.Copy(this.readBuffer, ChunkTransmissionProtocol.chunkMaxTotalSize + payloadPosition, chunkData, 0, -payloadPosition);
                            ArrayUtils.Copy(this.readBuffer, 0, chunkData, -payloadPosition, payloadSize + payloadPosition);
                            short actualCrc16 = MathUtils.CRC16(chunkData, payloadSize);
                            if (actualCrc16 == payloadCrc16)
                            {
                                this.OnReceiveChunk(chunkData, payloadSize);

                                this.readBufferPosition = 0;
                            }
                        }
                    }
                }

                this.readBufferPosition %= ChunkTransmissionProtocol.chunkMaxTotalSize;
            }
        }

        private byte[] inputBuffer = new byte[128];
        public void ProcessInput()
        {
            int receivedBytesCount = this.Read(inputBuffer, 0, inputBuffer.Length);
            this.Feed(inputBuffer, receivedBytesCount);
        }
    }
}
