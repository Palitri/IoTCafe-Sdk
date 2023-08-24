package com.palitri.openiot.construction.framework.tools.utils;

import java.util.UUID;
import java.util.regex.Pattern;

public class GuidUtils {
    static public String Zero()
    {
        return "00000000-0000-0000-0000-000000000000";
    }

    static public boolean IsZero(String guid)
    {
        return GuidUtils.Zero().equals(GuidUtils.Normalize(guid));
    }

    static public boolean IsValid(String guid)
    {
        return Pattern.compile("^[0-9A-Fa-f]+$").matcher(guid).matches();
    }

    static public String FromBytes(byte[] bytes)
    {
        return GuidUtils.Normalize(StringUtils.BytesToHex(bytes));
    }

    static public byte[] ToBytes(String guid)
    {
        return StringUtils.HexToBytes(GuidUtils.Shorten(guid));
    }

    static public String Shorten(String guid)
    {
        return Pattern.compile("[^0-9A-Fa-f]").matcher(guid).replaceAll("");
    }

    static public String Normalize(String guid)
    {
        StringBuffer buffer = new StringBuffer(GuidUtils.Shorten(guid));

        buffer.insert(8, "-");
        buffer.insert(13, "-");
        buffer.insert(18, "-");
        buffer.insert(23, "-");

        return buffer.toString();
    }

    static public UUID ToGuid(String guid)
    {
        return UUID.fromString(GuidUtils.Normalize(guid));
    }

    static public String ToString(UUID guid)
    {
        return GuidUtils.Normalize(guid.toString());
    }
}
