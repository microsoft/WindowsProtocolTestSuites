#############################################################################
## Copyright (c) Microsoft Corporation. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
#############################################################################

#############################################################################
##
## Microsoft Windows PowerShell Sripting
## File:           ControlRegistryKey.ps1
## Purpose:        Remove or add key value to Windows registry.
## Requirements:   Windows PowerShell 2.0
## Supported OS:   Windows Server 2012 or later versions
##
##############################################################################
Param(
    [ValidateSet('Add', 'Remove')]
    [string]$ControlRegistryKey,       # Control Add or Remove registry key.

    [ValidateScript({
        # Only absolute path is accepted. Because relative path
        # may cause trouble after rebooting. 
        if([System.IO.Path]::IsPathRooted($_) -eq $False)
        {
            throw "Argument ScriptPath must be absolute path"
        }
        return $true
    })]
    [string]$ScriptPath,                # Path of the calling script to be executed after rebooting.       
    
    [string]$RegKeyName,                # Name of the add/remove registry key.   
    [string]$PhaseIndicator,            # Phase in the calling script after reboot.    
    [string]$ArgumentList,              # Argument list needed in the calling script.
    [bool]$AutoRestart = $false
)
$RegKeyPath = "HKLM:\Software\Microsoft\Windows\CurrentVersion\Run"

Start-Transcript -Path "$env:HOMEDRIVE\Logs\ControlRegistryKey.ps1.log" -Append -Force

Function RemoveRegistryKey {
    Write-Host "Remove regisrty key: $RegKeyPath, Name: $RegKeyName"
    Remove-ItemProperty -Path $RegKeyPath -Name $RegKeyName
}

Function AddRegistryKey {
    Write-Host "Add regisrty key: $RegKeyPath, Name: $RegKeyName"
    try {
        Set-ItemProperty -Path $RegKeyPath -Name $RegKeyName `
                        -Value "cmd /c powershell $ScriptPath $PhaseIndicator $ArgumentList" `
                        -Force -ErrorAction Stop
    }
    catch {
        throw "Unable to add regisety key. Error happened: $_.Exception.Message"
    }
}

# Remove or add registry key
switch ($ControlRegistryKey) {
    Add { AddRegistryKey }
    Remove { RemoveRegistryKey } 
}

if ($AutoRestart) {
    Write-Host "The computer is going to restart..." -ForegroundColor Yellow
    shutdown -r -t 10 -f
}

Stop-Transcript