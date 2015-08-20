# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

[String]$serverName
[String]$signState

$regeditPath = "\\" + $serverName + "\HKEY_LOCAL_MACHINE\System\ControlSet001\Services\LanmanServer\Parameters"

if ($signState -eq "Disabled")
{
    cmd /C reg ADD $regeditPath /f /v enablesecuritysignature /t REG_DWORD /d 0
    cmd /C reg ADD $regeditPath /f /v requiresecuritysignature /t REG_DWORD /d 0
}
elseif ($signState -eq "Enabled")
{
    cmd /C reg ADD $regeditPath /f /v enablesecuritysignature /t REG_DWORD /d 1
    cmd /C reg ADD $regeditPath /f /v requiresecuritysignature /t REG_DWORD /d 0
}
elseif ($signState -eq "Required")
{
    cmd /C reg ADD $regeditPath /f /v enablesecuritysignature /t REG_DWORD /d 0
    cmd /C reg ADD $regeditPath /f /v requiresecuritysignature /t REG_DWORD /d 1
}
elseif ($signState -eq "DisabledUnlessRequired")
{
    cmd /C reg ADD $regeditPath /f /v enablesecuritysignature /t REG_DWORD /d 0
    cmd /C reg ADD $regeditPath /f /v requiresecuritysignature /t REG_DWORD /d 0
}

exit 0