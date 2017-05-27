// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace Microsoft.Protocols.TestSuites.MS_FRS2
{


    #region enum
    /// <summary>
    /// Specifies an enum which represents the authentication level used for RPC binding
    /// </summary>
    public enum RPC_C_AUTHN_LEVEL
    {
        /// <summary>
        /// Tells DCOM to choose the authentication level using its normal security blanket
        /// negotiation algorithm.
        ///This behavior occurs in Windows 2000 and later versions.
        ///In Windows NT 4.0, this value defaults to RPC_C_AUTHN_LEVEL_CONNECT.
        /// </summary>
        RPC_C_AUTHN_LEVEL_DEFAULT = 0,

        /// <summary>
        /// Performs no authentication.
        /// </summary>
        RPC_C_AUTHN_LEVEL_NONE = 1,

        /// <summary>
        /// Authenticates the credentials of the client only when the client establishes a relationship
        /// with the server.
        /// Datagram transports always use RPC_AUTHN_LEVEL_PKT instead
        /// </summary>
        RPC_C_AUTHN_LEVEL_CONNECT = 2,

        /// <summary>
        /// Authenticates only at the beginning of each remote procedure call when the server receives
        /// the request.Datagram transports use RPC_C_AUTHN_LEVEL_PKT instead. 
        /// </summary>
        RPC_C_AUTHN_LEVEL_CALL = 3,

        /// <summary>
        /// Authenticates that all data received is from the expected client. 
        /// </summary>
        RPC_C_AUTHN_LEVEL_PKT = 4,


        /// <summary>
        /// Authenticates and verifies that none of the data transferred between client and server
        /// has been modified.
        /// </summary>
        RPC_C_AUTHN_LEVEL_PKT_INTEGRITY = 5,

        /// <summary>
        /// Authenticates all previous levels and encrypts the argument value of each remote
        /// procedure call.
        /// </summary>
        RPC_C_AUTHN_LEVEL_PKT_PRIVACY = 6
    }

    /// <summary>
    /// Specifies the authentication service used for RPC binding
    /// </summary>
    public enum RPC_C_AUTHN_SVC
    {
        /// <summary>
        /// Specifies that RPC_C_AUTHN_NONE is set
        /// </summary>
        RPC_C_AUTHN_NONE = 0,
        /// <summary>
        /// Specifies that RPC_C_AUTHN_DCE_PRIVATE is set
        /// </summary>
        RPC_C_AUTHN_DCE_PRIVATE = 1,
        /// <summary>
        /// Specifies that RPC_C_AUTHN_DCE_PUBLIC is set
        /// </summary>
        RPC_C_AUTHN_DCE_PUBLIC = 2,
        /// <summary>
        /// Specifies that RPC_C_AUTHN_DEC_PUBLIC is set
        /// </summary>
        RPC_C_AUTHN_DEC_PUBLIC = 4,
        /// <summary>
        /// Specifies that RPC_C_AUTHN_GSS_NEGOTIATE is set
        /// </summary>
        RPC_C_AUTHN_GSS_NEGOTIATE = 9,
        /// <summary>
        /// Specifies that RPC_C_AUTHN_WINNT is set
        /// </summary>
        RPC_C_AUTHN_WINNT = 10,
        /// <summary>
        /// Specifies that RPC_C_AUTHN_GSS_SCHANNEL is set
        /// </summary>
        RPC_C_AUTHN_GSS_SCHANNEL = 14,
        /// <summary>
        /// Specifies that RPC_C_AUTHN_GSS_KERBEROS is set
        /// </summary>
        RPC_C_AUTHN_GSS_KERBEROS = 16,
        /// <summary>
        /// Specifies that RPC_C_AUTHN_MSN is set
        /// </summary>
        RPC_C_AUTHN_MSN = 17,
        /// <summary>
        /// Specifies that RPC_C_AUTHN_DPA is set
        /// </summary>
        RPC_C_AUTHN_DPA = 18,
        /// <summary>
        /// Specifies that RPC_C_AUTHN_MQ is set
        /// </summary>
        RPC_C_AUTHN_MQ = 100,
        //RPC_C_AUTHN_DEFAULT = 0xFFFFFFFFL,
    }
    #endregion
    /// <summary>
    /// Class RPC Binding.
    /// </summary>
    class RPCBinding
    {
        const string RPC_RuntimeDllName = "rpcrt4.dll";

        #region RPC Runtime Methods
        [DllImport(RPC_RuntimeDllName)]
        public extern static UInt32 RpcStringBindingCompose(
            string ObjUuid,
            string Protseq,
            string NetworkAddr,
            string Endpoint,
            string Options,
            out string szStringBinding);

        [DllImport(RPC_RuntimeDllName)]
        public extern static UInt32 RpcBindingFromStringBinding(
                                             string StringBinding,
                                             out IntPtr Binding);

        [DllImport(RPC_RuntimeDllName)]
        public extern static uint RpcBindingSetAuthInfo(
                                         IntPtr Binding,
                                         string ServerPrincName,
                                         UInt32 AuthnLevel,
                                         UInt32 AuthnSvc,
                                         ref SEC_WINNT_AUTH_IDENTITY AuthIdentity,
                                         UInt32 AuthzService);


        #endregion

    }
}
