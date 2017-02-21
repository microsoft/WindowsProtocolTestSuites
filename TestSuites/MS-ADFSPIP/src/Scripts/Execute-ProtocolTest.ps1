########################################################################################################
## Copyright (c) Microsoft. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
########################################################################################################

#############################################################################
#
# Microsoft Windows Powershell Scripting
# File        : Execute-ProtocolTest.ps1
# Purpose     : Protocol Test Suite Entry Point for MS-KILE
# Requirements: Windows Powershell 2.0
# Supported OS: Windows Server 2012R2
# Remark      : This script is used in WinBlue Regression Run only.
#
# This script will remotely install MSI and run configuration scripts on each
# VM listed in the specified XML file, run test cases on the driver and copy
# test result back to the host machine.
#
# To make this script work, you must specify the parameters for each VM in a 
# XML file ($ConfigFile) and the XML MUST contain the nodes below for each VM.
# XML nodes example:
#	<vm>
#	  <hypervname>MS-KILE-DC01</hypervname>  
#	  <name>DC01</name>
#	  <username>Administrator</username>   
#	  <password>Password01@</password>	  
#	  <domain>contoso.com</domain>
#	  <isdc>true</isdc>	   
#	  <ip>192.168.0.1</ip>
#	  <configscript>Config-DC01.ps1</configscript>
#	</vm>
# Missisng any node listed above will make the script execution fail.
# If <isdc> node is true, the <username> and <password> will be treated
# as domain username and password; or they will be treated as a local
# account. 
#
# Directory structure on host:
# D:\
# |-- WinBlueRegressionTest ($WorkingDirOnHost)
#     |-- TestResults
#     |-- ScriptLib
#     |-- ProtocolName
#         |-- Scripts
# Directory structure on VM:
# C:\
# |-- Deploy (Deploy folder on host will be copied here)
#     |-- *.msi
# |-- MicrosoftProtocolTests
#     |-- ...
#         |-- Batch ($BatchFolderOnVM)
#             |-- TestResults
#             |-- RunAllTestCases.cmd
#
# The script will call some scripts in the ScriptLib. So make sure the Script-
# Lib folder is there. Besides, no extra scripts are needed. Scripts running 
# on the VM will be executed directly from this script. No files need to be 
# copied to the VM.
# 
# This script is designed to be capable with any protocol test. Just modidy
# the parameters at the beginning. Other parts of the script do not need to 
# be modified. If you have special need for your protocol test, see the 
# comments in the in the script, and modify accordingly.
#
#
##############################################################################

Param
(   # WTT will pass in the first three arguments.
    [string]$ProtocolName         = "MS-ADFSPIP",
    [string]$WorkingDirOnHost     = "D:\WinteropProtocolTesting",
    [string]$TestResultDirOnHost  = "$WorkingDirOnHost\TestResults\$ProtocolName",
    [string]$TestSuiteDirOnHost   = "$WorkingDirOnHost\$ProtocolName",
    # Modify according real driver configuration. 
    [string]$DriverVMName         = "MS-ADFSPIP-DRIVER",    
    # The batch file needs to be executed on driver
    [string]$BatchToRun           = "RunAllTestCases.cmd",
    # XML file which stores VM information. 
    [string]$ConfigFile           = "$TestSuiteDirOnHost\Scripts\$ProtocolName.xml"
)

#----------------------------------------------------------------------------
# Global variables
#----------------------------------------------------------------------------
$global:testResultDir = $TestResultDirOnHost
$CurrentScriptName = $MyInvocation.MyCommand.Name

#region Utility Functions
#----------------------------------------------------------------------------
# Utility Function Declaration
#----------------------------------------------------------------------------
# Define call stack functions, which used to print log file.
[int]$global:indentControl = 0
$global:callStackLogFile = "$TestResultDirOnHost\Execute-ProtocolTest.ps1.CallStack.log"
function global:EnterCallStack($scriptName)
{
	if($scriptName -ne "Execute-ProtocolTest.ps1")
	{
		$global:indentControl++
	}
	$tab = ""
	for ($i=1; $i -le $global:indentControl; $i++)
	{
		$tab += "    "
	}
	$date = get-date -Format "M/d/yyyy hh:mm:ss"
	("") | Out-File -FilePath $callStackLogFile -Append -Force
	($tab + "Start $scriptName    " + $date) | Out-File -FilePath $callStackLogFile -Append -Force
}

function global:ExitCallStack($scriptName)
{

	$tab = ""
	for ($i=1; $i -le $global:indentControl; $i++)
	{
		$tab += "    "
	}
	if($scriptName -ne "Execute-ProtocolTest.ps1")
	{
		$global:indentControl--
	}
	$date = get-date -Format "M/d/yyyy hh:mm:ss"
	($tab + "Exit $scriptName    " + $date) | Out-File -FilePath $callStackLogFile -Append -Force
}

# A quick utility to call Get-Timestamp.ps1
Function SetTimestamp
{
    Param
    (
        [Parameter(Mandatory=$true)]
        [ValidateSet("initial","startconfig","startruntest","testdone")]
        [string]$State
    )

    if (($State -eq "startconfig") -or ($State -eq "startruntest"))
    {
        $ExecState = $State + $ProtocolName
    }
    else
    {
        $ExecState = $State
    }

    .\Get-Timestamp.ps1 $ProtocolName $ExecState $TestResultDirOnHost
}


#  Make the script wait for some seconds while prrinting dots on the screen.
Function Wait([int]$seconds = 10, [string]$prompt)
{
    if($prompt -ne $null)
    {
        Write-Host $prompt -ForegroundColor Yellow
    }
    
    for ($count = 1; $count -lt $seconds; $count ++)
    {
		$TimeLeft = $seconds - $count
        Write-Progress -Activity "Please wait..." `
					   -Status "$([int]($TimeLeft / 60)) minutes $($TimeLeft % 60) seconds left." `
                       -PercentComplete (($count / $seconds) * 100)
        sleep 1
    }
}

Function SetTrustedHosts{
    $originalValue = (get-item WSMan:\localhost\Client\TrustedHosts).value    
    [xml]$Content = Get-Content $ConfigFile
    $ips = $Content.SelectNodes("//vm/ip").InnerText

    $ipstr = ""
    foreach ($ip in $ips){
        if($ipstr){
            $ipstr = $ipstr + "," + $ip 
        }else{
            $ipstr = $ip
        }
    }

    $curValue=""
    if($originalValue){
        $curValue = $originalValue + "," + $ipstr
    }else{
        $curValue = $ipstr
    }

    set-item WSMan:\localhost\Client\TrustedHosts -Value $curValue -force

    return $originalValue
}


Function RestoreTrustedHosts{
    param($originalValue)

    if($originalValue){
            set-item WSMan:\localhost\Client\TrustedHosts -Value $originalValue -force
    }else{
            set-item WSMan:\localhost\Client\TrustedHosts -Value "" -force
    }
}
#endregion

#region ScriptBlocks
#----------------------------------------------------------------------------
# Utility ScriptBlock Declaration
#----------------------------------------------------------------------------
# The below functions are declared as scriptblocks because they are going to be 
# executed on remote computers.

# Install all MSI files in the specified folder
[ScriptBlock]$InstallMSIs = {
    Param([string]$FolderPath,[string]$EndPoint)
            
    $InstallMSIs = Get-ChildItem $FolderPath

    foreach ($Msi in $InstallMSIs)
    {
        cmd /c msiexec -i ($Msi.FullName) -q TARGET_ENDPOINT=$EndPoint
    }
}

# Find specified file or directory from the MicrosoftProtocolTests folder on 
# the computer. The folder will be created after the test suite MSI is installed.
[ScriptBlock]$GetItemInTestSuite = {
    Param([string]$Name)

    # Try if the name specified is a directory
    [string]$Path = [System.IO.Directory]::GetDirectories("$env:HOMEDRIVE\MicrosoftProtocolTests",`
                   $Name,[System.IO.SearchOption]::AllDirectories)
 
    if(($Path -eq $null) -or ($Path -eq ""))
    {
        # Try if the name specified is a file
        [string]$Path = [System.IO.Directory]::GetFiles("$env:HOMEDRIVE\MicrosoftProtocolTests",`
                        $Name,[System.IO.SearchOption]::AllDirectories)
    }

    return $Path
}

# Remove all the PAUSEs in the script or batch in order to make the script
# or batch can finish automatically.
[ScriptBlock]$RemovePauses = {
    Param([string]$FilePath)

    $Content = Get-Content $FilePath
    $NewContent = ""

    foreach ($Line in $Content)
    {
        if($Line.ToLower().Contains(" pause") -or ($Line.ToLower().Trim() -eq "pause"))
        {
            $Line = ""
        }
        $NewContent = $NewContent + $Line + "`r`n"
    }

    Set-Content $FilePath $NewContent
}

# Create a windows task and run it.
# To do this is because if the commands executed remotely do not have
# full permissions to access all system resources. But a program run by
# the task scheduler has local previleges.
# Notice that the task creater must be the administrator, or the task
# will not be started.
[ScriptBlock]$CreateTaskAndRun = {
    Param([string]$FilePath,[string]$TaskName,[string]$TaskUser)

    # Push to the parent folder folder first, then run
    $ParentDir = [System.IO.Directory]::GetParent($FilePath)
    $Command = "{Push-Location $ParentDir; Invoke-Expression $FilePath}"
    # Guarantee commands run in powershell environment
    $Task = "Powershell Powershell -Command $Command"
    # Create task
    cmd /c schtasks /Create /RL HIGHEST /RU Administrators /SC ONCE /ST 00:00 /TN $TaskName /TR $Task /IT /F
    Sleep 5
    # Run task
    cmd /c schtasks /Run /TN $TaskName  
}
#endregion

#region Prepare and Finish
#----------------------------------------------------------------------------
# Procedure Functions
#----------------------------------------------------------------------------
Function Prepare
{
    Write-Host "Start Executing [$CurrentScriptName] ... " -ForegroundColor Cyan

    # Enter call stack
    if($function:EnterCallStack -ne $null)
    {
	    EnterCallStack "Execute-ProtocolTest.ps1"
    }

    # Check test result directory
    if(!(Test-Path -Path $TestResultDirOnHost))
    {
        md $TestResultDirOnHost
    }

    # Start logging
    Start-Transcript -Path "$TestResultDirOnHost\Execute-ProtocolTest.ps1.log" -Append -Force -ErrorAction SilentlyContinue

    # Push location to the ScriptLib folder
    # Because scripts in lib will be called
    $ScriptLibPathOnHost = "$WorkingDirOnHost\ScriptLib"
    Push-Location $ScriptLibPathOnHost
}

Function Finish
{
    # Finish script
    Pop-Location
    Stop-Transcript -ErrorAction SilentlyContinue
    # Write Call Stack
    if($function:ExitCallStack -ne $null)
    {
	    ExitCallStack "Execute-ProtocolTest.ps1"
    }
    Write-Host "Protocol Test Execute Completed." -ForegroundColor Green
}
#endregion

#region Configure VMs
# Configurate the specified VM: Install MSI and run config script.
Function RemoteConfigVM($VmParamArray,$InstallMSI = $true)
{
    [string]$RemoteIP = $VmParamArray["ip"]
    [string]$RemoteComputerName = $VmParamArray["name"]
    [string]$RemoteComputerDomain = $VmParamArray["domain"]
    [string]$RemotePassword = $VmParamArray["password"]
    [string]$RemoteUserName = ""   
    [string]$IsDC = $VmParamArray["isdc"]
          
    # If the computer is a DC, use the domain user to logon.
	# Because DC cannot be logged on with local user accounts.
    # If the computer is not a DC, use the local account to 
	# logon, no matter whether the computer has been joined to 
	# a domain.
    if($IsDC.ToLower() -eq "true")
    {
        $RemoteUserName = $RemoteComputerDomain + "\" + $VmParamArray["username"]
    }
    else
    {
        $RemoteUserName = $RemoteComputerName + "\" + $VmParamArray["username"]
    }
    
    Write-Host "Start configuring $RemoteComputerName" -ForegroundColor Cyan


    # Build remote session
    Write-Host "Try to connect to computer $RemoteIP" -ForegroundColor Yellow
    $VmCredential = New-Object System.Management.Automation.PSCredential `
        -ArgumentList $RemoteUserName,(ConvertTo-SecureString $RemotePassword -AsPlainText -Force)
    $RemoteSession = New-PSSession -ComputerName $RemoteIP -Credential $VmCredential
    
	# Failed to start pssession
    if($RemoteSession -eq $null)
    {
        Write-Error "Failed to connect to $RemoteIP" 
        return
    }
    
    # Install MSI on the VM
    if ($InstallMSI -eq $true)
    {     
        # Copy deploy files to the VM
        # Directory on VM: C:\Deploy
        $DeployPathOnHost = "$TestSuiteDirOnHost\Deploy"
        Write-Host "Copying MSI to VM" -ForegroundColor Yellow
        # Copy deploy files to the VM
        net use "\\$RemoteIP\C$" $RemotePassword /User:$RemoteUserName
        Copy-Item -Path $DeployPathOnHost -Destination "\\$RemoteIP\C$" -Recurse -Force

        # Determine whether the target VM is Driver or SUT
        if ($VmParamArray["hypervname"] -eq $DriverVMName){ $TargetEndPoint = "TESTSUITE" }
        else { $TargetEndPoint = "SUT" }

        # Remote Install MSI
        Write-Host "Installing MSI" -ForegroundColor Yellow
        Invoke-Command -Session $RemoteSession -ScriptBlock $InstallMSIs -ArgumentList "C:\Deploy","$TargetEndPoint"
    }

    # Run config script on VM
    [string]$ConfigScript = $VmParamArray["configscript"]
    # Get config script path
    Write-Host "Trying to get script path on VM" -ForegroundColor Yellow
    $ConfigScriptPathOnVM = "c:\temp\scripts\$ConfigScript" 
    
    Write-Host "Config Script Path: $ConfigScriptPathOnVM"

    # Remove PAUSEs in the script
    #Write-Host "Removing PAUSEs in the script" -ForegroundColor Yellow
    #Invoke-Command -Session $RemoteSession -ScriptBlock $RemovePauses -ArgumentList $ConfigScriptPathOnVM
    
    # Remove signal files if any
    $RemoveSignalFiles = [ScriptBlock]{
        Get-ChildItem "$env:HOMEDRIVE\" | Where-Object {$_.Name.ToLower().Contains("signal")} | Remove-Item
    }
    Invoke-Command -Session $RemoteSession -ScriptBlock $RemoveSignalFiles
    
    # Run config script
    Write-Host "Running config script. Please wait..." -ForegroundColor Yellow
    Invoke-Command -Session $RemoteSession -ScriptBlock $CreateTaskAndRun `
        -ArgumentList $ConfigScriptPathOnVM,"RunConfig",$RemoteUserName

    # All remote commands finished
    Remove-PSSession $RemoteSession
    

    # Wait for script done and restart
	# Because some scripts generate signal file while some do not,
	# some scripts need reboot some do not, 
	# we do not have a general method to tell whether the script finishes
	# or not.
	# Usually 200 seconds is enough for the script to finish.
    Wait 600 ## TODO: Modify according to the actual requirement
    
    #re-setup the remote session, since some computer may restart and the remote session breaks
    while ($RemoteSession.state -ne "Opened")
    {
        try
        {
            $RemoteSession = New-PSSession -ComputerName $RemoteIP -Credential $VmCredential
        }
        catch
        {
            write-host "Trying to setup new ps-session to $RemoteIP ..."
        }
    }
    if($RemoteSession.state -eq "Opened")
    {
        #restart the computer when remote session setup.
        Invoke-Command -Session $RemoteSession -ScriptBlock {Restart-Computer -force}         

        # All remote commands finished
        Remove-PSSession $RemoteSession
    }
    # Wait for computer restart completely. So that dependent computers can find the computer successfully
	wait 120    
	
    # Finish
    Write-Host "Configuring Computer $RemoteComputerName Done" -ForegroundColor Green
}

# Read all the VMs from the XML and config each VM.
Function ConfigEachVM
{
    [xml]$Content = Get-Content $ConfigFile
    $VMs = $Content.SelectNodes("//vm")   

    foreach ($VM in $VMs)
    {
        $VmParamArray = @{}

        foreach($Node in $VM.ChildNodes)
        {
            $VmParamArray[$Node.Name] = $Node.InnerText
        }

        RemoteConfigVM $VmParamArray
		
		## TODO: If you do not need install MSI on the computer, use
		# RemoteConfigVM $VmParamArray -InstallMSI $false
    }
}
#endregion

#region Run Test Cases
# Run test cases on driver and copy test results back to the host.
Function RunTestCaseOnDriver
{
    # Get Driver Infomation
    [xml]$Content = Get-Content $ConfigFile
    $DriverNode = $Content.SelectSingleNode("//vm[hypervname=`'$DriverVMName`']")
    $DriverParamArray = @{}
    foreach ($Node in $DriverNode.ChildNodes)
    {
        $DriverParamArray[$Node.Name] = $Node.InnerText
    }

    [string]$DriverIPAddress      = $DriverParamArray["ip"]
    [string]$DriverComputerName   = $DriverParamArray["name"]
    [string]$DriverUserName       = $DriverParamArray["username"]
    [string]$DriverPassword       = $DriverParamArray["password"]
    [string]$DriverDomain         = $DriverParamArray["domain"]
    [string]$DriverFullUserName   = $DriverComputerName + "\" + $DriverUserName

    # Build session. Prepare to execute script on driver computer.
    Write-Host "Trying to connect to computer $DriverIPAddress" -ForegroundColor Yellow
    $DriverCredential = New-Object System.Management.Automation.PSCredential `
        -ArgumentList $DriverFullUserName,(ConvertTo-SecureString $DriverPassword -AsPlainText -Force)		
    $DriverSession = New-PSSession -ComputerName $DriverIPAddress -Credential $DriverCredential
    
	# Failed to start pssession
    if($DriverSession -eq $null)
    {
        Write-Error "Failed to connect to driver computer"
        return
    }
    
    # Get batch file path
    Write-Host "Trying to get the batch folder" -ForegroundColor Yellow
    $BatchFolderOnVM = Invoke-Command -Session $DriverSession -ScriptBlock $GetItemInTestSuite -ArgumentList "Batch"	
    $BatchFilePathOnVM = "$BatchFolderOnVM\$BatchToRun"
    Write-Host "Batch file on driver: $BatchFilePathOnVM"

    # Modify batch file. Remove PAUSEs.
    Write-Host "Removing PAUSEs in the batch file" -ForegroundColor Yellow
    Invoke-Command -Session $DriverSession -ScriptBlock $RemovePauses -ArgumentList $BatchFilePathOnVM

    # Run test cases
    Write-Host "Start to run test cases. " -ForegroundColor Yellow
    Invoke-Command -Session $DriverSession -ScriptBlock $CreateTaskAndRun `
        -ArgumentList $BatchFilePathOnVM,"RunTestCases",$DriverFullUserName
 
    # ScriptBlock to run on driver computer
    # Wait for test cases done
    Write-Host "Running test cases. Please wait..." -ForegroundColor Yellow
    
    $WaitForTestDone = [ScriptBlock]{        
        # Wait MSTest start
		$ProcList = [string](Get-Process)
		$Times = 60 # Try 60 times, i.e. 1 minute
		for ($count = 0; ($count -lt $Times) -and !($ProcList.ToLower().Contains("vstest")); $count ++)
		{
			Sleep 1
			$ProcList = [string](Get-Process)
		}	
		# Wait until test finished
		Get-Process MSTest | Wait-Process
    }
    #Invoke-Command -Session $DriverSession -ScriptBlock $WaitForTestDone

    sleep 1000 # sleep 1000 seconds for test case execution done
    
    #Write-Host "Waiting for test result" -ForegroundColor Yellow
    #Sleep 5
	
    # Test Done
    Write-Host "Run Test Suite Done" -foregroundcolor Yellow
    Remove-PSSession $DriverSession
		
    # Get test result network path
	if ($BatchFolderOnVM.IndexOf('\') -ne 0)
	{
		# If the batch folder path contains system drive, like "C:\MicrosotProtocolTest\..."
		# Remove the system drive, to make it like "\MicrosoftProtocolTest\..."
		# so that it is able to concatenate with the network location
		$BatchFolderOnVM = $BatchFolderOnVM.Remove(0, $BatchFolderOnVM.IndexOf('\'))
	}
    $TestResultDirOnVM = "\\$DriverIPAddress\C$" + "$BatchFolderOnVM\TestResults"
	
	# Copy test result to host
    Write-Host "Copying test result from test VM to host machine ..." -foregroundcolor Yellow
	net use "\\$DriverIPAddress\C$" $DriverPassword /User:$DriverFullUserName
    Get-Item  -Path "$TestResultDirOnVM\*" # List all files under test result folder on VM
    Copy-Item -Path "$TestResultDirOnVM\*" -Destination $TestResultDirOnHost -Recurse -Force
}
#endregion

#----------------------------------------------------------------------------
# Main Function
#----------------------------------------------------------------------------
Function Main
{
    $originalHosts = SetTrustedHosts
    Prepare
    SetTimestamp -State initial

    SetTimestamp -State startconfig
	# we do not need to run any config file after VSTORMLITE postscripts
    # so skip ConfigEachVM and run test cases directly
    # ConfigEachVM  
   
    SetTimestamp -State startruntest
	# Run test cases on driver and copy test result to the host.
	# This procedure can be removed if you do not need it 
    RunTestCaseOnDriver  
    SetTimestamp -State testdone
        
    Finish
    RestoreTrustedHosts($originalHosts)
    Exit 0
}

Main