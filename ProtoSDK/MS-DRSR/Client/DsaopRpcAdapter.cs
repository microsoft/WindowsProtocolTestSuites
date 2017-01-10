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
    /// The implementation of IDrsrRpcAdapter
    /// </summary>  
    internal class DsaopRpcAdapter : IDsaopRpcAdapter, IDisposable
    {
        // RPCE client transport
        internal RpceClientTransport rpceClientTransport;

        // Timeout for RPC bind/call
        private TimeSpan rpceTimeout;

        // Return value for DRSR methods
        private uint retVal;


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
        public void Bind(
            string protocolSequence,
            string networkAddress,
            string endpoint,
            ClientSecurityContext securityContext,
            RpceAuthenticationLevel authenticationLevel,
            TimeSpan timeout)
        {
            if (rpceClientTransport != null)
            {
                throw new InvalidOperationException("DRSR has already been bind.");
            }

            rpceTimeout = timeout;

            rpceClientTransport = new RpceClientTransport();

            rpceClientTransport.Bind(
                protocolSequence,
                networkAddress,
                endpoint,
                null,
                DrsrUtility.DSAOP_RPC_INTERFACE_UUID,
                DrsrUtility.DSAOP_RPC_INTERFACE_MAJOR_VERSION,
                DrsrUtility.DSAOP_RPC_INTERFACE_MINOR_VERSION,
                securityContext,
                authenticationLevel,
                false,
                rpceTimeout);
        }



        /// <summary>
        /// RPC unbind.
        /// </summary>
        public void Unbind()
        {
            if (rpceClientTransport != null)
            {
                rpceClientTransport.Unbind(rpceTimeout);
                rpceClientTransport = null;
            }
        }


        /// <summary>
        /// RPC handle.
        /// </summary>
        public IntPtr Handle
        {
            get
            {
                return rpceClientTransport.Handle;
            }
        }


        #region Drsr dsaop methods

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
        public uint IDL_DSAPrepareScript(
              IntPtr hRpc,
              uint dwInVersion,
            //[Switch("dwInVersion")]
              DSA_MSG_PREPARE_SCRIPT_REQ? pmsgIn,
              out uint? pdwOutVersion,
            //[Switch("*pdwOutVersion")] 
              out DSA_MSG_PREPARE_SCRIPT_REPLY? pmsgOut)
        {
            const ushort opnum = 0;

            byte[] requestStub;
            byte[] responseStub;
            Int3264[] paramList;

            SafeIntPtr ptrMsgIn = TypeMarshal.ToIntPtr(pmsgIn, dwInVersion, null, null);

            paramList = new Int3264[] 
            {
                dwInVersion,
                ptrMsgIn,
                IntPtr.Zero,
                IntPtr.Zero,
                0//retVal
            };

            requestStub = RpceStubEncoder.ToBytes(
                    RpceStubHelper.GetPlatform(),
                    DrsrRpcStubFormatString.TypeFormatString,
                    null,
                    DrsrRpcStubFormatString.ProcFormatString,
                    DrsrRpcStubFormatString.Dsaop_ProcFormatStringOffsetTable[opnum],
                    true,
                    paramList);

            rpceClientTransport.Call(opnum, requestStub, rpceTimeout, out responseStub);

            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
                    RpceStubHelper.GetPlatform(),
                    DrsrRpcStubFormatString.TypeFormatString,
                    null,
                    DrsrRpcStubFormatString.ProcFormatString,
                    DrsrRpcStubFormatString.Dsaop_ProcFormatStringOffsetTable[opnum],
                    true,
                    responseStub,
                    rpceClientTransport.Context.PackedDataRepresentationFormat,
                    paramList))
            {
                pdwOutVersion = TypeMarshal.ToNullableStruct<uint>(outParamList[2]);
                pmsgOut = TypeMarshal.ToNullableStruct<DSA_MSG_PREPARE_SCRIPT_REPLY>(
                    outParamList[3], pdwOutVersion, null, null);
                retVal = outParamList[4].ToUInt32();
            }

            ptrMsgIn.Dispose();

            return retVal;
        }


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
        public uint IDL_DSAExecuteScript(
              IntPtr hRpc,
              uint dwInVersion,
            //[Switch("dwInVersion")]
              DSA_MSG_EXECUTE_SCRIPT_REQ? pmsgIn,
              out uint? pdwOutVersion,
            //[Switch("*pdwOutVersion")]
              out DSA_MSG_EXECUTE_SCRIPT_REPLY? pmsgOut)
        {
            const ushort opnum = 1;

            byte[] requestStub;
            byte[] responseStub;
            Int3264[] paramList;

            SafeIntPtr ptrMsgIn = TypeMarshal.ToIntPtr(pmsgIn, dwInVersion, null, null);

            paramList = new Int3264[] 
            {
                dwInVersion,
                ptrMsgIn,
                IntPtr.Zero,
                IntPtr.Zero,
                0//retVal
            };

            requestStub = RpceStubEncoder.ToBytes(
                    RpceStubHelper.GetPlatform(),
                    DrsrRpcStubFormatString.TypeFormatString,
                    null,
                    DrsrRpcStubFormatString.ProcFormatString,
                    DrsrRpcStubFormatString.Dsaop_ProcFormatStringOffsetTable[opnum],
                    true,
                    paramList);

            rpceClientTransport.Call(opnum, requestStub, rpceTimeout, out responseStub);

            using (RpceInt3264Collection outParamList = RpceStubDecoder.ToParamList(
                    RpceStubHelper.GetPlatform(),
                    DrsrRpcStubFormatString.TypeFormatString,
                    null,
                    DrsrRpcStubFormatString.ProcFormatString,
                    DrsrRpcStubFormatString.Dsaop_ProcFormatStringOffsetTable[opnum],
                    true,
                    responseStub,
                    rpceClientTransport.Context.PackedDataRepresentationFormat,
                    paramList))
            {
                pdwOutVersion = TypeMarshal.ToNullableStruct<uint>(outParamList[2]);
                pmsgOut = TypeMarshal.ToNullableStruct<DSA_MSG_EXECUTE_SCRIPT_REPLY>(
                    outParamList[3], pdwOutVersion, null, null);
                retVal = outParamList[4].ToUInt32();
            }

            ptrMsgIn.Dispose();

            return retVal;
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
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // Release managed resources.
                if (rpceClientTransport != null)
                {
                    rpceClientTransport.Dispose();
                    rpceClientTransport = null;
                }
            }

            // Release unmanaged resources.
        }


        /// <summary>
        /// finalizer
        /// </summary>
        ~DsaopRpcAdapter()
        {
            Dispose(false);
        }

        #endregion
    }
}
