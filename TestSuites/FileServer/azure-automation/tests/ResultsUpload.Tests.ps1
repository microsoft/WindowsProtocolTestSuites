# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

$here = Split-Path -Parent $MyInvocation.MyCommand.Path
$root = (Resolve-Path (Join-Path $here '..')).Path
$helpersPath = Join-Path $root 'shared\Deploy-Helpers.psm1'
Import-Module $helpersPath -Force

Describe 'Test results upload SAS handling' {
    InModuleScope Deploy-Helpers {
        It 'inserts the query delimiter when Az returns a bare SAS token' {
            $result = Join-StorageSasUrl -BlobEndpoint 'https://example.blob.core.windows.net/' `
                -ContainerName 'test-results' -SasToken 'sv=1&sp=rwl&sig=fake'

            $result | Should Be 'https://example.blob.core.windows.net/test-results?sv=1&sp=rwl&sig=fake'
        }

        It 'normalizes a SAS token that already includes the query delimiter' {
            $result = Join-StorageSasUrl -BlobEndpoint 'https://example.blob.core.windows.net/' `
                -ContainerName 'test-results' -SasToken '?sv=1&sp=rwl&sig=fake'

            $result | Should Be 'https://example.blob.core.windows.net/test-results?sv=1&sp=rwl&sig=fake'
        }
    }
}

Describe 'Test result upload reporting' {
    $testRun = Get-Content (Join-Path $root 'shared\DSC\Scripts\Invoke-TestRun.ps1') -Raw

    It 'builds blob URLs structurally and reports incomplete uploads honestly' {
        $testRun.Contains('[UriBuilder]$sasUrl') | Should Be $true
        $testRun.Contains('$blobUriBuilder.Path =') | Should Be $true
        $testRun.Contains('test.results.upload.failed.signal') | Should Be $true
        $testRun.Contains('if ($uploaded -eq $filesToUpload.Count)') | Should Be $true
        $testRun.Contains('[OK] Uploaded $uploaded/$($filesToUpload.Count)') | Should Be $true
    }

    It 'reads an active transcript with shared file access' {
        $testRun.Contains('[System.IO.FileShare]::ReadWrite') | Should Be $true
    }

    It 'keeps SMB credentials out of native process command lines' {
        $testRun.Contains('WNetAddConnection2') | Should Be $true
        $testRun.Contains('-A $authFile') | Should Be $true
        $testRun.Contains('& chmod 600 $authFile') | Should Be $true
        $testRun.Contains("Start-Process -FilePath 'net.exe'") | Should Be $false
        $testRun.Contains('%$CredentialPassword') | Should Be $false
        $testRun.Contains('%$adminPassword') | Should Be $false
    }
}

Describe 'Test result upload verification' {
    $verifier = Get-Content (Join-Path $root 'shared\scripts\Verify-Deployment.ps1') -Raw

    It 'fails immediately when the Driver reports incomplete result upload' {
        $verifier.Contains('TEST_UPLOAD_FAILED') | Should Be $true
        $verifier.Contains('$uploadFailureModified -ge $NotBeforeEpoch') | Should Be $true
        $verifier.Contains('Automatic test result upload failed on') | Should Be $true
        $verifier.Contains('if ($_.Exception.Message -like ''Automatic test result upload failed*'')') |
            Should Be $true
    }
}
