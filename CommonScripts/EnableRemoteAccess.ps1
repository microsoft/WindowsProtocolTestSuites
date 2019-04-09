##################################################################################
## Copyright (c) Microsoft Corporation. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
##################################################################################
#-------------------------------------------------------------------------------
# Function: EnableRemoteAccess
# Usage   : Enable RemoteAccess feature.
#-------------------------------------------------------------------------------

Import-Module ServerManager
Add-WindowsFeature RemoteAccess -IncludeAllSubFeature -ErrorAction Stop ##-IncludeManagementTools


$CommandList = @("netsh ras set type lanonly lanonly IPv4", 
                "netsh ras set conf ENABLED",
                "net stop sstpsvc",
                "net start sstpsvc",
                "net stop rasman",
                "net start rasman",
                "net stop RemoteAccess",
                "sc config RemoteAccess start=Auto",
                "net start RemoteAccess")

foreach ($Command in $CommandList)
{
    cmd /c $Command 2>&1 | Write-Host

    # If RemoteAccess has already stopped before executing 
    # net stop, or started before executing net start,
    # net will return 2. In this situation, no exception
    # will be thrown.
    If ($Command.Contains("net stop") -and ($LASTEXITCODE -eq 2))
    {
        continue
    }

    if($LASTEXITCODE -ne 0)
    {
        throw "Error happened in executing $Command"
    }
}
