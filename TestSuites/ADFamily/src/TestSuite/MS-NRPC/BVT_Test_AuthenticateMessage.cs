// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Reflection;
    using Microsoft.SpecExplorer.Runtime.Testing;
    using Microsoft.Protocols.TestTools;
    using Microsoft.VisualStudio.TestTools.UnitTesting;


    [System.CodeDom.Compiler.GeneratedCodeAttribute("Spec Explorer", "3.5.3146.0")]
    [TestClassAttribute()]
    public partial class BVT_Test_AuthenticateMessage : PtfTestClassBase
    {

        public BVT_Test_AuthenticateMessage()
        {
            this.SetSwitch("ProceedControlTimeout", "100");
            this.SetSwitch("QuiescenceTimeout", "30000");
        }

        #region Adapter Instances
        private Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.INrpcServerAdapter INrpcServerAdapterInstance;

        private Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.INrpcServerSutControlAdapter INrpcServerSutControlAdapterInstance;
        #endregion

        #region Class Initialization and Cleanup
        [ClassInitializeAttribute()]
        public static void ClassInitialize(TestContext context)
        {
            PtfTestClassBase.Initialize(context);
        }

        [ClassCleanupAttribute()]
        public static void ClassCleanup()
        {
            PtfTestClassBase.Cleanup();
        }
        #endregion

        #region Test Initialization and Cleanup
        protected override void TestInitialize()
        {
            this.InitializeTestManager();
            this.INrpcServerAdapterInstance = ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.INrpcServerAdapter)(this.Manager.GetAdapter(typeof(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.INrpcServerAdapter))));
            this.INrpcServerSutControlAdapterInstance = ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.INrpcServerSutControlAdapter)(this.Manager.GetAdapter(typeof(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.INrpcServerSutControlAdapter))));
        }

        protected override void TestCleanup()
        {
            base.TestCleanup();
            this.CleanupTestManager();
        }
        #endregion

        #region Test Starting in S0
        [TestMethod]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("BVT")]
        [TestCategory("PDC")]
        [TestCategory("MS-NRPC")]
        public void NRPC_BVT_Test_AuthenticateMessageS0()
        {
            this.Manager.BeginTest("BVT_Test_AuthenticateMessageS0");
            this.Manager.Comment("reaching state \'S0\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType temp0;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp0);
            this.Manager.Comment("reaching state \'S1\'");
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType.WindowsServer2008R2, temp0, "sutPlatform of GetPlatform, state S1");
            this.Manager.Comment("reaching state \'S6\'");
            bool temp1;
            this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
            this.INrpcServerAdapterInstance.GetClientAccountType(out temp1);
            this.Manager.Comment("reaching state \'S9\'");
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, temp1, "isAdministrator of GetClientAccountType, state S9");
            this.Manager.Comment("reaching state \'S12\'");
            this.Manager.Comment("executing step \'call ChangeNonDCMachineAccountStatus(False)\'");
            this.INrpcServerSutControlAdapterInstance.ChangeNonDCMachineAccountStatus(false);
            this.Manager.Comment("reaching state \'S15\'");
            this.Manager.Comment("checking step \'return ChangeNonDCMachineAccountStatus\'");
            this.Manager.Comment("reaching state \'S18\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp2;
            this.Manager.Comment("executing step \'call NetrLogonComputeServerDigest(PrimaryDc,RidOfNonDcMachineAcco" +
                    "unt,MessageOne)\'");
            temp2 = this.INrpcServerAdapterInstance.NetrLogonComputeServerDigest(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.RidType.RidOfNonDcMachineAccount, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.MessageType)(0)));
            this.Manager.Checkpoint("MS-NRPC_R103757");
            this.Manager.Checkpoint("MS-NRPC_R103743");
            this.Manager.Comment("reaching state \'S20\'");
            this.Manager.Comment("checking step \'return NetrLogonComputeServerDigest/ERROR_NO_SUCH_USER\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT.ERROR_NO_SUCH_USER, temp2, "return of NetrLogonComputeServerDigest, state S20");
            this.Manager.Comment("reaching state \'S21\'");
            this.Manager.Comment("executing step \'call ChangeNonDCMachineAccountStatus(True)\'");
            this.INrpcServerSutControlAdapterInstance.ChangeNonDCMachineAccountStatus(true);
            this.Manager.Comment("reaching state \'S22\'");
            this.Manager.Comment("checking step \'return ChangeNonDCMachineAccountStatus\'");
            BVT_Test_AuthenticateMessageS19();
            this.Manager.EndTest();
        }

        private void BVT_Test_AuthenticateMessageS19()
        {
            this.Manager.Comment("reaching state \'S19\'");
        }
        #endregion

        #region Test Starting in S2
        [TestMethod]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("BVT")]
        [TestCategory("PDC")]
        [TestCategory("MS-NRPC")]
        public void NRPC_BVT_Test_AuthenticateMessageS2()
        {
            this.Manager.BeginTest("BVT_Test_AuthenticateMessageS2");
            this.Manager.Comment("reaching state \'S2\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType temp3;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp3);
            this.Manager.Comment("reaching state \'S3\'");
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType.WindowsServer2008R2, temp3, "sutPlatform of GetPlatform, state S3");
            this.Manager.Comment("reaching state \'S7\'");
            bool temp4;
            this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
            this.INrpcServerAdapterInstance.GetClientAccountType(out temp4);
            this.Manager.Comment("reaching state \'S10\'");
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, temp4, "isAdministrator of GetClientAccountType, state S10");
            this.Manager.Comment("reaching state \'S13\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp5;
            this.Manager.Comment("executing step \'call NetrLogonComputeServerDigest(PrimaryDc,RidOfNonDcMachineAcco" +
                    "unt,MessageOne)\'");
            temp5 = this.INrpcServerAdapterInstance.NetrLogonComputeServerDigest(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.RidType.RidOfNonDcMachineAccount, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.MessageType)(0)));
            this.Manager.Checkpoint("MS-NRPC_R103742");
            this.Manager.Comment("reaching state \'S16\'");
            this.Manager.Comment("checking step \'return NetrLogonComputeServerDigest/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp5, "return of NetrLogonComputeServerDigest, state S16");
            BVT_Test_AuthenticateMessageS19();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S4
        [TestMethod]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("BVT")]
        [TestCategory("PDC")]
        [TestCategory("MS-NRPC")]
        public void NRPC_BVT_Test_AuthenticateMessageS4()
        {
            this.Manager.BeginTest("BVT_Test_AuthenticateMessageS4");
            this.Manager.Comment("reaching state \'S4\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType temp6;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp6);
            this.Manager.Comment("reaching state \'S5\'");
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType.WindowsServer2008R2, temp6, "sutPlatform of GetPlatform, state S5");
            this.Manager.Comment("reaching state \'S8\'");
            bool temp7;
            this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
            this.INrpcServerAdapterInstance.GetClientAccountType(out temp7);
            this.Manager.Comment("reaching state \'S11\'");
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, temp7, "isAdministrator of GetClientAccountType, state S11");
            this.Manager.Comment("reaching state \'S14\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp8;
            this.Manager.Comment("executing step \'call NetrLogonComputeClientDigest(PrimaryDc,FqdnFormatDomainName," +
                    "MessageOne)\'");
            temp8 = this.INrpcServerAdapterInstance.NetrLogonComputeClientDigest(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.DomainNameType.FqdnFormatDomainName, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.MessageType)(0)));
            this.Manager.Checkpoint("MS-NRPC_R103789");
            this.Manager.Comment("reaching state \'S17\'");
            this.Manager.Comment("checking step \'return NetrLogonComputeClientDigest/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp8, "return of NetrLogonComputeClientDigest, state S17");
            BVT_Test_AuthenticateMessageS19();
            this.Manager.EndTest();
        }
        #endregion
    }
}
