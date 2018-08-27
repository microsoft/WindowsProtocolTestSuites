#############################################################################
## Copyright (c) Microsoft. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
##
#############################################################################

param([string]$Name,[string]$Path,[string]$FullAccess,[string]$CachingMode="none",[bool]$EncryptData=$false)

#----------------------------------------------------------------------------
# Print exection information
#----------------------------------------------------------------------------
Write-Host "EXECUTING [Create-SMBShare.ps1]..." -foregroundcolor cyan
Write-Host "`$Name        = $Name" 
Write-Host "`$Path        = $Path"
Write-Host "`$FullAccess  = $FullAccess"
Write-Host "`$CachingMode = $CachingMode"
Write-Host "`$EncryptData = $EncryptData"

#----------------------------------------------------------------------------
# Verify required parameters
#----------------------------------------------------------------------------
if ([System.String]::IsNullOrEmpty($Name))
{
    Throw "Parameter `$Name is required."
}
if ([System.String]::IsNullOrEmpty($Path))
{
    Throw "Parameter `$Path is required."
}

if(!(Test-Path $Path))
{
    New-Item -ItemType directory -Path $Path
}

#----------------------------------------------------------------------------
# Check access account
#----------------------------------------------------------------------------
if([System.String]::IsNullOrEmpty($FullAccess))
{
    $FullAccess = "BUILTIN\Administrators"
}

#----------------------------------------------------------------------------
# Grant access
#----------------------------------------------------------------------------
Write-Info.ps1 "Grant access"
CMD /C "icacls $Path /grant $FullAccess`:(OI)(CI)(F)" 2>&1 | Write-Host

#----------------------------------------------------------------------------
# Share the folder
#----------------------------------------------------------------------------
$smbShare = Get-SmbShare | where {$_.Name -eq "$Name" -and $_.Path -eq "$Path"}
if($smbShare -eq $null)
{        
    New-SMBShare -name "$Name" -Path "$Path" -FullAccess "$FullAccess" -CachingMode $CachingMode -EncryptData $EncryptData
}


#----------------------------------------------------------------------------
# Print verification and exit information
#----------------------------------------------------------------------------
Write-Host "EXECUTE [Create-SMBShare.ps1] FINISHED (NOT VERIFIED)." -foregroundcolor yellow
exit 0