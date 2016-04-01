// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpedisp
{
    /// <summary>
    /// The value of this integer indicates the type of message following the header. 
    /// </summary>
    public enum PDUTypeValues : uint
    {
        /// <summary>
        /// Indicates that this message should be interpreted as a DISPLAYCONTROL_CAPS_PDU structure.
        /// </summary>
        DISPLAYCONTROL_PDU_TYPE_CAPS = 0x00000005,

        /// <summary>
        /// Indicates that this message should be interpreted as a DISPLAYCONTROL_MONITOR_LAYOUT_PDU structure.
        /// </summary>
        DISPLAYCONTROL_PDU_TYPE_MONITOR_LAYOUT = 0x00000002
    }

    /// <summary>
    /// A 32-bit unsigned integer that specifies monitor configuration flags. This number is a bitmask.
    /// </summary>
    [Flags]
    public enum MonitorLayout_FlagValues : uint
    {
        /// <summary>
        /// The monitor specified by this structure is the primary monitor.
        /// </summary>
        DISPLAYCONTROL_MONITOR_PRIMARY = 0x00000001
    }

    /// <summary>
    /// A 32-bit unsigned integer that specifies the orientation of the monitor in degrees.
    /// </summary>
    public enum MonitorLayout_OrientationValues : uint
    {
        /// <summary>
        /// The desktop is not rotated.
        /// </summary>
        ORIENTATION_LANDSCAPE = 0,

        /// <summary>
        /// The desktop is rotated clockwise by 90 degrees.
        /// </summary>
        ORIENTATION_PORTRAIT = 90,

        /// <summary>
        /// The desktop is rotated clockwise by 180 degrees.
        /// </summary>
        ORIENTATION_LANDSCAPE_FLIPPED = 180,

        /// <summary>
        /// The desktop is rotated clockwise by 270 degrees.
        /// </summary>
        ORIENTATION_PORTRAIT_FLIPPED = 270
    }

}
