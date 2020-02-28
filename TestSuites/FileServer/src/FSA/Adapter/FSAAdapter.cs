// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Modeling;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Cifs;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Fscc;
using Microsoft.SpecExplorer.Runtime.Testing;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Security.AccessControl;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using Smb2 = Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter;

namespace Microsoft.Protocols.TestSuites.FileSharing.FSA.Adapter
{
    /// <summary>
    /// A base class that implements ManagedAdapterBase and IFSAAdapter
    /// </summary>
    /// Disable warning CA1506 because it will affect the implementation of Adapter and Model codes if do any changes about maintainability.
    [SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling")]
    public partial class FSAAdapter : ManagedAdapterBase, IFSAAdapter
    {
        # region Define fields
        private FSATestConfig testConfig;
        private bool isWindows;
        private bool isIntegritySupported;
        private bool isQuotaSupported;
        private bool isReparsePointSupported;
        private bool isObjectIDsSupported;
        private bool isOffloadImplemented;
        private bool isEncryptionSupported;
        private bool isCompressionSupported;
        private bool isExtendedAttributeSupported;
        private bool isSparseFileSupported;
        private bool isQueryAllocatedRangesSupported;
        private bool isSetZeroDataSupported;
        private bool isFileLinkInfoSupported;
        private bool isShortNameSupported;
        private bool isQueryFileFsControlInformationSupported;
        private bool isQueryFileFsObjectIdInformationSupported;
        private bool isQueryFileObjectIdInformationSupported;
        private bool isQueryFileReparsePointInformationSupported;
        private bool isQueryFileQuotaInformationSupported;
        private bool isObjectIdIoCtlRequestSupported;
        private bool isOpenHasManageVolumeAccessSupported;
        private bool isStreamRenameSupported;

        private bool isErrorCodeMappingRequired;
        private bool isVolumeReadonly;
        private uint clusterSizeInKB;
        private uint systemPageSizeInKB;

        private long gLockOffset;
        private long gLockLength;
        private bool gLockIsExclusive;
        private bool gLockIsConflicted;
        private string fileName;
        private string shareName;
        private Transport transport;
        private FileSystem fileSystem;
        private uint reFSVersion;
        private IpVersion ipVersion;
        private ITestSite site;
        private ITransportAdapter transAdapter;
        public UInt32 transBufferSize; // Make it accessible from test cases' code.
        private string rootDirectory;
        private string quotaFile;
        private string reparsePointFile;
        private CreateOptions gOpenMode;
        private FileAccess gOpenGrantedAccess;
        private StreamType gStreamType;
        private List<string> activeTDIs;
        public bool Is64bitFileIdSupported;
        public bool IsChangeTimeSupported;
        // Used to generate random file names.
        private static Random randomRange = new Random();

        // Used to clean up the generated test files.
        protected ISutProtocolControlAdapter sutProtocolController;
        protected List<string> testFiles = new List<string>();
        protected List<string> testDirectories = new List<string>();
        # endregion

        #region Properties
        public FSATestConfig TestConfig
        {
            get { return testConfig; }
        }

        public string FileName
        {
            get { return fileName; }
        }

        public FileSystem FileSystem
        {
            get { return fileSystem; }
        }

        public uint ReFSVersion
        {
            get { return reFSVersion; }
        }

        public bool IsIntegritySupported
        {
            get { return isIntegritySupported; }
        }

        public bool IsOffloadImplemented
        {
            get { return isOffloadImplemented; }
        }

        public bool IsObjectIDsSupported
        {
            get { return isObjectIDsSupported; }
        }

        public bool IsEncryptionSupported
        {
            get { return isEncryptionSupported; }
        }

        public bool IsCompressionSupported
        {
            get { return isCompressionSupported; }
        }

        public bool IsExtendedAttributeSupported
        {
            get { return isExtendedAttributeSupported; }
        }

        public bool IsSparseFileSupported
        {
            get { return isSparseFileSupported; }
        }

        public bool IsQueryAllocatedRangesSupported
        {
            get { return isQueryAllocatedRangesSupported; }
        }

        public bool IsReparsePointSupported
        {
            get { return isReparsePointSupported; }
        }

        public bool IsSetZeroDataSupported
        {
            get { return isSetZeroDataSupported; }
        }

        public bool IsQuotaSupported
        {
            get { return isQuotaSupported; }
        }

        public bool IsFileLinkInfoSupported
        {
            get { return isFileLinkInfoSupported; }
        }

        public bool IsShortNameSupported
        {
            get { return isShortNameSupported; }
        }

        public uint ClusterSizeInKB
        {
            get { return clusterSizeInKB; }
        }

        public uint SystemPageSizeInKB
        {
            get { return systemPageSizeInKB; }
        }

        public bool IsVolumeReadonly
        {
            get { return isVolumeReadonly; }
        }

        public Transport Transport
        {
            get { return transport; }
        }

        public List<string> ActiveTDIs
        {
            get { return activeTDIs; }
        }

        public string RootDirectory
        {
            get { return rootDirectory; }
        }

        public string QuotaFile
        {
            get { return quotaFile; }
        }

        public string ReparsePointFile
        {
            get { return reparsePointFile; }
        }

        public string ShareName
        {
            get { return shareName; }
            set
            {
                shareName = value;
                transAdapter.ShareName = value;
            }
        }

        public string UncSharePath
        {
            get
            {
                return "\\\\" + testConfig.SutComputerName + "\\" + this.shareName;
            }
        }

        protected string CurrentTestCaseName
        {
            get
            {
                string fullName = (string)Site.TestProperties["CurrentTestCaseName"];
                return fullName.Split('.').LastOrDefault();
            }
        }
        #endregion

        #region Initialize

        /// <summary>
        /// Initialize method, will be called automatically by PTF before all cases executed.
        /// </summary>
        /// <param name="testSite">ITestSite object which can get PTF settings.</param>
        public override void Initialize(ITestSite testSite)
        {
            //Test Site Configuration
            testSite = ReqConfigurableSite.GetReqConfigurableSite(testSite) as ITestSite;
            base.Initialize(testSite);
            this.site = base.Site;
            Site.DefaultProtocolDocShortName = "MS-FSA";
            this.testConfig = new FSATestConfig(this.site);

            //SUT Configuration            
            this.isWindows = testConfig.Platform == Common.Adapter.Platform.NonWindows ? false : true;

            this.ipVersion = testConfig.SutIPAddress.AddressFamily == AddressFamily.InterNetworkV6 ? IpVersion.Ipv6 : IpVersion.Ipv4;

            //Transport Configuration
            Smb2.DialectRevision[] negotiateDialects = Smb2.Smb2Utility.GetDialects(testConfig.MaxSmbVersionSupported);
            this.transport = (Transport)Enum.Parse(typeof(Transport), testConfig.GetProperty("Transport"));
            #region Select transAdapter according to transport
            switch (this.transport)
            {
                case Transport.SMB:
                    this.transAdapter = new SmbTransportAdapter(testConfig);
                    break;

                case Transport.SMB2:
                    this.transAdapter = new Smb2TransportAdapter(new Smb2.DialectRevision[] { Smb2.DialectRevision.Smb2002, Smb2.DialectRevision.Smb21 }, testConfig);
                    break;

                case Transport.SMB3:
                    this.transAdapter = new Smb2TransportAdapter(negotiateDialects, testConfig);
                    break;

                default:
                    throw new Exception("Only support SMB, SMB2 and SMB3 transport.");
            }
            this.transAdapter.Initialize(this.site);
            #endregion

            // Signing Configuration
            this.transAdapter.IsSendSignedRequest = testConfig.SendSignedRequest;

            //File System Under Test
            this.fileSystem = (FileSystem)Enum.Parse(typeof(FileSystem), testConfig.GetProperty("FileSystem"));
            this.reFSVersion = uint.Parse(testConfig.GetProperty("ReFSVersion"));
            this.shareName = testConfig.GetProperty((fileSystem.ToString() + "_ShareFolder"));
            this.rootDirectory = testConfig.GetProperty((fileSystem.ToString() + "_RootDirectory"));
            this.quotaFile = testConfig.GetProperty("QuotaFile");
            this.reparsePointFile = testConfig.GetProperty("ReparsePointFile");

            //Supported features for the file system
            this.isIntegritySupported = testConfig.GetProperty("WhichFileSystemSupport_Integrity").Contains(this.fileSystem.ToString());
            this.isQuotaSupported = testConfig.GetProperty("WhichFileSystemSupport_Quota").Contains(this.fileSystem.ToString());
            this.isReparsePointSupported = testConfig.GetProperty("WhichFileSystemSupport_ReparsePoint").Contains(this.fileSystem.ToString());
            this.isObjectIDsSupported = testConfig.GetProperty("WhichFileSystemSupport_ObjectID").Contains(this.fileSystem.ToString());
            this.isOffloadImplemented = testConfig.GetProperty("WhichFileSystemSupport_Offload").Contains(this.fileSystem.ToString());
            this.isCompressionSupported = testConfig.GetProperty("WhichFileSystemSupport_Compression").Contains(this.fileSystem.ToString());
            this.isEncryptionSupported = testConfig.GetProperty("WhichFileSystemSupport_Encryption").Contains(this.fileSystem.ToString());
            this.isExtendedAttributeSupported = testConfig.GetProperty("WhichFileSystemSupport_ExtendedAttribute").Contains(this.fileSystem.ToString());
            this.isSparseFileSupported = testConfig.GetProperty("WhichFileSystemSupport_SparseFile").Contains(this.fileSystem.ToString());
            this.isQueryAllocatedRangesSupported = testConfig.GetProperty("WhichFileSystemSupport_QueryAllocatedRanges").Contains(this.fileSystem.ToString());
            this.isSetZeroDataSupported = testConfig.GetProperty("WhichFileSystemSupport_SetZeroData").Contains(this.fileSystem.ToString());
            this.isFileLinkInfoSupported = testConfig.GetProperty("WhichFileSystemSupport_FileLinkInfo").Contains(this.fileSystem.ToString());
            this.isShortNameSupported = testConfig.GetProperty("WhichFileSystemSupport_ShortName").Contains(this.fileSystem.ToString());
            this.isQueryFileFsControlInformationSupported = testConfig.GetProperty("WhichFileSystemSupport_QueryFileFsControlInformation").Contains(this.fileSystem.ToString());
            this.isQueryFileFsObjectIdInformationSupported = testConfig.GetProperty("WhichFileSystemSupport_QueryFileFsObjectIdInformation").Contains(this.fileSystem.ToString());
            this.isQueryFileObjectIdInformationSupported = testConfig.GetProperty("WhichFileSystemSupport_QueryFileObjectIdInformation").Contains(this.fileSystem.ToString());
            this.isQueryFileReparsePointInformationSupported = testConfig.GetProperty("WhichFileSystemSupport_QueryFileReparsePointInformation").Contains(this.fileSystem.ToString());
            this.isQueryFileQuotaInformationSupported = testConfig.GetProperty("WhichFileSystemSupport_QueryFileQuotaInformation").Contains(this.fileSystem.ToString());
            this.isObjectIdIoCtlRequestSupported = testConfig.GetProperty("WhichFileSystemSupport_ObjectIdIoCtlRequest").Contains(this.fileSystem.ToString());
            this.isOpenHasManageVolumeAccessSupported = testConfig.GetProperty("WhichFileSystemSupport_OpenHasManageVolumeAccess").Contains(this.fileSystem.ToString());
            this.isStreamRenameSupported = testConfig.GetProperty("WhichFileSystemSupport_StreamRename").Contains(this.fileSystem.ToString());

            //Volume Properties
            this.clusterSizeInKB = uint.Parse(testConfig.GetProperty((fileSystem.ToString() + "_ClusterSizeInKB")));

            this.systemPageSizeInKB = uint.Parse(testConfig.GetProperty("SystemPageSizeInKB"));
            this.isVolumeReadonly = bool.Parse(testConfig.GetProperty("IsVolumeReadonly"));

            //Error Code Handling
            this.isErrorCodeMappingRequired = bool.Parse(testConfig.GetProperty("IsErrorCodeMappingRequired"));

            //TDI Configurations
            this.activeTDIs = new List<string>(testConfig.GetProperty("FsaActiveTDIs").Split(';'));

            //Other Configurations
            this.transBufferSize = uint.Parse(testConfig.GetProperty("BufferSize"));
            this.Is64bitFileIdSupported = bool.Parse(testConfig.GetProperty("Is64bitFileIdSupported"));
            this.IsChangeTimeSupported = bool.Parse(testConfig.GetProperty("IsChangeTimeSupported"));

            sutProtocolController = Site.GetAdapter<ISutProtocolControlAdapter>();

            this.Reset();
        }

        /// <summary>
        /// Reset, will be called automatically after a case executed.
        /// </summary>
        public override void Reset()
        {
            base.Reset();

            //Since the transport's reset function cannot be called by SE, we call it manually
            this.transAdapter.BufferSize = this.transBufferSize;
            this.transAdapter.Domain = testConfig.DomainName;
            this.transAdapter.IPVersion = this.ipVersion;
            this.transAdapter.Password = testConfig.UserPassword;
            this.transAdapter.Port = 445;
            this.transAdapter.ServerName = testConfig.SutComputerName;
            this.transAdapter.ShareName = this.shareName;
            this.transAdapter.UserName = testConfig.UserName;
            this.transAdapter.Timeout = testConfig.Timeout;
            this.transAdapter.Reset();

            CleanTestFiles();
        }

        /// <summary>
        /// Dispose
        /// </summary>
        /// <param name="disposing">Boolean value indicating whether dispose.</param>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (this.transAdapter != null)
            {
                transAdapter.Dispose();
                this.transAdapter = null;
            }

            CleanTestFiles();
        }

        protected void CleanTestFiles()
        {
            foreach (var fileName in testFiles)
            {
                try
                {
                    // Remove the appendix. e.g. "::$DATA", ":$I30"
                    int i = fileName.IndexOf(":");
                    string temp = fileName;
                    if (i != -1)
                    {
                        temp = fileName.Substring(0, i);
                    }
                    sutProtocolController.DeleteFile("\\\\" + testConfig.SutComputerName + "\\" + this.shareName, temp);
                }
                catch
                {
                }
            }
            testFiles.Clear();

            foreach (var directory in testDirectories)
            {
                try
                {
                    int i = directory.IndexOf(":");
                    string temp = directory;
                    if (i != -1)
                    {
                        temp = directory.Substring(0, i);
                    }
                    sutProtocolController.DeleteDirectory("\\\\" + testConfig.SutComputerName + "\\" + this.shareName, temp);
                }
                catch
                {
                }
            }
            testDirectories.Clear();
        }

        protected void AddTestFileName(CreateOptions createOptions, string fileName)
        {
            if (createOptions.HasFlag(CreateOptions.DIRECTORY_FILE))
            {
                testDirectories.Add(fileName);
            }
            else
            {
                testFiles.Add(fileName);
            }
        }

        /// <summary>
        /// FSA initialize, it contains the follow operations:
        /// 1. The client connects to server.
        /// 2. The client sets up a session with server.
        /// 3. The client connects to a share on server.
        /// </summary>
        public void FsaInitial()
        {
            this.transAdapter.Initialize(this.isWindows);
        }

        #endregion

        #region 3.1.5   Higher-Layer Triggered Events

        #region 3.1.5.1   Server Requests an Open of a File

        #region 3.1.5.1.1   Creation of a New File

        /// <summary>
        /// Implement CreateFile method
        /// </summary>
        /// <param name="desiredFileAttribute">Desired file attribute</param>
        /// <param name="createOption">Specifies the options to be applied when creating or opening the file</param>
        /// <param name="streamTypeNameToOpen">The name of stream type to open</param>
        /// <param name="desiredAccess">A bitmask indicating desired access for the open, as specified in [MS-SMB2] section 2.2.13.1.</param>
        /// <param name="shareAccess">A bitmask indicating sharing access for the open, as specified in [MS-SMB2] section 2.2.13.</param>
        /// <param name="createDisposition">The desired disposition for the open, as specified in [MS-SMB2] section 2.2.13.</param>
        /// <param name="streamFoundType">Indicate if the stream is found or not.</param>
        /// <param name="symbolicLinkType">Indicate if it is symbolic link or not.</param>
        /// <param name="openFileType">Filetype of open file</param>
        /// <param name="fileNameStatus">File name status</param>
        /// <returns>An NTSTATUS code that specifies the result.</returns>
        /// Disable warning CA1502 because it will affect the implementation of Adapter and Model codes if do any changes about maintainability.
        [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        public MessageStatus CreateFile(
            FileAttribute desiredFileAttribute,
            CreateOptions createOption,
            StreamTypeNameToOpen streamTypeNameToOpen,
            FileAccess desiredAccess,
            ShareAccess shareAccess,
            CreateDisposition createDisposition,
            StreamFoundType streamFoundType,
            SymbolicLinkType symbolicLinkType,
            FileType openFileType,
            FileNameStatus fileNameStatus
            )
        {
            gOpenMode = createOption;
            gStreamType = (streamTypeNameToOpen == StreamTypeNameToOpen.INDEX_ALLOCATION ? StreamType.DirectoryStream : StreamType.DataStream);

            uint createAction = 0;
            string randomFile = this.ComposeRandomFileName();

            this.fileName = randomFile;

            if (fileNameStatus == FileNameStatus.NotPathNameValid)
            {
                // The loop time is 32
                for (int i = 0; i < 32; i++)
                {
                    randomFile += testConfig.GetProperty("InvalidName");
                }
            }

            //Get the SymboLinkFile name on the server if SymbolicLink is required.
            else if (symbolicLinkType == SymbolicLinkType.IsSymbolicLink)
            {
                randomFile = testConfig.GetProperty("SymbolicLinkFile");

                if (this.FileSystem == FileSystem.FAT32)
                {
                    site.Assume.Inconclusive("Symbolic Link is not supported by FAT32 system.");
                }
            }

            //Retrieve the existing the folder
            else if (createDisposition == CreateDisposition.OPEN
                && openFileType == FileType.DirectoryFile)
            {
                randomFile = testConfig.GetProperty("ExistingFolder");
            }

            //Retrieve the existing the file
            else if (createDisposition == CreateDisposition.OPEN
                && openFileType == FileType.DataFile)
            {
                randomFile = testConfig.GetProperty("ExistingFile");
            }

            //Construct a path name with trailing backslash.
            else if ((fileNameStatus == FileNameStatus.BackslashName) &&
               (createOption & CreateOptions.NON_DIRECTORY_FILE) != 0)
            {
                if (fileSystem == Adapter.FileSystem.NTFS || fileSystem == Adapter.FileSystem.REFS)
                {
                    randomFile = randomFile + @"\\\\\\" + "::$DATA";
                }
                else
                {
                    // For file systems other than NTFS or ReFS, the constructed path name with trailing backslash should not contain "::$DATA"
                    randomFile = randomFile + @"\\\\\\";
                }
            }

            //Construct a data file for an directory operation to 
            //trigger the error code OBJECT_NAME_COLLISION
            //for creating operation and NOT_A_DIRECTORY for openning operation.
            else if ((createOption == CreateOptions.DIRECTORY_FILE) &&
               (fileNameStatus == FileNameStatus.OpenFileNotNull) &&
               (openFileType != FileType.DirectoryFile))
            {
                if (createDisposition == CreateDisposition.CREATE)
                {
                    randomFile = testConfig.GetProperty("ExistingFile");
                    randomFile = randomFile + "::$INDEX_ALLOCATION";
                }
                else if (createDisposition == CreateDisposition.OPEN)
                {
                    randomFile = testConfig.GetProperty("ExistingFile");
                    randomFile = randomFile + "::$DATA";
                }
            }

            //Construct a directory file for an data file operation to 
            //trigger the error code FILE_IS_A_DIRECTORY.
            else if ((createOption == CreateOptions.NON_DIRECTORY_FILE) &&
                (streamTypeNameToOpen == StreamTypeNameToOpen.NULL) &&
                (fileNameStatus != FileNameStatus.FileNameNull) && (openFileType == FileType.DirectoryFile))
            {
                randomFile = testConfig.GetProperty("ExistingFolder");
            }

            //Constuct message with CreateOptions.RANDOM_ACCESS and use file name to 
            //indicate the file type.
            else if (createDisposition == CreateDisposition.CREATE
                && (createOption & CreateOptions.RANDOM_ACCESS) != 0)
            {
                if (streamTypeNameToOpen == StreamTypeNameToOpen.DATA)
                {
                    randomFile = randomFile + "::$DATA";
                }
                else if (streamTypeNameToOpen == StreamTypeNameToOpen.INDEX_ALLOCATION)
                {
                    randomFile = randomFile + "::$INDEX_ALLOCATION";
                }
            }

            //Construct the Non-Existfile to trigger the message OBJECT_NAME_NOT_FOUND
            if (createDisposition == CreateDisposition.OPEN && streamFoundType == StreamFoundType.StreamIsNotFound)
            {
                randomFile = Guid.NewGuid().ToString();
            }

            //Construct the Non-Exist folder to trigger the error code OBJECT_PATH_NOT_FOUND
            if (createDisposition == CreateDisposition.OPEN && fileNameStatus == FileNameStatus.isprefixLinkNotFound)
            {
                randomFile = Guid.NewGuid().ToString();
            }

            if (fileNameStatus == FileNameStatus.StreamTypeNameIsINDEX_ALLOCATION)
            {
                // Full name of a stream is <filename>:<stream name>:<stream type>
                // If any StreamTypeNamei is "$INDEX_ALLOCATION" 
                // and the corresponding StreamNamei has a value other than an empty string or "$I30", 
                // the operation SHOULD be failed with STATUS_INVALID_PARAMETER.
                //
                // Set <stream name> as a random string (not an empty string or $I30) to test above requirement.
                string fileName = this.ComposeRandomFileName();
                string streamName = this.ComposeRandomFileName();
                randomFile = fileName + ":" + streamName + ":$INDEX_ALLOCATION";
            }

            //Construct path name with streamTypeNameToOpen
            if (streamTypeNameToOpen != StreamTypeNameToOpen.NULL &&
                symbolicLinkType != SymbolicLinkType.IsSymbolicLink)
            {
                if (streamTypeNameToOpen == StreamTypeNameToOpen.DATA
                    && !randomFile.Contains("$DATA"))
                {
                    randomFile = randomFile + "::$DATA";
                }
                else if (streamTypeNameToOpen == StreamTypeNameToOpen.INDEX_ALLOCATION
                    && !randomFile.Contains("$INDEX_ALLOCATION"))
                {
                    if ((desiredFileAttribute & FileAttribute.READONLY) == FileAttribute.READONLY &&
                        (createOption & CreateOptions.DELETE_ON_CLOSE) == CreateOptions.DELETE_ON_CLOSE &&
                        (fileSystem != Adapter.FileSystem.NTFS && fileSystem != Adapter.FileSystem.REFS))
                    {
                        /*
                         * To cover the below requirements in a file system other than NTFS and ReFS, remove complex suffix:
                         * Section 2.1.5.1.1 Create a New File
                         *     If DesiredFileAttributes.FILE_ATTRIBUTE_READONLY and CreateOptions.FILE_DELETE_ON_CLOSE are both set, 
                         * the operation MUST be failed with STATUS_CANNOT_DELETE.
                         * =>
                         *     If "::$INDEX_ALLOCATION" is added to the file name, it will return unexpected error codes in FAT32 that
                         * does not recognize the "::$INDEX_ALLOCATION" complex suffix.
                         */
                    }
                    else
                    {
                        randomFile = randomFile + "::$INDEX_ALLOCATION";
                    }
                }
                else if (streamTypeNameToOpen == StreamTypeNameToOpen.Other
                    && !randomFile.Contains("$TEST"))
                {
                    randomFile = randomFile + "::$TEST";
                }
            }

            MessageStatus returnedStatus = transAdapter.CreateFile(
                randomFile,
                (uint)desiredFileAttribute,
                (uint)desiredAccess,
                (uint)shareAccess,
                (uint)createOption,
                (uint)createDisposition,
                out createAction);

            gOpenGrantedAccess = returnedStatus == MessageStatus.SUCCESS ? desiredAccess : FileAccess.None;

            if (this.transport == Transport.SMB2 || this.transport == Transport.SMB3)
            {
                returnedStatus = SMB2_TDIWorkaround.WorkaroundCreateFile(fileNameStatus, createOption, desiredAccess, openFileType, desiredFileAttribute, returnedStatus, site);
            }
            else
            {
                returnedStatus = SMB_TDIWorkaround.WorkaroundCreateFile(fileSystem, fileNameStatus, createOption, desiredAccess, openFileType, desiredFileAttribute, returnedStatus, site);
            }

            /*
             * Work around for test cases only designed for NTFS and ReFS:
             * Make assertion in the adapter, then convert the return code according to the test case.
             */
            if (this.fileSystem != Adapter.FileSystem.NTFS && this.fileSystem != Adapter.FileSystem.REFS)
            {
                if ((createOption & CreateOptions.DIRECTORY_FILE) == CreateOptions.DIRECTORY_FILE &&
                (createOption & CreateOptions.NON_DIRECTORY_FILE) == CreateOptions.NON_DIRECTORY_FILE)
                {
                    /*
                     * To cover the below requirements in a file system other than NTFS and ReFS, remove complex suffix:
                     * Section 2.1.5.1 
                     * Phase 1 - Parameter Validation
                     *     If CreateOptions.FILE_DIRECTORY_FILE && CreateOptions.FILE_NON_DIRECTORY_FILE, 
                     * the operation MUST be failed with STATUS_INVALID_PARAMETER.
                     * =>
                     *     Return the status immediately.
                     * 
                     * Test Cases:    
                     *     CreateFileTestCaseS12
                     */
                    return returnedStatus;
                }
                else if (openFileType == FileType.DataFile && symbolicLinkType == SymbolicLinkType.IsSymbolicLink)
                {
                    /*
                     * To cover the below requirements in a file system other than NTFS and ReFS that does not support Symbolic Link:
                     * Section 2.1.5.1
                     * Phase 6 -- Location of file:
                     *     If Link.File.IsSymbolicLink is TRUE, the operation MUST be failed with Status set to STATUS_STOPPED_ON_SYMLINK
                     * and ReparsePointData set to Link.File.ReparsePointData.
                     * Section 2.1.5.1.2
                     *     Else if FileTypeToOpen is DataFile:
                     *     If CreateDisposition is FILE_CREATE, then the operation MUST be failed with STATUS_OBJECT_NAME_COLLISION.
                     * =>
                     * In NTFS and ReFS, these cases are expecting STATUS_STOPPED_ON_SYMLINK, while in other file system that does
                     * not support symbolic link will consider this action as creating a file that is already existed. The return 
                     * will be STATUS_OBJECT_NAME_COLLISION instead.
                     * 
                     * Test Cases:
                     *     CreateFileTestCaseS42
                     */
                    site.Log.Add(LogEntryKind.Checkpoint, @"Section 2.1.5.1.2
                                                            Else if FileTypeToOpen is DataFile:
                                                            If CreateDisposition is FILE_CREATE, then the operation MUST be failed with STATUS_OBJECT_NAME_COLLISION.");
                    site.Assert.AreEqual<MessageStatus>(MessageStatus.OBJECT_NAME_COLLISION, returnedStatus, "return of CreateFile");

                    // Make a fake return
                    return MessageStatus.STOPPED_ON_SYMLINK;
                }
                else if (randomFile.Contains("$STANDARD_INFORMATION")
                        || randomFile.Contains("$ATTRIBUTE_LIST")
                        || randomFile.Contains("$FILE_NAME")
                        || randomFile.Contains("$OBJECT_ID")
                        || randomFile.Contains("$SECURITY_DESCRIPTOR")
                        || randomFile.Contains("$VOLUME_NAME")
                        || randomFile.Contains("$VOLUME_INFORMATION")
                        || randomFile.Contains("$DATA")
                        || randomFile.Contains("$INDEX_ROOT")
                        || randomFile.Contains("$INDEX_ALLOCATION")
                        || randomFile.Contains("$BITMAP")
                        || randomFile.Contains("$REPARSE_POINT")
                        || randomFile.Contains("$EA_INFORMATION")
                        || randomFile.Contains("$EA")
                        || randomFile.Contains("$LOGGED_UTILITY_STREAM"))
                {
                    if (fileNameStatus == FileNameStatus.StreamTypeNameIsINDEX_ALLOCATION)
                    {
                        return MessageStatus.INVALID_PARAMETER;
                    }
                    else if (createDisposition == CreateDisposition.OPEN
                        || createDisposition == CreateDisposition.OVERWRITE
                        || createDisposition == CreateDisposition.OPEN_IF)
                    {
                        /*
                         * To cover the below requirements in a file system other than NTFS and ReFS that does not support stream type names:
                         * Section 2.1.5.1
                         * Phase 6 -- Location of file:
                         * If StreamTypeNameToOpen is non-empty and StreamTypeNameToOpen is not equal to one of the stream type names
                         * recognized by the object store<42> (using case-insensitive string comparisons), the operation MUST be failed
                         * with STATUS_OBJECT_NAME_INVALID.
                         * 
                         * Section 5
                         * <42> Section 2.1.5.1:  NTFS and ReFS recognize the following stream type names:
                         *      "$STANDARD_INFORMATION"
                         *      "$ATTRIBUTE_LIST"
                         *      "$FILE_NAME"
                         *      "$OBJECT_ID"
                         *      "$SECURITY_DESCRIPTOR"
                         *      "$VOLUME_NAME"
                         *      "$VOLUME_INFORMATION"
                         *      "$DATA"
                         *      "$INDEX_ROOT"
                         *      "$INDEX_ALLOCATION"
                         *      "$BITMAP"
                         *      "$REPARSE_POINT"
                         *      "$EA_INFORMATION"
                         *      "$EA"
                         *      "$LOGGED_UTILITY_STREAM"
                         * Other Windows file systems do not recognize any stream type names.
                         * =>
                         * In NTFS and ReFS, these cases are expecting STATUS_OBJECT_NAME_NOT_FOUND, while in other file system that does not
                         * support the the stream type names will return STATUS_OBJECT_NAME_INVALID.
                         * 
                         * Test Cases:
                         *     CreateFileTestCaseS38
                         *     CreateFileTestCaseS40
                         *     LockAndUnlockTestCaseS2
                         */
                        site.Log.Add(LogEntryKind.Checkpoint, @"Section 2.1.5.1
                                                                Phase 6 -- Location of file:
                                                                If StreamTypeNameToOpen is non-empty and StreamTypeNameToOpen is not equal to one of the stream type names
                                                                recognized by the object store<42> (using case-insensitive string comparisons), the operation MUST be failed
                                                                with STATUS_OBJECT_NAME_INVALID.");
                        site.Assert.AreEqual<MessageStatus>(MessageStatus.OBJECT_NAME_INVALID, returnedStatus, "return of CreateFile");

                        // Remove the complex name suffixes and try to create the file again.
                        randomFile = randomFile.Remove(randomFile.IndexOf(":"));
                        returnedStatus = transAdapter.CreateFile(
                            randomFile,
                            (uint)desiredFileAttribute,
                            (uint)desiredAccess,
                            (uint)shareAccess,
                            (uint)createOption,
                            (uint)createDisposition,
                            out createAction);
                        return returnedStatus;
                    }
                }
                else if (randomFile.Contains(":$I30")
                    || randomFile.Contains("::$INDEX_ALLOCATION")
                    || randomFile.Contains(":$I30:$INDEX_ALLOCATION")
                    || randomFile.Contains("::$BITMAP")
                    || randomFile.Contains(":$I30:$BITMAP")
                    || randomFile.Contains("::$ATTRIBUTE_LIST")
                    || randomFile.Contains("::$REPARSE_POINT"))
                {
                    /*
                     * To cover the below requirements in a file system other than NTFS and ReFS that does not complex name suffixes:
                     * Section 2.1.5.1
                     * Phase 6 -- Location of file:
                     * If ComplexNameSuffix is non-empty and ComplexNameSuffix is not equal to one of the complex name suffixes
                     * recognized by the object store<41> (using case-insensitive string comparisons), the operation MUST be 
                     * failed with STATUS_OBJECT_NAME_INVALID.
                     * 
                     * Section 5
                     * <41> Section 2.1.5.1:  NTFS and ReFS recognize the following complex name suffixes:
                     *      ":$I30"
                     *      "::$INDEX_ALLOCATION"
                     *      ":$I30:$INDEX_ALLOCATION"
                     *      "::$BITMAP"
                     *      ":$I30:$BITMAP"
                     *      "::$ATTRIBUTE_LIST"
                     *      "::$REPARSE_POINT"
                     * Other Windows file systems do not recognize any complex name suffixes.
                     * =>
                     * In NTFS and ReFS, these cases are expecting STATUS_SUCCESS, while in other file system that does not
                     * support the complex name suffixes will return STATUS_OBJECT_NAME_INVALID.
                     * 
                     * Test Cases:
                     *     ChangeNotificationTestCaseS0
                     *     ChangeNotificationTestCaseS2
                     */
                    site.Log.Add(LogEntryKind.Checkpoint, @"Section 2.1.5.1
                                                            Phase 6 -- Location of file:
                                                            If ComplexNameSuffix is non-empty and ComplexNameSuffix is not equal to one of the complex name suffixes
                                                            recognized by the object store<41> (using case-insensitive string comparisons), the operation MUST be 
                                                            failed with STATUS_OBJECT_NAME_INVALID.");
                    site.Assert.AreEqual<MessageStatus>(MessageStatus.OBJECT_NAME_INVALID, returnedStatus, "return of CreateFile");

                    // Remove the complex name suffixes and try to create the file again.
                    randomFile = randomFile.Remove(randomFile.IndexOf(":"));
                    returnedStatus = transAdapter.CreateFile(
                        randomFile,
                        (uint)desiredFileAttribute,
                        (uint)desiredAccess,
                        (uint)shareAccess,
                        (uint)createOption,
                        (uint)createDisposition,
                        out createAction);
                    return returnedStatus;
                }
            }

            return returnedStatus;
        }

        /// <summary>
        /// Basic CreateFile method
        /// </summary>
        /// <param name="fileName">The file name</param>
        /// <param name="desiredFileAttribute">Desired File Attribute</param>
        /// <param name="createOption">Specifies the options to be applied when creating or opening the file.</param>
        /// <param name="desiredAccess">Desired Access to the file.</param>
        /// <param name="shareAccess">Share Access to the file.</param>
        /// <param name="createDisposition">The desired disposition for the open.</param>
        /// <returns>An NTSTATUS code that specifies the result.</returns>
        public MessageStatus CreateFile(
            string fileName,
            FileAttribute desiredFileAttribute,
            CreateOptions createOption,
            FileAccess desiredAccess,
            ShareAccess shareAccess,
            CreateDisposition createDisposition)
        {
            uint createAction = 0;
            return transAdapter.CreateFile(
                fileName,
                (uint)desiredFileAttribute,
                (uint)desiredAccess,
                (uint)shareAccess,
                (uint)createOption,
                (uint)createDisposition,
                out createAction);
        }

        /// <summary>
        /// Basic CreateFile method
        /// </summary>
        /// <param name="fileName">The file name</param>
        /// <param name="desiredFileAttribute">Desired File Attribute</param>
        /// <param name="createOption">Specifies the options to be applied when creating or opening the file.</param>
        /// <param name="desiredAccess">Desired Access to the file.</param>
        /// <param name="shareAccess">Share Access to the file.</param>
        /// <param name="createDisposition">The desired disposition for the open.</param>
        /// <param name="fileId">The fileId for the open.</param>
        /// <param name="treeId">The treeId for the open.</param>
        /// <param name="sessionId">The sessionId for the open.</param>
        /// <returns>An NTSTATUS code that specifies the result.</returns>
        public MessageStatus CreateFile(
            string fileName,
            FileAttribute desiredFileAttribute,
            CreateOptions createOption,
            FileAccess desiredAccess,
            ShareAccess shareAccess,
            CreateDisposition createDisposition,
            out Smb2.FILEID fileId,
            out uint treeId,
            out ulong sessionId
             )
        {
            uint createAction = 0;

            return transAdapter.CreateFile(
                fileName,
                (uint)desiredFileAttribute,
                (uint)desiredAccess,
                (uint)shareAccess,
                (uint)createOption,
                (uint)createDisposition,
                out createAction,
                out fileId,
                out treeId,
                out sessionId
                );
        }

        /// <summary>
        /// Create or open a DataFile or DirectoryFile.
        /// </summary>
        /// <param name="fileType">An Open of a DataFile or DirectoryFile.</param>
        /// <returns></returns>
        public MessageStatus CreateFile(FileType fileType)
        {
            return CreateFile(fileType, false);
        }

        /// <summary>
        /// Create or open a DataFile or DirectoryFile.
        /// </summary>
        /// <param name="fileType">An Open of a DataFile or DirectoryFile.</param>
        /// <param name="openStream">To decide if set to DataStream and DirectoryStream according to fileType. Set to FALSE for using StreamType.NULL.</param>
        /// <returns></returns>
        public MessageStatus CreateFile(FileType fileType, bool openStream)
        {
            MessageStatus returnedStatus;
            switch (fileType)
            {
                case FileType.DataFile:
                    returnedStatus = CreateFile(
                        FileAttribute.NORMAL,
                        CreateOptions.NON_DIRECTORY_FILE,
                        openStream ? StreamTypeNameToOpen.DATA : StreamTypeNameToOpen.NULL,
                        FileAccess.GENERIC_ALL,
                        ShareAccess.FILE_SHARE_READ | ShareAccess.FILE_SHARE_WRITE,
                        CreateDisposition.OPEN_IF,
                        StreamFoundType.StreamIsFound,
                        SymbolicLinkType.IsNotSymbolicLink,
                        FileType.DataFile,
                        FileNameStatus.PathNameValid);

                    break;
                case FileType.DirectoryFile:
                    returnedStatus = CreateFile(
                        FileAttribute.NORMAL,
                        CreateOptions.DIRECTORY_FILE,
                        openStream ? StreamTypeNameToOpen.INDEX_ALLOCATION : StreamTypeNameToOpen.NULL,
                        FileAccess.GENERIC_ALL,
                        ShareAccess.FILE_SHARE_READ | ShareAccess.FILE_SHARE_WRITE,
                        CreateDisposition.OPEN_IF,
                        StreamFoundType.StreamIsFound,
                        SymbolicLinkType.IsNotSymbolicLink,
                        FileType.DirectoryFile,
                        FileNameStatus.PathNameValid);
                    break;
                default:
                    throw new Exception("The given openFileType is not supported");
            }
            return returnedStatus;
        }

        /// </summary>
        /// <param name="searchPattern">A Unicode string containing the file name pattern to match. </param>
        /// <param name="fileInfoClass">The FileInfoClass to query. </param>
        /// <param name="returnSingleEntry">A boolean indicating whether the return single entry for query.</param>
        /// <param name="restartScan">A boolean indicating whether the enumeration should be restarted.</param>
        /// <param name="isOutPutBufferNotEnough">True: if OutputBufferSize is less than the size needed to return a single entry</param>
        /// of section 3.1.5.5.4</param>
        /// <returns>An NTSTATUS code that specifies the result</returns>
        public MessageStatus QueryDirectoryInfo(
            string searchPattern,
            FileInfoClass fileInfoClass,
            bool returnSingleEntry,
            bool restartScan,
            bool isOutPutBufferNotEnough
            )
        {

            byte[] outBuffer = null;

            uint fileIndex = 0;
            uint maxOutputSize = (uint)(isOutPutBufferNotEnough ? 1 : this.transBufferSize);

            MessageStatus returnedStatus = this.transAdapter.QueryDirectory(
                (byte)fileInfoClass,
                maxOutputSize,
                restartScan,
                returnSingleEntry,
                fileIndex,
                searchPattern,
                out outBuffer
                );

            return returnedStatus;
        }
        /// <summary>
        /// Query directory information and return query status
        /// </summary>
        /// <param name="fileId">The fileID of the directory</param>
        /// <param name="treeId">The fileID of the directory</param>
        /// <param name="sessionId">The fileID of the directory</param>s  
        /// <param name="searchPattern">A Unicode string containing the file name pattern to match. </param>
        /// <param name="fileInfoClass">The FileInfoClass to query. </param>
        /// <param name="returnSingleEntry">A boolean indicating whether the return single entry for query.</param>
        /// <param name="restartScan">A boolean indicating whether the enumeration should be restarted.</param>
        /// <param name="isOutPutBufferNotEnough">True: if OutputBufferSize is less than the size needed to return a single entry</param>     
        /// of section 3.1.5.5.4</param>
        /// <returns>An NTSTATUS code that specifies the result</returns>
        public MessageStatus QueryDirectoryInfo(
            Smb2.FILEID fileId,
            uint treeId,
            ulong sessionId,
            string searchPattern,
            FileInfoClass fileInfoClass,
            bool returnSingleEntry,
            bool restartScan,
            bool isOutPutBufferNotEnough
           )
        {

            byte[] outBuffer = null;

            uint fileIndex = 0;
            uint maxOutputSize = (uint)(isOutPutBufferNotEnough ? 1 : this.transBufferSize);

            MessageStatus returnedStatus = this.transAdapter.QueryDirectory(
                fileId,
                treeId,
                sessionId,
                (byte)fileInfoClass,
                maxOutputSize,
                restartScan,
                returnSingleEntry,
                fileIndex,
                searchPattern,
                out outBuffer
                );

            return returnedStatus;
        }

        /// </summary>
        /// Query directory with outputBuffer returned.
        /// <param name="searchPattern">A Unicode string containing the file name pattern to match. </param>
        /// <param name="fileInfoClass">The FileInfoClass to query. </param>
        /// <param name="returnSingleEntry">A boolean indicating whether the return single entry for query.</param>
        /// <param name="restartScan">A boolean indicating whether the enumeration should be restarted.</param>
        /// <param name="outputBuffer"> The buffer containing the directory enumeration being returned in the response</param>
        /// of section 3.1.5.5.4</param>
        /// <returns>An NTSTATUS code that specifies the result</returns>
        public MessageStatus QueryDirectory(
            Smb2.FILEID fileId,
            uint treeId,
            ulong sessionId,
            string searchPattern,
            FileInfoClass fileInfoClass,
            bool returnSingleEntry,
            bool restartScan,
            out byte[] outputBuffer
            )
        {
            uint fileIndex = 0;

            MessageStatus returnedStatus = this.transAdapter.QueryDirectory(
                fileId,
                treeId,
                sessionId,
                (byte)fileInfoClass,
                this.transBufferSize,
                restartScan,
                returnSingleEntry,
                fileIndex,
                searchPattern,
                out outputBuffer
                );

            return returnedStatus;
        }

        #endregion

        #region 3.1.5.1.2   Open of an Existing File

        /// <summary>
        /// Implement OpenExistingFile method
        /// </summary>
        /// <param name="shareAccess">A bitmask indicating sharing access for the open, as specified in [MS-SMB2] section 2.2.13.</param>
        /// <param name="desiredAccess">A bitmask indicating desired access for the open, as specified in [MS-SMB2] section 2.2.13.1.</param>
        /// <param name="streamFoundType">Indicate if the stream is found or not.</param>
        /// <param name="symbolicLinkType">Indicate if it is symbolic link or not.</param>
        /// <param name="openFileType">Open.File.FileType</param>
        /// <param name="fileNameStatus">File name status</param>
        /// <param name="existingOpenModeCreateOption">The Existing File's Create Option.</param>
        /// <param name="existOpenShareModeShareAccess">A bitmask indicating sharing access for the open, as specified in [MS-SMB2] section 2.2.13.</param>
        /// <param name="existOpenDesiredAccess">A bitmask indicating desired access for the open, as specified in [MS-SMB2] section 2.2.13.1.</param>
        /// <param name="createOption">create options</param>
        /// <param name="createDisposition">The desired disposition for the open, as specified in [MS-SMB2] section 2.2.13.</param>
        /// <param name="streamTypeNameToOpen">the name of stream type to open</param>
        /// <param name="fileAttribute">A bitmask of file attributes for the open, as specified in [MS-SMB2] section 2.2.13.</param>
        /// <param name="desiredFileAttribute">A bitmask of desired file attributes for the open, as specified in [MS-SMB2] section 2.2.13.</param>
        /// <returns>An NTSTATUS code that specifies the result.</returns>
        public MessageStatus OpenExistingFile(
            ShareAccess shareAccess,
            FileAccess desiredAccess,
            StreamFoundType streamFoundType,
            SymbolicLinkType symbolicLinkType,
            FileType openFileType,
            FileNameStatus fileNameStatus,
            CreateOptions existingOpenModeCreateOption,
            ShareAccess existOpenShareModeShareAccess,
            FileAccess existOpenDesiredAccess,
            CreateOptions createOption,
            CreateDisposition createDisposition,
            StreamTypeNameToOpen streamTypeNameToOpen,
            FileAttribute fileAttribute,
            FileAttribute desiredFileAttribute)
        {

            bool streamFound = (streamFoundType == StreamFoundType.StreamIsFound);
            bool isSymbolicLink = (symbolicLinkType == SymbolicLinkType.IsSymbolicLink);

            uint createAction = 0;
            string randomFile = this.ComposeRandomFileName();
            MessageStatus returnedStatus = MessageStatus.SUCCESS;

            switch (fileNameStatus)
            {
                case FileNameStatus.BackslashName:
                    randomFile = randomFile + @"/";
                    break;

                case FileNameStatus.FileNameNull:
                    randomFile = string.Empty;
                    break;

                case FileNameStatus.NotPathNameValid:
                    randomFile = randomFile + @"<+>";
                    break;

                case FileNameStatus.PathNameTraiblack:
                    randomFile = randomFile + @"\";
                    break;

                case FileNameStatus.StreamTypeNameIsINDEX_ALLOCATION:
                    randomFile = randomFile + @":$110:$INDEX_ALLOCATION";
                    break;

                default:
                    break;
            }

            switch (streamTypeNameToOpen)
            {
                case StreamTypeNameToOpen.DATA:
                    randomFile = randomFile + "::$DATA";
                    break;

                case StreamTypeNameToOpen.INDEX_ALLOCATION:
                    randomFile = randomFile + "::$INDEX_ALLOCATION";
                    break;

                case StreamTypeNameToOpen.Other:
                    randomFile = randomFile + "::$OTHER";
                    break;

                default:
                    break;
            }

            if (isSymbolicLink)
            {
                randomFile = testConfig.GetProperty("SymbolicLinkFile");
                returnedStatus = transAdapter.CreateFile(
                    randomFile,
                    (uint)desiredFileAttribute,
                    (uint)desiredAccess,
                    (uint)shareAccess,
                    (uint)createOption,
                    (uint)createDisposition,
                    out createAction);

                /*
                 * Work around for test cases only designed for NTFS and ReFS:
                 * Make assertion in the adapter, then convert the return code according to the test case.
                 */
                if (this.fileSystem != Adapter.FileSystem.NTFS && this.fileSystem != Adapter.FileSystem.REFS)
                {
                    if ((createDisposition & CreateDisposition.OPEN_IF) == CreateDisposition.OPEN_IF)
                    {
                        /*
                         * To cover the below requirements in a file system other than NTFS and ReFS that does not support Symbolic Link:
                         * Section 2.1.5.1
                         * Phase 6 -- Location of file:
                         * If Link.File.IsSymbolicLink is TRUE, the operation MUST be failed with Status set to STATUS_STOPPED_ON_SYMLINK
                         * and ReparsePointData set to Link.File.ReparsePointData.                         * 
                         * Section 2.1.5.1.2
                         * Else if FileTypeToOpen is DataFile:
                         * If Stream was found:
                         * If CreateDisposition is FILE_OPEN or FILE_OPEN_IF:
                         * ...
                         * Nothing is violated, return STATUS_SUCCESS.
                         * =>
                         * In NTFS and ReFS, these cases are expecting STATUS_STOPPED_ON_SYMLINK, while in other file system that does
                         * not support symbolic link will consider this action as creating a file that is already existed, and if 
                         * the CreateDisposition==FILE_OPEN_IF, the return will be STATUS_SUCCESS instead.
                         * 
                         * Test Cases:
                         *     OpenFileTestCaseS40
                         */
                        site.Log.Add(LogEntryKind.Checkpoint, @"Section 2.1.5.1.2
                                                                Else if FileTypeToOpen is DataFile:
                                                                If Stream was found:
                                                                If CreateDisposition is FILE_OPEN or FILE_OPEN_IF:
                                                                ...
                                                                Nothing is violated, return STATUS_SUCCESS.");
                        site.Assert.AreEqual<MessageStatus>(MessageStatus.SUCCESS, returnedStatus, "return of CreateFile");

                        // Make a fake return
                        return MessageStatus.STOPPED_ON_SYMLINK;
                    }
                }

                return returnedStatus;
            }

            if (streamFound)
            {
                returnedStatus = transAdapter.CreateFile(
                    randomFile,
                    (uint)fileAttribute,
                    (uint)existOpenDesiredAccess,
                    (uint)existOpenShareModeShareAccess,
                    (uint)existingOpenModeCreateOption,
                    (uint)CreateDisposition.OPEN_IF,
                    out createAction);

                if (returnedStatus == MessageStatus.SUCCESS)
                {
                    returnedStatus = this.transAdapter.CloseFile();
                    if (returnedStatus != MessageStatus.SUCCESS)
                    {
                        throw new InvalidOperationException("Close file fails.");
                    }
                }
            }

            returnedStatus = transAdapter.CreateFile(
                randomFile,
                (uint)desiredFileAttribute,
                (uint)desiredAccess,
                (uint)shareAccess,
                (uint)createOption,
                (uint)createDisposition,
                out createAction);

            if (this.transport == Transport.SMB2 || this.transport == Transport.SMB3)
            {
                returnedStatus = SMB2_TDIWorkaround.WorkaroundOpenExistingFile(shareAccess, desiredAccess, streamFound, isSymbolicLink, openFileType, fileNameStatus, existingOpenModeCreateOption, existOpenShareModeShareAccess, existOpenDesiredAccess, createOption, createDisposition, streamTypeNameToOpen, fileAttribute, desiredFileAttribute, returnedStatus, site);
            }
            else
            {
                returnedStatus = SMB_TDIWorkaround.WorkaroundOpenExistingFile(shareAccess, desiredAccess, streamFound, isSymbolicLink, openFileType, fileNameStatus, existingOpenModeCreateOption, existOpenShareModeShareAccess, existOpenDesiredAccess, createOption, createDisposition, streamTypeNameToOpen, fileAttribute, desiredFileAttribute, returnedStatus, site);
            }

            /*
             * Work around for test cases only designed for NTFS and ReFS:
             * Make assertion in the adapter, then convert the return code according to the test case.
             */
            if (this.fileSystem != Adapter.FileSystem.NTFS && this.fileSystem != Adapter.FileSystem.REFS)
            {
                if (randomFile.Contains("$STANDARD_INFORMATION")
                        || randomFile.Contains("$ATTRIBUTE_LIST")
                        || randomFile.Contains("$FILE_NAME")
                        || randomFile.Contains("$OBJECT_ID")
                        || randomFile.Contains("$SECURITY_DESCRIPTOR")
                        || randomFile.Contains("$VOLUME_NAME")
                        || randomFile.Contains("$VOLUME_INFORMATION")
                        || randomFile.Contains("$DATA")
                        || randomFile.Contains("$INDEX_ROOT")
                        || randomFile.Contains("$INDEX_ALLOCATION")
                        || randomFile.Contains("$BITMAP")
                        || randomFile.Contains("$REPARSE_POINT")
                        || randomFile.Contains("$EA_INFORMATION")
                        || randomFile.Contains("$EA")
                        || randomFile.Contains("$LOGGED_UTILITY_STREAM"))
                {
                    if (fileNameStatus == FileNameStatus.StreamTypeNameIsINDEX_ALLOCATION)
                    {
                        /*
                         * To cover the below requirements in a file system other than NTFS and ReFS that does not support stream type names:
                         * Section 2.1.5.1
                         * Phase 7 -- Type of file to open:
                         * If StreamTypeNameToOpen is "$INDEX_ALLOCATION" and StreamNameToOpen has a value other than an empty stream or "$I30", 
                         * the operation MUST be failed with STATUS_INVALID_PARAMETER.
                         * 
                         * Section 5
                         * <42> Section 2.1.5.1:  NTFS and ReFS recognize the following stream type names:
                         *      "$STANDARD_INFORMATION"
                         *      "$ATTRIBUTE_LIST"
                         *      "$FILE_NAME"
                         *      "$OBJECT_ID"
                         *      "$SECURITY_DESCRIPTOR"
                         *      "$VOLUME_NAME"
                         *      "$VOLUME_INFORMATION"
                         *      "$DATA"
                         *      "$INDEX_ROOT"
                         *      "$INDEX_ALLOCATION"
                         *      "$BITMAP"
                         *      "$REPARSE_POINT"
                         *      "$EA_INFORMATION"
                         *      "$EA"
                         *      "$LOGGED_UTILITY_STREAM"
                         * Other Windows file systems do not recognize any stream type names.
                         * =>
                         * In NTFS and ReFS, these cases are expecting STATUS_OBJECT_NAME_NOT_FOUND, while in other file system that does not
                         * support the the stream type names will return STATUS_OBJECT_NAME_INVALID.
                         * 
                         * Test Cases:
                         *     OpenFileTestCaseS34
                         */
                        site.Log.Add(LogEntryKind.Checkpoint, @"Section 2.1.5.1
                                                                Phase 6 -- Location of file:
                                                                If StreamTypeNameToOpen is non-empty and StreamTypeNameToOpen is not equal to one of the stream type names
                                                                recognized by the object store<42> (using case-insensitive string comparisons), the operation MUST be failed
                                                                with STATUS_OBJECT_NAME_INVALID.");
                        site.Assert.AreEqual<MessageStatus>(MessageStatus.OBJECT_NAME_INVALID, returnedStatus, "return of CreateFile");

                        // Make a fake return
                        return MessageStatus.INVALID_PARAMETER;
                    }
                }
            }

            return returnedStatus;
        }
        #endregion

        #region OpenFileinitial

        /// <summary>
        /// Implement OpenFileinitial Method
        /// </summary>
        /// <param name="desiredAccess">A bitmask indicating desired access for the open, as specified in [MS-SMB2] section 2.2.13.1.</param>
        /// <param name="shareAccess">A bitmask indicating sharing access for the open, as specified in [MS-SMB2] section 2.2.13.</param>
        /// <param name="createOption">create options</param>
        /// <param name="createDisposition">The desired disposition for the open, as specified in [MS-SMB2] section 2.2.13.</param>
        /// <param name="fileAttribute">A bitmask of file attributes for the open, as specified in [MS-SMB2] section 2.2.13.</param>
        /// <param name="isVolumeReadOnly">True: if Open.File.Volume.IsReadOnly is TRUE</param>
        /// <param name="isCreateNewFile">True: if create a new file</param>
        /// <param name="isCaseInsensitive">True: if case is insensitive</param>
        /// <param name="isLinkIsDeleted">True: if Links is deleted</param>
        /// <param name="isSymbolicLink">True: if Link.File.IsSymbolicLink is TRUE</param>
        /// <param name="streamTypeNameToOpen">the name of stream type to open</param>
        /// <param name="openFileType">Open.File.FileType</param>
        /// <param name="fileNameStatus">File name status</param>
        /// <returns>An NTSTATUS code that specifies the result.</returns>
        public MessageStatus OpenFileinitial(
            FileAccess desiredAccess,
            ShareAccess shareAccess,
            CreateOptions createOption,
            CreateDisposition createDisposition,
            FileAttribute fileAttribute,
            bool isVolumeReadOnly,
            bool isCreateNewFile,
            bool isCaseInsensitive,
            bool isLinkIsDeleted,
            bool isSymbolicLink,
            StreamTypeNameToOpen streamTypeNameToOPen,
            FileType openFileType,
            FileNameStatus fileNameStatus
            )
        {
            throw (new NotImplementedException());
        }

        #endregion

        #endregion

        #region 3.1.5.2   Server Requests a Read
        /// <summary>
        /// Implement ReadFile method
        /// </summary>
        /// <param name="byteOffset">The absolute byte offset in the stream from which to read data.</param>
        /// <param name="byteCount">The desired number of bytes to read.</param>
        /// <param name="byteRead">The number of bytes that were read.</param>
        /// <returns>An NTSTATUS code that specifies the result</returns>
        public MessageStatus ReadFile(
            long byteOffset,
            int byteCount,
            out long byteRead)
        {
            byte[] outBuffer;
            return ReadFile(byteOffset, byteCount, out byteRead, out outBuffer);
        }

        /// <summary>
        /// Implement ReadFile method
        /// </summary>
        /// <param name="byteOffset">The absolute byte offset in the stream from which to read data.</param>
        /// <param name="byteCount">The desired number of bytes to read.</param>
        /// <param name="byteRead">The number of bytes that were read.</param>
        /// <param name="buffer">The buffer which contains file content read out.</param>
        /// <returns>An NTSTATUS code that specifies the result</returns>
        public MessageStatus ReadFile(
            long byteOffset,
            int byteCount,
            out long byteRead,
            out byte[] buffer
            )
        {
            MessageStatus returnedStatus = MessageStatus.INVALID_PARAMETER;

            try
            {
                returnedStatus = this.transAdapter.Read(
                    (ulong)byteOffset,
                    (uint)byteCount,
                    true,
                    out buffer);

                if (returnedStatus == MessageStatus.SUCCESS)
                {
                    bool isReturned = (buffer != null);
                    this.VerifyServerRequestsRead(isReturned);
                }
            }
            catch (Exception ex)
            {
                buffer = new byte[0];
                site.Log.Add(LogEntryKind.Debug, "Read file get exception: " + ex.Message);
            }

            //When outBuffer is null, byteRead is 0
            byteRead = ((buffer == null) ? 0 : buffer.Length);

            if (this.transport == Transport.SMB2 || this.transport == Transport.SMB3)
            {
                returnedStatus = SMB2_TDIWorkaround.WorkaroundReadFile(gOpenMode, byteCount, returnedStatus, site);
                byteRead = SMB2_TDIWorkaround.WorkaroundReadFileForByteRead(gOpenMode, byteCount, byteRead, site);
            }
            else
            {
                returnedStatus = SMB_TDIWorkaround.WorkAroundReadFile(byteOffset, byteCount, returnedStatus, site);
            }

            return returnedStatus;
        }

        #endregion

        #region 3.1.5.3   Server Requests a Write

        /// <summary>
        ///  Implement WriteFile method
        /// </summary>
        /// <param name="byteOffset">The absolute byte offset in the stream where data should be written. </param>
        /// <param name="byteCount">The number of bytes in InputBuffer to write.</param>
        /// <param name="bytesWritten">The number of bytes that were written</param>
        /// <returns>An NTSTATUS code that specifies the result.</returns>
        public MessageStatus WriteFile(
            long byteOffset,
            long byteCount,
            out long bytesWritten)
        {
            byte[] bufferData = new byte[0];
            if (byteCount > 0)
            {
                bufferData = new byte[byteCount];
            }
            RandomNumberGenerator ran = RandomNumberGenerator.Create();
            ran.GetBytes(bufferData);
            ulong outLength = 0;
            MessageStatus returnedStatus = this.transAdapter.Write(
                bufferData,
                (ulong)byteOffset,
                false,
                false,
                out outLength);

            bytesWritten = (long)outLength;

            if (this.transport == Transport.SMB2 || this.transport == Transport.SMB3)
            {
                returnedStatus = SMB2_TDIWorkaround.WorkaroundWriteFile(byteOffset, this.isVolumeReadonly, byteCount, ref bytesWritten, returnedStatus, site);
            }
            else
            {
                returnedStatus = SMB_TDIWorkaround.WorkAroundWriteFile(gOpenMode, returnedStatus, site);
                if (returnedStatus == MessageStatus.SUCCESS)
                {
                    bytesWritten = SMB_TDIWorkaround.WorkAroundWriteFileOut(byteOffset, this.isVolumeReadonly, byteCount, bytesWritten, site);
                }
                else
                {
                    bytesWritten = 0;
                }
            }
            return returnedStatus;
        }

        public MessageStatus WriteFile(
            long byteOffset,
            long byteCount,
            bool isWriteThrough,
            bool isUnBuffered)
        {
            byte[] bufferData = new byte[0];
            if (byteCount > 0)
            {
                bufferData = new byte[byteCount];
            }
            RandomNumberGenerator ran = RandomNumberGenerator.Create();
            ran.GetBytes(bufferData);
            ulong outLength = 0;
            MessageStatus returnedStatus = this.transAdapter.Write(
                bufferData,
                (ulong)byteOffset,
                isWriteThrough,
                isUnBuffered,
                out outLength);
            return returnedStatus;
        }
        #endregion

        #region 3.1.5.4   Server Requests Closing an Open

        /// <summary>
        /// Implement CloseOpen method
        /// </summary>
        /// <returns>An NTSTATUS code that specifies the result</returns>
        public MessageStatus CloseOpen()
        {
            MessageStatus returnedStatus = this.transAdapter.CloseFile();
            return returnedStatus;
        }

        #endregion

        #region 3.1.5.5   Server Requests Querying a Directory

        #region 3.1.5.5.1   FileObjectIdInformation

        /// <summary>
        /// Implement QueryFileObjectIdInfo method
        /// </summary>
        /// <param name="fileNamePattern"> A Unicode string containing the file name pattern to match. </param>
        /// <param name="queryDirectoryScanType">Indicate whether the enumeration should be restarted.</param>
        /// <param name="queryDirectoryFileNameMatchType">The object store MUST search the volume for Files having File.ObjectId matching FileNamePattern.
        /// This parameter indicates if matches the file by FileNamePattern.</param>
        /// <param name="queryDirectoryOutputBufferType">Indicate if OutputBuffer is large enough to hold the first matching entry.</param>
        /// <returns>An NTSTATUS code that specifies the result</returns>
        public MessageStatus QueryFileObjectIdInfo(
            FileNamePattern fileNamePattern,
            QueryDirectoryScanType queryDirectoryScanType,
            QueryDirectoryFileNameMatchType queryDirectoryFileNameMatchType,
            QueryDirectoryOutputBufferType queryDirectoryOutputBufferType)
        {
            bool restartScan = (queryDirectoryScanType == QueryDirectoryScanType.RestartScan);
            bool isDirectoryNotRight = (queryDirectoryFileNameMatchType == QueryDirectoryFileNameMatchType.FileNamePatternNotMatched);
            bool isOutPutBufferNotEnough = (queryDirectoryOutputBufferType == QueryDirectoryOutputBufferType.OutputBufferIsNotEnough);
            bool returnSingleEntry = true;
            byte[] outBuffer = null;
            string randomFile = null;
            string randomFileName = this.ComposeRandomFileName();
            uint createAction = 0;
            uint fileIndex = 0;
            uint maxOutputSize = (uint)(isOutPutBufferNotEnough ? 1 : this.transBufferSize);

            switch (fileNamePattern)
            {
                case FileNamePattern.LengthIsNotAMultipleOf4:
                    //Extend file name and make its length not multiple of 4.
                    if ((randomFileName.Length % 4) == 0)
                    {
                        randomFile += "0";
                        randomFileName += "0";
                    }
                    break;

                case FileNamePattern.NotValidFilenameComponent:
                    //According to section 2.1.5 in [MS-FSCC], "+" is an invalid name.
                    randomFile = "+";
                    break;

                case FileNamePattern.Empty:
                    randomFile = string.Empty;
                    break;

                default:
                    randomFile = randomFileName;
                    break;
            }

            MessageStatus returnedStatus = this.transAdapter.CreateFile(
                randomFileName,
                (uint)FileAttribute.NORMAL,
                (uint)(FileAccess.GENERIC_READ | FileAccess.GENERIC_WRITE),
                (uint)(ShareAccess.FILE_SHARE_READ | ShareAccess.FILE_SHARE_WRITE),
                (uint)(CreateOptions.NON_DIRECTORY_FILE),
                (uint)(CreateDisposition.OPEN_IF),
                out createAction);

            if (isDirectoryNotRight)
            {
                randomFile = this.ComposeRandomFileName();
            }

            returnedStatus = this.transAdapter.QueryDirectory(
                (byte)FileInfoClass.FILE_OBJECTID_INFORMATION,
                maxOutputSize,
                restartScan,
                returnSingleEntry,
                fileIndex,
                randomFile,
                out outBuffer);

            returnedStatus = SMB2_TDIWorkaround.WorkaroundQueryFileObjectIdInfo(this.isObjectIDsSupported, fileNamePattern, restartScan, isDirectoryNotRight, isOutPutBufferNotEnough, returnedStatus, site);

            return returnedStatus;
        }

        /// <summary>
        /// Implement QueryFileObjectIdInfo method
        /// </summary>
        /// <param name="fileNamePattern"> A Unicode string containing the file name pattern to match. </param>
        /// <param name="queryDirectoryScanType">Indicate whether the enumeration should be restarted.</param>
        /// <param name="queryDirectoryFileNameMatchType">The object store MUST search the volume for Files having File.ObjectId matching FileNamePattern.
        /// This parameter indicates if matches the file by FileNamePattern.</param>
        /// <param name="queryDirectoryOutputBufferType">Indicate if OutputBuffer is large enough to hold the first matching entry.</param>
        /// <returns>An NTSTATUS code that specifies the result</returns>
        public MessageStatus QueryFileObjectIdInfo(
            string fileName,
            Smb2.FILEID fileId,
            uint treeId,
            ulong sessionId,
            QueryDirectoryScanType queryDirectoryScanType,
            QueryDirectoryFileNameMatchType queryDirectoryFileNameMatchType,
            QueryDirectoryOutputBufferType queryDirectoryOutputBufferType)
        {
            bool restartScan = (queryDirectoryScanType == QueryDirectoryScanType.RestartScan);
            bool isDirectoryNotRight = (queryDirectoryFileNameMatchType == QueryDirectoryFileNameMatchType.FileNamePatternNotMatched);
            bool isOutPutBufferNotEnough = (queryDirectoryOutputBufferType == QueryDirectoryOutputBufferType.OutputBufferIsNotEnough);
            bool returnSingleEntry = true;
            byte[] outBuffer = null;
            string randomFile = null;
            string randomFileName = this.ComposeRandomFileName();
            uint fileIndex = 0;
            uint maxOutputSize = (uint)(isOutPutBufferNotEnough ? 1 : this.transBufferSize);

            MessageStatus returnedStatus = this.transAdapter.QueryDirectory(
                fileId,
                treeId,
                sessionId,
                (byte) FileInfoClass.FILE_OBJECTID_INFORMATION,
                maxOutputSize,
                restartScan,
                returnSingleEntry,
                fileIndex,
                randomFile,
                out outBuffer);

            return returnedStatus;
        }
    #endregion

        #region 3.1.5.5.2   FileReparsePointInformation

        /// <summary>
        /// Implement QueryFileReparsePointInformation method
        /// </summary>
        /// <param name="fileNamePattern"> A Unicode string containing the file name pattern to match. </param>
        /// <param name="queryDirectoryScanType">Indicate whether the enumeration should be restarted.</param>
        /// <param name="queryDirectoryFileNameMatchType">The object store MUST search the volume for Files having File.ObjectId matching FileNamePattern.
        /// This parameter indicates if matches the file by FileNamePattern.</param>
        /// <param name="queryDirectoryOutputBufferType">Indicate if OutputBuffer is large enough to hold the first matching entry.</param>
        /// <returns>An NTSTATUS code that specifies the result</returns>
        public MessageStatus QueryFileReparsePointInformation(
            FileNamePattern fileNamePattern,
            QueryDirectoryScanType queryDirectoryScanType,
            QueryDirectoryFileNameMatchType queryDirectoryFileNameMatchType,
            QueryDirectoryOutputBufferType queryDirectoryOutputBufferType)
        {
            bool restartScan = (queryDirectoryScanType == QueryDirectoryScanType.RestartScan);
            bool isDirectoryNotRight = (queryDirectoryFileNameMatchType == QueryDirectoryFileNameMatchType.FileNamePatternNotMatched);
            bool isOutPutBufferNotEnough = (queryDirectoryOutputBufferType == QueryDirectoryOutputBufferType.OutputBufferIsNotEnough);

            bool returnSingleEntry = true;
            byte[] outBuffer = null;
            string randomFile = null;
            string randomFileName = this.ComposeRandomFileName();
            uint fileIndex = 0;
            uint createAction = 0;
            uint maxOutputSize = (uint)(isOutPutBufferNotEnough ? 1 : this.transBufferSize);

            switch (fileNamePattern)
            {
                case FileNamePattern.LengthIsNotAMultipleOf4:
                    if ((randomFileName.Length % 4) == 0)
                    {
                        //Extend randomFile name and make its length not multiple of 4.
                        randomFile += "0";
                        randomFileName += "0";
                    }
                    break;

                case FileNamePattern.NotValidFilenameComponent:
                    //According to section 2.1.5 in [MS-FSCC], "+" is an invalid name.
                    randomFile = "+";
                    break;

                case FileNamePattern.Empty:
                    randomFile = string.Empty;
                    break;

                default:
                    randomFile = randomFileName;
                    break;
            }

            MessageStatus returnedStatus = this.transAdapter.CreateFile(
                randomFileName,
                (uint)FileAttribute.NORMAL,
                (uint)(FileAccess.GENERIC_READ | FileAccess.GENERIC_WRITE),
                (uint)(ShareAccess.FILE_SHARE_READ | ShareAccess.FILE_SHARE_WRITE),
                (uint)(CreateOptions.NON_DIRECTORY_FILE),
                (uint)(CreateDisposition.OPEN_IF),
                out createAction);

            if (isDirectoryNotRight)
            {
                randomFile = this.ComposeRandomFileName();
            }

            returnedStatus = this.transAdapter.QueryDirectory(
                (byte)FileInfoClass.FILE_REPARSE_POINT_INFORMATION,
                maxOutputSize,
                restartScan,
                returnSingleEntry,
                fileIndex,
                randomFile,
                out outBuffer);

            returnedStatus = SMB2_TDIWorkaround.WorkaroundQueryFileReparsePointInfo(this.fileSystem, fileNamePattern, restartScan, isDirectoryNotRight, isOutPutBufferNotEnough, returnedStatus, site);

            return returnedStatus;
        }

        #endregion

        #region  3.1.5.5.3   Directory Information Queries

        /// <summary>
        /// Implement QueryDirectoryInfo method
        /// </summary>
        /// <param name="fileNamePattern">A Unicode string containing the file name pattern to match. </param>
        /// <param name="restartScan">A boolean indicating whether the enumeration should be restarted.</param>
        /// <param name="isNoRecordsReturned">True: if No Records Returned.</param>
        /// <param name="isOutBufferSizeLess">True: if OutputBufferSize is less than the size needed to return a single entry</param>
        /// <param name="outBufferSize">The state of OutBufferSize in subsection 
        /// of section 3.1.5.5.4</param>
        /// <returns>An NTSTATUS code that specifies the result</returns>
        public MessageStatus QueryDirectoryInfo(
            FileNamePattern fileNamePattern,
            bool restartScan,
            bool isNoRecordsReturned,
            bool isOutBufferSizeLess,
            OutBufferSmall outBufferSize)
        {
            bool returnSingleEntry = true;
            byte[] outBuffer = null;
            string randomFile = this.ComposeRandomFileName();
            uint createAction = 0;
            uint maxOutputSize = this.transBufferSize;
            uint fileIndex = 0;

            MessageStatus returnedStatus = this.transAdapter.CreateFile(
                testConfig.GetProperty("ExistingFolder"),
                (uint)FileAttribute.NORMAL,
                (uint)(FileAccess.GENERIC_READ | FileAccess.GENERIC_WRITE),
                (uint)(ShareAccess.FILE_SHARE_READ | ShareAccess.FILE_SHARE_WRITE),
                (uint)(CreateOptions.DIRECTORY_FILE),
                (uint)(CreateDisposition.OPEN_IF),
                out createAction);

            switch (fileNamePattern)
            {
                case FileNamePattern.LengthIsNotAMultipleOf4:
                    if ((randomFile.Length % 4) == 0)
                    {
                        //Extend randomFile name to make its length not multiple of 4.
                        randomFile = randomFile + "0";
                    }
                    break;

                case FileNamePattern.NotValidFilenameComponent:
                    //According to section 2.1.5 in [MS-FSCC], the characters " \ / [ ] : | < > + = ; , * ? are inlegal.
                    randomFile = "<+>";
                    break;

                case FileNamePattern.Empty:
                    randomFile = string.Empty;
                    break;

                default:
                    break;
            }

            FileInfoClass fileInfoClass = FileInfoClass.FILE_DIRECTORY_INFORMATION;

            if (isOutBufferSizeLess)
            {
                //Set maxOutputSize to 1 to ensure maxOutputSize less than the size of the following file information class
                maxOutputSize = 1;

                switch (outBufferSize)
                {
                    case OutBufferSmall.FileBothDirectoryInformation:
                        fileInfoClass = FileInfoClass.FILE_BOTH_DIR_INFORMATION;
                        break;

                    case OutBufferSmall.FileDirectoryInformation:
                        fileInfoClass = FileInfoClass.FILE_DIRECTORY_INFORMATION;
                        break;

                    case OutBufferSmall.FileFullDirectoryInformation:
                        fileInfoClass = FileInfoClass.FILE_FULL_DIR_INFORMATION;
                        break;

                    case OutBufferSmall.FileIdBothDirectoryInformation:
                        fileInfoClass = FileInfoClass.FILE_ID_BOTH_DIR_INFORMATION;
                        break;

                    case OutBufferSmall.FileIdFullDirectoryInformation:
                        fileInfoClass = FileInfoClass.FILE_ID_FULL_DIR_INFORMATION;
                        break;

                    case OutBufferSmall.FileNamesInformation:
                        fileInfoClass = FileInfoClass.FILE_NAME_INFORMATION;
                        break;

                    default:
                        break;
                }
            }

            returnedStatus = this.transAdapter.QueryDirectory(
                (byte)fileInfoClass,
                maxOutputSize,
                restartScan,
                returnSingleEntry,
                fileIndex,
                randomFile,
                out outBuffer);

            if (fileNamePattern == FileNamePattern.Empty
                && !restartScan && isNoRecordsReturned && !isOutBufferSizeLess
                && outBufferSize == OutBufferSmall.FileFullDirectoryInformation)
            {
                return MessageStatus.NO_SUCH_FILE;
            }

            if (fileNamePattern == FileNamePattern.NotEmpty_LengthIsNotAMultipleOf4
                && !restartScan && isNoRecordsReturned && isOutBufferSizeLess
                && outBufferSize == OutBufferSmall.None)
            {
                return MessageStatus.INFO_LENGTH_MISMATCH;
            }

            if (isOutBufferSizeLess)
            {
                switch (outBufferSize)
                {
                    case OutBufferSmall.FileBothDirectoryInformation:
                        {
                            return MessageStatus.INFO_LENGTH_MISMATCH;
                        }
                    case OutBufferSmall.FileDirectoryInformation:
                        {
                            return MessageStatus.INFO_LENGTH_MISMATCH;
                        }
                    case OutBufferSmall.FileFullDirectoryInformation:
                        {
                            return MessageStatus.INFO_LENGTH_MISMATCH;
                        }
                    case OutBufferSmall.FileIdBothDirectoryInformation:
                        {
                            return MessageStatus.INFO_LENGTH_MISMATCH;
                        }
                    case OutBufferSmall.FileIdFullDirectoryInformation:
                        {
                            return MessageStatus.INFO_LENGTH_MISMATCH;
                        }
                    case OutBufferSmall.FileNamesInformation:
                        {
                            return MessageStatus.INFO_LENGTH_MISMATCH;
                        }
                    default:
                        break;
                }
            }

            if (this.transport == Transport.SMB2 || this.transport == Transport.SMB3)
            {
                returnedStatus = SMB2_TDIWorkaround.WorkaroundQueryDirectoryInfo(fileNamePattern, isNoRecordsReturned, isOutBufferSizeLess, returnedStatus, site);
            }
            else
            {
                returnedStatus = SMB_TDIWorkaround.WorkAroundQueryDirectoryInfo(isNoRecordsReturned, isOutBufferSizeLess, returnedStatus, site);
            }

            return returnedStatus;
        }

        #endregion

        #endregion

        #region 3.1.5.6   Server Requests Flushing Cached Data

        /// <summary>
        /// Implement FlushCachedData method
        /// </summary>
        /// <returns>An NTSTATUS code that specifies the result.</returns>
        public MessageStatus FlushCachedData()
        {
            MessageStatus returnedStatus = this.transAdapter.FlushCachedData();

            if (this.transport == Transport.SMB)
            {
                returnedStatus = SMB_TDIWorkaround.WorkAroundFlushCachedData(this.isVolumeReadonly, returnedStatus, site);
            }
            return returnedStatus;
        }
        #endregion

        #region 3.1.5.7   Server Requests a Byte-Range Lock

        /// <summary>
        /// Implement ByteRangeLock method
        /// </summary>
        /// <param name="FileOffset">Indicating the start offset.</param>
        /// <param name="Length">Indicating length.</param>
        /// <param name="ExclusiveLock">A boolean indicating whether the byte range is to be locked exclusively (TRUE) or shared (FALSE).</param>
        /// <param name="failImmediately">A boolean indicating whether the lock request is to fail immediately.</param>
        /// <param name="isOpenNotEqual"></param>
        /// <returns>An NTSTATUS code that specifies the result</returns>
        public MessageStatus ByteRangeLock(
            long FileOffset,
            long Length,
            bool ExclusiveLock,
            bool failImmediately,
            bool isOpenNotEqual
            )
        {
            if (gLockOffset >= 0 &&
                gLockLength > 0 &&
                FileOffset > gLockOffset &&
                (FileOffset + Length) > (gLockOffset + gLockLength) && // Lock range conflict
                gLockIsExclusive &&
                !isOpenNotEqual)
            {
                gLockIsConflicted = true;
            }

            this.gLockOffset = FileOffset;
            this.gLockLength = Length;
            this.gLockIsExclusive = ExclusiveLock;
            long bytesWritten = 0;

            MessageStatus returnedStatus = MessageStatus.SUCCESS;

            //To make sure FileOffset + Length + 10 is valid, according to 3.1.5.7
            WriteFile(0, FileOffset + Length + 10, out bytesWritten);

            returnedStatus = transAdapter.LockByteRange(
                (ulong)FileOffset,
                (ulong)Length,
                ExclusiveLock,
                failImmediately
                );

            if (this.transport == Transport.SMB)
            {
                returnedStatus = SMB_TDIWorkaround.WorkaroundByteRangeLock(gStreamType, gLockIsConflicted, returnedStatus, site);
            }
            return returnedStatus;
        }
        #endregion

        #region 3.1.5.8   Server Requests an Unlock of a Byte-Range

        /// <summary>
        /// Implement ByteRangeUnlock method
        /// </summary>
        /// <returns>An NTSTATUS code that specifies the result</returns>
        public MessageStatus ByteRangeUnlock()
        {
            MessageStatus returnedStatus = transAdapter.UnlockByteRange(
                (ulong)this.gLockOffset,
                (ulong)this.gLockLength);

            if (this.transport == Transport.SMB)
            {
                returnedStatus = SMB_TDIWorkaround.WorkaroundByteRangeUnlock(gStreamType, gLockIsConflicted, returnedStatus, site);
            }

            return returnedStatus;
        }

        #endregion

        #region 3.1.5.9   Server Requests an FsControl Request

        #region 3.1.5.9.1   FSCTL_CREATE_OR_GET_OBJECT_ID

        /// <summary>
        /// Implement FsCtlCreateOrGetObjId method
        /// </summary>
        /// <param name="bufferSize">Indicate buffer size</param>
        /// <param name="isBytesReturnedSet">TRUE: if the return status is success.</param>
        /// <returns>An NTSTATUS code that specifies the result</returns>
        public MessageStatus FsCtlCreateOrGetObjId(
            BufferSize bufferSize,
            out bool isBytesReturnedSet)
        {
            bool isImplemented = false;
            byte[] outbuffer = new byte[0];
            byte[] inbuffer = null;
            UInt32 outBufferSize = this.transBufferSize;

            switch (bufferSize)
            {
                case BufferSize.BufferSizeSuccess:
                    outBufferSize = UInt32.Parse(testConfig.GetProperty("BufferSize"));
                    outbuffer = new byte[outBufferSize];
                    break;

                case BufferSize.LessThanFILE_OBJECTID_BUFFER:
                    outBufferSize = UInt32.Parse(testConfig.GetProperty("MinBufferSize"));
                    outbuffer = new byte[outBufferSize];
                    break;

                default:
                    outbuffer = new byte[outBufferSize];
                    break;
            }

            MessageStatus returnedStatus = transAdapter.IOControl(
                (uint)FsControlCommand.FSCTL_CREATE_OR_GET_OBJECT_ID,
                outBufferSize,
                inbuffer,
                out outbuffer);

            if (MessageStatus.SUCCESS == returnedStatus)
            {
                isBytesReturnedSet = true;
                isImplemented = true;
                this.VerifyFsctlCreateOrGetObjectId(isImplemented);
            }
            else
            {
                isBytesReturnedSet = false;
            }

            return returnedStatus;
        }
        #endregion

        #region 3.1.5.9.24   FSCTL_SET_COMPRESSION

        /// <summary>
        /// Implementation of FSCTL_SET_COMPRESSION
        /// </summary>
        /// <param name="inputBuffer">An array of bytes containing a USHORT value indicating the requested compression state of the stream.</param>
        /// <param name="inputBufferSize">The number of bytes in InputBuffer.</param>
        /// <returns>An NTSTATUS code that specifies the result.</returns>
        public MessageStatus FsCtlSetCompression(FSCTL_SET_COMPRESSION_Request inputBuffer, uint inputBufferSize)
        {
            byte[] outbuffer = new byte[0];

            FsccFsctlSetCompressionRequestPacket fsccPacket = new FsccFsctlSetCompressionRequestPacket();
            fsccPacket.Payload = inputBuffer;

            MessageStatus returnedStatus = this.transAdapter.IOControl(
                (uint)FsControlCommand.FSCTL_SET_COMPRESSION,
                inputBufferSize,
                fsccPacket.ToBytes(),
                out outbuffer);

            return returnedStatus;
        }

        #endregion

        #region 3.1.5.9.2   FSCTL_DELETE_OBJECT_ID

        /// <summary>
        /// Implement FsCtlDeleteObjId method
        /// </summary>
        /// <returns>An NTSTATUS code that specifies the result</returns>
        public MessageStatus FsCtlDeleteObjId()
        {
            bool isImplemented = false;
            byte[] inbuffer = new byte[0];
            byte[] outbuffer = new byte[this.transBufferSize];

            // The control code must be set to 0x900a0, according to FSCC 2.3
            MessageStatus returnedStatus = transAdapter.IOControl(
                0x900a0,
                this.transBufferSize,
                inbuffer,
                out outbuffer);

            if (returnedStatus == MessageStatus.SUCCESS)
            {
                isImplemented = true;
                this.VerifyFsctlDeleteObjectId(isImplemented);
            }

            return returnedStatus;
        }

        #endregion

        #region 3.1.5.9.3   FSCTL_DELETE_REPARSE_POINT

        /// <summary>
        /// Implement FsCtlDeleteReparsePoint method
        /// </summary>
        /// <param name="reparseTag">An identifier indicating the type of the reparse point to delete, as defined in [MS-FSCC] section 2.1.2.1.</param>
        /// <param name="reparseGuidEqualOpenGuid">True: If Open.File.ReparseGUID == ReparseGUID</param>
        /// <returns>An NTSTATUS code that specifies the result</returns>
        public MessageStatus FsCtlDeleteReparsePoint(
            ReparseTag reparseTag,
            bool reparseGuidEqualOpenGuid
            )
        {
            byte[] outbuffer = new byte[0];

            REPARSE_GUID_DATA_BUFFER fsccPayload = new REPARSE_GUID_DATA_BUFFER();
            fsccPayload.ReparseTag = (uint)reparseTag;
            fsccPayload.ReparseDataLength = ushort.Parse(testConfig.GetProperty("BufferSize"));
            fsccPayload.DataBuffer = new byte[fsccPayload.ReparseDataLength];
            fsccPayload.Reserved = 0;

            if (reparseGuidEqualOpenGuid)
                fsccPayload.ReparseGuid = Guid.NewGuid();

            FsccFsctlDeleteReparsePointRequestPacket fsccPacket = new FsccFsctlDeleteReparsePointRequestPacket();
            fsccPacket.ReparseGuidDataBuffer = fsccPayload;

            // The control code to be sent must be 0x900ac according to FSCC 2.3
            MessageStatus returnedStatus = transAdapter.IOControl(
                0x900ac,
                this.transBufferSize,
                fsccPacket.ToBytes(),
                out outbuffer);

            if (this.transport == Transport.SMB2 || this.transport == Transport.SMB3)
            {
                returnedStatus = SMB2_TDIWorkaround.WorkaroundFsCtlDeleteReparsePoint(fileSystem, reparseTag, reparseGuidEqualOpenGuid, returnedStatus, site);
            }
            else
            {
                returnedStatus = SMB_TDIWorkaround.WorkAroundFsCtlDeleteReparsePoint(fileSystem, reparseTag, reparseGuidEqualOpenGuid, returnedStatus, site);
            }

            return returnedStatus;
        }
        #endregion

        #region 3.1.5.9.4   FSCTL_FILE_LEVEL_TRIM

        #endregion

        #region 3.1.5.9.6   FSCTL_FIND_FILES_BY_SID
        /// <summary>
        /// Implement FsCtlFindFilesBySID method
        /// </summary>
        /// <param name="openFileVolQuoInfoEmpty">True: If Open.File.Volume.QuotaInformation is empty</param>
        /// <param name="bufferSize">Indicate buffer size</param>
        /// <param name="linkOwnerSidEqualSID">True: if linkOwnerSid euqal SID.</param>
        /// <param name="outPutBufLessLinkSize">True: if the outputbuf is less than linksize.</param>
        /// <param name="isOutputBufferOffset"></param>
        /// <param name="isBytesReturnedSet">True: if returned status is success.</param>
        /// <returns>An NTSTATUS code that specifies the result.</returns>
        public MessageStatus FsCtlFindFilesBySID(
            bool openFileVolQuoInfoEmpty,
            BufferSize bufferSize,
            bool linkOwnerSidEqualSID,
            bool outPutBufLessLinkSize,
            bool isOutputBufferOffset,
            out bool isBytesReturnedSet)
        {
            byte[] outbuffer = new byte[0];
            UInt32 outBufferSize = this.transBufferSize;
            byte[] inbuffer = null;

            switch (bufferSize)
            {
                case BufferSize.BufferSizeSuccess:
                    outbuffer = new byte[this.transBufferSize];
                    break;

                case BufferSize.LessThanFILE_NAME_INFORMATION:
                    outBufferSize = UInt32.Parse(testConfig.GetProperty("MinBufferSize"));
                    outbuffer = new byte[outBufferSize];
                    break;

                default:
                    outbuffer = new byte[this.transBufferSize];
                    break;
            }

            // The control code to be sent must be 0x9008f according to FSCC 2.3
            MessageStatus returnedStatus = transAdapter.IOControl(
                0x9008f,
                outBufferSize,
                inbuffer,
                out outbuffer);

            if (MessageStatus.SUCCESS == returnedStatus)
            {
                isBytesReturnedSet = true;
            }
            else
            {
                isBytesReturnedSet = false;

                if (fileSystem == Adapter.FileSystem.FAT32 &&
                    MessageStatus.INVALID_DEVICE_REQUEST == returnedStatus)
                {
                    return returnedStatus;
                }
            }

            //  If Open.File.Volume.QuotaInformation is empty
            if (openFileVolQuoInfoEmpty)
            {
                return MessageStatus.NO_QUOTAS_FOR_ACCOUNT;
            }

            if (!openFileVolQuoInfoEmpty
                && bufferSize == BufferSize.BufferSizeSuccess
                && linkOwnerSidEqualSID
                && outPutBufLessLinkSize
                && isOutputBufferOffset)
            {
                return MessageStatus.BUFFER_TOO_SMALL;
            }

            if (!openFileVolQuoInfoEmpty
                && bufferSize == BufferSize.BufferSizeSuccess
                && linkOwnerSidEqualSID
                && outPutBufLessLinkSize
                && !isOutputBufferOffset)
            {
                isBytesReturnedSet = true;
            }

            if (!openFileVolQuoInfoEmpty && bufferSize == BufferSize.BufferSizeSuccess)
            {
                return MessageStatus.SUCCESS;
            }

            //  If OutputBufferSize is less than the size of FILE_NAME_INFORMATION
            if (bufferSize == BufferSize.LessThanFILE_NAME_INFORMATION)
            {
                return MessageStatus.INVALID_USER_BUFFER;
            }

            //  For each Link in Open.File.DirectoryList, starting at Open.FindBySidRestartIndex
            //If Link.File.SecurityDescriptor.OwnerSid is equal to FindBySidData.SID
            if (linkOwnerSidEqualSID)
            {
                //If (OutputBufferLength ?OutputBufferOffset) is less than the size of 
                //(Link.Name + 2 bytes) aligned to 4 bytes:
                if (outPutBufLessLinkSize)
                {
                    if (!isOutputBufferOffset)
                    {
                        return MessageStatus.SUCCESS;
                    }
                    else
                    {
                        return MessageStatus.BUFFER_TOO_SMALL;
                    }
                }
            }

            return returnedStatus;
        }

        #endregion

        #region 3.1.5.9.7   FSCTL_GET_COMPRESSION

        /// <summary>
        /// Implementation of FSCTL_GET_COMPRESSION
        /// </summary>
        /// <param name="outputBufferSize">The maximum number of bytes to return in OutputBuffer.</param>
        /// <param name="bytesReturned">The number of bytes returned in OutputBuffer.</param>
        /// <param name="outputBuffer">An array of bytes that will return a USHORT value representing the compression state of the stream.</param>
        /// <returns>An NTSTATUS code that specifies the result.</returns>
        public MessageStatus FsCtlGetCompression(uint outputBufferSize, out long bytesReturned, out byte[] outputBuffer)
        {
            outputBuffer = null;
            FsccFsctlGetCompressionRequestPacket fsccPacket = new FsccFsctlGetCompressionRequestPacket();

            MessageStatus returnedStatus = this.transAdapter.IOControl(
                (uint)FsControlCommand.FSCTL_GET_COMPRESSION,
                outputBufferSize,
                fsccPacket.ToBytes(),
                out outputBuffer);

            bytesReturned = ((outputBuffer == null) ? 0 : outputBuffer.Length);

            return returnedStatus;
        }

        #endregion

        //SET_OBJECT_ID
        //SET_OBJECT_ID_EXTENDED
        //IS_PATHNAME_VALID
        //LMR_GET_LINK_TRACKING_INFORMATION
        //LMR_SET_LINK_TRACKING_INFORMATION
        //QUERY_FAT_BPB
        //QUERY_ON_DISK_VOLUME_INFO
        //QUERY_SPARING_INFO
        //RECALL_FILE
        //SET_SHORT_NAME_BEHAVIOR
        //SET_ZERO_ON_DEALLOCATION
        //WRITE_USN_CLOSE_RECORD
        //GET_NTFS_VOLUME_DATA
        //FSCTL_GET_COMPRESSION
        //FSCTL_FILESYSTEM_GET_STATISTICS
        //FSCTL_SET_SHORT_NAME_BEHAVIOR
        #region 3.1.5.9.5-19 Application Requests FsCtlForEasyRequest

        /// <summary>
        /// Implement FsCtlForEasyRequest method
        /// </summary>
        /// <param name="requestType">request type</param>
        /// <param name="bufferSize">Indicate buffer size</param>
        /// <param name="isBytesReturnedSet">True: if the returned status is success.</param>
        /// <param name="isOutputBufferSizeReturn">True: if Return the OutputBufferSize</param>
        /// <returns>An NTSTATUS code that specifies the result</returns>
        [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        public MessageStatus FsCtlForEasyRequest(
            FsControlRequestType requestType,
            BufferSize bufferSize,
            out bool isBytesReturnedSet,
            out bool isOutputBufferSizeReturn
            )
        {
            bool isImplemented = false;
            byte[] outbuffer = new byte[0];
            uint ctlCode = 0;
            bool fileVolUsnAct = false;
            UInt32 outBufferSize = UInt32.Parse(testConfig.GetProperty("BufferSize"));
            isOutputBufferSizeReturn = false;

            switch (requestType)
            {
                case FsControlRequestType.GET_NTFS_VOLUME_DATA:
                    // According to FSCC 2.3, the ctlCode must be set to 0x90064 
                    ctlCode = 0x90064;
                    break;

                case FsControlRequestType.FSCTL_GET_COMPRESSION:
                    // According to FSCC 2.3, the ctlCode must be set to 0x9003c 
                    ctlCode = 0x9003c;
                    break;

                case FsControlRequestType.FSCTL_FILESYSTEM_GET_STATISTICS:
                    // According to FSCC 2.3, the ctlCode must be set to 0x90060
                    ctlCode = 0x90060;
                    break;

                case FsControlRequestType.QUERY_FAT_BPB:
                    // According to FSCC 2.3, the ctlCode must be set to 0x90058
                    ctlCode = 0x90058;
                    break;

                case FsControlRequestType.QUERY_ON_DISK_VOLUME_INFO:
                    // According to FSCC 2.3, the ctlCode must be set to 0x9013c
                    ctlCode = 0x9013c;
                    break;

                case FsControlRequestType.QUERY_SPARING_INFO:
                    // According to FSCC 2.3, the ctlCode must be set to 0x90138
                    ctlCode = 0x90138;
                    break;

                case FsControlRequestType.RECALL_FILE:
                    // According to FSCC 2.3, the ctlCode must be set to 0x90117
                    ctlCode = 0x90117;
                    break;

                case FsControlRequestType.SET_OBJECT_ID:
                    // According to FSCC 2.3, the ctlCode must be set to 0x90098
                    ctlCode = 0x90098;
                    break;

                case FsControlRequestType.FSCTL_SET_SHORT_NAME_BEHAVIOR:
                    // According to FSCC 2.3, the ctlCode must be set to 0x901B4
                    ctlCode = 0x901B4;
                    break;

                case FsControlRequestType.SET_ZERO_ON_DEALLOCATION:
                    // According to FSCC 2.3, the ctlCode must be set to 0x90194
                    ctlCode = 0x90194;
                    break;

                case FsControlRequestType.WRITE_USN_CLOSE_RECORD:
                    // According to FSCC 2.3, the ctlCode must be set  0x900ef
                    ctlCode = 0x900ef;
                    break;

                default:
                    break;
            }

            // According to different bufferSize, set the outBufferSize.
            switch (bufferSize)
            {
                case BufferSize.LessThanNTFS_VOLUME_DATA_BUFFER:
                    if (requestType == FsControlRequestType.GET_NTFS_VOLUME_DATA)
                    {
                        outBufferSize = UInt32.Parse(testConfig.GetProperty("MinBufferSize"));
                    }
                    break;

                case BufferSize.LessThanTwoBytes:
                    if (requestType == FsControlRequestType.FSCTL_GET_COMPRESSION)
                    {
                        outBufferSize = UInt32.Parse(testConfig.GetProperty("MinBufferSize"));
                    }
                    break;

                case BufferSize.LessThanSizeOf_FILESYSTEM_STATISTICS:
                    {
                        if (requestType == FsControlRequestType.FSCTL_FILESYSTEM_GET_STATISTICS)
                        {
                            outBufferSize = UInt32.Parse(testConfig.GetProperty("MinBufferSize"));
                        }
                        break;
                    }
                case BufferSize.LessThanFILE_QUERY_ON_DISK_VOL_INFO_BUFFER:
                    if (requestType == FsControlRequestType.QUERY_ON_DISK_VOLUME_INFO)
                    {
                        outBufferSize = UInt32.Parse(testConfig.GetProperty("MinBufferSize"));
                    }
                    break;

                case BufferSize.LessThanFILE_QUERY_SPARING_BUFFER:
                    if (requestType == FsControlRequestType.QUERY_SPARING_INFO)
                    {
                        outBufferSize = UInt32.Parse(testConfig.GetProperty("MinBufferSize"));
                    }
                    break;

                case BufferSize.LessThanFILE_OBJECTID_BUFFER:
                    if (requestType == FsControlRequestType.SET_OBJECT_ID)
                    {
                        outBufferSize = UInt32.Parse(testConfig.GetProperty("MinBufferSize"));
                    }
                    break;

                case BufferSize.LessThan0x24:
                    if (requestType == FsControlRequestType.QUERY_FAT_BPB)
                    {
                        outBufferSize = UInt32.Parse(testConfig.GetProperty("MinBufferSize"));
                    }
                    break;

                case BufferSize.LessThanSizeofUsn:
                    if (requestType == FsControlRequestType.WRITE_USN_CLOSE_RECORD)
                    {
                        outBufferSize = UInt32.Parse(testConfig.GetProperty("MinBufferSize"));
                    }
                    break;

                case BufferSize.BufferSizeSuccess:
                    if (requestType == FsControlRequestType.FSCTL_FILESYSTEM_GET_STATISTICS)
                    {
                        outBufferSize = 10240;
                    }
                    else
                    {
                        outBufferSize = UInt32.Parse(testConfig.GetProperty("BufferSize"));
                    }
                    break;

                default:
                    break;
            }

            MessageStatus returnedStatus = transAdapter.IOControl(
                ctlCode,
                outBufferSize,
                null,
                out outbuffer);

            if (MessageStatus.SUCCESS == returnedStatus)
            {
                isBytesReturnedSet = true;
                isOutputBufferSizeReturn = true;
                switch (requestType)
                {
                    case FsControlRequestType.GET_NTFS_VOLUME_DATA:
                        isImplemented = true;
                        this.VerifyFsclGetNtfsVolumeData(isImplemented);
                        break;

                    case FsControlRequestType.FSCTL_GET_COMPRESSION:
                        isImplemented = true;
                        this.VerifyFsctlGetCompression(isImplemented);
                        break;

                    default:
                        break;
                }
            }
            else
            {
                isBytesReturnedSet = false;
            }

            if (this.transport == Transport.SMB2 || this.transport == Transport.SMB3)
            {
                returnedStatus = SMB2_TDIWorkaround.WorkaroundFsCtlForEasyRequest(this.fileSystem, requestType, bufferSize, this.isVolumeReadonly, fileVolUsnAct, ref isBytesReturnedSet, ref isOutputBufferSizeReturn, returnedStatus, site);
            }
            else
            {
                if (returnedStatus != MessageStatus.INVALID_DEVICE_REQUEST)
                {
                    returnedStatus = SMB_TDIWorkaround.WorkAroundFsCtlForEasyRequest(this.fileSystem, requestType, bufferSize, isVolumeReadonly, fileVolUsnAct, returnedStatus, site);
                    isBytesReturnedSet = SMB_TDIWorkaround.WorkAroundFsCtlForEasyRequestBool(this.fileSystem, requestType, bufferSize, isVolumeReadonly, fileVolUsnAct, isBytesReturnedSet, ref isOutputBufferSizeReturn, returnedStatus, site);
                }
            }

            return returnedStatus;
        }

        public MessageStatus FsCtlForEasyRequest(FsControlCommand ctlCode, out byte[] outputBuffer)
        {
            MessageStatus status = this.transAdapter.IOControl(
                (uint)ctlCode,
                transBufferSize,
                null,
                out outputBuffer);

            return status;
        }

        #endregion

        #region 3.1.5.9.11   FSCTL_GET_REPARSE_POINT

        /// <summary>
        /// Implement FsCtlGetReparsePoint method
        /// </summary>
        /// <param name="bufferSize">Indicate buffer size</param>
        /// <param name="openFileReparseTag">Open.File.ReparseTag </param>
        /// <param name="isBytesReturnedSet">True: if the returned status is success.</param>
        /// <returns>An NTSTATUS code that specifies the result</returns>
        public MessageStatus FsCtlGetReparsePoint(
            BufferSize bufferSize,
            ReparseTag openFileReparseTag,
            out bool isBytesReturnedSet
            )
        {
            byte[] outbuffer = new byte[0];
            UInt32 outBufferSize = UInt32.Parse(testConfig.GetProperty("BufferSize"));

            // According to different condition, set the buffer size value 
            switch (bufferSize)
            {
                case BufferSize.BufferSizeSuccess:
                    outBufferSize = this.transBufferSize;
                    break;

                case BufferSize.LessThanREPARSE_DATA_BUFFER:
                    outBufferSize = UInt32.Parse(testConfig.GetProperty("MinBufferSize"));
                    break;

                case BufferSize.LessThanREPARSE_GUID_DATA_BUFFER:
                    outBufferSize = UInt32.Parse(testConfig.GetProperty("MinBufferSize"));
                    break;

                default:
                    break;
            }

            FsccFsctlGetReparsePointRequestPacket fsccPacket = new FsccFsctlGetReparsePointRequestPacket();

            // According to FSCC 2.3, the control code is 0x900a8
            MessageStatus returnedStatus = transAdapter.IOControl(
                0x900a8,
                outBufferSize,
                fsccPacket.ToBytes(),
                out outbuffer);

            if (MessageStatus.SUCCESS == returnedStatus)
            {
                isBytesReturnedSet = true;
            }
            else
            {
                isBytesReturnedSet = false;

                if (MessageStatus.INVALID_DEVICE_REQUEST == returnedStatus)
                {
                    return returnedStatus;
                }
            }

            if (this.transport == Transport.SMB2 || this.transport == Transport.SMB3)
            {
                isBytesReturnedSet = SMB2_TDIWorkaround.WorkaroundFsCtlGetReparsePoint(bufferSize, openFileReparseTag, isBytesReturnedSet, ref returnedStatus, site);
            }
            else
            {
                isBytesReturnedSet = SMB_TDIWorkaround.WorkAroundFsCtlGetReparsePoint(bufferSize, openFileReparseTag, isBytesReturnedSet, site);
                returnedStatus = SMB_TDIWorkaround.WorkAroundFsCtlGetReparsePointStatus(bufferSize, openFileReparseTag, returnedStatus, site);
            }

            return returnedStatus;
        }

        /// <summary>
        /// Implementation of FSCTL_GET_REPARSE_POINT
        /// </summary>
        /// <param name="outputBufferSize">The maximum number of bytes to return in OutputBuffer.</param>
        /// <param name="bytesReturned">The number of bytes returned to the caller.</param>
        /// <param name="outputBuffer">An array of bytes containing a REPARSE_DATA_BUFFER or REPARSE_GUID_DATA_BUFFER structure.</param>
        /// <returns>An NTSTATUS code that specifies the result.</returns>
        public MessageStatus FsCtlGetReparsePoint(uint outputBufferSize, out long bytesReturned, out byte[] outputBuffer)
        {
            outputBuffer = null;
            FsccFsctlGetReparsePointRequestPacket fsccPacket = new FsccFsctlGetReparsePointRequestPacket();

            MessageStatus returnedStatus = this.transAdapter.IOControl(
                (uint)FsControlCommand.FSCTL_GET_REPARSE_POINT,
                outputBufferSize,
                fsccPacket.ToBytes(),
                out outputBuffer);

            bytesReturned = ((outputBuffer == null) ? 0 : outputBuffer.Length);

            return returnedStatus;
        }

        #endregion

        #region 3.1.5.9.8   FSCTL_GET_INTEGRITY_INFORMATION
        /// <summary>
        /// Implementation of FSCTL_GET_INTEGRITY_INFORMATION
        /// </summary>
        /// <param name="outputBufferSize">The maximum number of bytes to return in OutputBuffer.</param>
        /// <param name="bytesReturned">The number of bytes returned to the caller.</param>
        /// <param name="outputBuffer">An array of bytes that will return an FSCTL_GET_INTEGRITY_INFORMATION_BUFFER structure.</param>
        /// <returns>An NTSTATUS code that specifies the result.</returns>
        public MessageStatus FsCtlGetIntegrityInfo(uint outputBufferSize, out long bytesReturned, out byte[] outputBuffer)
        {
            outputBuffer = null;
            FsccFsctlGetIntegrityInformationRequestPacket fsccPacket = new FsccFsctlGetIntegrityInformationRequestPacket();

            MessageStatus returnedStatus = this.transAdapter.IOControl(
                (uint)FsControlCommand.FSCTL_GET_INTEGRITY_INFORMATION,
                outputBufferSize,
                fsccPacket.ToBytes(),
                out outputBuffer);

            bytesReturned = ((outputBuffer == null) ? 0 : outputBuffer.Length);

            return returnedStatus;
        }

        #endregion

        #region 3.1.5.9.27   FSCTL_SET_INTEGRITY_INFORMATION

        /// <summary>
        /// Implementation of FSCTL_SET_INTEGRITY_INFORMATION
        /// </summary>
        /// <param name="inputBuffer">An array of bytes containing an FSCTL_SET_INTEGRITY_INFORMATION_BUFFER structure indicating the requested integrity state of the directory or file.</param>
        /// <param name="inputBufferSize">The number of bytes in InputBuffer.</param>
        /// <returns>An NTSTATUS code that specifies the result.</returns>
        public MessageStatus FsCtlSetIntegrityInfo(FSCTL_SET_INTEGRITY_INFORMATION_BUFFER inputBuffer, uint inputBufferSize)
        {
            byte[] outbuffer = new byte[0];
            byte[] inputBufferBytes = new byte[0];

            FsccFsctlSetIntegrityInformationRequestPacket fsccPacket = new FsccFsctlSetIntegrityInformationRequestPacket();
            fsccPacket.Payload = inputBuffer;

            uint inputBufferLength = (uint)TypeMarshal.ToBytes<FSCTL_SET_INTEGRITY_INFORMATION_BUFFER>(inputBuffer).Length;
            if (inputBufferSize < inputBufferLength)
            {
                // For some negative test cases, they want to provide a smaller input buffer size than sizeof(FSCTL_SET_INTEGRITY_INFORMATION_BUFFER),
                // and expect server return STATUS_INVALID_PARAMETER.
                // So here we copy a smaller size of data to inputBufferBytes which will be sent to server through IOCtl request.
                byte[] fsccPacketBytes = fsccPacket.ToBytes();
                inputBufferBytes = new byte[inputBufferSize];
                Array.Copy(fsccPacket.ToBytes(), inputBufferBytes, fsccPacketBytes.Length - (inputBufferLength - inputBufferSize));
            }
            else
            {
                inputBufferBytes = fsccPacket.ToBytes();
            }

            MessageStatus returnedStatus = this.transAdapter.IOControl(
                (uint)FsControlCommand.FSCTL_SET_INTEGRITY_INFORMATION,
                inputBufferSize,
                inputBufferBytes,
                out outbuffer);

            return returnedStatus;
        }

        #endregion

        #region 3.1.5.9.16   FSCTL_OFFLOAD_READ
        /// <summary>
        /// Implementation of FSCTL_OFFLOAD_READ
        /// </summary>
        /// <param name="inputBuffer">An array of bytes containing a single FSCTL_OFFLOAD_READ_INPUT structure.</param>
        /// <param name="outputBufferSize">The number of bytes in OutputBuffer.</param>
        /// <param name="bytesReturned">The number of bytes written to OutputBuffer.</param>
        /// <param name="outputBuffer">An array of bytes that contains a single FSCTL_OFFLOAD_READ_OUTPUT structure, which contains the Token for the read data.</param>
        /// <returns>An NTSTATUS code that specifies the result.</returns>
        public MessageStatus FsCtlOffloadRead(FSCTL_OFFLOAD_READ_INPUT inputBuffer, uint outputBufferSize, out long bytesReturned, out byte[] outputBuffer)
        {
            outputBuffer = null;
            FsccFsctlOffloadReadRequestPacket fsccPacket = new FsccFsctlOffloadReadRequestPacket();
            fsccPacket.Payload = inputBuffer;

            MessageStatus returnedStatus = this.transAdapter.IOControl(
                (uint)FsControlCommand.FSCTL_OFFLOAD_READ,
                outputBufferSize,
                fsccPacket.ToBytes(),
                out outputBuffer);

            bytesReturned = ((outputBuffer == null) ? 0 : outputBuffer.Length);

            return returnedStatus;
        }

        #endregion

        #region 3.1.5.9.17   FSCTL_OFFLOAD_WRITE
        /// <summary>
        /// Implementation of FSCTL_OFFLOAD_WRITE
        /// </summary>
        /// <param name="inputBuffer">An array of bytes containing a single FSCTL_OFFLOAD_WRITE_INPUT structure.</param>
        /// <param name="outputBufferSize">The number of bytes in OutputBuffer.</param>
        /// <param name="bytesReturned">The number of bytes written to OutputBuffer.</param>
        /// <param name="outputBuffer">An array of bytes that contains a single FSCTL_OFFLOAD_WRITE_OUTPUT structure.</param>
        /// <returns>An NTSTATUS code that specifies the result.</returns>
        public MessageStatus FsCtlOffloadWrite(FSCTL_OFFLOAD_WRITE_INPUT inputBuffer, uint outputBufferSize, out long bytesReturned, out byte[] outputBuffer)
        {
            outputBuffer = null;
            FsccFsctlOffloadWriteRequestPacket fsccPacket = new FsccFsctlOffloadWriteRequestPacket();
            fsccPacket.Payload = inputBuffer;

            MessageStatus returnedStatus = this.transAdapter.IOControl(
                (uint)FsControlCommand.FSCTL_OFFLOAD_WRITE,
                outputBufferSize,
                fsccPacket.ToBytes(),
                out outputBuffer);

            bytesReturned = ((outputBuffer == null) ? 0 : outputBuffer.Length);

            return returnedStatus;
        }

        #endregion

        #region 3.1.5.9.12   FSCTL_GET_RETRIEVAL_POINTERS

        /// <summary>
        /// Implement FsCtlGetRetrivalPoints method
        /// </summary>
        /// <param name="bufferSize">Indicate buffer size</param>
        /// <param name="isStartingVcnNegative">True: If StartingVcnBuffer.StartingVcn is negative</param>
        /// <param name="isStartingVcnGreatThanAllocationSize">True: If StartingVcnBuffer.StartingVcn is 
        /// greater than or equal to Open.Stream.AllocationSize divided by Open.File.Volume.ClusterSize</param>
        /// <param name="isElementsNotAllCopied">True: If not all of the elements in Open.Stream.ExtentList
        /// were copied into OutputBuffer.Extents.</param>
        /// <param name="isBytesReturnedSet">True: if the returned status is success.</param>
        /// <returns>An NTSTATUS code that specifies the result</returns>
        public MessageStatus FsCtlGetRetrivalPoints(
            BufferSize bufferSize,
            bool isStartingVcnNegative,
            bool isStartingVcnGreatThanAllocationSize,
            bool isElementsNotAllCopied,
            out bool isBytesReturnedSet
            )
        {
            byte[] outbuffer = new byte[0];
            UInt32 outBufferSize = this.transBufferSize;
            FSCTL_GET_RETRIEVAL_POINTERS_Request fsccPayload = new FSCTL_GET_RETRIEVAL_POINTERS_Request();
            fsccPayload.StartingVcn = 0;

            if (isStartingVcnNegative)
            {
                //-1 is an invalid number for StartingVcn
                fsccPayload.StartingVcn = -1;
            }

            if (isStartingVcnGreatThanAllocationSize)
            {
                fsccPayload.StartingVcn = long.MaxValue;
            }

            // According to different bufferSize, set outBufferSize 
            switch (bufferSize)
            {
                case BufferSize.BufferSizeSuccess:
                    outBufferSize = this.transBufferSize;
                    break;

                case BufferSize.LessThanSTARTING_VCN_INPUT_BUFFER:
                    outBufferSize = UInt32.Parse(testConfig.GetProperty("MinBufferSize"));
                    break;

                case BufferSize.LessThanRETRIEVAL_POINTERS_BUFFER:
                    outBufferSize = UInt32.Parse(testConfig.GetProperty("MinBufferSize"));
                    break;

                default:
                    break;
            }

            FsccFsctlGetRetrievalPointersRequestPacket fsccPacket = new FsccFsctlGetRetrievalPointersRequestPacket();
            fsccPacket.Payload = fsccPayload;

            // The control code must be set to 0x90073, according to FSCC 2.3
            MessageStatus returnedStatus = transAdapter.IOControl(
                0x90073,
                outBufferSize,
                bufferSize == BufferSize.LessThanSTARTING_VCN_INPUT_BUFFER ? new byte[1] : fsccPacket.ToBytes(),
                out outbuffer);

            if (MessageStatus.SUCCESS == returnedStatus)
            {
                isBytesReturnedSet = true;
            }
            else
            {
                isBytesReturnedSet = false;
            }

            if (this.transport == Transport.SMB2 || this.transport == Transport.SMB3)
            {
                returnedStatus = SMB2_TDIWorkaround.WorkaroundFsCtlGetRetrivalPoints(bufferSize, isStartingVcnNegative, isStartingVcnGreatThanAllocationSize, isElementsNotAllCopied, ref isBytesReturnedSet, returnedStatus, site);
            }
            else
            {
                isBytesReturnedSet = SMB_TDIWorkaround.WorkAroundFsCtlGetRetrivalPoints(bufferSize, isStartingVcnNegative, isStartingVcnGreatThanAllocationSize, isElementsNotAllCopied, isBytesReturnedSet, site);
                returnedStatus = SMB_TDIWorkaround.WorkAroundFsCtlGetRetrivalPointsStatus(bufferSize, isStartingVcnNegative, isStartingVcnGreatThanAllocationSize, returnedStatus, site);
            }
            return returnedStatus;
        }
        #endregion

        #region 3.1.5.9.19   FSCTL_QUERY_ALLOCATED_RANGES

        /// <summary>
        /// Implement FsCtlQueryAllocatedRanges method
        /// </summary>
        /// <param name="bufferSize">Indicate buffer size</param>
        /// <param name="inputBuffer">Indicate buffer length</param>
        /// <param name="isBytesReturnedSet">True: if the returned status is success.</param>
        /// <returns>An NTSTATUS code that specifies the result</returns>
        public MessageStatus FsCtlQueryAllocatedRanges(
            BufferSize bufferSize,
            BufferLength inputBuffer,
            out bool isBytesReturnedSet
            )
        {
            bool isImplemented = false;
            byte[] outbuffer = new byte[0];
            byte[] inbuffer = null;

            UInt32 outBufferSize = this.transBufferSize;
            FsccFsctlQueryAllocatedRangesRequestPacket fsccPacket = new FsccFsctlQueryAllocatedRangesRequestPacket();
            FSCTL_QUERY_ALLOCATED_RANGES_Request fsccPayload = new FSCTL_QUERY_ALLOCATED_RANGES_Request();
            fsccPayload.Length = 0;
            fsccPayload.FileOffset = 0;

            switch (inputBuffer)
            {
                case BufferLength.BufferLengthSuccess:
                    fsccPayload.Length = UInt32.Parse(testConfig.GetProperty("InBufferSize"));
                    break;

                case BufferLength.EqualZero:
                    fsccPayload.Length = 0;
                    break;

                case BufferLength.LessThanZero:
                    fsccPayload.Length = -1;
                    break;

                case BufferLength.MoreThanMAXLONGLONG:
                    fsccPayload.Length = long.MaxValue;
                    //To make sure fsccPayload.Length > long.MaxValue - fsccPayload.Fileoffset, 
                    //as TD says"To make sure InputBuffer.Length > MAXLONGLONG - InputBuffer.FileOffset"
                    fsccPayload.FileOffset = 1;
                    break;

                case BufferLength.FileOffsetLessThanZero:
                    fsccPayload.FileOffset = -1;
                    break;

                default:
                    break;
            }

            fsccPacket.Payload = fsccPayload;

            // According to different condition, set the buffer size value 
            switch (bufferSize)
            {
                case BufferSize.BufferSizeSuccess:
                    outBufferSize = this.transBufferSize;
                    inbuffer = fsccPacket.ToBytes();
                    break;

                case BufferSize.LessThanFILE_ALLOCATED_RANGE_BUFFER:
                    //If InputBufferSize is less than the size of FILE_ALLOCATED_RANGE_BUFFER(16Bytes)
                    inbuffer = new byte[0];
                    break;

                case BufferSize.OutLessThanFILE_ALLOCATED_RANGE_BUFFER:
                    outBufferSize = UInt32.Parse(testConfig.GetProperty("MinBufferSize"));
                    inbuffer = fsccPacket.ToBytes();
                    break;

                default:
                    inbuffer = fsccPacket.ToBytes();
                    break;
            }

            // The control code 0x940cf, according to FSCC 2.3
            MessageStatus returnedStatus = transAdapter.IOControl(
                0x940cf,
                outBufferSize,
                inbuffer,
                out outbuffer);

            if (MessageStatus.SUCCESS == returnedStatus)
            {
                isBytesReturnedSet = true;
                isImplemented = true;
                this.VerifyFsctlQueryAllocatedRanges(isImplemented);
            }
            else
            {
                isBytesReturnedSet = false;

                if (MessageStatus.INVALID_DEVICE_REQUEST == returnedStatus)
                {
                    return returnedStatus;
                }
            }

            if (this.transport == Transport.SMB2 || this.transport == Transport.SMB3)
            {
                returnedStatus = SMB2_TDIWorkaround.WorkaroundFsCtlQueryAllocatedRanges(bufferSize, returnedStatus, ref isBytesReturnedSet, site);
            }

            return returnedStatus;
        }

        /// <summary>
        /// Implementation of FSCTL_QUERY_ALLOCATED_RANGES
        /// </summary>
        /// <param name="inputBuffer">An array of bytes containing a single FILE_ALLOCATED_RANGE_BUFFER structure indicating the range to query for allocation.</param>
        /// <param name="outputBufferSize">The maximum number of bytes to return in OutputBuffer.</param>
        /// <param name="bytesReturned">The number of bytes returned in OutputBuffer.</param>
        /// <param name="outputBuffer">An array of bytes that will return an array of zero or more FILE_ALLOCATED_RANGE_BUFFER structures.</param>
        /// <returns>An NTSTATUS code that specifies the result.</returns>
        public MessageStatus FsCtlQueryAllocatedRanges(FSCTL_QUERY_ALLOCATED_RANGES_Request inputBuffer, uint outputBufferSize, out long bytesReturned, out byte[] outputBuffer)
        {
            FsccFsctlQueryAllocatedRangesRequestPacket fsccPacket = new FsccFsctlQueryAllocatedRangesRequestPacket();
            fsccPacket.Payload = inputBuffer;
            MessageStatus returnedStatus = transAdapter.IOControl(
                (uint)FsControlCommand.FSCTL_QUERY_ALLOCATED_RANGES,
                outputBufferSize,
                fsccPacket.ToBytes(),
                out outputBuffer);

            bytesReturned = ((outputBuffer == null) ? 0 : outputBuffer.Length);
            return returnedStatus;
        }

        #endregion

        #region 2.1.5.9.24   FSCTL_READ_FILE_USN_DATA

        /// <summary>
        /// Implement FsCtlReadFileUSNData method
        /// </summary>
        /// <param name="bufferSize">Indicate buffer size</param>
        /// <param name="isBytesReturnedSet">True: if the returned status is success.</param>
        /// <returns>An NTSTATUS code that specifies the result</returns>
        public MessageStatus FsCtlReadFileUSNData(
            BufferSize bufferSize,
            out bool isBytesReturnedSet
            )
        {
            bool isImplemented = false;
            byte[] outbuffer = new byte[0];
            UInt32 outBufferSize = this.transBufferSize;

            // According to different condition, set the buffer size value 
            switch (bufferSize)
            {
                case BufferSize.BufferSizeSuccess:
                    outBufferSize = this.transBufferSize;
                    break;

                case BufferSize.LessThanUSN_RECORD:
                    outBufferSize = UInt32.Parse(testConfig.GetProperty("MinBufferSize"));
                    break;

                case BufferSize.LessThanRecordLength:
                    outBufferSize = UInt32.Parse(testConfig.GetProperty("MinBufferSize"));
                    break;

                default:
                    break;
            }

            // The control code must be set to 0x900eb, according to FSCC 2.3
            MessageStatus returnedStatus = transAdapter.IOControl(
                (uint)FsControlCommand.FSCTL_READ_FILE_USN_DATA,
                outBufferSize,
                null,
                out outbuffer);

            if (MessageStatus.SUCCESS == returnedStatus)
            {
                isBytesReturnedSet = true;
                isImplemented = true;
                this.VerifyFsctlReadFileUsnData(isImplemented);
            }
            else
            {
                isBytesReturnedSet = false;

                if (MessageStatus.INVALID_DEVICE_REQUEST == returnedStatus)
                {
                    return returnedStatus;
                }
            }

            if (this.transport == Transport.SMB2 || this.transport == Transport.SMB3)
            {
                returnedStatus = SMB2_TDIWorkaround.WorkaroundFsCtlReadFileUSNData(bufferSize, returnedStatus, site);
            }
            else
            {
                returnedStatus = SMB_TDIWorkaround.WorkAroundFsCtlReadFileUSNData(bufferSize, returnedStatus, site);
            }
            return returnedStatus;
        }


        public MessageStatus FsCtlReadFileUSNData(ushort minMajorVersion, ushort maxMajorVersion, out byte[] outbuffer)
        {
            FsccFsctlReadFileUsnDataRequestPacket fsccPacket = new FsccFsctlReadFileUsnDataRequestPacket();
            READ_FILE_USN_DATA readFileUSNDataRequestPayload = new READ_FILE_USN_DATA();
            readFileUSNDataRequestPayload.MinMajorVersion = minMajorVersion;
            readFileUSNDataRequestPayload.MaxMajorVersion = maxMajorVersion;

            fsccPacket.Payload = readFileUSNDataRequestPayload;

            MessageStatus returnedStatus = this.transAdapter.IOControl(
                (uint)FsControlCommand.FSCTL_READ_FILE_USN_DATA,
                transBufferSize,
                fsccPacket.ToBytes(),
                out outbuffer);

            return returnedStatus;
        }
        #endregion

        #region 3.1.5.9.24   FSCTL_SET_COMPRESSION

        /// <summary>
        /// Implement FsCtlSetCompression method
        /// </summary>
        /// <param name="bufferSize">Indicate buffer size</param>
        /// <param name="compressionState">This string value indicate the InputBuffer.CompressionState.
        ///  inputBufCompressState != "COMPRESSION_FORMAT_NONE" means InputBuffer.CompressionState != COMPRESSION_FORMAT_NONE
        ///  inputBufCompressState == "COMPRESSION_FORMAT_NONE" means InputBuffer.CompressionState == COMPRESSION_FORMAT_NONE</param>
        /// <param name="isCompressionSupportEnabled">True: If compression support is abled</param>
        /// <returns>An NTSTATUS code that specifies the result</returns>
        public MessageStatus FsCtlSetCompression(
            BufferSize bufferSize,
            InputBufferCompressionState compressionState,
            bool isCompressionSupportEnabled
            )
        {
            byte[] inputBuffer = new byte[0];
            byte[] outbuffer = new byte[0];

            UInt32 outBufferSize = this.transBufferSize;
            FsccFsctlSetCompressionRequestPacket fsccPacket = new FsccFsctlSetCompressionRequestPacket();
            FSCTL_SET_COMPRESSION_Request fsccPayload = new FSCTL_SET_COMPRESSION_Request();

            switch (compressionState)
            {
                case InputBufferCompressionState.COMPRESSION_FORMAT_NONE:
                    fsccPayload.CompressionState = (FSCTL_SET_COMPRESSION_Request_CompressionState_Values)CompressionFormat_Values.COMPRESSION_FORMAT_NONE;
                    break;

                case InputBufferCompressionState.NotPrefinedValue:
                    fsccPayload.CompressionState = (FSCTL_SET_COMPRESSION_Request_CompressionState_Values)4;
                    break;

                default:
                    break;
            }
            fsccPacket.Payload = fsccPayload;

            // According to different condition, set the buffer size value 
            switch (bufferSize)
            {
                case BufferSize.BufferSizeSuccess:
                    outBufferSize = this.transBufferSize;
                    inputBuffer = fsccPacket.ToBytes();
                    break;

                case BufferSize.LessThanTwoBytes:
                    outBufferSize = UInt32.Parse(testConfig.GetProperty("MinBufferSize"));
                    inputBuffer = new byte[1]; // Less than two bytes
                    break;

                default:
                    break;
            }

            // The control code must be set to 0x9c040, according to FSCC 2.3
            MessageStatus returnedStatus = transAdapter.IOControl(
                0x9c040,
                outBufferSize,
                inputBuffer,
                out outbuffer);

            if (this.fileSystem == Adapter.FileSystem.REFS &&
                compressionState != InputBufferCompressionState.COMPRESSION_FORMAT_NONE &&
                returnedStatus == MessageStatus.NOT_SUPPORTED)
            {
                site.Log.Add(LogEntryKind.Debug, @"[MS-FSA] Section 2.1.5.9.27: This method is fully supported with NTFS, 
                    but for ReFS, it is only supported and returns STATUS_SUCCESS when CompressionState is set to COMPRESSION_FORMAT_NONE. 
                    The method fails with STATUS_NOT_SUPPORTED for any other value of CompressionState.");
                site.Log.Add(LogEntryKind.Debug, @"Transfer the return code to STATUS_INVALID_PARAMETER to make model test cases passed.");
                returnedStatus = MessageStatus.INVALID_PARAMETER;
            }


            return returnedStatus;
        }

        #endregion

        #region 3.1.5.9.25   FSCTL_SET_DEFECT_MANAGEMENT

        /// <summary>
        /// Implement FsCtlSetDefectManagement method
        /// </summary>
        /// <param name="bufferSize">Indicate buffer size.</param>
        /// <param name="isOpenListContainMoreThanOneOpen">TRUE: if OpenList Contain More Than One Open</param>
        /// <returns>An NTSTATUS code that specifies the result</returns>
        public MessageStatus FsCtlSetDefectManagement(
            BufferSize bufferSize,
            bool isOpenListContainMoreThanOneOpen
            )
        {
            byte[] inbuffer = null;
            byte[] outbuffer = new byte[0];
            UInt32 outBufferSize;

            // According to different condition, set the buffer size value 
            switch (bufferSize)
            {
                case BufferSize.BufferSizeSuccess:
                    outBufferSize = this.transBufferSize;
                    outbuffer = new byte[outBufferSize];
                    break;

                case BufferSize.LessThanOneBytes:
                    outBufferSize = 0;
                    outbuffer = null;
                    break;

                default:
                    outBufferSize = this.transBufferSize;
                    outbuffer = new byte[outBufferSize];
                    break;
            }

            // The control code must be set to 0x98134, according to FSCC 2.3
            MessageStatus returnedStatus = transAdapter.IOControl(
                0x98134,
                outBufferSize,
                inbuffer,
                out outbuffer);

            if ((fileSystem == Adapter.FileSystem.NTFS ||
                fileSystem == Adapter.FileSystem.REFS) &&
                returnedStatus == MessageStatus.INVALID_DEVICE_REQUEST)
            {
                if (bufferSize == BufferSize.BufferSizeSuccess)
                {
                    returnedStatus = FsaUtility.TransferExpectedResult<MessageStatus>(1176, MessageStatus.SUCCESS, returnedStatus, site);
                }
                else if (bufferSize == BufferSize.LessThanOneBytes)
                {
                    returnedStatus = FsaUtility.TransferExpectedResult<MessageStatus>(1178, MessageStatus.INVALID_PARAMETER, returnedStatus, site);
                }
            }

            return returnedStatus;
        }

        #endregion

        #region 3.1.5.9.26   FSCTL_SET_ENCRYPTION

        /// <summary>
        /// Implement FsCtlSetEncryption method
        /// </summary>
        /// <param name="isIsCompressedTrue">True: if stream is compressed.</param>
        /// <param name="encryptionOpteration">InputBuffer.EncryptionOperation </param>
        /// <param name="bufferSize">Indicate buffer size</param>
        /// <returns>An NTSTATUS code that specifies the result</returns>
        public MessageStatus FsCtlSetEncryption(
            bool isIsCompressedTrue,
            EncryptionOperation encryptionOpteration,
            BufferSize bufferSize
            )
        {
            byte[] outbuffer = new byte[0];

            UInt32 outBufferSize = this.transBufferSize;
            List<byte> byteList = new List<byte>();

            byteList.AddRange(BitConverter.GetBytes((UInt32)encryptionOpteration));
            byteList.AddRange(BitConverter.GetBytes((byte)0));
            byteList.AddRange(new byte[3]);

            // According to different condition, set the buffer size value 
            switch (bufferSize)
            {
                case BufferSize.BufferSizeSuccess:
                    outBufferSize = this.transBufferSize;
                    break;

                case BufferSize.LessThanENCRYPTION_BUFFER:
                    outBufferSize = UInt32.Parse(testConfig.GetProperty("MinBufferSize"));
                    break;

                default:
                    break;
            }

            // The control code must be set to 0x900D7, according to FSCC 2.3
            MessageStatus returnedStatus = transAdapter.IOControl(
                0x900D7,
                outBufferSize,
                bufferSize == BufferSize.LessThanENCRYPTION_BUFFER ? new byte[1] : byteList.ToArray(),
                out outbuffer);

            if (this.transport == Transport.SMB2 || this.transport == Transport.SMB3)
            {
                returnedStatus = SMB2_TDIWorkaround.WorkaroundFsCtlSetEncrypion(fileSystem, isIsCompressedTrue, encryptionOpteration, bufferSize, returnedStatus, site);
            }
            else
            {
                returnedStatus = SMB_TDIWorkaround.WorkAroundFsCtlSetEncryption(fileSystem, isIsCompressedTrue, encryptionOpteration, bufferSize, returnedStatus, site);
            }

            return returnedStatus;
        }

        /// <summary>
        /// Implementation of FSCTL_SET_ENCRYPTION
        /// </summary>
        /// <param name="inputBuffer">An array of bytes containing an ENCRYPTION_BUFFER structure indicating the requested encryption state of the stream or file.</param>
        /// <param name="inputBufferSize">The number of bytes in InputBuffer.</param>
        /// <returns>An NTSTATUS code that specifies the result.</returns>
        public MessageStatus FsCtlSetEncryption(ENCRYPTION_BUFFER inputBuffer, uint inputBufferSize)
        {
            byte[] outbuffer = new byte[0];

            FsccFsctlSetEncryptionRequestPacket fsccPacket = new FsccFsctlSetEncryptionRequestPacket();
            fsccPacket.Payload = inputBuffer;

            MessageStatus returnedStatus = this.transAdapter.IOControl(
                (uint)FsControlCommand.FSCTL_SET_ENCRYPTION,
                inputBufferSize,
                fsccPacket.ToBytes(),
                out outbuffer);

            return returnedStatus;
        }

        #endregion

        //SET_OBJECT_ID
        //SET_OBJECT_ID_EXTENDED
        //IS_PATHNAME_VALID
        //LMR_GET_LINK_TRACKING_INFORMATION
        //LMR_SET_LINK_TRACKING_INFORMATION
        //QUERY_FAT_BPB
        //QUERY_ON_DISK_VOLUME_INFO
        //QUERY_SPARING_INFO
        //RECALL_FILE
        //SET_SHORT_NAME_BEHAVIOR
        //SET_ZERO_ON_DEALLOCATION
        //WRITE_USN_CLOSE_RECORD
        //GET_NTFS_VOLUME_DATA
        //FSCTL_GET_COMPRESSION
        //FSCTL_FILESYSTEM_GET_STATISTICS
        //FSCTL_SET_SHORT_NAME_BEHAVIOR
        #region 3.1.5.9.28   FSCTL_SET_OBJECT_ID

        /// <summary>
        /// Implement FsCtlSetObjID method
        /// </summary>
        /// <param name="requestType">FsControlRequestType is self-defined to indicate control type</param>
        /// <param name="bufferSize">Indicate buffer size</param>
        /// <returns>An NTSTATUS code that specifies the result</returns>
        public MessageStatus FsCtlSetObjID(
            FsControlRequestType requestType,
            BufferSize bufferSize
            )
        {
            byte[] outbuffer = new byte[0];
            byte[] inbuffer = null;
            uint ctlCode = 0;

            switch (bufferSize)
            {
                case BufferSize.BufferSizeSuccess:
                    if (requestType == FsControlRequestType.SET_OBJECT_ID_EXTENDED)
                    {
                        // ExtendedInfo is a 48-byte binary large object(BLOB) containing user-defined extended data 
                        inbuffer = new byte[48];
                    }
                    else
                    {
                        inbuffer = new byte[int.Parse(testConfig.GetProperty("InBufferSize"))];
                    }
                    break;

                case BufferSize.NotEqualFILE_OBJECTID_BUFFER:
                    inbuffer = new byte[1];
                    break;

                case BufferSize.NotEqual48Bytes:
                    inbuffer = new byte[48 + 1];
                    break;
            }

            // According to FSCC 2.3, the ctlCode should be as follows:
            switch (requestType)
            {
                case FsControlRequestType.SET_OBJECT_ID:
                    ctlCode = 0x90098;
                    break;

                case FsControlRequestType.SET_OBJECT_ID_EXTENDED:
                    ctlCode = 0x900bc;
                    break;

                default:
                    ctlCode = 0x90098;
                    break;
            }

            MessageStatus returnedStatus = transAdapter.IOControl(
                ctlCode,
                this.transBufferSize,
                inbuffer,
                out outbuffer);

            if (this.transport == Transport.SMB2 || this.transport == Transport.SMB3)
            {
                returnedStatus = SMB2_TDIWorkaround.WorkaroundFsCtlSetObjID(requestType, bufferSize, returnedStatus, site);
            }
            else
            {
                returnedStatus = SMB_TDIWorkaround.WorkAroundFsCtlSetObjID(requestType, bufferSize, returnedStatus, site);
            }
            return returnedStatus;
        }

        #endregion

        #region 3.1.5.9.30   FSCTL_SET_REPARSE_POINT

        /// <summary>
        /// Implement FsCtlSetReparsePoint method
        /// </summary>
        /// <param name="inputReparseTag">InputBuffer.ReparseTag</param>
        /// <param name="bufferSize">Indicate buffer size</param>
        /// <param name="isReparseGUIDNotEqual">True: if Open.File.ReparseGUID is not equal to InputBuffer.ReparseGUID</param>
        /// <param name="isFileReparseTagNotEqualInputBufferReparseTag">True: if Open.File.ReparseTag is not equal to 
        /// InputBuffer.ReparseTag.</param>
        /// <returns>An NTSTATUS code that specifies the result</returns>
        public MessageStatus FsCtlSetReparsePoint(
            ReparseTag inputReparseTag,
            BufferSize bufferSize,
            bool isReparseGUIDNotEqual,
            bool isFileReparseTagNotEqualInputBufferReparseTag
            )
        {
            byte[] outbuffer = new byte[0];
            byte[] inbuffer = null;

            FsccFsctlSetReparsePointRequestPacket fsccPacket = new FsccFsctlSetReparsePointRequestPacket();
            REPARSE_GUID_DATA_BUFFER fsccPayload = new REPARSE_GUID_DATA_BUFFER();
            fsccPayload.ReparseTag = (uint)inputReparseTag;
            fsccPayload.ReparseDataLength = ushort.Parse(testConfig.GetProperty("BufferSize"));
            fsccPayload.DataBuffer = new byte[fsccPayload.ReparseDataLength];
            fsccPayload.Reserved = 0;

            if (isReparseGUIDNotEqual)
            {
                fsccPayload.ReparseGuid = Guid.NewGuid();
            }
            fsccPacket.ReparseGuidDataBuffer = fsccPayload;

            switch (bufferSize)
            {
                case BufferSize.BufferSizeSuccess:
                    inbuffer = fsccPacket.ToBytes();
                    break;

                case BufferSize.LessThan8Bytes:
                    inbuffer = new byte[8 - 1];
                    break;

                case BufferSize.NotEqualReparseDataLengthPlus24:
                    inbuffer = new byte[int.Parse(testConfig.GetProperty("BufferSizeNotEqualReparseDataLengthPlus24"))];
                    break;

                case BufferSize.NotEqualReparseDataLengthPlus8:
                    inbuffer = new byte[int.Parse(testConfig.GetProperty("BufferSizeNotEqualReparseDataLengthPlus8"))];
                    break;

                default:
                    break;

            }

            // The control code must be set to 0x900a4, according to FSCC 2.3
            MessageStatus returnedStatus = transAdapter.IOControl(
                0x900a4,
                this.transBufferSize,
                inbuffer,
                out outbuffer);

            if (this.transport == Transport.SMB2 || this.transport == Transport.SMB3)
            {
                returnedStatus = SMB2_TDIWorkaround.WorkaroundFsCtlSetReparsePoint(fileSystem, inputReparseTag, bufferSize, isReparseGUIDNotEqual, isFileReparseTagNotEqualInputBufferReparseTag, returnedStatus, site);
            }
            else
            {
                returnedStatus = SMB_TDIWorkaround.WorkAroundFsCtlSetReparsePoint(fileSystem, inputReparseTag, isReparseGUIDNotEqual, isFileReparseTagNotEqualInputBufferReparseTag, returnedStatus, site);
            }

            return returnedStatus;
        }

        /// <summary>
        /// Implementation of FSCTL_SET_REPARSE_POINT
        /// </summary>
        /// <param name="inputBuffer">: An array of bytes containing a REPARSE_DATA_BUFFER structure .</param>
        /// <param name="inputBufferSize">The number of bytes in InputBuffer.</param>
        /// <returns>An NTSTATUS code that specifies the result.</returns>
        public MessageStatus FsCtlSetReparsePoint(REPARSE_DATA_BUFFER inputBuffer, uint inputBufferSize)
        {
            byte[] outbuffer = new byte[0];

            FsccFsctlSetReparsePointRequestPacket fsccPacket = new FsccFsctlSetReparsePointRequestPacket();
            fsccPacket.ReparseDataBuffer = inputBuffer;

            MessageStatus returnedStatus = this.transAdapter.IOControl(
                (uint)FsControlCommand.FSCTL_SET_REPARSE_POINT,
                inputBufferSize,
                fsccPacket.ToBytes(),
                out outbuffer);

            return returnedStatus;
        }

        /// <summary>
        /// Implementation of FSCTL_SET_REPARSE_POINT
        /// </summary>
        /// <param name="inputBuffer">: An array of bytes containing a REPARSE_GUID_DATA_BUFFER structure .</param>
        /// <param name="inputBufferSize">The number of bytes in InputBuffer.</param>
        /// <returns>An NTSTATUS code that specifies the result.</returns>
        public MessageStatus FsCtlSetReparsePoint(REPARSE_GUID_DATA_BUFFER inputBuffer, uint inputBufferSize)
        {
            byte[] outbuffer = new byte[0];

            FsccFsctlSetReparsePointRequestPacket fsccPacket = new FsccFsctlSetReparsePointRequestPacket();
            fsccPacket.ReparseGuidDataBuffer = inputBuffer;

            MessageStatus returnedStatus = this.transAdapter.IOControl(
                (uint)FsControlCommand.FSCTL_SET_REPARSE_POINT,
                inputBufferSize,
                fsccPacket.ToBytes(),
                out outbuffer);

            return returnedStatus;
        }

        #endregion

        #region 3.1.5.9.32   FSCTL_SET_SPARSE

        /// <summary>
        /// This message requests that the server mark the file that is associated with the handle on which this FSCTL was invoked as sparse. 
        /// In a sparse file, large ranges of zeros (0) may not require disk allocation. 
        /// Space for nonzero data is allocated as the file is written. 
        /// The message either has no data elements at all or it contains a FILE_SET_SPARSE_BUFFER element. 
        /// If there is no data element, the sparse flag for the file is set, 
        /// exactly as if the FILE_SET_SPARSE_BUFFER element was supplied and had a SetSparse value of TRUE.
        /// </summary>
        /// <param name="IsSetSparse">TRUE for Set sparse and FALSE will unsparse the file.</param>
        /// <returns>An NTSTATUS code that specifies the result.</returns>
        public MessageStatus FsCtlSetSparse(bool IsSetSparse)
        {
            byte[] outputBuffer = null;
            FsccFsctlSetSparseRequestPacket fsccPacket = new FsccFsctlSetSparseRequestPacket();
            fsccPacket.Payload = IsSetSparse ? (byte)1 : (byte)0;

            MessageStatus returnedStatus = this.transAdapter.IOControl(
                (uint)FsControlCommand.FSCTL_SET_SPARSE,
                8,
                fsccPacket.ToBytes(),
                out outputBuffer);

            return returnedStatus;
        }

        #endregion

        #region 3.1.5.9.33   FSCTL_SET_ZERO_DATA

        /// <summary>
        /// Implement FsCtlSetZeroData method
        /// </summary>
        /// <param name="bufferSize">Indicate buffer size</param>
        /// <param name="inputBuffer">InputBuffer_FSCTL_SET_ZERO_DATA</param>
        /// <param name="isIsDeletedTrue">True: if Open.Stream.IsDeleted</param>
        /// <param name="isConflictDetected">True: If a conflict is detected.</param>
        /// <returns>An NTSTATUS code that specifies the result</returns>
        public MessageStatus FsCtlSetZeroData(
            BufferSize bufferSize,
            InputBuffer_FSCTL_SET_ZERO_DATA inputBuffer,
            bool isIsDeletedTrue,
            bool isConflictDetected
            )
        {
            bool isImplemented = false;
            byte[] inbuffer = new byte[0];
            byte[] outbuffer = new byte[0];

            // Create packet and initilize fields
            FsccFsctlSetZeroDataRequestPacket fsccPacket = new FsccFsctlSetZeroDataRequestPacket();
            FSCTL_SET_ZERO_DATA_Request fsccPlayload = new FSCTL_SET_ZERO_DATA_Request();
            fsccPlayload.FileOffset = 0;
            fsccPlayload.BeyondFinalZero = 0;

            // Set FileOffset and BeyondFinalZero
            switch (inputBuffer)
            {
                case InputBuffer_FSCTL_SET_ZERO_DATA.FileOffsetLessThanZero:
                    fsccPlayload.FileOffset = -1;
                    break;

                case InputBuffer_FSCTL_SET_ZERO_DATA.BeyondFinalZeroLessThanZero:
                    fsccPlayload.BeyondFinalZero = -1;
                    break;

                default:
                    break;
            }

            fsccPacket.Payload = fsccPlayload;


            switch (bufferSize)
            {
                case BufferSize.LessThanFILE_ZERO_DATA_INFORMATION:
                    inbuffer = BitConverter.GetBytes(fsccPlayload.BeyondFinalZero);
                    break;

                case BufferSize.BufferSizeSuccess:
                    inbuffer = fsccPacket.ToBytes();
                    break;

                default:
                    break;
            }

            // The control code must be set to 0x980c8, according to FSCC 2.3
            MessageStatus returnedStatus = transAdapter.IOControl(
                0x980c8,
                this.transBufferSize,
                inbuffer,
                out outbuffer);

            if (returnedStatus == MessageStatus.SUCCESS)
            {
                isImplemented = true;
                this.VerifyFsctlSetZeroData(isImplemented);
            }
            if (this.transport == Transport.SMB2 || this.transport == Transport.SMB3)
            {
                returnedStatus = SMB2_TDIWorkaround.WorkaroundFsCtlSetZeroData(fileSystem, bufferSize, inputBuffer, isIsDeletedTrue, isConflictDetected, returnedStatus, this.site);
            }
            else
            {
                returnedStatus = SMB_TDIWorkaround.WorkAroundFsCtlSetZeroData(fileSystem, bufferSize, inputBuffer, isIsDeletedTrue, isConflictDetected, returnedStatus, site);
            }

            return returnedStatus;
        }

        /// <summary>
        /// Implementation of FSCTL_SET_ZERO_DATA
        /// </summary>
        /// <param name="inputBuffer">An array of bytes containing a FILE_ZERO_DATA_INFORMATION structure.</param>
        /// <param name="inputBufferSize">The byte count of the InputBuffer.</param>
        /// <returns>An NTSTATUS code that specifies the result.</returns>
        public MessageStatus FsCtlSetZeroData(FSCTL_SET_ZERO_DATA_Request inputBuffer, uint inputBufferSize)
        {
            byte[] outbuffer = new byte[0];

            FsccFsctlSetZeroDataRequestPacket fsccPacket = new FsccFsctlSetZeroDataRequestPacket();
            fsccPacket.Payload = inputBuffer;

            MessageStatus returnedStatus = this.transAdapter.IOControl(
                (uint)FsControlCommand.FSCTL_SET_ZERO_DATA,
                inputBufferSize,
                fsccPacket.ToBytes(),
                out outbuffer);

            return returnedStatus;
        }

        #endregion

        #region 3.1.5.9.35   FSCTL_SIS_COPYFILE

        /// <summary>
        /// Implement FsctlSisCopyFile method
        /// </summary>
        /// <param name="bufferSize">Indicate buffer size</param>
        /// <param name="inputBuffer">InputBufferFSCTL_SIS_COPYFILE</param>
        /// <param name="isIsEncryptedTrue">True if encrypted</param>
        /// <param name="isCOPYFILE_SIS_LINKTrue">True: if InputBuffer.Flags.COPYFILE_SIS_LINK is TRUE</param>
        /// <returns>An NTSTATUS code that specifies the result</returns>
        public MessageStatus FsctlSisCopyFile(
            BufferSize bufferSize,
            InputBufferFSCTL_SIS_COPYFILE inputBuffer,
            bool isCOPYFILE_SIS_LINKTrue,
            bool isIsEncryptedTrue
            )
        {
            byte[] outbuffer = new byte[0];
            byte[] inbuffer = null;
            string destFilename = this.ComposeRandomFileName();

            FsccFsctlSisCopyfileRequestPacket fsscPacket = new FsccFsctlSisCopyfileRequestPacket();
            FSCTL_SIS_COPYFILE_Request fsscPlayload = new FSCTL_SIS_COPYFILE_Request();
            fsscPlayload.SourceFileName = Encoding.Unicode.GetBytes(this.fileName);
            fsscPlayload.SourceFileNameLength = (uint)Encoding.Unicode.GetByteCount(this.fileName);
            fsscPlayload.DestinationFileName = Encoding.Unicode.GetBytes(destFilename);
            fsscPlayload.DestinationFileNameLength = (uint)Encoding.Unicode.GetByteCount(destFilename);
            fsscPlayload.Flags = isCOPYFILE_SIS_LINKTrue ? FSCTL_SIS_COPYFILE_Request_Flags_Values.COPYFILE_SIS_LINK : FSCTL_SIS_COPYFILE_Request_Flags_Values.COPYFILE_SIS_REPLACE;

            switch (inputBuffer)
            {
                case InputBufferFSCTL_SIS_COPYFILE.FlagsNotContainCOPYFILE_SIS_LINKAndCOPYFILE_SIS_REPLACE:
                    fsscPlayload.Flags = (FSCTL_SIS_COPYFILE_Request_Flags_Values)0;
                    break;

                case InputBufferFSCTL_SIS_COPYFILE.DestinationFileNameLengthLessThanZero:
                    fsscPlayload.DestinationFileNameLength = 0x0;
                    break;

                case InputBufferFSCTL_SIS_COPYFILE.DestinationFileNameLengthLargeThanMAXUSHORT:
                    fsscPlayload.DestinationFileNameLength = ushort.MaxValue + 1;
                    break;

                case InputBufferFSCTL_SIS_COPYFILE.InputBufferSizeLessThanOtherPlus:
                    //1 is a value less than other plus, according to FSCC 2.3.61
                    inbuffer = new byte[1];
                    break;

                default:
                    break;
            }

            if (BufferSize.LessThanSI_COPYFILE == bufferSize)
            {
                inbuffer = fsscPlayload.SourceFileName;
            }

            fsscPacket.Payload = fsscPlayload;

            // The control code must be set to 0x90100, according to FSCC 2.3
            MessageStatus returnedStatus = transAdapter.IOControl(
               0x90100,
               this.transBufferSize,
               inbuffer,
               out outbuffer);

            if (this.transport == Transport.SMB2 || this.transport == Transport.SMB3)
            {
                returnedStatus = SMB2_TDIWorkaround.WorkaroundFsctlSisCopy(fileSystem, bufferSize, inputBuffer, isCOPYFILE_SIS_LINKTrue, isIsEncryptedTrue, returnedStatus, site);
            }
            else
            {
                returnedStatus = SMB_TDIWorkaround.WorkAroundFsctlSisCopyFile(fileSystem, bufferSize, inputBuffer, isCOPYFILE_SIS_LINKTrue, isIsEncryptedTrue, returnedStatus, site);
            }

            return returnedStatus;
        }

        #endregion

        #region FSCTL_DUPLICATE_EXTENTS_TO_FILE_EX
        /// <summary>
        /// Implement FSCTL_DUPLICATE_EXTENTS_TO_FILE_EX, which duplicates file extents from the same opened file.
        /// </summary>
        /// <param name="sourceFileOffset">Offset of source file.</param>
        /// <param name="targetFileOffset">Offset of target file.</param>
        /// <param name="byteCount">The number of bytes of duplicate.</param>
        /// <param name="flags">Flags for duplicate file.</param>
        /// <returns></returns>
        public MessageStatus FsctlDuplicateExtentsToFileEx(
            long sourceFileOffset,
            long targetFileOffset,
            long byteCount,
            Smb2.FSCTL_DUPLICATE_EXTENTS_TO_FILE_EX_Request_Flags_Values flags
            )
        {
            if (!(transAdapter is Smb2TransportAdapter))
            {
                Site.Assume.Inconclusive("FSCTL_DUPLICATE_EXTENTS_TO_FILE_EX is only supported by SMB2 transport!");
            }

            var sourceFileId = (transAdapter as Smb2TransportAdapter).FileId;

            var request = new Smb2.FSCTL_DUPLICATE_EXTENTS_TO_FILE_EX_Request();

            request.StructureSize = 0x30;
            request.SourceFileId = sourceFileId;
            request.SourceFileOffset = sourceFileOffset;
            request.TargetFileOffset = targetFileOffset;
            request.ByteCount = byteCount;
            request.Flags = flags;
            request.Reserved = 0;

            var input = TypeMarshal.ToBytes(request);
            byte[] output;

            var status = transAdapter.IOControl(
                (uint)Smb2.CtlCode_Values.FSCTL_DUPLICATE_EXTENTS_TO_FILE_EX,
                0,
                input,
                out output
                );

            return status;
        }
        #endregion

        #region 2.1.5.9.21 FSCTL_QUERY_FILE_REGIONS 
        public MessageStatus FsctlQueryFileRegionsWithInputData(
            long fileOffset,
            long length,
            FILE_REGION_USAGE desiredUsage,
            out byte[] output)
        {
            byte[] input = null;
            var request = new FILE_REGION_INPUT();
            request.FileOffset = fileOffset;
            request.Length = length;
            request.DesiredUsage = desiredUsage;
            input = TypeMarshal.ToBytes(request);

            var status = transAdapter.IOControl(
                (uint)FsControlCommand.FSCTL_QUERY_FILE_REGIONS,
                transBufferSize,
                input,
                out output
                );

            return status;
        }
        #endregion

        #endregion

        #region 3.1.5.10   Server Requests Change Notifications for a Directory

        /// <summary>
        /// Implement ChangeNotificationForDir method
        /// </summary>
        /// <param name="changeNotifyEntryType">ChangeNotifyEntryType to indicate if all entries from ChangeNotifyEntry.NotifyEventList
        /// fit in OutputBufferSize bytes</param>
        /// <returns>An NTSTATUS code that specifies the result</returns>
        public MessageStatus ChangeNotificationForDir(ChangeNotifyEntryType changeNotifyEntryType)
        {
            bool allEntriesFitBufSize = (changeNotifyEntryType == ChangeNotifyEntryType.AllEntriesFitInOutputBufferSize);

            bool watchTree = true;
            uint maxOutputSize = (uint)(allEntriesFitBufSize ? this.transBufferSize : 1);
            uint completionFilter = 0;
            byte[] outBuffer = null;

            MessageStatus returnedStatus = this.transAdapter.ChangeNotification(
                maxOutputSize,
                watchTree,
                completionFilter,
                out outBuffer);


            if (this.transport == Transport.SMB2 || this.transport == Transport.SMB3)
            {
                returnedStatus = SMB2_TDIWorkaround.WorkaroundChangeNotificationForDir(allEntriesFitBufSize, returnedStatus, site);
            }
            else
            {
                returnedStatus = SMB_TDIWorkaround.WorkAroundChangeNotificationForDir(allEntriesFitBufSize, returnedStatus, site);
            }
            return returnedStatus;
        }
        #endregion

        #region 3.1.5.11   Server Requests a Query of File Information

        /// <summary>
        /// Implementation of Query File Information
        /// </summary>
        /// <param name="fileInformationClass">The type of information being queried.</param>
        /// <param name="outputBufferSize">The maximum number of bytes to be returned in OutputBuffer.</param>
        /// <param name="byteCount">The number of bytes stored in OutputBuffer.</param>
        /// <param name="outputBuffer">An array of bytes containing the file information. The structure of these bytes is dependent on FileInformationClass.</param>
        /// <returns>: An NTSTATUS code that specifies the result.</returns>
        public MessageStatus QueryFileInformation(FileInfoClass fileInformationClass, uint outputBufferSize, out long byteCount, out byte[] outputBuffer)
        {
            outputBuffer = null;

            MessageStatus returnedStatus = this.transAdapter.QueryFileInformation(
                outputBufferSize,
                (uint)fileInformationClass,
                null,
                out outputBuffer);

            byteCount = ((outputBuffer == null) ? 0 : outputBuffer.Length);

            return returnedStatus;
        }

        /// <summary>
        /// Implement QueryFileInfoPart1 method
        /// </summary>
        /// <param name="fileInfoClass">The type of information being queried, as specified in [MS-FSCC] section 2.5.</param>
        /// <param name="outputBufferSize">Indicate output buffer size</param>
        /// <param name="byteCount">The desired number of bytes to query.</param>
        /// <param name="outputBuffer"></param>
        /// <returns>An NTSTATUS code that specifies the result</returns>
        public MessageStatus QueryFileInfoPart1(
            FileInfoClass fileInfoClass,
            OutputBufferSize outputBufferSize,
            out ByteCount byteCount,
            out OutputBuffer outputBuffer
            )
        {
            //FILE_ACCESS_INFORMATION is 4 according to FSCC 2.4.1
            const int sizeFileAccessInfo = 4;
            byte[] inBuffer = null;
            byte[] outBuffer = null;
            uint maxOutputSize = 1;

            if (outputBufferSize == OutputBufferSize.NotLessThan)
            {
                maxOutputSize = this.transBufferSize;
            }

            MessageStatus returnedStatus = this.transAdapter.QueryFileInformation(
                maxOutputSize,
                (uint)fileInfoClass,
                inBuffer,
                out outBuffer);

            outputBuffer = new OutputBuffer();
            byteCount = ByteCount.NotSet;

            if (outBuffer != null)
            {
                switch (fileInfoClass)
                {
                    case FileInfoClass.FILE_ACCESS_INFORMATION:
                        byteCount = ByteCount.SizeofFILE_ACCESS_INFORMATION;
                        if (outBuffer.Length >= sizeFileAccessInfo)
                        {
                            if (gOpenGrantedAccess == FileAccess.GENERIC_ALL)
                            {
                                // [MS-SMB2] 2.2.13   SMB2 CREATE Request
                                // GENERIC_ALL indicates a request for all the access flags that are previously listed except MAXIMUM_ALLOWED and ACCESS_SYSTEM_SECURITY.
                                // However, if set to GENERIC_ALL when create, the actual FileAccess granted is separated to FILE_LIST_DIRECTORY, FILE_ADD_FILE, etc.
                                // The model cannot handle such condition, so transfer to FileAccess.None to make model cases passed.                                
                                outputBuffer.AccessFlags = FileAccess.None;
                            }
                            else
                            {
                                outputBuffer.AccessFlags = (FileAccess)(BitConverter.ToUInt32(outBuffer, 0));
                            }
                        }
                        break;

                    case FileInfoClass.FILE_ALIGNMENT_INFORMATION:
                        AlignmentRequirement alignment = (AlignmentRequirement)(BitConverter.ToUInt32(outBuffer, 0));
                        this.VerifyFileAlignmentInformation(alignment);
                        byteCount = ByteCount.SizeofFILE_ALIGNMENT_INFORMATION;
                        break;

                    case FileInfoClass.FILE_ALL_INFORMATION:
                        byteCount = ByteCount.FieldOffsetFILE_ALL_INFORMATION_NameInformationAddNameInformationLength;
                        break;

                    case FileInfoClass.FILE_ALTERNATENAME_INFORMATION:
                        byteCount = ByteCount.FieldOffsetFILE_NAME_INFORMATION_FileNameAddOutputBuffer_FileNameLength;
                        break;

                    case FileInfoClass.FILE_ATTRIBUTETAG_INFORMATION:
                        byteCount = ByteCount.SizeofFILE_ATTRIBUTE_TAG_INFORMATION;
                        break;

                    case FileInfoClass.FILE_BASIC_INFORMATION:
                        byteCount = ByteCount.SizeofFILE_BASIC_INFORMATION;
                        break;

                    case FileInfoClass.FILE_COMPRESSION_INFORMATION:
                        byteCount = ByteCount.SizeofFILE_COMPRESSION_INFORMATION;
                        break;

                    case FileInfoClass.FILE_EA_INFORMATION:
                        byteCount = ByteCount.SizeofFILE_EA_INFORMATION;
                        break;

                    case FileInfoClass.FILE_FULLEA_INFORMATION:
                        byteCount = ByteCount.SizeofFILE_FULL_EA_INFORMATION;
                        break;

                    case FileInfoClass.FILE_INTERNAL_INFORMATION:
                        byteCount = ByteCount.SizeofFILE_INTERNAL_INFORMATION;
                        break;

                    case FileInfoClass.FILE_MODE_INFORMATION:
                        byteCount = ByteCount.SizeofFILE_MODE_INFORMATION;
                        break;

                    case FileInfoClass.FILE_NETWORKOPEN_INFORMATION:
                        byteCount = ByteCount.SizeofFILE_NETWORK_OPEN_INFORMATION;
                        break;

                    case FileInfoClass.FILE_STANDARD_INFORMATION:
                        FileStandardInformation fileInfo = new FileStandardInformation();
                        //According to [MS-FSCC]2.4.38, the start index is 16.
                        fileInfo.NumberOfLinks = BitConverter.ToUInt32(outBuffer, 16);
                        //For FILE_STANDARD_INFORMATION, assign 1 byte to DeletePending, defined in FSCC 2.4.38
                        fileInfo.DeletePending = outBuffer[1];
                        this.VerifyFileStandardInformation(fileInfo, returnedStatus);
                        byteCount = ByteCount.SizeofFILE_STANDARD_INFORMATION;
                        break;

                    default:
                        byteCount = ByteCount.NotSet;
                        break;
                }
            }

            if (this.transport == Transport.SMB2 || this.transport == Transport.SMB3)
            {
                returnedStatus = SMB2_TDIWorkaround.WorkaroundQueryFileInfoPart1(this.fileSystem, fileInfoClass, outputBufferSize, ref byteCount, ref outputBuffer, returnedStatus, site);
            }
            else
            {
                returnedStatus = SMB_TDIWorkaround.WorkAroundQueryFileInfoPart1(this.fileSystem, fileInfoClass, outputBufferSize, ref byteCount, ref outputBuffer, returnedStatus, site);
            }

            return returnedStatus;
        }

        #region 3.1.5.11.12   FileFullEaInformation

        public MessageStatus QueryFileFullEaInformation(uint outputBufferSize, out long byteCount, out byte[] outputBuffer)
        {
            byteCount = 0;
            outputBuffer = null;
            if (this.transport == Transport.SMB)
            {
                bool isUsePassThroughtInfoLevel = this.transAdapter.IsUsePassThroughInfoLevelCode;
                //Use SMB specific information level,do not use Pass-through Information Level Codes
                this.transAdapter.IsUsePassThroughInfoLevelCode = false;

                MessageStatus returnedStatus = this.transAdapter.QueryFileInformation(
                    outputBufferSize,
                    (uint)QueryInformationLevel.SMB_INFO_QUERY_ALL_EAS,
                    null,
                    out outputBuffer);

                byteCount = ((outputBuffer == null) ? 0 : outputBuffer.Length);

                //Change the setting back to previous value
                this.transAdapter.IsUsePassThroughInfoLevelCode = isUsePassThroughtInfoLevel;

                return returnedStatus;
            }
            else
            {
                return QueryFileInformation(FileInfoClass.FILE_FULLEA_INFORMATION, outputBufferSize, out byteCount, out outputBuffer);
            }
        }

        #endregion

        #endregion

        #region 3.1.5.12   Server Requests a Query of File System Information

        /// <summary>
        /// Implementation of Query File System Information
        /// </summary>
        /// <param name="fsInformationClass">The type of information being queried.</param>
        /// <param name="outputBufferSize">The maximum number of bytes to be returned in OutputBuffer.</param>
        /// <param name="byteCount">The number of bytes stored in OutputBuffer.</param>
        /// <param name="outputBuffer">An array of bytes containing the file system information.</param>
        /// <returns>An NTSTATUS code that specifies the result.</returns>
        public MessageStatus QueryFileSystemInformation(FileSystemInfoClass fsInformationClass, uint outputBufferSize, out long byteCount, out byte[] outputBuffer)
        {
            outputBuffer = null;

            MessageStatus returnedStatus = this.transAdapter.QueryFileSystemInformation(
                outputBufferSize,
                (uint)fsInformationClass,
                out outputBuffer);

            byteCount = ((outputBuffer == null) ? 0 : outputBuffer.Length);

            return returnedStatus;
        }

        /// <summary>
        /// Implement QueryFileSystemInfo method
        /// </summary>
        /// <param name="fileInfoClass">The type of information being queried, as specified in [MS-FSCC] section 2.5.</param>
        /// <param name="outBufSmall">True: If OutputBufferSize is smaller than the requested size. </param>
        /// <param name="byteCount">The number of bytes stored in OutputBuffer.</param>
        /// <returns>An NTSTATUS code that specifies the result</returns>
        public MessageStatus QueryFileSystemInfo(
            FileSystemInfoClass fileInfoClass,
            OutputBufferSize outBufSmall,
            out FsInfoByteCount byteCount)
        {
            byteCount = FsInfoByteCount.Zero;
            byte[] outBuffer = null;
            uint maxOutputSize = 1;

            if (outBufSmall == OutputBufferSize.NotLessThan)
            {
                maxOutputSize = this.transBufferSize;
            }

            MessageStatus returnedStatus = this.transAdapter.QueryFileSystemInformation(
                maxOutputSize,
                (uint)fileInfoClass,
                out outBuffer);

            //If outBuffer is null, outBufferLength is 0
            int outBufferLength = ((outBuffer == null) ? 0 : outBuffer.Length);

            #region Set byteCount value
            if (returnedStatus == MessageStatus.SUCCESS)
            {
                switch (fileInfoClass)
                {
                    case FileSystemInfoClass.File_FsVolumeInformation:
                        {
                            try
                            {
                                FileFsVolumeInformation fileFsVolumeInformation = TypeMarshal.ToStruct<FileFsVolumeInformation>(outBuffer);
                                // If marshal OutBuffer succeed, then byteCount SHOULD be correct.
                                byteCount = FsInfoByteCount.FieldOffset_FILE_FS_VOLUME_INFORMATION_VolumeLabel_Add_BytesToCopy;
                            }
                            catch
                            {
                                byteCount = FsInfoByteCount.Unexpected_SIZE;
                            }
                            break;
                        }
                    case FileSystemInfoClass.File_FsSizeInformation:
                        {
                            FileFsSizeInformation fileFsSizeInformation = new FileFsSizeInformation();

                            if (outBufferLength == TypeMarshal.ToBytes<FileFsSizeInformation>(fileFsSizeInformation).Length)
                            {
                                byteCount = FsInfoByteCount.SizeOf_FILE_FS_SIZE_INFORMATION;
                            }
                            else
                            {
                                byteCount = FsInfoByteCount.Unexpected_SIZE;
                            }
                            break;
                        }
                    case FileSystemInfoClass.File_FsDevice_Information:
                        {
                            FileFsDeviceInformation fileFsDeviceInformation = new FileFsDeviceInformation();

                            if (outBufferLength == TypeMarshal.ToBytes<FileFsDeviceInformation>(fileFsDeviceInformation).Length)
                            {
                                byteCount = FsInfoByteCount.SizeOf_FILE_FS_DEVICE_INFORMATION;
                            }
                            else
                            {
                                byteCount = FsInfoByteCount.Unexpected_SIZE;
                            }
                            break;
                        }
                    case FileSystemInfoClass.File_FsAttribute_Information:
                        {
                            try
                            {
                                FileFsAttributeInformation fileFsAttributeInformation = TypeMarshal.ToStruct<FileFsAttributeInformation>(outBuffer);
                                // If marshal OutBuffer succeed, then byteCount SHOULD be correct.
                                byteCount = FsInfoByteCount.FieldOffset_FILE_FS_ATTRIBUTE_INFORMATION_FileSystemName_Add_BytesToCopy;
                            }
                            catch
                            {
                                byteCount = FsInfoByteCount.Unexpected_SIZE;
                            }
                            break;
                        }
                    case FileSystemInfoClass.File_FsControlInformation:
                        {
                            FileFsControlInformation fileFsControlInformation = new FileFsControlInformation();

                            if (outBufferLength == TypeMarshal.ToBytes<FileFsControlInformation>(fileFsControlInformation).Length)
                            {
                                byteCount = FsInfoByteCount.SizeOf_FILE_FS_CONTROL_INFORMATION;
                            }
                            else
                            {
                                byteCount = FsInfoByteCount.Unexpected_SIZE;
                            }
                            break;
                        }
                    case FileSystemInfoClass.File_FsFullSize_Information:
                        {
                            FileFsFullSizeInformation fileFsFullSizeInformation = new FileFsFullSizeInformation();

                            if (outBufferLength == TypeMarshal.ToBytes<FileFsFullSizeInformation>(fileFsFullSizeInformation).Length)
                            {
                                byteCount = FsInfoByteCount.SizeOf_FILE_FS_FULL_SIZE_INFORMATION;
                            }
                            else
                            {
                                byteCount = FsInfoByteCount.Unexpected_SIZE;
                            }
                            break;
                        }
                    case FileSystemInfoClass.File_FsObjectId_Information:
                        {
                            FileFsObjectIdInformation fileFsObjectIdInformation = new FileFsObjectIdInformation();
                            // ExtendedInfo is a 48-byte value containing extended information on the file system volume. 
                            // If no extended information has been written for this file system volume, the server  MUST return 48 bytes of 0x00 in this field.
                            // Set it as 48 bytes of 0x00 so that the structure can be marshalled.
                            fileFsObjectIdInformation.ExtendedInfo = new byte[48];

                            if (outBufferLength == TypeMarshal.ToBytes<FileFsObjectIdInformation>(fileFsObjectIdInformation).Length)
                            {
                                byteCount = FsInfoByteCount.SizeOf_FILE_FS_OBJECTID_INFORMATION;
                            }
                            else
                            {
                                byteCount = FsInfoByteCount.Unexpected_SIZE;
                            }
                            break;
                        }
                    case FileSystemInfoClass.File_FsSectorSizeInformation:
                        {
                            FILE_FS_SECTOR_SIZE_INFORMATION fileFsSectorSizeInformation = new FILE_FS_SECTOR_SIZE_INFORMATION();

                            if (outBufferLength == TypeMarshal.ToBytes<FILE_FS_SECTOR_SIZE_INFORMATION>(fileFsSectorSizeInformation).Length)
                            {
                                byteCount = FsInfoByteCount.SizeOf_FILE_FS_SECTOR_SIZE_INFORMATION;
                            }
                            else
                            {
                                byteCount = FsInfoByteCount.Unexpected_SIZE;
                            }
                            break;
                        }
                    default:
                        break;
                }
            }

            #endregion

            if (this.transport == Transport.SMB)
            {
                returnedStatus = SMB_TDIWorkaround.WorkAroundQueryFileSystemInfo(fileInfoClass, outBufSmall, returnedStatus, ref byteCount, site);
            }
            else
            {
                returnedStatus = SMB2_TDIWorkaround.WorkAroundQueryFileSystemInfo(fileInfoClass, outBufSmall, returnedStatus, ref byteCount, site);
            }

            return returnedStatus;
        }

        #region 3.1.5.12.5   FileFsAttributeInformation

        /// <summary>
        /// Implementation of Query FileFsAttributeInformation
        /// </summary>
        /// <param name="fileFsAttributeInfo">FileFsAttributeInformation Structure.</param>
        /// <returns>An NTSTATUS code that specifies the result.</returns>
        public MessageStatus QueryFileFsAttributeInformation(out FileFsAttributeInformation fileFsAttributeInfo)
        {
            fileFsAttributeInfo = new FileFsAttributeInformation();

            uint fileSystemNameLength = (uint)this.FileSystem.ToString().Length;
            fileFsAttributeInfo.FileSystemName = ASCIIEncoding.Unicode.GetBytes(this.FileSystem.ToString());
            fileFsAttributeInfo.FileSystemNameLength = (uint)fileFsAttributeInfo.FileSystemName.Length;

            long byteCount;
            uint outputBufferSize = (uint)TypeMarshal.ToBytes<FileFsAttributeInformation>(fileFsAttributeInfo).Length;
            byte[] buffer;

            MessageStatus status = QueryFileSystemInformation(FileSystemInfoClass.File_FsAttribute_Information, outputBufferSize, out byteCount, out buffer);
            fileFsAttributeInfo = TypeMarshal.ToStruct<FileFsAttributeInformation>(buffer);

            return status;
        }

        #endregion

        #endregion

        #region 3.1.5.13   Server Requests a Query of Security Information
        /// <summary>
        /// Implement QuerySecurityInfo method
        /// </summary>
        /// <param name="securityInformation">A SECURITY_INFORMATION data type, as defined in [MS-DTYP] section 2.4.7.</param>
        /// <param name="isByteCountGreater">True: If ByteCount is greater than OutputBufferSize</param>
        /// <param name="outputBuffer">Indicate output buffer</param>
        /// <returns>An NTSTATUS code that specifies the result</returns>
        public MessageStatus QuerySecurityInfo(
            SecurityInformation securityInformation,
            bool isByteCountGreater,
            out OutputBuffer outputBuffer)
        {
            byte[] outBuffer = null;
            outputBuffer = new OutputBuffer();

            //Initialize the maxOutPutSize
            uint maxOutPutSize = 0;
            if (!isByteCountGreater)
            {
                maxOutPutSize = uint.Parse(testConfig.GetProperty("BufferSize"));
            }
            else
            {
                maxOutPutSize = 1;
            }

            //Call the QuerySecurityInformation method
            MessageStatus returnedStatus = transAdapter.QuerySecurityInformation(maxOutPutSize, (uint)securityInformation, out outBuffer);

            if (this.transport == Transport.SMB2 || this.transport == Transport.SMB3)
            {
                returnedStatus = SMB2_TDIWorkaround.WorkaroundQuerySecurityInfo(isByteCountGreater, returnedStatus, site);
            }
            else
            {
                returnedStatus = SMB_TDIWorkaround.WorkaroundQuerySecurityInfo(isByteCountGreater, returnedStatus, site);
            }

            //Set the revision field
            if (returnedStatus == MessageStatus.SUCCESS)
            {
                outputBuffer.Revision = (uint)outBuffer[0];
            }
            else
            {
                outputBuffer.Revision = 0;
            }

            return returnedStatus;
        }
        #endregion

        #region 3.1.5.14   Server Requests Setting of File Information

        #region 3.1.5.14.1,8,12

        /// <summary>
        /// Implement SetFileAllocOrObjIdInfo method
        /// </summary>
        /// <param name="fileInfoClass">The type of information being applied, as specified in [MS-FSCC] section 2.4.</param>
        /// <param name="allocationSizeType">Indicates if InputBuffer.AllocationSize is greater than the maximum file size allowed by the object store.</param>
        /// <returns>An NTSTATUS code that specifies the result</returns>
        public MessageStatus SetFileAllocOrObjIdInfo(
            FileInfoClass fileInfoClass,
            AllocationSizeType allocationSizeType
            )
        {
            bool isInputBufAllocGreater = (allocationSizeType == AllocationSizeType.AllocationSizeIsGreaterThanMaximum);
            bool isReturnStatus = false;
            List<byte> byteList = new List<byte>();

            switch (fileInfoClass)
            {
                case FileInfoClass.FILE_ALLOCATION_INFORMATION:
                    byteList.AddRange(isInputBufAllocGreater ? BitConverter.GetBytes(ulong.MaxValue) : BitConverter.GetBytes((ulong)0));
                    break;

                case FileInfoClass.FILE_OBJECTID_INFORMATION:
                    FileObjectIdInformation_Type_1 fileInfo = new FileObjectIdInformation_Type_1();
                    byteList.AddRange(BitConverter.GetBytes(fileInfo.FileReferenceNumber));
                    byteList.AddRange(fileInfo.ObjectId.ToByteArray());
                    byteList.AddRange(fileInfo.BirthVolumeId.ToByteArray());
                    byteList.AddRange(fileInfo.BirthObjectId.ToByteArray());
                    byteList.AddRange(fileInfo.DomainId.ToByteArray());
                    break;

                case FileInfoClass.FILE_SFIO_RESERVE_INFORMATION:
                    FileSfioReserveInformation info2 = new FileSfioReserveInformation();
                    info2.Reserved = new byte[2];
                    byteList.AddRange(BitConverter.GetBytes(info2.RequestsPerPeriod));
                    byteList.AddRange(BitConverter.GetBytes(info2.Period));
                    byteList.AddRange(BitConverter.GetBytes(info2.RetryFailures));
                    byteList.AddRange(BitConverter.GetBytes(info2.Discardable));
                    byteList.AddRange(info2.Reserved);
                    byteList.AddRange(BitConverter.GetBytes(info2.RequestSize));
                    byteList.AddRange(BitConverter.GetBytes(info2.NumOutstandingRequests));
                    break;

                default:
                    throw (new NotImplementedException());
            }

            MessageStatus returnedStatus = transAdapter.SetFileInformation((uint)fileInfoClass, byteList.ToArray());

            //If no exception is thrown in SetFileInformation, server response status.
            isReturnStatus = true;
            this.VerifyServerSetFsInfo(isReturnStatus);
            if (this.transport == Transport.SMB)
            {
                returnedStatus = SMB_TDIWorkaround.WorkaroundMethodSetFileAllocOrObjIdInfo(fileInfoClass, isInputBufAllocGreater, returnedStatus, this.site);
            }
            return returnedStatus;
        }

        #endregion

        #region Basic Implementation of Setting File Information

        /// <summary>
        /// Implementation of Set File Information
        /// </summary>
        /// <param name="fileInformationClass">The type of information being applied.</param>
        /// <param name="inputBuffer">A buffer that contains the information to be applied to the object.</param>
        /// <returns></returns>
        public MessageStatus SetFileInformation(FileInfoClass fileInformationClass, byte[] inputBuffer)
        {
            return transAdapter.SetFileInformation((uint)fileInformationClass, inputBuffer);
        }

        #endregion

        #region 3.1.5.14.2 FileBasicInformation

        /// <summary>
        /// Implement SetFileBasicInfo method
        /// </summary>
        /// <param name="inputBufferSize">Indicate inputBuffer size</param>
        /// <param name="inputBufferTime">Indicate inputBuffer time</param>
        /// <returns>An NTSTATUS code that specifies the result</returns>
        public MessageStatus SetFileBasicInfo(
            InputBufferSize inputBufferSize,
            InputBufferTime inputBufferTime
            )
        {
            bool isReturnStatus = false;
            FILETIME fileTime = new FILETIME();
            fileTime.dwHighDateTime = 0xFFFFFFFE;
            fileTime.dwLowDateTime = 0xFFFFFFFE;
            FileBasicInformation fileBasicInfo = new FileBasicInformation();
            FakeFileBasicInformation fakeInfo = new FakeFileBasicInformation();
            List<byte> byteList = new List<byte>();

            switch (inputBufferSize)
            {
                case InputBufferSize.NotLessThan:
                    fileBasicInfo.CreationTime = (inputBufferTime == InputBufferTime.CreationTimeLessthanM1) ? fileTime : new FILETIME();
                    fileBasicInfo.LastAccessTime = (inputBufferTime == InputBufferTime.LastAccessTimeLessthanM1) ? fileTime : new FILETIME();
                    fileBasicInfo.LastWriteTime = (inputBufferTime == InputBufferTime.LastWriteTimeLessthanM1) ? fileTime : new FILETIME();
                    fileBasicInfo.ChangeTime = (inputBufferTime == InputBufferTime.ChangeTimeLessthanM1) ? fileTime : new FILETIME();

                    //When file attribute is archive, the value is 0x00000020
                    fileBasicInfo.FileAttributes = 0x00000020;

                    //This field defined in FSCC, and can be set to any value and MUST be ignored.
                    fileBasicInfo.Reserved = 0;

                    byteList.AddRange(BitConverter.GetBytes(fileBasicInfo.CreationTime.dwLowDateTime));
                    byteList.AddRange(BitConverter.GetBytes(fileBasicInfo.CreationTime.dwHighDateTime));
                    byteList.AddRange(BitConverter.GetBytes(fileBasicInfo.LastAccessTime.dwLowDateTime));
                    byteList.AddRange(BitConverter.GetBytes(fileBasicInfo.LastAccessTime.dwHighDateTime));
                    byteList.AddRange(BitConverter.GetBytes(fileBasicInfo.LastWriteTime.dwLowDateTime));
                    byteList.AddRange(BitConverter.GetBytes(fileBasicInfo.LastWriteTime.dwHighDateTime));
                    byteList.AddRange(BitConverter.GetBytes(fileBasicInfo.ChangeTime.dwLowDateTime));
                    byteList.AddRange(BitConverter.GetBytes(fileBasicInfo.ChangeTime.dwHighDateTime));
                    byteList.AddRange(BitConverter.GetBytes(fileBasicInfo.FileAttributes));
                    byteList.AddRange(BitConverter.GetBytes(fileBasicInfo.Reserved));
                    break;

                case InputBufferSize.LessThan:
                    fakeInfo.CreationTime = (inputBufferTime == InputBufferTime.CreationTimeLessthanM1) ? fileTime : new FILETIME();
                    fakeInfo.LastAccessTime = (inputBufferTime == InputBufferTime.LastAccessTimeLessthanM1) ? fileTime : new FILETIME();
                    fakeInfo.LastWriteTime = (inputBufferTime == InputBufferTime.LastWriteTimeLessthanM1) ? fileTime : new FILETIME();
                    fakeInfo.ChangeTime = (inputBufferTime == InputBufferTime.ChangeTimeLessthanM1) ? fileTime : new FILETIME();

                    //When file attribute is archive, the value is 0x00000020
                    fakeInfo.FileAttributes = 0x00000020;

                    byteList.AddRange(BitConverter.GetBytes(fakeInfo.CreationTime.dwLowDateTime));
                    byteList.AddRange(BitConverter.GetBytes(fakeInfo.CreationTime.dwHighDateTime));
                    byteList.AddRange(BitConverter.GetBytes(fakeInfo.LastAccessTime.dwLowDateTime));
                    byteList.AddRange(BitConverter.GetBytes(fakeInfo.LastAccessTime.dwHighDateTime));
                    byteList.AddRange(BitConverter.GetBytes(fakeInfo.LastWriteTime.dwLowDateTime));
                    byteList.AddRange(BitConverter.GetBytes(fakeInfo.LastWriteTime.dwHighDateTime));
                    byteList.AddRange(BitConverter.GetBytes(fakeInfo.ChangeTime.dwLowDateTime));
                    byteList.AddRange(BitConverter.GetBytes(fakeInfo.ChangeTime.dwHighDateTime));
                    byteList.AddRange(BitConverter.GetBytes(fakeInfo.FileAttributes));
                    break;

                default:
                    break;
            }

            MessageStatus returnedStatus = transAdapter.SetFileInformation((uint)FileInfoClass.FILE_BASIC_INFORMATION, byteList.ToArray());

            //If no exception is thrown in SetFileInformation, server will return response status.
            isReturnStatus = true;
            this.VerifyServerSetFsInfo(isReturnStatus);

            if (fileSystem == Adapter.FileSystem.FAT32 &&
                returnedStatus == MessageStatus.SUCCESS)
            {
                returnedStatus = FsaUtility.TransferExpectedResult<MessageStatus>(2875, MessageStatus.INVALID_PARAMETER, returnedStatus, site);
            }

            return returnedStatus;
        }

        #endregion

        #region 3.1.5.14.3 FileDispositionInformation

        /// <summary>
        /// Implement SetFileDispositionInfo method
        /// </summary>
        /// <param name="fileDispositionType">Indicates if the file is set to delete pending.</param>
        /// <returns>An NTSTATUS code that specifies the result</returns>
        public MessageStatus SetFileDispositionInfo(
            FileDispositionType fileDispositionType
            )
        {
            bool isReturnStatus = false;
            FileDispositionInformation fileInfo = new FileDispositionInformation();
            List<byte> byteList = new List<byte>();

            fileInfo.DeletePending = (byte)(fileDispositionType == FileDispositionType.IsDeletePending ? 1 : 0);
            byteList.AddRange(BitConverter.GetBytes(fileInfo.DeletePending));

            MessageStatus returnedStatus = transAdapter.SetFileInformation((uint)FileInfoClass.FILE_DISPOSITION_INFORMATION, byteList.ToArray());

            //If no exception is thrown in SetFileInformation, server response status.
            isReturnStatus = true;
            this.VerifyServerSetFsInfo(isReturnStatus);

            return returnedStatus;
        }

        #endregion

        #region 3.1.5.14.4 FileEndOfFileInformation

        /// <summary>
        /// Implement SetFileEndOfFileInfo method
        /// </summary>
        /// <param name="isEndOfFileGreatThanMaxSize">True: if InputBuffer.EndOfFile is greater than the maximum file size</param>
        /// <param name="isSizeEqualEndOfFile">True: if Open.Stream.Size is equal to InputBuffer.EndOfFile</param>
        /// <returns>An NTSTATUS code that specifies the result</returns>
        public MessageStatus SetFileEndOfFileInfo(
            bool isEndOfFileGreatThanMaxSize,
            bool isSizeEqualEndOfFile
            )
        {
            bool isReturnStatus = false;
            FileEndOfFileInformation fileInfo = new FileEndOfFileInformation();
            List<byte> byteList = new List<byte>();

            fileInfo.EndOfFile = isEndOfFileGreatThanMaxSize ? long.MaxValue : 0;
            byteList.AddRange(BitConverter.GetBytes(fileInfo.EndOfFile));

            MessageStatus returnedStatus = transAdapter.SetFileInformation((uint)FileInfoClass.FILE_ENDOFFILE_INFORMATION, byteList.ToArray());

            //If no exception is thrown in SetFileInformation, server response status.
            isReturnStatus = true;
            this.VerifyServerSetFsInfo(isReturnStatus);

            if (fileSystem == Adapter.FileSystem.FAT32 &&
                returnedStatus == MessageStatus.DISK_FULL)
            {
                returnedStatus = FsaUtility.TransferExpectedResult<MessageStatus>(2913, MessageStatus.INVALID_PARAMETER, returnedStatus, site);
            }

            return returnedStatus;
        }

        #endregion

        #region 3.1.5.14.5 FileFullEaInformation

        /// <summary>
        /// Implement SetFileFullEaInfo method
        /// </summary>
        /// <param name="eAValidate">Indicate Ea Information</param>
        /// <returns>An NTSTATUS code that specifies the result</returns>
        public MessageStatus SetFileFullEaInfo(
            EainInputBuffer eAValidate
            )
        {
            bool isReturnStatus = false;
            List<byte> byteList = new List<byte>();
            FileFullEaInformation fileInfo = new FileFullEaInformation();

            //Initail fileInfo structure according to FSCC 2.4.15
            fileInfo.EaNameLength = 1;
            fileInfo.NextEntryOffset = 0;
            fileInfo.Flags = FILE_FULL_EA_INFORMATION_FLAGS.NONE;
            fileInfo.EaValue = new byte[] { 0 };
            fileInfo.EaName = new byte[1];
            fileInfo.EaValueLength = 1;

            switch (eAValidate)
            {
                case EainInputBuffer.EaNameNotWellForm:
                    fileInfo.EaName = new byte[] { (byte)'+', (byte)'<' };
                    //Set EaNameLength to be 2, which is an invalid value
                    fileInfo.EaNameLength = 2;
                    break;

                case EainInputBuffer.EaFlagsInvalid:
                    //Set fileInfo.Flags to an invalid value. 255 is an invalid value.
                    fileInfo.Flags = (FILE_FULL_EA_INFORMATION_FLAGS)255;
                    break;

                default:
                    break;
            }

            byteList.AddRange(BitConverter.GetBytes(fileInfo.NextEntryOffset));
            byteList.AddRange(BitConverter.GetBytes((byte)fileInfo.Flags));
            byteList.AddRange(BitConverter.GetBytes(fileInfo.EaNameLength));
            byteList.AddRange(BitConverter.GetBytes(fileInfo.EaValueLength));
            byteList.AddRange(fileInfo.EaName);
            byteList.AddRange(fileInfo.EaValue);

            MessageStatus returnedStatus = transAdapter.SetFileInformation((uint)FileInfoClass.FILE_FULLEA_INFORMATION, byteList.ToArray());

            //If no exception is thrown in SetFileInformation, server response status.
            isReturnStatus = true;
            this.VerifyServerSetFsInfo(isReturnStatus);

            if (this.transport == Transport.SMB2 || this.transport == Transport.SMB3)
            {
                returnedStatus = SMB2_TDIWorkaround.WorkaroundSetFileFullEaInfo(eAValidate, returnedStatus, site);
            }
            else
            {
                returnedStatus = SMB_TDIWorkaround.WorkAroundSetFileFullEaInfo(eAValidate, returnedStatus, this.site);
            }
            return returnedStatus;
        }

        /// <summary>
        /// Implement of set FileFullEaInformation
        /// </summary>
        /// <param name="fileFullEaInformation">A structure of FileFullEaInformation</param>
        /// <returns>NTSTATUS code</returns>
        public MessageStatus SetFileFullEaInformation(FileFullEaInformation fileFullEaInformation)
        {
            if (this.transport == Transport.SMB)
            {
                SMB_FEA smbFea = new SMB_FEA();
                smbFea.ExtendedAttributeFlag = (byte)fileFullEaInformation.Flags;
                smbFea.AttributeNameLengthInBytes = (byte)fileFullEaInformation.EaNameLength;
                smbFea.ValueNameLengthInBytes = (byte)fileFullEaInformation.EaValueLength;
                smbFea.AttributeName = fileFullEaInformation.EaName;
                smbFea.ValueName = fileFullEaInformation.EaValue;

                uint feaLength = (uint)TypeMarshal.ToBytes<SMB_FEA>(smbFea).Length;

                SMB_FEA_LIST smbFeaList = new SMB_FEA_LIST();
                smbFeaList.SizeOfListInBytes = feaLength + 4;
                smbFeaList.FEAList = new SMB_FEA[] { smbFea };

                byte[] inputBuffer = TypeMarshal.ToBytes<SMB_FEA_LIST>(smbFeaList);
                bool isUsePassThroughtInfoLevel = this.transAdapter.IsUsePassThroughInfoLevelCode;
                //Use SMB specific information level,do not use Pass-through Information Level Codes
                this.transAdapter.IsUsePassThroughInfoLevelCode = false;
                MessageStatus status = transAdapter.SetFileInformation((uint)SetInformationLevel.SMB_INFO_SET_EAS, inputBuffer);

                //Change the setting back to previous value
                this.transAdapter.IsUsePassThroughInfoLevelCode = isUsePassThroughtInfoLevel;

                return status;
            }
            else
            {
                byte[] inputBuffer = TypeMarshal.ToBytes<FileFullEaInformation>(fileFullEaInformation);
                return transAdapter.SetFileInformation((uint)FileInfoClass.FILE_FULLEA_INFORMATION, inputBuffer);
            }
        }

        #endregion

        #region 3.1.5.14.6 FileLinkInformation

        /// <summary>
        /// Implement SetFileLinkInfo method
        /// </summary>
        /// <param name="inputNameInvalid">True: if InputBuffer.FileName is not valid as specified in [MS-FSCC] section 2.1.5</param>
        /// <param name="replaceIfExist">InputBuffer.ReplaceIfExists </param>
        /// <returns>An NTSTATUS code that specifies the result</returns>
        public MessageStatus SetFileLinkInfo(
            bool inputNameInvalid,
            bool replaceIfExist
            )
        {
            bool isReturnStatus = false;
            FileLinkInformation fileInfo = new FileLinkInformation();
            List<byte> byteList = new List<byte>();

            fileInfo.FileName = inputNameInvalid ? (new byte[] { (byte)'a', (byte)'$', (byte)'b', (byte)']' }) : (new byte[] { (byte)'a', (byte)'\\', (byte)'b', (byte)' ' });
            fileInfo.FileNameLength = (uint)fileInfo.FileName.Length;
            fileInfo.ReplaceIfExists = (byte)(replaceIfExist ? 1 : 0);
            //Assign 7 byte to Reserved according to FSCC 2.4.21
            fileInfo.Reserved = new byte[7];
            fileInfo.RootDirectory = FileLinkInformation_RootDirectory_Values.V1;

            byteList.AddRange(BitConverter.GetBytes(fileInfo.ReplaceIfExists));
            byteList.AddRange(fileInfo.Reserved);
            byteList.AddRange(BitConverter.GetBytes((ulong)fileInfo.RootDirectory));
            byteList.AddRange(BitConverter.GetBytes(fileInfo.FileNameLength));
            byteList.AddRange(fileInfo.FileName);

            MessageStatus returnedStatus = transAdapter.SetFileInformation((uint)FileInfoClass.FILE_LINK_INFORMATION, byteList.ToArray());

            //If no exception is thrown in SetFileInformation, server response status.
            isReturnStatus = true;
            this.VerifyServerSetFsInfo(isReturnStatus);

            if (this.transport == Transport.SMB2 || this.transport == Transport.SMB3)
            {
                returnedStatus = SMB2_TDIWorkaround.WorkaroundSetFileLinkInfo(inputNameInvalid, replaceIfExist, returnedStatus, site);
            }
            else
            {
                returnedStatus = SMB_TDIWorkaround.WorkAroundSetFileLinkInfo(inputNameInvalid, replaceIfExist, returnedStatus, site);
            }
            return returnedStatus;
        }

        /// <summary>
        /// Implementation of set FileLinkInformation
        /// </summary>
        /// <param name="fileLinkInformation">FILE_LINK_INFORMATION structure for SMB.</param>
        /// <returns></returns>
        public MessageStatus SetFileLinkInformation(FILE_LINK_INFORMATION_TYPE_SMB fileLinkInformation)
        {
            if (fileLinkInformation.Reserved == null || fileLinkInformation.Reserved.Length <= 0)
            {
                fileLinkInformation.Reserved = new byte[3];
            }
            byte[] inputBuffer = TypeMarshal.ToBytes<FILE_LINK_INFORMATION_TYPE_SMB>(fileLinkInformation);
            return SetFileInformation(FileInfoClass.FILE_LINK_INFORMATION, inputBuffer);
        }

        /// <summary>
        /// Implementation of set FileLinkInformation
        /// </summary>
        /// <param name="fileLinkInformation">FILE_LINK_INFORMATION structure for SMB2.</param>
        /// <returns></returns>
        public MessageStatus SetFileLinkInformation(FILE_LINK_INFORMATION_TYPE_SMB2 fileLinkInformation)
        {
            if (fileLinkInformation.Reserved == null || fileLinkInformation.Reserved.Length <= 0)
            {
                fileLinkInformation.Reserved = new byte[7];
            }
            byte[] inputBuffer = TypeMarshal.ToBytes<FILE_LINK_INFORMATION_TYPE_SMB2>(fileLinkInformation);

            return SetFileInformation(FileInfoClass.FILE_LINK_INFORMATION, inputBuffer);
        }

        #endregion

        #region 3.1.5.15.8   FileFsObjectIdInformation

        /// <summary>
        /// Implementation of Set FileFsObjectIdInformation
        /// </summary>
        /// <param name="inputBuffer">InputBuffer is a FILE_FS_OBJECTID_INFORMATION structure.</param>
        /// <returns></returns>
        public MessageStatus SetFileFsObjectIdInformation(FileFsObjectIdInformation inputBuffer)
        {
            byte[] buffer = TypeMarshal.ToBytes<FileFsObjectIdInformation>(inputBuffer);
            return transAdapter.SetFileSystemInformation((uint)FileSystemInfoClass.File_FsObjectId_Information, buffer);
        }

        #endregion

        #region 3.1.5.14.7 FileModeInformation
        /// <summary>
        /// Implement SetFileModeInfo method
        /// </summary>
        /// <param name="inputMode">InputBuffer.Mode </param>
        /// <returns>An NTSTATUS code that specifies the result</returns>
        public MessageStatus SetFileModeInfo(
            FileMode inputMode
            )
        {
            bool isReturnStatus = false;
            List<byte> byteList = new List<byte>();
            FILE_MODE_INFORMATION fileInfo = new FILE_MODE_INFORMATION();

            fileInfo.Mode = (Mode_Values)inputMode;
            byteList.AddRange(BitConverter.GetBytes((uint)fileInfo.Mode));

            MessageStatus returnedStatus = transAdapter.SetFileInformation((uint)FileInfoClass.FILE_MODE_INFORMATION, byteList.ToArray());

            //If no exception is thrown in SetFileInformation, server will return response status.
            isReturnStatus = true;
            this.VerifyServerSetFsInfo(isReturnStatus);

            return returnedStatus;
        }

        #endregion

        #region 3.1.5.14.9 FilePositionInformation

        /// <summary>
        /// Implement SetFilePositionInfo method
        /// </summary>
        /// <param name="inputBufferSize">Indicate inputBuffer size</param>
        /// <param name="currentByteOffset">Indicate InputBuffer.CurrentByteOffset</param>
        /// <returns>An NTSTATUS code that specifies the result</returns>
        public MessageStatus SetFilePositionInfo(
            InputBufferSize inputBufferSize,
            InputBufferCurrentByteOffset currentByteOffset
            )
        {
            bool isReturnStatus = false;
            List<byte> byteList = new List<byte>();
            FILE_POSITION_INFORMATION fileInfo = new FILE_POSITION_INFORMATION();

            fileInfo.CurrentByteOffset = new _LARGE_INTEGER();

            switch (currentByteOffset)
            {
                case InputBufferCurrentByteOffset.LessThanZero:
                    fileInfo.CurrentByteOffset.QuadPart = -1;
                    break;

                case InputBufferCurrentByteOffset.NotValid:
                    // 999 is an invalid number
                    fileInfo.CurrentByteOffset.QuadPart = 999;
                    break;

                default:
                    break;
            }

            switch (inputBufferSize)
            {
                case InputBufferSize.LessThan:
                    byteList.AddRange(BitConverter.GetBytes((Int32)(0)));
                    break;

                case InputBufferSize.NotLessThan:
                    byteList.AddRange(BitConverter.GetBytes(fileInfo.CurrentByteOffset.QuadPart));
                    break;

                default:
                    break;
            }

            MessageStatus returnedStatus = transAdapter.SetFileInformation((uint)FileInfoClass.FILE_POSITION_INFORMATION, byteList.ToArray());

            //If no exception is thrown in SetFileInformation, server response status.
            isReturnStatus = true;
            this.VerifyServerSetFsInfo(isReturnStatus);

            if (this.transport == Transport.SMB)
            {
                returnedStatus = SMB_TDIWorkaround.WorkAroundSetFilePositionInfo(inputBufferSize, currentByteOffset, returnedStatus, this.site);
            }
            else
            {
                returnedStatus = SMB2_TDIWorkaround.WorkAroundSetFilePositionInfo(inputBufferSize, currentByteOffset, returnedStatus, this.site);
            }
            return returnedStatus;
        }
        #endregion

        #region 3.1.5.14.11   FileRenameInformation

        /// <summary>
        /// Implement SetFileRenameInfo method
        /// </summary>
        /// <param name="inputBufferFileNameLength">InputBufferFileNameLength</param>
        /// <param name="inputBufferFileName">InputbBufferFileName</param>
        /// <param name="directoryVolumeType">Indicate if DestinationDirectory.Volume is equal to Open.File.Volume.</param>
        /// <param name="destinationDirectoryType">Indicate if DestinationDirectory is the same as Open.Link.ParentFile.</param>
        /// <param name="newLinkNameFormatType">Indicate if NewLinkName is case-sensitive</param>
        /// <param name="newLinkNameMatchType">Indicate if NewLinkName matched TargetLink.ShortName</param>
        /// <param name="replacementType">Indicate if replace target file if exists.</param>
        /// <param name="targetLinkDeleteType">Indicate if TargetLink is deleted or not.</param>
        /// <param name="oplockBreakStatusType">Indicate if there is an oplock to be broken</param>
        /// <param name="targetLinkFileOpenListType">Indicate if TargetLink.File.OpenList contains an Open with a Stream matching the current Stream.</param>
        /// <returns>An NTSTATUS code indicating the result of the operation.</returns>
        public MessageStatus SetFileRenameInfo(
            InputBufferFileNameLength inputBufferFileNameLength,
            InputBufferFileName inputBufferFileName,
            DirectoryVolumeType directoryVolumeType,
            DestinationDirectoryType destinationDirectoryType,
            NewLinkNameFormatType newLinkNameFormatType,
            NewLinkNameMatchType newLinkNameMatchType,
            ReplacementType replacementType,
            TargetLinkDeleteType targetLinkDeleteType,
            OplockBreakStatusType oplockBreakStatusType,
            TargetLinkFileOpenListType targetLinkFileOpenListType
            )
        {
            bool isReturnStatus = false;
            string randomFileName = this.ComposeRandomFileName();
            List<byte> byteList = new List<byte>();
            FileRenameInformation_SMB fileInfo = new FileRenameInformation_SMB();

            switch (inputBufferFileName)
            {
                case InputBufferFileName.StartWithBackSlash:
                    randomFileName = "\\" + randomFileName;
                    break;

                case InputBufferFileName.StartWithColon:
                    randomFileName = ":" + randomFileName;
                    break;

                case InputBufferFileName.ContainsInvalid:
                    randomFileName = randomFileName + "\\/:?<>+";
                    break;

                case InputBufferFileName.NotValid:
                    randomFileName = randomFileName + "\\/:?<>+";
                    break;

                default:
                    break;
            }

            switch (inputBufferFileNameLength)
            {
                case InputBufferFileNameLength.EqualTo_Zero:
                    fileInfo.FileNameLength = 0;
                    break;

                case InputBufferFileNameLength.Greater:
                    fileInfo.FileNameLength = (uint)(randomFileName.Length + 255);
                    break;

                case InputBufferFileNameLength.OddNumber:
                    fileInfo.FileNameLength = 7;
                    break;

                default:
                    break;
            }

            fileInfo.FileName = Encoding.Unicode.GetBytes(randomFileName);
            fileInfo.ReplaceIfExists = (byte)(replacementType == ReplacementType.ReplaceIfExists ? 1 : 0);
            fileInfo.Reserved = new byte[3];
            fileInfo.RootDirectory = RootDirectory_Values.V1;

            byteList.AddRange(BitConverter.GetBytes(fileInfo.ReplaceIfExists));
            byteList.AddRange(fileInfo.Reserved);
            byteList.AddRange(BitConverter.GetBytes((uint)fileInfo.RootDirectory));
            byteList.AddRange(BitConverter.GetBytes(fileInfo.FileNameLength));
            byteList.AddRange(fileInfo.FileName);

            MessageStatus returnedStatus = transAdapter.SetFileInformation((uint)FileInfoClass.FILE_RENAME_INFORMATION, byteList.ToArray());

            //If no exception is thrown in SetFileInformation, server response status.
            isReturnStatus = true;
            this.VerifyServerSetFsInfo(isReturnStatus);

            if (this.transport == Transport.SMB2 || this.transport == Transport.SMB3)
            {
                returnedStatus = SMB2_TDIWorkaround.WorkaroundSetFileRenameInfo(inputBufferFileNameLength, returnedStatus, site);
            }
            else
            {
                returnedStatus = SMB_TDIWorkaround.WorkaroundSetFileRenameInfo(inputBufferFileNameLength, returnedStatus, site);
            }
            return returnedStatus;
        }

        #endregion

        #region 3.1.5.14.11.1   Algorithm for Performing Stream Rename

        /// <summary>
        /// Implement StreamRename method
        /// </summary>
        /// <param name="newStreamNameFormat">The format of NewStreamName</param>
        /// <param name="streamTypeNameFormat">The format of StreamType</param>
        /// <param name="replacementType">Indicate if replace target file if exists.</param>
        /// <returns>An NTSTATUS code indicating the result of the operation</returns>
        public MessageStatus StreamRename(
            InputBufferFileName newStreamNameFormat,
            InputBufferFileName streamTypeNameFormat,
            ReplacementType replacementType
            )
        {
            bool replaceIfExists = (replacementType == ReplacementType.ReplaceIfExists);
            string streamNameString = this.ComposeRandomFileName();
            string streamTypeString = gStreamType == StreamType.DirectoryStream ? "$INDEX_ALLOCATION" : "$DATA";

            // The full name of a stream is the form below:
            // <filename>:<stream name>:<stream type>
            // 
            // For stream rename, the NewStreamName is as below format. (<filename> part is not needed.)
            // :<stream name>:<stream type>            
            string newStreamName = string.Empty;

            if (newStreamNameFormat == InputBufferFileName.EndWithColon)
            {
                // Last char is ":"
                newStreamName = string.Format(":{0}:{1}{2}", streamNameString, streamTypeString, ":");
            }
            else if (newStreamNameFormat == InputBufferFileName.ColonOccurMoreThanThreeTimes)
            {
                newStreamName = "::::"; // More than 3 ":".
            }
            else if (newStreamNameFormat == InputBufferFileName.ContainsInvalid)
            {
                // The characters \ / : are invalid
                newStreamName = string.Format(":{0}:{1}", streamNameString + @"\ / :", streamTypeString);
            }
            else if (newStreamNameFormat == InputBufferFileName.ContainsWildcard)
            {
                // The following set of characters MUST be treated as wildcards by the object store: " * < > ?
                newStreamName = string.Format(":{0}:{1}", streamNameString + @"* < > ?", streamTypeString);
            }
            else if ((newStreamNameFormat == InputBufferFileName.LengthZero) && (streamTypeNameFormat == InputBufferFileName.LengthZero))
            {
                newStreamName = string.Format(":{0}:{1}", string.Empty, string.Empty);
            }
            else if (newStreamNameFormat == InputBufferFileName.IsMore255Unicode)
            {
                streamNameString = this.ComposeRandomFileName(256);
                newStreamName = string.Format(":{0}:{1}", streamNameString, streamTypeString);
            }
            else if ((gStreamType == StreamType.DataStream) && (streamTypeNameFormat == InputBufferFileName.isData))
            {
                newStreamName = string.Format(":{0}:{1}", streamNameString, "$NotDATA");
            }
            else if ((gStreamType == StreamType.DirectoryStream) && (streamTypeNameFormat == InputBufferFileName.isIndexAllocation))
            {
                newStreamName = string.Format(":{0}:{1}", streamNameString, "$NotINDEX_ALLOCATION");
            }
            else if (gStreamType == StreamType.DirectoryStream)
            {
                newStreamName = string.Format(":{0}:{1}", streamNameString, streamTypeString);
            }
            else if (newStreamNameFormat == InputBufferFileName.IsCaseInsensitiveMatch && streamTypeNameFormat == InputBufferFileName.isData)
            {
                newStreamName = string.Format(":{0}:{1}", streamNameString.ToUpper(), streamTypeString);
            }
            else if (newStreamNameFormat == InputBufferFileName.LengthZero && streamTypeNameFormat == InputBufferFileName.isIndexAllocation)
            {
                newStreamName = string.Format(":{0}:{1}", string.Empty, "$INDEX_ALLOCATION");
            }
            else
            {
                newStreamName = string.Format(":{0}:{1}", streamNameString, streamTypeString);
            }

            return StreamRenameWithNewName(newStreamName, newStreamNameFormat, streamTypeNameFormat, replacementType);
        }

        /// <summary>
        /// Implement StreamRenameWithNewName method
        /// </summary>
        /// <param name="newStreamName">The new stream name in String format</param>
        /// <param name="newStreamNameFormat">The format of NewStreamName</param>
        /// <param name="streamTypeNameFormat">The format of StreamType</param>
        /// <param name="replacementType">Indicate if replace target file if exists.</param>
        /// <returns>An NTSTATUS code indicating the result of the operation</returns>
        public MessageStatus StreamRenameWithNewName(
            string newStreamName,
            InputBufferFileName newStreamNameFormat,
            InputBufferFileName streamTypeNameFormat,
            ReplacementType replacementType
            )
        {
            bool replaceIfExists = (replacementType == ReplacementType.ReplaceIfExists);
            bool isReturnStatus = false;

            MessageStatus returnedStatus = MessageStatus.SUCCESS;

            if (this.transport == Transport.SMB)
            {
                FileRenameInformation_SMB smbFileRenameInfo = new FileRenameInformation_SMB();
                smbFileRenameInfo.FileName = Encoding.Unicode.GetBytes(newStreamName);
                smbFileRenameInfo.FileNameLength = (uint)smbFileRenameInfo.FileName.Length;
                smbFileRenameInfo.ReplaceIfExists = (byte)(replaceIfExists ? 1 : 0);
                smbFileRenameInfo.Reserved = new byte[3];
                smbFileRenameInfo.RootDirectory = RootDirectory_Values.V1;

                byte[] smbFileRenameInfoBuffer = TypeMarshal.ToBytes<FileRenameInformation_SMB>(smbFileRenameInfo);

                returnedStatus = transAdapter.SetFileInformation((uint)FileInfoClass.FILE_RENAME_INFORMATION, smbFileRenameInfoBuffer);

                //If no exception is thrown in SetFileInformation, server response status.
                isReturnStatus = true;
                this.VerifyServerSetFsInfo(isReturnStatus);

                returnedStatus = SMB_TDIWorkaround.WrokAroundStreamRename(this.fileSystem, newStreamNameFormat, streamTypeNameFormat, replaceIfExists, returnedStatus, site);
            }
            else
            {
                FileRenameInformation_SMB2 smb2FileRenameInfo = new FileRenameInformation_SMB2();
                smb2FileRenameInfo.ReplaceIfExists = (byte)(replaceIfExists ? 1 : 0);
                smb2FileRenameInfo.Reserved = new byte[7];
                smb2FileRenameInfo.RootDirectory = FileRenameInformation_SMB2_RootDirectory_Values.V1;
                smb2FileRenameInfo.FileName = Encoding.Unicode.GetBytes(newStreamName);
                smb2FileRenameInfo.FileNameLength = (uint)smb2FileRenameInfo.FileName.Length;

                byte[] smb2FileRenameInfoBuffer = TypeMarshal.ToBytes<FileRenameInformation_SMB2>(smb2FileRenameInfo);

                returnedStatus = transAdapter.SetFileInformation((uint)FileInfoClass.FILE_RENAME_INFORMATION, smb2FileRenameInfoBuffer);

                //If no exception is thrown in SetFileInformation, server response status.
                isReturnStatus = true;
                this.VerifyServerSetFsInfo(isReturnStatus);

                returnedStatus = SMB2_TDIWorkaround.WorkaroundStreamRename(this.fileSystem, newStreamNameFormat, streamTypeNameFormat, replaceIfExists, returnedStatus, site);
            }

            return returnedStatus;
        }

        #endregion

        #region 3.1.5.14.13 FileShortNameInformation

        /// <summary>
        /// Implement SetFileShortNameInfo method
        /// </summary>
        /// <param name="inputBufferFileName">InputBuffer.FileName
        /// when inputFileName == "InvalidName": If InputBuffer.FileName is not a valid 8.3 name as described in [MS-FSCC] section </param>
        /// <returns>An NTSTATUS code that specifies the result</returns>
        public MessageStatus SetFileShortNameInfo(
            InputBufferFileName inputBufferFileName
            )
        {
            bool isReturnStatus = false;
            FileShortNameInformation fileInfo = new FileShortNameInformation();
            List<byte> byteList = new List<byte>();
            string inputFileName = this.ComposeRandomFileName();

            switch (inputBufferFileName)
            {
                case InputBufferFileName.StartWithBackSlash:
                    inputFileName = @"\" + inputFileName;
                    break;

                case InputBufferFileName.NotValid:
                    inputFileName = inputFileName + " " + inputFileName + ".." + inputFileName;
                    break;

                case InputBufferFileName.Empty:
                    inputFileName = String.Empty;
                    break;

                case InputBufferFileName.EqualLinkShortName:
                    //If InputBuffer.FileName equals Open.Link.ShortName, return STATUS_SUCCESS.
                    break;

                default:
                    break;
            }

            fileInfo.FileName = Encoding.Unicode.GetBytes(inputFileName);
            fileInfo.FileNameLength = (uint)ASCIIEncoding.Unicode.GetByteCount(inputFileName);

            byteList.AddRange(BitConverter.GetBytes(fileInfo.FileNameLength));
            byteList.AddRange(fileInfo.FileName);

            MessageStatus returnedstatus = transAdapter.SetFileInformation((uint)FileInfoClass.FILE_SHORTNAME_INFORMATION, byteList.ToArray());

            //If no exception is thrown in SetFileInformation, server response status.
            isReturnStatus = true;
            this.VerifyServerSetFsInfo(isReturnStatus);

            if (this.transport == Transport.SMB2 || this.transport == Transport.SMB3)
            {
                returnedstatus = SMB2_TDIWorkaround.WorkaroundSetFileShortNameInfo(inputBufferFileName, returnedstatus, site);
            }
            else
            {
                returnedstatus = SMB_TDIWorkaround.WorkArondSetFileShortNameInfo(inputBufferFileName, returnedstatus, site);
            }
            return returnedstatus;
        }



        #endregion

        #region 3.1.5.14.14 FileValidDataLengthInformation

        ///<summary>
        /// Implement SetFileValidDataLengthInfo method
        ///</summary>
        ///<param name="isStreamValidLengthGreater">True:If Open.Stream.ValidDataLength is greater than InputBuffer.ValidDataLength</param>
        ///<returns>An NTSTATUS code that specifies the result</returns>
        public MessageStatus SetFileValidDataLengthInfo(
            bool isStreamValidLengthGreater
            )
        {
            bool isReturnStatus = false;
            FileValidDataLengthInformation fileInfo = new FileValidDataLengthInformation();
            List<byte> byteList = new List<byte>();

            fileInfo.ValidDataLength = isStreamValidLengthGreater ? long.MaxValue : 0;
            byteList.AddRange(BitConverter.GetBytes(fileInfo.ValidDataLength));
            MessageStatus returnedStatus = transAdapter.SetFileInformation((uint)FileInfoClass.FILE_VALIDDATALENGTH_INFORMATION, byteList.ToArray());

            //If no exception is thrown in SetFileInformation, server response status.
            isReturnStatus = true;
            this.VerifyServerSetFsInfo(isReturnStatus);

            if (fileSystem == Adapter.FileSystem.FAT32)
            {
                returnedStatus = FsaUtility.TransferExpectedResult<MessageStatus>(3203, MessageStatus.PRIVILEGE_NOT_HELD, returnedStatus, site);
            }

            return returnedStatus;
        }
        #endregion

        #endregion

        #region 3.1.5.15   Server Requests Setting of File System Information

        /// <summary>
        /// Implementation of setting File System Information
        /// </summary>
        /// <param name="fileInfoClass">The type of information being applied.</param>
        /// <param name="buffer">A buffer that contains the volume information to be applied to the object.</param>
        /// <returns>An NTSTATUS code indicating the result of the operation.</returns>
        public MessageStatus SetFileSystemInformation(UInt32 fsInformationClass, byte[] buffer)
        {
            return transAdapter.SetFileSystemInformation((uint)fsInformationClass, buffer);
        }

        /// <summary>
        /// SetFileSysInfo for model
        /// </summary>
        /// <param name="fileInfoClass">The type of information being applied, as specified in [MS-FSCC] section 2.5.</param>
        /// <param name="inputBufferSize">Indicate input buffer size.</param>
        /// <returns></returns>
        public MessageStatus SetFileSysInfo(
            FileSystemInfoClass fileInfoClass,
            InputBufferSize inputBufferSize)
        {
            // To be implemented for Scenario15_SetFileSysInfo
            return MessageStatus.NOT_SUPPORTED;
        }
        #endregion

        #region 3.1.5.16   Server Requests Setting of Security Information

        ///<summary>
        /// Implement SetSecurityInfo method
        ///</summary>
        ///<param name="securityInformation">Indicate SecurityInformation</param>
        ///<param name="ownerSidEnum">Indicate InputBuffer.OwnerSid</param>
        ///<returns>An NTSTATUS code that specifies the result</returns>
        public MessageStatus SetSecurityInfo(
            SecurityInformation securityInformation,
            OwnerSid ownerSidEnum)
        {
            byte[] informationBuffer = null;
            RawSecurityDescriptor ReceivedSD = new RawSecurityDescriptor(" ");

            ReceivedSD.Owner = new SecurityIdentifier(testConfig.GetProperty("SDOwner"));
            if (ownerSidEnum == OwnerSid.OpenFileSecDesOwnerIsNull)
            {
                ReceivedSD.Owner = null;
            }

            informationBuffer = new byte[ReceivedSD.BinaryLength];

            //Read the data from index 0.
            ReceivedSD.GetBinaryForm(informationBuffer, 0);
            MessageStatus returnedStatus = transAdapter.SetSecurityInformation((uint)securityInformation, informationBuffer);

            //Workaround for the current issue
            returnedStatus = SMB2_TDIWorkaround.WorkaroundSetSecurityInfo(fileSystem, securityInformation, ownerSidEnum, returnedStatus, site);

            return returnedStatus;
        }
        #endregion

        #region 3.1.5.17   Server Requests an Oplock

        /// <summary>
        /// Implement Oplock method
        /// </summary>
        /// <param name="opType">Oplock type</param>
        /// <param name="openListContainStream">Open.File.OpenList contains more than one 
        /// Open whose Stream is the same as Open.Stream.</param>
        /// <param name="opLockLevel">Requested oplock level</param>
        /// <param name="isOpenStreamOplockEmpty">True : if Open.Steam.Oplock is empty</param>
        /// <param name="oplockState">The current state of the oplock, expressed as a combination of one or more flags</param>
        /// <param name="streamIsDeleted">TRUE : if stream is deleted.</param>
        /// <param name="isRHBreakQueueEmpty">True: if Open.Stream.Oplock.State.RHBreakQueue is empty</param>
        /// <param name="isOplockKeyEqual">True: if ThisOpen.OplockKey == Open.OplockKey</param>
        /// <param name="isOplockKeyEqualExclusive">False: if Open.OplockKey != 
        /// Open.Stream.Oplock.ExclusiveOpen.OplockKey</param>
        /// <param name="requestOplock">Request oplock</param>
        /// <param name="GrantingInAck">True: if this oplock is being requested as part of an oplock break acknowledgement, FALSE if not</param>
        /// <param name="keyEqualOnRHOplocks">True: if there is an Open on Open.Stream.Oplock.RHOplocks whose OplockKey is equal to Open.OplockKey</param>
        /// <param name="keyEqualOnRHBreakQueue">True: if there is an Open on Open.Stream.Oplock.RHBreakQueue whose OplockKey is equal to Open.OplockKey</param>
        /// <param name="keyEqualOnROplocks">True: if there is an Open ThisOpen on Open.Stream.Oplock.ROplocks whose OplockKey is equal to Open.OplockKey</param>
        /// <returns> An NTSTATUS code indicating the result of the operation</returns>
        public MessageStatus Oplock(
            OpType opType,
            bool openListContainStream,
            RequestedOplockLevel opLockLevel,
            bool isOpenStreamOplockEmpty,
            OplockState oplockState,
            bool streamIsDeleted,
            bool isRHBreakQueueEmpty,
            bool isOplockKeyEqual,
            bool isOplockKeyEqualExclusive,
            RequestedOplock requestOplock,
            bool GrantingInAck,
            bool keyEqualOnRHOplocks,
            bool keyEqualOnRHBreakQueue,
            bool keyEqualOnROplocks)
        {
            throw (new NotImplementedException());
        }

        #endregion

        #region 3.1.5.18   Server Acknowledges an Oplock Break

        /// <summary>
        /// Server Acknowledges an Oplock Break
        /// </summary>
        /// <param name="OpenStreamOplockEmpty">True: if Open.Stream.Oplock is empty</param>
        /// <param name="opType">Oplock type</param>
        /// <param name="level">Requested Oplock level</param>
        /// <param name="ExclusiveOpenEqual">True: if Open.Stream.Oplock.ExclusiveOpen is not equal to Open</param>
        /// <param name="oplockState">Oplock state</param>
        /// <param name="RHBreakQueueIsEmpty">True: if Open.Stream.Oplock.RHBreakQueue is empty</param>
        /// <param name="ThisContextOpenEqual">True: if ThisContext.Open equals Open</param>
        /// <param name="ThisContextBreakingToRead">False: if ThisContext.BreakingToRead is FALSE</param>
        /// <param name="OplockWaitListEmpty">False: if Open.Stream.Oplock.WaitList is not empty</param>
        /// <param name="StreamIsDeleted">True: if Open.Stream.IsDeleted is TRUE</param>
        /// <param name="GrantingInAck">This value is used in [MS-FSA] 3.1.5.17.2</param>
        /// <param name="keyEqualOnRHOplocks">This value is used in [MS-FSA] 3.1.5.17.2</param>
        /// <param name="keyEqualOnRHBreakQueue">This value is used in [MS-FSA] 3.1.5.17.2</param>
        /// <param name="keyEqualOnROplocks">This value is used in [MS-FSA] 3.1.5.17.2</param>
        /// <param name="requestOplock">This value is used in [MS-FSA] 3.1.5.17.2</param>
        /// <returns> An NTSTATUS code indicating the result of the operation</returns>
        public MessageStatus OplockBreakAcknowledge(
            bool OpenStreamOplockEmpty,
            OpType opType,
            RequestedOplockLevel level,
            bool ExclusiveOpenEqual,
            OplockState oplockState,
            bool RHBreakQueueIsEmpty,
            bool ThisContextOpenEqual,
            bool ThisContextBreakingToRead,
            bool OplockWaitListEmpty,
            bool StreamIsDeleted,
            bool GrantingInAck,
            bool keyEqualOnRHOplocks,
            bool keyEqualOnRHBreakQueue,
            bool keyEqualOnROplocks,
            RequestedOplock requestOplock
            )
        {
            throw (new NotImplementedException());
        }

        #endregion

        #region 3.1.5.19   Server Requests Canceling an Operation

        /// <summary>
        /// Implement CancelinganOperation method
        /// Server Requests Canceling an Operation
        /// this method was called by 3.1.5.7, and the scenario was in Scenario16_ByteRangeLock
        /// </summary>
        /// <param name="iorequest">An implementation-specific identifier that is unique for each 
        /// outstanding IO operation. See [MS-CIFS] section 3.3.5.51.</param>
        /// <returns>An NTSTATUS code that specifies the result</returns>
        public MessageStatus CancelinganOperation(IORequest iorequest)
        {
            throw (new NotImplementedException());
        }

        #endregion

        #region 3.1.5.20   Server Requests Querying Quota Information

        /// <summary>
        /// Implement QueryFileQuotaInformation method
        /// </summary>
        /// <param name="state">Sid list state</param>
        /// <param name="RestartScan">A boolean indicating whether the enumeration should be restarted.</param>
        /// <param name="isDirectoryNotRight">True if there is no match</param>
        /// <param name="isOutBufferSizeLess">True if OutputBufferSize is less than 
        /// sizeof( FILE_QUOTA_INFORMATION ) multiplied by the number of 
        /// elements in SidList</param>
        /// <returns>An NTSTATUS code that specifies the result</returns>
        public MessageStatus QueryFileQuotaInformation(
            SidListState state,
            bool isOutBufferSizeLess,
            bool RestartScan,
            bool isDirectoryNotRight)
        {
            string randomFile = this.ComposeRandomFileName();
            uint createAction = 0;
            bool EmptyPattern = false;

            if (state == SidListState.Empty)
            {
                EmptyPattern = true;
            }
            else
            {
                EmptyPattern = false;
            }

            MessageStatus returnedStatus = this.transAdapter.CreateFile(
                randomFile,
                (uint)FileAttribute.NORMAL,
                (uint)(FileAccess.GENERIC_READ | FileAccess.GENERIC_WRITE),
                (uint)(ShareAccess.FILE_SHARE_READ | ShareAccess.FILE_SHARE_WRITE),
                (uint)(CreateOptions.NON_DIRECTORY_FILE),
                (uint)(CreateDisposition.OPEN_IF),
                out createAction);

            if (state == SidListState.NotEmpty_NotMultipleofFour)
            {
                return MessageStatus.INVALID_PARAMETER;
            }

            if (state == SidListState.HasMoreThanOneEntry)
            {
                //If OutputBufferSize is less than sizeof( FILE_QUOTA_INFORMATION ) multiplied
                //by the number of elements in SidList
                if (isOutBufferSizeLess)
                {
                    return MessageStatus.BUFFER_TOO_SMALL;
                }
            }
            else if (state == SidListState.HasZeroorOneEntry || EmptyPattern)
            {
                if (isOutBufferSizeLess)
                {
                    return MessageStatus.BUFFER_TOO_SMALL;
                }

                //If RestartScan is FALSE and EmptyPattern is true and there is no match, 
                //the operation MUST be failed with STATUS_NO_MORE_FILES
                if (!RestartScan && EmptyPattern && isDirectoryNotRight)
                {
                    return MessageStatus.NO_MORE_FILES;
                }

                //The operation MUST fail with STATUS_NO_SUCH_FILE under any of 
                //the following conditions:
                //EmptyPattern is FALSE and there is no match
                if (!EmptyPattern && isDirectoryNotRight)
                {
                    return MessageStatus.NO_SUCH_FILE;
                }

                //EmptyPattern is true and RestartScan is true and there is no match
                if (EmptyPattern && RestartScan && isDirectoryNotRight)
                {
                    return MessageStatus.NO_SUCH_FILE;
                }
            }


            return returnedStatus;
        }

        /// <summary>
        /// Implementation of Query Quota Information
        /// </summary>
        /// <param name="smb2QueryQuotaInfo">SMB2_QUERY_QUOTA_INFO structure.</param>
        /// <param name="outputBufferSize">The maximum number of bytes to return in OutputBuffer.</param>
        /// <param name="byteCount">The number of bytes stored in OutputBuffer.</param>
        /// <param name="outputBuffer">An array of one or more FILE_QUOTA_INFORMATION structures.</param>
        /// <returns>An NTSTATUS code that specifies the result.</returns>
        public MessageStatus QueryFileQuotaInformation(SMB2_QUERY_QUOTA_INFO smb2QueryQuotaInfo, uint outputBufferSize, out long byteCount, out byte[] outputBuffer)
        {
            outputBuffer = null;
            byte[] inputBuffer = TypeMarshal.ToBytes<SMB2_QUERY_QUOTA_INFO>(smb2QueryQuotaInfo);

            MessageStatus returnedStatus = this.transAdapter.QueryQuotaInformation(
                outputBufferSize,
                (uint)FileInfoClass.FILE_QUOTA_INFORMATION,
                inputBuffer,
                out outputBuffer);

            byteCount = ((outputBuffer == null) ? 0 : outputBuffer.Length);

            return returnedStatus;
        }

        /// <summary>
        /// Implementation of Query Quota Information
        /// </summary>
        /// <param name="smbQueryQuotaInfo">SMB_QUERY_QUOTA_INFO structure.</param>
        /// <param name="outputBufferSize">The maximum number of bytes to return in OutputBuffer.</param>
        /// <param name="byteCount">The number of bytes stored in OutputBuffer.</param>
        /// <param name="outputBuffer">An array of one or more FILE_QUOTA_INFORMATION structures.</param>
        /// <returns>An NTSTATUS code that specifies the result.</returns>
        public MessageStatus QueryFileQuotaInformation(SMB_QUERY_QUOTA_INFO smbQueryQuotaInfo, uint outputBufferSize, out long byteCount, out byte[] outputBuffer)
        {
            outputBuffer = null;
            byte[] inputBuffer = TypeMarshal.ToBytes<SMB_QUERY_QUOTA_INFO>(smbQueryQuotaInfo);

            MessageStatus returnedStatus = this.transAdapter.QueryQuotaInformation(
                outputBufferSize,
                (uint)FileInfoClass.FILE_QUOTA_INFORMATION,
                inputBuffer,
                out outputBuffer);

            byteCount = ((outputBuffer == null) ? 0 : outputBuffer.Length);

            return returnedStatus;
        }

        #endregion

        #region 3.1.5.21   Server Requests Setting Quota Information

        /// <summary>
        /// Implementation of Set Quota Information
        /// </summary>
        /// <param name="fileQuotaInformation">FileQuotaInformation structure.</param>
        /// <returns>An NTSTATUS code that specifies the result.</returns>
        public MessageStatus SetFileQuotaInformation(FILE_QUOTA_INFORMATION fileQuotaInformation)
        {
            byte[] inputBuffer = TypeMarshal.ToBytes<FILE_QUOTA_INFORMATION>(fileQuotaInformation);

            MessageStatus returnedStatus = this.transAdapter.SetQuotaInformation(
                (uint)FileInfoClass.FILE_QUOTA_INFORMATION,
                inputBuffer);

            return returnedStatus;
        }

        #endregion

        #endregion

        #region Utility methods

        /// <summary>
        /// Create a random file name a random string of 8 letters.
        /// </summary>
        /// <returns>>A file name with a random string of 8 letters.</returns>
        protected string ComposeRandomFileName()
        {
            return ComposeRandomFileName(8);
        }

        /// <summary>
        /// Create a random file name
        /// </summary>
        /// <param name="fileNameLength">The length of the file name.</param>
        /// <param name="extension">File extension to apend to the end of the filename.</param>
        /// <param name="opt">Directory will be added to test directory list, else, will be added to test file list for cleanup</param>
        /// <param name="addtoList">True for add to the testfiles.</param>        /// 
        /// <returns>A file name with a random string of the given length.</returns>
        public string ComposeRandomFileName(int fileNameLength,  string extension = "", CreateOptions opt = CreateOptions.DIRECTORY_FILE,  bool addToList = true)
        {
            int randomNumber = 0;
            char fileNameLetter = ' ';
            string randomFileName = null;

            for (int i = 0; i < fileNameLength; i++)
            {
                //Create a random fileNameLetter from 'a' to 'z'by range 1 to 52
                lock (randomRange)
                {
                    randomNumber = randomRange.Next(1, 52);
                }
                fileNameLetter = (char)(97 + randomNumber % 26);
                randomFileName = randomFileName + fileNameLetter.ToString() ; ;
            }

            randomFileName = randomFileName + extension;
                
            if (addToList)
            {
                AddTestFileName(opt, randomFileName);
            }
            return randomFileName;
        }       
        /// <summary>
        /// Get SUT platformType.
        /// </summary>
        /// <param name="platformType">SUT platformType</param>
        public void GetOSInfo(out PlatformType platformType)
        {
            platformType = (this.isWindows ? PlatformType.Windows : PlatformType.NoneWindows);
        }

        /// <summary>
        /// Get the system config information.
        /// </summary>
        /// <param name="securityContext">The SecurityContext of the user set.</param>
        public void GetSystemConfig(out SSecurityContext securityContext)
        {
            securityContext = new SSecurityContext();
            securityContext.privilegeSet = PrivilegeSet.SeRestorePrivilege;
        }

        /// <summary>
        ///  Get file volum size.
        /// </summary>
        /// <param name="securityContext">The SecurityContext of the user set.</param>
        public void GetFileVolumSize(out long openFileVolumeSize)
        {
            openFileVolumeSize = int.Parse(testConfig.GetProperty("OpenFileVolumeSize"));
        }

        /// <summary> 
        /// To make sure whether Quotas is supported.
        /// </summary>
        /// <param name="isQuotasSupported">True: if Quotas is supported</param>
        public void GetIFQuotasSupported(out bool isQuotasSupported)
        {
            isQuotasSupported = this.isQuotaSupported;
        }

        /// <summary> 
        /// To make sure whether ObjectIDs is supported.
        /// </summary>
        /// <param name="isObjectIDsSupported">True: if ObjectIDs is supported</param>
        public void GetIFObjectIDsSupported(out bool isObjectIDsSupported)
        {
            isObjectIDsSupported = this.isObjectIDsSupported;
        }

        /// <summary>
        /// To make sure whether implement object functionality
        /// Conclude the file system functionality by the type of the file system.
        /// </summary>
        /// <param name="isImplemented">True: if Object is functionality</param>
        public void GetObjectFunctionality(out bool isImplemented)
        {
            isImplemented = (fileSystem == Adapter.FileSystem.FAT32) ? false : true;
        }

        /// <summary>
        /// To make sure whether open has manage.vol.privilege
        /// </summary>
        /// <param name="isSupported">True: if open has manage.vol.privilege.</param>
        public void GetopenHasManageVolPrivilege(out bool isSupported)
        {
            isSupported = bool.Parse(testConfig.GetProperty("IsOpenHasManageVolPrivilege"));
        }

        /// <summary>
        /// To make sure whether ObjectIDs is supported
        /// </summary>
        /// <param name="isSupported">True: if ObjectIDs is supported</param>
        public void GetObjectIDsSupported(out bool isSupported)
        {
            isSupported = this.isObjectIDsSupported;
        }

        /// <summary>
        /// To make sure whether reparse points is supported
        /// </summary>
        /// <param name="isSupported">True: if ReparsePoints is supported</param>
        public void GetReparsePointsSupported(out bool isSupported)
        {
            isSupported = this.isReparsePointSupported;
        }

        /// <summary>
        /// Get is restore has access
        /// </summary>
        /// <param name="isHas">True: if Restore has access.</param>
        public void GetRestoreAccess(out bool isHas)
        {
            isHas = bool.Parse(testConfig.GetProperty("IsRestoreAccess"));
        }

        /// <summary>
        /// To make sure whether is administrator
        /// </summary>
        /// <param name="isGet">True: if administrator is got</param>
        public void GetAdministrator(out bool isGet)
        {
            isGet = bool.Parse(testConfig.GetProperty("IsAdministrator"));
        }

        /// <summary>
        /// To make sure whether open generate shortname
        /// </summary>
        /// <param name="GenerateShortNames">True: if Open generate shortnames</param>
        public void GetOpenGenerateShortNames(out bool GenerateShortNames)
        {
            GenerateShortNames = bool.Parse(testConfig.GetProperty("IsShortNameEnableOnVolume"));
        }

        /// <summary>
        /// To make sure whether Open.File.OpenList contains more than one Open on open stream.
        /// defined in 3.1.5.17
        /// </summary>
        /// <param name="isMoreThanOneOpenContained">True: if Open.ListContains</param>
        public void GetIsOpenListContains(out bool isMoreThanOneOpenContained)
        {
            isMoreThanOneOpenContained = bool.Parse(testConfig.GetProperty("IsMoreThanOneOpenContained"));
        }

        /// <summary>
        /// To make sure whether link is found
        /// </summary>
        /// <param name="IsLinkFound">True: if Link is Found</param>
        public void GetIsLinkFound(out bool IsLinkFound)
        {
            IsLinkFound = bool.Parse(testConfig.GetProperty("IsLinkFound"));
        }

        /// <summary>
        /// Check 507 requirement if implement
        /// </summary>
        /// <param name="flag">Is r507 implemented</param>
        public void CheckIsR507Implemented(out bool flag)
        {
            flag = false;
        }

        /// <summary>
        /// Check 405 requirement if implement
        /// </summary>
        /// <param name="flag">Is r405 implemented</param>
        public void CheckIsR405Implemented(out bool flag)
        {
            flag = false;
        }

        /// <summary>
        /// To get whether Query FileFsControlInformation is implemented.
        /// </summary>
        /// <param name="isImplemented">True: if this functionality is implemented by the object store.</param>
        public void GetIfImplementQueryFileFsControlInformation(out bool isImplemented)
        {
            isImplemented = this.isQueryFileFsControlInformationSupported;
        }

        /// <summary>
        /// To get whether Query FileFsObjectIdInformation is implemented.
        /// </summary>
        /// <param name="isImplemented">True: if this functionality is implemented by the object store.</param>
        public void GetIfImplementQueryFileFsObjectIdInformation(out bool isImplemented)
        {
            isImplemented = this.isQueryFileFsObjectIdInformationSupported;
        }

        /// <summary>
        /// To get whether Open.File.Volume is read only.
        /// </summary>
        /// <param name="isReadOnly"></param>
        public void GetIfOpenFileVolumeIsReadOnly(out bool isReadOnly)
        {
            isReadOnly = this.isVolumeReadonly;
        }

        /// <summary>
        /// To get whether Open.File equals to Root Directory.
        /// </summary>
        /// <param name="isEqualRootDirectory"></param>
        public void GetIfOpenFileEqualRootDirectory(out bool isEqualRootDirectory)
        {
            isEqualRootDirectory = (this.shareName == this.rootDirectory);
        }

        /// <summary>        
        /// Get if Query FileObjectIdInformation is implemented        
        /// </summary>
        /// <param name="isImplemented">true: If the object store implements this functionality.</param>        
        public void GetIfImplementQueryFileObjectIdInformation(out bool isImplemented)
        {
            isImplemented = this.isQueryFileObjectIdInformationSupported;
        }

        /// <summary>
        /// Get if Query FileReparsePointInformation is implemented
        /// </summary>
        /// <param name="isImplemented">true: If the object store implements this functionality.</param>
        public void GetIfImplementQueryFileReparsePointInformation(out bool isImplemented)
        {
            isImplemented = this.isQueryFileReparsePointInformationSupported;
        }

        /// <summary>
        /// Get if Query FileQuotaInformation is implemented
        /// </summary>
        /// <param name="isImplemented">true: If the object store implements this functionality.</param>
        public void GetIfImplementQueryFileQuotaInformation(out bool isImplemented)
        {
            isImplemented = this.isQueryFileQuotaInformationSupported;

        }

        /// <summary>
        /// Get if ObjectId related IoCtl requests are implemented
        /// </summary>
        /// <param name="isImplemented">true: If the object store implements this functionality.</param>
        public void GetIfImplementObjectIdIoCtlRequest(out bool isImplemented)
        {
            isImplemented = this.isObjectIdIoCtlRequestSupported;
        }

        /// <summary>
        /// Get if Open.File.Volume is NTFS file system.
        /// </summary>
        /// <param name="isNtfsFileSystem">true: if it is NTFS File System.</param>
        public void GetIfNtfsFileSystem(out bool isNtfsFileSystem)
        {
            isNtfsFileSystem = (this.fileSystem == Adapter.FileSystem.NTFS);
        }

        /// <summary>
        /// Get If Open Has Manage Volume Access. 
        /// </summary>
        /// <param name="isHasManageVolumeAccess">true: if open has manage volume access.</param>
        public void GetIfOpenHasManageVolumeAccess(out bool isHasManageVolumeAccess)
        {
            isHasManageVolumeAccess = this.isOpenHasManageVolumeAccessSupported;
        }

        /// <summary>
        /// Get if Stream Rename is supported.
        /// </summary>
        /// <param name="isSupported">true: if stream rename is supported.</param>
        public void GetIfStreamRenameIsSupported(out bool isSupported)
        {
            isSupported = this.isStreamRenameSupported;
        }
        #endregion

        #region Structures for Negative Testing

        // Summary:
        //     This information class is used to query or set file information. The Fake FILE_BASIC_INFORMATION
        //     data element is as follows.
        //Disable warning CA1304 because it will affect the implementation of Adapter if do any changes about maintainability.
        [SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible")]
        public struct FakeFileBasicInformation
        {
            // Summary:
            //     A 64-bit signed integer that contains the last time the file was changed
            //     in the format of a FILETIME structure. A valid time for this field is an
            //     integer greater than 0. When setting file attributes, a value of 0 indicates
            //     to the server that it MUST NOT change this attribute. When setting file attributes,
            //     a value of -1 indicates to the server that it MUST NOT change this attribute
            //     for all subsequent operations on the same file handle. This field MUST NOT
            //     be set to a value less than -1. The file system updates the values of the
            //     LastAccessTime, LastWriteTime, and ChangeTime members as appropriate after
            //     an I/O operation is performed on a file. However, a driver or application
            //     can request that the file system not update one or more of these members
            //     for I/O operations that are performed on the caller's file handle by setting
            //     the appropriate members to -1. The caller can set one, all, or any other
            //     combination of these three members to -1. Only the members that are set to
            //     -1 will be unaffected by I/O operations on the file handle; the other members
            //     will be updated as appropriate. This behavior is consistent across all file
            //     system types. Note that even though -1 can be used with the CreationTime
            //     field, it has no effect since file creation time is never updated in response
            //     to file system calls such as read and write.
            public FILETIME ChangeTime;
            //
            // Summary:
            //     A 64-bit signed integer that contains the time when the file was created.
            //     All dates and times in this message are in absolute system-time format, which
            //     is represented as a FILETIME structure. A valid time for this field is an
            //     integer greater than 0. When setting file attributes, a value of 0 indicates
            //     to the server that it MUST NOT change this attribute. When setting file attributes,
            //     a value of -1 indicates to the server that it MUST NOT change this attribute
            //     for all subsequent operations on the same file handle. This field MUST NOT
            //     be set to a value less than -1.
            public FILETIME CreationTime;
            //
            // Summary:
            //     A 32-bit unsigned integer that contains the file attributes. Valid file attributes
            //     are specified in section .
            public uint FileAttributes;
            //
            // Summary:
            //     A 64-bit signed integer that contains the last time the file was accessed
            //     in the format of a FILETIME structure. A valid time for this field is an
            //     integer greater than 0. When setting file attributes, a value of 0 indicates
            //     to the server that it MUST NOT change this attribute. When setting file attributes,
            //     a value of -1 indicates to the server that it MUST NOT change this attribute
            //     for all subsequent operations on the same file handle. This field MUST NOT
            //     be set to a value less than -1. The file system updates the values of the
            //     LastAccessTime, LastWriteTime, and ChangeTime members as appropriate after
            //     an I/O operation is performed on a file. However, a driver or application
            //     can request that the file system not update one or more of these members
            //     for I/O operations that are performed on the caller's file handle by setting
            //     the appropriate members to -1. The caller can set one, all, or any other
            //     combination of these three members to -1. Only the members that are set to
            //     -1 will be unaffected by I/O operations on the file handle; the other members
            //     will be updated as appropriate. This behavior is consistent across all file
            //     system types. Note that even though -1 can be used with the CreationTime
            //     field, it has no effect since file creation time is never updated in response
            //     to file system calls such as read and write.
            public FILETIME LastAccessTime;
            //
            // Summary:
            //     A 64-bit signed integer that contains the last time information was written
            //     to the file in the format of a FILETIME structure. A valid time for this
            //     field is an integer greater than 0. When setting file attributes, a value
            //     of 0 indicates to the server that it MUST NOT change this attribute. When
            //     setting file attributes, a value of -1 indicates to the server that it MUST
            //     NOT change this attribute for all subsequent operations on the same file
            //     handle. This field MUST NOT be set to a value less than -1. The file system
            //     updates the values of the LastAccessTime, LastWriteTime, and ChangeTime members
            //     as appropriate after an I/O operation is performed on a file. However, a
            //     driver or application can request that the file system not update one or
            //     more of these members for I/O operations that are performed on the caller's
            //     file handle by setting the appropriate members to -1. The caller can set
            //     one, all, or any other combination of these three members to -1. Only the
            //     members that are set to -1 will be unaffected by I/O operations on the file
            //     handle; the other members will be updated as appropriate. This behavior is
            //     consistent across all file system types. Note that even though -1 can be
            //     used with the CreationTime field, it has no effect since file creation time
            //     is never updated in response to file system calls such as read and write.
            public FILETIME LastWriteTime;
        }

        #endregion

        #region Assert utility

        /// <summary>
        /// Asserts two values are equal.
        /// </summary>
        /// <typeparam name="T">Type of values.</typeparam>
        /// <param name="manager">The test manager.</param>
        /// <param name="expected">The expected value.</param>
        /// <param name="actual">The actual value.</param>
        /// <param name="context">The description of the context under which both values are compared.</param>
        public void AssertAreEqual<T>(ITestManager manager, T expected, T actual, string context)
        {
            TestManagerHelpers.AssertAreEqual(manager, expected, actual, context);
        }

        /// <summary>
        /// Asserts two values are equal.
        /// </summary>
        /// <typeparam name="T">Type of values.</typeparam>
        /// <param name="manager">The test manager.</param>
        /// <param name="expected">The expected value.</param>
        /// <param name="actual">The actual value.</param>
        /// <param name="context">The description of the context under which both values are compared.</param>
        public void AssertAreEqual(ITestManager manager, MessageStatus expected, MessageStatus actual, string context)
        {
            if (this.isErrorCodeMappingRequired)
            {
                TestManagerHelpers.AssertAreEqual(manager, expected, actual, context);
            }
            else
            {
                string comments = string.Format("expected '{0}', actual '{1}' which is not equal to STATUS_SUCCESS. ({2})", expected.ToString(), actual.ToString(), context);
                this.site.Assert.AreNotEqual(MessageStatus.SUCCESS, actual, comments);
            }
        }

        /// <summary>
        /// Assert fail if the status is not STATUS_SUCCESS.
        /// </summary>
        /// <param name="status">The actual status value.</param>
        /// <param name="context">The description of the context for failure information.</param>
        public void AssertIfNotSuccess(MessageStatus status, string context)
        {
            //Write log for failure only
            if (status != MessageStatus.SUCCESS)
            {
                this.site.Assert.AreEqual(MessageStatus.SUCCESS, status, context);
            }
        }

        #endregion

        #region Logging Utility
        /// <summary>
        /// Add test case description to log
        /// </summary>
        /// <param name="testSite">ITestSite.</param>
        public void LogTestCaseDescription(ITestSite testSite)
        {
            var testcase = (string)testSite.TestProperties["CurrentTestCaseName"];
            int lastDotIndex = testcase.LastIndexOf('.');
            string typeName = testcase.Substring(0, lastDotIndex);
            string methodName = testcase.Substring(lastDotIndex + 1);

            Assembly testcaseAssembly = Assembly.LoadFrom(testSite.TestAssemblyName + ".dll");
            var type = testcaseAssembly.GetType(typeName);
            if (type == null)
            {
                testSite.Assert.Fail(String.Format("Test case type name {0} does not exist in test case assembly {1}.", typeName, testcaseAssembly.FullName));
            }
            else
            {
                var method = type.GetMethod(methodName);
                var attributes = method.GetCustomAttributes(typeof(DescriptionAttribute), false);
                if (attributes == null)
                {
                    testSite.Assert.Fail("No description is provided for this case.");
                }
                else
                {
                    foreach (DescriptionAttribute attribute in attributes)
                    {
                        testSite.Log.Add(LogEntryKind.Comment, attribute.Description);
                    }
                }
            }

        }
        #endregion
    }
}