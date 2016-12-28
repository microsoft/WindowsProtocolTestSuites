#############################################################
## Copyright (c) Microsoft. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
#############################################################

#-----------------------------------------------------------------------------
# Get LDS Instance GUID
#-----------------------------------------------------------------------------

Param
(
    [parameter(Mandatory=$true)]
    [string]$InstanceName
)

# Get GUID
[string]$regPath = "HKLM:\SYSTEM\ControlSet001\services\ADAM_" + $InstanceName + "\Parameters"
[string]$ConNC = Get-ItemProperty -path $regPath -Name "Configuration NC"
$separators = @(' ', ',', ';')
[string[]]$Temp = $ConNC.Split($separators)
[string]$GUID=""

foreach($token in $Temp)
{
    if($token.Contains("{") -and $token.Contains("}"))
    {
        [string[]]$parts = $token.Split('=')
        $GUID = $parts[1]
        $GUID=$GUID.Replace("{","")
        $GUID=$GUID.Replace("}","")
        break;
    }
}

Write-Host "ConNC = $ConNC"
Write-Host "GUID = $GUID"

return $GUID
