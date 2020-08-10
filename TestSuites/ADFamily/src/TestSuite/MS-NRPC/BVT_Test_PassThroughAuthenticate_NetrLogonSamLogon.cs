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
    public partial class BVT_Test_PassThroughAuthenticate_NetrLogonSamLogon : PtfTestClassBase
    {

        public BVT_Test_PassThroughAuthenticate_NetrLogonSamLogon()
        {
            this.SetSwitch("ProceedControlTimeout", "100");
            this.SetSwitch("QuiescenceTimeout", "30000");
        }

        #region Adapter Instances
        private Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.INrpcServerAdapter INrpcServerAdapterInstance;
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
        public void NRPC_BVT_Test_PassThroughAuthenticate_NetrLogonSamLogonS0()
        {
            this.Manager.BeginTest("BVT_Test_PassThroughAuthenticate_NetrLogonSamLogonS0");
            this.Manager.Comment("reaching state \'S0\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType temp0;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp0);
            this.Manager.Comment("reaching state \'S1\'");
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType.WindowsServer2008R2, temp0, "sutPlatform of GetPlatform, state S1");
            this.Manager.Comment("reaching state \'S12\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp1;
            this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
            temp1 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.Client);
            this.Manager.Checkpoint("MS-NRPC_R103340");
            this.Manager.Comment("reaching state \'S18\'");
            this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp1, "return of NetrServerReqChallenge, state S18");
            this.Manager.Comment("reaching state \'S24\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp2;
            this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                    "t,WorkstationSecureChannel,Client,True,16644)\'");
            temp2 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.Client, true, 16644u);
            this.Manager.Checkpoint("MS-NRPC_R103455");
            this.Manager.Comment("reaching state \'S30\'");
            this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp2, "return of NetrServerAuthenticate3, state S30");
            this.Manager.Comment("reaching state \'S36\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp3;
            this.Manager.Comment("executing step \'call NetrLogonSamLogon(PrimaryDc,Client,True,NetlogonNetworkInfor" +
                    "mation,Valid,NetlogonValidationSamInfo)\'");
            temp3 = this.INrpcServerAdapterInstance.NetrLogonSamLogon(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.Client, true, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonNetworkInformation, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo);
            this.Manager.Comment("reaching state \'S42\'");
            this.Manager.Comment("checking step \'return NetrLogonSamLogon/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp3, "return of NetrLogonSamLogon, state S42");
            BVT_Test_PassThroughAuthenticate_NetrLogonSamLogonS48();
            this.Manager.EndTest();
        }

        private void BVT_Test_PassThroughAuthenticate_NetrLogonSamLogonS48()
        {
            this.Manager.Comment("reaching state \'S48\'");
        }
        #endregion

        #region Test Starting in S10
        [TestMethod]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("BVT")]
        [TestCategory("PDC")]
        [TestCategory("MS-NRPC")]
        public void NRPC_BVT_Test_PassThroughAuthenticate_NetrLogonSamLogonS10()
        {
            this.Manager.BeginTest("BVT_Test_PassThroughAuthenticate_NetrLogonSamLogonS10");
            this.Manager.Comment("reaching state \'S10\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType temp4;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp4);
            this.Manager.Comment("reaching state \'S11\'");
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType.WindowsServer2008R2, temp4, "sutPlatform of GetPlatform, state S11");
            this.Manager.Comment("reaching state \'S17\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp5;
            this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
            temp5 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.Client);
            this.Manager.Checkpoint("MS-NRPC_R103340");
            this.Manager.Comment("reaching state \'S23\'");
            this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp5, "return of NetrServerReqChallenge, state S23");
            this.Manager.Comment("reaching state \'S29\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp6;
            this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                    "t,WorkstationSecureChannel,Client,True,16644)\'");
            temp6 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.Client, true, 16644u);
            this.Manager.Checkpoint("MS-NRPC_R103455");
            this.Manager.Comment("reaching state \'S35\'");
            this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp6, "return of NetrServerAuthenticate3, state S35");
            this.Manager.Comment("reaching state \'S41\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp7;
            this.Manager.Comment("executing step \'call NetrLogonSamLogon(PrimaryDc,Client,True,NetlogonInteractiveT" +
                    "ransitiveInformation,Valid,NetlogonValidationSamInfo)\'");
            temp7 = this.INrpcServerAdapterInstance.NetrLogonSamLogon(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.Client, true, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonInteractiveTransitiveInformation, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo);
            this.Manager.Comment("reaching state \'S47\'");
            this.Manager.Comment("checking step \'return NetrLogonSamLogon/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp7, "return of NetrLogonSamLogon, state S47");
            BVT_Test_PassThroughAuthenticate_NetrLogonSamLogonS48();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S2
        [TestMethod]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("BVT")]
        [TestCategory("PDC")]
        [TestCategory("MS-NRPC")]
        public void NRPC_BVT_Test_PassThroughAuthenticate_NetrLogonSamLogonS2()
        {
            this.Manager.BeginTest("BVT_Test_PassThroughAuthenticate_NetrLogonSamLogonS2");
            this.Manager.Comment("reaching state \'S2\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType temp8;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp8);
            this.Manager.Comment("reaching state \'S3\'");
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType.WindowsServer2008R2, temp8, "sutPlatform of GetPlatform, state S3");
            this.Manager.Comment("reaching state \'S13\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp9;
            this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
            temp9 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.Client);
            this.Manager.Checkpoint("MS-NRPC_R103340");
            this.Manager.Comment("reaching state \'S19\'");
            this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp9, "return of NetrServerReqChallenge, state S19");
            this.Manager.Comment("reaching state \'S25\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp10;
            this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                    "t,WorkstationSecureChannel,Client,True,16644)\'");
            temp10 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.Client, true, 16644u);
            this.Manager.Checkpoint("MS-NRPC_R103455");
            this.Manager.Comment("reaching state \'S31\'");
            this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp10, "return of NetrServerAuthenticate3, state S31");
            this.Manager.Comment("reaching state \'S37\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp11;
            this.Manager.Comment("executing step \'call NetrLogonSamLogon(PrimaryDc,Client,True,NetlogonNetworkTrans" +
                    "itiveInformation,Valid,NetlogonValidationSamInfo)\'");
            temp11 = this.INrpcServerAdapterInstance.NetrLogonSamLogon(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.Client, true, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonNetworkTransitiveInformation, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo);
            this.Manager.Comment("reaching state \'S43\'");
            this.Manager.Comment("checking step \'return NetrLogonSamLogon/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp11, "return of NetrLogonSamLogon, state S43");
            BVT_Test_PassThroughAuthenticate_NetrLogonSamLogonS48();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S4
        [TestMethod]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("BVT")]
        [TestCategory("PDC")]
        [TestCategory("MS-NRPC")]
        public void NRPC_BVT_Test_PassThroughAuthenticate_NetrLogonSamLogonS4()
        {
            this.Manager.BeginTest("BVT_Test_PassThroughAuthenticate_NetrLogonSamLogonS4");
            this.Manager.Comment("reaching state \'S4\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType temp12;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp12);
            this.Manager.Comment("reaching state \'S5\'");
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType.WindowsServer2008R2, temp12, "sutPlatform of GetPlatform, state S5");
            this.Manager.Comment("reaching state \'S14\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp13;
            this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
            temp13 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.Client);
            this.Manager.Checkpoint("MS-NRPC_R103340");
            this.Manager.Comment("reaching state \'S20\'");
            this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp13, "return of NetrServerReqChallenge, state S20");
            this.Manager.Comment("reaching state \'S26\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp14;
            this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                    "t,WorkstationSecureChannel,Client,True,16644)\'");
            temp14 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.Client, true, 16644u);
            this.Manager.Checkpoint("MS-NRPC_R103455");
            this.Manager.Comment("reaching state \'S32\'");
            this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp14, "return of NetrServerAuthenticate3, state S32");
            this.Manager.Comment("reaching state \'S38\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp15;
            this.Manager.Comment("executing step \'call NetrLogonSamLogon(PrimaryDc,Client,True,NetlogonInteractiveI" +
                    "nformation,Valid,NetlogonValidationSamInfo)\'");
            temp15 = this.INrpcServerAdapterInstance.NetrLogonSamLogon(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.Client, true, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo);
            this.Manager.Comment("reaching state \'S44\'");
            this.Manager.Comment("checking step \'return NetrLogonSamLogon/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp15, "return of NetrLogonSamLogon, state S44");
            BVT_Test_PassThroughAuthenticate_NetrLogonSamLogonS48();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S6
        [TestMethod]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("BVT")]
        [TestCategory("PDC")]
        [TestCategory("MS-NRPC")]
        public void NRPC_BVT_Test_PassThroughAuthenticate_NetrLogonSamLogonS6()
        {
            this.Manager.BeginTest("BVT_Test_PassThroughAuthenticate_NetrLogonSamLogonS6");
            this.Manager.Comment("reaching state \'S6\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType temp16;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp16);
            this.Manager.Comment("reaching state \'S7\'");
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType.WindowsServer2008R2, temp16, "sutPlatform of GetPlatform, state S7");
            this.Manager.Comment("reaching state \'S15\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp17;
            this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
            temp17 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.Client);
            this.Manager.Checkpoint("MS-NRPC_R103340");
            this.Manager.Comment("reaching state \'S21\'");
            this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp17, "return of NetrServerReqChallenge, state S21");
            this.Manager.Comment("reaching state \'S27\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp18;
            this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                    "t,WorkstationSecureChannel,Client,True,16644)\'");
            temp18 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.Client, true, 16644u);
            this.Manager.Checkpoint("MS-NRPC_R103455");
            this.Manager.Comment("reaching state \'S33\'");
            this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp18, "return of NetrServerAuthenticate3, state S33");
            this.Manager.Comment("reaching state \'S39\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp19;
            this.Manager.Comment("executing step \'call NetrLogonSamLogon(PrimaryDc,Client,True,NetlogonServiceInfor" +
                    "mation,Valid,NetlogonValidationSamInfo)\'");
            temp19 = this.INrpcServerAdapterInstance.NetrLogonSamLogon(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.Client, true, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonServiceInformation, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo);
            this.Manager.Comment("reaching state \'S45\'");
            this.Manager.Comment("checking step \'return NetrLogonSamLogon/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp19, "return of NetrLogonSamLogon, state S45");
            BVT_Test_PassThroughAuthenticate_NetrLogonSamLogonS48();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S8
        [TestMethod]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("BVT")]
        [TestCategory("PDC")]
        [TestCategory("MS-NRPC")]
        public void NRPC_BVT_Test_PassThroughAuthenticate_NetrLogonSamLogonS8()
        {
            this.Manager.BeginTest("BVT_Test_PassThroughAuthenticate_NetrLogonSamLogonS8");
            this.Manager.Comment("reaching state \'S8\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType temp20;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp20);
            this.Manager.Comment("reaching state \'S9\'");
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType.WindowsServer2008R2, temp20, "sutPlatform of GetPlatform, state S9");
            this.Manager.Comment("reaching state \'S16\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp21;
            this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
            temp21 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.Client);
            this.Manager.Checkpoint("MS-NRPC_R103340");
            this.Manager.Comment("reaching state \'S22\'");
            this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp21, "return of NetrServerReqChallenge, state S22");
            this.Manager.Comment("reaching state \'S28\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp22;
            this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                    "t,WorkstationSecureChannel,Client,True,16644)\'");
            temp22 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.Client, true, 16644u);
            this.Manager.Checkpoint("MS-NRPC_R103455");
            this.Manager.Comment("reaching state \'S34\'");
            this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp22, "return of NetrServerAuthenticate3, state S34");
            this.Manager.Comment("reaching state \'S40\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp23;
            this.Manager.Comment("executing step \'call NetrLogonSamLogon(PrimaryDc,Client,True,NetlogonServiceTrans" +
                    "itiveInformation,Valid,NetlogonValidationSamInfo)\'");
            temp23 = this.INrpcServerAdapterInstance.NetrLogonSamLogon(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.Client, true, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonServiceTransitiveInformation, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo);
            this.Manager.Comment("reaching state \'S46\'");
            this.Manager.Comment("checking step \'return NetrLogonSamLogon/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp23, "return of NetrLogonSamLogon, state S46");
            BVT_Test_PassThroughAuthenticate_NetrLogonSamLogonS48();
            this.Manager.EndTest();
        }
        #endregion
    }
}
