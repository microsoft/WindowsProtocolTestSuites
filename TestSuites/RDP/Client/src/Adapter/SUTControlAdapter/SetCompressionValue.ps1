# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

# This script is used to set compression value in rdp file
# Return Value: 0 indicates successfully; -1 indicates failed to set the value

$filePaths = @(
'C:\\RDP-TestSuite-ClientEP\Data\Negotiate.RDP',
'C:\\RDP-TestSuite-ClientEP\Data\NegotiateFullScreen.RDP',
'C:\\RDP-TestSuite-ClientEP\Data\DirectCredSSP.RDP',
'C:\\RDP-TestSuite-ClientEP\Data\DirectCredSSPFullScreen.RDP'
 )

if ($isCompressionEnable -eq $true)
{
    $originalText = "compression:i:0";
    $replacedText = "compression:i:1";
}
else
{
    $originalText = "compression:i:1";
    $replacedText = "compression:i:0";
}
$returnValue = 0
foreach ($filePath in $filePaths)
{
    $currentReturnValue = ./EditFileWithPSRemoting.ps1 $PtfProp_SUTName $PtfProp_SUTUserName $filePath $originalText $replacedText
    "$isCompressionEnable $filePath $originalText $replacedText $currentReturnValue" | out-file "./EditFile.log" -Append -Encoding unicode
    if ($currentReturnValue -ne 0)
    {
        $returnValue = -1;
    }
}

""|out-file "./EditFile.log" -Append -Encoding unicode
return $returnValue

