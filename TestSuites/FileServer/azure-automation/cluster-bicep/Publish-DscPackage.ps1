# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

<#
.SYNOPSIS
    Publishes the Cluster DSC package + template for the one-click "Deploy to
    Azure" button. Thin wrapper over the shared publisher (../shared/Publish-DscPackage.ps1).

.EXAMPLE
    gh auth login
    ./Publish-DscPackage.ps1 -Tag 4.26.9.0

.EXAMPLE
    ./Publish-DscPackage.ps1 -SkipUpload -OutputZipPath .\Cluster-Package.zip
#>
[CmdletBinding(SupportsShouldProcess = $true)]
[Diagnostics.CodeAnalysis.SuppressMessageAttribute('PSAvoidUsingPlainTextForPassword', 'PasswordToken',
    Justification = 'Not a credential -- a placeholder token baked into the public Config.json and replaced on-VM')]
param(
    [Parameter(Mandatory = $false)]
    [string]$Repo = "microsoft/WindowsProtocolTestSuites",

    [Parameter(Mandatory = $false)]
    [string]$Tag = "4.26.9.0",

    [Parameter(Mandatory = $false)]
    [string]$Target = "",

    [Parameter(Mandatory = $false)]
    [string]$TemplateRepoPath = "TestSuites/FileServer/azure-automation/cluster-bicep/azuredeploy.json",

    [Parameter(Mandatory = $false)]
    [string]$AssetName = "Cluster-Package.zip",

    [Parameter(Mandatory = $false)]
    [string]$AdminUsername = "testadmin",

    [Parameter(Mandatory = $false)]
    [string]$PasswordToken = "#{ADMIN_PASSWORD}#",

    [Parameter(Mandatory = $false)]
    [string]$DomainName = "contoso.com",

    [Parameter(Mandatory = $false)]
    [string]$DomainNetBiosName = "CONTOSO",

    [Parameter(Mandatory = $false)]
    [string]$DCExternal1Ip = "192.168.1.10",

    [Parameter(Mandatory = $false)]
    [string]$DCExternal2Ip = "192.168.2.10",

    [Parameter(Mandatory = $false)]
    [string]$StorageExternal1Ip = "192.168.1.50",

    [Parameter(Mandatory = $false)]
    [string]$Node01External1Ip = "192.168.1.11",

    [Parameter(Mandatory = $false)]
    [string]$Node01External2Ip = "192.168.2.11",

    [Parameter(Mandatory = $false)]
    [string]$Node02External1Ip = "192.168.1.12",

    [Parameter(Mandatory = $false)]
    [string]$Node02External2Ip = "192.168.2.12",

    [Parameter(Mandatory = $false)]
    [string]$DriverExternal1Ip = "192.168.1.111",

    [Parameter(Mandatory = $false)]
    [string]$DriverExternal2Ip = "192.168.2.111",

    [Parameter(Mandatory = $false)]
    [ValidateSet("Windows", "Linux")]
    [string]$DriverOSType = "Windows",

    [Parameter(Mandatory = $false)]
    [string]$ClusterName = "Cluster01",

    [Parameter(Mandatory = $false)]
    [string]$ScaleOutFSName = "ScaleoutFS",

    [Parameter(Mandatory = $false)]
    [string]$ClusterExternal1Ip = "192.168.1.100",

    [Parameter(Mandatory = $false)]
    [string]$ClusterExternal2Ip = "192.168.2.100",

    [Parameter(Mandatory = $false)]
    [string]$GeneralFSExternal1Ip = "192.168.1.200",

    [Parameter(Mandatory = $false)]
    [string]$GeneralFSExternal2Ip = "192.168.2.200",

    [Parameter(Mandatory = $false)]
    [int]$ClusterExternal1ProbePort = 59998,

    [Parameter(Mandatory = $false)]
    [int]$ClusterExternal2ProbePort = 59999,

    [Parameter(Mandatory = $false)]
    [int]$GeneralFSExternal1ProbePort = 60000,

    [Parameter(Mandatory = $false)]
    [int]$GeneralFSExternal2ProbePort = 60001,

    [Parameter(Mandatory = $false)]
    [string]$DscFolderPath = "DSC",

    [Parameter(Mandatory = $false)]
    [string]$OutputZipPath = "",

    [Parameter(Mandatory = $false)]
    [switch]$SkipUpload
)

$ErrorActionPreference = "Stop"

$DscFolderPath = if ([System.IO.Path]::IsPathRooted($DscFolderPath)) {
    $DscFolderPath
}
else {
    Join-Path $PSScriptRoot $DscFolderPath
}

$configJsonParams = @{
    Scenario                    = 'Cluster'
    AdminUsername               = $AdminUsername
    AdminPassword               = $PasswordToken
    DomainName                  = $DomainName
    DomainNetBiosName           = $DomainNetBiosName
    DCExternal1Ip               = $DCExternal1Ip
    DCExternal2Ip               = $DCExternal2Ip
    StorageExternal1Ip          = $StorageExternal1Ip
    Node01External1Ip           = $Node01External1Ip
    Node01External2Ip           = $Node01External2Ip
    Node02External1Ip           = $Node02External1Ip
    Node02External2Ip           = $Node02External2Ip
    DriverExternal1Ip           = $DriverExternal1Ip
    DriverExternal2Ip           = $DriverExternal2Ip
    DriverOSType                = $DriverOSType
    EnableTestAutoRun           = $true
    ClusterName                 = $ClusterName
    ScaleOutFSName              = $ScaleOutFSName
    ClusterExternal1Ip          = $ClusterExternal1Ip
    ClusterExternal2Ip          = $ClusterExternal2Ip
    GeneralFSExternal1Ip        = $GeneralFSExternal1Ip
    GeneralFSExternal2Ip        = $GeneralFSExternal2Ip
    ClusterExternal1ProbePort   = $ClusterExternal1ProbePort
    ClusterExternal2ProbePort   = $ClusterExternal2ProbePort
    GeneralFSExternal1ProbePort = $GeneralFSExternal1ProbePort
    GeneralFSExternal2ProbePort = $GeneralFSExternal2ProbePort
    UnifyAccountPasswords       = $true
}

$shared = Join-Path $PSScriptRoot "..\shared\Publish-DscPackage.ps1"
& $shared `
    -Scenario 'Cluster' `
    -ConfigJsonParams $configJsonParams `
    -DscFolderPath $DscFolderPath `
    -MainBicepPath (Join-Path $PSScriptRoot 'main.bicep') `
    -AssetName $AssetName `
    -TemplateRepoPath $TemplateRepoPath `
    -PackageUrlParamName 'clusterPackageZipUrl' `
    -Repo $Repo -Tag $Tag -Target $Target `
    -PasswordToken $PasswordToken `
    -OutputZipPath $OutputZipPath `
    -SkipUpload:$SkipUpload `
    -WhatIf:$WhatIfPreference
