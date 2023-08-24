package com.palitri.openiot.construction.framework.web.models.configurations.project;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import java.io.Serializable;
import java.util.ArrayList;

public class ProjectConfiguration implements Serializable {
    public ArrayList<Peripheral> peripherals;
    public ArrayList<PeripheralProperty> properties;

    public ProjectConfiguration()
    {
        this.Clear();
    }

    public void Clear()
    {
        this.peripherals = new ArrayList<Peripheral>();
        this.properties = new ArrayList<PeripheralProperty>();
    }

    public Peripheral GetPropertyPeripheral(int semantic)
    {
        for (Peripheral peripheral : this.peripherals)
            for (PeripheralProperty property : peripheral.properties)
                if (property.semantic == semantic)
                    return peripheral;

        return null;
    }

    public boolean LoadFromJSONString(String jsonString)
    {
        try {
            return this.LoadFromJSON(jsonString == null ? null : new JSONObject(jsonString));
        } catch (JSONException e) {
            e.printStackTrace();
            return false;
        }
    }

    public boolean LoadFromJSON(JSONObject jsonObject)
    {
        this.peripherals.clear();
        this.properties.clear();

        if (jsonObject == null)
            return false;

        try {
            if (jsonObject.has("peripherals"))
            {
                JSONArray jsonPeripherals = jsonObject.getJSONArray("peripherals");

                for (int propIndex = 0; propIndex < jsonPeripherals.length(); propIndex++)
                {
                    JSONObject jsonPeripheral = jsonPeripherals.getJSONObject(propIndex);

                    Peripheral per = new Peripheral();
                    per.LoadFromJSON(jsonPeripheral);

                    this.peripherals.add(per);

                    for (PeripheralProperty prop : per.properties)
                        this.properties.add(prop);
                }
            }

        } catch (JSONException e) {
            e.printStackTrace();
            return false;
        }

        return true;
    }

}
