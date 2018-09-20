#############################################################################
## Copyright (c) Microsoft. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
##
#############################################################################

#----------------------------------------------------------------------------
# Global variables
#----------------------------------------------------------------------------
$scriptPath = Split-Path $MyInvocation.MyCommand.Definition -parent
$env:Path += ";$scriptPath;$scriptPath\Scripts"
$systemDrive = $ENV:SystemDrive
$fullAccessAccount = "BUILTIN\Administrators"
$buildinUsers = "BUILTIN\Users"

#----------------------------------------------------------------------------
# Start loging using start-transcript cmdlet
#----------------------------------------------------------------------------
[string]$logFile = $MyInvocation.MyCommand.Path + ".log"
try { Stop-Transcript -ErrorAction SilentlyContinue } catch {} # Ignore Stop-Transcript error messages
Start-Transcript -Path "$logFile" -Append -Force

#----------------------------------------------------------------------------
# Define common functions
#----------------------------------------------------------------------------
function ExitCode()
{ 
    return $MyInvocation.ScriptLineNumber 
}

function NewSMBShare([string]$Name,[string]$Path,[string[]]$FullAccess)
{

    Write-Info.ps1 "Create folder $Path"
    if(!(Test-Path $Path))
    {
        CMD /C MKDIR $Path
    }

    Write-Info.ps1 "Grant full access to BUILTIN\Administrators"
    CMD /C "icacls $Path /grant BUILTIN\Administrators`:(OI)(CI)(F)" 2>&1 | Write-Info.ps1

    $smbShare = Get-SmbShare | where {$_.Name -eq "$Name" -and $_.Path -eq "$Path"}
    if($smbShare -eq $null)
    {        
        New-SMBShare -name "$Name" -Path "$Path" -FullAccess $FullAccess
    }
}

function RemoveAcl($folder,$user)
{
    Write-Info.ps1 "Stop permission inheriting."
    $acl = Get-Acl -path $folder 
    $acl.SetAccessRuleProtection($true,$true)
    $acl | Set-Acl

    Write-Info.ps1 "Remove permissions."
    $acl = Get-Acl -path $folder
    Foreach($access in $acl.access) 
    { 
        Foreach($value in $access.identityReference.Value) 
        { 
            if ($value -eq $user) 
            { 
                $acl.RemoveAccessRuleAll($access) | Out-Null 
            } 
       }
    }
    Set-Acl -path $folder -aclObject $acl
}

#----------------------------------------------------------------------------
# Create shared folders for Auth test suite
#----------------------------------------------------------------------------
if((gwmi win32_computersystem).partofdomain -eq $true)
{

    #----------------------------------------------------------------------------
    # Install WindowsFeature FS-Resource-Manager
    #----------------------------------------------------------------------------
    Write-Info.ps1 "Install WindowsFeature FS-Resource-Manager"
    Add-WindowsFeature FS-Resource-Manager
    
    #----------------------------------------------------------------------------
    # Update FSRMClassificationpropertyDefinition
    #----------------------------------------------------------------------------
    Write-Info.ps1 "Update FSRMClassificationpropertyDefinition"
    Update-FSRMClassificationpropertyDefinition

    #----------------------------------------------------------------------------
    # Create shared folders for Auth test suite
    #----------------------------------------------------------------------------
    Write-Info.ps1 "Create shared folders for Auth test suite."

    $shareNames = "AzFile","AzFolder"
    foreach($shareName in $shareNames)
    {
        Write-Info.ps1 "Create shared folder: $shareName"
        $filePath = "$systemDrive\$shareName"
        Write-Info.ps1 "Share the folder $filePath."
        NewSMBShare -name "$shareName" -Path "$filePath" -FullAccess "$fullAccessAccount","$buildinUsers"
        
        Write-Info.ps1 "Remove $buildinUsers from Acl."
        RemoveAcl -folder "$filePath" -user "$buildinUsers"
    }

    Write-Info.ps1 "Create shared folder: AzShare"
    NewSMBShare -name "AzShare" -Path "$systemDrive\AzShare" -FullAccess "$fullAccessAccount"	
    Write-Info.ps1 "Keep $buildinUsers in Acl."

    Write-Info.ps1 "Create shared folder: AzCBAC"
    NewSMBShare -name "AzCBAC" -Path "$systemDrive\AzCBAC" -FullAccess "$fullAccessAccount","$buildinUsers"	
    Write-Info.ps1 "Keep $buildinUsers in Acl."

    Write-Info.ps1 "Completed creating all shared folders."
}
else
{
    Write-Info.ps1 "The ENV is workgroup, Auth test suite does not support such ENV, will skip creating shared folders."
}

#----------------------------------------------------------------------------
# Ending
#----------------------------------------------------------------------------
Stop-Transcript
exit 0