#############################################################
## Copyright (c) Microsoft. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
#############################################################

#-----------------------------------------------------------------------------
# Function: PromoteRODC
# Usage   : Install ADDS feature on the server and promote it to a RODC.
# Params  : [string]$DomainName: The name of the domain.
#           [string]$AdminUser : The username of the Administrator.
#           [string]$AdminPwd  : The password of the Administrator.
# Remark  : A reboot is needed after promoting to DC.
#-----------------------------------------------------------------------------
Param
(
    [Parameter(Mandatory=$true)]
    [ValidateNotNullOrEmpty()]
    [string]$DomainName, 

    [Parameter(Mandatory=$true)]
    [ValidateNotNullOrEmpty()]
    [string]$AdminUser,
    
    [Parameter(Mandatory=$true)]
    [ValidateNotNullOrEmpty()]
    [string]$AdminPwd,

    [Parameter(Mandatory=$true)]
    [ValidateNotNullOrEmpty()]
    [string]$ReplicationSourceDC
)
    
try
{
    # Install ADDS
    Write-Host "Add AD DS Features ..." -ForegroundColor Yellow
    Install-WindowsFeature -Name AD-Domain-Services `
                           -IncludeAllSubFeature `
                           -IncludeManagementTools `
                           -ErrorAction Stop
    
    # Promote to RODC
    Write-Host "Promoting this computer to RODC ..." -ForegroundColor Yellow

    Import-Module ADDSDeployment

    $SecurePwd = ConvertTo-SecureString $AdminPwd -AsPlainText -Force
    $credential = New-Object System.Management.Automation.PSCredential -ArgumentList "$DomainName\$AdminUser", $SecurePwd
    $DomainNameNetBios = $DomainName.Split('.')[0].ToUpper()

    Install-ADDSDomainController -DomainName $DomainName -Credential $credential -ReadOnlyReplica `
        -SiteName "Default-First-Site-Name" -SafeModeAdministratorPassword $credential.Password `
        -AllowPasswordReplicationAccountName @("$DomainNameNetBios\Allowed RODC Password Replication Group","$DomainNameNetBios\Domain Admins") `
        -DelegatedAdministratorAccountName $null `
        -ReplicationSourceDC $ReplicationSourceDC -InstallDns:$true `
        -NoRebootOnCompletion -Force
}
catch
{
    throw "Error happeded while executing PromoteRODC.ps1:" + $_.Exception.Message
}

