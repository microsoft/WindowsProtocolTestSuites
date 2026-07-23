# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

$here = Split-Path -Parent $MyInvocation.MyCommand.Path
$script = Join-Path $here '..\shared\DSC\Scripts\Modify-ConfigFileNode.ps1'
$sentinel = 'S3cr3t-SENTINEL-Passw0rd!'

function New-TempPtfConfig {
    $path = Join-Path ([System.IO.Path]::GetTempPath()) ("ptfcfg-" + [System.Guid]::NewGuid().ToString('N') + '.ptfconfig')
    @'
<?xml version="1.0" encoding="utf-8"?>
<TestSite xmlns="http://schemas.microsoft.com/windows/2004/02/mit/ptf">
  <Properties>
    <Property name="PasswordForAllUsers" value="old" />
    <Property name="SutComputerName" value="old" />
  </Properties>
</TestSite>
'@ | Set-Content -Path $path -Encoding UTF8
    return $path
}

Describe 'Modify-ConfigFileNode.ps1 secret redaction' {
    It 'writes a Secret value without printing it' {
        $cfg = New-TempPtfConfig
        try {
            $out = & $script -sourceFileName $cfg -nodeName 'PasswordForAllUsers' `
                -newContent $sentinel -ValueClassification Secret *>&1 | Out-String
            $out.Contains($sentinel) | Should Be $false
            $out.Contains('<redacted>') | Should Be $true
            ([xml](Get-Content $cfg)).GetElementsByTagName('Property') |
                Where-Object { $_.name -eq 'PasswordForAllUsers' } |
                ForEach-Object { $_.value } | Should Be $sentinel
        }
        finally {
            Remove-Item $cfg -Force -ErrorAction SilentlyContinue
        }
    }

    It 'prints a Public value for diagnostics' {
        $cfg = New-TempPtfConfig
        try {
            $out = & $script -sourceFileName $cfg -nodeName 'SutComputerName' `
                -newContent '10.1.2.3' -ValueClassification Public *>&1 | Out-String
            $out.Contains('10.1.2.3') | Should Be $true
        }
        finally {
            Remove-Item $cfg -Force -ErrorAction SilentlyContinue
        }
    }

    It 'defaults to Public for legacy positional calls' {
        $cfg = New-TempPtfConfig
        try {
            $out = & $script $cfg 'SutComputerName' '10.9.9.9' *>&1 | Out-String
            $out.Contains('10.9.9.9') | Should Be $true
        }
        finally {
            Remove-Item $cfg -Force -ErrorAction SilentlyContinue
        }
    }

    It 'does not leak a Secret value in a missing-node error' {
        $cfg = New-TempPtfConfig
        try {
            $err = $null
            try {
                & $script -sourceFileName $cfg -nodeName 'NoSuchNode' `
                    -newContent $sentinel -ValueClassification Secret *>&1 | Out-Null
            }
            catch {
                $err = $_.Exception.Message
            }
            $err | Should Not BeNullOrEmpty
            $err.Contains($sentinel) | Should Be $false
        }
        finally {
            Remove-Item $cfg -Force -ErrorAction SilentlyContinue
        }
    }
}
