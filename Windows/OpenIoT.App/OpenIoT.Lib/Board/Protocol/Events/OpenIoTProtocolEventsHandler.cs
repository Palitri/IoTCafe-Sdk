using Palitri.OpenIoT.Board.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Palitri.OpenIoT.Board.Protocol.Events
{
    public class OpenIoTProtocolEventsHandler : IOpenIoTProtocolEvents
    {
        public virtual void OnAllPropertiesInfoReceived(object sender)
        {
        }

        public virtual void OnDeviceNameReceived(object sender, string name)
        {
        }

        public virtual void OnDevicePropertiesReceived(object sender, Dictionary<int, byte[]> properties)
        {
        }

        public virtual void OnDevicePropertiesSet(object sender, Dictionary<int, byte[]> properties)
        {
        }

        public virtual void OnDeviceUidReceived(object sender, Guid uid)
        {
        }

        public virtual void OnFirmwareNameReceived(object sender, string name)
        {
        }

        public virtual void OnFirmwareVendorReceived(object sender, string name)
        {
        }

        public virtual void OnFirmwareVersionReceived(object sender, string name)
        {
        }

        public virtual void OnBoardNameReceived(object sender, string name)
        {
        }

        public virtual void OnInfoReceived(object sender, string info)
        {
        }

        public virtual void OnPingBack(object sender, byte[] data)
        {
        }

        public virtual void OnProgramLogicUploaded(object sender)
        {
        }

        public virtual void OnProjectHashReceived(object sender, uint hash)
        {
        }

        public virtual void OnProjectNameReceived(object sender, string name)
        {
        }

        public virtual void OnProjectUidReceived(object sender, Guid uid)
        {
        }

        public virtual void OnUserUidReceived(object sender, Guid uid)
        {
        }

        public virtual void OnPropertiesChangedSubscriptionReset(object sender)
        {
        }

        public virtual void OnPropertiesSubscriptionSet(object sender)
        {
        }

        public virtual void OnPropertyInfoReceived(object sender, BoardProperty p)
        {
        }

        public virtual void OnSchemeLogicUploaded(object sender)
        {
        }

        public virtual void OnSubscribedPropertyValueChanged(object sender, BoardProperty p, object oldValue)
        {
        }

        public virtual void OnResetLogic(object sender)
        {
        }

        public virtual void OnReset(object sender)
        {
        }
        public virtual void OnCommandExecuted(object sender)
        {
        }
    }
}
