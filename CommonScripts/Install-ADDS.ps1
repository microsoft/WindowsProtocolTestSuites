#############################################################################
## Copyright (c) Microsoft Corporation. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
#############################################################################

try {
    Write-Host "Installing ADDS..."
    Install-WindowsFeature -Name AD-Domain-Services -IncludeAllSubFeature
}
catch {
    [string]$Message = "Unable to install ADDS feature. Error Message: " + $_.Exception.Message
    throw $Message
}