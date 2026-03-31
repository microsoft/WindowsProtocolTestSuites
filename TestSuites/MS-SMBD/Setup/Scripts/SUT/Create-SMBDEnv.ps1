# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

#----------------------------------------------------------------------------
# Configuration Variables
#----------------------------------------------------------------------------
$Name         = "SMBDTest"
$FullAccess   = "BUILTIN\Administrators"
$Everyone     = "Everyone"
$Path         = "C:\SMBDTest"
$CachingMode  = "None"
$EncryptData  = $false
$CompressData = $false

# File creation details
$FileName     = "testFile_ReadLargeFile.txt"
$FileSizeKB   = 8192  # 8192 KB = 8 MB

#----------------------------------------------------------------------------
# 1. Create the Directory & Set NTFS Permissions
#----------------------------------------------------------------------------
if (-not (Test-Path -Path $Path)) {
    Write-Host "Creating folder: $Path" -ForegroundColor Cyan
    New-Item -Path $Path -ItemType Directory -Force | Out-Null
} else {
    Write-Host "Folder already exists: $Path" -ForegroundColor Yellow
}

Write-Host "Setting NTFS permissions for Everyone..." -ForegroundColor Cyan
$Acl = Get-Acl $Path
$Ar = New-Object System.Security.AccessControl.FileSystemAccessRule($Everyone, "Modify", "ContainerInherit,ObjectInherit", "None", "Allow")
$Acl.SetAccessRule($Ar)
Set-Acl $Path $Acl

#----------------------------------------------------------------------------
# 2. Create the SMB Share
#----------------------------------------------------------------------------
$smbShare = Get-SmbShare -Name $Name -ErrorAction SilentlyContinue

if ($null -eq $smbShare) {
    Write-Host "Creating SMB Share: $Name" -ForegroundColor Cyan

    New-SmbShare -Name $Name `
                 -Path $Path `
                 -FullAccess $FullAccess `
                 -ChangeAccess $Everyone `
                 -CachingMode $CachingMode `
                 -EncryptData $EncryptData `
                 -ErrorAction Stop

    if ($CompressData) {
        Set-SmbShare -Name $Name -CompressData $true -Force
    }
} else {
    Write-Host "SMB Share '$Name' already exists." -ForegroundColor Yellow
    Grant-SmbShareAccess -Name $Name -AccountName $Everyone -AccessRight Change -Force
}

#----------------------------------------------------------------------------
# 3. Create the Large Test File (8192KB)
#----------------------------------------------------------------------------
$FilePath = Join-Path -Path $Path -ChildPath $FileName
$SizeInBytes = $FileSizeKB * 1KB

if (-not (Test-Path -Path $FilePath)) {
    Write-Host "Creating file: $FilePath ($FileSizeKB KB)" -ForegroundColor Cyan
    fsutil file createnew "$FilePath" $SizeInBytes
} else {
    Write-Host "File '$FileName' already exists. Skipping creation." -ForegroundColor Yellow
}

Set-NetFirewallProfile -Profile Domain,Public,Private -Enabled False
Write-Host "Script completed successfully." -ForegroundColor Green
