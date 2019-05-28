#############################################################
## Copyright (c) Microsoft. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
#############################################################

$computerName = $PTFProp_Common_ENDPOINT_NetbiosName
$domainName   = $PTFProp_Common_PrimaryDomain_DNSName
$domainNetBiosName   = $PTFProp_Common_PrimaryDomain_NetBiosName

$domainNC = "DC=" + $domainName.Replace(".",",DC=")

$primaryDCName = $PTFProp_Common_WritableDC1_NetbiosName
$domainUserAccount = $PTFProp_Common_DomainAdministratorName
$userName = "$domainNetBiosName\$domainUserAccount"
$password = $PTFProp_Common_DomainUserPassword
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