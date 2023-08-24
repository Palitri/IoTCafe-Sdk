package com.palitri.openiot.construction.framework.web.api;

import com.palitri.openiot.construction.framework.web.models.Device;
import com.palitri.openiot.construction.framework.web.models.PresetsCollection;
import com.palitri.openiot.construction.framework.web.models.Project;
import com.palitri.openiot.construction.framework.web.models.Preset;
import com.palitri.openiot.construction.framework.web.networking.WebRequest;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import java.util.ArrayList;

public class OpenIotService {

    private String token;

    private String baseUrl;

    public OpenIotService()
    {
        this.baseUrl = "http://api.iot.cafe";
    }

    public OpenIotService setToken(String token)
    {
        this.token = token;

        return this;
    }

    public void requestUserLogin(String email, String password, Object... params)
    {
        new WebRequest(){
            @Override
            public void onResponse(String response, Object... params) {
                super.onResponse(response, params);

                OpenIotService service = (OpenIotService)params[1];
                service.userLoginResponse(response, params);
            }
        }
                .setRequestBody("{\"email\":\"" + email + "\",\"password\":\"" + password + "\"}")
                .setHttpMethod("POST")
                .send(this.baseUrl + "/users", this);
    }

    private void userLoginResponse(String response, Object... params)
    {
        try{
            if (response == null)
                this.token = null;
            else {
                JSONObject jsonResponse = new JSONObject(response);

                this.token = jsonResponse.getString("token");
            }

            this.onUserLoginResponse(this.token, params);

        } catch (JSONException e) {
            e.printStackTrace();
        }


    }

    public void onUserLoginResponse(String token, Object... params)
    {

    }



    public void requestUserProjects()
    {
        new WebRequest(){
            @Override
            public void onResponse(String response, Object... params) {
                super.onResponse(response, params);

                OpenIotService service = (OpenIotService)params[1];
                service.userProjectsResponse(response, params);
            }
        }
                .setToken(this.token)
                .setHttpMethod("GET")
                .send(this.baseUrl + "/projects", this);
    }

    private void userProjectsResponse(String response, Object... params)
    {
        ArrayList<Project> result = new ArrayList<Project>();
        try {
            JSONObject jsonResponse = new JSONObject(response);

            JSONArray jsonArray = jsonResponse.getJSONArray("userProjects");
            for (int i = 0; i < jsonArray.length(); i++) {
                JSONObject jsonProject = jsonArray.getJSONObject(i);

                Project p = new Project();
                p.LoadFromJSON(jsonProject);
                result.add(p);
            }

            this.onUserProjectsResponse((Project[]) result.toArray(new Project[result.size()]), params);

        } catch (JSONException e) {
            e.printStackTrace();
        }
    }

    public void onUserProjectsResponse(Project[] projects, Object... params)
    {

    }



    public void requestUserProject(String projectId, Object... params)
    {
        new WebRequest(){
            @Override
            public void onResponse(String response, Object... params) {
                super.onResponse(response, params);

                OpenIotService service = (OpenIotService)params[1];
                service.userProjectResponse(response, params);
            }
        }
                .setToken(this.token)
                .setHttpMethod("GET")
                .send(this.baseUrl + "/project/" + projectId, this);
    }

    private void userProjectResponse(String response, Object... params)
    {
        try{
        JSONObject jsonResponse = new JSONObject(response);

        JSONObject projectJson = jsonResponse.getJSONObject("project");

        PresetsCollection resultPresets = new PresetsCollection();
        JSONArray presetsJson = jsonResponse.getJSONArray("projectPresets");
        for (int i = 0; i < presetsJson.length(); i++)
        {
            Preset preset = new Preset();
            preset.LoadFromJSON(presetsJson.getJSONObject(i));
            resultPresets.add(preset);
        }

        Project resultProject = new Project();
        if (resultProject.LoadFromJSON(projectJson))
            this.onUserProjectResponse(resultProject, resultPresets, params);

        } catch (JSONException e) {
            e.printStackTrace();
        }
    }

    public void onUserProjectResponse(Project project, PresetsCollection presets, Object... params)
    {

    }



    public void requestProjectPresets(String projectId)
    {
        new WebRequest(){
            @Override
            public void onResponse(String response, Object... params) {
                super.onResponse(response, params);

                OpenIotService service = (OpenIotService)params[1];
                service.projectPresetsResponse(response, params);
            }
        }
            .setToken(this.token)
            .setHttpMethod("GET")
            .send(this.baseUrl + "/project/" + projectId + "/presets", this);
    }

    private void projectPresetsResponse(String response, Object... params)
    {
        PresetsCollection result = new PresetsCollection();
        try {
            JSONObject jsonResponse = new JSONObject(response);

            JSONArray jsonArray = jsonResponse.getJSONArray("projectPresets");
            for (int i = 0; i < jsonArray.length(); i++) {
                JSONObject jsonPreset = jsonArray.getJSONObject(i);

                Preset p = new Preset();
                p.LoadFromJSON(jsonPreset);
                result.add(p);
            }

            this.onProjectPresetsResponse(result, params);

        } catch (JSONException e) {
            e.printStackTrace();
        }
    }

    public void onProjectPresetsResponse(PresetsCollection presets, Object... params)
    {

    }



    public void requestSaveProjectPreset(String projectId, Preset preset)
    {
        new WebRequest(){
            @Override
            public void onResponse(String response, Object... params) {
                super.onResponse(response, params);

                OpenIotService service = (OpenIotService)params[1];
                service.saveProjectPresetResponse(response, params);
            }
        }
                .setToken(this.token)
                .setRequestBody(preset.SaveToJSON().toString())
                .setHttpMethod("PUT")
                .send(this.baseUrl + "/project/" + projectId + "/presets", this);
    }

    private void saveProjectPresetResponse(String response, Object... params)
    {
        try{
            JSONObject jsonResponse = new JSONObject(response);

            Preset result = new Preset();
            if (result.LoadFromJSON(jsonResponse.getJSONObject("projectPreset")))
                this.onSaveProjectPresetResponse(result, params);

        } catch (JSONException e) {
            e.printStackTrace();
        }
    }

    public void onSaveProjectPresetResponse(Preset preset, Object... params)
    {

    }



    public void requestUpdateProjectPreset(Preset preset)
    {
        new WebRequest(){
            @Override
            public void onResponse(String response, Object... params) {
                super.onResponse(response, params);

                OpenIotService service = (OpenIotService)params[1];
                service.updateProjectPresetResponse(response, params);
            }
        }
                .setToken(this.token)
                .setRequestBody(preset.SaveToJSON().toString())
                .setHttpMethod("POST")
                .send(this.baseUrl + "/project/" + preset.projectId + "/preset/" + preset.projectPresetId, this);
    }

    private void updateProjectPresetResponse(String response, Object... params)
    {
        try{
            JSONObject jsonResponse = new JSONObject(response);

            Preset result = new Preset();
            if (result.LoadFromJSON(jsonResponse.getJSONObject("projectPreset")))
                this.onUpdateProjectPresetResponse(result, params);

        } catch (JSONException e) {
            e.printStackTrace();
        }
    }

    public void onUpdateProjectPresetResponse(Preset preset, Object... params)
    {

    }



    public void requestDeleteProjectPreset(String projectId, final String presetId)
    {
        new WebRequest(){
            @Override
            public void onResponse(String response, Object... params) {
                super.onResponse(response, params);

                OpenIotService service = (OpenIotService)params[1];
                service.deleteProjectPresetResponse(response, presetId, params);
            }
        }
                .setToken(this.token)
                .setHttpMethod("DELETE")
                .send(this.baseUrl + "/project/" + projectId + "/preset/" + presetId, this);
    }

    private void deleteProjectPresetResponse(String response, String presetId, Object... params)
    {
        try{
            JSONObject jsonResponse = new JSONObject(response);

            this.onDeleteProjectPresetResponse(presetId, params);

        } catch (JSONException e) {
            e.printStackTrace();
        }
    }

    public void onDeleteProjectPresetResponse(String presetId, Object... params)
    {

    }



    public void requestUpdateDevice(final Device device)
    {
        new WebRequest(){
            @Override
            public void onResponse(String response, Object... params) {
                super.onResponse(response, params);

                OpenIotService service = (OpenIotService)params[1];
                service.requestUpdateDeviceResponse(response, device, params);
            }
        }
                .setToken(this.token)
                .setRequestBody(device.SaveToJSON().toString())
                .setHttpMethod("POST")
                .send(this.baseUrl + "/user/devices", this);
    }

    private void requestUpdateDeviceResponse(String response, Device device, Object... params)
    {
        try{
            JSONObject jsonResponse = new JSONObject(response);

            this.onRequestUpdateDeviceResponse(device, params);

        } catch (JSONException e) {
            e.printStackTrace();
        }
    }

    public void onRequestUpdateDeviceResponse(Device device, Object... params)
    {

    }

}
