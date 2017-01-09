// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.


using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

using Microsoft.Protocols.TestTools.StackSdk.Networking.Rpce;
using Microsoft.Protocols.TestTools.StackSdk.Security.Sspi;

namespace Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Drsr
{
    /// <summary>
    /// DRSR client for dsaop methods.
    /// You can call following methods from this class.<para/>
    /// RPC bind methods.<para/>
    /// DRSR RPC methods.<para/>
    /// </summary>
    public class DsaopClient : IDisposable
    {

        //Actual rpc adapter
        private IDsaopRpcAdapter rpcAdapter;


        #region Constructor

        /// <summary>
        /// Constructor, initialize a DRSR client.<para/>
        /// Create the instance will not connect to server, you should call BindOverTcp to
        /// actually connect to DRSR server.
        /// </summary>
        public DsaopClient()
        {
            rpcAdapter = new DsaopRpcAdapter();
        }

        #endregion


        #region RPC bind methods

        /// <summary>
        /// RPC bind over TCP/IP, using specified endpoint and authenticate provider.
        /// using static endpoint (AD DS?)
        /// </summary>
        /// <param name="serverName">DRSR server machine name.</param>
        /// <param name="securityContext">
        /// Security provider for RPC. 
        /// Set the value to null to disable authentication.
        /// </param>
        /// <param name="authenticationLevel">
        /// Authentication level for RPC. 
        /// </param>
        /// <param name="timeout">Timeout for bind and all future requests.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when serverName is null.
        /// </exception>
        /// <returns>DrsrClientSessionContext after tcp bind</returns>
        [CLSCompliant(false)]
        public DrsrClientSessionContext BindOverTcp(
            string serverName,
            ClientSecurityContext securityContext,
            RpceAuthenticationLevel authenticationLevel,
            TimeSpan timeout)
        {
            DrsrClientSessionContext clientSessionContext = new DrsrClientSessionContext();

            if (serverName == null)
            {
                throw new ArgumentNullException("serverName");
            }

            this.rpcAdapter.Bind(
                RpceUtility.RPC_OVER_TCPIP_PROTOCOL_SEQUENCE,
                serverName,
                DrsrUtility.QueryDrsrTcpEndpoint(DrsrRpcInterfaceType.DSAOP, serverName)[0].ToString(),
                securityContext,
                authenticationLevel,
                timeout);

            clientSessionContext.RPCHandle = rpcAdapter.Handle;

            return clientSessionContext;
        }

        /// <summary>
        /// TO Do: need to test whether the dynamic endpoint can be queried by DrsrUtility.QueryDrsrTcpEndpoint
        /// RPC bind over TCP/IP, using specified endpoint and authenticate provider.
        /// using dynamic endpoint(AD LDS?)
        /// </summary>
        /// <param name="serverName">DRSR server machine name.</param>
        /// <param name="servicePrincipalName">DRSR server machine Principal name.</param>
        /// <param name="clientGuid">DRS client Guid</param>
        /// <param name="protocolSequence">protocol sequence type</param>
        /// <param name="endPoint">DRSR endpoint, the value can be null when it is AD LDS?</param>
        /// <param name="networkOptions">a string representation of network options. The option string is associated with the protocol sequence.
        ///                              It is used in RpcStringBindingCompose</param>
        /// <param name="securityContext">
        /// Security provider for RPC. 
        /// Set the value to null to disable authentication.
        /// </param>
        /// <param name="authenticationLevel">
        /// Authentication level for RPC. 
        /// </param>
        /// <param name="timeout">Timeout for bind and all future requests.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when serverName is null.
        /// Thrown when servicePrincipalName is null.
        /// </exception>
        /// <returns>DrsrClientSessionContext after tcp bind</returns>
        [CLSCompliant(false)]
        public DrsrClientSessionContext BindOverTcp(
            string serverName,
            string servicePrincipalName,
            Guid? clientGuid,
            String protocolSequence,
            String endPoint,
            String networkOptions,
            ClientSecurityContext securityContext,
            RpceAuthenticationLevel authenticationLevel,
            TimeSpan timeout)
        {
            DrsrClientSessionContext clientSessionContext = new DrsrClientSessionContext();

            uint endpoint;
            endpoint = Convert.ToUInt16(this.InquiryAdldsDynamicPort(servicePrincipalName, clientGuid, protocolSequence, serverName, endPoint, networkOptions));

            if (serverName == null)
            {
                throw new ArgumentNullException("serverName");
            }

            if (servicePrincipalName == null)
            {
                throw new ArgumentNullException("servicePrincipalName");
            }

            this.rpcAdapter.Bind(
                RpceUtility.RPC_OVER_TCPIP_PROTOCOL_SEQUENCE,
                serverName,
                endpoint.ToString(),
                securityContext,
                authenticationLevel,
                timeout);

            clientSessionContext.RPCHandle = rpcAdapter.Handle;

            return clientSessionContext;
        }

        /// <summary>
        /// Create DsaExecute Script Request message
        /// </summary>
        /// <param name="pbPassword">The password</param>
        /// <returns>DSA_MSG_EXECUTE_SCRIPT_REQ request in message</returns>
        [CLSCompliant(false)]
        public DSA_MSG_EXECUTE_SCRIPT_REQ CreateDsaExecuteScriptRequest(byte[] pbPassword)
        {

            DSA_MSG_EXECUTE_SCRIPT_REQ_V1 V1 = new DSA_MSG_EXECUTE_SCRIPT_REQ_V1();
            V1.Flags = Flags_Values.V1;
            V1.pbPassword = pbPassword;
            V1.cbPassword = pbPassword == null ? 0 : (uint)pbPassword.Length;

            DSA_MSG_EXECUTE_SCRIPT_REQ message = new DSA_MSG_EXECUTE_SCRIPT_REQ();
            message.V1 = V1;

            return message;
        }

        /// <summary>
        /// Create Dsa Prepare Script
        /// </summary>
        /// <param name="pbPassword"></param>
        /// <returns>DSA_MSG_PREPARE_SCRIPT_REQ, in message of CreateDsaPrepareScript</returns>
        [CLSCompliant(false)]
        public DSA_MSG_PREPARE_SCRIPT_REQ CreateDsaPrepareScript()
        {
            DSA_MSG_PREPARE_SCRIPT_REQ_V1 V1 = new DSA_MSG_PREPARE_SCRIPT_REQ_V1();
            V1.Reserved = Reserved_Values.V1;


            DSA_MSG_PREPARE_SCRIPT_REQ message = new DSA_MSG_PREPARE_SCRIPT_REQ();
            message.V1 = V1;

            return message;
        }

        #endregion



        #region Raw RPC Call

        /// <summary>
        ///  The DSAPrepareScript method prepares the DC to run a maintenance script. 
        ///  Opnum: 0 
        /// </summary>
        /// <param name="rpcHandle">
        ///  The RPC binding handle, as specified in [C706].
        /// </param>
        /// <param name="dwInVersion">
        ///  The version of the request message.
        /// </param>
        /// <param name="inMessage">
        ///  A pointer to the request message.
        /// </param>
        /// <param name="outVersion">
        ///  A pointer to the version of the response message.
        /// </param>
        /// <param name="outMessage">
        ///  A pointer to the response message.
        /// </param>
        [CLSCompliant(false)]
        public uint DsaPrepareScript(
             DrsrClientSessionContext clientSessionContext,
             uint dwInVersion,
             DSA_MSG_PREPARE_SCRIPT_REQ? inMessage,
             out uint? outVersion,
             out DSA_MSG_PREPARE_SCRIPT_REPLY? outMessage)
        {
            return this.rpcAdapter.IDL_DSAPrepareScript(
                clientSessionContext.RPCHandle,
                dwInVersion,
                inMessage,
                out outVersion,
                out outMessage);
        }


        /// <summary>
        ///  The DSAExecuteScript method executes a maintenance script. 
        ///  Opnum: 1 
        /// </summary>
        /// <param name="rpcHandle">
        ///  The RPC binding handle, as specified in [C706].
        /// </param>
        /// <param name="dwInVersion">
        ///  The version of the request message.
        /// </param>
        /// <param name="inMessage">
        ///  A pointer to the request message.
        /// </param>
        /// <param name="outVersion">
        ///  A pointer to the version of the response message.
        /// </param>
        /// <param name="outMessage">
        ///  A pointer to the response message.
        /// </param>
        [CLSCompliant(false)]
        public uint DsaExecuteScript(
             DrsrClientSessionContext clientSessionContext,
             uint dwInVersion,
             DSA_MSG_EXECUTE_SCRIPT_REQ? inMessage,
             out uint? outVersion,
             out DSA_MSG_EXECUTE_SCRIPT_REPLY? outMessage)
        {
            return this.rpcAdapter.IDL_DSAExecuteScript(
                clientSessionContext.RPCHandle,
                dwInVersion,
                inMessage,
                out outVersion,
                out outMessage);
        }

        #endregion

        #region private method
        /// <summary>
        /// Get the dynamic port which is bound to the ADLDS service.
        /// </summary>
        /// <param name="servicePrincipalName">DRSR server machine Principal name.</param>
        /// <param name="clientGuid">DRS client Guid</param>
        /// <param name="protocolSequence">protocol sequence type</param>
        /// <param name="serverComputerName">DRSR server machine computer name</param>
        /// <param name="endPoint">DRSR endpoint, the value can be null when it is AD LDS?</param>
        /// <param name="networkOptions">a string representation of network options. The option string is associated with the protocol sequence.
        ///                              It is used in RpcStringBindingCompose</param>
        /// <exception cref="System.InvalidOperationException">Fail to PInvoke.</exception>
        /// <returns>dynamic port.</returns>
        [SuppressMessage("Microsoft.Usage", "CA1806:DoNotIgnoreMethodResults")]
        private string InquiryAdldsDynamicPort(String servicePrincipalName, Guid? clientGuid, String protocolSequence, String serverComputerName, String endPoint, String networkOptions)
        {
            //Get the ldap port of ADLDS service:
            int index = servicePrincipalName.IndexOf(':');
            if (index < 0)
            {
                throw new InvalidOperationException("The LDAP port should be provided when SUT is ADLDS.");
            }
            string ldapPort = servicePrincipalName.Substring(index + 1);

            IntPtr bindingString = IntPtr.Zero;
            IntPtr bindingHandle = IntPtr.Zero;
            uint status = RpcNativeMethods.RpcStringBindingCompose(
                (clientGuid == null) ?
                    null : clientGuid.Value.ToString(),
                protocolSequence,
                serverComputerName,
                endPoint,
                networkOptions,
                out bindingString);
            if (status != 0)
            {
                throw new InvalidOperationException("Failed to RpcStringBindingCompose. The returned status is "
                    + status);
            }

            status = RpcNativeMethods.RpcBindingFromStringBinding(
                bindingString,
                out bindingHandle);
            if (status != 0)
            {
                RpcNativeMethods.RpcBindingFree(ref bindingString);
                throw new InvalidOperationException("Failed to RpcBindingFromStringBinding. The returned status is "
                    + status);
            }

            RpcNativeMethods.RpcBindingFree(ref bindingString);

            IntPtr inquiryContext = IntPtr.Zero;
            Guid uuid = (clientGuid == null) ?
                new Guid() : clientGuid.Value;
            RPC_IF_ID rpcInterfaceInfo = new RPC_IF_ID();
            rpcInterfaceInfo.Uuid = DrsrUtility.DRSUAPI_RPC_INTERFACE_UUID;
            rpcInterfaceInfo.VersMajor = (short)RPC_C_VERS.MAJOR_ONLY;
            status = RpcNativeMethods.RpcMgmtEpEltInqBegin(
                bindingHandle,
                RPC_C_EP.MATCH_BY_IF,
                ref rpcInterfaceInfo,
                RPC_C_VERS.COMPATIBLE,
                ref uuid,
                out inquiryContext);
            if (status != 0)
            {
                RpcNativeMethods.RpcBindingFree(ref bindingString);
                throw new InvalidOperationException("Failed to RpcMgmtEpEltInqBegin. The returned status is "
                    + status);
            }

            //the loop will break when the dynamic port bound to the ADLDS is found. Otherwise, thrown exception:
            while (true)
            {
                Guid queriedUuid = new Guid();
                IntPtr queriedBindingHandle = IntPtr.Zero;
                IntPtr queriedAnnotation = IntPtr.Zero;
                status = RpcNativeMethods.RpcMgmtEpEltInqNext(
                    inquiryContext,
                    ref rpcInterfaceInfo,
                    out queriedBindingHandle,
                    out queriedUuid,
                    out queriedAnnotation);
                if (status != 0)
                {
                    RpcNativeMethods.RpcMgmtEpEltInqDone(ref inquiryContext);
                    RpcNativeMethods.RpcBindingFree(ref bindingString);
                    throw new InvalidOperationException("Failed to RpcMgmtEpEltInqNext. The returned status is "
                        + status);
                }

                //the LDAP port is found:
                if (Marshal.PtrToStringAnsi(queriedAnnotation).Equals(ldapPort))
                {
                    RpcNativeMethods.RpcStringFree(ref queriedAnnotation);
                    RpcNativeMethods.RpcMgmtEpEltInqDone(ref inquiryContext);

                    IntPtr queriedBindingString = IntPtr.Zero;
                    status = RpcNativeMethods.RpcBindingToStringBinding(
                        queriedBindingHandle, out queriedBindingString);
                    RpcNativeMethods.RpcBindingFree(ref queriedBindingHandle);
                    if (status != 0)
                    {
                        RpcNativeMethods.RpcBindingFree(ref bindingHandle);
                        throw new InvalidOperationException("Failed to RpcBindingToStringBinding. The status is "
                            + status);
                    }

                    //get the port from the stringBinding.
                    string dynamicPort = Marshal.PtrToStringAnsi(queriedBindingString);
                    dynamicPort = dynamicPort.Substring(dynamicPort.IndexOf('[') + 1);
                    dynamicPort = dynamicPort.Remove(dynamicPort.Length - 1, 1);

                    RpcNativeMethods.RpcStringFree(ref queriedBindingString);
                    RpcNativeMethods.RpcBindingFree(ref bindingHandle);
                    return dynamicPort;
                }
                else
                {
                    RpcNativeMethods.RpcStringFree(ref queriedAnnotation);
                    RpcNativeMethods.RpcBindingFree(ref queriedBindingHandle);
                }
            }
        }

        #endregion


        #region IDisposable Members

        /// <summary>
        /// Dispose method.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }


        /// <summary>
        /// Dispose method.
        /// </summary>
        /// <param name="disposing">
        /// True to release both managed and unmanaged resources.<para/>
        /// False to release unmanaged resources only.
        /// </param>
        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // Release managed resources.
                if (rpcAdapter != null)
                {
                    try
                    {
                        rpcAdapter.Unbind();
                    }
                    finally
                    {
                        rpcAdapter.Dispose();
                        rpcAdapter = null;
                    }
                }
            }

            // Release unmanaged resources.
        }


        /// <summary>
        /// finalizer
        /// </summary>
        ~DsaopClient()
        {
            Dispose(false);
        }

        #endregion
    }
}

