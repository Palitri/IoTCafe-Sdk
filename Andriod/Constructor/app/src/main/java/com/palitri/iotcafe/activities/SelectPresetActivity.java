package com.palitri.iotcafe.activities;

import android.os.Bundle;
import android.view.Menu;
import android.view.MenuItem;
import android.view.View;
import android.widget.ArrayAdapter;
import android.widget.ListView;
import android.widget.Toast;

import androidx.annotation.NonNull;

import com.palitri.openiot.construction.framework.web.api.OpenIotService;
import com.palitri.openiot.construction.framework.web.models.Preset;
import com.palitri.openiot.construction.framework.web.models.PresetsCollection;
import com.palitri.iotcafe.R;
import com.palitri.iotcafe.arrayadapters.PresetsArrayAdapter;
import com.palitri.iotcafe.dialogs.StringInputDialog;

public class SelectPresetActivity extends ActivityBase {

    ArrayAdapter presetsAdapter;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_select_preset);

        getSupportActionBar().setDisplayShowHomeEnabled(true);
        getSupportActionBar().setIcon(R.drawable.ic_baseline_checklist_rtl_24);
        getSupportActionBar().setDisplayShowTitleEnabled(true);
        getSupportActionBar().setTitle("Presets");

        final View viewWaitIcon = findViewById(R.id.view_empty);
        final ListView listProjects = findViewById(R.id.list_presets);

        new OpenIotService() {
            @Override
            public void onProjectPresetsResponse(final PresetsCollection presets, Object... params) {
                super.onProjectPresetsResponse(presets, params);

                final SelectPresetActivity activity = SelectPresetActivity.this;

                activity.runOnUiThread(new Runnable() {
                    public void run() {
                        viewWaitIcon.setVisibility(View.GONE);

                        board.presets = presets;
                        board.persistence.SetPresets(presets);

                        SelectPresetActivity.this.presetsAdapter = new PresetsArrayAdapter(activity, presets);

                        listProjects.setAdapter(presetsAdapter);
                        presetsAdapter.notifyDataSetChanged();
                    }
                });
            }
        }
                .setToken(this.getBoard().persistence.getToken())
                .requestProjectPresets(String.valueOf(this.getBoard().persistence.GetProject().projectId));
    }

    @Override
    public boolean onCreateOptionsMenu(Menu menu) {
        // Inflate the menu; this adds items to the action bar if it is present.
        getMenuInflater().inflate(R.menu.projects_menu, menu);

        return true;
    }
    @Override
    public boolean onOptionsItemSelected(@NonNull MenuItem item) {
        switch (item.getItemId()) {
            case R.id.menu_item_presets_add_preset:
            {
                new StringInputDialog(this, this.board.GenerateUniquePresetName(null), this.getResources().getText(R.string.preset_new_default_name).toString())
                {
                    @Override
                    public void setValue(String value) {
                        super.setValue(value);

                        Preset preset = SelectPresetActivity.this.board.SnapshotProjectPreset(value);
                        new OpenIotService(){
                            @Override
                            public void onSaveProjectPresetResponse(final Preset preset, Object... params) {
                                super.onSaveProjectPresetResponse(preset, params);

                                SelectPresetActivity.this.runOnUiThread(new Runnable() {
                                    @Override
                                    public void run() {
                                        board.presets.add(preset);
                                        board.persistence.SetPresets(board.presets);

                                        //presetsAdapter.add(preset);
                                        presetsAdapter.notifyDataSetChanged();
                                        Toast.makeText(SelectPresetActivity.this, String.format(SelectPresetActivity.this.getResources().getText(R.string.event_preset_saved).toString(), preset.name), Toast.LENGTH_SHORT).show();
                                    }
                                });
                            }
                        }
                                .setToken(SelectPresetActivity.this.board.persistence.getToken())
                                .requestSaveProjectPreset(SelectPresetActivity.this.board.project.projectId, preset);
                    }
                };

                break;
            }
        }

        return super.onOptionsItemSelected(item);
    }

}