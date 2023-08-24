package com.palitri.openiot.construction.framework.tools.utils;

public class ArrayUtils {
    public static void Copy(Object src, int srcOffset, Object dest, int destOffset, int length)
    {
        System.arraycopy(src, srcOffset, dest, destOffset, length);
    }

    public static byte[] CopyNew(Object src, int srcOffset, int length)
    {
        byte[] newCopy = new byte[length];
        System.arraycopy(src, srcOffset, newCopy, 0, length);

        return newCopy;
    }

    public static byte[] commaSeparatedValuesToBytes(String commaSeparatedValues)
    {
        if (StringUtils.IsNullOrEmpty(commaSeparatedValues))
            return new byte[0];

        String[] values = commaSeparatedValues.split(",");
        byte[] result = new byte[values.length];
        for (int i = 0; i < values.length; i++)
        {
            String value = values[i].trim();
            result[i] = (byte)Integer.parseInt(value);
        }

        return result;
    }

    private static final char[] HEX_ARRAY = "0123456789ABCDEF".toCharArray();

    public static byte[] hexToBytes(String hex)
    {
        int l = hex.length();
        byte[] result = new byte[l / 2];

        int i = hex.startsWith("0x") ? 2 : 0;
        while (i < l)
        {
            char c = hex.charAt(i);
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

    public static String bytesToHex(byte[] bytes)
    {
        if (bytes == null)
            return "";

        char[] hexChars = new char[bytes.length * 2];
        for (int i = 0; i < bytes.length; i++)
        {
            int v = bytes[i] & 0xFF;
            hexChars[i * 2] = HEX_ARRAY[v >>> 4];
            hexChars[i * 2 + 1] = HEX_ARRAY[v & 0x0F];
        }

        return new String(hexChars);
    }
}
