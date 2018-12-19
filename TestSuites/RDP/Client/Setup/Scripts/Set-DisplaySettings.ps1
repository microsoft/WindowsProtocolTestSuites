#############################################################################
## Copyright (c) Microsoft Corporation. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
#############################################################################

Function Set-ScreenResolution {
<#
	.SYNOPSIS
	Change screen resolution
	.DESCRIPTION
	Change screen resolution
	.EXAMPLE
	Set-ScreenResolution -Width 1024 -Height 768
	.EXAMPLE
	Set-ScreenResolution 1024 768
#>
param (
[Parameter(Mandatory=$true,
           Position = 0)]
[int]
$Width,

[Parameter(Mandatory=$true,
           Position = 1)]
[int]
$Height
)

Add-Type $DisplaySettingsCode
[DisplaySettings.DisplaySettings]::ChangeResolution($width,$height)
}

Function Set-ScreenOrientation {
<#
	.SYNOPSIS
	Change screen orientation
	.DESCRIPTION
	Orientation                    Value
	 Landscape                       0
	 Portrait                        1
	 Landscape (flipped)             2
	 Portrait (flipped)              3
	.EXAMPLE
	Set-ScreenOrientation -Orientation 1
	.EXAMPLE
	Set-ScreenOrentation 1
#>
param (
[Parameter(Mandatory=$true,
           Position = 0)]
[int]
$Orientation
)

Add-Type $DisplaySettingsCode
[DisplaySettings.DisplaySettings]::ChangeOrientation($Orientation)
}

$DisplaySettingsCode = @'

using System;
using System.Runtime.InteropServices;

/// <summary>
/// Define DEVMODE structure
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public struct DEVMODE
{
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
    public string dmDeviceName;
    
    public UInt16 dmSpecVersion;
    public UInt16 dmDriverVersion;
    public UInt16 dmSize;
    public UInt16 dmDriverExtra;
    public UInt32 dmFields;

    // Union
    public Int32 x;
    public Int32 y;
    public UInt32 dmDisplayOrientation;
    public UInt32 dmDisplayFixedOutput;
    
    public Int16 dmColor;
    public Int16 dmDuplex;
    public Int16 dmYResolution;
    public Int16 dmTTOption;
    public Int16 dmCollate;

    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
    public string dmFormName;

    public UInt16 dmLogPixels;
    public UInt32 dmBitsPerPel;
    public UInt32 dmPelsWidth;
    public UInt32 dmPelsHeight;
    public UInt32 dmDisplayFlags;
    public UInt32 dmDisplayFrequency;
    public UInt32 dmICMMethod;
    public UInt32 dmICMIntent;
    public UInt32 dmMediaType;
    public UInt32 dmDitherType;
    public UInt32 dmReserved1;
    public UInt32 dmReserved2;
    public UInt32 dmPanningWidth;
    public UInt32 dmPanningHeight;
};

class User_32
{
	[DllImport("user32.dll")]
	public static extern int EnumDisplaySettings (string deviceName, int modeNum, ref DEVMODE devMode );         
	[DllImport("user32.dll")]
	public static extern int ChangeDisplaySettings(ref DEVMODE devMode, int flags);

	public const int ENUM_CURRENT_SETTINGS = -1;
	public const int CDS_UPDATEREGISTRY = 0x01;
	public const int CDS_TEST = 0x02;
	public const int DISP_CHANGE_SUCCESSFUL = 0;
	public const int DISP_CHANGE_RESTART = 1;
	public const int DISP_CHANGE_FAILED = -1;

    public const UInt32 DMDO_DEFAULT = 0;
    public const UInt32 DMDO_90 = 1;
    public const UInt32 DMDO_180 = 2;
    public const UInt32 DMDO_270 = 3;
}

namespace DisplaySettings
{
	public class DisplaySettings
	{
        /// <summary>
        /// Change display orientation
        /// </summary>
        /// <param name="orientation">Orientation (0, 1, 2, 3)</param>
        /// <returns></returns>
        static public string ChangeOrientation(UInt32 orientation)
        {
            DEVMODE dm = new DEVMODE();
            dm.dmDeviceName = new String(new char[32]);
            dm.dmFormName = new String(new char[32]);
            dm.dmSize = (UInt16)Marshal.SizeOf(dm);

            if (0 != User_32.EnumDisplaySettings(null, User_32.ENUM_CURRENT_SETTINGS, ref dm))
            {
                UInt32 width = dm.dmPelsWidth;
                UInt32 height = dm.dmPelsHeight;
                dm.dmDisplayOrientation = orientation;
                dm.dmPelsWidth = height;
                dm.dmPelsHeight = width;

                return ChangeDisplaySettings(dm);
            }
            return "Enum Display Settings Error";
        }

        /// <summary>
        /// Change display resolution
        /// </summary>
        /// <param name="width">Width</param>
        /// <param name="height">Height</param>
        /// <returns></returns>
        static public string ChangeResolution(UInt32 width, UInt32 height)
        {
            DEVMODE dm = new DEVMODE();
            dm.dmDeviceName = new String(new char[32]);
            dm.dmFormName = new String(new char[32]);
            dm.dmSize = (UInt16)Marshal.SizeOf(dm);

            if (0 != User_32.EnumDisplaySettings(null, User_32.ENUM_CURRENT_SETTINGS, ref dm))
            {
                dm.dmPelsWidth = width;
                dm.dmPelsHeight = height;

                return ChangeDisplaySettings(dm);
            }
            return "Enum Display Settings Error";
        }

        /// <summary>
        /// Change display setting
        /// </summary>
        /// <param name="dm">DEVMODE structure</param>
        /// <returns></returns>
        static string ChangeDisplaySettings(DEVMODE dm)
        {
            int result = User_32.ChangeDisplaySettings(ref dm, User_32.CDS_TEST);
            if (result == User_32.DISP_CHANGE_FAILED)
            {
                return "Unable to process your request";
            }
            else
            {
                result = User_32.ChangeDisplaySettings(ref dm, User_32.CDS_UPDATEREGISTRY);
                switch (result)
                {
                    case User_32.DISP_CHANGE_SUCCESSFUL:
                        {
                            return "Success";
                        }
                    case User_32.DISP_CHANGE_RESTART:
                        {
                            return "You need to reboot for the change to happen";
                        }
                    default:
                        {
                            return "Fail to change display setting";
                        }
                }
            }
        }
    }

}

'@
