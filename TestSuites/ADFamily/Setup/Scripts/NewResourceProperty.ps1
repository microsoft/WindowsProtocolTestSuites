#############################################################
## Copyright (c) Microsoft. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
#############################################################

#-----------------------------------------------------------------------------------------------
# Define a New Claim Type
#-----------------------------------------------------------------------------------------------
Param
(
    [string]$Name,
    [string[]]$Values
)

Write-Host "Creating a New Resource Property: $Name ..." -ForegroundColor Yellow

$SuggestedValues = New-Object Collections.ArrayList
foreach ($Value in $Values)
{
    $ValueEntry = New-Object Microsoft.ActiveDirectory.Management.ADSuggestedValueEntry($Value, $Value, "")
    $SuggestedValues.Add($ValueEntry)
}

New-ADResourceProperty -DisplayName $Name -ID $Name -IsSecured $true -SuggestedValues $SuggestedValues -Enabled $true -ResourcePropertyValueType ms-DS-SingleValuedChoice
Add-ADResourcePropertyListMember "Global Resource Property List" -Members $Name

try
{
    $ResourceProperty = Get-ADResourceProperty -Identity $Name
}
catch
{
    Throw "New Resource Property Creation Failed!"
}

$ResourceProperty
Write-Host "New Resource Property Created Successfully. DisplayName: $Name" -ForegroundColor Green
