# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

# This script is used to edit rdp file in remote
# Return Value: 0 indicates task is started successfully; -1 indicates failed to run the specified task

Param(
[string]$computer, # host name or ip address
[string]$userName,
[string]$filePath,
[string]$originalText,
[string]$newText
)

# Edit the file
$returnValue = 1;

$returnValue = Invoke-Command -HostName $ptfprop_SUTName -UserName $ptfprop_SUTUserName -ScriptBlock {param([string]$filePath,[string]$originalText,[string]$newText) if((Get-Content $filePath).Contains($originalText)){ (Get-Content $filePath) | 
Foreach-Object {$_.replace($originalText, $newText)} | 
Set-Content $filePath} if((Get-Content $filePath).Contains($newText)){return 0} else{return -1}} -ArgumentList $filePath,$originalText,$newText
"$filePath replaced from $originalText to $newText; return value: $returnValue" | out-file "./EditFile.log" -Append  -Encoding unicode

return $returnValue  #operation succeed if 0
