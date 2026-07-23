# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

<#
.SYNOPSIS
    Publishes the Workgroup DSC package + template for the one-click "Deploy to
    Azure" button. Thin wrapper over the shared publisher (../shared/Publish-DscPackage.ps1).

.EXAMPLE
    gh auth login
    ./Publish-DscPackage.ps1 -Tag fileserver-workgroup-deploy-button-v1

.EXAMPLE
    ./Publish-DscPackage.ps1 -SkipUpload -OutputZipPath .\Workgroup-Package.zip
#>
[CmdletBinding(SupportsShouldProcess = $true)]
[Diagnostics.CodeAnalysis.SuppressMessageAttribute('PSAvoidUsingPlainTextForPassword', 'PasswordToken',
    Justification = 'Not a credential -- a placeholder token baked into the public Config.json and replaced on-VM')]
param(
    [Parameter(Mandatory = $false)]
    [string]$Repo = "microsoft/WindowsProtocolTestSuites",

    [Parameter(Mandatory = $false)]
    [string]$Tag = "fileserver-workgroup-deploy-button-v1",

    [Parameter(Mandatory = $false)]
    [string]$Target = "",

    [Parameter(Mandatory = $false)]
    [string]$TemplateRepoPath = "TestSuites/FileServer/azure-automation/workgroup-bicep/azuredeploy.json",

    [Parameter(Mandatory = $false)]
    [string]$AssetName = "Workgroup-Package.zip",

    [Parameter(Mandatory = $false)]
    [string]$AdminUsername = "testadmin",

    [Parameter(Mandatory = $false)]
    [string]$PasswordToken = "#{ADMIN_PASSWORD}#",

    # Default IP topology -- MUST match the main.bicep parameter defaults.
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

# Workgroup Config.json parameters (password fields carry the placeholder token;
# the single admin password is reused for the local NonAdmin account).
$configJsonParams = @{
    Scenario          = 'Workgroup'
    AdminUsername     = $AdminUsername
    AdminPassword     = $PasswordToken
    LocalUserPassword = $PasswordToken
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
    -Scenario 'Workgroup' `
    -ConfigJsonParams $configJsonParams `
    -DscFolderPath $DscFolderPath `
    -MainBicepPath (Join-Path $PSScriptRoot 'main.bicep') `
    -AssetName $AssetName `
    -TemplateRepoPath $TemplateRepoPath `
    -PackageUrlParamName 'dscPackageZipUrl' `
    -Repo $Repo -Tag $Tag -Target $Target `
    -PasswordToken $PasswordToken `
    -OutputZipPath $OutputZipPath `
    -SkipUpload:$SkipUpload `
    -WhatIf:$WhatIfPreference
