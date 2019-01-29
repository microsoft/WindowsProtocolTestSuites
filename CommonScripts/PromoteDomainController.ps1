#############################################################################
## Copyright (c) Microsoft Corporation. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
#############################################################################

#-----------------------------------------------------------------------------
# Function: PromoteDomainController
# Usage   : Install ADDS feature on the server and promote it to DC.
# Params  : [string]$DomainName  : The name of the domain.
#           [string]$AdminUser   : The name of the admin user.
#           [string]$AdminPwd    : The password of the Administrator.
#           [boolean]$IsPrimary  : indicator if its a primary domain.
#           [string]$ForestMode  : ForestMode of the domain.
# Remark  : A reboot is needed after promoting to DC.
#-----------------------------------------------------------------------------
Param
(
    [Parameter(Mandatory=$true)]
    [ValidateNotNullOrEmpty()]
    [string]$DomainName, 

    [string]$AdminUser="Administrator",
    
    [Parameter(Mandatory=$true)]
    [ValidateNotNullOrEmpty()]
    [string]$AdminPwd,

    [Parameter(Mandatory=$false)]
    [boolean]$IsPrimary = $true,

    [Parameter(Mandatory=$false)]
    [string]$ForestMode
)
    
try
{
    # Install ADDS
    Install-WindowsFeature -Name AD-Domain-Services `
                           -IncludeAllSubFeature `
                           -IncludeManagementTools `
                           -ErrorAction Stop
    
    # Promote to DC
    Import-Module ADDSDeployment

    $SecurePwd = ConvertTo-SecureString $AdminPwd -AsPlainText -Force

    if ($IsPrimary)
    {
        if ([System.String]::IsNullOrEmpty($ForestMode))
        {
            Install-ADDSForest -DomainName $domainName `
                               -InstallDns `
                               -SafeModeAdministratorPassword $SecurePwd `
                               -NoRebootOnCompletion `
                               -ErrorAction Stop `
                               -Force
        }
        else
        {
            Install-ADDSForest -ForestMode $ForestMode `
                               -DomainMode $ForestMode `
                               -DomainName $domainName `
                               -InstallDns `
                               -SafeModeAdministratorPassword $SecurePwd `
                               -NoRebootOnCompletion `
                               -ErrorAction Stop `
                               -Force
        }
    }
    else
    {
        $cred = New-Object System.Management.Automation.PSCredential "$domainName\$AdminUser", $SecurePwd -ErrorAction Stop
        Install-ADDSDomainController -DomainName $domainName `
                                     -Credential $cred `
                                     -InstallDNS `
                                     -SafeModeAdministratorPassword $SecurePwd `
                                     -NoRebootOnCompletion `
                                     -ErrorAction Stop `
                                     -Force
    }

}
catch
{
    throw "Error happeded while executing PromoteDomainController.ps1:" + $_.Exception.Message
}

