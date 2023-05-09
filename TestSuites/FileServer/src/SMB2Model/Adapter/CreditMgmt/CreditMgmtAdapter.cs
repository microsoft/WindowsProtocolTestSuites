// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;
using System;

namespace Microsoft.Protocols.TestSuites.FileSharing.SMB2Model.Adapter.CreditMgmt
{
    public class CreditMgmtAdapter : ModelManagedAdapterBase, ICreditMgmtAdapter
    {
        #region Fields
        private Smb2FunctionalClient testClient;
        private CreditMgmtConfig config;
        private DialectRevision negotiateDialect;
        private bool isMultiCreditSupportedOnConnection;

        private string uncSharePath;
        private string fileName;
        private uint treeId;
        private FILEID fileId;

        private const uint Smb2PacketHeaderSizeInByte = 64;
        private const uint Smb2WriteRequestBodySizeInByte = 48;
        private const uint MaxNetbiosBufferSize = 65535;
        #endregion

        #region Initialization

        public override void Initialize(ITestSite testSite)
        {
            base.Initialize(testSite);
        }

        public override void Reset()
        {
            if (testClient != null)
            {
                try
                {
                    testClient.Close(treeId, fileId);
                    testClient.TreeDisconnect(treeId);
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
            }

            base.Reset();
        }

        #endregion

        #region Events

        public event CreditOperationEventHandler CreditOperationResponse;
        public event DisconnectionEventHandler ExpectDisconnect;

        #endregion

        #region Actions

        public void ReadConfig(out CreditMgmtConfig c)
        {
            uncSharePath = Smb2Utility.GetUncPath(testConfig.SutComputerName, testConfig.BasicFileShare);

            c = new CreditMgmtConfig
            {
                MaxSmbVersionSupported = ModelUtility.GetModelDialectRevision(testConfig.MaxSmbVersionSupported),
                Platform = testConfig.Platform >= Platform.WindowsServer2016 ? Platform.WindowsServer2012R2 : testConfig.Platform,
                IsMultiCreditSupportedOnServer = testConfig.IsMultiCreditSupported
            };

            config = c;

            Site.Log.Add(LogEntryKind.Debug, c.ToString());
        }

        public void SetupConnection(ModelDialectRevision clientMaxDialect)
        {
            testClient = new Smb2FunctionalClient(testConfig.Timeout, testConfig, this.Site);
            testClient.ConnectToServer(testConfig.UnderlyingTransport, testConfig.SutComputerName, testConfig.SutIPAddress);
            testClient.RequestSent += new Action<Packet_Header>(PrintSequenceWindow);

            DialectRevision[] dialects = Smb2Utility.GetDialects(ModelUtility.GetDialectRevision(clientMaxDialect));

            uint status;
            NEGOTIATE_Response? negotiateResponse = null;
            status = testClient.Negotiate(
                dialects,
                testConfig.IsSMB1NegotiateEnabled,
                capabilityValue: Capabilities_Values.GLOBAL_CAP_LARGE_MTU,
                checker: (header, response) =>
                {
                    Site.Assert.AreEqual(
                        Smb2Status.STATUS_SUCCESS,
                        header.Status,
                        "{0} should succeed", header.Command);

                    // The server MUST grant the client at least 1 credit when responding to SMB2 NEGOTIATE.
                    Site.Assert.IsTrue(
                        header.CreditRequestResponse >= 1,
                        "The server MUST grant the client at least 1 credit when responding to SMB2 NEGOTIATE");

                    negotiateResponse = response;
                });

            Site.Log.Add(
                LogEntryKind.Debug,
                "The maximum size, in bytes, of Length in READ/WRITE that server will accept on the connection is {0}",
                testClient.MaxBufferSize);

            Site.Assert.AreEqual(
                ModelUtility.GetDialectRevision(clientMaxDialect),
                negotiateResponse.Value.DialectRevision,
                "DialectRevision {0} is expected", ModelUtility.GetDialectRevision(clientMaxDialect));

            negotiateDialect = negotiateResponse.Value.DialectRevision;

            if ((negotiateDialect == DialectRevision.Smb21 || ModelUtility.IsSmb3xFamily(negotiateDialect))
                // In case server does not support multicredit even implement Smb21 or Smb30
                && testConfig.IsMultiCreditSupported)
            {
                isMultiCreditSupportedOnConnection = true;
            }
            else
            {
                isMultiCreditSupportedOnConnection = false;
            }

            status = testClient.SessionSetup(
                testConfig.DefaultSecurityPackage,
                testConfig.SutComputerName,
                testConfig.AccountCredential,
                testConfig.UseServerGssToken);

            status = testClient.TreeConnect(
                uncSharePath,
                out treeId);

            Smb2CreateContextResponse[] serverCreateContexts;
            fileName = GetTestFileName(uncSharePath);
            status = testClient.Create(
                treeId,
                fileName,
                CreateOptions_Values.FILE_NON_DIRECTORY_FILE,
                out fileId,
                out serverCreateContexts);
        }

        public void CreditOperationRequest(
            ModelMidType midType,
            ModelCreditCharge creditCharge,
            ModelCreditRequestNum creditRequestNum,
            ModelPayloadSize payloadSize,
            ModelPayloadType payloadType)
        {
            #region Customize message id

            ulong smallestAvailableMId = testClient.SequenceWindow.Min;
            ulong largestAvailableMId = testClient.SequenceWindow.Max;
            ulong customizedMId = 0;

            switch (midType)
            {
                case ModelMidType.UnavailableMid:
                    {
                        customizedMId = largestAvailableMId + 1;
                        break;
                    }
                case ModelMidType.UsedMid:
                    {
                        customizedMId = smallestAvailableMId - 1;
                        break;
                    }
                case ModelMidType.ValidMid:
                    {
                        customizedMId = (ulong)(smallestAvailableMId + largestAvailableMId) / 2;
                        break;
                    }
                default:
                    throw new ArgumentException("midType");
            }

            testClient.GenerateMessageId = (sequenceWindow) => customizedMId;

            Site.Log.Add(
                LogEntryKind.Debug,
                "*****customizedMId = {0}", customizedMId);

            #endregion

            #region Customize credit charge

            ushort customizedCreditCharge = 0;
            // Only customize credit charge when midType is valid
            if (midType == ModelMidType.ValidMid)
            {
                switch (creditCharge)
                {
                    case ModelCreditCharge.CreditChargeWithinBoundary:
                        {
                            customizedCreditCharge = (ushort)((largestAvailableMId - customizedMId + 2) / 2);
                            break;
                        }
                    case ModelCreditCharge.CreditChargeExceedBoundary:
                        {
                            customizedCreditCharge = (ushort)(largestAvailableMId - customizedMId + 2);
                            break;
                        }
                    case ModelCreditCharge.CreditChargeSetZero:
                        {
                            customizedCreditCharge = 0;
                            break;
                        }
                    default:
                        {
                            Site.Assume.Fail("Unexpected creditCharge {0}", creditCharge);
                            break;
                        }
                }
            }

            testClient.GenerateCreditCharge = (size) => customizedCreditCharge;

            Site.Log.Add(
                LogEntryKind.Debug,
                "*****customizedCreditCharge = {0}", customizedCreditCharge);

            #endregion

            #region Customize credit request

            ushort customizedCreditRequest = 0;
            switch (creditRequestNum)
            {
                case ModelCreditRequestNum.CreditRequestSetNonZero:
                    {
                        customizedCreditRequest = (ushort)((1 + testClient.CreditGoal) / 2);
                        break;
                    }
                case ModelCreditRequestNum.CreditRequestSetZero:
                    {
                        customizedCreditRequest = 0;
                        break;
                    }
                default:
                    {
                        Site.Assume.Fail("Unexpected creditRequestNum {0}", creditRequestNum);
                        break;
                    }
            }

            testClient.GenerateCreditRequest = (sequeceWindow, creditGoal, charge) => customizedCreditRequest;

            Site.Log.Add(
                LogEntryKind.Debug,
                "*****customizedCreditRequest = {0}", customizedCreditRequest);

            #endregion

            #region Calculate payload size

            int dataLengthInByte = 0;
            switch (payloadSize)
            {
                case ModelPayloadSize.PayloadSizeEqualToBoundary:
                    {
                        if (customizedCreditCharge == 0 && isMultiCreditSupportedOnConnection)
                        {
                            dataLengthInByte = 64 * 1024;
                        }
                        else if (!isMultiCreditSupportedOnConnection)
                        {
                            //3.3.5.2: If Connection.SupportsMultiCredit is FALSE and the size of the request is greater than 68*1024 bytes, the server SHOULD<200> terminate the connection
                            //Note 1. 68*1024 byte is total size of Smb2 packet including header, so need to subtract such overhead to calculate payload size
                            //     2. This case is only for WRITE but here does not distiguish READ and WRITE for simplicity

                            uint maxSize = 0;
                            if (testConfig.UnderlyingTransport == Smb2TransportType.NetBios)
                            {
                                maxSize = MaxNetbiosBufferSize;
                            }
                            else
                            {
                                maxSize = 68 * 1024;
                            }
                            
                            dataLengthInByte = (int)(maxSize - Smb2PacketHeaderSizeInByte - Smb2WriteRequestBodySizeInByte);
                        }
                        else
                        {
                            dataLengthInByte = customizedCreditCharge * 64 * 1024;
                        }
                        break;
                    }
                case ModelPayloadSize.PayloadSizeLargerThanBoundary:
                    {
                        if (customizedCreditCharge == 0 && isMultiCreditSupportedOnConnection)
                        {
                            dataLengthInByte = 64 * 1024 + 1;
                        }
                        else if (!isMultiCreditSupportedOnConnection)
                        {
                            //3.3.5.2: If Connection.SupportsMultiCredit is FALSE and the size of the request is greater than 68*1024 bytes, the server SHOULD<200> terminate the connection
                            //Note 1. 68*1024 byte is total size of Smb2 packet including header, so need to subtract such overhead to calculate payload size
                            //     2. This case is only for WRITE but here does not distiguish READ and WRITE for simplicity

                            uint maxSize = 0;
                            if (testConfig.UnderlyingTransport == Smb2TransportType.NetBios)
                            {
                                maxSize = MaxNetbiosBufferSize;
                            }
                            else
                            {
                                maxSize = 68 * 1024;
                            }
                            dataLengthInByte = (int)(maxSize - Smb2PacketHeaderSizeInByte - Smb2WriteRequestBodySizeInByte + 1);
                        }
                        else
                        {
                            dataLengthInByte = customizedCreditCharge * 64 * 1024 + 1;
                        }
                        break;
                    }
                case ModelPayloadSize.PayloadSizeLessThanBoundary:
                    {
                        if (customizedCreditCharge == 0 && isMultiCreditSupportedOnConnection)
                        {
                            dataLengthInByte = 32 * 1024;
                        }
                        else if (!isMultiCreditSupportedOnConnection)
                        {
                            dataLengthInByte = 34 * 1024;
                        }
                        else
                        {
                            dataLengthInByte = customizedCreditCharge * 32 * 1024;
                        }
                        break;
                    }
                default:
                    {
                        Site.Assume.Fail("Unexpected creditRequestNum {0}", creditRequestNum);
                        break;
                    }
            }

            Site.Log.Add(
                LogEntryKind.Debug,
                "*****dataLengthInByte = {0}", dataLengthInByte);

            #endregion

            #region Send request

            uint status = 0;
            uint creditResponse = 0;

            switch (payloadType)
            {
                case ModelPayloadType.RequestPayload:
                    {
                        try
                        {
                            string contentToWrite = Smb2Utility.CreateRandomStringInByte(dataLengthInByte);

                            testClient.Write(
                                treeId,
                                fileId,
                                contentToWrite,
                                checker: (header, response) =>
                                {
                                    status = header.Status;
                                    creditResponse = header.CreditRequestResponse;
                                });

                            if (dataLengthInByte > testClient.MaxBufferSize
                                && config.Platform != Platform.WindowsServer2008)
                            {
                                // The server MUST validate that the length to write is within its configured maximum write size.
                                // If not, it SHOULD<283> fail the request with STATUS_INVALID_PARAMETER.
                                // <283> Section 3.3.5.13: Windows 7 and Windows Server 2008 R2 fail the request with STATUS_BUFFER_OVERFLOW instead of
                                // STATUS_INVALID_PARAMETER if the Length field is greater than Connection.MaxWriteSize.
                                // Windows Vista SP1 and Windows Server 2008 do not validate the Length field in SMB2 Write Request.
                                if (config.Platform == Platform.WindowsServer2008R2)
                                {
                                    Site.Assert.AreEqual(
                                        ModelSmb2Status.STATUS_BUFFER_OVERFLOW,
                                        (ModelSmb2Status)status,
                                        "Windows 7, Windows Server 2008 R2 fail the request with STATUS_BUFFER_OVERFLOW if exceeds max write size");
                                }
                                else
                                {
                                    Site.Assert.AreNotEqual(
                                        ModelSmb2Status.STATUS_SUCCESS,
                                        (ModelSmb2Status)status,
                                        "Server SHOULD fail the request with STATUS_INVALID_PARAMETER if exceeds max write data");
                                }
                                // Bypass the situation when data length exceeds max size that connection allows
                                CreditOperationResponse(ModelSmb2Status.STATUS_SUCCESS, creditResponse, config);
                            }
                            else
                            {
                                if (config.Platform == Platform.WindowsServer2008
                                    && dataLengthInByte > testClient.MaxBufferSize)
                                {
                                    Site.Assert.AreEqual(
                                        ModelSmb2Status.STATUS_SUCCESS,
                                        (ModelSmb2Status)status,
                                        "Windows Vista SP1 and Windows Server 2008 do not validate the Length field in SMB2 Write Request.");

                                    // Bypass the situation when data length exceeds max size that connection allows
                                    CreditOperationResponse(ModelSmb2Status.STATUS_INVALID_PARAMETER, creditResponse, config);
                                }
                                else
                                {
                                    CreditOperationResponse((ModelSmb2Status)status, creditResponse, config);
                                }
                            }

                            PostOperation();
                        }
                        catch (Exception ex)
                        {
                            // Make sure testClient was set to null during disconnection correctly
                            // In case we catch a timeout exception first
                            // Temp fix for test suite bug 6349 as per discussion

                            // For an SMB2 Write request with an invalid MessageId, Windows 8 and Windows Server 2012 will stop processing 
                            // the request and any further requests on that connection.
                            // So Smb2Client will throw a timeout exception instead of disconnect event.
                            // The test case should handle the timeout exception the same with disconnect event.
                            if (ex is TimeoutException || testClient.Smb2Client.IsServerDisconnected)
                            {
                                OnServerDisconnected();
                            }
                            else
                            {
                                throw;
                            }
                            return;
                        }
                        break;
                    }
                case ModelPayloadType.ResponsePayload:
                    {
                        CreateFile(uncSharePath, fileName, dataLengthInByte);

                        try
                        {
                            string contentToRead = null;
                            testClient.Read(
                                treeId,
                                fileId,
                                0,
                                (uint)dataLengthInByte,
                                out contentToRead,
                                (header, response) =>
                                {
                                    status = header.Status;
                                    creditResponse = header.CreditRequestResponse;
                                });

                            if (dataLengthInByte > testClient.MaxBufferSize)
                            {
                                // The server MUST validate that the length to read is within its configured maximum read size.
                                // If not, it SHOULD<277> fail the request with STATUS_INVALID_PARAMETER.
                                // <277> Section 3.3.5.12: Windows 7 and Windows Server 2008 R2 fail the request with STATUS_BUFFER_OVERFLOW if the Length field is greater than Connection.MaxReadSize.
                                // Windows Vista SP1 and Windows Server 2008 will fail the request with STATUS_BUFFER_OVERFLOW if the Length field is greater than 524288(0x80000).
                                if (config.Platform == Platform.WindowsServer2008
                                    || config.Platform == Platform.WindowsServer2008R2)
                                {
                                    if (config.Platform == Platform.WindowsServer2008R2)
                                    {
                                        Site.Assert.AreEqual(
                                            ModelSmb2Status.STATUS_BUFFER_OVERFLOW,
                                            (ModelSmb2Status)status,
                                            "Windows 7, and Windows Server 2008 R2 fail the request with STATUS_BUFFER_OVERFLOW if exceeds max read size");
                                    }
                                    else if (dataLengthInByte > 524288)
                                    {
                                        Site.Assert.AreEqual(
                                            ModelSmb2Status.STATUS_BUFFER_OVERFLOW,
                                            (ModelSmb2Status)status,
                                            "Windows Vista SP1 and Windows Server 2008 will fail the request with STATUS_BUFFER_OVERFLOW if the Length field is greater than 524288");
                                    }
                                }
                                else if (config.Platform == Platform.WindowsServer2012
                                        || config.Platform == Platform.WindowsServer2012R2)
                                {
                                    // The server MUST validate that the length to read is within its configured maximum read size.
                                    // If not, it SHOULD<277> fail the request with STATUS_INVALID_PARAMETER.
                                    Site.Assert.AreEqual(
                                        ModelSmb2Status.STATUS_INVALID_PARAMETER,
                                        (ModelSmb2Status)status,
                                        "The server SHOULD fail the request with STATUS_INVALID_PARAMETER if the length is NOT within its configured maximum size.");
                                }
                                else // NonWindows
                                {
                                    Site.Assert.AreNotEqual(
                                        ModelSmb2Status.STATUS_SUCCESS,
                                        (ModelSmb2Status)status,
                                        "Server SHOULD fail the request with STATUS_INVALID_PARAMETER if exceeds max read size");
                                }
                                // Bypass the situation when data length exceeds max size that connection allows
                                CreditOperationResponse(ModelSmb2Status.STATUS_SUCCESS, creditResponse, config);
                            }
                            else
                            {
                                CreditOperationResponse((ModelSmb2Status)status, creditResponse, config);
                            }

                            PostOperation();
                        }
                        catch (Exception ex)
                        {
                            // Make sure testClient was set to null during disconnection correctly
                            // In case we catch a timeout exception first
                            // Temp fix for test suite bug 6349 as per discussion

                            // For an SMB2 Write request with an invalid MessageId, Windows 8 and Windows Server 2012 will stop processing 
                            // the request and any further requests on that connection.
                            // So Smb2Client will throw a timeout exception instead of disconnect event.
                            // The test case should handle the timeout exception the same with disconnect event.
                            if (ex is TimeoutException || testClient.Smb2Client.IsServerDisconnected)
                            {
                                OnServerDisconnected();
                            }
                            else
                            {
                                throw;
                            }
                            return;
                        }
                        break;
                    }
                default:
                    throw new ArgumentException("payloadType");
            }

            #endregion
        }

        #endregion

        #region Public Methods

        public void OnServerDisconnected()
        {
            testClient = null;
            ExpectDisconnect();
        }

        #endregion

        #region Private Methods

        private void CreateFile(string uncShare, string fileName, int lengthInByte)
        {
            Site.Log.Add(
                LogEntryKind.Debug,
                "Create file {0} in share {1}", fileName, uncShare);

            Smb2FunctionalClient client = new Smb2FunctionalClient(testConfig.Timeout, testConfig, this.Site);
            client.ConnectToServer(testConfig.UnderlyingTransport, testConfig.SutComputerName, testConfig.SutIPAddress);

            client.CreditGoal = 32;

            client.Negotiate(
                new DialectRevision[] { ModelUtility.GetDialectRevision(config.MaxSmbVersionSupported) },
                testConfig.IsSMB1NegotiateEnabled,
                capabilityValue: Capabilities_Values.GLOBAL_CAP_LARGE_MTU);

            client.SessionSetup(
                testConfig.DefaultSecurityPackage,
                testConfig.SutComputerName,
                testConfig.AccountCredential,
                testConfig.UseServerGssToken);

            uint tId;
            client.TreeConnect(
                uncShare,
                out tId);

            Smb2CreateContextResponse[] serverCreateContexts;
            FILEID fId;
            client.Create(
                tId,
                fileName,
                CreateOptions_Values.FILE_NON_DIRECTORY_FILE,
                out fId,
                out serverCreateContexts);

            string content;
            if (isMultiCreditSupportedOnConnection)
            {
                content = Smb2Utility.CreateRandomStringInByte(lengthInByte);
                client.Write(tId, fId, content);
            }
            else
            {
                // Write several times if server does not support multi credit
                int writeTimes = lengthInByte / (64 * 1024);
                int rest = lengthInByte % (64 * 1024);
                ulong offset = 0;

                for (int time = 0; time < writeTimes; time++)
                {
                    content = Smb2Utility.CreateRandomString(64);
                    client.Write(tId, fId, content, offset);
                    offset += 64 * 1024;
                }

                if (rest != 0)
                {
                    content = Smb2Utility.CreateRandomStringInByte(rest);
                    client.Write(tId, fId, content, offset);
                }
            }

            client.Close(tId, fId);

            client.TreeDisconnect(tId);

            client.LogOff();

            client.Disconnect();

            Site.Log.Add(
                LogEntryKind.Debug,
                "Create file {0} in share {1}", fileName, uncShare);
        }

        private void PostOperation()
        {
            // Reset generators
            testClient.GenerateMessageId = Smb2FunctionalClient.GetDefaultMId;
            testClient.GenerateCreditCharge = Smb2FunctionalClient.GetDefaultCreditCharge;
            testClient.GenerateCreditRequest = Smb2FunctionalClient.GetDefaultCreditRequest;

            // The server MUST ensure that the number of credits held by the client is never reduced to zero.
            Site.Assert.IsTrue(
                testClient.Credits > 0,
                "The server MUST ensure that the number of credits held by the client is never reduced to zero. Current credits held by client is {0}", testClient.Credits);
        }

        void PrintSequenceWindow(Packet_Header header)
        {
            Site.Log.Add(LogEntryKind.Debug, "Server grants {0} credits after {1}:", header.CreditRequestResponse, header.Command);
            Site.Log.Add(LogEntryKind.Debug, "SequenceWindow after {0}:", header.Command);

            foreach (var item in testClient.SequenceWindow)
            {
                Site.Log.Add(LogEntryKind.Debug, "\t{0}", item);
            }
        }
        #endregion
    }
}
