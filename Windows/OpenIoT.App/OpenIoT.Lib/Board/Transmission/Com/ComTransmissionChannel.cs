using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenIoT.Lib.Board.Transmission.Com
{
    public class ComTransmissionChannel : ITransmissionChannel
    {
        public string Name { get; private set; }

        private SerialPort serialPort;
        
        public ComTransmissionChannel(string comPortName, int baudRate = 9600)
        {
            this.Name = comPortName;

            this.serialPort = new SerialPort();

            this.serialPort.PortName = this.Name;
            this.serialPort.BaudRate = baudRate;
            this.serialPort.Parity = Parity.None;
            this.serialPort.DataBits = 8;
            this.serialPort.StopBits = StopBits.One;
            this.serialPort.Handshake = Handshake.None;
            this.serialPort.ReadTimeout = 500;
            this.serialPort.WriteTimeout = 500;
        }

        public void Write(byte[] buffer, int offset, int length)
        {
            this.serialPort.Write(buffer, offset, length);
        }

        public int Read(byte[] buffer, int offset, int length)
        {
            if (this.serialPort.BytesToRead > 0)
                return this.serialPort.Read(buffer, offset, length);

            return 0;
        }

        public bool Open() 
        {
            this.serialPort.Open();
            return true;
        }

        public bool Close()
        {
            this.serialPort.Close();
            return true;
        }

        public bool IsOpen()
        {
            return this.serialPort.IsOpen;
        }



        public static IEnumerable<string> GetAvailablePorts()
        {
            return SerialPort.GetPortNames();
        }


    }
}
