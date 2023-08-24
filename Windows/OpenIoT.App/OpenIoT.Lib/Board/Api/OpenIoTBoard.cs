using OpenIoT.Lib.Board.Protocol;
using OpenIoT.Lib.Board.Transmission;
using OpenIoT.Lib.Tools.Threading;

namespace OpenIoT.Lib.Board.Api
{
    public class OpenIoTBoard : OpenIoTProtocol
    {
        public ITransmissionChannel transmissionChannel;
        private ContinuousThread? transmissionThread;

        public Dictionary<int, string> devicePropertyNames = new Dictionary<int, string>()
        {
            { DevicePropertyId_Uid, "Uid" },
            { DevicePropertyId_Name, "Name" },
            { DevicePropertyId_Password, "Password" },
            { DevicePropertyId_ProjectUid, "Project Uid" },
            { DevicePropertyId_ProjectName, "Project Name" },
            { DevicePropertyId_ProjectHash, "Project Hash" },
            { DevicePropertyId_UserUid, "User Uid" },
            { DevicePropertyId_FirmwareName, "Firmware Name" },
            { DevicePropertyId_FirmwareVendor, "Firmware Vendor" },
            { DevicePropertyId_FirmwareVersion, "Firmware Version" },
            { DevicePropertyId_BoardName, "Board" }
        };
        
        public OpenIoTBoard(ITransmissionChannel transmissionChannel = null)
                : base()
        {
            this.transmissionChannel = transmissionChannel;
            this.transmissionThread = null;
        }

        public override int Read(byte[] data, int offset, int size)
        {
            return this.transmissionChannel.Read(data, offset, size);
        }

        public override void Write(byte[] data, int offset, int size)
        {
            this.transmissionChannel.Write(data, offset, size);
        }

        public void Open()
        {
            if (!this.transmissionChannel.IsOpen())
                this.transmissionChannel.Open();

            if (this.transmissionThread == null)
                this.transmissionThread = new ContinuousThread(this.TransmissionTask);
            this.transmissionThread.Start();
        }

        public void Close(bool wait = true)
        {
            if (this.transmissionThread != null)
                this.transmissionThread.Terminate(wait);
            this.transmissionThread = null;

            if (this.transmissionChannel != null)
            {
                if (this.transmissionChannel.IsOpen())
                    this.transmissionChannel.Close();
            }
        }

        public string GetDevicePropertyName(int propertyId)
        {
            string name;
            if (!this.devicePropertyNames.TryGetValue(propertyId, out name))
                name = propertyId.ToString();

            return name;
        }

        private bool TransmissionTask()
        {
            this.ProcessInput();

            return true;
        }

    }
}

