package com.palitri.openiot.construction.framework.tools.utils;

public class ByteUtils {
    public static int unsign(byte value) {
        return value & 0xff;
    }



    public static boolean toBool(byte[] data, int offset)
    {
        return data[offset] != 0;
    }

    public static int fromBool(boolean value, byte[] data, int offset)
    {
        data[offset] = value ? (byte)1 : (byte)0;
        return 1;
    }

    public static byte fromBool(boolean value)
    {
        return value ? (byte)1 : (byte)0;
    }



    public static short toInt16(byte[] data, int offset) {
        return (short) (unsign(data[offset]) | ((unsign(data[offset + 1]) << 8) & 0xFF00));
    }

    public static int fromInt16(int value, byte[] data, int offset)
    {
        data[offset] = (byte)(value & 0xFF);
        data[offset + 1] = (byte)((value >> 8) & 0xFF);

        return 2;
    }

    public static byte[] fromInt16(int value)
    {
        byte[] result = new byte[2];
        fromInt16(value, result, 0);
        return result;
    }



    public static int toInt32(byte[] data, int offset) {
        return (int) (unsign(data[offset]) | ((unsign(data[offset + 1]) << 8) & 0xFF00) | ((unsign(data[offset + 2]) << 16) & 0xFF0000) | ((unsign(data[offset + 3]) << 24) & 0xFF000000));
    }

    public static int fromInt32(int value, byte[] data, int offset)
    {
        data[offset] = (byte)(value & 0xFF);
        data[offset + 1] = (byte)((value >> 8) & 0xFF);
        data[offset + 2] = (byte)((value >> 16) & 0xFF);
        data[offset + 3] = (byte)((value >> 24) & 0xFF);

        return 4;
    }

    public static byte[] fromInt32(int value)
    {
        byte[] result = new byte[4];
        fromInt32(value, result, 0);
        return result;
    }



    public static long toInt64(byte[] data, int offset) {
            return (long) ((long)unsign(data[offset]) |
                (((long)unsign(data[offset + 1]) << 8) & 0xFF00L) |
                (((long)unsign(data[offset + 2]) << 16) & 0xFF0000L) |
                (((long)unsign(data[offset + 3]) << 24) & 0xFF000000L) |
                (((long)unsign(data[offset + 4]) << 32) & 0xFF00000000L) |
                (((long)unsign(data[offset + 5]) << 40) & 0xFF0000000000L) |
                (((long)unsign(data[offset + 6]) << 48) & 0xFF000000000000L) |
                (((long)unsign(data[offset + 7]) << 56) & 0xFF00000000000000L));
    }

    public static long fromInt64(long value, byte[] data, int offset)
    {
        data[offset] = (byte)(value & 0xFF);
        data[offset + 1] = (byte)((value >> 8) & 0xFF);
        data[offset + 2] = (byte)((value >> 16) & 0xFF);
        data[offset + 3] = (byte)((value >> 24) & 0xFF);
        data[offset + 4] = (byte)((value >> 32) & 0xFF);
        data[offset + 5] = (byte)((value >> 40) & 0xFF);
        data[offset + 6] = (byte)((value >> 48) & 0xFF);
        data[offset + 7] = (byte)((value >> 56) & 0xFF);

        return 8;
    }

    public static byte[] fromInt64(long value)
    {
        byte[] result = new byte[8];
        fromInt64(value, result, 0);
        return result;
    }



    public static float toFloat(byte[] data, int offset) {
        return Float.intBitsToFloat(toInt32(data, offset));
    }

    public static int fromFloat(float value, byte[] data, int offset)
    {
        int bits = Float.floatToIntBits(value);
        data[offset] = (byte)(bits & 0xFF);
        data[offset + 1] = (byte)((bits >> 8) & 0xFF);
        data[offset + 2] = (byte)((bits >> 16) & 0xFF);
        data[offset + 3] = (byte)((bits >> 24) & 0xFF);

        return 4;
    }

    public static byte[] fromFloat(float value)
    {
        byte[] result = new byte[4];
        fromFloat(value, result, 0);
        return result;
    }

}
