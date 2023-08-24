package com.palitri.openiot.construction.framework.web.models;

import com.palitri.openiot.construction.framework.web.models.configurations.presets.PresetConfiguration;

import org.json.JSONException;
import org.json.JSONObject;

import java.io.Serializable;

public class Preset implements Serializable {
    public String name;
    public String projectId;
    public String projectPresetId;
    public PresetConfiguration config;

    public Preset()
    {
        this.config = new PresetConfiguration();
    }

    public boolean LoadFromJSON(JSONObject jsonObject) {

        try {
            this.name = jsonObject.has("name") ? jsonObject.getString("name") : null;
            this.projectId = jsonObject.has("projectId") ? jsonObject.getString("projectId") : null;
            this.projectPresetId = jsonObject.has("projectPresetId") ? jsonObject.getString("projectPresetId") : null;

            this.config = new PresetConfiguration();
            if (jsonObject.has("config"))
                this.config.LoadFromJSON(jsonObject.getJSONObject("config"));
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
            result.put("name", this.name);
            result.put("projectId", this.projectId);
            result.put("projectPresetId", this.projectPresetId);
            result.put("config", this.config.SaveToJSON());
        } catch (JSONException e) {
            e.printStackTrace();
        }

        return result;
    }
}
