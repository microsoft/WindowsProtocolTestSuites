// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics.CodeAnalysis;

namespace Microsoft.Protocols.TestTools.StackSdk.Networking.Rpce
{
    /// <summary>
    /// A numeric value indicating the level of authentication or 
    /// message protection that remote procedure call (RPC) 
    /// will apply to a specific message exchange. 
    /// For more information, see [C706] section 13.1.2.1 and [MS-RPCE].
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum RpceAuthenticationLevel : byte
    {
        /// <summary>
        /// Same as RPC_C_AUTHN_LEVEL_CONNECT.
        /// </summary>
        RPC_C_AUTHN_LEVEL_DEFAULT = 0x00,

        /// <summary>
        /// No authentication.
        /// </summary>
        RPC_C_AUTHN_LEVEL_NONE = 0x01,

        /// <summary>
        /// Authenticates the credentials of the client and server.
        /// </summary>
        RPC_C_AUTHN_LEVEL_CONNECT = 0x02,

        /// <summary>
        /// Same as RPC_C_AUTHN_LEVEL_PKT.
        /// </summary>
        RPC_C_AUTHN_LEVEL_CALL = 0x03,
    
        /// <summary>
        /// Same as RPC_C_AUTHN_LEVEL_CONNECT but also prevents replay attacks.
        /// </summary>
        RPC_C_AUTHN_LEVEL_PKT = 0x04,

        /// <summary>
        /// Same as RPC_C_AUTHN_LEVEL_PKT but also verifies that none of the 
        /// data transferred between the client and server has been modified.
        /// </summary>
        RPC_C_AUTHN_LEVEL_PKT_INTEGRITY = 0x05,

        /// <summary>
        /// Same as RPC_C_AUTHN_LEVEL_PKT_INTEGRITY but also ensures that the data 
        /// transferred can only be seen unencrypted by the client and the server.
        /// </summary>
        RPC_C_AUTHN_LEVEL_PKT_PRIVACY = 0x06
    }
}
