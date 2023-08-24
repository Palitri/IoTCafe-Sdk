using OpenIoT.Lib.Board.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenIoT.Lib.Board.Protocol.Events
{
    public interface IPropertyTransmissionProtocolEvents
    {
        void onPingBack(object sender, byte[] data);
        void onPropertyInfoReceived(object sender, BoardProperty p);
        void onAllPropertiesInfoReceived(object sender);
        void onSubscribedPropertyValueChanged(object sender, BoardProperty p, object oldValue);
        void onInfoReceived(object sender, string info);
        void onPropertiesSubscriptionSet(object sender);
        void onPropertiesChangedSubscriptionReset(object sender);
    }
}
