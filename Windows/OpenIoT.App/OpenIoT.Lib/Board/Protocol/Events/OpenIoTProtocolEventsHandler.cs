using OpenIoT.Lib.Board.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenIoT.Lib.Board.Protocol.Events
{
    public class OpenIoTProtocolEventsHandler : IOpenIoTProtocolEvents
    {
        public virtual void onAllPropertiesInfoReceived(object sender)
        {
        }

        public virtual void onDeviceNameReceived(object sender, string name)
        {
        }

        public virtual void onDevicePropertiesReceived(object sender, Dictionary<int, byte[]> properties)
        {
        }

        public virtual void onDevicePropertiesSet(object sender, Dictionary<int, byte[]> properties)
        {
        }

        public virtual void onDeviceUidReceived(object sender, Guid uid)
        {
        }

        public virtual void onFirmwareNameReceived(object sender, string name)
        {
        }

        public virtual void onFirmwareVendorReceived(object sender, string name)
        {
        }

        public virtual void onFirmwareVersionReceived(object sender, string name)
        {
        }

        public virtual void onBoardNameReceived(object sender, string name)
        {
        }

        public virtual void onInfoReceived(object sender, string info)
        {
        }

        public virtual void onPingBack(object sender, byte[] data)
        {
        }

        public virtual void onProgramLogicUploaded(object sender)
        {
        }

        public virtual void onProjectHashReceived(object sender, uint hash)
        {
        }

        public virtual void onProjectNameReceived(object sender, string name)
        {
        }

        public virtual void onProjectUidReceived(object sender, Guid uid)
        {
        }

        public virtual void onUserUidReceived(object sender, Guid uid)
        {
        }

        public virtual void onPropertiesChangedSubscriptionReset(object sender)
        {
        }

        public virtual void onPropertiesSubscriptionSet(object sender)
        {
        }

        public virtual void onPropertyInfoReceived(object sender, BoardProperty p)
        {
        }

        public virtual void onSchemeLogicUploaded(object sender)
        {
        }

        public virtual void onSubscribedPropertyValueChanged(object sender, BoardProperty p, object oldValue)
        {
        }

        public virtual void onResetLogic(object sender)
        {
        }

        public virtual void onReset(object sender)
        {
        }
    }
}
