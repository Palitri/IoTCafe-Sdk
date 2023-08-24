using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenIoT.Lib.Tools.Utils
{
    public class ArrayUtils
    {
        public static void Copy(Array src, int srcOffset, Array dest, int destOffset, int length)
        {
            Array.Copy(src, srcOffset, dest, destOffset, length);
        }

        public static byte[] CopyNew(Array src, int srcOffset, int length)
        {
            byte[] newCopy = new byte[length];
            Array.Copy(src, srcOffset, newCopy, 0, length);

            return newCopy;
        }

        public static byte[] CommaSeparatedValuesToBytes(String commaSeparatedValues)
        {
            if (StringUtils.IsNullOrEmpty(commaSeparatedValues))
                return Array.Empty<byte>();

            string[] values = commaSeparatedValues.Split(",");
            byte[] result = new byte[values.Length];
            for (int i = 0; i < values.Length; i++)
            {
                String value = values[i].Trim();
                result[i] = (byte)int.Parse(value);
            }

            return result;
        }

        private static readonly string HEX_ARRAY = "0123456789ABCDEF";

        public static byte[] hexToBytes(string hex)
        {
            int l = hex.Length;
            byte[] result = new byte[l / 2];

            int i = hex.StartsWith("0x") ? 2 : 0;
            while (i < l)
            {
                char c = hex[i];
                for (int index = 0; index < 16; index++)
                {
                    if (c == HEX_ARRAY[index])
                    {
                        if (i % 2 == 0)
                            result[i / 2] = (byte)(index << 4);
                        else
                            result[i / 2] |= (byte)(index);
                    }
                }

                i++;
            }

            return result;
        }

        public static string bytesToHex(byte[] bytes)
        {
            if (bytes == null)
                return "";

            char[] hexChars = new char[bytes.Length * 2];
            for (int i = 0; i < bytes.Length; i++)
            {
                int v = bytes[i];
                hexChars[i * 2] = HEX_ARRAY[v >> 4];
                hexChars[i * 2 + 1] = HEX_ARRAY[v & 0x0F];
            }

            return new string(hexChars);
        }
    }

}
