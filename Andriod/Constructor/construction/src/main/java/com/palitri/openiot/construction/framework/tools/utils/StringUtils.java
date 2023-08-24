package com.palitri.openiot.construction.framework.tools.utils;

import java.util.HashMap;
import java.util.Map;

public class StringUtils {

    public static boolean IsNullOrEmpty(String str)
    {
        return str == null || str.trim().isEmpty();
    }

    public static byte[] StringToBytes(String text)
    {
        int length = text.length();

        byte[] result = new byte[length];

        for (int i = 0; i < length; i++)
            result[i] = (byte)(int)text.charAt(i);

        return result;
    }

    public static String BytesToString(byte[] ascii)
    {
        int length = ascii == null ? 0 : ascii.length;

        char[] result = new char[length];

        for (int i = 0; i < length; i++)
        {
            byte value = ascii[i];
            if (value == 0)
                break;

            result[i] = (char)value;
        }

        return new String(result);
    }

    private static char[] hexDigits = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F' };
    private static Map<Character, Integer> hexValues = new HashMap<Character, Integer>() {{
        put('0', 0x00);
        put('1', 0x01);
        put('2', 0x02);
        put('3', 0x03);
        put('4', 0x04);
        put('5', 0x05);
        put('6', 0x06);
        put('7', 0x07);
        put('8', 0x08);
        put('9', 0x09);
        put('A', 0x0A);
        put('B', 0x0B);
        put('C', 0x0C);
        put('D', 0x0D);
        put('E', 0x0E);
        put('F', 0x0F);
    }};

    public static String BytesToHex(byte[] bytesArray)
    {
        String result = "";
        int length = bytesArray.length;
        for (int i = 0; i < length; i++)
        {
            int v = ByteUtils.unsign(bytesArray[i]);
            char c = hexDigits[v >> 4];
                result += c;

            c = hexDigits[v & 15];
                result += c;
        }

        return result;
    }

    public static byte[] HexToBytes(String hexString)
    {
        hexString = hexString.toUpperCase();

        int len = hexString.length();
        int halfLen = len >> 1;
        byte[] result = new byte[halfLen + (len & 1)];

        if (!StringUtils.IsNullOrEmpty(hexString))
        {
            int index = 0;
            for (int i = 0; i < halfLen; i++)
            {
                int v = hexValues.get(hexString.charAt(index++)) << 4;
                v |= hexValues.get(hexString.charAt(index++));
                result[i] = (byte)v;
            }

            if ((len & 1) != 0)
                result[halfLen] = (byte)(int)hexValues.get(hexString.charAt(len - 1));
        }

        return result;
    }

    public static String Join(String separator, Iterable elements)
    {
        StringBuilder result = new StringBuilder();

        boolean isFirst = true;
        for (Object element : elements)
        {
            if (!isFirst)
                result.append(separator);
            result.append(element);
            isFirst = false;
        }

        return result.toString();
    }

}
