package com.palitri.openiot.construction.framework.board.models;

import com.palitri.openiot.construction.framework.tools.utils.ArrayUtils;
import com.palitri.openiot.construction.framework.tools.utils.ByteUtils;
import com.palitri.openiot.construction.framework.tools.utils.StringUtils;

public class BoardProperty {
    public int index;
    public Object value;
    public int size;
    public int semantic;
    public int flags;
    public BoardPropertyType type;

    public static final int Flag_Read = 1 << 0;
    public static final int Flag_Write = 1 << 1;
    public static final int Flag_Subscribed = 1 << 7;

    public BoardProperty()
    {
    }

    public BoardProperty(int index, int value, int semantic, int flags)
    {
        this.Init(index, ByteUtils.fromInt32(value), 0, BoardPropertyType.Bool, semantic, flags);
    }

    public BoardProperty(int index, float value, int semantic, int flags)
    {
        this.Init(index, ByteUtils.fromFloat(value), 0, BoardPropertyType.Bool, semantic, flags);
    }

    public BoardProperty(int index, boolean value, int semantic, int flags)
    {
        this.Init(index, new byte[] { ByteUtils.fromBool(value) }, 0, BoardPropertyType.Bool, semantic, flags);
    }

    public BoardProperty(int index, byte[] data, int dataOffset, BoardPropertyType dataType, int semantic, int flags)
    {
        this.Init(index, data, dataOffset, dataType, semantic, flags);
    }

    public BoardProperty(BoardProperty pOriginal, Object pValue)
    {
        this.index = pOriginal.index;
        this.semantic = pOriginal.semantic;
        this.flags = pOriginal.flags;
        this.type = pOriginal.type;
        this.size = pOriginal.size;

        if (this.type == BoardPropertyType.Data)
            this.setValue((byte[])pValue, 0);
        else
            this.value = pValue;
    }

    public int Init(int index, byte[] data, int dataOffset, BoardPropertyType dataType, int semantic, int flags)
    {
        int offset = 0;

        this.index = index;
        this.semantic = semantic;
        this.flags = flags;
        this.type = dataType;

        switch (dataType) {
            case Integer: {
                this.value = new Integer(ByteUtils.toInt32(data, dataOffset + offset));
                this.size = this.type.GetStaticSize();
                offset += this.size;
                break;
            }
            case Float: {
                this.value = new Float(ByteUtils.toFloat(data, dataOffset + offset));
                this.size = this.type.GetStaticSize();
                offset += this.size;
                break;
            }
            case Bool: {
                this.value = new Boolean(ByteUtils.toBool(data, dataOffset + offset));
                this.size = this.type.GetStaticSize();
                offset += this.size;
                break;
            }
            case Data: {
                this.size = data[dataOffset + offset];
                offset++;
                this.value = new byte[this.size];
                ArrayUtils.Copy(data, dataOffset +  offset, this.value, 0, this.size);
                offset += this.size;
                break;
            }
        }

        return offset;
    }

    public boolean isWriteable()
    {
        return (this.flags & Flag_Write) != 0;
    }

    public boolean isReadable()
    {
        return (this.flags & Flag_Read) != 0;
    }

    public boolean isSubscribed()
    {
        return (this.flags & Flag_Subscribed) != 0;
    }

    public int size()
    {
        return this.size;
    }

    public int setValue(byte[] data, int dataOffset)
    {
        int offset = 0;

        switch (this.type)
        {
            case Integer: {
                this.value = ByteUtils.toInt32(data, dataOffset);
                offset += this.size();
                break;
            }

            case Float: {
                this.value = ByteUtils.toFloat(data, dataOffset);
                offset += this.size();
                break;
            }

            case Bool: {
                this.value = ByteUtils.toBool(data, dataOffset);
                offset += this.size();
                break;
            }

            case Data: {
                this.size = data[dataOffset];
                offset++;
                this.value = new byte[this.size];
                ArrayUtils.Copy(data, dataOffset +  1, this.value, 0, size);
                offset += this.size;
                break;
            }
        }

        return offset;
    }

    public int getValue(byte[] data, int offset)
    {
        switch (this.type)
        {
            case Integer: {
                return ByteUtils.fromInt32((int)this.value, data, offset);
            }

            case Float: {
                return ByteUtils.fromFloat((float)this.value, data, offset);
            }

            case Bool: {
                return ByteUtils.fromBool((boolean)this.value, data, offset);
            }

            case Data: {
                int size = this.size();
                data[offset] = (byte)size;
                ArrayUtils.Copy(data, offset, this.value, offset + 1, size);
                return 1 + size;
            }
        }

        return 0;
    }

    public int getInt()
    {
        return (int)this.value;
    }
    public float getFloat()
    {
        return (float)this.value;
    }
    public boolean getBool()
    {
        return (boolean)this.value;
    }

    public String getString()
    {
        return StringUtils.BytesToHex((byte[])this.value);
    }
}
