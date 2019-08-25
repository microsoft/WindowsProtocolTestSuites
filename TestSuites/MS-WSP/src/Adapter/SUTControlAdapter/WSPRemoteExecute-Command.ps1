#############################################################################
##
## Microsoft Windows Powershell Scripting
## Purpose:        Execute a command on a WSP server.
##
##############################################################################
param(
[string]$computerName, 
[string]$cmdLine, 
[string]$usr, 
[string]$pwd
)

#----------------------------------------------------------------------------
# Verify required parameters
#----------------------------------------------------------------------------
if ($computerName -eq $null -or $computerName -eq "")
{
    Throw "Parameter computerName is required."
}
if ($cmdLine -eq $null -or $cmdLine -eq "")
{
    Throw "Parameter cmdLine is required."
}

#----------------------------------------------------------------------------
# Using global username/password when caller doesnot provide.
#----------------------------------------------------------------------------
if ($usr -eq $null -or $usr -eq "")
{
    $usr = $global:usr
    $pwd = $global:pwd
}

#----------------------------------------------------------------------------
# Make username prefixed with domain/computername
#----------------------------------------------------------------------------
if ($usr.IndexOf("\") -eq -1)
{
    if ($global:domain  -eq $null -or $global:domain -eq "")
    {
        $usr = "$computerName\$usr"
    }
    else
    {
        $usr = "$global:domain\$usr"
    }
}

#----------------------------------------------------------------------------
# Try to connect to the remote computer
#----------------------------------------------------------------------------
$objCon = $null
for ($index = 0; $index -lt 15; $index++)
{
    $objSWemLocator = New-Object -ComObject WbemScripting.SWbemLocator
    $objCon = $objSWemLocator.ConnectServer($computerName, "root\CIMV2", $usr, $pwd, "", "", 128)

    $objCon.Security_.ImpersonationLevel = 3
    $objCon.Security_.AuthenticationLevel = 6

    if ($objCon -ne $null)
    {
        break
    }
 
    Start-Sleep -s 10
}

if ($objCon -eq $null)
{
    Throw "Connect to remote computer failed."
}

#----------------------------------------------------------------------------
# Remote execute command
#----------------------------------------------------------------------------
$oProc = $objCon.Get("Win32_Process")
$mthd = $oProc.Methods_.Item("Create")
$oInParams = $mthd.InParameters.SpawnInstance_()
$oInParams.Properties_.Item("CommandLine").Value = $cmdLine
$oOutParams = $objCon.ExecMethod("Win32_Process", "Create", $oInParams)
$returnFromExecRemote = $oOutParams.Properties_.Item("ReturnValue").Value 

#----------------------------------------------------------------------------
# Verifying Execute Result
#----------------------------------------------------------------------------
if ($returnFromExecRemote -ne 0)
{
    Throw "Execute [RemoteExecute-Command.ps1] failed. Return code is: $returnFromExecRemote."
}

exit
