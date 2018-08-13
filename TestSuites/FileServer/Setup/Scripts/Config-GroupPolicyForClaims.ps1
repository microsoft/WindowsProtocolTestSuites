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

#----------------------------------------------------------------------------
# Start loging using start-transcript cmdlet
#----------------------------------------------------------------------------
[string]$logFile = $MyInvocation.MyCommand.Path + ".log"
Start-Transcript -Path "$logFile" -Append -Force


#----------------------------------------------------------------------------
# Configure Group Policy for Claims
#----------------------------------------------------------------------------
Write-Info.ps1 "Configure Group Policy for Claims"

Write-Info.ps1 "Minimize all windows"
$shell = New-Object -ComObject "Shell.Application"
$shell.MinimizeAll()

Write-Info.ps1 "Create Wscript.shell for UI automation"
$AUKEY = New-Object -ComObject Wscript.shell

Write-Info.ps1 "Open Group Policy Browser"
get-process | where {$_.ProcessName -like "mmc*"} | foreach {$_.Kill()}
GPME.msc

Write-Info.ps1 "Navigate to Default Domain Policy and Open roup Policy Management Editor"
SLEEP 10
$AUKEY.SendKeys("{TAB}")       #Domain Controllers. contoso.com
SLEEP 3
$AUKEY.SendKeys("{down}")      #Default Domain Policy
SLEEP 3
$AUKEY.SendKeys("{enter}")     #Group Policy Management Editor

Write-Info.ps1 "Navigate to Computer Configureation > Policies > Windows Settings > Security > File System > Central Access Policy"
SLEEP 10

Write-Info.ps1 "Select Policies"
$AUKEY.SendKeys("{down 2}")    #Policies
SLEEP 5
$AUKEY.SendKeys("{enter}")     #Close unexpected Popup Window
SLEEP 5
$AUKEY.SendKeys("{enter}")     #Select Policies
SLEEP 5
$AUKEY.SendKeys("{right}")     #Expand Policies
SLEEP 3

Write-Info.ps1 "Select Windows Settings"
$AUKEY.SendKeys("{down 2}")    #Windows Settings
SLEEP 3
$AUKEY.SendKeys("{right}")     #Expand Windows Settings
SLEEP 3

Write-Info.ps1 "Select Security Settings"
$AUKEY.SendKeys("{down 3}")    #Security Settings
SLEEP 5
$AUKEY.SendKeys("{right}")     #Expand Security Settings
SLEEP 3

Write-Info.ps1 "Select File System"
$AUKEY.SendKeys("{down 7}")    #File System
SLEEP 3
$AUKEY.SendKeys("{right}")     #Expand File System
SLEEP 3

Write-Info.ps1 "Select Central Access Policy"
$AUKEY.SendKeys("{down 1}")    #Central Access Policy
SLEEP 3
$AUKEY.SendKeys("{enter}")     #Select Central Access Policy
SLEEP 3

Write-Info.ps1 "Right-click Central Access Policy and Select Manage Central Access Policy to open it."
$AUKEY.SendKeys("+{F10}")      #Open Right-click menu 
SLEEP 3
$AUKEY.SendKeys("{down 2}")    #Select Manage Central Access Policy
SLEEP 3
$AUKEY.SendKeys("{enter}")     #Open Central Access Policy
SLEEP 10

Write-Info.ps1 "Select all central access pollicies and add them."
$AUKEY.SendKeys("+^{END}")     #Select all central access pollicies
SLEEP 10	
$AUKEY.SendKeys("%A")          #Press ALT+A to Add all Access Policy 
SLEEP 3

Write-Info.ps1 "Close dialog: Manage Central Access Policy."		
$AUKEY.SendKeys("{enter}")     #Close dialog
SLEEP 2

Write-Info.ps1 "Finished setting group policy, colse MMC."	
# Close MMC
get-process | where {$_.ProcessName -like "mmc*"} | foreach {$_.Kill()} 	
sleep 15

Write-Info.ps1 "Publish the group policy updates to all computers by command: gpupdate /force "
CMD /C gpupdate /force 2>&1 | Write-Info.ps1

#----------------------------------------------------------------------------
# Ending
#----------------------------------------------------------------------------
Stop-Transcript
exit 0