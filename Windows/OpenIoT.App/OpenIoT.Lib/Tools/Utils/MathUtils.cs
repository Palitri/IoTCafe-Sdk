using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenIoT.Lib.Tools.Utils
{
    public class MathUtils
    {
        static public short CRC16(byte[] source, int size, int offset = 0, short seed = 0x1D0F)
        {
	        return CRC16Calculate(source, size, offset, CRC16Init(seed));
        }

        static public short CRC16Init(short seed = 0x1D0F)
        {
            return seed;
        }

        /// <summary>
        /// CRC16-CCITT, polynomial 0x1021 (0b1000000100001), that is x^16 + x^12 + x^5 + 1
        /// </summary>
        static public short CRC16Calculate(byte[] source, int size, int offset = 0, short seed = 0x1D0F)
        {
            int result = seed;

            for (int i = 0; i < size; i++)
            {
                int x = (result >> 8) ^ source[offset + i];
                x ^= x >> 4;
                result = ((result << 8) & 0xFFFF) ^ ((x << 12) & 0xFFFF) ^ ((x << 5) & 0xFFFF) ^ x;
            }

            return (short)result;
        }
    }
}
