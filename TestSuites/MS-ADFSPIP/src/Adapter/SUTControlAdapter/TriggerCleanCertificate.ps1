########################################################################################################
## Copyright (c) Microsoft. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
########################################################################################################

param
(
[string]$remoteSignalShare = $PTFProp_Common_SutShareFoler
)

$tmp = $remoteSignalShare + "remoteresponse.txt"
remove-item $tmp
$tmp = $remoteSignalShare + "remoterequest.txt"
$file = new-item $tmp -type file
"cleancert.ps1">>$file
$res = $null
$timeout = 600
$tmp = $remoteSignalShare + "remoteresponse.txt"
while(($res -eq $null) -and ($timeout -gt  0))
{
    sleep(2)
    $timeout-=2
    $res = get-content $tmp
}

return $res

