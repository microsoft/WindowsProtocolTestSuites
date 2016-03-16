// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Diagnostics.CodeAnalysis;

namespace Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdprfx
{
    /// <summary>
    /// The TS_RFX_RECT structure is used to specify a rectangle.
    /// </summary>
    public struct TS_RFX_RECT
    {
        /// <summary>
        /// The X-coordinate of the rectangle.
        /// </summary>
        public ushort x;

        /// <summary>
        /// The Y-coordinate of the rectangle.
        /// </summary>
        public ushort y;

        /// <summary>
        /// The width of the rectangle.
        /// </summary>
        public ushort width;

        /// <summary>
        /// The height of the rectangle.
        /// </summary>
        public ushort height;
    }
}
