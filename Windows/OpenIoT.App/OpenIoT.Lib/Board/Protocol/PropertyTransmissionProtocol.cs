using Palitri.OpenIoT.Board.Models;
using Palitri.OpenIoT.Board.Protocol.Events;
using Palitri.OpenIoT.Tools.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Palitri.OpenIoT.Board.Protocol
{
    public abstract class PropertyTransmissionProtocol : CommandTransmissionProtocol
    {
        public const byte Code_ResponseBit = 0x80;

        public const byte CommandCode_Ping                          = 0x10;
        public const byte CommandCode_Info                          = 0x11;
        public const byte CommandCode_QueryPropertiesInfo           = 0x21;
        public const byte CommandCode_QueryPropertiesValues         = 0x22;
        public const byte CommandCode_SetPropertiesValues           = 0x23;
        public const byte CommandCode_SetPropertiesSubscription     = 0x24;
        public const byte CommandCode_ResetPropertiesSubscription   = 0x25;


        public const byte ResponseCode_Ping                         = 0x10 | Code_ResponseBit;
        public const byte ResponseCode_Info                         = 0x11 | Code_ResponseBit;
        public const byte ResponseCode_QueryPropertiesInfo          = 0x21 | Code_ResponseBit;
        public const byte ResponseCode_QueryPropertiesValues        = 0x22 | Code_ResponseBit;
        public const byte ResponseCode_SetPropertiesValues          = 0x23 | Code_ResponseBit;
        public const byte ResponseCode_SetPropertiesSubscription    = 0x24 | Code_ResponseBit;
        public const byte ResponseCode_ResetPropertiesSubscription  = 0x25 | Code_ResponseBit;

        public const byte ResponseCode_SubscribedPropertiesChanged  = 0x30 | Code_ResponseBit;


        public List<BoardProperty> properties;
        private List<BoardProperty> acquisitionProperties;
        private int acquisitionPropertyIndex;

        public List<IPropertyTransmissionProtocolEvents> EventHandlers { get; private set; }

        public PropertyTransmissionProtocol()
            : base()
        {
            this.EventHandlers = new List<IPropertyTransmissionProtocolEvents>();
        }

        public override bool OnReceiveCommand(byte command, byte[] data, int dataSize)
        {
            switch (command)
            {
                case CommandCode_Ping:
                    {
                        this.SendCommand(ResponseCode_Ping, data);

                        break;
                    }

                case CommandCode_Info:
                    {
                        byte[] info = "Windows client".Select(c => (byte)c).ToArray();
                        this.SendCommand(ResponseCode_Info, info);

                        break;
                    }


                case ResponseCode_Ping:
                    {
                        foreach (IPropertyTransmissionProtocolEvents eventHandler in this.EventHandlers)
                            eventHandler.OnPingBack(this, data);

                        break;
                    }


                case ResponseCode_Info:
                    {
                        string infoString = ByteUtils.ToString(data, 0, data.Length);

                        foreach (IPropertyTransmissionProtocolEvents eventHandler in this.EventHandlers)
                            eventHandler.OnInfoReceived(this, infoString);

                        break;
                    }

                case ResponseCode_QueryPropertiesInfo:
                    {
                        int offset = 0;
                        while (offset < dataSize)
                        {
                            int semantic = data[offset++] & 0xff;
                            int type = data[offset++] & 0xff;
                            int flags = data[offset++] & 0xff;

                            BoardProperty property = new BoardProperty();
                            offset += property.Init(this.acquisitionPropertyIndex, data, offset, (BoardPropertyType)type, semantic, flags);
                            this.acquisitionProperties.Add(property);

                            foreach (IPropertyTransmissionProtocolEvents eventHandler in this.EventHandlers)
                                eventHandler.OnPropertyInfoReceived(this, property);

                            this.acquisitionPropertyIndex++;
                        }

                        if (dataSize > 1)
                            this.SendCommand(CommandCode_QueryPropertiesInfo, new byte[] { (byte)this.acquisitionPropertyIndex, 10 });
                        else
                        {
                            this.properties = this.acquisitionProperties;
                            this.acquisitionProperties = null;
                            foreach (IPropertyTransmissionProtocolEvents eventHandler in this.EventHandlers)
                                eventHandler.OnAllPropertiesInfoReceived(this);
                        }

                        break;
                    }

                case ResponseCode_SubscribedPropertiesChanged:
                    {
                        int offset = 0;
                        int propertiesCount = data[offset++] & 0xff;

                        for (int i = 0; i < propertiesCount; i++)
                        {
                            int propertyIndex = data[offset++] & 0xff;

                            if (propertyIndex < this.properties.Count)
                            {
                                BoardProperty property = this.properties[propertyIndex];

                                offset += property.SetValue(data, offset);

                                foreach (IPropertyTransmissionProtocolEvents eventHandler in this.EventHandlers)
                                    eventHandler.OnSubscribedPropertyValueChanged(this, property, null);
                            }
                        }

                        break;
                    }

                case ResponseCode_SetPropertiesSubscription:
                    {
                        foreach (IPropertyTransmissionProtocolEvents eventHandler in this.EventHandlers)
                            eventHandler.OnPropertiesSubscriptionSet(this);

                        break;
                    }

                case ResponseCode_ResetPropertiesSubscription:
                    {
                        foreach (IPropertyTransmissionProtocolEvents eventHandler in this.EventHandlers)
                            eventHandler.OnPropertiesChangedSubscriptionReset(this);

                        break;
                    }

                default:
                    return false;
            }

            return true;
        }

        public void RequestPropertyUpdate(BoardProperty p)
        {
            this.RequestPropertiesUpdate(new BoardProperty[] { p });
        }

        public void RequestPropertiesUpdate(BoardProperty[] properties)
        {
            int count = properties.Length;

            int payloadSize = 0;
            foreach (BoardProperty p in properties)
                payloadSize += 1 + p.Size() + (p.type == BoardPropertyType.Data ? 1 : 0);

            byte[] data = new byte[1 + payloadSize];

            int dataIndex = 0;
            data[dataIndex++] = (byte)count;
            foreach (BoardProperty p in properties)
            {
                data[dataIndex++] = (byte)p.index;

                dataIndex += p.getValue(data, dataIndex);
            }

            this.SendCommand(CommandCode_SetPropertiesValues, data);
        }

        public void RequestProperties()
        {
            this.acquisitionProperties = new List<BoardProperty>();
            this.acquisitionPropertyIndex = 0;
            this.SendCommand(CommandCode_QueryPropertiesInfo, new byte[] { (byte)this.acquisitionPropertyIndex, 10 });
        }


        public void RequestBoardInfo()
        {
            this.SendCommand(CommandCode_Info);
        }

        public void RequestPropertiesChangedSubscription(byte[] subscriptionPropertiesIndices)
        {
            int subscriptionPropertiesCount = subscriptionPropertiesIndices.Length;
            byte[] subscriptionBuffer = new byte[subscriptionPropertiesCount + 2];
            subscriptionBuffer[0] = 1;
            subscriptionBuffer[1] = (byte)subscriptionPropertiesCount;
            for (int i = 0; i < subscriptionPropertiesCount; i++)
                subscriptionBuffer[2 + i] = (byte)subscriptionPropertiesIndices[i];

            this.SendCommand(CommandCode_SetPropertiesSubscription, subscriptionBuffer);
        }

        public void RequestPropertiesChangedSubscriptionReset()
        {
            this.SendCommand(CommandCode_ResetPropertiesSubscription);
        }

        public void RequestPing(byte[] data = null)
        {
            this.SendCommand(CommandCode_Ping, data);
        }
    }
}
