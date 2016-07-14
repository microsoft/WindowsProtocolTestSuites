#############################################################
## Copyright (c) Microsoft Corporation. All rights reserved.
#############################################################

#############################################################################
#
# Microsoft Windows PowerShell Scripting
# File        : Execute-ProtocolTest.ps1
# Purpose     : Protocol Test Suite Entry Point for Kerberos
# Requirements: Windows PowerShell 2.0
# Supported OS: Windows Server 2012 or later versions
#
# This script will remotely install MSI and run configuration scripts on each
# VM listed in the specified XML file, run test cases on the driver and copy
# test result back to the host machine.
#
# To make this script work, you must specify the parameters for each VM in a 
# XML file ($ConfigFile) and the XML MUST contain the nodes below for each VM.
# XML nodes example:
#	<vm>
#	  <hypervname>Kerberos-DC01</hypervname>  
#	  <name>DC01</name>
#	  <username>Administrator</username>   
#	  <password>Password01@</password>	  
#	  <domain>contoso.com</domain>
#	  <isdc>true</isdc>	   
#	  <ip>192.168.0.1</ip>
#	  <configscript>Config-DC01.ps1</configscript>
#	</vm>
# Missing any node listed above will make the script execution fail.
# If <isdc> node is true, the <username> and <password> will be treated
# as domain username and password; or they will be treated as a local
# account. 
#
# Directory structure on host:
# D:\
# |-- WinBlueRegressionTest ($WorkingDirOnHost)
#     |-- TestResults
#     |-- ScriptLib
#     |-- $ProtocolName
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
# This script is designed to be capable with any protocol test. Just modify
# the parameters at the beginning. Other parts of the script do not need to 
# be modified. If you have special need for your protocol test, see the 
# comments in the in the script, and modify accordingly.
#
# Created by t-zewang@microsoft.com on 12/5/2012.
#
##############################################################################

Param
(   # WTT will pass in the first three arguments.
    [string]$ProtocolName         = "Kerberos",
    [string]$WorkingDirOnHost     = "D:\WinteropProtocolTesting",
    [string]$TestResultDirOnHost  = "$WorkingDirOnHost\TestResults\$ProtocolName",
    [string]$TestSuiteDirOnHost   = "$WorkingDirOnHost\$ProtocolName",
    # Modify according real driver configuration. 
    [string]$DriverVMName         = "Kerberos-OSS-ENDPOINT01",    
    # The batch file needs to be executed on driver
    # XML file which stores VM information. 
    [string]$ConfigFile           = "$TestSuiteDirOnHost\Scripts\$ProtocolName.xml"
)

#----------------------------------------------------------------------------
# Global variables
#----------------------------------------------------------------------------
$global:testResultDir = $TestResultDirOnHost
$CurrentScriptName = $MyInvocation.MyCommand.Name
$RunCaseScript = "Execute-ProtocolTestByOsVersion.ps1"
$CurrentScriptPath = Split-Path $MyInvocation.MyCommand.Definition -Parent
$env:Path += ";$CurrentScriptPath"

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


#  Make the script wait for some seconds while printing dots on the screen.
Function Wait([int]$seconds = 10, [string]$prompt)
{
    if($prompt -ne $null)
    {
        Write-Info.ps1 $prompt -ForegroundColor Yellow
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

# Create a remote powershell session to VM
Function CreatePSSession($RemoteSession, $RemoteIP, $VmCredential)
{
    while (($RemoteSession -eq $null) -or ($RemoteSession.state -ne "Opened"))
    {
        try
        {
		    Write-Info.ps1 "Create new PSSession to $RemoteIP ..."
            $RemoteSession = New-PSSession -ComputerName $RemoteIP -Credential $VmCredential
        }
        catch
        {
            Write-Info.ps1 "Trying to setup new ps-session to $RemoteIP ..."
			wait 10
        }
    }
    return $RemoteSession
}

#----------------------------------------------------------------------------
# Utility ScriptBlock Declaration
#----------------------------------------------------------------------------

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
    $Task = "PowerShell PowerShell -Command $Command"
    # Create task
    cmd /c schtasks /Create /RL HIGHEST /RU Administrators /SC ONCE /ST 00:00 /TN $TaskName /TR $Task /IT /F
    Sleep 5
    # Run task
    cmd /c schtasks /Run /TN $TaskName  
}

# Check the test case result
[ScriptBlock]$ScriptToCheckTrxResult = {
    Param([string]$trxPath)

	$result = Get-Item $trxPath\*.trx

    return $result
}

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

#----------------------------------------------------------------------------
# Procedure Functions
#----------------------------------------------------------------------------
Function Prepare
{
    Write-Info.ps1 "Start Executing [$CurrentScriptName] ... " -ForegroundColor Cyan

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

# Configurate the specified VM: Install MSI and run config script.
Function RemoteConfigVM($VmParamArray)
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
    
    Write-Info.ps1 "Start configuring $RemoteComputerName" -ForegroundColor Cyan


    # Build remote session
    Write-Info.ps1 "Try to connect to computer $RemoteIP" -ForegroundColor Yellow
    $VmCredential = New-Object System.Management.Automation.PSCredential `
        -ArgumentList $RemoteUserName,(ConvertTo-SecureString $RemotePassword -AsPlainText -Force)

    # Run config script on VM
    [string]$ConfigScript = $VmParamArray["configscript"]
    # Get config script path
    Write-Info.ps1 "Trying to get script path on VM" -ForegroundColor Yellow
    $ConfigScriptPathOnVM = "c:\temp\scripts\$ConfigScript" 
    
    Write-Info.ps1 "Config Script Path: $ConfigScriptPathOnVM"
    
    # Remove signal files if any
    $RemoveSignalFiles = [ScriptBlock]{
        Get-ChildItem "$env:HOMEDRIVE\" | Where-Object {$_.Name.ToLower().Contains("signal")} | Remove-Item
    }
    #Always check remote session status before run command remotely.
    $RemoteSession = CreatePSSession $RemoteSession $RemoteIP $VmCredential
    Invoke-Command -Session $RemoteSession -ScriptBlock $RemoveSignalFiles
    
    # Run config script
    Write-Info.ps1 "Running config script. Please wait..." -ForegroundColor Yellow
    #Always check remote session status before run command remotely.
    $RemoteSession = CreatePSSession $RemoteSession $RemoteIP $VmCredential
    Invoke-Command -Session $RemoteSession -ScriptBlock $CreateTaskAndRun `
        -ArgumentList $ConfigScriptPathOnVM,"RunConfig",$RemoteUserName

    Write-Info.ps1 "Wait for script to finish by checking config-*.finished.signal"
	$waitTimes = 90 # Default times of waiting
	$retryConfig = $false
	if($RemoteComputerName -match "AP0*")
	{
		# For AP01/AP02, it may hang with unknown reason
		# So we use a retry 
	    $retryConfig = $true
		$waitTimes = 45 
	}
	
    While ($waitTimes -ge 0)
    {
        #Always check remote session status before run command remotely.
        $RemoteSession = CreatePSSession $RemoteSession $RemoteIP $VmCredential

        $signalFile = Invoke-Command -Session $RemoteSession -ScriptBlock { Get-Item C:\Config-*.finished.signal }
    
        if($signalFile -eq $null)
        {
            Write-Info.ps1 "Did not find signal file, will try again." -ForegroundColor Yellow
			Wait 20
        }
        else
        {
		    Write-Info.ps1 "Found signal file: $signalFile" 
				
	        # Copy test logs to host
            Write-Info.ps1 "Copying test logs from test VM to host machine ..."
	        net use "\\$RemoteIP\C$" $RemotePassword /User:$RemoteUserName
            Get-Item  -Path "\\$RemoteIP\C$\Temp\*.log" # List all logs files under test result folder on VM
            $Hypervname = $VmParamArray["hypervname"]
            Copy-Item -Path "\\$RemoteIP\C$\Temp\*.log" -Destination $TestResultDirOnHost\$Hypervname\ -Recurse -Force

	    	# Restart computer (all VMs in Kerberos require restart after run config)
			Write-Info.ps1 "Restart Computer $RemoteComputerName" -ForegroundColor Yellow
            
            #Always check remote session status before run command remotely.
            $RemoteSession = CreatePSSession $RemoteSession $RemoteIP $VmCredential
	        
            Invoke-Command -Session $RemoteSession -ScriptBlock {Restart-Computer -force} 
			wait 60
            break
        }

        $waitTimes = $waitTimes - 1
		
		if($waitTimes -le 0 -and $retryConfig -eq $true)
		{
			Write-Info.ps1 "$RemoteComputerName does not finish config within 15 minutes, will retry again." -ForegroundColor Yellow
			$retryConfig = $false # only retry once, set retryConfig to false
			$waitTimes = 45 # reset wait time
            
            #Always check remote session status before run command remotely.
            $RemoteSession = CreatePSSession $RemoteSession $RemoteIP $VmCredential
			Invoke-Command -Session $RemoteSession -ScriptBlock $CreateTaskAndRun `
        		-ArgumentList $ConfigScriptPathOnVM,"RetryRunConfig",$RemoteUserName
		}
    } 
	
	if($waitTimes -le 0)
	{
		Write-Info.ps1 "$RemoteComputerName does not finish config within 30 minutes, quit current script." -ForegroundColor Yellow
		exit 1 #Notify wtt to let the job fail here
	}
	
	Write-Info.ps1 "Wait for computer restart completely. So that dependent computers can find the computer successfully."
    $RemoteSession = CreatePSSession $RemoteSession $RemoteIP $VmCredential

    # Remove PSSession
	Write-Info.ps1 "Remove RemoteSession"
    Remove-PSSession $RemoteSession
		
    # Finish
    Write-Info.ps1 "Configuring Computer $RemoteComputerName Done" -ForegroundColor Green
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
    }
}

# Run test cases on driver and copy test results back to the host.
Function RunTestCaseOnDriver
{
	Write-Info.ps1 "-------------------------------------"
    Write-Info.ps1 "Run test case on driver computer."
	
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
    Write-Info.ps1 "Trying to connect to computer $DriverIPAddress" -ForegroundColor Yellow
    $DriverCredential = New-Object System.Management.Automation.PSCredential `
        -ArgumentList $DriverFullUserName,(ConvertTo-SecureString $DriverPassword -AsPlainText -Force)

    # Run test cases
    Write-Info.ps1 "Start to run test cases. " -ForegroundColor Yellow
    #Always check remote session status before run command remotely.
    $DriverSession = CreatePSSession $DriverSession $DriverIPAddress $DriverCredential
    $RunCaseScriptPathOnVM = "c:\temp\scripts\$RunCaseScript" 
    Invoke-Command -Session $DriverSession -ScriptBlock $CreateTaskAndRun `
        -ArgumentList $RunCaseScriptPathOnVM,"RunTestCases",$RemoteUserName
 
    # ScriptBlock to run on driver computer
    # Wait for test cases done
    Write-Info.ps1 "Running test cases. Please wait..." -ForegroundColor Yellow
    Write-Info.ps1 "Wait for test run to finish by checking TRX result."
    Wait 240 # sleep 4 minutes for test case execution done  
	
	$waitTimes = 90

    #Always check remote session status before run command remotely.
    $DriverSession = CreatePSSession $DriverSession $DriverIPAddress $DriverCredential
    $BatchFolderOnVM = Invoke-Command -Session $DriverSession -ScriptBlock $GetItemInTestSuite -ArgumentList "Batch"	
	$trxResultPath = "$BatchFolderOnVM\TestResults"
	Write-Info.ps1 "Batch result path on driver computer: $trxResultPath"
    While ($waitTimes -ge 0)
    {
        #Always check remote session status before run command remotely.
        $DriverSession = CreatePSSession $DriverSession $DriverIPAddress $DriverCredential
        $trxResultFile = Invoke-Command -Session $DriverSession -ScriptBlock $ScriptToCheckTrxResult -ArgumentList $trxResultPath
    
        if($trxResultFile -eq $null)
        {
            Write-Info.ps1 "Did not find TRX result file, will try again."
			Wait 20
        }
        else
        {
		    Write-Info.ps1 "Found TRX result file." 
			Write-Info.ps1 $trxResultFile
            break
        }
        $waitTimes = $waitTimes - 1
    } 	

    if($waitTimes -le 0)
	{
		Write-Info.ps1 "Running test case doesn't finish in 30 minutes, quit current script." -ForegroundColor Yellow
		exit 1 #Notify wtt to let the job fail here
	}	

    # Test Done
    Write-Info.ps1 "Run Test Suite Done" -foregroundcolor Yellow
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
    Write-Info.ps1 "Copying test result from test VM to host machine ..." -foregroundcolor Yellow
	net use "\\$DriverIPAddress\C$" $DriverPassword /User:$DriverFullUserName
    Get-Item  -Path "$TestResultDirOnVM\*" # List all files under test result folder on VM
    Copy-Item -Path "$TestResultDirOnVM\*" -Destination $TestResultDirOnHost -Recurse -Force
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
    Write-Info.ps1 "Protocol Test Execute Completed." -ForegroundColor Green
}

#----------------------------------------------------------------------------
# Main Function
#----------------------------------------------------------------------------

Function Main
{
	# Enter call stack and start logging
    
    set-item WSMan:\localhost\Client\TrustedHosts -Value * -force

    Prepare
    SetTimestamp -State initial

    SetTimestamp -State startconfig
	# Install MSI and run config scripts
	# This procedure can be removed if you do not need it 
	# If you do not need to install MSI, configurate it 
	# in the function. See the function above.
    ConfigEachVM  
   
    SetTimestamp -State startruntest
	# Run test cases on driver and copy test result to the host.
	# This procedure can be removed if you do not need it 
    RunTestCaseOnDriver  
    SetTimestamp -State testdone
        
	# Exit call stack and stop logging
    Finish
        
    Exit 0
}

# Call Main function
Main
