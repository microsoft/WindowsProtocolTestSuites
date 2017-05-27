#############################################################
## Copyright (c) Microsoft. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
#############################################################

$PSScriptRoot = Split-Path -Parent -Path $MyInvocation.MyCommand.Definition
Push-Location $PSScriptRoot

function GetPtfVariable
{
    param($name)
	$v = Get-Variable -Name ("PTFProp"+$name)
	return $v.Value
}

$objectPath = .\GetAdministratorObjPath.ps1
$attribute = "lastLogon"
$primaryDCName = GetPtfVariable "Common.WritableDC1.NetbiosName"
$primaryDomainNetBiosName = GetPtfVariable "Common.PrimaryDomain.NetBiosName"
$normalDomainUserAccount = GetPtfVariable "Common.DomainAdministratorName"
$userName = "$primaryDomainNetBiosName\$normalDomainUserAccount"
$password = GetPtfVariable "Common.DomainUserPassword"

# Get Information
$objectInstance = New-Object System.DirectoryServices.DirectoryEntry("LDAP://$primaryDCName/$objectPath",$userName,$password)
if ($objectInstance -eq $null)
{
    Throw "EXECUTE [GetAttributeValueFromAD.ps1] FAILED. Object does not exist in Active Directory."
}

# Verifying the result
$adsLargeInteger = $objectInstance.Get($attribute)
if ($adsLargeInteger -eq $null)
{
    Throw "EXECUTE [GetAttributeValueFromAD.ps1] FAILED. Get attribute value failed."
}

$comObj = $adsLargeInteger.GetType()
$highPart = $comObj.InvokeMember("HighPart", [System.Reflection.BindingFlags]::GetProperty, $null, $adsLargeInteger, $null)
$lowPart  = $comObj.InvokeMember("LowPart",  [System.Reflection.BindingFlags]::GetProperty, $null, $adsLargeInteger, $null)

$bytes = [System.BitConverter]::GetBytes($highPart)
$tmp   = [System.Byte[]]@(0,0,0,0,0,0,0,0)
[System.Array]::Copy($bytes, 0, $tmp, 4, 4)
$highPart = [System.BitConverter]::ToInt64($tmp, 0)

$bytes = [System.BitConverter]::GetBytes($lowPart)
$lowPart = [System.BitConverter]::ToUInt32($bytes, 0)
 
return $lowPart + $highPart