# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

# This script is used to trigger one touch event on client.

cmd /c schtasks /run /s $PtfProp_SUTName /U $PtfProp_SUTUserName /P $PtfProp_SUTUserPassword /TN $PtfProp_TriggerTouchHoverEvent_Task 2>&1 | out-file ".\error.txt"

$cmdError = get-content ".\error.txt"
if($cmdError -ne $null)
{
    if($cmdError.GetType().IsArray)
    {
        $cmdError = $cmdError[0]
    }
    if($cmdError.ToUpper().Contains("ERROR"))
    {
        return -1 # operation failed
    }
}

sleep 1
#return 0  #operation succeed
return -1