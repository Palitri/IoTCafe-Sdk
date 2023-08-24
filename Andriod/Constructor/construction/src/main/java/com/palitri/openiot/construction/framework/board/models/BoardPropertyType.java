package com.palitri.openiot.construction.framework.board.models;

//public class BoardPropertyType {
//    public static final int None = 0;
//    public static final int Action = 1;
//    public static final int Integer = 2;
//    public static final int Float = 3;
//    public static final int Bool = 4;
//
//    public static final int[] sizes = { 0, 0, 4, 4, 1 };
//}

public enum BoardPropertyType {
    None (0),
    Action (1),
    Integer (2),
    Float (3),
    Bool (4),
    Data (5);

    public final int value;

    public static final int[] sizes = { 0, 0, 4, 4, 1, 0 };

    private BoardPropertyType(int value)
    {
        this.value  = value;
    }

    public static BoardPropertyType FromValue(int value)
    {
        return BoardPropertyType.values()[value];
    }

    public int GetStaticSize()
    {
        return sizes[this.value];
    }

    public int GetValue () { return this.value; }
}
