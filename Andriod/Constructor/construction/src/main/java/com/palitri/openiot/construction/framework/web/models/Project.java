package com.palitri.openiot.construction.framework.web.models;

import com.palitri.openiot.construction.framework.tools.utils.ArrayUtils;
import com.palitri.openiot.construction.framework.web.models.configurations.project.ProjectConfiguration;

import org.json.JSONException;
import org.json.JSONObject;

import java.io.Serializable;

public class Project implements Serializable {
    public String projectId;
    public String name;
    public String userId;
    public ProjectConfiguration boardConfig;
    public String schemeCode;
    public String scriptCode;
    public String creationDate;

    public Project()
    {
        this.boardConfig = new ProjectConfiguration();
    }

    public boolean LoadFromJSON(JSONObject jsonObject)
    {
        try {
            this.projectId = jsonObject.has("projectId") ? jsonObject.getString("projectId") : null;
            this.name = jsonObject.has("name") ? jsonObject.getString("name") : null;
            this.userId = jsonObject.has("userId") ? jsonObject.getString("userId") : null;
            this.schemeCode = jsonObject.has("schemeCode") ? jsonObject.getString("schemeCode") : null;
            this.scriptCode = jsonObject.has("scriptCode") ? jsonObject.getString("scriptCode") : null;
            this.creationDate = jsonObject.has("creationDate") ? jsonObject.getString("creationDate") : null;

            this.boardConfig = new ProjectConfiguration();
            if (jsonObject.has("boardConfig"))
                this.boardConfig.LoadFromJSON(jsonObject.getJSONObject("boardConfig"));
        } catch (JSONException e) {
            e.printStackTrace();
            return false;
        }

        return true;
    }

    public byte[] getCompiledSchemeCode()
    {
        return ArrayUtils.commaSeparatedValuesToBytes(this.schemeCode);
    }

    public byte[] getCompiledScriptCode()
    {
        return ArrayUtils.commaSeparatedValuesToBytes(this.scriptCode);
    }
}
