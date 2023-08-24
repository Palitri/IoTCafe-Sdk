package com.palitri.openiot.construction.framework.web.models.configurations.project;

import com.palitri.openiot.construction.framework.tools.utils.StringUtils;

public enum PeripheralPropertyType {
    None (0),
    Action (1),
    Integer (2),
    Float (3),
    Bool (4),
    Data (5);

    public final int value;

    public static final int[] sizes = { 0, 0, 4, 4, 1, 0 };

    private PeripheralPropertyType(int value)
    {
        this.value  = value;
    }

    public static PeripheralPropertyType FromValue(int value)
    {
        return PeripheralPropertyType.values()[value];
    }

    public int GetStaticSize()
    {
        return sizes[this.value];
    }

    public int GetValue () { return this.value; }

    public static Object Parse(String value, PeripheralPropertyType type)
    {
        switch (type)
        {
            case Bool:
                return Boolean.parseBoolean(value);
            case Integer:
                return java.lang.Integer.parseInt(value);
            case Float:
                return (float)Double.parseDouble(value);

            case Data:
                return StringUtils.HexToBytes(value);
        }

        return (int)0;
    }

    public static String ToString(Object value, PeripheralPropertyType type){
        return value.toString();
    }

}