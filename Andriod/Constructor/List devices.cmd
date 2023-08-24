@ECHO OFF

CALL ADBSettings.cmd

%AdbDrive%:

cd %AdbPath%

adb devices

PAUSE