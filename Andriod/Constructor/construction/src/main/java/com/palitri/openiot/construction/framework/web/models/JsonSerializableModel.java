package com.palitri.openiot.construction.framework.web.models;

import org.json.JSONException;
import org.json.JSONObject;

public abstract class JsonSerializableModel {
    public abstract boolean LoadFromJSON(JSONObject jsonObject);
    public abstract JSONObject SaveToJSON();

    public boolean LoadFromJSONString(String jsonString)
    {
        try {
            return this.LoadFromJSON(jsonString == null ? null : new JSONObject(jsonString));
        } catch (JSONException e) {
            e.printStackTrace();
            return false;
        }
    }

    public String SaveToJSONString()
    {
        JSONObject jsonPresetConfig = this.SaveToJSON();

        return jsonPresetConfig.toString();
    }

}
