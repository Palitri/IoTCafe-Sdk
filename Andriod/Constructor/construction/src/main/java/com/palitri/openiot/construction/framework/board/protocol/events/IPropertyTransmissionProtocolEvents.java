package com.palitri.openiot.construction.framework.board.protocol.events;

import com.palitri.openiot.construction.framework.board.models.BoardProperty;

public interface IPropertyTransmissionProtocolEvents {
    void onPingRequest(Object sender, byte[] data);
    void onPingBack(Object sender, byte[] data);
    void onPropertyInfoReceived(Object sender, BoardProperty p);
    void onAllPropertiesInfoReceived(Object sender);
    void onSubscribedPropertyValueChanged(Object sender, BoardProperty p, Object oldValue);
    void onInfoReceived(Object sender, String info) ;
    void onPropertiesSubscriptionSet(Object sender);
    void onPropertiesChangedSubscriptionReset(Object sender);
}
