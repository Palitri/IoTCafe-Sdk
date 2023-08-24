using OpenIoT.Lib.Board.Models;
using OpenIoT.Lib.Board.Protocol.Events;
using OpenIoT.Lib.Tools.Utils;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace OpenIoT.Lib.Board.Protocol
{


    public abstract class OpenIoTProtocol : PropertyTransmissionProtocol
    {
        public const byte CommandCode_SetDeviceProperties       = 0x41;
        public const byte CommandCode_GetDeviceProperties       = 0x42;
        public const byte CommandCode_UploadSchemeLogic         = 0x43;
        public const byte CommandCode_UploadProgramLogic        = 0x44;
        public const byte CommandCode_ResetLogic                = 0x45;
        public const byte CommandCode_Reset                     = 0x46;

        public const byte ResponseCode_SetDeviceProperties      = 0x41 | Code_ResponseBit;
        public const byte ResponseCode_GetDeviceProperties      = 0x42 | Code_ResponseBit;
        public const byte ResponseCode_UploadSchemeLogic        = 0x43 | Code_ResponseBit;
        public const byte ResponseCode_UploadProgramLogic       = 0x44 | Code_ResponseBit;
        public const byte ResponseCode_ResetLogic               = 0x45 | Code_ResponseBit;
        public const byte ResponseCode_Reset                    = 0x46 | Code_ResponseBit;

        public const byte DevicePropertyId_Uid              = 0x11;
        public const byte DevicePropertyId_Name             = 0x12;
        public const byte DevicePropertyId_Password         = 0x13;
        public const byte DevicePropertyId_ProjectUid       = 0x14;
        public const byte DevicePropertyId_ProjectName      = 0x15;
        public const byte DevicePropertyId_ProjectHash      = 0x16;
        public const byte DevicePropertyId_UserUid          = 0x17;
        public const byte DevicePropertyId_FirmwareName     = 0x21;
        public const byte DevicePropertyId_FirmwareVendor   = 0x22;
        public const byte DevicePropertyId_FirmwareVersion  = 0x23;
        public const byte DevicePropertyId_BoardName        = 0x31;

        public OpenIoTProtocol()
                : base()
        {
        }

        public override bool OnReceiveCommand(byte command, byte[] data, int dataSize)
        {
            switch (command)
            {
                case ResponseCode_Info:
                    {
                        string infoString = ByteUtils.ToString(data, 0, data.Length);

                        foreach (IOpenIoTProtocolEvents eventHandler in this.EventHandlers)
                            eventHandler.onInfoReceived(this, infoString);

                        break;
                    }

                case ResponseCode_SetDeviceProperties:
                    {
                        int offset = 0;

                        Dictionary<int, byte[]> properties = new Dictionary<int, byte[]>();
                        while (offset < dataSize)
                        {
                            int propertyId = data[offset++];
                            int propertySize = data[offset++];

                            byte[] valueBytes = new byte[propertySize];
                            Array.Copy(data, offset, valueBytes, 0, propertySize);
                            properties[propertyId] = valueBytes;

                            offset += propertySize;
                        }

                        foreach (IOpenIoTProtocolEvents eventHandler in this.EventHandlers)
                            eventHandler.onDevicePropertiesSet(this, properties);

                        break;
                    }

                case ResponseCode_GetDeviceProperties:
                    {
                        int offset = 0;

                        Dictionary<int, byte[]> properties = new Dictionary<int, byte[]>();

                        while (offset < dataSize)
                        {
                            int propertyId = data[offset++];
                            int propertySize = data[offset++];

                            byte[] valueBytes = new byte[propertySize];
                            Array.Copy(data, offset, valueBytes, 0, propertySize);
                            properties[propertyId] = valueBytes;

                            string name;
                            object value;
                            OpenIoTProtocol.GetDevicePropertyFriendly(propertyId, valueBytes, out name, out value);
                            switch (propertyId)
                            {
                                case DevicePropertyId_Uid:
                                    {
                                        foreach (IOpenIoTProtocolEvents eventHandler in this.EventHandlers)
                                            eventHandler.onDeviceUidReceived(this, (Guid)value);

                                        break;
                                    }

                                case DevicePropertyId_Name:
                                    {
                                        foreach (IOpenIoTProtocolEvents eventHandler in this.EventHandlers)
                                            eventHandler.onDeviceNameReceived(this, (string)value);

                                        break;
                                    }

                                case DevicePropertyId_Password:
                                    {
                                        break;
                                    }

                                case DevicePropertyId_ProjectUid:
                                    {
                                        foreach (IOpenIoTProtocolEvents eventHandler in this.EventHandlers)
                                            eventHandler.onProjectUidReceived(this, (Guid)value);

                                        break;
                                    }

                                case DevicePropertyId_ProjectName:
                                    {
                                        foreach (IOpenIoTProtocolEvents eventHandler in this.EventHandlers)
                                            eventHandler.onProjectNameReceived(this, (string)value);

                                        break;
                                    }

                                case DevicePropertyId_ProjectHash:
                                    {
                                        foreach (IOpenIoTProtocolEvents eventHandler in this.EventHandlers)
                                            eventHandler.onProjectHashReceived(this, (uint)value);

                                        break;
                                    }

                                case DevicePropertyId_UserUid:
                                    {
                                        foreach (IOpenIoTProtocolEvents eventHandler in this.EventHandlers)
                                            eventHandler.onUserUidReceived(this, (Guid)value);

                                        break;
                                    }

                                case DevicePropertyId_FirmwareName:
                                    {
                                        foreach (IOpenIoTProtocolEvents eventHandler in this.EventHandlers)
                                            eventHandler.onFirmwareNameReceived(this, (string)value);

                                        break;
                                    }

                                case DevicePropertyId_FirmwareVendor:
                                    {
                                        foreach (IOpenIoTProtocolEvents eventHandler in this.EventHandlers)
                                            eventHandler.onFirmwareVendorReceived(this, (string)value);

                                        break;
                                    }

                                case DevicePropertyId_FirmwareVersion:
                                    {
                                        foreach (IOpenIoTProtocolEvents eventHandler in this.EventHandlers)
                                            eventHandler.onFirmwareVersionReceived(this, (string)value);

                                        break;
                                    }

                                case DevicePropertyId_BoardName:
                                    {
                                        foreach (IOpenIoTProtocolEvents eventHandler in this.EventHandlers)
                                            eventHandler.onBoardNameReceived(this, (string)value);

                                        break;
                                    }
                            }

                            offset += propertySize;
                        }

                        foreach (IOpenIoTProtocolEvents eventHandler in this.EventHandlers)
                            eventHandler.onDevicePropertiesReceived(this, properties);

                        break;
                    }

                case ResponseCode_UploadSchemeLogic:
                    {
                        foreach (IOpenIoTProtocolEvents eventHandler in this.EventHandlers)
                            eventHandler.onSchemeLogicUploaded(this);

                        break;
                    }

                case ResponseCode_UploadProgramLogic:
                    {
                        foreach (IOpenIoTProtocolEvents eventHandler in this.EventHandlers)
                            eventHandler.onProgramLogicUploaded(this);

                        break;
                    }

                case ResponseCode_ResetLogic:
                    {
                        foreach (IOpenIoTProtocolEvents eventHandler in this.EventHandlers)
                            eventHandler.onResetLogic(this);

                        if (this.logicResetTaskSource != null)
                            this.logicResetTaskSource.SetResult(true);

                        break;
                    }

                case ResponseCode_Reset:
                    {
                        foreach (IOpenIoTProtocolEvents eventHandler in this.EventHandlers)
                            eventHandler.onReset(this);

                        break;
                    }

                default:
                    return base.OnReceiveCommand(command, data, dataSize);
            }

            return true;
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

        TaskCompletionSource<bool> logicResetTaskSource;
        public Task resetLogicAsync()
        {
            this.logicResetTaskSource = new TaskCompletionSource<bool>();
            
            this.resetLogic();

            return this.logicResetTaskSource.Task;
        }

        public void reset()
        {
            this.SendCommand(CommandCode_Reset);
        }

        public void requestDeviceName()
        {
            this.requestDeviceProperties(DevicePropertyId_Name);
        }

        public void requestAllDeviceProperties()
        {
            this.requestDeviceProperties(
                DevicePropertyId_Uid,
                DevicePropertyId_Name,
                //DevicePropertyId_Password,
                DevicePropertyId_ProjectUid,
                DevicePropertyId_ProjectName,
                DevicePropertyId_ProjectHash,
                DevicePropertyId_UserUid,
                DevicePropertyId_FirmwareName,
                DevicePropertyId_FirmwareVendor,
                DevicePropertyId_FirmwareVersion,
                DevicePropertyId_BoardName
            );
        }

        public void requestDeviceProperties(params byte[] propertyIds)
        {
            this.SendCommand(CommandCode_GetDeviceProperties, propertyIds);
        }

        public void requestSetDeviceName(string name)
        {
            byte[] data = new byte[2 + name.Length];
            data[0] = DevicePropertyId_Name;
            data[1] = (byte)name.Length;
            ByteUtils.FromString(name, data, 2);

            this.SendCommand(CommandCode_SetDeviceProperties, data);
        }

        public void requestSetUserId(string userId)
        {
            byte[] userIdBytes = GuidUtils.ToBytes(userId);

            byte[] data = new byte[2 + userIdBytes.Length];
            data[0] = DevicePropertyId_UserUid;
            data[1] = (byte)userIdBytes.Length;
            Array.Copy(userIdBytes, 0, data, 2, userIdBytes.Length);

            this.SendCommand(CommandCode_SetDeviceProperties, data);
        }

        public void requestSetProjectDetails(string projectId, string projectName, string userId)
        {
            byte[] projectIdBytes = GuidUtils.ToBytes(projectId);
            byte[] userIdBytes = GuidUtils.ToBytes(userId);
           
            byte[] data = new byte[2 + projectIdBytes.Length + 2 + projectName.Length + 2 + userIdBytes.Length];
            int offset = 0;
            data[offset++] = DevicePropertyId_ProjectUid;
            data[offset++] = (byte)projectIdBytes.Length;
            Array.Copy(projectIdBytes, 0, data, offset, projectIdBytes.Length);
            offset += projectIdBytes.Length;

            data[offset++] = DevicePropertyId_ProjectName;
            data[offset++] = (byte)projectName.Length;
            ByteUtils.FromString(projectName, data, offset);
            offset += projectName.Length;

            data[offset++] = DevicePropertyId_UserUid;
            data[offset++] = (byte)userIdBytes.Length;
            Array.Copy(userIdBytes, 0, data, offset, userIdBytes.Length);
            offset += userIdBytes.Length;

            this.SendCommand(CommandCode_SetDeviceProperties, data);
        }

        static public bool GetDevicePropertyFriendly(int propertyId, byte[] propertyValue, out string name, out object value)
        {
            switch (propertyId)
            {
                case DevicePropertyId_Uid:
                    {
                        name = "Device Uid";
                        value = GuidUtils.ToGuid(GuidUtils.FromBytes(propertyValue));
                        return true;
                    }

                case DevicePropertyId_Name:
                    {
                        name = "Device Name";
                        value = StringUtils.BytesToString(propertyValue);
                        return true;
                    }

                //case DevicePropertyId_Password:
                //    {
                //        return true;
                //    }

                case DevicePropertyId_ProjectUid:
                    {
                        name = "Project Uid";
                        value = GuidUtils.ToGuid(GuidUtils.FromBytes(propertyValue));
                        return true;
                    }

                case DevicePropertyId_ProjectName:
                    {
                        name = "Project Name";
                        value = StringUtils.BytesToString(propertyValue);
                        return true;
                    }

                case DevicePropertyId_ProjectHash:
                    {
                        name = "Project Hash";
                        value = (uint)ByteUtils.ToInt32(propertyValue, 0);
                        return true;
                    }

                case DevicePropertyId_UserUid:
                    {
                        name = "User Uid";
                        value = GuidUtils.ToGuid(GuidUtils.FromBytes(propertyValue));
                        return true;
                    }

                case DevicePropertyId_FirmwareName:
                    {
                        name = "Firmware Name";
                        value = StringUtils.BytesToString(propertyValue);
                        return true;
                    }

                case DevicePropertyId_FirmwareVendor:
                    {
                        name = "Firmware Vendor";
                        value = StringUtils.BytesToString(propertyValue);
                        return true;
                    }

                case DevicePropertyId_FirmwareVersion:
                    {
                        name = "Firmware Version";
                        value = String.Join('.', propertyValue);
                        return true;
                    }

                case DevicePropertyId_BoardName:
                    {
                        name = "Board";
                        value = StringUtils.BytesToString(propertyValue);
                        return true;
                    }
            }

            name = propertyId.ToString();
            value = propertyValue;
            return false;
        }
    }

}
