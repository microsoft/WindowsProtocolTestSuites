#############################################################################
## Copyright (c) Microsoft. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
#############################################################################

#------------------------------------------------------------------------------
# Script function description: 
# This script's function is to enable auditing on SUT.
# Param $computerName: The computer name of the SUT, string type.
#------------------------------------------------------------------------------
param(
[string]$computerName = $computerName,
[string]$usr = $usr,
[string]$pwd = $pwd
)


#----------------------------------------------------------------------------
# Connect Remote Computer
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
	
    # Sleep 10 seconds for waiting computer ready.                                 
    Start-Sleep -s 10
}
                                         
if ($objCon -eq $null)
{
    Throw "Connect to remote computer failed."
}  
          
#----------------------------------------------------------------------------
# Remote execute command to enable auditing on SUT
#----------------------------------------------------------------------------
$cmdLine="auditpol /set /category:`"`*`"` /success:enable /failure:enable"                                         
$oProc = $objCon.Get("Win32_Process")
$mthd = $oProc.Methods_.Item("Create")
$oInParams = $mthd.InParameters.SpawnInstance_()
$oInParams.Properties_.Item("CommandLine").Value = $cmdLine
$oOutParams = $objCon.ExecMethod("Win32_Process", "Create", $oInParams)
$returnFromExecRemote = $oOutParams.Properties_.Item("ReturnValue").Value

if($returnFromExecRemote -eq 0)
{
    return $true
}
