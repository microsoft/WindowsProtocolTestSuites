# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.-

$sutName      = $PtfProp_SutMachineName
$sharefolder1 = $PtfProp_SutNtfsShare1
$sharefolder2 = $PtfProp_SutNtfsShare2
$sharefolder3 = $PtfProp_SutFatShare1
$sharefolder4 = $PtfProp_SutFatShare2
$testfile1    = $PtfProp_SutShareTest1
$testfile2    = $PtfProp_SutShareTest2
$newfile      = $PtfProp_SutShareNewName

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