package com.palitri.openiot.construction.framework.web.models.configurations.project;

import org.json.JSONException;
import org.json.JSONObject;

import java.io.Serializable;

public class PeripheralProperty implements Serializable {
    public String scriptId;
    public PeripheralPropertyType type;
    public String name;
    public Object value;
    public String displayFormat;
    public float min, max, step;
    public boolean instantUpdate;
    public int semantic;
    public boolean visible;

    public boolean hasMin, hasMax, hasValue;

    public PeripheralProperty()
    {
        this.scriptId = "";
        this.type = PeripheralPropertyType.None;
        this.name = "";
        this.value = 0.0f;
        this.displayFormat = "%.2f";
        this.min = 0.0f;
        this.max = 1.0f;
        this.step = 0.01f;
        this.instantUpdate = false;
        this.semantic = 0;
        this.visible = false;

        this.hasMin = false;
        this.hasMax = false;
        this.hasValue = false;
    }

    public boolean LoadFromJSON(JSONObject source)
    {
        try {
            if (source.has("semantic"))
                this.semantic = source.getInt("semantic");

            if (source.has("type"))
                this.type = PeripheralPropertyType.FromValue(source.getInt("type"));

            if (source.has("semantic"))
                this.semantic = source.getInt("semantic");

            if (source.has("name"))
                this.name = source.getString("name");

            if (source.has("id"))
                this.scriptId = source.getString("id");

            if (source.has("displayFormat"))
                this.displayFormat = source.getString("displayFormat");

            if (source.has("step"))
                this.step = (float) source.getDouble("step");

            this.hasMin = source.has("min");
            if (this.hasMin)
                this.min = (float) source.getDouble("min");

            this.hasMax = source.has("max");
            if (this.hasMax)
                this.max = (float) source.getDouble("max");

            this.hasValue = source.has("value");
            if (this.hasValue)
                this.value = PeripheralPropertyType.Parse(source.getString("value"), this.type);

            if (source.has("visible"))
                this.visible = source.getBoolean("visible");

            if (source.has("instantUpdate"))
                this.instantUpdate = source.getBoolean("instantUpdate");

        } catch (JSONException e) {
            e.printStackTrace();
            return false;
        }

        return true;
    }

}
