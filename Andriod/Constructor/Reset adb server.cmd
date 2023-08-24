@ECHO OFF

REM To make sure that the same IP is always assigned to this device, go to the router settings and bind the device's MAC to the desired IP

CALL ADBSettings.cmd

%AdbDrive%:

cd %AdbPath%

adb kill-server

@ECHO "Please ensure the device is plugged in via USB"
PAUSE
