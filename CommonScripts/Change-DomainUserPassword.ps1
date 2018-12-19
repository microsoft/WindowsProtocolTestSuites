#############################################################################
## Copyright (c) Microsoft Corporation. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
#############################################################################

#-----------------------------------------------------------------------------
# Function: PromoteDomainController
# Usage   : Install ADDS feature on the server and promote it to DC.
# Params  : [string]$DomainName: The name of the domain.
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
    [string]$Username, 
    
    [Parameter(Mandatory=$true)]
    [ValidateNotNullOrEmpty()]
    [string]$NewPassword
)
    
try
{
    # Get domain object
    $Domain = Get-ADDomain $DomainName
    # Get user distinguished name
    $AdminDN = "CN=$Username," + $Domain.UsersContainer
    # Convert password to secure string
    $SecurePwd = ConvertTo-SecureString $NewPassword -AsPlainText -Force
    # Reset password
    Set-ADAccountPassword -Identity $AdminDN `
                          -Reset `
                          -NewPassword $SecurePwd `
                          -ErrorAction Stop
}
catch
{
    throw "Unable to change domain user password. Error happened: " + $_.Exception.Message
}

