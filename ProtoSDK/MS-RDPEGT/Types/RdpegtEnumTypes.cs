// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpedyc;

namespace Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpegt
{
    /// <summary>
    /// The possible values of the version of the Remote Desktop Protocol: Geometry Tracking Virtual Channel Extension.
    /// </summary>
    public enum RdpegtVersionValues : uint
    {
        /// <summary>
        /// In RDP8, this MUST be set to 0x01.
        /// </summary>
        RDP8 = 0x01,
    }

    /// <summary>
    /// UINT32. A number that identifies which operation the client is to perform.
    /// </summary>
    public enum UpdateTypeValues : uint
    {
        /// <summary>
        /// Identifies client should update the geometry.
        /// </summary>
        GEOMETRY_UPDATE = 0x01,

        /// <summary>
        /// Identifies client should clear the geometry.
        /// </summary>
        GEOMETRY_CLEAR = 0x02
    }

    /// <summary>
    /// Identifies the type of the geometry.
    /// </summary>
    public enum GeometryTypeValues : uint
    {
        /// <summary>
        /// This MUST be set to 0x02 in RDP8.
        /// </summary>
        RDP8 = 0x02
    }

}
