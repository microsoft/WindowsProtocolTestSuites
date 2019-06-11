#############################################################
## Copyright (c) Microsoft. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
#############################################################

#-----------------------------------------------------------------------------------------------
# Define a New Claim Type
#-----------------------------------------------------------------------------------------------
Param
(
    [ValidateSet("User", "Computer", "Both")]
    [string]$ClaimType = "User",
    [string]$ClaimID,
    [string]$Name,
    [string]$SourceAttribute,
    [string[]]$Values
)

Write-Host "Creating a New Claim Type: $Name ..." -ForegroundColor Yellow

$SuggestedValues = New-Object Collections.ArrayList
foreach ($Value in $Values)
{
    $ValueEntry = New-Object Microsoft.ActiveDirectory.Management.ADSuggestedValueEntry($Value, $Value, "")
    $SuggestedValues.Add($ValueEntry)
}

if($ClaimType -eq "Both")
{
    New-ADClaimType -AppliesToClasses @("User", "Computer") -DisplayName $Name -SourceAttribute $SourceAttribute -ID $ClaimID -SuggestedValues $SuggestedValues
}
else
{
    New-ADClaimType -AppliesToClasses $ClaimType -DisplayName $Name -SourceAttribute $SourceAttribute -ID $ClaimID -SuggestedValues $SuggestedValues
}

try
{
    $Claim = Get-ADClaimType -Identity $Name
}
catch
{
    Throw "New Claim Type Creation Failed!"
}

$Claim
Write-Host "New Claim Type Created Successfully. DisplayName: $Name" -ForegroundColor Green
