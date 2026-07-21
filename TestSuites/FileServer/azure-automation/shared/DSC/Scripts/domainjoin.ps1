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
	[Diagnostics.CodeAnalysis.SuppressMessageAttribute('PSAvoidUsingUsernameAndPasswordParams', '',
		Justification = 'Credentials are written to registry for autologon; separate string values required.')]
	Param ($username, $domain, $password, $count)

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

	# Skip if already joined to the target domain (avoids wasting time on re-runs)
	$currentDomain = (Get-CimInstance Win32_ComputerSystem).Domain
	if ($currentDomain -eq $exchserver.Value.domain) {
		.\Write-Info.ps1 "Already joined to domain $currentDomain. Skipping domain join." -ForegroundColor Green
		return $true
	}

	#Start DNS Client Service, if not started.
	If ((Get-Service "DNS Client").status -eq "Stopped") { Start-Service "dns client" }

	# Build a Credential Object to use with the domain join
	$pass = New-Object SecureString
	$config.Core.Password.ToCharArray() | ForEach-Object {$pass.AppendChar($_)}

	$user = $exchserver.Value.domain + "\" + $config.Core.Username
	$cred = New-Object System.Management.Automation.PsCredential($user, $pass)

	# Random stagger (0-30s) to reduce simultaneous Add-Computer calls from Driver+SUT
	$staggerSec = Get-Random -Minimum 0 -Maximum 30
	.\Write-Info.ps1 "Staggering domain join start by ${staggerSec}s to avoid contention..." -ForegroundColor Green
	Start-Sleep -Seconds $staggerSec

	# Probe what a join actually needs: a discoverable/advertising DC (nltest /dsgetdc).
	# ICMP (Test-Connection) is an unreliable proxy on Azure and never proves advertising.
	# nltest writes to STDERR (e.g. "ERROR_NO_SUCH_DOMAIN") while the DC is still promoting,
	# which under the bootstrap's $ErrorActionPreference='Stop' would surface as a TERMINATING
	# error and abort the retry loop on the first attempt. Neutralize EAP around the native
	# call and decide purely on $LASTEXITCODE; all probe output is suppressed so stray pipeline
	# text cannot make a failed join look truthy to the caller.
	$domain = $exchserver.Value.domain
	ipconfig /flushdns | Out-Null
	sleep_progress 10
	$domainReachable = $false
	$maxDnsRetries = 20
	$dnsBackoff = 10
	for ($i = 0; $i -lt $maxDnsRetries; $i++) {
		$dcOk = $false
		$prevEap = $ErrorActionPreference
		try {
			$ErrorActionPreference = 'SilentlyContinue'
			& nltest "/dsgetdc:$domain" 2>$null 1>$null
			$dcOk = ($LASTEXITCODE -eq 0)
		} catch {
			$dcOk = $false
		} finally {
			$ErrorActionPreference = $prevEap
		}
		if ($dcOk) {
			.\Write-Info.ps1 "Domain controller for $domain is discoverable and advertising." -ForegroundColor Green
			$domainReachable = $true
			break
		}
		.\Write-Info.ps1 "DC not discoverable yet (attempt $($i+1)/$maxDnsRetries). Retrying in ${dnsBackoff}s..." -ForegroundColor Yellow
		sleep_progress $dnsBackoff
		$dnsBackoff = [math]::Min($dnsBackoff * 2, 60)
	}

	if (-not $domainReachable) {
		.\Write-Error.ps1 "Domain $($exchserver.Value.domain) not reachable after $maxDnsRetries attempts."
		return $false
	}

	# Now we are adding the computer to the domain
	.\Write-Info.ps1 "`nJoining Domain" -ForegroundColor Green

	$joined = $false
	$maxJoinRetries = 20
	$joinBackoff = 10
	for ($i = 0; $i -lt $maxJoinRetries; $i++) {
		try {
			Add-computer -domainname $exchserver.Value.domain -credential $cred -ErrorAction Stop
			.\Write-Info.ps1 "Domain join succeeded." -ForegroundColor Green
			$joined = $true
			break
		}
		catch {
			.\Write-Info.ps1 "Domain join attempt $($i+1)/${maxJoinRetries} failed: $($_.Exception.Message). Retrying in ${joinBackoff}s..." -ForegroundColor Yellow
			sleep_progress $joinBackoff
			$joinBackoff = [math]::Min($joinBackoff * 2, 60)
		}
	}

	if (-not $joined) {
		.\Write-Error.ps1 "Failed to join domain after $maxJoinRetries attempts."
		return $false
	}

	return $true
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

	# Determine if we need to join a domain
	if (($null -ne $exchserver.Value.domain) -and ($exchserver.Value.domain -ne 'Workgroup')) {
		# Setup AutoLogon
		.\Write-Info.ps1 "Setting AutoLogon information" -ForegroundColor Green

		AutoLogin $config.Core.Username $exchserver.Value.Domain $config.Core.Password 999
		$joinResult = DomainJoin
		if (-not $joinResult) {
			return $false
		}

	}
	# If not setup autologon for non domain logon
	else {
		# Setup AutoLogon
		.\Write-Info.ps1 "Setting AutoLogon information" -ForegroundColor Green
		AutoLogin $config.Core.Username $exchserver.Value.ComputerName $config.Core.Password 999
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
$name = (Get-CimInstance Win32_ComputerSystem).Name
$exchserver = $config.Machines.PSObject.Properties | Where-Object { $_.name -match $name -or $_.Value.ComputerName -match $name }

if ($null -eq $exchserver) {
	.\Write-Error.ps1 "Failed to find server in config file."
	return $false
}

return Phase0
