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
    public partial class BVT_Test_EstablishSecureChannel_NetrServerAuthenticate3 : PtfTestClassBase
    {

        public BVT_Test_EstablishSecureChannel_NetrServerAuthenticate3()
        {
            this.SetSwitch("ProceedControlTimeout", "100");
            this.SetSwitch("QuiescenceTimeout", "30000");
        }

        #region Expect Delegates
        public delegate void GetPlatformDelegate1(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType sutPlatform);
        #endregion

        #region Event Metadata
        static System.Reflection.MethodBase GetPlatformInfo = TestManagerHelpers.GetMethodInfo(typeof(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.INrpcServerAdapter), "GetPlatform", typeof(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType).MakeByRefType());
        #endregion

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
        public void NRPC_BVT_Test_EstablishSecureChannel_NetrServerAuthenticate3S0()
        {
            this.Manager.BeginTest("BVT_Test_EstablishSecureChannel_NetrServerAuthenticate3S0");
            this.Manager.Comment("reaching state \'S0\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType temp0;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp0);
            this.Manager.AddReturn(GetPlatformInfo, null, temp0);
            this.Manager.Comment("reaching state \'S1\'");
            int temp7 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(BVT_Test_EstablishSecureChannel_NetrServerAuthenticate3.GetPlatformInfo, null, new GetPlatformDelegate1(this.BVT_Test_EstablishSecureChannel_NetrServerAuthenticate3S0GetPlatformChecker)), new ExpectedReturn(BVT_Test_EstablishSecureChannel_NetrServerAuthenticate3.GetPlatformInfo, null, new GetPlatformDelegate1(this.BVT_Test_EstablishSecureChannel_NetrServerAuthenticate3S0GetPlatformChecker1)));
            if ((temp7 == 0))
            {
                this.Manager.Comment("reaching state \'S6\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S12\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S18\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp1;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp1 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S24\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp1, "return of NetrServerReqChallenge, state S24");
                this.Manager.Comment("reaching state \'S30\'");
                this.Manager.Comment("executing step \'call ConfigServerRejectMD5Client(True)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServerRejectMD5Client(true);
                this.Manager.Comment("reaching state \'S36\'");
                this.Manager.Comment("checking step \'return ConfigServerRejectMD5Client\'");
                this.Manager.Comment("reaching state \'S42\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp2;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16388)\'");
                temp2 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.Client, true, 16388u);
                this.Manager.Checkpoint("MS-NRPC_R203371");
                this.Manager.Checkpoint("MS-NRPC_R103456");
                this.Manager.Comment("reaching state \'S48\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/STATUS_DOWNGRADE_DETECTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT.STATUS_DOWNGRADE_DETECTED, temp2, "return of NetrServerAuthenticate3, state S48");
                this.Manager.Comment("reaching state \'S54\'");
                this.Manager.Comment("executing step \'call ConfigServerRejectMD5Client(False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServerRejectMD5Client(false);
                this.Manager.Comment("reaching state \'S60\'");
                this.Manager.Comment("checking step \'return ConfigServerRejectMD5Client\'");
                this.Manager.Comment("reaching state \'S66\'");
                goto label0;
            }
            if ((temp7 == 1))
            {
                this.Manager.Comment("reaching state \'S7\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S13\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S19\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp3;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp3 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S25\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp3, "return of NetrServerReqChallenge, state S25");
                this.Manager.Comment("reaching state \'S31\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp4;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16777220)\'");
                temp4 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.Client, true, 16777220u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S37\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp4, "return of NetrServerAuthenticate3, state S37");
                this.Manager.Comment("reaching state \'S43\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp5;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp5 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S49\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp5, "return of NetrServerReqChallenge, state S49");
                this.Manager.Comment("reaching state \'S55\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp6;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16777220)\'");
                temp6 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.Client, true, 16777220u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S61\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp6, "return of NetrServerAuthenticate3, state S61");
                this.Manager.Comment("reaching state \'S67\'");
                goto label0;
            }
            throw new InvalidOperationException("never reached");
        label0:
            ;
            this.Manager.EndTest();
        }

        private void BVT_Test_EstablishSecureChannel_NetrServerAuthenticate3S0GetPlatformChecker(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType sutPlatform)
        {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S1");
        }

        private void BVT_Test_EstablishSecureChannel_NetrServerAuthenticate3S0GetPlatformChecker1(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType sutPlatform)
        {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S1");
        }
        #endregion

        #region Test Starting in S2
        [TestMethod]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("BVT")]
        [TestCategory("PDC")]
        [TestCategory("MS-NRPC")]
        public void NRPC_BVT_Test_EstablishSecureChannel_NetrServerAuthenticate3S2()
        {
            this.Manager.BeginTest("BVT_Test_EstablishSecureChannel_NetrServerAuthenticate3S2");
            this.Manager.Comment("reaching state \'S2\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType temp8;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp8);
            this.Manager.AddReturn(GetPlatformInfo, null, temp8);
            this.Manager.Comment("reaching state \'S3\'");
            int temp17 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(BVT_Test_EstablishSecureChannel_NetrServerAuthenticate3.GetPlatformInfo, null, new GetPlatformDelegate1(this.BVT_Test_EstablishSecureChannel_NetrServerAuthenticate3S2GetPlatformChecker)), new ExpectedReturn(BVT_Test_EstablishSecureChannel_NetrServerAuthenticate3.GetPlatformInfo, null, new GetPlatformDelegate1(this.BVT_Test_EstablishSecureChannel_NetrServerAuthenticate3S2GetPlatformChecker1)));
            if ((temp17 == 0))
            {
                this.Manager.Comment("reaching state \'S8\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S14\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S20\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp9;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp9 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S26\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp9, "return of NetrServerReqChallenge, state S26");
                this.Manager.Comment("reaching state \'S32\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp10;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16777220)\'");
                temp10 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.Client, true, 16777220u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S38\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp10, "return of NetrServerAuthenticate3, state S38");
                this.Manager.Comment("reaching state \'S44\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp11;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp11 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S50\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp11, "return of NetrServerReqChallenge, state S50");
                this.Manager.Comment("reaching state \'S56\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp12;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16777220)\'");
                temp12 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.Client, true, 16777220u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S62\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp12, "return of NetrServerAuthenticate3, state S62");
                this.Manager.Comment("reaching state \'S68\'");
                goto label1;
            }
            if ((temp17 == 1))
            {
                this.Manager.Comment("reaching state \'S9\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S15\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S21\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp13;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp13 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S27\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp13, "return of NetrServerReqChallenge, state S27");
                this.Manager.Comment("reaching state \'S33\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp14;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16388)\'");
                temp14 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.Client, true, 16388u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S39\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp14, "return of NetrServerAuthenticate3, state S39");
                this.Manager.Comment("reaching state \'S45\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp15;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp15 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S51\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp15, "return of NetrServerReqChallenge, state S51");
                this.Manager.Comment("reaching state \'S57\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp16;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16388)\'");
                temp16 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.Client, true, 16388u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S63\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp16, "return of NetrServerAuthenticate3, state S63");
                this.Manager.Comment("reaching state \'S69\'");
                goto label1;
            }
            throw new InvalidOperationException("never reached");
        label1:
            ;
            this.Manager.EndTest();
        }

        private void BVT_Test_EstablishSecureChannel_NetrServerAuthenticate3S2GetPlatformChecker(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType sutPlatform)
        {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S3");
        }

        private void BVT_Test_EstablishSecureChannel_NetrServerAuthenticate3S2GetPlatformChecker1(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType sutPlatform)
        {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S3");
        }
        #endregion

        #region Test Starting in S4
        [TestMethod]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("BVT")]
        [TestCategory("PDC")]
        [TestCategory("MS-NRPC")]
        public void NRPC_BVT_Test_EstablishSecureChannel_NetrServerAuthenticate3S4()
        {
            this.Manager.BeginTest("BVT_Test_EstablishSecureChannel_NetrServerAuthenticate3S4");
            this.Manager.Comment("reaching state \'S4\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType temp18;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp18);
            this.Manager.AddReturn(GetPlatformInfo, null, temp18);
            this.Manager.Comment("reaching state \'S5\'");
            int temp25 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(BVT_Test_EstablishSecureChannel_NetrServerAuthenticate3.GetPlatformInfo, null, new GetPlatformDelegate1(this.BVT_Test_EstablishSecureChannel_NetrServerAuthenticate3S4GetPlatformChecker)), new ExpectedReturn(BVT_Test_EstablishSecureChannel_NetrServerAuthenticate3.GetPlatformInfo, null, new GetPlatformDelegate1(this.BVT_Test_EstablishSecureChannel_NetrServerAuthenticate3S4GetPlatformChecker1)));
            if ((temp25 == 0))
            {
                this.Manager.Comment("reaching state \'S10\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S16\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S22\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp19;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp19 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S28\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp19, "return of NetrServerReqChallenge, state S28");
                this.Manager.Comment("reaching state \'S34\'");
                this.Manager.Comment("executing step \'call ConfigServerRejectMD5Client(True)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServerRejectMD5Client(true);
                this.Manager.Comment("reaching state \'S40\'");
                this.Manager.Comment("checking step \'return ConfigServerRejectMD5Client\'");
                this.Manager.Comment("reaching state \'S46\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp20;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16388)\'");
                temp20 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.Client, true, 16388u);
                this.Manager.Checkpoint("MS-NRPC_R203371");
                this.Manager.Checkpoint("MS-NRPC_R103456");
                this.Manager.Comment("reaching state \'S52\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/STATUS_DOWNGRADE_DETECTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT.STATUS_DOWNGRADE_DETECTED, temp20, "return of NetrServerAuthenticate3, state S52");
                this.Manager.Comment("reaching state \'S58\'");
                this.Manager.Comment("executing step \'call ConfigServerRejectMD5Client(False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServerRejectMD5Client(false);
                this.Manager.Comment("reaching state \'S64\'");
                this.Manager.Comment("checking step \'return ConfigServerRejectMD5Client\'");
                this.Manager.Comment("reaching state \'S70\'");
                goto label2;
            }
            if ((temp25 == 1))
            {
                this.Manager.Comment("reaching state \'S11\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S17\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S23\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp21;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp21 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S29\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp21, "return of NetrServerReqChallenge, state S29");
                this.Manager.Comment("reaching state \'S35\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp22;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16388)\'");
                temp22 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.Client, true, 16388u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S41\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp22, "return of NetrServerAuthenticate3, state S41");
                this.Manager.Comment("reaching state \'S47\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp23;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp23 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S53\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp23, "return of NetrServerReqChallenge, state S53");
                this.Manager.Comment("reaching state \'S59\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp24;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16388)\'");
                temp24 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.Client, true, 16388u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S65\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp24, "return of NetrServerAuthenticate3, state S65");
                this.Manager.Comment("reaching state \'S71\'");
                goto label2;
            }
            throw new InvalidOperationException("never reached");
        label2:
            ;
            this.Manager.EndTest();
        }

        private void BVT_Test_EstablishSecureChannel_NetrServerAuthenticate3S4GetPlatformChecker(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType sutPlatform)
        {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S5");
        }

        private void BVT_Test_EstablishSecureChannel_NetrServerAuthenticate3S4GetPlatformChecker1(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType sutPlatform)
        {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S5");
        }
        #endregion
    }
}
