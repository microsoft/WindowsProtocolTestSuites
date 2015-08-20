// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Net;
using System.Diagnostics.CodeAnalysis;

using Microsoft.Protocols.TestTools.StackSdk.Messages;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Cifs;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb
{
    /// <summary>
    /// the decode packet class. used to decode the packet from the recieve thread. 
    /// </summary>
    [SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling")]
    internal class SmbClientDecodePacket : CifsClientDecodePacket
    {
        /// <summary>
        /// the smb client 
        /// </summary>
        private SmbClient smbClient;

        /// <summary>
        /// default constructor. 
        /// </summary>
        /// <param name = "smbClient">
        /// the smbclient contains context for decoder. to update the states of sdk. 
        /// </param>
        public SmbClientDecodePacket(SmbClient smbClient)
            : base(smbClient.Context, new CifsClientConfig())
        {
            this.smbClient = smbClient;
        }


        /// <summary>
        /// to decode stack packet from the received message bytes. 
        /// </summary>
        /// <param name = "endPointIdentity">the endPointIdentity from which the message bytes are received. </param>
        /// <param name = "messageBytes">the received message bytes to be decoded. </param>
        /// <param name = "consumedLength">the length of message bytes consumed by decoder. </param>
        /// <param name = "expectedLength">the length of message bytes the decoder expects to receive. </param>
        /// <returns>the stack packets decoded from the received message bytes. </returns>
        internal new SmbPacket[] DecodePacket(
            object endPointIdentity, byte[] messageBytes, out int consumedLength, out int expectedLength)
        {
            // initialize the default values
            expectedLength = 0;
            consumedLength = 0;
            SmbPacket[] packets = null;

            switch (this.smbClient.Capability.TransportType)
            {
                case TransportType.TCP:
                    // direct tcp transport management instance
                    SmbDirectTcpPacket directTcp = new SmbDirectTcpPacket(null);

                    // get the endpoint for direct tcp transport
                    int endpoint = smbClient.Context.GetConnectionID(endPointIdentity as IPEndPoint);

                    // message bytes is invalid
                    if (directTcp.IsPacketInvalid(messageBytes))
                    {
                        return null;
                    }

                    // if the packet has continue packet, do not parse it.
                    if (directTcp.HasContinuousPacket(messageBytes))
                    {
                        return null;
                    }

                    // decode the data of packet
                    byte[] packetBytes = directTcp.GetTcpData(messageBytes);
                    packets = DecodePacketFromBytesWithoutTransport(endpoint, packetBytes, out consumedLength, out expectedLength);

                    // decode the header of packet
                    if (packets != null && packets.Length > 0)
                    {
                        packets[0].TransportHeader = CifsMessageUtils.ToStuct<TransportHeader>(messageBytes);
                    }

                    // to consume the additional data
                    consumedLength = Math.Max(consumedLength, directTcp.GetTcpPacketLength(messageBytes));

                    // update the consumed length with transport
                    consumedLength += directTcp.TcpTransportHeaderSize;

                    break;

                case TransportType.NetBIOS:
                    // decode packet
                    packets = DecodePacketFromBytesWithoutTransport(
                        Convert.ToInt32(endPointIdentity), messageBytes, out consumedLength, out expectedLength);

                    break;

                default:
                    break;
            }

            return packets;
        }


        /// <summary>
        /// to decode stack packet from the received message bytes.
        /// the message bytes contains data without transport information
        /// </summary>
        /// <param name = "endpoint">the endpoint from which the message bytes are received. </param>
        /// <param name = "packetBytesWithoutTransport">the received message bytes to be decoded. </param>
        /// <param name = "consumedLength">the length of message bytes consumed by decoder. </param>
        /// <param name = "expectedLength">the length of message bytes the decoder expects to receive. </param>
        /// <returns>the stack packets decoded from the received message bytes. </returns>
        private SmbPacket[] DecodePacketFromBytesWithoutTransport(
            int endpoint, byte[] packetBytesWithoutTransport, out int consumedLength, out int expectedLength)
        {
            expectedLength = 0;
            consumedLength = 0;
            SmbPacket[] packets = new SmbPacket[0];

            int packetConsumedLength = 0;
            int packetExpectedLength = 0;

            // decode packets using cifs decorder.
            SmbPacket response = this.DecodeSmbResponseFromBytes(
                (int)endpoint, packetBytesWithoutTransport, out packetConsumedLength);

            // Use the decoded packet to UpdateRoleContext if it is not null and ContextUpdate is enabled:
            if (response != null)
            {
                if (this.IsContextUpdateEnabled)
                {
                    this.clientContext.UpdateRoleContext((int)endpoint, response);
                }
                packets = new SmbPacket[] { response };
            }

            // update the length after decode packet.
            consumedLength += packetConsumedLength;
            expectedLength += packetExpectedLength;

            return packets;
        }


        /// <summary>
        /// decode the batched request packet
        /// </summary>
        /// <param name="channel">the channel of bytes to read</param>
        /// <param name="request">the request of the response.</param>
        /// <param name="smbBatchedResponse">the batched response</param>
        /// <returns>the consumed length of batched response packet</returns>
        protected override int DecodeBatchedRequest(
            Channel channel,
            SmbPacket request, SmbBatchedResponsePacket smbBatchedResponse)
        {
            int result = base.DecodeBatchedRequest(channel, request, smbBatchedResponse);

            for (SmbBatchedResponsePacket currentPacket = smbBatchedResponse;
                currentPacket != null && currentPacket.AndxPacket != null;
                currentPacket = currentPacket.AndxPacket as SmbBatchedResponsePacket)
            {
                SmbPacket andxPacket = currentPacket.AndxPacket;

                // create the smb packet
                object smbParameters = ObjectUtility.GetFieldValue(currentPacket, "smbParameters");
                if (smbParameters != null)
                {
                    SmbHeader smbHeader = smbBatchedResponse.SmbHeader;
                    smbHeader.Command = (SmbCommand)ObjectUtility.GetFieldValue(smbParameters, "AndXCommand");
                    andxPacket = CreateSmbResponsePacket(null, smbHeader, null);
                }

                // convert from cifs packet to smb packet
                if (andxPacket != null)
                {
                    Type smbPacketType = andxPacket.GetType();
                    andxPacket = ObjectUtility.CreateInstance(
                        smbPacketType.Module.FullyQualifiedName,
                        smbPacketType.FullName,
                        new object[] { currentPacket.AndxPacket }) as SmbPacket;
                }

                currentPacket.AndxPacket = andxPacket;
            }

            return result;
        }


        #region Create Special Response Packet

        /// <summary>
        /// to new a Smb response packet in type of the Command in SmbHeader.
        /// </summary>
        /// <param name="request">the request of the response.</param>
        /// <param name="smbHeader">the SMB header of the packet.</param>
        /// <param name="channel">the channel started with SmbParameters.</param>
        /// <returns>
        /// the new response packet. 
        /// the null means that the utility don't know how to create the response.
        /// </returns>
        [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling")]
        protected override SmbPacket CreateSmbResponsePacket(
            SmbPacket request,
            SmbHeader smbHeader,
            Channel channel)
        {
            SmbPacket smbPacket = null;

            // error packet
            SmbStatus packetStatus = (SmbStatus)smbHeader.Status;

            // error packet
            if (packetStatus != SmbStatus.STATUS_SUCCESS &&
                    packetStatus != SmbStatus.STATUS_MORE_PROCESSING_REQUIRED &&
                    packetStatus != SmbStatus.STATUS_BUFFER_OVERFLOW)
            {
                smbPacket = new SmbErrorResponsePacket();
                smbPacket.SmbHeader = smbHeader;

                return smbPacket;
            }

            // success packet
            switch (smbHeader.Command)
            {
                case SmbCommand.SMB_COM_NEGOTIATE:
                    if (smbClient.Capability.IsSupportsExtendedSecurity)
                    {
                        smbPacket = new SmbNegotiateResponsePacket();
                    }
                    else
                    {
                        smbPacket = new SmbNegotiateImplicitNtlmResponsePacket();
                    }
                    break;

                case SmbCommand.SMB_COM_SESSION_SETUP_ANDX:
                    if (smbClient.Capability.IsSupportsExtendedSecurity)
                    {
                        smbPacket = new SmbSessionSetupAndxResponsePacket();
                    }
                    else
                    {
                        smbPacket = new SmbSessionSetupImplicitNtlmAndxResponsePacket();
                    }
                    break;

                case SmbCommand.SMB_COM_TREE_CONNECT_ANDX:
                    smbPacket = new SmbTreeConnectAndxResponsePacket();
                    break;

                case SmbCommand.SMB_COM_TREE_DISCONNECT:
                    smbPacket = new SmbTreeDisconnectResponsePacket();
                    break;

                case SmbCommand.SMB_COM_LOGOFF_ANDX:
                    smbPacket = new SmbLogoffAndxResponsePacket();
                    break;

                case SmbCommand.SMB_COM_NT_CREATE_ANDX:
                    smbPacket = new SmbNtCreateAndxResponsePacket();
                    break;

                case SmbCommand.SMB_COM_CLOSE:
                    smbPacket = new SmbCloseResponsePacket();
                    break;

                case SmbCommand.SMB_COM_OPEN_ANDX:
                    smbPacket = new SmbOpenAndxResponsePacket();
                    break;

                case SmbCommand.SMB_COM_WRITE_ANDX:
                    smbPacket = new SmbWriteAndxResponsePacket();
                    break;

                case SmbCommand.SMB_COM_READ_ANDX:
                    smbPacket = new SmbReadAndxResponsePacket();
                    break;

                case SmbCommand.SMB_COM_TRANSACTION:
                    smbPacket = this.CreateTransactionResponsePacket(request, smbHeader, channel);

                    break;

                case SmbCommand.SMB_COM_TRANSACTION2:
                    smbPacket = this.CreateTransaction2ResponsePacket(request, smbHeader, channel);

                    break;

                case SmbCommand.SMB_COM_NT_TRANSACT:
                    smbPacket = this.CreateNtTransactionResponsePacket(request, smbHeader, channel);

                    break;

                default:
                    break;

            }
            if (smbPacket != null)
            {
                smbPacket.SmbHeader = smbHeader;
                return smbPacket;
            }

            return base.CreateSmbResponsePacket(request, smbHeader, channel);
        }


        /// <summary>
        /// createt the transactions packet
        /// </summary>
        /// <param name="request">the request packet</param>
        /// <param name="smbHeader">the smb header of response packet</param>
        /// <param name="channel">the channel contains the packet bytes</param>
        /// <returns>the response packet</returns>
        private SmbPacket CreateTransactionResponsePacket(SmbPacket request, SmbHeader smbHeader, Channel channel)
        {
            SmbPacket smbPacket = null;
            
            if (smbHeader.Status == 0 && channel.Peek<byte>(0) == 0 && channel.Peek<ushort>(1) == 0)
            {
                return smbPacket;
            }

            SmbTransactionRequestPacket transactionRequest = request as SmbTransactionRequestPacket;
            if (transactionRequest == null)
            {
                return smbPacket;
            }
            switch (smbClient.Capability.TransactionSubCommand)
            {
                case TransSubCommandExtended.TRANS_EXT_MAILSLOT_WRITE:
                    smbPacket = new SmbTransMailslotWriteResponsePacket();
                    break;

                case TransSubCommandExtended.TRANS_EXT_RAP:
                    smbPacket = new SmbTransRapResponsePacket();
                    break;

                default:
                    break;

            }

            // the packet is find
            if (smbPacket != null)
            {
                return smbPacket;
            }

            // if no setup command. break
            if (transactionRequest.SmbParameters.SetupCount == 0)
            {
                return smbPacket;
            }

            // decode packet using the setup command
            switch ((TransSubCommand)transactionRequest.SmbParameters.Setup[0])
            {
                case TransSubCommand.TRANS_SET_NMPIPE_STATE:
                    smbPacket = new SmbTransSetNmpipeStateResponsePacket();
                    break;

                case TransSubCommand.TRANS_QUERY_NMPIPE_STATE:
                    smbPacket = new SmbTransQueryNmpipeStateResponsePacket();
                    break;

                case TransSubCommand.TRANS_RAW_READ_NMPIPE:
                    smbPacket = new SmbTransRawReadNmpipeResponsePacket();
                    break;

                case TransSubCommand.TRANS_QUERY_NMPIPE_INFO:
                    smbPacket = new SmbTransQueryNmpipeInfoResponsePacket();
                    break;

                case TransSubCommand.TRANS_PEEK_NMPIPE:
                    smbPacket = new SmbTransPeekNmpipeResponsePacket();
                    break;

                case TransSubCommand.TRANS_TRANSACT_NMPIPE:
                    smbPacket = new SmbTransTransactNmpipeResponsePacket();
                    break;

                case TransSubCommand.TRANS_READ_NMPIPE:
                    smbPacket = new SmbTransReadNmpipeResponsePacket();
                    break;

                case TransSubCommand.TRANS_WRITE_NMPIPE:
                    smbPacket = new SmbTransWriteNmpipeResponsePacket();
                    break;

                case TransSubCommand.TRANS_WAIT_NMPIPE:
                    smbPacket = new SmbTransWaitNmpipeResponsePacket();
                    break;

                case TransSubCommand.TRANS_CALL_NMPIPE:
                    smbPacket = new SmbTransCallNmpipeResponsePacket();
                    break;

                default:
                    break;
            }

            return smbPacket;
        }


        /// <summary>
        /// createt the transactions2 packet
        /// </summary>
        /// <param name="request">the request packet</param>
        /// <param name="smbHeader">the smb header of response packet</param>
        /// <param name="channel">the channel contains the packet bytes</param>
        /// <returns>the response packet</returns>
        [SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling")]
        [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        private SmbPacket CreateTransaction2ResponsePacket(SmbPacket request, SmbHeader smbHeader, Channel channel)
        {
            SmbPacket smbPacket = null;

            if (smbHeader.Status == 0 && channel.Peek<byte>(0) == 0 && channel.Peek<ushort>(1) == 0)
            {
                return smbPacket;
            }

            SmbTransaction2RequestPacket transaction2Request = request as SmbTransaction2RequestPacket;
            if (transaction2Request == null)
            {
                return smbPacket;
            }

            // if no setup command. break
            if (transaction2Request.SmbParameters.SetupCount == 0)
            {
                return smbPacket;
            }

            // decode packet using the setup command
            switch ((Trans2SubCommand)transaction2Request.SmbParameters.Setup[0])
            {
                case Trans2SubCommand.TRANS2_QUERY_FILE_INFORMATION:
                    SmbTrans2QueryFileInformationRequestPacket queryFileRequest =
                        transaction2Request as SmbTrans2QueryFileInformationRequestPacket;
                    if (queryFileRequest != null)
                    {
                        smbPacket = new SmbTrans2QueryFileInformationResponsePacket(
                            queryFileRequest.Trans2Parameters.InformationLevel);
                    }
                    break;

                case Trans2SubCommand.TRANS2_QUERY_PATH_INFORMATION:
                    SmbTrans2QueryPathInformationRequestPacket queryPathRequest =
                       transaction2Request as SmbTrans2QueryPathInformationRequestPacket;
                    if (queryPathRequest != null)
                    {
                        smbPacket = new SmbTrans2QueryPathInformationResponsePacket(
                            queryPathRequest.Trans2Parameters.InformationLevel);
                    }
                    break;

                case Trans2SubCommand.TRANS2_SET_FILE_INFORMATION:
                    smbPacket = new SmbTrans2SetFileInformationResponsePacket();
                    break;

                case Trans2SubCommand.TRANS2_SET_PATH_INFORMATION:
                    smbPacket = new SmbTrans2SetPathInformationResponsePacket();
                    break;

                case Trans2SubCommand.TRANS2_QUERY_FS_INFORMATION:
                    SmbTrans2QueryFsInformationRequestPacket queryFsRequest =
                        transaction2Request as SmbTrans2QueryFsInformationRequestPacket;
                    if (queryFsRequest != null)
                    {
                        smbPacket = new SmbTrans2QueryFsInformationResponsePacket(
                            queryFsRequest.Trans2Parameters.InformationLevel);
                    }
                    break;

                case Trans2SubCommand.TRANS2_SET_FS_INFORMATION:
                    smbPacket = new SmbTrans2SetFsInformationResponsePacket();
                    break;

                case Trans2SubCommand.TRANS2_FIND_FIRST2:
                    SmbTrans2FindFirst2RequestPacket first2Request =
                       transaction2Request as SmbTrans2FindFirst2RequestPacket;
                    if (first2Request != null)
                    {
                        smbPacket = new SmbTrans2FindFirst2ResponsePacket(first2Request.Trans2Parameters.InformationLevel,
                            (first2Request.Trans2Parameters.Flags & Trans2FindFlags.SMB_FIND_RETURN_RESUME_KEYS)
                            == Trans2FindFlags.SMB_FIND_RETURN_RESUME_KEYS);
                    }
                    break;

                case Trans2SubCommand.TRANS2_FIND_NEXT2:
                    SmbTrans2FindNext2RequestPacket next2Request =
                       transaction2Request as SmbTrans2FindNext2RequestPacket;
                    if (next2Request != null)
                    {
                        smbPacket = new SmbTrans2FindNext2ResponsePacket(next2Request.Trans2Parameters.InformationLevel,
                            (next2Request.Trans2Parameters.Flags & Trans2FindFlags.SMB_FIND_RETURN_RESUME_KEYS)
                            == Trans2FindFlags.SMB_FIND_RETURN_RESUME_KEYS);
                    }
                    break;

                case Trans2SubCommand.TRANS2_GET_DFS_REFERRAL:
                    smbPacket = new SmbTrans2GetDfsReferralResponsePacket();
                    break;

                default:
                    break;
            }

            return smbPacket;
        }


        /// <summary>
        /// create the nt transaction packet
        /// </summary>
        /// <param name="request">the request packet</param>
        /// <param name="smbHeader">the smb header of response packet</param>
        /// <param name="channel">the channel contains the packet bytes</param>
        /// <returns>the response packet</returns>
        private SmbPacket CreateNtTransactionResponsePacket(SmbPacket request, SmbHeader smbHeader, Channel channel)
        {
            SmbPacket smbPacket = null;

            if (smbHeader.Status == 0 && channel.Peek<byte>(0) == 0 && channel.Peek<ushort>(1) == 0)
            {
                return smbPacket;
            }

            SmbNtTransactRequestPacket ntTransactRequest = request as SmbNtTransactRequestPacket;
            if (ntTransactRequest == null)
            {
                return smbPacket;
            }

            // find regular packet
            switch ((uint)ntTransactRequest.SmbParameters.Function)
            {
                case (uint)NtTransSubCommand.NT_TRANSACT_RENAME:
                    smbPacket = new SmbNtTransRenameResponsePacket();
                    break;

                case (uint)NtTransSubCommand.NT_TRANSACT_CREATE:
                    smbPacket = new SmbNtTransactCreateResponsePacket();
                    break;

                case (uint)NtTransSubCommand.NT_TRANSACT_IOCTL:

                    NT_TRANSACT_IOCTL_SETUP setup =
                       CifsMessageUtils.ToStuct<NT_TRANSACT_IOCTL_SETUP>(
                       CifsMessageUtils.ToBytesArray<ushort>(ntTransactRequest.SmbParameters.Setup));

                    switch ((NtTransFunctionCode)setup.FunctionCode)
                    {
                        case NtTransFunctionCode.FSCTL_SRV_ENUMERATE_SNAPSHOTS:
                            smbPacket = new SmbNtTransFsctlSrvEnumerateSnapshotsResponsePacket();
                            break;

                        case NtTransFunctionCode.FSCTL_SRV_REQUEST_RESUME_KEY:
                            smbPacket = new SmbNtTransFsctlSrvRequestResumeKeyResponsePacket();
                            break;

                        case NtTransFunctionCode.FSCTL_SRV_COPYCHUNK:
                            smbPacket = new SmbNtTransFsctlSrvCopyChunkResponsePacket();
                            break;

                        default:
                            smbPacket = new SmbNtTransactIoctlResponsePacket();
                            break;
                    }

                    break;

                case (uint)SmbNtTransSubCommand.NT_TRANSACT_QUERY_QUOTA:
                    smbPacket = new SmbNtTransQueryQuotaResponsePacket();
                    break;

                case (uint)SmbNtTransSubCommand.NT_TRANSACT_SET_QUOTA:
                    smbPacket = new SmbNtTransSetQuotaResponsePacket();
                    break;

                default:
                    break;
            }

            return smbPacket;
        }


        #endregion

    }
}
