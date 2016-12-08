:: Copyright (c) Microsoft. All rights reserved.
:: Licensed under the MIT license. See LICENSE file in the project root for full license information.

Echo reg ADD HKCU\SOFTWARE\Microsoft\Windows\CurrentVersion\RunOnce /f /v SCRIPT /t REG_SZ /d %1 >%SystemDrive%\RestartAndRun.signal 
Echo reg ADD HKCU\SOFTWARE\Microsoft\Windows\CurrentVersion\RunOnce /f /v SCRIPT /t REG_SZ /d %1 >%SystemDrive%\RestartAndRun.cmd

reg ADD HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\RunOnce /f /v SCRIPT /t REG_SZ /d %SystemDrive%\RestartAndRun.cmd

shutdown -r -f -t 3
