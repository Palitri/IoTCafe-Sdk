using Palitri.OpenIoT.Board.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Palitri.OpenIoT.Board.Protocol.Events
{
    public interface IOpenIoTProtocolEvents : IPropertyTransmissionProtocolEvents
    {
        void OnDevicePropertiesReceived(object sender, Dictionary<int, byte[]> properties);
        void OnDevicePropertiesSet(object sender, Dictionary<int, byte[]> properties);
        void OnDeviceUidReceived(object sender, Guid uid);
        void OnDeviceNameReceived(object sender, string name);
        void OnProjectUidReceived(object sender, Guid uid);
        void OnProjectNameReceived(object sender, string name);
        void OnProjectHashReceived(object sender, uint hash);
        void OnUserUidReceived(object sender, Guid uid);
        void OnFirmwareNameReceived(object sender, string name);
        void OnFirmwareVendorReceived(object sender, string name);
        void OnFirmwareVersionReceived(object sender, string name);
        void OnBoardNameReceived(object sender, string name);



        void OnSchemeLogicUploaded(object sender);
        void OnProgramLogicUploaded(object sender);
        void OnResetLogic(object sender);
        void OnReset(object sender);
        void OnCommandExecuted(object sender);
    }
}
