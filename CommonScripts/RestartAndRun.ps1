#############################################################################
## Copyright (c) Microsoft Corporation. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
#############################################################################

#-----------------------------------------------------------------------------
# Function: RestartAndRun
# Usage   : Restart the computer and run the specified script.
# Params  : -ScriptPath <String> : The path of the script to be executed after
#           rebooting. Only absolute path will be accepted; reletive path will
#           cause an exception thrown.
#           -PhaseIndicator <String> : If the calling script needs to continue
#           executing, usually a phase indicator is needed to indicate which
#           part of the script will be executed after rebooting. A phase
#           indicator is usually a parameter in the calling script.
#           -ArgumentList <String>: If extra arguments need to be passed in the 
#           script, use this parameter.
#           -AutoRestart <bool>: Default set to false. If it is false, the 
#           script will pause to wait for key stoke before rebooting. If it is 
#           set to true, the script will restart the computer after waiting for
#           3 seconds without waiting for the user input.
# Remark  : After calling this script, if you are not going to use it anymore
#           in your script, you must call RestartAndRunFinish.ps1 to clean up
#           the registry entry.
#-----------------------------------------------------------------------------

Param
(
    [Parameter(Mandatory=$true)]
    [ValidateNotNullOrEmpty()]
    [ValidateScript({
        # Only absolute path is accepted. Because relative path
        # may cause trouble after rebooting.
        if([System.IO.Path]::IsPathRooted($_) -eq $False)
        {
            throw "Argument ScriptPath must be absolute path"
        }
        return $true
    })]
    [String]$ScriptPath,
        
    [Parameter(Mandatory=$false)]
    [ValidateNotNullOrEmpty()]
    [String]$PhaseIndicator,

    [Parameter(Mandatory=$false)]
    [String]$ArgumentList,

    [Parameter(Mandatory=$false)]
    [bool]$AutoRestart = $false
)


$private:regRunPath = "HKLM:\SOFTWARE\Microsoft\Windows\CurrentVersion\Run" 
$private:regKeyName = "TKFRSAR"

# If the key has already been set, remove it
if (((Get-ItemProperty $regRunPath).$regKeyName) -ne $null)
{
	Remove-ItemProperty -Path $regRunPath -Name $regKeyName
}

try
{
    Set-ItemProperty -Path $regRunPath -Name $regKeyName `
                        -Value "cmd /c powershell $ScriptPath $PhaseIndicator $ArgumentList" `
                        -Force -ErrorAction Stop
}
catch
{
    throw "Unable to restart. Error happened: $_.Exception.Message"
}

# Show prompt
Write-Host "The computer is going to restart..." -ForegroundColor Yellow
if($AutoRestart -eq $true) { 
    # Auto restart
    # Restart
    shutdown -r -t 10 -f
 } 