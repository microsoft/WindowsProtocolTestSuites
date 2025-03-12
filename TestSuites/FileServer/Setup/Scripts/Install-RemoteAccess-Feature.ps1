# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

#############################################################################
##
## Microsoft Windows Powershell Scripting
## File:           Install-RemoteAccess-Feature.ps1
## Purpose:        Install all the RemoteAccess features and tools, and enable autostart.
## Requirements:   Windows Powershell 2.0
## Supported OS:   Windows Server 2008 R2, Windows Server 2012, Windows Server 2012 R2,
##                 Windows Server 2016 and later
##
##############################################################################

Import-Module ServerManager
Add-WindowsFeature RemoteAccess -IncludeAllSubFeature -IncludeManagementTools

$feature = Get-WindowsFeature -Name RemoteAccess -ErrorAction SilentlyContinue
$featureInstalled = $feature -and $feature.Installed

if (!$featureInstalled) {
    Write-Error "Failed to install RemoteAccess feature."
    return 1
}

# set type
# ipv4rtrtype = lanonly, specifies that this computer is configured as an IPv4 router. And is a LAN-only router and does not support demand-dial or VPN connections to remote networks.
# ipv6rtrtype = lanonly, specifies that this computer is configured as an IPv6 router. And is a LAN-only router and does not support demand-dial or VPN connections to remote networks.
# rastype = ipv4, specifies that this computer accepts IPv4-based remote access connections.
CMD /C NETSH ras set type lanonly lanonly IPv4

# set conf
# confstate = enabled, enables the server configuration.
CMD /C NETSH ras set conf ENABLED

# sc config
# start = auto, specifies that automatically starts each time the computer is restarted and runs even if no one logs on to the computer.
CMD /C NET stop RemoteAccess
CMD /C SC config RemoteAccess start=Auto
CMD /C NET start RemoteAccess

return 0