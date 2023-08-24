package com.palitri.openiot.construction.framework.web.models;

import org.json.JSONException;
import org.json.JSONObject;

import java.io.Serializable;

public class Device  implements Serializable {
    public String token;
    public DeviceOS os;
    public String friendlyName;
    public String fingerprint;
    public String info;

    public boolean LoadFromJSON(JSONObject jsonObject) {

        try {
            this.token = jsonObject.has("token") ? jsonObject.getString("token") : null;
            this.os = jsonObject.has("os") ? DeviceOS.FromValue(jsonObject.getInt("os")) : DeviceOS.None;
            this.friendlyName = jsonObject.has("friendlyName") ? jsonObject.getString("friendlyName") : null;
            this.fingerprint = jsonObject.has("fingerprint") ? jsonObject.getString("fingerprint") : null;
            this.info = jsonObject.has("info") ? jsonObject.getString("info") : null;
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
            result.put("token", this.token);
            result.put("os", this.os.GetValue());
            result.put("friendlyName", this.friendlyName);
            result.put("fingerprint", this.fingerprint);
            result.put("info", this.info);
        } catch (JSONException e) {
            e.printStackTrace();
        }

        return result;
    }
}
