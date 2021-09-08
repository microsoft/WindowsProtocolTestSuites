# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

#This script is used to check if PowerShell Remoting is started on the given computer

param(
[string] $computerName = ".", # host name or ip address
[string] $userName
)

$triedCount = 0
$sutStatus = $false
$sutRemoteConnectivityStatus = $Null

try {
    while($triedCount -lt 300)
    {
        $triedCount++
    
        # Test if we can connect to SUT
        $sutRemoteConnectivityStatus = Invoke-Command -HostName $computerName -UserName $userName -ScriptBlock { 
            Get-Host 
        } -ErrorAction Ignore

        if ($Null -ne $sutRemoteConnectivityStatus)
        {
            break
        }
    }
}
finally {
    if ($Null -ne $sutRemoteConnectivityStatus)
    {
        $sutStatus = $true
    }
}

return $sutStatus



