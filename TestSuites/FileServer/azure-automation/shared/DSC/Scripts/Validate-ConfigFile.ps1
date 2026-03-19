# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

param(
    [Parameter(Mandatory = $true)]
    [string]$ConfigPath
)

function Test-IPv4Address {
    param([string]$IP)
    
    if ([string]::IsNullOrWhiteSpace($IP)) {
        return $false
    }
    
    try {
        [ipaddress]$IP
        return $true
    }
    catch {
        return $false
    }
}

function Test-ConfigFile {
    param(
        [Parameter(Mandatory = $true)]
        [string]$ConfigPath
    )
    
    $errors = @()
    
    # Read and parse the JSON file
    try {
        $config = Get-Content -Path $ConfigPath -Raw | ConvertFrom-Json
    }
    catch {
        Write-Host "Failed to parse config file: $_"  -ForegroundColor Red
        return $false
    }
    
    # Validate Core section
    # TODO: Make more dynamic
    foreach ($property in @('Username', 'Password', 'TestSuiteName', 'Scenario', 'RegressionType', 'UseAgent')) {
        if ([string]::IsNullOrWhiteSpace($config.Core.$property)) {
            $errors += "Core.$property is empty"
        }
    }
    
    # Function to validate computer section
    function Test-ComputerSection {
        param($Section, $SectionName, [ref]$Errors)

        if ($null -eq $Section) {
            $Errors.Value += "$SectionName is missing"
            return
        }
        
        if ([string]::IsNullOrWhiteSpace($Section.HyperVName)) {
            $Errors.Value += "$SectionName.HyperVName is empty"
        }
        if ([string]::IsNullOrWhiteSpace($Section.ComputerName)) {
            $Errors.Value += "$SectionName.ComputerName is empty"
        }
        
        # Validate IP configurations
        foreach ($index in 0..($Section.IpConfig.Count - 1)) {
            $ipConfig = $Section.IpConfig[$index]
            
            if (-not (Test-IPv4Address $ipConfig.Ip)) {
                $Errors.Value += "$SectionName.IpConfig[$index].Ip is not a valid IPv4 address: '$($ipConfig.Ip)'"
            }
        }
    }
    
    # Validate SUT (or Node01 for Cluster) and DriverComputer sections (always required)
    $scenario = $config.Core.Scenario
    if ($scenario -eq "Cluster") {
        Test-ComputerSection -Section $config.Machines.Node01 -SectionName "Node01" -Errors ([ref]$errors)
    } else {
        Test-ComputerSection -Section $config.Machines.SUT -SectionName "SUT" -Errors ([ref]$errors)
    }
    Test-ComputerSection -Section $config.Machines.DriverComputer -SectionName "DriverComputer" -Errors ([ref]$errors)

    # Scenario-aware validation: DC required for Domain/Cluster, Node02/Storage for Cluster
    if ($scenario -in @("Domain", "Cluster")) {
        Test-ComputerSection -Section $config.Machines.DC -SectionName "DC" -Errors ([ref]$errors)
    }
    if ($scenario -eq "Cluster") {
        Test-ComputerSection -Section $config.Machines.Node02 -SectionName "Node02" -Errors ([ref]$errors)
        Test-ComputerSection -Section $config.Machines.Storage -SectionName "Storage" -Errors ([ref]$errors)
    }
    
    # Output results
    if ($errors.Count -gt 0) {
        Write-Host "Configuration validation failed with the following errors:" -ForegroundColor Red
        $errors | ForEach-Object { Write-Host "- $_" -ForegroundColor Red }
        return $false
    }
    else {
        Write-Host "Configuration validation successful!" -ForegroundColor Green
        return $true
    }
}

# Execute the validation with the provided path
if (!(Test-Path $ConfigPath)) {
    Write-Host "Configuration file not found at: $ConfigPath" -ForegroundColor Red
    exit 1
}

Test-ConfigFile -ConfigPath $ConfigPath