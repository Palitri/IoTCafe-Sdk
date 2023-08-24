package com.palitri.openiot.constructor.notifications;

import android.util.Log;


import com.google.firebase.messaging.FirebaseMessagingService;
import com.google.firebase.messaging.RemoteMessage;

public class NotificationsService extends FirebaseMessagingService {

    private static final String TAG = "NotificationsService";

    /**
     * Called when message is received.
     *
     * @param remoteMessage Object representing the message received from Firebase Cloud Messaging.
     */
    // [START receive_message]
    @Override
    public void onMessageReceived(RemoteMessage remoteMessage) {
        // [START_EXCLUDE]
        // There are two types of messages data messages and notification messages. Data messages
        // are handled
        // here in onMessageReceived whether the app is in the foreground or background. Data
        // messages are the type
        // traditionally used with GCM. Notification messages are only received here in
        // onMessageReceived when the app
        // is in the foreground. When the app is in the background an automatically generated
        // notification is displayed.
        // When the user taps on the notification they are returned to the app. Messages
        // containing both notification
        // and data payloads are treated as notification messages. The Firebase console always
        // sends notification
        // messages. For more see: https://firebase.google.com/docs/cloud-messaging/concept-options
        // [END_EXCLUDE]

        // TODO(developer): Handle FCM messages here.
        // Not getting messages here? See why this may be: https://goo.gl/39bRNJ
        Log.d(TAG, "From: " + remoteMessage.getFrom());

        // Check if message contains a data payload.
        if (remoteMessage.getData().size() > 0) {
            Log.d(TAG, "Message data payload: " + remoteMessage.getData());

            if (/* Check if data needs to be processed by long running job */ true) {
                // For long-running tasks (10 seconds or more) use WorkManager.
                //scheduleJob();
            } else {
                // Handle message within 10 seconds
                //handleNow();
            }

        }

        // Check if message contains a notification payload.
        if (remoteMessage.getNotification() != null) {
            String body = remoteMessage.getNotification().getBody();

            // Show some message here

//            Activity currentActivity = ((ConstructorApplication)getApplication()).getCurrentActivity();
//            if (currentActivity == null)
//                Toast.makeText(getApplicationContext(), body, Toast.LENGTH_SHORT).show();
//            else
//                currentActivity.runOnUiThread(new Runnable() {
//                    public void run() {
//                        Toast.makeText(getApplicationContext(), body, Toast.LENGTH_SHORT).show();
//                    }
//                });

            Log.d(TAG, "Message Notification Body: " + body);
        }

        // Also if you intend on generating your own notifications as a result of a received FCM
        // message, here is where that should be initiated. See sendNotification method below.
    }

    /**
     * There are two scenarios when onNewToken is called:
     * 1) When a new token is generated on initial app startup
     * 2) Whenever an existing token is changed
     * Under #2, there are three scenarios when the existing token is changed:
     * A) App is restored to a new device
     * B) User uninstalls/reinstalls the app
     * C) User clears app data
     */
    @Override
    public void onNewToken(String token) {
        // Update device in web service (if persistance has a user logged in)

//        new OpenCNCService(){
//            @Override
//            public void onRequestUpdateDeviceResponse(Device device, Object... params) {
//                super.onRequestUpdateDeviceResponse(device, params);
//                Log.d(TAG, "Updated token: " + device.token);
//            }
//        }
//            .requestUpdateDevice(new Device() {{ token = token; os = DeviceOS.Android; }});
//
        Log.d(TAG, "Refreshed token: " + token);
    }
}