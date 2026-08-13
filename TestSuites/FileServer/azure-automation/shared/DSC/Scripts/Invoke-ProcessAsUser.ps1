# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

[Diagnostics.CodeAnalysis.SuppressMessageAttribute('PSAvoidUsingPlainTextForPassword', 'Password',
    Justification = 'The deployment account password originates in private Config.json and is required by the Windows LogonUser API.')]
[CmdletBinding()]
param(
    [Parameter(Mandatory)]
    [string]$UserName,

    [Parameter(Mandatory)]
    [string]$Domain,

    [Parameter(Mandatory)]
    [string]$Password,

    [Parameter(Mandatory)]
    [string]$FilePath,

    [string[]]$ArgumentList = @(),

    [string]$WorkingDirectory = 'C:\',

    [switch]$KeepProfileLoaded,

    [switch]$WaitForExit,

    [ValidateRange(1, 86400)]
    [int]$TimeoutSeconds = 600
)

if (-not ('ProtocolTestSuites.UserProcess' -as [type])) {
    Add-Type -TypeDefinition @'
using System;
using System.Runtime.InteropServices;

namespace ProtocolTestSuites
{
    public static class UserProcess
    {
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct StartupInfo
        {
            public int cb;
            public string lpReserved;
            public string lpDesktop;
            public string lpTitle;
            public int dwX;
            public int dwY;
            public int dwXSize;
            public int dwYSize;
            public int dwXCountChars;
            public int dwYCountChars;
            public int dwFillAttribute;
            public int dwFlags;
            public short wShowWindow;
            public short cbReserved2;
            public IntPtr lpReserved2;
            public IntPtr hStdInput;
            public IntPtr hStdOutput;
            public IntPtr hStdError;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct ProcessInformation
        {
            public IntPtr hProcess;
            public IntPtr hThread;
            public int dwProcessId;
            public int dwThreadId;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct ProfileInfo
        {
            public int dwSize;
            public int dwFlags;
            public string lpUserName;
            public string lpProfilePath;
            public string lpDefaultPath;
            public string lpServerName;
            public string lpPolicyPath;
            public IntPtr hProfile;
        }

        [DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern bool LogonUser(
            string userName,
            string domain,
            string password,
            int logonType,
            int logonProvider,
            out IntPtr token);

        [DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern bool CreateProcessWithTokenW(
            IntPtr token,
            int logonFlags,
            string applicationName,
            string commandLine,
            int creationFlags,
            IntPtr environment,
            string currentDirectory,
            ref StartupInfo startupInfo,
            out ProcessInformation processInformation);

        [DllImport("userenv.dll", SetLastError = true)]
        public static extern bool CreateEnvironmentBlock(out IntPtr environment, IntPtr token, bool inherit);

        [DllImport("userenv.dll", SetLastError = true)]
        public static extern bool DestroyEnvironmentBlock(IntPtr environment);

        [DllImport("userenv.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern bool LoadUserProfile(IntPtr token, ref ProfileInfo profileInfo);

        [DllImport("userenv.dll", SetLastError = true)]
        public static extern bool UnloadUserProfile(IntPtr token, IntPtr profile);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool CloseHandle(IntPtr handle);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern uint WaitForSingleObject(IntPtr handle, uint milliseconds);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool TerminateProcess(IntPtr process, uint exitCode);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool GetExitCodeProcess(IntPtr process, out uint exitCode);
    }
}
'@
}

function ConvertTo-WindowsCommandLineArgument {
    param([string]$Value)

    if ($Value -notmatch '[\s"]') { return $Value }
    return '"' + (($Value -replace '(\\*)"', '$1$1\"') -replace '(\\+)$', '$1$1') + '"'
}

$token = [IntPtr]::Zero
$environment = [IntPtr]::Zero
$userProfile = [ProtocolTestSuites.UserProcess+ProfileInfo]::new()
$profileLoaded = $false
$processInfo = [ProtocolTestSuites.UserProcess+ProcessInformation]::new()
$LOGON32_LOGON_INTERACTIVE = 2
$LOGON32_PROVIDER_DEFAULT = 0
$LOGON_WITH_PROFILE = 1
$CREATE_NO_WINDOW = 0x08000000
$CREATE_UNICODE_ENVIRONMENT = 0x00000400
$WAIT_OBJECT_0 = 0
$WAIT_TIMEOUT = 0x00000102
$WAIT_FAILED = [uint32]::MaxValue

# Asynchronous launches need the profile to remain loaded after this helper
# returns. Synchronous launches keep it loaded until the child exits, then the
# normal finally path unloads it.
if (-not $PSBoundParameters.ContainsKey('KeepProfileLoaded') -and -not $WaitForExit) {
    $KeepProfileLoaded = $true
}

try {
    $loggedOn = [ProtocolTestSuites.UserProcess]::LogonUser(
        $UserName,
        $Domain,
        $Password,
        $LOGON32_LOGON_INTERACTIVE,
        $LOGON32_PROVIDER_DEFAULT,
        [ref]$token)
    if (-not $loggedOn) {
        throw "LogonUser failed for '$Domain\$UserName' with Win32 error $([Runtime.InteropServices.Marshal]::GetLastWin32Error())."
    }

    $userProfile.dwSize = [Runtime.InteropServices.Marshal]::SizeOf($userProfile)
    $userProfile.lpUserName = $UserName
    $profileLoaded = [ProtocolTestSuites.UserProcess]::LoadUserProfile($token, [ref]$userProfile)
    if (-not $profileLoaded) {
        throw "LoadUserProfile failed for '$Domain\$UserName' with Win32 error $([Runtime.InteropServices.Marshal]::GetLastWin32Error())."
    }

    if (-not [ProtocolTestSuites.UserProcess]::CreateEnvironmentBlock([ref]$environment, $token, $false)) {
        throw "CreateEnvironmentBlock failed for '$Domain\$UserName' with Win32 error $([Runtime.InteropServices.Marshal]::GetLastWin32Error())."
    }

    $commandLineParts = @((ConvertTo-WindowsCommandLineArgument $FilePath))
    $commandLineParts += @($ArgumentList | ForEach-Object { ConvertTo-WindowsCommandLineArgument $_ })
    $commandLine = $commandLineParts -join ' '
    $startup = [ProtocolTestSuites.UserProcess+StartupInfo]::new()
    $startup.cb = [Runtime.InteropServices.Marshal]::SizeOf($startup)
    $startup.lpDesktop = 'winsta0\default'

    $created = [ProtocolTestSuites.UserProcess]::CreateProcessWithTokenW(
        $token,
        $LOGON_WITH_PROFILE,
        $FilePath,
        $commandLine,
        ($CREATE_NO_WINDOW -bor $CREATE_UNICODE_ENVIRONMENT),
        $environment,
        $WorkingDirectory,
        [ref]$startup,
        [ref]$processInfo)
    if (-not $created) {
        throw "CreateProcessWithTokenW failed for '$Domain\$UserName' with Win32 error $([Runtime.InteropServices.Marshal]::GetLastWin32Error())."
    }

    $exitCode = $null
    if ($WaitForExit) {
        $waitResult = [ProtocolTestSuites.UserProcess]::WaitForSingleObject(
            $processInfo.hProcess,
            [uint32]($TimeoutSeconds * 1000))
        if ($waitResult -eq $WAIT_TIMEOUT) {
            [void][ProtocolTestSuites.UserProcess]::TerminateProcess($processInfo.hProcess, 1460)
            [void][ProtocolTestSuites.UserProcess]::WaitForSingleObject($processInfo.hProcess, 30000)
            throw [TimeoutException]::new("Process $($processInfo.dwProcessId) exceeded the $TimeoutSeconds-second timeout and was terminated.")
        }
        if ($waitResult -eq $WAIT_FAILED) {
            throw "WaitForSingleObject failed for process $($processInfo.dwProcessId) with Win32 error $([Runtime.InteropServices.Marshal]::GetLastWin32Error())."
        }
        if ($waitResult -ne $WAIT_OBJECT_0) {
            throw "WaitForSingleObject returned unexpected status $waitResult for process $($processInfo.dwProcessId)."
        }

        [uint32]$nativeExitCode = 0
        if (-not [ProtocolTestSuites.UserProcess]::GetExitCodeProcess(
            $processInfo.hProcess,
            [ref]$nativeExitCode)) {
            throw "GetExitCodeProcess failed for process $($processInfo.dwProcessId) with Win32 error $([Runtime.InteropServices.Marshal]::GetLastWin32Error())."
        }
        $exitCode = [int]$nativeExitCode
    }

    [pscustomobject]@{
        ProcessId = $processInfo.dwProcessId
        User = "$Domain\$UserName"
        Started = $true
        ExitCode = $exitCode
    }
}
finally {
    if ($processInfo.hThread -ne [IntPtr]::Zero) {
        [void][ProtocolTestSuites.UserProcess]::CloseHandle($processInfo.hThread)
    }
    if ($processInfo.hProcess -ne [IntPtr]::Zero) {
        [void][ProtocolTestSuites.UserProcess]::CloseHandle($processInfo.hProcess)
    }
    if ($environment -ne [IntPtr]::Zero) {
        [void][ProtocolTestSuites.UserProcess]::DestroyEnvironmentBlock($environment)
    }
    if ($profileLoaded -and -not $KeepProfileLoaded) {
        [void][ProtocolTestSuites.UserProcess]::UnloadUserProfile($token, $userProfile.hProfile)
    }
    if ($token -ne [IntPtr]::Zero) {
        [void][ProtocolTestSuites.UserProcess]::CloseHandle($token)
    }
}
