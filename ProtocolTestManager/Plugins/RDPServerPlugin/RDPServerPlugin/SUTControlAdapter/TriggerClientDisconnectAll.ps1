# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

#This method is used to trigger RDP server to close all RDP connection to server for clean up.

cmd /c net use \\$PtfPropSUTName\$PtfPropSUTSystemDrive$ $PtfPropSUTUserPassword /user:$PtfPropSUTUserName
cmd /c del \\$PtfPropSUTName\$PtfPropSUTSystemDrive$\DisconnectAll.signal /F

# Check SUT started the PS Remoting
$isSutPSRemotingStarted = .\Check-PSRemoting.ps1 $PtfPropSUTName
if(-not $isSutPSRemotingStarted) {return -1}

# Run task to simulate a server initiated disconnect request
$returnValue = .\Run-TaskWithPSRemoting.ps1 $PtfPropSUTName $PtfPropTriggerServerDisconnectAll_Task $PtfPropSUTUserName $PtfPropSUTUserPassword
if($returnValue -lt 0) {return $returnValue}

$count = 0
while (!(test-path \\$PtfPropSUTName\$PtfPropSUTSystemDrive$\DisconnectAll.signal) -and $count -lt 20)
{
    $count++
    sleep 1
}

cmd /c net use \\$PtfPropSUTName\$PtfPropSUTSystemDrive$ /delete /y

return 0  #operation succeed