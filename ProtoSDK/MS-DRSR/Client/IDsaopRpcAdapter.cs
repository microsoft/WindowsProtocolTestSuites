// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics.CodeAnalysis;

using Microsoft.Protocols.TestTools.StackSdk.Networking.Rpce;
using Microsoft.Protocols.TestTools.StackSdk.Security.Sspi;

namespace Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Drsr
{
    /// <summary>
    /// DRSR RPC interface for dsaop methods.
    /// </summary>   
    [CLSCompliant(false)]
    public partial interface IDsaopRpcAdapter : IDisposable
    {
        /// <summary>
        /// Bind to DRSR RPC server.
        /// </summary>
        /// <param name="protocolSequence">
        /// RPC protocol sequence.
        /// </param>
        /// <param name="networkAddress">
        /// RPC network address.
        /// </param>
        /// <param name="endpoint">
        /// RPC endpoint.
        /// </param>
        /// <param name="securityContext">
        /// RPC security provider.
        /// </param>
        /// <param name="authenticationLevel">
        /// RPC authentication level.
        /// </param>
        /// <param name="timeout">
        /// Timeout
        /// </param>
        [CLSCompliant(false)]
        void Bind(
            string protocolSequence,
            string networkAddress,
            string endpoint,
            ClientSecurityContext securityContext,
            RpceAuthenticationLevel authenticationLevel,
            TimeSpan timeout);


        /// <summary>
        /// RPC unbind.
        /// </summary>
        void Unbind();


        /// <summary>
        /// RPC handle.
        /// </summary>
        IntPtr Handle
        {
            get;
        }


        /// <summary>
        ///  The IDL_DSAPrepareScript method prepares the DC to run a maintenance script. 
        ///  Opnum: 0 
        /// </summary>
        /// <param name="hRpc">
        ///  The RPC binding handle, as specified in [C706].
        /// </param>
        /// <param name="dwInVersion">
        ///  The version of the request message.
        /// </param>
        /// <param name="pmsgIn">
        ///  A pointer to the request message.
        /// </param>
        /// <param name="pdwOutVersion">
        ///  A pointer to the version of the response message.
        /// </param>
        /// <param name="pmsgOut">
        ///  A pointer to the response message.
        /// </param>
        [CLSCompliant(false)]
        uint IDL_DSAPrepareScript(
            IntPtr hRpc,
            uint dwInVersion,
            //[Switch("dwInVersion")]
            DSA_MSG_PREPARE_SCRIPT_REQ? pmsgIn,
            out uint? pdwOutVersion,
            //[Switch("*pdwOutVersion")] 
            out DSA_MSG_PREPARE_SCRIPT_REPLY? pmsgOut);


        /// <summary>
        ///  The IDL_DSAExecuteScript method executes a maintenance script. 
        ///  Opnum: 1 
        /// </summary>
        /// <param name="hRpc">
        ///  The RPC binding handle, as specified in [C706].
        /// </param>
        /// <param name="dwInVersion">
        ///  The version of the request message.
        /// </param>
        /// <param name="pmsgIn">
        ///  A pointer to the request message.
        /// </param>
        /// <param name="pdwOutVersion">
        ///  A pointer to the version of the response message.
        /// </param>
        /// <param name="pmsgOut">
        ///  A pointer to the response message.
        /// </param>
        [CLSCompliant(false)]
        uint IDL_DSAExecuteScript(
            IntPtr hRpc,
            uint dwInVersion,
            //[Switch("dwInVersion")]
            DSA_MSG_EXECUTE_SCRIPT_REQ? pmsgIn,
            out uint? pdwOutVersion,
            //[Switch("*pdwOutVersion")]
            out DSA_MSG_EXECUTE_SCRIPT_REPLY? pmsgOut);
    }
}
