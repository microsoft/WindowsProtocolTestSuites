#############################################################################
## Copyright (c) Microsoft Corporation. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
#############################################################################

$MethodDefine = @'
[DllImport("user32.dll", CharSet = CharSet.Auto)]
public static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, IntPtr lParam);
'@
$WM_SYSCOMMAND = 0x0112
$SC_MAXIMIZE = 0xF030
$SC_RESTORE = 0xF120
$WindowOp = Add-Type -MemberDefinition $MethodDefine -name NativeMethods -namespace Win32 -PassThru
$mstsc = Get-Process mstsc
$hwnd = $mstsc[0].MainWindowHandle
# Maximize window
Start-Sleep -Milliseconds 200
$WindowOp::SendMessage($hwnd, $WM_SYSCOMMAND, $SC_MAXIMIZE, 0)



