// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Modeling;
using Microsoft.Protocols.TestSuites.FileSharing.FSA.Adapter;
using System.Diagnostics.CodeAnalysis;

namespace Microsoft.Protocols.TestSuites.FileSharing.FSA.Model
{
    /// <summary>
    /// MS-FSA model program
    /// </summary>
    public static partial class ModelProgram
    {
        #region variables

        /// <summary>
        /// FSA initialization state.
        /// </summary>
        /// Disable warning CA2211, because this action confuses with the actual model design if modify it.
        [SuppressMessage("Microsoft.Usage", "CA2211:NonConstantFieldsShouldNotBeVisible")]
        public static FSStates fsStates = FSStates.ReadyInitial;

        /// <summary>
        /// Global variable of SUT platform, to save the SUT platform. 
        /// </summary>
        /// Disable warning CA2211, because this action confuses with the actual model design if modify it.
        [SuppressMessage("Microsoft.Usage", "CA2211:NonConstantFieldsShouldNotBeVisible")]
        public static PlatformType sutPlatForm;

        /// <summary>
        /// Used to get sut information.
        /// </summary>
        /// Disable warning CA2211, because this action confuses with the actual model design if modify it.
        [SuppressMessage("Microsoft.Usage", "CA2211:NonConstantFieldsShouldNotBeVisible")]
        public static SutOSInfo sutOSInfo = SutOSInfo.ReadyGetSutInfo;

        /// <summary>
        /// A list of waiting operations that can be canceled by adding them to the 
        /// CancelableOperations.CancelableOperationList as defined in section 3.1.1.12
        /// </summary>
        static SequenceContainer<IORequest> sequenceIORequest = new SequenceContainer<IORequest>();

        /// <summary>
        /// Judge if requirement 507 is implemented.
        /// </summary>
        static bool isR507Implemented;

        /// <summary>
        /// judge 405 requirement if implement
        /// </summary>
        static bool isR405Implemented;

        /// <summary>
        /// Security context.
        /// </summary>
        static SSecurityContext gSecurityContext;

        #endregion

        #region FSA Model State

        #region 3.1.5.1 global variables

        /// <summary>
        /// The access granted for this open as specified in [MS-SMB2] section 2.2.13.1.
        /// Open.GrantedAccess used in 3.1.5.1
        /// </summary>
        /// Disable warning CA2211, because this action confuses with the actual model design if modify it.
        [SuppressMessage("Microsoft.Usage", "CA2211:NonConstantFieldsShouldNotBeVisible")]
        public static FileAccess gOpenGrantedAccess;

        /// <summary>
        /// The access requested for this Open but not yet granted, as specified in [MS-SMB2] section 2.2.13.1.
        /// Open.RemainingDesiredAccess used in 3.1.5.1
        /// </summary>
        /// Disable warning CA2211, because this action confuses with the actual model design if modify it.
        [SuppressMessage("Microsoft.Usage", "CA2211:NonConstantFieldsShouldNotBeVisible")]
        public static FileAccess gOpenRemainingDesiredAccess;

        /// <summary>
        /// The sharing mode for this Open as specified in [MS-SMB2] section 2.2.13.
        /// Open.SharingMode used in 3.1.5.1
        /// </summary>
        /// Disable warning CA2211, because this action confuses with the actual model design if modify it.
        [SuppressMessage("Microsoft.Usage", "CA2211:NonConstantFieldsShouldNotBeVisible")]
        public static ShareAccess gOpenSharingMode;

        /// <summary>
        /// The mode flags for this Open as specified in [MS-FSCC] section 2.4.24.
        /// Open.Mode used in 3.1.5.1
        /// </summary>
        /// Disable warning CA2211, because this action confuses with the actual model design if modify it.
        [SuppressMessage("Microsoft.Usage", "CA2211:NonConstantFieldsShouldNotBeVisible")]
        public static CreateOptions gOpenMode;

        /// <summary>
        /// A boolean that is true if the Open was performed by a user who is allowed to perform backup operations.
        /// Open.HasBackupAccess used in 3.1.5.1 and 3.1.5.9.5
        /// </summary>
        /// Disable warning CA2211, because this action confuses with the actual model design if modify it.
        [SuppressMessage("Microsoft.Usage", "CA2211:NonConstantFieldsShouldNotBeVisible")]
        public static bool isOpenHasBackupAccess;

        /// <summary>
        /// A boolean that is true if the Open was performed by a user who is allowed to perform restore operations.
        /// Open.HasRestoreAccess used in 3.1.5.1 and 3.1.5.9.23
        /// </summary>
        /// Disable warning CA2211, because this action confuses with the actual model design if modify it.
        [SuppressMessage("Microsoft.Usage", "CA2211:NonConstantFieldsShouldNotBeVisible")]
        public static bool isOpenHasRestoreAccess;

        /// <summary>
        /// A boolean that is true if the Open was performed by a user who is allowed to create symbolic links.
        /// Open.HasCreateSymbolicLinkAccess used in 3.1.5.1 and 3.1.5.9.25
        /// </summary>
        /// Disable warning CA2211, because this action confuses with the actual model design if modify it.
        [SuppressMessage("Microsoft.Usage", "CA2211:NonConstantFieldsShouldNotBeVisible")]
        public static bool isOpenHasCreateSymbolicLinkAccess;

        /// <summary>
        /// Open.HasManageVolumeAccess used in 3.1.5.1 and 3.1.5.9.5 and 3.1.5.14.14
        /// </summary>
        /// Disable warning CA2211, because this action confuses with the actual model design if modify it.
        [SuppressMessage("Microsoft.Usage", "CA2211:NonConstantFieldsShouldNotBeVisible")]
        public static bool isOpenHasManageVolumeAccess;

        /// <summary>
        /// A boolean that is true if the Open was performed by a user who is allowed to manage the volume.
        /// Open.IsAdministrator used in 3.1.5.1 and 3.1.5.9.30
        /// </summary>
        /// Disable warning CA2211, because this action confuses with the actual model design if modify it.
        [SuppressMessage("Microsoft.Usage", "CA2211:NonConstantFieldsShouldNotBeVisible")]
        public static bool isOpenIsAdministrator;

        /// <summary>
        /// The type of file to open. This value MUST be either DataFile or DirectoryFile.
        /// FileTypeToOpen used in 3.1.5.1
        /// </summary>
        /// Disable warning CA2211, because this action confuses with the actual model design if modify it.
        [SuppressMessage("Microsoft.Usage", "CA2211:NonConstantFieldsShouldNotBeVisible")]
        public static FileType gfileTypeToOpen;

        /// <summary>
        /// A code defining the action taken by the open operation, as specified in [MS-SMB2] 
        /// section 2.2.14 for the CreateAction field
        /// </summary>
        /// Disable warning CA2211, because this action confuses with the actual model design if modify it.
        [SuppressMessage("Microsoft.Usage", "CA2211:NonConstantFieldsShouldNotBeVisible")]
        public static CreateAction gCreateAction;

        /// <summary>
        /// A list of zero or more ByteRangeLocks describing the bytes ranges of 
        /// this stream that are currently locked.
        /// </summary>
        /// Disable warning CA2211, because this action confuses with the actual model design if modify it.
        [SuppressMessage("Microsoft.Usage", "CA2211:NonConstantFieldsShouldNotBeVisible")]
        public static SequenceContainer<ByteRangeLock> ByteRangeLockList = new SequenceContainer<ByteRangeLock>();

        /// <summary>
        /// The type of stream. This value MUST be either DataStream or DirectoryStream.
        /// Stream.StreamType
        /// </summary>
        /// Disable warning CA2211, because this action confuses with the actual model design if modify it.
        [SuppressMessage("Microsoft.Usage", "CA2211:NonConstantFieldsShouldNotBeVisible")]
        public static StreamType gStreamType;

        /// <summary>
        /// A boolean that is true if the volume is read-only and MUST NOT be modified; 
        /// otherwise, the volume is both readable and writable.
        /// true: If RootOpen.Volume.IsReadOnly
        /// </summary>
        /// Disable warning CA2211, because this action confuses with the actual model design if modify it.
        [SuppressMessage("Microsoft.Usage", "CA2211:NonConstantFieldsShouldNotBeVisible")]
        public static bool isFileVolumeReadOnly;

        /// <summary>
        /// A boolean that is true if Open.File.Volume.IsUsnJournalActive is TRUE and MUST NOT be modified;
        /// </summary>
        /// Disable warning CA2211, because this action confuses with the actual model design if modify it.
        [SuppressMessage("Microsoft.Usage", "CA2211:NonConstantFieldsShouldNotBeVisible")]
        public static bool isFileVolumeUsnJournalActive;

        /// <summary>
        /// The DirectoryFile for the root of this volume.
        /// True: If File == File.Volume.RootDirectory
        /// </summary>
        /// Disable warning CA2211, because this action confuses with the actual model design if modify it.
        [SuppressMessage("Microsoft.Usage", "CA2211:NonConstantFieldsShouldNotBeVisible")]
        public static bool isFileEqualRootDirectory;

        /// <summary>
        /// Indicate if Open.File.Volume is NTFS file system.
        /// </summary>
        /// Disable warning CA2211, because this action confuses with the actual model design if modify it.
        [SuppressMessage("Microsoft.Usage", "CA2211:NonConstantFieldsShouldNotBeVisible")]
        public static bool isFileVolumeNtfsFileSystem;
        #endregion

        #region 3.1.5.2 global variables

        /// <summary>
        /// The byte offset immediately following the most recent successful synchronous read or write operation
        /// of one or more bytes, or 0 if there have not been any.
        /// Open.CurrentByteOffset seted in 3.1.5.2
        /// </summary>
        /// Disable warning CA2211, because this action confuses with the actual model design if modify it.
        [SuppressMessage("Microsoft.Usage", "CA2211:NonConstantFieldsShouldNotBeVisible")]
        public static long gOpenCurrentOffset;

        /// <summary>
        /// file volume size
        /// </summary>
        /// Disable warning CA2211, because this action confuses with the actual model design if modify it.
        [SuppressMessage("Microsoft.Usage", "CA2211:NonConstantFieldsShouldNotBeVisible")]
        public static long gOpenFileVolumeSize;

        #endregion

        #region 3.1.5.12 global variable

        /// <summary>
        /// get if Quotas is supported.
        /// </summary>
        /// Disable warning CA2211, because this action confuses with the actual model design if modify it.
        [SuppressMessage("Microsoft.Usage", "CA2211:NonConstantFieldsShouldNotBeVisible")]
        public static bool gisQuotasSupported;

        /// <summary>
        /// get if ObjectIDs is supported
        /// </summary>
        /// Disable warning CA2211, because this action confuses with the actual model design if modify it.
        [SuppressMessage("Microsoft.Usage", "CA2211:NonConstantFieldsShouldNotBeVisible")]
        public static bool gisObjectIDsSupported;

        #endregion

        #region 3.1.5.17 global variable

        /// <summary>
        /// The current state of the oplock, expressed as a combination of one or more flags. 
        /// Oplock.State set in 3.1.5.17.1
        /// </summary>
        /// Disable warning CA2211, because this action confuses with the actual model design if modify it.
        [SuppressMessage("Microsoft.Usage", "CA2211:NonConstantFieldsShouldNotBeVisible")]
        public static OplockState gOplockState;

        #endregion

        #region 3.1.5.14 global variable

        /// <summary>
        /// Attributes of the file in the form specified in [MS-FSCC] section 2.6.
        /// </summary>
        /// Disable warning CA2211, because this action confuses with the actual model design if modify it.
        [SuppressMessage("Microsoft.Usage", "CA2211:NonConstantFieldsShouldNotBeVisible")]
        public static FileAttribute gFileAttribute;

        /// <summary>
        /// A boolean that is true if the contents of the stream are compressed.
        /// </summary>
        /// Disable warning CA2211, because this action confuses with the actual model design if modify it.
        [SuppressMessage("Microsoft.Usage", "CA2211:NonConstantFieldsShouldNotBeVisible")]
        public static bool gOpenStreamIsCompressed;

        /// <summary>
        /// A boolean that is true if the object store is storing a sparse representation of the stream.
        /// </summary>
        /// Disable warning CA2211, because this action confuses with the actual model design if modify it.
        [SuppressMessage("Microsoft.Usage", "CA2211:NonConstantFieldsShouldNotBeVisible")]
        public static bool gOpenStreamIsSparse;

        /// <summary>
        /// A boolean that is true if short name creation support is enabled on this Volume. 
        /// FALSE if short name creation is not supported on this Volume.
        /// </summary>
        /// Disable warning CA2211, because this action confuses with the actual model design if modify it.
        [SuppressMessage("Microsoft.Usage", "CA2211:NonConstantFieldsShouldNotBeVisible")]
        public static bool gOpenGenerateShortNames;

        /// <summary>
        /// True if TargetStream is found. Used in section 3.1.5.14.11.1
        /// </summary>
        /// Disable warning CA2211, because this action confuses with the actual model design if modify it.
        [SuppressMessage("Microsoft.Usage", "CA2211:NonConstantFieldsShouldNotBeVisible")]
        public static bool gIsTargetStreamFound;

        /// <summary>
        /// True if the size of TargetStream is not 0. Used in section 3.1.5.14.11.1
        /// </summary>
        /// Disable warning CA2211, because this action confuses with the actual model design if modify it.
        [SuppressMessage("Microsoft.Usage", "CA2211:NonConstantFieldsShouldNotBeVisible")]
        public static bool gIsTargetStreamSizeNotZero;

        #endregion 3.1.5.14 global variable

        #region 3.1.5.14 call return

        #region get if gOpenGenerateShortNames

        /// <summary>
        /// The call part of the method GetIFQuotasSupported which is used to
        /// get if ObjectIDs is supported.
        /// This is a call-return pattern
        /// </summary>
        [Rule(Action = "call GetOpenGenerateShortNames(out _)")]
        public static void CallGetOpenGenerateShortNames()
        {
        }

        /// <summary>
        /// The return part of the method GetIFQuotasSupported which is used to 
        /// get if ObjectIDs is supported.
        /// This is a call-returns pattern. 
        /// </summary>
        /// <param name="GenerateShortNames">true: if ObjectIDs is supported</param>
        [Rule(Action = "return GetOpenGenerateShortNames(out GenerateShortNames)")]
        public static void ReturnGetOpenGenerateShortNames(bool GenerateShortNames)
        {
            gOpenGenerateShortNames = GenerateShortNames;
        }

        #endregion get if gOpenGenerateShortNames

        /// <summary>
        /// True if file.Openlist contains the stream specified.
        /// </summary>
        /// Disable warning CA2211, because this action confuses with the actual model design if modify it.
        [SuppressMessage("Microsoft.Usage", "CA2211:NonConstantFieldsShouldNotBeVisible")]
        public static bool gIsOpenListContains;

        /// <summary>
        /// Call function GetIsOpenListContains.
        /// </summary>
        [Rule(Action = "call GetIsOpenListContains(out _)")]
        public static void CallGetIsOpenListContains()
        {

        }

        /// <summary>
        /// Return the result of function GetIsOpenListContains.
        /// </summary>
        /// <param name="IsOpenListContains">A flag</param>
        [Rule(Action = "return GetIsOpenListContains(out IsOpenListContains)")]
        public static void ReturnGetIsOpenListContains(bool IsOpenListContains)
        {
            gIsOpenListContains = IsOpenListContains;
        }

        //Search DestinationDirectory.File.DirectoryList for an ExistingLink where ExistingLink.Name 
        //or ExistingLink.ShortName matches FileName using case-sensitivity according to Open.IsCaseSensitive
        //If such a link is found:
        /// <summary>
        /// True if the link is found.
        /// </summary>
        /// Disable warning CA2211, because this action confuses with the actual model design if modify it.
        [SuppressMessage("Microsoft.Usage", "CA2211:NonConstantFieldsShouldNotBeVisible")]
        public static bool gIsLinkFound;

        /// <summary>
        /// Call function GetIsLinkFound.
        /// </summary>
        [Rule(Action = "call GetIsLinkFound(out _)")]
        public static void CallGetIsLinkFound()
        {

        }

        /// <summary>
        /// Return the result of function GetIsLinkFound.
        /// </summary>
        /// <param name="IsLinkFound">A flag</param>
        [Rule(Action = "return GetIsLinkFound(out IsLinkFound)")]
        public static void ReturnGetIsLinkFound(bool IsLinkFound)
        {
            gIsLinkFound = IsLinkFound;
        }


        #endregion   3.1.5.14 call return

        #endregion

        #region Initialize

        /// <summary>
        /// FSA initialize, it contains the follow operations:
        /// 1. The client connects to server.
        /// 2. The client sets up a session with server.
        /// 3. The client connects to a share on server.
        /// </summary>
        [Rule]
        public static void FsaInitial()
        {
            Condition.IsTrue(fsStates == FSStates.ReadyInitial);
            fsStates = FSStates.FinsihInitial;
        }

        #endregion

        #region rule methods

        #region get sut information

        /// <summary>
        /// The call part of the method GetOSInfo which is used to
        /// get the SUT platform.
        /// This is a call-return pattern
        /// </summary>
        [Rule(Action = "call GetOSInfo(out _)")]
        public static void CallGetSutInfo()
        {
            Condition.IsTrue(fsStates == FSStates.FinsihInitial);
            Condition.IsTrue(sutOSInfo == SutOSInfo.ReadyGetSutInfo);
            sutOSInfo = SutOSInfo.RequestGetSutInfo;
        }

        /// <summary>
        /// The return part of the method GetOSInfo which is used to 
        /// get the SUT platform.
        /// This is a call-returns pattern. 
        /// </summary>
        /// <param name="platForm">SUT platform</param>
        [Rule(Action = "return GetOSInfo(out platForm)")]
        public static void ReturnGetSutInfo(PlatformType platForm)
        {
            Condition.IsTrue(fsStates == FSStates.FinsihInitial);
            Condition.IsTrue(sutOSInfo == SutOSInfo.RequestGetSutInfo);
            sutPlatForm = platForm;
            sutOSInfo = SutOSInfo.FinishGetSutInfo;

            // Force Spec Explorer to expand sutPlatForm.
            Condition.IsTrue(sutPlatForm == PlatformType.NoneWindows || sutPlatForm == PlatformType.Windows);
        }

        #endregion

        #region get system config

        /// <summary>
        /// The call part of the method GetOSInfo which is used to
        /// get the system config information.
        /// This is a call-return pattern
        /// </summary>
        [Rule(Action = "call GetSystemConfig(out _)")]
        public static void CallGetystemConfig()
        {
        }

        /// <summary>
        /// The return part of the method GetOSInfo which is used to 
        /// get the system config information.
        /// This is a call-returns pattern. 
        /// </summary>
        /// <param name="securityContext">SSecurityContext</param>
        [Rule(Action = "return GetSystemConfig(out securityContext)")]
        public static void ReturnGetystemConfig(SSecurityContext securityContext)
        {
            Condition.IsTrue(securityContext.privilegeSet == PrivilegeSet.SeRestorePrivilege);
            Condition.IsTrue(!securityContext.isImplementsEncryption);
            Condition.IsTrue(!securityContext.isSecurityContextSIDsContainWellKnown);
            gSecurityContext = securityContext;
        }

        #endregion

        #region get file volum size

        /// <summary>
        /// The call part of the method GetFileVolumSize which is used to
        /// get file volum size.
        /// This is a call-return pattern
        /// </summary>
        [Rule(Action = "call GetFileVolumSize(out _)")]
        public static void CallGetFileVolumSize()
        {
        }

        /// <summary>
        /// The return part of the method GetFileVolumSize which is used to 
        /// get file volum size.
        /// This is a call-returns pattern. 
        /// </summary>
        /// <param name="openFileVolumeSize">file volum size</param>
        [Rule(Action = "return GetFileVolumSize(out openFileVolumeSize)")]
        public static void ReturnGetFileVolumSize(long openFileVolumeSize)
        {
            // It need openFileVolumeSize equal to 4096
            Condition.IsTrue(openFileVolumeSize == 4096);
            gOpenFileVolumeSize = openFileVolumeSize;
        }

        #endregion

        #region get if Quotas is Supported

        /// <summary>
        /// The call part of the method GetIFQuotasSupported which is used to
        /// get if Quotas is supported.
        /// This is a call-return pattern
        /// </summary>
        [Rule(Action = "call GetIFQuotasSupported(out _)")]
        public static void CallGetIFQuotasSupported()
        {
        }

        /// <summary>
        /// The return part of the method GetIFQuotasSupported which is used to 
        /// get if Quotas is supported.
        /// This is a call-returns pattern. 
        /// </summary>
        /// <param name="isQuotasSupported">true: if Quotas is supported</param>
        [Rule(Action = "return GetIFQuotasSupported(out isQuotasSupported)")]
        public static void ReturnGetIFQuotasSupported(bool isQuotasSupported)
        {
            gisQuotasSupported = isQuotasSupported;
        }

        #endregion

        #region get if ObjectIDs is supported

        /// <summary>
        /// The call part of the method GetIFQuotasSupported which is used to
        /// get if ObjectIDs is supported.
        /// This is a call-return pattern
        /// </summary>
        [Rule(Action = "call GetIFObjectIDsSupported(out _)")]
        public static void CallGetIFObjectIDsSupported()
        {
        }

        /// <summary>
        /// The return part of the method GetIFQuotasSupported which is used to 
        /// get if ObjectIDs is supported.
        /// This is a call-returns pattern. 
        /// </summary>
        /// <param name="isObjectIDsSupported">true: if ObjectIDs is supported</param>
        [Rule(Action = "return GetIFObjectIDsSupported(out isObjectIDsSupported)")]
        public static void ReturnGetIFObjectIDsSupported(bool isObjectIDsSupported)
        {
            gisObjectIDsSupported = isObjectIDsSupported;
        }

        #endregion

        #endregion
    }
}
