// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Net;

using Microsoft.Protocols.TestTools.StackSdk.FileAccessService;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb;
using Microsoft.Protocols.TestTools.StackSdk.Security.Sspi;
using Microsoft.Protocols.TestTools.StackSdk.Transport;

namespace Microsoft.Protocols.TestTools.StackSdk.Networking.Rpce
{
    internal delegate void SmbSendFunc();


    /// <summary>
    /// Describes the session of RPCE connection.
    /// </summary>
    public class RpceServerSessionContext : RpceContext
    {
        #region Field members

        // RPC network address and endPort of client
        private object remoteEndpoint;

        //RPCE server context.
        private RpceServerContext serverContext;

        //Call id list for the connection.
        private List<uint> outstandingCalls;

        private IntPtr handle;

        //Associate Group ID list 
        private List<uint> associateGroupIdList;

        //SMB/SMB2 connection and open
        internal IFileServiceServerConnection fsConnection;
        internal IFileServiceServerOpen fsOpen;

        //Buffer for multiple SMB READ/WRITE requests/responses.
        internal byte[] smbBufferReceiving = new byte[0];
        internal byte[] smbBufferSending = new byte[0];
        internal object smbSendingLocker = new object();
        internal int smbMaxSendingLength;
        internal SmbSendFunc smbSendingFunc;


        #endregion


        #region ctor

        /// <summary>
        /// Initialize Rpce server context
        /// </summary>
        /// <param name="serverContext">The rpce server context.</param>
        /// <param name="remoteEndpoint">Remote endpoint.</param>
        internal RpceServerSessionContext(
            RpceServerContext serverContext, 
            object remoteEndpoint)
            : base()
        {
            this.serverContext = serverContext;

            this.ProtocolSequence = serverContext.ProtocolSequence;
            this.RpcVersionMajor = serverContext.RpcVersionMajor;
            this.RpcVersionMinor = serverContext.RpcVersionMinor;

            // Make handle always different.
            handle = new IntPtr(Environment.TickCount);

            this.remoteEndpoint = remoteEndpoint;
            this.outstandingCalls = new List<uint>();
            this.associateGroupIdList = new List<uint>();
        }

        #endregion


        #region properties

        /// <summary>
        /// Call ID collection for the connection.
        /// </summary>
        public ReadOnlyCollection<uint> OutstandingCalls
        {
            get
            {
                return new ReadOnlyCollection<uint>(this.outstandingCalls);
            }
        }


        /// <summary>
        /// Remote Endpoint.
        /// </summary>
        public object RemoteEndpoint
        {
            get
            {
                return this.remoteEndpoint;
            }
        }


        /// <summary>
        /// The RPCE server context.
        /// </summary>
        public RpceServerContext ServerContext
        {
            get
            {
                return this.serverContext;
            }
        }


        /// <summary>
        /// The RPCE server context
        /// </summary>
        public IntPtr Handle
        {
            get
            {
                return handle;
            }
        }


        /// <summary>
        /// Security provider (SSPI).
        /// </summary>
        public new SecurityContext SecurityContext
        {
            get
            {
                return base.SecurityContext;
            }
            set
            {
                base.SecurityContext = value;
            }
        }


        /// <summary>
        /// Context of underlayer named pipe transport (SMB/SMB2).
        /// User should cast return value to SmbServerOpen or Smb2ServerOpen.
        /// If RPCE is using TCP as its transport, this property returns null.
        /// If underlayer named pipe transport is closed, this property returns null.
        /// </summary>
        public IFileServiceServerOpen FileServiceTransportOpen
        {
            get
            {
                return fsOpen;
            }
        }

        #endregion


        #region Update SessionContext

        /// <summary>
        /// update context on sending PDU.
        /// </summary>
        /// <param name="pdu">PDU to send.</param>
        internal void UpdateContextOnSendingPdu(RpcePdu pdu)
        {
            RpceCoPdu coPdu = pdu as RpceCoPdu;
            if (coPdu == null)
            {
                return;
            }

            this.RpcVersionMajor = coPdu.rpc_vers;
            this.RpcVersionMinor = coPdu.rpc_vers_minor;
            if (coPdu.PTYPE == RpcePacketType.Bind ||
                coPdu.PTYPE == RpcePacketType.BindAck ||
                coPdu.PTYPE == RpcePacketType.AlterContext ||
                coPdu.PTYPE == RpcePacketType.AlterContextResp)
            {
                this.SupportsHeaderSign
                    = (coPdu.pfc_flags & RpceCoPfcFlags.PFC_SUPPORT_HEADER_SIGN)
                            == RpceCoPfcFlags.PFC_SUPPORT_HEADER_SIGN;
            }

            this.SupportsConcurrentMultiplexing
                = (coPdu.pfc_flags & RpceCoPfcFlags.PFC_CONC_MPX)
                        == RpceCoPfcFlags.PFC_CONC_MPX;
            this.PackedDataRepresentationFormat = coPdu.packed_drep.dataRepFormat;

            if (coPdu.PTYPE != RpcePacketType.Request)
            {
                this.outstandingCalls.Remove(coPdu.call_id);
            }

            switch (coPdu.PTYPE)
            {
                case RpcePacketType.BindAck:
                    RpceCoBindAckPdu bindAckPdu = coPdu as RpceCoBindAckPdu;
                    if (bindAckPdu != null)
                    {
                        UpdateContextOnSendingBindAckPdu(bindAckPdu);
                    }
                    break;

                case RpcePacketType.Request:
                    RpceCoRequestPdu requestPdu = coPdu as RpceCoRequestPdu;
                    if (requestPdu != null)
                    {
                        this.ContextIdentifier = requestPdu.p_cont_id;
                    }
                    break;

                case RpcePacketType.Response:
                    RpceCoResponsePdu responsePdu = coPdu as RpceCoResponsePdu;
                    if (responsePdu != null)
                    {
                        this.ContextIdentifier = responsePdu.p_cont_id;
                    }
                    break;

                case RpcePacketType.BindNak:
                case RpcePacketType.AlterContextResp:
                case RpcePacketType.Fault:
                case RpcePacketType.Shutdown:
                default:
                    //default situation should do nothing.
                    //This is just update the context, if we cannot recognize the PDU, ignore it.
                    break;
            }
        }


        /// <summary>
        /// update context on sending RPCE CO BindAck PDU.
        /// </summary>
        /// <param name="bindAckPdu">BindAck PDU to send.</param>
        internal void UpdateContextOnSendingBindAckPdu(RpceCoBindAckPdu bindAckPdu)
        {
            this.MaxTransmitFragmentSize = bindAckPdu.max_xmit_frag;
            this.MaxReceiveFragmentSize = bindAckPdu.max_recv_frag;
            this.AssociateGroupId = bindAckPdu.assoc_group_id;
            if (bindAckPdu.sec_addr.port_spec != null)
            {
                this.SecondaryAddress
                    = Encoding.ASCII.GetString(bindAckPdu.sec_addr.port_spec);
            }

            if (bindAckPdu.p_result_list.p_results != null)
            {
                this.NdrVersion = RpceNdrVersion.None;
                for (int i = 0; i < bindAckPdu.p_result_list.p_results.Length; i++)
                {
                    p_result_t p_result = bindAckPdu.p_result_list.p_results[i];

                    if (p_result.result == p_cont_def_result_t.acceptance)
                    {
                        if (p_result.transfer_syntax.if_uuid == RpceUtility.NDR_INTERFACE_UUID
                            &&
                            p_result.transfer_syntax.if_vers_major == RpceUtility.NDR_INTERFACE_MAJOR_VERSION
                            &&
                            p_result.transfer_syntax.if_vers_minor == RpceUtility.NDR_INTERFACE_MINOR_VERSION)
                        {
                            this.NdrVersion |= RpceNdrVersion.NDR;
                        }
                        else if (p_result.transfer_syntax.if_uuid == RpceUtility.NDR64_INTERFACE_UUID
                            &&
                            p_result.transfer_syntax.if_vers_major == RpceUtility.NDR64_INTERFACE_MAJOR_VERSION
                            &&
                            p_result.transfer_syntax.if_vers_minor == RpceUtility.NDR64_INTERFACE_MINOR_VERSION)
                        {
                            this.NdrVersion |= RpceNdrVersion.NDR64;
                        }
                    }
                    else if (p_result.result == p_cont_def_result_t.negotiate_ack)
                    {
                        byte[] bitmask = BitConverter.GetBytes((ushort)p_result.reason);
                        this.BindTimeFeatureNegotiationBitmask = (RpceBindTimeFeatureNegotiationBitmask)bitmask[0];
                        if ((this.BindTimeFeatureNegotiationBitmask
                            & RpceBindTimeFeatureNegotiationBitmask.KeepConnectionOnOrphanSupported_Resp)
                            != 0)
                        {
                            this.BindTimeFeatureNegotiationBitmask 
                                |= RpceBindTimeFeatureNegotiationBitmask.KeepConnectionOnOrphanSupported;
                        }
                    }
                }

                foreach (KeyValuePair<ushort, RpceNdrVersion> item in this.PresentationContextsTable)
                {
                    if ((this.NdrVersion & item.Value) == item.Value)
                    {
                        this.ContextIdentifier = item.Key;
                        break;
                    }
                }
            }
        }


        /// <summary>
        /// update context on receiving PDU.
        /// </summary>
        /// <param name="pdu">PDU to receive.</param>
        /// <exception cref="ArgumentNullException">Thrown when pdu is null.</exception>
        internal void UpdateContextOnReceivingPdu(RpcePdu pdu)
        {
            RpceCoPdu coPdu = pdu as RpceCoPdu;
            if (coPdu == null)
            {
                return;
            }

            this.RpcVersionMajor = coPdu.rpc_vers;
            this.RpcVersionMinor = coPdu.rpc_vers_minor;
            if (coPdu.PTYPE == RpcePacketType.Bind ||
                coPdu.PTYPE == RpcePacketType.BindAck)
            {
                //TDI: 
                //TD said PFC_SUPPORT_HEADER_SIGN is meanful in Bind and AltContext.
                //But when using Kerberos, AltContext doesn't have this flag, 
                //if server or client read this flag according to AltContext, Sign/Encrypt will fail.
                this.SupportsHeaderSign
                    = (coPdu.pfc_flags & RpceCoPfcFlags.PFC_SUPPORT_HEADER_SIGN)
                            == RpceCoPfcFlags.PFC_SUPPORT_HEADER_SIGN;
                this.SupportsConcurrentMultiplexing
                    = (coPdu.pfc_flags & RpceCoPfcFlags.PFC_CONC_MPX)
                            == RpceCoPfcFlags.PFC_CONC_MPX;
            }

            this.PackedDataRepresentationFormat = coPdu.packed_drep.dataRepFormat;
            this.CurrentCallId = coPdu.call_id;
            if (coPdu.PTYPE != RpcePacketType.Response && coPdu.PTYPE != RpcePacketType.Auth3)
            {
                this.outstandingCalls.Add(coPdu.call_id);
            }

            switch (coPdu.PTYPE)
            {
                case RpcePacketType.Bind:
                    RpceCoBindPdu bindPdu = coPdu as RpceCoBindPdu;
                    if (bindPdu != null)
                    {
                        UpdateContextOnReceivingBindPdu(bindPdu);
                    }
                    break;

                case RpcePacketType.Request:
                    RpceCoRequestPdu requestPdu = coPdu as RpceCoRequestPdu;
                    if (requestPdu != null)
                    {
                        this.ContextIdentifier = requestPdu.p_cont_id;
                    }
                    break;

                case RpcePacketType.Response:
                    RpceCoResponsePdu responsePdu = coPdu as RpceCoResponsePdu;
                    if (responsePdu != null)
                    {
                        this.ContextIdentifier = responsePdu.p_cont_id;
                    }
                    break;

                case RpcePacketType.Auth3:
                case RpcePacketType.AlterContext:
                case RpcePacketType.CoCancel:
                case RpcePacketType.Orphaned:
                default:
                    //default situation should do nothing.
                    //This is just update the context, if we cannot recognize the PDU, ignore it.
                    break;
            }

            this.SecurityContextNeedContinueProcessing = coPdu.securityContextNeedContinueProcessing;
        }


        /// <summary>
        /// update context on receiving RPCE CO Bind PDU.
        /// </summary>
        /// <param name="bindPdu">Bind PDU to receive.</param>
        internal void UpdateContextOnReceivingBindPdu(RpceCoBindPdu bindPdu)
        {
            this.MaxTransmitFragmentSize = bindPdu.max_xmit_frag;
            this.MaxReceiveFragmentSize = bindPdu.max_recv_frag;

            if (bindPdu.assoc_group_id != 0)
            {
                this.AssociateGroupId = bindPdu.assoc_group_id;
            }
            else
            {
                if (this.associateGroupIdList.Count == 0)
                {
                    this.AssociateGroupId = bindPdu.assoc_group_id + 1;
                }
                else
                {
                    this.associateGroupIdList.Sort();
                    this.AssociateGroupId = this.associateGroupIdList[this.associateGroupIdList.Count - 1] + 1;
                }
            }

            associateGroupIdList.Add(this.AssociateGroupId);

            if (bindPdu.p_context_elem.p_cont_elem != null &&
                bindPdu.p_context_elem.p_cont_elem.Length > 0)
            {
                this.InterfaceId
                    = bindPdu.p_context_elem.p_cont_elem[0].abstract_syntax.if_uuid;
                this.InterfaceMajorVersion
                    = bindPdu.p_context_elem.p_cont_elem[0].abstract_syntax.if_vers_major;
                this.InterfaceMinorVersion
                    = bindPdu.p_context_elem.p_cont_elem[0].abstract_syntax.if_vers_minor;

                this.NdrVersion = RpceNdrVersion.None;
                this.PresentationContextsTable.Clear();
                for (int i = 0; i < bindPdu.p_context_elem.p_cont_elem.Length; i++)
                {
                    p_cont_elem_t p_cont_elem = bindPdu.p_context_elem.p_cont_elem[i];

                    if (p_cont_elem.transfer_syntaxes == null)
                    {
                        continue;
                    }
                    for (int j = 0; j < p_cont_elem.transfer_syntaxes.Length; j++)
                    {
                        p_syntax_id_t transfer_syntax = p_cont_elem.transfer_syntaxes[j];

                        if (transfer_syntax.if_uuid == RpceUtility.NDR_INTERFACE_UUID
                            &&
                            transfer_syntax.if_vers_major == RpceUtility.NDR_INTERFACE_MAJOR_VERSION
                            &&
                            transfer_syntax.if_vers_minor == RpceUtility.NDR_INTERFACE_MINOR_VERSION)
                        {
                            this.NdrVersion |= RpceNdrVersion.NDR;
                            this.PresentationContextsTable.Add(p_cont_elem.p_cont_id, RpceNdrVersion.NDR);
                        }
                        else if (transfer_syntax.if_uuid == RpceUtility.NDR64_INTERFACE_UUID
                            &&
                            transfer_syntax.if_vers_major == RpceUtility.NDR64_INTERFACE_MAJOR_VERSION
                            &&
                            transfer_syntax.if_vers_minor == RpceUtility.NDR64_INTERFACE_MINOR_VERSION)
                        {
                            this.NdrVersion |= RpceNdrVersion.NDR64;
                            this.PresentationContextsTable.Add(p_cont_elem.p_cont_id, RpceNdrVersion.NDR64);
                        }
                        else
                        {
                            byte[] uuid = transfer_syntax.if_uuid.ToByteArray();
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
                                transfer_syntax.if_vers_major == 1
                                &&
                                transfer_syntax.if_vers_minor == 0)
                            {
                                this.BindTimeFeatureNegotiationBitmask = (RpceBindTimeFeatureNegotiationBitmask)
                                    uuid[RpceUtility.BIND_TIME_FEATURE_NEGOTIATION_BITMASK_PREFIX_LENGTH];
                            }
                        }
                    }
                }
            }
        }

        #endregion
    }
}
