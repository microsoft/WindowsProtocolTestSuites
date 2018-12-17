##################################################################################
## Copyright (c) Microsoft Corporation. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
##################################################################################
#-------------------------------------------------------------------------------
# Function: Set-NetworkLocation
# Usage   : Set network location to public or private
# Params  : -NetworkLocation <string>: Indicate the network location which 
#           Must be either public or private.
#-------------------------------------------------------------------------------
Param
(
    [parameter(mandatory=$true)]
    [ValidateNotNullOrEmpty()]
    [string]$NetworkLocation
)

switch ($NetworkLocation.ToLower())
{
	"private" {
        $category = 1
        break
    }
    "public" {
        $category = 3
        break
    }
    default {
        throw "Incorrent NetworkLocation! Must be private or public."
    }
}
    
try{
    # Network location setting only available after Windows Vista
    if([environment]::OSVersion.Version.Major -lt 6){ return }

    # Skip network location setting if local machine is joined to a domain
    if(1,3,4,5 -contains (Get-WmiObject win32_computersystem).DomainRole) { return }

    # Get network connections
    $NetworkListManager = [Activator]::CreateInstance([Type]::GetTypeFromCLSID([Guid]"{DCB00C01-570F-4A9B-8D69-199FDBA5723B}"))
    $Connections = $NetworkListManager.GetNetworkConnections()

    # Set network location
    $Connections | foreach {$_.GetNetwork().SetCategory($category) }
}
catch
{
    [string]$Message = "Unable to set network location. Error Message: " + $_.Exception.Message
    throw $Message
}
