// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

using Microsoft.Protocols.TestTools.StackSdk.FileAccessService;
using Cifs = Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Cifs;
using Smb = Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb;
using Microsoft.Protocols.TestTools.StackSdk.Security.Sspi;
using Microsoft.Protocols.TestTools.StackSdk.Transport;

namespace Microsoft.Protocols.TestTools.StackSdk.Networking.Rpce
{
    /// <summary>
    /// RpceServer is the exposed interface to 
    /// receive a connection, receive and send a PDU.
    /// </summary>
    [SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling")]
    public class RpceServer : IDisposable
    {
        #region Field members

        //A instance to manage all RPCE server context
        private RpceContextManager serverContextManager;

        //A delegate to create ServerSecurityContext.
        private RpceSecurityContextCreatingEventHandler securityContextImp;

        private SyncFilterQueue<RpceTransportEvent> receivedTransportEvents;

        //receiving threads
        private Thread tcpReceiveThread;
        private Thread smbReceiveThread;

        //lock
        private object tcpThreadLocker = new object();
        private object smbThreadLocker = new object();

        //transport
        private TransportStack tcpTransport;
        private Smb.SmbServerTransport smbTransport;

        // redundant tcp port lista and named pipe list. 
        private List<ushort> openedTcpPortList;
        private List<string> openedNamedPipeList;

        // The waiting seconds for receive loop (1s)
        private readonly TimeSpan TRANSPORT_RECEIVE_LOOP_TIMEOUT = new TimeSpan(0, 0, 1);

        #endregion


        #region ctor

        /// <summary>
        /// Initialize Rpce server.
        /// </summary>
        public RpceServer()
        {
            this.serverContextManager = new RpceContextManager();
            this.receivedTransportEvents = new SyncFilterQueue<RpceTransportEvent>();
        }

        #endregion


        #region Properties

        /// <summary>
        /// A delegate to create ServerSecurityContext.
        /// </summary>
        public RpceSecurityContextCreatingEventHandler SecurityContextCreator
        {
            get
            {
                return this.securityContextImp;
            }
        }


        /// <summary>
        /// A read-only collection for servercontexts in the RPCE server.
        /// </summary>
        public ReadOnlyCollection<RpceServerContext> ServerContexts
        {
            get
            {
                return this.serverContextManager.ServerContexts;
            }
        }


        #endregion


        #region Create PDU

        /// <summary>
        /// Create a RpceCoBindAckPdu.
        /// </summary>
        /// <param name="sessionContext">Context of RPCE session.</param>
        /// <returns>Created RpceCoBindAckPdu.</returns>
        /// <exception cref="ArgumentNullException">Thrown when sessionContext is null.</exception>
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        public RpceCoBindAckPdu CreateCoBindAckPdu(
            RpceServerSessionContext sessionContext)
        {
            if (sessionContext == null)
            {
                throw new ArgumentNullException("sessionContext");
            }

            RpceCoBindAckPdu bindAckPdu = new RpceCoBindAckPdu(sessionContext);

            float serverRpcVersion
                = (float)sessionContext.ServerContext.RpcVersionMajor
                + (float)sessionContext.ServerContext.RpcVersionMinor / 10.0f;
            float proposedRpcVersion
                = (float)sessionContext.RpcVersionMajor
                + (float)sessionContext.RpcVersionMinor / 10.0f;
            float rpcVersion = Math.Min(serverRpcVersion, proposedRpcVersion);

            bindAckPdu.rpc_vers = (byte)Math.Truncate(rpcVersion);
            bindAckPdu.rpc_vers_minor = (byte)((rpcVersion - Math.Truncate(rpcVersion)) * 10.0f);
            bindAckPdu.PTYPE = RpcePacketType.BindAck;
            bindAckPdu.pfc_flags = RpceUtility.GeneratePfcFlags(sessionContext, RpcePacketType.BindAck);
            bindAckPdu.packed_drep.dataRepFormat = sessionContext.PackedDataRepresentationFormat;
            bindAckPdu.packed_drep.reserved = 0;
            bindAckPdu.call_id = sessionContext.CurrentCallId;

            bindAckPdu.max_xmit_frag = sessionContext.MaxTransmitFragmentSize;
            bindAckPdu.max_recv_frag = sessionContext.MaxReceiveFragmentSize;
            bindAckPdu.assoc_group_id = sessionContext.AssociateGroupId;
            if (sessionContext.SecondaryAddress == null)
            {
                bindAckPdu.sec_addr.port_spec = new byte[0];
            }
            else
            {
                bindAckPdu.sec_addr.port_spec = Encoding.ASCII.GetBytes(sessionContext.SecondaryAddress);
            }
            bindAckPdu.sec_addr.length = (ushort)bindAckPdu.sec_addr.port_spec.Length;

            int sizeOfSecAddr = bindAckPdu.sec_addr.length + Marshal.SizeOf(bindAckPdu.sec_addr.length);
            bindAckPdu.pad2 = new byte[RpceUtility.Align(sizeOfSecAddr, 4) - sizeOfSecAddr];

            RpceNdrVersion ndrVersion = sessionContext.ServerContext.NdrVersion;
            List<p_result_t> presentationResultList = new List<p_result_t>();
            for (int i = 0; i < sessionContext.PresentationContextsTable.Count; i++)
            {
                p_result_t pResult = new p_result_t();
                RpceNdrVersion proposedNdrVersion = sessionContext.PresentationContextsTable.Values[i];
                if ((proposedNdrVersion & ndrVersion) != 0)
                {
                    pResult.result = p_cont_def_result_t.acceptance;
                    // donot accept any more NDR version.
                    ndrVersion = RpceNdrVersion.None;
                }
                else
                {
                    pResult.result = p_cont_def_result_t.provider_rejection;
                }
                pResult.reason = p_provider_reason_t.reason_not_specified;
                if (proposedNdrVersion == RpceNdrVersion.NDR)
                {
                    pResult.transfer_syntax.if_uuid = RpceUtility.NDR_INTERFACE_UUID;
                    pResult.transfer_syntax.if_vers_major = RpceUtility.NDR_INTERFACE_MAJOR_VERSION;
                    pResult.transfer_syntax.if_vers_minor = RpceUtility.NDR_INTERFACE_MINOR_VERSION;
                }
                else if (proposedNdrVersion == RpceNdrVersion.NDR64)
                {
                    pResult.transfer_syntax.if_uuid = RpceUtility.NDR64_INTERFACE_UUID;
                    pResult.transfer_syntax.if_vers_major = RpceUtility.NDR64_INTERFACE_MAJOR_VERSION;
                    pResult.transfer_syntax.if_vers_minor = RpceUtility.NDR64_INTERFACE_MINOR_VERSION;
                }
                presentationResultList.Add(pResult);
            }
            if (sessionContext.BindTimeFeatureNegotiationBitmask != RpceBindTimeFeatureNegotiationBitmask.None)
            {
                RpceBindTimeFeatureNegotiationBitmask bindTimeFeatureNegotiationBitmask
                    = sessionContext.BindTimeFeatureNegotiationBitmask;
                if ((bindTimeFeatureNegotiationBitmask
                    & RpceBindTimeFeatureNegotiationBitmask.KeepConnectionOnOrphanSupported)
                    != 0)
                {
                    bindTimeFeatureNegotiationBitmask
                        |= RpceBindTimeFeatureNegotiationBitmask.KeepConnectionOnOrphanSupported_Resp;
                }
                bindTimeFeatureNegotiationBitmask
                    &= (sessionContext.ServerContext.SupportsSecurityContextMultiplexing
                        ? RpceBindTimeFeatureNegotiationBitmask.SecurityContextMultiplexingSupported
                        : RpceBindTimeFeatureNegotiationBitmask.None)
                        |
                       (sessionContext.ServerContext.SupportsKeepConnectionOnOrphan
                        ? RpceBindTimeFeatureNegotiationBitmask.KeepConnectionOnOrphanSupported_Resp
                        : RpceBindTimeFeatureNegotiationBitmask.None);

                p_result_t pResult = new p_result_t();
                pResult.result = p_cont_def_result_t.negotiate_ack;
                pResult.reason = (p_provider_reason_t)bindTimeFeatureNegotiationBitmask;
                presentationResultList.Add(pResult);
            }

            bindAckPdu.p_result_list.n_results = (byte)presentationResultList.Count;
            bindAckPdu.p_result_list.p_results = presentationResultList.ToArray();

            bindAckPdu.AppendAuthenticationVerifier();
            bindAckPdu.SetLength();

            return bindAckPdu;
        }


        /// <summary>
        /// Create a RpceCoBindNakPdu.
        /// </summary>
        /// <param name="sessionContext">Context of RPCE session.</param>
        /// <param name="rejectReason">Presentation context reject.</param>
        /// <param name="extendedErrorInfo">RPC extended error information.</param>
        /// <returns>Created RpceCoBindAckPdu.</returns>
        /// <exception cref="ArgumentNullException">Thrown when sessionContext is null.</exception>
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        public RpceCoBindNakPdu CreateCoBindNakPdu(
            RpceServerSessionContext sessionContext,
            p_reject_reason_t rejectReason,
            byte[] extendedErrorInfo)
        {
            if (sessionContext == null)
            {
                throw new ArgumentNullException("sessionContext");
            }

            RpceCoBindNakPdu bindNakPdu = new RpceCoBindNakPdu(sessionContext);

            bindNakPdu.rpc_vers = sessionContext.RpcVersionMajor;
            bindNakPdu.rpc_vers_minor = sessionContext.RpcVersionMinor;
            bindNakPdu.PTYPE = RpcePacketType.BindNak;
            bindNakPdu.pfc_flags = RpceUtility.GeneratePfcFlags(sessionContext, RpcePacketType.BindNak);
            bindNakPdu.packed_drep.dataRepFormat = sessionContext.PackedDataRepresentationFormat;
            bindNakPdu.packed_drep.reserved = 0;
            bindNakPdu.call_id = sessionContext.CurrentCallId;

            bindNakPdu.provider_reject_reason = rejectReason;
            bindNakPdu.versions.n_protocols = 1; // only one rpc version in server context.
            bindNakPdu.versions.p_protocols = new version_t[bindNakPdu.versions.n_protocols];
            // It's the supported rpc versions, so we response version number in server context.
            bindNakPdu.versions.p_protocols[0].major = sessionContext.ServerContext.RpcVersionMajor;
            bindNakPdu.versions.p_protocols[0].major = sessionContext.ServerContext.RpcVersionMinor;

            int sizeOfReasonAndVersions = Marshal.SizeOf(typeof(ushort)); // bindNakPdu.provider_reject_reason
            sizeOfReasonAndVersions += Marshal.SizeOf(bindNakPdu.versions.n_protocols);
            sizeOfReasonAndVersions += Marshal.SizeOf(typeof(version_t)) * bindNakPdu.versions.n_protocols;
            bindNakPdu.pad = new byte[RpceUtility.Align(sizeOfReasonAndVersions, 4) - sizeOfReasonAndVersions];

            if (extendedErrorInfo != null)
            {
                bindNakPdu.signature = RpceUtility.BINDNAK_SIGNATURE;
                bindNakPdu.extended_error_info = extendedErrorInfo;
            }

            bindNakPdu.SetLength();

            return bindNakPdu;
        }


        /// <summary>
        /// Create a RpceCoFaultPdu. 
        /// </summary>
        /// <param name="sessionContext">Context of the session.</param>
        /// <param name="stub">stub data, 8-octet aligned.</param>
        /// <param name="statusCode">Fault status.</param>
        /// <returns>Created RpceCoFaultPdu.</returns>
        /// <exception cref="ArgumentNullException">Thrown when sessionContext is null.</exception>
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        public RpceCoFaultPdu CreateCoFaultPdu(
            RpceServerSessionContext sessionContext,
            byte[] stub,
            uint statusCode)
        {
            if (sessionContext == null)
            {
                throw new ArgumentNullException("sessionContext");
            }

            RpceCoFaultPdu faultPdu = new RpceCoFaultPdu(sessionContext);

            faultPdu.rpc_vers = sessionContext.RpcVersionMajor;
            faultPdu.rpc_vers_minor = sessionContext.RpcVersionMinor;
            faultPdu.PTYPE = RpcePacketType.Fault;
            faultPdu.pfc_flags = RpceUtility.GeneratePfcFlags(sessionContext, RpcePacketType.Fault);
            faultPdu.packed_drep.dataRepFormat = sessionContext.PackedDataRepresentationFormat;
            faultPdu.packed_drep.reserved = 0;
            faultPdu.call_id = sessionContext.CurrentCallId;

            faultPdu.alloc_hint = (uint)(stub == null ? 0 : stub.Length);
            faultPdu.p_cont_id = sessionContext.ContextIdentifier;
            faultPdu.cancel_count = 0;
            faultPdu.reserved = 0;
            faultPdu.status = statusCode;
            faultPdu.reserved2 = 0;
            faultPdu.stub = stub;

            faultPdu.AppendAuthenticationVerifier();
            faultPdu.SetLength();

            return faultPdu;
        }


        /// <summary>
        /// Create a RpceCoAlterContextRespPdu.
        /// </summary>
        /// <param name="sessionContext">Context of the connection.</param>
        /// <returns>Created RpceCoAlterContextRespPdu.</returns>
        /// <exception cref="ArgumentNullException">Thrown when sessionContext is null.</exception>
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        public RpceCoAlterContextRespPdu CreateCoAlterContextRespPdu(
            RpceServerSessionContext sessionContext)
        {
            if (sessionContext == null)
            {
                throw new ArgumentNullException("sessionContext");
            }

            RpceCoAlterContextRespPdu alterContextRespPdu = new RpceCoAlterContextRespPdu(sessionContext);

            alterContextRespPdu.rpc_vers = sessionContext.RpcVersionMajor;
            alterContextRespPdu.rpc_vers_minor = sessionContext.RpcVersionMinor;
            alterContextRespPdu.PTYPE = RpcePacketType.AlterContextResp;
            alterContextRespPdu.pfc_flags = RpceUtility.GeneratePfcFlags(sessionContext, RpcePacketType.AlterContextResp);
            alterContextRespPdu.packed_drep.dataRepFormat = sessionContext.PackedDataRepresentationFormat;
            alterContextRespPdu.packed_drep.reserved = 0;
            alterContextRespPdu.call_id = sessionContext.CurrentCallId;

            alterContextRespPdu.max_xmit_frag = 0;
            alterContextRespPdu.max_recv_frag = 0;
            alterContextRespPdu.assoc_group_id = 0;
            alterContextRespPdu.sec_addr.length = 0;
            alterContextRespPdu.sec_addr.port_spec = new byte[0];

            int sizeOfSecAddr = alterContextRespPdu.sec_addr.length + Marshal.SizeOf(alterContextRespPdu.sec_addr.length);
            alterContextRespPdu.pad2 = new byte[RpceUtility.Align(sizeOfSecAddr, 4) - sizeOfSecAddr];

            //there's only one presentation result in alter_context_resp
            alterContextRespPdu.p_result_list.n_results = 1;
            alterContextRespPdu.p_result_list.p_results = new p_result_t[1];
            alterContextRespPdu.p_result_list.p_results[0].result = p_cont_def_result_t.acceptance;
            alterContextRespPdu.p_result_list.p_results[0].reason = p_provider_reason_t.reason_not_specified;
            if (sessionContext.NdrVersion == RpceNdrVersion.NDR)
            {
                alterContextRespPdu.p_result_list.p_results[0].transfer_syntax.if_uuid
                    = RpceUtility.NDR_INTERFACE_UUID;
                alterContextRespPdu.p_result_list.p_results[0].transfer_syntax.if_vers_major
                    = RpceUtility.NDR_INTERFACE_MAJOR_VERSION;
                alterContextRespPdu.p_result_list.p_results[0].transfer_syntax.if_vers_minor
                    = RpceUtility.NDR_INTERFACE_MINOR_VERSION;
            }
            else if (sessionContext.NdrVersion == RpceNdrVersion.NDR64)
            {
                alterContextRespPdu.p_result_list.p_results[0].transfer_syntax.if_uuid
                    = RpceUtility.NDR64_INTERFACE_UUID;
                alterContextRespPdu.p_result_list.p_results[0].transfer_syntax.if_vers_major
                    = RpceUtility.NDR64_INTERFACE_MAJOR_VERSION;
                alterContextRespPdu.p_result_list.p_results[0].transfer_syntax.if_vers_minor
                    = RpceUtility.NDR64_INTERFACE_MINOR_VERSION;
            }

            alterContextRespPdu.AppendAuthenticationVerifier();
            alterContextRespPdu.SetLength();

            return alterContextRespPdu;
        }


        /// <summary>
        /// Create a RpceCoResponsePdu.
        /// </summary>
        /// <param name="sessionContext">Context of the RPCE session.</param>
        /// <param name="callId">
        /// Each RPC is identified by a call identifier 
        /// that is unique for all currently active RPCs within an association group.
        /// </param>
        /// <param name="stub">Response stub.</param>
        /// <returns>Created RpceCoResponsePdu.</returns>
        /// <exception cref="ArgumentNullException">Thrown when sessionContext is null.</exception>
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        public RpceCoResponsePdu CreateCoResponsePdu(
            RpceServerSessionContext sessionContext,
            uint callId,
            byte[] stub)
        {
            if (sessionContext == null)
            {
                throw new ArgumentNullException("sessionContext");
            }

            if (stub == null)
            {
                stub = new byte[0];
            }

            RpceCoResponsePdu responsePdu = new RpceCoResponsePdu(sessionContext);

            responsePdu.rpc_vers = sessionContext.RpcVersionMajor;
            responsePdu.rpc_vers_minor = sessionContext.RpcVersionMinor;
            responsePdu.PTYPE = RpcePacketType.Response;
            responsePdu.pfc_flags = RpceUtility.GeneratePfcFlags(sessionContext, RpcePacketType.Response);
            responsePdu.packed_drep.dataRepFormat = sessionContext.PackedDataRepresentationFormat;
            responsePdu.packed_drep.reserved = 0;
            responsePdu.call_id = callId;

            responsePdu.alloc_hint = (uint)stub.Length;
            responsePdu.p_cont_id = sessionContext.ContextIdentifier;
            responsePdu.stub = stub;
            responsePdu.cancel_count = 0;
            responsePdu.reserved = 0;

            responsePdu.AppendAuthenticationVerifier();
            responsePdu.SetLength();

            return responsePdu;
        }


        /// <summary>
        /// Create a RpceCoResponsePdu, the call_id is set to current call_id.
        /// The server maintains a current call_id for each connection.
        /// The current call_id is the highest call_id that the server has processed on this connection.
        /// </summary>
        /// <param name="sessionContext">Context of the RPCE session.</param>
        /// <param name="stub">Response stub.</param>
        /// <returns>Created RpceCoResponsePdu.</returns>
        public RpceCoResponsePdu CreateCoResponsePdu(
            RpceServerSessionContext sessionContext,
            byte[] stub)
        {
            return this.CreateCoResponsePdu(sessionContext, sessionContext.CurrentCallId, stub);
        }


        /// <summary>
        /// Create a RpceCoRequestPdu.
        /// </summary>
        /// <param name="sessionContext">Context of the RPCE session.</param>
        /// <param name="opnum">Operation number (opnum) to 
        /// inform a callback server of the operation it should invoke.</param>
        /// <param name="callId">Callback request callId</param>
        /// <param name="stub">Request stub.</param>
        /// <returns>Created RpceCoRequestPdu.</returns>
        /// <exception cref="ArgumentNullException">Thrown when sessionContext is null.</exception>
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        public RpceCoRequestPdu CreateCoRequestPdu(
            RpceServerSessionContext sessionContext,
            ushort opnum,
            uint callId,
            byte[] stub)
        {
            if (sessionContext == null)
            {
                throw new ArgumentNullException("sessionContext");
            }

            if (stub == null)
            {
                stub = new byte[0];
            }

            RpceCoRequestPdu requestPdu = new RpceCoRequestPdu(sessionContext);

            requestPdu.rpc_vers = sessionContext.RpcVersionMajor;
            requestPdu.rpc_vers_minor = sessionContext.RpcVersionMinor;
            requestPdu.PTYPE = RpcePacketType.Request;
            requestPdu.pfc_flags = RpceUtility.GeneratePfcFlags(sessionContext, RpcePacketType.Request);
            requestPdu.packed_drep.dataRepFormat = sessionContext.PackedDataRepresentationFormat;
            requestPdu.packed_drep.reserved = 0;
            requestPdu.call_id = callId;

            requestPdu.alloc_hint = (uint)stub.Length;
            requestPdu.p_cont_id = sessionContext.ContextIdentifier;
            requestPdu.opnum = opnum;
            requestPdu.stub = stub;

            requestPdu.AppendAuthenticationVerifier();
            requestPdu.SetLength();

            return requestPdu;
        }


        /// <summary>
        /// Create a RpceCoRequestPdu.
        /// </summary>
        /// <param name="sessionContext">Context of the RPCE session.</param>
        /// <param name="opnum">Operation number (opnum) to 
        /// inform a callback server of the operation it should invoke.</param>
        /// <param name="stub">Request stub.</param>
        /// <returns>Created RpceCoRequestPdu.</returns>
        /// <exception cref="ArgumentNullException">Thrown when sessionContext is null.</exception>
        public RpceCoRequestPdu CreateCoRequestPdu(
            RpceServerSessionContext sessionContext,
            ushort opnum,
            byte[] stub)
        {
            return CreateCoRequestPdu(sessionContext, opnum, sessionContext.CurrentCallId, stub);
        }


        /// <summary>
        /// Create a RpceCoShutdownPdu.
        /// </summary>
        /// <param name="sessionContext">Context of the RPCE session.</param>
        /// <returns>Created RpceCoShutdownPdu.</returns>
        /// <exception cref="ArgumentNullException">Thrown when sessionContext is null.</exception>
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        public RpceCoShutdownPdu CreateCoShutdownPdu(RpceServerSessionContext sessionContext)
        {
            if (sessionContext == null)
            {
                throw new ArgumentNullException("sessionContext");
            }

            RpceCoShutdownPdu shutdownPdu = new RpceCoShutdownPdu(sessionContext);

            shutdownPdu.rpc_vers = sessionContext.RpcVersionMajor;
            shutdownPdu.rpc_vers_minor = sessionContext.RpcVersionMinor;
            shutdownPdu.PTYPE = RpcePacketType.Shutdown;
            shutdownPdu.pfc_flags = RpceUtility.GeneratePfcFlags(sessionContext, RpcePacketType.Shutdown);
            shutdownPdu.packed_drep.dataRepFormat = sessionContext.PackedDataRepresentationFormat;
            shutdownPdu.packed_drep.reserved = 0;
            shutdownPdu.call_id = sessionContext.CurrentCallId;

            shutdownPdu.AppendAuthenticationVerifier();
            shutdownPdu.SetLength();

            return shutdownPdu;
        }

        #endregion


        #region start and stop

        /// <summary>
        /// Start to listen a TCP port.
        /// </summary>
        /// <param name="port">The TCP port to listen.</param>
        /// <exception cref="InvalidOperationException">
        /// Thrown when the server with the port has been started.
        /// </exception>
        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        public virtual RpceServerContext StartTcp(ushort port)
        {
            lock (this.tcpThreadLocker)
            {
                RpceServerContext serverContext = this.serverContextManager.LookupServerContext(
                    RpceUtility.RPC_OVER_TCPIP_PROTOCOL_SEQUENCE,
                    port.ToString());

                if (serverContext != null)
                {
                    throw new InvalidOperationException("The server with the port has been started. Please try other port.");
                }

                serverContext = new RpceServerContext(
                        RpceUtility.RPC_OVER_TCPIP_PROTOCOL_SEQUENCE,
                        port.ToString());

                this.serverContextManager.AddServerContext(serverContext);
                if (this.openedTcpPortList == null)
                {
                    this.openedTcpPortList = new List<ushort>();
                }
                this.openedTcpPortList.Add(port);

                bool ipv4Started = false;
                bool ipv6Started = false;
                Exception ex = null;

                try
                {
                    if (this.tcpTransport == null)
                    {
                        SocketTransportConfig config = new SocketTransportConfig();
                        config.Type = StackTransportType.Tcp;
                        config.Role = Role.Server;
                        config.LocalIpAddress = IPAddress.Any;
                        config.MaxConnections = RpceServerContext.DEFAULT_MAX_CONNECTIONS;
                        config.BufferSize = Math.Max(serverContext.MaxReceiveFragmentSize, serverContext.MaxTransmitFragmentSize);

                        this.tcpTransport = new TransportStack(config, RpceDecodePduCallback);
                    }
                }
                catch (Exception e)
                {
                    ex = e;
                    this.tcpTransport = null;
                }

                try
                {
                    //Start IPv4
                    IPEndPoint ipv4Endpoint = new IPEndPoint(IPAddress.Any, port);
                    this.tcpTransport.Start(ipv4Endpoint);
                    ipv4Started = true;
                }
                catch (Exception e)
                {
                    ex = e;
                }

                //Start IPv6
                try
                {
                    IPEndPoint ipv6Endpoint = new IPEndPoint(IPAddress.IPv6Any, port);
                    this.tcpTransport.Start(ipv6Endpoint);
                    ipv6Started = true;
                }
                catch (Exception e)
                {
                    ex = e;
                }

                if (!ipv4Started && !ipv6Started)
                {
                    this.serverContextManager.RemoveServerContext(serverContext);
                    this.openedTcpPortList.Remove(port);
                    throw new InvalidOperationException("TCP server failed to start.", ex);
                }

                if (this.tcpReceiveThread == null)
                {
                    this.tcpReceiveThread = new Thread(TcpReceiveLoop);
                    this.tcpReceiveThread.Start();
                }

                return serverContext;
            }
        }


        /// <summary>
        ///  Stop specified Tcp server.
        /// </summary>
        /// <param name="port">The TCP port listened.</param>
        /// <exception cref="InvalidOperationException">Thrown when the server is not started.</exception>
        public virtual void StopTcp(ushort port)
        {
            lock (this.tcpThreadLocker)
            {
                RpceServerContext serverContext = this.serverContextManager.LookupServerContext(
                    RpceUtility.RPC_OVER_TCPIP_PROTOCOL_SEQUENCE,
                    port.ToString());

                if (this.tcpTransport == null || serverContext == null)
                {
                    throw new InvalidOperationException("The server is not started, please invoke StartTcp first.");
                }

                foreach (RpceServerSessionContext sessionContext in serverContext.SessionContexts)
                {
                    this.Disconnect(sessionContext);
                }

                this.tcpTransport.Stop(port);

                this.openedTcpPortList.Remove(port);
                this.serverContextManager.RemoveServerContext(serverContext);
            }
        }


        /// <summary>
        /// Start to listen on a named pipe.
        /// </summary>
        /// <param name="namedPipe">The name of named pipe to listen on.</param>
        /// <param name="credential">Credential to be used by underlayer SMB/SMB2 transport.</param>
        /// <exception cref="InvalidOperationException">
        /// Thrown when the server with the named pipe has been started.
        /// </exception>
        /// <param name="ipAddress">server's ipAddress</param>
        public virtual RpceServerContext StartNamedPipe(string namedPipe, AccountCredential credential, 
            IPAddress ipAddress)
        {
            namedPipe = namedPipe.ToUpper();

            lock (this.smbThreadLocker)
            {
                RpceServerContext serverContext = this.serverContextManager.LookupServerContext(
                    RpceUtility.RPC_OVER_NAMED_PIPE_PROTOCOL_SEQUENCE,
                    namedPipe);

                if (serverContext != null)
                {
                    throw new InvalidOperationException("The server with the named pipe has been started. Please try other port.");
                }

                serverContext = new RpceServerContext(
                        RpceUtility.RPC_OVER_NAMED_PIPE_PROTOCOL_SEQUENCE,
                        namedPipe,
                        credential);

                this.serverContextManager.AddServerContext(serverContext);
                if (this.openedNamedPipeList == null)
                {
                    this.openedNamedPipeList = new List<string>();
                }
                this.openedNamedPipeList.Add(namedPipe);

                try
                {
                    if (this.smbTransport == null)
                    {
                        this.smbTransport = new Smb.SmbServerTransport();
                        this.smbTransport.Start(RpceUtility.NAMED_PIPE_PORT, credential, ipAddress);
                    }
                }
                catch
                {
                    this.serverContextManager.RemoveServerContext(serverContext);
                    this.openedNamedPipeList.Remove(namedPipe);
                    throw;
                }

                if (this.smbReceiveThread == null)
                {
                    this.smbReceiveThread = new Thread(SmbReceiveLoop);
                    this.smbReceiveThread.Start();
                }

                return serverContext;
            }
        }


        /// <summary>
        ///  Stop specified named pipe server.
        /// </summary>
        /// <param name="namedPipe">
        /// The name of the named pipe listened on. The name should start with "\Pipe\".
        /// </param>
        /// <exception cref="InvalidOperationException">
        /// Thrown when the server is not started.
        /// </exception>
        public virtual void StopNamedPipe(string namedPipe)
        {
            namedPipe = namedPipe.ToUpper();

            lock (this.smbThreadLocker)
            {
                RpceServerContext serverContext = this.serverContextManager.LookupServerContext(
                    RpceUtility.RPC_OVER_NAMED_PIPE_PROTOCOL_SEQUENCE,
                    namedPipe);

                if (this.smbTransport == null || serverContext == null)
                {
                    throw new InvalidOperationException("The server is not started, please invoke StartNamedPipe first.");
                }

                foreach (RpceServerSessionContext sessionContext in serverContext.SessionContexts)
                {
                    this.Disconnect(sessionContext);
                }

                this.openedNamedPipeList.Remove(namedPipe);
                this.serverContextManager.RemoveServerContext(serverContext);
            }
        }


        /// <summary>
        /// Stop all servers.
        /// </summary>
        public virtual void StopAll()
        {
            List<ushort> portListToStop = new List<ushort>();
            List<string> namedPipeListToStop = new List<string>();

            foreach (RpceServerContext context in this.serverContextManager.ServerContexts)
            {
                // We cannot change values for ServerContexts inside a foreach.
                // Save them in another list and stop them later.
                if (context.ProtocolSequence.Equals(
                    RpceUtility.RPC_OVER_TCPIP_PROTOCOL_SEQUENCE,
                    StringComparison.OrdinalIgnoreCase))
                {
                    portListToStop.Add(ushort.Parse(context.Endpoint));
                }

                if (context.ProtocolSequence.Equals(
                    RpceUtility.RPC_OVER_NAMED_PIPE_PROTOCOL_SEQUENCE,
                    StringComparison.OrdinalIgnoreCase))
                {
                    namedPipeListToStop.Add(context.Endpoint);
                }
            }

            for (int i = 0; i < portListToStop.Count; i++)
            {
                this.StopTcp(portListToStop[i]);
            }

            for (int i = 0; i < namedPipeListToStop.Count; i++)
            {
                this.StopNamedPipe(namedPipeListToStop[i]);
            }
        }

        #endregion


        #region send data

        /// <summary>
        /// Send a PDU to a specific connected client.
        /// </summary>
        /// <param name="pdu">Rpce PDU to send.</param>
        /// <param name="sessionContext">Context of the RPCE session.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when sessionContext or pdu is null.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// Thrown when underlayer transport was not established.
        /// Thrown when protocol sequence is unknown.
        /// </exception>
        public virtual void SendPdu(
            RpceServerSessionContext sessionContext,
            RpcePdu pdu)
        {
            if (sessionContext == null)
            {
                throw new ArgumentNullException("sessionContext");
            }

            if (pdu == null)
            {
                throw new ArgumentNullException("pdu");
            }

            EnsureTransportIsValid();

            sessionContext.UpdateContextOnSendingPdu(pdu);

            if (sessionContext.ProtocolSequence.Equals(
                RpceUtility.RPC_OVER_TCPIP_PROTOCOL_SEQUENCE,
                StringComparison.OrdinalIgnoreCase))
            {
                this.tcpTransport.SendPacket(sessionContext.RemoteEndpoint, pdu);
            }
            else if (sessionContext.ProtocolSequence.Equals(
                RpceUtility.RPC_OVER_NAMED_PIPE_PROTOCOL_SEQUENCE,
                StringComparison.OrdinalIgnoreCase))
            {
                lock (sessionContext.smbSendingLocker)
                {
                    sessionContext.smbBufferSending = ArrayUtility.ConcatenateArrays(
                        sessionContext.smbBufferSending,
                        pdu.ToBytes());

                    if (sessionContext.smbSendingFunc != null)
                    {
                        sessionContext.smbSendingFunc();
                    }
                }
            }
            else
            {
                throw new InvalidOperationException("Unknown protocol sequence.");
            }
        }


        /// <summary>
        ///  Send bytes to specific connected client.
        /// </summary>
        /// <param name="bytes">The bytes to send to client.</param>
        /// <param name="sessionContext">Context of the RPCE session</param>
        /// <exception cref="InvalidOperationException">Thrown when No connection was established.</exception>
        /// <exception cref="ArgumentNullException">
        /// Thrown when sessionContext is null.
        /// Thrown when sessionContext or bytes is null.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// Thrown when underlayer transport was not established.
        /// Thrown when protocol sequence is unknown.
        /// </exception>
        public virtual void SendBytes(
            RpceServerSessionContext sessionContext,
            byte[] bytes)
        {
            if (sessionContext == null)
            {
                throw new ArgumentNullException("sessionContext");
            }

            if (bytes == null)
            {
                throw new ArgumentNullException("bytes");
            }

            EnsureTransportIsValid();

            if (sessionContext.ProtocolSequence.Equals(
                RpceUtility.RPC_OVER_TCPIP_PROTOCOL_SEQUENCE,
                StringComparison.OrdinalIgnoreCase))
            {
                this.tcpTransport.SendBytes(sessionContext.RemoteEndpoint, bytes);
            }
            else if (sessionContext.ProtocolSequence.Equals(
                RpceUtility.RPC_OVER_NAMED_PIPE_PROTOCOL_SEQUENCE,
                StringComparison.OrdinalIgnoreCase))
            {
                lock (sessionContext.smbSendingLocker)
                {
                    sessionContext.smbBufferSending = ArrayUtility.ConcatenateArrays(
                        sessionContext.smbBufferSending,
                        bytes);

                    if (sessionContext.smbSendingFunc != null)
                    {
                        sessionContext.smbSendingFunc();
                    }
                }
            }
            else
            {
                throw new InvalidOperationException("Unknown protocol sequence.");
            }
        }

        #endregion


        /// <summary>
        /// Expect client connect event.
        /// </summary>
        /// <param name="timeout">Timeout of expecting a connection.</param>
        /// <returns>The connection with the server.</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when server was not started.
        /// </exception>
        public virtual RpceServerSessionContext ExpectConnect(TimeSpan timeout)
        {
            RpcePdu pdu;
            RpceServerSessionContext sessionContext = null;

            EventType eventType = ExpectEvent(
                timeout,
                ref sessionContext,
                out pdu);

            if (eventType != EventType.Connected)
            {
                throw new InvalidOperationException(
                    string.Format("{0} (unexpected) event received.", eventType));
            }

            return sessionContext;
        }


        /// <summary>
        /// Expect to disconnect from the client.
        /// </summary>
        /// <param name="timeout">Timeout of expecting a disconnect.</param>
        /// <param name="sessionContext">The sessionContext of expecting to disconnect.</param>
        /// <exception cref="InvalidOperationException">
        /// Thrown when unknown object was received from transport.
        /// Thrown when server was not started.
        /// </exception>
        public virtual void ExpectDisconnect(
            TimeSpan timeout,
            ref RpceServerSessionContext sessionContext)
        {
            RpcePdu pdu;

            EventType eventType = ExpectEvent(
                timeout,
                ref sessionContext,
                out pdu);

            if (eventType != EventType.Disconnected)
            {
                throw new InvalidOperationException(
                    string.Format("{0} (unexpected) event received.", eventType));
            }
        }


        /// <summary>
        /// Disconnect from the specific client
        /// </summary>
        /// <param name="sessionContext">Context of session</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when sessionContext is null.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// Thrown when server was not started.
        /// </exception>
        public virtual void Disconnect(RpceServerSessionContext sessionContext)
        {
            if (sessionContext == null)
            {
                throw new ArgumentNullException("sessionContext");
            }

            EnsureTransportIsValid();

            if (sessionContext.ProtocolSequence.Equals(
                RpceUtility.RPC_OVER_TCPIP_PROTOCOL_SEQUENCE,
                StringComparison.OrdinalIgnoreCase))
            {
                this.tcpTransport.Disconnect(sessionContext.RemoteEndpoint);
            }
            else if (sessionContext.ProtocolSequence.Equals(
                RpceUtility.RPC_OVER_NAMED_PIPE_PROTOCOL_SEQUENCE,
                StringComparison.OrdinalIgnoreCase))
            {
                if (sessionContext.fsConnection != null)
                {
                    Smb.SmbServerConnection connection = sessionContext.fsConnection as Smb.SmbServerConnection;
                    if (connection.OpenList.Count == 0
                        || (connection.OpenList.Count == 1 && connection.OpenList[0].Equals(sessionContext.fsOpen)))
                    {
                        this.smbTransport.Disconnect(connection);
                    }
                }
            }

            sessionContext.ServerContext.RemoveSessionContext(sessionContext);
        }


        /// <summary>
        /// Expect a Pdu with the specific sessionContext.
        /// </summary>
        /// <param name="timeout">Timeout of expecting a PDU.</param>
        /// <param name="sessionContext">
        /// Session Context to receive PDU.
        /// Null means expect PDU on all sessions.
        /// </param>
        /// <returns>The expected PDU.</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when server was not started.
        /// </exception>
        public virtual RpcePdu ExpectPdu(
            TimeSpan timeout,
            ref RpceServerSessionContext sessionContext)
        {
            RpcePdu pdu;

            EventType eventType = ExpectEvent(
                timeout,
                ref sessionContext,
                out pdu);

            if (eventType != EventType.ReceivedPacket)
            {
                throw new InvalidOperationException(
                    string.Format("{0} (unexpected) event received.", eventType));                
            }

            return pdu;
        }


        /// <summary>
        /// Expect an event (connect, disconnect, pdu received) with the specific sessionContext.
        /// </summary>
        /// <param name="timeout">Timeout of expecting a PDU.</param>
        /// <param name="sessionContext">
        /// Session context to receive the event.
        /// Null means expect event on all sessions.
        /// </param>
        /// <param name="pdu">The PDU if receiving a packet; otherwise, null.</param>
        /// <returns>The expected PDU. It is null when event is connect or disconnect.</returns>
        /// <returns>The event type.</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when server was not started.
        /// </exception>
        public virtual EventType ExpectEvent(
            TimeSpan timeout,
            ref RpceServerSessionContext sessionContext,
            out RpcePdu pdu)
        {
            EnsureTransportIsValid();

            RpceEventFilter filter;
            if (sessionContext == null)
            {
                filter = new RpceEventFilter(
                EventType.Connected | EventType.ReceivedPacket | EventType.Disconnected);
            }
            else
            {
                filter = new RpceEventFilter(
                EventType.Connected | EventType.ReceivedPacket | EventType.Disconnected,
                    sessionContext.RemoteEndpoint);
            }

            RpceTransportEvent transportEvent =
                this.receivedTransportEvents.Dequeue(timeout, filter.EventFilter);
            ValidateTransportEvent(
                transportEvent, 
                EventType.Connected | EventType.ReceivedPacket | EventType.Disconnected);

            if (transportEvent.EventType == EventType.Connected)
            {
                pdu = null;
                RpceServerContext serverContext = transportEvent.ServerContext;
                sessionContext = serverContext.CreateAndAddSessionContext(
                    transportEvent.RemoteEndPoint);
                return EventType.Connected;
            }
            
            sessionContext = transportEvent.SessionContext;
            if (sessionContext == null)
            {
                throw new InvalidOperationException(
                    "Session context was not found for this disconnect event.");
            }

            if (transportEvent.EventType == EventType.ReceivedPacket)
            {
                pdu = transportEvent.Pdu;
                if (pdu == null)
                {
                    throw new InvalidOperationException("Unknown object received from transport.");
                }
                sessionContext.UpdateContextOnReceivingPdu(pdu);

                return EventType.ReceivedPacket;
            }

            if (transportEvent.EventType == EventType.Disconnected)
            {
                pdu = null;
                transportEvent.ServerContext.RemoveSessionContext(sessionContext);
                return EventType.Disconnected;
            }

            throw new InvalidOperationException("unknown event type");
        }


        /// <summary>
        /// Remove a session from the server context.
        /// </summary>
        /// <param name="sessionContext">Context of the RPCE session.</param>
        /// <exception cref="ArgumentNullException">Thrown when sessionContext is null.</exception>
        public virtual void RemoveSessionContext(RpceServerSessionContext sessionContext)
        {
            if (sessionContext == null)
            {
                throw new ArgumentNullException("sessionContext");
            }

            for (int i = 0; i < this.ServerContexts.Count; i++)
            {
                this.ServerContexts[i].RemoveSessionContext(sessionContext);
            }
        }


        /// <summary>
        /// Set callback to create a ServerSecurityContext.
        /// </summary>
        /// <param name="securityContextCreator">A callback to create ServerSecurityContext.</param>
        public virtual void SetSecurityContextCreator(
            RpceSecurityContextCreatingEventHandler securityContextCreator)
        {
            this.securityContextImp = securityContextCreator;
        }


        #region private method

        /// <summary>
        /// Make sure tcp or smb transport is valid, otherwise, throw exception.
        /// </summary>
        private void EnsureTransportIsValid()
        {
            if (this.tcpTransport == null && this.smbTransport == null)
            {
                throw new InvalidOperationException("Transport is null, Please call Start() first.");
            }
        }


        /// <summary>
        /// Check if transportEvent is not null, and is not exception.
        /// </summary>
        /// <param name="transportEvent">An instance of TransportEvent to check.</param>
        /// <param name="expectedEventType">Expected event type, throw exception when not equal.</param>
        private void ValidateTransportEvent(TransportEvent transportEvent, EventType expectedEventType)
        {
            if (transportEvent == null)
            {
                throw new InvalidOperationException("Unknown object received from transport.");
            }
            if (transportEvent.EventType == EventType.Exception)
            {
                throw new InvalidOperationException(
                    "Error occurred, please find details in innerException.",
                    (Exception)transportEvent.EventObject);
            }
            if ((transportEvent.EventType & expectedEventType) == 0)
            {
                throw new InvalidOperationException(
                    string.Format("{0} (unexpected) event received.", transportEvent.EventType));
            }
        }


        /// <summary>
        /// Callback function for decoding PDU.
        /// </summary>
        /// <param name="remoteEndpoint">remote endpoint</param>
        /// <param name="messageBytes">bytes received</param>
        /// <param name="consumedLength">num of bytes consumed in processing</param>
        /// <param name="expectedLength">num of bytes expected if the bytes is not enough</param>
        /// <returns>pdus</returns>
        private StackPacket[] RpceDecodePduCallback(
            object remoteEndpoint,
            byte[] messageBytes,
            out int consumedLength,
            out int expectedLength)
        {
            RpceServerSessionContext sessionContext
                = this.serverContextManager.LookupSessionContext(remoteEndpoint);

            RpceCoPdu[] pduList = RpceUtility.DecodeCoPdu(
                sessionContext,
                messageBytes,
                out consumedLength,
                out expectedLength);

            for (int i = 0; i < pduList.Length; i++)
            {
                RpceCoPdu pdu = pduList[i];
                if (pdu.PTYPE == RpcePacketType.Bind)
                {
                    RpceCoBindPdu bindPdu = pdu as RpceCoBindPdu;
                    if (bindPdu != null && bindPdu.auth_verifier != null)
                    {
                        sessionContext.AuthenticationContextId
                            = bindPdu.auth_verifier.Value.auth_context_id;
                        sessionContext.AuthenticationType
                            = (RpceAuthenticationType)bindPdu.auth_verifier.Value.auth_type;
                        sessionContext.AuthenticationLevel
                            = (RpceAuthenticationLevel)bindPdu.auth_verifier.Value.auth_level;

                        sessionContext.SecurityContext = securityContextImp(sessionContext);
                    }
                }

                if (!pdu.ValidateAuthenticationToken())
                {
                    throw new InvalidOperationException("RPCE signature is invalid.");
                }
            }

            return pduList;
        }


        /// <summary>
        /// Receive Tcp data to store in a global list.
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown when transport is null.</exception>
        /// <exception cref="InvalidOperationException">Thrown when Unknown object received.</exception>
        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        private void TcpReceiveLoop()
        {
            if (this.tcpTransport == null)
            {
                throw new InvalidOperationException(
                   "The transport is null or not started. Please invoke Start() first.");
            }

            while (true)
            {
                lock (this.tcpThreadLocker)
                {
                    if (this.openedTcpPortList.Count == 0)
                    {
                        break;
                    }

                    try
                    {
                        TransportEvent transportEvent =
                            this.tcpTransport.ExpectTransportEvent(TRANSPORT_RECEIVE_LOOP_TIMEOUT);

                        if (transportEvent.EventType == EventType.Connected
                            || transportEvent.EventType == EventType.Disconnected
                            || transportEvent.EventType == EventType.ReceivedPacket)
                        {
                            if (transportEvent.LocalEndPoint == null)
                            {
                                throw new InvalidOperationException("Local endpoint is missing");
                            }

                            IPEndPoint localEndpoint = (IPEndPoint)transportEvent.LocalEndPoint;

                            RpceServerContext serverContext = this.serverContextManager.LookupServerContext(
                                RpceUtility.RPC_OVER_TCPIP_PROTOCOL_SEQUENCE,
                                localEndpoint.Port.ToString());
                            RpceServerSessionContext sessionContext = null;
                            if (transportEvent.RemoteEndPoint != null)
                            {
                                IPEndPoint remoteEndpoint = (IPEndPoint)transportEvent.RemoteEndPoint;
                                sessionContext = serverContext.LookupSessionContext(remoteEndpoint);
                            }

                            this.receivedTransportEvents.Enqueue(
                                new RpceTransportEvent(
                                    transportEvent.EventType,
                                    transportEvent.RemoteEndPoint,
                                    transportEvent.LocalEndPoint,
                                    serverContext,
                                    sessionContext,
                                    transportEvent.EventObject as RpcePdu));
                        }
                        else
                        {
                            // This is an exception or something unknown.
                            this.receivedTransportEvents.Enqueue(
                                new RpceTransportEvent(
                                    transportEvent.EventType,
                                    transportEvent.RemoteEndPoint,
                                    transportEvent.LocalEndPoint,
                                    transportEvent.EventObject));
                        }
                    }
                    catch (TimeoutException)
                    {
                        continue; //continue while loop
                    }
                    catch (Exception ex)
                    {
                        // Because this is inside a receive loop thread, 
                        // save the exception into event queue.
                        this.receivedTransportEvents.Enqueue(
                            new RpceTransportEvent(
                                EventType.Exception,
                                null,
                                null,
                                ex));
                        break; // exit while loop
                    }
                }
            }

            lock (this.tcpThreadLocker)
            {
                this.tcpReceiveThread = null;
            }
        }


        /// <summary>
        /// Receive SMB data to store in a global list.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// Thrown when the transport is not started.
        /// </exception>
        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        [SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling")]
        [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        private void SmbReceiveLoop()
        {
            if (this.smbTransport == null)
            {
                throw new InvalidOperationException(
                   "The transport is null or not started. Please invoke Start() first.");
            }

            while (true)
            {
                lock (this.smbThreadLocker)
                {
                    if (this.openedNamedPipeList.Count == 0)
                    {
                        break;
                    }

                    try
                    {
                        IFileServiceServerConnection connection;
                        IFileServiceServerSession session;
                        IFileServiceServerTreeConnect treeConnect;
                        IFileServiceServerOpen open;
                        SmbFamilyPacket requestPacket;
                        this.smbTransport.ExpectRequest(
                            TRANSPORT_RECEIVE_LOOP_TIMEOUT,
                            out connection,
                            out session,
                            out treeConnect,
                            out open,
                            out requestPacket);

                        Cifs.SmbPacket packet = requestPacket as Cifs.SmbPacket;
                        string shareName = (treeConnect != null) ? treeConnect.Name : null;
                        string namedPipeName = (open != null) ? open.PathName : null;

                        if (packet == null || connection == null)
                        {
                            // not interested, ask transport layer to handle it.
                            this.smbTransport.DefaultSendResponse(
                                connection,
                                session,
                                treeConnect,
                                open,
                                requestPacket);
                            continue;
                        }
                        else if (packet.SmbHeader.Command == Cifs.SmbCommand.SMB_COM_NEGOTIATE)
                        {
                            this.smbTransport.DefaultSendResponse(
                                connection,
                                session,
                                treeConnect,
                                open,
                                requestPacket);
                            continue;
                        }
                        else if (packet.SmbHeader.Command == Cifs.SmbCommand.SMB_COM_SESSION_SETUP_ANDX)
                        {
                            session = this.smbTransport.SendSessionSetupResponse(
                                connection,
                                requestPacket);
                            continue;
                        }
                        else if (packet.SmbHeader.Command == Cifs.SmbCommand.SMB_COM_TREE_CONNECT_ANDX)
                        {
                            treeConnect = this.smbTransport.SendTreeConnectResponse(
                                session,
                                requestPacket);

                            shareName = treeConnect.Name;

                            if (shareName == null || !shareName.EndsWith(RpceUtility.NAMED_PIPE_SHARENAME))
                            {
                                // this is not a RPC call.
                                this.smbTransport.Disconnect(connection);
                            }

                            continue;
                        }
                        else if (packet.SmbHeader.Command == Cifs.SmbCommand.SMB_COM_NT_CREATE_ANDX)
                        {
                            Cifs.SmbNtCreateAndxRequestPacket createRequest = packet as Cifs.SmbNtCreateAndxRequestPacket;
                            if ((createRequest.SmbHeader.Flags2 & Cifs.SmbFlags2.SMB_FLAGS2_UNICODE) == 0)
                            {
                                namedPipeName = Encoding.ASCII.GetString(createRequest.SmbData.FileName);
                            }
                            else
                            {
                                namedPipeName = Encoding.Unicode.GetString(createRequest.SmbData.FileName);
                            }
                            namedPipeName = namedPipeName.TrimEnd('\0');
                            // continue executin after get server context.
                        }

                        RpceServerContext serverContext;
                        if (namedPipeName == null)
                        {
                            serverContext = null;
                        }
                        else
                        {
                            namedPipeName = RpceUtility.NAMED_PIPE_ENDPOINT_PREFIX + namedPipeName.TrimStart('\\');
                            serverContext = serverContextManager.LookupServerContext(
                                RpceUtility.RPC_OVER_NAMED_PIPE_PROTOCOL_SEQUENCE,
                                namedPipeName);
                        }

                        if (packet.SmbHeader.Command == Cifs.SmbCommand.SMB_COM_NT_CREATE_ANDX)
                        {
                            if (serverContext == null)
                            {
                                this.smbTransport.SendErrorResponse(connection, (uint)NtStatus.STATUS_OBJECT_NAME_NOT_FOUND, requestPacket);
                                continue;
                            }

                            open = this.smbTransport.SendCreateResponse(treeConnect, requestPacket);
                            
                            //FileId should not null after sending response.
                            long fid = open.FileId;

                            RpceServerSessionContext sessionContext
                                = (serverContext != null)
                                ? sessionContext = serverContext.LookupSessionContext(fid)
                                : null;

                            if (sessionContext != null)
                            {
                                sessionContext.fsConnection = connection;
                                sessionContext.fsOpen = open;
                            }

                            this.receivedTransportEvents.Enqueue(
                                new RpceTransportEvent(
                                    EventType.Connected,
                                    fid,
                                    namedPipeName,
                                    serverContext,
                                    sessionContext,
                                    null));

                            continue;
                        }

                        if (serverContext != null)
                        {
                            long fid = open.FileId;

                            RpceServerSessionContext sessionContext
                                = (serverContext != null)
                                ? sessionContext = serverContext.LookupSessionContext(fid)
                                : null;

                            if (packet.SmbHeader.Command == Cifs.SmbCommand.SMB_COM_CLOSE)
                            {
                                this.smbTransport.SendCloseResponse(open);

                                if (sessionContext != null)
                                {
                                    sessionContext.fsOpen = null;
                                }

                                this.receivedTransportEvents.Enqueue(
                                    new RpceTransportEvent(
                                        EventType.Disconnected,
                                        fid,
                                        namedPipeName,
                                        serverContext,
                                        sessionContext,
                                        null));

                                continue;
                            }
                            else if (packet.SmbHeader.Command == Cifs.SmbCommand.SMB_COM_TRANSACTION2)
                            {
                                Cifs.SmbTrans2QueryFileInformationRequestPacket trans2QueryFileInfoRequest = packet as Cifs.SmbTrans2QueryFileInformationRequestPacket;
                                if (trans2QueryFileInfoRequest != null
                                    && trans2QueryFileInfoRequest.Trans2Parameters.InformationLevel == Cifs.QueryInformationLevel.SMB_QUERY_FILE_STANDARD_INFO)
                                {
                                    Cifs.SMB_QUERY_FILE_STANDARD_INFO_OF_TRANS2_QUERY_PATH_INFORMATION standardInfo;
                                    standardInfo.AllocationSize = 0x1000;
                                    standardInfo.EndOfFile = 0;
                                    standardInfo.NumberOfLinks = 1;
                                    standardInfo.DeletePending = 0x01; //true
                                    standardInfo.Directory = 0x00; //false
                                    this.smbTransport.SendTrans2QueryFileInformationResponse(open, standardInfo);

                                    continue;
                                }
                            }
                            else if (packet.SmbHeader.Command == Cifs.SmbCommand.SMB_COM_WRITE_ANDX)
                            {
                                Smb.SmbWriteAndxRequestPacket writeRequest = packet as Smb.SmbWriteAndxRequestPacket;

                                //Only one thread will read/write smbBufferReceiving, not necessary to lock.
                                byte[] buf = writeRequest.SmbData.Data;
                                this.smbTransport.SendWriteResponse(open, buf.Length);

                                ProcessReceivedData(sessionContext, buf);

                                continue;
                            }
                            else if (packet.SmbHeader.Command == Cifs.SmbCommand.SMB_COM_READ_ANDX)
                            {
                                Cifs.SmbReadAndxRequestPacket readRequest = packet as Cifs.SmbReadAndxRequestPacket;

                                lock (sessionContext.smbSendingLocker)
                                {
                                    if (sessionContext.smbSendingFunc != null)
                                    {
                                        throw new InvalidOperationException("Sending function is not null.");
                                    }
                                    sessionContext.smbMaxSendingLength = readRequest.SmbParameters.MaxNumberOfBytesToReturn;
                                    sessionContext.smbSendingFunc = delegate()
                                    {
                                        byte[] sendingBuf = PrepareSendingBuffer(sessionContext);
                                        
                                        if (sendingBuf.Length == 0)
                                        {
                                            this.smbTransport.SendErrorResponse(
                                                connection,
                                                (uint)NtStatus.STATUS_PIPE_EMPTY,
                                                requestPacket);
                                        }
                                        else
                                        {
                                            this.smbTransport.SendReadResponse(
                                                open,
                                                sendingBuf,
                                                sessionContext.smbBufferSending.Length);
                                        }

                                        sessionContext.smbSendingFunc = null;
                                    };

                                    if (sessionContext.smbBufferSending.Length > 0)
                                    {
                                        sessionContext.smbSendingFunc();
                                    }
                                }

                                continue;
                            }
                            else if (packet.SmbHeader.Command == Cifs.SmbCommand.SMB_COM_TRANSACTION)
                            {
                                Smb.SmbTransTransactNmpipeRequestPacket smbTransTransactNmpipeRequest
                                    = packet as Smb.SmbTransTransactNmpipeRequestPacket;

                                if (smbTransTransactNmpipeRequest != null)
                                {
                                    lock (sessionContext.smbSendingLocker)
                                    {
                                        if (sessionContext.smbSendingFunc != null)
                                        {
                                            throw new InvalidOperationException("Sending function is not null.");
                                        }
                                        sessionContext.smbMaxSendingLength = smbTransTransactNmpipeRequest.SmbParameters.MaxDataCount;
                                        sessionContext.smbSendingFunc = delegate()
                                        {
                                            byte[] sendingBuf = PrepareSendingBuffer(sessionContext);
                                            this.smbTransport.SendTransTransactNmpipeResponse(
                                                open,
                                                sendingBuf,
                                                sessionContext.smbBufferSending.Length);
                                            sessionContext.smbSendingFunc = null;
                                        };
                                    }

                                    ProcessReceivedData(
                                        sessionContext,
                                        smbTransTransactNmpipeRequest.TransData.WriteData);

                                    continue;
                                }
                            }
                        }

                        // no response has been sent, reply with default.
                        this.smbTransport.DefaultSendResponse(
                            connection,
                            session,
                            treeConnect,
                            open,
                            requestPacket);
                    }
                    catch (TimeoutException)
                    {
                        continue; //continue while loop
                    }
                    catch (Exception ex)
                    {
                        // Because this is inside a receive loop thread, 
                        // save the exception into event queue.
                        this.receivedTransportEvents.Enqueue(
                            new RpceTransportEvent(
                                EventType.Exception,
                                null,
                                null,
                                ex));
                        break; // exit while loop
                    }
                }
            }

            lock (this.smbThreadLocker)
            {
                this.smbReceiveThread = null;
            }
        }


        private static byte[] PrepareSendingBuffer(RpceServerSessionContext sessionContext)
        {
            byte[] buf = sessionContext.smbBufferSending;
            sessionContext.smbBufferSending = new byte[0];

            if (sessionContext.smbMaxSendingLength < buf.Length)
            {
                sessionContext.smbBufferSending = ArrayUtility.SubArray(
                    buf,
                    sessionContext.smbMaxSendingLength);
                buf = ArrayUtility.SubArray(
                    buf,
                    0,
                    sessionContext.smbMaxSendingLength);
            }

            return buf;
        }


        private void ProcessReceivedData(
            RpceServerSessionContext sessionContext,
            byte[] buf)
        {
            RpceServerContext serverContext = sessionContext.ServerContext;
            string namedPipeName = serverContext.Endpoint;
            object fid = sessionContext.fsOpen.FileId;

            buf = ArrayUtility.ConcatenateArrays(sessionContext.smbBufferReceiving, buf);

            int consumedLength;
            int expectedLength;

            RpceCoPdu[] pduList = (RpceCoPdu[])this.RpceDecodePduCallback(
                fid,
                buf,
                out consumedLength,
                out expectedLength);

            for (int i = 0; i < pduList.Length; i++)
            {
                this.receivedTransportEvents.Enqueue(
                    new RpceTransportEvent(
                        EventType.ReceivedPacket,
                        fid,
                        namedPipeName,
                        serverContext,
                        sessionContext,
                        pduList[i]));
            }

            sessionContext.smbBufferReceiving = ArrayUtility.SubArray(buf, consumedLength);
        }


        #endregion


        #region IDisposable Members

        /// <summary>
        /// the dispose flags 
        /// </summary>
        private bool disposed;


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
            if (!this.disposed)
            {
                Thread tcpThread = this.tcpReceiveThread;
                Thread smbThread = this.smbReceiveThread;

                this.StopAll();

                if (tcpThread != null)
                {
                    tcpThread.Join();
                }

                if (smbThread != null)
                {
                    smbThread.Join();
                }

                if (disposing)
                {
                    //Release managed resources.
                    if (this.tcpTransport != null)
                    {
                        this.tcpTransport.Dispose();
                        this.tcpTransport = null;
                    }

                    if (this.smbTransport != null)
                    {
                        this.smbTransport.Dispose();
                        this.smbTransport = null;
                    }

                    this.receivedTransportEvents.Dispose();
                }

                this.disposed = true;
            }
        }


        /// <summary>
        /// finalizer
        /// </summary>
        ~RpceServer()
        {
            Dispose(false);
        }

        #endregion
    }
}
