package com.palitri.iotcafe.activities;

import android.os.Bundle;

import android.view.View;
import android.widget.Button;
import android.widget.EditText;
import android.widget.ProgressBar;

import com.palitri.openiot.construction.framework.tools.utils.StringUtils;
import com.palitri.openiot.construction.framework.web.api.OpenIotService;
import com.palitri.iotcafe.R;

public class LoginActivity extends ActivityBase {

    @Override
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_login);

        getSupportActionBar().setDisplayShowHomeEnabled(true);
        getSupportActionBar().setIcon(R.drawable.ic_account);
        getSupportActionBar().setDisplayShowTitleEnabled(true);
        getSupportActionBar().setTitle("Login");

        final EditText txtEmail = findViewById(R.id.email);
        final EditText txtPassword = findViewById(R.id.password);
        final Button loginButton = findViewById(R.id.login);
        final ProgressBar loadingProgressBar = findViewById(R.id.loading);

        loginButton.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                final String email = txtEmail.getText().toString();
                final String password = txtPassword.getText().toString();

                new OpenIotService() {
                    @Override
                    public void onUserLoginResponse(final String token, Object... params) {
                        super.onUserLoginResponse(token, params);

                        LoginActivity.this.runOnUiThread(new Runnable() {
                            public void run() {
                                if (StringUtils.IsNullOrEmpty(token)) {
                                    LoginActivity.this.FinishActivity(null, ActivityResult_Error);
                                }
                                else {
                                    LoginActivity.this.LogIn(token);
                                    LoginActivity.this.FinishActivity(null, ActivityResult_OK);
                                }

                            }
                        });
                    }
                }
                    .requestUserLogin(email, password);
            }
        });
    }

    private void LogIn(String authenticationToken)
    {
        this.getBoard().persistence.setToken(authenticationToken);

        this.UpdateDeviceInfo();
    }

}