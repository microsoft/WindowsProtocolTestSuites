##################################################################################
## Copyright (c) Microsoft Corporation. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
##################################################################################

#############################################################################
##
## Microsoft Windows Powershell Scripting
## File:           Install-DfsManagement-Feature.ps1
## Purpose:        Install DFS Management Tools.
## Requirements:   Windows Powershell 2.0
## Supported OS:   Windows Server 2008 R2, Windows Server 2012, Windows Server 2012 R2,
##                 Windows Server 2016 and later
##
##############################################################################

Param
(
    [Switch]$FsRole,

    [Switch]$DfsRole,

    [Switch]$DfsNamespaceRole,

    [Switch]$DfsReplRole,

    [Switch]$DfsMgmtTools    
)

Import-Module ServerManager

if($FsRole)
{
    # Install File Services server role feature
    Add-WindowsFeature File-Services -IncludeAllSubFeature -Confirm:$false
}
if($DfsRole)
{
    # Install Distributed File System role service feature
    Add-WindowsFeature FS-DFS -IncludeAllSubFeature -Confirm:$false
}
if($DfsNamespaceRole)
{
    # Install DFS Namespaces role service feature
    Add-WindowsFeature FS-DFS-Namespace -IncludeAllSubFeature -Confirm:$false
}
if($DfsReplRole)
{
    # Install DFS Replication role service feature
    Add-WindowsFeature FS-DFS-Replication -IncludeAllSubFeature -Confirm:$false
}
if($DfsMgmtTools)
{
    $type = (Get-ItemProperty -Path "HKLM:\SOFTWARE\Microsoft\Windows NT\CurrentVersion").InstallationType
    if ($type -ne "Server Core") {
        # Install Distributed File System Tools feature feature
        Add-WindowsFeature RSAT-DFS-Mgmt-Con -IncludeAllSubFeature -Confirm:$false
    }
}

# Set DFS Replication debug log level to 5(max)
wmic /namespace:\\root\microsoftdfs path dfsrmachineconfig set debuglogseverity=5