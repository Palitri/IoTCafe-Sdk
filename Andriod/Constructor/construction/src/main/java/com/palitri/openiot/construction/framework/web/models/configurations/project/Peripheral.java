package com.palitri.openiot.construction.framework.web.models.configurations.project;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import java.io.Serializable;
import java.util.ArrayList;

public class Peripheral implements Serializable {
    public String typeId;
    public String scriptId;
    public String name;

    public ArrayList<PeripheralProperty> properties;
    public ArrayList<PeripheralSetting> settings;
    public ArrayList<PeripheralPin> pins;

    public Peripheral()
    {
        this.properties = new ArrayList<PeripheralProperty>();
        this.settings = new ArrayList<PeripheralSetting>();
        this.pins = new ArrayList<PeripheralPin>();
    }

    public boolean LoadFromJSON(JSONObject jsonObject)
    {
        try {
            if (jsonObject.has("peripheralTypeId"))
                this.typeId = jsonObject.getString("peripheralTypeId");

            if (jsonObject.has("id"))
                this.scriptId = jsonObject.getString("id");

            if (jsonObject.has("name"))
                this.name = jsonObject.getString("name");

            if (jsonObject.has("properties")) {
                JSONArray jsonPeripheralProperties = jsonObject.getJSONArray("properties");

                for (int propIndex = 0; propIndex < jsonPeripheralProperties.length(); propIndex++) {
                    JSONObject jsonPeripheralProperty = jsonPeripheralProperties.getJSONObject(propIndex);

                    PeripheralProperty p = new PeripheralProperty();
                    p.LoadFromJSON(jsonPeripheralProperty);

                    this.properties.add(p);
                }
            }

        } catch (JSONException e) {
            e.printStackTrace();
            return false;
        }

        return true;
    }

}
