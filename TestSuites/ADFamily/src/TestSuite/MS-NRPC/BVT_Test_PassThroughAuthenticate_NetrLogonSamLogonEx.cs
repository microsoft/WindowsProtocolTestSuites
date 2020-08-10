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
    public partial class BVT_Test_PassThroughAuthenticate_NetrLogonSamLogonEx : PtfTestClassBase
    {

        public BVT_Test_PassThroughAuthenticate_NetrLogonSamLogonEx()
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
        public void NRPC_BVT_Test_PassThroughAuthenticate_NetrLogonSamLogonExS0()
        {
            this.Manager.BeginTest("BVT_Test_PassThroughAuthenticate_NetrLogonSamLogonExS0");
            this.Manager.Comment("reaching state \'S0\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType temp0;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp0);
            this.Manager.Comment("reaching state \'S1\'");
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType.WindowsServer2008R2, temp0, "sutPlatform of GetPlatform, state S1");
            this.Manager.Comment("reaching state \'S10\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp1;
            this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
            temp1 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.Client);
            this.Manager.Checkpoint("MS-NRPC_R103340");
            this.Manager.Comment("reaching state \'S15\'");
            this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp1, "return of NetrServerReqChallenge, state S15");
            this.Manager.Comment("reaching state \'S20\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp2;
            this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                    "t,WorkstationSecureChannel,Client,True,16644)\'");
            temp2 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.Client, true, 16644u);
            this.Manager.Checkpoint("MS-NRPC_R103455");
            this.Manager.Comment("reaching state \'S25\'");
            this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp2, "return of NetrServerAuthenticate3, state S25");
            this.Manager.Comment("reaching state \'S30\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp3;
            this.Manager.Comment("executing step \'call NetrLogonSamLogonEx(True,PrimaryDc,Client,NetlogonNetworkTra" +
                    "nsitiveInformation,Valid,NetlogonValidationSamInfo2,2147483648)\'");
            temp3 = this.INrpcServerAdapterInstance.NetrLogonSamLogonEx(true, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.Client, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonNetworkTransitiveInformation, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo2, 2147483648u);
            this.Manager.Checkpoint("MS-NRPC_R1160");
            this.Manager.Comment("reaching state \'S35\'");
            this.Manager.Comment("checking step \'return NetrLogonSamLogonEx/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp3, "return of NetrLogonSamLogonEx, state S35");
            this.Manager.Comment("reaching state \'S40\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp4;
            this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
            temp4 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.Client);
            this.Manager.Checkpoint("MS-NRPC_R103340");
            this.Manager.Comment("reaching state \'S45\'");
            this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp4, "return of NetrServerReqChallenge, state S45");
            this.Manager.Comment("reaching state \'S46\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp5;
            this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                    "t,WorkstationSecureChannel,Client,True,16644)\'");
            temp5 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.Client, true, 16644u);
            this.Manager.Checkpoint("MS-NRPC_R103455");
            this.Manager.Comment("reaching state \'S47\'");
            this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp5, "return of NetrServerAuthenticate3, state S47");
            this.Manager.Comment("reaching state \'S48\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp6;
            this.Manager.Comment("executing step \'call NetrLogonSamLogonEx(True,PrimaryDc,Client,NetlogonNetworkTra" +
                    "nsitiveInformation,Valid,NetlogonValidationSamInfo2,0)\'");
            temp6 = this.INrpcServerAdapterInstance.NetrLogonSamLogonEx(true, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.Client, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonNetworkTransitiveInformation, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo2, 0u);
            this.Manager.Checkpoint("MS-NRPC_R1154");
            this.Manager.Checkpoint("MS-NRPC_R1160");
            this.Manager.Comment("reaching state \'S49\'");
            this.Manager.Comment("checking step \'return NetrLogonSamLogonEx/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp6, "return of NetrLogonSamLogonEx, state S49");
            this.Manager.Comment("reaching state \'S50\'");
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S2
        [TestMethod]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("BVT")]
        [TestCategory("PDC")]
        [TestCategory("MS-NRPC")]
        public void NRPC_BVT_Test_PassThroughAuthenticate_NetrLogonSamLogonExS2()
        {
            this.Manager.BeginTest("BVT_Test_PassThroughAuthenticate_NetrLogonSamLogonExS2");
            this.Manager.Comment("reaching state \'S2\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType temp7;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp7);
            this.Manager.Comment("reaching state \'S3\'");
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType.WindowsServer2008R2, temp7, "sutPlatform of GetPlatform, state S3");
            this.Manager.Comment("reaching state \'S11\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp8;
            this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
            temp8 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.Client);
            this.Manager.Checkpoint("MS-NRPC_R103340");
            this.Manager.Comment("reaching state \'S16\'");
            this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp8, "return of NetrServerReqChallenge, state S16");
            this.Manager.Comment("reaching state \'S21\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp9;
            this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                    "t,WorkstationSecureChannel,Client,True,16644)\'");
            temp9 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.Client, true, 16644u);
            this.Manager.Checkpoint("MS-NRPC_R103455");
            this.Manager.Comment("reaching state \'S26\'");
            this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp9, "return of NetrServerAuthenticate3, state S26");
            this.Manager.Comment("reaching state \'S31\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp10;
            this.Manager.Comment("executing step \'call NetrLogonSamLogonEx(True,PrimaryDc,Client,NetlogonInteractiv" +
                    "eTransitiveInformation,Valid,NetlogonValidationSamInfo2,0)\'");
            temp10 = this.INrpcServerAdapterInstance.NetrLogonSamLogonEx(true, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.Client, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonInteractiveTransitiveInformation, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo2, 0u);
            this.Manager.Checkpoint("MS-NRPC_R1175");
            this.Manager.Checkpoint("MS-NRPC_R1160");
            this.Manager.Comment("reaching state \'S36\'");
            this.Manager.Comment("checking step \'return NetrLogonSamLogonEx/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp10, "return of NetrLogonSamLogonEx, state S36");
            this.Manager.Comment("reaching state \'S41\'");
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S4
        [TestMethod]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("BVT")]
        [TestCategory("PDC")]
        [TestCategory("MS-NRPC")]
        public void NRPC_BVT_Test_PassThroughAuthenticate_NetrLogonSamLogonExS4()
        {
            this.Manager.BeginTest("BVT_Test_PassThroughAuthenticate_NetrLogonSamLogonExS4");
            this.Manager.Comment("reaching state \'S4\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType temp11;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp11);
            this.Manager.Comment("reaching state \'S5\'");
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType.WindowsServer2008R2, temp11, "sutPlatform of GetPlatform, state S5");
            this.Manager.Comment("reaching state \'S12\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp12;
            this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
            temp12 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.Client);
            this.Manager.Checkpoint("MS-NRPC_R103340");
            this.Manager.Comment("reaching state \'S17\'");
            this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp12, "return of NetrServerReqChallenge, state S17");
            this.Manager.Comment("reaching state \'S22\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp13;
            this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                    "t,WorkstationSecureChannel,Client,True,16644)\'");
            temp13 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.Client, true, 16644u);
            this.Manager.Checkpoint("MS-NRPC_R103455");
            this.Manager.Comment("reaching state \'S27\'");
            this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp13, "return of NetrServerAuthenticate3, state S27");
            this.Manager.Comment("reaching state \'S32\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp14;
            this.Manager.Comment("executing step \'call NetrLogonSamLogonEx(True,PrimaryDc,Client,NetlogonInteractiv" +
                    "eTransitiveInformation,Valid,NetlogonValidationSamInfo,0)\'");
            temp14 = this.INrpcServerAdapterInstance.NetrLogonSamLogonEx(true, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.Client, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonInteractiveTransitiveInformation, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo, 0u);
            this.Manager.Checkpoint("MS-NRPC_R1175");
            this.Manager.Checkpoint("MS-NRPC_R1160");
            this.Manager.Comment("reaching state \'S37\'");
            this.Manager.Comment("checking step \'return NetrLogonSamLogonEx/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp14, "return of NetrLogonSamLogonEx, state S37");
            this.Manager.Comment("reaching state \'S42\'");
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S6
        [TestMethod]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("BVT")]
        [TestCategory("PDC")]
        [TestCategory("MS-NRPC")]
        public void NRPC_BVT_Test_PassThroughAuthenticate_NetrLogonSamLogonExS6()
        {
            this.Manager.BeginTest("BVT_Test_PassThroughAuthenticate_NetrLogonSamLogonExS6");
            this.Manager.Comment("reaching state \'S6\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType temp15;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp15);
            this.Manager.Comment("reaching state \'S7\'");
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType.WindowsServer2008R2, temp15, "sutPlatform of GetPlatform, state S7");
            this.Manager.Comment("reaching state \'S13\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp16;
            this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
            temp16 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.Client);
            this.Manager.Checkpoint("MS-NRPC_R103340");
            this.Manager.Comment("reaching state \'S18\'");
            this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp16, "return of NetrServerReqChallenge, state S18");
            this.Manager.Comment("reaching state \'S23\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp17;
            this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                    "t,WorkstationSecureChannel,Client,True,16644)\'");
            temp17 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.Client, true, 16644u);
            this.Manager.Checkpoint("MS-NRPC_R103455");
            this.Manager.Comment("reaching state \'S28\'");
            this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp17, "return of NetrServerAuthenticate3, state S28");
            this.Manager.Comment("reaching state \'S33\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp18;
            this.Manager.Comment("executing step \'call NetrLogonSamLogonEx(True,PrimaryDc,Client,NetlogonInteractiv" +
                    "eTransitiveInformation,Valid,NetlogonValidationGenericInfo2,0)\'");
            temp18 = this.INrpcServerAdapterInstance.NetrLogonSamLogonEx(true, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.Client, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonInteractiveTransitiveInformation, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationGenericInfo2, 0u);
            this.Manager.Checkpoint("MS-NRPC_R1161");
            this.Manager.Comment("reaching state \'S38\'");
            this.Manager.Comment("checking step \'return NetrLogonSamLogonEx/STATUS_INVALID_INFO_CLASS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT.STATUS_INVALID_INFO_CLASS, temp18, "return of NetrLogonSamLogonEx, state S38");
            this.Manager.Comment("reaching state \'S43\'");
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S8
        [TestMethod]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("BVT")]
        [TestCategory("PDC")]
        [TestCategory("MS-NRPC")]
        public void NRPC_BVT_Test_PassThroughAuthenticate_NetrLogonSamLogonExS8()
        {
            this.Manager.BeginTest("BVT_Test_PassThroughAuthenticate_NetrLogonSamLogonExS8");
            this.Manager.Comment("reaching state \'S8\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType temp19;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp19);
            this.Manager.Comment("reaching state \'S9\'");
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType.WindowsServer2008R2, temp19, "sutPlatform of GetPlatform, state S9");
            this.Manager.Comment("reaching state \'S14\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp20;
            this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
            temp20 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.Client);
            this.Manager.Checkpoint("MS-NRPC_R103340");
            this.Manager.Comment("reaching state \'S19\'");
            this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp20, "return of NetrServerReqChallenge, state S19");
            this.Manager.Comment("reaching state \'S24\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp21;
            this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                    "t,WorkstationSecureChannel,Client,True,16644)\'");
            temp21 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.Client, true, 16644u);
            this.Manager.Checkpoint("MS-NRPC_R103455");
            this.Manager.Comment("reaching state \'S29\'");
            this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp21, "return of NetrServerAuthenticate3, state S29");
            this.Manager.Comment("reaching state \'S34\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp22;
            this.Manager.Comment("executing step \'call NetrLogonSamLogonEx(True,PrimaryDc,Client,NetlogonInteractiv" +
                    "eTransitiveInformation,Valid,NetlogonValidationSamInfo4,0)\'");
            temp22 = this.INrpcServerAdapterInstance.NetrLogonSamLogonEx(true, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.Client, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonInteractiveTransitiveInformation, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo4, 0u);
            this.Manager.Checkpoint("MS-NRPC_R1175");
            this.Manager.Checkpoint("MS-NRPC_R1160");
            this.Manager.Comment("reaching state \'S39\'");
            this.Manager.Comment("checking step \'return NetrLogonSamLogonEx/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp22, "return of NetrLogonSamLogonEx, state S39");
            this.Manager.Comment("reaching state \'S44\'");
            this.Manager.EndTest();
        }
        #endregion
    }
}
