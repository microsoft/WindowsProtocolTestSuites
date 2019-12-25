// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Linq;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Protocols.TestTools.StackSdk.Security.Cryptographic;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2.Common;
using System.Text;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2
{
    /// <summary>
    /// Smb2Decoder is used to decode smb2 packet
    /// </summary>
    public class Smb2Decoder
    {
        //the decode role tell the decoder the packet should be decoded as request packet or response packet
        private Smb2Role decodeRole;
        //the payload of Tcp transport will have extra 4 bytes in front of the message to indicate the packet length
        private Smb2TransportType transportType;

        private Dictionary<ulong, Smb2CryptoInfo> cryptoInfoTable;
        private Smb2CompressionInfo compressionInfo;

        //The share name of named pipe
        private const string NamedPipeShareName = "IPC$";

        private bool checkEncrypt;
        /// <summary>
        /// The underlying transport type
        /// </summary>
        public Smb2TransportType TransportType
        {
            get
            {
                return transportType;
            }
            set
            {
                transportType = value;
            }
        }

        /// <summary>
        /// the decode role tell the decoder the packet should be decoded as request packet or response packet
        /// </summary>
        public Smb2Role DecodeRole
        {
            get
            {
                return decodeRole;
            }
        }

        /// <summary>
        /// Indicates whether to check the response from the server is actually encrypted.
        /// </summary>
        public bool CheckEncrypt
        {
            get
            {
                return checkEncrypt;
            }
            set
            {
                checkEncrypt = value;
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="decodeRole">The decode role, client or server</param>
        /// <param name="cryptoInfoTable">Crypto info table indexed by session ID</param>
        public Smb2Decoder(Smb2Role decodeRole, Dictionary<ulong, Smb2CryptoInfo> cryptoInfoTable)
        {
            this.decodeRole = decodeRole;
            this.cryptoInfoTable = cryptoInfoTable;
            this.compressionInfo = new Smb2CompressionInfo();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="decodeRole">The decode role, client or server</param>
        /// <param name="cryptoInfoTable">Crypto info table indexed by session ID</param>
        /// <param name="compressionInfo">Compression information for SMB2 connection.</param>
        public Smb2Decoder(Smb2Role decodeRole, Dictionary<ulong, Smb2CryptoInfo> cryptoInfoTable, Smb2CompressionInfo compressionInfo)
        {
            this.decodeRole = decodeRole;
            this.cryptoInfoTable = cryptoInfoTable;
            this.compressionInfo = compressionInfo;
        }

        /// <summary>
        /// This function is called by transport stack as a callback when the transport receive any message
        /// </summary>
        /// <param name="endPoint">Where the packet is received</param>
        /// <param name="messageBytes">The received packet</param>
        /// <param name="consumedLength">[OUT]The consumed length of the message</param>
        /// <param name="expectedLength">[OUT]The expected length</param>
        /// <returns>A array of stackpacket</returns>
        public StackPacket[] Smb2DecodePacketCallback(
            object endPoint,
            byte[] messageBytes,
            out int consumedLength,
            out int expectedLength)
        {
            if (messageBytes.Length == 0)
            {
                consumedLength = 0;
                expectedLength = 0;
                return null;
            }

            Smb2Packet packet = DecodeTransportPayload(
                messageBytes,
                decodeRole,
                transportType,
                out consumedLength,
                out expectedLength);

            if (packet == null)
            {
                return null;
            }
            else
            {
                return new StackPacket[] { packet };
            }
        }

        /// <summary>
        /// Decode the payload which transport received.
        /// </summary>
        /// <param name="messageBytes">The received packet</param>
        /// <param name="role">The role of this decoder, client or server</param>
        /// <param name="transportType">The underlying transport type</param>
        /// <param name="consumedLength">[OUT]The consumed length of the message</param>
        /// <param name="expectedLength">[OUT]The expected length</param>
        /// <returns>A Smb2Packet</returns>
        [SuppressMessage(
            "Microsoft.Maintainability",
            "CA1500:VariableNamesShouldNotMatchFieldNames",
            MessageId = "transportType")]
        public Smb2Packet DecodeTransportPayload(
            byte[] messageBytes,
            Smb2Role role,
            Smb2TransportType transportType,
            out int consumedLength,
            out int expectedLength
            )
        {
            //tcp transport will prefix 4 bytes length in the beginning. and netbios won't do this.
            if (transportType == Smb2TransportType.Tcp)
            {
                if (messageBytes.Length < Smb2Consts.TcpPrefixedLenByteCount)
                {
                    consumedLength = 0;
                    expectedLength = 4;
                    return null;
                }

                //in the header of tcp payload, there are 4 bytes(in fact only 3 bytes are used) which indicate
                //the length of smb2
                int dataLenShouldHave = (messageBytes[1] << 16) + (messageBytes[2] << 8) + messageBytes[3];

                if (dataLenShouldHave > (messageBytes.Length - Smb2Consts.TcpPrefixedLenByteCount))
                {
                    consumedLength = 0;
                    expectedLength = Smb2Consts.TcpPrefixedLenByteCount + dataLenShouldHave;
                    return null;
                }

                byte[] smb2Message = new byte[dataLenShouldHave];

                Array.Copy(messageBytes, Smb2Consts.TcpPrefixedLenByteCount, smb2Message, 0, smb2Message.Length);

                Smb2Packet packet = DecodeCompletePacket(
                    smb2Message,
                    role,
                    0,
                    0,
                    out consumedLength,
                    out expectedLength);

                // Here we ignore the consumedLength returned by DecodeCompletePacket(), there may be some tcp padding data 
                // at the end which we are not interested.
                consumedLength = dataLenShouldHave + Smb2Consts.TcpPrefixedLenByteCount;

                return packet;
            }
            else
            {
                Smb2Packet packet = DecodeCompletePacket(
                    messageBytes,
                    role,
                    0,
                    0,
                    out consumedLength,
                    out expectedLength);

                //Some packet has unknown padding data at the end.
                consumedLength = messageBytes.Length;

                return packet;
            }
        }


        /// <summary>
        /// Decode the  message except length field which may exist if transport is tcp
        /// </summary>
        /// <param name="messageBytes">The received packet</param>
        /// <param name="role">The role of this decoder, client or server</param>
        /// <param name="realSessionId">The real sessionId for this packet</param>
        /// <param name="realTreeId">The real treeId for this packet</param>
        /// <param name="consumedLength">[OUT]The consumed length of the message</param>
        /// <param name="expectedLength">[OUT]The expected length</param>
        /// <returns>A Smb2Packet</returns>
        public Smb2Packet DecodeCompletePacket(
            byte[] messageBytes,
            Smb2Role role,
            ulong realSessionId,
            uint realTreeId,
            out int consumedLength,
            out int expectedLength
            )
        {
            //protocol version is of 4 bytes len
            byte[] protocolVersion = new byte[sizeof(uint)];
            Array.Copy(messageBytes, 0, protocolVersion, 0, protocolVersion.Length);

            SmbVersion version = DecodeVersion(protocolVersion);

            if (version == SmbVersion.Version1)
            {
                // SMB Negotiate packet
                return DecodeSmbPacket(messageBytes, role, out consumedLength, out expectedLength);
            }
            else if (version == SmbVersion.Version2Encrypted)
            {
                // SMB2 encrypted packet
                return DecodeEncryptedSmb2Packet(
                    messageBytes,
                    role,
                    realSessionId,
                    realTreeId,
                    out consumedLength,
                    out expectedLength
                    );
            }
            else if (version == SmbVersion.Version2Compressed)
            {
                var decompressedPacket = DecodeCompressedSmb2Packet(
                    messageBytes,
                    role,
                    realSessionId,
                    realTreeId,
                    out consumedLength,
                    out expectedLength,
                    null
                    );

                //For single packet signature verification
                if (decompressedPacket is Smb2SinglePacket)
                {
                    //verify signature of a single packet
                    Smb2SinglePacket singlePacket = decompressedPacket as Smb2SinglePacket;

                    TryVerifySignatureExceptSessionSetupResponse(singlePacket, singlePacket.Header.SessionId, messageBytes);

                    CheckIfNeedEncrypt(singlePacket);
                }
                else if (decompressedPacket is Smb2CompoundPacket)//For Compound packet signature verification
                {
                    //verify signature of the compound packet
                    TryVerifySignature(decompressedPacket as Smb2CompoundPacket, messageBytes);

                    CheckIfNeedEncrypt(decompressedPacket as Smb2CompoundPacket);
                }

                return decompressedPacket;
            }
            else
            {
                // SMB2 packet not encrypted
                Smb2Packet decodedPacket = DecodeSmb2Packet(
                    messageBytes,
                    role,
                    realSessionId,
                    realTreeId,
                    out consumedLength,
                    out expectedLength,
                    null
                    );

                //For single packet signature verification               
                if (decodedPacket is Smb2SinglePacket)
                {
                    //verify signature of a single packet
                    Smb2SinglePacket singlePacket = decodedPacket as Smb2SinglePacket;

                    TryVerifySignatureExceptSessionSetupResponse(singlePacket, singlePacket.Header.SessionId, messageBytes);

                    CheckIfNeedEncrypt(singlePacket);
                }
                else if (decodedPacket is Smb2CompoundPacket)//For Compound packet signature verification
                {
                    //verify signature of the compound packet
                    TryVerifySignature(decodedPacket as Smb2CompoundPacket, messageBytes);

                    CheckIfNeedEncrypt(decodedPacket as Smb2CompoundPacket);
                }

                return decodedPacket;
            }
        }

        /// <summary>
        /// Check if the packet needs to be encrypted but actually not encrypted.
        /// </summary>
        private void CheckIfNeedEncrypt(Smb2SinglePacket packet, ulong sessionId = 0)
        {
            if (!CheckEncrypt)
                return;
            var realSessionId = sessionId == 0 ? packet.Header.SessionId : sessionId;
            var cryptoInfo = cryptoInfoTable.ContainsKey(realSessionId) ? cryptoInfoTable[realSessionId] : null;
            if (cryptoInfo != null)
            {
                if (cryptoInfo.EnableSessionEncryption || (cryptoInfo.EnableTreeEncryption.Contains(packet.Header.TreeId) && packet.Header.Command != Smb2Command.TREE_CONNECT))
                {
                    throw new Exception($"The packet should be encrypted: \"{packet.ToString()}\"");
                }
            }
        }

        /// <summary>
        /// Check if the compound packet needs to be encrypted but actually not encrypted.
        /// </summary>
        private void CheckIfNeedEncrypt(Smb2CompoundPacket packet)
        {
            if (!CheckEncrypt)
                return;
            ulong firstSessionId = packet.Packets[0].Header.SessionId;

            for (int i = 0; i < packet.Packets.Count; i++)
            {
                Smb2SinglePacket singlePacket = packet.Packets[i];

                // For Related operations, the sessionId is in the first packet of the compound packet.
                ulong sessionId = singlePacket.Header.Flags.HasFlag(Packet_Header_Flags_Values.FLAGS_RELATED_OPERATIONS) ? firstSessionId : singlePacket.Header.SessionId;
                CheckIfNeedEncrypt(singlePacket, sessionId);
            }
        }

        /// <summary>
        /// Decode the packet as smb packet
        /// </summary>
        /// <param name="messageBytes">The received packet</param>
        /// <param name="role">The role of this decoder, client or server</param>
        /// <param name="consumedLen">[OUT]The consumed length of the message</param>
        /// <param name="expectedLen">[OUT]The expected length</param>
        /// <returns>A smb2Packet</returns>
        private static Smb2Packet DecodeSmbPacket(byte[] messageBytes, Smb2Role role, out int consumedLen, out int expectedLen)
        {
            // SMB2 only uses smb negotiate packet
            Smb2Packet packet;
            if (role == Smb2Role.Client)
            {
                packet = new SmbNegotiateResponsePacket();
            }
            else
            {
                packet = new SmbNegotiateRequestPacket();
            }
            packet.FromBytes(messageBytes, out consumedLen, out expectedLen);
            return packet;
        }

        private Smb2Packet DecodeEncryptedSmb2Packet(
            byte[] messageBytes,
            Smb2Role role,
            ulong realSessionId,
            uint realTreeId,
            out int consumedLength,
            out int expectedLength
            )
        {
            Transform_Header transformHeader;
            var decryptedBytes = Smb2Crypto.Decrypt(messageBytes, cryptoInfoTable, decodeRole, out transformHeader);

            byte[] protocolVersion = new byte[sizeof(uint)];
            Array.Copy(decryptedBytes, 0, protocolVersion, 0, protocolVersion.Length);

            SmbVersion version = DecodeVersion(protocolVersion);

            if (version == SmbVersion.Version2Compressed)
            {
                return DecodeCompressedSmb2Packet(
                        decryptedBytes,
                        role,
                        realSessionId,
                        realTreeId,
                        out consumedLength,
                        out expectedLength,
                        transformHeader
                        );
            }
            else if (version == SmbVersion.Version2)
            {

                return DecodeSmb2Packet(
                        decryptedBytes,
                        role,
                        realSessionId,
                        realTreeId,
                        out consumedLength,
                        out expectedLength,
                        transformHeader
                        );
            }
            else
            {
                throw new InvalidOperationException("Unkown ProtocolId!");
            }
        }

        private Smb2Packet DecodeCompressedSmb2Packet(
            byte[] messageBytes,
            Smb2Role role,
            ulong realSessionId,
            uint realTreeId,
            out int consumedLength,
            out int expectedLength,
            Transform_Header? transformHeader
            )
        {
            var compressedPacket = new Smb2CompressedPacket();
            compressedPacket.FromBytes(messageBytes, out consumedLength, out expectedLength);
            var orignialPacketBytes = Smb2Compression.Decompress(compressedPacket, compressionInfo, role);
            var decodedPacket = DecodeSmb2Packet(
                        orignialPacketBytes,
                        role,
                        realSessionId,
                        realTreeId,
                        out consumedLength,
                        out expectedLength,
                        transformHeader
                        );


            decodedPacket.Compressed = true;
            decodedPacket.CompressedPacket = compressedPacket;

            return decodedPacket;
        }

        /// <summary>
        /// Decode the message as smb2 packet
        /// </summary>
        /// <param name="messageBytes">The received packet</param>
        /// <param name="role">The role of this decoder, client or server</param>
        /// <param name="realSessionId">The real sessionId for this packet</param>
        /// <param name="realTreeId">The real treeId for this packet</param>
        /// <param name="consumedLength">[OUT]The consumed length of the message</param>
        /// <param name="expectedLength">[OUT]The expected length</param>
        /// <param name="transformHeader">The optional transform header</param>
        /// <returns>A Smb2Packet</returns>
        private Smb2CompressiblePacket DecodeSmb2Packet(
            byte[] messageBytes,
            Smb2Role role,
            ulong realSessionId,
            uint realTreeId,
            out int consumedLength,
            out int expectedLength,
            Transform_Header? transformHeader
            )
        {
            Packet_Header smb2Header;

            smb2Header = TypeMarshal.ToStruct<Packet_Header>(messageBytes);

            if (smb2Header.NextCommand != 0)
            {
                return DecodeCompoundPacket(messageBytes, role, out consumedLength, out expectedLength, transformHeader);
            }
            else
            {
                if (transformHeader != null)
                {
                    // For client: If the NextCommand field in the first SMB2 header of the message is equal to 0 
                    // and SessionId of the first SMB2 header is not equal to the SessionId field in SMB2 TRANSFORM_HEADER of response, the client MUST discard the message.
                    //
                    // For server: For a singleton request if the SessionId field in the SMB2 header of the request is not equal to Request.TransformSessionId, 
                    // the server MUST disconnect the connection.
                    if (smb2Header.SessionId != transformHeader.Value.SessionId)
                    {
                        throw new InvalidOperationException("SessionId is inconsistent for encrypted response.");
                    }
                }
                return DecodeSinglePacket(
                    messageBytes,
                    role,
                    realSessionId,
                    realTreeId,
                    out consumedLength,
                    out expectedLength
                    );
            }
        }


        /// <summary>
        /// Decode the message as smb2 compound packet
        /// </summary>
        /// <param name="messageBytes">The received packet</param>
        /// <param name="role">The role of this decoder, client or server</param>
        /// <param name="consumedLength">[OUT]The consumed length of the message</param>
        /// <param name="expectedLength">[OUT]The expected length</param>
        /// <param name="transformHeader">The optional transform header</param>
        /// <returns>A Smb2Packet</returns>
        public Smb2CompressiblePacket DecodeCompoundPacket(
            byte[] messageBytes,
            Smb2Role role,
            out int consumedLength,
            out int expectedLength,
            Transform_Header? transformHeader
            )
        {
            Smb2CompoundPacket compoundPacket = new Smb2CompoundPacket();
            compoundPacket.decoder = this;

            compoundPacket.FromBytes(messageBytes, out consumedLength, out expectedLength);

            VerifyCompoundPacket(compoundPacket, role, transformHeader);

            return compoundPacket;
        }

        private void VerifyCompoundPacket(Smb2CompoundPacket compoundPacket, Smb2Role role, Transform_Header? transformHeader)
        {
            for (int i = 0; i < compoundPacket.Packets.Count; i++)
            {
                var packet = compoundPacket.Packets[i];

                if (packet.Header.NextCommand % 8 != 0)
                {
                    throw new InvalidOperationException("NextCommand is not a 8-byte aligned offset!");
                }

                switch (role)
                {
                    case Smb2Role.Client:
                        {
                            if (transformHeader != null)
                            {
                                // For each response in a compounded response, if the SessionId field of SMB2 header is not equal to the SessionId field in the SMB2 TRANSFORM_HEADER, 
                                // the client SHOULD<139> discard the entire compounded response and stop processing.
                                if (packet.Header.SessionId != transformHeader.Value.SessionId)
                                {
                                    throw new InvalidOperationException("SessionId is inconsistent for encrypted compounded response.");
                                }
                            }
                        }
                        break;

                    case Smb2Role.Server:
                        {
                            // The server MUST verify if any of the following conditions returns TRUE and, if so, the server MUST disconnect the connection:
                            // For the first operation of a compounded request, 
                            // - SMB2_FLAGS_RELATED_OPERATIONS is set in the Flags field of the SMB2 header of the request 
                            // - The SessionId field in the SMB2 header of the request is not equal to Request.TransformSessionId.
                            // In a compounded request, for each operation in the compounded chain except the first one, 
                            // - SMB2_FLAGS_RELATED_OPERATIONS is not set in the Flags field of the SMB2 header of the operation and SessionId in the SMB2 header of the operation is not equal to Request.TransformSessionId.
                            if (i == 0)
                            {
                                if (packet.Header.Flags.HasFlag(Packet_Header_Flags_Values.FLAGS_RELATED_OPERATIONS))
                                {
                                    throw new InvalidOperationException("FLAGS_RELATED_OPERATIONS should not be set for the first compounded request.");
                                }
                                if (transformHeader != null)
                                {
                                    if (packet.Header.SessionId != transformHeader.Value.SessionId)
                                    {
                                        throw new InvalidOperationException("SessionId is inconsistent for encrypted compounded request.");
                                    }
                                }
                            }
                            else
                            {
                                if (transformHeader != null)
                                {
                                    if (!packet.Header.Flags.HasFlag(Packet_Header_Flags_Values.FLAGS_RELATED_OPERATIONS))
                                    {
                                        if (packet.Header.SessionId != transformHeader.Value.SessionId)
                                        {
                                            throw new InvalidOperationException("SessionId is inconsistent for encrypted compounded request.");
                                        }
                                    }
                                }
                            }
                        }
                        break;
                }
            }
        }


        /// <summary>
        /// Decode the message as smb2 single packet
        /// </summary>
        /// <param name="messageBytes">The received packet</param>
        /// <param name="role">The role of this decoder, client or server</param>
        /// <param name="realSessionId">The real sessionId for this packet</param>
        /// <param name="realTreeId">The real treeId for this packet</param>
        /// <param name="consumedLength">[OUT]The consumed length of the message</param>
        /// <param name="expectedLength">[OUT]The expected length</param>
        /// <returns>A Smb2Packet</returns>
        public Smb2CompressiblePacket DecodeSinglePacket(
            byte[] messageBytes,
            Smb2Role role,
            ulong realSessionId,
            uint realTreeId,
            out int consumedLength,
            out int expectedLength
            )
        {
            if (role == Smb2Role.Client)
            {
                return DecodeSingleResponsePacket(
                    messageBytes,
                    realSessionId,
                    realTreeId,
                    out consumedLength,
                    out expectedLength
                    );
            }
            else if (role == Smb2Role.Server)
            {
                return DecodeSingleRequestPacket(messageBytes, out consumedLength, out expectedLength);
            }
            else
            {
                throw new ArgumentException("role should be client or server", "role");
            }
        }


        /// <summary>
        /// Decode the message as smb2 single request packet
        /// </summary>
        /// <param name="messageBytes">The received packet</param>
        /// <param name="consumedLength">[OUT]The consumed length of the message</param>
        /// <param name="expectedLength">[OUT]The expected length</param>
        /// <returns>A Smb2Packet</returns>
        private static Smb2CompressiblePacket DecodeSingleRequestPacket(byte[] messageBytes, out int consumedLength, out int expectedLength)
        {
            Packet_Header smb2Header;

            int offset = 0;
            smb2Header = TypeMarshal.ToStruct<Packet_Header>(messageBytes, ref offset);

            if (smb2Header.Command == Smb2Command.OPLOCK_BREAK)
            {
                ushort structureSize = TypeMarshal.ToStruct<ushort>(messageBytes, ref offset);
            }

            Smb2CompressiblePacket packet = null;

            switch (smb2Header.Command)
            {

                case Smb2Command.NEGOTIATE:
                    packet = new Smb2NegotiateRequestPacket();
                    break;
                default:
                    throw new InvalidOperationException("Received an unknown packet! the type of the packet is "
                        + smb2Header.Command.ToString());
            }

            packet.FromBytes(messageBytes, out consumedLength, out expectedLength);

            return packet;
        }


        /// <summary>
        /// Decode the message as smb2 single response packet
        /// </summary>
        /// <param name="messageBytes">The received packet</param>
        /// <param name="realSessionId">The real sessionId for this packet</param>
        /// <param name="realTreeId">The real treeId for this packet</param>
        /// <param name="consumedLength">[OUT]The consumed length of the message</param>
        /// <param name="expectedLength">[OUT]The expected length</param>
        /// <returns>A Smb2Packet</returns>
        [SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling")]
        [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        private Smb2CompressiblePacket DecodeSingleResponsePacket(
            byte[] messageBytes,
            ulong realSessionId,
            uint realTreeId,
            out int consumedLength,
            out int expectedLength
            )
        {
            Packet_Header smb2Header;

            bool isLeaseBreakPacket = false;

            int offset = 0;
            smb2Header = TypeMarshal.ToStruct<Packet_Header>(messageBytes, ref offset);

            if (smb2Header.Command == Smb2Command.OPLOCK_BREAK)
            {
                ushort structureSize = TypeMarshal.ToStruct<ushort>(messageBytes, ref offset);

                if (structureSize == (ushort)OplockLeaseBreakStructureSize.LeaseBreakNotification
                    || structureSize == (ushort)OplockLeaseBreakStructureSize.LeaseBreakResponse
                    || structureSize == 9) // Add this condition temporally to handle LeaseBreakResponse is error response (i.e. structureSize == 9), but this will still hide the condition when OplockBreakResponse is error response
                {
                    isLeaseBreakPacket = true;
                }
            }


            Smb2SinglePacket packet = null;
            ushort structSize = BitConverter.ToUInt16(messageBytes, Smb2Consts.Smb2HeaderLen);

            switch (smb2Header.Command)
            {
                case Smb2Command.CANCEL:
                    packet = new Smb2CancelResponsePacket();
                    break;
                case Smb2Command.CHANGE_NOTIFY:
                    packet = new Smb2ChangeNotifyResponsePacket();
                    break;
                case Smb2Command.CLOSE:
                    packet = new Smb2CloseResponsePacket();
                    break;
                case Smb2Command.CREATE:
                    packet = new Smb2CreateResponsePacket();
                    break;
                case Smb2Command.ECHO:
                    packet = new Smb2EchoResponsePacket();
                    break;
                case Smb2Command.FLUSH:
                    packet = new Smb2FlushResponsePacket();
                    break;
                case Smb2Command.IOCTL:
                    packet = new Smb2IOCtlResponsePacket();
                    break;
                case Smb2Command.LOCK:
                    packet = new Smb2LockResponsePacket();
                    break;
                case Smb2Command.LOGOFF:
                    packet = new Smb2LogOffResponsePacket();
                    break;
                case Smb2Command.NEGOTIATE:
                    packet = new Smb2NegotiateResponsePacket();
                    break;
                case Smb2Command.OPLOCK_BREAK:
                    if (smb2Header.MessageId == ulong.MaxValue)
                    {
                        if (!isLeaseBreakPacket)
                        {
                            packet = new Smb2OpLockBreakNotificationPacket();
                        }
                        else
                        {
                            packet = new Smb2LeaseBreakNotificationPacket();
                        }
                    }
                    else
                    {
                        if (!isLeaseBreakPacket)
                        {
                            packet = new Smb2OpLockBreakResponsePacket();
                        }
                        else
                        {
                            packet = new Smb2LeaseBreakResponsePacket();
                        }
                    }
                    break;
                case Smb2Command.QUERY_DIRECTORY:
                    packet = new Smb2QueryDirectoryResponePacket();
                    break;
                case Smb2Command.QUERY_INFO:
                    packet = new Smb2QueryInfoResponsePacket();
                    break;
                case Smb2Command.READ:
                    packet = new Smb2ReadResponsePacket();
                    break;
                case Smb2Command.SESSION_SETUP:
                    packet = new Smb2SessionSetupResponsePacket();
                    ((Smb2SessionSetupResponsePacket)packet).MessageBytes = messageBytes;
                    break;
                case Smb2Command.SET_INFO:
                    packet = new Smb2SetInfoResponsePacket();
                    break;
                case Smb2Command.TREE_CONNECT:
                    packet = new Smb2TreeConnectResponsePacket();
                    break;
                case Smb2Command.TREE_DISCONNECT:
                    packet = new Smb2TreeDisconnectResponsePacket();
                    break;
                case Smb2Command.WRITE:
                    packet = new Smb2WriteResponsePacket();
                    break;
                default:
                    throw new InvalidOperationException("Received an unknown packet! the type of the packet is "
                        + smb2Header.Command.ToString());
            }

            if (IsErrorPacket(smb2Header))
            {
                var error = new Smb2ErrorResponsePacket();
                error.FromBytes(messageBytes, out consumedLength, out expectedLength);

                packet.Header = error.Header;
                packet.Error = error;
            }
            else
            {
                packet.FromBytes(messageBytes, out consumedLength, out expectedLength);
            }

            return packet;
        }

        /// <summary>
        /// Get version information
        /// </summary>
        /// <param name="message">The received message, the array must be 4 bytes, else it will throw exception</param>
        /// <returns></returns>
        private static SmbVersion DecodeVersion(byte[] message)
        {
            //This field MUST be the 4-byte header (0xFF/0xFE/0xFD, S, M, B) with the letters represented by their ASCII characters in network byte order
            if (message[1] == 'S' && message[2] == 'M' && message[3] == 'B')
            {
                if (message[0] == 0xFF) // ProtocolId of SMB Packet Header 
                {
                    return SmbVersion.Version1;
                }
                else if (message[0] == 0xFE) // ProtocolId of SMB2 Packet Header
                {
                    return SmbVersion.Version2;
                }
                else if (message[0] == 0xFD) // ProtocolId of SMB2 TRANSFORM_HEADER
                {
                    return SmbVersion.Version2Encrypted;
                }
                else if (message[0] == 0xFC) // ProtocolId of SMB2 COMPRESSION_TRANSFORM_HEADER
                {
                    return SmbVersion.Version2Compressed;
                }
                else
                {
                    throw new FormatException("The packet is not a valid smb or smb2 packet");
                }
            }
            else
            {
                throw new FormatException("The packet is not a valid smb or smb2 packet");
            }
        }


        /// <summary>
        /// Test if the packet is a Smb2ErrorPacket
        /// </summary>
        /// <param name="header">The header of the packet</param>
        /// <returns>True if it is a Smb2ErrorPacket, otherwise return false</returns>       
        [SuppressMessage("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        private bool IsErrorPacket(Packet_Header header)
        {
            switch (header.Status)
            {
                case Smb2Status.STATUS_SUCCESS:
                    return false;

                case Smb2Status.STATUS_MORE_PROCESSING_REQUIRED:
                    return (header.Command != Smb2Command.SESSION_SETUP) ? true : false;

                case Smb2Status.STATUS_BUFFER_OVERFLOW:
                    // STATUS_BUFFER_OVERFLOW is not an error packet because:
                    // (1) The Sev bit is STATUS_SEVERITY_WARNING, not error (According to [MS-ERREF] 2.3 NTSTATUS.)
                    // (2) It contains response data in its payload data.
                    return false;

                case Smb2Status.STATUS_NOTIFY_ENUM_DIR:
                case Smb2Status.STATUS_NOTIFY_CLEANUP:
                    return header.Command != Smb2Command.CHANGE_NOTIFY;

                default:
                    //other error response like STATUS_INVALID_PARAMETER
                    return true;
            }
        }

        /// <summary>
        /// Verify the signature of a Smb2SinglePacket
        /// </summary>
        /// <param name="packet">The packet to be verified</param>
        /// <param name="cryptoInfo">The cryptoInfo of smb2client</param>        
        /// <returns>True when signature verification succeeds and false when fails</returns>
        private bool VerifySignature(Smb2SinglePacket packet, Smb2CryptoInfo cryptoInfo, byte[] messageBytes)
        {
            if (cryptoInfo.DisableVerifySignature)
            {
                // Skip the verification.
                return true;
            }

            try
            {
                if (IsErrorPacket(packet.Header))
                {
                    packet = packet.Error;
                }

                byte[] bytesToCompute = messageBytes;
                // Zero out the 16-byte signature field in the SMB2 Header of the incoming message.
                Array.Clear(bytesToCompute, System.Runtime.InteropServices.Marshal.SizeOf(packet.Header) - Smb2Consts.SignatureSize, Smb2Consts.SignatureSize);

                //Compute the message with signing key  
                byte[] computedSignature = null;
                if (Smb2Utility.IsSmb3xFamily(cryptoInfo.Dialect))
                {
                    //[MS-SMB2] 3.1.5.1   
                    //If Session.Connection.Dialect belongs to the SMB 3.x dialect family, 
                    //the receiver MUST compute a 16-byte hash by using AES-128-CMAC over the entire message, 
                    //beginning with the SMB2 Header from step 2, and using the key provided. 
                    //The AES-128-CMAC is specified in [RFC4493].

                    //TD has mentioned to use Session.SigningKey for SESSION_SETUP Response and Channel.SigningKey for other responses
                    //In the current SDK, the SigningKey is the Channel.SigningKey
                    computedSignature = AesCmac128.ComputeHash(cryptoInfo.SigningKey, bytesToCompute);
                }
                else
                {
                    //[MS-SMB2] 3.1.5.1   
                    //If Session.Connection.Dialect is "2.002" or "2.100", the receiver MUST compute a 32-byte hash by using HMAC-SHA256 over the entire message, 
                    //beginning with the SMB2 Header from step 2, and using the key provided.
                    //The HMAC-SHA256 hash is specified in [FIPS180-2] and [RFC2104].  

                    HMACSHA256 hmacSha = new HMACSHA256(cryptoInfo.SigningKey);
                    computedSignature = hmacSha.ComputeHash(bytesToCompute);
                }

                //[MS-SMB2] 3.1.5.1   
                //If the first 16 bytes (the high-order portion) of the computed signature from step 3 or step 4 matches the saved signature from step 1, the message is signed correctly
                // compare the first 16 bytes of the originalSignature and computedSignature
                return packet.Header.Signature.SequenceEqual(computedSignature.Take(Smb2Consts.SignatureSize));
            }
            catch (Exception ex)
            {
                throw new Exception("Error happened during signature verification of packet: " + packet.ToString() + ". Exception message: " + ex.Message);
            }
        }

        private void TryVerifySignature(Smb2SinglePacket singlePacket, ulong sessionId, byte[] messageBytes)
        {
            // [MS-SMB2] 3.2.5.1.3             
            //If the MessageId is 0xFFFFFFFFFFFFFFFF, no verification is necessary.
            if (singlePacket.Header.MessageId == 0xFFFFFFFFFFFFFFFF)
            {
                return;
            }

            Smb2CryptoInfo cryptoInfo = null;
            //[MS-SMB2] 3.2.5.1.3
            //If the SMB2 header of the response has SMB2_FLAGS_SIGNED set in the Flags field, the client MUST verify the signature
            if (singlePacket.Header.Flags.HasFlag(Packet_Header_Flags_Values.FLAGS_SIGNED))
            {
                //This is for the session binding success situation.
                //After the session setup success, the cryptoInfoTable will be updated with new signingkey
                //The new signingkey is not in the cryptoInfoTable now. Cannot verify the signature before the table is updated.   

                if (cryptoInfoTable.TryGetValue(sessionId, out cryptoInfo))
                {
                    if (!VerifySignature(singlePacket, cryptoInfo, messageBytes))
                    {
                        //If signature verification fails, the client MUST discard the received message and do no further processing for it.
                        //The client MAY also choose to disconnect the connection.
                        //Throw exception here.                        
                        throw new InvalidOperationException("Incorrect signed packet: " + singlePacket.ToString());
                    }
                }
            }
        }

        /// <summary>
        /// Try to verify the signature of a singlePacket which is not a Session Setup Response.
        /// </summary>
        private void TryVerifySignatureExceptSessionSetupResponse(Smb2SinglePacket singlePacket, ulong sessionId, byte[] messageBytes)
        {
            if (singlePacket.Header.Command == Smb2Command.SESSION_SETUP)
            {
                return;
            }

            TryVerifySignature(singlePacket, sessionId, messageBytes);
        }

        /// <summary>
        /// Try to verify the signature of a singlePacket which is a Session Setup Response.
        /// </summary>
        public void TryVerifySessionSetupResponseSignature(Smb2SinglePacket singlePacket, ulong sessionId, byte[] messageBytes)
        {
            if (singlePacket.Header.Command != Smb2Command.SESSION_SETUP)
            {
                return;
            }

            TryVerifySignature(singlePacket, sessionId, messageBytes);
        }


        /// <summary>
        /// Verify the signature of a Smb2CompoundPacket
        /// </summary>
        /// <param name="packet">The compound packet to be verified</param>        
        private void TryVerifySignature(Smb2CompoundPacket packet, byte[] messageBytes)
        {
            try
            {
                ulong firstSessionId = packet.Packets[0].Header.SessionId;

                uint offset = 0;
                for (int i = 0; i < packet.Packets.Count; i++)
                {
                    Smb2SinglePacket singlePacket = packet.Packets[i];
                    // NextCommand is the offset, in bytes, from the beginning of this SMB2 header to the start of the subsequent 8-byte aligned SMB2 header. 
                    uint packetLen = singlePacket.Header.NextCommand != 0 ? singlePacket.Header.NextCommand : (uint)(messageBytes.Length - offset);
                    byte[] packetBytes = new byte[packetLen];
                    Array.Copy(messageBytes, offset, packetBytes, 0, packetLen);
                    offset += packetLen;

                    // For Related operations, the sessionId is in the first packet of the compound packet.
                    ulong sessionId = singlePacket.Header.Flags.HasFlag(Packet_Header_Flags_Values.FLAGS_RELATED_OPERATIONS) ? firstSessionId : singlePacket.Header.SessionId;

                    TryVerifySignature(singlePacket, sessionId, packetBytes);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error happened during signature verification of compound packet: " + packet.ToString() + ". Exception message:" + ex.Message);
            }
        }
    }
}
