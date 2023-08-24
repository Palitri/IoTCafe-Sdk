package com.palitri.openiot.construction.framework.web.models.configurations.presets;

import com.palitri.openiot.construction.framework.web.models.JsonSerializableModel;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import java.io.Serializable;
import java.util.ArrayList;

public class PresetConfiguration extends JsonSerializableModel implements Serializable {
    public ArrayList<PresetProperty> properties;

    public PresetConfiguration()
    {
        this.properties = new ArrayList<PresetProperty>();
    }

    public boolean LoadFromJSON(JSONObject jsonObject)
    {
        this.properties.clear();

        if (jsonObject == null)
            return false;

        try {
            if (jsonObject.has("properties"))
            {
                JSONArray jsonProperties = jsonObject.getJSONArray("properties");

                for (int propIndex = 0; propIndex < jsonProperties.length(); propIndex++)
                {
                    JSONObject jsonProperty = jsonProperties.getJSONObject(propIndex);

                    PresetProperty presetProperty = new PresetProperty();
                    presetProperty.LoadFromJSON(jsonProperty);

                    this.properties.add(presetProperty);
                }
            }

        } catch (JSONException e) {
            e.printStackTrace();
            return false;
        }

        return true;
    }

    public JSONObject SaveToJSON()
    {

        JSONArray jsonProperties = new JSONArray();
        for (PresetProperty property : this.properties)
            jsonProperties.put(property.SaveToJSON());

        JSONObject jsonObject = new JSONObject();
        try {
            jsonObject.put("properties", jsonProperties);
        } catch (JSONException e) {
            e.printStackTrace();
        }

        return jsonObject;
    }
}
