# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

<#
.SYNOPSIS
    Publishes the Domain DSC package + template for the one-click "Deploy to Azure"
    button. Thin wrapper over the shared publisher (../shared/Publish-DscPackage.ps1).

.EXAMPLE
    gh auth login
    ./Publish-DscPackage.ps1 -Tag 4.26.8.0

.EXAMPLE
    ./Publish-DscPackage.ps1 -SkipUpload -OutputZipPath .\Domain-Package.zip
#>
[CmdletBinding(SupportsShouldProcess = $true)]
[Diagnostics.CodeAnalysis.SuppressMessageAttribute('PSAvoidUsingPlainTextForPassword', 'PasswordToken',
    Justification = 'Not a credential -- a placeholder token baked into the public Config.json and replaced on-VM')]
param(
    [Parameter(Mandatory = $false)]
    [string]$Repo = "microsoft/WindowsProtocolTestSuites",

    [Parameter(Mandatory = $false)]
    [string]$Tag = "4.26.8.0",

    [Parameter(Mandatory = $false)]
    [string]$Target = "",

    [Parameter(Mandatory = $false)]
    [string]$TemplateRepoPath = "TestSuites/FileServer/azure-automation/domain-bicep/azuredeploy.json",

    [Parameter(Mandatory = $false)]
    [string]$AssetName = "Domain-Package.zip",

    [Parameter(Mandatory = $false)]
    [string]$AdminUsername = "testadmin",

    [Parameter(Mandatory = $false)]
    [string]$PasswordToken = "#{ADMIN_PASSWORD}#",

    # Domain identity -- MUST match the main.bicep parameter defaults.
    [Parameter(Mandatory = $false)]
    [string]$DomainName = "contoso.com",

    [Parameter(Mandatory = $false)]
    [string]$DomainNetBiosName = "CONTOSO",

    # Default IP topology -- MUST match the main.bicep parameter defaults.
    [Parameter(Mandatory = $false)]
    [string]$DCExternal1Ip = "192.168.1.10",

    [Parameter(Mandatory = $false)]
    [string]$DCExternal2Ip = "192.168.2.10",

    [Parameter(Mandatory = $false)]
    [string]$SutExternal1Ip = "192.168.1.11",

    [Parameter(Mandatory = $false)]
    [string]$SutExternal2Ip = "192.168.2.11",

    [Parameter(Mandatory = $false)]
    [string]$DriverExternal1Ip = "192.168.1.111",

    [Parameter(Mandatory = $false)]
    [string]$DriverExternal2Ip = "192.168.2.111",

    [Parameter(Mandatory = $false)]
    [ValidateSet("Windows", "Linux")]
    [string]$DriverOSType = "Windows",

    [Parameter(Mandatory = $false)]
    [string]$DscFolderPath = "DSC",

    [Parameter(Mandatory = $false)]
    [string]$OutputZipPath = "",

    [Parameter(Mandatory = $false)]
    [switch]$SkipUpload
)

$ErrorActionPreference = "Stop"

$DscFolderPath = if ([System.IO.Path]::IsPathRooted($DscFolderPath)) { $DscFolderPath } else { Join-Path $PSScriptRoot $DscFolderPath }

# Domain Config.json parameters (password field carries the placeholder token;
# domain-join uses Core.Username/Password against the DC).
$configJsonParams = @{
    Scenario          = 'Domain'
    AdminUsername     = $AdminUsername
    AdminPassword     = $PasswordToken
    DomainName        = $DomainName
    DomainNetBiosName = $DomainNetBiosName
    DCExternal1Ip     = $DCExternal1Ip
    DCExternal2Ip     = $DCExternal2Ip
    SutExternal1Ip    = $SutExternal1Ip
    SutExternal2Ip    = $SutExternal2Ip
    DriverExternal1Ip = $DriverExternal1Ip
    DriverExternal2Ip = $DriverExternal2Ip
    DriverOSType      = $DriverOSType
    # Button path: give every test account the single (injected) admin password so
    # secondary-account logons match the framework's PasswordForAllUsers.
    UnifyAccountPasswords = $true
}

$shared = Join-Path $PSScriptRoot "..\shared\Publish-DscPackage.ps1"
& $shared `
    -Scenario 'Domain' `
    -ConfigJsonParams $configJsonParams `
    -DscFolderPath $DscFolderPath `
    -MainBicepPath (Join-Path $PSScriptRoot 'main.bicep') `
    -AssetName $AssetName `
    -TemplateRepoPath $TemplateRepoPath `
    -PackageUrlParamName 'domainPackageZipUrl' `
    -Repo $Repo -Tag $Tag -Target $Target `
    -PasswordToken $PasswordToken `
    -OutputZipPath $OutputZipPath `
    -SkipUpload:$SkipUpload `
    -WhatIf:$WhatIfPreference
