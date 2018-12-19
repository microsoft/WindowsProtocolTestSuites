##################################################################################
## Copyright (c) Microsoft Corporation. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
##################################################################################

#############################################################################
##
## Microsoft Windows Powershell Scripting
## File:           Register-DbgSrv.ps1
## Purpose:        Register the windbg dbgsrv for debugging purpose.
## Requirements:   Windows Powershell 2.0
## Supported OS:   Windows Server 2008 R2, Windows Server 2012, Windows Server 2012 R2,
##                 Windows Server 2016 and later
##
##############################################################################

if(Test-Path -Path "c:\temp\scripts\TTT\tttracer.exe" -PathType Leaf)
{
    .\scripts\registerwdbgsvr.ps1
}
Sleep 30