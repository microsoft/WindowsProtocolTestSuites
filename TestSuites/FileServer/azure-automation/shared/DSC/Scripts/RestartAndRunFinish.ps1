# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

##############################################################################
# Function: RestartAndRunFinish
# Usage   : Call this script to clean up the scheduled task after calling 
#           RestartAndRun.ps1.
# Remark  : This script should be called at the end of your script, if you 
#           have ever called RestartAndRun.ps1.
##############################################################################

$private:taskName = "TKFRSAR"

$existingTask = Get-ScheduledTask -TaskName $taskName -ErrorAction SilentlyContinue
if ($null -ne $existingTask) {
    Unregister-ScheduledTask -TaskName $taskName -Confirm:$false
}

# Also clean up the one-shot reboot task registered by Deploy-SUT.ps1
$private:rebootTaskName = "PostDeployReboot"
$existingReboot = Get-ScheduledTask -TaskName $rebootTaskName -ErrorAction SilentlyContinue
if ($null -ne $existingReboot) {
    Unregister-ScheduledTask -TaskName $rebootTaskName -Confirm:$false
}
