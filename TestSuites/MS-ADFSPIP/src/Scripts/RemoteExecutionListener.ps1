########################################################################################################
## Copyright (c) Microsoft. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
########################################################################################################

Param
(
    [bool]$IsWindowsActivated = $True
)

[bool]$Global:Activated = $IsWindowsActivated
$shell = New-Object -ComObject "Shell.Application"
$shell.minimizeall()
$requestFile = "c:\temp\RemoteRequest.txt"
$responseFile = "c:\temp\RemoteResponse.txt"
Remove-Item $requestFile -ErrorAction SilentlyContinue
Remove-Item $responseFile -ErrorAction SilentlyContinue

Start-Transcript -Path "c:\temp\listener.log" -Append

while($True)
{
    try
    {

        $str = Get-Content $requestFile -ErrorAction SilentlyContinue
   
        if($str-ne $null)
        {
            Write-Host "Request received: $str"

            $execution = $str -split "\r\n"
            $count =$execution.Count
            $cmd = "c:\temp\" + $execution[0] + " " + $execution[1]
if($count -eq 3)
{
$cmd = $cmd + " " + $execution[2]
}

            Write-Host "Command: $cmd"

            $output = Invoke-Expression -Command "$cmd"

            Remove-Item $requestFile -ErrorAction SilentlyContinue
            Remove-Item $responseFile -ErrorAction SilentlyContinue

            $file = New-Item $responseFile -type file
            $output >> $file

            Write-Host "Output value: $output"
        }
        
    }
    catch
    {
        # ignore any exception
    }
    sleep(5);
}