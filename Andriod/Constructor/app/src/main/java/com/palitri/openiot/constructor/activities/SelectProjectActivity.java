package com.palitri.openiot.constructor.activities;

import android.os.Bundle;
import android.view.View;
import android.widget.ArrayAdapter;
import android.widget.ListView;
import android.widget.TextView;

import com.palitri.openiot.construction.framework.web.api.OpenIotService;
import com.palitri.openiot.construction.framework.web.models.PresetsCollection;
import com.palitri.openiot.constructor.R;
import com.palitri.openiot.constructor.arrayadapters.BasicItemArrayAdapter;
import com.palitri.openiot.construction.framework.web.models.Project;

import java.util.ArrayList;
import java.util.Arrays;

public class SelectProjectActivity extends ActivityBase {

    private View viewWaitIcon;
    private ListView listProjects;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_select_project);

        //getActionBar().setDisplayHomeAsUpEnabled(true);

        getSupportActionBar().setDisplayShowHomeEnabled(true);
        getSupportActionBar().setIcon(R.drawable.ic_baseline_lightbulb_24);
        getSupportActionBar().setDisplayShowTitleEnabled(true);
        getSupportActionBar().setTitle("Projects");

        this.viewWaitIcon = findViewById(R.id.view_wait_icon);
        this.listProjects = findViewById(R.id.list_projects);

        this.setWaitMode(true);

        new OpenIotService() {
            @Override
            public void onUserProjectsResponse(final Project[] projects, Object... params) {
                super.onUserProjectsResponse(projects, params);

                final SelectProjectActivity activity = SelectProjectActivity.this;

                activity.runOnUiThread(new Runnable() {
                    public void run() {
                        activity.setWaitMode(false);

                        ArrayAdapter adapter = new BasicItemArrayAdapter<Project>(activity, new ArrayList<Project>(Arrays.asList(projects))) {
                            @Override
                            public void SetView(TextView view, Project project) {
                                view.setText(project.name);
                            }

                            @Override
                            public void OnItemClick(Project project) {
                                activity.setWaitMode(true);

                                new OpenIotService()
                                {
                                    @Override
                                    public void onUserProjectResponse(final Project project, final PresetsCollection presets, Object... params) {
                                        super.onUserProjectResponse(project, presets, params);

                                        activity.runOnUiThread(new Runnable() {
                                            public void run() {
                                                activity.getBoard().LoadProject(project, presets);
                                                activity.FinishActivity(null);
                                            }
                                        });
                                    }
                                }
                                        .setToken(activity.getBoard().persistence.getToken())
                                        .requestUserProject(project.projectId);
                            }
                        };
                        listProjects.setAdapter(adapter);
                        adapter.notifyDataSetChanged();
                    }
                });
            }
        }
            .setToken(this.getBoard().persistence.getToken())
            .requestUserProjects();
    }

    private void setWaitMode(boolean isWaiting)
    {
        this.viewWaitIcon.setVisibility(isWaiting ? View.VISIBLE :  View.GONE);
        this.listProjects.setVisibility(isWaiting ? View.GONE :  View.VISIBLE);
    }

}