package com.palitri.openiot.construction.framework.board.protocol;

import com.palitri.openiot.construction.framework.board.models.BoardProperty;
import com.palitri.openiot.construction.framework.board.models.BoardPropertyType;
import com.palitri.openiot.construction.framework.board.protocol.events.IPropertyTransmissionProtocolEvents;

import java.nio.charset.StandardCharsets;
import java.util.ArrayList;

public abstract class PropertyTransmissionProtocol extends CommandTransmissionProtocol {
    public static final int Code_ResponseBit = 0x80;

    public static final int CommandCode_Ping                            = 0x10;
    public static final int CommandCode_QueryBoardInfo                  = 0x11;
    public static final int CommandCode_QueryPropertiesInfo             = 0x21;
    public static final int CommandCode_QueryPropertiesValues           = 0x22;
    public static final int CommandCode_SetPropertiesValues             = 0x23;
    public static final int CommandCode_SetPropertiesSubscription       = 0x24;
    public static final int CommandCode_ResetPropertiesSubscription     = 0x25;

    public static final int ResponseCode_Ping                           = 0x10 | Code_ResponseBit;
    public static final int ResponseCode_QueryBoardInfo                 = 0x11 | Code_ResponseBit;
    public static final int ResponseCode_QueryPropertiesInfo            = 0x21 | Code_ResponseBit;
    public static final int ResponseCode_QueryPropertiesValues          = 0x22 | Code_ResponseBit;
    public static final int ResponseCode_SetPropertiesValues            = 0x23 | Code_ResponseBit;
    public static final int ResponseCode_SetPropertiesSubscription      = 0x24 | Code_ResponseBit;
    public static final int ResponseCode_ResetPropertiesSubscription    = 0x25 | Code_ResponseBit;

    public static final int ResponseCode_SubscribedPropertiesChanged    = 0x30 | Code_ResponseBit;

    public ArrayList<BoardProperty> properties;
    private ArrayList<BoardProperty> acquisitionProperties;
    private int acquisitionPropertyIndex;

    public ArrayList<IPropertyTransmissionProtocolEvents> eventHandlers;

    public PropertyTransmissionProtocol()
    {
        super();

        this.eventHandlers = new ArrayList<IPropertyTransmissionProtocolEvents>();
    }

    @Override
    public boolean OnReceiveCommand(byte command, byte[] data, int dataSize)
    {
        switch ((int) (command & 0xff)) {
            case CommandCode_Ping: {
                this.SendCommand(ResponseCode_Ping, data);

                for (IPropertyTransmissionProtocolEvents eventHandler : this.eventHandlers)
                    eventHandler.onPingRequest(this, data);

                break;
            }

            case CommandCode_QueryBoardInfo: {
                this.SendCommand(ResponseCode_QueryBoardInfo, "Android client".getBytes(StandardCharsets.US_ASCII));

                break;
            }

            case ResponseCode_Ping: {
                for (IPropertyTransmissionProtocolEvents eventHandler : this.eventHandlers)
                    eventHandler.onPingBack(this, data);

                break;
            }

            case ResponseCode_QueryBoardInfo: {
                for (IPropertyTransmissionProtocolEvents eventHandler : this.eventHandlers)
                    eventHandler.onInfoReceived(this, new String(data));

                break;
            }

            case ResponseCode_QueryPropertiesInfo: {
                int offset = 0;
                while (offset < dataSize) {
                    int semantic = data[offset++] & 0xff;
                    int type = data[offset++] & 0xff;
                    int flags = data[offset++] & 0xff;

                    BoardProperty property = new BoardProperty();
                    offset += property.Init(this.acquisitionPropertyIndex, data, offset, BoardPropertyType.FromValue(type), semantic, flags);
                    this.acquisitionProperties.add(property);

                    for (IPropertyTransmissionProtocolEvents eventHandler : this.eventHandlers)
                        eventHandler.onPropertyInfoReceived(this, property);

                    this.acquisitionPropertyIndex++;
                }

                if (dataSize > 1)
                    this.SendCommand(CommandCode_QueryPropertiesInfo, new byte[] { (byte)this.acquisitionPropertyIndex, 10 });
                else {
                    this.properties = this.acquisitionProperties;
                    this.acquisitionProperties = null;

                    for (IPropertyTransmissionProtocolEvents eventHandler : this.eventHandlers)
                        eventHandler.onAllPropertiesInfoReceived(this);
                }

                break;
            }

            case ResponseCode_SubscribedPropertiesChanged: {
                int offset = 0;
                int propertiesCount = data[offset++] & 0xff;

                for (int i = 0; i < propertiesCount; i++) {
                    int propertyIndex = data[offset++] & 0xff;

                    if (propertyIndex < this.properties.size()) {
                        BoardProperty property = this.properties.get(propertyIndex);

                        offset += property.setValue(data, offset);

                        for (IPropertyTransmissionProtocolEvents eventHandler : this.eventHandlers)
                            eventHandler.onSubscribedPropertyValueChanged(this, property, null);
                    }
                }

                break;
            }

            case ResponseCode_SetPropertiesSubscription: {
                for (IPropertyTransmissionProtocolEvents eventHandler : this.eventHandlers)
                    eventHandler.onPropertiesSubscriptionSet(this);

                break;
            }

            case ResponseCode_ResetPropertiesSubscription: {
                for (IPropertyTransmissionProtocolEvents eventHandler : this.eventHandlers)
                    eventHandler.onPropertiesChangedSubscriptionReset(this);

                break;
            }

            default:
                return false;
        }

        return true;
    }

    public void RequestPropertyUpdate(BoardProperty p) {
        this.RequestPropertiesUpdate(new BoardProperty[]{ p });
    }

    public void RequestPropertiesUpdate(BoardProperty[] properties) {
        int count = properties.length;

        int payloadSize = 0;
        for (BoardProperty p : properties)
            payloadSize += 1 + p.size();

        byte[] data = new byte[1 + payloadSize];

        int dataIndex = 0;
        data[dataIndex++] = (byte)count;
        for (BoardProperty p : properties)
        {
            data[dataIndex++] = (byte)p.index;

            dataIndex += p.getValue(data, dataIndex);
        }

        this.SendCommand(CommandCode_SetPropertiesValues, data);
    }

    public void requestProperties()
    {
        this.acquisitionProperties = new ArrayList<BoardProperty>();
        acquisitionPropertyIndex = 0;
        this.SendCommand(CommandCode_QueryPropertiesInfo, new byte[] { (byte) acquisitionPropertyIndex, 10 });
    }

    public void requestBoardInfo() {
        this.SendCommand(CommandCode_QueryBoardInfo);
    }

    public void requestPropertiesChangedSubscription(byte[] subscriptionPropertiesIndices)
    {
        int subscriptionPropertiesCount = subscriptionPropertiesIndices.length;
        byte[] subscriptionBuffer = new byte[subscriptionPropertiesCount + 2];
        subscriptionBuffer[0] = 1;
        subscriptionBuffer[1] = (byte) subscriptionPropertiesCount;
        for (int i = 0; i < subscriptionPropertiesCount; i++)
            subscriptionBuffer[2 + i] = (byte) subscriptionPropertiesIndices[i];

        this.SendCommand(CommandCode_SetPropertiesSubscription, subscriptionBuffer);
    }

    public void requestPropertiesChangedSubscriptionReset()
    {
        this.SendCommand(CommandCode_ResetPropertiesSubscription);
    }

    public void requestPing(byte[] data)
    {
        this.SendCommand(CommandCode_Ping, data);
    }
}
