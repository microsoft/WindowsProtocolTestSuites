# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

param (
    [ValidateSet("Check", "Install")]
    [string]$Action,
    [string]$DownloadedArtifact	# Path to the downloaded SSH file
)

$mydir = Split-Path $MyInvocation.MyCommand.Path -Parent

Function InstallSSH {
    $mydir = "$mydir\Win32OpenSSH"
    # Unzip file
    Write-Host "Expand files into folder $mydir"
    Expand-Archive $DownloadedArtifact -DestinationPath $mydir -force
    $mydir = "$mydir\OpenSSH-Win64"
    # Add C:\OpenSSH-Win64 into 'PATH' environment variable
    $INCLUDE = "$mydir"
    $OLDPATH = [System.Environment]::GetEnvironmentVariable('PATH', 'machine')
    $NEWPATH = "$OLDPATH;$INCLUDE"
    [Environment]::SetEnvironmentVariable("PATH", "$NEWPATH", "Machine")

    # Install sshd
    & "$mydir\install-sshd.ps1"

    # Fixes user file permisions
    & "$mydir\FixUserFilePermissions.ps1"

    # Fixes host keys permissions
    & "$mydir\FixHostFilePermissions.ps1"

    # Start sshd service
    Start-Service sshd

    # OPTIONAL but recommended:
    Set-Service -Name sshd -StartupType 'Automatic'

    # Confirm the Firewall rule is configured. It should be created automatically by setup.
    Get-NetFirewallRule -Name *ssh*

    # There should be a firewall rule named "OpenSSH-Server-In-TCP", which should be enabled
    # If the firewall does not exist, create one
    New-NetFirewallRule -Name sshd -DisplayName 'OpenSSH Server (sshd)' -Enabled True -Direction Inbound -Protocol TCP -Action Allow -LocalPort 22

    # Set OpenSSH as powershell default shell
    New-ItemProperty -Path "HKLM:\SOFTWARE\OpenSSH" -Name DefaultShell -Value "C:\Windows\System32\WindowsPowerShell\v1.0\powershell.exe" -PropertyType String -Force

    # Restart sshd service
    Restart-Service sshd
}

Function CheckSSH {
    try { 
        & "$mydir\Win32OpenSSH\OpenSSH-Win64\ssh-agent"
        return $true
    }
    catch { 
        return $false
    }
}

switch ($Action) {
    "Check" {
        $isInstalled = CheckSSH
        return $isInstalled
    }

    "Install" {
        $result = InstallSSH
        return $result
    }
}