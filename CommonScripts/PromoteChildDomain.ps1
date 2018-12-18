#############################################################
## Copyright (c) Microsoft Corporation. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
#############################################################

#-----------------------------------------------------------------------------
# Function: PromoteChildDomain.ps1
# Usage   : Install ADDS feature on the server and promote it to a CDC.
# Params  : [string]$NewDomainName:    The name of the child domain.
#           [string]$ParentDomainName: The name of the parent domain.
#           [string]$AdminUser :       The username of the Administrator.
#           [string]$AdminPwd  :       The password of the Administrator.
#           [string]$DomainMode:       The Mode of the child domain.
# Remark  : A reboot is needed after promoting to DC.
#-----------------------------------------------------------------------------
Param
(
    [Parameter(Mandatory=$true)]
    [ValidateNotNullOrEmpty()]
    [string]$NewDomainName,

    [Parameter(Mandatory=$true)]
    [ValidateNotNullOrEmpty()]
    [string]$ParentDomainName,

    [Parameter(Mandatory=$true)]
    [ValidateNotNullOrEmpty()]
    [string]$AdminUser,
    
    [Parameter(Mandatory=$true)]
    [ValidateNotNullOrEmpty()]
    [string]$AdminPwd,

    [Parameter(Mandatory=$false)]
    [string]$DomainMode
)

# Install ADDS
Write-Host "Add AD DS Features ..." -ForegroundColor Yellow
Install-WindowsFeature -Name AD-Domain-Services `
	-IncludeAllSubFeature `
	-IncludeManagementTools `
	-ErrorAction Stop

# Promote to Child Domain DC
Write-Host "Promoting this computer to Child Domain DC ..." -ForegroundColor Yellow

Import-Module ADDSDeployment

$SecurePwd = ConvertTo-SecureString $AdminPwd -AsPlainText -Force
$credential = New-Object System.Management.Automation.PSCredential "$ParentDomainName\$AdminUser", $SecurePwd -ErrorAction Stop

if ([System.String]::IsNullOrEmpty($DomainMode))
{
	$promoteResult = Install-ADDSDomain -DomainType ChildDomain `
	-ParentDomainName $ParentDomainName `
	-NewDomainName $NewDomainName `
	-Credential $credential `
	-SafeModeAdministratorPassword $SecurePwd `
	-InstallDns `
	-NoRebootOnCompletion `
	-ErrorAction Stop `
	-Force
}
else
{
	$promoteResult = Install-ADDSDomain -DomainType ChildDomain `
	-ParentDomainName $ParentDomainName `
	-NewDomainName $NewDomainName `
	-Credential $credential `
	-DomainMode $DomainMode `
	-SafeModeAdministratorPassword $SecurePwd `
	-InstallDns `
	-NoRebootOnCompletion `
	-ErrorAction Stop `
	-Force
} 

return $promoteResult