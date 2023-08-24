package com.palitri.openiot.construction.framework.board.transmission.bluetooth;

public class BluetoothStateMonitor {

//    private final BroadcastReceiver mReceiver = new BroadcastReceiver() {
//        @Override
//        public void onReceive(Context context, Intent intent) {
//            final String action = intent.getAction();
//
//            if (action.equals(BluetoothAdapter.ACTION_STATE_CHANGED)) {
//                final int state = intent.getIntExtra(BluetoothAdapter.EXTRA_STATE,
//                        BluetoothAdapter.ERROR);
//                switch (state) {
//                    case BluetoothAdapter.STATE_OFF:
//                        // Bluetooth has been turned off;
//                        break;
//                    case BluetoothAdapter.STATE_TURNING_OFF:
//                        // Bluetooth is turning off;
//                        break;
//                    case BluetoothAdapter.STATE_ON:
//                        // Bluetooth is on
//                        break;
//                    case BluetoothAdapter.STATE_TURNING_ON:
//                        // Bluetooth is turning on
//                        break;
//                }
//            }
//        }
//    };
//
//    @Override
//    public void onCreate(Bundle savedInstanceState) {
//        super.onCreate(savedInstanceState);
//
//        // ...
//
//        // Register for broadcasts on BluetoothAdapter state change
//        IntentFilter filter = new IntentFilter(BluetoothAdapter.ACTION_STATE_CHANGED);
//        registerReceiver(mReceiver, filter);
//    }
//
//    @Override
//    public void onStop() {
//        super.onStop();
//
//        // ...
//
//        // Unregister broadcast listeners
//        unregisterReceiver(mReceiver);
//    }
}
