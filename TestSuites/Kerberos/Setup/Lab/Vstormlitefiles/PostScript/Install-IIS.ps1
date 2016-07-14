##################################################################################
## Copyright (c) Microsoft Corporation. All rights reserved.
##################################################################################

try {
#-----------------------------------------------------------------------------------------------
# Install Web Server
#-----------------------------------------------------------------------------------------------
Write-Host "Installing windows authenticcation..."
Install-WindowsFeature -Name Web-Windows-Auth

Write-Host "Installing Web Server..."
Install-WindowsFeature -Name Web-Server 

}
catch{
    [string]$Message = "Install Web server failed. Error Message: " + $_.Exception.Message
    throw $Message
}