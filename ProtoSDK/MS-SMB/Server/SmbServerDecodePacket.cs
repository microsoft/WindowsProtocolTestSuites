// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.IO;
using System.Diagnostics.CodeAnalysis;

using Microsoft.Protocols.TestTools.StackSdk.Messages;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Cifs;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb
{
    /// <summary>
    /// packet decoder
    /// </summary>
    public class SmbServerDecodePacket : CifsServerDecodePacket
    {
        /// <summary>
        /// server context
        /// </summary>
        private SmbServerContext context;

        /// <summary>
        /// server context
        /// </summary>
        public SmbServerContext Context
        {
            get
            {
                return context;
            }
            set
            {
                context = value;
            }
        }


        /// <summary>
        /// Constructor
        /// </summary>
        public SmbServerDecodePacket()
            : base()
        {
        }


        /// <summary>
        /// to decode stack packet from the received message bytes. 
        /// </summary>
        /// <param name = "endPointIdentity">the endpoint from which the message bytes are received. </param>
        /// <param name = "messageBytes">the received message bytes to be decoded. </param>
        /// <param name = "consumedLength">the length of message bytes consumed by decoder. </param>
        /// <param name = "expectedLength">the length of message bytes the decoder expects to receive. </param>
        /// <returns>the stack packets decoded from the received message bytes. </returns>
        [SuppressMessage("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        public SmbPacket[] DecodePacket(
            object endPointIdentity, byte[] messageBytes, out int consumedLength, out int expectedLength)
        {
            // initialize the default values
            expectedLength = 0;
            consumedLength = 0;
            SmbPacket[] packets = null;

            byte[] packetBytes = null;
            // direct tcp transport management instance
            SmbDirectTcpPacket directTcp = new SmbDirectTcpPacket(null);

            // Netbios over Tcp
            if (endPointIdentity is int)
            {
                packetBytes = messageBytes;
            }
            // Direct Tcp
            else
            {
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
                packetBytes = directTcp.GetTcpData(messageBytes);
            }

            packets = DecodePacketFromBytesWithoutTransport(
                this.context.GetConnection(endPointIdentity), packetBytes, out consumedLength, out expectedLength);

            // decode the header of packet
            if (packets != null && packets.Length > 0)
            {
                packets[0].TransportHeader = CifsMessageUtils.ToStuct<TransportHeader>(messageBytes);
            }

            // for Direct Tcp, calculate the cosumedlength and plus the Tcp header length.
            if (!(endPointIdentity is int))
            {
                // to consume the additional data
                consumedLength = Math.Max(consumedLength, directTcp.GetTcpPacketLength(messageBytes));

                // update the consumed length with transport
                consumedLength += directTcp.TcpTransportHeaderSize;
            }

            return packets;
        }


        /// <summary>
        /// to decode stack packet from the received message bytes.
        /// the message bytes contains data without transport information
        /// </summary>
        /// <param name = "connection">the connection from which the message bytes are received. </param>
        /// <param name = "packetBytesWithoutTransport">the received message bytes to be decoded. </param>
        /// <param name = "consumedLength">the length of message bytes consumed by decoder. </param>
        /// <param name = "expectedLength">the length of message bytes the decoder expects to receive. </param>
        /// <returns>the stack packets decoded from the received message bytes. </returns>
        /// <exception cref="InvalidOperationException">
        /// thrown when packet security signature is invalid.
        /// </exception>
        private SmbPacket[] DecodePacketFromBytesWithoutTransport(
            SmbServerConnection connection, byte[] packetBytesWithoutTransport, 
            out int consumedLength, out int expectedLength)
        {
            expectedLength = 0;
            consumedLength = 0;
            SmbPacket[] packets = new SmbPacket[0];

            int packetConsumedLength = 0;
            int packetExpectedLength = 0;

            // decode packets using cifs decorder.
            SmbPacket request = this.DecodeSmbRequestFromBytes(
                packetBytesWithoutTransport, out packetConsumedLength);

            // valid the signature
            if (request != null && request.SmbHeader.SecurityFeatures != 0
                && connection.GssApi != null && connection.GssApi.SessionKey != null)
            {
                byte[] bytesToValid = ArrayUtility.SubArray(packetBytesWithoutTransport, 0);

                // the signature offset is 14.
                byte[] zeroSignature = new byte[sizeof(ulong)];
                Array.Copy(zeroSignature, 0, bytesToValid, 14, zeroSignature.Length);

                // the signature offset is 14.
                zeroSignature = BitConverter.GetBytes((ulong)connection.ServerNextReceiveSequenceNumber);
                Array.Copy(zeroSignature, 0, bytesToValid, 14, zeroSignature.Length);

                byte[] signature = CifsMessageUtils.CreateSignature(bytesToValid, connection.GssApi.SessionKey);
                if (request.SmbHeader.SecurityFeatures != BitConverter.ToUInt64(signature, 0))
                {
                    throw new InvalidOperationException("packet security signature is invalid");
                }
            }

            // Use the decoded packet to UpdateRoleContext if it is not null and ContextUpdate is enabled:
            if (request != null)
            {
                if (this.context.IsUpdateContext)
                {
                    this.context.UpdateRoleContext(connection, request);
                }
                packets = new SmbPacket[] { request };
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
        /// <param name="smbBatchedRequest">the batched request</param>
        /// <returns>the consumed length of batched request packet</returns>
        protected override int DecodeBatchedRequest(Channel channel, SmbBatchedRequestPacket smbBatchedRequest)
        {
            // do not process batched request.
            return 0;
        }


        /// <summary>
        /// to new a Smb request packet in type of the Command in SmbHeader.
        /// </summary>
        /// <param name="messageBytes">bytes contains packet</param>
        /// <returns>
        /// the new request packet. 
        /// the null means that the utility don't know how to create the request.
        /// </returns>
        protected override SmbPacket CreateSmbRequestPacket(byte[] messageBytes)
        {
            SmbPacket smbRequest = null;

            using (MemoryStream stream = new MemoryStream(messageBytes, true))
            {
                using (Channel channel = new Channel(null, stream))
                {
                    // read smb header and new SmbPacket: 
                    if (channel.Stream.Position < channel.Stream.Length
                        && messageBytes.Length >= CifsMessageUtils.GetSize<SmbHeader>(new SmbHeader()))
                    {
                        SmbHeader smbHeader = channel.Read<SmbHeader>();
                        smbRequest = FindTheTargetPacket(smbHeader, channel);
                    }
                }
            }

            return smbRequest;
        }


        /// <summary>
        /// find the target packet.
        /// </summary>
        /// <param name="smbHeader">the header of smb packet</param>
        /// <param name="channel">the channel to access bytes</param>
        /// <returns>the target packet</returns>
        private static SmbPacket FindTheTargetPacket(SmbHeader smbHeader, Channel channel)
        {
            SmbPacket smbPacket = null;

            switch (smbHeader.Command)
            {
                case SmbCommand.SMB_COM_NEGOTIATE:
                        smbPacket = new SmbNegotiateRequestPacket();
                    break;

                case SmbCommand.SMB_COM_SESSION_SETUP_ANDX:
                    SmbHeader_Flags2_Values flags2 = (SmbHeader_Flags2_Values)smbHeader.Flags2;
                    if ((flags2 & SmbHeader_Flags2_Values.SMB_FLAGS2_EXTENDED_SECURITY)
                        == SmbHeader_Flags2_Values.SMB_FLAGS2_EXTENDED_SECURITY)
                    {
                        smbPacket = new Smb.SmbSessionSetupAndxRequestPacket();
                    }
                    else
                    {
                        smbPacket = new Cifs.SmbSessionSetupAndxRequestPacket();
                    }
                    break;

                case SmbCommand.SMB_COM_TREE_CONNECT_ANDX:
                    smbPacket = new SmbTreeConnectAndxRequestPacket();
                    break;

                case SmbCommand.SMB_COM_NT_CREATE_ANDX:
                    smbPacket = new SmbNtCreateAndxRequestPacket();
                    break;

                case SmbCommand.SMB_COM_OPEN_ANDX:
                    smbPacket = new SmbOpenAndxRequestPacket();
                    break;

                case SmbCommand.SMB_COM_WRITE_ANDX:
                    smbPacket = new SmbWriteAndxRequestPacket();
                    break;

                case SmbCommand.SMB_COM_READ_ANDX:
                    smbPacket = new SmbReadAndxRequestPacket();
                    break;

                case SmbCommand.SMB_COM_CLOSE:
                    smbPacket = new SmbCloseRequestPacket();
                    break;

                case SmbCommand.SMB_COM_TREE_DISCONNECT:
                    smbPacket = new SmbTreeDisconnectRequestPacket();
                    break;

                case SmbCommand.SMB_COM_LOGOFF_ANDX:
                    smbPacket = new SmbLogoffAndxRequestPacket();
                    break;

                case SmbCommand.SMB_COM_TRANSACTION:
                    SMB_COM_TRANSACTION_Request_SMB_Parameters transaction =
                        channel.Read<SMB_COM_TRANSACTION_Request_SMB_Parameters>();
                    if (transaction.SetupCount == 0)
                    {
                        smbPacket = new SmbTransRapRequestPacket();
                    }
                    else
                    {
                        smbPacket = FindTheTransactionPacket(
                            transaction.SetupCount, (TransSubCommand)transaction.Setup[0]);
                    }
                    break;

                case SmbCommand.SMB_COM_TRANSACTION2:
                    SMB_COM_TRANSACTION2_Request_SMB_Parameters transaction2 =
                        channel.Read<SMB_COM_TRANSACTION2_Request_SMB_Parameters>();
                    smbPacket = FindTheTrans2Packet((Trans2SubCommand)transaction2.Subcommand);
                    break;

                case SmbCommand.SMB_COM_NT_TRANSACT:
                    SMB_COM_NT_TRANSACT_Request_SMB_Parameters ntTransactoin =
                        channel.Read<SMB_COM_NT_TRANSACT_Request_SMB_Parameters>();
                    smbPacket = FindTheNtTransPacket(ntTransactoin.Function, CifsMessageUtils.ToBytesArray<ushort>(ntTransactoin.Setup));
                    break;

                default:
                    break;
            }

            return smbPacket;
        }


        /// <summary>
        /// find the nt transaction packets
        /// </summary>
        /// <param name="command">the command of nt transaction</param>
        /// <param name="setup">the setup contains the sub command</param>
        /// <returns>the target nt transaction packet</returns>
        private static SmbPacket FindTheNtTransPacket(NtTransSubCommand command, byte[] setup)
        {
            SmbPacket smbPacket = null;
            switch (command)
            {
                case NtTransSubCommand.NT_TRANSACT_CREATE:
                    smbPacket = new SmbNtTransactCreateRequestPacket();
                    break;

                case NtTransSubCommand.NT_TRANSACT_RENAME:
                    smbPacket = new SmbNtTransRenameRequestPacket();
                    break;

                case NtTransSubCommand.NT_TRANSACT_IOCTL:
                    NT_TRANSACT_IOCTL_SETUP subCommand = CifsMessageUtils.ToStuct<NT_TRANSACT_IOCTL_SETUP>(setup);
                    switch ((NtTransFunctionCode)subCommand.FunctionCode)
                    {
                        case NtTransFunctionCode.FSCTL_SRV_ENUMERATE_SNAPSHOTS:
                            smbPacket = new SmbNtTransFsctlSrvEnumerateSnapshotsRequestPacket();
                            break;

                        case NtTransFunctionCode.FSCTL_SRV_REQUEST_RESUME_KEY:
                            smbPacket = new SmbNtTransFsctlSrvRequestResumeKeyRequestPacket();
                            break;

                        case NtTransFunctionCode.FSCTL_SRV_COPYCHUNK:
                            smbPacket = new SmbNtTransFsctlSrvCopyChunkRequestPacket();
                            break;

                        default:
                            smbPacket = new SmbNtTransactIoctlRequestPacket();
                            break;
                    }
                    break;

                default:
                    switch ((SmbNtTransSubCommand)command)
                    {
                        case SmbNtTransSubCommand.NT_TRANSACT_QUERY_QUOTA:
                            smbPacket = new SmbNtTransQueryQuotaRequestPacket();
                            break;

                        case SmbNtTransSubCommand.NT_TRANSACT_SET_QUOTA:
                            smbPacket = new SmbNtTransSetQuotaRequestPacket();
                            break;
                    }
                    break;
            }

            return smbPacket;
        }


        /// <summary>
        /// find the transaction2 packet.
        /// </summary>
        /// <param name="command">the command of transaction2 packet.</param>
        /// <returns>the target transaction2 packet</returns>
        private static SmbPacket FindTheTrans2Packet(Trans2SubCommand command)
        {
            SmbPacket smbPacket = null;
            switch ((Trans2SubCommand)command)
            {
                case Trans2SubCommand.TRANS2_FIND_FIRST2:
                    smbPacket = new SmbTrans2FindFirst2RequestPacket();
                    break;

                case Trans2SubCommand.TRANS2_FIND_NEXT2:
                    smbPacket = new SmbTrans2FindNext2RequestPacket();
                    break;

                case Trans2SubCommand.TRANS2_QUERY_FS_INFORMATION:
                    smbPacket = new SmbTrans2QueryFsInformationRequestPacket();
                    break;

                case Trans2SubCommand.TRANS2_SET_FS_INFORMATION:
                    smbPacket = new SmbTrans2SetFsInformationRequestPacket();
                    break;

                case Trans2SubCommand.TRANS2_QUERY_PATH_INFORMATION:
                    smbPacket = new SmbTrans2QueryPathInformationRequestPacket();
                    break;

                case Trans2SubCommand.TRANS2_SET_PATH_INFORMATION:
                    smbPacket = new SmbTrans2SetPathInformationRequestPacket();
                    break;

                case Trans2SubCommand.TRANS2_QUERY_FILE_INFORMATION:
                    smbPacket = new SmbTrans2QueryFileInformationRequestPacket();
                    break;

                case Trans2SubCommand.TRANS2_SET_FILE_INFORMATION:
                    smbPacket = new SmbTrans2SetFileInformationRequestPacket();
                    break;

                case Trans2SubCommand.TRANS2_GET_DFS_REFERRAL:
                    smbPacket = new SmbTrans2GetDfsReferralRequestPacket();
                    break;

                default:
                    break;
            }
            return smbPacket;
        }


        /// <summary>
        /// find the transaction packet.
        /// </summary>
        /// <param name="setupCount">the count of setup</param>
        /// <param name="command">the command of transaction packet</param>
        /// <returns>the target transaction packet</returns>
        private static SmbPacket FindTheTransactionPacket(byte setupCount, TransSubCommand command)
        {
            if (setupCount == 0)
            {
                return new SmbTransRapRequestPacket();
            }
            else if (setupCount == 3)
            {
                return new SmbTransMailslotWriteRequestPacket();
            }

            SmbPacket smbPacket = null;
            switch ((TransSubCommand)command)
            {
                case TransSubCommand.TRANS_SET_NMPIPE_STATE:
                    smbPacket = new SmbTransSetNmpipeStateRequestPacket();
                    break;
                case TransSubCommand.TRANS_QUERY_NMPIPE_STATE:
                    smbPacket = new SmbTransQueryNmpipeStateRequestPacket();
                    break;
                case TransSubCommand.TRANS_QUERY_NMPIPE_INFO:
                    smbPacket = new SmbTransQueryNmpipeInfoRequestPacket();
                    break;
                case TransSubCommand.TRANS_PEEK_NMPIPE:
                    smbPacket = new SmbTransPeekNmpipeRequestPacket();
                    break;
                case TransSubCommand.TRANS_TRANSACT_NMPIPE:
                    smbPacket = new SmbTransTransactNmpipeRequestPacket();
                    break;
                case TransSubCommand.TRANS_RAW_READ_NMPIPE:
                    smbPacket = new SmbTransRawReadNmpipeRequestPacket();
                    break;
                case TransSubCommand.TRANS_READ_NMPIPE:
                    smbPacket = new SmbTransReadNmpipeRequestPacket();
                    break;
                case TransSubCommand.TRANS_WRITE_NMPIPE:
                    smbPacket = new SmbTransWriteNmpipeRequestPacket();
                    break;
                case TransSubCommand.TRANS_WAIT_NMPIPE:
                    smbPacket = new SmbTransWaitNmpipeRequestPacket();
                    break;
                case TransSubCommand.TRANS_CALL_NMPIPE:
                    smbPacket = new SmbTransCallNmpipeRequestPacket();
                    break;
                case TransSubCommand.TRANS_RAW_WRITE_NMPIPE:
                    smbPacket = new SmbTransRawWriteNmpipeRequestPacket();
                    break;
                default:
                    break;
            }
            return smbPacket;
        }


    }
}
