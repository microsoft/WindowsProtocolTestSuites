#############################################################
## Copyright (c) Microsoft. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
#############################################################

#-----------------------------------------------------------------------------------------------
# Define a New Central Access Policy
#-----------------------------------------------------------------------------------------------
Param
(
    [string]$Name,
    [string[]]$CentralAccessRules
)

Write-Host "Creating a New Central Access Policy: $Name ..." -ForegroundColor Yellow

New-ADCentralAccessPolicy -Name $Name
Add-ADCentralAccessPolicyMember -Identity $Name -Members $CentralAccessRules

try
{
    $CentralAccessPolicy = Get-ADCentralAccessPolicy -Identity $Name
}
catch
{
    Throw "New Central Access Policy Creation Failed!"
}

$CentralAccessPolicy
Write-Host "New Central Access Policy Created Successfully. DisplayName: $Name" -ForegroundColor Green
