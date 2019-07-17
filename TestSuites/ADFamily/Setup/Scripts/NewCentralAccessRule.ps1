#############################################################
## Copyright (c) Microsoft. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
#############################################################

#-----------------------------------------------------------------------------------------------
# Define a New Central Access Rule
#-----------------------------------------------------------------------------------------------
Param
(
    [string]$Name,
    [string]$ClaimTypeName,
    [string]$ClaimTypeValue,
    [string]$ResourcePropertyName,
    [string]$ResourcePropertyValue
)

Write-Host "Creating a New Central Access Rule: $Name ..." -ForegroundColor Yellow

$Condition = "(@RESOURCE.$ResourcePropertyName == `"$ResourcePropertyValue`")"
$Acl = "O:SYG:SYD:AR(A;;FA;;;OW)(A;;FA;;;BA)(A;;FA;;;SY)(XA;;FA;;;AU;(@USER.$ClaimTypeName Any_of {`"$ClaimTypeValue`"}))"
New-ADCentralAccessRule -Name $Name -ResourceCondition $Condition -CurrentAcl $Acl

try
{
    $CentralAccessRule = Get-ADCentralAccessRule -Identity $Name
}
catch
{
    Throw "New Central Access Rule Creation Failed!"
}

$CentralAccessRule
Write-Host "New Central Access Rule Created Successfully. DisplayName: $Name" -ForegroundColor Green
