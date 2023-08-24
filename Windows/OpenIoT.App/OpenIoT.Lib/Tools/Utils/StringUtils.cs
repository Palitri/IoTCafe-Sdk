using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenIoT.Lib.Tools.Utils
{
    public class StringUtils
    {

        public static bool IsNullOrEmpty(string str)
        {
            return String.IsNullOrWhiteSpace(str);
        }

        public static byte[] StringToBytes(string text)
        {
            return text.Select(c => (byte)c).ToArray();
        }

        public static string BytesToString(byte[] ascii)
        {
            return new string(ascii.Select(b => (char)b).ToArray());
        }

        public static string BytesToHex(byte[] bytesArray)
        {
            return Convert.ToHexString(bytesArray);
        }

        public static byte[] HexToBytes(string hexString)
        {
            return Convert.FromHexString(hexString);
        }

    }
}
