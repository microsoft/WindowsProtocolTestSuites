# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

$methodDefinitions = @'
    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    public static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, IntPtr lParam);

    [return: MarshalAs(UnmanagedType.Bool)]
    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    public static extern bool PostMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, IntPtr lParam);
'@

$windowOp = Add-Type -MemberDefinition $methodDefinitions -Name NativeMethods -Namespace Win32 -PassThru

$mstsc = Get-Process -Name "mstsc"
$hMstsc = $mstsc[0].MainWindowHandle

# Messages for keyborad events
$WM_KEYDOWN = 0x0100
$WM_KEYUP = 0x0101
$VK_LEFT = 0x25
$VK_UP = 0x26
$VK_RIGHT = 0x27
$VK_DOWN = 0x28
$VK_CAPITAL = 0x14
$VK_NUMLOCK = 0x90

# Messages for mouse events
$WM_MOUSEMOVE = 0x200

# Messages for window events
$WM_SYSCOMMAND = 0x0112
$SC_MAXIMIZE = 0xF030
$SC_MINIMIZE = 0xF020
$SC_RESTORE = 0xF120

$Script:returnCodes = @()
$Script:booleanResults = @()

function ToggleKey($keyCode) {
    $downLParam = if ($keyCode -eq $VK_CAPITAL) { 0x00000001 } else { 0x01000001 }
    $upLParam = if ($keyCode -eq $VK_CAPITAL ) { 0xC0000001 } else { 0xC1000001 }

    $Script:booleanResults += $windowOp::PostMessage($hMstsc, $WM_KEYDOWN, $keyCode, $downLParam)
    Start-Sleep -Milliseconds 50
    $Script:booleanResults += $windowOp::PostMessage($hMstsc, $WM_KEYUP, $keyCode, $upLParam)
}

function MoveMouse($coordinate) {
    $Script:booleanResults += $windowOp::PostMessage($hMstsc, $WM_MOUSEMOVE, 0, $coordinate)
}

function MinimizeMstsc {
    $Script:returnCodes += $windowOp::SendMessage($hMstsc, $WM_SYSCOMMAND, $SC_MINIMIZE, 0)
}

function MaximizeMstsc {
    $Script:returnCodes += $windowOp::SendMessage($hMstsc, $WM_SYSCOMMAND, $SC_MAXIMIZE, 0)
}

function RestoreMstsc {
    $Script:returnCodes += $windowOp::SendMessage($hMstsc, $WM_SYSCOMMAND, $SC_RESTORE, 0)
}

Start-Sleep -Milliseconds 200

# Press keys
foreach ($key in @($VK_LEFT, $VK_UP, $VK_RIGHT, $VK_DOWN)) {
    ToggleKey($key)
    Start-Sleep -Milliseconds 50
}

# Move mouse
foreach ($cord in @(0x00400040, 0x00100010, 0x01000100)) {
    MoveMouse($cord)
    Start-Sleep -Milliseconds 50
}

# Toggle function keys
foreach ($key in @($VK_CAPITAL, $VK_NUMLOCK)) {
    ToggleKey($key)
    Start-Sleep -Milliseconds 50
}

# Maximize window
Start-Sleep -Milliseconds 200
MaximizeMstsc

# Minimize window
Start-Sleep -Milliseconds 200
MinimizeMstsc

# Restore window
Start-Sleep -Milliseconds 200
RestoreMstsc

$areAllZeros = $true
foreach ($code in $Script:returnCodes) {
    if ($code -ne 0) {
        $areAllZeros = $false
        break
    }
}

$areAllTrue = $true
foreach ($result in $Script:booleanResults) {
    if ($result -ne $true) {
        $areAllTrue = $false
        break
    }
}

if ($areAllZeros -and $areAllTrue) {
    Write-Host "All messages are sent successfully."
}
else {
    Write-Host "The `"mstsc`" window failed to receive all messages."
}