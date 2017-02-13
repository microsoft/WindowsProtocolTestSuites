// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Reflection;
    using System.Text;

    using Microsoft.Protocols.TestTools;
    using Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc;
    using Microsoft.Protocols.TestTools.StackSdk.Security.Sspi;
    using Microsoft.SpecExplorer.Runtime.Testing;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Tradtional test case, used to test SHOULD behaviors.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design",
                                                     "CA1001:TypesThatOwnDisposableFieldsShouldBeDisposable")]
    [TestClass]
    public class TraditionalTestCase : TestClassBase
    {
        #region Preperty
        /// <summary>
        /// Parameter nrpcServerSutControlAdapter.
        /// </summary>
        private static INrpcServerSutControlAdapter nrpcServerSutControlAdapter;

        /// <summary>
        /// Parameter testSite.
        /// </summary>
        private static ITestSite testSite = null;

        /// <summary>
        /// Parameter nrpcServerAdapter.
        /// </summary>
        private static INrpcServerAdapter nrpcServerAdapter;

        /// <summary>
        /// Parameter nrpcClient.
        /// </summary>
        private NrpcClient nrpcClient;
        #endregion

        #region Initialize and Cleanup
        /// <summary>
        /// Initializes the test suite base class. This method must be called by class
        /// initialize method in your test class.
        /// </summary>
        /// <param name="testContext">
        ///  Used to store information that is provided to unit tests.
        /// </param>
        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            TestClassBase.Initialize(testContext, "AD_ServerTestSuite");
        }

        /// <summary>
        /// Cleans up the test suite.
        /// </summary>
        [ClassCleanup]
        public static void ClassCleanup()
        {
            TestClassBase.Cleanup();
        }


        #endregion

        #region TestMethod

        /// <summary>
        /// TestID: Traditional01_VerifyMsvApSecureChannel
        /// Description: Test SHOULD behavior when MsvApSecureChannel is used as secure channel type.
        /// Requirement covered:MS-NRPC_R1554,MS-NRPC_R1557
        /// </summary>
        [TestMethod]
        [TestCategory("BVT")]
        [TestCategory("PDC")]
        [TestCategory("MS-NRPC")]
        [TestCategory("DomainWin2008R2")]
        public void NRPC_Traditional01_VerifyMsvApSecureChannel()
        {
            PlatformType platform;
            HRESULT result;

            Site.Log.Add(LogEntryKind.Comment, "Call GetPlatform");

            nrpcServerAdapter.GetPlatform(out platform);

            Site.Log.Add(LogEntryKind.Comment, "Return GetPlatform(out {0})", platform);


            Site.Log.Add(LogEntryKind.Comment, "Call ConfigServer");

            nrpcServerSutControlAdapter.ConfigServer(true, false);

            Site.Log.Add(LogEntryKind.Comment, "Return ConfigServer");


            Site.Log.Add(
                LogEntryKind.Comment,
                "Call NetrServerReqChallenge({0},{1})",
                ComputerType.PrimaryDc,
                ComputerType.Client);

            result = nrpcServerAdapter.NetrServerReqChallenge(ComputerType.PrimaryDc, ComputerType.Client);

            Site.Log.Add(LogEntryKind.Comment, "Return NetrServerReqChallenge/{0}", result);
            Assert.AreEqual(HRESULT.ERROR_SUCCESS, result);

            Site.Log.Add(
                LogEntryKind.Comment,
                "Call NetrServerAuthenticate3({0},{1},{2},{3},{4},{5})",
                ComputerType.PrimaryDc,
                AccounterNameType.DomainMemberComputerAccount,
                _NETLOGON_SECURE_CHANNEL_TYPE.MsvApSecureChannel,
                ComputerType.Client,
                true,
                (uint)(NrpcNegotiateFlags.SupportsStrongKeys | NrpcNegotiateFlags.SupportsRC4
                    | NrpcNegotiateFlags.SupportsRefusePasswordChange));

            result = nrpcServerAdapter.NetrServerAuthenticate3(
                ComputerType.PrimaryDc,
                AccounterNameType.DomainMemberComputerAccount,
                _NETLOGON_SECURE_CHANNEL_TYPE.MsvApSecureChannel,
                ComputerType.Client,
                true,
                (uint)(NrpcNegotiateFlags.SupportsStrongKeys | NrpcNegotiateFlags.SupportsRC4
                    | NrpcNegotiateFlags.SupportsRefusePasswordChange));

            Site.Log.Add(LogEntryKind.Comment, "Return NetrServerAuthenticate3/{0}", result);


            string requirementID1554Implemented = Site.Properties.Get("SHOULDMAY.R1554Implementation");
            if (platform != PlatformType.NonWindows)
            {
                // Verify MS-NRPC requirement: MS-NRPC_R1557
                Site.CaptureRequirementIfAreEqual<HRESULT>(
                    HRESULT.STATUS_INVALID_PARAMETER,
                    result,
                    1557,
                    @"[In NETLOGON_SECURE_CHANNEL_TYPE enumeration]MsvApSecureChannel:In Windows,[if this value is 
                    used in the Netlogon RPC calls between a client and a remote server,]the error code 
                    STATUS_INVALID_PARAMETER is returned.");

                if (requirementID1554Implemented == null)
                {
                    Site.Properties.Add("SHOULDMAY.R1554Implementation", bool.TrueString);
                    requirementID1554Implemented = bool.TrueString;
                }
            }

            if (requirementID1554Implemented != null)
            {
                bool isR1554Implemented = bool.Parse(requirementID1554Implemented);
                bool isSatisfied = result == HRESULT.STATUS_INVALID_PARAMETER;

                // Verify MS-NRPC requirement: MS-NRPC_R1554
                Site.CaptureRequirementIfAreEqual<bool>(
                    isR1554Implemented,
                    isSatisfied,
                    1554,
                    string.Format(CultureInfo.InvariantCulture,
                        @"[In NETLOGON_SECURE_CHANNEL_TYPE enumeration]MsvApSecureChannel:[If this value is 
                        used in the Netlogon RPC calls between a client and a remote server,]The error code 
                        STATUS_INVALID_PARAMETER SHOULD be returned.This requirement is {0} implemented",
                        isSatisfied ? string.Empty : "not"));
            }
        }

        /// <summary>
        /// TestID: Traditional02_VerifyUasServerSecureChannel
        /// Description: Test SHOULD behavior when UasServerSecureChannel is used as secure channel type.
        /// Requirement covered:MS-NRPC_R1565,MS-NRPC_R103416
        /// </summary>
        [TestMethod]
        [TestCategory("BVT")]
        [TestCategory("PDC")]
        [TestCategory("MS-NRPC")]
        [TestCategory("DomainWin2008R2")]
        public void NRPC_Traditional02_VerifyUasServerSecureChannel()
        {
            PlatformType platform;
            HRESULT result;

            Site.Log.Add(LogEntryKind.Comment, "Call GetPlatform");

            nrpcServerAdapter.GetPlatform(out platform);

            Site.Log.Add(LogEntryKind.Comment, "Return GetPlatform(out {0})", platform);


            Site.Log.Add(LogEntryKind.Comment, "Call ConfigServer");

            nrpcServerSutControlAdapter.ConfigServer(true, false);

            Site.Log.Add(LogEntryKind.Comment, "Return ConfigServer");

            Site.Log.Add(
                LogEntryKind.Comment,
                "Call NetrServerReqChallenge({0},{1})",
                ComputerType.PrimaryDc,
                ComputerType.Client);

            result = nrpcServerAdapter.NetrServerReqChallenge(ComputerType.PrimaryDc, ComputerType.Client);

            Site.Log.Add(LogEntryKind.Comment, "Return NetrServerReqChallenge/{0}", result);
            Assert.AreEqual(HRESULT.ERROR_SUCCESS, result);

            Site.Log.Add(
                LogEntryKind.Comment,
                "Call NetrServerAuthenticate3({0},{1},{2},{3},{4},{5})",
                ComputerType.PrimaryDc,
                AccounterNameType.DomainMemberComputerAccount,
                _NETLOGON_SECURE_CHANNEL_TYPE.UasServerSecureChannel,
                ComputerType.Client,
                true,
                (uint)(NrpcNegotiateFlags.SupportsStrongKeys | NrpcNegotiateFlags.SupportsRC4
                    | NrpcNegotiateFlags.SupportsRefusePasswordChange));

            result = nrpcServerAdapter.NetrServerAuthenticate3(
                ComputerType.PrimaryDc,
                AccounterNameType.DomainMemberComputerAccount,
                _NETLOGON_SECURE_CHANNEL_TYPE.UasServerSecureChannel,
                ComputerType.Client,
                true,
                (uint)(NrpcNegotiateFlags.SupportsStrongKeys | NrpcNegotiateFlags.SupportsRC4
                    | NrpcNegotiateFlags.SupportsRefusePasswordChange));

            Site.Log.Add(LogEntryKind.Comment, "Return NetrServerAuthenticate3/{0}", result);

            string requirementID1565Implemented = Site.Properties.Get("SHOULDMAY.R1565Implementation");
            if (platform != PlatformType.NonWindows)
            {
                if (platform == PlatformType.Windows2000 || platform == PlatformType.WindowsXp ||
                    platform == PlatformType.WindowsServer2003 || platform == PlatformType.WindowsVista ||
                    platform == PlatformType.WindowsServer2008 || platform == PlatformType.Windows7 ||
                    platform == PlatformType.WindowsServer2008R2)
                {
                    // <174> Section 3.5.5.3.2: For Windows 2000, Windows XP, Windows Server 2003, Windows Vista, 
                    // Windows Server 2008, Windows 7, and Windows Server 2008 R2, 
                    // if the value is 5 (UasServerSecureChannel), the server always returns an access-denied error 
                    // because this functionality is no longer supported. 

                    // Verify MS-NRPC requirement: MS-NRPC_R103416
                    Site.CaptureRequirementIfAreEqual<HRESULT>(
                        HRESULT.STATUS_ACCESS_DENIED,
                        result,
                        103416,
                        @"<176> Section 3.5.5.3.2: [for NetrServerAuthenticate3 ] For Windows 2000, Windows XP, 
                        Windows Server 2003, Windows Vista, Windows Server 2008, Windows 7, and Windows Server 2008 R2,
                        if the value is 5 (UasServerSecureChannel), the server always returns an access-denied error 
                        because this functionality is no longer supported.");

                    if (requirementID1565Implemented == null)
                    {
                        Site.Properties.Add("SHOULDMAY.R1565Implementation", bool.FalseString);
                        requirementID1565Implemented = bool.FalseString;
                    }
                }
                else if (platform == PlatformType.WindowsNT4_0SP2 || platform == PlatformType.WindowsNT4_0SP4)
                {
                    // Windows NT 4.0 has configuration parameter options allowing UAS compatibility mode, and if this 
                    // mode is enabled, the error is not returned and 
                    // further processing occurs. Otherwise, it returns an access-denied error.
                    if (requirementID1565Implemented == null)
                    {
                        Site.Properties.Add("SHOULDMAY.R1565Implementation", bool.TrueString);
                        requirementID1565Implemented = bool.TrueString;
                    }
                }
            }

            if (requirementID1565Implemented != null)
            {
                bool isR1565Implemented = bool.Parse(requirementID1565Implemented);
                bool isSatisfied = result == HRESULT.STATUS_INVALID_PARAMETER;

                // Verify MS-NRPC requirement: MS-NRPC_R1565
                Site.CaptureRequirementIfAreEqual<bool>(
                    isR1565Implemented,
                    isSatisfied,
                    1565,
                    string.Format(CultureInfo.InvariantCulture,
                        @"[In NETLOGON_SECURE_CHANNEL_TYPE enumeration]UasServerSecureChannel:[If it is used 
                        in the Netlogon RPC calls between a client and a remote server]The error code 
                        STATUS_INVALID_PARAMETER SHOULD be returned.This requirement is {0} implemented",
                        isSatisfied ? string.Empty : "not"));
            }
        }

        /// <summary>
        /// TestID: Traditional03_VerifyNetrAccountDeltas
        /// Description: Test SHOULD behavior when NetrAccountDeltas is called
        /// Requirement Coptured: MS-NRPC_R104037, MS-NRPC_R104036
        /// </summary>
        [TestMethod]
        [TestCategory("BVT")]
        [TestCategory("PDC")]
        [TestCategory("MS-NRPC")]
        [TestCategory("DomainWin2008R2")]
        public void NRPC_Traditional03_VerifyNetrAccountDeltas()
        {
            PlatformType platform;
            HRESULT result;

            Site.Log.Add(LogEntryKind.Comment, "Call GetPlatform");

            nrpcServerAdapter.GetPlatform(out platform);

            Site.Log.Add(LogEntryKind.Comment, "Return GetPlatform(out {0})", platform);


            Site.Log.Add(
                LogEntryKind.Comment,
                "Call NetrServerReqChallenge({0},{1})",
                ComputerType.PrimaryDc,
                ComputerType.Client);

            result = nrpcServerAdapter.NetrServerReqChallenge(ComputerType.PrimaryDc, ComputerType.Client);

            Site.Log.Add(LogEntryKind.Comment, "Return NetrServerReqChallenge/{0}", result);
            Assert.AreEqual(HRESULT.ERROR_SUCCESS, result);

            Site.Log.Add(
                LogEntryKind.Comment,
                "Call NetrServerAuthenticate3({0},{1},{2},{3},{4},{5})",
                ComputerType.PrimaryDc,
                AccounterNameType.DomainMemberComputerAccount,
                _NETLOGON_SECURE_CHANNEL_TYPE.UasServerSecureChannel,
                ComputerType.Client,
                true,
                (uint)(NrpcNegotiateFlags.SupportsStrongKeys | NrpcNegotiateFlags.SupportsRC4
                    | NrpcNegotiateFlags.SupportsRefusePasswordChange));

            result = nrpcServerAdapter.NetrServerAuthenticate3(
                ComputerType.PrimaryDc,
                AccounterNameType.DomainMemberComputerAccount,
                _NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel,
                ComputerType.Client,
                true,
                (uint)(NrpcNegotiateFlags.SupportsStrongKeys | NrpcNegotiateFlags.SupportsRC4
                    | NrpcNegotiateFlags.SupportsRefusePasswordChange));

            Site.Log.Add(LogEntryKind.Comment, "Return NetrServerAuthenticate3/{0}", result);
            Assert.AreEqual(HRESULT.ERROR_SUCCESS, result);


            Site.Log.Add(LogEntryKind.Comment, "Call NetrAccountDeltas())");

            result = nrpcServerAdapter.NetrAccountDeltas();

            Site.Log.Add(LogEntryKind.Comment, "Return NetrAccountDeltas");


            string requirementID104036Implemented = Site.Properties.Get("SHOULDMAY.R104036Implementation");
            if (platform != PlatformType.NonWindows)
            {
                // Verify MS-NRPC requirement: MS-NRPC_R104037
                Site.CaptureRequirementIfAreEqual<HRESULT>(
                    HRESULT.STATUS_NOT_IMPLEMENTED,
                    result,
                    104037,
                    @"<243> Section 3.5.5.9.3: The Netlogon server returns 
                    STATUS_NOT_IMPLEMENTED [for NetrAccountDeltas method].");

                if (requirementID104036Implemented == null)
                {
                    Site.Properties.Add("SHOULDMAY.R104036Implementation", bool.TrueString);
                    requirementID104036Implemented = bool.TrueString;
                }
            }

            if (requirementID104036Implemented != null)
            {
                bool isR104036Implemented = bool.Parse(requirementID104036Implemented);
                bool isSatisfied = result != HRESULT.ERROR_SUCCESS;

                // Verify MS-NRPC requirement: MS-NRPC_R104036
                Site.CaptureRequirementIfAreEqual<bool>(
                    isR104036Implemented,
                    isSatisfied,
                    104036,
                    string.Format(CultureInfo.InvariantCulture,
                        @"[In NetrAccountDeltas (Opnum 9), this method]SHOULD<243> be rejected with an error 
                        code. This requirement is {0} implemented",
                        isSatisfied ? string.Empty : "not"));
            }
        }

        /// <summary>
        /// TestID: Traditional04_VerifyNetrAccountSync
        /// Description: Test SHOULD behavior when NetrAccountSync is called
        /// Requirement Coptured: MS-NRPC_R104040, MS-NRPC_R104039
        /// </summary>
        [TestMethod]
        [TestCategory("BVT")]
        [TestCategory("PDC")]
        [TestCategory("MS-NRPC")]
        [TestCategory("DomainWin2008R2")]
        public void NRPC_Traditional04_VerifyNetrAccountSync()
        {
            PlatformType platform;
            HRESULT result;

            Site.Log.Add(LogEntryKind.Comment, "Call GetPlatform");

            nrpcServerAdapter.GetPlatform(out platform);

            Site.Log.Add(LogEntryKind.Comment, "Return GetPlatform(out {0})", platform);


            Site.Log.Add(
                LogEntryKind.Comment,
                "Call NetrServerReqChallenge({0},{1})",
                ComputerType.PrimaryDc,
                ComputerType.Client);

            result = nrpcServerAdapter.NetrServerReqChallenge(ComputerType.PrimaryDc, ComputerType.Client);

            Site.Log.Add(LogEntryKind.Comment, "Return NetrServerReqChallenge/{0}", result);
            Assert.AreEqual(HRESULT.ERROR_SUCCESS, result);

            Site.Log.Add(
                LogEntryKind.Comment,
                "Call NetrServerAuthenticate3({0},{1},{2},{3},{4},{5})",
                ComputerType.PrimaryDc,
                AccounterNameType.DomainMemberComputerAccount,
                _NETLOGON_SECURE_CHANNEL_TYPE.UasServerSecureChannel,
                ComputerType.Client,
                true,
                (uint)(NrpcNegotiateFlags.SupportsStrongKeys | NrpcNegotiateFlags.SupportsRC4
                    | NrpcNegotiateFlags.SupportsRefusePasswordChange));

            result = nrpcServerAdapter.NetrServerAuthenticate3(
                ComputerType.PrimaryDc,
                AccounterNameType.DomainMemberComputerAccount,
                _NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel,
                ComputerType.Client,
                true,
                (uint)(NrpcNegotiateFlags.SupportsStrongKeys | NrpcNegotiateFlags.SupportsRC4
                | NrpcNegotiateFlags.SupportsRefusePasswordChange));

            Site.Log.Add(LogEntryKind.Comment, "Return NetrServerAuthenticate3/{0}", result);
            Assert.AreEqual(HRESULT.ERROR_SUCCESS, result);


            Site.Log.Add(LogEntryKind.Comment, "Call NetrAccountDeltas())");

            result = nrpcServerAdapter.NetrAccountSync();

            Site.Log.Add(LogEntryKind.Comment, "Return NetrAccountDeltas");

            string requirementID104039Implemented = Site.Properties.Get("SHOULDMAY.R104039Implementation");
            if (platform != PlatformType.NonWindows)
            {
                // Verify MS-NRPC requirement: MS-NRPC_R104040
                Site.CaptureRequirementIfAreEqual<HRESULT>(
                    HRESULT.STATUS_NOT_IMPLEMENTED,
                    result,
                    104040,
                    @"<244> Section 3.5.5.9.4: The Netlogon server returns 
                    STATUS_NOT_IMPLEMENTED [for NetrAccountSync method].");

                if (requirementID104039Implemented == null)
                {
                    Site.Properties.Add("SHOULDMAY.R104039Implementation", bool.TrueString);
                    requirementID104039Implemented = bool.TrueString;
                }
            }

            if (requirementID104039Implemented != null)
            {
                bool isR104039Implemented = bool.Parse(requirementID104039Implemented);
                bool isSatisfied = result != HRESULT.ERROR_SUCCESS;

                // Verify MS-NRPC requirement: MS-NRPC_R104039
                Site.CaptureRequirementIfAreEqual<bool>(
                    isR104039Implemented,
                    isSatisfied,
                    104039,
                    string.Format(CultureInfo.InvariantCulture,
                        @"[In NetrAccountSync (Opnum 10), this method]SHOULD<244> be rejected with an error 
                        code. This requirement is {0} implemented",
                        isSatisfied ? string.Empty : "not"));
            }
        }

        /// <summary>
        /// TestID: Traditional05_VerifyOutOfSequenceMessage
        /// Description: Test the server behavior if an out of sequence message is send by the client.
        /// Requirement covered:MS-NRPC_R853
        /// </summary>
        [TestMethod]
        [TestCategory("BVT")]
        [TestCategory("PDC")]
        [TestCategory("MS-NRPC")]
        [TestCategory("DomainWin2008R2")]
        public void NRPC_Traditional05_VerifyOutOfSequenceMessage()
        {
            PlatformType platform;
            HRESULT result = HRESULT.ERROR_SUCCESS;

            Site.Log.Add(LogEntryKind.Comment, "Call GetPlatform");

            nrpcServerAdapter.GetPlatform(out platform);

            Site.Log.Add(LogEntryKind.Comment, "Return GetPlatform(out {0})", platform);


            Site.Log.Add(LogEntryKind.Comment, "Call ConfigServer");

            nrpcServerSutControlAdapter.ConfigServer(true, false);

            Site.Log.Add(LogEntryKind.Comment, "Return ConfigServer");

            NrpcServerAdapter adapter = new NrpcServerAdapter();
            adapter.Initialize(Site);

            using (this.nrpcClient = NrpcClient.CreateNrpcClient(adapter.PrimaryDomainDnsName))
            {
                this.nrpcClient.Context.NegotiateFlags = (NrpcNegotiateFlags)NrpcServerAdapter.NegotiateFlags;
                MachineAccountCredential machineCredential = new MachineAccountCredential(
                        adapter.PrimaryDomainDnsName,
                        adapter.ENDPOINTNetbiosName,
                        adapter.ENDPOINTPassword);
                NrpcClientSecurityContext secuContext = new NrpcClientSecurityContext(
                    adapter.PrimaryDomainDnsName,
                    adapter.PDCNetbiosName,
                    machineCredential,
                    true,
                    this.nrpcClient.Context.NegotiateFlags);
                AccountCredential accountCredential = new AccountCredential(
                    adapter.PrimaryDomainDnsName,
                    adapter.DomainAdministratorName,
                    adapter.DomainUserPassword);
                try
                {
                    this.nrpcClient.BindOverNamedPipe(
                            adapter.PDCNetbiosName,
                            accountCredential,
                            secuContext,
                            adapter.timeOut);
                }
                catch (Exception e)
                {
                    Site.Log.Add(LogEntryKind.Debug, "Failed to bind NamedPipe to " + adapter.PDCNetbiosName + "due to reason: " + e.Message);
                    Site.Assert.Fail("Failed on init NRPC client on transport NamedPipe");
                }
                this.nrpcClient.Context.SequenceNumber++;

                _NETLOGON_LOGON_INFO_CLASS logonLevelInfoClass = _NETLOGON_LOGON_INFO_CLASS.NetlogonInteractiveInformation;
                _NETLOGON_LEVEL logonLevel = this.nrpcClient.CreateNetlogonLevel(
                    logonLevelInfoClass,
                    NrpcParameterControlFlags.AllowLogonWithComputerAccount,
                    adapter.PrimaryDomainDnsName,
                    adapter.DomainAdministratorName,
                    adapter.DomainUserPassword);

                // Client calls EncryptNetlogonLevel
                _NETLOGON_LEVEL? encryptedLogonLevel = this.nrpcClient.EncryptNetlogonLevel(
                   logonLevelInfoClass,
                   logonLevel);
                _NETLOGON_AUTHENTICATOR? serverAuthenticator = this.nrpcClient.CreateEmptyNetlogonAuthenticator();
                _NETLOGON_AUTHENTICATOR? clientAuthenticator = this.nrpcClient.ComputeNetlogonAuthenticator();
                _NETLOGON_VALIDATION? validationInfomation = null;
                byte? authoritative = null;

                try
                {
                    result = (HRESULT)this.nrpcClient.NetrLogonSamLogon(
                        adapter.PDCNetbiosName,
                        adapter.ENDPOINTNetbiosName,
                        clientAuthenticator,
                        ref serverAuthenticator,
                        logonLevelInfoClass,
                        encryptedLogonLevel,
                        _NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo2,
                        out validationInfomation,
                        out authoritative);
                }
                catch (InvalidOperationException e)
                {
                    result = (HRESULT)int.Parse(e.Message, CultureInfo.InvariantCulture);
                }

                // If the sequence number in security context is out of sequence, the server will return an error message.
                Site.CaptureRequirementIfAreNotEqual<HRESULT>(
                    HRESULT.ERROR_SUCCESS,
                    result,
                    853,
                    @"[In Receiving an Initial Netlogon Signature Token,  The following steps are 
                performed to verify the data  and to decrypt with AES if negotiated, otherwise 
                RC4 if required: in step 7: ] If these two [SequenceNumber  and CopySeqNumber ] 
                do not match, an error is returned indicating that out-of-sequence data was received.");
            }
        }

        #endregion

        #region Protected Override Methods

        /// <summary>
        /// Test initialize
        /// </summary>
        protected override void TestInitialize()
        {
            if (testSite == null)
            {
                testSite = this.Site;
            }

            nrpcServerAdapter = testSite.GetAdapter<INrpcServerAdapter>();
            nrpcServerSutControlAdapter = testSite.GetAdapter<INrpcServerSutControlAdapter>();
            Site.DefaultProtocolDocShortName = "MS-NRPC";
        }

        /// <summary>
        /// Cleans up the test suite.
        /// </summary>
        protected override void TestCleanup()
        {
            base.TestCleanup();
            if (null != this.nrpcClient)
            {
                this.nrpcClient.Dispose();
            }

            NrpcClient.CleanAll();
        }

        #endregion
    }
}