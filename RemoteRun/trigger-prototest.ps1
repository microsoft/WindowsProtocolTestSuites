Param
(
    [Parameter(Mandatory=$true)]
    [ValidateNotNullOrEmpty()]
    [string]$DriverMachine,

    [Parameter(Mandatory=$true)]
    [ValidateNotNullOrEmpty()]
    [string]$UserName,

    [Parameter(Mandatory=$true)]
    [ValidateNotNullOrEmpty()]
    [string]$Task,

    [Parameter(Mandatory=$true)]
    [ValidateNotNullOrEmpty()]
    [string]$ResultPathOnWindows,

    [Parameter(Mandatory=$true)]
    [ValidateNotNullOrEmpty()]
    [string]$ResultFile
)

$session = New-PSSession -HostName $DriverMachine -UserName $UserName

Write-Output "Starting testing..."
Invoke-Command -Session $session -ScriptBlock { param($TaskName) schtasks.exe /Run /TN $TaskName } -ArgumentList $Task

$timeout = New-TimeSpan -Minutes 20

$sw = [diagnostics.stopwatch]::StartNew()
while ($sw.elapsed -lt $timeout)
{
  $task = Invoke-Command -ScriptBlock { param($TaskName) schtasks.exe /Query /TN $TaskName /v /fo csv | ConvertFrom-Csv } -ArgumentList $Task -Session $session

  if ($task.Status -eq "Ready")
  {
    Write-Output "Finished!"

    Write-Output "Copy test result..."
    Copy-Item -FromSession $session -Path $ResultPathOnWindows -Destination $ResultFile
    Write-Output "Test result is located at $HOME/report.txt"

    $res = Get-Content $ResultFile | Measure-Object -Line
    $total = $res.Lines
    $res = Get-Content $ResultFile | Select-String "Passed" | Measure-Object -Line
    $pass = $res.Lines
    $res = Get-Content $ResultFile | Select-String "Failed" | Measure-Object -Line
    $fail = $res.Lines
    $res = Get-Content $ResultFile | Select-String "Inconclusive" | Measure-Object -Line
    $notrun = $res.Lines

    Write-Output "Ran $total cases, $pass passed, $fail failed, $notrun is inconclusive."

    Return
  }
  Start-Sleep -seconds 5
}

Write-Output "Timed out!"
