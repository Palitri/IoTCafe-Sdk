using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenIoT.Lib.Tools.Utils
{
    public class ByteUtils
    {
        public static int Unsign(byte value)
        {
            return value;
        }

        public static bool ToBool(byte[] data, int offset)
        {
            return data[offset] != 0;
        }

        public static int FromBool(bool value, byte[] data, int offset)
        {
            data[offset] = value ? (byte)1 : (byte)0;
            return 1;
        }

        public static byte FromBool(bool value)
        {
            return value ? (byte)1 : (byte)0;
        }



        public static short ToInt16(byte[] data, int offset)
        {
            return (short)(data[offset] | ((data[offset + 1] << 8) & 0xFF00));
        }

        public static int FromInt16(int value, byte[] data, int offset)
        {
            data[offset] = (byte)(value & 0xFF);
            data[offset + 1] = (byte)((value >> 8) & 0xFF);

            return 2;
        }

        public static byte[] FromInt16(int value)
        {
            byte[] result = new byte[2];
            FromInt16(value, result, 0);
            return result;
        }



        public static int ToInt32(byte[] data, int offset)
        {
            return (int)(data[offset] | ((data[offset + 1] << 8) & 0xFF00) | ((data[offset + 2] << 16) & 0xFF0000) | ((data[offset + 3] << 24) & 0xFF000000));
        }

        public static int FromInt32(int value, byte[] data, int offset)
        {
            data[offset] = (byte)(value & 0xFF);
            data[offset + 1] = (byte)((value >> 8) & 0xFF);
            data[offset + 2] = (byte)((value >> 16) & 0xFF);
            data[offset + 3] = (byte)((value >> 24) & 0xFF);

            return 4;
        }

        public static byte[] FromInt32(int value)
        {
            byte[] result = new byte[4];
            FromInt32(value, result, 0);
            return result;
        }



        public static float ToFloat(byte[] data, int offset)
        {
            return BitConverter.ToSingle(data, offset);
        }

        public static int FromFloat(float value, byte[] data, int offset)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            data[offset] = bytes[0];
            data[offset + 1] = bytes[1];
            data[offset + 2] = bytes[2];
            data[offset + 3] = bytes[3];

            return 4;
        }

        public static byte[] FromFloat(float value)
        {
            byte[] result = new byte[4];
            FromFloat(value, result, 0);
            return result;
        }



        public static string ToString(byte[] data, int offset, int length)
        {
            char[] chars = new char[length];
            for (int i = 0; i < length; i++)
                chars[i] = (char)data[offset + i];

            return new string(chars);
        }

        public static int FromString(string value, byte[] data, int offset)
        {
            int length = value.Length;
            for (int i = 0; i < length; i++)
                data[offset + i] = (byte)value[i];

            return length;
        }

        public static byte[] FromString(string value)
        {
            byte[] result = new byte[value.Length];
            FromString(value, result, 0);
            return result;
        }

    }

}
