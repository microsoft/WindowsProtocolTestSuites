# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

$methodDefinitions = @'
    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    public static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, IntPtr lParam);
'@

$windowOp = Add-Type -MemberDefinition $methodDefinitions -name NativeMethods -namespace Win32 -PassThru

$WM_SYSCOMMAND = 0x0112
$SC_MAXIMIZE = 0xF030

$mstsc = Get-Process -Name "mstsc"
$hMstsc = $mstsc[0].MainWindowHandle

# Maximize window
Start-Sleep -Milliseconds 200
$returnCode = $windowOp::SendMessage($hMstsc, $WM_SYSCOMMAND, $SC_MAXIMIZE, 0)

if ($returnCode -eq 0) {
    Write-Host "The SC_MACIMIZE command is sent and processed successfully."
}
else {
    Write-Host "The `"mstsc`" window failed to receive the SC_MACIMIZE command."
}



