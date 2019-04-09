#############################################################################
## Copyright (c) Microsoft Corporation. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
#############################################################################

#----------------------------------------------------------------------------
# Global variables
#----------------------------------------------------------------------------
$scriptPath = Split-Path $MyInvocation.MyCommand.Definition -parent
$env:Path += ";$scriptPath;$scriptPath\Scripts"
$homeDrive = $ENV:HomeDrive

#----------------------------------------------------------------------------
# Start loging using start-transcript cmdlet
#----------------------------------------------------------------------------
[string]$logFile = $MyInvocation.MyCommand.Path + ".log"
try { Stop-Transcript -ErrorAction SilentlyContinue } catch {} # Ignore Stop-Transcript error messages
Start-Transcript -Path "$logFile" -Append -Force

#----------------------------------------------------------------------------
# Define common functions
#----------------------------------------------------------------------------
function CreateShareFolder($fullPath)
{
    if(!(Test-Path $fullPath))
    {
        CMD /C "md $fullPath"
    }
    CMD /C "icacls $fullPath /grant contoso.com\Administrator:(OI)(CI)(F)"
}

function CreateFile($filePath,$fileName,[int]$fileSize)
{
    $fileFullPath = "$filePath\$fileName"
    if(!(Test-Path "$fileFullPath"))
    {
        Write-Info.ps1 "Create file $fileFullPath with size $fileSize"
        $file=new-object system.io.filestream "$fileFullPath",create,readwrite
    	$file.setlength($fileSize)
    	$file.close()
    }
}

#----------------------------------------------------------------------------
# Create FileShare for SMB Transport
#----------------------------------------------------------------------------
Write-Info.ps1 "Create FileShare for SMB Transport."
Import-module SmbShare

$shareName = "FileShare"
$smbSharePath = "$homeDrive\$shareName"
CreateShareFolder "$homeDrive\FileShare" 

$share = Get-SmbShare | where {$_.Name -eq $shareName}
if($share -eq $null)
{
    $retrycount = 0
    $totalRetryTimes = 3
    
    While($retrycount -lt $totalRetryTimes){
        Try{
            New-SMBShare -name "FileShare" -Path "$smbSharePath" -FullAccess "contoso.com\administrator"  -CachingMode BranchCache
            $retrycount = 6
        }catch{
            if($retrycount -ge $totalRetryTimes){
                throw
            }else{
                Start-Sleep 60
                $retrycount++
            }
        }
    }
}
 
#----------------------------------------------------------------------------
# Create test files for SMB Transport
#----------------------------------------------------------------------------
Write-Info.ps1 "Create test files for SMB Transport."
CreateFile $smbSharePath "MultipleBlocks.txt" 79KB
CreateFile $smbSharePath "MultipleSegments.txt" 40MB
CreateFile $smbSharePath "SingleBlock.txt" 10KB

#----------------------------------------------------------------------------
# Create FileShare for PCCRTP Transport, after enabling IIS
#----------------------------------------------------------------------------
Write-Info.ps1 "Create FileShare for PCCRTP Transport, after enabling IIS"
$pccrtpSharePath = "$homeDrive\inetpub\wwwroot"
CreateShareFolder "$pccrtpSharePath"


#----------------------------------------------------------------------------
# Create test files for PCCRTP Transport
#----------------------------------------------------------------------------
Write-Info.ps1 "Create test files for PCCRTP Transport."
CreateFile $pccrtpSharePath "MultipleBlocks.txt" 79KB
CreateFile $pccrtpSharePath "MultipleSegments.txt" 40MB
CreateFile $pccrtpSharePath "SingleBlock.txt" 10KB

#----------------------------------------------------------------------------
# Ending
#----------------------------------------------------------------------------
Write-Info.ps1 "Completed deploy test files."
Stop-Transcript
exit 0