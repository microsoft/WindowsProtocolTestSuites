# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

# Check or enable .NET Framework 3.5

param (
	[ValidateSet("Check", "Install")]
	[string]$Action
)

Function Check-NetFx3 {

	Write-Host "Checking .NET Framework 3.5"

	$result = get-childitem -path "HKLM:\SOFTWARE\Microsoft\NET Framework Setup\NDP" | Where-Object -FilterScript {$_.name -match "v3.5"}   

	if($result -ne $null) {
		Write-Host ".NET Framework 3.5 is already enabled." 
		return $true;
	}
	else {
		return $false
	}
}

Function Enable-NetFx3 {

	Write-Host ".NET Framework 3.5 is not enabled. Enabling this feature now."

	try{
		Add-WindowsCapability -Online -Name NetFx3~~~~
		Write-Host ".NET Framework 3.5 is enabled." -ForegroundColor Green
		return $true
	}
	catch
	{
		Write-Host "Failed to enable .Net Framework 3.5: $_"  -ForegroundColor Red
		return $false
	}
}

switch($Action) {
	"Check" {
		$isInstalled = Check-NetFx3
		return $isInstalled
	}

	"Install" {
		$result = Enable-NetFx3
		return $result
	}
}
