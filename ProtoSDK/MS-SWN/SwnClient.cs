// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.Security.Sspi;
using Microsoft.Protocols.TestTools.StackSdk.Networking.Rpce;

namespace Microsoft.Protocols.TestTools.StackSdk.Swn
{
    /// <summary>
    /// SWN client class.
    /// </summary>
    public class SwnClient : IDisposable
    {
        #region Fields
        //RPCE client transport
        internal RpceClientTransport rpceClientTransport;
        
        //RPCE timeout for bind/call
        private TimeSpan rpceTimeout;

        // Store the format for the response of asynchronous calls
        private Dictionary<uint, RpceResponseFormat> responseFormat = new Dictionary<uint, RpceResponseFormat>();

        private object responseLock = new object();
        #endregion

        #region Properties

        /// <summary>
        /// RPCE handle
        /// </summary>
        public IntPtr Handle
        {
            get
            {
                return rpceClientTransport.Handle;
            }
        }
        #endregion

        #region Constructor
        #endregion

        #region IDisposable members
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
                SwnUnbind(new TimeSpan(0, 0, 0, 0, 10));
            }
        }

        /// <summary>
        /// finalizer
        /// </summary>
        ~SwnClient()
        {
            Dispose(false);
        }
        #endregion

        #region Public Functions
        /// <summary>
        /// Bind SWN to the RPC server.
        /// </summary>
        /// <param name="serverName">SWN server name to bind</param>
        /// <param name="bindCred">Credential to bind SWN server</param>
        /// <param name="secContext">Security provider for RPC</param>
        /// <param name="timeout">Timeout for bind and requests</param>
        /// <param name="authLevel">RPCE authentication level</param>
        /// <returns>Return true if success, or false for fail</returns>
        ///<exception cref="ArgumentNullException">
        /// Thrown when serverName is null or empty.
        /// </exception>
        public bool SwnBind(
            string serverName,
            AccountCredential bindCred,
            ClientSecurityContext secContext,
            RpceAuthenticationLevel authLevel,
            TimeSpan timeout)
        {
            if (string.IsNullOrEmpty(serverName))
            {
                throw new ArgumentNullException("serverName");
            }

            //Query endpoint on SWN server
            ushort[] endpoints = SwnUtility.QueryEndpoints(serverName);

            bool retVal = RpcBind(
                RpceUtility.RPC_OVER_TCPIP_PROTOCOL_SEQUENCE,
                serverName,
                endpoints[0].ToString(),
                bindCred,
                secContext,
                authLevel,
                timeout);

            rpceTimeout = timeout;

            return retVal;

        }

        /// <summary>
        /// RPCE unbind and disconnect.
        /// </summary>
        /// <param name="timeout">
        /// Timeout period.
        /// </param>
        /// <exception cref="InvalidOperationException">
        /// Thrown when RPC has not been bind.
        /// </exception>
        public void SwnUnbind(TimeSpan timeout)
        {
            if (rpceClientTransport != null)
            {
                rpceClientTransport.Unbind(timeout);
                rpceClientTransport.Dispose();
                rpceClientTransport = null;
            }
        }

        /// <summary>
        /// SWN client invoke WitnessrGetInterfaceList method to retrieve information about the interfaces to which witness client connections can be made.
        /// </summary>
        /// <param name="InterfaceList">A pointer to a PWITNESS_INTERFACE_LIST, as specified in section 2.2.1.9.</param>
        /// <returns>Return zero if success, otherwise return nonzero.</returns>
        public int WitnessrGetInterfaceList(out WITNESS_INTERFACE_LIST InterfaceList)
        {
            Int3264[] paramList;
            int retVal = 0; 

            paramList = new Int3264[] {
                IntPtr.Zero,    //out param
                IntPtr.Zero //return value
            };

            using (RpceInt3264Collection outParamList = RpceCall(paramList, (ushort)SWN_OPNUM.WitnessrGetInterfaceList))
            {
                WITNESS_INTERFACE_LIST_RPC rpcList = TypeMarshal.ToStruct<WITNESS_INTERFACE_LIST_RPC>(Marshal.ReadIntPtr(outParamList[0]));
                WITNESS_INTERFACE_INFO[] infoList = new WITNESS_INTERFACE_INFO[rpcList.NumberOfInterfaces];
                int sizeInByte = Marshal.SizeOf(typeof(WITNESS_INTERFACE_INFO));
                for (int i = 0; i < rpcList.NumberOfInterfaces; i++)
                {
                    IntPtr pInfo = IntPtrUtility.Add(rpcList.InterfaceInfo, i * sizeInByte);
                    infoList[i] = (WITNESS_INTERFACE_INFO)Marshal.PtrToStructure(pInfo, typeof(WITNESS_INTERFACE_INFO));
                }

                InterfaceList = new WITNESS_INTERFACE_LIST();
                InterfaceList.NumberOfInterfaces = rpcList.NumberOfInterfaces;
                InterfaceList.InterfaceInfo = infoList;

                retVal = outParamList[paramList.Length - 1].ToInt32();
            }
            return retVal;
        }

        /// <summary>
        /// SWN client invoke WitnessrRegister to register for resource state change notifications of a NetName and IPAddress.
        /// </summary>
        /// <param name="Version">The version of the Witness protocol currently in use by the client.</param>
        /// <param name="NetName">Specifies a null-terminated string that specifies the name of the resource for which the client requires notifications.</param>
        /// <param name="IpAddr">Specifies a null-terminated string that specifies the IP address to which the client application connection is established.</param>
        /// <param name="ClientName">Specifies a null-terminated string that is used to identify the Witness client.</param>
        /// <param name="pContext">A context handle to identifies the client on the server. </param>
        /// <returns>Return zero if success, otherwise return nonzero.</returns>
        public int WitnessrRegister(SwnVersion Version, string NetName, string IpAddr, string ClientName, out IntPtr pContext)
        {
            Int3264[] paramList;
            int retVal = 0;

            SafeIntPtr pNetName = Marshal.StringToHGlobalUni(NetName);
            SafeIntPtr pIpAddr = Marshal.StringToHGlobalUni(IpAddr);
            SafeIntPtr pClientName = Marshal.StringToHGlobalUni(ClientName);

            paramList = new Int3264[] {
                IntPtr.Zero,//out param
                (uint)Version,
                pNetName,
                pIpAddr,            
                pClientName,    
                IntPtr.Zero //return value
            };

            try
            {
                using (RpceInt3264Collection outParamList = RpceCall(paramList, (ushort)SWN_OPNUM.WitnessrRegister))
                {
                    pContext = Marshal.ReadIntPtr(outParamList[0]);
                    retVal = outParamList[paramList.Length - 1].ToInt32();
                }
            }
            finally
            {
                pNetName.Dispose();
                pIpAddr.Dispose();
                pClientName.Dispose();
            }

            return retVal;
        }

        /// <summary>
        /// SWN client invoke WitnessrRegisterEx to register for resource state change notifications of a NetName and IPAddress.
        /// </summary>
        /// <param name="Version">The version of the Witness protocol currently in use by the client.</param>
        /// <param name="NetName">Specifies a null-terminated string that specifies the name of the resource for which the client requires notifications.</param>
        /// <param name="ShareName">Specifies a null-terminated string that specifies the name of the share resource for which the client requires notifications.</param>
        /// <param name="IpAddr">Specifies a null-terminated string that specifies the IP address to which the client application connection is established.</param>
        /// <param name="ClientName">Specifies a null-terminated string that is used to identify the Witness client.</param>
        /// <param name="Flags">Specifies the type of Witness registration.</param>
        /// <param name="KeepAliveTimout">Specifies the maximum number of milliseconds for any notification response from the server.</param>
        /// <param name="pContext">A context handle to identifies the client on the server. </param>
        /// <returns>Return zero if success, otherwise return nonzero.</returns>
        public int WitnessrRegisterEx(SwnVersion Version, string NetName, string ShareName, string IpAddr, string ClientName, WitnessrRegisterExFlagsValue Flags, uint KeepAliveTimout, out IntPtr pContext)
        {
            Int3264[] paramList;
            int retVal = 0;

            SafeIntPtr pNetName = Marshal.StringToHGlobalUni(NetName);
            SafeIntPtr pShareName = Marshal.StringToHGlobalUni(ShareName);
            SafeIntPtr pIpAddr = Marshal.StringToHGlobalUni(IpAddr);
            SafeIntPtr pClientName = Marshal.StringToHGlobalUni(ClientName);

            paramList = new Int3264[] {
                IntPtr.Zero,//out param
                (uint)Version,
                pNetName,
                pShareName,
                pIpAddr,            
                pClientName, 
                (uint)Flags,
                KeepAliveTimout,
                IntPtr.Zero //return value
            };

            try
            {
                using (RpceInt3264Collection outParamList = RpceCall(paramList, (ushort)SWN_OPNUM.WitnessrRegisterEx))
                {
                    pContext = Marshal.ReadIntPtr(outParamList[0]);
                    retVal = outParamList[paramList.Length - 1].ToInt32();
                }
            }
            finally
            {
                pNetName.Dispose();
                pShareName.Dispose();
                pIpAddr.Dispose();
                pClientName.Dispose();
            }

            return retVal;
        }

        /// <summary>
        /// SWN client invoke WitnessrUnRegister to unregister for notifications from the server.
        /// </summary>
        /// <param name="pContext">A context handle to identifies the client on the server.</param>
        /// <returns>Return zero if success, otherwise return nonzero.</returns>
        public int WitnessrUnRegister(IntPtr pContext)
        {
            Int3264[] paramList;
            int retVal = 0;

            paramList = new Int3264[] {
                pContext,
                IntPtr.Zero //return value
            };

            using (RpceInt3264Collection outParamList = RpceCall(paramList, (ushort)SWN_OPNUM.WitnessrUnRegister))
            {
                retVal = outParamList[paramList.Length - 1].ToInt32();
            }

            return retVal;
        }

        /// <summary>
        /// SWN client invoke WitnessrAsyncNotify method to post a message on the server; this message is completed when there are no longer any notifications which are required to be communicated with the client.
        /// </summary>
        /// <param name="pContext">A context handle to identifies the client on the server.</param>
        /// <returns>The identifier of this async call.</returns>
        public uint WitnessrAsyncNotify(IntPtr pContext)
        {
            Int3264[] paramList;

            paramList = new Int3264[] {
                pContext,
                IntPtr.Zero, //out param
                IntPtr.Zero //return value
            };

            return RpceAsyncCall(paramList, (ushort)SWN_OPNUM.WitnessrAsyncNotify);
        }

        /// <summary>
        /// SWN client invoke WitnessrAsyncNotify method to post a message on the server; this message is completed when there are no longer any notifications which are required to be communicated with the client.
        /// </summary>
        /// <param name="callId">The identifier of this async call.</param>
        /// <param name="NotifyResp">Structure contains the resource change notification from server.</param>
        /// <returns>Return zero if success, otherwise return nonzero.</returns>
        public int ExpectWitnessrAsyncNotify(uint callId, out RESP_ASYNC_NOTIFY NotifyResp)
        {
            int retVal = 0;

            using (RpceInt3264Collection outParamList = RpceAsyncCallExpect(callId))
            {
                retVal = outParamList[2].ToInt32();
                if (retVal == (int)SwnErrorCode.ERROR_SUCCESS)
                {
                    NotifyResp = TypeMarshal.ToStruct<RESP_ASYNC_NOTIFY>(Marshal.ReadIntPtr(outParamList[1]));
                }
                else
                {
                    NotifyResp = new RESP_ASYNC_NOTIFY();
                }
            }

            return retVal;
        }
        #endregion
        
        #region Private Functions
        /// <summary>
        /// RPC bind to interface, using specified endpoint and authenticate provider.
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
        /// <param name="transportCred">
        /// Credential to bind SWN server
        /// </param>
        /// <param name="secContext">
        /// RPC security provider.
        /// </param>
        /// <param name="authLevel">
        /// RPC authentication level.
        /// </param>
        /// <param name="timeout">
        /// Timeout
        /// </param>
        /// <exception cref="InvalidOperationException">
        /// Thrown when there's existing connection.
        /// </exception>
        private bool RpcBind(
            string protocolSequence,
            string networkAddress,
            string endpoint,
            AccountCredential transportCred,
            ClientSecurityContext secContext,
            RpceAuthenticationLevel authLevel,
            TimeSpan timeout)
        {
            if (rpceClientTransport != null)
            {
                throw new InvalidOperationException("Connection has been established");
            }

            rpceTimeout = timeout;
            rpceClientTransport = new RpceClientTransport();


            try
            {
                rpceClientTransport.Bind(
                    protocolSequence,
                    networkAddress,
                    endpoint,
                    transportCred,
                    SwnUtility.SWN_INTERFACE_UUID,
                    SwnUtility.SWN_INTERFACE_MAJOR_VERSION,
                    SwnUtility.SWN_INTERFACE_MINOR_VERSION,
                    secContext,
                    authLevel,
                    true,
                    rpceTimeout);
            }
            catch (Exception)
            {
                rpceClientTransport.Dispose();
                rpceClientTransport = null;
                throw;
            }
            
            return true;
            
        }

        /// <summary>
        /// The common method of all the SWN calls
        /// </summary>
        /// <param name="paramList">input param list to decode</param>
        /// <param name="opnum">opnum of the current SWN calls</param>
        /// <returns>the decoded paramlist</returns>
        private RpceInt3264Collection RpceCall(Int3264[] paramList, ushort opnum)
        {
            RpceInt3264Collection outParamList = null;
            byte[] requestStub;
            byte[] responseStub;

            requestStub = RpceStubEncoder.ToBytes(
                RpceStubHelper.GetPlatform(),
                SwnStubFormatString.TypeFormatString,
                null,
                SwnStubFormatString.ProcFormatString,
                SwnStubFormatString.ProcFormatStringOffsetTable[opnum],
                true,
                paramList);

            rpceClientTransport.Call(opnum, requestStub, rpceTimeout, out responseStub);

            outParamList = RpceStubDecoder.ToParamList(
                RpceStubHelper.GetPlatform(),
                SwnStubFormatString.TypeFormatString,
                null,
                SwnStubFormatString.ProcFormatString,
                SwnStubFormatString.ProcFormatStringOffsetTable[opnum],
                true,
                responseStub,
                rpceClientTransport.Context.PackedDataRepresentationFormat,
                paramList);

            return outParamList;
        }

        /// <summary>
        /// The common method of all the SWN async calls
        /// </summary>
        /// <param name="paramList">input param list to decode</param>
        /// <param name="opnum">opnum of the current SWN calls</param>
        /// <returns>The identifier of this async call.</returns>
        private uint RpceAsyncCall(Int3264[] paramList, ushort opnum)
        {
            byte[] requestStub;
            uint callId;

            requestStub = RpceStubEncoder.ToBytes(
                RpceStubHelper.GetPlatform(),
                SwnStubFormatString.TypeFormatString,
                null,
                SwnStubFormatString.ProcFormatString,
                SwnStubFormatString.ProcFormatStringOffsetTable[opnum],
                true,
                paramList);

            rpceClientTransport.SendRequest(opnum, requestStub, out callId);

            RpceResponseFormat format = new RpceResponseFormat();
            format.opnum = opnum;
            format.paramList = paramList;

            lock (responseLock)
            {
                responseFormat.Add(callId, format);
            }

            return callId;
        }
        /// <summary>
        /// The common method of all the SWN calls
        /// </summary>
        /// <param name="callId">The identifier of async call.</param>
        /// <returns>The decoded paramlist</returns>
        /// <exception cref="ArgumentException">
        /// Thrown when the call id is invalid.
        /// </exception>
        private RpceInt3264Collection RpceAsyncCallExpect(uint callId)
        {
            RpceInt3264Collection outParamList = null;
            byte[] responseStub;
            RpceResponseFormat format;

            lock (responseLock)
            {
                if (!responseFormat.TryGetValue(callId, out format))
                {
                    throw new ArgumentException("Invalid call id.");
                }
            }

            rpceClientTransport.RecvResponse(callId, rpceTimeout, out responseStub);

            outParamList = RpceStubDecoder.ToParamList(
                RpceStubHelper.GetPlatform(), 
                SwnStubFormatString.TypeFormatString,
                null,
                SwnStubFormatString.ProcFormatString,
                SwnStubFormatString.ProcFormatStringOffsetTable[format.opnum],
                true,
                responseStub,
                rpceClientTransport.Context.PackedDataRepresentationFormat,
                format.paramList);

            lock (responseLock)
            {
                responseFormat.Remove(callId);
            }

            return outParamList;
        }

        #endregion

    }
}
