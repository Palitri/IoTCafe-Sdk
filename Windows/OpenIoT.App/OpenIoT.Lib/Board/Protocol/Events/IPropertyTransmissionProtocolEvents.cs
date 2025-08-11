using Palitri.OpenIoT.Board.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Palitri.OpenIoT.Board.Protocol.Events
{
    public interface IPropertyTransmissionProtocolEvents
    {
        void OnPingBack(object sender, byte[] data);
        void OnPropertyInfoReceived(object sender, BoardProperty p);
        void OnAllPropertiesInfoReceived(object sender);
        void OnSubscribedPropertyValueChanged(object sender, BoardProperty p, object oldValue);
        void OnInfoReceived(object sender, string info);
        void OnPropertiesSubscriptionSet(object sender);
        void OnPropertiesChangedSubscriptionReset(object sender);
    }
}
