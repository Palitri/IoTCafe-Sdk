package com.palitri.openiot.construction.framework.composite;

import com.palitri.openiot.construction.framework.tools.persistence.IPersistence;
import com.palitri.openiot.construction.framework.tools.utils.StringUtils;
import com.palitri.openiot.construction.framework.web.models.PresetsCollection;
import com.palitri.openiot.construction.framework.web.models.Project;

public class CompositeBoardPersistence {

    public IPersistence persistence;

    public CompositeBoardPersistence(IPersistence persistence)
    {
        this.persistence = persistence;
    }


    public boolean IsUserLogged()
    {
        return !StringUtils.IsNullOrEmpty(this.getToken());
    }

    public boolean IsDeviceSelected()
    {
        return !StringUtils.IsNullOrEmpty(this.GetDeviceName());
    }

    public boolean IsProjectSelected()
    {
        return this.GetProject() != null;
    }



    public void setToken(String token)
    {
        this.persistence.SetString("token", token);
    }

    public String getToken()
    {
        return this.persistence.GetString("token");
    }



    public void SetProject(Project value)
    {
        this.persistence.SetObject("project", value);
    }

    public Project GetProject()
    {
        Project result = (Project)this.persistence.GetObject("project");

        return result;
    }

    public void DeleteProject()
    {
        this.persistence.Delete("project");
    }



    public void SetPresets(PresetsCollection value)
    {
        this.persistence.SetObject("presets", value);
    }

    public PresetsCollection GetPresets()
    {
        try {
            PresetsCollection result = (PresetsCollection) this.persistence.GetObject("presets");

            return result;
        }
        catch (Exception e)
        {
            return null;
        }
    }

    public void DeletePresets()
    {
        this.persistence.Delete("presets");
    }



    public void SetDeviceName(String deviceName)
    {
        this.persistence.SetString("deviceName", deviceName);
    }

    public String GetDeviceName()
    {
        return this.persistence.GetString("deviceName");
    }

}
