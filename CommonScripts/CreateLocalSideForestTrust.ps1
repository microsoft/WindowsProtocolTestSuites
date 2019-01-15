#############################################################
## Copyright (c) Microsoft Corporation. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
#############################################################

Param
(
    [Parameter(Mandatory=$true)]
    [ValidateNotNullOrEmpty()]
    [String]$TargetForestName,

    [Parameter(Mandatory=$true)]
    [ValidateNotNullOrEmpty()]
    [String]$TrustPassword
)

$LocalForest = [System.DirectoryServices.ActiveDirectory.Forest]::GetCurrentForest()

try
{
    # Build trust relationship on local forest only
    $LocalForest.CreateLocalSideOfTrustRelationship($TargetForestName, "Bidirectional", $TrustPassword)
}
# If trust relationship already exists
catch [System.DirectoryServices.ActiveDirectory.ActiveDirectoryObjectExistsException]
{
    Write-Host "Trust relationship already exists."
}
catch
{
    throw "Failed to create trust relationship. Error: " + $_.Exception.Message
}
