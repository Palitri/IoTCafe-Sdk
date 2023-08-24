package com.palitri.openiot.constructor;

import androidx.annotation.NonNull;
import androidx.lifecycle.DefaultLifecycleObserver;
import androidx.lifecycle.LifecycleOwner;

public class ApplicationLifecycleObserver implements DefaultLifecycleObserver
{
    private ConstructorApplication app;

    public ApplicationLifecycleObserver(ConstructorApplication app)
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
        this.app.getBoard().boardDevice.Close();
    }
}
