package com.palitri.openiot.constructor.arrayadapters;

import android.content.Context;
import android.view.View;
import android.widget.ImageButton;
import android.widget.TextView;
import android.widget.Toast;

import com.palitri.openiot.construction.framework.web.api.OpenIotService;
import com.palitri.openiot.construction.framework.web.models.Preset;
import com.palitri.openiot.constructor.R;
import com.palitri.openiot.constructor.activities.ActivityBase;
import com.palitri.openiot.constructor.dialogs.StringInputDialog;

import java.util.ArrayList;

public class PresetsArrayAdapter extends ArrayAdapterBase<PresetsArrayAdapter.ViewContainer, Preset> {
    static class ViewContainer {
        TextView tvName;
        ImageButton btnOverwrite;
        ImageButton btnRename;
        ImageButton btnDelete;
    }

    public PresetsArrayAdapter(Context context, ArrayList<Preset> objects) {
        super(context, R.layout.array_adapter_preset, objects);
    }

    @Override
    public ViewContainer GetView(final View convertView, Preset preset) {
        return new ViewContainer()
        {{
            tvName =  convertView.findViewById(R.id.tvName);
            btnOverwrite =  convertView.findViewById(R.id.btnOverwrite);
            btnRename =  convertView.findViewById(R.id.btnRename);
            btnDelete =  convertView.findViewById(R.id.btnDelete);
        }};
    }

    @Override
    public void SetView(ViewContainer view, final Preset preset) {
        view.tvName.setText(preset.name);

        view.btnOverwrite.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                final ActivityBase activity = ((ActivityBase)PresetsArrayAdapter.this.getContext());

                preset.config = activity.board.SnapshotProjectPreset(preset.name).config;

                new OpenIotService(){
                    @Override
                    public void onUpdateProjectPresetResponse(final Preset preset, Object... params) {
                        super.onUpdateProjectPresetResponse(preset, params);

                        activity.runOnUiThread(new Runnable() {
                            @Override
                            public void run() {
                                PresetsArrayAdapter.this.notifyDataSetChanged();

                                activity.board.persistence.SetPresets(activity.board.presets);

                                Toast.makeText(activity, String.format(activity.getResources().getText(R.string.event_preset_updated).toString(), preset.name), Toast.LENGTH_SHORT).show();
                            }
                        });
                    }
                }
                        .setToken(activity.board.persistence.getToken())
                        .requestUpdateProjectPreset(preset);
            }
        });

        view.btnRename.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                final ActivityBase activity = ((ActivityBase)PresetsArrayAdapter.this.getContext());
                new StringInputDialog(getContext(), preset.name, activity.getResources().getText(R.string.preset_rename_title).toString())
                {
                    @Override
                    public void setValue(String value) {
                        super.setValue(value);

                        preset.name = value;

                        new OpenIotService()
                        {
                            @Override
                            public void onUpdateProjectPresetResponse(final Preset preset, Object... params) {
                                super.onUpdateProjectPresetResponse(preset, params);

                                activity.runOnUiThread(new Runnable() {
                                    @Override
                                    public void run() {
                                        activity.board.persistence.SetPresets(activity.board.presets);

                                        PresetsArrayAdapter.this.notifyDataSetChanged();

                                        Toast.makeText(activity, String.format(activity.getResources().getText(R.string.event_preset_renamed).toString(), preset.name), Toast.LENGTH_SHORT).show();
                                    }
                                });
                            }
                        }
                                .setToken(activity.board.persistence.getToken())
                                .requestUpdateProjectPreset(preset);
                    }
                };
            }
        });


        view.btnDelete.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                final ActivityBase activity = ((ActivityBase)PresetsArrayAdapter.this.getContext());
                new OpenIotService(){
                    @Override
                    public void onDeleteProjectPresetResponse(final String presetId, Object... params) {
                        super.onDeleteProjectPresetResponse(presetId, params);

                        activity.runOnUiThread(new Runnable() {
                            @Override
                            public void run() {
                                Preset removedPreset = activity.board.presets.GetPresetById(presetId);

                                activity.board.presets.remove(removedPreset);
                                activity.board.persistence.SetPresets(activity.board.presets);

                                PresetsArrayAdapter.this.notifyDataSetChanged();

                                Toast.makeText(activity, String.format(activity.getResources().getText(R.string.event_preset_deleted).toString(), removedPreset.name), Toast.LENGTH_SHORT).show();

                            }
                        });
                    }
                }
                        .setToken(activity.board.persistence.getToken())
                        .requestDeleteProjectPreset(preset.projectId, preset.projectPresetId);
            }
        });
    }

    @Override
    public void OnItemClick(Preset preset) {

    }
}
