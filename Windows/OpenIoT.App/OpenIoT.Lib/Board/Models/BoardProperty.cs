using OpenIoT.Lib.Tools.Utils;

namespace OpenIoT.Lib.Board.Models
{
    public class BoardProperty
    {
        public int index;
        public object value;
        public int size;
        public int semantic;
        public int flags;
        public BoardPropertyType type;

        public static int Flag_Read = 1 << 0;
        public static int Flag_Write = 1 << 1;
        public static int Flag_Subscribed = 1 << 7;

        public static int Flag_Default = Flag_Read;

        public BoardProperty()
        {
        }

        public BoardProperty(int index, int value, int semantic, int flags)
        {
            this.Init(index, ByteUtils.FromInt32(value), 0, BoardPropertyType.Bool, semantic, flags);
        }

        public BoardProperty(int index, float value, int semantic, int flags)
        {
            this.Init(index, ByteUtils.FromFloat(value), 0, BoardPropertyType.Bool, semantic, flags);
        }

        public BoardProperty(int index, bool value, int semantic, int flags)
        {
            this.Init(index, new byte[] { ByteUtils.FromBool(value) }, 0, BoardPropertyType.Bool, semantic, flags);
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
                this.SetValue((byte[])pValue, 0);
            else
                this.value = pValue;
        }

        private static readonly int[] sizes = { 0, 0, 4, 4, 1, 0 };

        public int Init(int index, byte[] data, int dataOffset, BoardPropertyType dataType, int semantic, int flags)
        {
            int offset = 0;

            this.index = index;
            this.semantic = semantic;
            this.flags = flags;
            this.type = dataType;


            switch (dataType)
            {
                case BoardPropertyType.Integer:
                    {
                        this.value = ByteUtils.ToInt32(data, dataOffset + offset);
                        this.size = sizes[(int)this.type];
                        offset += this.size;
                        break;
                    }
                case BoardPropertyType.Float:
                    {
                        this.value = ByteUtils.ToFloat(data, dataOffset + offset);
                        this.size = sizes[(int)this.type];
                        offset += this.size;
                        break;
                    }
                case BoardPropertyType.Bool:
                    {
                        this.value = ByteUtils.ToBool(data, dataOffset + offset);
                        this.size = sizes[(int)this.type];
                        offset += this.size;
                        break;
                    }
                case BoardPropertyType.Data:
                    {
                        this.size = data[dataOffset + offset];
                        offset++;
                        this.value = new byte[this.size];
                        ArrayUtils.Copy(data, dataOffset +  offset, (byte[])this.value, 0, this.size);
                        offset += this.size;
                        break;
                    }
            }

            return offset;
        }

        public bool IsWriteable()
        {
            return (this.flags & Flag_Write) != 0;
        }

        public bool IsReadable()
        {
            return (this.flags & Flag_Read) != 0;
        }

        public bool sSubscribed()
        {
            return (this.flags & Flag_Subscribed) != 0;
        }

        public int Size()
        {
            return this.size;
        }

        public void SetValue(object value)
        {
            switch (this.type)
            {
                case BoardPropertyType.Integer:
                    this.value = (int)value;
                    break;

                case BoardPropertyType.Float:
                    this.value = (float)value;
                    break;

                case BoardPropertyType.Bool:
                    this.value = (bool)value;
                    break;

                case BoardPropertyType.Data:
                    ArrayUtils.Copy((byte[])value, 0, (byte[])this.value, 0, this.size);
                    break;
            }
        }

        public int SetValue(byte[] data, int dataOffset)
        {
            int offset = 0;

            switch (this.type)
            {
                case BoardPropertyType.Integer:
                    {
                        this.value = ByteUtils.ToInt32(data, dataOffset);
                        offset += this.Size();
                        break;
                    }

                case BoardPropertyType.Float:
                    {
                        this.value = ByteUtils.ToFloat(data, dataOffset);
                        offset += this.Size();
                        break;
                    }

                case BoardPropertyType.Bool:
                    {
                        this.value = ByteUtils.ToBool(data, dataOffset);
                        offset += this.Size();
                        break;
                    }

                case BoardPropertyType.Data:
                    {
                        this.size = data[dataOffset];
                        offset++;
                        this.value = new byte[this.size];
                        ArrayUtils.Copy(data, dataOffset +  1, (byte[])this.value, 0, size);
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
                case BoardPropertyType.Integer:
                    {
                        return ByteUtils.FromInt32((int)this.value, data, offset);
                    }

                case BoardPropertyType.Float:
                    {
                        return ByteUtils.FromFloat((float)this.value, data, offset);
                    }

                case BoardPropertyType.Bool:
                    {
                        return ByteUtils.FromBool((bool)this.value, data, offset);
                    }

                case BoardPropertyType.Data:
                    {
                        int size = this.Size();
                        data[offset] = (byte)size;
                        ArrayUtils.Copy(data, offset, (byte[])this.value, offset + 1, size);
                        return 1 + size;
                    }
            }

            return 0;
        }

        public int GetInt()
        {
            return (int)this.value;
        }
        public float GetFloat()
        {
            return (float)this.value;
        }
        public bool GetBool()
        {
            return (bool)this.value;
        }

        public string GetString()
        {
            return StringUtils.BytesToHex((byte[])this.value);
        }
    }

}
