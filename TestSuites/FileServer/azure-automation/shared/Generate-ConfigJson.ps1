# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

# Generate-ConfigJson.ps1
# Generates Config.json for Azure File Server Test Suite deployments
# Supports both Domain and Cluster scenarios

[Diagnostics.CodeAnalysis.SuppressMessageAttribute('PSAvoidUsingPlainTextForPassword', 'AdminPassword',
    Justification = 'Accepts already-decrypted password from ConvertFrom-SecurePassword for Config.json generation')]
[Diagnostics.CodeAnalysis.SuppressMessageAttribute('PSAvoidUsingPlainTextForPassword', 'LocalUserPassword',
    Justification = 'Accepts already-decrypted password for local user account in Config.json')]
[CmdletBinding()]
param(
    [Parameter(Mandatory=$true)]
    [ValidateSet("Domain", "Cluster", "Workgroup")]
    [string]$Scenario,

    [Parameter(Mandatory=$true)]
    [string]$AdminUsername,

    [Parameter(Mandatory=$true)]
    [string]$AdminPassword,

    # Domain/Cluster-only parameters (not required for Workgroup)
    [Parameter(Mandatory=$false)]
    [string]$DomainName = "",

    [Parameter(Mandatory=$false)]
    [string]$DomainNetBiosName = "",

    [Parameter(Mandatory=$false)]
    [string]$DCExternal1Ip = "",

    [Parameter(Mandatory=$false)]
    [string]$DCExternal2Ip = "",

    # Domain-scenario SUT (also used as Node01 in Cluster)
    [Parameter(Mandatory=$false)]
    [string]$SutExternal1Ip = "192.168.1.11",

    [Parameter(Mandatory=$false)]
    [string]$SutExternal2Ip = "192.168.2.11",

    [Parameter(Mandatory=$false)]
    [string]$DriverExternal1Ip = "192.168.1.111",

    [Parameter(Mandatory=$false)]
    [string]$DriverExternal2Ip = "192.168.2.111",

    # Cluster-only parameters
    [Parameter(Mandatory=$false)]
    [string]$StorageExternal1Ip = "192.168.0.1",

    [Parameter(Mandatory=$false)]
    [string]$Node01External1Ip = "192.168.1.11",

    [Parameter(Mandatory=$false)]
    [string]$Node01External2Ip = "192.168.2.11",

    [Parameter(Mandatory=$false)]
    [string]$Node02External1Ip = "192.168.1.12",

    [Parameter(Mandatory=$false)]
    [string]$Node02External2Ip = "192.168.2.12",

    [Parameter(Mandatory=$false)]
    [ValidateNotNullOrEmpty()]
    [string]$ClusterName = "Cluster01",

    [Parameter(Mandatory=$false)]
    [ValidateNotNullOrEmpty()]
    [string]$ScaleOutFSName = "ScaleoutFS",

    # Cluster endpoint IPs
    [Parameter(Mandatory=$false)]
    [string]$ClusterExternal1Ip = "192.168.1.100",

    [Parameter(Mandatory=$false)]
    [string]$ClusterExternal2Ip = "192.168.2.100",

    [Parameter(Mandatory=$false)]
    [string]$GeneralFSExternal1Ip = "192.168.1.200",

    [Parameter(Mandatory=$false)]
    [string]$GeneralFSExternal2Ip = "192.168.2.200",

    # Workgroup-only parameter
    [Parameter(Mandatory=$false)]
    [string]$LocalUserPassword = "",

    # Driver OS type (Windows or Linux)
    [Parameter(Mandatory=$false)]
    [ValidateSet("Windows", "Linux")]
    [string]$DriverOSType = "Windows",

    # When set, marks Config.json so on-VM account creation gives EVERY test account
    # the single admin password (Core.Password). The one-click Deploy-to-Azure button
    # lets the operator pick an arbitrary password and the framework logs in all
    # accounts with PasswordForAllUsers (= that password); without this, ParamConfig's
    # baked per-account passwords would cause secondary-account logon failures. The
    # pipeline/CLI leave this off, so their behavior is unchanged.
    [Parameter(Mandatory=$false)]
    [switch]$UnifyAccountPasswords,

    [Parameter(Mandatory=$true)]
    [string]$OutputPath
)

Write-Output "Generating Config.json for scenario: $Scenario"

# Validate required parameters for Domain/Cluster scenarios
if ($Scenario -in @("Domain", "Cluster")) {
    if (-not $DomainName) { throw "DomainName is required for $Scenario scenario" }
    if (-not $DomainNetBiosName) { throw "DomainNetBiosName is required for $Scenario scenario" }
    if (-not $DCExternal1Ip) { throw "DCExternal1Ip is required for $Scenario scenario" }
    if (-not $DCExternal2Ip) { throw "DCExternal2Ip is required for $Scenario scenario" }
}

if ($Scenario -eq "Cluster") {
    if (-not $ClusterName) { throw "ClusterName is required for Cluster scenario" }
    if (-not $ScaleOutFSName) { throw "ScaleOutFSName is required for Cluster scenario" }
}

# Build the core configuration
if ($Scenario -eq "Workgroup") {
    # Workgroup scenario - no domain
    $config = @{
        Core = @{
            Username       = $AdminUsername
            Password       = $AdminPassword
            TestSuiteName  = "FileServer"
            Scenario       = $Scenario
            DomainName     = ""
            RegressionType = "Azure"
            UseAgent       = "false"
        }
        Machines = @{
            SUT = @{
                Role          = "SUT"
                HyperVName    = "Node01"
                ComputerName  = "Node01"
                IsClusterNode = "false"
                Domain        = "Workgroup"
                IpConfig      = @(
                    @{
                        NIC       = "External1"
                        Ip        = $SutExternal1Ip
                        Subnet    = "255.255.255.0"
                        Gateway   = ""
                        DNSServer = ""
                    },
                    @{
                        NIC       = "External2"
                        Ip        = $SutExternal2Ip
                        Subnet    = "255.255.255.0"
                        Gateway   = ""
                        DNSServer = ""
                    }
                )
            }
            DriverComputer = @{
                Role          = "DriverComputer"
                HyperVName    = "Client01"
                ComputerName  = "Client01"
                IsClusterNode = "false"
                OSType        = $DriverOSType
                Domain        = "Workgroup"
                IpConfig      = @(
                    @{
                        NIC       = "External1"
                        Ip        = $DriverExternal1Ip
                        Subnet    = "255.255.255.0"
                        Gateway   = ""
                        DNSServer = ""
                    },
                    @{
                        NIC       = "External2"
                        Ip        = $DriverExternal2Ip
                        Subnet    = "255.255.255.0"
                        Gateway   = ""
                        DNSServer = ""
                    }
                )
            }
        }
        LocalAccounts = @{
            Administrator = @{
                Username = "Administrator"
                Password = $AdminPassword
            }
            NonAdmin = @{
                Username = "nonadmin"
                Password = $LocalUserPassword
            }
            Guest = @{
                Username = "Guest"
                Password = ""
            }
        }
    }
} else {
    # Domain/Cluster scenarios - include DC
    $config = @{
        Core = @{
            Username       = $AdminUsername
            Password       = $AdminPassword
            TestSuiteName  = "FileServer"
            Scenario       = $Scenario
            DomainName     = $DomainName
            RegressionType = "Azure"
            UseAgent       = "false"
        }
        Machines = @{
            DC = @{
                Role          = "DC"
                HyperVName    = "DC01"
                ComputerName  = "DC01"
                IsClusterNode = "false"
                Domain        = $DomainName
                IpConfig      = @(
                    @{
                    NIC       = "External1"
                    Ip        = $DCExternal1Ip
                    Subnet    = "255.255.255.0"
                    Gateway   = ""
                    DNSServer = "127.0.0.1"
                },
                @{
                    NIC       = "External2"
                    Ip        = $DCExternal2Ip
                    Subnet    = "255.255.255.0"
                    Gateway   = ""
                    DNSServer = "127.0.0.1"
                }
            )
            }
        }
    }
}

# Build scenario-specific machines and endpoints
if ($Scenario -eq "Domain") {
    # Domain scenario: DC + SUT (Node01) + DriverComputer
    $config.Machines.SUT = @{
        Role          = "SUT"
        HyperVName    = "Node01"
        ComputerName  = "Node01"
        IsClusterNode = "false"
        Domain        = $DomainName
        IpConfig      = @(
            @{
                NIC       = "External1"
                Ip        = $SutExternal1Ip
                Subnet    = "255.255.255.0"
                Gateway   = $DCExternal1Ip
                DNSServer = $DCExternal1Ip
            },
            @{
                NIC       = "External2"
                Ip        = $SutExternal2Ip
                Subnet    = "255.255.255.0"
                Gateway   = $DCExternal2Ip
                DNSServer = $DCExternal2Ip
            }
        )
    }
    $config.Machines.DriverComputer = @{
        Role          = "DriverComputer"
        HyperVName    = "Client01"
        ComputerName  = "Client01"
        IsClusterNode = "false"
        OSType        = $DriverOSType
        Domain        = $DomainName
        IpConfig      = @(
            @{
                NIC       = "External1"
                Ip        = $DriverExternal1Ip
                Subnet    = "255.255.255.0"
                Gateway   = $DCExternal1Ip
                DNSServer = $DCExternal1Ip
            },
            @{
                NIC       = "External2"
                Ip        = $DriverExternal2Ip
                Subnet    = "255.255.255.0"
                Gateway   = $DCExternal2Ip
                DNSServer = $DCExternal2Ip
            }
        )
    }
    $config.Domain = @{
        Name        = $DomainName
        NetBiosName = $DomainNetBiosName
    }
}
elseif ($Scenario -eq "Cluster") {
    # Cluster scenario: DC + Storage + Node01 + Node02 + DriverComputer + Endpoints
    $config.Machines.Storage = @{
        Role            = "Storage"
        HyperVName      = "Storage01"
        ComputerName    = "Storage01"
        IsClusterNode   = "false"
        Domain          = "Workgroup"
        iSCSITarget     = $true
        iSCSITargetName = "ClusterTarget"
        IpConfig        = @(
            @{
                NIC       = "External1"
                Ip        = $StorageExternal1Ip
                Subnet    = "255.255.255.0"
                Gateway   = ""
                DNSServer = ""
            }
        )
    }
    $config.Machines.Node01 = @{
        Role          = "Node01"
        HyperVName    = "Node01"
        ComputerName  = "Node01"
        IsClusterNode = "true"
        Domain        = $DomainName
        IpConfig      = @(
            @{
                NIC       = "External1"
                Ip        = $Node01External1Ip
                Subnet    = "255.255.255.0"
                Gateway   = $DCExternal1Ip
                DNSServer = $DCExternal1Ip
            },
            @{
                NIC       = "External2"
                Ip        = $Node01External2Ip
                Subnet    = "255.255.255.0"
                Gateway   = $DCExternal2Ip
                DNSServer = $DCExternal2Ip
            }
        )
    }
    $config.Machines.Node02 = @{
        Role          = "Node02"
        HyperVName    = "Node02"
        ComputerName  = "Node02"
        IsClusterNode = "true"
        Domain        = $DomainName
        IpConfig      = @(
            @{
                NIC       = "External1"
                Ip        = $Node02External1Ip
                Subnet    = "255.255.255.0"
                Gateway   = $DCExternal1Ip
                DNSServer = $DCExternal1Ip
            },
            @{
                NIC       = "External2"
                Ip        = $Node02External2Ip
                Subnet    = "255.255.255.0"
                Gateway   = $DCExternal2Ip
                DNSServer = $DCExternal2Ip
            }
        )
    }
    $config.Machines.DriverComputer = @{
        Role          = "DriverComputer"
        HyperVName    = "Driver"
        ComputerName  = "Client01"
        IsClusterNode = "false"
        OSType        = $DriverOSType
        Domain        = $DomainName
        IpConfig      = @(
            @{
                NIC       = "External1"
                Ip        = $DriverExternal1Ip
                Subnet    = "255.255.255.0"
                Gateway   = $DCExternal1Ip
                DNSServer = $DCExternal1Ip
            },
            @{
                NIC       = "External2"
                Ip        = $DriverExternal2Ip
                Subnet    = "255.255.255.0"
                Gateway   = $DCExternal2Ip
                DNSServer = $DCExternal2Ip
            }
        )
    }
    $config.Endpoints = @{
        Cluster = @{
            Name     = $ClusterName
            IpConfig = @(
                @{
                    NIC = "External1"
                    Ip  = $ClusterExternal1Ip
                },
                @{
                    NIC = "External2"
                    Ip  = $ClusterExternal2Ip
                }
            )
        }
        GeneralFS = @{
            Name     = "GeneralFS"
            IpConfig = @(
                @{
                    NIC = "External1"
                    Ip  = $GeneralFSExternal1Ip
                },
                @{
                    NIC = "External2"
                    Ip  = $GeneralFSExternal2Ip
                }
            )
        }
        InfrastructureFS = @{
            Name = "InfraFS"
        }
        ScaleoutFS = @{
            Name = $ScaleOutFSName
        }
    }
    $config.Domain = @{
        Name        = $DomainName
        NetBiosName = $DomainNetBiosName
    }
}

# Convert to JSON and save
# Opt-in flag for the one-click button path: create all test accounts with the
# single admin password (see -UnifyAccountPasswords). Off by default.
if ($UnifyAccountPasswords) {
    $config.Core.UsePasswordForAllUsers = "true"
}

$jsonContent = $config | ConvertTo-Json -Depth 10
$jsonContent | Out-File -FilePath $OutputPath -Encoding UTF8 -Force

Write-Output "Config.json generated successfully at: $OutputPath"
Write-Output "Scenario: $Scenario"
