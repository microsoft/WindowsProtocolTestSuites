#############################################################################
## Copyright (c) Microsoft Corporation. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
#############################################################################

#-----------------------------------------------------
# Function : Get-InstalledScriptPath.ps1
# Usage    : Get the script path in the installed msi directory
#            and store this Information in the MSIInstalled.signal file
# Parameter: $Name - Test Suite Name
#-----------------------------------------------------

Param(
    [parameter(Mandatory=$true)]
    [ValidateNotNullOrEmpty()]
    [string]$Name = "MS-ADOD"
)

[string]$Path = [System.IO.Directory]::GetDirectories("$env:HOMEDRIVE\MicrosoftProtocolTests\$Name",`
            "Scripts",[System.IO.SearchOption]::AllDirectories)


[string]$SignalPath = "$env:HOMEDRIVE\MicrosoftProtocolTests\$Name\MSIInstalled.signal"

try
{
   if(Test-Path -Path $SignalPath)
   {
        Remove-Item -Path $SignalPath -Force
   }
   
   $Path | Out-File $SignalPath
   return $Path
}
catch
{
   [String]$Emsg = "Unable to get install script path for the msi. Error happened: " + $_.Exception.Message
   throw $Emsg
}

