// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2
{
    /// <summary>
    /// Smb2Consts contains the const variable used by sdk
    /// </summary>
    internal static class Smb2Consts
    {
        /// <summary>
        /// Used to separate Smb2Event.ExtraInfo
        /// </summary>
        public const string ExtraInfoSeperator = "^SeP^";

        /// <summary>
        /// The max tcp buffer size used by underlying tcp transport
        /// </summary>
        public const int MaxTcpBufferSize = 1024 * 100;

        /// <summary>
        /// The max netbios buffer size used by underlying netbios transport
        /// </summary>
        public const int MaxNetbiosBufferSize = 65535;

        /// <summary>
        /// The max names underlying netbios transport can use.
        /// </summary>
        public const int MaxNames = 30;

        /// <summary>
        /// The max sessions the underlying netbios transport can establish.
        /// </summary>
        public const int MaxSessions = 30;

        /// <summary>
        /// The header length defined in td is always 64 bytes
        /// </summary>
        public const int Smb2HeaderLen = 64;

        /// <summary>
        /// The max client connection server can accept.
        /// </summary>
        public const int MaxConnectionNumer = 30;

        /// <summary>
        /// The ID of smb2 2.1
        /// </summary>
        public const ushort Smb2_1Dialect = 0x0210;

        /// <summary>
        /// The offset, in bytes, from the beginning of the SMB2 header to the security buffer
        /// </summary>
        public const ushort SecurityBufferOffsetInNegotiateResponse = 128;

        /// <summary>
        /// The default credit server will grand is 1;
        /// </summary>
        public const ushort DefaultCreditResponse = 1;

        /// <summary>
        /// The protocol identifier. The value MUST be (in network order) 0xFE, 'S', 'M', and 'B'.
        /// </summary>
        public static readonly byte[] Smb2ProtocolId = new byte[] { 0xfe, (byte)'S', (byte)'M', (byte)'B' };

        /// <summary>
        /// The signature size for smb2 is always 16 bytes
        /// </summary>
        public const int SignatureSize = 16;

        /// <summary>
        /// The default CreditCharge in Header
        /// </summary>
        public const ushort DefaultCreditCharge = 0;

        /// <summary>
        /// The default creditrequest in header
        /// </summary>
        public const ushort DefaultCreditRequest = 100;

        /// <summary>
        /// The offset, in bytes, from the beginning of the SMB2 header to the security buffer
        /// </summary>
        public const ushort SecurityBufferOffsetInSessionSetup = 72;

        /// <summary>
        /// The securityBufferOffset in negotiate request
        /// </summary>
        public const ushort SecurityBufferOffsetInNegotiateRequest = 88;

        /// <summary>
        /// The protocol version is 2.02
        /// </summary>
        public const uint NegotiateDialect2_02 = 0x0202;

        /// <summary>
        /// The smb2 version 2.002
        /// </summary>
        public const string NegotiateDialect2_02String = "2.002";

        /// <summary>
        /// The protocol version is larger than 2.0
        /// </summary>
        public const uint NegotiateDialect2_XX = 0x02ff;

        /// <summary>
        /// The negotiated smb2 version is 2.100
        /// </summary>
        public const uint NegotiateDialect2_10 = 0x0210;

        /// <summary>
        /// The smb2 version 2.100
        /// </summary>
        public const string NegotiateDialect2_10String = "2.100";

        /// <summary>
        /// The offset, in bytes, of the full share path name from the beginning of the packet header
        /// </summary>
        public const ushort TreeConnectPathOffset = 72;

        /// <summary>
        /// If use Tcp as transport, the message will have 4 byte messageLen information.
        /// </summary>
        public const int TcpPrefixedLenByteCount = 4;

        /// <summary>
        /// The NameOffset value in createRequestPacket if name exist.
        /// </summary>
        public const ushort NameOffsetInCreateRequestPacket = 120;

        /// <summary>
        /// The offset from the beginning of this structure(SMB2_CREATE_CONTEXT Request Values) to its 8-byte aligned name value
        /// </summary>
        public const ushort NameOffsetInCreateContextValues = 16;


        /// <summary>
        /// Open.OperationBuckets[x].Free = TRUE for all x = 0 through 63; 
        /// Open.OperationBuckets[x].SequenceNumber = 0 for all x = 0 through 63.
        /// so the OperationBucketsCount is always 64
        /// </summary>
        public const int OperationBucketsCount = 64;

        /// <summary>
        /// The buffer start index of SMB2 CREATE Response start from header
        /// </summary>
        public const int CreateResponseBufferStartIndex = 152;

        /// <summary>
        /// The buffer start index of SMB2 CREATE request start from header
        /// </summary>
        public const int CreateRequestBufferStartIndex = 120;

        /// <summary>
        /// The buffer start index in 2.2.13.2 SMB2_CREATE_CONTEXT Request Values
        /// </summary>
        public const int CreateContextBufferStartIndex = 16;

        /// <summary>
        /// The offset, in bytes, from the beginning of the SMB2 header to the first
        /// 8-byte aligned SMB2_CREATE_CONTEXT response that is contained in this response.
        /// </summary>
        public const int CreateContextOffsetInCreateResponse = 152;

        /// <summary>
        /// The offset, in bytes, from the beginning of the SMB2 header to the data being written in 
        /// SMB2 WRITE Request
        /// </summary>
        public const ushort DataOffsetInWriteRequest = 112;

        /// <summary>
        /// The offset, in bytes, from the beginning of the header to the data read being returned in this response
        /// in SMB2 READ Response
        /// </summary>
        public const byte DataOffsetInReadResponse = 80;

        /// <summary>
        /// The offset, in bytes, from the beginning of the SMB2 header to the input data buffer in SMB2 IOCTL Request
        /// </summary>
        public const uint InputOffsetInIOCtlRequest = 120;

        /// <summary>
        /// The offset, in bytes, from the beginning of the SMB2 header to the input data buffer in SMB2 IOCTL Response
        /// </summary>
        public const uint InputOffsetInIOCtlResponse = 112;

        /// <summary>
        /// he offset, in bytes, from the beginning of the SMB2 header to the input buffer in SMB2 QUERY_INFO Request
        /// </summary>
        public const ushort InputBufferOffsetInQueryInfoRequest = 104;

        /// <summary>
        /// The offset, in bytes, from the beginning of the SMB2 header to the information to be set in SMB2 SET_INFO Request
        /// </summary>
        public const ushort BufferOffsetInSetInfoRequest = 96;

        /// <summary>
        /// The offset, in bytes, from the beginning of the SMB2 header to the information being returned in SMB2 QUERY_INFO Response
        /// </summary>
        public const ushort OutputBufferOffsetInQueryInfoResponse = 72;

        /// <summary>
        /// The offset, in bytes, from the beginning of the SMB2 header to the search pattern to be used for the enumeration in 
        /// SMB2 QUERY_DIRECTORY Request
        /// </summary>
        public const ushort FileNameOffsetInQueryDirectoryRequest = 96;

        /// <summary>
        /// The offset, in bytes, from the beginning of the SMB2 header to the directory enumeration data being returned 
        /// in SMB2 QUERY_DIRECTORY Response
        /// </summary>
        public const ushort OutputBufferOffsetInQueryDirectoryResponse = 72;

        /// <summary>
        /// The offset, in bytes, from the beginning of the SMB2 header to the change information being returned 
        /// in SMB2 CHANGE_NOTIFY Response
        /// </summary>
        public const ushort OutputBufferOffsetInChangeNotifyResponse = 72;

        /// <summary>
        /// 12 is the size of SubstituteNameOffset, SubstituteNameLength, PrintNameOffset, PrintNameLength, and Flags.
        /// </summary>
        public const int StaticPortionSizeInSymbolicLinkErrorResponse = 12;

        /// <summary>
        /// The count of lockSequence in Smb2ServerOpen
        /// </summary>
        public const int LockSequenceCountInServerOpen = 64;

        /// <summary>
        /// Length of the AES128CCM_Nonce field in SMB2 Transform_Header
        /// </summary>
        public const int AES128CCM_Nonce_Length = 11;

        /// <summary>
        /// Length of the AES128GCM_Nonce field in SMB2 Transform_Header
        /// </summary>
        public const int AES128GCM_Nonce_Length = 12;

        /// <summary>
        /// The ProtocolId used in SMB2 Transform_Header
        /// </summary>
        public const uint ProtocolIdInTransformHeader = 0x424d53FD;

        /// <summary>
        /// The length of preauthentication integrity hash salt value
        /// </summary>
        public const int PreauthIntegrityHashSaltLength = 32;

        /// <summary>
        /// The ProtocolId used in SMB2 Compression Transform Header.
        /// </summary>
        public const uint ProtocolIdInCompressionTransformHeader = 0x424D53FC;
    }
}
