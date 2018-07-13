// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Net;
using System.Threading;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;

namespace Microsoft.Protocols.TestSuites.FileSharing.SMB2Model.Adapter.MixedOplockLease
{
    public class MixedOplockLeaseAdapter : ModelManagedAdapterBase, IMixedOplockLeaseAdapter
    {
        #region Event
        public event VerificationEventHandler Verification;
        #endregion

        #region Fields
        private string fileName;
        private Smb2FunctionalClient oplockClient;
        private Smb2FunctionalClient leaseClient;
        private uint treeIdOplock;
        private uint treeIdLease;
        private const int DEFAULT_WRITE_BUFFER_SIZE_IN_KB = 1;
        private ModelBreakType breakType;
        private bool alreadyRequested = false;
        private OplockLevel_Values grantedOplockLevel;
        private LeaseStateValues grantedLeaseState;
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
            if (oplockClient != null)
            {
                oplockClient.Disconnect();
                oplockClient = null;
            }

            if (leaseClient != null)
            {
                leaseClient.Disconnect();
                leaseClient = null;
            }

            fileName = null;
            breakType = ModelBreakType.NoBreak;
            treeIdOplock = 0;
            treeIdLease = 0;
            alreadyRequested = false;
            grantedLeaseState = LeaseStateValues.SMB2_LEASE_NONE;
            grantedOplockLevel = OplockLevel_Values.OPLOCK_LEVEL_NONE;

            base.Reset();
        }
        #endregion

        #region Actions
        /// <summary>
        /// Initialize the two clients. And register Oplock/Lease break notification event.
        /// </summary>
        public void Preparation()
        {
            oplockClient = InitializeClient(testConfig.SutIPAddress, out treeIdOplock);
            oplockClient.Smb2Client.OplockBreakNotificationReceived += new Action<Packet_Header, OPLOCK_BREAK_Notification_Packet>(OnLeaseBreakNotificationReceived);
            leaseClient = InitializeClient(testConfig.SutIPAddress, out treeIdLease);
            leaseClient.Smb2Client.LeaseBreakNotificationReceived += new Action<Packet_Header, LEASE_BREAK_Notification_Packet>(OnLeaseBreakNotificationReceived);
            fileName = GetTestFileName(Smb2Utility.GetUncPath(testConfig.SutComputerName, testConfig.BasicFileShare));
        }

        /// <summary>
        /// One client requests an Oplock.
        /// </summary>
        /// <param name="oplockLevel">Type of the oplock level</param>
        public void RequestOplock(OplockLevel_Values oplockLevel)
        {
            FILEID fileId;
            Smb2CreateContextResponse[] responses;

            oplockClient.Create(
                treeIdOplock,
                fileName,
                CreateOptions_Values.FILE_NON_DIRECTORY_FILE,
                out fileId,
                out responses,
                (RequestedOplockLevel_Values)oplockLevel,
                checker: (header, response) =>
                {
                    grantedOplockLevel = response.OplockLevel;
                });
            HandleResult();
        }

        /// <summary>
        /// One client requests a lease
        /// </summary>
        /// <param name="leaseStateType">Type of the LeaseState in the requested lease context</param>
        public void RequestLease(ModelLeaseStateType leaseStateType)
        {
            Smb2CreateContextResponse[] responses;
            FILEID fileId;
            LeaseStateValues leaseState = LeaseStateValues.SMB2_LEASE_NONE;
            switch (leaseStateType)
            {
                case ModelLeaseStateType.Lease_None:
                    leaseState = LeaseStateValues.SMB2_LEASE_NONE;
                    break;
                case ModelLeaseStateType.Lease_R:
                    leaseState = LeaseStateValues.SMB2_LEASE_READ_CACHING;
                    break;
                case ModelLeaseStateType.Lease_RH:
                    leaseState = LeaseStateValues.SMB2_LEASE_READ_CACHING | LeaseStateValues.SMB2_LEASE_HANDLE_CACHING;
                    break;
                case ModelLeaseStateType.Lease_RW:
                    leaseState = LeaseStateValues.SMB2_LEASE_READ_CACHING | LeaseStateValues.SMB2_LEASE_WRITE_CACHING;
                    break;
                case ModelLeaseStateType.Lease_RWH:
                    leaseState = LeaseStateValues.SMB2_LEASE_READ_CACHING | LeaseStateValues.SMB2_LEASE_WRITE_CACHING | LeaseStateValues.SMB2_LEASE_HANDLE_CACHING;
                    break;
                default:
                    break;
            }

            leaseClient.Create(
                treeIdLease,
                fileName,
                CreateOptions_Values.FILE_NON_DIRECTORY_FILE,
                out fileId,
                out responses,
                RequestedOplockLevel_Values.OPLOCK_LEVEL_LEASE,
                new Smb2CreateContextRequest[]
                {
                    new Smb2CreateRequestLease()
                    {
                        LeaseKey = Guid.NewGuid(),
                        LeaseState = leaseState
                    }
                });
            grantedLeaseState = ((Smb2CreateResponseLease)responses[0]).LeaseState;
            HandleResult();
        }
        #endregion

        #region Private methods
        /// <summary>
        /// Initialize the test client by 
        /// ConnectToServer, Negotiate, SessionSetup and TreeConnect
        /// </summary>
        private Smb2FunctionalClient InitializeClient(IPAddress ip, out uint treeId)
        {
            Smb2FunctionalClient client = new Smb2FunctionalClient(testConfig.Timeout, testConfig, this.Site);
            client.ConnectToServer(testConfig.UnderlyingTransport, testConfig.SutComputerName, ip);

            client.Negotiate(
                // Model cases only test Dialect lower than 3.11
                Smb2Utility.GetDialects(testConfig.MaxSmbVersionClientSupported < DialectRevision.Smb311 ? testConfig.MaxSmbVersionClientSupported : DialectRevision.Smb302),
                testConfig.IsSMB1NegotiateEnabled);
            client.SessionSetup(
                testConfig.DefaultSecurityPackage,
                testConfig.SutComputerName,
                testConfig.AccountCredential,
                testConfig.UseServerGssToken);
            client.TreeConnect(Smb2Utility.GetUncPath(testConfig.SutComputerName, testConfig.BasicFileShare), out treeId);
            return client;
        }

        private void HandleResult()
        {
            if (!alreadyRequested)
            {
                alreadyRequested = true;
                return;
            }

            // Wait 0.5 sec for lease break notification.
            Thread.Sleep(500);
            ModelLeaseStateType grantedLease = ModelLeaseStateType.Lease_None;
            switch (grantedLeaseState)
            {
                case LeaseStateValues.SMB2_LEASE_NONE:
                    grantedLease = ModelLeaseStateType.Lease_None;
                    break;
                case LeaseStateValues.SMB2_LEASE_READ_CACHING:
                    grantedLease = ModelLeaseStateType.Lease_R;
                    break;
                case LeaseStateValues.SMB2_LEASE_READ_CACHING | LeaseStateValues.SMB2_LEASE_WRITE_CACHING:
                    grantedLease = ModelLeaseStateType.Lease_RW;
                    break;
                case LeaseStateValues.SMB2_LEASE_READ_CACHING | LeaseStateValues.SMB2_LEASE_HANDLE_CACHING:
                    grantedLease = ModelLeaseStateType.Lease_RH;
                    break;
                case LeaseStateValues.SMB2_LEASE_READ_CACHING | LeaseStateValues.SMB2_LEASE_WRITE_CACHING | LeaseStateValues.SMB2_LEASE_HANDLE_CACHING:
                    grantedLease = ModelLeaseStateType.Lease_RWH;
                    break;

                default:
                    break;
            }

            // Verify if there's lease/oplock break and if the granted lease/oplock is correct for the second request.
            Verification(breakType, grantedOplockLevel, grantedLease);
        }

        private void OnLeaseBreakNotificationReceived(Packet_Header header, LEASE_BREAK_Notification_Packet notification)
        {
            // Set Lease break state
            breakType = ModelBreakType.LeaseBreak;
            leaseClient.LeaseBreakAcknowledgment(treeIdLease, notification.LeaseKey, notification.NewLeaseState);
        }

        private void OnLeaseBreakNotificationReceived(Packet_Header header, OPLOCK_BREAK_Notification_Packet notification)
        {
            breakType = ModelBreakType.OplockBreak;
            oplockClient.OplockAcknowledgement(treeIdOplock, notification.FileId, (OPLOCK_BREAK_Acknowledgment_OplockLevel_Values)notification.OplockLevel);
        }

        #endregion
    }
}
