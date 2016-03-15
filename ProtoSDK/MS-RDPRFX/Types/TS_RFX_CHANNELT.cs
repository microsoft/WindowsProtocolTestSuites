// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Diagnostics.CodeAnalysis;

namespace Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdprfx
{
    /// <summary>
    /// The TS_RFX_CHANNELT structure is used to specify the screen resolution of a channel.
    /// </summary>
    public struct TS_RFX_CHANNELT
    {
        /// <summary>
        /// Specifies the identifier of the channel.
        /// </summary>
        public byte channelId;

        /// <summary>
        /// Specifies the frame width of the channel.
        /// </summary>
        public short width;

        /// <summary>
        /// Specifies the frame height of the channel.
        /// </summary>
        public short height;
    }
}
