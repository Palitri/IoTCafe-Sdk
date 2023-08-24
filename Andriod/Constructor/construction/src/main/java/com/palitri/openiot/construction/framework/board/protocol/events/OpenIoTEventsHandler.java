package com.palitri.openiot.construction.framework.board.protocol.events;

import com.palitri.openiot.construction.framework.board.models.BoardProperty;

import java.util.HashMap;
import java.util.UUID;

public class OpenIoTEventsHandler implements IOpenIoTProtocolEvents {

    @Override
    public void onDevicePropertiesReceived(Object sender, HashMap<Integer, byte[]> properties) {

    }

    @Override
    public void onDevicePropertiesSet(Object sender, HashMap<Integer, byte[]> properties) {

    }

    @Override
    public void onDeviceUidReceived(Object sender, UUID uid) {

    }

    @Override
    public void onDeviceNameReceived(Object sender, String name) {

    }

    @Override
    public void onProjectUidReceived(Object sender, UUID uid) {

    }

    @Override
    public void onProjectNameReceived(Object sender, String name) {

    }

    @Override
    public void onProjectHashReceived(Object sender, int hash) {

    }

    @Override
    public void onUserUidReceived(Object sender, UUID uid) {

    }

    @Override
    public void onFirmwareNameReceived(Object sender, String name) {

    }

    @Override
    public void onFirmwareVendorReceived(Object sender, String name) {

    }

    @Override
    public void onFirmwareVersionReceived(Object sender, String name) {

    }

    @Override
    public void onBoardNameReceived(Object sender, String name) {

    }

    @Override
    public void onSchemeLogicUploaded(Object sender) {

    }

    @Override
    public void onResetLogic(Object sender) {

    }

    @Override
    public void onReset(Object sender) {

    }

    @Override
    public void onProgramLogicUploaded(Object sender) {

    }

    @Override
    public void onPingRequest(Object sender, byte[] data) {

    }

    @Override
    public void onPingBack(Object sender, byte[] data) {

    }

    @Override
    public void onPropertyInfoReceived(Object sender, BoardProperty p) {

    }

    @Override
    public void onAllPropertiesInfoReceived(Object sender) {

    }

    @Override
    public void onSubscribedPropertyValueChanged(Object sender, BoardProperty p, Object oldValue) {

    }

    @Override
    public void onInfoReceived(Object sender, String info) {

    }

    @Override
    public void onPropertiesSubscriptionSet(Object sender) {

    }

    @Override
    public void onPropertiesChangedSubscriptionReset(Object sender) {

    }
}
