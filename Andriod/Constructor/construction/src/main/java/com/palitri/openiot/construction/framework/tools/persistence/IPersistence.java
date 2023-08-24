package com.palitri.openiot.construction.framework.tools.persistence;

import java.io.Serializable;

public interface IPersistence {
    void SetString(String key, String value);
    String GetString(String key);
    void SetObject(String key, Serializable value);
    Object GetObject(String key);
    void Delete(String key);
}
