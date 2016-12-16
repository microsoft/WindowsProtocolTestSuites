// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

using Microsoft.Protocols.TestTools.StackSdk.FileAccessService;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;
using Microsoft.Protocols.TestTools.StackSdk.Security.Sspi;
using Microsoft.Protocols.TestTools.StackSdk.Transport;

namespace Microsoft.Protocols.TestTools.StackSdk.Networking.Rpce
{
    /// <summary>
    /// RPCE client.
    /// </summary>
    public class RpceClient : IDisposable
    {
        // RPCE context
        private RpceClientContext context;

        // RPCE handle
        private IntPtr handle;

        // Queue for PIPE_TRANSCEIVE response data (SMB/SMB2)
        private Queue<byte[]> pipeTransceiveResponseQueue;

        // Timeout for File Sharing Transport, same as connect.
        private TimeSpan timeoutForFsTransport;


        private bool isDisposed = false;

        /// <summary>
        /// true if disposed
        /// </summary>
        public bool IsDisposed
        {
            get
            {
                lock (this)
                    return isDisposed;
            }
        }


        /// <summary>
        /// Create and initialize an instance of RpceClient class.
        /// </summary>
        public RpceClient()
        {
            context = new RpceClientContext();
        }


        /// <summary>
        /// RPCE client context.
        /// </summary>
        public RpceClientContext Context
        {
            get
            {
                return context;
            }
        }


        /// <summary>
        /// RPCE client handle.
        /// </summary>
        public IntPtr Handle
        {
            get
            {
                return handle;
            }
        }


        /// <summary>
        /// Set the security provider and authentication level of the RPCE connection.
        /// </summary>
        /// <param name="securityContext">
        /// A security provider.
        /// </param>
        /// <param name="authenticationLevel">
        /// An authentication level.
        /// </param>
        /// <param name="authenticationContextId">
        /// Authentication context id, can be any value, 
        /// but must be different in different security context.
        /// </param>
        /// <exception cref="ArgumentException">
        /// Thrown when securityProvider is null but authenticationLevel is not NONE.
        /// </exception>
        /// <exception cref="NotSupportedException">
        /// Thrown when securityProvider is not supported.
        /// </exception>
        public void SetAuthInfo(
            ClientSecurityContext securityContext,
            RpceAuthenticationLevel authenticationLevel,
            uint authenticationContextId)
        {
            if (securityContext == null)
            {
                if (authenticationLevel != RpceAuthenticationLevel.RPC_C_AUTHN_LEVEL_NONE)
                {
                    throw new ArgumentException(
                        "auth_level must set to NONE when securityProvider is null",
                        "authenticationLevel");
                }

                context.AuthenticationType = RpceAuthenticationType.RPC_C_AUTHN_NONE;
            }
            else
            {
                securityContext.Initialize(null);

                switch (securityContext.PackageType)
                {
                    case SecurityPackageType.Kerberos:
                        context.AuthenticationType = RpceAuthenticationType.RPC_C_AUTHN_GSS_KERBEROS;
                        break;

                    case SecurityPackageType.Ntlm:
                        context.AuthenticationType = RpceAuthenticationType.RPC_C_AUTHN_WINNT;
                        break;

                    case SecurityPackageType.Negotiate:
                        context.AuthenticationType = RpceAuthenticationType.RPC_C_AUTHN_GSS_NEGOTIATE;
                        break;

                    case SecurityPackageType.NetLogon:
                        context.AuthenticationType = RpceAuthenticationType.RPC_C_AUTHN_NETLOGON;
                        break;

                    default:
                        throw new NotSupportedException("Specified SSPI is not supported.");
                }
            }

            context.SecurityContext = securityContext;
            context.AuthenticationLevel = authenticationLevel;
            context.AuthenticationContextId = authenticationContextId;
        }


        /// <summary>
        /// Create an RpceCoBindPdu.
        /// </summary>
        /// <param name="rpcVersionMinor">
        /// RPC version minor.
        /// </param>
        /// <param name="pfcFlags">
        /// PFC_*** flags.
        /// </param>
        /// <param name="callId">
        /// Call Id.
        /// </param>
        /// <param name="maxTransmitFragmentSize">
        /// Max transmit fragment size, in bytes.
        /// </param>
        /// <param name="maxReceiveFragmentSize">
        /// Max receive fragment size, in bytes.
        /// </param>
        /// <param name="associateGroupId">
        /// Associated group id.
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
        /// <param name="ndrVersion">
        /// NDR version to be used to marshal/un-marshal stub.
        /// </param>
        /// <param name="bindTimeFeatureNegotiationBitmask">
        /// BindTimeFeatureNegotiationBitmask to sent. 
        /// Set the value to null if client donot support the feature.
        /// </param>
        /// <returns>
        /// Created RpceCoBindPdu, it's ok to be sent out if there's 
        /// no modification to any field of the PDU.
        /// </returns>
        public RpceCoBindPdu CreateCoBindPdu(
            byte rpcVersionMinor,
            RpceCoPfcFlags pfcFlags,
            uint callId,
            ushort maxTransmitFragmentSize,
            ushort maxReceiveFragmentSize,
            uint associateGroupId,
            Guid interfaceId,
            ushort interfaceMajorVersion,
            ushort interfaceMinorVersion,
            RpceNdrVersion ndrVersion,
            RpceBindTimeFeatureNegotiationBitmask? bindTimeFeatureNegotiationBitmask)
        {
            RpceCoBindPdu bindPdu = new RpceCoBindPdu(context);

            bindPdu.rpc_vers = 5;
            bindPdu.rpc_vers_minor = rpcVersionMinor;
            bindPdu.PTYPE = RpcePacketType.Bind;
            bindPdu.pfc_flags = pfcFlags
                | RpceCoPfcFlags.PFC_FIRST_FRAG
                | RpceCoPfcFlags.PFC_LAST_FRAG;
            // test suite always sends packets in little endian
            bindPdu.packed_drep.dataRepFormat = RpceDataRepresentationFormat.IEEE_LittleEndian_ASCII;
            bindPdu.packed_drep.reserved = 0;
            bindPdu.call_id = callId;

            bindPdu.max_xmit_frag = maxTransmitFragmentSize;
            bindPdu.max_recv_frag = maxReceiveFragmentSize;
            bindPdu.assoc_group_id = associateGroupId;

            List<p_cont_elem_t> p_cont_elem_list = new List<p_cont_elem_t>();
            if ((ndrVersion & RpceNdrVersion.NDR) == RpceNdrVersion.NDR)
            {
                p_cont_elem_t p_cont_elem = new p_cont_elem_t();
                p_cont_elem.p_cont_id = (ushort)(p_cont_elem_list.Count);
                p_cont_elem.n_transfer_syn = 1;
                p_cont_elem.reserved = 0;
                p_cont_elem.abstract_syntax.if_uuid = interfaceId;
                p_cont_elem.abstract_syntax.if_vers_major = interfaceMajorVersion;
                p_cont_elem.abstract_syntax.if_vers_minor = interfaceMinorVersion;
                p_cont_elem.transfer_syntaxes = new p_syntax_id_t[1];
                p_cont_elem.transfer_syntaxes[0].if_uuid
                    = RpceUtility.NDR_INTERFACE_UUID;
                p_cont_elem.transfer_syntaxes[0].if_vers_major
                    = RpceUtility.NDR_INTERFACE_MAJOR_VERSION;
                p_cont_elem.transfer_syntaxes[0].if_vers_minor
                    = RpceUtility.NDR_INTERFACE_MINOR_VERSION;
                p_cont_elem_list.Add(p_cont_elem);
            }
            if ((ndrVersion & RpceNdrVersion.NDR64) == RpceNdrVersion.NDR64)
            {
                p_cont_elem_t p_cont_elem = new p_cont_elem_t();
                p_cont_elem.p_cont_id = (ushort)(p_cont_elem_list.Count);
                p_cont_elem.n_transfer_syn = 1;
                p_cont_elem.reserved = 0;
                p_cont_elem.abstract_syntax.if_uuid = interfaceId;
                p_cont_elem.abstract_syntax.if_vers_major = interfaceMajorVersion;
                p_cont_elem.abstract_syntax.if_vers_minor = interfaceMinorVersion;
                p_cont_elem.transfer_syntaxes = new p_syntax_id_t[1];
                p_cont_elem.transfer_syntaxes[0].if_uuid
                    = RpceUtility.NDR64_INTERFACE_UUID;
                p_cont_elem.transfer_syntaxes[0].if_vers_major
                    = RpceUtility.NDR64_INTERFACE_MAJOR_VERSION;
                p_cont_elem.transfer_syntaxes[0].if_vers_minor
                    = RpceUtility.NDR64_INTERFACE_MINOR_VERSION;
                p_cont_elem_list.Add(p_cont_elem);
            }
            if (bindTimeFeatureNegotiationBitmask != null)
            {
                p_cont_elem_t p_cont_elem = new p_cont_elem_t();
                p_cont_elem.p_cont_id = (ushort)(p_cont_elem_list.Count);
                p_cont_elem.n_transfer_syn = 1;
                p_cont_elem.reserved = 0;
                p_cont_elem.abstract_syntax.if_uuid = interfaceId;
                p_cont_elem.abstract_syntax.if_vers_major = interfaceMajorVersion;
                p_cont_elem.abstract_syntax.if_vers_minor = interfaceMinorVersion;
                p_cont_elem.transfer_syntaxes = new p_syntax_id_t[1];
                byte[] guidBytes = new byte[RpceUtility.GUID_SIZE]; // length of a Guid
                Buffer.BlockCopy(
                    RpceUtility.BIND_TIME_FEATURE_NEGOTIATION_BITMASK_GUID_BYTES,
                    0,
                    guidBytes,
                    0,
                    RpceUtility.GUID_SIZE);
                guidBytes[RpceUtility.BIND_TIME_FEATURE_NEGOTIATION_BITMASK_PREFIX_LENGTH] 
                    = (byte)bindTimeFeatureNegotiationBitmask.Value;
                p_cont_elem.transfer_syntaxes[0].if_uuid = new Guid(guidBytes);
                p_cont_elem.transfer_syntaxes[0].if_vers_major = 1;
                p_cont_elem.transfer_syntaxes[0].if_vers_minor = 0;
                p_cont_elem_list.Add(p_cont_elem);
            }
            bindPdu.p_context_elem.n_context_elem = (byte)p_cont_elem_list.Count;
            bindPdu.p_context_elem.reserved = 0;
            bindPdu.p_context_elem.reserved2 = 0;
            bindPdu.p_context_elem.p_cont_elem = p_cont_elem_list.ToArray();

            bindPdu.AppendAuthenticationVerifier();
            bindPdu.SetLength();

            return bindPdu;
        }


        /// <summary>
        /// Create a RpceCoAlterContextPdu.
        /// </summary>
        /// <returns>
        /// Created RpceCoAlterContextPdu, it's ok to be sent out if there's 
        /// no modification to any field of the PDU.
        /// </returns>
        public RpceCoAlterContextPdu CreateCoAlterContextPdu()
        {
            RpceCoAlterContextPdu alterContextPdu = new RpceCoAlterContextPdu(context);

            alterContextPdu.rpc_vers = context.RpcVersionMajor;
            alterContextPdu.rpc_vers_minor = context.RpcVersionMinor;
            alterContextPdu.PTYPE = RpcePacketType.AlterContext;
            alterContextPdu.pfc_flags = RpceUtility.GeneratePfcFlags(context, RpcePacketType.AlterContext);
            // test suite always sends packets in little endian
            alterContextPdu.packed_drep.dataRepFormat = RpceDataRepresentationFormat.IEEE_LittleEndian_ASCII;
            alterContextPdu.packed_drep.reserved = 0;
            alterContextPdu.call_id = ComputeNextCallId();

            alterContextPdu.max_xmit_frag = 0;
            alterContextPdu.max_recv_frag = 0;
            alterContextPdu.assoc_group_id = 0;

            alterContextPdu.p_context_elem.n_context_elem = 1;
            alterContextPdu.p_context_elem.reserved = 0;
            alterContextPdu.p_context_elem.reserved2 = 0;
            alterContextPdu.p_context_elem.p_cont_elem = new p_cont_elem_t[1];

            p_cont_elem_t p_cont_elem = new p_cont_elem_t();
            p_cont_elem.p_cont_id = context.ContextIdentifier;
            p_cont_elem.n_transfer_syn = 1;
            p_cont_elem.reserved = 0;
            p_cont_elem.abstract_syntax.if_uuid = context.InterfaceId;
            p_cont_elem.abstract_syntax.if_vers_major = context.InterfaceMajorVersion;
            p_cont_elem.abstract_syntax.if_vers_minor = context.InterfaceMinorVersion;
            p_cont_elem.transfer_syntaxes = new p_syntax_id_t[1];
            if (context.NdrVersion == RpceNdrVersion.NDR)
            {
                p_cont_elem.transfer_syntaxes[0].if_uuid = RpceUtility.NDR_INTERFACE_UUID;
                p_cont_elem.transfer_syntaxes[0].if_vers_major = RpceUtility.NDR_INTERFACE_MAJOR_VERSION;
                p_cont_elem.transfer_syntaxes[0].if_vers_minor = RpceUtility.NDR_INTERFACE_MINOR_VERSION;
            }
            else if (context.NdrVersion == RpceNdrVersion.NDR64)
            {
                p_cont_elem.transfer_syntaxes[0].if_uuid = RpceUtility.NDR64_INTERFACE_UUID;
                p_cont_elem.transfer_syntaxes[0].if_vers_major = RpceUtility.NDR64_INTERFACE_MAJOR_VERSION;
                p_cont_elem.transfer_syntaxes[0].if_vers_minor = RpceUtility.NDR64_INTERFACE_MINOR_VERSION;
            }
            alterContextPdu.p_context_elem.p_cont_elem[0] = p_cont_elem;

            alterContextPdu.AppendAuthenticationVerifier();
            alterContextPdu.SetLength();

            return alterContextPdu;
        }


        /// <summary>
        /// Create a RpceCoAuth3Pdu.
        /// </summary>
        /// <returns>
        /// Created RpceCoAuth3Pdu, it's ok to be sent out if there's 
        /// no modification to any field of the PDU.
        /// </returns>
        public RpceCoAuth3Pdu CreateCoAuth3Pdu()
        {
            RpceCoAuth3Pdu auth3Pdu = new RpceCoAuth3Pdu(context);

            auth3Pdu.rpc_vers = context.RpcVersionMajor;
            auth3Pdu.rpc_vers_minor = context.RpcVersionMinor;
            auth3Pdu.PTYPE = RpcePacketType.Auth3;
            auth3Pdu.pfc_flags = RpceUtility.GeneratePfcFlags(context, RpcePacketType.Auth3);
            // test suite always sends packets in little endian
            auth3Pdu.packed_drep.dataRepFormat = RpceDataRepresentationFormat.IEEE_LittleEndian_ASCII;
            auth3Pdu.packed_drep.reserved = 0;
            auth3Pdu.call_id = ComputeNextCallId();

            auth3Pdu.pad = 0;

            auth3Pdu.AppendAuthenticationVerifier();
            auth3Pdu.SetLength();

            return auth3Pdu;
        }


        /// <summary>
        /// Create a RpceCoRequestPdu.
        /// </summary>
        /// <param name="opnum">
        /// Opnum of a method.
        /// </param>
        /// <param name="stub">
        /// Request stub.
        /// </param>
        /// <returns>
        /// Created RpceCoRequestPdu, it's ok to be sent out if there's 
        /// no modification to any field of the PDU.
        /// </returns>
        public RpceCoRequestPdu CreateCoRequestPdu(
            ushort opnum,
            byte[] stub)
        {
            if (stub == null)
            {
                stub = new byte[0];
            }

            RpceCoRequestPdu requestPdu = new RpceCoRequestPdu(context);

            requestPdu.rpc_vers = context.RpcVersionMajor;
            requestPdu.rpc_vers_minor = context.RpcVersionMinor;
            requestPdu.PTYPE = RpcePacketType.Request;
            requestPdu.pfc_flags = RpceUtility.GeneratePfcFlags(context, RpcePacketType.Request);
            // test suite always sends packets in little endian
            requestPdu.packed_drep.dataRepFormat = RpceDataRepresentationFormat.IEEE_LittleEndian_ASCII;
            requestPdu.packed_drep.reserved = 0;
            requestPdu.call_id = ComputeNextCallId();

            requestPdu.alloc_hint = (uint)stub.Length;
            requestPdu.p_cont_id = context.ContextIdentifier;
            requestPdu.opnum = opnum;
            requestPdu.stub = stub;

            requestPdu.AppendAuthenticationVerifier();
            requestPdu.SetLength();

            return requestPdu;
        }


        /// <summary>
        /// Create a RpceCoResponsePdu.
        /// </summary>
        /// <param name="callId">
        /// Call Id.
        /// </param>
        /// <param name="stub">
        /// Response stub.
        /// </param>
        /// <returns>
        /// Created RpceCoResponsePdu, it's ok to be sent out if there's 
        /// no modification to any field of the PDU.
        /// </returns>
        public RpceCoResponsePdu CreateCoResponsePdu(
            uint callId,
            byte[] stub)
        {
            if (stub == null)
            {
                stub = new byte[0];
            }

            RpceCoResponsePdu responsePdu = new RpceCoResponsePdu(context);

            responsePdu.rpc_vers = context.RpcVersionMajor;
            responsePdu.rpc_vers_minor = context.RpcVersionMinor;
            responsePdu.PTYPE = RpcePacketType.Response;
            responsePdu.pfc_flags = RpceUtility.GeneratePfcFlags(context, RpcePacketType.Response);
            // test suite always sends packets in little endian
            responsePdu.packed_drep.dataRepFormat = RpceDataRepresentationFormat.IEEE_LittleEndian_ASCII;
            responsePdu.packed_drep.reserved = 0;
            responsePdu.call_id = callId;

            responsePdu.alloc_hint = (uint)stub.Length;
            responsePdu.p_cont_id = context.ContextIdentifier;
            responsePdu.stub = stub;
            responsePdu.cancel_count = 0;
            responsePdu.reserved = 0;

            responsePdu.AppendAuthenticationVerifier();
            responsePdu.SetLength();

            return responsePdu;
        }


        /// <summary>
        /// Create a RpceCoCancelPdu.
        /// </summary>
        /// <param name="callId">
        /// Call Id.
        /// </param>
        /// <returns>
        /// Created RpceCoCancelPdu, it's ok to be sent out if there's 
        /// no modification to any field of the PDU.
        /// </returns>
        public RpceCoCancelPdu CreateCoCancelPdu(uint callId)
        {
            RpceCoCancelPdu cancelPdu = new RpceCoCancelPdu(context);

            cancelPdu.rpc_vers = context.RpcVersionMajor;
            cancelPdu.rpc_vers_minor = context.RpcVersionMinor;
            cancelPdu.PTYPE = RpcePacketType.CoCancel;
            cancelPdu.pfc_flags = RpceUtility.GeneratePfcFlags(context, RpcePacketType.CoCancel);
            // test suite always sends packets in little endian
            cancelPdu.packed_drep.dataRepFormat = RpceDataRepresentationFormat.IEEE_LittleEndian_ASCII;
            cancelPdu.packed_drep.reserved = 0;
            cancelPdu.call_id = callId;

            cancelPdu.AppendAuthenticationVerifier();
            cancelPdu.SetLength();

            return cancelPdu;
        }


        /// <summary>
        /// Create a RpceCoOrphanedPdu.
        /// </summary>
        /// <param name="callId">
        /// Call Id.
        /// </param>
        /// <returns>
        /// Created RpceCoOrphanedPdu, it's ok to be sent out if there's 
        /// no modification to any field of the PDU.
        /// </returns>
        public RpceCoOrphanedPdu CreateCoOrphanedPdu(uint callId)
        {
            RpceCoOrphanedPdu orphanedPdu = new RpceCoOrphanedPdu(context);

            orphanedPdu.rpc_vers = context.RpcVersionMajor;
            orphanedPdu.rpc_vers_minor = context.RpcVersionMinor;
            orphanedPdu.PTYPE = RpcePacketType.Orphaned;
            orphanedPdu.pfc_flags = RpceUtility.GeneratePfcFlags(context, RpcePacketType.Orphaned);
            // test suite always sends packets in little endian
            orphanedPdu.packed_drep.dataRepFormat = RpceDataRepresentationFormat.IEEE_LittleEndian_ASCII;
            orphanedPdu.packed_drep.reserved = 0;
            orphanedPdu.call_id = callId;

            orphanedPdu.AppendAuthenticationVerifier();
            orphanedPdu.SetLength();

            return orphanedPdu;
        }


        #region Transport related methods

        /// <summary>
        /// Connect to a RPCE remote host.
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
        /// <param name="timeout">
        /// Timeout to make the connection. Ignored.
        /// </param>
        /// <exception cref="NotSupportedException">
        /// Thrown when the protocol sequence parameter 
        /// passed to the method is not supported.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// Thrown when protSeq or networkAddr or endpoint is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Thrown when protocolSequence or networkAddress or endpoint is invalid.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// Thrown when RPCE is already connected.
        /// </exception>
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters")]
        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        public void Connect(
            string protocolSequence,
            string networkAddress,
            string endpoint,
            AccountCredential transportCredential,
            TimeSpan timeout)
        {
            Connect(protocolSequence, networkAddress, endpoint, transportCredential, timeout, SecurityPackageType.Ntlm);
        }

        /// <summary>
        /// Connect to a RPCE remote host.
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
        /// <param name="timeout">
        /// Timeout to make the connection. Ignored.
        /// </param>
        /// <param name="securityPackage">
        /// security package.
        /// </param>
        /// <exception cref="NotSupportedException">
        /// Thrown when the protocol sequence parameter 
        /// passed to the method is not supported.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// Thrown when protSeq or networkAddr or endpoint is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Thrown when protocolSequence or networkAddress or endpoint is invalid.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// Thrown when RPCE is already connected.
        /// </exception>
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters")]
        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        public void Connect(
            string protocolSequence,
            string networkAddress,
            string endpoint,
            AccountCredential transportCredential, 
            TimeSpan timeout, 
            SecurityPackageType securityPackage)
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

            if (context.tcpTransport != null || context.fileServiceTransport != null)
            {
                throw new InvalidOperationException("RPCE is already connected.");
            }

            if (string.Compare(protocolSequence, RpceUtility.RPC_OVER_TCPIP_PROTOCOL_SEQUENCE, true) == 0)
            {
                IPAddress[] addresses = Dns.GetHostAddresses(networkAddress);
                if (addresses == null || addresses.Length == 0)
                {
                    throw new ArgumentException(
                        "Cannot resolve network address.",
                        "networkAddress");
                }
                IPAddress addr = addresses[0];

                int port;
                if (!int.TryParse(endpoint, out port))
                {
                    throw new ArgumentException("Invalid endpoint.", "endpoint");
                }

                SocketTransportConfig config = new SocketTransportConfig();
                config.BufferSize = Math.Max(context.MaxTransmitFragmentSize, context.MaxReceiveFragmentSize);
                config.RemoteIpAddress = addr;
                config.RemoteIpPort = port;
                config.Role = Role.Client;
                config.Type = StackTransportType.Tcp;

                context.tcpTransport = new TransportStack(config, RpceDecodePduCallback);
                context.tcpTransport.Connect();
            }
            else if (string.Compare(protocolSequence, RpceUtility.RPC_OVER_NAMED_PIPE_PROTOCOL_SEQUENCE, true) == 0)
            {
                if (!endpoint.StartsWith(
                    RpceUtility.NAMED_PIPE_ENDPOINT_PREFIX, 
                    StringComparison.InvariantCultureIgnoreCase))
                {
                    throw new ArgumentException("endpoint format is incorrect.", "endpoint");
                }

                if (transportCredential == null)
                {
                    throw new ArgumentNullException("transportCredential");
                }

                timeoutForFsTransport = timeout;
                pipeTransceiveResponseQueue = new Queue<byte[]>();

                if (!RpceUtility.DisableSmb2)
                {
                    try
                    {
                        context.fileServiceTransport = new Smb2ClientTransport(timeout);
                        context.fileServiceTransport.ConnectShare(
                            networkAddress,
                            RpceUtility.NAMED_PIPE_PORT,
                            IpVersion.Any,
                            transportCredential.DomainName,
                            transportCredential.AccountName,
                            transportCredential.Password,
                            RpceUtility.NAMED_PIPE_SHARENAME,
                            securityPackage,
                            false);
                    }
                    catch
                    {
                        context.fileServiceTransport.Dispose();
                        context.fileServiceTransport = null;
                    }
                }

                if (context.fileServiceTransport == null)
                {
                    // Remote doesn't support SMB2, use SMB.
                    context.fileServiceTransport = new SmbClientTransport();
                    context.fileServiceTransport.ConnectShare(
                        networkAddress,
                        RpceUtility.NAMED_PIPE_PORT,
                        IpVersion.Any,
                        transportCredential.DomainName,
                        transportCredential.AccountName,
                        transportCredential.Password,
                        RpceUtility.NAMED_PIPE_SHARENAME,
                        securityPackage,
                        false);
                }

                context.fileServiceTransport.Create(
                    endpoint.Substring(RpceUtility.NAMED_PIPE_ENDPOINT_PREFIX.Length), 
                    FsFileDesiredAccess.FILE_READ_DATA | FsFileDesiredAccess.FILE_WRITE_DATA
                    | FsFileDesiredAccess.FILE_APPEND_DATA 
                    | FsFileDesiredAccess.FILE_READ_EA | FsFileDesiredAccess.FILE_WRITE_EA
                    | FsFileDesiredAccess.FILE_READ_ATTRIBUTES | FsFileDesiredAccess.FILE_WRITE_ATTRIBUTES
                    | FsFileDesiredAccess.READ_CONTROL | FsFileDesiredAccess.SYNCHRONIZE,
                    FsImpersonationLevel.Impersonation,
                    FsFileAttribute.NONE,
                    FsCreateDisposition.FILE_OPEN,
                    FsCreateOption.FILE_NON_DIRECTORY_FILE | FsCreateOption.FILE_OPEN_NO_RECALL);
            }
            else
            {
                throw new NotSupportedException("Specified protocol sequence is not supported.");
            }

            context.ProtocolSequence = protocolSequence;
            context.NetworkAddress = networkAddress;
            context.Endpoint = endpoint;

            // Make handle always different.
            handle = new IntPtr(Environment.TickCount);
        }


        /// <summary>
        /// Disconnect from RPCE remote host.
        /// </summary>
        /// <param name="timeout">
        /// Timeout to disconnect. Ignored.
        /// </param>
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters")]
        public void Disconnect(TimeSpan timeout)
        {
            if (context.tcpTransport != null)
            {
                context.tcpTransport.Disconnect();
                context.tcpTransport.Dispose();
                context.tcpTransport = null;
            }
            else if (context.fileServiceTransport != null)
            {
                context.fileServiceTransport.DisconnetShare();
                context.fileServiceTransport.Dispose();
                context.fileServiceTransport = null;
            }
        }

        /// <summary>
        /// Encode a PDU to a binary stream. Then send the stream.
        /// The PDU can be got by calling method Create***Pdu.
        /// </summary>
        /// <param name="pdu">
        /// A specified type of a PDU. This argument can not be null.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when pdu parameter passed to the method is null.
        /// </exception>
        public void SendPdu(RpcePdu pdu)
        {
            if (pdu == null)
            {
                throw new ArgumentNullException("pdu");
            }

            UpdateContextOnSendingPdu(pdu);

            if (context.tcpTransport != null)
            {
                context.tcpTransport.SendPacket(pdu);
            }
            else if (context.fileServiceTransport != null)
            {
                RpceCoPdu coPdu = pdu as RpceCoPdu;
                if (coPdu != null)
                {
                    switch (coPdu.PTYPE)
                    {
                        case RpcePacketType.Request:
                        case RpcePacketType.Response:
                            if (context.fileServiceTransport is SmbClientTransport)
                            {
                                // SMB doesn't support FSCTL_PIPE_TRANSCEIVE.
                                // This is the last fragment of a request for synchronous RPCs.
                                if (context.UseTransactionForNamedPipe && !context.IsAsynchronous && ((coPdu.pfc_flags & RpceCoPfcFlags.PFC_LAST_FRAG) != 0))
                                {
                                    TransactionOverWriteAndRead(pdu);
                                }
                                else
                                {
                                    context.fileServiceTransport.Write(timeoutForFsTransport, 0, pdu.ToBytes());
                                }
                            }
                            else if ((coPdu.pfc_flags & RpceCoPfcFlags.PFC_LAST_FRAG) == 0)
                            {
                                // This is one of fragment PDU, but not the last.
                                context.fileServiceTransport.Write(timeoutForFsTransport, 0, pdu.ToBytes());
                            }
                            else
                            {
                                // This is the only PDU of a request,
                                // or this is the last fragment of a request.
                                byte[] inputResponse;
                                byte[] outputResponse;
                                context.fileServiceTransport.IoControl(
                                    timeoutForFsTransport,
                                    FsCtlCode.FSCTL_PIPE_TRANSCEIVE,
                                    pdu.ToBytes(),
                                    out inputResponse,
                                    out outputResponse,
                                    0,
                                    context.MaxOutputResponse);
                                if (outputResponse == null)
                                {
                                    throw new InvalidOperationException(
                                        "Got an error in SMB/SMB2 transport. SMB/SMB2 should throw exception.");
                                }
                                pipeTransceiveResponseQueue.Enqueue(outputResponse);
                            }
                            break;
                        case RpcePacketType.Bind:
                        case RpcePacketType.AlterContext:
                            if (context.UseTransactionForNamedPipe && (context.fileServiceTransport is SmbClientTransport) && !context.IsAsynchronous)
                            {
                                TransactionOverWriteAndRead(pdu);
                            }
                            else
                            {
                                context.fileServiceTransport.Write(timeoutForFsTransport, 0, pdu.ToBytes());
                            }
                            break;
                        case RpcePacketType.Auth3:
                        case RpcePacketType.CoCancel:
                        case RpcePacketType.Orphaned:
                        default:
                            context.fileServiceTransport.Write(timeoutForFsTransport, 0, pdu.ToBytes());
                            break;
                    }
                }
            }
            else
            {
                throw new InvalidOperationException("No connection was established.");
            }
        }

        /// <summary>
        /// MS-RPCE Section 2.1.1.2: In the case of synchronous RPCs, an implementation of these extensions MAY require the Server Message Block (SMB) Protocol implementation 
        /// to execute a transaction encompassing the write of the last request PDU and the read of the first response PDU on the client.     
        /// The last request PDU MUST be a bind, an alter_context, or the last fragment of a request. 
        /// The first response PDU MUST be a bind_ack or bind_nak, an alter_context_response, or the first fragment of a response.
        /// The transaction over a write and read is as specified in [MS-CIFS].
        /// Windows always asks the Server Message Block implementation to execute a transaction on the client for synchronous RPCs.
        /// </summary>
        /// <param name="pdu">A specified type of a PDU. This argument can not be null.</param>
        private void TransactionOverWriteAndRead(RpcePdu pdu)
        {
            byte[] readData;
            SmbClientTransport smbClientTransport = context.fileServiceTransport as SmbClientTransport;
            smbClientTransport.Transaction(timeoutForFsTransport, pdu.ToBytes(), out readData);
            if (readData == null)
            {
                throw new InvalidOperationException(
                    "Got an error in SMB/SMB2 transport. SMB/SMB2 should throw exception.");
            }
            pipeTransceiveResponseQueue.Enqueue(readData);
        }


        /// <summary>
        /// Send a byte array to the remote host. 
        /// This method is designed for negative test.
        /// </summary>
        /// <param name="pdu">
        /// The bytes to be sent. This argument can not be null.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when pdu parameter passed to the method is null.
        /// </exception>
        public void SendBytes(byte[] pdu)
        {
            if (pdu == null)
            {
                throw new ArgumentNullException("pdu");
            }

            if (context.tcpTransport != null)
            {
                context.tcpTransport.SendBytes(pdu);
            }
            else if (context.fileServiceTransport != null)
            {
                context.fileServiceTransport.Write(timeoutForFsTransport, 0, pdu);
            }
            else
            {
                throw new InvalidOperationException("No connection was established.");
            }
        }


        /// <summary>
        /// Expect to receive a PDU of any type from the remote host.
        /// </summary>
        /// <param name="timeout">Timeout of receiving PDU.</param>
        /// <returns>The expected PDU.</returns>
        /// <exception cref="TimeoutException">
        /// Thrown when specified timeout period has expired.
        /// </exception>
        public RpcePdu ExpectPdu(TimeSpan timeout)
        {
            RpcePdu pdu;

            if (context.tcpTransport != null)
            {
                TransportEvent transportEvent = context.tcpTransport.ExpectTransportEvent(timeout);

                if (transportEvent.EventType == EventType.ReceivedPacket)
                {
                    pdu = (RpcePdu)transportEvent.EventObject;
                }
                else if (transportEvent.EventType == EventType.Exception)
                {
                    throw new InvalidOperationException("Exception occurred, please find detailed information in inner exception.",
                        transportEvent.EventObject as Exception);
                }
                else if (transportEvent.EventType == EventType.Disconnected)
                {
                    throw new RpceDisconnectedException("The connection is disconnected.");
                }
                else
                {
                    throw new InvalidOperationException(
                        string.Format("Received an un-expected event, the event type is: {0}.",
                                      transportEvent.EventType));
                }
            }
            else if (context.fileServiceTransport != null)
            {
                if (pipeTransceiveResponseQueue.Count > 0)
                {
                    byte[] buf = pipeTransceiveResponseQueue.Dequeue();

                    int consumedLength;
                    int expectedLength;
                    StackPacket[] stackPackets = RpceDecodePduCallback(
                        null,
                        buf,
                        out consumedLength,
                        out expectedLength);
                    if (consumedLength != buf.Length 
                        || expectedLength != 0
                        || stackPackets == null 
                        || stackPackets.Length != 1)
                    {
                        //Unlikely to happen
                        throw new InvalidOperationException("RPCE decode error.");
                    }
                    pdu = (RpcePdu)stackPackets[0];
                }
                else
                {
                    pdu = SmbRead(timeout);
                }
            }
            else
            {
                throw new InvalidOperationException("No connection was established.");
            }

            UpdateContextOnReceivingPdu(pdu);
            return pdu;
        }


        /// <summary>
        /// Issue SMB/SMB2 READ to get response.
        /// </summary>
        /// <param name="timeout">The waiting time</param>
        /// <returns>A RPCE PDU.</returns>
        private RpcePdu SmbRead(TimeSpan timeout)
        {
            byte[] buffer = new byte[0];
            // first attempt to read 1024 bytes.
            int expectedLength = 1024;
            // first retry interval: 1 sec. double it every retry until timeout.
            TimeSpan retryInterval = new TimeSpan(0, 0, 1);

            while (true)
            {
                DateTime t = DateTime.Now;

                byte[] readBuffer;
                NtStatus status = (NtStatus)context.fileServiceTransport.Read(
                    timeout,
                    0,
                    (uint)(expectedLength - buffer.Length),
                    out readBuffer);

                timeout -= (DateTime.Now - t);

                if (status == NtStatus.STATUS_PIPE_EMPTY || status == NtStatus.STATUS_PIPE_BUSY)
                {
                    // Just retry
                    timeout -= retryInterval;
                    if (timeout < TimeSpan.Zero)
                    {
                        throw new TimeoutException();
                    }
                    Thread.Sleep(retryInterval);

                    retryInterval += retryInterval;
                    continue;
                }

                if (status != NtStatus.STATUS_SUCCESS && status != NtStatus.STATUS_BUFFER_OVERFLOW)
                {
                    throw new InvalidOperationException(string.Format("SMB Read error {0}", status));
                }

                buffer = ArrayUtility.ConcatenateArrays(buffer, readBuffer);
                int consumedLength;

                StackPacket[] stackPackets = RpceDecodePduCallback(
                    null,
                    buffer,
                    out consumedLength,
                    out expectedLength);

                if (consumedLength > 0)
                {
                    if (consumedLength != buffer.Length)
                    {
                        // Unlikely to happen.
                        throw new InvalidOperationException("Find many PDUs in a single SMB packet.");
                    }
                    if (stackPackets == null || stackPackets.Length != 1)
                    {
                        // Unlikely to happen.
                        throw new InvalidOperationException("Decode error: stackPackets == null || stackPackets.Length != 1");
                    }

                    return (RpcePdu)stackPackets[0];
                }
                if (expectedLength <= buffer.Length)
                {
                    // Unlikely to happen.
                    throw new InvalidOperationException("Decode error: expectedLength <= buffer.Length.");
                }
            }
        }

        #endregion


        /// <summary>
        /// Callback function for decoding PDU.
        /// </summary>
        /// <param name="endPoint">remote endpoint</param>
        /// <param name="messageBytes">bytes received</param>
        /// <param name="consumedLength">num of bytes consumed in processing</param>
        /// <param name="expectedLength">num of bytes expected if the bytes is not enough</param>
        /// <returns>pdus</returns>
        private StackPacket[] RpceDecodePduCallback(
            object endPoint,
            byte[] messageBytes,
            out int consumedLength,
            out int expectedLength)
        {
            RpceCoPdu[] pduList = RpceUtility.DecodeCoPdu(
                context,
                messageBytes,
                out consumedLength,
                out expectedLength);

            for (int i = 0; i < pduList.Length; i++)
            {
                if (!pduList[i].ValidateAuthenticationToken())
                {
                    throw new InvalidOperationException("RPCE signature is invalid.");
                }
            }

            return pduList;
        }


        /// <summary>
        /// update context on sending PDU
        /// </summary>
        /// <param name="pdu">PDU</param>
        private void UpdateContextOnSendingPdu(RpcePdu pdu)
        {
            RpceCoPdu coPdu = pdu as RpceCoPdu;
            if (coPdu == null)
            {
                return;
            }

            context.RpcVersionMajor = coPdu.rpc_vers;
            context.RpcVersionMinor = coPdu.rpc_vers_minor;
            if (coPdu.PTYPE == RpcePacketType.Bind ||
                coPdu.PTYPE == RpcePacketType.BindAck ||
                coPdu.PTYPE == RpcePacketType.AlterContext ||
                coPdu.PTYPE == RpcePacketType.AlterContextResp)
            {
                context.SupportsHeaderSign
                    = (coPdu.pfc_flags & RpceCoPfcFlags.PFC_SUPPORT_HEADER_SIGN)
                            == RpceCoPfcFlags.PFC_SUPPORT_HEADER_SIGN;
            }
            context.SupportsConcurrentMultiplexing
                = (coPdu.pfc_flags & RpceCoPfcFlags.PFC_CONC_MPX)
                        == RpceCoPfcFlags.PFC_CONC_MPX;
            context.PackedDataRepresentationFormat = coPdu.packed_drep.dataRepFormat;
            context.OutstandingCalls.Add(coPdu.call_id);

            switch (coPdu.PTYPE)
            {
                case RpcePacketType.Bind:
                    RpceCoBindPdu bindPdu = coPdu as RpceCoBindPdu;
                    if (bindPdu != null)
                    {
                        UpdateContextOnSendingBindPdu(bindPdu);
                    }
                    break;

                case RpcePacketType.AlterContext:
                    break;

                case RpcePacketType.Auth3:
                    break;

                case RpcePacketType.Request:
                    RpceCoRequestPdu requestPdu = coPdu as RpceCoRequestPdu;
                    if (requestPdu != null)
                    {
                        context.ContextIdentifier = requestPdu.p_cont_id;
                    }
                    break;

                case RpcePacketType.Response:
                case RpcePacketType.CoCancel:
                case RpcePacketType.Orphaned:
                default:
                    //default situation should do nothing.
                    //This is just update the context, if we cannot recognize the PDU, ignore it.
                    break;
            }

        }


        /// <summary>
        /// update context on sending RPCE CO Bind PDU
        /// </summary>
        /// <param name="bindPdu">Bind PDU</param>
        private void UpdateContextOnSendingBindPdu(RpceCoBindPdu bindPdu)
        {
            context.MaxTransmitFragmentSize = bindPdu.max_xmit_frag;
            context.MaxReceiveFragmentSize = bindPdu.max_recv_frag;
            context.AssociateGroupId = bindPdu.assoc_group_id;
            if (bindPdu.p_context_elem.p_cont_elem != null &&
                bindPdu.p_context_elem.p_cont_elem.Length > 0)
            {
                context.InterfaceId
                    = bindPdu.p_context_elem.p_cont_elem[0].abstract_syntax.if_uuid;
                context.InterfaceMajorVersion
                    = bindPdu.p_context_elem.p_cont_elem[0].abstract_syntax.if_vers_major;
                context.InterfaceMinorVersion
                    = bindPdu.p_context_elem.p_cont_elem[0].abstract_syntax.if_vers_minor;

                context.NdrVersion = RpceNdrVersion.None;
                context.PresentationContextsTable.Clear();
                for (int i = 0; i < bindPdu.p_context_elem.p_cont_elem.Length; i++)
                {
                    if (bindPdu.p_context_elem.p_cont_elem[i].transfer_syntaxes == null)
                    {
                        continue;
                    }
                    for (int j = 0;
                        j < bindPdu.p_context_elem.p_cont_elem[i].transfer_syntaxes.Length;
                        j++)
                    {
                        if (bindPdu.p_context_elem.p_cont_elem[i].transfer_syntaxes[j].if_uuid
                                == RpceUtility.NDR_INTERFACE_UUID
                            &&
                            bindPdu.p_context_elem.p_cont_elem[i].transfer_syntaxes[j].if_vers_major
                                == RpceUtility.NDR_INTERFACE_MAJOR_VERSION
                            &&
                            bindPdu.p_context_elem.p_cont_elem[i].transfer_syntaxes[j].if_vers_minor
                                == RpceUtility.NDR_INTERFACE_MINOR_VERSION)
                        {
                            context.NdrVersion |= RpceNdrVersion.NDR;
                            context.PresentationContextsTable.Add(
                                bindPdu.p_context_elem.p_cont_elem[i].p_cont_id,
                                RpceNdrVersion.NDR);
                        }
                        else if (bindPdu.p_context_elem.p_cont_elem[i].transfer_syntaxes[j].if_uuid
                                == RpceUtility.NDR64_INTERFACE_UUID
                            &&
                            bindPdu.p_context_elem.p_cont_elem[i].transfer_syntaxes[j].if_vers_major
                                == RpceUtility.NDR64_INTERFACE_MAJOR_VERSION
                            &&
                            bindPdu.p_context_elem.p_cont_elem[i].transfer_syntaxes[j].if_vers_minor
                                == RpceUtility.NDR64_INTERFACE_MINOR_VERSION)
                        {
                            context.NdrVersion |= RpceNdrVersion.NDR64;
                            context.PresentationContextsTable.Add(
                                bindPdu.p_context_elem.p_cont_elem[i].p_cont_id,
                                RpceNdrVersion.NDR64);
                        }
                        else
                        {
                            byte[] uuid 
                                = bindPdu.p_context_elem.p_cont_elem[i].transfer_syntaxes[j].if_uuid.ToByteArray();
                            if (ArrayUtility.CompareArrays(
                                    ArrayUtility.SubArray(
                                        uuid, 
                                        0, 
                                        RpceUtility.BIND_TIME_FEATURE_NEGOTIATION_BITMASK_PREFIX_LENGTH),
                                    ArrayUtility.SubArray(
                                        RpceUtility.BIND_TIME_FEATURE_NEGOTIATION_BITMASK_GUID_BYTES, 
                                        0, 
                                        RpceUtility.BIND_TIME_FEATURE_NEGOTIATION_BITMASK_PREFIX_LENGTH))
                                &&
                                bindPdu.p_context_elem.p_cont_elem[i].transfer_syntaxes[j].if_vers_major == 1
                                &&
                                bindPdu.p_context_elem.p_cont_elem[i].transfer_syntaxes[j].if_vers_minor == 0)
                            {
                                context.BindTimeFeatureNegotiationBitmask = (RpceBindTimeFeatureNegotiationBitmask)
                                    uuid[RpceUtility.BIND_TIME_FEATURE_NEGOTIATION_BITMASK_PREFIX_LENGTH];
                            }
                        }

                    }
                }
            }
        }


        /// <summary>
        /// update context on receiving PDU
        /// </summary>
        /// <param name="pdu">PDU</param>
        private void UpdateContextOnReceivingPdu(RpcePdu pdu)
        {
            RpceCoPdu coPdu = pdu as RpceCoPdu;
            if (coPdu == null)
            {
                return;
            }

            context.RpcVersionMajor = coPdu.rpc_vers;
            context.RpcVersionMinor = coPdu.rpc_vers_minor;
            if (coPdu.PTYPE == RpcePacketType.Bind ||
                coPdu.PTYPE == RpcePacketType.BindAck)
            {
                context.SupportsHeaderSign
                    = (coPdu.pfc_flags & RpceCoPfcFlags.PFC_SUPPORT_HEADER_SIGN)
                            == RpceCoPfcFlags.PFC_SUPPORT_HEADER_SIGN;
                context.SupportsConcurrentMultiplexing
                    = (coPdu.pfc_flags & RpceCoPfcFlags.PFC_CONC_MPX)
                            == RpceCoPfcFlags.PFC_CONC_MPX;
            }

            context.PackedDataRepresentationFormat = coPdu.packed_drep.dataRepFormat;
            context.OutstandingCalls.Remove(coPdu.call_id);

            switch (coPdu.PTYPE)
            {
                case RpcePacketType.BindAck:
                    RpceCoBindAckPdu bindAckPdu = coPdu as RpceCoBindAckPdu;
                    if (bindAckPdu != null)
                    {
                        UpdateContextOnReceivingBindAckPdu(bindAckPdu);
                    }
                    break;

                case RpcePacketType.Response:
                    RpceCoResponsePdu responsePdu = coPdu as RpceCoResponsePdu;
                    if (responsePdu != null)
                    {
                        context.ContextIdentifier = responsePdu.p_cont_id;
                    }
                    break;

                case RpcePacketType.BindNak:
                case RpcePacketType.AlterContextResp:
                case RpcePacketType.Request:
                case RpcePacketType.Fault:
                case RpcePacketType.Shutdown:
                default:
                    //default situation should do nothing.
                    //This is just update the context, if we cannot recognize the PDU, ignore it.
                    break;
            }

            context.SecurityContextNeedContinueProcessing = coPdu.securityContextNeedContinueProcessing;
        }


        /// <summary>
        /// update context on receiving RPCE CO BindAck PDU
        /// </summary>
        /// <param name="bindAckPdu">BindAck PDU</param>
        private void UpdateContextOnReceivingBindAckPdu(RpceCoBindAckPdu bindAckPdu)
        {
            context.MaxTransmitFragmentSize = bindAckPdu.max_xmit_frag;
            context.MaxReceiveFragmentSize = bindAckPdu.max_recv_frag;
            context.AssociateGroupId = bindAckPdu.assoc_group_id;
            if (bindAckPdu.sec_addr.port_spec != null)
            {
                context.SecondaryAddress
                    = Encoding.ASCII.GetString(bindAckPdu.sec_addr.port_spec);
            }

            if (bindAckPdu.p_result_list.p_results != null)
            {
                context.NdrVersion = RpceNdrVersion.None;
                for (int i = 0; i < bindAckPdu.p_result_list.p_results.Length; i++)
                {
                    if (bindAckPdu.p_result_list.p_results[i].result
                        == p_cont_def_result_t.acceptance)
                    {
                        if (bindAckPdu.p_result_list.p_results[i].transfer_syntax.if_uuid
                                == RpceUtility.NDR_INTERFACE_UUID
                            &&
                            bindAckPdu.p_result_list.p_results[i].transfer_syntax.if_vers_major
                                == RpceUtility.NDR_INTERFACE_MAJOR_VERSION
                            &&
                            bindAckPdu.p_result_list.p_results[i].transfer_syntax.if_vers_minor
                                == RpceUtility.NDR_INTERFACE_MINOR_VERSION)
                        {
                            context.NdrVersion |= RpceNdrVersion.NDR;
                        }
                        else if (bindAckPdu.p_result_list.p_results[i].transfer_syntax.if_uuid
                                == RpceUtility.NDR64_INTERFACE_UUID
                            &&
                            bindAckPdu.p_result_list.p_results[i].transfer_syntax.if_vers_major
                                == RpceUtility.NDR64_INTERFACE_MAJOR_VERSION
                            &&
                            bindAckPdu.p_result_list.p_results[i].transfer_syntax.if_vers_minor
                                == RpceUtility.NDR64_INTERFACE_MINOR_VERSION)
                        {
                            context.NdrVersion |= RpceNdrVersion.NDR64;
                        }
                    }
                    else if (bindAckPdu.p_result_list.p_results[i].result
                        == p_cont_def_result_t.negotiate_ack)
                    {
                        byte[] bitmask = BitConverter.GetBytes(
                            (ushort)bindAckPdu.p_result_list.p_results[i].reason);
                        context.BindTimeFeatureNegotiationBitmask
                            = (RpceBindTimeFeatureNegotiationBitmask)bitmask[0];
                    }
                }
                foreach (KeyValuePair<ushort, RpceNdrVersion> item in
                    context.PresentationContextsTable)
                {
                    if ((context.NdrVersion & item.Value) == item.Value)
                    {
                        context.ContextIdentifier = item.Key;
                        break;
                    }
                }
            }
        }


        /// <summary>
        /// compute next call id.
        /// </summary>
        /// <returns>next available call id.</returns>
        public uint ComputeNextCallId()
        {
            return ++context.CurrentCallId;
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
                isDisposed = true;
                // Release managed resources.
                if (context.tcpTransport != null)
                {
                    context.tcpTransport.Dispose();
                    context.tcpTransport = null;
                }
                if (context.fileServiceTransport != null)
                {
                    context.fileServiceTransport.Dispose();
                    context.fileServiceTransport = null;
                }
            }

            // Release unmanaged resources.
        }


        /// <summary>
        /// finalizer
        /// </summary>
        ~RpceClient()
        {
            Dispose(false);
        }

        #endregion
    }
}
