#############################################################################
## Copyright (c) Microsoft Corporation. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
#############################################################################

try {
    Install-WindowsFeature File-Services
    Install-WindowsFeature FS-Resource-Manager
}
catch{
    [string]$Message = "Unable to install FSRM feature. Error Message: " + $_.Exception.Message
    throw $Message
}