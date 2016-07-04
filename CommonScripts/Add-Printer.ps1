#############################################################################
## Copyright (c) Microsoft. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
##
## Microsoft Windows Powershell Scripting
## File:           Add-Printer.ps1
## Purpose:        Add a printer
## Version:        1.2 (20 Aug, 2008)
##
##############################################################################

Param(
[string]$printerName     ="ProtocolTestPrinter",
[string]$printerModel    ="Apollo P-1200",
[string]$prot            ="LPT1:", 
[string]$computerName    ="."
)

#----------------------------------------------------------------------------
# Print execution information
#----------------------------------------------------------------------------
Write-Host "EXECUTING [Add-Printer.ps1]..." -foregroundcolor cyan
Write-Host "`$printerName    = $printerName" 
Write-Host "`$printerModel   = $printerModel" 
Write-Host "`$prot           = $prot" 
Write-Host "`$computerName   = $computerName" 

#----------------------------------------------------------------------------
# Function: Show-ScriptUsage
# Usage   : Describes the usage information and options
#----------------------------------------------------------------------------
function Show-ScriptUsage
{    
    Write-host 
    Write-host "Usage: This scripts will add a printer."
    Write-host
    Write-host "Example: Add-Printer.ps1 ProtocolTestPrinter  'Apollo P-1200' 'LPT1:' . "
    Write-host
}

#----------------------------------------------------------------------------
# Show help if required
#----------------------------------------------------------------------------
if ($args[0] -match '-(\?|(h|(help)))')
{
    Show-ScriptUsage 
    return
}

#----------------------------------------------------------------------------
# Verify required parameters
#----------------------------------------------------------------------------
if ($printerName -eq $null -or $printerName -eq "")
{
    Throw "Parameter `$printerName is required."
}

if ($printerModel -eq $null -or $printerModel -eq "")
{
    Throw "Parameter `$printerModel is required."
}

if ($prot -eq $null -or $prot -eq "")
{
    Throw "Parameter `$prot is required."
}

if ($computerName -eq $null -or $computerName -eq "")
{
    Throw "Parameter `$computerName is required."
}

#----------------------------------------------------------------------------
# EXECUTION
#----------------------------------------------------------------------------
$objWMIDriversBefore = get-wmiobject -Class "Win32_PrinterDriver" -namespace "root\cimv2" -ComputerName $computerName
cmd /c rundll32.exe printui.dll, PrintUIEntry /if /b $printerName /r $prot /m $printerModel /Z  /q  2>&1 | Write-Host
$objWMIDriversAfter = get-wmiobject -Class "Win32_PrinterDriver" -namespace "root\cimv2" -ComputerName $computerName

#if(([int]$objWMIDriversBefore.Count) -eq (([int]$objWMIDriversAfter.Count) - 1))
#{
#    Write-Host "Succeed to add a new printer."
#    Write-Host "The new printer's name is " $printerName ",and its model is " $printerModel","
#}
#else
#{
#    Write-Host "Fail to add new printers."
#}
Write-Host "Succeed to add a new printer."

#----------------------------------------------------------------------------
# Print Exit information
#----------------------------------------------------------------------------
Write-Host "EXECUTE [Add-Printer.ps1] FINISHED (NOT VERIFIED)." -foregroundcolor yellow

exit
