using OpenIoT.Lib.Board.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenIoT.Lib.Board.Protocol.Events
{
    public interface IOpenIoTProtocolEvents : IPropertyTransmissionProtocolEvents
    {
        void onDevicePropertiesReceived(object sender, Dictionary<int, byte[]> properties);
        void onDevicePropertiesSet(object sender, Dictionary<int, byte[]> properties);
        void onDeviceUidReceived(object sender, Guid uid);
        void onDeviceNameReceived(object sender, string name);
        void onProjectUidReceived(object sender, Guid uid);
        void onProjectNameReceived(object sender, string name);
        void onProjectHashReceived(object sender, uint hash);
        void onUserUidReceived(object sender, Guid uid);
        void onFirmwareNameReceived(object sender, string name);
        void onFirmwareVendorReceived(object sender, string name);
        void onFirmwareVersionReceived(object sender, string name);
        void onBoardNameReceived(object sender, string name);



        void onSchemeLogicUploaded(object sender);
        void onProgramLogicUploaded(object sender);
        void onResetLogic(object sender);
        void onReset(object sender);
    }
}
