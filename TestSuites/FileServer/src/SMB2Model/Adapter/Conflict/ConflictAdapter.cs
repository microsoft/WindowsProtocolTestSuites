// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Net;
using System.Threading;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;

namespace Microsoft.Protocols.TestSuites.FileSharing.SMB2Model.Adapter.Conflict
{
    public class ConflictAdapter : ModelManagedAdapterBase, IConflictAdapter
    {
        #region Event
        public event ConflictResponseEventHandler ConflictResponse;
        #endregion

        #region Fields
        private string fileName;
        private Smb2FunctionalClient firstClient;
        private Smb2FunctionalClient secondClient;
        private uint treeId1;
        private uint treeId2;
        private FILEID fileId1;
        private FILEID fileId2;
        private LeaseBreakState leaseBreakState;
        private const int DEFAULT_WRITE_BUFFER_SIZE_IN_KB = 1;
        #endregion

        #region Initialization & Reset
        /// <summary>
        /// Initialization
        /// </summary>
        /// <param name="testSite"></param>
        public override void Initialize(ITestSite testSite)
        {
            base.Initialize(testSite);
        }

        /// <summary>
        /// Reset all member variables before running next test case
        /// </summary>
        public override void Reset()
        {
            if (firstClient != null)
            {
                firstClient.Disconnect();
                firstClient = null;
            }

            if (secondClient != null)
            {
                secondClient.Disconnect();
                secondClient = null;
            }

            fileName = null;
            leaseBreakState = LeaseBreakState.NoLeaseBreak;
            treeId1 = 0;
            treeId2 = 0;
            fileId1 = FILEID.Zero;
            fileId2 = FILEID.Zero;
            base.Reset();
        }
        #endregion

        #region Actions
        /// <summary>
        /// 1. Prepare the test file
        /// The two clients connects to the two Nodes of the scaleout file server, including: Negotiate, SessionSetup, TreeConnect
        /// </summary>
        public void Preparation()
        {
            PrepareTestFile();

            firstClient = InitializeClient(testConfig.ScaleOutFileServerIP1, out treeId1);
            firstClient.Smb2Client.LeaseBreakNotificationReceived += new Action<Packet_Header, LEASE_BREAK_Notification_Packet>(OnLeaseBreakNotificationReceived);

            secondClient = InitializeClient(testConfig.ScaleOutFileServerIP2, out treeId2);
        }

        /// <summary>
        /// Conflict requests sent from the two clients
        /// </summary>
        /// <param name="requestFromFirstClient">Type of the specified request from the first client</param>
        /// <param name="requestFromSecondClient">Type of the specified request from the second client</param> 
        public void ConflictRequest(RequestType requestFromFirstClient, RequestType requestFromSecondClient)
        {
            // Create an open for the second client before the first client deletes the file
            if (requestFromFirstClient == RequestType.UncommitedDelete)
            {
                CreateOptions_Values createOptions = (requestFromSecondClient == RequestType.Delete) ?
                    (CreateOptions_Values.FILE_NON_DIRECTORY_FILE | CreateOptions_Values.FILE_DELETE_ON_CLOSE)
                    : CreateOptions_Values.FILE_NON_DIRECTORY_FILE;
                Smb2CreateContextResponse[] contexts;
                secondClient.Create(
                    treeId2,
                    fileName,
                    createOptions,
                    out fileId2,
                    out contexts,
                    RequestedOplockLevel_Values.OPLOCK_LEVEL_NONE);
            }

            RequestFromFirstClient(requestFromFirstClient);
            RequestFromSecondClient(requestFromSecondClient);
        }

        /// <summary>
        /// Do operation to the file from the first client
        /// </summary>
        private void RequestFromFirstClient(RequestType requestFromFirstClient)
        {
            Smb2CreateContextResponse[] contexts;
            switch (requestFromFirstClient)
            {
                case RequestType.ExclusiveLock:
                    firstClient.Create(
                        treeId1,
                        fileName,
                        CreateOptions_Values.FILE_NON_DIRECTORY_FILE,
                        out fileId1,
                        out contexts,
                        RequestedOplockLevel_Values.OPLOCK_LEVEL_NONE);
                    firstClient.Lock(
                        treeId1,
                        0,
                        fileId1,
                        new LOCK_ELEMENT[]
                        {
                            new LOCK_ELEMENT
                            {
                                Offset = 0,
                                Length = DEFAULT_WRITE_BUFFER_SIZE_IN_KB * 1024,
                                Flags = LOCK_ELEMENT_Flags_Values.LOCKFLAG_EXCLUSIVE_LOCK | LOCK_ELEMENT_Flags_Values.LOCKFLAG_FAIL_IMMEDIATELY
                            }
                        });
                    break;
                case RequestType.Lease:
                    if (!testConfig.IsLeasingSupported)
                    {
                        // skip this case if leasing is not supported
                        Site.Assert.Inconclusive("This test case is applicable only when leasing is supported.");
                    }
                    firstClient.Create(
                        treeId1,
                        fileName,
                        CreateOptions_Values.FILE_NON_DIRECTORY_FILE,
                        out fileId1,
                        out contexts,
                        RequestedOplockLevel_Values.OPLOCK_LEVEL_LEASE,
                        new Smb2CreateContextRequest[]
                        {
                            new Smb2CreateRequestLease
                            {
                                LeaseKey = Guid.NewGuid(),
                                // [MS-SMB2] 3.3.5.9.8 
                                // If Connection.Dialect belongs to the SMB 3.x dialect family, TreeConnect.Share.Type includes STYPE_CLUSTER_SOFS, 
                                // and if LeaseState includes SMB2_LEASE_READ_CACHING, the server MUST set LeaseState to SMB2_LEASE_READ_CACHING, 
                                // otherwise set LeaseState to SMB2_LEASE_NONE.
                                // The share.type includes STYPE_CLUSTER_SOFS, so only READ_CACHING could be granted even the client applied for other lease.
                                // So the client only applies READ_CACHING here.
                                LeaseState = LeaseStateValues.SMB2_LEASE_READ_CACHING
                            }
                        });
                    break;
                case RequestType.UncommitedDelete:
                case RequestType.Delete:
                    firstClient.Create(
                        treeId1,
                        fileName,
                        CreateOptions_Values.FILE_DELETE_ON_CLOSE | CreateOptions_Values.FILE_NON_DIRECTORY_FILE,
                        out fileId1,
                        out contexts,
                        RequestedOplockLevel_Values.OPLOCK_LEVEL_NONE);

                    // Close to delete the file
                    firstClient.Close(treeId1, fileId1);
                    break;
                case RequestType.Write:
                    firstClient.Create(
                        treeId1,
                        fileName,
                        CreateOptions_Values.FILE_NON_DIRECTORY_FILE,
                        out fileId1,
                        out contexts,
                        RequestedOplockLevel_Values.OPLOCK_LEVEL_NONE);
                    firstClient.Write(treeId1, fileId1, Smb2Utility.CreateRandomString(DEFAULT_WRITE_BUFFER_SIZE_IN_KB));
                    break;
                case RequestType.Read:
                    firstClient.Create(
                        treeId1,
                        fileName,
                        CreateOptions_Values.FILE_NON_DIRECTORY_FILE,
                        out fileId1,
                        out contexts,
                        RequestedOplockLevel_Values.OPLOCK_LEVEL_NONE);
                    byte[] data;
                    firstClient.Read(treeId1, fileId1, 0, (uint)DEFAULT_WRITE_BUFFER_SIZE_IN_KB * 1024, out data);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Do operation to the file from the second client
        /// </summary>
        private void RequestFromSecondClient(RequestType requestFromSecondClient)
        {
            uint status = 0;
            Smb2CreateContextResponse[] contexts;
            switch (requestFromSecondClient)
            {
                case RequestType.ExclusiveLock:
                    // If the open is not created before, create here.
                    if (fileId2.Persistent == 0)
                    {
                        status = secondClient.Create(
                            treeId2,
                            fileName,
                            CreateOptions_Values.FILE_NON_DIRECTORY_FILE,
                            out fileId2,
                            out contexts,
                            RequestedOplockLevel_Values.OPLOCK_LEVEL_NONE,
                            createDisposition: CreateDisposition_Values.FILE_OPEN,
                            checker: (header, response) => { });
                    }

                    // If open is created successfully, then do the specified file operation.
                    // Otherwise, return the status to Model.
                    if (fileId2.Persistent != 0)
                    {
                        status = secondClient.Lock(
                            treeId2,
                            0,
                            fileId2,
                            new LOCK_ELEMENT[]
                            {
                                new LOCK_ELEMENT
                                {
                                    Offset = 0,
                                    Length = (ulong)DEFAULT_WRITE_BUFFER_SIZE_IN_KB * 1024,
                                    Flags = LOCK_ELEMENT_Flags_Values.LOCKFLAG_EXCLUSIVE_LOCK | LOCK_ELEMENT_Flags_Values.LOCKFLAG_FAIL_IMMEDIATELY
                                }
                            },
                            checker: (header, response) => { });
                    }
                    HandleConflictResult(status);
                    break;
                case RequestType.Lease:
                    if (!testConfig.IsLeasingSupported)
                    {
                        // skip this case if leasing is not supported
                        Site.Assert.Inconclusive("This test case is applicable only when leasing is supported.");
                    }
                    status = secondClient.Create(
                        treeId2,
                        fileName,
                        CreateOptions_Values.FILE_NON_DIRECTORY_FILE,
                        out fileId2,
                        out contexts,
                        RequestedOplockLevel_Values.OPLOCK_LEVEL_LEASE,
                        new Smb2CreateContextRequest[]
                        {
                            new Smb2CreateRequestLease
                            {
                                LeaseKey = Guid.NewGuid(),
                                LeaseState = LeaseStateValues.SMB2_LEASE_READ_CACHING
                            }
                        },
                        createDisposition: CreateDisposition_Values.FILE_OPEN,
                        checker: (header, response) => { });
                    HandleConflictResult(status);
                    break;
                case RequestType.Delete:
                    if (fileId2.Persistent == 0)
                    {
                        status = secondClient.Create(
                            treeId2,
                            fileName,
                            CreateOptions_Values.FILE_DELETE_ON_CLOSE | CreateOptions_Values.FILE_NON_DIRECTORY_FILE,
                            out fileId2,
                            out contexts,
                            RequestedOplockLevel_Values.OPLOCK_LEVEL_NONE,
                            createDisposition: CreateDisposition_Values.FILE_OPEN,
                            checker: (header, response) => { });
                    }

                    if (fileId2.Persistent != 0)
                    {
                        status = secondClient.Close(treeId2, fileId2, checker: (responseHeader, response) => { });
                    }
                    HandleConflictResult(status);
                    break;
                case RequestType.Write:
                    if (fileId2.Persistent == 0)
                    {
                        status = secondClient.Create(
                            treeId2,
                            fileName,
                            CreateOptions_Values.FILE_NON_DIRECTORY_FILE,
                            out fileId2,
                            out contexts,
                            RequestedOplockLevel_Values.OPLOCK_LEVEL_NONE,
                            createDisposition: CreateDisposition_Values.FILE_OPEN,
                            checker: (header, response) => { });
                    }

                    if (fileId2.Persistent != 0)
                    {
                        status = secondClient.Write(
                            treeId2,
                            fileId2,
                            Smb2Utility.CreateRandomString(DEFAULT_WRITE_BUFFER_SIZE_IN_KB),
                            checker: (header, response) => { });
                    }
                    HandleConflictResult(status);
                    break;
                case RequestType.Read:
                    if (fileId2.Persistent == 0)
                    {
                        status = secondClient.Create(
                            treeId2,
                            fileName,
                            CreateOptions_Values.FILE_NON_DIRECTORY_FILE,
                            out fileId2,
                            out contexts,
                            RequestedOplockLevel_Values.OPLOCK_LEVEL_NONE,
                            createDisposition: CreateDisposition_Values.FILE_OPEN,
                            checker: (header, response) => { });
                    }

                    if (fileId2.Persistent != 0)
                    {
                        byte[] data;
                        status = secondClient.Read(
                            treeId2,
                            fileId2,
                            0,
                            (uint)DEFAULT_WRITE_BUFFER_SIZE_IN_KB * 1024,
                            out data,
                            checker: (header, response) => { });
                    }
                    HandleConflictResult(status);
                    break;
                default:
                    break;
            }
        }

        private void OnLeaseBreakNotificationReceived(Packet_Header header, LEASE_BREAK_Notification_Packet notification)
        {
            // Set Lease breake state
            leaseBreakState = LeaseBreakState.LeaseBreakExisted;
        }

        private void HandleConflictResult(uint status)
        {
            // Wait 0.5 sec for lease break notification.
            Thread.Sleep(500);
            ConflictResponse((ModelSmb2Status)status, leaseBreakState);
        }

        /// <summary>
        /// The two client connects to the two IP addresses of scaleout file server
        /// Negotiate, SessionSetup, TreeConnect
        /// </summary>
        private Smb2FunctionalClient InitializeClient(IPAddress ip, out uint treeId)
        {
            Smb2FunctionalClient client = new Smb2FunctionalClient(testConfig.Timeout, testConfig, this.Site);
            client.ConnectToServerOverTCP(ip);
            client.Negotiate(
                Smb2Utility.GetDialects(DialectRevision.Smb21),
                testConfig.IsSMB1NegotiateEnabled);
            client.SessionSetup(
                testConfig.DefaultSecurityPackage,
                testConfig.ScaleOutFileServerName,
                testConfig.AccountCredential,
                testConfig.UseServerGssToken);
            client.TreeConnect(Smb2Utility.GetUncPath(testConfig.ScaleOutFileServerName, testConfig.CAShareName), out treeId);

            return client;
        }

        private void PrepareTestFile()
        {
            uint treeId;
            Smb2FunctionalClient client = InitializeClient(testConfig.ScaleOutFileServerIP1, out treeId);

            // Initialize file name
            fileName = GetTestFileName(Smb2Utility.GetUncPath(testConfig.ScaleOutFileServerName, testConfig.CAShareName));
            FILEID fileId;
            Smb2CreateContextResponse[] contexts;
            client.Create(
                treeId,
                fileName,
                CreateOptions_Values.FILE_NON_DIRECTORY_FILE,
                out fileId,
                out contexts);
            client.Write(treeId, fileId, Smb2Utility.CreateRandomString(DEFAULT_WRITE_BUFFER_SIZE_IN_KB));
            client.Close(treeId, fileId);
            client.LogOff();
            client.Disconnect();
        }
        #endregion
    }
}
