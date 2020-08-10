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
    public partial class BVT_Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlags : PtfTestClassBase
    {

        public BVT_Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlags()
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
        public void NRPC_BVT_Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS0()
        {
            this.Manager.BeginTest("BVT_Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS0");
            this.Manager.Comment("reaching state \'S0\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType temp0;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp0);
            this.Manager.Comment("reaching state \'S1\'");
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType.WindowsServer2008R2, temp0, "sutPlatform of GetPlatform, state S1");
            this.Manager.Comment("reaching state \'S48\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp1;
            this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
            temp1 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.Client);
            this.Manager.Checkpoint("MS-NRPC_R103340");
            this.Manager.Comment("reaching state \'S72\'");
            this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp1, "return of NetrServerReqChallenge, state S72");
            this.Manager.Comment("reaching state \'S96\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp2;
            this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                    "t,WorkstationSecureChannel,Client,True,16644)\'");
            temp2 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.Client, true, 16644u);
            this.Manager.Checkpoint("MS-NRPC_R103455");
            this.Manager.Comment("reaching state \'S120\'");
            this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp2, "return of NetrServerAuthenticate3, state S120");
            this.Manager.Comment("reaching state \'S144\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp3;
            this.Manager.Comment("executing step \'call NetrLogonSamLogonWithFlags(PrimaryDc,Client,True,NetlogonNet" +
                    "workTransitiveInformation,Valid,NetlogonValidationSamInfo2,2147483648)\'");
            temp3 = this.INrpcServerAdapterInstance.NetrLogonSamLogonWithFlags(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.Client, true, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonNetworkTransitiveInformation, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo2, 2147483648u);
            this.Manager.Checkpoint("MS-NRPC_R1240");
            this.Manager.Comment("reaching state \'S168\'");
            this.Manager.Comment("checking step \'return NetrLogonSamLogonWithFlags/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp3, "return of NetrLogonSamLogonWithFlags, state S168");
            this.Manager.Comment("reaching state \'S192\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp4;
            this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
            temp4 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.Client);
            this.Manager.Checkpoint("MS-NRPC_R103340");
            this.Manager.Comment("reaching state \'S216\'");
            this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp4, "return of NetrServerReqChallenge, state S216");
            this.Manager.Comment("reaching state \'S217\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp5;
            this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                    "t,WorkstationSecureChannel,Client,True,16644)\'");
            temp5 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.Client, true, 16644u);
            this.Manager.Checkpoint("MS-NRPC_R103455");
            this.Manager.Comment("reaching state \'S218\'");
            this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp5, "return of NetrServerAuthenticate3, state S218");
            this.Manager.Comment("reaching state \'S219\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp6;
            this.Manager.Comment("executing step \'call NetrLogonSamLogonWithFlags(PrimaryDc,Client,True,NetlogonNet" +
                    "workTransitiveInformation,Valid,NetlogonValidationSamInfo2,0)\'");
            temp6 = this.INrpcServerAdapterInstance.NetrLogonSamLogonWithFlags(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.Client, true, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonNetworkTransitiveInformation, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo2, 0u);
            this.Manager.Checkpoint("MS-NRPC_R1239");
            this.Manager.Checkpoint("MS-NRPC_R1240");
            this.Manager.Comment("reaching state \'S220\'");
            this.Manager.Comment("checking step \'return NetrLogonSamLogonWithFlags/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp6, "return of NetrLogonSamLogonWithFlags, state S220");
            this.Manager.Comment("reaching state \'S221\'");
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S10
        [TestMethod]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("BVT")]
        [TestCategory("PDC")]
        [TestCategory("MS-NRPC")]
        public void NRPC_BVT_Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS10()
        {
            this.Manager.BeginTest("BVT_Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS10");
            this.Manager.Comment("reaching state \'S10\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType temp7;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp7);
            this.Manager.Comment("reaching state \'S11\'");
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType.WindowsServer2008R2, temp7, "sutPlatform of GetPlatform, state S11");
            this.Manager.Comment("reaching state \'S53\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp8;
            this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
            temp8 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.Client);
            this.Manager.Checkpoint("MS-NRPC_R103340");
            this.Manager.Comment("reaching state \'S77\'");
            this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp8, "return of NetrServerReqChallenge, state S77");
            this.Manager.Comment("reaching state \'S101\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp9;
            this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                    "t,WorkstationSecureChannel,Client,True,16644)\'");
            temp9 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.Client, true, 16644u);
            this.Manager.Checkpoint("MS-NRPC_R103455");
            this.Manager.Comment("reaching state \'S125\'");
            this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp9, "return of NetrServerAuthenticate3, state S125");
            this.Manager.Comment("reaching state \'S149\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp10;
            this.Manager.Comment("executing step \'call NetrLogonSamLogonWithFlags(PrimaryDc,Client,True,NetlogonSer" +
                    "viceInformation,Valid,NetlogonValidationSamInfo,0)\'");
            temp10 = this.INrpcServerAdapterInstance.NetrLogonSamLogonWithFlags(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.Client, true, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonServiceInformation, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo, 0u);
            this.Manager.Checkpoint("MS-NRPC_R10193");
            this.Manager.Checkpoint("MS-NRPC_R1240");
            this.Manager.Comment("reaching state \'S173\'");
            this.Manager.Comment("checking step \'return NetrLogonSamLogonWithFlags/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp10, "return of NetrLogonSamLogonWithFlags, state S173");
            this.Manager.Comment("reaching state \'S197\'");
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S12
        [TestMethod]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("BVT")]
        [TestCategory("PDC")]
        [TestCategory("MS-NRPC")]
        public void NRPC_BVT_Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS12()
        {
            this.Manager.BeginTest("BVT_Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS12");
            this.Manager.Comment("reaching state \'S12\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType temp11;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp11);
            this.Manager.Comment("reaching state \'S13\'");
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType.WindowsServer2008R2, temp11, "sutPlatform of GetPlatform, state S13");
            this.Manager.Comment("reaching state \'S54\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp12;
            this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
            temp12 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.Client);
            this.Manager.Checkpoint("MS-NRPC_R103340");
            this.Manager.Comment("reaching state \'S78\'");
            this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp12, "return of NetrServerReqChallenge, state S78");
            this.Manager.Comment("reaching state \'S102\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp13;
            this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                    "t,WorkstationSecureChannel,Client,True,16644)\'");
            temp13 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.Client, true, 16644u);
            this.Manager.Checkpoint("MS-NRPC_R103455");
            this.Manager.Comment("reaching state \'S126\'");
            this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp13, "return of NetrServerAuthenticate3, state S126");
            this.Manager.Comment("reaching state \'S150\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp14;
            this.Manager.Comment("executing step \'call NetrLogonSamLogonWithFlags(PrimaryDc,Client,True,NetlogonNet" +
                    "workInformation,Valid,NetlogonValidationSamInfo4,0)\'");
            temp14 = this.INrpcServerAdapterInstance.NetrLogonSamLogonWithFlags(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.Client, true, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonNetworkInformation, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo4, 0u);
            this.Manager.Checkpoint("MS-NRPC_R1240");
            this.Manager.Comment("reaching state \'S174\'");
            this.Manager.Comment("checking step \'return NetrLogonSamLogonWithFlags/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp14, "return of NetrLogonSamLogonWithFlags, state S174");
            this.Manager.Comment("reaching state \'S198\'");
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S14
        [TestMethod]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("BVT")]
        [TestCategory("PDC")]
        [TestCategory("MS-NRPC")]
        public void NRPC_BVT_Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS14()
        {
            this.Manager.BeginTest("BVT_Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS14");
            this.Manager.Comment("reaching state \'S14\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType temp15;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp15);
            this.Manager.Comment("reaching state \'S15\'");
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType.WindowsServer2008R2, temp15, "sutPlatform of GetPlatform, state S15");
            this.Manager.Comment("reaching state \'S55\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp16;
            this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
            temp16 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.Client);
            this.Manager.Checkpoint("MS-NRPC_R103340");
            this.Manager.Comment("reaching state \'S79\'");
            this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp16, "return of NetrServerReqChallenge, state S79");
            this.Manager.Comment("reaching state \'S103\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp17;
            this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                    "t,WorkstationSecureChannel,Client,True,16644)\'");
            temp17 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.Client, true, 16644u);
            this.Manager.Checkpoint("MS-NRPC_R103455");
            this.Manager.Comment("reaching state \'S127\'");
            this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp17, "return of NetrServerAuthenticate3, state S127");
            this.Manager.Comment("reaching state \'S151\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp18;
            this.Manager.Comment("executing step \'call NetrLogonSamLogonWithFlags(PrimaryDc,Client,True,NetlogonNet" +
                    "workInformation,Valid,NetlogonValidationSamInfo2,0)\'");
            temp18 = this.INrpcServerAdapterInstance.NetrLogonSamLogonWithFlags(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.Client, true, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonNetworkInformation, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo2, 0u);
            this.Manager.Checkpoint("MS-NRPC_R1240");
            this.Manager.Comment("reaching state \'S175\'");
            this.Manager.Comment("checking step \'return NetrLogonSamLogonWithFlags/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp18, "return of NetrLogonSamLogonWithFlags, state S175");
            this.Manager.Comment("reaching state \'S199\'");
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S16
        [TestMethod]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("BVT")]
        [TestCategory("PDC")]
        [TestCategory("MS-NRPC")]
        public void NRPC_BVT_Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS16()
        {
            this.Manager.BeginTest("BVT_Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS16");
            this.Manager.Comment("reaching state \'S16\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType temp19;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp19);
            this.Manager.Comment("reaching state \'S17\'");
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType.WindowsServer2008R2, temp19, "sutPlatform of GetPlatform, state S17");
            this.Manager.Comment("reaching state \'S56\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp20;
            this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
            temp20 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.Client);
            this.Manager.Checkpoint("MS-NRPC_R103340");
            this.Manager.Comment("reaching state \'S80\'");
            this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp20, "return of NetrServerReqChallenge, state S80");
            this.Manager.Comment("reaching state \'S104\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp21;
            this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                    "t,WorkstationSecureChannel,Client,True,16644)\'");
            temp21 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.Client, true, 16644u);
            this.Manager.Checkpoint("MS-NRPC_R103455");
            this.Manager.Comment("reaching state \'S128\'");
            this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp21, "return of NetrServerAuthenticate3, state S128");
            this.Manager.Comment("reaching state \'S152\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp22;
            this.Manager.Comment("executing step \'call NetrLogonSamLogonWithFlags(PrimaryDc,Client,True,NetlogonInt" +
                    "eractiveTransitiveInformation,Valid,NetlogonValidationSamInfo,0)\'");
            temp22 = this.INrpcServerAdapterInstance.NetrLogonSamLogonWithFlags(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.Client, true, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonInteractiveTransitiveInformation, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo, 0u);
            this.Manager.Checkpoint("MS-NRPC_R10190");
            this.Manager.Checkpoint("MS-NRPC_R1240");
            this.Manager.Comment("reaching state \'S176\'");
            this.Manager.Comment("checking step \'return NetrLogonSamLogonWithFlags/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp22, "return of NetrLogonSamLogonWithFlags, state S176");
            this.Manager.Comment("reaching state \'S200\'");
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S18
        [TestMethod]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("BVT")]
        [TestCategory("PDC")]
        [TestCategory("MS-NRPC")]
        public void NRPC_BVT_Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS18()
        {
            this.Manager.BeginTest("BVT_Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS18");
            this.Manager.Comment("reaching state \'S18\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType temp23;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp23);
            this.Manager.Comment("reaching state \'S19\'");
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType.WindowsServer2008R2, temp23, "sutPlatform of GetPlatform, state S19");
            this.Manager.Comment("reaching state \'S57\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp24;
            this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
            temp24 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.Client);
            this.Manager.Checkpoint("MS-NRPC_R103340");
            this.Manager.Comment("reaching state \'S81\'");
            this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp24, "return of NetrServerReqChallenge, state S81");
            this.Manager.Comment("reaching state \'S105\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp25;
            this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                    "t,WorkstationSecureChannel,Client,True,16644)\'");
            temp25 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.Client, true, 16644u);
            this.Manager.Checkpoint("MS-NRPC_R103455");
            this.Manager.Comment("reaching state \'S129\'");
            this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp25, "return of NetrServerAuthenticate3, state S129");
            this.Manager.Comment("reaching state \'S153\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp26;
            this.Manager.Comment("executing step \'call NetrLogonSamLogonWithFlags(PrimaryDc,Client,True,NetlogonSer" +
                    "viceInformation,Valid,NetlogonValidationSamInfo4,0)\'");
            temp26 = this.INrpcServerAdapterInstance.NetrLogonSamLogonWithFlags(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.Client, true, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonServiceInformation, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo4, 0u);
            this.Manager.Checkpoint("MS-NRPC_R10193");
            this.Manager.Checkpoint("MS-NRPC_R1240");
            this.Manager.Comment("reaching state \'S177\'");
            this.Manager.Comment("checking step \'return NetrLogonSamLogonWithFlags/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp26, "return of NetrLogonSamLogonWithFlags, state S177");
            this.Manager.Comment("reaching state \'S201\'");
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S2
        [TestMethod]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("BVT")]
        [TestCategory("PDC")]
        [TestCategory("MS-NRPC")]
        public void NRPC_BVT_Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS2()
        {
            this.Manager.BeginTest("BVT_Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS2");
            this.Manager.Comment("reaching state \'S2\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType temp27;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp27);
            this.Manager.Comment("reaching state \'S3\'");
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType.WindowsServer2008R2, temp27, "sutPlatform of GetPlatform, state S3");
            this.Manager.Comment("reaching state \'S49\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp28;
            this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
            temp28 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.Client);
            this.Manager.Checkpoint("MS-NRPC_R103340");
            this.Manager.Comment("reaching state \'S73\'");
            this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp28, "return of NetrServerReqChallenge, state S73");
            this.Manager.Comment("reaching state \'S97\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp29;
            this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                    "t,WorkstationSecureChannel,Client,True,16644)\'");
            temp29 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.Client, true, 16644u);
            this.Manager.Checkpoint("MS-NRPC_R103455");
            this.Manager.Comment("reaching state \'S121\'");
            this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp29, "return of NetrServerAuthenticate3, state S121");
            this.Manager.Comment("reaching state \'S145\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp30;
            this.Manager.Comment("executing step \'call NetrLogonSamLogonWithFlags(PrimaryDc,Client,True,NetlogonNet" +
                    "workInformation,Valid,NetlogonValidationSamInfo,0)\'");
            temp30 = this.INrpcServerAdapterInstance.NetrLogonSamLogonWithFlags(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.Client, true, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonNetworkInformation, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo, 0u);
            this.Manager.Checkpoint("MS-NRPC_R1240");
            this.Manager.Comment("reaching state \'S169\'");
            this.Manager.Comment("checking step \'return NetrLogonSamLogonWithFlags/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp30, "return of NetrLogonSamLogonWithFlags, state S169");
            this.Manager.Comment("reaching state \'S193\'");
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S20
        [TestMethod]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("BVT")]
        [TestCategory("PDC")]
        [TestCategory("MS-NRPC")]
        public void NRPC_BVT_Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS20()
        {
            this.Manager.BeginTest("BVT_Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS20");
            this.Manager.Comment("reaching state \'S20\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType temp31;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp31);
            this.Manager.Comment("reaching state \'S21\'");
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType.WindowsServer2008R2, temp31, "sutPlatform of GetPlatform, state S21");
            this.Manager.Comment("reaching state \'S58\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp32;
            this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
            temp32 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.Client);
            this.Manager.Checkpoint("MS-NRPC_R103340");
            this.Manager.Comment("reaching state \'S82\'");
            this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp32, "return of NetrServerReqChallenge, state S82");
            this.Manager.Comment("reaching state \'S106\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp33;
            this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                    "t,WorkstationSecureChannel,Client,True,16644)\'");
            temp33 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.Client, true, 16644u);
            this.Manager.Checkpoint("MS-NRPC_R103455");
            this.Manager.Comment("reaching state \'S130\'");
            this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp33, "return of NetrServerAuthenticate3, state S130");
            this.Manager.Comment("reaching state \'S154\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp34;
            this.Manager.Comment("executing step \'call NetrLogonSamLogonWithFlags(PrimaryDc,Client,True,NetlogonSer" +
                    "viceInformation,Valid,NetlogonValidationSamInfo2,0)\'");
            temp34 = this.INrpcServerAdapterInstance.NetrLogonSamLogonWithFlags(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.Client, true, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonServiceInformation, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo2, 0u);
            this.Manager.Checkpoint("MS-NRPC_R10193");
            this.Manager.Checkpoint("MS-NRPC_R1240");
            this.Manager.Comment("reaching state \'S178\'");
            this.Manager.Comment("checking step \'return NetrLogonSamLogonWithFlags/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp34, "return of NetrLogonSamLogonWithFlags, state S178");
            this.Manager.Comment("reaching state \'S202\'");
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S22
        [TestMethod]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("BVT")]
        [TestCategory("PDC")]
        [TestCategory("MS-NRPC")]
        public void NRPC_BVT_Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS22()
        {
            this.Manager.BeginTest("BVT_Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS22");
            this.Manager.Comment("reaching state \'S22\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType temp35;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp35);
            this.Manager.Comment("reaching state \'S23\'");
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType.WindowsServer2008R2, temp35, "sutPlatform of GetPlatform, state S23");
            this.Manager.Comment("reaching state \'S59\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp36;
            this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
            temp36 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.Client);
            this.Manager.Checkpoint("MS-NRPC_R103340");
            this.Manager.Comment("reaching state \'S83\'");
            this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp36, "return of NetrServerReqChallenge, state S83");
            this.Manager.Comment("reaching state \'S107\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp37;
            this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                    "t,WorkstationSecureChannel,Client,True,16644)\'");
            temp37 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.Client, true, 16644u);
            this.Manager.Checkpoint("MS-NRPC_R103455");
            this.Manager.Comment("reaching state \'S131\'");
            this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp37, "return of NetrServerAuthenticate3, state S131");
            this.Manager.Comment("reaching state \'S155\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp38;
            this.Manager.Comment("executing step \'call NetrLogonSamLogonWithFlags(PrimaryDc,Client,True,NetlogonSer" +
                    "viceTransitiveInformation,Valid,NetlogonValidationSamInfo,0)\'");
            temp38 = this.INrpcServerAdapterInstance.NetrLogonSamLogonWithFlags(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.Client, true, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonServiceTransitiveInformation, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo, 0u);
            this.Manager.Checkpoint("MS-NRPC_R10194");
            this.Manager.Checkpoint("MS-NRPC_R1240");
            this.Manager.Comment("reaching state \'S179\'");
            this.Manager.Comment("checking step \'return NetrLogonSamLogonWithFlags/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp38, "return of NetrLogonSamLogonWithFlags, state S179");
            this.Manager.Comment("reaching state \'S203\'");
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S24
        [TestMethod]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("BVT")]
        [TestCategory("PDC")]
        [TestCategory("MS-NRPC")]
        public void NRPC_BVT_Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS24()
        {
            this.Manager.BeginTest("BVT_Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS24");
            this.Manager.Comment("reaching state \'S24\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType temp39;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp39);
            this.Manager.Comment("reaching state \'S25\'");
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType.WindowsServer2008R2, temp39, "sutPlatform of GetPlatform, state S25");
            this.Manager.Comment("reaching state \'S60\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp40;
            this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
            temp40 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.Client);
            this.Manager.Checkpoint("MS-NRPC_R103340");
            this.Manager.Comment("reaching state \'S84\'");
            this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp40, "return of NetrServerReqChallenge, state S84");
            this.Manager.Comment("reaching state \'S108\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp41;
            this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                    "t,WorkstationSecureChannel,Client,True,16644)\'");
            temp41 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.Client, true, 16644u);
            this.Manager.Checkpoint("MS-NRPC_R103455");
            this.Manager.Comment("reaching state \'S132\'");
            this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp41, "return of NetrServerAuthenticate3, state S132");
            this.Manager.Comment("reaching state \'S156\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp42;
            this.Manager.Comment("executing step \'call NetrLogonSamLogonWithFlags(PrimaryDc,Client,True,NetlogonNet" +
                    "workTransitiveInformation,Valid,NetlogonValidationSamInfo,0)\'");
            temp42 = this.INrpcServerAdapterInstance.NetrLogonSamLogonWithFlags(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.Client, true, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonNetworkTransitiveInformation, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo, 0u);
            this.Manager.Checkpoint("MS-NRPC_R1240");
            this.Manager.Comment("reaching state \'S180\'");
            this.Manager.Comment("checking step \'return NetrLogonSamLogonWithFlags/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp42, "return of NetrLogonSamLogonWithFlags, state S180");
            this.Manager.Comment("reaching state \'S204\'");
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S26
        [TestMethod]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("BVT")]
        [TestCategory("PDC")]
        [TestCategory("MS-NRPC")]
        public void NRPC_BVT_Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS26()
        {
            this.Manager.BeginTest("BVT_Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS26");
            this.Manager.Comment("reaching state \'S26\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType temp43;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp43);
            this.Manager.Comment("reaching state \'S27\'");
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType.WindowsServer2008R2, temp43, "sutPlatform of GetPlatform, state S27");
            this.Manager.Comment("reaching state \'S61\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp44;
            this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
            temp44 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.Client);
            this.Manager.Checkpoint("MS-NRPC_R103340");
            this.Manager.Comment("reaching state \'S85\'");
            this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp44, "return of NetrServerReqChallenge, state S85");
            this.Manager.Comment("reaching state \'S109\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp45;
            this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                    "t,WorkstationSecureChannel,Client,True,16644)\'");
            temp45 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.Client, true, 16644u);
            this.Manager.Checkpoint("MS-NRPC_R103455");
            this.Manager.Comment("reaching state \'S133\'");
            this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp45, "return of NetrServerAuthenticate3, state S133");
            this.Manager.Comment("reaching state \'S157\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp46;
            this.Manager.Comment("executing step \'call NetrLogonSamLogonWithFlags(PrimaryDc,Client,True,NetlogonInt" +
                    "eractiveTransitiveInformation,Valid,NetlogonValidationSamInfo4,0)\'");
            temp46 = this.INrpcServerAdapterInstance.NetrLogonSamLogonWithFlags(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.Client, true, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonInteractiveTransitiveInformation, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo4, 0u);
            this.Manager.Checkpoint("MS-NRPC_R10190");
            this.Manager.Checkpoint("MS-NRPC_R1240");
            this.Manager.Comment("reaching state \'S181\'");
            this.Manager.Comment("checking step \'return NetrLogonSamLogonWithFlags/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp46, "return of NetrLogonSamLogonWithFlags, state S181");
            this.Manager.Comment("reaching state \'S205\'");
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S28
        [TestMethod]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("BVT")]
        [TestCategory("PDC")]
        [TestCategory("MS-NRPC")]
        public void NRPC_BVT_Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS28()
        {
            this.Manager.BeginTest("BVT_Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS28");
            this.Manager.Comment("reaching state \'S28\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType temp47;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp47);
            this.Manager.Comment("reaching state \'S29\'");
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType.WindowsServer2008R2, temp47, "sutPlatform of GetPlatform, state S29");
            this.Manager.Comment("reaching state \'S62\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp48;
            this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
            temp48 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.Client);
            this.Manager.Checkpoint("MS-NRPC_R103340");
            this.Manager.Comment("reaching state \'S86\'");
            this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp48, "return of NetrServerReqChallenge, state S86");
            this.Manager.Comment("reaching state \'S110\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp49;
            this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                    "t,WorkstationSecureChannel,Client,True,16644)\'");
            temp49 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.Client, true, 16644u);
            this.Manager.Checkpoint("MS-NRPC_R103455");
            this.Manager.Comment("reaching state \'S134\'");
            this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp49, "return of NetrServerAuthenticate3, state S134");
            this.Manager.Comment("reaching state \'S158\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp50;
            this.Manager.Comment("executing step \'call NetrLogonSamLogonWithFlags(PrimaryDc,Client,True,NetlogonInt" +
                    "eractiveTransitiveInformation,Valid,NetlogonValidationSamInfo2,0)\'");
            temp50 = this.INrpcServerAdapterInstance.NetrLogonSamLogonWithFlags(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.Client, true, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonInteractiveTransitiveInformation, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo2, 0u);
            this.Manager.Checkpoint("MS-NRPC_R10190");
            this.Manager.Checkpoint("MS-NRPC_R1240");
            this.Manager.Comment("reaching state \'S182\'");
            this.Manager.Comment("checking step \'return NetrLogonSamLogonWithFlags/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp50, "return of NetrLogonSamLogonWithFlags, state S182");
            this.Manager.Comment("reaching state \'S206\'");
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S30
        [TestMethod]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("BVT")]
        [TestCategory("PDC")]
        [TestCategory("DM")]
        [TestCategory("MS-NRPC")]
        public void NRPC_BVT_Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS30()
        {
            this.Manager.BeginTest("BVT_Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS30");
            this.Manager.Comment("reaching state \'S30\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType temp51;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp51);
            this.Manager.Comment("reaching state \'S31\'");
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType.WindowsServer2008R2, temp51, "sutPlatform of GetPlatform, state S31");
            this.Manager.Comment("reaching state \'S63\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp52;
            this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
            temp52 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.Client);
            this.Manager.Checkpoint("MS-NRPC_R103340");
            this.Manager.Comment("reaching state \'S87\'");
            this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp52, "return of NetrServerReqChallenge, state S87");
            this.Manager.Comment("reaching state \'S111\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp53;
            this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                    "t,WorkstationSecureChannel,Client,True,16644)\'");
            temp53 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.Client, true, 16644u);
            this.Manager.Checkpoint("MS-NRPC_R103455");
            this.Manager.Comment("reaching state \'S135\'");
            this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp53, "return of NetrServerAuthenticate3, state S135");
            this.Manager.Comment("reaching state \'S159\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp54;
            this.Manager.Comment("executing step \'call NetrLogonSamLogonWithFlags(NonDcServer,Client,True,NetlogonN" +
                    "etworkTransitiveInformation,Valid,NetlogonValidationSamInfo2,0)\'");
            temp54 = this.INrpcServerAdapterInstance.NetrLogonSamLogonWithFlags(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.Client, true, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonNetworkTransitiveInformation, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo2, 0u);
            this.Manager.Checkpoint("MS-NRPC_R425");
            this.Manager.Checkpoint("MS-NRPC_R1241");
            this.Manager.Comment("reaching state \'S183\'");
            this.Manager.Comment("checking step \'return NetrLogonSamLogonWithFlags/STATUS_NOT_SUPPORTED\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp54, "return of NetrLogonSamLogonWithFlags, state S183");
            this.Manager.Comment("reaching state \'S207\'");
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S32
        [TestMethod]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("BVT")]
        [TestCategory("PDC")]
        [TestCategory("MS-NRPC")]
        public void NRPC_BVT_Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS32()
        {
            this.Manager.BeginTest("BVT_Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS32");
            this.Manager.Comment("reaching state \'S32\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType temp55;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp55);
            this.Manager.Comment("reaching state \'S33\'");
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType.WindowsServer2008R2, temp55, "sutPlatform of GetPlatform, state S33");
            this.Manager.Comment("reaching state \'S64\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp56;
            this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
            temp56 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.Client);
            this.Manager.Checkpoint("MS-NRPC_R103340");
            this.Manager.Comment("reaching state \'S88\'");
            this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp56, "return of NetrServerReqChallenge, state S88");
            this.Manager.Comment("reaching state \'S112\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp57;
            this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                    "t,WorkstationSecureChannel,Client,True,16644)\'");
            temp57 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.Client, true, 16644u);
            this.Manager.Checkpoint("MS-NRPC_R103455");
            this.Manager.Comment("reaching state \'S136\'");
            this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp57, "return of NetrServerAuthenticate3, state S136");
            this.Manager.Comment("reaching state \'S160\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp58;
            this.Manager.Comment("executing step \'call NetrLogonSamLogonWithFlags(PrimaryDc,Client,True,NetlogonSer" +
                    "viceTransitiveInformation,Valid,NetlogonValidationSamInfo4,0)\'");
            temp58 = this.INrpcServerAdapterInstance.NetrLogonSamLogonWithFlags(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.Client, true, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonServiceTransitiveInformation, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo4, 0u);
            this.Manager.Checkpoint("MS-NRPC_R10194");
            this.Manager.Checkpoint("MS-NRPC_R1240");
            this.Manager.Comment("reaching state \'S184\'");
            this.Manager.Comment("checking step \'return NetrLogonSamLogonWithFlags/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp58, "return of NetrLogonSamLogonWithFlags, state S184");
            this.Manager.Comment("reaching state \'S208\'");
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S34
        [TestMethod]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("BVT")]
        [TestCategory("PDC")]
        [TestCategory("MS-NRPC")]
        public void NRPC_BVT_Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS34()
        {
            this.Manager.BeginTest("BVT_Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS34");
            this.Manager.Comment("reaching state \'S34\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType temp59;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp59);
            this.Manager.Comment("reaching state \'S35\'");
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType.WindowsServer2008R2, temp59, "sutPlatform of GetPlatform, state S35");
            this.Manager.Comment("reaching state \'S65\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp60;
            this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
            temp60 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.Client);
            this.Manager.Checkpoint("MS-NRPC_R103340");
            this.Manager.Comment("reaching state \'S89\'");
            this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp60, "return of NetrServerReqChallenge, state S89");
            this.Manager.Comment("reaching state \'S113\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp61;
            this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                    "t,WorkstationSecureChannel,Client,True,16644)\'");
            temp61 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.Client, true, 16644u);
            this.Manager.Checkpoint("MS-NRPC_R103455");
            this.Manager.Comment("reaching state \'S137\'");
            this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp61, "return of NetrServerAuthenticate3, state S137");
            this.Manager.Comment("reaching state \'S161\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp62;
            this.Manager.Comment("executing step \'call NetrLogonSamLogonWithFlags(PrimaryDc,Client,True,NetlogonSer" +
                    "viceTransitiveInformation,Valid,NetlogonValidationSamInfo2,0)\'");
            temp62 = this.INrpcServerAdapterInstance.NetrLogonSamLogonWithFlags(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.Client, true, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonServiceTransitiveInformation, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo2, 0u);
            this.Manager.Checkpoint("MS-NRPC_R10194");
            this.Manager.Checkpoint("MS-NRPC_R1240");
            this.Manager.Comment("reaching state \'S185\'");
            this.Manager.Comment("checking step \'return NetrLogonSamLogonWithFlags/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp62, "return of NetrLogonSamLogonWithFlags, state S185");
            this.Manager.Comment("reaching state \'S209\'");
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S36
        [TestMethod]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("BVT")]
        [TestCategory("PDC")]
        [TestCategory("MS-NRPC")]
        public void NRPC_BVT_Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS36()
        {
            this.Manager.BeginTest("BVT_Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS36");
            this.Manager.Comment("reaching state \'S36\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType temp63;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp63);
            this.Manager.Comment("reaching state \'S37\'");
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType.WindowsServer2008R2, temp63, "sutPlatform of GetPlatform, state S37");
            this.Manager.Comment("reaching state \'S66\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp64;
            this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
            temp64 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.Client);
            this.Manager.Checkpoint("MS-NRPC_R103340");
            this.Manager.Comment("reaching state \'S90\'");
            this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp64, "return of NetrServerReqChallenge, state S90");
            this.Manager.Comment("reaching state \'S114\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp65;
            this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                    "t,WorkstationSecureChannel,Client,True,16644)\'");
            temp65 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.Client, true, 16644u);
            this.Manager.Checkpoint("MS-NRPC_R103455");
            this.Manager.Comment("reaching state \'S138\'");
            this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp65, "return of NetrServerAuthenticate3, state S138");
            this.Manager.Comment("reaching state \'S162\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp66;
            this.Manager.Comment("executing step \'call NetrLogonSamLogonWithFlags(PrimaryDc,Client,True,NetlogonNet" +
                    "workTransitiveInformation,Valid,NetlogonValidationSamInfo4,0)\'");
            temp66 = this.INrpcServerAdapterInstance.NetrLogonSamLogonWithFlags(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.Client, true, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonNetworkTransitiveInformation, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo4, 0u);
            this.Manager.Checkpoint("MS-NRPC_R1240");
            this.Manager.Comment("reaching state \'S186\'");
            this.Manager.Comment("checking step \'return NetrLogonSamLogonWithFlags/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp66, "return of NetrLogonSamLogonWithFlags, state S186");
            this.Manager.Comment("reaching state \'S210\'");
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S38
        [TestMethod]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("BVT")]
        [TestCategory("PDC")]
        [TestCategory("MS-NRPC")]
        public void NRPC_BVT_Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS38()
        {
            this.Manager.BeginTest("BVT_Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS38");
            this.Manager.Comment("reaching state \'S38\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType temp67;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp67);
            this.Manager.Comment("reaching state \'S39\'");
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType.WindowsServer2008R2, temp67, "sutPlatform of GetPlatform, state S39");
            this.Manager.Comment("reaching state \'S67\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp68;
            this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
            temp68 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.Client);
            this.Manager.Checkpoint("MS-NRPC_R103340");
            this.Manager.Comment("reaching state \'S91\'");
            this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp68, "return of NetrServerReqChallenge, state S91");
            this.Manager.Comment("reaching state \'S115\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp69;
            this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                    "t,WorkstationSecureChannel,Client,True,16644)\'");
            temp69 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.Client, true, 16644u);
            this.Manager.Checkpoint("MS-NRPC_R103455");
            this.Manager.Comment("reaching state \'S139\'");
            this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp69, "return of NetrServerAuthenticate3, state S139");
            this.Manager.Comment("reaching state \'S163\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp70;
            this.Manager.Comment("executing step \'call NetrLogonSamLogonWithFlags(PrimaryDc,Client,True,NetlogonNet" +
                    "workTransitiveInformation,Valid,NetlogonValidationSamInfo2,4)\'");
            temp70 = this.INrpcServerAdapterInstance.NetrLogonSamLogonWithFlags(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.Client, true, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonNetworkTransitiveInformation, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo2, 4u);
            this.Manager.Checkpoint("MS-NRPC_R1240");
            this.Manager.Comment("reaching state \'S187\'");
            this.Manager.Comment("checking step \'return NetrLogonSamLogonWithFlags/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp70, "return of NetrLogonSamLogonWithFlags, state S187");
            this.Manager.Comment("reaching state \'S211\'");
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S4
        [TestMethod]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("BVT")]
        [TestCategory("PDC")]
        [TestCategory("MS-NRPC")]
        public void NRPC_BVT_Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS4()
        {
            this.Manager.BeginTest("BVT_Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS4");
            this.Manager.Comment("reaching state \'S4\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType temp71;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp71);
            this.Manager.Comment("reaching state \'S5\'");
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType.WindowsServer2008R2, temp71, "sutPlatform of GetPlatform, state S5");
            this.Manager.Comment("reaching state \'S50\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp72;
            this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
            temp72 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.Client);
            this.Manager.Checkpoint("MS-NRPC_R103340");
            this.Manager.Comment("reaching state \'S74\'");
            this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp72, "return of NetrServerReqChallenge, state S74");
            this.Manager.Comment("reaching state \'S98\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp73;
            this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                    "t,WorkstationSecureChannel,Client,True,16644)\'");
            temp73 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.Client, true, 16644u);
            this.Manager.Checkpoint("MS-NRPC_R103455");
            this.Manager.Comment("reaching state \'S122\'");
            this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp73, "return of NetrServerAuthenticate3, state S122");
            this.Manager.Comment("reaching state \'S146\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp74;
            this.Manager.Comment("executing step \'call NetrLogonSamLogonWithFlags(PrimaryDc,Client,True,NetlogonInt" +
                    "eractiveInformation,Valid,NetlogonValidationSamInfo,0)\'");
            temp74 = this.INrpcServerAdapterInstance.NetrLogonSamLogonWithFlags(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.Client, true, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo, 0u);
            this.Manager.Checkpoint("MS-NRPC_R10189");
            this.Manager.Checkpoint("MS-NRPC_R1240");
            this.Manager.Comment("reaching state \'S170\'");
            this.Manager.Comment("checking step \'return NetrLogonSamLogonWithFlags/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp74, "return of NetrLogonSamLogonWithFlags, state S170");
            this.Manager.Comment("reaching state \'S194\'");
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S40
        [TestMethod]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("BVT")]
        [TestCategory("PDC")]
        [TestCategory("MS-NRPC")]
        public void NRPC_BVT_Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS40()
        {
            this.Manager.BeginTest("BVT_Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS40");
            this.Manager.Comment("reaching state \'S40\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType temp75;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp75);
            this.Manager.Comment("reaching state \'S41\'");
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType.WindowsServer2008R2, temp75, "sutPlatform of GetPlatform, state S41");
            this.Manager.Comment("reaching state \'S68\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp76;
            this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
            temp76 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.Client);
            this.Manager.Checkpoint("MS-NRPC_R103340");
            this.Manager.Comment("reaching state \'S92\'");
            this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp76, "return of NetrServerReqChallenge, state S92");
            this.Manager.Comment("reaching state \'S116\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp77;
            this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                    "t,WorkstationSecureChannel,Client,True,16644)\'");
            temp77 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.Client, true, 16644u);
            this.Manager.Checkpoint("MS-NRPC_R103455");
            this.Manager.Comment("reaching state \'S140\'");
            this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp77, "return of NetrServerAuthenticate3, state S140");
            this.Manager.Comment("reaching state \'S164\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp78;
            this.Manager.Comment("executing step \'call NetrLogonSamLogonWithFlags(PrimaryDc,Client,True,NetlogonNet" +
                    "workTransitiveInformation,Valid,NetlogonValidationSamInfo2,8)\'");
            temp78 = this.INrpcServerAdapterInstance.NetrLogonSamLogonWithFlags(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.Client, true, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonNetworkTransitiveInformation, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo2, 8u);
            this.Manager.Checkpoint("MS-NRPC_R1240");
            this.Manager.Comment("reaching state \'S188\'");
            this.Manager.Comment("checking step \'return NetrLogonSamLogonWithFlags/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp78, "return of NetrLogonSamLogonWithFlags, state S188");
            this.Manager.Comment("reaching state \'S212\'");
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S42
        [TestMethod]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("BVT")]
        [TestCategory("PDC")]
        [TestCategory("MS-NRPC")]
        public void NRPC_BVT_Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS42()
        {
            this.Manager.BeginTest("BVT_Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS42");
            this.Manager.Comment("reaching state \'S42\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType temp79;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp79);
            this.Manager.Comment("reaching state \'S43\'");
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType.WindowsServer2008R2, temp79, "sutPlatform of GetPlatform, state S43");
            this.Manager.Comment("reaching state \'S69\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp80;
            this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
            temp80 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.Client);
            this.Manager.Checkpoint("MS-NRPC_R103340");
            this.Manager.Comment("reaching state \'S93\'");
            this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp80, "return of NetrServerReqChallenge, state S93");
            this.Manager.Comment("reaching state \'S117\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp81;
            this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                    "t,WorkstationSecureChannel,Client,True,16644)\'");
            temp81 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.Client, true, 16644u);
            this.Manager.Checkpoint("MS-NRPC_R103455");
            this.Manager.Comment("reaching state \'S141\'");
            this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp81, "return of NetrServerAuthenticate3, state S141");
            this.Manager.Comment("reaching state \'S165\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp82;
            this.Manager.Comment("executing step \'call NetrLogonSamLogonWithFlags(PrimaryDc,Client,True,NetlogonNet" +
                    "workTransitiveInformation,Valid,NetlogonValidationSamInfo2,2147483648)\'");
            temp82 = this.INrpcServerAdapterInstance.NetrLogonSamLogonWithFlags(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.Client, true, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonNetworkTransitiveInformation, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo2, 2147483648u);
            this.Manager.Checkpoint("MS-NRPC_R1240");
            this.Manager.Comment("reaching state \'S189\'");
            this.Manager.Comment("checking step \'return NetrLogonSamLogonWithFlags/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp82, "return of NetrLogonSamLogonWithFlags, state S189");
            this.Manager.Comment("reaching state \'S213\'");
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S44
        [TestMethod]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("BVT")]
        [TestCategory("PDC")]
        [TestCategory("MS-NRPC")]
        public void NRPC_BVT_Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS44()
        {
            this.Manager.BeginTest("BVT_Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS44");
            this.Manager.Comment("reaching state \'S44\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType temp83;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp83);
            this.Manager.Comment("reaching state \'S45\'");
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType.WindowsServer2008R2, temp83, "sutPlatform of GetPlatform, state S45");
            this.Manager.Comment("reaching state \'S70\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp84;
            this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
            temp84 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.Client);
            this.Manager.Checkpoint("MS-NRPC_R103340");
            this.Manager.Comment("reaching state \'S94\'");
            this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp84, "return of NetrServerReqChallenge, state S94");
            this.Manager.Comment("reaching state \'S118\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp85;
            this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                    "t,WorkstationSecureChannel,Client,True,16644)\'");
            temp85 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.Client, true, 16644u);
            this.Manager.Checkpoint("MS-NRPC_R103455");
            this.Manager.Comment("reaching state \'S142\'");
            this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp85, "return of NetrServerAuthenticate3, state S142");
            this.Manager.Comment("reaching state \'S166\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp86;
            this.Manager.Comment("executing step \'call NetrLogonSamLogonWithFlags(PrimaryDc,Client,True,NetlogonNet" +
                    "workTransitiveInformation,Valid,NetlogonValidationSamInfo2,2)\'");
            temp86 = this.INrpcServerAdapterInstance.NetrLogonSamLogonWithFlags(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.Client, true, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonNetworkTransitiveInformation, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo2, 2u);
            this.Manager.Checkpoint("MS-NRPC_R1447");
            this.Manager.Checkpoint("MS-NRPC_R1241");
            this.Manager.Comment("reaching state \'S190\'");
            this.Manager.Comment("checking step \'return NetrLogonSamLogonWithFlags/STATUS_NO_SUCH_USER\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT.STATUS_NO_SUCH_USER, temp86, "return of NetrLogonSamLogonWithFlags, state S190");
            this.Manager.Comment("reaching state \'S214\'");
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S46
        [TestMethod]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("BVT")]
        [TestCategory("PDC")]
        [TestCategory("MS-NRPC")]
        public void NRPC_BVT_Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS46()
        {
            this.Manager.BeginTest("BVT_Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS46");
            this.Manager.Comment("reaching state \'S46\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType temp87;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp87);
            this.Manager.Comment("reaching state \'S47\'");
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType.WindowsServer2008R2, temp87, "sutPlatform of GetPlatform, state S47");
            this.Manager.Comment("reaching state \'S71\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp88;
            this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
            temp88 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.Client);
            this.Manager.Checkpoint("MS-NRPC_R103340");
            this.Manager.Comment("reaching state \'S95\'");
            this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp88, "return of NetrServerReqChallenge, state S95");
            this.Manager.Comment("reaching state \'S119\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp89;
            this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                    "t,WorkstationSecureChannel,Client,True,16644)\'");
            temp89 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.Client, true, 16644u);
            this.Manager.Checkpoint("MS-NRPC_R103455");
            this.Manager.Comment("reaching state \'S143\'");
            this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp89, "return of NetrServerAuthenticate3, state S143");
            this.Manager.Comment("reaching state \'S167\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp90;
            this.Manager.Comment("executing step \'call NetrLogonSamLogonWithFlags(PrimaryDc,Client,True,NetlogonNet" +
                    "workTransitiveInformation,Valid,NetlogonValidationSamInfo2,0)\'");
            temp90 = this.INrpcServerAdapterInstance.NetrLogonSamLogonWithFlags(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.Client, true, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonNetworkTransitiveInformation, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo2, 0u);
            this.Manager.Checkpoint("MS-NRPC_R1240");
            this.Manager.Comment("reaching state \'S191\'");
            this.Manager.Comment("checking step \'return NetrLogonSamLogonWithFlags/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp90, "return of NetrLogonSamLogonWithFlags, state S191");
            this.Manager.Comment("reaching state \'S215\'");
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S6
        [TestMethod]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("BVT")]
        [TestCategory("PDC")]
        [TestCategory("MS-NRPC")]
        public void NRPC_BVT_Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS6()
        {
            this.Manager.BeginTest("BVT_Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS6");
            this.Manager.Comment("reaching state \'S6\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType temp91;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp91);
            this.Manager.Comment("reaching state \'S7\'");
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType.WindowsServer2008R2, temp91, "sutPlatform of GetPlatform, state S7");
            this.Manager.Comment("reaching state \'S51\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp92;
            this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
            temp92 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.Client);
            this.Manager.Checkpoint("MS-NRPC_R103340");
            this.Manager.Comment("reaching state \'S75\'");
            this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp92, "return of NetrServerReqChallenge, state S75");
            this.Manager.Comment("reaching state \'S99\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp93;
            this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                    "t,WorkstationSecureChannel,Client,True,16644)\'");
            temp93 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.Client, true, 16644u);
            this.Manager.Checkpoint("MS-NRPC_R103455");
            this.Manager.Comment("reaching state \'S123\'");
            this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp93, "return of NetrServerAuthenticate3, state S123");
            this.Manager.Comment("reaching state \'S147\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp94;
            this.Manager.Comment("executing step \'call NetrLogonSamLogonWithFlags(PrimaryDc,Client,True,NetlogonInt" +
                    "eractiveInformation,Valid,NetlogonValidationSamInfo4,0)\'");
            temp94 = this.INrpcServerAdapterInstance.NetrLogonSamLogonWithFlags(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.Client, true, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo4, 0u);
            this.Manager.Checkpoint("MS-NRPC_R10189");
            this.Manager.Checkpoint("MS-NRPC_R1240");
            this.Manager.Comment("reaching state \'S171\'");
            this.Manager.Comment("checking step \'return NetrLogonSamLogonWithFlags/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp94, "return of NetrLogonSamLogonWithFlags, state S171");
            this.Manager.Comment("reaching state \'S195\'");
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S8
        [TestMethod]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("BVT")]
        [TestCategory("PDC")]
        [TestCategory("MS-NRPC")]
        public void NRPC_BVT_Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS8()
        {
            this.Manager.BeginTest("BVT_Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS8");
            this.Manager.Comment("reaching state \'S8\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType temp95;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp95);
            this.Manager.Comment("reaching state \'S9\'");
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType.WindowsServer2008R2, temp95, "sutPlatform of GetPlatform, state S9");
            this.Manager.Comment("reaching state \'S52\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp96;
            this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
            temp96 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.Client);
            this.Manager.Checkpoint("MS-NRPC_R103340");
            this.Manager.Comment("reaching state \'S76\'");
            this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp96, "return of NetrServerReqChallenge, state S76");
            this.Manager.Comment("reaching state \'S100\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp97;
            this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                    "t,WorkstationSecureChannel,Client,True,16644)\'");
            temp97 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.Client, true, 16644u);
            this.Manager.Checkpoint("MS-NRPC_R103455");
            this.Manager.Comment("reaching state \'S124\'");
            this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp97, "return of NetrServerAuthenticate3, state S124");
            this.Manager.Comment("reaching state \'S148\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp98;
            this.Manager.Comment("executing step \'call NetrLogonSamLogonWithFlags(PrimaryDc,Client,True,NetlogonInt" +
                    "eractiveInformation,Valid,NetlogonValidationSamInfo2,0)\'");
            temp98 = this.INrpcServerAdapterInstance.NetrLogonSamLogonWithFlags(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.Client, true, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo2, 0u);
            this.Manager.Checkpoint("MS-NRPC_R10189");
            this.Manager.Checkpoint("MS-NRPC_R1240");
            this.Manager.Comment("reaching state \'S172\'");
            this.Manager.Comment("checking step \'return NetrLogonSamLogonWithFlags/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp98, "return of NetrLogonSamLogonWithFlags, state S172");
            this.Manager.Comment("reaching state \'S196\'");
            this.Manager.EndTest();
        }
        #endregion
    }
}
