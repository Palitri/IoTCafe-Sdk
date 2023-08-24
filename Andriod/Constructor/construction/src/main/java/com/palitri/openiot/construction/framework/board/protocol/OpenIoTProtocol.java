package com.palitri.openiot.construction.framework.board.protocol;

import android.util.Pair;

import com.palitri.openiot.construction.framework.board.protocol.events.IOpenIoTProtocolEvents;
import com.palitri.openiot.construction.framework.board.protocol.events.IPropertyTransmissionProtocolEvents;
import com.palitri.openiot.construction.framework.tools.utils.ArrayUtils;
import com.palitri.openiot.construction.framework.tools.utils.ByteUtils;
import com.palitri.openiot.construction.framework.tools.utils.GuidUtils;
import com.palitri.openiot.construction.framework.tools.utils.StringUtils;

import java.util.Arrays;
import java.util.HashMap;
import java.util.UUID;

public abstract class OpenIoTProtocol extends PropertyTransmissionProtocol {

    public static final int CommandCode_SetDeviceProperties     = 0x41;
    public static final int CommandCode_GetDeviceProperties		= 0x42;
    public static final int CommandCode_UploadSchemeLogic       = 0x43;
    public static final int CommandCode_UploadProgramLogic      = 0x44;
    public static final int CommandCode_ResetLogic              = 0x45;
    public static final int CommandCode_Reset                   = 0x46;

    public static final int ResponseCode_SetDeviceProperties	= 0x41 | Code_ResponseBit;
    public static final int ResponseCode_GetDeviceProperties	= 0x42 | Code_ResponseBit;
    public static final int ResponseCode_UploadSchemeLogic      = 0x43 | Code_ResponseBit;
    public static final int ResponseCode_UploadProgramLogic     = 0x44 | Code_ResponseBit;
    public static final int ResponseCode_ResetLogic             = 0x45 | Code_ResponseBit;
    public static final int ResponseCode_Reset                  = 0x46 | Code_ResponseBit;

    public static final int DevicePropertyId_Uid			    = 0x11;
    public static final int DevicePropertyId_Name			    = 0x12;
    public static final int DevicePropertyId_Password		    = 0x13;
    public static final int DevicePropertyId_ProjectUid         = 0x14;
    public static final int DevicePropertyId_ProjectName	    = 0x15;
    public static final int DevicePropertyId_ProjectHash	    = 0x16;
    public static final int DevicePropertyId_UserUid            = 0x17;
    public static final int DevicePropertyId_FirmwareName       = 0x23;
    public static final int DevicePropertyId_FirmwareVendor     = 0x22;
    public static final int DevicePropertyId_FirmwareVersion    = 0x21;
    public static final int DevicePropertyId_BoardName          = 0x31;


    public OpenIoTProtocol() {
        super();
    }

    public boolean OnReceiveCommand(byte command, byte[] data, int dataSize)
    {
        if (super.OnReceiveCommand(command, data, dataSize))
            return true;

        switch ((int) (command & 0xff)) {
            case ResponseCode_SetDeviceProperties: {
                int offset = 0;

                HashMap<Integer, byte[]> properties = new HashMap<Integer, byte[]>();
                while (offset < dataSize)
                {
                    int propertyId = data[offset++];
                    int propertySize = data[offset++];

                    byte[] valueBytes = new byte[propertySize];
                    ArrayUtils.Copy(data, offset, valueBytes, 0, propertySize);
                    properties.put(propertyId, valueBytes);

                    offset += propertySize;
                }

                for (IPropertyTransmissionProtocolEvents eventHandler : this.eventHandlers)
                    ((IOpenIoTProtocolEvents)eventHandler).onDevicePropertiesSet(this, properties);

                break;
            }

            case ResponseCode_GetDeviceProperties:
            {
                int offset = 0;

                HashMap<Integer, byte[]> properties = new HashMap<Integer, byte[]>();

                while (offset < dataSize)
                {
                    int propertyId = data[offset++];
                    int propertySize = data[offset++];

                    byte[] valueBytes = new byte[propertySize];
                    ArrayUtils.Copy(data, offset, valueBytes, 0, propertySize);
                    properties.put(propertyId, valueBytes);

                    Object value = OpenIoTProtocol.GetDevicePropertyFriendly(propertyId, valueBytes).second;
                    switch (propertyId)
                    {
                        case DevicePropertyId_Uid:
                        {
                            for (IPropertyTransmissionProtocolEvents eventHandler: this.eventHandlers)
                                ((IOpenIoTProtocolEvents)eventHandler).onDeviceUidReceived(this, (UUID)value);

                            break;
                        }

                        case DevicePropertyId_Name:
                        {
                            for (IPropertyTransmissionProtocolEvents eventHandler: this.eventHandlers)
                                ((IOpenIoTProtocolEvents)eventHandler).onDeviceNameReceived(this, (String)value);

                            break;
                        }

                        case DevicePropertyId_Password:
                        {
                            break;
                        }

                        case DevicePropertyId_ProjectUid:
                        {
                            for (IPropertyTransmissionProtocolEvents eventHandler: this.eventHandlers)
                                ((IOpenIoTProtocolEvents)eventHandler).onProjectUidReceived(this, (UUID)value);

                            break;
                        }

                        case DevicePropertyId_ProjectName:
                        {
                            for (IPropertyTransmissionProtocolEvents eventHandler: this.eventHandlers)
                                ((IOpenIoTProtocolEvents)eventHandler).onProjectNameReceived(this, (String)value);

                            break;
                        }

                        case DevicePropertyId_ProjectHash:
                        {
                            for (IPropertyTransmissionProtocolEvents eventHandler: this.eventHandlers)
                                ((IOpenIoTProtocolEvents)eventHandler).onProjectHashReceived(this, (int)value);

                            break;
                        }

                        case DevicePropertyId_UserUid:
                        {
                            for (IPropertyTransmissionProtocolEvents eventHandler: this.eventHandlers)
                                ((IOpenIoTProtocolEvents)eventHandler).onUserUidReceived(this, (UUID)value);

                            break;
                        }

                        case DevicePropertyId_FirmwareName:
                        {
                            for (IPropertyTransmissionProtocolEvents eventHandler: this.eventHandlers)
                                ((IOpenIoTProtocolEvents)eventHandler).onFirmwareNameReceived(this, (String)value);

                            break;
                        }

                        case DevicePropertyId_FirmwareVendor:
                        {
                            for (IPropertyTransmissionProtocolEvents eventHandler: this.eventHandlers)
                                ((IOpenIoTProtocolEvents)eventHandler).onFirmwareVendorReceived(this, (String)value);

                            break;
                        }

                        case DevicePropertyId_FirmwareVersion:
                        {
                            for (IPropertyTransmissionProtocolEvents eventHandler: this.eventHandlers)
                                ((IOpenIoTProtocolEvents)eventHandler).onFirmwareVersionReceived(this, (String)value);

                            break;
                        }

                        case DevicePropertyId_BoardName:
                        {
                            for (IPropertyTransmissionProtocolEvents eventHandler: this.eventHandlers)
                                ((IOpenIoTProtocolEvents)eventHandler).onBoardNameReceived(this, (String)value);

                            break;
                        }
                    }

                    offset += propertySize;
                }

                for (IPropertyTransmissionProtocolEvents eventHandler : this.eventHandlers)
                    ((IOpenIoTProtocolEvents)eventHandler).onDevicePropertiesReceived(this, properties);

                break;
            }

            case ResponseCode_UploadSchemeLogic: {
                for (IPropertyTransmissionProtocolEvents eventHandler : this.eventHandlers)
                    ((IOpenIoTProtocolEvents)eventHandler).onSchemeLogicUploaded(this);

                break;
            }

            case ResponseCode_UploadProgramLogic: {
                for (IPropertyTransmissionProtocolEvents eventHandler : this.eventHandlers)
                    ((IOpenIoTProtocolEvents)eventHandler).onProgramLogicUploaded(this);

                break;
            }

            case ResponseCode_ResetLogic: {
                for (IPropertyTransmissionProtocolEvents eventHandler : this.eventHandlers)
                    ((IOpenIoTProtocolEvents)eventHandler).onResetLogic(this);

                break;
            }

            case ResponseCode_Reset: {
                for (IPropertyTransmissionProtocolEvents eventHandler : this.eventHandlers)
                    ((IOpenIoTProtocolEvents)eventHandler).onReset(this);

                break;
            }

            default:
                return false;
        }

        return true;
    }

    static public Pair<String, Object> GetDevicePropertyFriendly(int propertyId, byte[] propertyValue)
    {
        switch (propertyId)
        {
            case DevicePropertyId_Uid:
                return new Pair<String, Object>("Device Uid", GuidUtils.ToGuid(GuidUtils.FromBytes(propertyValue)));

            case DevicePropertyId_Name:
                return new Pair<String, Object>("Device Name", StringUtils.BytesToString(propertyValue));

            //case DevicePropertyId_Password:
            //    {
            //        return true;
            //    }

            case DevicePropertyId_ProjectUid:
                return new Pair<String, Object>("Project Uid", GuidUtils.ToGuid(GuidUtils.FromBytes(propertyValue)));

            case DevicePropertyId_ProjectName:
                return new Pair<String, Object>("Project Name", StringUtils.BytesToString(propertyValue));

            case DevicePropertyId_ProjectHash:
                return new Pair<String, Object>("Project Hash", ByteUtils.toInt32(propertyValue, 0));

            case DevicePropertyId_UserUid:
                return new Pair<String, Object>("User Uid", GuidUtils.ToGuid(GuidUtils.FromBytes(propertyValue)));

            case DevicePropertyId_FirmwareName:
                return new Pair<String, Object>("Firmware Name", StringUtils.BytesToString(propertyValue));

            case DevicePropertyId_FirmwareVendor:
                return new Pair<String, Object>("Firmware Vendor", StringUtils.BytesToString(propertyValue));

            case DevicePropertyId_FirmwareVersion:
                return new Pair<String, Object>("Firmware Version", StringUtils.Join(".", Arrays.asList(propertyValue)));

            case DevicePropertyId_BoardName:
                return new Pair<String, Object>("Board", StringUtils.BytesToString(propertyValue));
        }

        return new Pair<String, Object>(Integer.toString(propertyId), StringUtils.BytesToString(propertyValue));
    }

    public void uploadSchemeLogic(byte[] code)
    {
        this.SendCommand(CommandCode_UploadSchemeLogic, code);
    }

    public void uploadProgramLogic(byte[] code)
    {
        this.SendCommand(CommandCode_UploadProgramLogic, code);
    }

    public void resetLogic()
    {
        this.SendCommand(CommandCode_ResetLogic);
    }

    public void reset()
    {
        this.SendCommand(CommandCode_Reset);
    }

    public void requestDeviceName()
    {
        this.SendCommand(CommandCode_GetDeviceProperties, new byte[] { DevicePropertyId_Name });
    }

    public void requestSetDeviceName(String name)
    {
        byte[] nameBytes = StringUtils.StringToBytes(name);

        byte[] data = new byte[2 + name.length()];
        data[0] = DevicePropertyId_Name;
        data[1] = (byte)nameBytes.length;
        ArrayUtils.Copy(nameBytes, 0, data, 2, nameBytes.length);

        this.SendCommand(CommandCode_SetDeviceProperties, data);
    }


    public void requestSetProjectDetails(String projectId, String projectName)
    {
        byte[] projectIdBytes = StringUtils.HexToBytes(projectId.replace("-", ""));// this.FromHex(projectId.Replace("-", ""));

        //string projectIdString = BitConverter.ToString(projectIdBytes).Replace("-", string.Empty);

        byte[] data = new byte[2 + projectIdBytes.length + 2 + projectName.length()];
        int offset = 0;
        data[offset++] = DevicePropertyId_ProjectUid;
        data[offset++] = (byte)projectIdBytes.length;
        ArrayUtils.Copy(projectIdBytes, 0, data, offset, projectIdBytes.length);
        offset += projectIdBytes.length;

        data[offset++] = DevicePropertyId_ProjectName;
        data[offset++] = (byte)projectName.length();
        byte[] projectNameBytes = StringUtils.StringToBytes(projectName);
        ArrayUtils.Copy(projectNameBytes, 0, data, offset, projectNameBytes.length);
        offset += projectNameBytes.length;

        this.SendCommand(CommandCode_SetDeviceProperties, data);
    }
}
