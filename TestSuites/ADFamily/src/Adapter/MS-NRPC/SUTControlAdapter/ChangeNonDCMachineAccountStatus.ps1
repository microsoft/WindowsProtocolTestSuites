#############################################################
## Copyright (c) Microsoft. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
#############################################################

function GetPtfVariable
{
    param($name)
	$v = Get-Variable -Name ("PTFProp"+$name)
	return $v.Value
}

$computerName = GetPtfVariable "Common.ENDPOINT.NetbiosName"
$domainName   = GetPtfVariable "Common.PrimaryDomain.DNSName"
$domainNetBiosName   = GetPtfVariable "Common.PrimaryDomain.NetBiosName"

$domainNC = "DC=" + $domainName.Replace(".",",DC=")

$primaryDCName = GetPtfVariable "Common.WritableDC1.NetbiosName"
$domainUserAccount = GetPtfVariable "Common.DomainAdministratorName"
$userName = "$domainNetBiosName\$domainUserAccount"
$password = GetPtfVariable "Common.DomainUserPassword"
#----------------------------------------------------------------------------
# Get Object instance
#----------------------------------------------------------------------------
$objectPath = "cn=$computerName, cn=computers, $domainNC"
$machineObj = New-Object System.DirectoryServices.DirectoryEntry("LDAP://$primaryDCName/$objectPath",$userName,$password)
if ($machineObj -eq $null)
{
    Throw "EXECUTE [ChangeNonDCMachineAccountStatus.ps1] FAILED. Machine object does not exist in Active Directory."
}

#----------------------------------------------------------------------------
# Disable or Enable
#----------------------------------------------------------------------------
if ($isEnable -eq $true)
{
New-Item -Force -ItemType directory -Path c:\temp\
$strFileName="c:\temp\changednondcaccountstatus.txt"
If (Test-Path $strFileName)
{
    Remove-Item $strFileName
}
    $machineObj.InvokeSet("AccountDisabled", 0) 

}
else
{
New-Item -Force -ItemType directory -Path c:\temp\
$strFileName="c:\temp\changednondcaccountstatus.txt"
"DONE" >> $strFileName

    $machineObj.InvokeSet("AccountDisabled", 1) 
}

$machineObj.SetInfo()