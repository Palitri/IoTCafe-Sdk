package com.palitri.iotcafe;

import androidx.annotation.NonNull;
import androidx.lifecycle.DefaultLifecycleObserver;
import androidx.lifecycle.LifecycleOwner;

public class ApplicationLifecycleObserver implements DefaultLifecycleObserver
{
    private IoTCafeApplication app;

    public ApplicationLifecycleObserver(IoTCafeApplication app)
    {
        this.app = app;
    }

    @Override
    public void onStart(@NonNull LifecycleOwner owner) {
//        this.app.getBoard().boardDevice.Open();
//        this.app.getBoard().requestProjectUploadSequence();
    }

    @Override
    public void onStop(@NonNull LifecycleOwner owner) {
        this.app.getBoard().DisconnectFromBoard();
    }
}
