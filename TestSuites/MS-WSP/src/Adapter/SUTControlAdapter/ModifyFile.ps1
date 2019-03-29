########################################################################################
##
## Microsoft Windows Powershell Scripting
## Purpose: Execute a command on a server to modify a file under shared folder
##
#########################################################################################
#param---list---------------------------------------------------------------------------
#$serverName
#$domainName
#$userName
#$password
#$fileName
#---------------------------------------------------------------------------------------

.\WSPRemoteExecute-Command.ps1 $serverName "cmd /c ECHO newfile to modify>$env:HOMEDRIVE\Test\Data\Test\$fileName" $domainName\$userName $password

#sleep to wait for file indexed
sleep 5

$homedrive = .\Get-RemoteSystemDrive.ps1 $serverName $serverName\$userName $password
$s = $homedrive.Substring(0,1)

$exist = Test-Path \\$serverName\$s$\Test\Data\Test\$fileName

if($exist -eq $true)
{
    return 0
}
else
{
    return 1
}

#---------------------------------------------------------------------------------------
## End to modify file
#---------------------------------------------------------------------------------------