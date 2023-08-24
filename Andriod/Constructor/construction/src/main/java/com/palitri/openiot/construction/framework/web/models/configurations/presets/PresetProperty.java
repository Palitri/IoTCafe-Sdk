package com.palitri.openiot.construction.framework.web.models.configurations.presets;

import com.palitri.openiot.construction.framework.web.models.configurations.project.PeripheralPropertyType;

import org.json.JSONException;
import org.json.JSONObject;

import java.io.Serializable;

public class PresetProperty implements Serializable {
    public String scriptId;
    public PeripheralPropertyType type;
    public Object value;

    public boolean LoadFromJSON(JSONObject jsonObject)
    {
        try {
            this.scriptId = jsonObject.has("scriptId") ? jsonObject.getString("scriptId") : null;
            this.type = jsonObject.has("type") ? PeripheralPropertyType.FromValue(jsonObject.getInt("type")) : PeripheralPropertyType.None;
            this.value = jsonObject.has("value") ? PeripheralPropertyType.Parse(jsonObject.getString("value"), this.type) : null;

        } catch (JSONException e) {
            e.printStackTrace();
            return false;
        }

        return true;
    }

    public JSONObject SaveToJSON()
    {
        JSONObject result = new JSONObject();

        try {
            result.put("scriptId", this.scriptId);
            result.put("type", this.type.GetValue());
            result.put("value", this.value.toString());
        } catch (JSONException e) {
            e.printStackTrace();
        }

        return result;
    }

}
