package com.palitri.openiot.construction.framework.web.models;

import java.util.ArrayList;

public class PresetsCollection extends ArrayList<Preset> {
    public Preset GetPresetById(String presetId)
    {
        for (Preset preset : this)
            if (preset.projectPresetId.equals(presetId))
                return preset;

        return null;
    }
}
