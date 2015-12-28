// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics.CodeAnalysis;

namespace Microsoft.Protocols.TestTools.StackSdk.Networking.Rpce
{
    /// <summary>
    /// These extensions do not require support for the dce_c_rpc_authn_protocol_krb5 
    /// security provider, as specified in [C706] section 13. All of the requirements 
    /// specified in [C706] section 13 are removed by these extensions.<para/>
    /// These extensions specify the following values for the security provider.<para/>
    /// On the client side, if the higher level protocol requests RPC_C_AUTHN_DEFAULT, 
    /// the implementation MUST use RPC_C_AUTHN_WINNT instead.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum RpceAuthenticationType : byte
    {
        /// <summary>
        /// No Authentication
        /// </summary>
        RPC_C_AUTHN_NONE = 0,

        /// <summary>
        /// SPNEGO
        /// </summary>
        RPC_C_AUTHN_GSS_NEGOTIATE = 0x09,

        /// <summary>
        /// NTLM
        /// </summary>
        RPC_C_AUTHN_WINNT = 0x0A,

        /// <summary>
        /// Kerberos
        /// </summary>
        RPC_C_AUTHN_GSS_KERBEROS = 0x10,

        /// <summary>
        /// Netlogon
        /// </summary>
        RPC_C_AUTHN_NETLOGON = 0x44,

        /// <summary>
        /// Same as RPC_C_AUTHN_WINNT
        /// </summary>
        RPC_C_AUTHN_DEFAULT = 0xFF
    }
}
