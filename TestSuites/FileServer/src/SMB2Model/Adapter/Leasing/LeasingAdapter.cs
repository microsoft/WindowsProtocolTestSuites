// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;
using Microsoft.Protocols.TestTools.StackSdk.Security.Sspi;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Microsoft.Protocols.TestSuites.FileSharing.SMB2Model.Adapter.Leasing
{
    /// <summary>
    /// SMB2 client related attributes and operations which are used in leasing adapter.
    /// </summary>
    public class LeasingClientInfo
    {
        /// <summary>
        /// SMB2 client.
        /// </summary>
        public Smb2FunctionalClient Client;
        /// <summary>
        /// The maximum amount of time in milliseconds to wait for the operation to complete. 
        /// </summary>
        public TimeSpan Timeout;
        /// <summary>
        /// The identifier of the client.
        /// </summary>
        public Guid ClientGuid;
        /// <summary>
        /// The flags included in the SMB2 header.
        /// </summary>
        public Packet_Header_Flags_Values Flags;
        /// <summary>
        /// Dialect which is negotiated with the server.
        /// </summary>
        public DialectRevision Dialect;
        /// <summary>
        /// The id of next message.
        /// </summary>
        public ulong MessageId;
        /// <summary>
        /// Credits granted by the server.
        /// </summary>
        public ushort GrantedCredit;
        /// <summary>
        /// The first 16 bytes of the cryptographic key for this authenticated context.
        /// </summary>
        public byte[] SessionKey;
        /// <summary>
        /// GSS token.
        /// </summary>
        public byte[] ServerGssToken;
        /// <summary>
        /// Uniquely identifies the tree connect for the command.
        /// </summary>
        public uint TreeId;
        /// <summary>
        /// Uniquely identifies the established session for the command.
        /// </summary>
        public ulong SessionId;
        /// <summary>
        /// The parent directory of the target on the server used to test.
        /// </summary>
        public string ParentDirectory;
        /// <summary>
        /// The target on the server used to test.
        /// </summary>
        public string File;
        /// <summary>
        /// The FileId of the target on the server used to test.
        /// </summary>
        public FILEID FileId;
        /// <summary>
        /// Indicates whether the target used to test is a file or a directory.
        /// </summary>
        public bool IsDirectory;
        /// <summary>
        /// An array of LockCount (SMB2_LOCK_ELEMENT) structures that define the ranges to be locked or unlocked.
        /// </summary>
        public LOCK_ELEMENT[] Locks;
        /// <summary>
        /// For SMB 2.1 and 3.0, indicates a value that identifies a lock or unlock request uniquely across all lock or unlock requests that are sent on the same file.
        /// </summary>
        public uint LockSequence;
        /// <summary>
        /// Current lease state.
        /// </summary>
        public LeaseStateValues LeaseState;
        /// <summary>
        /// Create context for leasing.
        /// </summary>
        public Smb2CreateContextRequest[] CreateContexts;

        /// <summary>
        /// Message id of the last operation.
        /// </summary>
        private ulong OperationMessageId;

        /// <summary>
        /// test configuration.
        /// </summary>
        private TestConfigBase testConfig;

        /// <summary>
        /// test site
        /// </summary>
        private ITestSite Site;

        /// <summary>
        /// Indicates whether the client has executed TreeConnect or not.
        /// </summary>
        public bool IsInitialized
        {
            get
            {
                unchecked
                {
                    return TreeId != (uint)-1;
                }
            }
        }

        /// <summary>
        /// Indicates whether the client has executed Create or not.
        /// </summary>
        public bool IsOpened
        {
            get
            {
                return !FileId.Equals(FILEID.Zero);
            }
        }

        /// <summary>
        /// Get/Set the message id of last operation.
        /// </summary>
        public ulong LastOperationMessageId
        {
            set
            {
                OperationMessageId = value;
            }
            get
            {
                ulong id = OperationMessageId;
                OperationMessageId = 0;
                return id;
            }
        }

        /// <summary>
        /// Initialize all fields to the default values.
        /// </summary>
        private void Initialize()
        {
            Flags = testConfig.SendSignedRequest ? Packet_Header_Flags_Values.FLAGS_SIGNED : Packet_Header_Flags_Values.NONE;
            Dialect = DialectRevision.Smb2Unknown;
            MessageId = 0;
            GrantedCredit = 0;
            SessionKey = null;
            ServerGssToken = null;
            unchecked
            {
                TreeId = (uint)-1;
            }
            SessionId = 0;

            ParentDirectory = null;
            File = null;
            FileId = FILEID.Zero;
            IsDirectory = false;

            Locks = null;
            LockSequence = 0;
            LeaseState = LeaseStateValues.SMB2_LEASE_NONE;
            CreateContexts = null;
            OperationMessageId = 0;

            Client = new Smb2FunctionalClient(Timeout, testConfig, Site);
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="timeout">The maximum amount of time in milliseconds to wait for the operation to complete. </param>
        public LeasingClientInfo(TimeSpan timeout, TestConfigBase testConfig, ITestSite site)
        {
            Timeout = timeout;
            this.testConfig = testConfig;
            ClientGuid = Guid.NewGuid();
            Site = site;
            Initialize();
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="timeout">The maximum amount of time in milliseconds to wait for the operation to complete. </param>
        /// <param name="clientGuid">The identifier of the client.</param>
        public LeasingClientInfo(TimeSpan timeout, TestConfigBase testConfig, Guid clientGuid, ITestSite site)
        {
            Timeout = timeout;
            this.testConfig = testConfig;
            ClientGuid = clientGuid;
            Site = site;
            Initialize();
        }
        
        /// <summary>
        /// Cleanup.
        /// </summary>
        public void Cleanup()
        {
            if (!FileId.Equals(FILEID.Zero))
            {
                Client.Close(
                    TreeId, 
                    FileId,
                    (header, response) =>
                    {
                        GrantedCredit = header.CreditRequestResponse;
                    });
            }

            unchecked
            {
                if (TreeId != (uint)-1)
                {
                    Client.TreeDisconnect(TreeId);
                }
            }

            if (SessionId != 0)
            {
                Client.LogOff();
            }

            Client.Disconnect();
            Client = null;
        }

        /// <summary>
        /// Cleanup and reset all fields to the default values.
        /// </summary>
        /// <param name="isResetGuid">Indicates whether to reset ClientGuid or not. Clients cannot reconnect to the server with same ClientGuid and different Dialects.</param>
        public void Reset(bool isResetGuid = false)
        {
            Cleanup();

            Initialize();

            if (isResetGuid)
            {
                ClientGuid = Guid.NewGuid();
            }
        }

        /// <summary>
        /// Handle the lease break notification.
        /// </summary>
        /// <param name="respHeader">The SMB2 header included in the notification.</param>
        /// <param name="leaseBreakNotify">Lease break notification payload in the notification.</param>
        public void OnLeaseBreakNotificationReceived(Packet_Header respHeader, LEASE_BREAK_Notification_Packet leaseBreakNotify)
        {
            Client.LeaseBreakAcknowledgment(TreeId, leaseBreakNotify.LeaseKey, leaseBreakNotify.NewLeaseState);
        }
    }

    /// <summary>
    /// File operation to break the lease.
    /// </summary>
    public class BreakOperation
    {
        /// <summary>
        /// The type of file operation.
        /// </summary>
        public FileOperation Operation;
        /// <summary>
        /// The type of the operator.
        /// </summary>
        public OperatorType Operator;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="operation">The type of file operation.</param>
        /// <param name="operatorType">The type of the operator.</param>
        public BreakOperation(FileOperation operation, OperatorType operatorType)
        {
            this.Operation = operation;
            this.Operator = operatorType;
        }
    }

    /// <summary>
    /// The adapter for leasing model.
    /// </summary>
    [SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling")]
    public class LeasingAdapter : ModelManagedAdapterBase, ILeasingAdapter
    {
        #region Fields
        protected LeasingClientInfo originalClient;
        protected List<LeasingClientInfo> clients;
        protected BreakOperation lastOperation;

        private LeasingConfig leasingConfig;
        private string uncSharePath;

        private Guid LeaseKey;
        private Guid ParentLeaseKey;
        #endregion

        #region Action Fileds
        public event CreateResponseEventHandler CreateResponse;

        public event OnLeaseBreakNotificationEventHandler OnLeaseBreakNotification;

        public event LeaseBreakResponseEventHandler LeaseBreakResponse;
        #endregion

        #region Initialization

        public override void Initialize(ITestSite testSite)
        {
            base.Initialize(testSite);

            clients = new List<LeasingClientInfo>();
            lastOperation = null;
        }

        public override void Reset()
        {
            if (clients != null)
            {
                try
                {
                    foreach (var clientInfo in clients)
                    {
                        clientInfo.Cleanup();
                    }
                }
                finally
                {
                    clients.Clear();
                }
            }

            try
            {
                originalClient.Cleanup();
            }
            catch
            {
            }

            base.Reset();

            lastOperation = null;
        }

        #endregion

        #region Private Methods
        private void ValidateLeaseState(uint leaseState)
        {
            if (testConfig.Platform != Platform.NonWindows)
            {
                Site.Assert.IsTrue(leaseState == (uint)LeaseStateValues.SMB2_LEASE_NONE
                    || leaseState == (uint)LeaseStateValues.SMB2_LEASE_READ_CACHING
                    || leaseState == (uint)(LeaseStateValues.SMB2_LEASE_READ_CACHING | LeaseStateValues.SMB2_LEASE_WRITE_CACHING)
                    || leaseState == (uint)(LeaseStateValues.SMB2_LEASE_READ_CACHING | LeaseStateValues.SMB2_LEASE_HANDLE_CACHING)
                    || leaseState == (uint)(LeaseStateValues.SMB2_LEASE_READ_CACHING | LeaseStateValues.SMB2_LEASE_WRITE_CACHING | LeaseStateValues.SMB2_LEASE_HANDLE_CACHING),
                    "Expect that the leaseState contains a valid value for Windows implementation.");
            }
        }

        private void InitializeClient(LeasingClientInfo clientInfo, ModelDialectRevision maxSmbVersionClientSupported, bool isClientSupportDirectoryLeasing = false)
        {
            clientInfo.Client.Smb2Client.LeaseBreakNotificationReceived += new Action<Packet_Header, LEASE_BREAK_Notification_Packet>(OnLeaseBreakNotificationReceived);
            #region Connect to server
            clientInfo.Client.ConnectToServer(testConfig.UnderlyingTransport, testConfig.SutComputerName, testConfig.SutIPAddress);
            #endregion

            DialectRevision[] dialects = Smb2Utility.GetDialects(ModelUtility.GetDialectRevision(maxSmbVersionClientSupported));
            NEGOTIATE_Response_Capabilities_Values? capabilities = null;

            #region Negotiate
            uint status = clientInfo.Client.Negotiate(
                dialects,
                testConfig.IsSMB1NegotiateEnabled,
                capabilityValue: isClientSupportDirectoryLeasing ? Capabilities_Values.GLOBAL_CAP_DIRECTORY_LEASING : Capabilities_Values.NONE,
                clientGuid: clientInfo.ClientGuid,
                checker: (header, response) =>
                {
                    Site.Assert.AreEqual(
                        Smb2Status.STATUS_SUCCESS,
                        header.Status,
                        "{0} should succeed", header.Command);
                    clientInfo.Dialect = response.DialectRevision;
                    capabilities = response.Capabilities;
                    // out clientInfo.ServerGssToken
                });

            #region Validate Negotiate Response
            if (Smb2Utility.IsSmb3xFamily(clientInfo.Dialect))
            {
                Site.Assert.AreEqual<bool>(leasingConfig.IsLeasingSupported,
                    capabilities.Value.HasFlag(NEGOTIATE_Response_Capabilities_Values.GLOBAL_CAP_LEASING),
                    "Expect that the Capabilities in the response {0} SMB2_GLOBAL_CAP_LEASING 0x00000002.", leasingConfig.IsLeasingSupported ? "contains" : "does not contain");
                Site.Assert.AreEqual<bool>(leasingConfig.IsDirectoryLeasingSupported & isClientSupportDirectoryLeasing,
                    capabilities.Value.HasFlag(NEGOTIATE_Response_Capabilities_Values.GLOBAL_CAP_DIRECTORY_LEASING),
                    "Expect that the Capabilities in the response {0} SMB2_GLOBAL_CAP_DIRECTORY_LEASING 0x00000020.",
                    leasingConfig.IsDirectoryLeasingSupported & isClientSupportDirectoryLeasing ? "contains" : "does not contain");
            }
            else if (clientInfo.Dialect == DialectRevision.Smb21)
            {
                Site.Assert.AreEqual<bool>(leasingConfig.IsLeasingSupported,
                    capabilities.Value.HasFlag(NEGOTIATE_Response_Capabilities_Values.GLOBAL_CAP_LEASING),
                    "Expect that the Capabilities in the response {0} SMB2_GLOBAL_CAP_LEASING 0x00000002.", leasingConfig.IsLeasingSupported ? "contains" : "does not contain");
                Site.Assert.IsFalse(capabilities.Value.HasFlag(NEGOTIATE_Response_Capabilities_Values.GLOBAL_CAP_DIRECTORY_LEASING),
                    "Expect that the Capabilities in the response does not contain SMB2_GLOBAL_CAP_DIRECTORY_LEASING 0x00000020.");
            }
            else
            {
                Site.Assert.IsFalse(capabilities.Value.HasFlag(NEGOTIATE_Response_Capabilities_Values.GLOBAL_CAP_LEASING),
                    "Expect that the Capabilities in the response does not contain SMB2_GLOBAL_CAP_LEASING 0x00000002.");
                Site.Assert.IsFalse(capabilities.Value.HasFlag(NEGOTIATE_Response_Capabilities_Values.GLOBAL_CAP_DIRECTORY_LEASING),
                    "Expect that the Capabilities in the response does not contain SMB2_GLOBAL_CAP_DIRECTORY_LEASING 0x00000020.");
            }
            #endregion
            #endregion

            #region SESSION_SETUP

            status = clientInfo.Client.SessionSetup(
                testConfig.DefaultSecurityPackage,
                testConfig.SutComputerName,
                testConfig.AccountCredential,
                testConfig.UseServerGssToken,
                capabilities: SESSION_SETUP_Request_Capabilities_Values.NONE,
                checker: (header, response) =>
                {
                    Site.Assert.AreEqual(
                        Smb2Status.STATUS_SUCCESS,
                        header.Status,
                        "SessionSetup should succeed, actual status is {0}", Smb2Status.GetStatusCode(header.Status));
                    clientInfo.GrantedCredit = header.CreditRequestResponse;
                    clientInfo.SessionId = header.SessionId;
                    //out clientInfo.ServerGssToken
                });
            #endregion

            #region TREE_CONNECT to share
            TREE_CONNECT_Response? treeConnectResponse = null;
            status = clientInfo.Client.TreeConnect(
                uncSharePath, 
                out clientInfo.TreeId,
                checker: (header, response) =>
                {
                    Site.Assert.AreEqual(
                        Smb2Status.STATUS_SUCCESS,
                        header.Status,
                        "TreeConnect to {0} should succeed, actual status is {1}", uncSharePath, Smb2Status.GetStatusCode(header.Status));
                    treeConnectResponse = response;
                });
            if (treeConnectResponse.Value.ShareFlags.HasFlag(ShareFlags_Values.SHAREFLAG_FORCE_LEVELII_OPLOCK))
            {
                Site.Assert.Inconclusive("This test case is not applicable for the share whose ShareFlags includes SHAREFLAG_FORCE_LEVELII_OPLOCK.");
            }
            if (treeConnectResponse.Value.Capabilities.HasFlag(Share_Capabilities_Values.SHARE_CAP_SCALEOUT))
            {
                Site.Assert.Inconclusive("This test case is not applicable for the share whose Capabilities includes SHARE_CAP_SCALEOUT.");
            }
            #endregion
        }

        private void CreateFile(ModelDialectRevision dialect, string target, bool isDirectory)
        {
            LeasingClientInfo clientInfo = new LeasingClientInfo(testConfig.Timeout, testConfig, Site);
            clientInfo.File = target;

            InitializeClient(clientInfo, dialect);

            Smb2CreateContextResponse[] serverCreateContexts;
            uint status = clientInfo.Client.Create(
                clientInfo.TreeId,
                clientInfo.File, 
                isDirectory ? CreateOptions_Values.FILE_DIRECTORY_FILE : CreateOptions_Values.FILE_NON_DIRECTORY_FILE,
                out clientInfo.FileId,
                out serverCreateContexts,
                checker: (header, response) =>
                {
                    Site.Assert.AreEqual(
                        Smb2Status.STATUS_SUCCESS,
                        header.Status,
                        "Create a file {0} should succeed, actual status is {1}", clientInfo.File, Smb2Status.GetStatusCode(header.Status));
                    clientInfo.GrantedCredit = header.CreditRequestResponse;
                });
            clientInfo.Cleanup();
        }

        private void DeleteFile(ModelDialectRevision dialect, string target, bool isDirectory)
        {
            LeasingClientInfo clientInfo = new LeasingClientInfo(testConfig.Timeout, testConfig, Site);

            InitializeClient(clientInfo, dialect);

            Smb2CreateContextResponse[] serverCreateContexts;
            clientInfo.Client.Create(
                clientInfo.TreeId,
                target,
                (isDirectory ? CreateOptions_Values.FILE_DIRECTORY_FILE : CreateOptions_Values.FILE_NON_DIRECTORY_FILE) | CreateOptions_Values.FILE_DELETE_ON_CLOSE,
                out clientInfo.FileId,
                out serverCreateContexts,
                checker: (header, response) =>
                {
                    Site.Assert.AreEqual(
                        Smb2Status.STATUS_SUCCESS,
                        header.Status,
                        "Create a file {0} should succeed, actual status is {1}", clientInfo.File, Smb2Status.GetStatusCode(header.Status));
                    clientInfo.GrantedCredit = header.CreditRequestResponse;
                });

            FileDispositionInformation deleteInfo;
            deleteInfo.DeletePending = 1;

            byte[] inputBuffer;
            inputBuffer = TypeMarshal.ToBytes<FileDispositionInformation>(deleteInfo);

            clientInfo.Client.SetFileAttributes(
                clientInfo.TreeId,
                (byte)FileInformationClasses.FileDispositionInformation,
                clientInfo.FileId,
                inputBuffer);
            clientInfo.Cleanup();
        }

        private void OnLeaseBreakNotificationReceived(Packet_Header respHeader, LEASE_BREAK_Notification_Packet leaseBreakNotify)
        {
            Site.Log.Add(LogEntryKind.Debug, "LeaseBreakNotification was received from server");

            Site.Assert.AreEqual<Smb2Command>(Smb2Command.OPLOCK_BREAK, respHeader.Command, "Expect that the Command MUST be OPLOCK_BREAK.");
            Site.Assert.AreEqual<ulong>(0xFFFFFFFFFFFFFFFF, respHeader.MessageId, "Expect that the field MessageId MUST be set to 0xFFFFFFFFFFFFFFFF.");
            Site.Assert.AreEqual<ulong>(0, respHeader.SessionId, "Expect that the field SessionId MUST be set to 0.");
            Site.Assert.AreEqual<uint>(0, respHeader.TreeId, "Expect that the field TreeId MUST be set to 0.");

            Site.CaptureRequirementIfAreEqual<ushort>((ushort)44, leaseBreakNotify.StructureSize,
                        RequirementCategory.MUST_BE_SPECIFIED_VALUE.Id,
                        RequirementCategory.MUST_BE_SPECIFIED_VALUE.Description);
            Site.Assert.AreEqual<Guid>(LeaseKey, leaseBreakNotify.LeaseKey, "Expect that the field LeaseKey equals {0}.", LeaseKey.ToString());
            Site.CaptureRequirementIfAreEqual<uint>(0, leaseBreakNotify.BreakReason,
                        RequirementCategory.MUST_BE_ZERO.Id,
                        RequirementCategory.MUST_BE_ZERO.Description);
            Site.CaptureRequirementIfAreEqual<uint>(0, leaseBreakNotify.AccessMaskHint,
                        RequirementCategory.MUST_BE_ZERO.Id,
                        RequirementCategory.MUST_BE_ZERO.Description);
            Site.CaptureRequirementIfAreEqual<uint>(0, leaseBreakNotify.ShareMaskHint,
                        RequirementCategory.MUST_BE_ZERO.Id,
                        RequirementCategory.MUST_BE_ZERO.Description);

            Site.Log.Add(LogEntryKind.Debug, "Current lease state: \t{0}", leaseBreakNotify.CurrentLeaseState);
            Site.Log.Add(LogEntryKind.Debug, "New lease state: \t{0}", leaseBreakNotify.NewLeaseState);
            Site.Log.Add(LogEntryKind.Debug, "New epoch: \t{0}", leaseBreakNotify.NewEpoch);

            this.OnLeaseBreakNotification(leaseBreakNotify.NewEpoch, leaseBreakNotify.Flags, (uint)leaseBreakNotify.CurrentLeaseState,
                (uint)leaseBreakNotify.NewLeaseState);
        }

        private byte[] ConvertStringToByteArray(string str)
        {
            if (System.Text.Encoding.Default == System.Text.Encoding.ASCII)
            {
                return System.Text.Encoding.ASCII.GetBytes(str);
            }
            else
            {
                return System.Text.Encoding.Unicode.GetBytes(str);
            }
        }
        #endregion

        #region Actions
        public void ReadConfig(out LeasingConfig c)
        {
            uncSharePath = Smb2Utility.GetUncPath(testConfig.SutIPAddress.ToString(), testConfig.BasicFileShare);

            c = new LeasingConfig
            {
                MaxSmbVersionSupported = ModelUtility.GetModelDialectRevision(testConfig.MaxSmbVersionSupported),
                IsLeasingSupported = testConfig.IsLeasingSupported,
                IsDirectoryLeasingSupported = testConfig.IsDirectoryLeasingSupported,
            };

            leasingConfig = c;

            Site.Log.Add(LogEntryKind.Debug, leasingConfig.ToString());
        }

        public void SetupConnection(ModelDialectRevision dialect, ClientSupportDirectoryLeasingType clientSupportDirectoryLeasingType)
        {
            string parentDirectory = GetTestDirectoryName(uncSharePath);
            CreateFile(dialect, parentDirectory, true);

            originalClient = new LeasingClientInfo(testConfig.Timeout, testConfig, Site);
            originalClient.ParentDirectory = parentDirectory;
            originalClient.File = parentDirectory + "\\" + Guid.NewGuid().ToString();
            AddTestFileName(uncSharePath, originalClient.File);

            bool isClientSupportDirectoryLeasing = clientSupportDirectoryLeasingType == ClientSupportDirectoryLeasingType.ClientSupportDirectoryLeasing;
            InitializeClient(originalClient, dialect, isClientSupportDirectoryLeasing);

            clients.Add(new LeasingClientInfo(testConfig.Timeout, testConfig, originalClient.ClientGuid, Site)); // SameClientId
            clients.Add(new LeasingClientInfo(testConfig.Timeout, testConfig, originalClient.ClientGuid, Site)); // SameClientGuidDifferentLeaseKey
            clients.Add(new LeasingClientInfo(testConfig.Timeout, testConfig, Site)); // Second client
         }

        public void CreateRequest(ConnectTargetType connectTargetType, LeaseContextType leaseContextType,
            LeaseKeyType leaseKey, uint leaseState, LeaseFlagsValues leaseFlags,
            ParentLeaseKeyType parentLeaseKey)
        {
            ValidateLeaseState(leaseState);

            #region Fill Contexts
            LeaseKey = (leaseKey == LeaseKeyType.ValidLeaseKey ? Guid.NewGuid() : Guid.Empty);
            ParentLeaseKey = (parentLeaseKey == ParentLeaseKeyType.EmptyParentLeaseKey ? Guid.Empty : Guid.NewGuid());
            originalClient.IsDirectory = connectTargetType == ConnectTargetType.ConnectToDirectory;

            switch (leaseContextType)
            {
                case LeaseContextType.LeaseV1:
                    testConfig.CheckCreateContext(CreateContextTypeValue.SMB2_CREATE_REQUEST_LEASE);

                    originalClient.CreateContexts = new Smb2CreateContextRequest[] 
                    {
                        new Smb2CreateRequestLease
                        {
                            LeaseKey = LeaseKey,
                            LeaseState = (LeaseStateValues)leaseState,
                            LeaseFlags = (uint)leaseFlags,
                        }
                    };
                    break;
                case LeaseContextType.LeaseV2:
                    testConfig.CheckCreateContext(CreateContextTypeValue.SMB2_CREATE_REQUEST_LEASE_V2);

                    originalClient.CreateContexts = new Smb2CreateContextRequest[] 
                    {
                        new Smb2CreateRequestLeaseV2
                        {
                            LeaseKey = LeaseKey,
                            LeaseState = (LeaseStateValues)leaseState,
                            LeaseFlags = (uint)leaseFlags,
                            ParentLeaseKey = ParentLeaseKey,
                        }
                    };
                    break;
                default:
                    Site.Assume.Fail("Unexpected type: {0}", leaseContextType);
                    break;
            }
            #endregion

            Smb2CreateContextResponse[] serverCreateContexts;

            uint status = originalClient.Client.Create(
                originalClient.TreeId,
                originalClient.File,
                (connectTargetType == ConnectTargetType.ConnectToDirectory) ? CreateOptions_Values.FILE_DIRECTORY_FILE : CreateOptions_Values.FILE_NON_DIRECTORY_FILE,
                out originalClient.FileId,
                out serverCreateContexts,
                RequestedOplockLevel_Values.OPLOCK_LEVEL_LEASE,
                originalClient.CreateContexts,
                checker: (header, response) =>
                {
                    Site.Assert.AreEqual(
                        Smb2Status.STATUS_SUCCESS,
                        header.Status,
                        "Create a file {0} should succeed, actual status is {1}", originalClient.File, Smb2Status.GetStatusCode(header.Status));
                });
            #region Handle Create Response

            // 3.3.5.9.11   Handling the SMB2_CREATE_REQUEST_LEASE_V2 Create Context
            // If Connection.Dialect does not belong to the SMB 3.x dialect family or if RequestedOplockLevel is not SMB2_OPLOCK_LEVEL_LEASE, the server SHOULD<270> ignore the SMB2_CREATE_REQUEST_LEASE_V2 Create Context request.
            // <270> Section 3.3.5.9.11: Windows 8, Windows Server 2012, Windows 8.1, 
            // and Windows Server 2012 R2 servers do not ignore the SMB2_CREATE_REQUEST_LEASE_V2 create context when Connection.Dialect is equal to "2.100" or if RequestedOplockLevel is not equal to SMB2_OPLOCK_LEVEL_LEASE.
            if (!Smb2Utility.IsSmb3xFamily(originalClient.Dialect) && leaseContextType == LeaseContextType.LeaseV2)
            {
                Smb2CreateResponseLeaseV2 leaseContext = null;
                if (serverCreateContexts != null)
                {
                    foreach (var context in serverCreateContexts)
                    {
                        if (context is Smb2CreateResponseLeaseV2)
                        {
                            leaseContext = context as Smb2CreateResponseLeaseV2;
                            break;
                        }
                    }
                }
                if (testConfig.Platform == Platform.NonWindows)
                {
                    Site.Assert.IsNull(leaseContext, "Create response should not include SMB2_CREATE_RESPONSE_LEASE_V2.");
                }
                else
                {
                    if (originalClient.Dialect == DialectRevision.Smb21
                        && (testConfig.Platform == Platform.WindowsServer2012
                            || testConfig.Platform == Platform.WindowsServer2012R2))
                    {
                        Site.Assert.IsNotNull(leaseContext, "Create response should include SMB2_CREATE_RESPONSE_LEASE_V2 when platform is Windows 8, Windows 2012 and Windows 2012R2 and dialect is equal to \"2.100\".");
                    }
                    else
                    {
                        Site.Assert.IsNull(leaseContext, "Create response should not include SMB2_CREATE_RESPONSE_LEASE_V2.");
                    }
                }
                // Bypass the situation to avoid Windows issues.
                this.CreateResponse(ModelSmb2Status.STATUS_SUCCESS, ReturnLeaseContextType.ReturnLeaseContextNotIncluded, 
                    (uint)LeaseStateValues.SMB2_LEASE_NONE, LeaseFlagsValues.NONE, leasingConfig);
            }
            else if (status == Smb2Status.STATUS_SUCCESS)
            {
                if (leaseContextType == LeaseContextType.LeaseV1)
                {
                    if (serverCreateContexts != null)
                    {
                        Smb2CreateResponseLease leaseContext = null;
                        foreach (var context in serverCreateContexts)
                        {
                            if (context is Smb2CreateResponseLease)
                            {
                                leaseContext = context as Smb2CreateResponseLease;
                                break;
                            }
                        }
                        Site.Assert.IsNotNull(leaseContext, "Create response MUST include SMB2_CREATE_RESPONSE_LEASE.");

                        Site.Assert.AreEqual<Guid>(LeaseKey, leaseContext.LeaseKey, "Expect the LeaseKey in the response equals the LeaseKey in the request.");
                        Site.Assert.AreEqual<LeaseFlagsValues>(LeaseFlagsValues.NONE, leaseContext.LeaseFlags, "Expect the lease is not in breaking.");
                        Site.CaptureRequirementIfAreEqual<ulong>(0, leaseContext.LeaseDuration,
                            RequirementCategory.MUST_BE_ZERO.Id,
                            RequirementCategory.MUST_BE_ZERO.Description);

                        originalClient.LeaseState = leaseContext.LeaseState;
                        this.CreateResponse((ModelSmb2Status)status, ReturnLeaseContextType.ReturnLeaseContextIncluded, 
                            (uint)leaseContext.LeaseState, leaseContext.LeaseFlags, leasingConfig);
                    }
                    else
                    {
                        this.CreateResponse((ModelSmb2Status)status, ReturnLeaseContextType.ReturnLeaseContextNotIncluded, 
                            (uint)LeaseStateValues.SMB2_LEASE_NONE, LeaseFlagsValues.NONE,  leasingConfig);
                    }
                }
                else if (leaseContextType == LeaseContextType.LeaseV2)
                {
                    if (serverCreateContexts != null)
                    {
                        Smb2CreateResponseLeaseV2 leaseContext = null;
                        foreach (var context in serverCreateContexts)
                        {
                            if (context is Smb2CreateResponseLeaseV2)
                            {
                                leaseContext = context as Smb2CreateResponseLeaseV2;
                                break;
                            }
                        }
                        Site.Assert.IsNotNull(leaseContext, "Create response MUST include SMB2_CREATE_RESPONSE_LEASE_V2.");

                        Site.Assert.AreEqual<Guid>(LeaseKey, leaseContext.LeaseKey, "Expect the LeaseKey in the response equals the LeaseKey in the request.");
                        Site.Assert.IsTrue((leaseContext.Flags & LeaseFlagsValues.LEASE_FLAG_BREAK_IN_PROGRESS) == 0, "Expect the lease is not in breaking.");
                        Site.CaptureRequirementIfAreEqual<ulong>(0, leaseContext.LeaseDuration,
                            RequirementCategory.MUST_BE_ZERO.Id,
                            RequirementCategory.MUST_BE_ZERO.Description);
                        if ((leaseContext.Flags & LeaseFlagsValues.SMB2_LEASE_FLAG_PARENT_LEASE_KEY_SET) != 0)
                        {
                            Site.Assert.AreEqual<Guid>(ParentLeaseKey, leaseContext.ParentLeaseKey, "Expect the ParentLeaseKey in the response equals the ParentLeaseKey in the request.");
                        }
                        else
                        {
                            Site.Assert.AreEqual<Guid>(Guid.Empty, leaseContext.ParentLeaseKey, "Expect the ParentLeaseKey in the response is empty.");
                        }
                        // Epoch (2 bytes): A 16-bit unsigned integer incremented by the server on a lease state change.
                        Site.Assert.AreEqual<ulong>(1, leaseContext.Epoch, "Expect the Epoch in the response equals 1.");
                        if (testConfig.Platform == Platform.NonWindows)
                        {
                            // Reserved (2 bytes): This field MUST NOT be used and MUST be reserved. The server SHOULD<52> set this to 0, and the client MUST ignore it on receipt.
                            // <52> Section 2.2.14.2.11: Windows 8, Windows Server 2012, Windows 8.1, and Windows Server 2012 R2 set this field to an arbitrary value.
                            Site.CaptureRequirementIfAreEqual<ulong>(0, leaseContext.Reserved,
                                RequirementCategory.MUST_BE_ZERO.Id,
                                RequirementCategory.MUST_BE_ZERO.Description);
                        }

                        originalClient.LeaseState = leaseContext.LeaseState;
                        this.CreateResponse((ModelSmb2Status)status, ReturnLeaseContextType.ReturnLeaseContextIncluded, 
                            (uint)leaseContext.LeaseState, leaseContext.Flags, leasingConfig);
                    }
                    else
                    {
                        this.CreateResponse((ModelSmb2Status)status, ReturnLeaseContextType.ReturnLeaseContextNotIncluded, 
                            (uint)LeaseStateValues.SMB2_LEASE_NONE, LeaseFlagsValues.NONE, leasingConfig);
                    }
                }
                else
                {
                    Site.Assume.Fail("Unexpected type: {0}", leaseContextType);
                }
            }
            else
            {
                this.CreateResponse((ModelSmb2Status)status, ReturnLeaseContextType.ReturnLeaseContextNotIncluded, 
                    (uint)LeaseStateValues.SMB2_LEASE_NONE, LeaseFlagsValues.NONE, leasingConfig);
            }
            #endregion

            #region Write Data
            if (status == Smb2Status.STATUS_SUCCESS && (connectTargetType == ConnectTargetType.ConnectToNonDirectory))
            {
                byte[] data = Encoding.ASCII.GetBytes(Smb2Utility.CreateRandomString(1));
                status = originalClient.Client.Write(
                    originalClient.TreeId, 
                    originalClient.FileId, 
                    data,
                    checker: (header, response) =>
                    {
                        Site.Assert.AreEqual(
                            Smb2Status.STATUS_SUCCESS, status, 
                            "Write data to the file {0} failed with error code={1}", 
                            originalClient.File, Smb2Status.GetStatusCode(header.Status)
                            );
                        originalClient.GrantedCredit = header.CreditRequestResponse;
                    });

                status = originalClient.Client.Flush(
                    originalClient.TreeId, 
                    originalClient.FileId,
                    checker: (header, response) =>
                    {
                        Site.Assert.AreEqual(
                            Smb2Status.STATUS_SUCCESS, status,
                            "Flush data to the file {0} failed with error code={1}",
                            originalClient.File, Smb2Status.GetStatusCode(header.Status)
                            );
                        originalClient.GrantedCredit = header.CreditRequestResponse;
                    });
           }
            #endregion

            if (status == Smb2Status.STATUS_SUCCESS)
            {
                clients[(int)OperatorType.SameClientId].CreateContexts = originalClient.CreateContexts;
            }
        }

        public void LeaseBreakAcknowledgmentRequest(ModelLeaseKeyType modelLeaseKeyType, uint leaseState)
        {
            Guid leaseKey = ((modelLeaseKeyType == ModelLeaseKeyType.ValidLeaseKey) ? LeaseKey : Guid.NewGuid());

            uint status = originalClient.Client.LeaseBreakAcknowledgment(
            originalClient.TreeId,
            leaseKey,
            (LeaseStateValues)leaseState,
            checker: (header, response) =>
            {
                Site.Assert.AreEqual<Smb2Command>(Smb2Command.OPLOCK_BREAK, header.Command, "Expect that the Command is OPLOCK_BREAK.");
                if (header.Status == Smb2Status.STATUS_SUCCESS)
                {
                    Site.CaptureRequirementIfAreEqual<ushort>(36, response.StructureSize,
                        RequirementCategory.MUST_BE_SPECIFIED_VALUE.Id,
                        RequirementCategory.MUST_BE_SPECIFIED_VALUE.Description);
                    Site.CaptureRequirementIfAreEqual<ulong>(0, response.Reserved,
                        RequirementCategory.MUST_BE_ZERO.Id,
                        RequirementCategory.MUST_BE_ZERO.Description);
                    Site.CaptureRequirementIfAreEqual<ulong>(0, response.Flags,
                        RequirementCategory.MUST_BE_ZERO.Id,
                        RequirementCategory.MUST_BE_ZERO.Description);
                    Site.Assert.AreEqual<Guid>(leaseKey, response.LeaseKey, "Expect that the field LeaseKey equals {0}.", leaseKey.ToString());
                    Site.CaptureRequirementIfAreEqual<ulong>(0, response.LeaseDuration,
                        RequirementCategory.MUST_BE_ZERO.Id,
                        RequirementCategory.MUST_BE_ZERO.Description);
                }
                originalClient.LeaseState = response.LeaseState;
            });

            this.LeaseBreakResponse((ModelSmb2Status)status, (uint)originalClient.LeaseState);
        }

        public void FileOperationToBreakLeaseRequest(FileOperation operation, OperatorType operatorType, ModelDialectRevision dialect, out LeasingConfig c)
        {
            c = leasingConfig;

            LeasingClientInfo clientInfo = clients[(int)operatorType];

            // Avoid to fail because Windows issue
            if (dialect == ModelDialectRevision.Smb2002)
            {
                clientInfo.ClientGuid = Guid.NewGuid();
            }

            if (!clientInfo.IsInitialized)
            {
                InitializeClient(clientInfo, dialect);
            }

            if (operation == FileOperation.WRITE_DATA)
            {
                #region WRITE_DATA

                Smb2CreateContextResponse[] serverCreateContexts;
                    
                if (!clientInfo.IsOpened)
                {
                    clientInfo.Client.Create(
                        clientInfo.TreeId,
                        originalClient.File,
                        originalClient.IsDirectory ? CreateOptions_Values.FILE_DIRECTORY_FILE : CreateOptions_Values.FILE_NON_DIRECTORY_FILE,
                        out clientInfo.FileId,
                        out serverCreateContexts,
                        clientInfo.CreateContexts == null ? RequestedOplockLevel_Values.OPLOCK_LEVEL_NONE : RequestedOplockLevel_Values.OPLOCK_LEVEL_LEASE,
                        clientInfo.CreateContexts,
                        checker: (header, response) =>
                        {
                            Site.Assert.AreEqual(
                                Smb2Status.STATUS_SUCCESS,
                                header.Status,
                                "Create a file {0} should succeed, actual status is {1}", originalClient.File, Smb2Status.GetStatusCode(header.Status));
                        });
                }

                byte[] data = Encoding.ASCII.GetBytes("Write data to break READ caching.");

                clientInfo.Client.WriteRequest(
                    clientInfo.TreeId,
                    clientInfo.FileId,
                    data,
                    out clientInfo.MessageId);
                clientInfo.LastOperationMessageId = clientInfo.MessageId;

                #endregion
            }
            else if (operation == FileOperation.SIZE_CHANGED)
            {
                #region SIZE_CHANGED

                Smb2CreateContextResponse[] serverCreateContexts;

                if (!clientInfo.IsOpened)
                {
                    clientInfo.Client.Create(
                        clientInfo.TreeId,
                        originalClient.File,
                        originalClient.IsDirectory ? CreateOptions_Values.FILE_DIRECTORY_FILE : CreateOptions_Values.FILE_NON_DIRECTORY_FILE,
                        out clientInfo.FileId,
                        out serverCreateContexts,
                        clientInfo.CreateContexts == null ? RequestedOplockLevel_Values.OPLOCK_LEVEL_NONE : RequestedOplockLevel_Values.OPLOCK_LEVEL_LEASE,
                        clientInfo.CreateContexts,
                        checker: (header, response) =>
                        {
                            Site.Assert.AreEqual(
                                Smb2Status.STATUS_SUCCESS,
                                header.Status,
                                "Create a file {0} should succeed, actual status is {1}", originalClient.File, Smb2Status.GetStatusCode(header.Status));
                        });
                }

                FileEndOfFileInformation changeSizeInfo;
                changeSizeInfo.EndOfFile = 512;

                byte[] inputBuffer;
                inputBuffer = TypeMarshal.ToBytes<FileEndOfFileInformation>(changeSizeInfo);

                clientInfo.Client.SetFileAttributesRequest(
                    clientInfo.TreeId,
                    (byte)FileInformationClasses.FileEndOfFileInformation,
                    clientInfo.FileId,
                    inputBuffer,
                    out clientInfo.MessageId);
                clientInfo.LastOperationMessageId = clientInfo.MessageId;
                #endregion
            }
            else if (operation == FileOperation.RANGE_LOCK)
            {
                #region RANGE_LOCK

                Smb2CreateContextResponse[] serverCreateContexts;

                if (!clientInfo.IsOpened)
                {
                    clientInfo.Client.Create(
                        clientInfo.TreeId,
                        originalClient.File,
                        originalClient.IsDirectory ? CreateOptions_Values.FILE_DIRECTORY_FILE : CreateOptions_Values.FILE_NON_DIRECTORY_FILE,
                        out clientInfo.FileId,
                        out serverCreateContexts,
                        clientInfo.CreateContexts == null ? RequestedOplockLevel_Values.OPLOCK_LEVEL_NONE : RequestedOplockLevel_Values.OPLOCK_LEVEL_LEASE,
                        clientInfo.CreateContexts,
                        createDisposition: CreateDisposition_Values.FILE_OPEN,
                        checker: (header, response) =>
                        {
                            Site.Assert.AreEqual(
                                Smb2Status.STATUS_SUCCESS,
                                header.Status,
                                "Create a file {0} should succeed, actual status is {1}", originalClient.File, Smb2Status.GetStatusCode(header.Status));
                        }
                    );
                }

                clientInfo.Locks = new LOCK_ELEMENT[1];
                clientInfo.LockSequence = 0;
                clientInfo.Locks[0].Offset = 0;
                clientInfo.Locks[0].Length = (ulong)1 * 1024 / 2;
                clientInfo.Locks[0].Flags = LOCK_ELEMENT_Flags_Values.LOCKFLAG_SHARED_LOCK;

                clientInfo.Client.LockRequest(clientInfo.TreeId, clientInfo.LockSequence++, clientInfo.FileId, clientInfo.Locks, out clientInfo.MessageId);
                clientInfo.LastOperationMessageId = clientInfo.MessageId;
                #endregion
            }
            else if (operation == FileOperation.OPEN_OVERWRITE)
            {
                #region OPEN_OVERWRITE
                if (clientInfo.IsOpened)
                {
                    clientInfo.Reset(operatorType == OperatorType.SecondClient);

                    InitializeClient(clientInfo, dialect);
                }

                clientInfo.Client.CreateRequest(
                        clientInfo.TreeId,
                        originalClient.File,
                        originalClient.IsDirectory ? CreateOptions_Values.FILE_DIRECTORY_FILE : CreateOptions_Values.FILE_NON_DIRECTORY_FILE,
                        out clientInfo.MessageId,
                        clientInfo.CreateContexts == null ? RequestedOplockLevel_Values.OPLOCK_LEVEL_NONE : RequestedOplockLevel_Values.OPLOCK_LEVEL_LEASE,
                        clientInfo.CreateContexts,
                        createDisposition: CreateDisposition_Values.FILE_OVERWRITE
                    );
                clientInfo.LastOperationMessageId = clientInfo.MessageId;

                #endregion
            }
            else if (operation == FileOperation.OPEN_WITHOUT_OVERWRITE)
            {
                #region OPEN_WITHOUT_OVERWRITE
                if (clientInfo.IsOpened)
                {
                    clientInfo.Reset(operatorType == OperatorType.SecondClient);

                    InitializeClient(clientInfo, dialect);
                }

                if (!clientInfo.IsOpened)
                {
                    clientInfo.Client.CreateRequest(
                            clientInfo.TreeId,
                            originalClient.File,
                            originalClient.IsDirectory ? CreateOptions_Values.FILE_DIRECTORY_FILE : CreateOptions_Values.FILE_NON_DIRECTORY_FILE,
                            out clientInfo.MessageId,
                            clientInfo.CreateContexts == null ? RequestedOplockLevel_Values.OPLOCK_LEVEL_NONE : RequestedOplockLevel_Values.OPLOCK_LEVEL_LEASE,
                            clientInfo.CreateContexts,
                            createDisposition: CreateDisposition_Values.FILE_OPEN
                        );
                    clientInfo.LastOperationMessageId = clientInfo.MessageId;
                }

                #endregion
            }
            else if (operation == FileOperation.OPEN_SHARING_VIOLATION)
            {
                #region OPEN_SHARING_VIOLATION
                if (clientInfo.IsOpened)
                {
                    clientInfo.Reset(operatorType == OperatorType.SecondClient);

                    InitializeClient(clientInfo, dialect);
                }

                if (!clientInfo.IsOpened)
                {
                    clientInfo.Client.CreateRequest(
                        clientInfo.TreeId,
                        originalClient.File,
                        originalClient.IsDirectory ? CreateOptions_Values.FILE_DIRECTORY_FILE : CreateOptions_Values.FILE_NON_DIRECTORY_FILE,
                        out clientInfo.MessageId,
                        clientInfo.CreateContexts == null ? RequestedOplockLevel_Values.OPLOCK_LEVEL_NONE : RequestedOplockLevel_Values.OPLOCK_LEVEL_LEASE,
                        clientInfo.CreateContexts,
                        shareAccess: ShareAccess_Values.FILE_SHARE_READ,
                        createDisposition: CreateDisposition_Values.FILE_OPEN
                    );
                    clientInfo.LastOperationMessageId = clientInfo.MessageId;
                }
                #endregion
            }
            else if (operation == FileOperation.OPEN_SHARING_VIOLATION_WITH_OVERWRITE)
            {
                #region OPEN_SHARING_VIOLATION_WITH_OVERWRITE
                if (clientInfo.IsOpened)
                {
                    clientInfo.Reset(operatorType == OperatorType.SecondClient);

                    InitializeClient(clientInfo, dialect);
                }

                if (!clientInfo.IsOpened)
                {
                    clientInfo.Client.CreateRequest(
                        clientInfo.TreeId,
                        originalClient.File,
                        originalClient.IsDirectory ? CreateOptions_Values.FILE_DIRECTORY_FILE : CreateOptions_Values.FILE_NON_DIRECTORY_FILE,
                        out clientInfo.MessageId,
                        clientInfo.CreateContexts == null ? RequestedOplockLevel_Values.OPLOCK_LEVEL_NONE : RequestedOplockLevel_Values.OPLOCK_LEVEL_LEASE,
                        clientInfo.CreateContexts,
                        shareAccess: ShareAccess_Values.FILE_SHARE_READ,
                        createDisposition: CreateDisposition_Values.FILE_OVERWRITE
                    );
                    clientInfo.LastOperationMessageId = clientInfo.MessageId;
                }

                #endregion
            }
            else if (operation == FileOperation.DELETED)
            {
                #region DELETED
                if (clientInfo.IsOpened)
                {
                    clientInfo.Reset(operatorType == OperatorType.SecondClient);

                    InitializeClient(clientInfo, dialect);
                }

                Smb2CreateContextResponse[] serverCreateContexts;

                if (!clientInfo.IsOpened)
                {
                    uint status = clientInfo.Client.Create(
                        clientInfo.TreeId,
                        originalClient.File,
                        (originalClient.IsDirectory ? CreateOptions_Values.FILE_DIRECTORY_FILE : CreateOptions_Values.FILE_NON_DIRECTORY_FILE) | CreateOptions_Values.FILE_DELETE_ON_CLOSE,
                        out clientInfo.FileId,
                        out serverCreateContexts,
                        clientInfo.CreateContexts == null ? RequestedOplockLevel_Values.OPLOCK_LEVEL_NONE : RequestedOplockLevel_Values.OPLOCK_LEVEL_LEASE,
                        clientInfo.CreateContexts,
                        createDisposition: CreateDisposition_Values.FILE_OPEN,
                        checker: (header, response) =>
                        {
                            Site.Assert.AreEqual(
                                Smb2Status.STATUS_SUCCESS,
                                header.Status,
                                "Create a file {0} should succeed, actual status is {1}", originalClient.File, Smb2Status.GetStatusCode(header.Status));
                        }
                    );
                }

                FileDispositionInformation deleteInfo;
                deleteInfo.DeletePending = 1;

                byte[] inputBuffer;
                inputBuffer = TypeMarshal.ToBytes<FileDispositionInformation>(deleteInfo);

                clientInfo.Client.SetFileAttributesRequest(
                    clientInfo.TreeId,
                    (byte)FileInformationClasses.FileDispositionInformation,
                    clientInfo.FileId,
                    inputBuffer,
                    out clientInfo.MessageId);
                clientInfo.LastOperationMessageId = clientInfo.MessageId;
                #endregion
            }
            else if (operation == FileOperation.RENAMEED)
            {
                #region RENAMEED
                if (clientInfo.IsOpened)
                {
                    clientInfo.Reset(operatorType == OperatorType.SecondClient);

                    InitializeClient(clientInfo, dialect);
                }

                Smb2CreateContextResponse[] serverCreateContexts;

                if (!clientInfo.IsOpened)
                {
                    clientInfo.Client.Create(
                        clientInfo.TreeId,
                        originalClient.File,
                        originalClient.IsDirectory ? CreateOptions_Values.FILE_DIRECTORY_FILE : CreateOptions_Values.FILE_NON_DIRECTORY_FILE,
                        out clientInfo.FileId,
                        out serverCreateContexts,
                        clientInfo.CreateContexts == null ? RequestedOplockLevel_Values.OPLOCK_LEVEL_NONE : RequestedOplockLevel_Values.OPLOCK_LEVEL_LEASE,
                        clientInfo.CreateContexts,
                        createDisposition: CreateDisposition_Values.FILE_OPEN,
                        checker: (header, response) =>
                        {
                            Site.Assert.AreEqual(
                                Smb2Status.STATUS_SUCCESS,
                                header.Status,
                                "Create a file {0} should succeed, actual status is {1}", originalClient.File, Smb2Status.GetStatusCode(header.Status));
                        }
                    );
                }
                
                string newFileName = originalClient.ParentDirectory + "\\" + Guid.NewGuid().ToString();
                AddTestFileName(uncSharePath, newFileName);

                FileRenameInformation info = new FileRenameInformation();
                info.ReplaceIfExists = 1;
                info.FileName = ConvertStringToByteArray(newFileName);
                info.FileNameLength = (uint)info.FileName.Length;
                info.RootDirectory = FileRenameInformation_RootDirectory_Values.V1;
                info.Reserved = new byte[7];

                byte[] inputBuffer;
                inputBuffer = TypeMarshal.ToBytes<FileRenameInformation>(info);

                clientInfo.Client.SetFileAttributesRequest(
                    clientInfo.TreeId,
                    (byte)FileInformationClasses.FileRenameInformation,
                    clientInfo.FileId,
                    inputBuffer,
                    out clientInfo.MessageId);
                clientInfo.LastOperationMessageId = clientInfo.MessageId;

                originalClient.File = newFileName;
                #endregion
            }
            else if (operation == FileOperation.PARENT_DIR_RENAMED)
            {
                #region PARENT_DIR_RENAMED
                if (clientInfo.IsOpened)
                {
                    clientInfo.Reset(operatorType == OperatorType.SecondClient);

                    InitializeClient(clientInfo, dialect);
                }

                Smb2CreateContextResponse[] serverCreateContexts;

                if (!clientInfo.IsOpened)
                {
                    clientInfo.Client.Create(
                        clientInfo.TreeId,
                        originalClient.ParentDirectory,
                        CreateOptions_Values.FILE_DIRECTORY_FILE,
                        out clientInfo.FileId,
                        out serverCreateContexts,
                        createDisposition: CreateDisposition_Values.FILE_OPEN,
                        checker: (header, response) =>
                        {
                            Site.Assert.AreEqual(
                                Smb2Status.STATUS_SUCCESS,
                                header.Status,
                                "Create a file {0} should succeed, actual status is {1}", originalClient.File, Smb2Status.GetStatusCode(header.Status));
                        }
                    );
                }

                string newFileName = "LeasingDir_" + Guid.NewGuid().ToString();
                AddTestDirectoryName(uncSharePath, newFileName);

                FileRenameInformation info = new FileRenameInformation();
                info.ReplaceIfExists = 0;
                info.FileName = ConvertStringToByteArray(newFileName);
                info.FileNameLength = (uint)info.FileName.Length;
                info.RootDirectory = FileRenameInformation_RootDirectory_Values.V1;
                info.Reserved = new byte[7];

                byte[] inputBuffer;
                inputBuffer = TypeMarshal.ToBytes<FileRenameInformation>(info);

                clientInfo.Client.SetFileAttributesRequest(
                    clientInfo.TreeId,
                    (byte)FileInformationClasses.FileRenameInformation,
                    clientInfo.FileId,
                    inputBuffer,
                    out clientInfo.MessageId);
                clientInfo.LastOperationMessageId = clientInfo.MessageId;
                 // Does not need to update these two fields File and ParentDirectory in orginal client because the operation will fail.
                #endregion
            }

            lastOperation = new BreakOperation(operation, operatorType);
        }

        public void FileOperationToBreakLeaseResponse()
        {
            LeasingClientInfo clientInfo = clients[(int)lastOperation.Operator];

            if (lastOperation.Operation == FileOperation.WRITE_DATA)
            {
                #region WRITE_DATA

                clientInfo.Client.WriteResponse(
                    clientInfo.LastOperationMessageId,
                    (header, response) =>
                    {
                        clientInfo.GrantedCredit = header.CreditRequestResponse;
                    });
                #endregion
            }
            else if (lastOperation.Operation == FileOperation.SIZE_CHANGED)
            {
                #region SIZE_CHANGED

                clientInfo.Client.SetInfoResponse(
                    clientInfo.LastOperationMessageId,
                    (header, response) =>
                    {
                        clientInfo.GrantedCredit = header.CreditRequestResponse;
                    });
                #endregion
            }
            else if (lastOperation.Operation == FileOperation.RANGE_LOCK)
            {
                #region RANGE_LOCK
                clientInfo.Client.LockResponse(
                    clientInfo.LastOperationMessageId,
                    (header, response) =>
                    {
                        clientInfo.GrantedCredit = header.CreditRequestResponse;
                    });

                clientInfo.Locks[0].Flags = LOCK_ELEMENT_Flags_Values.LOCKFLAG_UNLOCK;
                clientInfo.Client.Lock(clientInfo.TreeId, clientInfo.LockSequence++, clientInfo.FileId, clientInfo.Locks);
                #endregion
            }
            else if (lastOperation.Operation == FileOperation.OPEN_OVERWRITE)
            {
                #region OPEN_OVERWRITE
                Smb2CreateContextResponse[] serverCreateContexts;

                clientInfo.Client.CreateResponse(
                    clientInfo.LastOperationMessageId,
                    out clientInfo.FileId,
                    out serverCreateContexts,
                    checker: (header, response) =>
                    {
                        clientInfo.GrantedCredit = header.CreditRequestResponse;
                    });
                #endregion
            }
            else if (lastOperation.Operation == FileOperation.OPEN_WITHOUT_OVERWRITE)
            {
                #region OPEN_BY_ANOTHER_CLIENT
                Smb2CreateContextResponse[] serverCreateContexts;
                clientInfo.Client.CreateResponse(
                    clientInfo.LastOperationMessageId,
                    out clientInfo.FileId,
                    out serverCreateContexts,
                    checker: (header, response) =>
                    {
                        clientInfo.GrantedCredit = header.CreditRequestResponse;
                    });
                #endregion
            }
            else if (lastOperation.Operation == FileOperation.OPEN_SHARING_VIOLATION)
            {
                #region OPEN_SHARING_VIOLATION
                Smb2CreateContextResponse[] serverCreateContexts;
                uint status = clientInfo.Client.CreateResponse(
                    clientInfo.LastOperationMessageId,
                    out clientInfo.FileId,
                    out serverCreateContexts,
                    checker: (header, response) =>
                    {
                        clientInfo.GrantedCredit = header.CreditRequestResponse;
                    });

                if (testConfig.Platform != Platform.NonWindows)
                {
                    Site.Assert.AreEqual(Smb2Status.STATUS_SHARING_VIOLATION, status, "Expect that creation fails with STATUS_SHARING_VIOLATION.");
                }
                #endregion
            }
            else if (lastOperation.Operation == FileOperation.OPEN_SHARING_VIOLATION_WITH_OVERWRITE)
            {
                #region OPEN_SHARING_VIOLATION_WITH_OVERWRITE
                Smb2CreateContextResponse[] serverCreateContexts;
                uint status = clientInfo.Client.CreateResponse(
                    clientInfo.LastOperationMessageId,
                    out clientInfo.FileId,
                    out serverCreateContexts,
                    checker: (header, response) =>
                    {
                        clientInfo.GrantedCredit = header.CreditRequestResponse;
                    });
                if (testConfig.Platform != Platform.NonWindows)
                {
                    Site.Assert.AreEqual(Smb2Status.STATUS_SHARING_VIOLATION, status, "Expect that creation fails with STATUS_SHARING_VIOLATION.");
                }
                #endregion
            }
            else if (lastOperation.Operation == FileOperation.DELETED)
            {
                #region DELETED
                clientInfo.Client.SetInfoResponse(
                    clientInfo.LastOperationMessageId,
                    (header, response) =>
                    {
                        clientInfo.GrantedCredit = header.CreditRequestResponse;
                    });
                #endregion
            }
            else if (lastOperation.Operation == FileOperation.RENAMEED)
            {
                #region RENAMEED
                clientInfo.Client.SetInfoResponse(
                    clientInfo.LastOperationMessageId,
                    (header, response) =>
                    {
                        clientInfo.GrantedCredit = header.CreditRequestResponse;
                    });
                #endregion
            }
            else if (lastOperation.Operation == FileOperation.PARENT_DIR_RENAMED)
            {
                #region PARENT_DIR_RENAMED
                clientInfo.Client.SetInfoResponse(
                    clientInfo.LastOperationMessageId,
                    (header, response) =>
                    {
                        clientInfo.GrantedCredit = header.CreditRequestResponse;
                    });

                clientInfo.Reset(lastOperation.Operator == OperatorType.SecondClient);
                #endregion
            }
        }
        #endregion
    }
}
