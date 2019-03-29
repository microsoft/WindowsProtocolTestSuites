########################################################################################
##
## Microsoft Windows Powershell Scripting
## Purpose: Execute a command on a server to delete a file under shared folder
##
#########################################################################################
#param---list---------------------------------------------------------------------------
#$serverName
#$domainName
#$userName
#$password
#$fileName
#---------------------------------------------------------------------------------------


.\WSPRemoteExecute-Command.ps1 $serverName "cmd /c del $env:HOMEDRIVE\Test\Data\Test\$fileName" $domainName\$userName $password

#sleep to wait for file indexed
sleep 5

$homedrive = .\Get-RemoteSystemDrive.ps1 $serverName $serverName\$userName $password
$s = $homedrive.Substring(0,1)

$exist = Test-Path \\$serverName\$s$\Test\Data\Test\$fileName

if($exist -eq $true)
{
    return 1
}
else
{
    return 0
}

#---------------------------------------------------------------------------------------
## End to delete file
#---------------------------------------------------------------------------------------