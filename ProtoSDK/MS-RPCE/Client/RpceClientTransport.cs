// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Threading;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.Protocols.TestTools.StackSdk.Security.Sspi;

namespace Microsoft.Protocols.TestTools.StackSdk.Networking.Rpce
{
    /// <summary>
    /// RpceClientTransport provides transport layer support 
    /// for RPC bind and methods calls.<para/>
    /// This RPC transport class does support: 
    /// RPC over TCP/IP, RPC over Named Pipes, NDR, PDU fragment, security provider, 
    /// authentication level, verification trailer and call timeout.<para/>
    /// It does not support: 
    /// RPC over other transport layers, partially-bound binding handle, 
    /// data representation other than IEEE big or little endian ASCII,
    /// NDR64, security context multiplexing, concurrent multiplexing, 
    /// callback, and keeping connection open on orphaned.
    /// </summary>
    public class RpceClientTransport : IDisposable
    {
        // RPCE client
        private RpceClient rpceClient;

        // The lock for response
        private object responseLock = new object();

        // RPCE thread to receive the response from server
        private Thread receiveThread;

        // List of outstanding call id
        private Collection<uint> outstandingCallIds;

        /// <summary>
        /// Initialize an instance of RpceClientTransport.
        /// </summary>
        public RpceClientTransport()
        {
            rpceClient = new RpceClient();
            outstandingCallIds = new Collection<uint>();
            receiveThread = null;
        }


        /// <summary>
        /// Gets RPC client handle.
        /// </summary>
        public virtual IntPtr Handle
        {
            get
            {
                return rpceClient.Handle;
            }
        }


        /// <summary>
        /// Gets RPC transport context.
        /// </summary>
        public virtual RpceClientContext Context
        {
            get
            {
                return rpceClient.Context;
            }
        }

        /// <summary>
        /// Let SMB transport execute a transaction over the named pipe for synchronous RPCs
        /// </summary>
        public void UseTransactionForNamedPipe(bool useTransactionForNamedPipe)
        {
            rpceClient.Context.UseTransactionForNamedPipe = useTransactionForNamedPipe;
        }

        /// <summary>
        /// Connect and bind to a RPCE remote host.
        /// </summary>
        /// <param name="protocolSequence">
        /// A protocol sequence.<para/>
        /// Support ncacn_ip_tcp and ncacn_np only.
        /// </param>
        /// <param name="networkAddress">
        /// A network address of RPCE remote host.
        /// </param>
        /// <param name="endpoint">
        /// An endpoint that its format and content 
        /// are associated with the protocol sequence.
        /// </param>
        /// <param name="transportCredential">
        /// If connect by SMB/SMB2, it's the security credential 
        /// used by underlayer transport (SMB/SMB2). 
        /// If connect by TCP, this parameter is ignored.
        /// </param>
        /// <param name="interfaceId">
        /// A Guid of interface_id that is binding to.
        /// </param>
        /// <param name="interfaceMajorVersion">
        /// interface_major_ver that is binding to.
        /// </param>
        /// <param name="interfaceMinorVersion">
        /// interface_minor_ver that is binding to.
        /// </param>
        /// <param name="securityContext">
        /// A security provider. If setting to null, indicate the default authentication type NTLM is selected.
        /// </param>
        /// <param name="connectSecurityContext">
        /// A security provider for connect authentication. If setting to null, indicate the default authentication type NTLM is selected.
        /// </param>
        /// <param name="authenticationLevel">
        /// An authentication level.
        /// </param>
        /// <param name="supportsHeaderSign">
        /// Indicates whether client supports header sign or not.
        /// </param>
        /// <param name="timeout">
        /// Timeout period.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when protSeq, networkAddr or endpoint is null.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// Thrown when receive error from server.
        /// </exception>
        public virtual void Bind(
            string protocolSequence,
            string networkAddress,
            string endpoint,
            AccountCredential transportCredential,
            Guid interfaceId,
            ushort interfaceMajorVersion,
            ushort interfaceMinorVersion,
            ClientSecurityContext securityContext,
            ClientSecurityContext connectSecurityContext,
            RpceAuthenticationLevel authenticationLevel,
            bool supportsHeaderSign,
            TimeSpan timeout)
        {
            if (protocolSequence == null)
            {
                throw new ArgumentNullException("protocolSequence");
            }
            if (networkAddress == null)
            {
                throw new ArgumentNullException("networkAddress");
            }
            if (endpoint == null)
            {
                throw new ArgumentNullException("endpoint");
            }

            // RPC over named-pipe does not support asynchronous call in this library.
            // http://msdn.microsoft.com/en-us/library/windows/desktop/aa373551(v=vs.85).aspx
            rpceClient.Context.IsAsynchronous = (string.Compare(protocolSequence, RpceUtility.RPC_OVER_NAMED_PIPE_PROTOCOL_SEQUENCE, true) != 0);

            rpceClient.Connect(protocolSequence, networkAddress, endpoint, transportCredential, timeout,
                connectSecurityContext == null ? SecurityPackageType.Ntlm : connectSecurityContext.PackageType);

            rpceClient.SetAuthInfo(securityContext, authenticationLevel, rpceClient.Context.AuthenticationContextId);

            RpceCoBindPdu bindPdu = rpceClient.CreateCoBindPdu(
                //read from context, donot hardcode.
                rpceClient.Context.RpcVersionMinor, // default is rpc vers 5.0
                supportsHeaderSign ? RpceCoPfcFlags.PFC_SUPPORT_HEADER_SIGN : RpceCoPfcFlags.None,
                rpceClient.ComputeNextCallId(), // call id, default is 1
                rpceClient.Context.MaxTransmitFragmentSize, // max xmit frag
                rpceClient.Context.MaxReceiveFragmentSize, // max recv frag
                rpceClient.Context.AssociateGroupId, // assoc group id, default 0
                interfaceId,
                interfaceMajorVersion,
                interfaceMinorVersion,
                rpceClient.Context.NdrVersion, // default is NDR
                (rpceClient.Context.BindTimeFeatureNegotiationBitmask != RpceBindTimeFeatureNegotiationBitmask.None) // default is None
                    ? (RpceBindTimeFeatureNegotiationBitmask?)rpceClient.Context.BindTimeFeatureNegotiationBitmask
                    : null);

            FragmentAndSendPdu(bindPdu);

            RpcePdu receivedPdu = ReceiveAndReassemblePdu(timeout);

            if (receivedPdu is RpceCoBindAckPdu)
            {
                if (rpceClient.Context.NdrVersion == RpceNdrVersion.None)
                {
                    throw new InvalidOperationException("Neither NDR nor NDR64 is supported.");
                }
            }
            else
            {
                RpceCoBindNakPdu bindNakPdu = receivedPdu as RpceCoBindNakPdu;
                if (bindNakPdu != null)
                {
                    throw new InvalidOperationException(bindNakPdu.provider_reject_reason.ToString());
                }
                else
                {
                    throw new InvalidOperationException(
                        string.Format("Unexpected packet type received - {0}.",
                        receivedPdu.GetType().Name));
                }
            }

            while (rpceClient.Context.SecurityContext != null
                && rpceClient.Context.SecurityContext.NeedContinueProcessing)
            {
                RpceCoAlterContextPdu alterContextPdu = rpceClient.CreateCoAlterContextPdu();
                FragmentAndSendPdu(alterContextPdu);
                receivedPdu = ReceiveAndReassemblePdu(timeout);

                RpceCoFaultPdu faultPdu = receivedPdu as RpceCoFaultPdu;
                if (faultPdu != null)
                {
                    throw new InvalidOperationException(faultPdu.status.ToString());
                }

                if (!(receivedPdu is RpceCoAlterContextRespPdu))
                {
                    throw new InvalidOperationException("Expect alter_context_pdu, but received others.");
                }
            }

            if (rpceClient.Context.SecurityContext != null
                && rpceClient.Context.SecurityContext.Token != null
                && rpceClient.Context.SecurityContext.Token.Length != 0)
            {
                RpceCoAuth3Pdu auth3Pdu = rpceClient.CreateCoAuth3Pdu();
                FragmentAndSendPdu(auth3Pdu);
                // no expected response from server
                rpceClient.Context.OutstandingCalls.Remove(auth3Pdu.call_id);
            }

            if (rpceClient.Context.IsAsynchronous)
            {
                // Start the receiving thread to receive the response from server.
                receiveThread = new Thread(new ThreadStart(EventLoop));
                receiveThread.IsBackground = true;
                receiveThread.Start();
            }
        }

        /// <summary>
        /// Connect and bind to a RPCE remote host.
        /// </summary>
        /// <param name="protocolSequence">
        /// A protocol sequence.<para/>
        /// Support ncacn_ip_tcp and ncacn_np only.
        /// </param>
        /// <param name="networkAddress">
        /// A network address of RPCE remote host.
        /// </param>
        /// <param name="endpoint">
        /// An endpoint that its format and content 
        /// are associated with the protocol sequence.
        /// </param>
        /// <param name="transportCredential">
        /// If connect by SMB/SMB2, it's the security credential 
        /// used by underlayer transport (SMB/SMB2). 
        /// If connect by TCP, this parameter is ignored.
        /// </param>
        /// <param name="interfaceId">
        /// A Guid of interface_id that is binding to.
        /// </param>
        /// <param name="interfaceMajorVersion">
        /// interface_major_ver that is binding to.
        /// </param>
        /// <param name="interfaceMinorVersion">
        /// interface_minor_ver that is binding to.
        /// </param>
        /// <param name="securityContext">
        /// A security provider. If setting to null, indicate the default authentication type NTLM is selected.
        /// </param>
        /// <param name="authenticationLevel">
        /// An authentication level.
        /// </param>
        /// <param name="supportsHeaderSign">
        /// Indicates whether client supports header sign or not.
        /// </param>
        /// <param name="timeout">
        /// Timeout period.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when protSeq, networkAddr or endpoint is null.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// Thrown when receive error from server.
        /// </exception>
        public virtual void Bind(
            string protocolSequence,
            string networkAddress,
            string endpoint,
            AccountCredential transportCredential,
            Guid interfaceId,
            ushort interfaceMajorVersion,
            ushort interfaceMinorVersion,
            ClientSecurityContext securityContext,
            RpceAuthenticationLevel authenticationLevel,
            bool supportsHeaderSign,
            TimeSpan timeout)
        {
            Bind(protocolSequence, networkAddress, endpoint, transportCredential, interfaceId, interfaceMajorVersion, interfaceMinorVersion,
                securityContext, securityContext, authenticationLevel, supportsHeaderSign, timeout);
        }

        /// <summary>
        /// Synchronously call a RPCE method.
        /// </summary>
        /// <param name="opnum">
        /// The opnum of a method.
        /// </param>
        /// <param name="requestStub">
        /// A byte array of the request stub of a method.<para/>
        /// RpceStubEncoder can be used to NDR marshal parameters to a byte array.
        /// </param>
        /// <param name="timeout">
        /// Timeout period.
        /// </param>
        /// <param name="responseStub">
        /// A byte array of the response stub of a method.<para/>
        /// RpceStubDecoder can be used to NDR un-marshal parameters to a byte array.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when requestStub is null.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// Thrown when receive error from server or RPC connection has not been established.
        /// </exception>
        public virtual void Call(
            ushort opnum,
            byte[] requestStub,
            TimeSpan timeout,
            out byte[] responseStub)
        {
            uint callId;
            SendRequest(opnum, requestStub, out callId);
            RecvResponse(callId, timeout, out responseStub);
        }


        /// <summary>
        /// Send request to RPCE server.
        /// </summary>
        /// <param name="opnum">
        /// The opnum of a method.
        /// </param>
        /// <param name="requestStub">
        /// A byte array of the request stub of a method.<para/>
        /// RpceStubEncoder can be used to NDR marshal parameters to a byte array.
        /// </param>
        /// <param name="callId">
        /// The identifier of this call.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when requestStub is null.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// Thrown when receive error from server or RPC connection has not been established.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// Thrown when this transport has been used as an synchronous transport.
        /// </exception>
        public virtual void SendRequest(
            ushort opnum,
            byte[] requestStub,
            out uint callId)
        {
            if (requestStub == null)
            {
                throw new ArgumentNullException("requestStub");
            }
            if (rpceClient.IsDisposed)
            {
                throw new InvalidOperationException("RPC connection has not been established.");
            }

            RpceCoRequestPdu requestPdu = rpceClient.CreateCoRequestPdu(
                opnum,
                requestStub);

            if (rpceClient.Context.AuthenticationType == RpceAuthenticationType.RPC_C_AUTHN_WINNT
                && rpceClient.Context.AuthenticationLevel == RpceAuthenticationLevel.RPC_C_AUTHN_LEVEL_PKT_INTEGRITY)
            {
                verification_trailer_t verificationTrailer = requestPdu.CreateVerificationTrailer(
                    SEC_VT_COMMAND.SEC_VT_COMMAND_BITMASK_1,
                    SEC_VT_COMMAND.SEC_VT_COMMAND_HEADER2,
                    SEC_VT_COMMAND.SEC_VT_COMMAND_PCONTEXT | SEC_VT_COMMAND.SEC_VT_COMMAND_END);
                requestPdu.AppendVerificationTrailerToStub(verificationTrailer);
            }

            FragmentAndSendPdu(requestPdu);

            lock (responseLock)
            {
                callId = requestPdu.call_id;
                outstandingCallIds.Add(callId);
            }
        }
        /// <summary>
        /// Receive response from RPCE server.
        /// </summary>
        /// <param name="callId">
        /// The identifier of this call.
        /// </param>
        /// <param name="timeout">
        /// Timeout period.
        /// </param>
        /// <param name="responseStub">
        /// A byte array of the response stub of a method.
        /// RpceStubDecoder can be used to NDR un-marshal parameters to a byte array.
        /// </param>
        /// <exception cref="ArgumentException">
        /// Thrown when callId is invalid.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// Thrown when receive error from server or RPC connection has not been established.
        /// </exception>
        /// <exception cref="RpceDisconnectedException">
        /// Thrown when the connection is disconnected.
        /// </exception>
        /// <exception cref="TimeoutException">
        /// Thrown when timeout to expect the response of the async call.
        /// </exception>
        public virtual void RecvResponse(
            uint callId,
            TimeSpan timeout,
            out byte[] responseStub)
        {
            if (rpceClient.IsDisposed)
            {
                throw new InvalidOperationException("RPC connection has not been established.");
            }
            lock (responseLock)
            {
                if (!outstandingCallIds.Contains(callId))
                {
                    throw new ArgumentException("Invalid call id.");
                }
            }

            if (rpceClient.Context.IsAsynchronous)
            {
                TimeSpan leftTime = timeout;
                DateTime expiratedTime = DateTime.Now + timeout;
                while (leftTime.CompareTo(new TimeSpan(0)) > 0)
                {
                    lock (responseLock)
                    {
                        // Handle the events added in receiving thread.
                        while (rpceClient.Context.Events.Count > 0)
                        {
                            if (rpceClient.Context.Events[0] is RpcePdu)
                            {
                                RpcePdu rpcePdu = rpceClient.Context.Events[0] as RpcePdu;
                                uint id = ParseRpcePdu(rpcePdu);

                                rpceClient.Context.ReceivedPdus.Add(id, rpcePdu);
                                rpceClient.Context.Events.RemoveAt(0);
                            }
                            else
                            {
                                throw rpceClient.Context.Events[0] as Exception;
                            }
                        }

                        RpcePdu receivedPdu;
                        if (rpceClient.Context.ReceivedPdus.TryGetValue(callId, out receivedPdu))
                        {
                            rpceClient.Context.ReceivedPdus.Remove(callId);
                            outstandingCallIds.Remove(callId);

                            HandleRpcePdu(receivedPdu, out responseStub);
                            return;
                        }
                    }
                    System.Threading.Thread.Sleep(10);
                    leftTime = expiratedTime - DateTime.Now;
                }

                throw new TimeoutException(string.Format("RecvResponse timeout to expect the response of the call. call_id: {0}", callId));
            }
            else
            {
                RpcePdu receivedPdu = ReceiveAndReassemblePdu(timeout);
                ParseRpcePdu(receivedPdu);
                HandleRpcePdu(receivedPdu, out responseStub);
            }
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
        public virtual void Unbind(TimeSpan timeout)
        {
            if (rpceClient.IsDisposed)
            {
                throw new InvalidOperationException("RPC connection has not been established.");
            }

            rpceClient.Disconnect(timeout);
            if (receiveThread != null)
            {
                if (!receiveThread.Join(new TimeSpan(0, 0, 10)))
                {
                    receiveThread.Abort();
                }
                receiveThread = null;
            }
        }

        /// <summary>
        /// Fragment and send PDU.
        /// </summary>
        /// <param name="pdu">PDU</param>
        private void FragmentAndSendPdu(RpceCoPdu pdu)
        {
            if (pdu.PTYPE == RpcePacketType.Bind
                || pdu.PTYPE == RpcePacketType.BindAck
                || pdu.PTYPE == RpcePacketType.AlterContext
                || pdu.PTYPE == RpcePacketType.AlterContextResp
                || pdu.PTYPE == RpcePacketType.Auth3)
            {
                pdu.InitializeAuthenticationToken();
                pdu.SetLength();
                foreach (RpceCoPdu fragPdu in RpceUtility.FragmentPdu(rpceClient.Context, pdu))
                {
                    rpceClient.SendPdu(fragPdu);
                }
            }
            else
            {
                foreach (RpceCoPdu fragPdu in RpceUtility.FragmentPdu(rpceClient.Context, pdu))
                {
                    fragPdu.InitializeAuthenticationToken();
                    rpceClient.SendPdu(fragPdu);
                }
            }
        }


        /// <summary>
        /// Receive and reassemble PDU.
        /// </summary>
        /// <param name="timeout">timeout</param>
        /// <returns>PDU</returns>
        private RpcePdu ReceiveAndReassemblePdu(TimeSpan timeout)
        {
            RpcePdu receivedPdu = rpceClient.ExpectPdu(timeout);
            RpceCoPdu receivedCoPdu = receivedPdu as RpceCoPdu;
            if (receivedCoPdu == null)
            {
                return receivedPdu;
            }

            List<RpceCoPdu> pduList = new List<RpceCoPdu>();
            pduList.Add(receivedCoPdu);

            while ((receivedCoPdu.pfc_flags & RpceCoPfcFlags.PFC_LAST_FRAG) == 0)
            {
                receivedPdu = rpceClient.ExpectPdu(timeout);
                receivedCoPdu = receivedPdu as RpceCoPdu;
                if (receivedCoPdu == null)
                {
                    throw new InvalidOperationException("CL PDU received inside a connection.");
                }

                pduList.Add(receivedCoPdu);
            }

            return RpceUtility.ReassemblePdu(rpceClient.Context, pduList.ToArray());
        }

        /// <summary>
        /// Loop for asychronous receive response.
        /// </summary>
        private void EventLoop()
        {
            while (!rpceClient.IsDisposed)
            {
                try
                {
                    RpcePdu receivedPdu;
                    receivedPdu = ReceiveAndReassemblePdu(new TimeSpan(1, 0, 0));
                    if (receivedPdu is RpceCoShutdownPdu)
                    {
                        throw new RpceDisconnectedException("The connection is disconnected by server.");
                    }
                    lock (responseLock)
                    {
                        rpceClient.Context.Events.Add(receivedPdu);
                    }
                }
                catch (TimeoutException)
                {
                    continue;
                }
                catch (RpceDisconnectedException e)
                {
                    if (!rpceClient.IsDisposed && rpceClient.Context != null && rpceClient.Context.Events != null)
                    {
                        rpceClient.Context.Events.Add(e);
                    }
                    return;
                }
                catch (Exception e)
                {
                    if (!rpceClient.IsDisposed && rpceClient.Context != null && rpceClient.Context.Events != null)
                    {
                        rpceClient.Context.Events.Add(e);
                    }
                    continue;
                }
            }
        }

        /// <summary>
        /// Parse the identifier of the call from the received pdu from server.
        /// </summary>
        /// <param name="rpcePdu">
        /// Received pdu received from server.
        /// </param>
        /// <exception cref="RpceDisconnectedException">
        /// Thrown when receive shutdown PDU.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// Thrown when receive error from server.
        /// </exception>
        /// <returns>Return the identifier of the call if success, otherwise throw an exception.</returns>
        private uint ParseRpcePdu(RpcePdu rpcePdu)
        {
            if (rpcePdu is RpceCoResponsePdu)
            {
                return (rpcePdu as RpceCoResponsePdu).call_id;
            }
            else if (rpcePdu is RpceCoFaultPdu)
            {
                return (rpcePdu as RpceCoFaultPdu).call_id;
            }

            if (rpcePdu is RpceCoShutdownPdu)
            {
                throw new RpceDisconnectedException("Shutdown PDU received");
            }

            throw new InvalidOperationException(
                string.Format("Unexpected packet type received - {0}.",
                rpcePdu.GetType().Name));
        }

        /// <summary>
        /// Handle the received pdu from server.
        /// </summary>
        /// <param name="rpcePdu">
        /// Received pdu received from server.
        /// </param>
        /// <param name="responseStub">
        /// A byte array of the response stub of a method.
        /// RpceStubDecoder can be used to NDR un-marshal parameters to a byte array.
        /// </param>
        /// <exception cref="InvalidOperationException">
        /// Thrown when receive error from server or RPC connection has not been established.
        /// </exception>
        private void HandleRpcePdu(RpcePdu rpcePdu, out byte[] responseStub)
        {
            if (rpcePdu is RpceCoResponsePdu)
            {
                responseStub = (rpcePdu as RpceCoResponsePdu).stub;
            }
            else if (rpcePdu is RpceCoFaultPdu)
            {
                throw new InvalidOperationException((rpcePdu as RpceCoFaultPdu).status.ToString());
            }
            else
            {
                throw new InvalidOperationException(rpcePdu.GetType().ToString());
            }
        }

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
                try
                {
                    receiveThread.Abort();
                }
                catch { }
                // Release managed resources.
                if (!rpceClient.IsDisposed)
                {
                    rpceClient.Dispose();
                }
            }

            // Release unmanaged resources.
        }


        /// <summary>
        /// finalizer
        /// </summary>
        ~RpceClientTransport()
        {
            Dispose(false);
        }

        #endregion
    }
}
