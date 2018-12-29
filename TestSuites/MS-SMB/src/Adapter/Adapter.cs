// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

using Microsoft.Modeling;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb;

using NamespaceCifs = Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Cifs;
using NamespaceFscc = Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Fscc;
using NamespaceSmb = Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb;


namespace Microsoft.Protocol.TestSuites.Smb
{
    /// <summary>
    /// The SMBAdapter class implementation.
    /// </summary>
    /// Disable warning CA1506 because according to Test Case design, excessive class coupling is necessary.
    [SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling")]
    public partial class SmbAdapter : ManagedAdapterBase, ISmbAdapter
    {
        #region Field

        #region Static field

        // Disable warning CA2211 because according to Test Case design, 
        // it is unnecessary to make it non-public or contant.
        [SuppressMessage("Microsoft.Usage", "CA2211:NonConstantFieldsShouldNotBeVisible")]
        public static bool IsQueryQuotaFirstResponse = true;

        // Disable warning CA2211 because according to Test Case design, 
        // it is unnecessary to make it non-public or contant.
        [SuppressMessage("Microsoft.Usage", "CA2211:NonConstantFieldsShouldNotBeVisible")]
        public static ushort TotalReturnedData;

        // Disable warning CA2211 because according to Test Case design, 
        // it is unnecessary to make it non-public or contant.
        [SuppressMessage("Microsoft.Usage", "CA2211:NonConstantFieldsShouldNotBeVisible")]
        public static ushort QuotaSize;

        // Disable warning CA2211 because according to Test Case design, 
        // it is unnecessary to make it non-public or contant.
        [SuppressMessage("Microsoft.Usage", "CA2211:NonConstantFieldsShouldNotBeVisible")]
        public static string FsType = string.Empty;

        // Disable warning CA2211 because according to Test Case design, 
        // it is unnecessary to make it non-public or contant.
        [SuppressMessage("Microsoft.Usage", "CA2211:NonConstantFieldsShouldNotBeVisible")]
        public static ShareType ServiceShareType = ShareType.Disk;

        // Disable warning CA2211 because according to Test Case design, 
        // it is unnecessary to make it non-public or contant.
        [SuppressMessage("Microsoft.Usage", "CA2211:NonConstantFieldsShouldNotBeVisible")]
        protected static bool isSupportStream;

        // Disable warning CA2211 because according to Test Case design, 
        // it is unnecessary to make it non-public or contant.
        [SuppressMessage("Microsoft.Usage", "CA2211:NonConstantFieldsShouldNotBeVisible")]
        protected static bool isSupportInfoLevelPassThru;

        // Disable warning CA2211 because according to Test Case design, 
        // it is unnecessary to make it non-public or contant.
        [SuppressMessage("Microsoft.Usage", "CA2211:NonConstantFieldsShouldNotBeVisible")]
        protected static bool isSupportDfs;

        // Disable warning CA2211 because according to Test Case design, 
        // it is unnecessary to make it non-public or contant.
        [SuppressMessage("Microsoft.Usage", "CA2211:NonConstantFieldsShouldNotBeVisible")]
        protected static bool isSupportNtSmb;

        // Disable warning CA2211 because according to Test Case design, 
        // it is unnecessary to make it non-public or contant.
        [SuppressMessage("Microsoft.Usage", "CA2211:NonConstantFieldsShouldNotBeVisible")]
        protected static bool isMessageModePipe;

        // Disable warning CA2211 because according to Test Case design, 
        // it is unnecessary to make it non-public or contant.
        [SuppressMessage("Microsoft.Usage", "CA2211:NonConstantFieldsShouldNotBeVisible")]
        protected static bool isPreviousVersion;

        // Disable warning CA2211 because according to Test Case design, 
        // it is unnecessary to make it non-public or contant.
        [SuppressMessage("Microsoft.Usage", "CA2211:NonConstantFieldsShouldNotBeVisible")]
        protected static bool isRapServerActive;

        // Disable warning CA2211 because according to Test Case design, 
        // it is unnecessary to make it non-public or contant.
        [SuppressMessage("Microsoft.Usage", "CA2211:NonConstantFieldsShouldNotBeVisible")]
        protected static SignState state;

        // Disable warning CA2211 because according to Test Case design, 
        // it is unnecessary to make it non-public or contant.
        [SuppressMessage("Microsoft.Usage", "CA2211:NonConstantFieldsShouldNotBeVisible")]
        protected static FileSystemType fileSystemType;

        private static byte[] resumeKey;

        #endregion

        #region General field

        public bool IsClosePacket;
        public bool IsReadMode;
        public bool IsBlockMode = true;
        public bool IsReturnSingleEntry;

        private bool checkWindowsImplementation;
        private bool isfindIdFullDirectoryInfo;
        private bool isfinIdBothDirectoryInfo;
        private bool isCopyChunkWrite;

        /// <summary>
        /// The flag indicates that if the test suite is test against a Windows implementation.
        /// </summary>
        private bool isWindows;

        private ushort maxDataCount;
        private ushort dialectIndex;
        private ushort[] informationLevelBytes;
        private ushort smbFileId;

        private int copyWordCount;

        /// <summary>
        /// For messageId add temp, StackSdk bug
        /// </summary>
        private int addMidMark;

        /// <summary>
        /// The quota warning limit, in bytes, for this user. This field is formatted
        /// as a LARGE_INTEGER, as specified in [CIFS] section 2.4.2.
        /// </summary>
        private ulong quotaThreshold = 0x1000;

        /// <summary>
        /// The quota limit, in bytes, for this user. This field is formatted as a LARGE_INTEGER,
        /// as specified in [CIFS] section 2.4.2.
        /// </summary>
        private ulong quotalimit = 0x100;

        private string openedFileName = string.Empty;

        #region FSCC properties

        protected string fsccFSCTLName = string.Empty;
        private int fsccQueryPathLevel = -1;
        private int fsccQueryFSLevel = -1;
        private ushort[] FSCCInformationLevelBytesQueryPath;
        private ushort[] FSCCInformationLevelBytesQueryFS;

        // Fscc query path level value
        private const ushort FSCCInformationLevelQueryPathBasic = 4;
        private const ushort FSCCInformationLevelQueryPathStandard = 5;
        private const ushort FSCCInformationLevelQueryPathInternal = 6;
        private const ushort FSCCInformationLevelQueryPathEa = 7;
        private const ushort FSCCInformationLevelQueryPathAccess = 8;
        private const ushort FSCCInformationLevelQueryPathName = 9;
        private const ushort FSCCInformationLevelQueryPathMode = 16;
        private const ushort FSCCInformationLevelQueryPathAlignment = 17;
        private const ushort FSCCInformationLevelQueryPathAlternateName = 21;
        private const ushort FSCCInformationLevelQueryPathStream = 22;
        private const ushort FSCCInformationLevelQueryPathCompression = 28;
        private const ushort FSCCInformationLevelQueryPathNetworkOpen = 34;
        private const ushort FSCCInformationLevelQueryPathAttributeTag = 35;
        private const ushort FSCCInformationLevelQueryPathPosition = 14;

        //fscc query fs level value
        private const ushort FSCCInformationLevelQueryFSVolume = 258;
        private const ushort FSCCInformationLevelQueryFSSize = 259;
        private const ushort FSCCInformationLevelQueryFSDevice = 260;
        private const ushort FSCCInformationLevelQueryFSAttribute = 261;

        #endregion

        private Dictionary<uint, uint> fId;
        private Dictionary<int, string> gmtTokens;
        private Dictionary<uint, uint> sId;
        private Dictionary<uint, uint> tId;
        private Dictionary<uint, uint> uId;

        private Platform sutOsVersion = Platform.Win2K8R2;
        private Platform clientOsVersion = Platform.Win7;

        /// <summary>
        /// An instance of class SmbClient which mocks the client functionality of SMB. 
        /// </summary>
        private SmbClient smbClientStack;

        /// <summary>
        /// The waiting time to get response from SUT.
        /// </summary>
        private TimeSpan timeout = new TimeSpan();
        private TransportType transport = TransportType.DirectTcp;

        private Microsoft.Modeling.Set<Capabilities> negResponseCap;
        private Microsoft.Modeling.Set<Capabilities> negReturnedCap;
        private Microsoft.Modeling.Set<CreateAction> createActionInternal;

        #endregion

        #region Const field

        private const byte CommandSendMessage = 0xD0;
        private const byte CommandSendStartMbMessage = 0xD5;
        private const byte CommandSendEndMbMessage = 0xD6;
        private const byte CommandSendTextMbMessage = 0xd7;
        private const byte OplockLevelNone = 0x00;
        private const byte OplockLevelExclusive = 0x01;
        private const byte OplockLevelBatch = 0x02;
        private const byte OplockLevel2 = 0x03;
        private const byte SecurityModeUnused = 0xf0;
        private const byte SecurityModeNegotiateSecuritySignatureRequired = 0x08;
        private const byte SecurityModeNegotiateSecuritySignatureEnabled = 0x04;


        private const ushort TreeConnectExtendedInfo = 0x0008;
        private const ushort DeviceCount = 0x00ff;
        private const ushort DeviceReadMode = 0x0100;
        private const ushort DeviceNamePipeTypeByte = 0x0000;
        private const ushort DeviceNamePipeTypeMessage = 0x0001;
        private const ushort DeviceEndPoint = 0x4000;
        private const ushort DeviceBlocking = 0x8000;
        private const ushort DeviceUnused = 0x3a00;
        private const ushort FileTypeDisk = 0x0000;
        private const ushort FileTypeByteModePipe = 0x0001;
        private const ushort FileTypeMessageModePipe = 0x0002;
        private const ushort FileTypePrinter = 0x0003;
        private const ushort FileTypeCommDevice = 0x0004;
        private const ushort FileTypeUnknown = 0xffff;
        private const ushort FileLength = 1000;
        private const ushort FileNoReparseTag = 0x0004;
        private const ushort FileNoSubStreams = 0x0002;
        private const ushort FileNoEas = 0x0001;
        private const ushort FileUnused = 0xfff8;
        private const ushort FileAttrUnused = 0xffc0;
        private const ushort FileAttrReadOnly = 0x01;
        private const ushort FileAttrHidden = 0x02;
        private const ushort FileAttrSystem = 0x04;
        private const ushort FileAttrVolumn = 0x08;
        private const ushort FileAttrDir = 0x10;
        private const ushort FileAttrArchive = 0x20;
        private const ushort FileStatusReparseSub = 0x0006;
        private const ushort FileStatusReparseNoEas = 0x0005;
        private const ushort FileStatusSubNoEas = 0x0003;
        private const ushort FielStatusReSubNoEas = 0x0007;
        private const ushort InformationLevelStanderd = 1;
        private const ushort InformationLevelQueryFsAttribute = 0x105;
        private const ushort InformationLevelQueryFileStreamInfo = 0x109;
        private const ushort InformationLevelSetFileBasicInfo = 0x101;
        private const ushort InformationLevelQueryFileAllocationInfo = 0x105;
        private const ushort InformationLevelQueryFileEndOfFileInfo = 0x106;
        private const ushort InformationLevelFindFileBothDirInfo = 0x104;
        private const ushort InformationLevelFindFileIDFullDirInfo = 0x105;
        private const ushort InformationLevelFindFileIDBothDirInfo = 0x106;
        private const ushort InformationLevelFileAccessInfo = 8;
        private const ushort InformationLevelFileLinkInfo = 11;
        private const ushort InformationLevelFileRenameInfo = 10;
        private const ushort InformationLevelFileAllocationInfo = 19;
        private const ushort InformationLevelFileFsSizeInfo = 3;
        private const ushort InformationLevelFileFsControlInfo = 6;
        private const ushort InformationLevelInvalid = ushort.MaxValue;
        private const ushort InformationLevelPathThrouth = 1000;
        private const ushort OptionalSupportCscMaskNoCaching = 0x000c;
        private const ushort OptionalSupportCscMaskCacheAutoReint = 0x0004;
        private const ushort OptionalSupportCscMaskCacheVDO = 0x0008;
        private const ushort OptionalSupportUnused = 0xffc0;
        private const ushort OpenQueryInformation = 0x0001;
        private const ushort OactOpened = 0x0001;
        private const ushort OactCreated = 0x0002;
        private const ushort OactTruncated = 0x0003;
        private const ushort OpOpened = 0x8001;
        private const ushort OpCreated = 0x8002;
        private const ushort OpTruncated = 0x8003;
        private const ushort OptionalSupportSearchBits = 0x0001;
        private const ushort OptionalSupportInDfs = 0x0002;
        private const ushort OptionalSupportMask1 = 0x0004;
        private const ushort OptionalSupportMask2 = 0x0008;
        private const ushort OptionalSupportFileName = 0x0010;
        private const ushort OptionalSupportExSig = 0x0020;
        private const ushort SmbSupportSearchBits = 0x0001;
        private const ushort SmbCscMask = 0x000C;
        private const ushort SmbUniqueFileName = 0x0010;
        private const ushort SmbExtendedSignatures = 0x0020;
        private const ushort Unicode = 0x8000;

        /// <summary>
        /// If set 0x0002, this share is managed by Distributed File System (DFS), as specified in [MS-DFSC].
        /// </summary>
        private const ushort SmbShareIsInDfs = 0x0002;

        /// <summary>
        /// This field MUST be in the range of 0 to 9. The larger value indicates the higher priority.
        /// </summary>
        private const ushort Priority = 5;

        /// <summary>
        /// The index of the dialect selected by the server from the list presented in the request. 
        /// Dialect entries are numbered starting with zero, so a DialectIndex value of zero indicates 
        /// the first entry in the list. 
        /// If the server does not support any of the listed dialects, it MUST return a DialectIndex of 0XFFFF.
        /// </summary>
        private const ushort ServerNotSupportDialectIndex = 0xffff;

        /// <summary>
        /// If NT LAN Manager or later version is negotiated for the SMB dialect, the DialectIndex value is 5.
        /// </summary>
        private const ushort NtManagerNegotiated = 5;

        /// <summary>
        /// The size of transaction data bytes sent in this SMB message.
        /// </summary>
        private const ushort DataCount = 48;

        /// <summary>
        /// The identifier of the connection to get.
        /// </summary>
        private const int ConnectionId = 0;

        /// <summary>
        /// The MaxData size is too small that the server will return InvalidParameter.
        /// </summary>
        private const int VerySmall = 0;

        /// <summary>
        /// The MaxData size is larger than 0x000F (15) and smaller than the total size of snapshots in server side.
        /// </summary>
        private const int Mid = 0x0010;

        /// <summary>
        /// The MaxData size is larger than the total size of snapshots in server side.
        /// </summary>
        private const int VeryLarge = 0x1000;
        private const int Flags2SecuritySignature = 0x0004;
        private const int Flags2Unused = 0x03A0;
        private const int Flags2NtStatus = 0x4000;

        private const uint CapabilitesUnused = 0x1d7e0c00;
        private const uint CapabilitedExtendedSecurity = 0x80000000;
        private const uint Flags2Unicode = 0x8000;
        private const uint AttributeUnused = 0xff007e00;
        private const uint ExtendedAttributeUnused = 0x0048;
        private const uint CreateActionSuperseded = 0x0000;
        private const uint CreateActionOpened = 0x0001;
        private const uint CreateActionCreated = 0x0002;
        private const uint CreateActionOverwritten = 0x0003;
        private const uint CreateActionExists = 0x0004;
        private const uint CreateActionNotExists = 0x0005;
        private const uint SizeOfInfoData = 40;

        private const uint FilePipePrinterAcess =
            0x00000001 | 0x00000002 | 0x00000004 | 0x00000008 | 0x00000010 | 0x00000020
            | 0x00000080 | 0x00000100 | 0x000100000 | 0x00020000 | 0x00040000 | 0x00080000
            | 0x0010000 | 0x01000000 | 0x02000000 | 0x10000000 | 0x20000000 | 0x40000000 | 0x80000000;

        /// <summary>
        /// This value is all the flags of DIRECTORY_ACCESS_MASK is set.
        /// </summary>
        private const uint DirectoryAccessMask =
            0x00000001 | 0x00000002 | 0x00000004 | 0x00000008 | 0x00000010 | 0x00000020
            | 0x00000040 | 0x00000080 | 0x00000100 | 0x00010000 | 0x00020000 | 0x00040000
            | 0x00080000 | 0x00100000 | 0x01000000 | 0x02000000 | 0x10000000 | 0x20000000
            | 0x40000000 | 0x80000000;

        private const uint AccessMask = FilePipePrinterAcess | DirectoryAccessMask;

        /// <summary>
        /// The field is used for splitting string.
        /// </summary>
        private const char Semicolon = ';';
        private const char Comma = ',';

        /// <summary>
        /// The server resume key for a source file.
        /// </summary>
        private const string CopyChunkResumeKey = "Key2";

        private const string Period = @".";

        /// <summary>
        /// The type of the shared resource to which the TID is connected. 
        /// The Service field MUST be encoded as a null-terminated array of OEM characters, even if the client 
        /// and server have negotiated to use Unicode strings. 
        /// The valid values for this field are as following Service String Descriptions: "A:" Disk Share,
        /// "LPT1:": Printer Share, "IPC": Named Pipe, "COMM": Serial Communications device.
        /// </summary>
        private const string Disk = "A:\0";
        private const string Printer = "LPT1:\0";
        private const string NamePipe = "IPC\0";
        private const string CommunicationsDevice = "COMM\0";

        /// <summary>
        /// All the flags of Capabilities of SMB is listed in the following equation.
        /// </summary>
        private const NamespaceSmb.Capabilities CapabilitiesAllSet =
            NamespaceSmb.Capabilities.CAP_RAW_MODE
            | NamespaceSmb.Capabilities.CAP_MPX_MODE
            | NamespaceSmb.Capabilities.CAP_UNICODE
            | NamespaceSmb.Capabilities.CAP_LARGE_FILES
            | NamespaceSmb.Capabilities.CAP_NT_SMBS
            | NamespaceSmb.Capabilities.CAP_RPC_REMOTE_APIS
            | NamespaceSmb.Capabilities.CAP_STATUS32
            | NamespaceSmb.Capabilities.CAP_LEVEL_II_OPLOCKS
            | NamespaceSmb.Capabilities.CAP_LOCK_AND_READ
            | NamespaceSmb.Capabilities.CAP_NT_FIND
            | NamespaceSmb.Capabilities.CAP_DFS
            | NamespaceSmb.Capabilities.CAP_INFOLEVEL_PASSTHRU
            | NamespaceSmb.Capabilities.CAP_LARGE_READX
            | NamespaceSmb.Capabilities.CAP_LARGE_WRITE
            | NamespaceSmb.Capabilities.CAP_LWIO
            | NamespaceSmb.Capabilities.CAP_UNIX
            | NamespaceSmb.Capabilities.CAP_COMPRESSED_DATA
            | NamespaceSmb.Capabilities.CAP_DYNAMIC_REAUTH
            | NamespaceSmb.Capabilities.CAP_PERSISTENT_HANDLES
            | NamespaceSmb.Capabilities.CAP_EXTENDED_SECURITY;

        // The field is the size of FileRenameInformation_SMB.Reserved,
        // which is used to create Trans2SetFileInformation Request Packet.
        private const uint ReservedSize = 3;

        //The data to write to file used for create SmbWriteAndx Request Packet.
        private const string Data = "Data";

        // The field is the valid GmtToken index.
        private const int ValidgmtTokenIndex = 1;

        #endregion

        #endregion

        #region Event

        /// Disable warning CA1009 because according to Test Case design, 
        /// the two parameters, System.Object and System.EventArgs, are unnecessary.
        [SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly")]
        public event SmbConnectionResponseHandler SmbConnectionResponse;

        /// Disable warning CA1009 because according to Test Case design, 
        /// the two parameters, System.Object and System.EventArgs, are unnecessary.
        [SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly")]
        public event NegotiateResponseHandler NegotiateResponse;

        /// Disable warning CA1009 because according to Test Case design, 
        /// the two parameters, System.Object and System.EventArgs, are unnecessary.
        [SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly")]
        public event SessionSetupResponseHandler SessionSetupResponse;

        /// Disable warning CA1009 because according to Test Case design, 
        /// the two parameters, System.Object and System.EventArgs, are unnecessary.
        [SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly")]
        public event TreeConnectResponseHandler TreeConnectResponse;

        /// Disable warning CA1009 because according to Test Case design, 
        /// the two parameters, System.Object and System.EventArgs, are unnecessary.
        [SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly")]
        public event CreateResponseHandler CreateResponse;

        /// Disable warning CA1009 because according to Test Case design, 
        /// the two parameters, System.Object and System.EventArgs, are unnecessary.
        [SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly")]
        public event ReadResponseHandle ReadResponse;

        /// Disable warning CA1009 because according to Test Case design, 
        /// the two parameters, System.Object and System.EventArgs, are unnecessary.
        [SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly")]
        public event WriteResponseHandle WriteResponse;

        /// Disable warning CA1009 because according to Test Case design, 
        /// the two parameters, System.Object and System.EventArgs, are unnecessary.
        [SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly")]
        public event ErrorWriteResponseHandle ErrorWriteResponse;

        /// Disable warning CA1009 because according to Test Case design, 
        /// the two parameters, System.Object and System.EventArgs, are unnecessary.
        [SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly")]
        public event IsRs2299ImplementedResponseHandle IsRs2299ImplementedResponse;

        /// Disable warning CA1009 because according to Test Case design, 
        /// the two parameters, System.Object and System.EventArgs, are unnecessary.
        [SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly")]
        public event IsRs4984ImplementedResponseHandle IsRs4984ImplementedResponse;

        /// Disable warning CA1009 because according to Test Case design, 
        /// the two parameters, System.Object and System.EventArgs, are unnecessary.
        [SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly")]
        public event ErrorResponseHandler ErrorResponse;

        /// Disable warning CA1009 because according to Test Case design, 
        /// the two parameters, System.Object and System.EventArgs, are unnecessary.
        [SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly")]
        public event ErrorTreeConnectResponseHandler ErrorTreeConnectResponse;

        /// Disable warning CA1009 because according to Test Case design, 
        /// the two parameters, System.Object and System.EventArgs, are unnecessary.
        [SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly")]
        public event ErrorTrans2QueryFileInfoResponseHandler ErrorTrans2QueryFileInfoResponse;

        /// Disable warning CA1009 because according to Test Case design, 
        /// the two parameters, System.Object and System.EventArgs, are unnecessary.
        [SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly")]
        public event ErrorTrans2QueryPathInfoResponseHandler ErrorTrans2QueryPathInfoResponse;

        /// Disable warning CA1009 because according to Test Case design, 
        /// the two parameters, System.Object and System.EventArgs, are unnecessary.
        [SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly")]
        public event NonExtendedNegotiateResponseHandler NonExtendedNegotiateResponse;

        /// Disable warning CA1009 because according to Test Case design, 
        /// the two parameters, System.Object and System.EventArgs, are unnecessary.
        [SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly")]
        public event NonExtendedSessionSetupResponseHandler NonExtendedSessionSetupResponse;

        /// Disable warning CA1009 because according to Test Case design, 
        /// the two parameters, System.Object and System.EventArgs, are unnecessary.
        [SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly")]
        public event Trans2QueryFileInfoResponseHandler Trans2QueryFileInfoResponse;

        /// Disable warning CA1009 because according to Test Case design, 
        /// the two parameters, System.Object and System.EventArgs, are unnecessary.
        [SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly")]
        public event Trans2QueryPathInfoResponseHandler Trans2QueryPathInfoResponse;

        /// Disable warning CA1009 because according to Test Case design, 
        /// the two parameters, System.Object and System.EventArgs, are unnecessary.
        [SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly")]
        public event Trans2QueryFsInfoResponseHandler Trans2QueryFsInfoResponse;

        /// Disable warning CA1009 because according to Test Case design, 
        /// the two parameters, System.Object and System.EventArgs, are unnecessary.
        [SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly")]
        public event Trans2SetFileInfoResponseHandler Trans2SetFileInfoResponse;

        /// Disable warning CA1009 because according to Test Case design, 
        /// the two parameters, System.Object and System.EventArgs, are unnecessary.
        [SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly")]
        public event Trans2SetPathInfoResponseHandler Trans2SetPathInfoResponse;

        /// Disable warning CA1009 because according to Test Case design, 
        /// the two parameters, System.Object and System.EventArgs, are unnecessary.
        [SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly")]
        public event Trans2SetFsInfoResponseHandler Trans2SetFsInfoResponse;

        /// Disable warning CA1009 because according to Test Case design, 
        /// the two parameters, System.Object and System.EventArgs, are unnecessary.
        [SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly")]
        public event Trans2FindFirst2ResponseHandler Trans2FindFirst2Response;

        /// Disable warning CA1009 because according to Test Case design, 
        /// the two parameters, System.Object and System.EventArgs, are unnecessary.
        [SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly")]
        public event Trans2FindNext2ResponseHandler Trans2FindNext2Response;

        /// Disable warning CA1009 because according to Test Case design, 
        /// the two parameters, System.Object and System.EventArgs, are unnecessary.
        [SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly")]
        public event NtTransQueryQuotaResponseHandler NtTransQueryQuotaResponse;

        /// Disable warning CA1009 because according to Test Case design, 
        /// the two parameters, System.Object and System.EventArgs, are unnecessary.
        [SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly")]
        public event NtTransSetQuotaResponseHandler NtTransSetQuotaResponse;

        /// Disable warning CA1009 because according to Test Case design, 
        /// the two parameters, System.Object and System.EventArgs, are unnecessary.
        [SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly")]
        public event FsctlSrvEnumSnapshotsResponseHandler FsctlSrvEnumSnapshotsResponse;

        /// Disable warning CA1009 because according to Test Case design, 
        /// the two parameters, System.Object and System.EventArgs, are unnecessary.
        [SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly")]
        public event FsctlSrvRequestResumeKeyResponseHandler FsctlSrvRequestResumeKeyResponse;

        /// Disable warning CA1009 because according to Test Case design, 
        /// the two parameters, System.Object and System.EventArgs, are unnecessary.
        [SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly")]
        public event FsctlSrvCopyChunkResponseHandler FsctlSrvCopyChunkResponse;

        /// Disable warning CA1009 because according to Test Case design, 
        /// the two parameters, System.Object and System.EventArgs, are unnecessary.
        [SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly")]
        public event NtTransactCreateResponseHandle NtTransactCreateResponse;

        /// Disable warning CA1009 because according to Test Case design, 
        /// the two parameters, System.Object and System.EventArgs, are unnecessary.
        [SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly")]
        public event NtTransSetQuotaResponseAdditionalHandle NtTransSetQuotaResponseAdditional;

        /// Disable warning CA1009 because according to Test Case design, 
        /// the two parameters, System.Object and System.EventArgs, are unnecessary.
        [SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly")]
        public event ErrorNtTransSetQuotaResponseAdditionalHandle ErrorNtTransSetQuotaResponseAdditional;

        /// Disable warning CA1009 because according to Test Case design, 
        /// the two parameters, System.Object and System.EventArgs, are unnecessary.
        [SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly")]
        public event Trans2SetFsInfoResponseAdditionalHandle Trans2SetFsInfoResponseAdditional;

        /// Disable warning CA1009 because according to Test Case design, 
        /// the two parameters, System.Object and System.EventArgs, are unnecessary.
        [SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly")]
        public event ErrorTrans2SetFsInfoResponseAdditionalHandle ErrorTrans2SetFsInfoResponseAdditional;

        /// Disable warning CA1009 because according to Test Case design, 
        /// the two parameters, System.Object and System.EventArgs, are unnecessary.
        [SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly")]
        public event SessionSetupResponseAdditionalHandle SessionSetupResponseAdditional;

        #region FSCC Part

        /// <summary>
        /// NT transact IOCTL response handler.
        /// </summary>
        /// Disable warning CA1009 because according to Test Case design, 
        /// the two parameters, System.Object and System.EventArgs, are unnecessary.
        [SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly")]
        public event FSCCFSCTLNameResponseHandle FSCCFSCTLNameResponse;

        /// <summary>
        /// NT transact2 query path info response handler.
        /// </summary>
        /// Disable warning CA1009 because according to Test Case design, 
        /// the two parameters, System.Object and System.EventArgs, are unnecessary.
        [SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly")]
        public event FSCCTrans2QueryPathInfoResponseHandle FSCCTrans2QueryPathInfoResponse;

        /// <summary>
        /// NT transact2 query fs info response handler.
        /// </summary>
        /// Disable warning CA1009 because according to Test Case design, 
        /// the two parameters, System.Object and System.EventArgs, are unnecessary.
        [SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly")]
        public event FSCCTrans2QueryFSInfoResponseHandle FSCCTrans2QueryFSInfoResponse;

        #endregion

        #endregion

        #region Initialize methods

        /// <summary>
        /// Create the connection for the first test case.
        /// </summary>
        /// <param name="testSite">
        /// An ITestSite interface which provides logging and assertions for test code in its execution context.
        /// </param>
        public override void Initialize(ITestSite testSite)
        {
            base.Initialize(testSite);

            this.maxDataCount = ushort.Parse(Site.Properties["TransportMaxDataCount"]);
            this.checkWindowsImplementation = bool.Parse(Site.Properties["SmbIsFileIdZeroAndIsVolumnGuidZero"]);

            testSite.DefaultProtocolDocShortName = Site.Properties["ProtocolShortName"];
            this.IsClosePacket = false;
            this.transport = TransportType.DirectTcp;

            this.informationLevelBytes = new ushort[16];
            this.informationLevelBytes[0] = InformationLevelStanderd;
            this.informationLevelBytes[1] = InformationLevelQueryFsAttribute;
            this.informationLevelBytes[2] = InformationLevelQueryFileStreamInfo;
            this.informationLevelBytes[3] = InformationLevelSetFileBasicInfo;
            this.informationLevelBytes[4] = InformationLevelQueryFileAllocationInfo;
            this.informationLevelBytes[5] = InformationLevelQueryFileEndOfFileInfo;
            this.informationLevelBytes[6] = InformationLevelFindFileBothDirInfo;
            this.informationLevelBytes[7] = InformationLevelFindFileIDFullDirInfo;
            this.informationLevelBytes[8] = InformationLevelFindFileIDBothDirInfo;
            this.informationLevelBytes[9] = InformationLevelFileAccessInfo;
            this.informationLevelBytes[10] = InformationLevelFileLinkInfo;
            this.informationLevelBytes[11] = InformationLevelFileRenameInfo;
            this.informationLevelBytes[12] = InformationLevelFileAllocationInfo;
            this.informationLevelBytes[13] = InformationLevelFileFsSizeInfo;
            this.informationLevelBytes[14] = InformationLevelFileFsControlInfo;
            this.informationLevelBytes[15] = InformationLevelInvalid;

            #region FSCC properties
            // Fscc information level for query path
            this.FSCCInformationLevelBytesQueryPath = new ushort[14];
            this.FSCCInformationLevelBytesQueryPath[0] = FSCCInformationLevelQueryPathBasic;
            this.FSCCInformationLevelBytesQueryPath[1] = FSCCInformationLevelQueryPathStandard;
            this.FSCCInformationLevelBytesQueryPath[2] = FSCCInformationLevelQueryPathInternal;
            this.FSCCInformationLevelBytesQueryPath[3] = FSCCInformationLevelQueryPathEa;
            this.FSCCInformationLevelBytesQueryPath[4] = FSCCInformationLevelQueryPathAccess;
            this.FSCCInformationLevelBytesQueryPath[5] = FSCCInformationLevelQueryPathName;
            this.FSCCInformationLevelBytesQueryPath[6] = FSCCInformationLevelQueryPathMode;
            this.FSCCInformationLevelBytesQueryPath[7] = FSCCInformationLevelQueryPathAlignment;
            this.FSCCInformationLevelBytesQueryPath[8] = FSCCInformationLevelQueryPathAlternateName;
            this.FSCCInformationLevelBytesQueryPath[9] = FSCCInformationLevelQueryPathStream;
            this.FSCCInformationLevelBytesQueryPath[10] = FSCCInformationLevelQueryPathCompression;
            this.FSCCInformationLevelBytesQueryPath[11] = FSCCInformationLevelQueryPathNetworkOpen;
            this.FSCCInformationLevelBytesQueryPath[12] = FSCCInformationLevelQueryPathAttributeTag;
            this.FSCCInformationLevelBytesQueryPath[13] = FSCCInformationLevelQueryPathPosition;

            // Fscc information level for query fs
            this.FSCCInformationLevelBytesQueryFS = new ushort[7];
            this.FSCCInformationLevelBytesQueryFS[0] = FSCCInformationLevelQueryFSVolume;
            this.FSCCInformationLevelBytesQueryFS[1] = FSCCInformationLevelQueryFSSize;
            this.FSCCInformationLevelBytesQueryFS[2] = FSCCInformationLevelQueryFSDevice;
            this.FSCCInformationLevelBytesQueryFS[3] = FSCCInformationLevelQueryFSAttribute;

            #endregion

            this.createActionInternal = new Microsoft.Modeling.Set<CreateAction>();
            this.negReturnedCap = new Microsoft.Modeling.Set<Capabilities>();
            this.negResponseCap = new Microsoft.Modeling.Set<Capabilities>();
            this.fId = new Dictionary<uint, uint>();
            this.sId = new Dictionary<uint, uint>();
            this.uId = new Dictionary<uint, uint>();
            this.tId = new Dictionary<uint, uint>();
            this.gmtTokens = new Dictionary<int, string>();
            this.timeout = TimeSpan.FromMilliseconds(int.Parse(Site.Properties["SmbTimeoutMillisec"]));

            #region FSCC Properties

            this.fsccFSCTLName = string.Empty;
            this.fsccQueryPathLevel = -1;
            this.fsccQueryFSLevel = -1;

            #endregion
        }


        /// <summary>
        /// Close the files that are opened and disconnect the tree connection in the end of each test case.
        /// </summary>
        /// <param name="disposing">
        /// If disposing equals true, the method has been called directly or indirectly by a user's code. Managed and 
        /// unmanaged resources can be disposed. 
        /// If disposing equals false, the method has been called by the runtime inside the finalizer and only 
        /// unmanaged resources can be disposed.
        /// </param>
        protected override void Dispose(bool disposing)
        {
            if (this.smbClientStack != null)
            {
                this.smbClientStack.Dispose();
            }
            base.Dispose(disposing);
            this.smbClientStack = null;
        }


        /// <summary>
        /// This method is called before each test case runs. User dose not need to call it directly.
        /// </summary>
        public override void Reset()
        {
            base.Reset();
            this.fId.Clear();
            this.sId.Clear();
            this.uId.Clear();
            this.tId.Clear();

            this.negReturnedCap = new Microsoft.Modeling.Set<Capabilities>();
            this.negResponseCap = new Microsoft.Modeling.Set<Capabilities>();
            this.createActionInternal = new Set<CreateAction>();
            // Disconnect the connection.
            if (this.smbClientStack != null)
            {
                this.smbClientStack.Disconnect();
                this.smbClientStack.Dispose();
                this.smbClientStack = null;
            }
        }


        /// <summary>
        /// This method is used to create the SMB client stack connection to get ready for further test.
        /// </summary>
        private void TestStarted()
        {
            this.addMidMark = (int)uint.MinValue;
            this.IsClosePacket = true;
            this.smbClientStack = new SmbClient();
            this.gmtTokens = new Dictionary<int, string>();
            IsQueryQuotaFirstResponse = true;
            this.openedFileName = string.Empty;

            // The server name or server IP address to connect to.
            string sutName = Site.Properties["SutMachineName"];

            // The port of server to connect to.
            int port = int.Parse(Site.Properties["SutPort"]);

            // The buffer size of transport.
            int bufferSize = int.Parse(Site.Properties["SmbTransportBufferSize"]);

            // The IP version to connect to server.
            IpVersion ipVersion = (IpVersion)Enum.Parse(
                typeof(IpVersion),
                Site.Properties["SmbTransportIpVersion"].ToString(),
                false);

            // To set up the TCP connection, and add the connection into context.
            // Exception will be thrown if failed to set up connection with server.
            this.smbClientStack.Connect(sutName, port, ipVersion, bufferSize);
        }

        #endregion

        #region Adapter methods

        #region COM request

        /// <summary>
        /// This method is used to get client and SUT OS information.
        /// </summary>
        public void SmbConnectionRequest()
        {
            string ptfClientOs = Site.Properties["SmbClientOS"].ToString();
            string ptfSutOs = Site.Properties["SutPlatformOS"].ToString();


            if (Enum.Parse(typeof(Platform), ptfClientOs, true) != null)
            {
                this.clientOsVersion = (Platform)Enum.Parse(typeof(Platform), ptfClientOs, true);
            }
            else
            {
                this.clientOsVersion = Platform.NonWindows;
            }

            if (Enum.Parse(typeof(Platform), ptfSutOs, true) != null)
            {
                this.sutOsVersion = (Platform)Enum.Parse(typeof(Platform), ptfSutOs, true);
            }
            else
            {
                this.sutOsVersion = Platform.NonWindows;
            }

            Platform returnSutOsVersion = this.sutOsVersion;
            if (this.sutOsVersion == Platform.NonWindows)
            {
                //Run Win2K8R2 test cases for licensee implementation
                returnSutOsVersion = Platform.Win2K8R2;
            }

            this.SmbConnectionResponse(this.clientOsVersion, returnSutOsVersion);
            this.isWindows = (this.sutOsVersion != Platform.NonWindows);
        }


        /// <summary>
        /// Negotiate request.
        /// </summary>
        /// <param name="messageId">This is used to associate a response with a request.</param>
        /// <param name="isSupportExtSecurity">Indicate whether the client supports extended security.</param>
        /// <param name="clientSignState">
        /// Indicate the sign state of the client: Required, Enabled, Disabled or Disabled Unless Required.
        /// </param>
        /// <param name="dialectName">The input array of dialects.</param>
        /// Disable warning CA1502 because according to Test Case design, excessive complexity is necessary.
        /// Disable warning CA1506 because according to Test Case design, excessive class coupling is necessary.
        [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling")]
        public void NegotiateRequest(
            int messageId,
            bool isSupportExtSecurity,
            SignState clientSignState,
            Sequence<Dialect> dialectName)
        {
            #region Create SMB Connection
            TestStarted();
            #endregion

            #region Create Packet

            SmbNegotiateRequestPacket smbPacket = new SmbNegotiateRequestPacket();
            string[] dialectNameArray = new string[dialectName.Count];
            int i = (int)UInt16.MinValue;
            //fsccIsSupportExtSecurity = isSupportExtSecurity;

            foreach (Dialect name in dialectName)
            {
                switch (name)
                {
                    case Dialect.PcNet1:
                        dialectNameArray[i++] = DialectNameString.PCNET1;
                        break;
                    case Dialect.XenixCore:
                        dialectNameArray[i++] = DialectNameString.XENIXCORE;
                        break;
                    case Dialect.PcLan1:
                        dialectNameArray[i++] = DialectNameString.PCLAN1;
                        break;
                    case Dialect.MsNet103:
                        dialectNameArray[i++] = DialectNameString.MSNET103;
                        break;
                    case Dialect.MsNet30:
                        dialectNameArray[i++] = DialectNameString.MSNET30;
                        break;
                    case Dialect.LanMan10:
                        dialectNameArray[i++] = DialectNameString.LANMAN10;
                        break;
                    case Dialect.Wfw10:
                        dialectNameArray[i++] = DialectNameString.WFW10;
                        break;
                    case Dialect.DosLanMan12:
                        dialectNameArray[i++] = DialectNameString.DOSLANMAN12;
                        break;
                    case Dialect.DosLanMan21:
                        dialectNameArray[i++] = DialectNameString.DOSLANMAN21;
                        break;
                    case Dialect.LanMan12:
                        dialectNameArray[i++] = DialectNameString.LANMAN12;
                        break;
                    case Dialect.LanMan21:
                        dialectNameArray[i++] = DialectNameString.LANMAN21;
                        break;
                    case Dialect.NtLanMan:
                        dialectNameArray[i++] = DialectNameString.NTLANMAN;
                        break;
                    default:
                        dialectNameArray[i++] = string.Empty;
                        break;
                }
            }

            this.smbClientStack.Capability.IsSupportsExtendedSecurity = isSupportExtSecurity;

            NamespaceSmb.SignState signState =
                (NamespaceSmb.SignState)Enum.Parse(
                typeof(NamespaceSmb.SignState),
                clientSignState.ToString(),
                true);

            smbPacket = this.smbClientStack.CreateNegotiateRequest(signState, dialectNameArray);

            #endregion

            #region Send and Receive ExpectPacket

            this.smbClientStack.SendPacket(smbPacket);
            StackPacket response = this.smbClientStack.ExpectPacket(this.timeout);

            NamespaceCifs.SmbPacket smbPacketResponse = (NamespaceCifs.SmbPacket)response;

            this.QueryUidTable(smbPacketResponse);
            this.QueryTidTable(smbPacketResponse);

            VerifyTransport(smbPacketResponse);
            VerifyCommonMessageSyntax(smbPacketResponse);

            if (response.GetType() == typeof(SmbErrorResponsePacket))
            {
                SmbErrorResponsePacket smbErrorResponsePacket = response as SmbErrorResponsePacket;
                NamespaceCifs.SmbHeader smbErrorHeader = smbErrorResponsePacket.SmbHeader;
                this.ErrorResponse(smbErrorHeader.Mid + this.addMidMark, (MessageStatus)smbErrorHeader.Status);
            }
            else
            {
                if (this.smbClientStack.Capability.IsSupportsExtendedSecurity)
                {
                    SmbNegotiateResponsePacket smbNegotiateResponse = (SmbNegotiateResponsePacket)response;

                    NamespaceCifs.SmbHeader negotiateResponseHeader = smbNegotiateResponse.SmbHeader;

                    SMB_COM_NEGOTIATE_NtLanManagerResponse_SMB_Parameters negotiateResponsePayload =
                        smbNegotiateResponse.SmbParameters;

                    if (negotiateResponsePayload.DialectIndex == ServerNotSupportDialectIndex)
                    {
                        this.NegotiateResponse(
                            negotiateResponseHeader.Mid + this.addMidMark,
                            false,
                            false,
                            int.MinValue,
                            new Microsoft.Modeling.Set<Capabilities>(Capabilities.None),
                            (MessageStatus)negotiateResponseHeader.Status);
                    }
                    else
                    {
                        byte securityMode = (byte)negotiateResponsePayload.SecurityMode;
                        bool isSignEnabled = false;
                        bool isSignRequired = false;

                        if ((securityMode & SecurityModeNegotiateSecuritySignatureEnabled) ==
                            SecurityModeNegotiateSecuritySignatureEnabled)
                        {
                            isSignEnabled = true;
                        }

                        if ((securityMode & SecurityModeNegotiateSecuritySignatureRequired) ==
                            SecurityModeNegotiateSecuritySignatureRequired)
                        {
                            isSignRequired = true;
                        }

                        #region Get and Store  Capabilities

                        uint capabilities = (uint)negotiateResponsePayload.Capabilities;
                        if ((capabilities & Convert.ToUInt32(Capabilities.CapUnicode.ToString("D"))) ==
                            Convert.ToUInt32(Capabilities.CapUnicode.ToString("D")))
                        {
                            this.negReturnedCap = this.negReturnedCap.Add(Capabilities.CapUnicode);
                        }

                        if ((capabilities & Convert.ToUInt32(Capabilities.CapStatus32.ToString("D"))) ==
                            Convert.ToUInt32(Capabilities.CapStatus32.ToString("D")))
                        {
                            this.negReturnedCap = this.negReturnedCap.Add(Capabilities.CapStatus32);
                        }

                        if ((capabilities & Convert.ToUInt32(Capabilities.CapDynamicReauth.ToString("D"))) ==
                            Convert.ToUInt32(Capabilities.CapDynamicReauth.ToString("D")))
                        {
                            this.negReturnedCap = this.negReturnedCap.Add(Capabilities.CapDynamicReauth);
                        }

                        if ((capabilities & Convert.ToUInt32(Capabilities.CapLevelIIOplocks.ToString("D"))) ==
                            Convert.ToUInt32(Capabilities.CapLevelIIOplocks.ToString("D")))
                        {
                            this.negReturnedCap = this.negReturnedCap.Add(Capabilities.CapLevelIIOplocks);
                        }

                        if ((capabilities & Convert.ToUInt32(Capabilities.CapNtSmbs.ToString("D"))) ==
                            Convert.ToUInt32(Capabilities.CapNtSmbs.ToString("D")))
                        {
                            this.negResponseCap = this.negResponseCap.Add(Capabilities.CapNtSmbs);
                        }

                        if ((capabilities & Convert.ToUInt32(Capabilities.CapDfs.ToString("D"))) ==
                            Convert.ToUInt32(Capabilities.CapDfs.ToString("D")))
                        {
                            this.negResponseCap = this.negResponseCap.Add(Capabilities.CapDfs);
                        }

                        if ((capabilities & Convert.ToUInt32(Capabilities.CapInfoLevelPassThru.ToString("D"))) ==
                            Convert.ToUInt32(Capabilities.CapInfoLevelPassThru.ToString("D")))
                        {
                            this.negResponseCap = this.negResponseCap.Add(Capabilities.CapInfoLevelPassThru);
                        }

                        if ((capabilities & Convert.ToUInt32(Capabilities.CapExtendedSecurity.ToString("D"))) ==
                            Convert.ToUInt32(Capabilities.CapExtendedSecurity.ToString("D")))
                        {
                            this.negResponseCap = this.negResponseCap.Add(Capabilities.CapExtendedSecurity);
                        }

                        #endregion

                        bool isTokenConfiguredToUsed = Boolean.Parse(Site.Properties["IsTokenConfiguredToUsed"]);
                        bool isReAuthentSupported = Boolean.Parse(Site.Properties["IsReAuthentSupported"]);
                        VerifyMessageSyntaxSmbComNegotiateExtendedSecurityServerResponse(
                            smbNegotiateResponse,
                            isTokenConfiguredToUsed,
                            isReAuthentSupported);

                        bool isNTManagerNegotiated = false;

                        if (smbNegotiateResponse.SmbParameters.DialectIndex == NtManagerNegotiated)
                        {
                            isNTManagerNegotiated = true;
                        }

                        bool isExtendedSecuritySupported = false;

                        if (((int)negotiateResponseHeader.Flags2
                            & (int)SmbHeader_Flags2_Values.SMB_FLAGS2_EXTENDED_SECURITY) ==
                            (int)SmbHeader_Flags2_Values.SMB_FLAGS2_EXTENDED_SECURITY)
                        {
                            isExtendedSecuritySupported = true;
                        }

                        bool isPathContainsLongNames = Boolean.Parse(Site.Properties["IsPathContainsLongNames"]);

                        VerifyMessageSyntaxSMBHeaderExtension(
                            negotiateResponseHeader,
                            isNTManagerNegotiated,
                            isExtendedSecuritySupported,
                            isPathContainsLongNames);

                        this.NegotiateResponse(
                            negotiateResponseHeader.Mid + this.addMidMark,
                            isSignRequired,
                            isSignEnabled,
                            negotiateResponsePayload.DialectIndex,
                            this.negResponseCap,
                            (MessageStatus)negotiateResponseHeader.Status);
                    }
                }
                else
                {
                    SmbNegotiateImplicitNtlmResponsePacket smbNegotiateImplicitNtlmPacket =
                        response as SmbNegotiateImplicitNtlmResponsePacket;

                    NamespaceCifs.SmbHeader negotiateImplicitNtlmResponseHeader =
                        smbNegotiateImplicitNtlmPacket.SmbHeader;

                    NamespaceCifs.SMB_COM_NEGOTIATE_NtLanManagerResponse_SMB_Parameters
                        negotiateImplicitNtlmResponsePayload =
                        smbNegotiateImplicitNtlmPacket.SmbParameters;

                    if (negotiateImplicitNtlmResponsePayload.DialectIndex == ServerNotSupportDialectIndex)
                    {
                        this.NonExtendedNegotiateResponse(
                            negotiateImplicitNtlmResponseHeader.Mid + this.addMidMark,
                            false,
                            false,
                            int.MinValue,
                            new Microsoft.Modeling.Set<Capabilities>(Capabilities.None),
                            (MessageStatus)negotiateImplicitNtlmResponseHeader.Status);
                    }
                    else
                    {
                        byte securityMode = (byte)negotiateImplicitNtlmResponsePayload.SecurityMode;

                        bool isSignEnabled = false;
                        bool isSignRequired = false;

                        if ((securityMode & SecurityModeNegotiateSecuritySignatureEnabled) ==
                            SecurityModeNegotiateSecuritySignatureEnabled)
                        {
                            isSignEnabled = true;
                        }

                        if ((securityMode & SecurityModeNegotiateSecuritySignatureRequired) ==
                            SecurityModeNegotiateSecuritySignatureRequired)
                        {
                            isSignRequired = true;
                        }

                        this.dialectIndex = negotiateImplicitNtlmResponsePayload.DialectIndex;

                        uint capabilities = (uint)negotiateImplicitNtlmResponsePayload.Capabilities;

                        if ((capabilities & Convert.ToUInt32(Capabilities.CapUnicode.ToString("D"))) ==
                            Convert.ToUInt32(Capabilities.CapUnicode.ToString("D")))
                        {
                            this.negReturnedCap = this.negReturnedCap.Add(Capabilities.CapUnicode);
                        }

                        if ((capabilities & Convert.ToUInt32(Capabilities.CapStatus32.ToString("D"))) ==
                            Convert.ToUInt32(Capabilities.CapStatus32.ToString("D")))
                        {
                            this.negReturnedCap = this.negReturnedCap.Add(Capabilities.CapStatus32);
                        }

                        if ((capabilities & Convert.ToUInt32(Capabilities.CapDynamicReauth.ToString("D"))) ==
                            Convert.ToUInt32(Capabilities.CapDynamicReauth.ToString("D")))
                        {
                            this.negReturnedCap = this.negReturnedCap.Add(Capabilities.CapDynamicReauth);
                        }

                        if ((capabilities & Convert.ToUInt32(Capabilities.CapLevelIIOplocks.ToString("D"))) ==
                            Convert.ToUInt32(Capabilities.CapLevelIIOplocks.ToString("D")))
                        {
                            this.negReturnedCap = this.negReturnedCap.Add(Capabilities.CapLevelIIOplocks);
                        }

                        if ((capabilities & Convert.ToUInt32(Capabilities.CapNtSmbs.ToString("D"))) ==
                            Convert.ToUInt32(Capabilities.CapNtSmbs.ToString("D")))
                        {
                            this.negResponseCap = this.negResponseCap.Add(Capabilities.CapNtSmbs);
                        }

                        if ((capabilities & Convert.ToUInt32(Capabilities.CapDfs.ToString("D"))) ==
                            Convert.ToUInt32(Capabilities.CapDfs.ToString("D")))
                        {
                            this.negResponseCap = this.negResponseCap.Add(Capabilities.CapDfs);
                        }

                        if ((capabilities & Convert.ToUInt32(Capabilities.CapInfoLevelPassThru.ToString("D"))) ==
                            Convert.ToUInt32(Capabilities.CapInfoLevelPassThru.ToString("D")))
                        {
                            this.negResponseCap = this.negResponseCap.Add(Capabilities.CapInfoLevelPassThru);
                        }

                        bool isAuthenticationSupported = Boolean.Parse(Site.Properties["IsAuthenticationSupported"]);

                        VerifyMessageSyntaxSmbComNegotiateNonExtendedSecurityServerResponse(
                            smbNegotiateImplicitNtlmPacket,
                            isAuthenticationSupported);

                        this.NonExtendedNegotiateResponse(
                            negotiateImplicitNtlmResponseHeader.Mid + this.addMidMark,
                            isSignRequired,
                            isSignEnabled,
                            this.dialectIndex,
                            this.negResponseCap,
                            (MessageStatus)negotiateImplicitNtlmResponseHeader.Status);
                    }
                }
            }

            #endregion
        }


        /// <summary>
        /// Session setup request.
        /// </summary>
        /// <param name="account">Indicate the account type to establish the session.</param>
        /// <param name="messageId">This is used to associate a response with a request.</param>
        /// <param name="sessionId">
        /// Set this value to 0 to request a new session setup, or set this value to a previously established session 
        /// identifier to request reauthenticate to an existing session.
        /// </param>
        /// <param name="securitySignature">
        /// Delegate the security signature used in session setup request header.
        /// </param>
        /// <param name="isSigned">
        /// Indicate whether the SUT has message signing enabled or required.
        /// </param>
        /// <param name="capabilities">A set of client capabilities.</param>
        /// <param name="isSendBufferSizeExceedMaxBufferSize">
        /// Indicate whether the bufferSize sent by the SUT exceeds the max buffer size or not.
        /// </param>
        /// <param name="isWriteBufferSizeExceedMaxBufferSize">
        /// Indicate whether the bufferSize written by the SUT exceeds the max buffer size or not.
        /// </param>
        /// <param name="flag2">Whether the Flag2 field of the SMB header is valid or not.</param>
        public void SessionSetupRequest(
            AccountType account,
            int messageId,
            int sessionId,
            int securitySignature,
            bool isSigned,
            [Domain("clientCapabilities")] Microsoft.Modeling.Set<Capabilities> capabilities,
            bool isSendBufferSizeExceedMaxBufferSize,
            bool isWriteBufferSizeExceedMaxBufferSize,
            bool flag2)
        {
            #region Create Package

            SmbSessionSetupAndxRequestPacket smbPacket = new SmbSessionSetupAndxRequestPacket();
            string serverName = Site.Properties["SutMachineName"] as string;
            string domainName = Site.Properties["SutLoginDomain"] as string;
            string userName = string.Empty;
            string password = string.Empty;

            if (account == AccountType.Admin)
            {
                userName = Site.Properties["SutLoginAdminUserName"];
                password = Site.Properties["SutLoginAdminPwd"];
            }
            else
            {
                userName = Site.Properties["SutLoginGuestUserName"];
                password = Site.Properties["SutLoginGuestPwd"];
            }

            SmbSecurityPackage smbSecurityPackage = (SmbSecurityPackage)Enum.Parse(
                typeof(SmbSecurityPackage),
                (string)Site.Properties["SmbSecurityPackageType"],
                true);

            // The first step of SessionSetup Request
            if (sessionId == (int)uint.MinValue)
            {
                smbPacket = this.smbClientStack.CreateFirstSessionSetupRequest(
                    smbSecurityPackage,
                    serverName,
                    domainName,
                    userName,
                    password);
            }
            else
            {
                smbPacket = this.smbClientStack.CreateSecondSessionSetupRequest(
                    (ushort)this.uId[(uint)sessionId],
                    smbSecurityPackage);
            }

            #endregion

            #region Send and Receive ExpectPacket

            this.smbClientStack.SendPacket(smbPacket);
            StackPacket response = this.smbClientStack.ExpectPacket(this.timeout);

            NamespaceCifs.SmbPacket smbPacketResponse = (NamespaceCifs.SmbPacket)response;

            ushort uid = this.QueryUidTable(smbPacketResponse);
            this.QueryTidTable(smbPacketResponse);

            VerifyTransport(smbPacketResponse);
            VerifyCommonMessageSyntax(smbPacketResponse);

            if (response.GetType() == typeof(SmbErrorResponsePacket))
            {
                SmbErrorResponsePacket smbErrorResponsePacket = response as SmbErrorResponsePacket;
                NamespaceCifs.SmbHeader smbErrorHeader = smbErrorResponsePacket.SmbHeader;
                this.ErrorResponse(smbErrorHeader.Mid + this.addMidMark, (MessageStatus)smbErrorHeader.Status);
            }
            else
            {
                if (this.smbClientStack.Capability.IsSupportsExtendedSecurity)
                {
                    SmbSessionSetupAndxResponsePacket smbSessionSetupAndXPacket = response
                        as SmbSessionSetupAndxResponsePacket;

                    NamespaceCifs.SmbHeader sessionSetupAndXResponseHeader = smbSessionSetupAndXPacket.SmbHeader;

                    MessageStatus status = (MessageStatus)sessionSetupAndXResponseHeader.Status;

                    VerifyReceiveSmbComSessionSetupAndXRequest(smbPacket.SmbHeader.Uid, smbSessionSetupAndXPacket);

                    bool isGuestAccount = false;
                    if (account == AccountType.Guest)
                    {
                        isGuestAccount = true;
                    }

                    VerifyReceiveSmbComSessionSetupAndXRequest(smbPacket.SmbHeader.Uid, smbSessionSetupAndXPacket);
                    VerifySmbComSessionSetupAndxResponse();

                    if (status == MessageStatus.MoreProcessingRequired)
                    {
                        this.addMidMark--;
                        this.SessionSetupRequest(
                            account,
                            sessionSetupAndXResponseHeader.Mid,
                            uid,
                            securitySignature,
                            isSigned,
                            capabilities,
                            true,
                            true,
                            true);

                        VerifyMessageSyntaxSmbComSessionSetupAndXResponse(smbSessionSetupAndXPacket);
                    }
                    else
                    {
                        int returnSig = (int)uint.MinValue;

                        if (sessionSetupAndXResponseHeader.SecurityFeatures != ulong.MinValue)
                        {
                            returnSig++;
                        }

                        this.SessionSetupResponse(
                            sessionSetupAndXResponseHeader.Mid + this.addMidMark,
                            uid,
                            returnSig,
                            isSigned,
                            isGuestAccount,
                            (MessageStatus)sessionSetupAndXResponseHeader.Status);
                    }
                }
                else
                {
                    SmbSessionSetupImplicitNtlmAndxResponsePacket smbSessionSetupImplicitNtlmPacket =
                        response as SmbSessionSetupImplicitNtlmAndxResponsePacket;

                    NamespaceCifs.SmbHeader sessionSetupImplicitNtlmResponseHeader =
                        smbSessionSetupImplicitNtlmPacket.SmbHeader;

                    bool isGuestAccount = false;

                    if (account == AccountType.Guest)
                    {
                        isGuestAccount = true;
                    }

                    int returnSig = (int)uint.MinValue;

                    if (sessionSetupImplicitNtlmResponseHeader.SecurityFeatures != ulong.MinValue)
                    {
                        returnSig++;
                    }

                    this.NonExtendedSessionSetupResponse(
                        sessionSetupImplicitNtlmResponseHeader.Mid + this.addMidMark,
                        uid,
                        returnSig,
                        isSigned,
                        isGuestAccount,
                        Boolean.Parse(this.Site.Properties["SHOULDMAYR2322Implementation"]),
                        (MessageStatus)sessionSetupImplicitNtlmResponseHeader.Status);
                }
            }

            #endregion
        }


        /// <summary>
        /// Non Extended Security for session setup request.
        /// </summary>
        /// <param name="account">Indicate the account type to establish the session.</param>
        /// <param name="messageId">This is used to associate a response with a request.</param>
        /// <param name="sessionId">
        /// Set this value to 0 to request a new session setup, or set this value to a previously established session 
        /// identifier to request reauthenticate to an existing session.
        /// </param>
        /// <param name="securitySignature">
        /// Delegate the security signature used in session setup request header.
        /// </param>
        /// <param name="isSigned">
        /// Indicate whether the SUT has message signing enabled or required.
        /// </param>
        /// <param name="capabilities">A set of client capabilities. </param>
        /// <param name="isSendBufferSizeExceedMaxBufferSize">
        /// Indicate whether the bufferSize sent by the SUT exceeds the max buffer size or not.
        /// </param>
        /// <param name="isWriteBufferSizeExceedMaxBufferSize">
        /// Indicate whether the bufferSize written by the SUT exceeds the max buffer size or not.
        /// </param>
        /// <param name="flag2">Whether the Flag2 field of the SMB header is valid or not.</param>
        public void NonExtendedSessionSetupRequest(
            AccountType account,
            int messageId,
            int sessionId,
            int securitySignature,
            bool isSigned,
            [Domain("clientCapabilitiesForNonextendedSecurity")] Microsoft.Modeling.Set<Capabilities> capabilities,
            bool isSendBufferSizeExceedMaxBufferSize,
            bool isWriteBufferSizeExceedMaxBufferSize,
            bool flag2)
        {
            #region Create Package

            SmbSessionSetupImplicitNtlmAndxRequestPacket smbPacket = new SmbSessionSetupImplicitNtlmAndxRequestPacket();

            string domain = Site.Properties["SutLoginDomain"];
            string userName;
            string password;

            if (account == AccountType.Admin)
            {
                userName = Site.Properties["SutLoginAdminUserName"];
                password = Site.Properties["SutLoginAdminPwd"];
            }
            else
            {
                userName = Site.Properties["SutLoginGuestUserName"];
                password = Site.Properties["SutLoginGuestPwd"];
            }

            ImplicitNtlmVersion version = (ImplicitNtlmVersion)Enum.Parse(
                typeof(ImplicitNtlmVersion),
                Site.Properties["SmbSecurityPackageNlmpVersion"],
                true);

            ImplicitNtlmVersion implicitNtlmVersion = ImplicitNtlmVersion.PlainTextPassword;

            switch (version)
            {
                case ImplicitNtlmVersion.PlainTextPassword:
                    implicitNtlmVersion = ImplicitNtlmVersion.PlainTextPassword;
                    break;
                case ImplicitNtlmVersion.NtlmVersion1:
                    implicitNtlmVersion = ImplicitNtlmVersion.NtlmVersion1;
                    break;
                case ImplicitNtlmVersion.NtlmVersion2:
                    implicitNtlmVersion = ImplicitNtlmVersion.NtlmVersion2;
                    break;
                default:
                    break;
            }

            smbPacket = this.smbClientStack.CreateSessionSetupImplicitNtlmRequest(
                implicitNtlmVersion,
                domain,
                userName,
                password);

            #endregion

            #region Send and Receive ExpectPacket

            this.smbClientStack.SendPacket(smbPacket);
            StackPacket response = this.smbClientStack.ExpectPacket(this.timeout);

            NamespaceCifs.SmbPacket smbPacketResponse = (NamespaceCifs.SmbPacket)response;

            ushort uid = this.QueryUidTable(smbPacketResponse);
            this.QueryTidTable(smbPacketResponse);

            VerifyTransport(smbPacketResponse);
            VerifyCommonMessageSyntax(smbPacketResponse);

            if (response.GetType() == typeof(SmbErrorResponsePacket))
            {
                SmbErrorResponsePacket smbErrorResponsePacket = response as SmbErrorResponsePacket;
                NamespaceCifs.SmbHeader smbErrorHeader = smbErrorResponsePacket.SmbHeader;
                this.ErrorResponse(smbErrorHeader.Mid + this.addMidMark, (MessageStatus)smbErrorHeader.Status);
            }
            else
            {
                SmbSessionSetupImplicitNtlmAndxResponsePacket smbSessionSetupImplicitNtlmPacket = response
                    as SmbSessionSetupImplicitNtlmAndxResponsePacket;

                NamespaceCifs.SmbHeader sessionSetupImplicitNtlmResponseHeader =
                    smbSessionSetupImplicitNtlmPacket.SmbHeader;

                bool isGuestAccount = false;

                if (account == AccountType.Guest)
                {
                    isGuestAccount = true;
                }

                int returnSig = (int)uint.MinValue;

                if (sessionSetupImplicitNtlmResponseHeader.SecurityFeatures != ulong.MinValue)
                {
                    returnSig++;
                }

                this.NonExtendedSessionSetupResponse(
                    sessionSetupImplicitNtlmResponseHeader.Mid + this.addMidMark,
                    uid,
                    returnSig,
                    isSigned,
                    isGuestAccount,
                    Boolean.Parse(this.Site.Properties["SHOULDMAYR2322Implementation"]),
                    (MessageStatus)sessionSetupImplicitNtlmResponseHeader.Status);
            }

            #endregion
        }


        /// <summary>
        /// Session setup request additional.
        /// </summary>
        /// <param name="account">Indicate the account type to establish the session.</param>
        /// <param name="messageId">This is used to associate a response with a request.</param>
        /// <param name="sessionId">The session identifier.</param>
        /// <param name="securitySignature">
        /// Delegate the security signature used in session setup request header.
        /// </param>
        /// <param name="isRequireSign">
        /// Indicate whether the server has message signing enabled or required.
        /// </param>
        /// <param name="capabilities">A set of client capabilities.</param>
        /// <param name="isSendBufferSizeExceedMaxBufferSize">
        /// Indicate whether the bufferSize sent by the SUT exceeds the max buffer size or not.
        /// </param>
        /// <param name="isWriteBufferSizeExceedMaxBufferSize">
        /// Indicate whether the bufferSize written by the SUT exceeds the max buffer size or not.
        /// </param>
        /// <param name="flag2">Whether the Flag2 field of the SMB header is valid or not.</param>
        /// <param name="isGssTokenValid">Whether the GSS token is valid or not.</param>
        /// <param name="isUserIdValid">Whether the user ID is valid or not.</param>
        public void SessionSetupRequestAdditional(
            AccountType account,
            int messageId,
            int sessionId,
            int securitySignature,
            bool isRequireSign,
            [Domain("clientCapabilities")] Set<Capabilities> capabilities,
            bool isSendBufferSizeExceedMaxBufferSize,
            bool isWriteBufferSizeExceedMaxBufferSize,
            bool flag2,
            bool isGSSTokenValid,
            bool isUserIdValid)
        {
            #region Create Package

            SmbSessionSetupAndxRequestPacket smbPacket = new SmbSessionSetupAndxRequestPacket();

            ushort requestedUid = (ushort)this.uId[(uint)sessionId];
            string serverName = Site.Properties["SutMachineName"] as string;
            string domainName = Site.Properties["SutLoginDomain"] as string;
            string userName = string.Empty;
            string password = string.Empty;

            if (account == AccountType.Admin)
            {
                userName = Site.Properties["SutLoginAdminUserName"];
                password = Site.Properties["SutLoginAdminPwd"];
            }
            else
            {
                userName = Site.Properties["SutLoginGuestUserName"];
                password = Site.Properties["SutLoginGuestPwd"];
            }

            SmbSecurityPackage smbSecurityPackage = (SmbSecurityPackage)Enum.Parse(
                typeof(SmbSecurityPackage),
                Site.Properties["SmbSecurityPackageType"] as string,
                true);

            // Create the session setup request.
            if (sessionId == (int)uint.MinValue && smbSecurityPackage != SmbSecurityPackage.Kerberos)
            {
                smbPacket = this.smbClientStack.CreateFirstSessionSetupRequest(
                    smbSecurityPackage,
                    serverName,
                    domainName,
                    userName,
                    password);
            }
            else
            {
                if (smbSecurityPackage == SmbSecurityPackage.Kerberos)
                {
                    smbPacket = this.smbClientStack.CreateFirstSessionSetupRequest(
                        smbSecurityPackage,
                        serverName,
                        domainName,
                        userName,
                        password);
                }
                else
                {
                    smbPacket = this.smbClientStack.CreateSecondSessionSetupRequest(requestedUid, smbSecurityPackage);
                }

                if (!isUserIdValid)
                {
                    NamespaceCifs.SmbHeader header = smbPacket.SmbHeader;
                    header.Uid = ushort.MaxValue;
                    smbPacket.SmbHeader = header;
                }

                if (!isGSSTokenValid)
                {
                    SMB_COM_SESSION_SETUP_ANDX_Request_SMB_Parameters smbParameters = smbPacket.SmbParameters;
                    smbParameters.SecurityBlobLength = ushort.MinValue;
                    smbPacket.SmbParameters = smbParameters;
                }
            }

            #endregion

            #region Send and Receive ExpectPacket

            this.smbClientStack.SendPacket(smbPacket);
            StackPacket response = this.smbClientStack.ExpectPacket(this.timeout);

            NamespaceCifs.SmbPacket smbPacketResponse = (NamespaceCifs.SmbPacket)response;

            ushort sessionid = this.QueryUidTable(smbPacketResponse);
            this.QueryTidTable(smbPacketResponse);

            VerifyTransport(smbPacketResponse);
            VerifyCommonMessageSyntax(smbPacketResponse);

            if (response.GetType() == typeof(SmbErrorResponsePacket))
            {
                SmbErrorResponsePacket smbErrorResponsePacket = response as SmbErrorResponsePacket;
                NamespaceCifs.SmbHeader smbErrorHeader = smbErrorResponsePacket.SmbHeader;
                this.ErrorResponse(smbErrorHeader.Mid + this.addMidMark, (MessageStatus)smbErrorHeader.Status);
            }
            else
            {
                if (this.smbClientStack.Capability.IsSupportsExtendedSecurity)
                {
                    SmbSessionSetupAndxResponsePacket smbSessionSetupAndXPacket
                    = response as SmbSessionSetupAndxResponsePacket;

                    NamespaceCifs.SmbHeader sessionSetupAndXResponseHeader = smbSessionSetupAndXPacket.SmbHeader;

                    MessageStatus status = (MessageStatus)sessionSetupAndXResponseHeader.Status;

                    VerifyReceiveSmbComSessionSetupAndXRequest(smbPacket.SmbHeader.Uid, smbSessionSetupAndXPacket);

                    bool isGuestAccount = false;
                    if (account == AccountType.Guest)
                    {
                        isGuestAccount = true;
                    }

                    VerifyReceiveSmbComSessionSetupAndXRequest(smbPacket.SmbHeader.Uid, smbSessionSetupAndXPacket);

                    if (status == MessageStatus.MoreProcessingRequired)
                    {
                        this.addMidMark--;
                        this.SessionSetupRequestAdditional(
                            account,
                            sessionSetupAndXResponseHeader.Mid,
                            sessionid,
                            securitySignature,
                            isRequireSign,
                            capabilities,
                            true,
                            true,
                            true,
                            isGSSTokenValid,
                            isUserIdValid);

                        VerifyMessageSyntaxSmbComSessionSetupAndXResponse(smbSessionSetupAndXPacket);
                    }
                    else
                    {
                        int returnSig = (int)uint.MinValue;
                        if (sessionSetupAndXResponseHeader.SecurityFeatures != ulong.MinValue)
                        {
                            returnSig++;
                        }

                        //Workaround temp code (Guest session setup)
                        if (isGuestAccount)
                        {
                            this.ErrorResponse(sessionSetupAndXResponseHeader.Mid + this.addMidMark, MessageStatus.NetworkSessionExpired);
                        }
                        else
                        {
                            this.SessionSetupResponseAdditional(
                                sessionSetupAndXResponseHeader.Mid + this.addMidMark,
                                sessionid,
                                returnSig,
                                smbSessionSetupAndXPacket.IsSignRequired,
                                isGuestAccount,
                                (MessageStatus)sessionSetupAndXResponseHeader.Status);
                        }
                    }
                }
                else
                {
                    SmbSessionSetupImplicitNtlmAndxResponsePacket smbSessionSetupImplicitNtlmPacket
                        = response as SmbSessionSetupImplicitNtlmAndxResponsePacket;

                    NamespaceCifs.SmbHeader sessionSetupImplicitNtlmResponseHeader =
                        smbSessionSetupImplicitNtlmPacket.SmbHeader;

                    bool isGuestAccount = false;
                    if (account == AccountType.Guest)
                    {
                        isGuestAccount = true;
                    }

                    int returnSig = (int)uint.MinValue;
                    if (sessionSetupImplicitNtlmResponseHeader.SecurityFeatures != ulong.MinValue)
                    {
                        returnSig++;
                    }

                    this.NonExtendedSessionSetupResponse(
                        sessionSetupImplicitNtlmResponseHeader.Mid + this.addMidMark,
                        sessionid,
                        returnSig,
                        smbSessionSetupImplicitNtlmPacket.IsSignRequired,
                        isGuestAccount,
                        Boolean.Parse(this.Site.Properties["SHOULDMAYR2322Implementation"]),
                        (MessageStatus)sessionSetupImplicitNtlmResponseHeader.Status);
                }
            }

            #endregion
        }

        /// <summary>
        /// Session setup request with SMB_FLAGS2_UNICODE not set in Flags2.
        /// </summary>
        /// <param name="account">Indicate the account type to establish the session.</param>
        /// <param name="messageId">This is used to associate a response with a request.</param>
        /// <param name="sessionId">
        /// Set this value to 0 to request a new session setup, or set this value to a previously established session 
        /// identifier to request re-authenticatean existing session.
        /// </param>
        /// <param name="securitySignature">
        /// Delegate the security signature used in session setup request header.</param>
        /// <param name="isRequireSign">Indicate whether the message signing is required.</param>
        /// <param name="capabilities">A set of client capabilities. </param>
        /// <param name="isSendBufferSizeExceedMaxBufferSize">
        /// Indicate whether the maximum buffer size for sending can exceed the MaxBufferSize field.
        /// </param>
        /// <param name="isWriteBufferSizeExceedMaxBufferSize">
        /// Indicate whether the maximum buffer size for writing can exceed the MaxBufferSize field.
        /// </param>
        /// <param name="flag2"> This value is ignored by the server and it is used for traditional test.</param>
        public void SessionSetupNonUnicodeRequest(
            AccountType account,
            int messageId,
            int sessionId,
            int securitySignature,
            bool isRequireSign,
            [Domain("clientCapabilities")] Set<Capabilities> capabilities,
            bool isSendBufferSizeExceedMaxBufferSize,
            bool isWriteBufferSizeExceedMaxBufferSize,
            bool flag2)
        {
            #region Create Package

            SmbSessionSetupAndxRequestPacket smbPacket = new SmbSessionSetupAndxRequestPacket();
            string serverName = Site.Properties["SutMachineName"] as string;
            string domainName = Site.Properties["SutLoginDomain"] as string;
            string userName = string.Empty;
            string password = string.Empty;

            if (account == AccountType.Admin)
            {
                userName = Site.Properties["SutLoginAdminUserName"];
                password = Site.Properties["SutLoginAdminPwd"];
            }
            else
            {
                userName = Site.Properties["SutLoginGuestUserName"];
                password = Site.Properties["SutLoginGuestPwd"];
            }

            SmbSecurityPackage smbSecurityPackage = (SmbSecurityPackage)Enum.Parse(
                typeof(SmbSecurityPackage),
                (string)Site.Properties["SmbSecurityPackageType"],
                true);

            // The first step of SessionSetup Request
            if (sessionId == (int)uint.MinValue)
            {
                smbClientStack.Capability.IsUnicode = false;

                smbPacket = this.smbClientStack.CreateFirstSessionSetupRequest(
                    smbSecurityPackage,
                    serverName,
                    domainName,
                    userName,
                    password);
            }
            else
            {
                smbPacket = this.smbClientStack.CreateSecondSessionSetupRequest(
                    (ushort)this.uId[(uint)sessionId],
                    smbSecurityPackage);
            }

            #endregion

            #region Send and Receive ExpectPacket

            this.smbClientStack.SendPacket(smbPacket);
            StackPacket response = this.smbClientStack.ExpectPacket(this.timeout);

            NamespaceCifs.SmbPacket smbPacketResponse = (NamespaceCifs.SmbPacket)response;

            ushort uid = this.QueryUidTable(smbPacketResponse);
            this.QueryTidTable(smbPacketResponse);

            if (response.GetType() == typeof(SmbErrorResponsePacket))
            {
                SmbErrorResponsePacket smbErrorResponsePacket = response as SmbErrorResponsePacket;
                NamespaceCifs.SmbHeader smbErrorHeader = smbErrorResponsePacket.SmbHeader;
                this.ErrorResponse(smbErrorHeader.Mid + this.addMidMark, (MessageStatus)smbErrorHeader.Status);
            }
            else
            {
                if (this.smbClientStack.Capability.IsSupportsExtendedSecurity)
                {
                    SmbSessionSetupAndxResponsePacket smbSessionSetupAndXPacket = response
                        as SmbSessionSetupAndxResponsePacket;

                    NamespaceCifs.SmbHeader sessionSetupAndXResponseHeader = smbSessionSetupAndXPacket.SmbHeader;

                    MessageStatus status = (MessageStatus)sessionSetupAndXResponseHeader.Status;

                    bool isGuestAccount = false;
                    if (account == AccountType.Guest)
                    {
                        isGuestAccount = true;
                    }

                    if (status == MessageStatus.MoreProcessingRequired)
                    {
                        this.addMidMark--;
                        this.SessionSetupNonUnicodeRequest(
                            account,
                            sessionSetupAndXResponseHeader.Mid,
                            uid,
                            securitySignature,
                            isRequireSign,
                            capabilities,
                            true,
                            true,
                            true);

                    }
                    else
                    {
                        int returnSig = (int)uint.MinValue;

                        if (sessionSetupAndXResponseHeader.SecurityFeatures != ulong.MinValue)
                        {
                            returnSig++;
                        }

                        // todo : add non unicode capture code 
                        this.SessionSetupResponse(
                            sessionSetupAndXResponseHeader.Mid + this.addMidMark,
                            uid,
                            returnSig,
                            isRequireSign,
                            isGuestAccount,
                            (MessageStatus)sessionSetupAndXResponseHeader.Status);
                    }
                }
                else
                {
                    SmbSessionSetupImplicitNtlmAndxResponsePacket smbSessionSetupImplicitNtlmPacket =
                        response as SmbSessionSetupImplicitNtlmAndxResponsePacket;

                    NamespaceCifs.SmbHeader sessionSetupImplicitNtlmResponseHeader =
                        smbSessionSetupImplicitNtlmPacket.SmbHeader;

                    bool isGuestAccount = false;

                    if (account == AccountType.Guest)
                    {
                        isGuestAccount = true;
                    }

                    int returnSig = (int)uint.MinValue;

                    if (sessionSetupImplicitNtlmResponseHeader.SecurityFeatures != ulong.MinValue)
                    {
                        returnSig++;
                    }

                    this.NonExtendedSessionSetupResponse(
                        sessionSetupImplicitNtlmResponseHeader.Mid + this.addMidMark,
                        uid,
                        returnSig,
                        isRequireSign,
                        isGuestAccount,
                        Boolean.Parse(this.Site.Properties["SHOULDMAYR2322Implementation"]),
                        (MessageStatus)sessionSetupImplicitNtlmResponseHeader.Status);
                }
            }

            #endregion
        }

        /// <summary>
        /// Close session request
        /// </summary>
        /// <param name="sessionId"> The session Id which the session is being closed.</param>
        public void SessionClose(int sessionId)
        {
        }

        /// <summary>
        /// Tree connect request.
        /// </summary>
        /// <param name="messageId">This is used to associate a response with a request.</param>
        /// <param name="sessionId">Session id. </param>
        /// <param name="isTidDisconnectionSet">Indicate whether the client sets the tid disconnection.</param>
        /// <param name="isRequestExtSignature">Indicate whether the client requests extended signature.</param>
        /// <param name="isRequestExtResponse">
        /// Indicate whether the client requests extended information on Tree connection response.
        /// </param>
        /// <param name="share">The share method.</param>
        /// <param name="shareType">The type of resource the client intends to access.</param>
        /// <param name="isSigned" >
        /// Indicate whether the SUT has message signing enabled or required.
        /// </param>
        public void TreeConnectRequest(
            int messageId,
            int sessionId,
            bool isTidDisconnectionSet,
            bool isRequestExtSignature,
            bool isRequestExtResponse,
            string share,
            ShareType shareType,
            bool isSigned)
        {
            #region Create Packet

            ushort sessionUid = ushort.MinValue;
            if (!this.uId.ContainsKey((uint)sessionId))
            {
                sessionUid = (ushort)sessionId;
            }
            else
            {
                sessionUid = (ushort)this.uId[(uint)sessionId];
            }

            SmbAdapter.ServiceShareType = shareType;
            string shareName = string.Empty;
            switch (shareType)
            {
                case ShareType.NamedPipe:
                    shareName = Site.Properties["SutNamedPipeFullName"];
                    break;
                case ShareType.Printer:
                    shareName = Site.Properties["SutSharePrinterFullName"];
                    break;
                case ShareType.Disk:
                    if (SmbAdapter.FsType == FileSystemType.Ntfs.ToString())
                    {
                        if (share == ShareName.Share1.ToString())
                        {
                            shareName = Site.Properties["SutNtfsShare1FullName"];
                        }
                        else
                        {
                            if (share == ShareName.Share2.ToString())
                            {
                                shareName = Site.Properties["SutNtfsShare2FullName"];
                            }
                            else if (share == ShareName.DfsShare.ToString())
                            {
                                shareName = Site.Properties["SutShareDfsTreeConnect"];
                            }
                            else if (share == ShareName.QuotaShare.ToString())
                            {
                                shareName = Site.Properties["SutNtfsQuotaShareFullName"];
                            }
                        }
                    }
                    else
                    {
                        if (share == ShareName.Share1.ToString())
                        {
                            shareName = Site.Properties["SutFatShare1FullName"];
                        }
                        else
                        {
                            if (share == ShareName.Share2.ToString())
                            {
                                shareName = Site.Properties["SutFatShare2FullName"];
                            }
                            else if (share == ShareName.DfsShare.ToString())
                                shareName = Site.Properties["SutShareDfsTreeConnect"];
                        }
                    }

                    break;
                default:
                    shareName = string.Empty;
                    break;
            }

            SmbTreeConnectAndxRequestPacket smbPacket = new SmbTreeConnectAndxRequestPacket();

            // Create TreeConnect Request.
            smbPacket = this.smbClientStack.CreateTreeConnectRequest(sessionUid, shareName);

            if (isSigned)
            {
                NamespaceCifs.CifsClientPerConnection connection =
                    this.smbClientStack.Context.GetConnection(ConnectionId);

                NamespaceCifs.CifsClientPerSession session =
                    this.smbClientStack.Context.GetSession(ConnectionId, sessionUid);

                smbPacket.Sign(connection.ClientNextSendSequenceNumber, session.SessionKey);
            }

            #endregion

            #region Send and Receive ExpectPacket

            this.smbClientStack.SendPacket(smbPacket);
            StackPacket response = this.smbClientStack.ExpectPacket(this.timeout);

            NamespaceCifs.SmbPacket smbPacketResponse = (NamespaceCifs.SmbPacket)response;

            isSigned = smbPacket.IsSignRequired;
            ushort uid = this.QueryUidTable(smbPacketResponse);
            ushort tid = this.QueryTidTable(smbPacketResponse);

            VerifyTransport(smbPacketResponse);
            VerifyCommonMessageSyntax(smbPacketResponse);

            if (response.GetType() == typeof(SmbErrorResponsePacket))
            {
                SmbErrorResponsePacket smbErrorResponsePacket = response as SmbErrorResponsePacket;
                NamespaceCifs.SmbHeader smbErrorHeader = smbErrorResponsePacket.SmbHeader;

                this.ErrorTreeConnectResponse(
                    smbErrorHeader.Mid + this.addMidMark,
                    (MessageStatus)smbErrorHeader.Status,
                    Boolean.Parse(this.Site.Properties["SHOULDMAYR357Implementation"]));
            }
            else
            {
                SmbTreeConnectAndxResponsePacket smbTreeConnectAndXPacket
                    = response as SmbTreeConnectAndxResponsePacket;

                NamespaceCifs.SmbHeader treeConnectAndXResponseHeader = smbTreeConnectAndXPacket.SmbHeader;

                SMB_COM_TREE_CONNECT_ANDX_Response_SMB_Parameters treeConnectAndXResponsePayload
                    = smbTreeConnectAndXPacket.SmbParameters;

                NamespaceCifs.SMB_COM_TREE_CONNECT_ANDX_Response_SMB_Data treeConnectAndXResponseData
                    = smbTreeConnectAndXPacket.SmbData;

                ShareType serviceShareType = ShareType.CommunicationDevice;
                string shareTypesString = Encoding.ASCII.GetString(treeConnectAndXResponseData.Service);

                if (shareTypesString == Disk)
                {
                    serviceShareType = ShareType.Disk;
                }
                else if (shareTypesString == Printer)
                {
                    serviceShareType = ShareType.Printer;
                }
                else if (shareTypesString == NamePipe)
                {
                    serviceShareType = ShareType.NamedPipe;
                }
                else if (shareTypesString == CommunicationsDevice)
                {
                    serviceShareType = ShareType.CommunicationDevice;
                }

                bool isNotionSupported = Boolean.Parse(Site.Properties["IsNotionSupported"]);
                bool isSHI1005Set = Boolean.Parse(Site.Properties["IsShi1005Set"]);
                bool isNoAliasingSet = Boolean.Parse(Site.Properties["IsNoAliasingSet"]);
                bool isShortFileNameDisabled = Boolean.Parse(Site.Properties["IsShortFileNameDisabled"]);
                int allowedGuestAccess = Int32.Parse(Site.Properties["AllowedGuestAccess"]);

                // If the Flags contains SMB_FLAGS2_EXTENDED_SECURITY (0x0800), 
                // means the server supports extended security
                if ((smbPacket.SmbParameters.Flags & 0x0800) == 0x0800)
                {
                    VerifyMessageSystaxAccessMasksForPrinter(
                    smbTreeConnectAndXPacket,
                    shareType);
                }

                VerifyMessageSyntaxSmbComTreeConnectResponse(
                   smbTreeConnectAndXPacket,
                   isNotionSupported,
                   isSHI1005Set,
                   isNoAliasingSet,
                   isShortFileNameDisabled,
                   allowedGuestAccess,
                   ((smbPacket.SmbParameters.Flags & 0x0008) == 0x0008));

                uint computedMaxRights = (uint)Int32.Parse(Site.Properties["ComputedMaxRights"]);
                ushort computedOptionalSupport = (ushort)Int32.Parse(Site.Properties["ComputedOptionalSupport"]);
                bool isGuestAccountSupported = Boolean.Parse(Site.Properties["IsGuestAccountSupported"]);

                VerifyReceiveSmbComTreeConnectAndXRequest(
                    smbTreeConnectAndXPacket,
                    ((smbPacket.SmbParameters.Flags & 0x0008) == 0x0008),
                    computedMaxRights,
                    computedOptionalSupport,
                    isGuestAccountSupported,
                    isRequestExtSignature);

                bool isSignSignatureZero = (treeConnectAndXResponseHeader.SecurityFeatures == uint.MinValue);
                bool isInDFS = ((treeConnectAndXResponsePayload.OptionalSupport & SmbShareIsInDfs) == SmbShareIsInDfs);

                this.TreeConnectResponse(
                    treeConnectAndXResponseHeader.Mid + this.addMidMark,
                    uid,
                    tid,
                    isSignSignatureZero,
                    serviceShareType,
                    (MessageStatus)treeConnectAndXResponseHeader.Status,
                    isSigned,
                    isInDFS,
                    isRequestExtSignature);
            }

            #endregion
        }

        /// <summary>
        /// Tree multiple connect request
        /// </summary>
        /// <param name="messageId">This is used to associate a response with a request.</param>
        /// <param name="sessionId">The current session ID for this connection.</param>
        /// <param name="isTidDisconnectionSet">Indicate whether the client sets the tid disconnection.</param>
        /// <param name="isRequestExtSignature">Indicate whether the client requests the extended signature.</param>
        /// <param name="isRequestExtResponse">
        /// Indicate whether the client requests the extended information on Tree connection response.
        /// </param>
        /// <param name="share">The share method.</param>
        /// <param name="shareType">The share type client intends to access.</param>
        /// <param name="isSigned">Indicate whether the message is signed or not for this request.</param>
        public void TreeMultipleConnectRequest(
            int messageId,
            int sessionId,
            bool isTidDisconnectionSet,
            bool isRequestExtSignature,
            bool isRequestExtResponse,
            string share,
            ShareType shareType,
            bool isSigned)
        {
            #region Create Packet

            ushort sessionUid = ushort.MinValue;
            if (!this.uId.ContainsKey((uint)sessionId))
            {
                sessionUid = (ushort)sessionId;
            }
            else
            {
                sessionUid = (ushort)this.uId[(uint)sessionId];
            }

            SmbAdapter.ServiceShareType = shareType;
            string shareName = string.Empty;
            switch (shareType)
            {
                case ShareType.NamedPipe:
                    shareName = Site.Properties["SutNamedPipeFullName"];
                    break;
                case ShareType.Printer:
                    shareName = Site.Properties["SutSharePrinterFullName"];
                    break;
                case ShareType.Disk:
                    if (SmbAdapter.FsType == FileSystemType.Ntfs.ToString())
                    {
                        if (share == ShareName.Share1.ToString())
                        {
                            shareName = Site.Properties["SutNtfsShare1FullName"];
                        }
                        else
                        {
                            if (share == ShareName.Share2.ToString())
                            {
                                shareName = Site.Properties["SutNtfsShare2FullName"];
                            }
                            else if (share == ShareName.DfsShare.ToString())
                            {
                                shareName = Site.Properties["SutShareDfsTreeConnect"];
                            }
                        }
                    }
                    else
                    {
                        if (share == ShareName.Share1.ToString())
                        {
                            shareName = Site.Properties["SutFatShare1FullName"];
                        }
                        else
                        {
                            if (share == ShareName.Share2.ToString())
                            {
                                shareName = Site.Properties["SutFatShare2FullName"];
                            }
                            else if (share == ShareName.DfsShare.ToString())
                                shareName = Site.Properties["SutShareDfsTreeConnect"];
                        }
                    }
                    break;
                default:
                    shareName = string.Empty;
                    break;
            }

            SmbTreeConnectAndxRequestPacket smbPacket = new SmbTreeConnectAndxRequestPacket();

            // Create TreeConnect Request.
            smbPacket = this.smbClientStack.CreateTreeConnectRequest(sessionUid, shareName);

            if (isSigned)
            {
                NamespaceCifs.CifsClientPerConnection connection =
                    this.smbClientStack.Context.GetConnection(ConnectionId);

                NamespaceCifs.CifsClientPerSession session =
                    this.smbClientStack.Context.GetSession(ConnectionId, sessionUid);

                smbPacket.Sign(connection.ClientNextSendSequenceNumber, session.SessionKey);
            }

            #endregion

            #region Send and Receive ExpectPacket

            this.smbClientStack.SendPacket(smbPacket);
            StackPacket response = this.smbClientStack.ExpectPacket(this.timeout);

            NamespaceCifs.SmbPacket smbPacketResponse = (NamespaceCifs.SmbPacket)response;

            isSigned = smbPacket.IsSignRequired;
            ushort uid = this.QueryUidTable(smbPacketResponse);
            ushort tid = this.QueryTidTable(smbPacketResponse);

            VerifyTransport(smbPacketResponse);
            VerifyCommonMessageSyntax(smbPacketResponse);

            if (response.GetType() == typeof(SmbErrorResponsePacket))
            {
                SmbErrorResponsePacket smbErrorResponsePacket = response as SmbErrorResponsePacket;
                NamespaceCifs.SmbHeader smbErrorHeader = smbErrorResponsePacket.SmbHeader;

                this.ErrorTreeConnectResponse(
                    smbErrorHeader.Mid + this.addMidMark,
                    (MessageStatus)smbErrorHeader.Status,
                    Boolean.Parse(this.Site.Properties["SHOULDMAYR357Implementation"]));
            }
            else
            {
                SmbTreeConnectAndxResponsePacket smbTreeConnectAndXPacket
                    = response as SmbTreeConnectAndxResponsePacket;

                NamespaceCifs.SmbHeader treeConnectAndXResponseHeader = smbTreeConnectAndXPacket.SmbHeader;

                SMB_COM_TREE_CONNECT_ANDX_Response_SMB_Parameters treeConnectAndXResponsePayload
                    = smbTreeConnectAndXPacket.SmbParameters;

                NamespaceCifs.SMB_COM_TREE_CONNECT_ANDX_Response_SMB_Data treeConnectAndXResponseData
                    = smbTreeConnectAndXPacket.SmbData;

                ShareType serviceShareType = ShareType.CommunicationDevice;
                string shareTypesString = Encoding.ASCII.GetString(treeConnectAndXResponseData.Service);

                if (shareTypesString == Disk)
                {
                    serviceShareType = ShareType.Disk;
                }
                else if (shareTypesString == Printer)
                {
                    serviceShareType = ShareType.Printer;
                }
                else if (shareTypesString == NamePipe)
                {
                    serviceShareType = ShareType.NamedPipe;
                }
                else if (shareTypesString == CommunicationsDevice)
                {
                    serviceShareType = ShareType.CommunicationDevice;
                }

                bool isSignSignatureZero = (treeConnectAndXResponseHeader.SecurityFeatures == uint.MinValue);
                bool isInDFS = ((treeConnectAndXResponsePayload.OptionalSupport & SmbShareIsInDfs) == SmbShareIsInDfs);

                this.TreeConnectResponse(
                    treeConnectAndXResponseHeader.Mid + this.addMidMark,
                    uid,
                    tid,
                    isSignSignatureZero,
                    serviceShareType,
                    (MessageStatus)treeConnectAndXResponseHeader.Status,
                    isSigned,
                    isInDFS,
                    isRequestExtSignature);
            }

            #endregion
        }


        /// <summary>
        /// Create request
        /// </summary>
        /// <param name="messageId">This is used to associate a response with a request.</param>
        /// <param name="sessionId">Set this value to 0 to request a new session setup, or set this value to a 
        /// previously established session identifier to reauthenticate to an existing session.
        /// </param>
        /// <param name="treeId">
        /// This field identifies the subdirectory (or tree) (also referred as a share in this document) on the 
        /// server that the client is accessing.
        /// </param>
        /// <param name="desiredAccess">
        /// The client wants to have access to the SUT. This value must be specified in the ACCESS_MASK 
        /// format.
        /// </param>
        /// <param name="createDisposition">The action to take if a file does or does not exist.</param>
        /// <param name="impersonationLevel">
        /// This field specifies the information given to the server about the client and how the server MUST 
        /// represent, or impersonate, the client.
        /// </param>
        /// <param name="fileName">File name.</param>
        /// <param name="shareType">The type of resource the client intends to access.</param>
        /// <param name="isSigned">
        /// Indicate whether the SUT has message signing enabled or required.
        /// </param>
        /// <param name="isOpenByFileId">
        /// Indicate whether the FILE_OPEN_BY_FILE_ID is set in CreateOptions field of Create Request.
        /// </param>
        /// <param name="isDirectoryFile">
        /// Indicate whether the FILE_DIRECTORY_FILE and FILE_NON_DIRECTORY_FILE are set. If true,FILE_DIRECTORY_FILE 
        /// is set; else FILE_NON_DIRECTORY_FILE is set.
        /// </param>
        /// <param name="isMaximumAllowedSet">Whether the maximum allowed value is set.</param>
        /// Disable warning CA1502 because according to Test Case design, excessive complexity is necessary.
        /// Disable warning CA1506 because according to Test Case design, excessive class coupling is necessary.
        [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling")]
        public void CreateRequest(
            int messageId,
            int sessionId,
            int treeId,
            int desiredAccess,
            CreateDisposition createDisposition,
            [Domain("ImpersonationLevel")] int impersonationLevel,
            string fileName,
            ShareType shareType,
            bool isSigned,
            bool isOpenByFileId,
            bool isDirectoryFile,
            bool isMaximumAllowedSet)
        {
            #region Create Packet

            SmbNtCreateAndxRequestPacket smbPacket = new SmbNtCreateAndxRequestPacket();
            ushort uid = (ushort)this.uId[(uint)sessionId];
            ushort tid = (ushort)this.tId[(uint)treeId];
            string shareName = string.Empty;
            CreateFlags createFlags;
            bool isCreateDirectory = false;

            if (fileName != Site.Properties["SutShareExistFile"].ToString())
            {
                if (createDisposition == CreateDisposition.FileOpenIf)
                {
                    this.createActionInternal = this.createActionInternal.Add(CreateAction.FileCreated);
                    this.createActionInternal = this.createActionInternal.Add(CreateAction.FileDoesNotExist);
                }
            }

            if (shareType == ShareType.NamedPipe)
            {
                if (SmbAdapter.isMessageModePipe)
                {
                    shareName = Site.Properties["SutShareMessagePipe"];
                }
                else
                {
                    shareName = Site.Properties["SutShareStreamPipe"];
                }

                isCreateDirectory = false;
            }
            else
            {
                if (fileName == Site.Properties["SutShareTest1"])
                {
                    shareName = Site.Properties["SutShareTest1"];
                    isCreateDirectory = false;
                }
                else if (fileName == Site.Properties["SutShareTest2"])
                {
                    shareName = Site.Properties["SutShareTest2"];
                    isCreateDirectory = false;
                }
                else if (fileName == Site.Properties["SutShareExistFile"])
                {
                    shareName = Site.Properties["SutShareExistFile"];
                    isCreateDirectory = false;
                }
                else if (fileName.Contains("."))
                {
                    shareName = fileName;
                    isCreateDirectory = false;
                }
                else
                {
                    shareName = fileName;
                    isCreateDirectory = true;
                }
            }

            this.openedFileName = shareName;

            if (isCreateDirectory)
            {
                createFlags = CreateFlags.NT_CREATE_OPEN_TARGET_DIR | CreateFlags.NT_CREATE_REQUEST_EXTENDED_RESPONSE;
            }
            else
            {
                createFlags = CreateFlags.NT_CREATE_REQUEST_EXTENDED_RESPONSE;
            }

            NamespaceCifs.NtTransactShareAccess shareAccess =
                (NamespaceCifs.NtTransactShareAccess)ushort.Parse(Site.Properties["SmbTransportShareAccess"]);

            NamespaceCifs.SMB_EXT_FILE_ATTR extFileAttributes = NamespaceCifs.SMB_EXT_FILE_ATTR.NONE;

            NamespaceCifs.NtTransactCreateDisposition ntCreateDisposition =
                (NamespaceCifs.NtTransactCreateDisposition)createDisposition;

            NtTransactCreateOptions createOption = NtTransactCreateOptions.FILE_DIRECTORY_FILE;
            if (isOpenByFileId && isDirectoryFile)
            {
                createOption =
                    NtTransactCreateOptions.FILE_OPEN_BY_FILE_ID | NtTransactCreateOptions.FILE_DIRECTORY_FILE;
            }

            if (isOpenByFileId && !isDirectoryFile)
            {
                createOption =
                    NtTransactCreateOptions.FILE_NON_DIRECTORY_FILE | NtTransactCreateOptions.FILE_OPEN_BY_FILE_ID;
            }

            if (!isOpenByFileId && isDirectoryFile)
            {
                createOption = NtTransactCreateOptions.FILE_DIRECTORY_FILE;
            }

            if (!isOpenByFileId && !isDirectoryFile)
            {
                createOption = NtTransactCreateOptions.FILE_NON_DIRECTORY_FILE;
            }

            NamespaceCifs.NtTransactDesiredAccess nTDesiredAccess =
                (NamespaceCifs.NtTransactDesiredAccess)desiredAccess;

            if (nTDesiredAccess == (
                NamespaceCifs.NtTransactDesiredAccess.FILE_READ_DATA
                | NamespaceCifs.NtTransactDesiredAccess.FILE_WRITE_DATA))
            {
                nTDesiredAccess = NamespaceCifs.NtTransactDesiredAccess.GENERIC_ALL;
            }

            if (isCreateDirectory)
            {
                nTDesiredAccess = Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Cifs.NtTransactDesiredAccess.FILE_READ_DATA |
                    Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Cifs.NtTransactDesiredAccess.FILE_READ_ATTRIBUTES |
                    Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Cifs.NtTransactDesiredAccess.SYNCHRONIZE;
                shareAccess = NamespaceCifs.NtTransactShareAccess.FILE_SHARE_READ | NamespaceCifs.NtTransactShareAccess.FILE_SHARE_WRITE;
            }

            smbPacket = this.smbClientStack.CreateCreateRequest(
                tid,
                shareName,
                nTDesiredAccess,
                extFileAttributes,
                shareAccess,
                ntCreateDisposition,
                createOption,
                (NamespaceCifs.NtTransactImpersonationLevel)impersonationLevel,
                createFlags);

            if (isSigned)
            {
                NamespaceCifs.CifsClientPerConnection connection =
                    this.smbClientStack.Context.GetConnection(ConnectionId);

                NamespaceCifs.CifsClientPerSession session =
                    this.smbClientStack.Context.GetSession(ConnectionId, uid);

                smbPacket.Sign(connection.ClientNextSendSequenceNumber, session.SessionKey);
            }

            #endregion

            #region Send and Receive ExpectPacket

            this.smbClientStack.SendPacket(smbPacket);
            StackPacket response = this.smbClientStack.ExpectPacket(this.timeout);

            NamespaceCifs.SmbPacket smbPacketResponse = (NamespaceCifs.SmbPacket)response;

            this.QueryUidTable(smbPacketResponse);
            this.QueryTidTable(smbPacketResponse);

            VerifyTransport(smbPacketResponse);
            VerifyCommonMessageSyntax(smbPacketResponse);

            if (response.GetType() == typeof(SmbErrorResponsePacket))
            {
                SmbErrorResponsePacket smbErrorResponsePacket = response as SmbErrorResponsePacket;
                NamespaceCifs.SmbHeader smbErrorHeader = smbErrorResponsePacket.SmbHeader;
                this.ErrorResponse(smbErrorHeader.Mid + this.addMidMark, (MessageStatus)smbErrorHeader.Status);
            }
            else
            {
                SmbNtCreateAndxResponsePacket smbNtCreateAndXPacket = response as SmbNtCreateAndxResponsePacket;
                NamespaceCifs.SmbHeader ntCreateAndXResponseHeader = smbNtCreateAndXPacket.SmbHeader;

                SMB_COM_NT_CREATE_ANDX_Response_SMB_Parameters ntCreateAndXResponsePayload =
                    smbNtCreateAndXPacket.SmbParameters;

                this.smbFileId = ntCreateAndXResponsePayload.FID;

                int fidValue = (int)uint.MinValue;
                if (!this.fId.ContainsValue(ntCreateAndXResponsePayload.FID))
                {
                    int count = this.fId.Count;
                    this.fId.Add((uint)(count), ntCreateAndXResponsePayload.FID);
                    fidValue = count;
                }
                else
                {
                    foreach (uint key in this.fId.Keys)
                    {
                        if (this.fId[key] == ntCreateAndXResponsePayload.FID)
                        {
                            fidValue = (int)key;
                            break;
                        }
                    }
                }

                uint creatAction = ntCreateAndXResponsePayload.CreationAction;
                Microsoft.Modeling.Set<CreateAction> actionSet = new Microsoft.Modeling.Set<CreateAction>();
                if (creatAction == CreateActionSuperseded)
                {
                    actionSet = actionSet.Add(CreateAction.FileSuperseded);
                }

                if ((creatAction & CreateActionExists) == CreateActionExists)
                {
                    actionSet = actionSet.Add(CreateAction.FileExists);
                }

                if ((creatAction & CreateActionOpened) == CreateActionOpened)
                {
                    actionSet = actionSet.Add(CreateAction.FileOpened);
                    if (!actionSet.Contains(CreateAction.FileExists))
                    {
                        actionSet = actionSet.Add(CreateAction.FileExists);
                    }
                }

                if ((creatAction & CreateActionNotExists) == CreateActionNotExists)
                {
                    actionSet = actionSet.Add(CreateAction.FileDoesNotExist);
                }

                if ((creatAction & CreateActionCreated) == CreateActionCreated)
                {
                    actionSet = actionSet.Add(CreateAction.FileCreated);
                    if (!actionSet.Contains(CreateAction.FileDoesNotExist))
                    {
                        actionSet = actionSet.Add(CreateAction.FileDoesNotExist);
                    }
                }

                if ((creatAction & CreateActionOverwritten) == CreateActionOverwritten)
                {
                    actionSet = actionSet.Add(CreateAction.FileOverwritten);
                }

                bool isIdZero = false;
                bool isNoSubStreams = false;
                if (ntCreateAndXResponsePayload.ResourceType == NamespaceCifs.FileTypeValue.FileTypeDisk)
                {
                    if (SmbAdapter.FsType == FileSystemType.Ntfs.ToString())
                    {
                        isNoSubStreams = false;
                    }
                    else
                    {
                        isNoSubStreams = true;
                    }
                }

                if (SmbAdapter.ServiceShareType == ShareType.NamedPipe)
                {
                    isNoSubStreams = true;
                }

                bool isVolumnGuidZero = false;
                if (!this.checkWindowsImplementation)
                {
                    Site.Log.Add(
                        LogEntryKind.Comment,
                        @"isFileIdZero: {0};
                        isVolumnGUIDZero: {1}",
                        isIdZero,
                        isVolumnGuidZero);
                    isIdZero = true;
                    isVolumnGuidZero = true;
                }

                bool isNotionSupportedForCreate = Boolean.Parse(Site.Properties["IsNotionSupportedForCreate"]);
                bool isSerWantCliLeverageNewCap = Boolean.Parse(Site.Properties["IsSerWantCliLeverageNewCap"]);

                VerifyMessageSyntaxSmbComNtCreateAndXResponse(
                    smbNtCreateAndXPacket,
                    isSerWantCliLeverageNewCap);

                VerifyMessageSyntaxSmbComNtCreateAndXResponseForNotNotionSupported(
                    smbNtCreateAndXPacket,
                    isNotionSupportedForCreate);

                VerifyMessageSyntaxFileIdGeneration(smbNtCreateAndXPacket.SmbParameters.FileId);
                VerifyMessageSyntaxVolumeGUIDGeneration(smbNtCreateAndXPacket.SmbParameters.VolumeGUID);

                if (0x00000003 >= (uint)impersonationLevel)
                {
                    VerifyMessageSyntaxNtTransactCreateRequest(impersonationLevel);
                }

                VerifyMessageSyntaxAccessMasks(smbNtCreateAndXPacket, isDirectoryFile);

                uint computedMaximalAccessRights = (uint)Int32.Parse(Site.Properties["ComputedMaximalAccessRights"]);
                uint computedGuestMaximalAccessRights =
                    (uint)Int32.Parse(Site.Properties["ComputedGuestMaximalAccessRights"]);

                VerifyReceiveSmbComNtCreateAndXRequest(
                    (uint)smbPacket.SmbHeader.Flags,
                    smbNtCreateAndXPacket,
                    computedMaximalAccessRights,
                    computedGuestMaximalAccessRights,
                    impersonationLevel == (int)NamespaceCifs.NtTransactImpersonationLevel.SEC_ANONYMOUS ? true : false);

                //Workaround temp code (Invalid impersonation level)
                if ((string.Equals(Site.Properties["SutPlatformOS"], "Win2K8") && impersonationLevel == 4))
                {
                    this.ErrorResponse(ntCreateAndXResponseHeader.Mid + this.addMidMark, MessageStatus.BadImpersonationLevel);
                }
                else
                {
                    if (this.createActionInternal.Count == uint.MinValue)
                    {
                        this.CreateResponse(
                            ntCreateAndXResponseHeader.Mid + this.addMidMark,
                            this.QueryUidTable(smbPacketResponse),
                            this.QueryTidTable(smbPacketResponse),
                            fidValue,
                            smbPacketResponse.IsSignRequired,
                            actionSet,
                            isIdZero,
                            isVolumnGuidZero,
                            (ntCreateAndXResponsePayload.Directory == uint.MinValue),
                            true,
                            isNoSubStreams,
                            (MessageStatus)ntCreateAndXResponseHeader.Status);
                    }
                    else
                    {
                        this.CreateResponse(
                            ntCreateAndXResponseHeader.Mid + this.addMidMark,
                            this.QueryUidTable(smbPacketResponse),
                            this.QueryTidTable(smbPacketResponse),
                            fidValue,
                            (smbPacketResponse).IsSignRequired,
                            this.createActionInternal,
                            isIdZero,
                            isVolumnGuidZero,
                            (ntCreateAndXResponsePayload.Directory == 0),
                            true,
                            isNoSubStreams,
                            (MessageStatus)ntCreateAndXResponseHeader.Status);
                    }
                }
                this.createActionInternal = new Microsoft.Modeling.Set<CreateAction>();
            }

            #endregion
        }


        /// <summary>
        /// Read request.
        /// </summary>
        /// <param name="messageId">This is used to associate a response with a request.</param>
        /// <param name="sessionId">The session identifier.</param>
        /// <param name="treeId">The tree identifier.</param>
        /// <param name="fid">The file identifier.</param>
        /// <param name="shareType">The type of share.</param>
        /// <param name="isSigned">
        /// Indicate whether the SUT has message signing enabled or required.
        /// </param>
        public void ReadRequest(
            int messageId,
            int sessionId,
            int treeId,
            int fid,
            ShareType shareType,
            bool isSigned)
        {
            #region  Create Packet

            SmbReadAndxRequestPacket smbPacket = new SmbReadAndxRequestPacket();
            ushort fileId = this.smbFileId;
            ushort uid = (ushort)this.uId[(uint)sessionId];
            uint offset = (uint)uint.Parse(Site.Properties["SmbTransportReadOffset"]);
            ushort maxCount = (ushort)ushort.Parse(Site.Properties["SmbTransportMaxReadDataCount"]);
            smbPacket = this.smbClientStack.CreateReadRequest(fileId, maxCount, offset);
            if (isSigned)
            {
                NamespaceCifs.CifsClientPerConnection connection =
                    this.smbClientStack.Context.GetConnection(ConnectionId);

                NamespaceCifs.CifsClientPerSession session = this.smbClientStack.Context.GetSession(ConnectionId, uid);

                smbPacket.Sign(connection.ClientNextSendSequenceNumber, session.SessionKey);
            }

            #endregion

            #region Send and Receive ExpectPacket

            this.smbClientStack.SendPacket(smbPacket);
            StackPacket response = this.smbClientStack.ExpectPacket(this.timeout);

            NamespaceCifs.SmbPacket smbPacketResponse = (NamespaceCifs.SmbPacket)response;

            this.QueryUidTable(smbPacketResponse);
            this.QueryTidTable(smbPacketResponse);

            VerifyTransport(smbPacketResponse);
            VerifyCommonMessageSyntax(smbPacketResponse);

            if (response.GetType() == typeof(SmbErrorResponsePacket))
            {
                SmbErrorResponsePacket smbErrorResponsePacket = response as SmbErrorResponsePacket;
                NamespaceCifs.SmbHeader smbErrorHeader = smbErrorResponsePacket.SmbHeader;
                this.ErrorResponse(smbErrorHeader.Mid + this.addMidMark, (MessageStatus)smbErrorHeader.Status);
            }
            else
            {
                SmbReadAndxResponsePacket readAndxResponsePacket = response as SmbReadAndxResponsePacket;
                NamespaceCifs.SmbHeader readAndxResponseHeader = readAndxResponsePacket.SmbHeader;
                int minNumberOfBytesToReturn = int.Parse(Site.Properties["MinNumberOfBytesToReturn"]);
                bool isReadOnPipe = false;
                bool isReadOnFile = false;

                if (shareType == ShareType.NamedPipe)
                {
                    isReadOnPipe = true;
                }
                else if (shareType == ShareType.Disk)
                {
                    isReadOnFile = true;
                }

                VerifyReceiveSmbComReadAndXRequest(
                    readAndxResponsePacket,
                    minNumberOfBytesToReturn,
                    isReadOnPipe,
                    isReadOnFile);

                this.ReadResponse(
                    readAndxResponseHeader.Mid + this.addMidMark,
                    this.QueryUidTable(smbPacketResponse),
                    this.QueryTidTable(smbPacketResponse),
                    (smbPacketResponse).IsSignRequired,
                    true,
                    (MessageStatus)readAndxResponseHeader.Status);
            }

            #endregion
        }


        /// <summary>
        /// Write request.
        /// </summary>
        /// <param name="messageId">This is used to associate a response with a request.</param>
        /// <param name="sessionId">The session identifier.</param>
        /// <param name="treeId">The tree identifier.</param>
        /// <param name="fid">The file identifier.</param>
        /// <param name="shareType">The type of share.</param>
        /// <param name="isSigned">
        /// Indicate whether the SUT has message signing enabled or required.
        /// </param>
        /// <param name="synchronize">The synchronize method.</param>
        public void WriteRequest(
            int messageId,
            int sessionId,
            int treeId,
            int fid,
            ShareType shareType,
            bool isSigned,
            int synchronize)
        {
            #region Create Packet

            SmbWriteAndxRequestPacket smbPacket = new SmbWriteAndxRequestPacket();
            ushort uid = (ushort)this.uId[(uint)sessionId];
            uint offset = uint.Parse(Site.Properties["SmbTransportWriteOffset"]);
            ushort fileId = this.smbFileId;
            byte[] writeData = Encoding.Unicode.GetBytes(Data);

            smbPacket = this.smbClientStack.CreateWriteRequest(fileId, offset, writeData);

            if (isSigned)
            {
                NamespaceCifs.CifsClientPerConnection connection =
                    this.smbClientStack.Context.GetConnection(ConnectionId);

                NamespaceCifs.CifsClientPerSession session = this.smbClientStack.Context.GetSession(ConnectionId, uid);

                smbPacket.Sign(connection.ClientNextSendSequenceNumber, session.SessionKey);
            }

            #endregion

            #region Send and Receive ExpectPacket

            this.smbClientStack.SendPacket(smbPacket);
            StackPacket response = this.smbClientStack.ExpectPacket(this.timeout);

            NamespaceCifs.SmbPacket smbPacketResponse = (NamespaceCifs.SmbPacket)response;

            this.QueryUidTable(smbPacketResponse);
            this.QueryTidTable(smbPacketResponse);

            VerifyTransport(smbPacketResponse);
            VerifyCommonMessageSyntax(smbPacketResponse);

            if (response.GetType() == typeof(SmbErrorResponsePacket))
            {
                SmbErrorResponsePacket smbErrorResponsePacket = response as SmbErrorResponsePacket;
                NamespaceCifs.SmbHeader smbErrorHeader = smbErrorResponsePacket.SmbHeader;
                this.ErrorWriteResponse(
                    smbErrorHeader.Mid + this.addMidMark,
                    (MessageStatus)smbErrorHeader.Status,
                    Boolean.Parse(this.Site.Properties["SHOULDMAYR5229Implementation"]));
            }
            else
            {
                SmbWriteAndxResponsePacket writeAndxResponsePacket = response as SmbWriteAndxResponsePacket;
                VerifyMessageSyntaxSmbComWriteAndXResponse(writeAndxResponsePacket);
                if (this.isCopyChunkWrite)
                {
                    this.copyWordCount = writeAndxResponsePacket.SmbParameters.WordCount;
                }
                else
                {
                    NamespaceCifs.SmbHeader writeAndxResponseHeader = writeAndxResponsePacket.SmbHeader;
                    this.WriteResponse(
                        writeAndxResponseHeader.Mid + this.addMidMark,
                        this.QueryUidTable(smbPacketResponse),
                        this.QueryTidTable(smbPacketResponse),
                        (smbPacketResponse).IsSignRequired,
                        (MessageStatus)writeAndxResponseHeader.Status,
                        true,
                        true,
                        true);
                }
            }

            #endregion
        }

        /// <summary>
        /// IsRS2299Implemented request.
        /// </summary>
        public void IsRs2299ImplementedRequest()
        {
            this.IsRs2299ImplementedResponse(Boolean.Parse(this.Site.Properties["SHOULDMAYR2299Implementation"]));
        }


        /// <summary>
        /// IsRS4984Implemented request.
        /// </summary>
        public void IsRs4984ImplementedRequest()
        {
            this.IsRs4984ImplementedResponse(Boolean.Parse(this.Site.Properties["SHOULDMAYR4984Implementation"]));
        }

        #endregion

        #region Transaction2 & TransactionNT request

        /// <summary>
        /// TRANS2_QUERY_FILE_INFORMATION Request handler.
        /// </summary>
        /// <param name="messageId">This is used to associate a response with a request.</param>
        /// <param name="sessionId">
        /// Set this value to 0 to request a new session setup, or set this value to a previously established session 
        /// identifier to request reauthenticate to an existing session.
        /// </param>
        /// <param name="treeId">
        /// This field identifies the subdirectory (or tree) (also referred as a share in this document) on the 
        /// server that the client is accessing.
        /// </param>
        /// <param name="isSigned">
        /// Indicate whether the SUT has message signing enabled or required.
        /// </param>
        /// <param name="isUsePassthrough">
        /// Indicate whether adding SMB_INFO_PASSTHROUGH in InformationLevel field of the request.
        /// </param>
        /// <param name="informationLevel">This can be used to query information from the server.</param>
        /// <param name="fid">The file identifier.</param>
        /// <param name="reserved">The reserved int value.</param>
        public void Trans2QueryFileInfoRequest(
            int messageId,
            int sessionId,
            int treeId,
            bool isSigned,
            bool isUsePassthrough,
            [Domain("InfoLevelQueriedByFid")] InformationLevel informationLevel,
            int fid,
            int reserved)
        {
            #region Create Packet

            SmbTrans2QueryFileInformationRequestPacket smbPacket = new SmbTrans2QueryFileInformationRequestPacket();
            ushort uid = (ushort)this.uId[(uint)sessionId];

            NamespaceCifs.Trans2SmbParametersFlags transactOptions = NamespaceCifs.Trans2SmbParametersFlags.NONE;
            ushort fileId = (ushort)this.fId[(uint)fid];
            ushort level = this.informationLevelBytes[(ushort)informationLevel];

            this.smbClientStack.Capability.IsUsePassThrough = isUsePassthrough;
            smbPacket = this.smbClientStack.CreateTrans2QueryFileInformationRequest(
                fileId,
                transactOptions,
                this.maxDataCount,
                (NamespaceCifs.QueryInformationLevel)level,
                null);

            if (isSigned)
            {
                NamespaceCifs.CifsClientPerConnection connection = this.smbClientStack.Context.GetConnection(ConnectionId);
                NamespaceCifs.CifsClientPerSession session = this.smbClientStack.Context.GetSession(ConnectionId, uid);

                smbPacket.Sign(connection.ClientNextSendSequenceNumber, session.SessionKey);
            }

            #endregion

            #region Send and Receive ExpectPacket

            this.smbClientStack.SendPacket(smbPacket);
            StackPacket response = this.smbClientStack.ExpectPacket(this.timeout);

            NamespaceCifs.SmbPacket smbPacketResponse = (NamespaceCifs.SmbPacket)response;

            this.QueryUidTable(smbPacketResponse);
            this.QueryTidTable(smbPacketResponse);

            VerifyTransport(smbPacketResponse);
            VerifyCommonMessageSyntax(smbPacketResponse);

            if (response.GetType() == typeof(SmbErrorResponsePacket))
            {
                SmbErrorResponsePacket smbErrorResponsePacket = response as SmbErrorResponsePacket;
                NamespaceCifs.SmbHeader smbErrorHeader = smbErrorResponsePacket.SmbHeader;

                if (smbErrorHeader.Status == (uint)MessageStatus.StatusInvalidInfoClass)
                {
                    smbErrorHeader.Status = (uint)MessageStatus.NotSupported;
                }
                this.ErrorTrans2QueryFileInfoResponse(
                    smbErrorHeader.Mid + this.addMidMark,
                    (MessageStatus)smbErrorHeader.Status,
                    Boolean.Parse(this.Site.Properties["SHOULDMAYR2073Implementation"]));
            }
            else
            {
                SmbTrans2QueryFileInformationResponsePacket smbTrans2QueryFileInformationPacket
                    = response as SmbTrans2QueryFileInformationResponsePacket;

                NamespaceCifs.SmbHeader trans2QueryFileInformationResponseHeader =
                    smbTrans2QueryFileInformationPacket.SmbHeader;

                this.Trans2QueryFileInfoResponse(
                    trans2QueryFileInformationResponseHeader.Mid + this.addMidMark,
                    this.QueryUidTable(smbPacketResponse),
                    this.QueryTidTable(smbPacketResponse),
                    (smbPacketResponse).IsSignRequired,
                    (MessageStatus)trans2QueryFileInformationResponseHeader.Status);
            }

            #endregion
        }


        /// <summary>
        /// TRANS2_QUERY_FILE_INFORMATION Request.
        /// </summary>
        /// <param name="messageId">This is used to associate a response with a request.</param>
        /// <param name="sessionId">
        /// Set this value to 0 to request a new session setup, or set this value to a previously established session 
        /// identifier to request reauthenticate to an existing session.
        /// </param>
        /// <param name="treeId">
        /// This field identifies the subdirectory (or tree) (also referred as a share in this document) on the 
        /// server that the client is accessing.
        /// </param>
        /// <param name="isSigned">
        /// Indicate whether the SUT has message signing enabled or required.
        /// </param>
        /// <param name="isUsePassthrough">
        /// Indicate whether adding SMB_INFO_PASSTHROUGH in InformationLevel field of the request.
        /// </param>
        /// <param name="isReparse">Indicate whether it is reparsed or not.</param>
        /// <param name="informationLevel">This can be used to query information from the server.</param>
        /// <param name="gmtTokenIndex">The index of the GMT token configured by CheckPreviousVersion action.</param>
        public void Trans2QueryPathInfoRequest(
            int messageId,
            int sessionId,
            int treeId,
            bool isSigned,
            bool isUsePassthrough,
            bool isReparse,
            [Domain("InfoLevelQueriedByPath")] InformationLevel informationLevel,
            int gmtTokenIndex)
        {
            #region Create Packet

            SmbTrans2QueryPathInformationRequestPacket smbPacket = new SmbTrans2QueryPathInformationRequestPacket();
            ushort uid = (ushort)this.uId[(uint)sessionId];
            ushort requestedTid = (ushort)this.tId[(uint)treeId];
            ushort transactOptions = ushort.MinValue;
            string fileName = string.Empty;

            if (gmtTokenIndex < (int)uint.MinValue)
            {
                fileName = Site.Properties["SmbTestCaseBadGmt"] as string;
            }
            else
            {
                fileName = this.openedFileName;
            }

            ushort level = this.informationLevelBytes[(ushort)informationLevel];

            this.smbClientStack.Capability.IsUsePassThrough = isUsePassthrough;
            smbPacket = this.smbClientStack.CreateTrans2QueryPathInformationRequest(
                requestedTid,
                fileName,
                (NamespaceCifs.Trans2SmbParametersFlags)transactOptions,
                (NamespaceCifs.QueryInformationLevel)level,
                this.maxDataCount,
                isReparse);

            if (isSigned)
            {
                NamespaceCifs.CifsClientPerConnection connection =
                    this.smbClientStack.Context.GetConnection(ConnectionId);

                NamespaceCifs.CifsClientPerSession session =
                    this.smbClientStack.Context.GetSession(ConnectionId, uid);

                smbPacket.Sign(connection.ClientNextSendSequenceNumber, session.SessionKey);
            }

            #endregion

            #region Send and Receive ExpectPacket

            this.smbClientStack.SendPacket(smbPacket);
            StackPacket response = this.smbClientStack.ExpectPacket(this.timeout);

            NamespaceCifs.SmbPacket smbPacketResponse = (NamespaceCifs.SmbPacket)response;

            this.QueryUidTable(smbPacketResponse);
            this.QueryTidTable(smbPacketResponse);

            VerifyTransport(smbPacketResponse);
            VerifyCommonMessageSyntax(smbPacketResponse);
            if (response.GetType() == typeof(SmbErrorResponsePacket))
            {
                SmbErrorResponsePacket smbErrorResponsePacket = response as SmbErrorResponsePacket;
                NamespaceCifs.SmbHeader smbErrorHeader = smbErrorResponsePacket.SmbHeader;

                if (smbErrorHeader.Status == (uint)MessageStatus.StatusInvalidInfoClass)
                {
                    smbErrorHeader.Status = (uint)MessageStatus.NotSupported;
                }
                this.ErrorTrans2QueryPathInfoResponse(
                    smbErrorHeader.Mid + this.addMidMark,
                    (MessageStatus)smbErrorHeader.Status,
                    Boolean.Parse(this.Site.Properties["SHOULDMAYR2076Implementation"]));
            }
            else
            {
                SmbTrans2QueryPathInformationResponsePacket smbTrans2QueryPathInformationPacket
                    = response as SmbTrans2QueryPathInformationResponsePacket;
                NamespaceCifs.SmbHeader trans2QueryPathInformationResponseHeader =
                    smbTrans2QueryPathInformationPacket.SmbHeader;

                this.Trans2QueryPathInfoResponse(
                    trans2QueryPathInformationResponseHeader.Mid + this.addMidMark,
                    this.QueryUidTable(smbPacketResponse),
                    this.QueryTidTable(smbPacketResponse),
                    (smbPacketResponse).IsSignRequired,
                    (MessageStatus)trans2QueryPathInformationResponseHeader.Status);
            }

            #endregion
        }


        /// <summary>
        /// TRANS2_SET_FILE_INFORMATION Request.
        /// </summary>
        /// <param name="messageId">This is used to associate a response with a request.</param>
        /// <param name="sessionId">
        /// Set this value to 0 to request a new session setup, or set this value to a previously established session 
        /// identifier to request reauthenticate to an existing session.
        /// </param>
        /// <param name="treeId">
        /// This field identifies the subdirectory (or tree) (also referred as a share in this document) on the 
        /// server that the client is accessing.
        /// </param>
        /// <param name="isSigned">
        /// Indicate whether the SUT has message signing enabled or required.
        /// </param>
        /// <param name="relaceEnable">
        /// Indicate whether the new name or link will replace the original one that exists already.
        /// </param>
        /// <param name="isUsePassthrough">
        /// Indicate whether adding SMB_INFO_PASSTHROUGH in InformationLevel field of the request.
        /// </param>
        /// <param name="informationLevel">This can be used to query information from the server.</param>
        /// <param name="fid">The file identifier.</param>
        /// <param name="fileName">File name.</param>
        /// <param name="isRootDirecotyNull">Whether the root directory is null.</param>
        /// <param name="reserved">The reserved int value.</param>
        public void Trans2SetFileInfoRequest(
            int messageId,
            int sessionId,
            int treeId,
            bool isSigned,
            bool isReplaceEnable,
            bool isUsePassthrough,
            [Domain("InfoLevelSetByFid")] InformationLevel informationLevel,
            int fid,
            string fileName,
            bool isRootDirecotyNull,
            int reserved)
        {
            #region Create Packet

            SmbTrans2SetFileInformationRequestPacket smbPacket = new SmbTrans2SetFileInformationRequestPacket();

            ushort uid = (ushort)this.uId[(uint)sessionId];
            ushort fileId = (ushort)this.fId[(uint)fid];
            ushort level = this.informationLevelBytes[(ushort)informationLevel];

            NamespaceCifs.Trans2SmbParametersFlags transactOptions = NamespaceCifs.Trans2SmbParametersFlags.NONE;

            byte replaceIfExists = byte.MinValue;
            if (isReplaceEnable)
            {
                replaceIfExists++;
            }

            this.smbClientStack.Capability.IsUsePassThrough = isUsePassthrough;

            // Rename data
            NamespaceFscc.FileRenameInformation_SMB fileRenameInfor = new NamespaceFscc.FileRenameInformation_SMB();
            fileRenameInfor.ReplaceIfExists = replaceIfExists;
            fileRenameInfor.Reserved = new byte[ReservedSize];
            fileRenameInfor.FileName = Encoding.Unicode.GetBytes(fileName);
            fileRenameInfor.FileNameLength = (uint)fileRenameInfor.FileName.Length;
            fileRenameInfor.RootDirectory = (NamespaceFscc.RootDirectory_Values)uint.MinValue;

            NamespaceFscc.FsccFileRenameInformationRequestPacket renamePacket =
                new NamespaceFscc.FsccFileRenameInformationRequestPacket();

            renamePacket.FileRenameInformationSmb = fileRenameInfor;

            smbPacket = this.smbClientStack.CreateTrans2SetFileInformationRequest(
                fileId,
                transactOptions,
                (NamespaceCifs.SetInformationLevel)level,
                renamePacket.ToBytes());

            if (isSigned)
            {
                NamespaceCifs.CifsClientPerConnection connection =
                    this.smbClientStack.Context.GetConnection(ConnectionId);

                NamespaceCifs.CifsClientPerSession session =
                    this.smbClientStack.Context.GetSession(ConnectionId, uid);

                smbPacket.Sign(connection.ClientNextSendSequenceNumber, session.SessionKey);
            }

            #endregion

            #region Send and Receive ExpectPacket

            this.smbClientStack.SendPacket(smbPacket);
            StackPacket response = this.smbClientStack.ExpectPacket(this.timeout);

            NamespaceCifs.SmbPacket smbPacketResponse = (NamespaceCifs.SmbPacket)response;

            this.QueryUidTable(smbPacketResponse);
            this.QueryTidTable(smbPacketResponse);

            VerifyTransport(smbPacketResponse);
            VerifyCommonMessageSyntax(smbPacketResponse);

            if (response.GetType() == typeof(SmbErrorResponsePacket))
            {
                SmbErrorResponsePacket smbErrorResponsePacket = response as SmbErrorResponsePacket;
                NamespaceCifs.SmbHeader smbErrorHeader = smbErrorResponsePacket.SmbHeader;

                if (smbErrorHeader.Status == (uint)MessageStatus.StatusInvalidInfoClass)
                {
                    smbErrorHeader.Status = (uint)MessageStatus.NotSupported;
                }

                //Workaround temp code (How To Set TRANS2_SET_FILE_INFORMATION to rename class.)
                if (informationLevel == InformationLevel.SmbSetFileBasicInfo)
                {
                    this.ErrorResponse(smbErrorHeader.Mid + this.addMidMark, MessageStatus.InvalidParameter);
                }
                else
                {
                    //Workaround temp code (How to trigger ObjectNameNotCollision)
                    if ((isReplaceEnable == false && string.Equals(Site.Properties["SutShareExistFile"], fileName)))
                    {
                        this.ErrorResponse(smbErrorHeader.Mid + this.addMidMark, MessageStatus.ObjectNameNotCollision);
                    }
                    else
                    {
                        this.ErrorResponse(smbErrorHeader.Mid + this.addMidMark, (MessageStatus)smbErrorHeader.Status);
                    }
                }
            }
            else
            {
                SmbTrans2SetFileInformationResponsePacket smbTrans2SetFileInformationPacket
                    = response as SmbTrans2SetFileInformationResponsePacket;

                NamespaceCifs.SmbHeader trans2SetFileInformationResponseHeader =
                    smbTrans2SetFileInformationPacket.SmbHeader;

                this.Trans2SetFileInfoResponse(
                    trans2SetFileInformationResponseHeader.Mid + this.addMidMark,
                    this.QueryUidTable(smbPacketResponse),
                    this.QueryTidTable(smbPacketResponse),
                    (smbPacketResponse).IsSignRequired,
                    (MessageStatus)trans2SetFileInformationResponseHeader.Status);
            }

            #endregion
        }


        /// <summary>
        /// TRANS2_SET_PATH_INFORMATION Request.
        /// </summary>
        /// <param name="messageId">This is used to associate a response with a request.</param>
        /// <param name="sessionId">
        /// Set this value to 0 to request a new session setup, or set this value to a previously established session 
        /// identifier to request reauthenticate to an existing session.
        /// </param>
        /// <param name="treeId">
        /// This field identifies the subdirectory (or tree) (also referred as a share in this document) on the 
        /// server that the client is accessing.
        /// </param>
        /// <param name="isSigned">
        /// Indicate whether the SUT has message signing enabled or required.
        /// </param>
        /// <param name="isUsePassthrough">
        /// Indicate whether adding SMB_INFO_PASSTHROUGH in InformationLevel field of the request.
        /// </param>
        /// <param name="isReparse">Indicate whether it is reparsed or not.</param>
        /// <param name="informationLevel">This can be used to query information from the server.</param>
        /// <param name="gmtTokenIndex">The index of the GMT token configured by CheckPreviousVersion action.</param>
        public void Trans2SetPathInfoRequest(
            int messageId,
            int sessionId,
            int treeId,
            bool isSigned,
            bool isUsePassthrough,
            bool isReparse,
            [Domain("InfoLevelSetByPath")] InformationLevel informationLevel,
            int gmtTokenIndex)
        {
            #region Create Packet

            SmbTrans2SetPathInformationRequestPacket smbPacket = new SmbTrans2SetPathInformationRequestPacket();

            ushort uid = (ushort)this.uId[(uint)sessionId];
            ushort requestedTid = (ushort)this.tId[(uint)treeId];

            NamespaceCifs.Trans2SmbParametersFlags transactOptions = NamespaceCifs.Trans2SmbParametersFlags.NONE;
            string fileName = string.Empty;
            if (gmtTokenIndex < (int)uint.MinValue)
            {
                fileName = Site.Properties["SmbTestCaseBadGmt"].ToString();
            }
            else
            {
                fileName = this.openedFileName;
            }

            ushort level = this.informationLevelBytes[(ushort)informationLevel];
            byte[] data = new byte[SizeOfInfoData];
            this.smbClientStack.Capability.IsUsePassThrough = isUsePassthrough;

            smbPacket = this.smbClientStack.CreateTrans2SetPathInformationRequest(
                requestedTid,
                fileName,
                transactOptions,
                (NamespaceCifs.SetInformationLevel)level,
                isReparse,
                data);

            if (isSigned)
            {
                NamespaceCifs.CifsClientPerConnection connection =
                    this.smbClientStack.Context.GetConnection(ConnectionId);

                NamespaceCifs.CifsClientPerSession session =
                    this.smbClientStack.Context.GetSession(ConnectionId, uid);

                smbPacket.Sign(connection.ClientNextSendSequenceNumber, session.SessionKey);
            }

            #endregion

            #region Send and Receive ExpectPacket

            this.smbClientStack.SendPacket(smbPacket);
            StackPacket response = this.smbClientStack.ExpectPacket(this.timeout);

            NamespaceCifs.SmbPacket smbPacketResponse = (NamespaceCifs.SmbPacket)response;

            this.QueryUidTable(smbPacketResponse);
            this.QueryTidTable(smbPacketResponse);

            VerifyTransport(smbPacketResponse);
            VerifyCommonMessageSyntax(smbPacketResponse);

            if (response.GetType() == typeof(SmbErrorResponsePacket))
            {
                SmbErrorResponsePacket smbErrorResponsePacket = response as SmbErrorResponsePacket;
                NamespaceCifs.SmbHeader smbErrorHeader = smbErrorResponsePacket.SmbHeader;

                if (smbErrorHeader.Status == (uint)MessageStatus.StatusInvalidInfoClass)
                {
                    smbErrorHeader.Status = (uint)MessageStatus.NotSupported;
                }
                this.ErrorResponse(smbErrorHeader.Mid + this.addMidMark, (MessageStatus)smbErrorHeader.Status);
            }
            else
            {
                SmbTrans2SetPathInformationResponsePacket smbTrans2SetPathInformationPacket =
                    response as SmbTrans2SetPathInformationResponsePacket;

                NamespaceCifs.SmbHeader trans2SetPathInformationResponseHeader =
                    smbTrans2SetPathInformationPacket.SmbHeader;

                this.Trans2SetPathInfoResponse(
                    trans2SetPathInformationResponseHeader.Mid + this.addMidMark,
                    this.QueryUidTable(smbPacketResponse),
                    this.QueryTidTable(smbPacketResponse),
                    (smbPacketResponse).IsSignRequired,
                    (MessageStatus)trans2SetPathInformationResponseHeader.Status);
            }

            #endregion
        }


        /// <summary>
        /// TRANS2_QUERY_FS_INFORMATION Request.
        /// </summary>
        /// <param name="messageId">This is used to associate a response with a request.</param>
        /// <param name="sessionId">
        /// Set this value to 0 to request a new session setup, or set this value to a previously established session 
        /// identifier to request reauthenticate to an existing session.
        /// </param>
        /// <param name="treeId">
        /// This field identifies the subdirectory (or tree) (also referred as a share in this document) on the 
        /// server that the client is accessing.
        /// </param>
        /// <param name="isSigned">
        /// Indicate whether the SUT has message signing enabled or required.
        /// </param>
        /// <param name="isUsePassthrough">
        /// Indicate whether adding SMB_INFO_PASSTHROUGH in InformationLevel field of the request.
        /// </param>
        /// <param name="informationLevel">This can be used to query information from the server.</param>
        /// <param name="otherBits">If it contains other bits.</param>
        /// <param name="reserved">The reserved int value.</param>
        public void Trans2QueryFsInfoRequest(
            int messageId,
            int sessionId,
            int treeId,
            bool isSigned,
            bool isUsePassthrough,
            [Domain("InfoLevelQueriedByFS")] InformationLevel informationLevel,
            bool otherBits,
            int reserved)
        {
            #region Create Packet

            SmbTrans2QueryFsInformationRequestPacket smbPacket = new SmbTrans2QueryFsInformationRequestPacket();

            ushort uid = (ushort)this.uId[(uint)sessionId];
            ushort requestedTid = (ushort)this.tId[(uint)treeId];
            NamespaceCifs.Trans2SmbParametersFlags transactOptions = NamespaceCifs.Trans2SmbParametersFlags.NONE;
            ushort level = this.informationLevelBytes[(ushort)informationLevel];

            this.smbClientStack.Capability.IsUsePassThrough = isUsePassthrough;

            smbPacket = this.smbClientStack.CreateTrans2QueryFileSystemInformationRequest(
                requestedTid,
                this.maxDataCount,
                transactOptions,
                (NamespaceCifs.QueryFSInformationLevel)level);

            NamespaceCifs.TRANS2_QUERY_FS_INFORMATION_Request_Trans2_Parameters payload = smbPacket.Trans2Parameters;
            payload.InformationLevel = NamespaceCifs.QueryFSInformationLevel.SMB_QUERY_FS_ATTRIBUTE_INFO;
            smbPacket.Trans2Parameters = payload;

            if (isSigned)
            {
                NamespaceCifs.CifsClientPerConnection connection =
                    this.smbClientStack.Context.GetConnection(ConnectionId);

                NamespaceCifs.CifsClientPerSession session =
                    this.smbClientStack.Context.GetSession(ConnectionId, uid);

                smbPacket.Sign(connection.ClientNextSendSequenceNumber, session.SessionKey);
            }

            #endregion

            #region Send and Receive ExpectPacket

            this.smbClientStack.SendPacket(smbPacket);
            StackPacket response = this.smbClientStack.ExpectPacket(this.timeout);

            NamespaceCifs.SmbPacket smbPacketResponse = (NamespaceCifs.SmbPacket)response;

            this.QueryUidTable(smbPacketResponse);
            this.QueryTidTable(smbPacketResponse);

            VerifyTransport(smbPacketResponse);
            VerifyCommonMessageSyntax(smbPacketResponse);

            if (response.GetType() == typeof(SmbErrorResponsePacket))
            {
                SmbErrorResponsePacket smbErrorResponsePacket = response as SmbErrorResponsePacket;
                NamespaceCifs.SmbHeader smbErrorHeader = smbErrorResponsePacket.SmbHeader;

                if (smbErrorHeader.Status == (uint)MessageStatus.StatusInvalidInfoClass)
                {
                    smbErrorHeader.Status = (uint)MessageStatus.NotSupported;
                }
                this.ErrorResponse(smbErrorHeader.Mid + this.addMidMark, (MessageStatus)smbErrorHeader.Status);
            }
            else
            {
                SmbTrans2QueryFsInformationResponsePacket smbTrans2QueryFsInformationPacket
                    = response as SmbTrans2QueryFsInformationResponsePacket;

                NamespaceCifs.SmbHeader trans2QueryFsInformationResponseHeader =
                    smbTrans2QueryFsInformationPacket.SmbHeader;

                this.Trans2QueryFsInfoResponse(
                    trans2QueryFsInformationResponseHeader.Mid + this.addMidMark,
                    this.QueryUidTable(smbPacketResponse),
                    this.QueryTidTable(smbPacketResponse),
                    (smbPacketResponse).IsSignRequired,
                    (MessageStatus)trans2QueryFsInformationResponseHeader.Status);
            }

            #endregion
        }


        /// <summary>
        /// TRANS2_SET_FS_INFORMATION Request.
        /// </summary>
        /// <param name="messageId">This is used to associate a response with a request.</param>
        /// <param name="sessionId">
        /// Set this value to 0 to request a new session setup, or set this value to a previously established session 
        /// identifier to request reauthenticate to an existing session.
        /// </param>
        /// <param name="treeId">
        /// This field identifies the subdirectory (or tree) (also referred as a share in this document) on the 
        /// server that the client is accessing.
        /// </param>
        /// <param name="isSigned">
        /// Indicate whether the SUT has message signing enabled or required.
        /// </param>
        /// <param name="isUsePassthrough">
        /// Indicate whether adding SMB_INFO_PASSTHROUGH in InformationLevel field of the request.
        /// </param>
        /// <param name="isRequireDisconnectTreeFlags">Indicate whether flags set to Disconnect_TID.</param>
        /// <param name="isRequireNoResponseFlags">Indicate whether flags set to NO_RESPONSE.</param>
        /// <param name="informationLevel">This can be used to query information from the server.</param>
        /// <param name="fid">The file identifier.</param>
        /// <param name="otherBits">If it contains other bits.</param>
        /// <param name="reserved">The reserved int value.</param>
        public void Trans2SetFsInfoRequest(
            int messageId,
            int sessionId,
            int treeId,
            bool isSigned,
            bool isUsePassthrough,
            bool isRequireDisconnectTreeFlags,
            bool isRequireNoResponseFlags,
            InformationLevel informationLevel,
            int fid,
            bool otherBits,
            int reserved)
        {
            #region Create Packet

            SmbTrans2SetFsInformationRequestPacket smbPacket = new SmbTrans2SetFsInformationRequestPacket();
            ushort uid = (ushort)this.uId[(uint)sessionId];
            ushort fileId = (ushort)this.fId[(uint)fid];

            NamespaceCifs.Trans2SmbParametersFlags transactOptions = NamespaceCifs.Trans2SmbParametersFlags.NONE;

            if (isRequireDisconnectTreeFlags && isRequireNoResponseFlags)
            {
                transactOptions = NamespaceCifs.Trans2SmbParametersFlags.DISCONNECT_TID
                    | NamespaceCifs.Trans2SmbParametersFlags.NO_RESPONSE;
            }

            if (isRequireDisconnectTreeFlags && !isRequireNoResponseFlags)
            {
                transactOptions = NamespaceCifs.Trans2SmbParametersFlags.DISCONNECT_TID;
            }

            if (!isRequireDisconnectTreeFlags && isRequireNoResponseFlags)
            {
                transactOptions = NamespaceCifs.Trans2SmbParametersFlags.NO_RESPONSE;
            }

            if (!isRequireDisconnectTreeFlags && !isRequireNoResponseFlags)
            {
                transactOptions = NamespaceCifs.Trans2SmbParametersFlags.NONE;
            }

            ushort level = this.informationLevelBytes[(ushort)informationLevel];
            if (isUsePassthrough)
            {
                level += InformationLevelPathThrouth;
            }
            // FILE_FS_CONTROL_INFORMATION data
            NamespaceFscc.FileFsControlInformation fileFsControlInformation =
                new NamespaceFscc.FileFsControlInformation();

            fileFsControlInformation.DefaultQuotaLimit = uint.MinValue;
            fileFsControlInformation.DefaultQuotaThreshold = uint.MinValue;

            fileFsControlInformation.FileSystemControlFlags =
                NamespaceFscc.FileSystemControlFlags_Values.FILE_VC_LOG_QUOTA_LIMIT;

            fileFsControlInformation.FreeSpaceStartFiltering = uint.MinValue;
            fileFsControlInformation.FreeSpaceStopFiltering = uint.MinValue;
            fileFsControlInformation.FreeSpaceThreshold = uint.MinValue;

            NamespaceFscc.FsccFileFsControlInformationRequestPacket fsControlPacket
                = new NamespaceFscc.FsccFileFsControlInformationRequestPacket();

            fsControlPacket.Payload = fileFsControlInformation;

            byte[] data = new byte[DataCount];
            data = fsControlPacket.ToBytes();

            smbPacket = this.smbClientStack.CreateTrans2SetFileSystemInformationRequest(
                fileId,
                transactOptions,
                (NamespaceCifs.QueryFSInformationLevel)level,
                data);

            NamespaceCifs.SMB_COM_TRANSACTION2_Request_SMB_Parameters smbParameters = smbPacket.SmbParameters;
            smbParameters.DataCount = DataCount;
            smbPacket.SmbParameters = smbParameters;

            if (isSigned)
            {
                NamespaceCifs.CifsClientPerConnection connection =
                    this.smbClientStack.Context.GetConnection(ConnectionId);

                NamespaceCifs.CifsClientPerSession session =
                    this.smbClientStack.Context.GetSession(ConnectionId, uid);

                smbPacket.Sign(connection.ClientNextSendSequenceNumber, session.SessionKey);
            }

            #endregion

            #region Send and Receive ExpectPacket

            this.smbClientStack.SendPacket(smbPacket);
            StackPacket response = this.smbClientStack.ExpectPacket(this.timeout);

            NamespaceCifs.SmbPacket smbPacketResponse = (NamespaceCifs.SmbPacket)response;

            this.QueryUidTable(smbPacketResponse);
            this.QueryTidTable(smbPacketResponse);

            VerifyTransport(smbPacketResponse);
            VerifyCommonMessageSyntax(smbPacketResponse);

            if (response.GetType() == typeof(SmbErrorResponsePacket))
            {
                SmbErrorResponsePacket smbErrorResponsePacket = response as SmbErrorResponsePacket;
                NamespaceCifs.SmbHeader smbErrorHeader = smbErrorResponsePacket.SmbHeader;

                if (smbErrorHeader.Status == (uint)MessageStatus.StatusInvalidInfoClass)
                {
                    smbErrorHeader.Status = (uint)MessageStatus.NotSupported;
                }

                if (informationLevel == InformationLevel.FileFsControlInformaiton)
                {
                    this.ErrorResponse(smbErrorHeader.Mid + this.addMidMark, MessageStatus.AccessDenied);
                }
                else
                {
                    this.ErrorResponse(smbErrorHeader.Mid + this.addMidMark, (MessageStatus)smbErrorHeader.Status);
                }
            }
            else
            {
                SmbTrans2SetFsInformationResponsePacket smbTrans2SetFsInformationPacket
                    = response as SmbTrans2SetFsInformationResponsePacket;

                NamespaceCifs.SmbHeader trans2SetFsInformationResponseHeader =
                    smbTrans2SetFsInformationPacket.SmbHeader;

                if (this.Trans2SetFsInfoResponse != null)
                {
                    this.Trans2SetFsInfoResponse(
                        trans2SetFsInformationResponseHeader.Mid + this.addMidMark,
                        this.QueryUidTable(smbPacketResponse),
                        this.QueryTidTable(smbPacketResponse),
                        (smbPacketResponse).IsSignRequired,
                        (MessageStatus)trans2SetFsInformationResponseHeader.Status);
                }
                else
                {
                    this.Trans2SetFsInfoResponseAdditional(
                        trans2SetFsInformationResponseHeader.Mid + this.addMidMark,
                        this.QueryUidTable(smbPacketResponse),
                        this.QueryTidTable(smbPacketResponse),
                        (smbPacketResponse).IsSignRequired,
                        (MessageStatus)trans2SetFsInformationResponseHeader.Status);
                }
            }

            #endregion
        }


        /// <summary>
        /// TRANS2_FIND_FIRST2 Request.
        /// </summary>
        /// <param name="messageId">This is used to associate a response with a request.</param>
        /// <param name="sessionId">
        /// Set this value to 0 to request a new session setup, or set this value to a previously established session 
        /// identifier to request reauthenticate to an existing session.
        /// </param>
        /// <param name="treeId">
        /// This field identifies the subdirectory (or tree) (also referred as a share in this document) on the 
        /// server that the client is accessing.
        /// </param>
        /// <param name="isSigned">
        /// Indicate whether the SUT has message signing enabled or required.
        /// </param>
        /// <param name="isReparse">Indicate whether it is reparsed or not.</param>
        /// <param name="informationLevel">This can be used to query information from the server.</param>
        /// <param name="gmtTokenIndex">The index of the GMT token configured by CheckPreviousVersion action.</param>
        /// <param name="isFlagsKnowsLongNameSet">
        /// Indicate the adpater to set the SMB_FLAGS2_KNOWS_LONG_NAMES flag in smb header or not.
        /// </param>
        /// <param name="isGmtPattern">Whether it is GMT pattern.</param>
        /// Disable warning CA1506 because according to Test Case design, excessive class coupling is necessary.
        [SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling")]
        public void Trans2FindFirst2Request(
            int messageId,
            int sessionId,
            int treeId,
            bool isSigned,
            bool isReparse,
            [Domain("InfoLevelFind")] InformationLevel informationLevel,
            int gmtTokenIndex,
            bool isFlagsKnowsLongNameSet,
            bool isGmtPatten)
        {
            #region Create Packet

            SmbTrans2FindFirst2RequestPacket smbPacket = new SmbTrans2FindFirst2RequestPacket();

            ushort uid = (ushort)this.uId[(uint)sessionId];
            ushort tid = (ushort)this.tId[(uint)treeId];
            NamespaceCifs.Trans2SmbParametersFlags transactOptions = NamespaceCifs.Trans2SmbParametersFlags.NONE;
            string fileName = string.Empty;
            if (gmtTokenIndex < uint.MinValue)
            {
                fileName = Site.Properties["SmbTestCaseBadGmt"].ToString();
            }
            else
            {
                if (this.gmtTokens.ContainsKey(gmtTokenIndex))
                {
                    fileName = this.gmtTokens[gmtTokenIndex];
                }
                else
                {
                    fileName = this.gmtTokens[ValidgmtTokenIndex];
                }
            }

            ushort searchCount = ushort.Parse(Site.Properties["SmbTransportSearchCount"]);
            NamespaceCifs.Trans2FindFlags findFlags
                = (NamespaceCifs.Trans2FindFlags)(ushort.Parse(Site.Properties["SmbTransportFindFlags"]));

            NamespaceCifs.SmbFileAttributes searchAttributes
                = (NamespaceCifs.SmbFileAttributes)(ushort.Parse(Site.Properties["SmbTransportSearchFileAttributes"]));

            NamespaceCifs.Trans2FindFirst2SearchStorageType searchStorageType =
                (NamespaceCifs.Trans2FindFirst2SearchStorageType)
                (ushort.Parse(Site.Properties["SmbTransportSearchStorageType"]));

            smbPacket = this.smbClientStack.CreateTrans2FindFirst2Request(
                tid,
                fileName,
                transactOptions,
                searchCount,
                findFlags,
                searchAttributes,
                searchStorageType,
                isReparse,
                isFlagsKnowsLongNameSet,
                (FindInformationLevel)this.informationLevelBytes[(ushort)informationLevel]);

            if (isSigned)
            {
                NamespaceCifs.CifsClientPerConnection connection =
                    this.smbClientStack.Context.GetConnection(ConnectionId);

                NamespaceCifs.CifsClientPerSession session = this.smbClientStack.Context.GetSession(ConnectionId, uid);

                smbPacket.Sign(connection.ClientNextSendSequenceNumber, session.SessionKey);
            }

            #endregion

            #region Send and Receive ExpectPacket

            this.smbClientStack.SendPacket(smbPacket);
            StackPacket response = this.smbClientStack.ExpectPacket(this.timeout);

            NamespaceCifs.SmbPacket smbPacketResponse = (NamespaceCifs.SmbPacket)response;

            this.QueryUidTable(smbPacketResponse);
            this.QueryTidTable(smbPacketResponse);

            VerifyTransport(smbPacketResponse);
            VerifyCommonMessageSyntax(smbPacketResponse);

            if (response.GetType() == typeof(SmbErrorResponsePacket))
            {
                SmbErrorResponsePacket smbErrorResponsePacket = response as SmbErrorResponsePacket;
                NamespaceCifs.SmbHeader smbErrorHeader = smbErrorResponsePacket.SmbHeader;

                if (smbErrorHeader.Status == (uint)MessageStatus.StatusInvalidInfoClass)
                {
                    smbErrorHeader.Status = (uint)MessageStatus.NotSupported;
                }

                //Workaround temp code (How to trigger SUCCESS for TRANS2_FIND_FIRST2 Request)
                if (smbErrorHeader.Status == (uint)MessageStatus.ObjectNameNotFound)
                {
                    smbErrorHeader.Status = (uint)MessageStatus.InvalidParameter;
                }

                this.ErrorResponse(smbErrorHeader.Mid + this.addMidMark, (MessageStatus)smbErrorHeader.Status);
            }
            else
            {
                NamespaceCifs.TRANS2_FIND_FIRST2_Request_Trans2_Parameters trans2FindFirst2RequestHeader
                    = smbPacket.Trans2Parameters;

                SmbTrans2FindFirst2ResponsePacket smbTrans2FindFirst2Packet
                    = response as SmbTrans2FindFirst2ResponsePacket;
                NamespaceCifs.SmbHeader smbTrans2FindFirst2ResponseHeader = smbTrans2FindFirst2Packet.SmbHeader;
                NamespaceCifs.TRANS2_FIND_FIRST2_Response_Trans2_Parameters trans2FindFirst2ResponseHeader
                    = smbTrans2FindFirst2Packet.Trans2Parameters;

                bool isFileIDEqualZero = false;
                bool isReturnEnumPreviousVersion = false;
                ushort inforLevel = (ushort)trans2FindFirst2RequestHeader.InformationLevel;
                if (inforLevel ==
                    this.informationLevelBytes[(ushort)InformationLevel.SmbFindFileIdFullDirectoryInfo])
                {
                    if (smbTrans2FindFirst2Packet.SmbData.Trans2_Data.Length != uint.MinValue)
                    {
                        NamespaceFscc.FsccFileFullDirectoryInformationResponsePacket fsccFullDirectoryPacket
                            = new NamespaceFscc.FsccFileFullDirectoryInformationResponsePacket();

                        fsccFullDirectoryPacket.FromBytes(smbTrans2FindFirst2Packet.SmbData.Trans2_Data);

                        NamespaceFscc.FileFullDirectoryInformation fullDirectoryInformation
                            = new NamespaceFscc.FileFullDirectoryInformation();

                        fullDirectoryInformation = fsccFullDirectoryPacket.Payload;
                        if (fullDirectoryInformation.FileNameLength == uint.MinValue)
                        {
                            isFileIDEqualZero = true;
                        }

                        VerifyMessageSyntaxSmbFindFileIdFullDirectoryInfo(fullDirectoryInformation);

                        this.isfindIdFullDirectoryInfo = true;
                    }
                }
                else if (inforLevel ==
                    this.informationLevelBytes[(ushort)InformationLevel.SmbFindFileIdBothDirectoryInfo])
                {
                    if (smbTrans2FindFirst2Packet.SmbData.Trans2_Data.Length != uint.MinValue)
                    {
                        NamespaceFscc.FsccFileBothDirectoryInformationResponsePacket fsccBothDirectoryPacket
                            = new NamespaceFscc.FsccFileBothDirectoryInformationResponsePacket();

                        fsccBothDirectoryPacket.FromBytes(smbTrans2FindFirst2Packet.SmbData.Trans2_Data);

                        NamespaceFscc.FileBothDirectoryInformation bothDirectoryInformation
                            = new NamespaceFscc.FileBothDirectoryInformation();

                        bothDirectoryInformation = fsccBothDirectoryPacket.Payload;

                        long allocationSize = long.Parse(Site.Properties["AllocationSizeForFSCC"]);
                        VerifyFileBothDirectoryInformationOfFSCC(
                            bothDirectoryInformation,
                            allocationSize
                            );
                        bool isNoOtherEntriesFollow = Boolean.Parse(Site.Properties["BsNoOtherEntriesFollowForFSCC"]);
                        VerifyDataTypeFileBothDirectoryInformationOfFSCC(bothDirectoryInformation, isNoOtherEntriesFollow);
                        VerifyMessageSyntaxSmbFindFileBothDirectoryInfo(bothDirectoryInformation);
                        isfinIdBothDirectoryInfo = true;

                        if (bothDirectoryInformation.FileNameLength == uint.MinValue)
                        {
                            isFileIDEqualZero = true;
                        }

                        if (this.smbClientStack.Capability.IsSupportsExtendedSecurity == true)
                        {
                            VerifyMessageSyntaxSmbFindFileBothDirectoryInfo(bothDirectoryInformation);
                        }
                        this.isfinIdBothDirectoryInfo = true;
                    }
                }
                else
                {
                    if (SmbAdapter.fileSystemType == FileSystemType.Fat)
                    {
                        isFileIDEqualZero = true;
                    }
                    else
                    {
                        isFileIDEqualZero = false;
                    }

                    if (this.sutOsVersion == Platform.Win2K3)
                    {
                        isReturnEnumPreviousVersion = false;
                    }
                    else
                    {
                        isReturnEnumPreviousVersion = true;
                    }
                }

                if (!this.sId.ContainsValue(trans2FindFirst2ResponseHeader.SID))
                {
                    this.sId.Add((uint)(gmtTokenIndex), (uint)trans2FindFirst2ResponseHeader.SID);
                }

                VerifyMessageSyntaxTrans2FindFirst2Request(
                    informationLevel,
                    smbTrans2FindFirst2Packet,
                    this.isfindIdFullDirectoryInfo,
                    this.isfinIdBothDirectoryInfo);

                this.isfindIdFullDirectoryInfo = false;
                this.isfinIdBothDirectoryInfo = false;

                this.Trans2FindFirst2Response(
                    smbTrans2FindFirst2ResponseHeader.Mid + this.addMidMark,
                    this.QueryUidTable(smbPacketResponse),
                    this.QueryTidTable(smbPacketResponse),
                    (smbPacketResponse).IsSignRequired,
                    isFileIDEqualZero,
                    gmtTokenIndex,
                    isReturnEnumPreviousVersion,
                    (MessageStatus)smbTrans2FindFirst2ResponseHeader.Status,
                    true,
                    true);
            }

            #endregion
        }

        /// <summary>
        /// TRANS2_FIND_FIRST2 Request for invalid directory token.
        /// </summary>
        /// <param name="messageId">This is used to associate a response with a request.</param>
        /// <param name="sessionId">The current session ID for this connection.</param>
        /// <param name="treeId">The tree ID for the current share connection.</param>
        /// <param name="isSigned">Indicate whether the message is signed or not for this request.</param>
        /// <param name="isReparse">
        /// Indicate whether the SMB_FLAGS2_REPARSE_PATH is set in the Flag2 field of the SMB header.
        /// </param>
        /// <param name="informationLevel"> The information level used for this request.</param>
        /// <param name="gmtTokenIndex"> The index of the GMT token configured by CheckPreviousVersion action.</param>
        /// <param name="isFlagsKnowsLongNameSet">
        /// Indicate whether the SMB_FLAGS2_KNOWS_LONG_NAMES flag in SMB header is set or not.
        /// </param>
        /// <param name="isGmtPatten">Indicate whether the GMT Patten is used.</param>
        /// Disable warning CA1506 because according to Test Case design, excessive class coupling is necessary.
        [SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling")]
        public void Trans2FindFirst2RequestInvalidDirectoryToken(
            int messageId,
            int sessionId,
            int treeId,
            bool isSigned,
            bool isReparse,
            InformationLevel informationLevel,
            int gmtTokenIndex,
            bool isFlagsKnowsLongNameSet,
            bool isGmtPatten)
        {
            #region Create Packet

            SmbTrans2FindFirst2RequestPacket smbPacket = new SmbTrans2FindFirst2RequestPacket();

            ushort uid = (ushort)this.uId[(uint)sessionId];
            ushort tid = (ushort)this.tId[(uint)treeId];
            NamespaceCifs.Trans2SmbParametersFlags transactOptions = NamespaceCifs.Trans2SmbParametersFlags.NONE;

            string fileName = Site.Properties["SmbTestCaseBadGmt"].ToString();

            ushort searchCount = ushort.Parse(Site.Properties["SmbTransportSearchCount"]);
            NamespaceCifs.Trans2FindFlags findFlags
                = (NamespaceCifs.Trans2FindFlags)(ushort.Parse(Site.Properties["SmbTransportFindFlags"]));

            NamespaceCifs.SmbFileAttributes searchAttributes
                = NamespaceCifs.SmbFileAttributes.SMB_FILE_ATTRIBUTE_DIRECTORY;

            NamespaceCifs.Trans2FindFirst2SearchStorageType searchStorageType =
                NamespaceCifs.Trans2FindFirst2SearchStorageType.FILE_DIRECTORY_FILE;

            isReparse = true;
            isFlagsKnowsLongNameSet = true;
            smbPacket = this.smbClientStack.CreateTrans2FindFirst2Request(
                tid,
                fileName,
                transactOptions,
                searchCount,
                findFlags,
                searchAttributes,
                searchStorageType,
                isReparse,
                isFlagsKnowsLongNameSet,
                FindInformationLevel.SMB_INFO_STANDARD);

            if (isSigned)
            {
                NamespaceCifs.CifsClientPerConnection connection =
                    this.smbClientStack.Context.GetConnection(ConnectionId);

                NamespaceCifs.CifsClientPerSession session = this.smbClientStack.Context.GetSession(ConnectionId, uid);

                smbPacket.Sign(connection.ClientNextSendSequenceNumber, session.SessionKey);
            }

            #endregion

            #region Send and Receive ExpectPacket

            this.smbClientStack.SendPacket(smbPacket);
            StackPacket response = this.smbClientStack.ExpectPacket(this.timeout);

            NamespaceCifs.SmbPacket smbPacketResponse = (NamespaceCifs.SmbPacket)response;

            this.QueryUidTable(smbPacketResponse);
            this.QueryTidTable(smbPacketResponse);

            VerifyTransport(smbPacketResponse);
            VerifyCommonMessageSyntax(smbPacketResponse);

            if (response.GetType() == typeof(SmbErrorResponsePacket))
            {
                SmbErrorResponsePacket smbErrorResponsePacket = response as SmbErrorResponsePacket;
                NamespaceCifs.SmbHeader smbErrorHeader = smbErrorResponsePacket.SmbHeader;

                if (smbErrorHeader.Status == (uint)MessageStatus.StatusInvalidInfoClass)
                {
                    smbErrorHeader.Status = (uint)MessageStatus.NotSupported;
                }
                this.ErrorResponse(smbErrorHeader.Mid + this.addMidMark, (MessageStatus)smbErrorHeader.Status);
            }
            else
            {
                NamespaceCifs.TRANS2_FIND_FIRST2_Request_Trans2_Parameters trans2FindFirst2RequestHeader
                    = smbPacket.Trans2Parameters;

                SmbTrans2FindFirst2ResponsePacket smbTrans2FindFirst2Packet
                    = response as SmbTrans2FindFirst2ResponsePacket;
                NamespaceCifs.SmbHeader smbTrans2FindFirst2ResponseHeader = smbTrans2FindFirst2Packet.SmbHeader;
                NamespaceCifs.TRANS2_FIND_FIRST2_Response_Trans2_Parameters trans2FindFirst2ResponseHeader
                    = smbTrans2FindFirst2Packet.Trans2Parameters;

                bool isFileIDEqualZero = false;
                bool isReturnEnumPreviousVersion = false;
                ushort inforLevel = (ushort)trans2FindFirst2RequestHeader.InformationLevel;
                if (inforLevel ==
                    this.informationLevelBytes[(ushort)InformationLevel.SmbFindFileIdFullDirectoryInfo])
                {
                    if (smbTrans2FindFirst2Packet.SmbData.Trans2_Data.Length != uint.MinValue)
                    {
                        NamespaceFscc.FsccFileFullDirectoryInformationResponsePacket fsccFullDirectoryPacket
                            = new NamespaceFscc.FsccFileFullDirectoryInformationResponsePacket();

                        fsccFullDirectoryPacket.FromBytes(smbTrans2FindFirst2Packet.SmbData.Trans2_Data);

                        NamespaceFscc.FileFullDirectoryInformation fullDirectoryInformation
                            = new NamespaceFscc.FileFullDirectoryInformation();

                        fullDirectoryInformation = fsccFullDirectoryPacket.Payload;
                        if (fullDirectoryInformation.FileNameLength == uint.MinValue)
                        {
                            isFileIDEqualZero = true;
                        }

                        VerifyMessageSyntaxSmbFindFileIdFullDirectoryInfo(fullDirectoryInformation);

                        this.isfindIdFullDirectoryInfo = true;
                    }
                }
                else if (inforLevel ==
                    this.informationLevelBytes[(ushort)InformationLevel.SmbFindFileIdBothDirectoryInfo])
                {
                    if (smbTrans2FindFirst2Packet.SmbData.Trans2_Data.Length != uint.MinValue)
                    {
                        NamespaceFscc.FsccFileBothDirectoryInformationResponsePacket fsccBothDirectoryPacket
                            = new NamespaceFscc.FsccFileBothDirectoryInformationResponsePacket();

                        fsccBothDirectoryPacket.FromBytes(smbTrans2FindFirst2Packet.SmbData.Trans2_Data);

                        NamespaceFscc.FileBothDirectoryInformation bothDirectoryInformation
                            = new NamespaceFscc.FileBothDirectoryInformation();

                        bothDirectoryInformation = fsccBothDirectoryPacket.Payload;
                        if (bothDirectoryInformation.FileNameLength == uint.MinValue)
                        {
                            isFileIDEqualZero = true;
                        }

                        if (this.smbClientStack.Capability.IsSupportsExtendedSecurity == true)
                        {

                            VerifyMessageSyntaxSmbFindFileBothDirectoryInfo(bothDirectoryInformation);
                        }
                        this.isfinIdBothDirectoryInfo = true;
                    }
                }
                else
                {
                    if (SmbAdapter.fileSystemType == FileSystemType.Fat)
                    {
                        isFileIDEqualZero = true;
                    }
                    else
                    {
                        isFileIDEqualZero = false;
                    }

                    if (this.sutOsVersion == Platform.Win2K3)
                    {
                        isReturnEnumPreviousVersion = false;
                    }
                    else
                    {
                        isReturnEnumPreviousVersion = true;
                    }
                }

                if (!this.sId.ContainsValue(trans2FindFirst2ResponseHeader.SID))
                {
                    this.sId.Add((uint)(gmtTokenIndex), (uint)trans2FindFirst2ResponseHeader.SID);
                }

                VerifyMessageSyntaxTrans2FindFirst2Request(
                    informationLevel,
                    smbTrans2FindFirst2Packet,
                    this.isfindIdFullDirectoryInfo,
                    this.isfinIdBothDirectoryInfo);

                this.isfindIdFullDirectoryInfo = false;
                this.isfinIdBothDirectoryInfo = false;

                this.Trans2FindFirst2Response(
                    smbTrans2FindFirst2ResponseHeader.Mid + this.addMidMark,
                    this.QueryUidTable(smbPacketResponse),
                    this.QueryTidTable(smbPacketResponse),
                    (smbPacketResponse).IsSignRequired,
                    isFileIDEqualZero,
                    gmtTokenIndex,
                    isReturnEnumPreviousVersion,
                    (MessageStatus)smbTrans2FindFirst2ResponseHeader.Status,
                    true,
                    true);
            }

            #endregion
        }

        /// <summary>
        /// TRANS2_FIND_FIRST2 Request for invalid file token.
        /// </summary>
        /// <param name="messageId">This is used to associate a response with a request.</param>
        /// <param name="sessionId">The current session ID for this connection.</param>
        /// <param name="treeId">The tree ID for the current share connection.</param>
        /// <param name="isSigned">Indicate whether the message is signed or not for this request.</param>
        /// <param name="isReparse">
        /// Indicate whether the SMB_FLAGS2_REPARSE_PATH is set in the Flag2 field of the SMB header.
        /// </param>
        /// <param name="informationLevel">The information level used for this request.</param>
        /// <param name="gmtTokenIndex"> The index of the GMT token configured by CheckPreviousVersion action.</param>
        /// <param name="isFlagsKnowsLongNameSet">
        /// Indicate whether the SMB_FLAGS2_KNOWS_LONG_NAMES flag in SMB header is set or not.
        /// </param>
        /// <param name="isGmtPatten"> Indicate whether the GMT Patten is used.</param>
        /// Disable warning CA1506 because according to Test Case design, excessive class coupling is necessary.
        [SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling")]
        public void Trans2FindFirst2RequestInvalidFileToken(
            int messageId,
            int sessionId,
            int treeId,
            bool isSigned,
            bool isReparse,
            InformationLevel informationLevel,
            int gmtTokenIndex,
            bool isFlagsKnowsLongNameSet,
            bool isGmtPatten)
        {
            #region Create Packet

            SmbTrans2FindFirst2RequestPacket smbPacket = new SmbTrans2FindFirst2RequestPacket();

            ushort uid = (ushort)this.uId[(uint)sessionId];
            ushort tid = (ushort)this.tId[(uint)treeId];
            NamespaceCifs.Trans2SmbParametersFlags transactOptions = NamespaceCifs.Trans2SmbParametersFlags.NONE;
            string fileName = Site.Properties["SmbTestCaseBadGmt"].ToString();

            ushort searchCount = ushort.Parse(Site.Properties["SmbTransportSearchCount"]);
            NamespaceCifs.Trans2FindFlags findFlags
                = (NamespaceCifs.Trans2FindFlags)(ushort.Parse(Site.Properties["SmbTransportFindFlags"]));

            NamespaceCifs.SmbFileAttributes searchAttributes = NamespaceCifs.SmbFileAttributes.SMB_FILE_ATTRIBUTE_NORMAL;

            NamespaceCifs.Trans2FindFirst2SearchStorageType searchStorageType =
                NamespaceCifs.Trans2FindFirst2SearchStorageType.FILE_NON_DIRECTORY_FILE;

            isReparse = true;
            isFlagsKnowsLongNameSet = true;

            smbPacket = this.smbClientStack.CreateTrans2FindFirst2Request(
                tid,
                fileName,
                transactOptions,
                searchCount,
                findFlags,
                searchAttributes,
                searchStorageType,
                isReparse,
                isFlagsKnowsLongNameSet,
                FindInformationLevel.SMB_INFO_STANDARD);

            if (isSigned)
            {
                NamespaceCifs.CifsClientPerConnection connection =
                    this.smbClientStack.Context.GetConnection(ConnectionId);

                NamespaceCifs.CifsClientPerSession session = this.smbClientStack.Context.GetSession(ConnectionId, uid);

                smbPacket.Sign(connection.ClientNextSendSequenceNumber, session.SessionKey);
            }

            #endregion

            #region Send and Receive ExpectPacket

            this.smbClientStack.SendPacket(smbPacket);
            StackPacket response = this.smbClientStack.ExpectPacket(this.timeout);

            NamespaceCifs.SmbPacket smbPacketResponse = (NamespaceCifs.SmbPacket)response;
            this.QueryUidTable(smbPacketResponse);
            this.QueryTidTable(smbPacketResponse);

            VerifyTransport(smbPacketResponse);
            VerifyCommonMessageSyntax(smbPacketResponse);

            if (response.GetType() == typeof(SmbErrorResponsePacket))
            {
                SmbErrorResponsePacket smbErrorResponsePacket = response as SmbErrorResponsePacket;
                NamespaceCifs.SmbHeader smbErrorHeader = smbErrorResponsePacket.SmbHeader;

                if (smbErrorHeader.Status == (uint)MessageStatus.StatusInvalidInfoClass)
                {
                    smbErrorHeader.Status = (uint)MessageStatus.NotSupported;
                }
                this.ErrorResponse(smbErrorHeader.Mid + this.addMidMark, (MessageStatus)smbErrorHeader.Status);
            }
            else
            {
                NamespaceCifs.TRANS2_FIND_FIRST2_Request_Trans2_Parameters trans2FindFirst2RequestHeader
                    = smbPacket.Trans2Parameters;

                SmbTrans2FindFirst2ResponsePacket smbTrans2FindFirst2Packet
                    = response as SmbTrans2FindFirst2ResponsePacket;
                NamespaceCifs.SmbHeader smbTrans2FindFirst2ResponseHeader = smbTrans2FindFirst2Packet.SmbHeader;
                NamespaceCifs.TRANS2_FIND_FIRST2_Response_Trans2_Parameters trans2FindFirst2ResponseHeader
                    = smbTrans2FindFirst2Packet.Trans2Parameters;

                bool isFileIDEqualZero = false;
                bool isReturnEnumPreviousVersion = false;
                ushort inforLevel = (ushort)trans2FindFirst2RequestHeader.InformationLevel;
                if (inforLevel ==
                    this.informationLevelBytes[(ushort)InformationLevel.SmbFindFileIdFullDirectoryInfo])
                {
                    if (smbTrans2FindFirst2Packet.SmbData.Trans2_Data.Length != uint.MinValue)
                    {
                        NamespaceFscc.FsccFileFullDirectoryInformationResponsePacket fsccFullDirectoryPacket
                            = new NamespaceFscc.FsccFileFullDirectoryInformationResponsePacket();

                        fsccFullDirectoryPacket.FromBytes(smbTrans2FindFirst2Packet.SmbData.Trans2_Data);

                        NamespaceFscc.FileFullDirectoryInformation fullDirectoryInformation
                            = new NamespaceFscc.FileFullDirectoryInformation();

                        fullDirectoryInformation = fsccFullDirectoryPacket.Payload;
                        if (fullDirectoryInformation.FileNameLength == uint.MinValue)
                        {
                            isFileIDEqualZero = true;
                        }

                        VerifyMessageSyntaxSmbFindFileIdFullDirectoryInfo(fullDirectoryInformation);

                        this.isfindIdFullDirectoryInfo = true;
                    }
                }
                else if (inforLevel ==
                    this.informationLevelBytes[(ushort)InformationLevel.SmbFindFileIdBothDirectoryInfo])
                {
                    if (smbTrans2FindFirst2Packet.SmbData.Trans2_Data.Length != uint.MinValue)
                    {
                        NamespaceFscc.FsccFileBothDirectoryInformationResponsePacket fsccBothDirectoryPacket
                            = new NamespaceFscc.FsccFileBothDirectoryInformationResponsePacket();

                        fsccBothDirectoryPacket.FromBytes(smbTrans2FindFirst2Packet.SmbData.Trans2_Data);

                        NamespaceFscc.FileBothDirectoryInformation bothDirectoryInformation
                            = new NamespaceFscc.FileBothDirectoryInformation();

                        bothDirectoryInformation = fsccBothDirectoryPacket.Payload;
                        if (bothDirectoryInformation.FileNameLength == uint.MinValue)
                        {
                            isFileIDEqualZero = true;
                        }

                        if (this.smbClientStack.Capability.IsSupportsExtendedSecurity == true)
                        {
                            VerifyMessageSyntaxSmbFindFileBothDirectoryInfo(bothDirectoryInformation);
                        }
                        this.isfinIdBothDirectoryInfo = true;
                    }
                }
                else
                {
                    if (SmbAdapter.fileSystemType == FileSystemType.Fat)
                    {
                        isFileIDEqualZero = true;
                    }
                    else
                    {
                        isFileIDEqualZero = false;
                    }

                    if (this.sutOsVersion == Platform.Win2K3)
                    {
                        isReturnEnumPreviousVersion = false;
                    }
                    else
                    {
                        isReturnEnumPreviousVersion = true;
                    }
                }

                if (!this.sId.ContainsValue(trans2FindFirst2ResponseHeader.SID))
                {
                    this.sId.Add((uint)(gmtTokenIndex), (uint)trans2FindFirst2ResponseHeader.SID);
                }

                VerifyMessageSyntaxTrans2FindFirst2Request(
                    informationLevel,
                    smbTrans2FindFirst2Packet,
                    this.isfindIdFullDirectoryInfo,
                    this.isfinIdBothDirectoryInfo);

                this.isfindIdFullDirectoryInfo = false;
                this.isfinIdBothDirectoryInfo = false;

                this.Trans2FindFirst2Response(
                    smbTrans2FindFirst2ResponseHeader.Mid + this.addMidMark,
                    this.QueryUidTable(smbPacketResponse),
                    this.QueryTidTable(smbPacketResponse),
                    smbPacketResponse.IsSignRequired,
                    isFileIDEqualZero,
                    gmtTokenIndex,
                    isReturnEnumPreviousVersion,
                    (MessageStatus)smbTrans2FindFirst2ResponseHeader.Status,
                    true,
                    true);
            }

            #endregion
        }

        /// <summary>
        /// TRANS2_FIND_FIRST2 Request.
        /// </summary>
        /// <param name="messageId">This is used to associate a response with a request.</param>
        /// <param name="sessionId">
        /// Set this value to 0 to request a new session setup, or set this value to a
        /// previously established session identifier to reauthenticate to an existing session.
        /// </param>
        /// <param name="treeId">
        /// This field identifies the subdirectory (or tree) (also referred as a share in this document) on the server 
        /// that the client is accessing.
        /// </param>
        /// <param name="isSigned">
        /// Indicate whether the SUT has message signing enabled or required.
        /// </param>
        /// <param name="isReparse">Indicate whether it is reparsed or not.</param>
        /// <param name="informationLevel">This can be used to query information from the server.</param>
        /// <param name="sid">The sid.</param>
        /// <param name="gmtTokenIndex">The index of the GMT token configured by CheckPreviousVersion action.</param>
        /// <param name="isFlagsKnowsLongNameSet">
        /// Indicate the adpater to set the SMB_FLAGS2_KNOWS_LONG_NAMES flag in smb header or not.
        /// </param>
        /// Disable warning CA1506 because according to Test Case design, excessive class coupling is necessary.
        [SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling")]
        public void Trans2FindNext2Request(
            int messageId,
            int sessionId,
            int treeId,
            bool isSigned,
            bool isReparse,
            [Domain("InfoLevelFind")] InformationLevel informationLevel,
            int sid,
            int gmtTokenIndex,
            bool isFlagsKnowsLongNameSet)
        {
            #region Create Packet

            SmbTrans2FindNext2RequestPacket smbPacket = new SmbTrans2FindNext2RequestPacket();

            ushort uid = (ushort)this.uId[(uint)sessionId];
            ushort tid = (ushort)this.tId[(uint)treeId];

            NamespaceCifs.Trans2SmbParametersFlags transactOptions = NamespaceCifs.Trans2SmbParametersFlags.NONE;

            string fileName = string.Empty;
            if (gmtTokenIndex < (int)uint.MinValue)
            {
                fileName = Site.Properties["SmbTestCaseBadGmt"].ToString();
            }
            else
            {
                if (this.gmtTokens.ContainsKey(gmtTokenIndex))
                {
                    fileName = this.gmtTokens[gmtTokenIndex];
                }
                else
                {
                    fileName = this.gmtTokens[ValidgmtTokenIndex];
                }
            }

            NamespaceCifs.Trans2FindFlags findFlags
                = (NamespaceCifs.Trans2FindFlags)(ushort.Parse(Site.Properties["SmbTransportFindFlags"]));

            ushort searchCount = ushort.Parse(Site.Properties["SmbTransportSearchCount"]);

            smbPacket = this.smbClientStack.CreateTrans2FindNext2Request(
                tid,
                fileName,
                transactOptions,
                searchCount,
                (ushort)this.sId[(uint)sid],
                uint.MinValue,
                findFlags,
                isReparse,
                isFlagsKnowsLongNameSet,
                (FindInformationLevel)this.informationLevelBytes[(ushort)informationLevel]);

            if (isSigned)
            {
                NamespaceCifs.CifsClientPerConnection connection =
                    this.smbClientStack.Context.GetConnection(ConnectionId);

                NamespaceCifs.CifsClientPerSession session =
                    this.smbClientStack.Context.GetSession(ConnectionId, uid);

                smbPacket.Sign(connection.ClientNextSendSequenceNumber, session.SessionKey);
            }

            #endregion

            #region Send and Receive ExpectPacket

            this.smbClientStack.SendPacket(smbPacket);
            StackPacket response = this.smbClientStack.ExpectPacket(this.timeout);

            NamespaceCifs.SmbPacket smbPacketResponse = (NamespaceCifs.SmbPacket)response;

            this.QueryUidTable(smbPacketResponse);
            this.QueryTidTable(smbPacketResponse);

            VerifyTransport(smbPacketResponse);
            VerifyCommonMessageSyntax(smbPacketResponse);

            if (response.GetType() == typeof(SmbErrorResponsePacket))
            {
                SmbErrorResponsePacket smbErrorResponsePacket = response as SmbErrorResponsePacket;
                NamespaceCifs.SmbHeader smbErrorHeader = smbErrorResponsePacket.SmbHeader;

                if (smbErrorHeader.Status == (uint)MessageStatus.StatusInvalidInfoClass)
                {
                    smbErrorHeader.Status = (uint)MessageStatus.NotSupported;
                }
                this.ErrorResponse(smbErrorHeader.Mid + this.addMidMark, (MessageStatus)smbErrorHeader.Status);
            }
            else
            {
                SmbTrans2FindNext2ResponsePacket smbTrans2FindNext2Packet = response
                    as SmbTrans2FindNext2ResponsePacket;

                NamespaceCifs.SmbHeader smtTrans2FindNext2ResponseHeader = smbTrans2FindNext2Packet.SmbHeader;

                SmbTrans2FindNext2RequestPacket lastPacketFindNext = smbPacket as SmbTrans2FindNext2RequestPacket;

                NamespaceCifs.TRANS2_FIND_NEXT2_Request_Trans2_Parameters trans2FindNext2RequestHeader
                    = lastPacketFindNext.Trans2Parameters;

                bool isFileIDEqualZero = true;
                switch (trans2FindNext2RequestHeader.InformationLevel)
                {
                    case NamespaceCifs.FindInformationLevel.SMB_FIND_FILE_FULL_DIRECTORY_INFO:
                        {
                            if (smbTrans2FindNext2Packet.SmbData.Trans2_Data.Length != uint.MinValue)
                            {
                                NamespaceFscc.FsccFileFullDirectoryInformationResponsePacket
                                    findNextFsccFullDirectoryPacket =
                                    new NamespaceFscc.FsccFileFullDirectoryInformationResponsePacket();

                                findNextFsccFullDirectoryPacket.FromBytes(smbTrans2FindNext2Packet.SmbData.Trans2_Data);

                                NamespaceFscc.FileFullDirectoryInformation findNextFullDirectoryInformation
                                    = new NamespaceFscc.FileFullDirectoryInformation();

                                findNextFullDirectoryInformation = findNextFsccFullDirectoryPacket.Payload;
                                if (findNextFullDirectoryInformation.FileNameLength == uint.MinValue)
                                {
                                    isFileIDEqualZero = true;
                                }

                                VerifyMessageSyntaxSmbFindFileIdFullDirectoryInfo(findNextFullDirectoryInformation);
                            }

                            break;
                        }

                    case NamespaceCifs.FindInformationLevel.SMB_FIND_FILE_BOTH_DIRECTORY_INFO:
                        {
                            if (smbTrans2FindNext2Packet.SmbData.Trans2_Data.Length != uint.MinValue)
                            {
                                NamespaceFscc.FsccFileBothDirectoryInformationResponsePacket
                                    findNextFsccBothDirectoryPacket =
                                    new NamespaceFscc.FsccFileBothDirectoryInformationResponsePacket();

                                findNextFsccBothDirectoryPacket.FromBytes(smbTrans2FindNext2Packet.SmbData.Trans2_Data);

                                NamespaceFscc.FileBothDirectoryInformation findNextBothDirectoryInformation =
                                    new NamespaceFscc.FileBothDirectoryInformation();

                                findNextBothDirectoryInformation = findNextFsccBothDirectoryPacket.Payload;
                                if (findNextBothDirectoryInformation.FileNameLength == uint.MinValue)
                                {
                                    isFileIDEqualZero = true;
                                }

                                if (this.smbClientStack.Capability.IsSupportsExtendedSecurity == true)
                                {
                                    VerifyMessageSyntaxSmbFindFileBothDirectoryInfo(findNextBothDirectoryInformation);
                                }
                            }

                            break;
                        }

                    case NamespaceCifs.FindInformationLevel.SMB_FIND_FILE_DIRECTORY_INFO:
                        {
                            if (SmbAdapter.fileSystemType == FileSystemType.Fat)
                            {
                                isFileIDEqualZero = true;
                            }
                            else
                            {
                                isFileIDEqualZero = false;
                            }

                            break;
                        }
                }

                this.Trans2FindNext2Response(
                    smtTrans2FindNext2ResponseHeader.Mid + this.addMidMark,
                    this.QueryUidTable(smbPacketResponse),
                    this.QueryTidTable(smbPacketResponse),
                    (smbPacketResponse).IsSignRequired,
                    isFileIDEqualZero,
                    (MessageStatus)smtTrans2FindNext2ResponseHeader.Status);
            }

            #endregion
        }


        /// <summary>
        /// Check previous version.
        /// </summary>
        /// <param name="fid">The file identifier.</param>
        /// <param name="previousVersion">The previous version.</param>
        /// <param name="isSucceed">Indicate whether the checking is successful or not.</param>
        public void CheckPreviousVersion(int fid, Microsoft.Modeling.Set<int> previousVersion, out bool isSucceed)
        {
            string streamString = Site.Properties["SmbTestCasePreviousVersion"] as string;
            if (string.IsNullOrEmpty(streamString))
            {
                isSucceed = false;
                return;
            }

            int startIndex = (int)uint.MinValue;
            while (streamString.IndexOf(Semicolon, startIndex) >= uint.MinValue)
            {
                int endIndex = streamString.IndexOf(Semicolon, startIndex);

                string gmtToken = streamString.Substring(startIndex, endIndex - startIndex);
                int gmtIndex = Int32.Parse(gmtToken.Substring(0, gmtToken.IndexOf(Comma)));
                string gmtString = gmtToken.Substring(gmtToken.IndexOf(Comma) + 1);

                if (previousVersion.Contains(gmtIndex))
                {
                    this.gmtTokens.Add(gmtIndex, gmtString);
                }

                startIndex = endIndex + 1;
            }

            isSucceed = true;
        }


        /// <summary>
        /// CheckSnapshots Client Request.
        /// </summary>
        /// <param name="fid">The file identifier.</param>
        /// <param name="snapShots">snapShots.</param>
        /// <param name="isSucceed">If it succeeds, true is returned; Otherwise, false is returned.</param>
        public void CheckSnapshots(int fid, Microsoft.Modeling.Set<int> snapShots, out bool isSucceed)
        {
            isSucceed = true;
        }


        /// <summary>
        /// NT_TRANSACT_QUERY_QUOTA Client Request.
        /// </summary>
        /// <param name="messageId">This is used to associate a response with a request.</param>
        /// <param name="sessionId">
        /// Set this value to 0 to request a new session setup, or set this value to a previously established session 
        /// identifier to request reauthenticate to an existing session.
        /// </param>
        /// <param name="treeId">
        /// This field identifies the subdirectory (or tree) (also referred as a share in this document) on the 
        /// server that the client is accessing.
        /// </param>
        /// <param name="isSigned">
        /// Indicate whether the SUT has message signing enabled or required.
        /// </param>
        /// <param name="fid">The file identifier.</param>
        /// <param name="isReturnSingle">
        /// A bool variable, if set, which indicates only a single entry is to be returned instead of filling the 
        /// entire buffer.
        /// </param>
        /// <param name="isRestartScan">
        /// A bool variable, if set, which indicates that the scan of the quota information is to be restarted.
        /// </param>
        /// <param name="sidListLength">Supplies the length in bytes of the SidList.</param>
        /// <param name="startSidLength">Supplies the length in bytes of the StartSid.</param>
        /// <param name="startSidOffset">
        /// Supplies the offset, in bytes, to the StartSid in the Parameter buffer.
        /// </param>
        public void NtTransQueryQuotaRequest(
            int messageId,
            int sessionId,
            int treeId,
            bool isSigned,
            int fid,
            bool isReturnSingle,
            bool isRestartScan,
            int sidListLength,
            int startSidLength,
            int startSidOffset)
        {
            #region Create Pcaket

            SmbNtTransQueryQuotaRequestPacket smbPacket = new SmbNtTransQueryQuotaRequestPacket();

            ushort uid = (ushort)this.uId[(uint)sessionId];
            ushort fileId = (ushort)this.fId[(uint)fid];

            this.IsReturnSingleEntry = isReturnSingle;

            smbPacket = this.smbClientStack.CreateNTTransQueryQuotaRequest(
                fileId,
                this.IsReturnSingleEntry,
                isRestartScan,
                sidListLength,
                startSidLength,
                startSidOffset);

            if (isSigned)
            {
                NamespaceCifs.CifsClientPerConnection connection =
                    this.smbClientStack.Context.GetConnection(ConnectionId);

                NamespaceCifs.CifsClientPerSession session =
                    this.smbClientStack.Context.GetSession(ConnectionId, uid);

                smbPacket.Sign(connection.ClientNextSendSequenceNumber, session.SessionKey);
            }

            #endregion

            #region Send and Receive ExpectPacket

            this.smbClientStack.SendPacket(smbPacket);
            StackPacket response = this.smbClientStack.ExpectPacket(this.timeout);

            NamespaceCifs.SmbPacket smbPacketResponse = (NamespaceCifs.SmbPacket)response;

            this.QueryUidTable(smbPacketResponse);
            this.QueryTidTable(smbPacketResponse);

            VerifyTransport(smbPacketResponse);
            VerifyCommonMessageSyntax(smbPacketResponse);

            if (response.GetType() == typeof(SmbErrorResponsePacket))
            {
                SmbErrorResponsePacket smbErrorResponsePacket = response as SmbErrorResponsePacket;
                NamespaceCifs.SmbHeader smbErrorHeader = smbErrorResponsePacket.SmbHeader;

                this.ErrorResponse(
                    smbErrorHeader.Mid + this.addMidMark,
                    (MessageStatus)smbErrorHeader.Status);
            }
            else
            {
                SmbNtTransQueryQuotaResponsePacket smbNtTransQueryQuotaPacket
                    = response as SmbNtTransQueryQuotaResponsePacket;

                NamespaceCifs.SmbHeader ntTransQueryQuotaResponseHeader = smbNtTransQueryQuotaPacket.SmbHeader;
                int quotaUsed = (int)uint.MinValue;
                if (IsQueryQuotaFirstResponse)
                {
                    this.NtTransQueryQuotaResponse(
                        ntTransQueryQuotaResponseHeader.Mid + this.addMidMark,
                        this.QueryUidTable(smbPacketResponse),
                        this.QueryTidTable(smbPacketResponse),
                        (smbPacketResponse).IsSignRequired,
                        quotaUsed,
                        (MessageStatus)ntTransQueryQuotaResponseHeader.Status);
                    IsQueryQuotaFirstResponse = false;
                }

                VerifyNtTransQueryQuotaRequestAndResponse(this.IsReturnSingleEntry, smbNtTransQueryQuotaPacket);

                int quotaInfoCount = Convert.ToInt32(Site.Properties["QuotaInfoCount"]);

                VerifyMessageSyntaxNtTransactQueryQuotaResponse(
                    smbNtTransQueryQuotaPacket,
                    sidListLength,
                    startSidLength,
                    quotaInfoCount,
                    smbPacket.SmbParameters.MaxDataCount);

                this.IsReturnSingleEntry = false;
            }

            #endregion
        }


        /// <summary>
        /// NT_TRANSACT_SET_QUOTA Client Request.
        /// </summary>
        /// <param name="messageId">This is used to associate a response with a request.</param>
        /// <param name="sessionId">
        /// Set this value to 0 to request a new session setup, or set this value to a previously established session 
        /// identifier to request reauthenticate to an existing session.
        /// </param>
        /// <param name="treeId">
        /// This field identifies the subdirectory (or tree) (also referred as a share in this document) on the 
        /// server that the client is accessing.
        /// </param>
        /// <param name="fid">The file identifier.</param>
        /// <param name="isSigned">
        /// Indicate whether the SUT has message signing enabled or required.
        /// </param>
        /// <param name="quotaInfo">The amount of quota, in bytes, used by this user.</param>
        public void NtTransSetQuotaRequest(
            int messageId,
            int sessionId,
            int treeId,
            int fid,
            bool isSigned,
            int quotaUsed)
        {
            #region Create Packet

            SmbNtTransSetQuotaRequestPacket smbPacket = new SmbNtTransSetQuotaRequestPacket();

            ushort uid = (ushort)this.uId[(uint)sessionId];
            ushort fileId = (ushort)this.fId[(uint)fid];
            string sid = Site.Properties["SutLoginAdminUserFullPathName"];
            byte[] sidByte = this.GetSid(sid);
            uint nextEntryOffset = uint.MinValue;
            ulong changeTime = uint.MinValue;

            smbPacket = this.smbClientStack.CreateNTTransSetQuotaRequest(
                fileId,
                nextEntryOffset,
                changeTime,
                (ulong)quotaUsed,
                this.quotaThreshold,
                this.quotalimit,
                sidByte);

            if (isSigned)
            {
                NamespaceCifs.CifsClientPerConnection connection =
                    this.smbClientStack.Context.GetConnection(ConnectionId);

                NamespaceCifs.CifsClientPerSession session =
                    this.smbClientStack.Context.GetSession(ConnectionId, uid);

                smbPacket.Sign(connection.ClientNextSendSequenceNumber, session.SessionKey);
            }

            #endregion

            #region Send and Receive ExpectPacket

            this.smbClientStack.SendPacket(smbPacket);
            StackPacket response = this.smbClientStack.ExpectPacket(this.timeout);

            NamespaceCifs.SmbPacket smbPacketResponse = (NamespaceCifs.SmbPacket)response;

            this.QueryUidTable(smbPacketResponse);
            this.QueryTidTable(smbPacketResponse);

            VerifyTransport(smbPacketResponse);
            VerifyCommonMessageSyntax(smbPacketResponse);

            if (response.GetType() == typeof(SmbErrorResponsePacket))
            {
                SmbErrorResponsePacket smbErrorResponsePacket = response as SmbErrorResponsePacket;
                NamespaceCifs.SmbHeader smbErrorHeader = smbErrorResponsePacket.SmbHeader;
                this.ErrorResponse(smbErrorHeader.Mid + this.addMidMark, (MessageStatus)smbErrorHeader.Status);
            }
            else
            {
                SmbNtTransSetQuotaResponsePacket smbNtTransSetQuotaPacket
                    = response as SmbNtTransSetQuotaResponsePacket;
                NamespaceCifs.SmbHeader ntTransSetQuotaResponseHeader = smbNtTransSetQuotaPacket.SmbHeader;

                VerifyMessageSyntaxNtTransactSetQuotaRequest(
                    smbNtTransSetQuotaPacket,
                    ntTransSetQuotaResponseHeader.Status);

                this.NtTransSetQuotaResponse(
                    ntTransSetQuotaResponseHeader.Mid + this.addMidMark,
                    this.QueryUidTable(smbPacketResponse),
                    this.QueryTidTable(smbPacketResponse),
                    (smbPacketResponse).IsSignRequired,
                    (MessageStatus)ntTransSetQuotaResponseHeader.Status);
            }

            #endregion
        }


        /// <summary>
        /// FSCTL_SRV_ENUMERATE_SNAPSHOTS Request.
        /// </summary>
        /// <param name="messageId">This is used to associate a response with a request.</param>
        /// <param name="sessionId">
        /// Set this value to 0 to request a new session setup, or set this value to a previously established session 
        /// identifier to request reauthenticate to an existing session.
        /// </param>
        /// <param name="treeId">
        /// This field identifies the subdirectory (or tree) (also referred as a share in this document) on the 
        /// server that the client is accessing.
        /// </param>
        /// <param name="isSigned">
        /// Indicate whether the SUT has message signing enabled or required.
        /// </param>
        /// <param name="fid">The file identifier.</param>
        /// <param name="fsctlMaxDataCount">used to control the MaxDataCount in FSCTL_SRV_ENUMERATE_SNAPSHOTS.</param>
        public void FsctlSrvEnumSnapshotsRequest(
            int messageId,
            int sessionId,
            int treeId,
            bool isSigned,
            int fid,
            MaxDataCount fsctlMaxDataCount)
        {
            #region Create Packet

            SmbNtTransFsctlSrvEnumerateSnapshotsRequestPacket smbPacket
                = new SmbNtTransFsctlSrvEnumerateSnapshotsRequestPacket();

            ushort uid = (ushort)this.uId[(uint)sessionId];

            int maxDataCountSize = (int)uint.MinValue;
            int maxLevel = (int)fsctlMaxDataCount;

            if (maxLevel == (int)MaxDataCount.VerySmall)
            {
                maxDataCountSize = VerySmall;
            }
            else if (maxLevel == (int)MaxDataCount.Mid)
            {
                maxDataCountSize = Mid;
            }
            else if (maxLevel == (int)MaxDataCount.VeryLarge)
            {
                maxDataCountSize = VeryLarge;
            }

            byte isFlags = byte.MinValue;

            smbPacket = this.smbClientStack.CreateNTTransIOCtlEnumerateSnapShotsRequest(
                (ushort)this.fId[(uint)fid],
                true,
                isFlags,
                maxDataCountSize);

            if (isSigned)
            {
                NamespaceCifs.CifsClientPerConnection connection =
                    this.smbClientStack.Context.GetConnection(ConnectionId);

                NamespaceCifs.CifsClientPerSession session =
                    this.smbClientStack.Context.GetSession(ConnectionId, uid);

                smbPacket.Sign(connection.ClientNextSendSequenceNumber, session.SessionKey);
            }

            #endregion

            #region Send and Receive ExpectPacket

            this.smbClientStack.SendPacket(smbPacket);
            StackPacket response = this.smbClientStack.ExpectPacket(this.timeout);

            NamespaceCifs.SmbPacket smbPacketResponse = (NamespaceCifs.SmbPacket)response;

            this.QueryUidTable(smbPacketResponse);
            this.QueryTidTable(smbPacketResponse);

            VerifyTransport(smbPacketResponse);
            VerifyCommonMessageSyntax(smbPacketResponse);

            if (response.GetType() == typeof(SmbErrorResponsePacket))
            {
                SmbErrorResponsePacket smbErrorResponsePacket = response as SmbErrorResponsePacket;
                NamespaceCifs.SmbHeader smbErrorHeader = smbErrorResponsePacket.SmbHeader;
                this.ErrorResponse(smbErrorHeader.Mid + this.addMidMark, (MessageStatus)smbErrorHeader.Status);
            }
            else
            {
                SmbNtTransFsctlSrvEnumerateSnapshotsResponsePacket smbFsctlSrvEnumerateSnapshotPacket
                    = response as SmbNtTransFsctlSrvEnumerateSnapshotsResponsePacket;

                NamespaceCifs.SmbHeader ntTransFsctlSrvEnumerateSnapshotsResponseHeader
                    = smbFsctlSrvEnumerateSnapshotPacket.SmbHeader;

                NT_TRANSACT_ENUMERATE_SNAPSHOTS_Response_NT_Trans_Data ntTransFsctlSrvEnumerateSnapshotsResponsePayloadHeader
                    = smbFsctlSrvEnumerateSnapshotPacket.NtTransData;

                messageId = ntTransFsctlSrvEnumerateSnapshotsResponseHeader.Mid;
                MessageStatus messageStatus = (MessageStatus)ntTransFsctlSrvEnumerateSnapshotsResponseHeader.Status;

                int numberOfSnapShots = (int)ntTransFsctlSrvEnumerateSnapshotsResponsePayloadHeader.NumberOfSnapShots;

                int numberOfSnapShotsReturned =
                    (int)ntTransFsctlSrvEnumerateSnapshotsResponsePayloadHeader.NumberOfSnapShotsReturned;

                IntegerCompare numberOfSnapShotsCompared;

                if (numberOfSnapShotsReturned < numberOfSnapShots)
                {
                    numberOfSnapShotsCompared = IntegerCompare.Smaller;
                }
                else if (numberOfSnapShotsReturned > numberOfSnapShots)
                {
                    numberOfSnapShotsCompared = IntegerCompare.Larger;
                }
                else
                {
                    numberOfSnapShotsCompared = IntegerCompare.Equal;
                }

                VerifyMessageSyntaxFsctlSrvEnumerateSnapshotsRequest(smbFsctlSrvEnumerateSnapshotPacket);

                int expectedSnapShotsCount = Int32.Parse(Site.Properties["ExpectedSnapShotsCount"]);
                int expectedSnapshotsReturnedCount = Int32.Parse(Site.Properties["ExpectedSnapshotsReturnedCount"]);
                bool isMaxDataCountLargeEnough = false;

                if (fsctlMaxDataCount == MaxDataCount.Mid)
                {
                    isMaxDataCountLargeEnough = false;
                }
                else
                {
                    isMaxDataCountLargeEnough = true;
                }

                VerifyReceiveFsctlSrvEnumerateSnapshotsFunctionCode(
                    smbFsctlSrvEnumerateSnapshotPacket,
                    expectedSnapShotsCount,
                    expectedSnapshotsReturnedCount,
                    isMaxDataCountLargeEnough);

                isMaxDataCountLargeEnough = false;
                this.FsctlSrvEnumSnapshotsResponse(
                    messageId + this.addMidMark,
                    this.QueryUidTable(smbPacketResponse),
                    this.QueryTidTable(smbPacketResponse),
                    (smbPacketResponse).IsSignRequired,
                    messageStatus,
                    numberOfSnapShotsCompared);
            }

            #endregion
        }


        /// <summary>
        /// FSCTL_SRV_REQUEST_RESUME_KEY Request.
        /// </summary>
        /// <param name="messageId">This is used to associate a response with a request.</param>
        /// <param name="sessionId">
        /// Set this value to 0 to request a new session setup, or set this value to a previously established session 
        /// identifier to request reauthenticate to an existing session.
        /// </param>
        /// <param name="treeId">
        /// This field identifies the subdirectory (or tree) (also referred as a share in this document) on the 
        /// server that the client is accessing.
        /// </param>
        /// <param name="isSigned">
        /// Indicate whether the SUT has message signing enabled or required.
        /// </param>
        /// <param name="fid">The file identifier.</param>
        public void FsctlSrvRequestResumeKeyRequest(int messageId, int sessionId, int treeId, bool isSigned, int fid)
        {
            #region Create Packet

            this.isCopyChunkWrite = true;

            this.WriteRequest(
                messageId,
                sessionId,
                treeId,
                fid,
                ShareType.NamedPipe,
                isSigned,
                (int)uint.MinValue);

            SmbNtTransFsctlSrvRequestResumeKeyRequestPacket smbPacket
                = new SmbNtTransFsctlSrvRequestResumeKeyRequestPacket();

            ushort uid = (ushort)this.uId[(uint)sessionId];

            smbPacket = this.smbClientStack.CreateNTTransIOCtlRequestResumeKeyRequest(
                (ushort)this.fId[(uint)fid],
                true,
                byte.MinValue);

            if (isSigned)
            {
                NamespaceCifs.CifsClientPerConnection connection =
                    this.smbClientStack.Context.GetConnection(ConnectionId);

                NamespaceCifs.CifsClientPerSession session = this.smbClientStack.Context.GetSession(ConnectionId, uid);

                smbPacket.Sign(connection.ClientNextSendSequenceNumber, session.SessionKey);
            }

            #endregion

            #region Send and Receive ExpectPacket

            this.smbClientStack.SendPacket(smbPacket);
            StackPacket response = this.smbClientStack.ExpectPacket(this.timeout);

            NamespaceCifs.SmbPacket smbPacketResponse = (NamespaceCifs.SmbPacket)response;

            this.QueryUidTable(smbPacketResponse);
            this.QueryTidTable(smbPacketResponse);

            VerifyTransport(smbPacketResponse);
            VerifyCommonMessageSyntax(smbPacketResponse);

            if (response.GetType() == typeof(SmbErrorResponsePacket))
            {
                SmbErrorResponsePacket smbErrorResponsePacket = response as SmbErrorResponsePacket;
                NamespaceCifs.SmbHeader smbErrorHeader = smbErrorResponsePacket.SmbHeader;
                this.ErrorResponse(smbErrorHeader.Mid + this.addMidMark, (MessageStatus)smbErrorHeader.Status);
            }
            else
            {
                if (this.isCopyChunkWrite)
                {
                    this.isCopyChunkWrite = false;
                    this.addMidMark--;
                }

                SmbNtTransFsctlSrvRequestResumeKeyResponsePacket smbFsctlSrvRequestResumeKeyPacket
                    = response as SmbNtTransFsctlSrvRequestResumeKeyResponsePacket;

                NamespaceCifs.SmbHeader ntTransFsctlSrvRequestResumeKeyResponseHeader
                    = smbFsctlSrvRequestResumeKeyPacket.SmbHeader;

                SmbAdapter.resumeKey = smbFsctlSrvRequestResumeKeyPacket.NtTransData.ResumeKey;

                VerifyMessageSyntaxCopychunkResumeKey(smbFsctlSrvRequestResumeKeyPacket.NtTransData.ResumeKey);
                VerifyMessageSyntaxFsctlSrvRequestResumeKeyResponse(smbFsctlSrvRequestResumeKeyPacket);
                verifyFsctlSrvRequestResumeKeyFunctionCode(smbFsctlSrvRequestResumeKeyPacket);

                this.FsctlSrvRequestResumeKeyResponse(
                    ntTransFsctlSrvRequestResumeKeyResponseHeader.Mid + this.addMidMark,
                    this.QueryUidTable(smbPacketResponse),
                    this.QueryTidTable(smbPacketResponse),
                    (smbPacketResponse).IsSignRequired,
                    CopyChunkResumeKey,
                    (MessageStatus)ntTransFsctlSrvRequestResumeKeyResponseHeader.Status);
            }

            #endregion
        }


        /// <summary>
        /// NT_TRANSACT_IOCTL Client Request.
        /// </summary>
        /// <param name="messageId">Message ID used to identify the message.</param>
        /// <param name="sessionId">Session ID used to identify the session.</param>
        /// <param name="treeId">Tree ID used to identify the tree connection.</param>
        /// <param name="isSigned">
        /// Indicate whether the SUT has message signing enabled or required.
        /// </param>
        /// <param name="fid">The file identifier.</param>
        /// <param name="copychunkResumeKeysize">The server resume key for a source file.</param>
        /// <param name="sourceOffset">The offset in the source file to copy from.</param>
        /// <param name="targetOffset">The offset in the target file to copy to.</param>
        /// <param name="length">The number of bytes to copy from the source file to the target file.</param>
        public void FsctlSrvCopyChunkRequest(
            int messageId,
            int sessionId,
            int treeId,
            bool isSigned,
            int fid,
            string copychunkResumeKeysize,
            int sourceOffset,
            int targetOffset,
            int length)
        {
            #region Create Packet

            SmbNtTransFsctlSrvCopyChunkRequestPacket smbPacket = new SmbNtTransFsctlSrvCopyChunkRequestPacket();
            ushort uid = (ushort)this.uId[(uint)sessionId];

            byte[] copyChunkResumeKey = null;
            if (copychunkResumeKeysize == CopyChunkResumeKey)
            {
                copyChunkResumeKey = SmbAdapter.resumeKey;
            }
            else
            {
                copyChunkResumeKey = new byte[24];
                for (int i = 0; i < 24; i++)
                {
                    copyChunkResumeKey[i] = 0x10;
                }
            }

            byte isFlags = byte.MinValue;
            NT_TRANSACT_COPY_CHUNK_List list = new NT_TRANSACT_COPY_CHUNK_List();
            list.Length = (uint)this.copyWordCount;
            list.SourceOffset = (ulong)sourceOffset;
            list.DestinationOffset = (ulong)targetOffset;

            smbPacket = this.smbClientStack.CreateNTTransIOCtlCopyChunkRequest(
                (ushort)this.fId[(uint)fid],
                true,
                isFlags,
                copyChunkResumeKey,
                list);

            if (isSigned)
            {
                NamespaceCifs.CifsClientPerConnection connection =
                    this.smbClientStack.Context.GetConnection(ConnectionId);

                NamespaceCifs.CifsClientPerSession session =
                    this.smbClientStack.Context.GetSession(ConnectionId, uid);

                smbPacket.Sign(connection.ClientNextSendSequenceNumber, session.SessionKey);
            }

            #endregion

            #region Send and Receive ExpectPacket

            this.smbClientStack.SendPacket(smbPacket);
            StackPacket response = this.smbClientStack.ExpectPacket(this.timeout);

            NamespaceCifs.SmbPacket smbPacketResponse = (NamespaceCifs.SmbPacket)response;
            this.QueryUidTable(smbPacketResponse);
            this.QueryTidTable(smbPacketResponse);

            VerifyTransport(smbPacketResponse);
            VerifyCommonMessageSyntax(smbPacketResponse);

            if (response.GetType() == typeof(SmbErrorResponsePacket))
            {
                SmbErrorResponsePacket smbErrorResponsePacket = response as SmbErrorResponsePacket;
                NamespaceCifs.SmbHeader smbErrorHeader = smbErrorResponsePacket.SmbHeader;
                this.ErrorResponse(smbErrorHeader.Mid + this.addMidMark, (MessageStatus)smbErrorHeader.Status);
            }
            else
            {
                SmbNtTransFsctlSrvCopyChunkResponsePacket smbFsctlSrvCopyChunkPacket
                    = response as SmbNtTransFsctlSrvCopyChunkResponsePacket;

                NamespaceCifs.SmbHeader ntTransFsctlSrvCopyChunkResponseHeader = smbFsctlSrvCopyChunkPacket.SmbHeader;

                VerifyMessageSyntaxFsctlSrvCopychunkResponse(smbFsctlSrvCopyChunkPacket);

                this.FsctlSrvCopyChunkResponse(
                    ntTransFsctlSrvCopyChunkResponseHeader.Mid + this.addMidMark,
                    this.QueryUidTable(smbPacketResponse),
                    this.QueryTidTable(smbPacketResponse),
                    (smbPacketResponse).IsSignRequired,
                    (MessageStatus)ntTransFsctlSrvCopyChunkResponseHeader.Status);
            }

            #endregion
        }


        /// <summary>
        /// FSCTL Bad command request.
        /// </summary>
        /// <param name="messageId">This is used to associate a response with a request.</param>
        /// <param name="sessionId">The current session ID for this connection.</param>
        /// <param name="treeId">The tree ID for the corrent share connection.</param>
        /// <param name="isSigned">Indicate whether the message is signed or not for this response.</param>
        /// <param name="fid">The file identifier.</param>
        /// <param name="command">This is used to tell the adapter to send an invalid kind of command.</param>
        public void FsctlBadCommandRequest(
            int messageId,
            int sessionId,
            int treeId,
            bool isSigned,
            int fid,
            FsctlInvalidCommand command)
        {
            #region Create Packet

            NamespaceCifs.SmbNtTransactIoctlRequestPacket smbPacket = new NamespaceCifs.SmbNtTransactIoctlRequestPacket();

            ushort uid = (ushort)this.uId[(uint)sessionId];
            uint functionCode = (uint)command;
            byte[] data = new byte[DataCount];

            smbPacket = this.smbClientStack.CreateNTTransIOCtlRequest(
                (ushort)this.fId[(uint)fid],
                true,
                byte.MinValue,
                functionCode,
                data);

            if (isSigned)
            {
                NamespaceCifs.CifsClientPerConnection connection =
                    this.smbClientStack.Context.GetConnection(ConnectionId);

                NamespaceCifs.CifsClientPerSession session = this.smbClientStack.Context.GetSession(ConnectionId, uid);

                smbPacket.Sign(connection.ClientNextSendSequenceNumber, session.SessionKey);
            }

            #endregion

            #region Send and Receive ExpectPacket

            this.smbClientStack.SendPacket(smbPacket);
            StackPacket response = this.smbClientStack.ExpectPacket(this.timeout);

            NamespaceCifs.SmbPacket smbPacketResponse = (NamespaceCifs.SmbPacket)response;

            this.QueryUidTable(smbPacketResponse);
            this.QueryTidTable(smbPacketResponse);

            VerifyTransport(smbPacketResponse);
            VerifyCommonMessageSyntax(smbPacketResponse);

            SmbErrorResponsePacket smbErrorResponsePacket = response as SmbErrorResponsePacket;
            NamespaceCifs.SmbHeader smbErrorHeader = smbErrorResponsePacket.SmbHeader;
            this.ErrorResponse(smbErrorHeader.Mid + this.addMidMark, (MessageStatus)smbErrorHeader.Status);

            #endregion
        }


        /// <summary>
        /// Transfer create request.
        /// </summary>
        /// <param name="messageId">This is used to associate a response with a request.</param>
        /// <param name="sessionId">Set this value to 0 to request a new session setup, or set this value to a 
        /// previously established session identifier to reauthenticate to an existing session.
        /// </param>
        /// <param name="treeId">
        /// This field identifies the subdirectory (or tree) (also referred as a sharein this document) on the server 
        /// that the client is accessing.
        /// </param>
        /// <param name="impersonationLevel">The impersonation level.</param>
        /// <param name="fileName">The file name that will be created.</param>
        /// <param name="shareType">The share type.</param>
        /// <param name="isSigned">
        /// Indicate whether the SUT has message signing enabled or required.
        /// </param>
        /// Disable warning CA1506 because according to Test Case design, excessive class coupling is necessary.
        [SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling")]
        public void NtTransactCreateRequest(
            int messageId,
            int sessionId,
            int treeId,
            [Domain("ImpersonationLevel")] int impersonationLevel,
            [Domain("FileDomain")] string fileName,
            ShareType shareType,
            bool isSigned)
        {
            #region Create Packet

            SmbNtCreateAndxRequestPacket smbPacket = new SmbNtCreateAndxRequestPacket();

            ushort uid = (ushort)this.uId[(uint)sessionId];
            ushort tid = (ushort)this.tId[(uint)treeId];
            string shareName = string.Empty;
            if (shareType == ShareType.NamedPipe)
            {
                if (SmbAdapter.isMessageModePipe)
                {
                    shareName = Site.Properties["SutShareMessagePipe"];
                }
                else
                {
                    shareName = Site.Properties["SutShareStreamPipe"];
                }
            }
            else
            {
                if (fileName == Site.Properties["SutShareTest1"])
                {
                    shareName = Site.Properties["SutShareTest1"];
                }
                else if (fileName == Site.Properties["SutShareTest2"])
                {
                    shareName = Site.Properties["SutShareTest2"];
                }
                else if (fileName == Site.Properties["SutShareExistFile"])
                {
                    shareName = Site.Properties["SutShareExistFile"];
                }
                else if (fileName.Contains("."))
                {
                    shareName = fileName;
                }
                else
                {
                    shareName = fileName;
                }
            }

            this.openedFileName = shareName;
            uint nTDesiredAccess = (uint)NamespaceCifs.NtTransactDesiredAccess.GENERIC_ALL;

            NamespaceCifs.SMB_EXT_FILE_ATTR extFileAttributes = NamespaceCifs.SMB_EXT_FILE_ATTR.NONE;

            NtTransactCreateOptions createOption = NtTransactCreateOptions.FILE_NON_DIRECTORY_FILE;

            smbPacket = this.smbClientStack.CreateCreateRequest(
                tid,
                shareName,
                (NamespaceCifs.NtTransactDesiredAccess)nTDesiredAccess,
                extFileAttributes,
                NamespaceCifs.NtTransactShareAccess.NONE,
                NamespaceCifs.NtTransactCreateDisposition.FILE_OPEN_IF,
                createOption,
                (NamespaceCifs.NtTransactImpersonationLevel)impersonationLevel,
                CreateFlags.NT_CREATE_REQUEST_OPLOCK);

            if (isSigned)
            {
                NamespaceCifs.CifsClientPerConnection connection =
                    this.smbClientStack.Context.GetConnection(ConnectionId);

                NamespaceCifs.CifsClientPerSession session = this.smbClientStack.Context.GetSession(ConnectionId, uid);

                smbPacket.Sign(connection.ClientNextSendSequenceNumber, session.SessionKey);
            }

            #endregion

            #region Send and Receive ExpectPacket

            this.smbClientStack.SendPacket(smbPacket);
            StackPacket response = this.smbClientStack.ExpectPacket(this.timeout);

            NamespaceCifs.SmbPacket smbPacketResponse = (NamespaceCifs.SmbPacket)response;

            this.QueryUidTable(smbPacketResponse);
            this.QueryTidTable(smbPacketResponse);

            VerifyTransport(smbPacketResponse);
            VerifyCommonMessageSyntax(smbPacketResponse);

            if (response.GetType() == typeof(SmbErrorResponsePacket))
            {
                SmbErrorResponsePacket smbErrorResponsePacket = response as SmbErrorResponsePacket;
                NamespaceCifs.SmbHeader smbErrorHeader = smbErrorResponsePacket.SmbHeader;
                this.ErrorResponse(smbErrorHeader.Mid + this.addMidMark, (MessageStatus)smbErrorHeader.Status);
            }
            else
            {
                SmbNtCreateAndxResponsePacket smbNtCreateAndXPacket = response as SmbNtCreateAndxResponsePacket;
                NamespaceCifs.SmbHeader ntCreateAndXResponseHeader = smbNtCreateAndXPacket.SmbHeader;

                SMB_COM_NT_CREATE_ANDX_Response_SMB_Parameters ntCreateAndXResponsePayload
                    = smbNtCreateAndXPacket.SmbParameters;

                this.smbFileId = ntCreateAndXResponsePayload.FID;

                if (!this.fId.ContainsValue(ntCreateAndXResponsePayload.FID))
                {
                    int count = this.fId.Count;
                    this.fId.Add((uint)(count), ntCreateAndXResponsePayload.FID);
                }
                else
                {
                    foreach (uint key in this.fId.Keys)
                    {
                        if (this.fId[key] == ntCreateAndXResponsePayload.FID)
                        {
                            break;
                        }
                    }
                }

                uint creatAction = ntCreateAndXResponsePayload.CreationAction;
                Microsoft.Modeling.Set<CreateAction> actionSet = new Microsoft.Modeling.Set<CreateAction>();
                if (creatAction == CreateActionSuperseded)
                {
                    actionSet = actionSet.Add(CreateAction.FileSuperseded);
                }

                if ((creatAction & CreateActionExists) == CreateActionExists)
                {
                    actionSet = actionSet.Add(CreateAction.FileExists);
                }

                if ((creatAction & CreateActionOpened) == CreateActionOpened)
                {
                    actionSet = actionSet.Add(CreateAction.FileOpened);
                    if (!actionSet.Contains(CreateAction.FileExists))
                    {
                        actionSet = actionSet.Add(CreateAction.FileExists);
                    }
                }

                if ((creatAction & CreateActionNotExists) == CreateActionNotExists)
                {
                    actionSet = actionSet.Add(CreateAction.FileDoesNotExist);
                }

                if ((creatAction & CreateActionCreated) == CreateActionCreated)
                {
                    actionSet = actionSet.Add(CreateAction.FileCreated);
                    if (!actionSet.Contains(CreateAction.FileDoesNotExist))
                    {
                        actionSet = actionSet.Add(CreateAction.FileDoesNotExist);
                    }
                }

                if ((creatAction & CreateActionOverwritten) == CreateActionOverwritten)
                {
                    actionSet = actionSet.Add(CreateAction.FileOverwritten);
                }

                bool isIDZero = false;
                bool isVolumnGuidZero = false;
                if (!this.checkWindowsImplementation)
                {
                    Site.Log.Add(
                        LogEntryKind.Comment,
                        @"isFileIdZero: {0};
                        isVolumnGUIDZero: {1}",
                        isIDZero,
                        isVolumnGuidZero);
                    isIDZero = true;
                    isVolumnGuidZero = true;
                }
                //Workaround temp code (Invalid impersonation level)
                if ((string.Equals(Site.Properties["SutPlatformOS"], "Win2K8") && impersonationLevel == 4))
                {
                    this.ErrorResponse(ntCreateAndXResponseHeader.Mid + this.addMidMark, MessageStatus.BadImpersonationLevel);
                }
                else
                {
                    this.NtTransactCreateResponse(
                        ntCreateAndXResponseHeader.Mid + this.addMidMark,
                        QueryUidTable(smbPacketResponse),
                        QueryTidTable(smbPacketResponse),
                        isSigned,
                        (MessageStatus)ntCreateAndXResponseHeader.Status);
                }
            }

            #endregion
        }


        /// <summary>
        /// NTTrans set quota request additional.
        /// </summary>
        /// <param name="messageId">Message ID used to identify the message.</param>
        /// <param name="sessionId">
        /// Set this value to 0 to request a new session setup, or set this value to a previously established session 
        /// identifier to request reauthenticate to an existing session.
        /// </param>
        /// <param name="treeId">
        /// This field identifies the subdirectory (or tree) (also referred as a share in this document) on the 
        /// server that the client is accessing.
        /// </param>
        /// <param name="fid">The file identifier.</param>
        /// <param name="isSigned">
        /// Indicate whether the SUT has message signing enabled or required.
        /// </param>
        /// <param name="requestPara">NTTrans set quota request parameter.</param>
        public void NtTransSetQuotaRequestAdditional(
            int messageId,
            int sessionId,
            int treeId,
            int fid,
            bool isSigned,
            NtTransSetQuotaRequestParameter requestPara)
        {
            #region Create Packet

            SmbNtTransSetQuotaRequestPacket smbPacket = new SmbNtTransSetQuotaRequestPacket();

            ushort uid = (ushort)this.uId[(uint)sessionId];
            ushort fileId = (ushort)this.fId[(uint)fid];
            string sid = Site.Properties["SutLoginAdminUserFullPathName"];
            byte[] sidByte = this.GetSid(sid);
            uint nextEntryOffset = uint.MinValue;
            ulong quotaUsed = ulong.MinValue;
            ulong changeTime = ulong.MinValue;
            quotaUsed++;

            smbPacket = this.smbClientStack.CreateNTTransSetQuotaRequest(
                fileId,
                nextEntryOffset,
                changeTime,
                (ulong)quotaUsed,
                this.quotaThreshold,
                this.quotalimit,
                sidByte);

            if (isSigned)
            {
                NamespaceCifs.CifsClientPerConnection connection =
                    this.smbClientStack.Context.GetConnection(ConnectionId);

                NamespaceCifs.CifsClientPerSession session = this.smbClientStack.Context.GetSession(ConnectionId, uid);

                smbPacket.Sign(connection.ClientNextSendSequenceNumber, session.SessionKey);
            }

            bool isVaildFileId = true;
            if (requestPara == NtTransSetQuotaRequestParameter.FileIdErrror)
            {
                isVaildFileId = false;
            }

            if (!isVaildFileId)
            {
                NT_TRANSACT_SET_QUOTA_Request_NT_Trans_Parameters setQuotaParameters = smbPacket.NtTransParameters;
                setQuotaParameters.Fid = ushort.MaxValue;
                smbPacket.NtTransParameters = setQuotaParameters;
            }

            if ((requestPara == NtTransSetQuotaRequestParameter.AccessDenied))
            {
                NT_TRANSACT_SET_QUOTA_Request_NT_Trans_Data setQuotaData = smbPacket.NtTransData;
                setQuotaData.QuotaLimit = ulong.MaxValue;
                smbPacket.NtTransData = setQuotaData;
            }

            bool isVaildQuotaInfo = true;
            if (requestPara == NtTransSetQuotaRequestParameter.QuotaInfoError)
            {
                isVaildQuotaInfo = false;
            }

            if (!isVaildQuotaInfo)
            {
                NT_TRANSACT_SET_QUOTA_Request_NT_Trans_Data setQuotaData = smbPacket.NtTransData;
                setQuotaData.QuotaLimit = ulong.MaxValue;
                setQuotaData.QuotaThreshold = ulong.MaxValue;
                setQuotaData.QuotaUsed = ulong.MaxValue;
                setQuotaData.NextEntryOffset = uint.MaxValue;
                setQuotaData.ChangeTime = ulong.MaxValue;
                smbPacket.NtTransData = setQuotaData;
            }

            #endregion

            #region Send and Receive ExpectPacket

            this.smbClientStack.SendPacket(smbPacket);
            StackPacket response = this.smbClientStack.ExpectPacket(this.timeout);

            NamespaceCifs.SmbPacket smbPacketResponse = (NamespaceCifs.SmbPacket)response;

            this.QueryUidTable(smbPacketResponse);
            this.QueryTidTable(smbPacketResponse);

            VerifyTransport(smbPacketResponse);
            VerifyCommonMessageSyntax(smbPacketResponse);

            if (response.GetType() == typeof(SmbErrorResponsePacket))
            {
                SmbErrorResponsePacket smbErrorResponsePacket = response as SmbErrorResponsePacket;
                NamespaceCifs.SmbHeader smbErrorHeader = smbErrorResponsePacket.SmbHeader;
                
                if (!isVaildQuotaInfo)
                {
                    this.ErrorNtTransSetQuotaResponseAdditional(
                        smbErrorHeader.Mid + this.addMidMark,
                        MessageStatus.InvalidParameter);
                }
                
                else if ((requestPara == NtTransSetQuotaRequestParameter.FileIdErrror))
                {
                    this.ErrorNtTransSetQuotaResponseAdditional(
                        smbErrorHeader.Mid + this.addMidMark,
                        MessageStatus.StatusInvalidHandle);
                }
                else
                {
                    this.ErrorNtTransSetQuotaResponseAdditional(
                        smbErrorHeader.Mid + this.addMidMark,
                        (MessageStatus)smbErrorHeader.Status);
                }
            }
            else
            {
                SmbNtTransSetQuotaResponsePacket smbNtTransSetQuotaPacket
                    = response as SmbNtTransSetQuotaResponsePacket;
                NamespaceCifs.SmbHeader ntTransSetQuotaResponseHeader = smbNtTransSetQuotaPacket.SmbHeader;

                VerifyMessageSyntaxNtTransactSetQuotaRequest(
                    smbNtTransSetQuotaPacket,
                    ntTransSetQuotaResponseHeader.Status);

                if ((requestPara == NtTransSetQuotaRequestParameter.AccessDenied))
                {
                    this.ErrorNtTransSetQuotaResponseAdditional(
                        ntTransSetQuotaResponseHeader.Mid + this.addMidMark,
                        MessageStatus.AccessDenied);
                }
                else
                {
                    this.NtTransSetQuotaResponseAdditional(
                        ntTransSetQuotaResponseHeader.Mid + this.addMidMark,
                        this.QueryUidTable(smbPacketResponse),
                        this.QueryTidTable(smbPacketResponse),
                        (smbPacketResponse).IsSignRequired,
                        (MessageStatus)ntTransSetQuotaResponseHeader.Status);
                }
            }

            #endregion
        }


        /// <summary>
        /// TRANS2_SET_FS_INFORMATION request additional.
        /// </summary>
        /// <param name="messageId">This is used to associate a response with a request.</param>
        /// <param name="sessionId">
        /// Set this value to 0 to request a new session setup, or set this value to a previously established session 
        /// identifier to request reauthenticate to an existing session.
        /// </param>
        /// <param name="treeId">
        /// This field identifies the subdirectory (or tree) (also referred as a share in this document) on the 
        /// server that the client is accessing.
        /// </param>
        /// <param name="fid">The file identifier.</param>
        /// <param name="isSigned">
        /// Indicate whether the SUT has message signing enabled or required.
        /// </param>
        /// <param name="informationLevel">This can be used to query information from the server.</param>
        /// <param name="requestPara">Trans2SetFSInfo response parameter.</param>
        public void Trans2SetFsInfoRequestAdditional(
            int messageId,
            int sessionId,
            int treeId,
            int fid,
            bool isSigned,
            [Domain("InfoLevelSetByFS")] InformationLevel informationLevel,
            Trans2SetFsInfoResponseParameter requestPara)
        {
            #region Create Packet

            SmbTrans2SetFsInformationRequestPacket smbPacket = new SmbTrans2SetFsInformationRequestPacket();
            ushort uid = (ushort)this.uId[(uint)sessionId];
            ushort fileId = (ushort)this.fId[(uint)fid];

            NamespaceCifs.Trans2SmbParametersFlags transactOptions = NamespaceCifs.Trans2SmbParametersFlags.NONE;

            ushort level = ushort.MaxValue;
            if (informationLevel != InformationLevel.Invalid)
            {
                level = this.informationLevelBytes[(ushort)informationLevel];
                //This field MUST be a pass-through Information Level
                this.smbClientStack.Capability.IsUsePassThrough = true;
            }
            else
            {
                level = this.informationLevelBytes[(ushort)InformationLevel.FileFsControlInformaiton];
                //This field MUST be a pass-through Information Level
                this.smbClientStack.Capability.IsUsePassThrough = false;
            }

            byte[] data = new byte[DataCount];

            smbPacket = this.smbClientStack.CreateTrans2SetFileSystemInformationRequest(
                fileId,
                transactOptions,
                (NamespaceCifs.QueryFSInformationLevel)level,
                data);

            bool isInvalidFileId = false;
            if (requestPara == Trans2SetFsInfoResponseParameter.FileIdErrror)
            {
                isInvalidFileId = true;
            }

            if (isInvalidFileId)
            {
                TRANS2_SET_FILE_SYSTEM_INFORMATION_Request_Trans2_Parameters trans2Parameters =
                    smbPacket.Trans2Parameters;

                trans2Parameters.FID = ushort.MaxValue;
                smbPacket.Trans2Parameters = trans2Parameters;
            }

            if (isSigned)
            {
                NamespaceCifs.CifsClientPerConnection connection =
                    this.smbClientStack.Context.GetConnection(ConnectionId);

                NamespaceCifs.CifsClientPerSession session =
                    this.smbClientStack.Context.GetSession(ConnectionId, uid);

                smbPacket.Sign(connection.ClientNextSendSequenceNumber, session.SessionKey);
            }

            #endregion

            #region Send and Receive ExpectPacket

            this.smbClientStack.SendPacket(smbPacket);
            StackPacket response = this.smbClientStack.ExpectPacket(this.timeout);

            NamespaceCifs.SmbPacket smbPacketResponse = (NamespaceCifs.SmbPacket)response;

            this.QueryUidTable(smbPacketResponse);
            this.QueryTidTable(smbPacketResponse);

            VerifyTransport(smbPacketResponse);
            VerifyCommonMessageSyntax(smbPacketResponse);

            if (response.GetType() == typeof(SmbErrorResponsePacket))
            {
                SmbErrorResponsePacket smbErrorResponsePacket = response as SmbErrorResponsePacket;
                NamespaceCifs.SmbHeader smbErrorHeader = smbErrorResponsePacket.SmbHeader;

                if (smbErrorHeader.Status == (uint)MessageStatus.StatusInvalidInfoClass)
                {
                    smbErrorHeader.Status = (uint)MessageStatus.NotSupported;
                }

                if ((informationLevel == InformationLevel.FileFsControlInformaiton))
                {
                    smbErrorHeader.Status = (uint)MessageStatus.StatusDataError;
                }
                this.ErrorTrans2SetFsInfoResponseAdditional(
                    smbErrorHeader.Mid + this.addMidMark,
                    (MessageStatus)smbErrorHeader.Status);
            }
            else
            {
                SmbTrans2SetFsInformationResponsePacket smbTrans2SetFsInformationPacket
                    = response as SmbTrans2SetFsInformationResponsePacket;
                NamespaceCifs.SmbHeader trans2SetFsInformationResponseHeader = smbTrans2SetFsInformationPacket.SmbHeader;

                this.Trans2SetFsInfoResponseAdditional(
                    trans2SetFsInformationResponseHeader.Mid + this.addMidMark,
                    this.QueryUidTable(smbPacketResponse),
                    this.QueryTidTable(smbPacketResponse),
                    (smbPacketResponse).IsSignRequired,
                    (MessageStatus)trans2SetFsInformationResponseHeader.Status);
            }

            #endregion
        }

        #endregion

        #endregion

        #region FSCC parts

        public void FSCCFSCTLNameRequest(int messageId,
                                        int sessionId,
                                        int treeId,
                                        bool isSigned,
                                        int fid,
                                        FSCCFSCTLName fsctlName)
        {
            #region Create Packet

            NamespaceCifs.SmbNtTransactIoctlRequestPacket smbPacket = new NamespaceCifs.SmbNtTransactIoctlRequestPacket();

            ushort uid = (ushort)this.uId[(uint)sessionId];
            uint functionCode = (uint)fsctlName;
            byte[] data = new byte[DataCount];
            this.fsccFSCTLName = fsctlName.ToString();

            smbPacket = this.smbClientStack.CreateNTTransIOCtlRequest(
                (ushort)this.fId[(uint)fid],
                true,
                byte.MinValue,
                functionCode,
                data);

            if (isSigned)
            {
                NamespaceCifs.CifsClientPerConnection connection =
                    this.smbClientStack.Context.GetConnection(ConnectionId);

                NamespaceCifs.CifsClientPerSession session = this.smbClientStack.Context.GetSession(ConnectionId, uid);

                smbPacket.Sign(connection.ClientNextSendSequenceNumber, session.SessionKey);
            }

            #endregion

            #region Send and Receive ExpectPacket

            this.smbClientStack.SendPacket(smbPacket);
            StackPacket response = this.smbClientStack.ExpectPacket(this.timeout);

            NamespaceCifs.SmbPacket smbPacketResponse = (NamespaceCifs.SmbPacket)response;

            this.QueryUidTable(smbPacketResponse);
            this.QueryTidTable(smbPacketResponse);

            VerifyTransport(smbPacketResponse);
            VerifyCommonMessageSyntax(smbPacketResponse);

            if (response.GetType() == typeof(SmbErrorResponsePacket))
            {
                SmbErrorResponsePacket smbErrorResponsePacket = response as SmbErrorResponsePacket;
                NamespaceCifs.SmbHeader smbErrorHeader = smbErrorResponsePacket.SmbHeader;
                this.ErrorResponse(
                    smbErrorHeader.Mid + this.addMidMark,
                    (MessageStatus)smbErrorHeader.Status);
            }
            else
            {
                NamespaceCifs.SmbNtTransactIoctlResponsePacket smbNtTransactIoctlResponsePacket
                    = response as SmbNtTransactIoctlResponsePacket;

                NamespaceCifs.SmbHeader ntTransactIoctlResponseHeader = smbNtTransactIoctlResponsePacket.SmbHeader;

                VerifyMessageSyntaxFsctlNameResponse(smbNtTransactIoctlResponsePacket);

                this.FSCCFSCTLNameResponse(
                    ntTransactIoctlResponseHeader.Mid + this.addMidMark,
                    this.QueryUidTable(smbPacketResponse),
                    this.QueryTidTable(smbPacketResponse),
                    (smbPacketResponse).IsSignRequired,
                    (MessageStatus)ntTransactIoctlResponseHeader.Status);
            }

            #endregion

        }

        /// <summary>
        /// verify fscc fsctlName
        /// </summary>
        /// <param name="ioctlResponse">ioctl response</param>
        [SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling")]
        private void VerifyMessageSyntaxFsctlNameResponse(NamespaceCifs.SmbNtTransactIoctlResponsePacket ioctlResponse)
        {
            string typeOfFileSystem = Site.Properties["TypeOfFileSystemForFscc"];
            switch (this.fsccFSCTLName)
            {
                case "FSCTL_CREATE_OR_GET_OBJECT_ID":
                    VerifyDataTypeFsctlCreateOrObjectIDReply(
                        (NamespaceFscc.FsctlCreateOrGetObjectIdReplyStatus)ioctlResponse.SmbHeader.Status);
                    break;
                case "FSCTL_DELETE_OBJECT_ID":
                    NamespaceFscc.FsccFsctlDeleteObjectIdResponsePacket deleteObjectIdResponse
                        = new NamespaceFscc.FsccFsctlDeleteObjectIdResponsePacket();
                    deleteObjectIdResponse.Payload
                        = (NamespaceFscc.FsctlDeleteObjectIdReplyStatus)ioctlResponse.SmbHeader.Status;
                    VerifyFsctlDeleteObjectIDReply(deleteObjectIdResponse);
                    break;
                case "FSCTL_FILESYSTEM_GET_STATISTICS":
                    NamespaceFscc.FsccFsctlFileSystemGetStatisticsResponsePacket getStatisticsResponse
                        = new NamespaceFscc.FsccFsctlFileSystemGetStatisticsResponsePacket();
                    getStatisticsResponse.FromBytes(ioctlResponse.SmbData.Data);

                    VerifyMessageSyntaxResonseStatusForFsctlFileSystemGetStatisticsRequest(
                        typeOfFileSystem,
                        (NamespaceFscc.FsctlFilesystemGetStatisticsReplyStatus)ioctlResponse.SmbHeader.Status);

                    VerifyFsctlFileSystemGetStatisticsReply(
                        getStatisticsResponse.FsctlFilesystemGetStatisticsReply);

                    break;
                case "FSCTL_GET_COMPRESSION":
                    NamespaceFscc.FsccFsctlGetCompressionResponsePacket getCompressionResponse
                        = new NamespaceFscc.FsccFsctlGetCompressionResponsePacket();
                    getCompressionResponse.FromBytes(ioctlResponse.SmbData.Data);
                    VerifyMessageSyntaxFsctlGetCompressionReply(
                        Site.Properties["TypeOfFileSystemForFscc"],
                        ioctlResponse.SmbHeader.Status);

                    VerifyDataTypeFSCTLGETCOMPRESSIONReply(getCompressionResponse, (uint)ioctlResponse.SmbHeader.Status);
                    break;
                case "FSCTL_GET_NTFS_VOLUME_DATA":
                    NamespaceFscc.FsccFsctlGetNtfsVolumeDataResponsePacket getNtfsVolumeResponse
                        = new NamespaceFscc.FsccFsctlGetNtfsVolumeDataResponsePacket();
                    getNtfsVolumeResponse.FromBytes(ioctlResponse.SmbData.Data);

                    NamespaceFscc.NTFS_VOLUME_DATA_BUFFER_Reply NTFSVolumeDataBufferReply
                        = (NamespaceFscc.NTFS_VOLUME_DATA_BUFFER_Reply)getNtfsVolumeResponse.Payload;
                    VerifyMessageSyntaxFsctlGetNtfsVolumeDataReply(
                        NTFSVolumeDataBufferReply,
                        (NamespaceFscc.FsctlGetNtfsVolumeDataReplyStatus)ioctlResponse.SmbHeader.Status);
                    break;
                case "FSCTL_GET_OBJECT_ID":
                    NamespaceFscc.FsccFsctlGetObjectIdResponsePacket getObjectIdResponse
                        = new NamespaceFscc.FsccFsctlGetObjectIdResponsePacket();
                    getObjectIdResponse.Payload = ioctlResponse.SmbData.Data;

                    bool isUseofObjectIDSupported = Boolean.Parse(Site.Properties["IsUseofObjectIDSupported"]);

                    VerifyDataTypeFsctlGetObjectIDReply((uint)ioctlResponse.SmbHeader.Status, isUseofObjectIDSupported);
                    break;
                case "FSCTL_IS_PATHNAME_VALID":

                    VerifyMessageSyntaxFsctlIsPathNameValidReply(typeOfFileSystem, ioctlResponse.SmbHeader.Status);
                    break;
                case "FSCTL_LMR_SET_LINK_TRACKING_INFORMATION":
                    NamespaceFscc.FsccFsctlLmrSetLinkTrackingInformationResponsePacket lmrSetLinkResponse
                        = new NamespaceFscc.FsccFsctlLmrSetLinkTrackingInformationResponsePacket();
                    lmrSetLinkResponse.Payload = (NamespaceFscc.FsctlLmrSetLinkTrackingInformationReplyStatus)ioctlResponse.SmbHeader.Status;


                    break;
                case "FSCTL_QUERY_ALLOCATED_RANGES"://need file have cotent.
                    NamespaceFscc.FsccFsctlQueryAllocatedRangesResponsePacket queryRangeResponse
                        = new NamespaceFscc.FsccFsctlQueryAllocatedRangesResponsePacket();
                    queryRangeResponse.FromBytes(ioctlResponse.SmbData.Data);

                    VerifyDataTypeFsctlQueryAllocatedRangesReply(
                    queryRangeResponse,
                    (uint)ioctlResponse.SmbHeader.Status);

                    NamespaceFscc.FSCTL_QUERY_ALLOCATED_RANGES_Reply fsctlQueryAllocatedRangesReply
                        = (NamespaceFscc.FSCTL_QUERY_ALLOCATED_RANGES_Reply)queryRangeResponse.Payload;
                    VerifyMessageSyntaxFsctlQueryAllocatedRanges(fsctlQueryAllocatedRangesReply);
                    break;
                case "FSCTL_QUERY_ON_DISK_VOLUME_INFO":
                    //TODO:sample expect error response
                    NamespaceFscc.FsccFsctlQueryOnDiskVolumeInfoResponsePacket onDiskVolumeInfoResponse
                        = new NamespaceFscc.FsccFsctlQueryOnDiskVolumeInfoResponsePacket();
                    onDiskVolumeInfoResponse.FromBytes(ioctlResponse.SmbData.Data);
                    //TODO: by stack
                    NamespaceFscc.FSCTL_QUERY_ON_DISK_VOLUME_INFO_Reply FSCTLQueryONDiskVolumnInfoReply
                        = (NamespaceFscc.FSCTL_QUERY_ON_DISK_VOLUME_INFO_Reply)onDiskVolumeInfoResponse.Payload;
                    VerifyMessageSyntaxFsctlQueryOnDiskVolumeInfoReply(FSCTLQueryONDiskVolumnInfoReply);
                    //onDiskVolumeInfoResponse.Payload
                    bool isDirectoryNumberKnown = Boolean.Parse(Site.Properties["IsDirectoryNumberKnownForFSCC"]);
                    bool isMajorVersionKnow = Boolean.Parse(Site.Properties["IsMajorVersionKnowForFSCC"]);
                    VerifyDataTypeFsctlQueryOnDiskVolumeInfoReply(
                         onDiskVolumeInfoResponse,
                         (uint)ioctlResponse.SmbHeader.Status,
                          isDirectoryNumberKnown,
                         isMajorVersionKnow);
                    break;
                case "FSCTL_QUERY_SPARING_INFO":
                    //TODO:sample expect error
                    NamespaceFscc.FsccFsctlQuerySparingInfoResponsePacket sparingInfoResponse
                        = new NamespaceFscc.FsccFsctlQuerySparingInfoResponsePacket();
                    sparingInfoResponse.FromBytes(ioctlResponse.SmbData.Data);

                    VerifyDataTypeFSCTLQUERYSPARINGINFOReply(sparingInfoResponse.Payload);
                    break;
                case "FSCTL_READ_FILE_USN_DATA":
                    NamespaceFscc.FsccFsctlReadFileUsnDataResponsePacket readUsnDataResponse
                        = new NamespaceFscc.FsccFsctlReadFileUsnDataResponsePacket();
                    readUsnDataResponse.FromBytes(ioctlResponse.SmbData.Data);

                    NamespaceFscc.FSCTL_READ_FILE_USN_DATA_Reply reply
                        = (NamespaceFscc.FSCTL_READ_FILE_USN_DATA_Reply)readUsnDataResponse.Payload;
                    VerifyMessageSyntaxFsctlReadFileUsnDataReply(reply);
                    bool isFileOrDirectoryClosed = Boolean.Parse(Site.Properties["IsFileOrDirectoryClosedFSCC"]);
                    bool isUsnChangeJournalRecordLogged
                        = Boolean.Parse(Site.Properties["IsUsnChangeJournalRecordLoggedForFSCC"]);
                    VerifyMessageSyntaxFsctlReadFileUsnDataReplyForOld(
                        reply,
                        isUsnChangeJournalRecordLogged);

                    VerifyMessageSyntaxFsctlReadFileUsnDataReply(
                        reply,
                        (NamespaceFscc.FsctlReadFileUsnDataReplyStatus)ioctlResponse.SmbHeader.Status,
                        isFileOrDirectoryClosed,
                        isUsnChangeJournalRecordLogged);
                    break;
                case "FSCTL_RECALL_FILE"://sample expect error, capture code expect error or success response.
                    NamespaceFscc.FsccFsctlRecallFileResponsePacket recallFileResponse
                        = new NamespaceFscc.FsccFsctlRecallFileResponsePacket();
                    recallFileResponse.Payload = (NamespaceFscc.FsctlRecallFileReplyStatus)ioctlResponse.SmbHeader.Status;

                    VerifyMessageSyntaxFsctlRecallFileReply(recallFileResponse.Payload);

                    break;
                case "FSCTL_SET_COMPRESSION":
                    NamespaceFscc.FsccFsctlSetCompressionResponsePacket setCompressionResponse
                        = new NamespaceFscc.FsccFsctlSetCompressionResponsePacket();
                    setCompressionResponse.Payload
                        = (NamespaceFscc.FsctlSetCompressionReplyStatus)ioctlResponse.SmbHeader.Status;

                    VerifyMessageSyntaxFsctlSetCompressionReply(
                        (NamespaceFscc.FsctlSetCompressionReplyStatus)setCompressionResponse.Payload);
                    break;
                case "FSCTL_SET_DEFECT_MANAGEMENT"://sample expect error, capture code expect error or success response.
                    NamespaceFscc.FsccFsctlSetDefectManagementResponsePacket setDefectManagementResponse
                        = new NamespaceFscc.FsccFsctlSetDefectManagementResponsePacket();
                    setDefectManagementResponse.Payload
                        = (NamespaceFscc.FsctlSetDefectManagementReplyStatus)ioctlResponse.SmbHeader.Status;

                    VerifyMessageSyntaxFsctlSetDefectManagementReply(setDefectManagementResponse.Payload);
                    break;

                case "FSCTL_SET_OBJECT_ID_EXTENDED"://second success
                    NamespaceFscc.FsccFsctlSetObjectIdExtendedResponsePacket setObjectIdExtendedResponse
                        = new NamespaceFscc.FsccFsctlSetObjectIdExtendedResponsePacket();
                    setObjectIdExtendedResponse.Payload
                        = (NamespaceFscc.FsctlSetObjectIdExtendedReplyStatus)ioctlResponse.SmbHeader.Status;

                    bool isFileSysContainSpecFile = Boolean.Parse(Site.Properties["IsFileSysContainSpecFileForFSCC"]);
                    bool isDirectoryNotSupportObjectIdUse
                        = Boolean.Parse(Site.Properties["IsDirectoryNotSupportObjectIdUseForFSCC"]);
                    VerifyMessageSyntaxFsctlSetObjectIdExtendedReply(
                                            (MessageStatus)ioctlResponse.SmbHeader.Status,
                                            isFileSysContainSpecFile,
                                            isDirectoryNotSupportObjectIdUse);
                    break;
                case "FSCTL_SET_SPARSE":
                    NamespaceFscc.FsccFsctlSetSparseResponsePacket setSparseResponse
                        = new NamespaceFscc.FsccFsctlSetSparseResponsePacket();
                    setSparseResponse.Payload = (NamespaceFscc.FsctlSetSparseReplyStatus)ioctlResponse.SmbHeader.Status;

                    break;
                case "FSCTL_SET_ZERO_DATA":
                    NamespaceFscc.FsccFsctlSetZeroDataResponsePacket setZeroDataResponse
                        = new NamespaceFscc.FsccFsctlSetZeroDataResponsePacket();
                    setZeroDataResponse.Payload
                        = (NamespaceFscc.FsctlSetZeroDataReplyStatus)ioctlResponse.SmbHeader.Status;

                    break;
                case "FSCTL_SET_ZERO_ON_DEALLOCATION":
                    NamespaceFscc.FsccFsctlSetZeroOnDeallocationResponsePacket setZeroOnDeallocationResponse
                        = new NamespaceFscc.FsccFsctlSetZeroOnDeallocationResponsePacket();
                    setZeroOnDeallocationResponse.Payload
                        = (NamespaceFscc.FsctlSetZeroOnDeallocationReplyStatus)ioctlResponse.SmbHeader.Status;

                    break;
                case "FSCTL_WRITE_USN_CLOSE_RECORD":
                    NamespaceFscc.FsccFsctlWriteUsnCloseRecordResponsePacket writeUsnCloseRecordResponse
                        = new NamespaceFscc.FsccFsctlWriteUsnCloseRecordResponsePacket();
                    writeUsnCloseRecordResponse.Payload = (long)ioctlResponse.SmbHeader.Status;

                    bool isUSNChangeSupport = Boolean.Parse(Site.Properties["IsUSNChangeSupportForFSCC"]);
                    VerifyDataTypeFsctlWriteUsnCloseRecordReply(
                        writeUsnCloseRecordResponse.Payload,
                        isUSNChangeSupport);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// TRANS2_QUERY_FILE_INFORMATION Request.
        /// </summary>
        /// <param name="messageId">This is used to associate a response with a request.</param>
        /// <param name="sessionId">
        /// Set this value to 0 to request a new session setup, or set this value to a previously established session 
        /// identifier to request reauthenticate to an existing session.
        /// </param>
        /// <param name="treeId">
        /// This field identifies the subdirectory (or tree) (also referred as a share in this document) on the 
        /// server that the client is accessing.
        /// </param>
        /// <param name="isSigned">
        /// Indicate whether the SUT has message signing enabled or required.
        /// </param>
        /// <param name="isUsePassthrough">
        /// Indicate whether adding SMB_INFO_PASSTHROUGH in InformationLevel field of the request.
        /// </param>
        /// <param name="isReparse">Indicate whether it is reparsed or not.</param>
        /// <param name="informationLevel">This can be used to query information from the server.</param>
        /// <param name="gmtTokenIndex">The index of the GMT token configured by CheckPreviousVersion action.</param>
        public void FSCCTrans2QueryPathInfoRequest(
            int messageId,
            int sessionId,
            int treeId,
            bool isSigned,
            //bool isUsePassthrough,
            //bool isReparse,
            //[Domain("InfoLevelQueriedByPath")] InformationLevel informationLevel,
            //int gmtTokenIndex)
            FSCCTransaction2QueryPathInforLevel informationLevel)
        {
            #region Create Packet

            SmbTrans2QueryPathInformationRequestPacket smbPacket = new SmbTrans2QueryPathInformationRequestPacket();
            ushort uid = (ushort)this.uId[(uint)sessionId];
            ushort requestedTid = (ushort)this.tId[(uint)treeId];
            ushort transactOptions = ushort.MinValue;
            string fileName = string.Empty;
            fsccQueryPathLevel = (ushort)informationLevel;

            fileName = Site.Properties["SutShareExistFile"] as string;

            ushort level = this.FSCCInformationLevelBytesQueryPath[(ushort)informationLevel];

            smbPacket = this.smbClientStack.CreateTrans2QueryPathInformationRequest(
                requestedTid,
                fileName,
                (NamespaceCifs.Trans2SmbParametersFlags)transactOptions,
                (NamespaceCifs.QueryInformationLevel)level,
                this.maxDataCount,
                false);

            if (isSigned)
            {
                NamespaceCifs.CifsClientPerConnection connection =
                    this.smbClientStack.Context.GetConnection(ConnectionId);

                NamespaceCifs.CifsClientPerSession session =
                    this.smbClientStack.Context.GetSession(ConnectionId, uid);

                smbPacket.Sign(connection.ClientNextSendSequenceNumber, session.SessionKey);
            }

            #endregion

            #region Send and Receive ExpectPacket

            this.smbClientStack.SendPacket(smbPacket);
            StackPacket response = this.smbClientStack.ExpectPacket(this.timeout);

            NamespaceCifs.SmbPacket smbPacketResponse = (NamespaceCifs.SmbPacket)response;

            this.QueryUidTable(smbPacketResponse);
            this.QueryTidTable(smbPacketResponse);

            if (response.GetType() == typeof(SmbErrorResponsePacket))
            {
                SmbErrorResponsePacket smbErrorResponsePacket = response as SmbErrorResponsePacket;
                NamespaceCifs.SmbHeader smbErrorHeader = smbErrorResponsePacket.SmbHeader;

                this.ErrorResponse(
                    smbErrorHeader.Mid + this.addMidMark,
                    (MessageStatus)smbErrorHeader.Status);
            }
            else
            {
                SmbTrans2QueryPathInformationResponsePacket smbTrans2QueryPathInformationPacket
                    = response as SmbTrans2QueryPathInformationResponsePacket;
                NamespaceCifs.SmbHeader trans2QueryPathInformationResponseHeader =
                    smbTrans2QueryPathInformationPacket.SmbHeader;

                FSCCTrans2QueryPathInformation(smbTrans2QueryPathInformationPacket);

                this.FSCCTrans2QueryPathInfoResponse(
                    trans2QueryPathInformationResponseHeader.Mid + this.addMidMark,
                    this.QueryUidTable(smbPacketResponse),
                    this.QueryTidTable(smbPacketResponse),
                    (smbPacketResponse).IsSignRequired,
                    (MessageStatus)trans2QueryPathInformationResponseHeader.Status);
            }

            #endregion
        }

        /// <summary>
        /// get fscc query path response, verify data
        /// </summary>
        /// <param name="smbTrans2QueryPathInformationPacket">The trans2 query path information packet to be verified</param>
        [SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling")]
        private void FSCCTrans2QueryPathInformation(SmbTrans2QueryPathInformationResponsePacket
            smbTrans2QueryPathInformationPacket)
        {
            switch (fsccQueryPathLevel)
            {
                //FileBasicInformation,
                case 0:
                    NamespaceFscc.FsccFileBasicInformationResponsePacket fileBasicInformationResponse
                        = new NamespaceFscc.FsccFileBasicInformationResponsePacket();
                    fileBasicInformationResponse.FromBytes(smbTrans2QueryPathInformationPacket.SmbData.Trans2_Data);
                    NamespaceFscc.FileBasicInformation fileBasicInformation = fileBasicInformationResponse.Payload;

                    VerifyDataTypeFileBasicInformation(fileBasicInformation);

                    break;

                //FileStandardInformation,
                case 1:
                    NamespaceFscc.FsccFileStandardInformationResponsePacket fileStandardInformationResponse
                        = new NamespaceFscc.FsccFileStandardInformationResponsePacket();
                    fileStandardInformationResponse.FromBytes(smbTrans2QueryPathInformationPacket.SmbData.Trans2_Data);
                    NamespaceFscc.FileStandardInformation fileStandardInformation = fileStandardInformationResponse.Payload;

                    bool isFileDeletionRequested = Boolean.Parse(Site.Properties["IsFileDeletionRequestedForFscc"]);
                    bool isDirector = Boolean.Parse(Site.Properties["IsDirectorForFscc"]);

                    VerifyDataTypeFileStandardInformation(
                        fileStandardInformation,
                        isFileDeletionRequested,
                        isDirector);
                    break;

                //FileInternalInformation,
                case 2:
                    NamespaceFscc.FsccFileInternalInformationResponsePacket fileInternalInformationResponse
                        = new NamespaceFscc.FsccFileInternalInformationResponsePacket();
                    fileInternalInformationResponse.FromBytes(smbTrans2QueryPathInformationPacket.SmbData.Trans2_Data);
                    NamespaceFscc.FileInternalInformation fileInternalInformation = fileInternalInformationResponse.Payload;

                    bool isSupportFileReferenceNum = Boolean.Parse(Site.Properties["IsSupportFileReferenceNumForFscc"]);
                    VerifyDataTypeFileInternalInformation(fileInternalInformation, isSupportFileReferenceNum);
                    break;

                //FileEaInformation,
                case 3:
                    NamespaceFscc.FsccFileEaInformationResponsePacket fileEaInformationResponse
                        = new NamespaceFscc.FsccFileEaInformationResponsePacket();
                    fileEaInformationResponse.FromBytes(smbTrans2QueryPathInformationPacket.SmbData.Trans2_Data);
                    NamespaceFscc.FileEaInformation fileEaInformation = fileEaInformationResponse.Payload;

                    uint EAlength = uint.Parse(Site.Properties["EAlengthForFscc"]);
                    VerifyDataTypeFileEaInformation(fileEaInformation, EAlength);
                    break;

                //FileAccessInformation,
                case 4:
                    NamespaceFscc.FsccFileAccessInformationResponsePacket fileAccessInformationResponse
                        = new NamespaceFscc.FsccFileAccessInformationResponsePacket();
                    fileAccessInformationResponse.FromBytes(smbTrans2QueryPathInformationPacket.SmbData.Trans2_Data);
                    NamespaceFscc.FILE_ACCESS_INFORMATION fileAccessInformation = fileAccessInformationResponse.Payload;

                    VerifyDataTypeFileAccessInformation(fileAccessInformation);
                    break;

                //FileNameInformation,
                case 5:
                    NamespaceFscc.FsccFileNameInformationResponsePacket fileNameInformationResponse
                        = new NamespaceFscc.FsccFileNameInformationResponsePacket();
                    fileNameInformationResponse.FromBytes(smbTrans2QueryPathInformationPacket.SmbData.Trans2_Data);
                    NamespaceFscc.FileNameInformation fileNameInformation = fileNameInformationResponse.Payload;

                    VerifyDataTypeFileNameInformation(fileNameInformation);
                    VerifyMessageSyntaxFileNameInformation(fileNameInformation);
                    break;

                //FileModeInformation,
                case 6:
                    NamespaceFscc.FsccFileModeInformationResponsePacket fileModeInformationResponse
                        = new NamespaceFscc.FsccFileModeInformationResponsePacket();
                    fileModeInformationResponse.FromBytes(smbTrans2QueryPathInformationPacket.SmbData.Trans2_Data);
                    NamespaceFscc.FILE_MODE_INFORMATION fileModeInformation = fileModeInformationResponse.Payload;

                    bool isDeleteFile = Boolean.Parse(Site.Properties["IsDeleteFileForFscc"]);
                    VerifyMessageSyntaxFileModeinformation(fileModeInformation, isDeleteFile);

                    VerifyDataTypeFileModeInformation(fileModeInformation);
                    break;

                //FileAlignmentInformation,
                case 7:
                    NamespaceFscc.FsccFileAlignmentInformationResponsePacket fileAlignmentInformationResponse
                        = new NamespaceFscc.FsccFileAlignmentInformationResponsePacket();
                    fileAlignmentInformationResponse.FromBytes(smbTrans2QueryPathInformationPacket.SmbData.Trans2_Data);
                    NamespaceFscc.FILE_ALIGNMENT_INFORMATION fileAlignmentInformation
                        = fileAlignmentInformationResponse.Payload;


                    VerifyDataTypeFileAlignmentInformation(
                        fileAlignmentInformation,
                        smbTrans2QueryPathInformationPacket.SmbData.Trans2_Data.Length);
                    break;

                //FileAlternateNameInformation,
                case 8:
                    NamespaceFscc.FsccFileAlternateNameInformationResponsePacket fileAlternateNameInformationResponse
                        = new NamespaceFscc.FsccFileAlternateNameInformationResponsePacket();
                    fileAlternateNameInformationResponse.FromBytes(smbTrans2QueryPathInformationPacket.SmbData.Trans2_Data);
                    NamespaceFscc.FileAlternateNameInformation fileAlternateNameInformation
                        = fileAlternateNameInformationResponse.Payload;


                    VerifyDataTypeFileAlternateNameInformation(fileAlternateNameInformation);
                    break;

                //FileStreamInformation,
                case 9:
                    NamespaceFscc.FsccFileStreamInformationResponsePacket fileStreamInformationResponse
                        = new NamespaceFscc.FsccFileStreamInformationResponsePacket();
                    fileStreamInformationResponse.FromBytes(smbTrans2QueryPathInformationPacket.SmbData.Trans2_Data);
                    NamespaceFscc.FileStreamInformation fileStreamInformation = fileStreamInformationResponse.Payload;

                    bool isNoOtherEntriesFollow = Boolean.Parse(Site.Properties["IsNoOtherEntriesFollowForFSCC"]);
                    bool isMultipleEntriesPresent = Boolean.Parse(Site.Properties["IsMultipleEntriesPresentForFSCC"]);
                    VerifyDataTypeFileStreamInformation(
                        fileStreamInformation,
                        isMultipleEntriesPresent,
                        isNoOtherEntriesFollow);
                    break;

                //FileCompressionInformation,
                case 10:
                    NamespaceFscc.FsccFileCompressionInformationResponsePacket fileCompressionInformationResponse
                        = new NamespaceFscc.FsccFileCompressionInformationResponsePacket();
                    fileCompressionInformationResponse.FromBytes(smbTrans2QueryPathInformationPacket.SmbData.Trans2_Data);
                    NamespaceFscc.FileCompressionInformation fileCompressionInformation
                        = fileCompressionInformationResponse.Payload;

                    int compressionFormat = int.Parse(Site.Properties["FompressionFormatForFSCC"]);
                    byte chunkShift = byte.Parse(Site.Properties["ChunkShiftForFSCC"]);
                    byte clusterShift = byte.Parse(Site.Properties["ClusterShiftForFSCC"]);
                    byte clusterSize = byte.Parse(Site.Properties["ClusterSizeForFSCC"]);
                    VerifyDataTypeFileCompressionInformationForOld(
                        fileCompressionInformation,
                        compressionFormat,
                        chunkShift,
                        clusterShift,
                        clusterSize);

                    string typeOfFileSystem = Site.Properties["TypeOfFileSystemForFscc"];
                    byte expectedInitializedClusterShift
                        = byte.Parse(Site.Properties["ExpectedInitializedClusterShiftForFscc"]);
                    VerifyDataTypeFileCompressionInformation(
                        fileCompressionInformation,
                        typeOfFileSystem,
                        expectedInitializedClusterShift
                        );

                    break;

                //FileNetworkOpenInformation,
                case 11:
                    NamespaceFscc.FsccFileNetworkOpenInformationResponsePacket fileNetworkOpenInformationResponse
                        = new NamespaceFscc.FsccFileNetworkOpenInformationResponsePacket();
                    fileNetworkOpenInformationResponse.FromBytes(smbTrans2QueryPathInformationPacket.SmbData.Trans2_Data);
                    NamespaceFscc.FileNetworkOpenInformation fileNetworkOpenInformation
                        = fileNetworkOpenInformationResponse.Payload;

                    int allocationSize = int.Parse(Site.Properties["AllocationSizeForFSCC"]);
                    uint fileAttributes = uint.Parse(Site.Properties["FileAttributesForFSCC"]);
                    VerifyDataTypeFileNetworkOpenInformation(
                        fileNetworkOpenInformation,
                        allocationSize,
                        fileAttributes);
                    break;

                //FileAttributeTagInformation,
                case 12:
                    NamespaceFscc.FsccFileAttributeTagInformationResponsePacket fileAttributeTagInformationResponse
                        = new NamespaceFscc.FsccFileAttributeTagInformationResponsePacket();
                    fileAttributeTagInformationResponse.FromBytes(smbTrans2QueryPathInformationPacket.SmbData.Trans2_Data);
                    NamespaceFscc.FileAttributeTagInformation fileAttributeTagInformation
                        = fileAttributeTagInformationResponse.Payload;

                    uint fileAttributesTag = uint.Parse(Site.Properties["FileAttributesForFSCC"]);
                    VerifyDataTypeFileAttributeTagInformation(fileAttributeTagInformation, fileAttributesTag);
                    break;

                case 13://FilePositionInformation
                    NamespaceFscc.FsccFilePositionInformationResponsePacket filePositionInformationResponse
                        = new NamespaceFscc.FsccFilePositionInformationResponsePacket();
                    filePositionInformationResponse.FromBytes(smbTrans2QueryPathInformationPacket.SmbData.Trans2_Data);
                    NamespaceFscc.FILE_POSITION_INFORMATION filePositionInformation
                        = filePositionInformationResponse.Payload;

                    VerifyDataTypeFilePositionInformation(filePositionInformation);

                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// TRANS2_QUERY_FS_INFORMATION Request.
        /// </summary>
        /// <param name="messageId">This is used to associate a response with a request.</param>
        /// <param name="sessionId">
        /// Set this value to 0 to request a new session setup, or set this value to a previously established session 
        /// identifier to request reauthenticate to an existing session.
        /// </param>
        /// <param name="treeId">
        /// This field identifies the subdirectory (or tree) (also referred as a share in this document) on the 
        /// server that the client is accessing.
        /// </param>
        /// <param name="isSigned">
        /// Indicate whether the SUT has message signing enabled or required.
        /// </param>
        /// <param name="informationLevel">This can be used to query information from the server.</param>
        public void FSCCTrans2QueryFSInfoRequest(int messageId,
                                                 int sessionId,
                                                 int treeId,
                                                 bool isSigned,
                                                 FSCCTransaction2QueryFSInforLevel informationLevel)
        {
            #region Create Packet

            SmbTrans2QueryFsInformationRequestPacket smbPacket = new SmbTrans2QueryFsInformationRequestPacket();

            ushort uid = (ushort)this.uId[(uint)sessionId];
            ushort requestedTid = (ushort)this.tId[(uint)treeId];
            NamespaceCifs.Trans2SmbParametersFlags transactOptions = NamespaceCifs.Trans2SmbParametersFlags.NONE;
            ushort level = this.FSCCInformationLevelBytesQueryFS[(ushort)informationLevel];
            fsccQueryFSLevel = (ushort)informationLevel;

            smbPacket = this.smbClientStack.CreateTrans2QueryFileSystemInformationRequest(
                requestedTid,
                this.maxDataCount,
                transactOptions,
                (NamespaceCifs.QueryFSInformationLevel)level);

            NamespaceCifs.TRANS2_QUERY_FS_INFORMATION_Request_Trans2_Parameters payload = smbPacket.Trans2Parameters;
            payload.InformationLevel = (NamespaceCifs.QueryFSInformationLevel)level;
            smbPacket.Trans2Parameters = payload;

            if (isSigned)
            {
                NamespaceCifs.CifsClientPerConnection connection =
                    this.smbClientStack.Context.GetConnection(ConnectionId);

                NamespaceCifs.CifsClientPerSession session =
                    this.smbClientStack.Context.GetSession(ConnectionId, uid);

                smbPacket.Sign(connection.ClientNextSendSequenceNumber, session.SessionKey);
            }

            #endregion

            #region Send and Receive ExpectPacket

            this.smbClientStack.SendPacket(smbPacket);
            StackPacket response = this.smbClientStack.ExpectPacket(this.timeout);

            NamespaceCifs.SmbPacket smbPacketResponse = (NamespaceCifs.SmbPacket)response;

            this.QueryUidTable(smbPacketResponse);
            this.QueryTidTable(smbPacketResponse);

            if (response.GetType() == typeof(SmbErrorResponsePacket))
            {
                SmbErrorResponsePacket smbErrorResponsePacket = response as SmbErrorResponsePacket;
                NamespaceCifs.SmbHeader smbErrorHeader = smbErrorResponsePacket.SmbHeader;
                this.ErrorResponse(smbErrorHeader.Mid + this.addMidMark, (MessageStatus)smbErrorHeader.Status);
            }
            else
            {
                SmbTrans2QueryFsInformationResponsePacket smbTrans2QueryFsInformationPacket
                    = response as SmbTrans2QueryFsInformationResponsePacket;

                NamespaceCifs.SmbHeader trans2QueryFsInformationResponseHeader =
                    smbTrans2QueryFsInformationPacket.SmbHeader;

                FSCCTrans2QueryFSInformationResponse(smbTrans2QueryFsInformationPacket);

                this.FSCCTrans2QueryFSInfoResponse(
                    trans2QueryFsInformationResponseHeader.Mid + this.addMidMark,
                    this.QueryUidTable(smbPacketResponse),
                    this.QueryTidTable(smbPacketResponse),
                    (smbPacketResponse).IsSignRequired,
                    (MessageStatus)trans2QueryFsInformationResponseHeader.Status);
            }

            #endregion
        }

        /// <summary>
        /// get fscc query fs response, verify data
        /// </summary>
        /// <param name="qureryFsInformationFsccPacket">The trans2 query fs information response packet to be verified</param>
        private void FSCCTrans2QueryFSInformationResponse(SmbTrans2QueryFsInformationResponsePacket
            qureryFsInformationFsccPacket)
        {
            bool isPerUserQuotasUsed = Boolean.Parse(Site.Properties["IsPerUserQuotasUsedForFscc"]);
            uint expectedTotalFreeAllocationUnits = uint.Parse(Site.Properties["ExpectedTotalFreeAllocationUnitsForFscc"]);
            string typeOfFileSystem = Site.Properties["TypeOfFileSystemForFscc"];
            switch (fsccQueryFSLevel)
            {
                case 0://FileFsVolumeInformation,
                    NamespaceFscc.FsccFileFsVolumeInformationResponsePacket fileFsVolumeInformationResponse
                        = new NamespaceFscc.FsccFileFsVolumeInformationResponsePacket();
                    fileFsVolumeInformationResponse.FromBytes(qureryFsInformationFsccPacket.SmbData.Trans2_Data);
                    NamespaceFscc.FileFsVolumeInformation fileFsVolumeInformation = fileFsVolumeInformationResponse.Payload;

                    uint volumeSerialNumber = uint.Parse(Site.Properties["VolumeSerialNumberForFSCC"]);
                    bool isSupportsOOFileSysObj = Boolean.Parse(Site.Properties["IsSupportsOOFileSysObj"]);
                    VerifyDataTypeFileFsVolumeInformationForOld(
                        fileFsVolumeInformation,
                        volumeSerialNumber,
                        isSupportsOOFileSysObj);
                    VerifyDataTypeFileFsVolumeInformation(fileFsVolumeInformation, typeOfFileSystem);
                    break;

                case 1://FileFsSizeInformation,
                    NamespaceFscc.FsccFileFsSizeInformationResponsePacket fileFsSizeInformationResponse
                        = new NamespaceFscc.FsccFileFsSizeInformationResponsePacket();
                    fileFsSizeInformationResponse.FromBytes(qureryFsInformationFsccPacket.SmbData.Trans2_Data);
                    NamespaceFscc.FileFsSizeInformation fileFsSizeInformation = fileFsSizeInformationResponse.Payload;

                    uint sectorsPerAllocationUnit = uint.Parse(Site.Properties["SectorsPerAllocationUnitFprFSCC"]);
                    uint bytesPerSector = uint.Parse(Site.Properties["BytesPerSectorForFSCC"]);
                    VerifyDataTypeFileFsSizeInformation(
                        fileFsSizeInformation,
                        sectorsPerAllocationUnit,
                        bytesPerSector);

                    VerifyDataTypeFileFsSizeInformationField(
                        fileFsSizeInformation,
                        isPerUserQuotasUsed,
                        expectedTotalFreeAllocationUnits);
                    break;

                case 2://FileFsDeviceInformation,
                    NamespaceFscc.FsccFileFsDeviceInformationResponsePacket fileFsDeviceInformationResponse
                        = new NamespaceFscc.FsccFileFsDeviceInformationResponsePacket();
                    fileFsDeviceInformationResponse.FromBytes(qureryFsInformationFsccPacket.SmbData.Trans2_Data);
                    NamespaceFscc.FileFsDeviceInformation fileFsDeviceInformation = fileFsDeviceInformationResponse.Payload;


                    VerifyDataTypeFileFsDeviceInformation(fileFsDeviceInformation);
                    break;

                case 3://FileFsAttributeInformation,
                    NamespaceFscc.FsccFileFsAttributeInformationResponsePacket fileFsAttributeInformationResponse
                        = new NamespaceFscc.FsccFileFsAttributeInformationResponsePacket();
                    fileFsAttributeInformationResponse.FromBytes(qureryFsInformationFsccPacket.SmbData.Trans2_Data);
                    NamespaceFscc.FileFsAttributeInformation fileFsAttributeInformation
                        = fileFsAttributeInformationResponse.Payload;

                    int maximumFileNameComponentLength = int.Parse(Site.Properties["MaximumFileNameComponentLengthForFscc"]);
                    VerifyMessageSyntaxFileFsAttributeInformation(fileFsAttributeInformation, maximumFileNameComponentLength);

                    VerifyDataTypeFileFsAttributeInformation(fileFsAttributeInformation);

                    VerifyDataTypeFileFsAttributeInformationForOld(fileFsAttributeInformation);
                    break;

                case 4://FileFsControlInformation,
                    NamespaceFscc.FsccFileFsControlInformationResponsePacket fileFsControlInformationResponse
                        = new NamespaceFscc.FsccFileFsControlInformationResponsePacket();
                    fileFsControlInformationResponse.FromBytes(qureryFsInformationFsccPacket.SmbData.Trans2_Data);
                    NamespaceFscc.FileFsControlInformation fileFsControlInformation = fileFsControlInformationResponse.Payload;

                    long defaultQuotaLimit = long.Parse(Site.Properties["DefaultQuotaLimitForFSCC"]);

                    VerifyDataTypeFileFsControlInformationForOld(
                        fileFsControlInformation,
                        defaultQuotaLimit);

                    long deaultPerUserDiskQuotaWarningThreshold
                        = long.Parse(Site.Properties["DeaultPerUserDiskQuotaWarningThresholdForFscc"]);
                    long minAmountOfFreeDiskSpaceForStartFiltering
                        = long.Parse(Site.Properties["MinAmountOfFreeDiskSpaceForStartFilteringForFscc"]);
                    long minAmountOfFreeDiskSpaceForStopFiltering
                        = long.Parse(Site.Properties["MinAmountOfFreeDiskSpaceForStopFilteringForFscc"]);
                    long minAmountOfFreeDiskSpaceForThreshold
                        = long.Parse(Site.Properties["MinAmountOfFreeDiskSpaceForThresholdForFscc"]);

                    VerifyMessageSyntaxFileFsControlInformation(
                        fileFsControlInformation,
                        deaultPerUserDiskQuotaWarningThreshold,
                        minAmountOfFreeDiskSpaceForStartFiltering,
                        minAmountOfFreeDiskSpaceForStopFiltering,
                        minAmountOfFreeDiskSpaceForThreshold);

                    VerifyDataTypeFileFsControlInformation(fileFsControlInformation);
                    break;

                case 5: //FileFsFullSizeInformation,
                    NamespaceFscc.FsccFileFsFullSizeInformationResponsePacket fileFsFullSizeInformationResponse
                        = new NamespaceFscc.FsccFileFsFullSizeInformationResponsePacket();
                    fileFsFullSizeInformationResponse.FromBytes(qureryFsInformationFsccPacket.SmbData.Trans2_Data);
                    NamespaceFscc.FileFsFullSizeInformation fileFsFullSizeInformation
                        = fileFsFullSizeInformationResponse.Payload;

                    uint bytesPerSectorForFullSize = uint.Parse(Site.Properties["BytesPerSectorForFSCC"]);
                    VerifyDataTypeFileFsFullSizeInformation(
                        fileFsFullSizeInformation,
                        bytesPerSectorForFullSize);

                    VerifyDataTypeFileFsFullSizeInformationField(
                        fileFsFullSizeInformation,
                        isPerUserQuotasUsed,
                        expectedTotalFreeAllocationUnits);
                    break;

                case 6://FileFsObjectIdInformation 
                    NamespaceFscc.FsccFileFsObjectIdInformationResponsePacket fileFsObjectIdInformationResponse
                        = new NamespaceFscc.FsccFileFsObjectIdInformationResponsePacket();
                    fileFsObjectIdInformationResponse.FromBytes(qureryFsInformationFsccPacket.SmbData.Trans2_Data);
                    NamespaceFscc.FileFsObjectIdInformation fileFsObjectIdInformation
                        = fileFsObjectIdInformationResponse.Payload;

                    uint returnedStatusCode = uint.Parse(Site.Properties["ReturnedStatusCodeForFscc"]);
                    VerifyResponseForFileFsObjectIdInformationQuery(
                        fileFsObjectIdInformation,
                        returnedStatusCode,
                        typeOfFileSystem);
                    bool isNoExInfoWritten = Boolean.Parse(Site.Properties["IsNoExInfoWrittenForFSCC"]);
                    VerifyDataTypeFileFsObjectIdInformation(
                        fileFsObjectIdInformation,
                        isNoExInfoWritten);
                    break;

                default:
                    break;
            }
        }

        #endregion

        #region  Help methods

        /// <summary>
        /// Query session table.
        /// </summary>
        /// <param name="packet">Get Uid from the SMB packet. </param>
        /// <returns>The key in the dictionary. </returns>
        private ushort QueryUidTable(NamespaceCifs.SmbPacket packet)
        {
            ushort uid = packet.SmbHeader.Uid;
            int uTableLen = this.uId.Count;
            if (!this.uId.ContainsValue(uid))
            {
                this.uId.Add((uint)uTableLen, (uint)uid);
                return (ushort)uTableLen;
            }
            else
            {
                foreach (uint key in this.uId.Keys)
                {
                    if (this.uId[key] == uid)
                    {
                        return (ushort)key;
                    }
                }

                return ushort.MinValue;
            }
        }


        /// <summary>
        /// Query tid table.
        /// </summary>
        /// <param name="packet">Get Uid from the SMB packet.</param>
        /// <returns>The key in the dictionary.</returns>
        private ushort QueryTidTable(NamespaceCifs.SmbPacket packet)
        {
            ushort tid = packet.SmbHeader.Tid;
            int tTableLen = this.tId.Count;

            if (!this.tId.ContainsValue(tid))
            {
                this.tId.Add((uint)tTableLen, (uint)tid);
                return (ushort)tTableLen;
            }
            else
            {
                foreach (uint key in this.tId.Keys)
                {
                    if (this.tId[key] == tid)
                    {
                        return (ushort)key;
                    }
                }

                return 0;
            }
        }


        /// <summary>
        /// Method to convert strlong to byte[].
        /// </summary>
        /// <param name="strLogin">The login string.</param>
        /// <returns>The bytes array that will be converted.</returns>
        private byte[] GetSid(string strLogin)
        {
            byte[] arr = null;

            // Parse the string to check if domain name is present.
            int idx = strLogin.IndexOf('\\');
            if (idx == -1)
            {
                idx = strLogin.IndexOf('@');
            }

            string strDomain;
            string strName;

            if (idx != -1)
            {
                strDomain = strLogin.Substring(0, idx);
                strName = strLogin.Substring(idx + 1);
            }
            else
            {
                strDomain = Environment.MachineName;
                strName = strLogin;
            }

            System.DirectoryServices.DirectoryEntry obDirEntry = null;
            try
            {
                obDirEntry = new System.DirectoryServices.DirectoryEntry("WinNT://" + strDomain + "/" + strName);
                System.DirectoryServices.PropertyCollection coll = obDirEntry.Properties;
                object obVal = coll["objectSid"].Value;

                if (null != obVal)
                {
                    arr = (byte[])obVal;
                }
            }
            catch (Exception)
            {
                throw;
            }

            return arr;
        }


        #region help method for capture code

        /// <summary>
        /// Method to covert 4 byte to UInt32.
        /// </summary>
        /// <param name="intByte">The bytes array that will be converted.</param>
        /// <returns>The returned UInt32 value.</returns>
        private uint BytesToUInt32(byte[] intByte)
        {
            uint value = uint.MinValue;
            uint tempValue = uint.MinValue;
            int i = (int)uint.MinValue;

            foreach (byte b in intByte)
            {
                tempValue = b;
                tempValue = tempValue << (i * 8);
                value = value | tempValue;
                i++;
            }

            if (i > 4)
            {
                throw new System.ArgumentOutOfRangeException("intByte",
                    "The parameter bytes count is greater than 4 for BytesToUInt32");
            }

            return value;
        }


        /// <summary>
        /// Method to covert 8 bytes to UInt64.
        /// </summary>
        /// <param name="intByte">The bytes array that will be converted</param>
        /// <returns>The returned UInt64 value.</returns>
        private ulong BytesToUInt64(byte[] intByte)
        {
            uint intV = uint.MinValue;
            uint temp = uint.MinValue;
            int i = (int)uint.MinValue;

            foreach (byte b in intByte)
            {
                temp = b;
                temp = temp << (i * 8);
                intV = intV | temp;
                i++;
            }

            if (i > 8)
            {
                throw new System.ArgumentOutOfRangeException("intByte",
                    "The parameter bytes count is greater than 8 for BytesToUInt64");
            }

            return intV;
        }


        /// <summary>
        /// Verify whether the bytes in the array are all zero or not. 
        /// </summary>
        /// <param name="bytes">The bytes array that will be converted.</param>
        /// <returns>A bool value, if all the bytes in the array are zero, return true, else return false.</returns>
        private bool IsAllBytesZero(byte[] bytes)
        {
            bool isAllBytesZero = true;
            byte byteZero = (byte)0;

            foreach (byte b in bytes)
            {
                if (b != byteZero)
                {
                    isAllBytesZero = false;
                    break;
                }
            }

            return isAllBytesZero;
        }


        /// <summary>
        /// Method to covert bytes to string.
        /// </summary>
        /// <param name="bytes">The bytes array that will be converted.</param>
        /// <returns>The returned string value.</returns>
        private string BytesToString(byte[] bytes)
        {
            string resultString = string.Empty;

            foreach (byte b in bytes)
            {
                resultString += "," + b.ToString();
            }

            if (!(string.IsNullOrEmpty(resultString)))
            {
                return resultString.Substring(1);
            }
            else
            {
                return resultString;
            }
        }


        /// <summary>
        /// Verify if a Token take the form "@GMT-YYYY.MM.DD-HH.MM.SS."
        /// </summary>
        /// <param name="gmtToken">The gmtToken string.</param>
        /// <returns> If the snapshot is valid it is true, else false.</returns>
        private bool IsSnapShotValid(string gmtToken)
        {
            if (gmtToken.Length < 24)
            {
                return false;
            }

            // get the "MM" field
            string monthStr = gmtToken.Substring(10, 2);
            int month = int.Parse(monthStr);

            // get the "DD" field
            string dateStr = gmtToken.Substring(13, 2);
            int date = int.Parse(dateStr);

            // get the "HH" field
            string hourStr = gmtToken.Substring(16, 2);
            int hour = int.Parse(hourStr);

            // get the "MM" field
            string minuteStr = gmtToken.Substring(19, 2);
            int minute = int.Parse(minuteStr);

            // get the "SS" field
            string secondStr = gmtToken.Substring(22, 2);
            int second = int.Parse(secondStr);

            if (gmtToken.StartsWith("@GMT-")
                && (gmtToken[9] == '.')
                && ((month >= 0) && (month <= 12))
                && (gmtToken[12] == '.')
                && ((date >= 0) && (date <= 31))
                && (gmtToken[15] == '-')
                && ((hour >= 0) && (hour <= 24))
                && (gmtToken[18] == '.')
                && ((minute >= 0) && (minute <= 60))
                && (gmtToken[21] == '.')
                && ((second >= 0) && (second <= 60)))
            {
                return true;
            }

            return false;
        }

        #endregion

        #endregion
    }
}
