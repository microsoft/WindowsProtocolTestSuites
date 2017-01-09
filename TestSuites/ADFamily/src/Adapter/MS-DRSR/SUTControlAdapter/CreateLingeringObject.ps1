#############################################################################
## Copyright (c) Microsoft. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
#############################################################################

$pwd = ConvertTo-SecureString $remotePassword -AsPlainText -Force
$cred = New-Object -TypeName System.Management.Automation.PSCredential -ArgumentList $userName, $pwd

# connect to network share on dc1
net use \\$dc1Name $remotePassword /USER:$userName
Copy-Item -Path $remoteScriptPath -Destination \\$dc1Name\C$ 
net use \\$dc1Name /delete

# enable script execution
Invoke-Command -ComputerName $dc1Name -Credential $cred -ScriptBlock {
    Param(
    [string]$dc1Name,
    [string]$dc2Name,
    [string]$userName,
    [string]$remotePassword,
    [string]$configNC
    )

    Set-ExecutionPolicy Unrestricted -Force

    C:\CreateLingeringObjectRemote.ps1 `
        -dc1Name $dc1Name.Split(".")[0] `
        -dc2Name $dc2Name.Split(".")[0] `
        -userName $userName `
        -password $remotePassword `
        -newSite "testLingeringSite" `
        -configNC $configNC
} -ArgumentList $dc1Name, $dc2Name, $userName, $remotePassword, $configNC


return $true


