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
    $script:clusterNodeCount = 0
    
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
    foreach ($property in @('Username', 'Password', 'TestSuiteName', 'Scenario', 'DomainName', 'RegressionType', 'UseAgent')) {
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
        if ([string]::IsNullOrWhiteSpace($Section.Domain)) {
            $Errors.Value += "$SectionName.Domain is empty"
        }

        # Count cluster nodes
        if (-not [string]::IsNullOrWhiteSpace($Section.IsClusterNode) -and $Section.IsClusterNode -eq "true") {
            $script:clusterNodeCount++
        }
        
        # Validate IP configurations
        foreach ($index in 0..($Section.IpConfig.Count - 1)) {
            $ipConfig = $Section.IpConfig[$index]
            
            if (-not (Test-IPv4Address $ipConfig.Ip)) {
                $Errors.Value += "$SectionName.IpConfig[$index].Ip is not a valid IPv4 address: '$($ipConfig.Ip)'"
            }
        }
    }

    function Test-Endpoints {
        param([ref]$Errors)

        if ($null -eq $config.Endpoints) {
            $Errors.Value += "No Endpoints configured"
            return
        }

        $requiredEndpoints = @("Cluster", "GeneralFS", "InfrastructureFS", "ScaleoutFS")

        # Validate required endpoints exist
        foreach ($endpointName in $requiredEndpoints) {
            $endpoint = $config.Endpoints.$endpointName

            if ($null -eq $endpoint) {
                $Errors.Value += "Missing required endpoint: $endpoint"
                return
            }
        
            if ([string]::IsNullOrWhiteSpace($endpoint.Name)) {
                $Errors.Value += "$endpoint.Name is empty"
            }
            
            if ($endpointName -in @("Cluster", "GeneralFS")) {
                if ($null -eq $endpoint.IpConfig) {
                    $Errors.Value += "$endpointName requires an IP Configuration"
                    return
                }

                foreach ($index in 0..($endpoint.IpConfig.Count - 1)) {
                    $ipConfig = $endpoint.IpConfig[$index]

                    if (-not (Test-IPv4Address $ipConfig.Ip)) {
                        $Errors.Value += "$endpoint.IpConfig[$index] is not a valid IPv4 address: '$($ipConfig.Ip)'"
                        return
                    }
                }
            }
        }

    }
    
    # Validate Sut and DriverComputer sections
    Test-ComputerSection -Section $config.Machines.Node01 -SectionName "Node01" -Errors ([ref]$errors)
    Test-ComputerSection -Section $config.Machines.Node02 -SectionName "Node02" -Errors ([ref]$errors)
    Test-ComputerSection -Section $config.Machines.Storage -SectionName "Storage" -Errors ([ref]$errors)
    Test-ComputerSection -Section $config.Machines.DC -SectionName "DC" -Errors ([ref]$errors)
    Test-ComputerSection -Section $config.Machines.DriverComputer -SectionName "DriverComputer" -Errors ([ref]$errors)

    Test-Endpoints -Errors ([ref]$errors)

    # Validate cluster node count
    if ($script:clusterNodeCount -lt 2) {
        $errors += "At least 2 cluster nodes are required, found: $script:clusterNodeCount"
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