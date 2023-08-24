package com.palitri.openiot.construction.framework.tools.persistence;

import android.content.Context;
import android.content.SharedPreferences;

import com.palitri.openiot.construction.framework.tools.utils.SerializationUtils;

import java.io.Serializable;

public class DefaultPersistence implements IPersistence {
    Context context;
    String persistenceName;

    public DefaultPersistence(Context context, String persistenceName)
    {
        this.context = context;
        this.persistenceName = persistenceName;
    }

    public void SetString(String key, String value)
    {
        SharedPreferences settings = this.context.getApplicationContext().getSharedPreferences(this.persistenceName, 0);
        SharedPreferences.Editor editor = settings.edit();
        editor.putString(key, value);

        editor.apply();
    }

    public String GetString(String key)
    {
        SharedPreferences settings = this.context.getApplicationContext().getSharedPreferences(this.persistenceName, 0);
        if (settings.contains(key)) {
            return settings.getString(key, null);
        }

        return null;
    }

    public void SetObject(String key, Serializable value)
    {
        String dataString = SerializationUtils.SerializeToString(value);
        this.SetString(key, dataString);
    }

    public Object GetObject(String key)
    {
        String dataString = this.GetString(key);
        if (dataString != null)
            return SerializationUtils.Deserialize(dataString);

        return null;
    }

    public void Delete(String key)
    {
        SharedPreferences settings = this.context.getApplicationContext().getSharedPreferences(this.persistenceName, 0);
        SharedPreferences.Editor editor = settings.edit();
        editor.remove(key);

        editor.apply();
    }
}
