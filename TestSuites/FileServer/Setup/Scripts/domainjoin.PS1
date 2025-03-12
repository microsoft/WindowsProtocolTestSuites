# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

# Script to joindomain
# Install prereq -> Reboots -> Copies and joindomain

Param (
	$phase = 0,
	[string]$install,
	$LClient,
	[switch]$noelevate,
	[string]$protocolConfigFile = "$PSScriptRoot\Config.json"
)

$Path = Split-Path -Parent $MyInvocation.MyCommand.Definition
# Function to setup for automatic login to the machine
Function AutoLogin {
	Param ($username, $domain, [SecureString] $password, $count)
	
	# Setup Autologon on
	Set-ItemProperty -Path "HKLM:\Software\Microsoft\Windows NT\CurrentVersion\Winlogon" -Name AutoAdminLogon -Value 1
	
	# Set Domain name
	Set-ItemProperty -Path "HKLM:\Software\Microsoft\Windows NT\CurrentVersion\Winlogon" -Name DefaultDomainName -Value $domain
	
	# Set User Name
	Set-ItemProperty -Path "HKLM:\Software\Microsoft\Windows NT\CurrentVersion\Winlogon" -Name DefaultUserName -Value $username
	
	# Set Password
	Set-ItemProperty -Path "HKLM:\Software\Microsoft\Windows NT\CurrentVersion\Winlogon" -Name DefaultPassword -Value $password
	
	# Set Logon Count
	Set-ItemProperty -Path "HKLM:\Software\Microsoft\Windows NT\CurrentVersion\Winlogon" -Name AutologonCount -Value $count
}


# Join the domain
Function DomainJoin {
	
	#Start DNS Client Service, if not started.
	If ((Get-Service "DNS Client").status -eq "Stopped") { Start-Service "dns client" }
	
	# Build a Credential Object to use with the domain join
	$pass = New-Object SecureString
	$config.Core.Password.ToCharArray() | ForEach-Object {$pass.AppendChar($_)}
	
	$user = $exchserver.Value.domain + "\" + $config.Core.Username
	$cred = New-Object System.Management.Automation.PsCredential($user, $pass)
	
	# Check if domain is ready
	ipconfig /flushdns
	sleep_progress 30
	for ($i = 0; $i -lt 120; $i++) {
		try {
			Test-Connection -ComputerName $exchserver.Value.domain -ErrorAction Stop
			break
		}
		catch {
			.\Write-Info.ps1 "Wait 30 seconds then retry." -ForegroundColor Green
			sleep_progress 30
		}
	}
	
	# Now we are adding the computer to the domain

	.\Write-Info.ps1 "`nJoining Domain" -ForegroundColor Green

	for ($i = 0; $i -lt 120; $i++) {
		try {
			Add-computer -domainname $exchserver.Value.domain -credential $cred -ErrorAction Stop
			break
		}
		catch {
			.\Write-Info.ps1 "Wait 30 seconds then retry to join domain again." -ForegroundColor Green
			sleep_progress 30
		}
	}
}

# Input the amount of time you want to sleep
Function Sleep_Progress {
	Param($sleeptime)

	# Loop Number of seconds you want to sleep
	For ($i = 0; $i -le $sleeptime; $i++) {
		$timeleft = ($sleeptime - $i);
		# Progress bar showing progress of the sleep
		Write-Progress "Sleeping" "$Timeleft More Seconds" -PercentComplete (($i / $sleeptime) * 100);
		If ($i -lt $sleeptime) { Start-Sleep 1 }
	}
}


# Function to control all actions in phase 1
Function Phase0 {

	.\Write-Info.ps1 "Entering Phase0" -ForegroundColor Green
	$exchserverpwd = New-Object SecureString
	$config.Core.Password.ToCharArray() | ForEach-Object { $exchserverpwd.AppendChar($_) }
	
	# Determine if we need to join a domain
	if (($null -ne $exchserver.Value.domain) -and ($exchserver.Value.domain -ne 'Workgroup')) {
		# Setup AutoLogon
		.\Write-Info.ps1 "Setting AutoLogon information" -ForegroundColor Green

		AutoLogin $config.Core.Username $exchserver.Value.Domain $exchserverpwd 999
		DomainJoin
		
	}
	# If not setup autologon for non domain logon
	else {
		# Setup AutoLogon
		.\Write-Info.ps1 "Setting AutoLogon information" -ForegroundColor Green
		AutoLogin $config.Core.Username $exchserver.Value.ComputerName $exchserverpwd 999
	}
	
	return $true
}

# Main Body of Script
# ===================================================

# Start Logging
Start-Transcript -Path $Path\DomainJoinInstall.log -Append -Force

$config = $null
try {
	$config = Get-Content -Path $protocolConfigFile -Raw | ConvertFrom-Json
}
catch {
	.\Write-Error.ps1 "Failed to parse config file: $_"
	return $false
}

# Determine our Server
$name = (Get-WmiObject Win32_computerSystem).Name
$exchserver = $config.Machines.PSObject.Properties | Where-Object { $_.name -match $name -or $_.Value.ComputerName -match $name }

if ($null -eq $exchserver) {
	.\Write-Error.ps1 "Failed to find server in config file."
	return $false
}

return Phase0