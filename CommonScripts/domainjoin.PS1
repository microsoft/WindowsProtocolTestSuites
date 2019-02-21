#############################################################################
## Copyright (c) Microsoft Corporation. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
#############################################################################
# Script to joindomain
# Install prereq -> Reboots -> Copies and joindomain

Param (
	$phase=0,
	[string]$install,
	$LClient,
	[switch]$noelevate,
	# The name of the XML file, indicating which environment you want to configure
	[string]$EnvXmlName = "protocol.xml"
)

$Path = Split-Path -Parent $MyInvocation.MyCommand.Definition
# Function to setup for automatic login to the machine
Function AutoLogin {
	Param ($username,$domain,$password,$count)
	
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
	If ((Get-Service "DNS Client").status -eq "Stopped"){ Start-Service "dns client" }
	
	# Build a Credential Object to use with the domain join
	$pass = ConvertTo-SecureString $setup.lab.core.password -AsPlainText -force
	$user = $exchserver.domain + "\" + $setup.lab.core.username
	$cred = New-Object System.Management.Automation.PsCredential($user,$pass)
	
	# Check if domain is ready
        ipconfig /flushdns
	sleep_progress 30
	for($i=0;$i -lt 120;$i++)
	{
		try
		{
			Test-Connection -ComputerName $exchserver.domain -ErrorAction Stop
			break
		}
		catch
		{
			WriteInfo "Wait 30 seconds then retry."
			sleep_progress 30
		}
	}
	
	# Now we are adding the computer to the domain

	WriteInfo "`nJoining Domain"

	for($i=0;$i -lt 120;$i++)
	{
		try
		{
			Add-computer -domainname $exchserver.domain -credential $cred -ErrorAction Stop
			break
		}
		catch
		{
			WriteInfo "Wait 30 seconds then retry to join domain again."
			sleep_progress 30
		}
	}
}

# Reboot the computer
Function RebootComputer {
	Param ($time)
	
	shutdown /r /f /t $time /d P:2:4
	
	WriteInfo "`n"
	WriteInfo "Reboot started"
	WriteInfo "To abort reboot type `"Shutdown /a`" "
	
	# Stop Logging
	Stop-Transcript
	
	Exit
}

# Input the amount of time you want to sleep
Function Sleep_Progress {
	Param($sleeptime)

	# Loop Number of seconds you want to sleep
	For ($i=0;$i -le $sleeptime;$i++){
		$timeleft = ($sleeptime - $i);
		# Progress bar showing progress of the sleep
		Write-Progress "Sleeping" "$Timeleft More Seconds" -PercentComplete (($i/$sleeptime)*100);
		If ($i -lt $sleeptime){sleep 1}
	}
}


# Function to Control Writing Information to the screen
Function WriteInfo {
	Param([string]$string)

	# Write Our Output
	Write-Host $string "`r" -ForegroundColor Green
}

# Function to control all actions in phase 1
Function Phase0 {

	WriteInfo "Entering Phase0"
	
	# Determine if we need to join a domain
	if (($exchserver.domain -ne $null) -and ($exchserver.domain -ne 'Workgroup')){
		# Setup AutoLogon
		WriteInfo "Setting AutoLogon information"
		AutoLogin $setup.lab.core.username $exchserver.domain $setup.lab.core.password 999
		DomainJoin
		
	}
	# If not setup autologon for non domain logon
	else {
		# Setup AutoLogon
		WriteInfo "Setting AutoLogon information"
		AutoLogin $setup.lab.core.username $exchserver.name $setup.lab.core.password 999
	}
}

# Main Body of Script
# ===================================================

# Start Logging
Start-Transcript -Path $Path\DomainJoinInstall.log -Append -Force

# Get the XML file
[xml]$setup = Get-Content $Path\$EnvXmlName

# Determine our Server
$name = (Get-WmiObject Win32_computerSystem).Name
$exchserver = $setup.lab.servers.vm | where {$_.name -eq $name}

Switch ($phase){
	0 {Phase0}
}