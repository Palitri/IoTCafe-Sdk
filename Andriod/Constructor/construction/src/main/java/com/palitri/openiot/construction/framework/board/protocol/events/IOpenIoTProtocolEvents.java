package com.palitri.openiot.construction.framework.board.protocol.events;

import java.util.HashMap;
import java.util.UUID;

public interface IOpenIoTProtocolEvents extends IPropertyTransmissionProtocolEvents {
    void onDevicePropertiesReceived(Object sender, HashMap<Integer, byte[]> properties);
    void onDevicePropertiesSet(Object sender, HashMap<Integer, byte[]> properties);
    void onDeviceUidReceived(Object sender, UUID uid);
    void onDeviceNameReceived(Object sender, String name);
    void onProjectUidReceived(Object sender, UUID uid);
    void onProjectNameReceived(Object sender, String name);
    void onProjectHashReceived(Object sender, int hash);
    void onUserUidReceived(Object sender, UUID uid);
    void onFirmwareNameReceived(Object sender, String name);
    void onFirmwareVendorReceived(Object sender, String name);
    void onFirmwareVersionReceived(Object sender, String name);
    void onBoardNameReceived(Object sender, String name);

    void onSchemeLogicUploaded(Object sender);
    void onProgramLogicUploaded(Object sender);
    void onResetLogic(Object sender);
    void onReset(Object sender);
 }
