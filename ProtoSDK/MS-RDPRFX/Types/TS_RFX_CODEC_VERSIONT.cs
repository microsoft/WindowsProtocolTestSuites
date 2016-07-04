// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Diagnostics.CodeAnalysis;

namespace Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdprfx
{
    /// <summary>
    /// The TS_RFX_CODEC_VERSIONT structure is used to specify support for a specific
    /// version of the RemoteFX codec.
    /// </summary>
    public struct TS_RFX_CODEC_VERSIONT
    {
        /// <summary>
        /// Specifies the codec through its identifier.
        /// </summary>
        public byte codecId;

        /// <summary>
        /// Specifies the version of the codec.
        /// </summary>
        public short version;
    }
}
