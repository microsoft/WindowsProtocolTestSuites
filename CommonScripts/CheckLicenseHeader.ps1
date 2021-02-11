# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.
param (
    [string]$TestSuitePath,
    [string]$IsCheckAll = "false",
    [string]$targetBranch = "staging",
    [string]$sourceBranch
)

$InvocationPath = Split-Path $MyInvocation.MyCommand.Definition -parent
if($targetBranch -notmatch "refs/heads/(.+)")
{
    $targetBranch = "origin/$targetBranch"
}
if($sourceBranch -notmatch "refs/heads/(.+)")
{
    $sourceBranch = "origin/$sourceBranch"
}

Write-Host "InvocationPath: $InvocationPath"
Write-Host "TestSuitePath:$TestSuitePath"
Write-Host "targetBranch:$targetBranch"
Write-Host "sourceBranch:$sourceBranch"

Push-Location $TestSuitePath
$Diff = git diff --name-only "$targetBranch...$sourceBranch"
Pop-Location

$extension = ".ps1",".cs",".bat",".cmd",".reg",".sh",".psm1"
$result = New-Object 'System.Collections.Generic.List[String]'

function Verify-License {
    param (   
        [string]$filename,
        [string]$content
    )
    $filename = $filename.Trim()
    $license = ""
    switch ($filename.Substring($filename.LastIndexOf('.'))) {        
        ".cs" {
            $license = `
            "// Copyright (c) Microsoft. All rights reserved.`r`n" + 
            "// Licensed under the MIT license. See LICENSE file in the project root for full license information."; break
        }
        ".reg" {
            $license = `
            "; Copyright (c) Microsoft. All rights reserved.`r`n" + 
            "; Licensed under the MIT license. See LICENSE file in the project root for full license information."; break
        }
        {$_ -eq ".cmd" -or ($_ -eq ".bat")} {
            $license = `
            ":: Copyright (c) Microsoft. All rights reserved.`r`n" + 
            ":: Licensed under the MIT license. See LICENSE file in the project root for full license information."; break
        }
        {$_ -eq ".ps1" -or ($_ -eq ".psm1")} {
            $license = `
            "# Copyright (c) Microsoft. All rights reserved.`r`n" + 
            "# Licensed under the MIT license. See LICENSE file in the project root for full license information."; break
        }
        ".sh" {
            $license = `
            "# Copyright (c) Microsoft. All rights reserved.`n" + 
            "# Licensed under the MIT license. See LICENSE file in the project root for full license information."; break
        }
    }

    if(!$content.Contains($license)){
        $result.Add($filename)
    }   
}

if($IsCheckAll -eq "true"){
    Write-Host "Check all files license header"
    Get-ChildItem $TestSuitePath  -Exclude "_Helper" -Recurse | ForEach-Object{ `
        if($extension.Contains($_.Extension.ToLower())){
            Write-Host "Check file: $_"
            $content = Get-Content $_.FullName -raw
            Verify-License -filename $_ -content $content
        }
  }
}
else {
    Write-Host "Check the license header for the different files"    
    $Diff | ForEach-Object{
        $file = $_.Trim()
        if($extension.Contains($file.SubString($file.lastindexOf('.')).Trim().ToLower()) -and (Test-Path "$TestSuitePath\$file"))
        {
            Write-Host "Check file: $TestSuitePath\$file"   
            $content = Get-Content "$TestSuitePath\$file" -raw
            Verify-License -filename $file -content $content
        }
    }
}

if ($result.Count -gt 0){
    Write-Host "==========================================================="
    Write-Host "Documents found not in compliance with license requirements"
    $result
    Write-Host "==========================================================="
    throw 
}
else {
    Write-Host "Check finished"
}