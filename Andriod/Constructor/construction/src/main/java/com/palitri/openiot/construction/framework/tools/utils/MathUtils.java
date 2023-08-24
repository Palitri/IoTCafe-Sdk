package com.palitri.openiot.construction.framework.tools.utils;

public class MathUtils {
    static public int unsign(byte value)
    {
        return value & 0xff;
    }

    static public int unsign(short value)
    {
        return value & 0xffff;
    }

    static public short CRC16(byte[] source, int size, int offset) {
        return CRC16Calculate(source, size, offset, CRC16Init());
    }

    static public short CRC16(byte[] source, int size) {
        return CRC16(source, size, 0);
    }

    static public short CRC16Init(short seed)
    {
        return seed;
    }

    static public short CRC16Init()
    {
        return CRC16Init((short) 0x1D0F);
    }

    // CRC16-CCITT, polynomial 0x1021 (0b1000000100001), that is x^16 + x^12 + x^5 + 1
    static public short CRC16Calculate(byte[] source, int size, int offset, short seed) {
        int result = unsign(seed);

        for (int i = 0; i < size; i++) {
            int x = (result >> 8) ^ unsign(source[offset + i]);
            x ^= x >> 4;
            result = ((result << 8) & 0xFFFF) ^ ((x << 12) & 0xFFFF) ^ ((x << 5) & 0xFFFF) ^ x;
        }

        return (short)result;
    }
}
