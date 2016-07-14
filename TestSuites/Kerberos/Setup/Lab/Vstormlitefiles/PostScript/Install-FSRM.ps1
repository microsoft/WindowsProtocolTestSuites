##################################################################################
## Copyright (c) Microsoft Corporation. All rights reserved.
##################################################################################

try {
    Install-WindowsFeature File-Services
    Install-WindowsFeature FS-Resource-Manager
}
catch{
    [string]$Message = "Unable to install FSRM feature. Error Message: " + $_.Exception.Message
    throw $Message
}