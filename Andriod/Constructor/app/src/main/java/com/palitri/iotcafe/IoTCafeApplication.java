package com.palitri.iotcafe;

import android.app.Application;

import androidx.lifecycle.ProcessLifecycleOwner;

import com.palitri.openiot.construction.framework.composite.CompositeBoard;


public class IoTCafeApplication
        extends Application
        //implements Application.ActivityLifecycleCallbacks
{

//    private int activityReferences = 0;
//    private boolean isActivityChangingConfigurations = false;

    private CompositeBoard board = null;
    public CompositeBoard getBoard()
    {
        if (this.board == null)
        {
            this.board = new CompositeBoard(this);
//            if (!this.board.IsConnected()) {
//                CompositeBoardResult result = this.board.ConnectToBoard();
//                if (result == CompositeBoardResult.DeviceNotSet || result == CompositeBoardResult.DeviceNotFound)
//                    this.startActivity(new Intent(this, SelectBluetoothActivity.class).setFlags(FLAG_ACTIVITY_NEW_TASK));
//            }
        }

        return this.board;
    }

//    public void onAppForeground()
//    {
//        this.getBoard().transmissionChannel.open();
//    }
//
//    public void onAppBackground()
//    {
//        this.getBoard().transmissionChannel.close();
//    }

    @Override
    public void onCreate() {
        super.onCreate();

        ProcessLifecycleOwner.get().getLifecycle().addObserver(new ApplicationLifecycleObserver(this));


    }

//    @Override
//    public void onTerminate() {
//        super.onTerminate();
//    }

//    @Override
//    public void onActivityCreated(@NonNull Activity activity, @Nullable Bundle bundle) {
//    }
//
//    @Override
//    public void onActivityStarted(@NonNull Activity activity) {
//        if (++activityReferences == 1 && !isActivityChangingConfigurations) {
//            this.onAppForeground();
//        }
//    }
//
//    @Override
//    public void onActivityResumed(@NonNull Activity activity) {
//    }
//
//    @Override
//    public void onActivityPaused(@NonNull Activity activity) {
//
//    }
//
//    @Override
//    public void onActivityStopped(@NonNull Activity activity) {
//        isActivityChangingConfigurations = activity.isChangingConfigurations();
//        if (--activityReferences == 0 && !isActivityChangingConfigurations) {
//            this.onAppBackground();
//        }
//    }
//
//    @Override
//    public void onActivitySaveInstanceState(@NonNull Activity activity, @NonNull Bundle bundle) {
//
//    }
//
//    @Override
//    public void onActivityDestroyed(@NonNull Activity activity) {
//
//    }
}
