# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.-

$sutName      = $PtfPropSutMachineName
$sharefolder1 = $PtfPropSutNtfsShare1
$sharefolder2 = $PtfPropSutNtfsShare2
$sharefolder3 = $PtfPropSutFatShare1
$sharefolder4 = $PtfPropSutFatShare2
$testfile1    = $PtfPropSutShareTest1
$testfile2    = $PtfPropSutShareTest2
$newfile      = $PtfPropSutShareNewName

Remove-Item \\$sutName\$sharefolder1\$testfile1 -Force
Remove-Item \\$sutName\$sharefolder1\$testfile2 -Force
Remove-Item \\$sutName\$sharefolder1\$newfile -Force
Remove-Item \\$sutName\$sharefolder2\$testfile1 -Force
Remove-Item \\$sutName\$sharefolder2\$testfile2 -Force
Remove-Item \\$sutName\$sharefolder2\$newfile -Force
Remove-Item \\$sutName\$sharefolder3\$testfile1 -Force
Remove-Item \\$sutName\$sharefolder3\$testfile2 -Force
Remove-Item \\$sutName\$sharefolder3\$newfile -Force
Remove-Item \\$sutName\$sharefolder4\$testfile1 -Force
Remove-Item \\$sutName\$sharefolder4\$testfile2 -Force
Remove-Item \\$sutName\$sharefolder4\$newfile -Force