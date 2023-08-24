package com.palitri.openiot.construction.framework.tools.utils;

import java.io.ByteArrayInputStream;
import java.io.ByteArrayOutputStream;
import java.io.IOException;
import java.io.ObjectInputStream;
import java.io.ObjectOutputStream;
import java.io.Serializable;

public class SerializationUtils {
    public static byte[] SerializeToBytes(Serializable obj)
    {
        try {
            ByteArrayOutputStream outputStream = new ByteArrayOutputStream();
            ObjectOutputStream serializationStream = new ObjectOutputStream(outputStream);
            serializationStream.writeObject(obj);

            return outputStream.toByteArray();
        } catch (IOException e) {
            e.printStackTrace();
        }

        return null;
    }

    public static String SerializeToString(Serializable obj)
    {
        return ArrayUtils.bytesToHex(SerializeToBytes(obj));
    }

    public static Object Deserialize(byte[] bytes)
    {
        try {
            ByteArrayInputStream inputStream = new ByteArrayInputStream(bytes);
            ObjectInputStream serializationStream = new ObjectInputStream(inputStream);

            return serializationStream.readObject();
        } catch (ClassNotFoundException e) {
            e.printStackTrace();
        } catch (IOException e) {
            e.printStackTrace();
        }

        return null;
    }

    public static Object Deserialize(String str)
    {
        return Deserialize(ArrayUtils.hexToBytes(str));
    }
}
