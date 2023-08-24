package com.palitri.openiot.construction.framework.web.models;

public enum DeviceOS {
    None (0),
    Android (1),
    iOS (2);

    public final int value;

    private DeviceOS(int value)
    {
        this.value  = value;
    }

    public static DeviceOS FromValue(int value)
    {
        return DeviceOS.values()[value];
    }

    public int GetValue () { return this.value; }
}
