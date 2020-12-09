# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

#This script is used to check if PowerShell Remoting is started on the given computer

param(
[string]$computerName = "." # host name or ip address
)

$triedCount = 0
$sutStatus = $null
$maximumRetryCount = 5
while($triedCount -lt $maximumRetryCount -and $sutStatus -eq $null)
{
    $triedCount++
    try
    {
        Get-PSSession|Remove-PSSession
        $psSession=New-PSSession -HostName $computerName -UserName $ptfprop_SUTUserName
        $sutStatus = "pass"
    }
    catch
    {
        # Sleep before retry if last attempt failed.
        Start-Sleep -Seconds 1
    }
    finally
    {
        Get-PSSession|Remove-PSSession
    }
}

if($sutStatus -ne $null)
{
    return $true
}
return $false


