// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Net;
using System.Threading;
using Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;

namespace Microsoft.Protocols.TestSuites.FileSharing.SMB2Model.Adapter.Oplock
{
    public delegate void FileOperationDelegate(OplockFileOperation fileOperation, string fileName);

    public class OplockAdapter : ModelManagedAdapterBase, IOplockAdapter
    {
        #region Fields

        private string server;
        private string uncSharePath;
        private OplockConfig oplockConfig;
        private Smb2FunctionalClient testClient;
        private DialectRevision negotiatedDialect;
        private uint treeId;
        private FILEID fileId;
        private OplockLevel_Values oplockLevelOnOpen;
        private FileOperationDelegate fileOperationInvoker;

        #endregion

        #region Initialize and Reset

        public override void Initialize(ITestSite testSite)
        {
            base.Initialize(testSite);
        }

        public override void Reset()
        {
            try
            {
                testClient.LogOff();
                testClient.Disconnect();
            }
            catch
            {
            }
            finally
            {
                testClient = null;
            }

            base.Reset();
        }

        #endregion

        #region Events

        public event OplockBreakNotificationEventHandler OplockBreakNotification;
        public event OplockBreakResponseEventHandler OplockBreakResponse;

        #endregion

        #region Actions

        public void ReadConfig(out OplockConfig c)
        {
            c = new OplockConfig
            {
                Platform = testConfig.Platform,
                MaxSmbVersionSupported = ModelUtility.GetModelDialectRevision(testConfig.MaxSmbVersionSupported)
            };

            oplockConfig = c;
            Site.Log.Add(LogEntryKind.Debug, oplockConfig.ToString());
        }

        public void SetupConnection(ModelDialectRevision maxSmbVersionClientSupported, ModelShareFlag shareFlag, ModelShareType shareType)
        {
            IPAddress ip;
            if (shareType == ModelShareType.STYPE_CLUSTER_SOFS)
            {
                server = testConfig.ScaleOutFileServerName;
                ip = Dns.GetHostEntry(server).AddressList[0];

                if (shareFlag == ModelShareFlag.SMB2_SHAREFLAG_FORCE_LEVELII_OPLOCK)
                {
                    uncSharePath = Smb2Utility.GetUncPath(testConfig.ScaleOutFileServerName, testConfig.ShareWithForceLevel2AndSOFS);
                }
                else
                {
                    uncSharePath = Smb2Utility.GetUncPath(testConfig.ScaleOutFileServerName, testConfig.ShareWithoutForceLevel2WithSOFS);
                }
            }
            else
            {
                server = testConfig.SutComputerName;
                ip = testConfig.SutIPAddress;
                if (shareFlag == ModelShareFlag.SMB2_SHAREFLAG_FORCE_LEVELII_OPLOCK)
                {
                    uncSharePath = Smb2Utility.GetUncPath(testConfig.SutComputerName, testConfig.ShareWithForceLevel2WithoutSOFS);
                }
                else
                {
                    uncSharePath = Smb2Utility.GetUncPath(testConfig.SutComputerName, testConfig.ShareWithoutForceLevel2OrSOFS);
                }
            }

            testClient = new Smb2FunctionalClient(testConfig.Timeout, testConfig, this.Site);
            testClient.Smb2Client.OplockBreakNotificationReceived += new Action<Packet_Header, OPLOCK_BREAK_Notification_Packet>(OnOplockBreakNotificationReceived);

            testClient.ConnectToServer(testConfig.UnderlyingTransport, server, ip, testConfig.ClientNic1IPAddress);

            DialectRevision[] dialects = Smb2Utility.GetDialects(ModelUtility.GetDialectRevision(maxSmbVersionClientSupported));

            NEGOTIATE_Response? negotiateResponse = null;
            testClient.Negotiate(
                dialects,
                testConfig.IsSMB1NegotiateEnabled,
                checker: (header, response) =>
                {
                    Site.Assert.AreEqual(
                        Smb2Status.STATUS_SUCCESS,
                        header.Status,
                        "{0} should succeed", header.Command);

                    negotiateResponse = response;
                });

            negotiatedDialect = negotiateResponse.Value.DialectRevision;

            testClient.SessionSetup(
                testConfig.DefaultSecurityPackage,
                server,
                testConfig.AccountCredential,
                testConfig.UseServerGssToken);

            testClient.TreeConnect(
                uncSharePath,
                out treeId,
                checker: (header, response) =>
                {
                    Site.Assert.AreEqual(
                        Smb2Status.STATUS_SUCCESS,
                        header.Status,
                        "{0} should succeed", header.Command);

                    Site.Assert.AreEqual(
                        shareFlag == ModelShareFlag.SMB2_SHAREFLAG_FORCE_LEVELII_OPLOCK,
                        response.ShareFlags.HasFlag(ShareFlags_Values.SHAREFLAG_FORCE_LEVELII_OPLOCK),
                        "SHAREFLAG_FORCE_LEVELII_OPLOCK is{0}expected to be set",
                        shareFlag == ModelShareFlag.SMB2_SHAREFLAG_FORCE_LEVELII_OPLOCK ? " " : " not ");

                    if (ModelUtility.IsSmb3xFamily(negotiateResponse.Value.DialectRevision))
                    {
                        Site.Assert.AreEqual(
                            shareType == ModelShareType.STYPE_CLUSTER_SOFS,
                            response.Capabilities.HasFlag(Share_Capabilities_Values.SHARE_CAP_SCALEOUT),
                            "SHARE_CAP_SCALEOUT is{0}expected to be set",
                            shareType == ModelShareType.STYPE_CLUSTER_SOFS ? " " : " not ");
                    }
                });

        }

        public void RequestOplockAndOperateFileRequest(
            RequestedOplockLevel_Values requestedOplockLevel,
            OplockFileOperation fileOperation,
            out OplockLevel_Values grantedOplockLevel,
            out OplockConfig c)
        {
            Smb2CreateContextResponse[] serverCreateContexts;
            string fileName = GetTestFileName(uncSharePath);

            OplockLevel_Values grantedTmp = OplockLevel_Values.OPLOCK_LEVEL_NONE;
            testClient.Create(
                    treeId,
                    fileName,
                    CreateOptions_Values.FILE_NON_DIRECTORY_FILE,
                    out fileId,
                    out serverCreateContexts,
                    requestedOplockLevel_Values: requestedOplockLevel,
                    checker: (header, response) =>
                    {
                        grantedTmp = response.OplockLevel;
                    });

            Site.Log.Add(LogEntryKind.Debug, "OplockLevel granted by server: {0}", grantedTmp);
            grantedOplockLevel = grantedTmp;
            c = oplockConfig;

            oplockLevelOnOpen = grantedTmp;

            fileOperationInvoker = new FileOperationDelegate(FileOperation);
            fileOperationInvoker.BeginInvoke(fileOperation, fileName, null, null);

            // Assume that notification will arrive in .5 sec if there's any
            // Without such sleep, OplockBreakNotification would not be expected by SE
            Thread.Sleep(500);

        }

        public void OplockBreakAcknowledgementRequest(OplockVolatilePortion volatilePortion, OplockPersistentPortion persistentPortion, OplockLevel_Values oplockLevel)
        {

            bool volatilePortionFound = volatilePortion == OplockVolatilePortion.VolatilePortionFound;
            bool persistentMatchesDurableFileId = persistentPortion == OplockPersistentPortion.PersistentMatchesDurableFileId;
            FILEID fileIdRequest = fileId;

            if (!volatilePortionFound)
            {
                fileIdRequest.Volatile = FILEID.Invalid.Volatile;
            }

            if (!persistentMatchesDurableFileId)
            {
                fileIdRequest.Persistent = FILEID.Invalid.Persistent;
            }

            OPLOCK_BREAK_Response? oplockBreakResponse = null;
            uint status = testClient.OplockAcknowledgement(treeId, fileIdRequest, (OPLOCK_BREAK_Acknowledgment_OplockLevel_Values)oplockLevel, (header, response) => { oplockBreakResponse = response; });

            OplockBreakResponse((ModelSmb2Status)status, oplockBreakResponse.Value.OplockLevel, oplockLevelOnOpen);
        }

        #endregion

        #region Private methods

        private void OnOplockBreakNotificationReceived(Packet_Header header, OPLOCK_BREAK_Notification_Packet packet)
        {
            Site.Log.Add(LogEntryKind.Debug, "OplockBreakNotification was received from server");

            Site.Assert.AreEqual(
                Smb2Command.OPLOCK_BREAK,
                header.Command,
                "The server MUST set the Command in the SMB2 header to SMB2 OPLOCK_BREAK");

            Site.Assert.AreEqual(
                0xFFFFFFFFFFFFFFFF,
                header.MessageId,
                "The server MUST set the MessageId to 0xFFFFFFFFFFFFFFFF");

            Site.Assert.AreEqual(
                (uint)0,
                header.TreeId,
                "The server MUST set the TreeId to 0");

            Site.Assert.AreEqual(
                fileId,
                packet.FileId,
                "The FileId field of the response structure MUST be set to the values from the Open structure, with the volatile part set to Open.FileId and the persistent part set to Open.DurableFileId");

            // Verify signature is set to 0 if it's not signed
            Site.Assert.AreEqual(
                true,
                BitConverter.ToUInt64(header.Signature, 0) == 0 && BitConverter.ToUInt64(header.Signature, 8) == 0,
                "The SMB2 Oplock Break Notification is sent to the client. The message MUST NOT be signed, as specified in section 3.3.4.1.1");

            OplockBreakNotification(packet.OplockLevel);
        }

        private void FileOperation(OplockFileOperation fileOperation, string fileName)
        {
            Site.Log.Add(LogEntryKind.Debug, "File operation on same file from another client");

            Smb2FunctionalClient clientForAnotherOpen = new Smb2FunctionalClient(testConfig.Timeout, testConfig, this.Site);
            clientForAnotherOpen.ConnectToServer(testConfig.UnderlyingTransport, testConfig.SutComputerName, testConfig.SutIPAddress, testConfig.ClientNic1IPAddress);

            clientForAnotherOpen.Negotiate(new DialectRevision[] { negotiatedDialect }, testConfig.IsSMB1NegotiateEnabled);

            clientForAnotherOpen.SessionSetup(
                testConfig.DefaultSecurityPackage,
                server,
                testConfig.AccountCredential,
                testConfig.UseServerGssToken);

            uint treeIdForAnotherOpen;
            clientForAnotherOpen.TreeConnect(
                uncSharePath,
                out treeIdForAnotherOpen);

            FILEID fileIdForAnotherOpen;
            Smb2CreateContextResponse[] serverCreateContexts;

            clientForAnotherOpen.Create(
                treeIdForAnotherOpen,
                fileName,
                CreateOptions_Values.FILE_NON_DIRECTORY_FILE,
                out fileIdForAnotherOpen,
                out serverCreateContexts);

            if (fileOperation == OplockFileOperation.WriteFromAnotherOpen)
            {
                string writeContent = Smb2Utility.CreateRandomString(1);

                clientForAnotherOpen.Write(treeIdForAnotherOpen, fileIdForAnotherOpen, writeContent);
            }

        }

        #endregion

    }
}
