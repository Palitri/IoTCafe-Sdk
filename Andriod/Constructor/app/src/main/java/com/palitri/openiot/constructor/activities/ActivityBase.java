package com.palitri.openiot.constructor.activities;

import android.content.Intent;
import android.os.Build;
import android.os.Bundle;

import androidx.annotation.NonNull;
import androidx.appcompat.app.AppCompatActivity;

import com.google.android.gms.tasks.OnCompleteListener;
import com.google.android.gms.tasks.Task;
import com.google.firebase.messaging.FirebaseMessaging;
import com.palitri.openiot.construction.framework.tools.utils.SerializationUtils;
import com.palitri.openiot.construction.framework.composite.CompositeBoard;
import com.palitri.openiot.construction.framework.web.api.OpenIotService;
import com.palitri.openiot.construction.framework.web.models.Device;
import com.palitri.openiot.construction.framework.web.models.DeviceOS;
import com.palitri.openiot.constructor.ConstructorApplication;

import java.io.Serializable;

public class ActivityBase extends AppCompatActivity  {

    public CompositeBoard board;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);

        this.board = this.getBoard();
    }

    public ConstructorApplication getConstructorApplication()
    {
        return (ConstructorApplication)this.getApplication();
    }

    public CompositeBoard getBoard()
    {
        return this.getConstructorApplication().getBoard();
    }

    public void StartActivity(Class<?> cls, int requestCode, Serializable activityData)
    {
        Intent intent = new Intent(this, cls);
        if (activityData != null)
            intent.putExtra("activityData", SerializationUtils.SerializeToString(activityData));
        this.startActivityForResult(intent, requestCode);
    }

    public void FinishActivity(Serializable activityData)
    {
        if (activityData != null) {
            Intent intent = new Intent();
            intent.putExtra("activityData", SerializationUtils.SerializeToString(activityData));
            this.setResult(RESULT_OK, intent);
        }
        else
            this.setResult(RESULT_OK);

        this.finish();
    }

    public Object GetActivityData()
    {
        Bundle bundle = getIntent().getExtras();
        return SerializationUtils.Deserialize(bundle.getString("activityData"));
    }

    public void UpdateDeviceInfo()
    {
        final String deviceInfo =
                "\"Device.Manufacturer\": \"" + android.os.Build.MANUFACTURER +
                        "\", \"Device.Brand\": \"" + android.os.Build.BRAND +
                        "\", \"Device.Model\": \"" + android.os.Build.MODEL +
                        "\", \"OS.Release\": \"" + android.os.Build.VERSION.RELEASE +
                        "\", \"OS.SDK\": \"" + String.valueOf(android.os.Build.VERSION.SDK_INT);

        final String deviceFingerprint = Build.FINGERPRINT;

        final String deviceFriendlyName = this.board.bluetoothManager.getName();

        final String authenticationToken = this.board.persistence.getToken();

        FirebaseMessaging.getInstance().getToken()
                .addOnCompleteListener(new OnCompleteListener<String>() {
                    @Override
                    public void onComplete(@NonNull Task<String> task) {
                        if (!task.isSuccessful()) {
                            //Log.w(TAG, "Fetching FCM registration token failed", task.getException());
                            return;
                        }

                        String deviceToken = task.getResult();

                        new OpenIotService(){
                            @Override
                            public void onRequestUpdateDeviceResponse(Device device, Object... params) {
                                super.onRequestUpdateDeviceResponse(device, params);

                                ActivityBase.this.onUpdatedDeviceInfo(device);
                            }
                        }
                            .setToken(authenticationToken)
                            .requestUpdateDevice(new Device() {{
                                token = deviceToken;
                                os = DeviceOS.Android;
                                friendlyName = deviceFriendlyName;
                                fingerprint = deviceFingerprint;
                                info = deviceInfo;
                            }});
                    }

                });
    }

    public void onUpdatedDeviceInfo(Device device)
    {

    }

}
