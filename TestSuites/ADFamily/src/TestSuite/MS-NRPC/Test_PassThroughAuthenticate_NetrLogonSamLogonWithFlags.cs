// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestSuites.Nrpc {
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Reflection;
    using Microsoft.SpecExplorer.Runtime.Testing;
    using Microsoft.Protocols.TestTools;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Spec Explorer", "3.5.3146.0")]
    [Microsoft.VisualStudio.TestTools.UnitTesting.TestClassAttribute()]
    public partial class Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlags : PtfTestClassBase {
        
        public Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlags() {
            this.SetSwitch("ProceedControlTimeout", "100");
            this.SetSwitch("QuiescenceTimeout", "30000");
        }
        
        #region Expect Delegates
        public delegate void GetPlatformDelegate1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform);
        #endregion
        
        #region Event Metadata
        static System.Reflection.MethodBase GetPlatformInfo = TestManagerHelpers.GetMethodInfo(typeof(Microsoft.Protocols.TestSuites.Nrpc.INrpcServerAdapter), "GetPlatform", typeof(Microsoft.Protocols.TestSuites.Nrpc.PlatformType).MakeByRefType());
        #endregion
        
        #region Adapter Instances
        private Microsoft.Protocols.TestSuites.Nrpc.INrpcServerAdapter INrpcServerAdapterInstance;
        #endregion
        
        #region Class Initialization and Cleanup
        [Microsoft.VisualStudio.TestTools.UnitTesting.ClassInitializeAttribute()]
        public static void ClassInitialize(Microsoft.VisualStudio.TestTools.UnitTesting.TestContext context) {
            PtfTestClassBase.Initialize(context);
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.ClassCleanupAttribute()]
        public static void ClassCleanup() {
            PtfTestClassBase.Cleanup();
        }
        #endregion
        
        #region Test Initialization and Cleanup
        protected override void TestInitialize() {
            this.InitializeTestManager();
            this.INrpcServerAdapterInstance = ((Microsoft.Protocols.TestSuites.Nrpc.INrpcServerAdapter)(this.Manager.GetAdapter(typeof(Microsoft.Protocols.TestSuites.Nrpc.INrpcServerAdapter))));
        }
        
        protected override void TestCleanup() {
            base.TestCleanup();
            this.CleanupTestManager();
        }
        #endregion
        
        #region Test Starting in S0
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS0() {
            this.Manager.BeginTest("Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS0");
            this.Manager.Comment("reaching state \'S0\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp0;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp0);
            this.Manager.AddReturn(GetPlatformInfo, null, temp0);
            this.Manager.Comment("reaching state \'S1\'");
            int temp13 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlags.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS0GetPlatformChecker)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlags.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS0GetPlatformChecker1)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlags.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS0GetPlatformChecker2)));
            if ((temp13 == 0)) {
                this.Manager.Comment("reaching state \'S102\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp1;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp1 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S255\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp1, "return of NetrServerReqChallenge, state S255");
                this.Manager.Comment("reaching state \'S408\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp2;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp2 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S561\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp2, "return of NetrServerAuthenticate3, state S561");
                this.Manager.Comment("reaching state \'S714\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp3;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonWithFlags(PrimaryDc,Client,True,NetlogonNet" +
                        "workTransitiveInformation,Valid,NetlogonValidationSamInfo2,2147483648)\'");
                temp3 = this.INrpcServerAdapterInstance.NetrLogonSamLogonWithFlags(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonNetworkTransitiveInformation, ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo2, 2147483648u);
                this.Manager.Checkpoint("MS-NRPC_R1240");
                this.Manager.Comment("reaching state \'S867\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonWithFlags/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp3, "return of NetrLogonSamLogonWithFlags, state S867");
                this.Manager.Comment("reaching state \'S1020\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp4;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp4 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S1173\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp4, "return of NetrServerReqChallenge, state S1173");
                this.Manager.Comment("reaching state \'S1176\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp5;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp5 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S1179\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp5, "return of NetrServerAuthenticate3, state S1179");
                this.Manager.Comment("reaching state \'S1182\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp6;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonWithFlags(PrimaryDc,Client,True,NetlogonNet" +
                        "workTransitiveInformation,Valid,NetlogonValidationSamInfo2,0)\'");
                temp6 = this.INrpcServerAdapterInstance.NetrLogonSamLogonWithFlags(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonNetworkTransitiveInformation, ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo2, 0u);
                this.Manager.Checkpoint("MS-NRPC_R1239");
                this.Manager.Checkpoint("MS-NRPC_R1240");
                this.Manager.Comment("reaching state \'S1185\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonWithFlags/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp6, "return of NetrLogonSamLogonWithFlags, state S1185");
                this.Manager.Comment("reaching state \'S1188\'");
                goto label0;
            }
            if ((temp13 == 1)) {
                this.Manager.Comment("reaching state \'S103\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp7;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp7 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S256\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp7, "return of NetrServerReqChallenge, state S256");
                this.Manager.Comment("reaching state \'S409\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp8;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp8 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S562\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp8, "return of NetrServerAuthenticate3, state S562");
                this.Manager.Comment("reaching state \'S715\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp9;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonWithFlags(PrimaryDc,Client,True,NetlogonNet" +
                        "workTransitiveInformation,Valid,NetlogonValidationSamInfo2,2)\'");
                temp9 = this.INrpcServerAdapterInstance.NetrLogonSamLogonWithFlags(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonNetworkTransitiveInformation, ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo2, 2u);
                this.Manager.Checkpoint("MS-NRPC_R1447");
                this.Manager.Checkpoint("MS-NRPC_R1241");
                this.Manager.Comment("reaching state \'S868\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonWithFlags/STATUS_NO_SUCH_USER\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NO_SUCH_USER, temp9, "return of NetrLogonSamLogonWithFlags, state S868");
                this.Manager.Comment("reaching state \'S1021\'");
                goto label0;
            }
            if ((temp13 == 2)) {
                this.Manager.Comment("reaching state \'S104\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp10;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp10 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S257\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp10, "return of NetrServerReqChallenge, state S257");
                this.Manager.Comment("reaching state \'S410\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp11;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp11 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S563\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp11, "return of NetrServerAuthenticate3, state S563");
                this.Manager.Comment("reaching state \'S716\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp12;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonWithFlags(PrimaryDc,Client,True,NetlogonNet" +
                        "workTransitiveInformation,Valid,NetlogonValidationSamInfo2,2)\'");
                temp12 = this.INrpcServerAdapterInstance.NetrLogonSamLogonWithFlags(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonNetworkTransitiveInformation, ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo2, 2u);
                this.Manager.Checkpoint("MS-NRPC_R1447");
                this.Manager.Checkpoint("MS-NRPC_R1241");
                this.Manager.Comment("reaching state \'S869\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonWithFlags/STATUS_NO_SUCH_USER\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NO_SUCH_USER, temp12, "return of NetrLogonSamLogonWithFlags, state S869");
                this.Manager.Comment("reaching state \'S1022\'");
                goto label0;
            }
            throw new InvalidOperationException("never reached");
        label0:
;
            this.Manager.EndTest();
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS0GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S1");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS0GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S1");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS0GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S1");
        }
        #endregion
        
        #region Test Starting in S10
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS10() {
            this.Manager.BeginTest("Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS10");
            this.Manager.Comment("reaching state \'S10\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp14;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp14);
            this.Manager.AddReturn(GetPlatformInfo, null, temp14);
            this.Manager.Comment("reaching state \'S11\'");
            int temp24 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlags.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS10GetPlatformChecker)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlags.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS10GetPlatformChecker1)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlags.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS10GetPlatformChecker2)));
            if ((temp24 == 0)) {
                this.Manager.Comment("reaching state \'S117\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp15;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp15 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S270\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp15, "return of NetrServerReqChallenge, state S270");
                this.Manager.Comment("reaching state \'S423\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp16;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp16 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S576\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp16, "return of NetrServerAuthenticate3, state S576");
                this.Manager.Comment("reaching state \'S729\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp17;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonWithFlags(PrimaryDc,Client,True,NetlogonSer" +
                        "viceInformation,Valid,NetlogonValidationSamInfo,0)\'");
                temp17 = this.INrpcServerAdapterInstance.NetrLogonSamLogonWithFlags(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonServiceInformation, ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo, 0u);
                this.Manager.Checkpoint("MS-NRPC_R10193");
                this.Manager.Checkpoint("MS-NRPC_R1240");
                this.Manager.Comment("reaching state \'S882\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonWithFlags/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp17, "return of NetrLogonSamLogonWithFlags, state S882");
                this.Manager.Comment("reaching state \'S1035\'");
                goto label1;
            }
            if ((temp24 == 1)) {
                this.Manager.Comment("reaching state \'S118\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp18;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp18 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S271\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp18, "return of NetrServerReqChallenge, state S271");
                this.Manager.Comment("reaching state \'S424\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp19;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp19 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S577\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp19, "return of NetrServerAuthenticate3, state S577");
                this.Manager.Comment("reaching state \'S730\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp20;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonWithFlags(PrimaryDc,Client,True,NetlogonSer" +
                        "viceInformation,Valid,NetlogonValidationSamInfo,0)\'");
                temp20 = this.INrpcServerAdapterInstance.NetrLogonSamLogonWithFlags(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonServiceInformation, ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo, 0u);
                this.Manager.Checkpoint("MS-NRPC_R10193");
                this.Manager.Checkpoint("MS-NRPC_R1240");
                this.Manager.Comment("reaching state \'S883\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonWithFlags/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp20, "return of NetrLogonSamLogonWithFlags, state S883");
                this.Manager.Comment("reaching state \'S1036\'");
                goto label1;
            }
            if ((temp24 == 2)) {
                this.Manager.Comment("reaching state \'S119\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp21;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp21 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S272\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp21, "return of NetrServerReqChallenge, state S272");
                this.Manager.Comment("reaching state \'S425\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp22;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp22 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S578\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp22, "return of NetrServerAuthenticate3, state S578");
                this.Manager.Comment("reaching state \'S731\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp23;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonWithFlags(PrimaryDc,Client,True,NetlogonInt" +
                        "eractiveInformation,Valid,NetlogonValidationSamInfo,0)\'");
                temp23 = this.INrpcServerAdapterInstance.NetrLogonSamLogonWithFlags(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo, 0u);
                this.Manager.Checkpoint("MS-NRPC_R10189");
                this.Manager.Checkpoint("MS-NRPC_R1240");
                this.Manager.Comment("reaching state \'S884\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonWithFlags/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp23, "return of NetrLogonSamLogonWithFlags, state S884");
                this.Manager.Comment("reaching state \'S1037\'");
                goto label1;
            }
            throw new InvalidOperationException("never reached");
        label1:
;
            this.Manager.EndTest();
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS10GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S11");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS10GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S11");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS10GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S11");
        }
        #endregion
        
        #region Test Starting in S100
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS100() {
            this.Manager.BeginTest("Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS100");
            this.Manager.Comment("reaching state \'S100\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp25;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp25);
            this.Manager.AddReturn(GetPlatformInfo, null, temp25);
            this.Manager.Comment("reaching state \'S101\'");
            int temp35 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlags.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS100GetPlatformChecker)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlags.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS100GetPlatformChecker1)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlags.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS100GetPlatformChecker2)));
            if ((temp35 == 0)) {
                this.Manager.Comment("reaching state \'S252\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp26;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp26 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S405\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp26, "return of NetrServerReqChallenge, state S405");
                this.Manager.Comment("reaching state \'S558\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp27;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp27 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S711\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp27, "return of NetrServerAuthenticate3, state S711");
                this.Manager.Comment("reaching state \'S864\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp28;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonWithFlags(PrimaryDc,Client,True,NetlogonNet" +
                        "workTransitiveInformation,Valid,NetlogonValidationSamInfo2,2)\'");
                temp28 = this.INrpcServerAdapterInstance.NetrLogonSamLogonWithFlags(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonNetworkTransitiveInformation, ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo2, 2u);
                this.Manager.Checkpoint("MS-NRPC_R1447");
                this.Manager.Checkpoint("MS-NRPC_R1241");
                this.Manager.Comment("reaching state \'S1017\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonWithFlags/STATUS_NO_SUCH_USER\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NO_SUCH_USER, temp28, "return of NetrLogonSamLogonWithFlags, state S1017");
                this.Manager.Comment("reaching state \'S1170\'");
                goto label2;
            }
            if ((temp35 == 1)) {
                this.Manager.Comment("reaching state \'S253\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp29;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp29 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S406\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp29, "return of NetrServerReqChallenge, state S406");
                this.Manager.Comment("reaching state \'S559\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp30;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp30 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S712\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp30, "return of NetrServerAuthenticate3, state S712");
                this.Manager.Comment("reaching state \'S865\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp31;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonWithFlags(PrimaryDc,Client,True,NetlogonInt" +
                        "eractiveInformation,Valid,NetlogonValidationSamInfo,0)\'");
                temp31 = this.INrpcServerAdapterInstance.NetrLogonSamLogonWithFlags(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo, 0u);
                this.Manager.Checkpoint("MS-NRPC_R10189");
                this.Manager.Checkpoint("MS-NRPC_R1240");
                this.Manager.Comment("reaching state \'S1018\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonWithFlags/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp31, "return of NetrLogonSamLogonWithFlags, state S1018");
                this.Manager.Comment("reaching state \'S1171\'");
                goto label2;
            }
            if ((temp35 == 2)) {
                this.Manager.Comment("reaching state \'S254\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp32;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp32 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S407\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp32, "return of NetrServerReqChallenge, state S407");
                this.Manager.Comment("reaching state \'S560\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp33;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp33 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S713\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp33, "return of NetrServerAuthenticate3, state S713");
                this.Manager.Comment("reaching state \'S866\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp34;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonWithFlags(PrimaryDc,Client,True,NetlogonInt" +
                        "eractiveInformation,Valid,NetlogonValidationSamInfo,0)\'");
                temp34 = this.INrpcServerAdapterInstance.NetrLogonSamLogonWithFlags(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo, 0u);
                this.Manager.Checkpoint("MS-NRPC_R10189");
                this.Manager.Checkpoint("MS-NRPC_R1240");
                this.Manager.Comment("reaching state \'S1019\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonWithFlags/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp34, "return of NetrLogonSamLogonWithFlags, state S1019");
                this.Manager.Comment("reaching state \'S1172\'");
                goto label2;
            }
            throw new InvalidOperationException("never reached");
        label2:
;
            this.Manager.EndTest();
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS100GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S101");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS100GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S101");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS100GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S101");
        }
        #endregion
        
        #region Test Starting in S12
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS12() {
            this.Manager.BeginTest("Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS12");
            this.Manager.Comment("reaching state \'S12\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp36;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp36);
            this.Manager.AddReturn(GetPlatformInfo, null, temp36);
            this.Manager.Comment("reaching state \'S13\'");
            int temp46 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlags.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS12GetPlatformChecker)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlags.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS12GetPlatformChecker1)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlags.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS12GetPlatformChecker2)));
            if ((temp46 == 0)) {
                this.Manager.Comment("reaching state \'S120\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp37;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp37 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S273\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp37, "return of NetrServerReqChallenge, state S273");
                this.Manager.Comment("reaching state \'S426\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp38;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp38 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S579\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp38, "return of NetrServerAuthenticate3, state S579");
                this.Manager.Comment("reaching state \'S732\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp39;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonWithFlags(PrimaryDc,Client,True,NetlogonNet" +
                        "workInformation,Valid,NetlogonValidationSamInfo4,0)\'");
                temp39 = this.INrpcServerAdapterInstance.NetrLogonSamLogonWithFlags(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonNetworkInformation, ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo4, 0u);
                this.Manager.Checkpoint("MS-NRPC_R1240");
                this.Manager.Comment("reaching state \'S885\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonWithFlags/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp39, "return of NetrLogonSamLogonWithFlags, state S885");
                this.Manager.Comment("reaching state \'S1038\'");
                goto label3;
            }
            if ((temp46 == 1)) {
                this.Manager.Comment("reaching state \'S121\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp40;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp40 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S274\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp40, "return of NetrServerReqChallenge, state S274");
                this.Manager.Comment("reaching state \'S427\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp41;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp41 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S580\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp41, "return of NetrServerAuthenticate3, state S580");
                this.Manager.Comment("reaching state \'S733\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp42;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonWithFlags(PrimaryDc,Client,True,NetlogonNet" +
                        "workInformation,Valid,NetlogonValidationSamInfo4,0)\'");
                temp42 = this.INrpcServerAdapterInstance.NetrLogonSamLogonWithFlags(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonNetworkInformation, ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo4, 0u);
                this.Manager.Checkpoint("MS-NRPC_R1240");
                this.Manager.Comment("reaching state \'S886\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonWithFlags/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp42, "return of NetrLogonSamLogonWithFlags, state S886");
                this.Manager.Comment("reaching state \'S1039\'");
                goto label3;
            }
            if ((temp46 == 2)) {
                this.Manager.Comment("reaching state \'S122\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp43;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp43 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S275\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp43, "return of NetrServerReqChallenge, state S275");
                this.Manager.Comment("reaching state \'S428\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp44;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp44 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S581\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp44, "return of NetrServerAuthenticate3, state S581");
                this.Manager.Comment("reaching state \'S734\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp45;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonWithFlags(PrimaryDc,Client,True,NetlogonInt" +
                        "eractiveInformation,Valid,NetlogonValidationSamInfo,0)\'");
                temp45 = this.INrpcServerAdapterInstance.NetrLogonSamLogonWithFlags(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo, 0u);
                this.Manager.Checkpoint("MS-NRPC_R10189");
                this.Manager.Checkpoint("MS-NRPC_R1240");
                this.Manager.Comment("reaching state \'S887\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonWithFlags/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp45, "return of NetrLogonSamLogonWithFlags, state S887");
                this.Manager.Comment("reaching state \'S1040\'");
                goto label3;
            }
            throw new InvalidOperationException("never reached");
        label3:
;
            this.Manager.EndTest();
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS12GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S13");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS12GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S13");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS12GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S13");
        }
        #endregion
        
        #region Test Starting in S14
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS14() {
            this.Manager.BeginTest("Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS14");
            this.Manager.Comment("reaching state \'S14\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp47;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp47);
            this.Manager.AddReturn(GetPlatformInfo, null, temp47);
            this.Manager.Comment("reaching state \'S15\'");
            int temp57 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlags.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS14GetPlatformChecker)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlags.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS14GetPlatformChecker1)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlags.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS14GetPlatformChecker2)));
            if ((temp57 == 0)) {
                this.Manager.Comment("reaching state \'S123\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp48;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp48 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S276\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp48, "return of NetrServerReqChallenge, state S276");
                this.Manager.Comment("reaching state \'S429\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp49;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp49 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S582\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp49, "return of NetrServerAuthenticate3, state S582");
                this.Manager.Comment("reaching state \'S735\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp50;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonWithFlags(PrimaryDc,Client,True,NetlogonNet" +
                        "workInformation,Valid,NetlogonValidationSamInfo2,0)\'");
                temp50 = this.INrpcServerAdapterInstance.NetrLogonSamLogonWithFlags(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonNetworkInformation, ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo2, 0u);
                this.Manager.Checkpoint("MS-NRPC_R1240");
                this.Manager.Comment("reaching state \'S888\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonWithFlags/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp50, "return of NetrLogonSamLogonWithFlags, state S888");
                this.Manager.Comment("reaching state \'S1041\'");
                goto label4;
            }
            if ((temp57 == 1)) {
                this.Manager.Comment("reaching state \'S124\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp51;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp51 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S277\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp51, "return of NetrServerReqChallenge, state S277");
                this.Manager.Comment("reaching state \'S430\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp52;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp52 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S583\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp52, "return of NetrServerAuthenticate3, state S583");
                this.Manager.Comment("reaching state \'S736\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp53;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonWithFlags(PrimaryDc,Client,True,NetlogonNet" +
                        "workInformation,Valid,NetlogonValidationSamInfo2,0)\'");
                temp53 = this.INrpcServerAdapterInstance.NetrLogonSamLogonWithFlags(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonNetworkInformation, ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo2, 0u);
                this.Manager.Checkpoint("MS-NRPC_R1240");
                this.Manager.Comment("reaching state \'S889\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonWithFlags/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp53, "return of NetrLogonSamLogonWithFlags, state S889");
                this.Manager.Comment("reaching state \'S1042\'");
                goto label4;
            }
            if ((temp57 == 2)) {
                this.Manager.Comment("reaching state \'S125\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp54;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp54 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S278\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp54, "return of NetrServerReqChallenge, state S278");
                this.Manager.Comment("reaching state \'S431\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp55;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp55 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S584\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp55, "return of NetrServerAuthenticate3, state S584");
                this.Manager.Comment("reaching state \'S737\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp56;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonWithFlags(PrimaryDc,Client,True,NetlogonInt" +
                        "eractiveInformation,Valid,NetlogonValidationSamInfo,0)\'");
                temp56 = this.INrpcServerAdapterInstance.NetrLogonSamLogonWithFlags(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo, 0u);
                this.Manager.Checkpoint("MS-NRPC_R10189");
                this.Manager.Checkpoint("MS-NRPC_R1240");
                this.Manager.Comment("reaching state \'S890\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonWithFlags/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp56, "return of NetrLogonSamLogonWithFlags, state S890");
                this.Manager.Comment("reaching state \'S1043\'");
                goto label4;
            }
            throw new InvalidOperationException("never reached");
        label4:
;
            this.Manager.EndTest();
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS14GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S15");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS14GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S15");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS14GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S15");
        }
        #endregion
        
        #region Test Starting in S16
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS16() {
            this.Manager.BeginTest("Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS16");
            this.Manager.Comment("reaching state \'S16\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp58;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp58);
            this.Manager.AddReturn(GetPlatformInfo, null, temp58);
            this.Manager.Comment("reaching state \'S17\'");
            int temp68 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlags.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS16GetPlatformChecker)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlags.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS16GetPlatformChecker1)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlags.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS16GetPlatformChecker2)));
            if ((temp68 == 0)) {
                this.Manager.Comment("reaching state \'S126\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp59;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp59 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S279\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp59, "return of NetrServerReqChallenge, state S279");
                this.Manager.Comment("reaching state \'S432\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp60;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp60 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S585\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp60, "return of NetrServerAuthenticate3, state S585");
                this.Manager.Comment("reaching state \'S738\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp61;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonWithFlags(PrimaryDc,Client,True,NetlogonInt" +
                        "eractiveTransitiveInformation,Valid,NetlogonValidationSamInfo,0)\'");
                temp61 = this.INrpcServerAdapterInstance.NetrLogonSamLogonWithFlags(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonInteractiveTransitiveInformation, ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo, 0u);
                this.Manager.Checkpoint("MS-NRPC_R10190");
                this.Manager.Checkpoint("MS-NRPC_R1240");
                this.Manager.Comment("reaching state \'S891\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonWithFlags/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp61, "return of NetrLogonSamLogonWithFlags, state S891");
                this.Manager.Comment("reaching state \'S1044\'");
                goto label5;
            }
            if ((temp68 == 1)) {
                this.Manager.Comment("reaching state \'S127\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp62;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp62 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S280\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp62, "return of NetrServerReqChallenge, state S280");
                this.Manager.Comment("reaching state \'S433\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp63;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp63 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S586\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp63, "return of NetrServerAuthenticate3, state S586");
                this.Manager.Comment("reaching state \'S739\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp64;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonWithFlags(PrimaryDc,Client,True,NetlogonInt" +
                        "eractiveTransitiveInformation,Valid,NetlogonValidationSamInfo,0)\'");
                temp64 = this.INrpcServerAdapterInstance.NetrLogonSamLogonWithFlags(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonInteractiveTransitiveInformation, ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo, 0u);
                this.Manager.Checkpoint("MS-NRPC_R10190");
                this.Manager.Checkpoint("MS-NRPC_R1240");
                this.Manager.Comment("reaching state \'S892\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonWithFlags/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp64, "return of NetrLogonSamLogonWithFlags, state S892");
                this.Manager.Comment("reaching state \'S1045\'");
                goto label5;
            }
            if ((temp68 == 2)) {
                this.Manager.Comment("reaching state \'S128\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp65;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp65 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S281\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp65, "return of NetrServerReqChallenge, state S281");
                this.Manager.Comment("reaching state \'S434\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp66;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp66 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S587\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp66, "return of NetrServerAuthenticate3, state S587");
                this.Manager.Comment("reaching state \'S740\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp67;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonWithFlags(PrimaryDc,Client,True,NetlogonInt" +
                        "eractiveInformation,Valid,NetlogonValidationSamInfo,0)\'");
                temp67 = this.INrpcServerAdapterInstance.NetrLogonSamLogonWithFlags(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo, 0u);
                this.Manager.Checkpoint("MS-NRPC_R10189");
                this.Manager.Checkpoint("MS-NRPC_R1240");
                this.Manager.Comment("reaching state \'S893\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonWithFlags/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp67, "return of NetrLogonSamLogonWithFlags, state S893");
                this.Manager.Comment("reaching state \'S1046\'");
                goto label5;
            }
            throw new InvalidOperationException("never reached");
        label5:
;
            this.Manager.EndTest();
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS16GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S17");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS16GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S17");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS16GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S17");
        }
        #endregion
        
        #region Test Starting in S18
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS18() {
            this.Manager.BeginTest("Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS18");
            this.Manager.Comment("reaching state \'S18\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp69;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp69);
            this.Manager.AddReturn(GetPlatformInfo, null, temp69);
            this.Manager.Comment("reaching state \'S19\'");
            int temp79 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlags.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS18GetPlatformChecker)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlags.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS18GetPlatformChecker1)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlags.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS18GetPlatformChecker2)));
            if ((temp79 == 0)) {
                this.Manager.Comment("reaching state \'S129\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp70;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp70 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S282\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp70, "return of NetrServerReqChallenge, state S282");
                this.Manager.Comment("reaching state \'S435\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp71;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp71 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S588\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp71, "return of NetrServerAuthenticate3, state S588");
                this.Manager.Comment("reaching state \'S741\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp72;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonWithFlags(PrimaryDc,Client,True,NetlogonSer" +
                        "viceInformation,Valid,NetlogonValidationSamInfo4,0)\'");
                temp72 = this.INrpcServerAdapterInstance.NetrLogonSamLogonWithFlags(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonServiceInformation, ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo4, 0u);
                this.Manager.Checkpoint("MS-NRPC_R10193");
                this.Manager.Checkpoint("MS-NRPC_R1240");
                this.Manager.Comment("reaching state \'S894\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonWithFlags/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp72, "return of NetrLogonSamLogonWithFlags, state S894");
                this.Manager.Comment("reaching state \'S1047\'");
                goto label6;
            }
            if ((temp79 == 1)) {
                this.Manager.Comment("reaching state \'S130\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp73;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp73 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S283\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp73, "return of NetrServerReqChallenge, state S283");
                this.Manager.Comment("reaching state \'S436\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp74;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp74 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S589\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp74, "return of NetrServerAuthenticate3, state S589");
                this.Manager.Comment("reaching state \'S742\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp75;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonWithFlags(PrimaryDc,Client,True,NetlogonSer" +
                        "viceInformation,Valid,NetlogonValidationSamInfo4,0)\'");
                temp75 = this.INrpcServerAdapterInstance.NetrLogonSamLogonWithFlags(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonServiceInformation, ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo4, 0u);
                this.Manager.Checkpoint("MS-NRPC_R10193");
                this.Manager.Checkpoint("MS-NRPC_R1240");
                this.Manager.Comment("reaching state \'S895\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonWithFlags/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp75, "return of NetrLogonSamLogonWithFlags, state S895");
                this.Manager.Comment("reaching state \'S1048\'");
                goto label6;
            }
            if ((temp79 == 2)) {
                this.Manager.Comment("reaching state \'S131\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp76;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp76 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S284\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp76, "return of NetrServerReqChallenge, state S284");
                this.Manager.Comment("reaching state \'S437\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp77;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp77 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S590\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp77, "return of NetrServerAuthenticate3, state S590");
                this.Manager.Comment("reaching state \'S743\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp78;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonWithFlags(PrimaryDc,Client,True,NetlogonInt" +
                        "eractiveInformation,Valid,NetlogonValidationSamInfo,0)\'");
                temp78 = this.INrpcServerAdapterInstance.NetrLogonSamLogonWithFlags(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo, 0u);
                this.Manager.Checkpoint("MS-NRPC_R10189");
                this.Manager.Checkpoint("MS-NRPC_R1240");
                this.Manager.Comment("reaching state \'S896\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonWithFlags/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp78, "return of NetrLogonSamLogonWithFlags, state S896");
                this.Manager.Comment("reaching state \'S1049\'");
                goto label6;
            }
            throw new InvalidOperationException("never reached");
        label6:
;
            this.Manager.EndTest();
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS18GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S19");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS18GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S19");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS18GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S19");
        }
        #endregion
        
        #region Test Starting in S2
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS2() {
            this.Manager.BeginTest("Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS2");
            this.Manager.Comment("reaching state \'S2\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp80;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp80);
            this.Manager.AddReturn(GetPlatformInfo, null, temp80);
            this.Manager.Comment("reaching state \'S3\'");
            int temp90 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlags.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS2GetPlatformChecker)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlags.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS2GetPlatformChecker1)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlags.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS2GetPlatformChecker2)));
            if ((temp90 == 0)) {
                this.Manager.Comment("reaching state \'S105\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp81;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp81 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S258\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp81, "return of NetrServerReqChallenge, state S258");
                this.Manager.Comment("reaching state \'S411\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp82;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp82 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S564\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp82, "return of NetrServerAuthenticate3, state S564");
                this.Manager.Comment("reaching state \'S717\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp83;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonWithFlags(PrimaryDc,Client,True,NetlogonNet" +
                        "workInformation,Valid,NetlogonValidationSamInfo,0)\'");
                temp83 = this.INrpcServerAdapterInstance.NetrLogonSamLogonWithFlags(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonNetworkInformation, ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo, 0u);
                this.Manager.Checkpoint("MS-NRPC_R1240");
                this.Manager.Comment("reaching state \'S870\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonWithFlags/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp83, "return of NetrLogonSamLogonWithFlags, state S870");
                this.Manager.Comment("reaching state \'S1023\'");
                goto label7;
            }
            if ((temp90 == 1)) {
                this.Manager.Comment("reaching state \'S106\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp84;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp84 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S259\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp84, "return of NetrServerReqChallenge, state S259");
                this.Manager.Comment("reaching state \'S412\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp85;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp85 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S565\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp85, "return of NetrServerAuthenticate3, state S565");
                this.Manager.Comment("reaching state \'S718\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp86;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonWithFlags(PrimaryDc,Client,True,NetlogonInt" +
                        "eractiveInformation,Valid,NetlogonValidationSamInfo,0)\'");
                temp86 = this.INrpcServerAdapterInstance.NetrLogonSamLogonWithFlags(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo, 0u);
                this.Manager.Checkpoint("MS-NRPC_R10189");
                this.Manager.Checkpoint("MS-NRPC_R1240");
                this.Manager.Comment("reaching state \'S871\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonWithFlags/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp86, "return of NetrLogonSamLogonWithFlags, state S871");
                this.Manager.Comment("reaching state \'S1024\'");
                goto label7;
            }
            if ((temp90 == 2)) {
                this.Manager.Comment("reaching state \'S107\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp87;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp87 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S260\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp87, "return of NetrServerReqChallenge, state S260");
                this.Manager.Comment("reaching state \'S413\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp88;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp88 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S566\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp88, "return of NetrServerAuthenticate3, state S566");
                this.Manager.Comment("reaching state \'S719\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp89;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonWithFlags(PrimaryDc,Client,True,NetlogonInt" +
                        "eractiveInformation,Valid,NetlogonValidationSamInfo,0)\'");
                temp89 = this.INrpcServerAdapterInstance.NetrLogonSamLogonWithFlags(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo, 0u);
                this.Manager.Checkpoint("MS-NRPC_R10189");
                this.Manager.Checkpoint("MS-NRPC_R1240");
                this.Manager.Comment("reaching state \'S872\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonWithFlags/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp89, "return of NetrLogonSamLogonWithFlags, state S872");
                this.Manager.Comment("reaching state \'S1025\'");
                goto label7;
            }
            throw new InvalidOperationException("never reached");
        label7:
;
            this.Manager.EndTest();
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS2GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S3");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS2GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S3");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS2GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S3");
        }
        #endregion
        
        #region Test Starting in S20
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS20() {
            this.Manager.BeginTest("Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS20");
            this.Manager.Comment("reaching state \'S20\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp91;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp91);
            this.Manager.AddReturn(GetPlatformInfo, null, temp91);
            this.Manager.Comment("reaching state \'S21\'");
            int temp101 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlags.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS20GetPlatformChecker)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlags.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS20GetPlatformChecker1)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlags.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS20GetPlatformChecker2)));
            if ((temp101 == 0)) {
                this.Manager.Comment("reaching state \'S132\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp92;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp92 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S285\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp92, "return of NetrServerReqChallenge, state S285");
                this.Manager.Comment("reaching state \'S438\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp93;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp93 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S591\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp93, "return of NetrServerAuthenticate3, state S591");
                this.Manager.Comment("reaching state \'S744\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp94;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonWithFlags(PrimaryDc,Client,True,NetlogonSer" +
                        "viceInformation,Valid,NetlogonValidationSamInfo2,0)\'");
                temp94 = this.INrpcServerAdapterInstance.NetrLogonSamLogonWithFlags(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonServiceInformation, ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo2, 0u);
                this.Manager.Checkpoint("MS-NRPC_R10193");
                this.Manager.Checkpoint("MS-NRPC_R1240");
                this.Manager.Comment("reaching state \'S897\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonWithFlags/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp94, "return of NetrLogonSamLogonWithFlags, state S897");
                this.Manager.Comment("reaching state \'S1050\'");
                goto label8;
            }
            if ((temp101 == 1)) {
                this.Manager.Comment("reaching state \'S133\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp95;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp95 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S286\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp95, "return of NetrServerReqChallenge, state S286");
                this.Manager.Comment("reaching state \'S439\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp96;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp96 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S592\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp96, "return of NetrServerAuthenticate3, state S592");
                this.Manager.Comment("reaching state \'S745\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp97;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonWithFlags(PrimaryDc,Client,True,NetlogonSer" +
                        "viceInformation,Valid,NetlogonValidationSamInfo2,0)\'");
                temp97 = this.INrpcServerAdapterInstance.NetrLogonSamLogonWithFlags(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonServiceInformation, ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo2, 0u);
                this.Manager.Checkpoint("MS-NRPC_R10193");
                this.Manager.Checkpoint("MS-NRPC_R1240");
                this.Manager.Comment("reaching state \'S898\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonWithFlags/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp97, "return of NetrLogonSamLogonWithFlags, state S898");
                this.Manager.Comment("reaching state \'S1051\'");
                goto label8;
            }
            if ((temp101 == 2)) {
                this.Manager.Comment("reaching state \'S134\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp98;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp98 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S287\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp98, "return of NetrServerReqChallenge, state S287");
                this.Manager.Comment("reaching state \'S440\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp99;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp99 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S593\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp99, "return of NetrServerAuthenticate3, state S593");
                this.Manager.Comment("reaching state \'S746\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp100;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonWithFlags(PrimaryDc,Client,True,NetlogonInt" +
                        "eractiveInformation,Valid,NetlogonValidationSamInfo,0)\'");
                temp100 = this.INrpcServerAdapterInstance.NetrLogonSamLogonWithFlags(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo, 0u);
                this.Manager.Checkpoint("MS-NRPC_R10189");
                this.Manager.Checkpoint("MS-NRPC_R1240");
                this.Manager.Comment("reaching state \'S899\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonWithFlags/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp100, "return of NetrLogonSamLogonWithFlags, state S899");
                this.Manager.Comment("reaching state \'S1052\'");
                goto label8;
            }
            throw new InvalidOperationException("never reached");
        label8:
;
            this.Manager.EndTest();
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS20GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S21");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS20GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S21");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS20GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S21");
        }
        #endregion
        
        #region Test Starting in S22
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS22() {
            this.Manager.BeginTest("Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS22");
            this.Manager.Comment("reaching state \'S22\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp102;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp102);
            this.Manager.AddReturn(GetPlatformInfo, null, temp102);
            this.Manager.Comment("reaching state \'S23\'");
            int temp112 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlags.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS22GetPlatformChecker)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlags.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS22GetPlatformChecker1)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlags.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS22GetPlatformChecker2)));
            if ((temp112 == 0)) {
                this.Manager.Comment("reaching state \'S135\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp103;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp103 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S288\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp103, "return of NetrServerReqChallenge, state S288");
                this.Manager.Comment("reaching state \'S441\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp104;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp104 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S594\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp104, "return of NetrServerAuthenticate3, state S594");
                this.Manager.Comment("reaching state \'S747\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp105;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonWithFlags(PrimaryDc,Client,True,NetlogonSer" +
                        "viceTransitiveInformation,Valid,NetlogonValidationSamInfo,0)\'");
                temp105 = this.INrpcServerAdapterInstance.NetrLogonSamLogonWithFlags(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonServiceTransitiveInformation, ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo, 0u);
                this.Manager.Checkpoint("MS-NRPC_R10194");
                this.Manager.Checkpoint("MS-NRPC_R1240");
                this.Manager.Comment("reaching state \'S900\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonWithFlags/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp105, "return of NetrLogonSamLogonWithFlags, state S900");
                this.Manager.Comment("reaching state \'S1053\'");
                goto label9;
            }
            if ((temp112 == 1)) {
                this.Manager.Comment("reaching state \'S136\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp106;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp106 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S289\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp106, "return of NetrServerReqChallenge, state S289");
                this.Manager.Comment("reaching state \'S442\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp107;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp107 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S595\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp107, "return of NetrServerAuthenticate3, state S595");
                this.Manager.Comment("reaching state \'S748\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp108;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonWithFlags(PrimaryDc,Client,True,NetlogonSer" +
                        "viceTransitiveInformation,Valid,NetlogonValidationSamInfo,0)\'");
                temp108 = this.INrpcServerAdapterInstance.NetrLogonSamLogonWithFlags(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonServiceTransitiveInformation, ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo, 0u);
                this.Manager.Checkpoint("MS-NRPC_R10194");
                this.Manager.Checkpoint("MS-NRPC_R1240");
                this.Manager.Comment("reaching state \'S901\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonWithFlags/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp108, "return of NetrLogonSamLogonWithFlags, state S901");
                this.Manager.Comment("reaching state \'S1054\'");
                goto label9;
            }
            if ((temp112 == 2)) {
                this.Manager.Comment("reaching state \'S137\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp109;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp109 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S290\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp109, "return of NetrServerReqChallenge, state S290");
                this.Manager.Comment("reaching state \'S443\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp110;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp110 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S596\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp110, "return of NetrServerAuthenticate3, state S596");
                this.Manager.Comment("reaching state \'S749\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp111;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonWithFlags(PrimaryDc,Client,True,NetlogonInt" +
                        "eractiveInformation,Valid,NetlogonValidationSamInfo,0)\'");
                temp111 = this.INrpcServerAdapterInstance.NetrLogonSamLogonWithFlags(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo, 0u);
                this.Manager.Checkpoint("MS-NRPC_R10189");
                this.Manager.Checkpoint("MS-NRPC_R1240");
                this.Manager.Comment("reaching state \'S902\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonWithFlags/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp111, "return of NetrLogonSamLogonWithFlags, state S902");
                this.Manager.Comment("reaching state \'S1055\'");
                goto label9;
            }
            throw new InvalidOperationException("never reached");
        label9:
;
            this.Manager.EndTest();
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS22GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S23");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS22GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S23");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS22GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S23");
        }
        #endregion
        
        #region Test Starting in S24
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS24() {
            this.Manager.BeginTest("Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS24");
            this.Manager.Comment("reaching state \'S24\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp113;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp113);
            this.Manager.AddReturn(GetPlatformInfo, null, temp113);
            this.Manager.Comment("reaching state \'S25\'");
            int temp123 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlags.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS24GetPlatformChecker)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlags.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS24GetPlatformChecker1)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlags.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS24GetPlatformChecker2)));
            if ((temp123 == 0)) {
                this.Manager.Comment("reaching state \'S138\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp114;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp114 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S291\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp114, "return of NetrServerReqChallenge, state S291");
                this.Manager.Comment("reaching state \'S444\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp115;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp115 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S597\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp115, "return of NetrServerAuthenticate3, state S597");
                this.Manager.Comment("reaching state \'S750\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp116;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonWithFlags(PrimaryDc,Client,True,NetlogonNet" +
                        "workTransitiveInformation,Valid,NetlogonValidationSamInfo2,0)\'");
                temp116 = this.INrpcServerAdapterInstance.NetrLogonSamLogonWithFlags(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonNetworkTransitiveInformation, ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo2, 0u);
                this.Manager.Checkpoint("MS-NRPC_R1240");
                this.Manager.Comment("reaching state \'S903\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonWithFlags/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp116, "return of NetrLogonSamLogonWithFlags, state S903");
                this.Manager.Comment("reaching state \'S1056\'");
                goto label10;
            }
            if ((temp123 == 1)) {
                this.Manager.Comment("reaching state \'S139\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp117;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp117 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S292\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp117, "return of NetrServerReqChallenge, state S292");
                this.Manager.Comment("reaching state \'S445\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp118;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp118 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S598\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp118, "return of NetrServerAuthenticate3, state S598");
                this.Manager.Comment("reaching state \'S751\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp119;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonWithFlags(PrimaryDc,Client,True,NetlogonNet" +
                        "workTransitiveInformation,Valid,NetlogonValidationSamInfo2,0)\'");
                temp119 = this.INrpcServerAdapterInstance.NetrLogonSamLogonWithFlags(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonNetworkTransitiveInformation, ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo2, 0u);
                this.Manager.Checkpoint("MS-NRPC_R1240");
                this.Manager.Comment("reaching state \'S904\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonWithFlags/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp119, "return of NetrLogonSamLogonWithFlags, state S904");
                this.Manager.Comment("reaching state \'S1057\'");
                goto label10;
            }
            if ((temp123 == 2)) {
                this.Manager.Comment("reaching state \'S140\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp120;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp120 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S293\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp120, "return of NetrServerReqChallenge, state S293");
                this.Manager.Comment("reaching state \'S446\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp121;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp121 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S599\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp121, "return of NetrServerAuthenticate3, state S599");
                this.Manager.Comment("reaching state \'S752\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp122;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonWithFlags(PrimaryDc,Client,True,NetlogonInt" +
                        "eractiveInformation,Valid,NetlogonValidationSamInfo,0)\'");
                temp122 = this.INrpcServerAdapterInstance.NetrLogonSamLogonWithFlags(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo, 0u);
                this.Manager.Checkpoint("MS-NRPC_R10189");
                this.Manager.Checkpoint("MS-NRPC_R1240");
                this.Manager.Comment("reaching state \'S905\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonWithFlags/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp122, "return of NetrLogonSamLogonWithFlags, state S905");
                this.Manager.Comment("reaching state \'S1058\'");
                goto label10;
            }
            throw new InvalidOperationException("never reached");
        label10:
;
            this.Manager.EndTest();
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS24GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S25");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS24GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S25");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS24GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S25");
        }
        #endregion
        
        #region Test Starting in S26
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS26() {
            this.Manager.BeginTest("Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS26");
            this.Manager.Comment("reaching state \'S26\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp124;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp124);
            this.Manager.AddReturn(GetPlatformInfo, null, temp124);
            this.Manager.Comment("reaching state \'S27\'");
            int temp134 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlags.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS26GetPlatformChecker)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlags.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS26GetPlatformChecker1)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlags.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS26GetPlatformChecker2)));
            if ((temp134 == 0)) {
                this.Manager.Comment("reaching state \'S141\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp125;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp125 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S294\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp125, "return of NetrServerReqChallenge, state S294");
                this.Manager.Comment("reaching state \'S447\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp126;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp126 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S600\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp126, "return of NetrServerAuthenticate3, state S600");
                this.Manager.Comment("reaching state \'S753\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp127;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonWithFlags(PrimaryDc,Client,False,NetlogonNe" +
                        "tworkTransitiveInformation,Valid,NetlogonValidationSamInfo2,0)\'");
                temp127 = this.INrpcServerAdapterInstance.NetrLogonSamLogonWithFlags(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, false, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonNetworkTransitiveInformation, ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo2, 0u);
                this.Manager.Checkpoint("MS-NRPC_R1241");
                this.Manager.Comment("reaching state \'S906\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonWithFlags/STATUS_ACCESS_DENIED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp127, "return of NetrLogonSamLogonWithFlags, state S906");
                this.Manager.Comment("reaching state \'S1059\'");
                goto label11;
            }
            if ((temp134 == 1)) {
                this.Manager.Comment("reaching state \'S142\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp128;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp128 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S295\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp128, "return of NetrServerReqChallenge, state S295");
                this.Manager.Comment("reaching state \'S448\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp129;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp129 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S601\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp129, "return of NetrServerAuthenticate3, state S601");
                this.Manager.Comment("reaching state \'S754\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp130;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonWithFlags(PrimaryDc,Client,False,NetlogonNe" +
                        "tworkTransitiveInformation,Valid,NetlogonValidationSamInfo2,0)\'");
                temp130 = this.INrpcServerAdapterInstance.NetrLogonSamLogonWithFlags(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, false, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonNetworkTransitiveInformation, ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo2, 0u);
                this.Manager.Checkpoint("MS-NRPC_R1241");
                this.Manager.Comment("reaching state \'S907\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonWithFlags/STATUS_ACCESS_DENIED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp130, "return of NetrLogonSamLogonWithFlags, state S907");
                this.Manager.Comment("reaching state \'S1060\'");
                goto label11;
            }
            if ((temp134 == 2)) {
                this.Manager.Comment("reaching state \'S143\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp131;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp131 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S296\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp131, "return of NetrServerReqChallenge, state S296");
                this.Manager.Comment("reaching state \'S449\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp132;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp132 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S602\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp132, "return of NetrServerAuthenticate3, state S602");
                this.Manager.Comment("reaching state \'S755\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp133;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonWithFlags(PrimaryDc,Client,True,NetlogonInt" +
                        "eractiveInformation,Valid,NetlogonValidationSamInfo,0)\'");
                temp133 = this.INrpcServerAdapterInstance.NetrLogonSamLogonWithFlags(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo, 0u);
                this.Manager.Checkpoint("MS-NRPC_R10189");
                this.Manager.Checkpoint("MS-NRPC_R1240");
                this.Manager.Comment("reaching state \'S908\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonWithFlags/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp133, "return of NetrLogonSamLogonWithFlags, state S908");
                this.Manager.Comment("reaching state \'S1061\'");
                goto label11;
            }
            throw new InvalidOperationException("never reached");
        label11:
;
            this.Manager.EndTest();
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS26GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S27");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS26GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S27");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS26GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S27");
        }
        #endregion
        
        #region Test Starting in S28
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS28() {
            this.Manager.BeginTest("Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS28");
            this.Manager.Comment("reaching state \'S28\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp135;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp135);
            this.Manager.AddReturn(GetPlatformInfo, null, temp135);
            this.Manager.Comment("reaching state \'S29\'");
            int temp145 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlags.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS28GetPlatformChecker)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlags.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS28GetPlatformChecker1)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlags.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS28GetPlatformChecker2)));
            if ((temp145 == 0)) {
                this.Manager.Comment("reaching state \'S144\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp136;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp136 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S297\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp136, "return of NetrServerReqChallenge, state S297");
                this.Manager.Comment("reaching state \'S450\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp137;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp137 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S603\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp137, "return of NetrServerAuthenticate3, state S603");
                this.Manager.Comment("reaching state \'S756\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp138;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonWithFlags(PrimaryDc,Client,True,NetlogonNet" +
                        "workTransitiveInformation,Valid,NetlogonValidationSamInfo,0)\'");
                temp138 = this.INrpcServerAdapterInstance.NetrLogonSamLogonWithFlags(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonNetworkTransitiveInformation, ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo, 0u);
                this.Manager.Checkpoint("MS-NRPC_R1240");
                this.Manager.Comment("reaching state \'S909\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonWithFlags/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp138, "return of NetrLogonSamLogonWithFlags, state S909");
                this.Manager.Comment("reaching state \'S1062\'");
                goto label12;
            }
            if ((temp145 == 1)) {
                this.Manager.Comment("reaching state \'S145\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp139;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp139 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S298\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp139, "return of NetrServerReqChallenge, state S298");
                this.Manager.Comment("reaching state \'S451\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp140;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp140 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S604\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp140, "return of NetrServerAuthenticate3, state S604");
                this.Manager.Comment("reaching state \'S757\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp141;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonWithFlags(PrimaryDc,Client,True,NetlogonNet" +
                        "workTransitiveInformation,Valid,NetlogonValidationSamInfo,0)\'");
                temp141 = this.INrpcServerAdapterInstance.NetrLogonSamLogonWithFlags(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonNetworkTransitiveInformation, ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo, 0u);
                this.Manager.Checkpoint("MS-NRPC_R1240");
                this.Manager.Comment("reaching state \'S910\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonWithFlags/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp141, "return of NetrLogonSamLogonWithFlags, state S910");
                this.Manager.Comment("reaching state \'S1063\'");
                goto label12;
            }
            if ((temp145 == 2)) {
                this.Manager.Comment("reaching state \'S146\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp142;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp142 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S299\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp142, "return of NetrServerReqChallenge, state S299");
                this.Manager.Comment("reaching state \'S452\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp143;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp143 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S605\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp143, "return of NetrServerAuthenticate3, state S605");
                this.Manager.Comment("reaching state \'S758\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp144;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonWithFlags(PrimaryDc,Client,True,NetlogonInt" +
                        "eractiveInformation,Valid,NetlogonValidationSamInfo,0)\'");
                temp144 = this.INrpcServerAdapterInstance.NetrLogonSamLogonWithFlags(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo, 0u);
                this.Manager.Checkpoint("MS-NRPC_R10189");
                this.Manager.Checkpoint("MS-NRPC_R1240");
                this.Manager.Comment("reaching state \'S911\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonWithFlags/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp144, "return of NetrLogonSamLogonWithFlags, state S911");
                this.Manager.Comment("reaching state \'S1064\'");
                goto label12;
            }
            throw new InvalidOperationException("never reached");
        label12:
;
            this.Manager.EndTest();
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS28GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S29");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS28GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S29");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS28GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S29");
        }
        #endregion
        
        #region Test Starting in S30
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS30() {
            this.Manager.BeginTest("Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS30");
            this.Manager.Comment("reaching state \'S30\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp146;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp146);
            this.Manager.AddReturn(GetPlatformInfo, null, temp146);
            this.Manager.Comment("reaching state \'S31\'");
            int temp156 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlags.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS30GetPlatformChecker)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlags.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS30GetPlatformChecker1)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlags.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS30GetPlatformChecker2)));
            if ((temp156 == 0)) {
                this.Manager.Comment("reaching state \'S147\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp147;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp147 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S300\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp147, "return of NetrServerReqChallenge, state S300");
                this.Manager.Comment("reaching state \'S453\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp148;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp148 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S606\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp148, "return of NetrServerAuthenticate3, state S606");
                this.Manager.Comment("reaching state \'S759\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp149;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonWithFlags(PrimaryDc,Client,True,NetlogonInt" +
                        "eractiveTransitiveInformation,Valid,NetlogonValidationSamInfo4,0)\'");
                temp149 = this.INrpcServerAdapterInstance.NetrLogonSamLogonWithFlags(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonInteractiveTransitiveInformation, ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo4, 0u);
                this.Manager.Checkpoint("MS-NRPC_R10190");
                this.Manager.Checkpoint("MS-NRPC_R1240");
                this.Manager.Comment("reaching state \'S912\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonWithFlags/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp149, "return of NetrLogonSamLogonWithFlags, state S912");
                this.Manager.Comment("reaching state \'S1065\'");
                goto label13;
            }
            if ((temp156 == 1)) {
                this.Manager.Comment("reaching state \'S148\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp150;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp150 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S301\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp150, "return of NetrServerReqChallenge, state S301");
                this.Manager.Comment("reaching state \'S454\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp151;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp151 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S607\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp151, "return of NetrServerAuthenticate3, state S607");
                this.Manager.Comment("reaching state \'S760\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp152;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonWithFlags(PrimaryDc,Client,True,NetlogonInt" +
                        "eractiveTransitiveInformation,Valid,NetlogonValidationSamInfo4,0)\'");
                temp152 = this.INrpcServerAdapterInstance.NetrLogonSamLogonWithFlags(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonInteractiveTransitiveInformation, ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo4, 0u);
                this.Manager.Checkpoint("MS-NRPC_R10190");
                this.Manager.Checkpoint("MS-NRPC_R1240");
                this.Manager.Comment("reaching state \'S913\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonWithFlags/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp152, "return of NetrLogonSamLogonWithFlags, state S913");
                this.Manager.Comment("reaching state \'S1066\'");
                goto label13;
            }
            if ((temp156 == 2)) {
                this.Manager.Comment("reaching state \'S149\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp153;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp153 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S302\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp153, "return of NetrServerReqChallenge, state S302");
                this.Manager.Comment("reaching state \'S455\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp154;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp154 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S608\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp154, "return of NetrServerAuthenticate3, state S608");
                this.Manager.Comment("reaching state \'S761\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp155;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonWithFlags(PrimaryDc,Client,True,NetlogonInt" +
                        "eractiveInformation,Valid,NetlogonValidationSamInfo,0)\'");
                temp155 = this.INrpcServerAdapterInstance.NetrLogonSamLogonWithFlags(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo, 0u);
                this.Manager.Checkpoint("MS-NRPC_R10189");
                this.Manager.Checkpoint("MS-NRPC_R1240");
                this.Manager.Comment("reaching state \'S914\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonWithFlags/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp155, "return of NetrLogonSamLogonWithFlags, state S914");
                this.Manager.Comment("reaching state \'S1067\'");
                goto label13;
            }
            throw new InvalidOperationException("never reached");
        label13:
;
            this.Manager.EndTest();
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS30GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S31");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS30GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S31");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS30GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S31");
        }
        #endregion
        
        #region Test Starting in S32
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS32() {
            this.Manager.BeginTest("Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS32");
            this.Manager.Comment("reaching state \'S32\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp157;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp157);
            this.Manager.AddReturn(GetPlatformInfo, null, temp157);
            this.Manager.Comment("reaching state \'S33\'");
            int temp167 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlags.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS32GetPlatformChecker)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlags.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS32GetPlatformChecker1)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlags.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS32GetPlatformChecker2)));
            if ((temp167 == 0)) {
                this.Manager.Comment("reaching state \'S150\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp158;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp158 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S303\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp158, "return of NetrServerReqChallenge, state S303");
                this.Manager.Comment("reaching state \'S456\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp159;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp159 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S609\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp159, "return of NetrServerAuthenticate3, state S609");
                this.Manager.Comment("reaching state \'S762\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp160;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonWithFlags(PrimaryDc,Client,True,NetlogonInt" +
                        "eractiveTransitiveInformation,Valid,NetlogonValidationSamInfo2,0)\'");
                temp160 = this.INrpcServerAdapterInstance.NetrLogonSamLogonWithFlags(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonInteractiveTransitiveInformation, ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo2, 0u);
                this.Manager.Checkpoint("MS-NRPC_R10190");
                this.Manager.Checkpoint("MS-NRPC_R1240");
                this.Manager.Comment("reaching state \'S915\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonWithFlags/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp160, "return of NetrLogonSamLogonWithFlags, state S915");
                this.Manager.Comment("reaching state \'S1068\'");
                goto label14;
            }
            if ((temp167 == 1)) {
                this.Manager.Comment("reaching state \'S151\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp161;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp161 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S304\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp161, "return of NetrServerReqChallenge, state S304");
                this.Manager.Comment("reaching state \'S457\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp162;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp162 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S610\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp162, "return of NetrServerAuthenticate3, state S610");
                this.Manager.Comment("reaching state \'S763\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp163;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonWithFlags(PrimaryDc,Client,True,NetlogonInt" +
                        "eractiveTransitiveInformation,Valid,NetlogonValidationSamInfo2,0)\'");
                temp163 = this.INrpcServerAdapterInstance.NetrLogonSamLogonWithFlags(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonInteractiveTransitiveInformation, ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo2, 0u);
                this.Manager.Checkpoint("MS-NRPC_R10190");
                this.Manager.Checkpoint("MS-NRPC_R1240");
                this.Manager.Comment("reaching state \'S916\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonWithFlags/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp163, "return of NetrLogonSamLogonWithFlags, state S916");
                this.Manager.Comment("reaching state \'S1069\'");
                goto label14;
            }
            if ((temp167 == 2)) {
                this.Manager.Comment("reaching state \'S152\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp164;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp164 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S305\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp164, "return of NetrServerReqChallenge, state S305");
                this.Manager.Comment("reaching state \'S458\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp165;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp165 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S611\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp165, "return of NetrServerAuthenticate3, state S611");
                this.Manager.Comment("reaching state \'S764\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp166;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonWithFlags(PrimaryDc,Client,True,NetlogonInt" +
                        "eractiveInformation,Valid,NetlogonValidationSamInfo,0)\'");
                temp166 = this.INrpcServerAdapterInstance.NetrLogonSamLogonWithFlags(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo, 0u);
                this.Manager.Checkpoint("MS-NRPC_R10189");
                this.Manager.Checkpoint("MS-NRPC_R1240");
                this.Manager.Comment("reaching state \'S917\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonWithFlags/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp166, "return of NetrLogonSamLogonWithFlags, state S917");
                this.Manager.Comment("reaching state \'S1070\'");
                goto label14;
            }
            throw new InvalidOperationException("never reached");
        label14:
;
            this.Manager.EndTest();
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS32GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S33");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS32GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S33");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS32GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S33");
        }
        #endregion
        
        #region Test Starting in S34
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS34() {
            this.Manager.BeginTest("Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS34");
            this.Manager.Comment("reaching state \'S34\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp168;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp168);
            this.Manager.AddReturn(GetPlatformInfo, null, temp168);
            this.Manager.Comment("reaching state \'S35\'");
            int temp178 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlags.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS34GetPlatformChecker)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlags.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS34GetPlatformChecker1)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlags.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS34GetPlatformChecker2)));
            if ((temp178 == 0)) {
                this.Manager.Comment("reaching state \'S153\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp169;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp169 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S306\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp169, "return of NetrServerReqChallenge, state S306");
                this.Manager.Comment("reaching state \'S459\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp170;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp170 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S612\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp170, "return of NetrServerAuthenticate3, state S612");
                this.Manager.Comment("reaching state \'S765\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp171;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonWithFlags(NonDcServer,Client,True,NetlogonN" +
                        "etworkTransitiveInformation,Valid,NetlogonValidationSamInfo2,0)\'");
                temp171 = this.INrpcServerAdapterInstance.NetrLogonSamLogonWithFlags(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonNetworkTransitiveInformation, ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo2, 0u);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R1241");
                this.Manager.Comment("reaching state \'S918\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonWithFlags/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp171, "return of NetrLogonSamLogonWithFlags, state S918");
                this.Manager.Comment("reaching state \'S1071\'");
                goto label15;
            }
            if ((temp178 == 1)) {
                this.Manager.Comment("reaching state \'S154\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp172;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp172 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S307\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp172, "return of NetrServerReqChallenge, state S307");
                this.Manager.Comment("reaching state \'S460\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp173;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp173 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S613\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp173, "return of NetrServerAuthenticate3, state S613");
                this.Manager.Comment("reaching state \'S766\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp174;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonWithFlags(NonDcServer,Client,True,NetlogonN" +
                        "etworkTransitiveInformation,Valid,NetlogonValidationSamInfo2,0)\'");
                temp174 = this.INrpcServerAdapterInstance.NetrLogonSamLogonWithFlags(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonNetworkTransitiveInformation, ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo2, 0u);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R1241");
                this.Manager.Comment("reaching state \'S919\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonWithFlags/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp174, "return of NetrLogonSamLogonWithFlags, state S919");
                this.Manager.Comment("reaching state \'S1072\'");
                goto label15;
            }
            if ((temp178 == 2)) {
                this.Manager.Comment("reaching state \'S155\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp175;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp175 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S308\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp175, "return of NetrServerReqChallenge, state S308");
                this.Manager.Comment("reaching state \'S461\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp176;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp176 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S614\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp176, "return of NetrServerAuthenticate3, state S614");
                this.Manager.Comment("reaching state \'S767\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp177;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonWithFlags(PrimaryDc,Client,True,NetlogonInt" +
                        "eractiveInformation,Valid,NetlogonValidationSamInfo,0)\'");
                temp177 = this.INrpcServerAdapterInstance.NetrLogonSamLogonWithFlags(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo, 0u);
                this.Manager.Checkpoint("MS-NRPC_R10189");
                this.Manager.Checkpoint("MS-NRPC_R1240");
                this.Manager.Comment("reaching state \'S920\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonWithFlags/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp177, "return of NetrLogonSamLogonWithFlags, state S920");
                this.Manager.Comment("reaching state \'S1073\'");
                goto label15;
            }
            throw new InvalidOperationException("never reached");
        label15:
;
            this.Manager.EndTest();
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS34GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S35");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS34GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S35");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS34GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S35");
        }
        #endregion
        
        #region Test Starting in S36
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS36() {
            this.Manager.BeginTest("Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS36");
            this.Manager.Comment("reaching state \'S36\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp179;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp179);
            this.Manager.AddReturn(GetPlatformInfo, null, temp179);
            this.Manager.Comment("reaching state \'S37\'");
            int temp189 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlags.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS36GetPlatformChecker)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlags.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS36GetPlatformChecker1)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlags.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS36GetPlatformChecker2)));
            if ((temp189 == 0)) {
                this.Manager.Comment("reaching state \'S156\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp180;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp180 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S309\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp180, "return of NetrServerReqChallenge, state S309");
                this.Manager.Comment("reaching state \'S462\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp181;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp181 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S615\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp181, "return of NetrServerAuthenticate3, state S615");
                this.Manager.Comment("reaching state \'S768\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp182;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonWithFlags(PrimaryDc,Client,True,NetlogonSer" +
                        "viceTransitiveInformation,Valid,NetlogonValidationSamInfo4,0)\'");
                temp182 = this.INrpcServerAdapterInstance.NetrLogonSamLogonWithFlags(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonServiceTransitiveInformation, ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo4, 0u);
                this.Manager.Checkpoint("MS-NRPC_R10194");
                this.Manager.Checkpoint("MS-NRPC_R1240");
                this.Manager.Comment("reaching state \'S921\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonWithFlags/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp182, "return of NetrLogonSamLogonWithFlags, state S921");
                this.Manager.Comment("reaching state \'S1074\'");
                goto label16;
            }
            if ((temp189 == 1)) {
                this.Manager.Comment("reaching state \'S157\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp183;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp183 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S310\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp183, "return of NetrServerReqChallenge, state S310");
                this.Manager.Comment("reaching state \'S463\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp184;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp184 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S616\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp184, "return of NetrServerAuthenticate3, state S616");
                this.Manager.Comment("reaching state \'S769\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp185;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonWithFlags(PrimaryDc,Client,True,NetlogonSer" +
                        "viceTransitiveInformation,Valid,NetlogonValidationSamInfo4,0)\'");
                temp185 = this.INrpcServerAdapterInstance.NetrLogonSamLogonWithFlags(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonServiceTransitiveInformation, ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo4, 0u);
                this.Manager.Checkpoint("MS-NRPC_R10194");
                this.Manager.Checkpoint("MS-NRPC_R1240");
                this.Manager.Comment("reaching state \'S922\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonWithFlags/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp185, "return of NetrLogonSamLogonWithFlags, state S922");
                this.Manager.Comment("reaching state \'S1075\'");
                goto label16;
            }
            if ((temp189 == 2)) {
                this.Manager.Comment("reaching state \'S158\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp186;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp186 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S311\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp186, "return of NetrServerReqChallenge, state S311");
                this.Manager.Comment("reaching state \'S464\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp187;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp187 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S617\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp187, "return of NetrServerAuthenticate3, state S617");
                this.Manager.Comment("reaching state \'S770\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp188;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonWithFlags(PrimaryDc,Client,True,NetlogonInt" +
                        "eractiveInformation,Valid,NetlogonValidationSamInfo,0)\'");
                temp188 = this.INrpcServerAdapterInstance.NetrLogonSamLogonWithFlags(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo, 0u);
                this.Manager.Checkpoint("MS-NRPC_R10189");
                this.Manager.Checkpoint("MS-NRPC_R1240");
                this.Manager.Comment("reaching state \'S923\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonWithFlags/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp188, "return of NetrLogonSamLogonWithFlags, state S923");
                this.Manager.Comment("reaching state \'S1076\'");
                goto label16;
            }
            throw new InvalidOperationException("never reached");
        label16:
;
            this.Manager.EndTest();
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS36GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S37");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS36GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S37");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS36GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S37");
        }
        #endregion
        
        #region Test Starting in S38
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS38() {
            this.Manager.BeginTest("Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS38");
            this.Manager.Comment("reaching state \'S38\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp190;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp190);
            this.Manager.AddReturn(GetPlatformInfo, null, temp190);
            this.Manager.Comment("reaching state \'S39\'");
            int temp200 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlags.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS38GetPlatformChecker)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlags.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS38GetPlatformChecker1)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlags.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS38GetPlatformChecker2)));
            if ((temp200 == 0)) {
                this.Manager.Comment("reaching state \'S159\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp191;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp191 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S312\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp191, "return of NetrServerReqChallenge, state S312");
                this.Manager.Comment("reaching state \'S465\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp192;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp192 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S618\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp192, "return of NetrServerAuthenticate3, state S618");
                this.Manager.Comment("reaching state \'S771\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp193;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonWithFlags(PrimaryDc,Client,True,NetlogonSer" +
                        "viceTransitiveInformation,Valid,NetlogonValidationSamInfo2,0)\'");
                temp193 = this.INrpcServerAdapterInstance.NetrLogonSamLogonWithFlags(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonServiceTransitiveInformation, ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo2, 0u);
                this.Manager.Checkpoint("MS-NRPC_R10194");
                this.Manager.Checkpoint("MS-NRPC_R1240");
                this.Manager.Comment("reaching state \'S924\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonWithFlags/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp193, "return of NetrLogonSamLogonWithFlags, state S924");
                this.Manager.Comment("reaching state \'S1077\'");
                goto label17;
            }
            if ((temp200 == 1)) {
                this.Manager.Comment("reaching state \'S160\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp194;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp194 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S313\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp194, "return of NetrServerReqChallenge, state S313");
                this.Manager.Comment("reaching state \'S466\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp195;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp195 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S619\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp195, "return of NetrServerAuthenticate3, state S619");
                this.Manager.Comment("reaching state \'S772\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp196;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonWithFlags(PrimaryDc,Client,True,NetlogonSer" +
                        "viceTransitiveInformation,Valid,NetlogonValidationSamInfo2,0)\'");
                temp196 = this.INrpcServerAdapterInstance.NetrLogonSamLogonWithFlags(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonServiceTransitiveInformation, ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo2, 0u);
                this.Manager.Checkpoint("MS-NRPC_R10194");
                this.Manager.Checkpoint("MS-NRPC_R1240");
                this.Manager.Comment("reaching state \'S925\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonWithFlags/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp196, "return of NetrLogonSamLogonWithFlags, state S925");
                this.Manager.Comment("reaching state \'S1078\'");
                goto label17;
            }
            if ((temp200 == 2)) {
                this.Manager.Comment("reaching state \'S161\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp197;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp197 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S314\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp197, "return of NetrServerReqChallenge, state S314");
                this.Manager.Comment("reaching state \'S467\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp198;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp198 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S620\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp198, "return of NetrServerAuthenticate3, state S620");
                this.Manager.Comment("reaching state \'S773\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp199;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonWithFlags(PrimaryDc,Client,True,NetlogonInt" +
                        "eractiveInformation,Valid,NetlogonValidationSamInfo,0)\'");
                temp199 = this.INrpcServerAdapterInstance.NetrLogonSamLogonWithFlags(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo, 0u);
                this.Manager.Checkpoint("MS-NRPC_R10189");
                this.Manager.Checkpoint("MS-NRPC_R1240");
                this.Manager.Comment("reaching state \'S926\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonWithFlags/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp199, "return of NetrLogonSamLogonWithFlags, state S926");
                this.Manager.Comment("reaching state \'S1079\'");
                goto label17;
            }
            throw new InvalidOperationException("never reached");
        label17:
;
            this.Manager.EndTest();
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS38GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S39");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS38GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S39");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS38GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S39");
        }
        #endregion
        
        #region Test Starting in S4
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS4() {
            this.Manager.BeginTest("Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS4");
            this.Manager.Comment("reaching state \'S4\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp201;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp201);
            this.Manager.AddReturn(GetPlatformInfo, null, temp201);
            this.Manager.Comment("reaching state \'S5\'");
            int temp211 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlags.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS4GetPlatformChecker)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlags.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS4GetPlatformChecker1)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlags.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS4GetPlatformChecker2)));
            if ((temp211 == 0)) {
                this.Manager.Comment("reaching state \'S108\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp202;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp202 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S261\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp202, "return of NetrServerReqChallenge, state S261");
                this.Manager.Comment("reaching state \'S414\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp203;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp203 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S567\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp203, "return of NetrServerAuthenticate3, state S567");
                this.Manager.Comment("reaching state \'S720\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp204;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonWithFlags(PrimaryDc,Client,True,NetlogonInt" +
                        "eractiveInformation,Valid,NetlogonValidationSamInfo,0)\'");
                temp204 = this.INrpcServerAdapterInstance.NetrLogonSamLogonWithFlags(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo, 0u);
                this.Manager.Checkpoint("MS-NRPC_R10189");
                this.Manager.Checkpoint("MS-NRPC_R1240");
                this.Manager.Comment("reaching state \'S873\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonWithFlags/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp204, "return of NetrLogonSamLogonWithFlags, state S873");
                this.Manager.Comment("reaching state \'S1026\'");
                goto label18;
            }
            if ((temp211 == 1)) {
                this.Manager.Comment("reaching state \'S109\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp205;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp205 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S262\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp205, "return of NetrServerReqChallenge, state S262");
                this.Manager.Comment("reaching state \'S415\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp206;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp206 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S568\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp206, "return of NetrServerAuthenticate3, state S568");
                this.Manager.Comment("reaching state \'S721\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp207;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonWithFlags(PrimaryDc,Client,True,NetlogonNet" +
                        "workInformation,Valid,NetlogonValidationSamInfo,0)\'");
                temp207 = this.INrpcServerAdapterInstance.NetrLogonSamLogonWithFlags(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonNetworkInformation, ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo, 0u);
                this.Manager.Checkpoint("MS-NRPC_R1240");
                this.Manager.Comment("reaching state \'S874\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonWithFlags/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp207, "return of NetrLogonSamLogonWithFlags, state S874");
                this.Manager.Comment("reaching state \'S1027\'");
                goto label18;
            }
            if ((temp211 == 2)) {
                this.Manager.Comment("reaching state \'S110\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp208;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp208 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S263\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp208, "return of NetrServerReqChallenge, state S263");
                this.Manager.Comment("reaching state \'S416\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp209;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp209 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S569\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp209, "return of NetrServerAuthenticate3, state S569");
                this.Manager.Comment("reaching state \'S722\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp210;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonWithFlags(PrimaryDc,Client,True,NetlogonInt" +
                        "eractiveInformation,Valid,NetlogonValidationSamInfo,0)\'");
                temp210 = this.INrpcServerAdapterInstance.NetrLogonSamLogonWithFlags(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo, 0u);
                this.Manager.Checkpoint("MS-NRPC_R10189");
                this.Manager.Checkpoint("MS-NRPC_R1240");
                this.Manager.Comment("reaching state \'S875\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonWithFlags/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp210, "return of NetrLogonSamLogonWithFlags, state S875");
                this.Manager.Comment("reaching state \'S1028\'");
                goto label18;
            }
            throw new InvalidOperationException("never reached");
        label18:
;
            this.Manager.EndTest();
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS4GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S5");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS4GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S5");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS4GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S5");
        }
        #endregion
        
        #region Test Starting in S40
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS40() {
            this.Manager.BeginTest("Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS40");
            this.Manager.Comment("reaching state \'S40\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp212;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp212);
            this.Manager.AddReturn(GetPlatformInfo, null, temp212);
            this.Manager.Comment("reaching state \'S41\'");
            int temp222 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlags.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS40GetPlatformChecker)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlags.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS40GetPlatformChecker1)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlags.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS40GetPlatformChecker2)));
            if ((temp222 == 0)) {
                this.Manager.Comment("reaching state \'S162\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp213;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp213 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S315\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp213, "return of NetrServerReqChallenge, state S315");
                this.Manager.Comment("reaching state \'S468\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp214;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp214 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S621\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp214, "return of NetrServerAuthenticate3, state S621");
                this.Manager.Comment("reaching state \'S774\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp215;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonWithFlags(PrimaryDc,Client,True,NetlogonNet" +
                        "workTransitiveInformation,Valid,NetlogonValidationSamInfo4,0)\'");
                temp215 = this.INrpcServerAdapterInstance.NetrLogonSamLogonWithFlags(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonNetworkTransitiveInformation, ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo4, 0u);
                this.Manager.Checkpoint("MS-NRPC_R1240");
                this.Manager.Comment("reaching state \'S927\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonWithFlags/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp215, "return of NetrLogonSamLogonWithFlags, state S927");
                this.Manager.Comment("reaching state \'S1080\'");
                goto label19;
            }
            if ((temp222 == 1)) {
                this.Manager.Comment("reaching state \'S163\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp216;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp216 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S316\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp216, "return of NetrServerReqChallenge, state S316");
                this.Manager.Comment("reaching state \'S469\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp217;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp217 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S622\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp217, "return of NetrServerAuthenticate3, state S622");
                this.Manager.Comment("reaching state \'S775\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp218;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonWithFlags(PrimaryDc,Client,True,NetlogonNet" +
                        "workTransitiveInformation,Valid,NetlogonValidationSamInfo4,0)\'");
                temp218 = this.INrpcServerAdapterInstance.NetrLogonSamLogonWithFlags(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonNetworkTransitiveInformation, ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo4, 0u);
                this.Manager.Checkpoint("MS-NRPC_R1240");
                this.Manager.Comment("reaching state \'S928\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonWithFlags/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp218, "return of NetrLogonSamLogonWithFlags, state S928");
                this.Manager.Comment("reaching state \'S1081\'");
                goto label19;
            }
            if ((temp222 == 2)) {
                this.Manager.Comment("reaching state \'S164\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp219;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp219 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S317\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp219, "return of NetrServerReqChallenge, state S317");
                this.Manager.Comment("reaching state \'S470\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp220;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp220 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S623\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp220, "return of NetrServerAuthenticate3, state S623");
                this.Manager.Comment("reaching state \'S776\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp221;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonWithFlags(PrimaryDc,Client,True,NetlogonInt" +
                        "eractiveInformation,Valid,NetlogonValidationSamInfo,0)\'");
                temp221 = this.INrpcServerAdapterInstance.NetrLogonSamLogonWithFlags(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo, 0u);
                this.Manager.Checkpoint("MS-NRPC_R10189");
                this.Manager.Checkpoint("MS-NRPC_R1240");
                this.Manager.Comment("reaching state \'S929\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonWithFlags/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp221, "return of NetrLogonSamLogonWithFlags, state S929");
                this.Manager.Comment("reaching state \'S1082\'");
                goto label19;
            }
            throw new InvalidOperationException("never reached");
        label19:
;
            this.Manager.EndTest();
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS40GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S41");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS40GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S41");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS40GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S41");
        }
        #endregion
        
        #region Test Starting in S42
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS42() {
            this.Manager.BeginTest("Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS42");
            this.Manager.Comment("reaching state \'S42\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp223;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp223);
            this.Manager.AddReturn(GetPlatformInfo, null, temp223);
            this.Manager.Comment("reaching state \'S43\'");
            int temp233 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlags.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS42GetPlatformChecker)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlags.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS42GetPlatformChecker1)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlags.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS42GetPlatformChecker2)));
            if ((temp233 == 0)) {
                this.Manager.Comment("reaching state \'S165\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp224;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp224 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S318\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp224, "return of NetrServerReqChallenge, state S318");
                this.Manager.Comment("reaching state \'S471\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp225;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp225 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S624\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp225, "return of NetrServerAuthenticate3, state S624");
                this.Manager.Comment("reaching state \'S777\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp226;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonWithFlags(PrimaryDc,Client,True,NetlogonNet" +
                        "workTransitiveInformation,Valid,NetlogonValidationSamInfo2,2147483648)\'");
                temp226 = this.INrpcServerAdapterInstance.NetrLogonSamLogonWithFlags(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonNetworkTransitiveInformation, ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo2, 2147483648u);
                this.Manager.Checkpoint("MS-NRPC_R1240");
                this.Manager.Comment("reaching state \'S930\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonWithFlags/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp226, "return of NetrLogonSamLogonWithFlags, state S930");
                this.Manager.Comment("reaching state \'S1083\'");
                goto label20;
            }
            if ((temp233 == 1)) {
                this.Manager.Comment("reaching state \'S166\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp227;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp227 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S319\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp227, "return of NetrServerReqChallenge, state S319");
                this.Manager.Comment("reaching state \'S472\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp228;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp228 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S625\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp228, "return of NetrServerAuthenticate3, state S625");
                this.Manager.Comment("reaching state \'S778\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp229;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonWithFlags(PrimaryDc,Client,True,NetlogonNet" +
                        "workTransitiveInformation,Valid,NetlogonValidationSamInfo2,2147483648)\'");
                temp229 = this.INrpcServerAdapterInstance.NetrLogonSamLogonWithFlags(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonNetworkTransitiveInformation, ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo2, 2147483648u);
                this.Manager.Checkpoint("MS-NRPC_R1240");
                this.Manager.Comment("reaching state \'S931\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonWithFlags/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp229, "return of NetrLogonSamLogonWithFlags, state S931");
                this.Manager.Comment("reaching state \'S1084\'");
                goto label20;
            }
            if ((temp233 == 2)) {
                this.Manager.Comment("reaching state \'S167\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp230;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp230 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S320\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp230, "return of NetrServerReqChallenge, state S320");
                this.Manager.Comment("reaching state \'S473\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp231;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp231 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S626\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp231, "return of NetrServerAuthenticate3, state S626");
                this.Manager.Comment("reaching state \'S779\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp232;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonWithFlags(PrimaryDc,Client,True,NetlogonInt" +
                        "eractiveInformation,Valid,NetlogonValidationSamInfo,0)\'");
                temp232 = this.INrpcServerAdapterInstance.NetrLogonSamLogonWithFlags(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo, 0u);
                this.Manager.Checkpoint("MS-NRPC_R10189");
                this.Manager.Checkpoint("MS-NRPC_R1240");
                this.Manager.Comment("reaching state \'S932\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonWithFlags/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp232, "return of NetrLogonSamLogonWithFlags, state S932");
                this.Manager.Comment("reaching state \'S1085\'");
                goto label20;
            }
            throw new InvalidOperationException("never reached");
        label20:
;
            this.Manager.EndTest();
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS42GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S43");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS42GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S43");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS42GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S43");
        }
        #endregion
        
        #region Test Starting in S44
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS44() {
            this.Manager.BeginTest("Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS44");
            this.Manager.Comment("reaching state \'S44\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp234;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp234);
            this.Manager.AddReturn(GetPlatformInfo, null, temp234);
            this.Manager.Comment("reaching state \'S45\'");
            int temp244 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlags.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS44GetPlatformChecker)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlags.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS44GetPlatformChecker1)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlags.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS44GetPlatformChecker2)));
            if ((temp244 == 0)) {
                this.Manager.Comment("reaching state \'S168\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp235;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp235 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S321\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp235, "return of NetrServerReqChallenge, state S321");
                this.Manager.Comment("reaching state \'S474\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp236;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp236 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S627\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp236, "return of NetrServerAuthenticate3, state S627");
                this.Manager.Comment("reaching state \'S780\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp237;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonWithFlags(PrimaryDc,Client,True,NetlogonNet" +
                        "workTransitiveInformation,Valid,NetlogonValidationSamInfo2,4)\'");
                temp237 = this.INrpcServerAdapterInstance.NetrLogonSamLogonWithFlags(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonNetworkTransitiveInformation, ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo2, 4u);
                this.Manager.Checkpoint("MS-NRPC_R1240");
                this.Manager.Comment("reaching state \'S933\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonWithFlags/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp237, "return of NetrLogonSamLogonWithFlags, state S933");
                this.Manager.Comment("reaching state \'S1086\'");
                goto label21;
            }
            if ((temp244 == 1)) {
                this.Manager.Comment("reaching state \'S169\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp238;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp238 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S322\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp238, "return of NetrServerReqChallenge, state S322");
                this.Manager.Comment("reaching state \'S475\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp239;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp239 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S628\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp239, "return of NetrServerAuthenticate3, state S628");
                this.Manager.Comment("reaching state \'S781\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp240;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonWithFlags(PrimaryDc,Client,True,NetlogonNet" +
                        "workTransitiveInformation,Valid,NetlogonValidationSamInfo2,4)\'");
                temp240 = this.INrpcServerAdapterInstance.NetrLogonSamLogonWithFlags(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonNetworkTransitiveInformation, ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo2, 4u);
                this.Manager.Checkpoint("MS-NRPC_R1240");
                this.Manager.Comment("reaching state \'S934\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonWithFlags/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp240, "return of NetrLogonSamLogonWithFlags, state S934");
                this.Manager.Comment("reaching state \'S1087\'");
                goto label21;
            }
            if ((temp244 == 2)) {
                this.Manager.Comment("reaching state \'S170\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp241;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp241 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S323\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp241, "return of NetrServerReqChallenge, state S323");
                this.Manager.Comment("reaching state \'S476\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp242;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp242 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S629\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp242, "return of NetrServerAuthenticate3, state S629");
                this.Manager.Comment("reaching state \'S782\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp243;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonWithFlags(PrimaryDc,Client,True,NetlogonInt" +
                        "eractiveInformation,Valid,NetlogonValidationSamInfo,0)\'");
                temp243 = this.INrpcServerAdapterInstance.NetrLogonSamLogonWithFlags(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo, 0u);
                this.Manager.Checkpoint("MS-NRPC_R10189");
                this.Manager.Checkpoint("MS-NRPC_R1240");
                this.Manager.Comment("reaching state \'S935\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonWithFlags/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp243, "return of NetrLogonSamLogonWithFlags, state S935");
                this.Manager.Comment("reaching state \'S1088\'");
                goto label21;
            }
            throw new InvalidOperationException("never reached");
        label21:
;
            this.Manager.EndTest();
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS44GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S45");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS44GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S45");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS44GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S45");
        }
        #endregion
        
        #region Test Starting in S46
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS46() {
            this.Manager.BeginTest("Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS46");
            this.Manager.Comment("reaching state \'S46\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp245;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp245);
            this.Manager.AddReturn(GetPlatformInfo, null, temp245);
            this.Manager.Comment("reaching state \'S47\'");
            int temp255 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlags.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS46GetPlatformChecker)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlags.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS46GetPlatformChecker1)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlags.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS46GetPlatformChecker2)));
            if ((temp255 == 0)) {
                this.Manager.Comment("reaching state \'S171\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp246;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp246 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S324\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp246, "return of NetrServerReqChallenge, state S324");
                this.Manager.Comment("reaching state \'S477\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp247;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp247 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S630\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp247, "return of NetrServerAuthenticate3, state S630");
                this.Manager.Comment("reaching state \'S783\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp248;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonWithFlags(PrimaryDc,Client,True,NetlogonNet" +
                        "workTransitiveInformation,Valid,NetlogonValidationSamInfo2,8)\'");
                temp248 = this.INrpcServerAdapterInstance.NetrLogonSamLogonWithFlags(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonNetworkTransitiveInformation, ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo2, 8u);
                this.Manager.Checkpoint("MS-NRPC_R1240");
                this.Manager.Comment("reaching state \'S936\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonWithFlags/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp248, "return of NetrLogonSamLogonWithFlags, state S936");
                this.Manager.Comment("reaching state \'S1089\'");
                goto label22;
            }
            if ((temp255 == 1)) {
                this.Manager.Comment("reaching state \'S172\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp249;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp249 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S325\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp249, "return of NetrServerReqChallenge, state S325");
                this.Manager.Comment("reaching state \'S478\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp250;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp250 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S631\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp250, "return of NetrServerAuthenticate3, state S631");
                this.Manager.Comment("reaching state \'S784\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp251;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonWithFlags(PrimaryDc,Client,True,NetlogonNet" +
                        "workTransitiveInformation,Valid,NetlogonValidationSamInfo2,8)\'");
                temp251 = this.INrpcServerAdapterInstance.NetrLogonSamLogonWithFlags(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonNetworkTransitiveInformation, ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo2, 8u);
                this.Manager.Checkpoint("MS-NRPC_R1240");
                this.Manager.Comment("reaching state \'S937\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonWithFlags/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp251, "return of NetrLogonSamLogonWithFlags, state S937");
                this.Manager.Comment("reaching state \'S1090\'");
                goto label22;
            }
            if ((temp255 == 2)) {
                this.Manager.Comment("reaching state \'S173\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp252;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp252 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S326\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp252, "return of NetrServerReqChallenge, state S326");
                this.Manager.Comment("reaching state \'S479\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp253;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp253 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S632\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp253, "return of NetrServerAuthenticate3, state S632");
                this.Manager.Comment("reaching state \'S785\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp254;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonWithFlags(PrimaryDc,Client,True,NetlogonInt" +
                        "eractiveInformation,Valid,NetlogonValidationSamInfo,0)\'");
                temp254 = this.INrpcServerAdapterInstance.NetrLogonSamLogonWithFlags(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo, 0u);
                this.Manager.Checkpoint("MS-NRPC_R10189");
                this.Manager.Checkpoint("MS-NRPC_R1240");
                this.Manager.Comment("reaching state \'S938\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonWithFlags/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp254, "return of NetrLogonSamLogonWithFlags, state S938");
                this.Manager.Comment("reaching state \'S1091\'");
                goto label22;
            }
            throw new InvalidOperationException("never reached");
        label22:
;
            this.Manager.EndTest();
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS46GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S47");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS46GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S47");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS46GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S47");
        }
        #endregion
        
        #region Test Starting in S48
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS48() {
            this.Manager.BeginTest("Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS48");
            this.Manager.Comment("reaching state \'S48\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp256;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp256);
            this.Manager.AddReturn(GetPlatformInfo, null, temp256);
            this.Manager.Comment("reaching state \'S49\'");
            int temp266 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlags.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS48GetPlatformChecker)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlags.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS48GetPlatformChecker1)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlags.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS48GetPlatformChecker2)));
            if ((temp266 == 0)) {
                this.Manager.Comment("reaching state \'S174\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp257;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp257 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S327\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp257, "return of NetrServerReqChallenge, state S327");
                this.Manager.Comment("reaching state \'S480\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp258;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp258 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S633\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp258, "return of NetrServerAuthenticate3, state S633");
                this.Manager.Comment("reaching state \'S786\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp259;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonWithFlags(PrimaryDc,Client,True,NetlogonNet" +
                        "workTransitiveInformation,Valid,NetlogonValidationSamInfo2,1)\'");
                temp259 = this.INrpcServerAdapterInstance.NetrLogonSamLogonWithFlags(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonNetworkTransitiveInformation, ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo2, 1u);
                this.Manager.Checkpoint("MS-NRPC_R1241");
                this.Manager.Comment("reaching state \'S939\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonWithFlags/STATUS_NO_SUCH_USER\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NO_SUCH_USER, temp259, "return of NetrLogonSamLogonWithFlags, state S939");
                this.Manager.Comment("reaching state \'S1092\'");
                goto label23;
            }
            if ((temp266 == 1)) {
                this.Manager.Comment("reaching state \'S175\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp260;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp260 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S328\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp260, "return of NetrServerReqChallenge, state S328");
                this.Manager.Comment("reaching state \'S481\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp261;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp261 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S634\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp261, "return of NetrServerAuthenticate3, state S634");
                this.Manager.Comment("reaching state \'S787\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp262;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonWithFlags(PrimaryDc,Client,True,NetlogonNet" +
                        "workTransitiveInformation,Valid,NetlogonValidationSamInfo2,1)\'");
                temp262 = this.INrpcServerAdapterInstance.NetrLogonSamLogonWithFlags(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonNetworkTransitiveInformation, ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo2, 1u);
                this.Manager.Checkpoint("MS-NRPC_R1241");
                this.Manager.Comment("reaching state \'S940\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonWithFlags/STATUS_NO_SUCH_USER\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NO_SUCH_USER, temp262, "return of NetrLogonSamLogonWithFlags, state S940");
                this.Manager.Comment("reaching state \'S1093\'");
                goto label23;
            }
            if ((temp266 == 2)) {
                this.Manager.Comment("reaching state \'S176\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp263;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp263 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S329\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp263, "return of NetrServerReqChallenge, state S329");
                this.Manager.Comment("reaching state \'S482\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp264;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp264 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S635\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp264, "return of NetrServerAuthenticate3, state S635");
                this.Manager.Comment("reaching state \'S788\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp265;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonWithFlags(PrimaryDc,Client,True,NetlogonInt" +
                        "eractiveInformation,Valid,NetlogonValidationSamInfo,0)\'");
                temp265 = this.INrpcServerAdapterInstance.NetrLogonSamLogonWithFlags(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo, 0u);
                this.Manager.Checkpoint("MS-NRPC_R10189");
                this.Manager.Checkpoint("MS-NRPC_R1240");
                this.Manager.Comment("reaching state \'S941\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonWithFlags/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp265, "return of NetrLogonSamLogonWithFlags, state S941");
                this.Manager.Comment("reaching state \'S1094\'");
                goto label23;
            }
            throw new InvalidOperationException("never reached");
        label23:
;
            this.Manager.EndTest();
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS48GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S49");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS48GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S49");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS48GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S49");
        }
        #endregion
        
        #region Test Starting in S50
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS50() {
            this.Manager.BeginTest("Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS50");
            this.Manager.Comment("reaching state \'S50\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp267;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp267);
            this.Manager.AddReturn(GetPlatformInfo, null, temp267);
            this.Manager.Comment("reaching state \'S51\'");
            int temp283 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlags.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS50GetPlatformChecker)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlags.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS50GetPlatformChecker1)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlags.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS50GetPlatformChecker2)));
            if ((temp283 == 0)) {
                this.Manager.Comment("reaching state \'S177\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp268;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp268 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S330\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp268, "return of NetrServerReqChallenge, state S330");
                this.Manager.Comment("reaching state \'S483\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp269;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp269 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S636\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp269, "return of NetrServerAuthenticate3, state S636");
                this.Manager.Comment("reaching state \'S789\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp270;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonWithFlags(PrimaryDc,Client,True,NetlogonNet" +
                        "workTransitiveInformation,Valid,NetlogonValidationSamInfo2,2147483648)\'");
                temp270 = this.INrpcServerAdapterInstance.NetrLogonSamLogonWithFlags(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonNetworkTransitiveInformation, ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo2, 2147483648u);
                this.Manager.Checkpoint("MS-NRPC_R1240");
                this.Manager.Comment("reaching state \'S942\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonWithFlags/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp270, "return of NetrLogonSamLogonWithFlags, state S942");
                this.Manager.Comment("reaching state \'S1095\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp271;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp271 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S1174\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp271, "return of NetrServerReqChallenge, state S1174");
                this.Manager.Comment("reaching state \'S1177\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp272;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp272 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S1180\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp272, "return of NetrServerAuthenticate3, state S1180");
                this.Manager.Comment("reaching state \'S1183\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp273;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonWithFlags(PrimaryDc,Client,True,NetlogonNet" +
                        "workTransitiveInformation,Valid,NetlogonValidationSamInfo2,0)\'");
                temp273 = this.INrpcServerAdapterInstance.NetrLogonSamLogonWithFlags(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonNetworkTransitiveInformation, ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo2, 0u);
                this.Manager.Checkpoint("MS-NRPC_R1239");
                this.Manager.Checkpoint("MS-NRPC_R1240");
                this.Manager.Comment("reaching state \'S1186\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonWithFlags/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp273, "return of NetrLogonSamLogonWithFlags, state S1186");
                this.Manager.Comment("reaching state \'S1189\'");
                goto label24;
            }
            if ((temp283 == 1)) {
                this.Manager.Comment("reaching state \'S178\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp274;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp274 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S331\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp274, "return of NetrServerReqChallenge, state S331");
                this.Manager.Comment("reaching state \'S484\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp275;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp275 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S637\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp275, "return of NetrServerAuthenticate3, state S637");
                this.Manager.Comment("reaching state \'S790\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp276;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonWithFlags(PrimaryDc,Client,True,NetlogonNet" +
                        "workTransitiveInformation,Valid,NetlogonValidationSamInfo2,2147483648)\'");
                temp276 = this.INrpcServerAdapterInstance.NetrLogonSamLogonWithFlags(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonNetworkTransitiveInformation, ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo2, 2147483648u);
                this.Manager.Checkpoint("MS-NRPC_R1240");
                this.Manager.Comment("reaching state \'S943\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonWithFlags/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp276, "return of NetrLogonSamLogonWithFlags, state S943");
                this.Manager.Comment("reaching state \'S1096\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp277;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp277 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S1175\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp277, "return of NetrServerReqChallenge, state S1175");
                this.Manager.Comment("reaching state \'S1178\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp278;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp278 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S1181\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp278, "return of NetrServerAuthenticate3, state S1181");
                this.Manager.Comment("reaching state \'S1184\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp279;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonWithFlags(PrimaryDc,Client,True,NetlogonNet" +
                        "workTransitiveInformation,Valid,NetlogonValidationSamInfo2,0)\'");
                temp279 = this.INrpcServerAdapterInstance.NetrLogonSamLogonWithFlags(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonNetworkTransitiveInformation, ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo2, 0u);
                this.Manager.Checkpoint("MS-NRPC_R1239");
                this.Manager.Checkpoint("MS-NRPC_R1240");
                this.Manager.Comment("reaching state \'S1187\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonWithFlags/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp279, "return of NetrLogonSamLogonWithFlags, state S1187");
                this.Manager.Comment("reaching state \'S1190\'");
                goto label24;
            }
            if ((temp283 == 2)) {
                this.Manager.Comment("reaching state \'S179\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp280;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp280 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S332\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp280, "return of NetrServerReqChallenge, state S332");
                this.Manager.Comment("reaching state \'S485\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp281;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp281 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S638\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp281, "return of NetrServerAuthenticate3, state S638");
                this.Manager.Comment("reaching state \'S791\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp282;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonWithFlags(PrimaryDc,Client,True,NetlogonInt" +
                        "eractiveInformation,Valid,NetlogonValidationSamInfo,0)\'");
                temp282 = this.INrpcServerAdapterInstance.NetrLogonSamLogonWithFlags(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo, 0u);
                this.Manager.Checkpoint("MS-NRPC_R10189");
                this.Manager.Checkpoint("MS-NRPC_R1240");
                this.Manager.Comment("reaching state \'S944\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonWithFlags/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp282, "return of NetrLogonSamLogonWithFlags, state S944");
                this.Manager.Comment("reaching state \'S1097\'");
                goto label24;
            }
            throw new InvalidOperationException("never reached");
        label24:
;
            this.Manager.EndTest();
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS50GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S51");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS50GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S51");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS50GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S51");
        }
        #endregion
        
        #region Test Starting in S52
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS52() {
            this.Manager.BeginTest("Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS52");
            this.Manager.Comment("reaching state \'S52\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp284;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp284);
            this.Manager.AddReturn(GetPlatformInfo, null, temp284);
            this.Manager.Comment("reaching state \'S53\'");
            int temp294 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlags.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS52GetPlatformChecker)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlags.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS52GetPlatformChecker1)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlags.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS52GetPlatformChecker2)));
            if ((temp294 == 0)) {
                this.Manager.Comment("reaching state \'S180\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp285;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp285 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S333\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp285, "return of NetrServerReqChallenge, state S333");
                this.Manager.Comment("reaching state \'S486\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp286;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp286 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S639\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp286, "return of NetrServerAuthenticate3, state S639");
                this.Manager.Comment("reaching state \'S792\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp287;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonWithFlags(PrimaryDc,Client,True,NetlogonInt" +
                        "eractiveInformation,Valid,NetlogonValidationSamInfo,0)\'");
                temp287 = this.INrpcServerAdapterInstance.NetrLogonSamLogonWithFlags(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo, 0u);
                this.Manager.Checkpoint("MS-NRPC_R10189");
                this.Manager.Checkpoint("MS-NRPC_R1240");
                this.Manager.Comment("reaching state \'S945\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonWithFlags/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp287, "return of NetrLogonSamLogonWithFlags, state S945");
                this.Manager.Comment("reaching state \'S1098\'");
                goto label25;
            }
            if ((temp294 == 1)) {
                this.Manager.Comment("reaching state \'S181\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp288;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp288 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S334\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp288, "return of NetrServerReqChallenge, state S334");
                this.Manager.Comment("reaching state \'S487\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp289;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp289 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S640\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp289, "return of NetrServerAuthenticate3, state S640");
                this.Manager.Comment("reaching state \'S793\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp290;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonWithFlags(PrimaryDc,Client,True,NetlogonInt" +
                        "eractiveInformation,Valid,NetlogonValidationSamInfo,0)\'");
                temp290 = this.INrpcServerAdapterInstance.NetrLogonSamLogonWithFlags(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo, 0u);
                this.Manager.Checkpoint("MS-NRPC_R10189");
                this.Manager.Checkpoint("MS-NRPC_R1240");
                this.Manager.Comment("reaching state \'S946\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonWithFlags/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp290, "return of NetrLogonSamLogonWithFlags, state S946");
                this.Manager.Comment("reaching state \'S1099\'");
                goto label25;
            }
            if ((temp294 == 2)) {
                this.Manager.Comment("reaching state \'S182\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp291;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp291 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S335\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp291, "return of NetrServerReqChallenge, state S335");
                this.Manager.Comment("reaching state \'S488\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp292;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp292 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S641\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp292, "return of NetrServerAuthenticate3, state S641");
                this.Manager.Comment("reaching state \'S794\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp293;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonWithFlags(PrimaryDc,Client,True,NetlogonInt" +
                        "eractiveInformation,Valid,NetlogonValidationSamInfo,0)\'");
                temp293 = this.INrpcServerAdapterInstance.NetrLogonSamLogonWithFlags(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo, 0u);
                this.Manager.Checkpoint("MS-NRPC_R10189");
                this.Manager.Checkpoint("MS-NRPC_R1240");
                this.Manager.Comment("reaching state \'S947\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonWithFlags/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp293, "return of NetrLogonSamLogonWithFlags, state S947");
                this.Manager.Comment("reaching state \'S1100\'");
                goto label25;
            }
            throw new InvalidOperationException("never reached");
        label25:
;
            this.Manager.EndTest();
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS52GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S53");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS52GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S53");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS52GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S53");
        }
        #endregion
        
        #region Test Starting in S54
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS54() {
            this.Manager.BeginTest("Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS54");
            this.Manager.Comment("reaching state \'S54\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp295;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp295);
            this.Manager.AddReturn(GetPlatformInfo, null, temp295);
            this.Manager.Comment("reaching state \'S55\'");
            int temp305 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlags.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS54GetPlatformChecker)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlags.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS54GetPlatformChecker1)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlags.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS54GetPlatformChecker2)));
            if ((temp305 == 0)) {
                this.Manager.Comment("reaching state \'S183\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp296;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp296 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S336\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp296, "return of NetrServerReqChallenge, state S336");
                this.Manager.Comment("reaching state \'S489\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp297;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp297 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S642\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp297, "return of NetrServerAuthenticate3, state S642");
                this.Manager.Comment("reaching state \'S795\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp298;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonWithFlags(PrimaryDc,Client,True,NetlogonNet" +
                        "workInformation,Valid,NetlogonValidationSamInfo,0)\'");
                temp298 = this.INrpcServerAdapterInstance.NetrLogonSamLogonWithFlags(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonNetworkInformation, ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo, 0u);
                this.Manager.Checkpoint("MS-NRPC_R1240");
                this.Manager.Comment("reaching state \'S948\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonWithFlags/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp298, "return of NetrLogonSamLogonWithFlags, state S948");
                this.Manager.Comment("reaching state \'S1101\'");
                goto label26;
            }
            if ((temp305 == 1)) {
                this.Manager.Comment("reaching state \'S184\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp299;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp299 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S337\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp299, "return of NetrServerReqChallenge, state S337");
                this.Manager.Comment("reaching state \'S490\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp300;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp300 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S643\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp300, "return of NetrServerAuthenticate3, state S643");
                this.Manager.Comment("reaching state \'S796\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp301;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonWithFlags(PrimaryDc,Client,True,NetlogonInt" +
                        "eractiveInformation,Valid,NetlogonValidationSamInfo,0)\'");
                temp301 = this.INrpcServerAdapterInstance.NetrLogonSamLogonWithFlags(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo, 0u);
                this.Manager.Checkpoint("MS-NRPC_R10189");
                this.Manager.Checkpoint("MS-NRPC_R1240");
                this.Manager.Comment("reaching state \'S949\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonWithFlags/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp301, "return of NetrLogonSamLogonWithFlags, state S949");
                this.Manager.Comment("reaching state \'S1102\'");
                goto label26;
            }
            if ((temp305 == 2)) {
                this.Manager.Comment("reaching state \'S185\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp302;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp302 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S338\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp302, "return of NetrServerReqChallenge, state S338");
                this.Manager.Comment("reaching state \'S491\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp303;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp303 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S644\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp303, "return of NetrServerAuthenticate3, state S644");
                this.Manager.Comment("reaching state \'S797\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp304;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonWithFlags(PrimaryDc,Client,True,NetlogonInt" +
                        "eractiveInformation,Valid,NetlogonValidationSamInfo,0)\'");
                temp304 = this.INrpcServerAdapterInstance.NetrLogonSamLogonWithFlags(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo, 0u);
                this.Manager.Checkpoint("MS-NRPC_R10189");
                this.Manager.Checkpoint("MS-NRPC_R1240");
                this.Manager.Comment("reaching state \'S950\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonWithFlags/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp304, "return of NetrLogonSamLogonWithFlags, state S950");
                this.Manager.Comment("reaching state \'S1103\'");
                goto label26;
            }
            throw new InvalidOperationException("never reached");
        label26:
;
            this.Manager.EndTest();
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS54GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S55");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS54GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S55");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS54GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S55");
        }
        #endregion
        
        #region Test Starting in S56
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS56() {
            this.Manager.BeginTest("Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS56");
            this.Manager.Comment("reaching state \'S56\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp306;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp306);
            this.Manager.AddReturn(GetPlatformInfo, null, temp306);
            this.Manager.Comment("reaching state \'S57\'");
            int temp316 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlags.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS56GetPlatformChecker)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlags.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS56GetPlatformChecker1)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlags.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS56GetPlatformChecker2)));
            if ((temp316 == 0)) {
                this.Manager.Comment("reaching state \'S186\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp307;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp307 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S339\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp307, "return of NetrServerReqChallenge, state S339");
                this.Manager.Comment("reaching state \'S492\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp308;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp308 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S645\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp308, "return of NetrServerAuthenticate3, state S645");
                this.Manager.Comment("reaching state \'S798\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp309;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonWithFlags(PrimaryDc,Client,True,NetlogonInt" +
                        "eractiveInformation,Valid,NetlogonValidationSamInfo4,0)\'");
                temp309 = this.INrpcServerAdapterInstance.NetrLogonSamLogonWithFlags(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo4, 0u);
                this.Manager.Checkpoint("MS-NRPC_R10189");
                this.Manager.Checkpoint("MS-NRPC_R1240");
                this.Manager.Comment("reaching state \'S951\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonWithFlags/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp309, "return of NetrLogonSamLogonWithFlags, state S951");
                this.Manager.Comment("reaching state \'S1104\'");
                goto label27;
            }
            if ((temp316 == 1)) {
                this.Manager.Comment("reaching state \'S187\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp310;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp310 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S340\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp310, "return of NetrServerReqChallenge, state S340");
                this.Manager.Comment("reaching state \'S493\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp311;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp311 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S646\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp311, "return of NetrServerAuthenticate3, state S646");
                this.Manager.Comment("reaching state \'S799\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp312;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonWithFlags(PrimaryDc,Client,True,NetlogonInt" +
                        "eractiveInformation,Valid,NetlogonValidationSamInfo,0)\'");
                temp312 = this.INrpcServerAdapterInstance.NetrLogonSamLogonWithFlags(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo, 0u);
                this.Manager.Checkpoint("MS-NRPC_R10189");
                this.Manager.Checkpoint("MS-NRPC_R1240");
                this.Manager.Comment("reaching state \'S952\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonWithFlags/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp312, "return of NetrLogonSamLogonWithFlags, state S952");
                this.Manager.Comment("reaching state \'S1105\'");
                goto label27;
            }
            if ((temp316 == 2)) {
                this.Manager.Comment("reaching state \'S188\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp313;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp313 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S341\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp313, "return of NetrServerReqChallenge, state S341");
                this.Manager.Comment("reaching state \'S494\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp314;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp314 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S647\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp314, "return of NetrServerAuthenticate3, state S647");
                this.Manager.Comment("reaching state \'S800\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp315;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonWithFlags(PrimaryDc,Client,True,NetlogonInt" +
                        "eractiveInformation,Valid,NetlogonValidationSamInfo,0)\'");
                temp315 = this.INrpcServerAdapterInstance.NetrLogonSamLogonWithFlags(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo, 0u);
                this.Manager.Checkpoint("MS-NRPC_R10189");
                this.Manager.Checkpoint("MS-NRPC_R1240");
                this.Manager.Comment("reaching state \'S953\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonWithFlags/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp315, "return of NetrLogonSamLogonWithFlags, state S953");
                this.Manager.Comment("reaching state \'S1106\'");
                goto label27;
            }
            throw new InvalidOperationException("never reached");
        label27:
;
            this.Manager.EndTest();
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS56GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S57");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS56GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S57");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS56GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S57");
        }
        #endregion
        
        #region Test Starting in S58
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS58() {
            this.Manager.BeginTest("Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS58");
            this.Manager.Comment("reaching state \'S58\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp317;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp317);
            this.Manager.AddReturn(GetPlatformInfo, null, temp317);
            this.Manager.Comment("reaching state \'S59\'");
            int temp327 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlags.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS58GetPlatformChecker)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlags.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS58GetPlatformChecker1)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlags.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS58GetPlatformChecker2)));
            if ((temp327 == 0)) {
                this.Manager.Comment("reaching state \'S189\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp318;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp318 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S342\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp318, "return of NetrServerReqChallenge, state S342");
                this.Manager.Comment("reaching state \'S495\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp319;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp319 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S648\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp319, "return of NetrServerAuthenticate3, state S648");
                this.Manager.Comment("reaching state \'S801\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp320;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonWithFlags(PrimaryDc,Client,True,NetlogonInt" +
                        "eractiveInformation,Valid,NetlogonValidationSamInfo2,0)\'");
                temp320 = this.INrpcServerAdapterInstance.NetrLogonSamLogonWithFlags(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo2, 0u);
                this.Manager.Checkpoint("MS-NRPC_R10189");
                this.Manager.Checkpoint("MS-NRPC_R1240");
                this.Manager.Comment("reaching state \'S954\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonWithFlags/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp320, "return of NetrLogonSamLogonWithFlags, state S954");
                this.Manager.Comment("reaching state \'S1107\'");
                goto label28;
            }
            if ((temp327 == 1)) {
                this.Manager.Comment("reaching state \'S190\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp321;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp321 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S343\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp321, "return of NetrServerReqChallenge, state S343");
                this.Manager.Comment("reaching state \'S496\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp322;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp322 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S649\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp322, "return of NetrServerAuthenticate3, state S649");
                this.Manager.Comment("reaching state \'S802\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp323;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonWithFlags(PrimaryDc,Client,True,NetlogonInt" +
                        "eractiveInformation,Valid,NetlogonValidationSamInfo,0)\'");
                temp323 = this.INrpcServerAdapterInstance.NetrLogonSamLogonWithFlags(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo, 0u);
                this.Manager.Checkpoint("MS-NRPC_R10189");
                this.Manager.Checkpoint("MS-NRPC_R1240");
                this.Manager.Comment("reaching state \'S955\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonWithFlags/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp323, "return of NetrLogonSamLogonWithFlags, state S955");
                this.Manager.Comment("reaching state \'S1108\'");
                goto label28;
            }
            if ((temp327 == 2)) {
                this.Manager.Comment("reaching state \'S191\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp324;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp324 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S344\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp324, "return of NetrServerReqChallenge, state S344");
                this.Manager.Comment("reaching state \'S497\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp325;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp325 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S650\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp325, "return of NetrServerAuthenticate3, state S650");
                this.Manager.Comment("reaching state \'S803\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp326;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonWithFlags(PrimaryDc,Client,True,NetlogonInt" +
                        "eractiveInformation,Valid,NetlogonValidationSamInfo,0)\'");
                temp326 = this.INrpcServerAdapterInstance.NetrLogonSamLogonWithFlags(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo, 0u);
                this.Manager.Checkpoint("MS-NRPC_R10189");
                this.Manager.Checkpoint("MS-NRPC_R1240");
                this.Manager.Comment("reaching state \'S956\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonWithFlags/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp326, "return of NetrLogonSamLogonWithFlags, state S956");
                this.Manager.Comment("reaching state \'S1109\'");
                goto label28;
            }
            throw new InvalidOperationException("never reached");
        label28:
;
            this.Manager.EndTest();
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS58GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S59");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS58GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S59");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS58GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S59");
        }
        #endregion
        
        #region Test Starting in S6
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS6() {
            this.Manager.BeginTest("Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS6");
            this.Manager.Comment("reaching state \'S6\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp328;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp328);
            this.Manager.AddReturn(GetPlatformInfo, null, temp328);
            this.Manager.Comment("reaching state \'S7\'");
            int temp338 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlags.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS6GetPlatformChecker)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlags.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS6GetPlatformChecker1)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlags.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS6GetPlatformChecker2)));
            if ((temp338 == 0)) {
                this.Manager.Comment("reaching state \'S111\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp329;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp329 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S264\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp329, "return of NetrServerReqChallenge, state S264");
                this.Manager.Comment("reaching state \'S417\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp330;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp330 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S570\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp330, "return of NetrServerAuthenticate3, state S570");
                this.Manager.Comment("reaching state \'S723\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp331;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonWithFlags(PrimaryDc,Client,True,NetlogonInt" +
                        "eractiveInformation,Valid,NetlogonValidationSamInfo4,0)\'");
                temp331 = this.INrpcServerAdapterInstance.NetrLogonSamLogonWithFlags(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo4, 0u);
                this.Manager.Checkpoint("MS-NRPC_R10189");
                this.Manager.Checkpoint("MS-NRPC_R1240");
                this.Manager.Comment("reaching state \'S876\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonWithFlags/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp331, "return of NetrLogonSamLogonWithFlags, state S876");
                this.Manager.Comment("reaching state \'S1029\'");
                goto label29;
            }
            if ((temp338 == 1)) {
                this.Manager.Comment("reaching state \'S112\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp332;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp332 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S265\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp332, "return of NetrServerReqChallenge, state S265");
                this.Manager.Comment("reaching state \'S418\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp333;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp333 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S571\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp333, "return of NetrServerAuthenticate3, state S571");
                this.Manager.Comment("reaching state \'S724\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp334;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonWithFlags(PrimaryDc,Client,True,NetlogonInt" +
                        "eractiveInformation,Valid,NetlogonValidationSamInfo4,0)\'");
                temp334 = this.INrpcServerAdapterInstance.NetrLogonSamLogonWithFlags(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo4, 0u);
                this.Manager.Checkpoint("MS-NRPC_R10189");
                this.Manager.Checkpoint("MS-NRPC_R1240");
                this.Manager.Comment("reaching state \'S877\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonWithFlags/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp334, "return of NetrLogonSamLogonWithFlags, state S877");
                this.Manager.Comment("reaching state \'S1030\'");
                goto label29;
            }
            if ((temp338 == 2)) {
                this.Manager.Comment("reaching state \'S113\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp335;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp335 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S266\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp335, "return of NetrServerReqChallenge, state S266");
                this.Manager.Comment("reaching state \'S419\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp336;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp336 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S572\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp336, "return of NetrServerAuthenticate3, state S572");
                this.Manager.Comment("reaching state \'S725\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp337;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonWithFlags(PrimaryDc,Client,True,NetlogonInt" +
                        "eractiveInformation,Valid,NetlogonValidationSamInfo,0)\'");
                temp337 = this.INrpcServerAdapterInstance.NetrLogonSamLogonWithFlags(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo, 0u);
                this.Manager.Checkpoint("MS-NRPC_R10189");
                this.Manager.Checkpoint("MS-NRPC_R1240");
                this.Manager.Comment("reaching state \'S878\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonWithFlags/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp337, "return of NetrLogonSamLogonWithFlags, state S878");
                this.Manager.Comment("reaching state \'S1031\'");
                goto label29;
            }
            throw new InvalidOperationException("never reached");
        label29:
;
            this.Manager.EndTest();
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS6GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S7");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS6GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S7");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS6GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S7");
        }
        #endregion
        
        #region Test Starting in S60
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS60() {
            this.Manager.BeginTest("Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS60");
            this.Manager.Comment("reaching state \'S60\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp339;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp339);
            this.Manager.AddReturn(GetPlatformInfo, null, temp339);
            this.Manager.Comment("reaching state \'S61\'");
            int temp349 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlags.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS60GetPlatformChecker)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlags.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS60GetPlatformChecker1)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlags.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS60GetPlatformChecker2)));
            if ((temp349 == 0)) {
                this.Manager.Comment("reaching state \'S192\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp340;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp340 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S345\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp340, "return of NetrServerReqChallenge, state S345");
                this.Manager.Comment("reaching state \'S498\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp341;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp341 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S651\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp341, "return of NetrServerAuthenticate3, state S651");
                this.Manager.Comment("reaching state \'S804\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp342;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonWithFlags(PrimaryDc,Client,True,NetlogonSer" +
                        "viceInformation,Valid,NetlogonValidationSamInfo,0)\'");
                temp342 = this.INrpcServerAdapterInstance.NetrLogonSamLogonWithFlags(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonServiceInformation, ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo, 0u);
                this.Manager.Checkpoint("MS-NRPC_R10193");
                this.Manager.Checkpoint("MS-NRPC_R1240");
                this.Manager.Comment("reaching state \'S957\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonWithFlags/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp342, "return of NetrLogonSamLogonWithFlags, state S957");
                this.Manager.Comment("reaching state \'S1110\'");
                goto label30;
            }
            if ((temp349 == 1)) {
                this.Manager.Comment("reaching state \'S193\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp343;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp343 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S346\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp343, "return of NetrServerReqChallenge, state S346");
                this.Manager.Comment("reaching state \'S499\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp344;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp344 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S652\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp344, "return of NetrServerAuthenticate3, state S652");
                this.Manager.Comment("reaching state \'S805\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp345;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonWithFlags(PrimaryDc,Client,True,NetlogonInt" +
                        "eractiveInformation,Valid,NetlogonValidationSamInfo,0)\'");
                temp345 = this.INrpcServerAdapterInstance.NetrLogonSamLogonWithFlags(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo, 0u);
                this.Manager.Checkpoint("MS-NRPC_R10189");
                this.Manager.Checkpoint("MS-NRPC_R1240");
                this.Manager.Comment("reaching state \'S958\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonWithFlags/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp345, "return of NetrLogonSamLogonWithFlags, state S958");
                this.Manager.Comment("reaching state \'S1111\'");
                goto label30;
            }
            if ((temp349 == 2)) {
                this.Manager.Comment("reaching state \'S194\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp346;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp346 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S347\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp346, "return of NetrServerReqChallenge, state S347");
                this.Manager.Comment("reaching state \'S500\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp347;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp347 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S653\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp347, "return of NetrServerAuthenticate3, state S653");
                this.Manager.Comment("reaching state \'S806\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp348;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonWithFlags(PrimaryDc,Client,True,NetlogonInt" +
                        "eractiveInformation,Valid,NetlogonValidationSamInfo,0)\'");
                temp348 = this.INrpcServerAdapterInstance.NetrLogonSamLogonWithFlags(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo, 0u);
                this.Manager.Checkpoint("MS-NRPC_R10189");
                this.Manager.Checkpoint("MS-NRPC_R1240");
                this.Manager.Comment("reaching state \'S959\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonWithFlags/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp348, "return of NetrLogonSamLogonWithFlags, state S959");
                this.Manager.Comment("reaching state \'S1112\'");
                goto label30;
            }
            throw new InvalidOperationException("never reached");
        label30:
;
            this.Manager.EndTest();
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS60GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S61");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS60GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S61");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS60GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S61");
        }
        #endregion
        
        #region Test Starting in S62
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS62() {
            this.Manager.BeginTest("Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS62");
            this.Manager.Comment("reaching state \'S62\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp350;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp350);
            this.Manager.AddReturn(GetPlatformInfo, null, temp350);
            this.Manager.Comment("reaching state \'S63\'");
            int temp360 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlags.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS62GetPlatformChecker)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlags.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS62GetPlatformChecker1)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlags.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS62GetPlatformChecker2)));
            if ((temp360 == 0)) {
                this.Manager.Comment("reaching state \'S195\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp351;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp351 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S348\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp351, "return of NetrServerReqChallenge, state S348");
                this.Manager.Comment("reaching state \'S501\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp352;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp352 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S654\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp352, "return of NetrServerAuthenticate3, state S654");
                this.Manager.Comment("reaching state \'S807\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp353;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonWithFlags(PrimaryDc,Client,True,NetlogonNet" +
                        "workInformation,Valid,NetlogonValidationSamInfo4,0)\'");
                temp353 = this.INrpcServerAdapterInstance.NetrLogonSamLogonWithFlags(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonNetworkInformation, ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo4, 0u);
                this.Manager.Checkpoint("MS-NRPC_R1240");
                this.Manager.Comment("reaching state \'S960\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonWithFlags/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp353, "return of NetrLogonSamLogonWithFlags, state S960");
                this.Manager.Comment("reaching state \'S1113\'");
                goto label31;
            }
            if ((temp360 == 1)) {
                this.Manager.Comment("reaching state \'S196\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp354;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp354 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S349\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp354, "return of NetrServerReqChallenge, state S349");
                this.Manager.Comment("reaching state \'S502\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp355;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp355 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S655\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp355, "return of NetrServerAuthenticate3, state S655");
                this.Manager.Comment("reaching state \'S808\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp356;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonWithFlags(PrimaryDc,Client,True,NetlogonInt" +
                        "eractiveInformation,Valid,NetlogonValidationSamInfo,0)\'");
                temp356 = this.INrpcServerAdapterInstance.NetrLogonSamLogonWithFlags(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo, 0u);
                this.Manager.Checkpoint("MS-NRPC_R10189");
                this.Manager.Checkpoint("MS-NRPC_R1240");
                this.Manager.Comment("reaching state \'S961\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonWithFlags/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp356, "return of NetrLogonSamLogonWithFlags, state S961");
                this.Manager.Comment("reaching state \'S1114\'");
                goto label31;
            }
            if ((temp360 == 2)) {
                this.Manager.Comment("reaching state \'S197\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp357;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp357 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S350\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp357, "return of NetrServerReqChallenge, state S350");
                this.Manager.Comment("reaching state \'S503\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp358;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp358 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S656\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp358, "return of NetrServerAuthenticate3, state S656");
                this.Manager.Comment("reaching state \'S809\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp359;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonWithFlags(PrimaryDc,Client,True,NetlogonInt" +
                        "eractiveInformation,Valid,NetlogonValidationSamInfo,0)\'");
                temp359 = this.INrpcServerAdapterInstance.NetrLogonSamLogonWithFlags(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo, 0u);
                this.Manager.Checkpoint("MS-NRPC_R10189");
                this.Manager.Checkpoint("MS-NRPC_R1240");
                this.Manager.Comment("reaching state \'S962\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonWithFlags/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp359, "return of NetrLogonSamLogonWithFlags, state S962");
                this.Manager.Comment("reaching state \'S1115\'");
                goto label31;
            }
            throw new InvalidOperationException("never reached");
        label31:
;
            this.Manager.EndTest();
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS62GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S63");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS62GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S63");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS62GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S63");
        }
        #endregion
        
        #region Test Starting in S64
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS64() {
            this.Manager.BeginTest("Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS64");
            this.Manager.Comment("reaching state \'S64\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp361;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp361);
            this.Manager.AddReturn(GetPlatformInfo, null, temp361);
            this.Manager.Comment("reaching state \'S65\'");
            int temp371 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlags.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS64GetPlatformChecker)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlags.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS64GetPlatformChecker1)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlags.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS64GetPlatformChecker2)));
            if ((temp371 == 0)) {
                this.Manager.Comment("reaching state \'S198\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp362;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp362 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S351\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp362, "return of NetrServerReqChallenge, state S351");
                this.Manager.Comment("reaching state \'S504\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp363;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp363 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S657\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp363, "return of NetrServerAuthenticate3, state S657");
                this.Manager.Comment("reaching state \'S810\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp364;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonWithFlags(PrimaryDc,Client,True,NetlogonNet" +
                        "workInformation,Valid,NetlogonValidationSamInfo2,0)\'");
                temp364 = this.INrpcServerAdapterInstance.NetrLogonSamLogonWithFlags(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonNetworkInformation, ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo2, 0u);
                this.Manager.Checkpoint("MS-NRPC_R1240");
                this.Manager.Comment("reaching state \'S963\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonWithFlags/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp364, "return of NetrLogonSamLogonWithFlags, state S963");
                this.Manager.Comment("reaching state \'S1116\'");
                goto label32;
            }
            if ((temp371 == 1)) {
                this.Manager.Comment("reaching state \'S199\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp365;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp365 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S352\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp365, "return of NetrServerReqChallenge, state S352");
                this.Manager.Comment("reaching state \'S505\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp366;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp366 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S658\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp366, "return of NetrServerAuthenticate3, state S658");
                this.Manager.Comment("reaching state \'S811\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp367;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonWithFlags(PrimaryDc,Client,True,NetlogonInt" +
                        "eractiveInformation,Valid,NetlogonValidationSamInfo,0)\'");
                temp367 = this.INrpcServerAdapterInstance.NetrLogonSamLogonWithFlags(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo, 0u);
                this.Manager.Checkpoint("MS-NRPC_R10189");
                this.Manager.Checkpoint("MS-NRPC_R1240");
                this.Manager.Comment("reaching state \'S964\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonWithFlags/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp367, "return of NetrLogonSamLogonWithFlags, state S964");
                this.Manager.Comment("reaching state \'S1117\'");
                goto label32;
            }
            if ((temp371 == 2)) {
                this.Manager.Comment("reaching state \'S200\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp368;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp368 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S353\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp368, "return of NetrServerReqChallenge, state S353");
                this.Manager.Comment("reaching state \'S506\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp369;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp369 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S659\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp369, "return of NetrServerAuthenticate3, state S659");
                this.Manager.Comment("reaching state \'S812\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp370;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonWithFlags(PrimaryDc,Client,True,NetlogonInt" +
                        "eractiveInformation,Valid,NetlogonValidationSamInfo,0)\'");
                temp370 = this.INrpcServerAdapterInstance.NetrLogonSamLogonWithFlags(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo, 0u);
                this.Manager.Checkpoint("MS-NRPC_R10189");
                this.Manager.Checkpoint("MS-NRPC_R1240");
                this.Manager.Comment("reaching state \'S965\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonWithFlags/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp370, "return of NetrLogonSamLogonWithFlags, state S965");
                this.Manager.Comment("reaching state \'S1118\'");
                goto label32;
            }
            throw new InvalidOperationException("never reached");
        label32:
;
            this.Manager.EndTest();
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS64GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S65");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS64GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S65");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS64GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S65");
        }
        #endregion
        
        #region Test Starting in S66
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS66() {
            this.Manager.BeginTest("Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS66");
            this.Manager.Comment("reaching state \'S66\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp372;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp372);
            this.Manager.AddReturn(GetPlatformInfo, null, temp372);
            this.Manager.Comment("reaching state \'S67\'");
            int temp382 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlags.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS66GetPlatformChecker)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlags.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS66GetPlatformChecker1)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlags.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS66GetPlatformChecker2)));
            if ((temp382 == 0)) {
                this.Manager.Comment("reaching state \'S201\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp373;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp373 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S354\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp373, "return of NetrServerReqChallenge, state S354");
                this.Manager.Comment("reaching state \'S507\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp374;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp374 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S660\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp374, "return of NetrServerAuthenticate3, state S660");
                this.Manager.Comment("reaching state \'S813\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp375;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonWithFlags(PrimaryDc,Client,True,NetlogonInt" +
                        "eractiveTransitiveInformation,Valid,NetlogonValidationSamInfo,0)\'");
                temp375 = this.INrpcServerAdapterInstance.NetrLogonSamLogonWithFlags(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonInteractiveTransitiveInformation, ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo, 0u);
                this.Manager.Checkpoint("MS-NRPC_R10190");
                this.Manager.Checkpoint("MS-NRPC_R1240");
                this.Manager.Comment("reaching state \'S966\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonWithFlags/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp375, "return of NetrLogonSamLogonWithFlags, state S966");
                this.Manager.Comment("reaching state \'S1119\'");
                goto label33;
            }
            if ((temp382 == 1)) {
                this.Manager.Comment("reaching state \'S202\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp376;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp376 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S355\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp376, "return of NetrServerReqChallenge, state S355");
                this.Manager.Comment("reaching state \'S508\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp377;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp377 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S661\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp377, "return of NetrServerAuthenticate3, state S661");
                this.Manager.Comment("reaching state \'S814\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp378;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonWithFlags(PrimaryDc,Client,True,NetlogonInt" +
                        "eractiveInformation,Valid,NetlogonValidationSamInfo,0)\'");
                temp378 = this.INrpcServerAdapterInstance.NetrLogonSamLogonWithFlags(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo, 0u);
                this.Manager.Checkpoint("MS-NRPC_R10189");
                this.Manager.Checkpoint("MS-NRPC_R1240");
                this.Manager.Comment("reaching state \'S967\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonWithFlags/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp378, "return of NetrLogonSamLogonWithFlags, state S967");
                this.Manager.Comment("reaching state \'S1120\'");
                goto label33;
            }
            if ((temp382 == 2)) {
                this.Manager.Comment("reaching state \'S203\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp379;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp379 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S356\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp379, "return of NetrServerReqChallenge, state S356");
                this.Manager.Comment("reaching state \'S509\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp380;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp380 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S662\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp380, "return of NetrServerAuthenticate3, state S662");
                this.Manager.Comment("reaching state \'S815\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp381;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonWithFlags(PrimaryDc,Client,True,NetlogonInt" +
                        "eractiveInformation,Valid,NetlogonValidationSamInfo,0)\'");
                temp381 = this.INrpcServerAdapterInstance.NetrLogonSamLogonWithFlags(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo, 0u);
                this.Manager.Checkpoint("MS-NRPC_R10189");
                this.Manager.Checkpoint("MS-NRPC_R1240");
                this.Manager.Comment("reaching state \'S968\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonWithFlags/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp381, "return of NetrLogonSamLogonWithFlags, state S968");
                this.Manager.Comment("reaching state \'S1121\'");
                goto label33;
            }
            throw new InvalidOperationException("never reached");
        label33:
;
            this.Manager.EndTest();
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS66GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S67");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS66GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S67");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS66GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S67");
        }
        #endregion
        
        #region Test Starting in S68
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS68() {
            this.Manager.BeginTest("Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS68");
            this.Manager.Comment("reaching state \'S68\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp383;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp383);
            this.Manager.AddReturn(GetPlatformInfo, null, temp383);
            this.Manager.Comment("reaching state \'S69\'");
            int temp393 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlags.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS68GetPlatformChecker)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlags.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS68GetPlatformChecker1)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlags.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS68GetPlatformChecker2)));
            if ((temp393 == 0)) {
                this.Manager.Comment("reaching state \'S204\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp384;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp384 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S357\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp384, "return of NetrServerReqChallenge, state S357");
                this.Manager.Comment("reaching state \'S510\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp385;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp385 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S663\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp385, "return of NetrServerAuthenticate3, state S663");
                this.Manager.Comment("reaching state \'S816\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp386;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonWithFlags(PrimaryDc,Client,True,NetlogonSer" +
                        "viceInformation,Valid,NetlogonValidationSamInfo4,0)\'");
                temp386 = this.INrpcServerAdapterInstance.NetrLogonSamLogonWithFlags(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonServiceInformation, ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo4, 0u);
                this.Manager.Checkpoint("MS-NRPC_R10193");
                this.Manager.Checkpoint("MS-NRPC_R1240");
                this.Manager.Comment("reaching state \'S969\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonWithFlags/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp386, "return of NetrLogonSamLogonWithFlags, state S969");
                this.Manager.Comment("reaching state \'S1122\'");
                goto label34;
            }
            if ((temp393 == 1)) {
                this.Manager.Comment("reaching state \'S205\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp387;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp387 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S358\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp387, "return of NetrServerReqChallenge, state S358");
                this.Manager.Comment("reaching state \'S511\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp388;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp388 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S664\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp388, "return of NetrServerAuthenticate3, state S664");
                this.Manager.Comment("reaching state \'S817\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp389;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonWithFlags(PrimaryDc,Client,True,NetlogonInt" +
                        "eractiveInformation,Valid,NetlogonValidationSamInfo,0)\'");
                temp389 = this.INrpcServerAdapterInstance.NetrLogonSamLogonWithFlags(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo, 0u);
                this.Manager.Checkpoint("MS-NRPC_R10189");
                this.Manager.Checkpoint("MS-NRPC_R1240");
                this.Manager.Comment("reaching state \'S970\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonWithFlags/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp389, "return of NetrLogonSamLogonWithFlags, state S970");
                this.Manager.Comment("reaching state \'S1123\'");
                goto label34;
            }
            if ((temp393 == 2)) {
                this.Manager.Comment("reaching state \'S206\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp390;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp390 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S359\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp390, "return of NetrServerReqChallenge, state S359");
                this.Manager.Comment("reaching state \'S512\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp391;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp391 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S665\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp391, "return of NetrServerAuthenticate3, state S665");
                this.Manager.Comment("reaching state \'S818\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp392;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonWithFlags(PrimaryDc,Client,True,NetlogonInt" +
                        "eractiveInformation,Valid,NetlogonValidationSamInfo,0)\'");
                temp392 = this.INrpcServerAdapterInstance.NetrLogonSamLogonWithFlags(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo, 0u);
                this.Manager.Checkpoint("MS-NRPC_R10189");
                this.Manager.Checkpoint("MS-NRPC_R1240");
                this.Manager.Comment("reaching state \'S971\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonWithFlags/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp392, "return of NetrLogonSamLogonWithFlags, state S971");
                this.Manager.Comment("reaching state \'S1124\'");
                goto label34;
            }
            throw new InvalidOperationException("never reached");
        label34:
;
            this.Manager.EndTest();
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS68GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S69");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS68GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S69");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS68GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S69");
        }
        #endregion
        
        #region Test Starting in S70
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS70() {
            this.Manager.BeginTest("Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS70");
            this.Manager.Comment("reaching state \'S70\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp394;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp394);
            this.Manager.AddReturn(GetPlatformInfo, null, temp394);
            this.Manager.Comment("reaching state \'S71\'");
            int temp404 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlags.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS70GetPlatformChecker)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlags.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS70GetPlatformChecker1)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlags.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS70GetPlatformChecker2)));
            if ((temp404 == 0)) {
                this.Manager.Comment("reaching state \'S207\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp395;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp395 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S360\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp395, "return of NetrServerReqChallenge, state S360");
                this.Manager.Comment("reaching state \'S513\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp396;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp396 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S666\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp396, "return of NetrServerAuthenticate3, state S666");
                this.Manager.Comment("reaching state \'S819\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp397;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonWithFlags(PrimaryDc,Client,True,NetlogonSer" +
                        "viceInformation,Valid,NetlogonValidationSamInfo2,0)\'");
                temp397 = this.INrpcServerAdapterInstance.NetrLogonSamLogonWithFlags(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonServiceInformation, ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo2, 0u);
                this.Manager.Checkpoint("MS-NRPC_R10193");
                this.Manager.Checkpoint("MS-NRPC_R1240");
                this.Manager.Comment("reaching state \'S972\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonWithFlags/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp397, "return of NetrLogonSamLogonWithFlags, state S972");
                this.Manager.Comment("reaching state \'S1125\'");
                goto label35;
            }
            if ((temp404 == 1)) {
                this.Manager.Comment("reaching state \'S208\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp398;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp398 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S361\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp398, "return of NetrServerReqChallenge, state S361");
                this.Manager.Comment("reaching state \'S514\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp399;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp399 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S667\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp399, "return of NetrServerAuthenticate3, state S667");
                this.Manager.Comment("reaching state \'S820\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp400;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonWithFlags(PrimaryDc,Client,True,NetlogonInt" +
                        "eractiveInformation,Valid,NetlogonValidationSamInfo,0)\'");
                temp400 = this.INrpcServerAdapterInstance.NetrLogonSamLogonWithFlags(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo, 0u);
                this.Manager.Checkpoint("MS-NRPC_R10189");
                this.Manager.Checkpoint("MS-NRPC_R1240");
                this.Manager.Comment("reaching state \'S973\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonWithFlags/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp400, "return of NetrLogonSamLogonWithFlags, state S973");
                this.Manager.Comment("reaching state \'S1126\'");
                goto label35;
            }
            if ((temp404 == 2)) {
                this.Manager.Comment("reaching state \'S209\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp401;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp401 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S362\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp401, "return of NetrServerReqChallenge, state S362");
                this.Manager.Comment("reaching state \'S515\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp402;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp402 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S668\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp402, "return of NetrServerAuthenticate3, state S668");
                this.Manager.Comment("reaching state \'S821\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp403;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonWithFlags(PrimaryDc,Client,True,NetlogonInt" +
                        "eractiveInformation,Valid,NetlogonValidationSamInfo,0)\'");
                temp403 = this.INrpcServerAdapterInstance.NetrLogonSamLogonWithFlags(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo, 0u);
                this.Manager.Checkpoint("MS-NRPC_R10189");
                this.Manager.Checkpoint("MS-NRPC_R1240");
                this.Manager.Comment("reaching state \'S974\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonWithFlags/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp403, "return of NetrLogonSamLogonWithFlags, state S974");
                this.Manager.Comment("reaching state \'S1127\'");
                goto label35;
            }
            throw new InvalidOperationException("never reached");
        label35:
;
            this.Manager.EndTest();
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS70GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S71");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS70GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S71");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS70GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S71");
        }
        #endregion
        
        #region Test Starting in S72
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS72() {
            this.Manager.BeginTest("Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS72");
            this.Manager.Comment("reaching state \'S72\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp405;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp405);
            this.Manager.AddReturn(GetPlatformInfo, null, temp405);
            this.Manager.Comment("reaching state \'S73\'");
            int temp415 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlags.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS72GetPlatformChecker)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlags.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS72GetPlatformChecker1)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlags.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS72GetPlatformChecker2)));
            if ((temp415 == 0)) {
                this.Manager.Comment("reaching state \'S210\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp406;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp406 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S363\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp406, "return of NetrServerReqChallenge, state S363");
                this.Manager.Comment("reaching state \'S516\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp407;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp407 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S669\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp407, "return of NetrServerAuthenticate3, state S669");
                this.Manager.Comment("reaching state \'S822\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp408;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonWithFlags(PrimaryDc,Client,True,NetlogonSer" +
                        "viceTransitiveInformation,Valid,NetlogonValidationSamInfo,0)\'");
                temp408 = this.INrpcServerAdapterInstance.NetrLogonSamLogonWithFlags(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonServiceTransitiveInformation, ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo, 0u);
                this.Manager.Checkpoint("MS-NRPC_R10194");
                this.Manager.Checkpoint("MS-NRPC_R1240");
                this.Manager.Comment("reaching state \'S975\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonWithFlags/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp408, "return of NetrLogonSamLogonWithFlags, state S975");
                this.Manager.Comment("reaching state \'S1128\'");
                goto label36;
            }
            if ((temp415 == 1)) {
                this.Manager.Comment("reaching state \'S211\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp409;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp409 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S364\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp409, "return of NetrServerReqChallenge, state S364");
                this.Manager.Comment("reaching state \'S517\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp410;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp410 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S670\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp410, "return of NetrServerAuthenticate3, state S670");
                this.Manager.Comment("reaching state \'S823\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp411;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonWithFlags(PrimaryDc,Client,True,NetlogonInt" +
                        "eractiveInformation,Valid,NetlogonValidationSamInfo,0)\'");
                temp411 = this.INrpcServerAdapterInstance.NetrLogonSamLogonWithFlags(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo, 0u);
                this.Manager.Checkpoint("MS-NRPC_R10189");
                this.Manager.Checkpoint("MS-NRPC_R1240");
                this.Manager.Comment("reaching state \'S976\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonWithFlags/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp411, "return of NetrLogonSamLogonWithFlags, state S976");
                this.Manager.Comment("reaching state \'S1129\'");
                goto label36;
            }
            if ((temp415 == 2)) {
                this.Manager.Comment("reaching state \'S212\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp412;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp412 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S365\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp412, "return of NetrServerReqChallenge, state S365");
                this.Manager.Comment("reaching state \'S518\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp413;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp413 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S671\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp413, "return of NetrServerAuthenticate3, state S671");
                this.Manager.Comment("reaching state \'S824\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp414;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonWithFlags(PrimaryDc,Client,True,NetlogonInt" +
                        "eractiveInformation,Valid,NetlogonValidationSamInfo,0)\'");
                temp414 = this.INrpcServerAdapterInstance.NetrLogonSamLogonWithFlags(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo, 0u);
                this.Manager.Checkpoint("MS-NRPC_R10189");
                this.Manager.Checkpoint("MS-NRPC_R1240");
                this.Manager.Comment("reaching state \'S977\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonWithFlags/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp414, "return of NetrLogonSamLogonWithFlags, state S977");
                this.Manager.Comment("reaching state \'S1130\'");
                goto label36;
            }
            throw new InvalidOperationException("never reached");
        label36:
;
            this.Manager.EndTest();
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS72GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S73");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS72GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S73");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS72GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S73");
        }
        #endregion
        
        #region Test Starting in S74
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS74() {
            this.Manager.BeginTest("Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS74");
            this.Manager.Comment("reaching state \'S74\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp416;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp416);
            this.Manager.AddReturn(GetPlatformInfo, null, temp416);
            this.Manager.Comment("reaching state \'S75\'");
            int temp426 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlags.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS74GetPlatformChecker)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlags.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS74GetPlatformChecker1)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlags.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS74GetPlatformChecker2)));
            if ((temp426 == 0)) {
                this.Manager.Comment("reaching state \'S213\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp417;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp417 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S366\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp417, "return of NetrServerReqChallenge, state S366");
                this.Manager.Comment("reaching state \'S519\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp418;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp418 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S672\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp418, "return of NetrServerAuthenticate3, state S672");
                this.Manager.Comment("reaching state \'S825\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp419;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonWithFlags(PrimaryDc,Client,True,NetlogonNet" +
                        "workTransitiveInformation,Valid,NetlogonValidationSamInfo2,0)\'");
                temp419 = this.INrpcServerAdapterInstance.NetrLogonSamLogonWithFlags(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonNetworkTransitiveInformation, ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo2, 0u);
                this.Manager.Checkpoint("MS-NRPC_R1240");
                this.Manager.Comment("reaching state \'S978\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonWithFlags/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp419, "return of NetrLogonSamLogonWithFlags, state S978");
                this.Manager.Comment("reaching state \'S1131\'");
                goto label37;
            }
            if ((temp426 == 1)) {
                this.Manager.Comment("reaching state \'S214\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp420;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp420 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S367\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp420, "return of NetrServerReqChallenge, state S367");
                this.Manager.Comment("reaching state \'S520\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp421;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp421 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S673\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp421, "return of NetrServerAuthenticate3, state S673");
                this.Manager.Comment("reaching state \'S826\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp422;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonWithFlags(PrimaryDc,Client,True,NetlogonInt" +
                        "eractiveInformation,Valid,NetlogonValidationSamInfo,0)\'");
                temp422 = this.INrpcServerAdapterInstance.NetrLogonSamLogonWithFlags(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo, 0u);
                this.Manager.Checkpoint("MS-NRPC_R10189");
                this.Manager.Checkpoint("MS-NRPC_R1240");
                this.Manager.Comment("reaching state \'S979\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonWithFlags/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp422, "return of NetrLogonSamLogonWithFlags, state S979");
                this.Manager.Comment("reaching state \'S1132\'");
                goto label37;
            }
            if ((temp426 == 2)) {
                this.Manager.Comment("reaching state \'S215\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp423;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp423 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S368\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp423, "return of NetrServerReqChallenge, state S368");
                this.Manager.Comment("reaching state \'S521\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp424;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp424 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S674\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp424, "return of NetrServerAuthenticate3, state S674");
                this.Manager.Comment("reaching state \'S827\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp425;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonWithFlags(PrimaryDc,Client,True,NetlogonInt" +
                        "eractiveInformation,Valid,NetlogonValidationSamInfo,0)\'");
                temp425 = this.INrpcServerAdapterInstance.NetrLogonSamLogonWithFlags(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo, 0u);
                this.Manager.Checkpoint("MS-NRPC_R10189");
                this.Manager.Checkpoint("MS-NRPC_R1240");
                this.Manager.Comment("reaching state \'S980\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonWithFlags/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp425, "return of NetrLogonSamLogonWithFlags, state S980");
                this.Manager.Comment("reaching state \'S1133\'");
                goto label37;
            }
            throw new InvalidOperationException("never reached");
        label37:
;
            this.Manager.EndTest();
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS74GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S75");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS74GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S75");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS74GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S75");
        }
        #endregion
        
        #region Test Starting in S76
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS76() {
            this.Manager.BeginTest("Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS76");
            this.Manager.Comment("reaching state \'S76\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp427;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp427);
            this.Manager.AddReturn(GetPlatformInfo, null, temp427);
            this.Manager.Comment("reaching state \'S77\'");
            int temp437 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlags.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS76GetPlatformChecker)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlags.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS76GetPlatformChecker1)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlags.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS76GetPlatformChecker2)));
            if ((temp437 == 0)) {
                this.Manager.Comment("reaching state \'S216\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp428;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp428 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S369\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp428, "return of NetrServerReqChallenge, state S369");
                this.Manager.Comment("reaching state \'S522\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp429;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp429 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S675\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp429, "return of NetrServerAuthenticate3, state S675");
                this.Manager.Comment("reaching state \'S828\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp430;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonWithFlags(PrimaryDc,Client,False,NetlogonNe" +
                        "tworkTransitiveInformation,Valid,NetlogonValidationSamInfo2,0)\'");
                temp430 = this.INrpcServerAdapterInstance.NetrLogonSamLogonWithFlags(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, false, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonNetworkTransitiveInformation, ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo2, 0u);
                this.Manager.Checkpoint("MS-NRPC_R1241");
                this.Manager.Comment("reaching state \'S981\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonWithFlags/STATUS_ACCESS_DENIED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp430, "return of NetrLogonSamLogonWithFlags, state S981");
                this.Manager.Comment("reaching state \'S1134\'");
                goto label38;
            }
            if ((temp437 == 1)) {
                this.Manager.Comment("reaching state \'S217\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp431;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp431 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S370\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp431, "return of NetrServerReqChallenge, state S370");
                this.Manager.Comment("reaching state \'S523\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp432;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp432 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S676\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp432, "return of NetrServerAuthenticate3, state S676");
                this.Manager.Comment("reaching state \'S829\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp433;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonWithFlags(PrimaryDc,Client,True,NetlogonInt" +
                        "eractiveInformation,Valid,NetlogonValidationSamInfo,0)\'");
                temp433 = this.INrpcServerAdapterInstance.NetrLogonSamLogonWithFlags(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo, 0u);
                this.Manager.Checkpoint("MS-NRPC_R10189");
                this.Manager.Checkpoint("MS-NRPC_R1240");
                this.Manager.Comment("reaching state \'S982\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonWithFlags/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp433, "return of NetrLogonSamLogonWithFlags, state S982");
                this.Manager.Comment("reaching state \'S1135\'");
                goto label38;
            }
            if ((temp437 == 2)) {
                this.Manager.Comment("reaching state \'S218\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp434;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp434 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S371\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp434, "return of NetrServerReqChallenge, state S371");
                this.Manager.Comment("reaching state \'S524\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp435;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp435 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S677\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp435, "return of NetrServerAuthenticate3, state S677");
                this.Manager.Comment("reaching state \'S830\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp436;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonWithFlags(PrimaryDc,Client,True,NetlogonInt" +
                        "eractiveInformation,Valid,NetlogonValidationSamInfo,0)\'");
                temp436 = this.INrpcServerAdapterInstance.NetrLogonSamLogonWithFlags(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo, 0u);
                this.Manager.Checkpoint("MS-NRPC_R10189");
                this.Manager.Checkpoint("MS-NRPC_R1240");
                this.Manager.Comment("reaching state \'S983\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonWithFlags/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp436, "return of NetrLogonSamLogonWithFlags, state S983");
                this.Manager.Comment("reaching state \'S1136\'");
                goto label38;
            }
            throw new InvalidOperationException("never reached");
        label38:
;
            this.Manager.EndTest();
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS76GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S77");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS76GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S77");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS76GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S77");
        }
        #endregion
        
        #region Test Starting in S78
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS78() {
            this.Manager.BeginTest("Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS78");
            this.Manager.Comment("reaching state \'S78\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp438;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp438);
            this.Manager.AddReturn(GetPlatformInfo, null, temp438);
            this.Manager.Comment("reaching state \'S79\'");
            int temp448 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlags.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS78GetPlatformChecker)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlags.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS78GetPlatformChecker1)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlags.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS78GetPlatformChecker2)));
            if ((temp448 == 0)) {
                this.Manager.Comment("reaching state \'S219\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp439;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp439 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S372\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp439, "return of NetrServerReqChallenge, state S372");
                this.Manager.Comment("reaching state \'S525\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp440;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp440 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S678\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp440, "return of NetrServerAuthenticate3, state S678");
                this.Manager.Comment("reaching state \'S831\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp441;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonWithFlags(PrimaryDc,Client,True,NetlogonNet" +
                        "workTransitiveInformation,Valid,NetlogonValidationSamInfo,0)\'");
                temp441 = this.INrpcServerAdapterInstance.NetrLogonSamLogonWithFlags(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonNetworkTransitiveInformation, ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo, 0u);
                this.Manager.Checkpoint("MS-NRPC_R1240");
                this.Manager.Comment("reaching state \'S984\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonWithFlags/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp441, "return of NetrLogonSamLogonWithFlags, state S984");
                this.Manager.Comment("reaching state \'S1137\'");
                goto label39;
            }
            if ((temp448 == 1)) {
                this.Manager.Comment("reaching state \'S220\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp442;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp442 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S373\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp442, "return of NetrServerReqChallenge, state S373");
                this.Manager.Comment("reaching state \'S526\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp443;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp443 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S679\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp443, "return of NetrServerAuthenticate3, state S679");
                this.Manager.Comment("reaching state \'S832\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp444;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonWithFlags(PrimaryDc,Client,True,NetlogonInt" +
                        "eractiveInformation,Valid,NetlogonValidationSamInfo,0)\'");
                temp444 = this.INrpcServerAdapterInstance.NetrLogonSamLogonWithFlags(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo, 0u);
                this.Manager.Checkpoint("MS-NRPC_R10189");
                this.Manager.Checkpoint("MS-NRPC_R1240");
                this.Manager.Comment("reaching state \'S985\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonWithFlags/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp444, "return of NetrLogonSamLogonWithFlags, state S985");
                this.Manager.Comment("reaching state \'S1138\'");
                goto label39;
            }
            if ((temp448 == 2)) {
                this.Manager.Comment("reaching state \'S221\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp445;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp445 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S374\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp445, "return of NetrServerReqChallenge, state S374");
                this.Manager.Comment("reaching state \'S527\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp446;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp446 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S680\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp446, "return of NetrServerAuthenticate3, state S680");
                this.Manager.Comment("reaching state \'S833\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp447;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonWithFlags(PrimaryDc,Client,True,NetlogonInt" +
                        "eractiveInformation,Valid,NetlogonValidationSamInfo,0)\'");
                temp447 = this.INrpcServerAdapterInstance.NetrLogonSamLogonWithFlags(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo, 0u);
                this.Manager.Checkpoint("MS-NRPC_R10189");
                this.Manager.Checkpoint("MS-NRPC_R1240");
                this.Manager.Comment("reaching state \'S986\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonWithFlags/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp447, "return of NetrLogonSamLogonWithFlags, state S986");
                this.Manager.Comment("reaching state \'S1139\'");
                goto label39;
            }
            throw new InvalidOperationException("never reached");
        label39:
;
            this.Manager.EndTest();
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS78GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S79");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS78GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S79");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS78GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S79");
        }
        #endregion
        
        #region Test Starting in S8
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS8() {
            this.Manager.BeginTest("Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS8");
            this.Manager.Comment("reaching state \'S8\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp449;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp449);
            this.Manager.AddReturn(GetPlatformInfo, null, temp449);
            this.Manager.Comment("reaching state \'S9\'");
            int temp459 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlags.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS8GetPlatformChecker)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlags.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS8GetPlatformChecker1)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlags.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS8GetPlatformChecker2)));
            if ((temp459 == 0)) {
                this.Manager.Comment("reaching state \'S114\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp450;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp450 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S267\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp450, "return of NetrServerReqChallenge, state S267");
                this.Manager.Comment("reaching state \'S420\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp451;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp451 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S573\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp451, "return of NetrServerAuthenticate3, state S573");
                this.Manager.Comment("reaching state \'S726\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp452;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonWithFlags(PrimaryDc,Client,True,NetlogonInt" +
                        "eractiveInformation,Valid,NetlogonValidationSamInfo2,0)\'");
                temp452 = this.INrpcServerAdapterInstance.NetrLogonSamLogonWithFlags(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo2, 0u);
                this.Manager.Checkpoint("MS-NRPC_R10189");
                this.Manager.Checkpoint("MS-NRPC_R1240");
                this.Manager.Comment("reaching state \'S879\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonWithFlags/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp452, "return of NetrLogonSamLogonWithFlags, state S879");
                this.Manager.Comment("reaching state \'S1032\'");
                goto label40;
            }
            if ((temp459 == 1)) {
                this.Manager.Comment("reaching state \'S115\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp453;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp453 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S268\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp453, "return of NetrServerReqChallenge, state S268");
                this.Manager.Comment("reaching state \'S421\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp454;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp454 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S574\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp454, "return of NetrServerAuthenticate3, state S574");
                this.Manager.Comment("reaching state \'S727\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp455;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonWithFlags(PrimaryDc,Client,True,NetlogonInt" +
                        "eractiveInformation,Valid,NetlogonValidationSamInfo2,0)\'");
                temp455 = this.INrpcServerAdapterInstance.NetrLogonSamLogonWithFlags(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo2, 0u);
                this.Manager.Checkpoint("MS-NRPC_R10189");
                this.Manager.Checkpoint("MS-NRPC_R1240");
                this.Manager.Comment("reaching state \'S880\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonWithFlags/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp455, "return of NetrLogonSamLogonWithFlags, state S880");
                this.Manager.Comment("reaching state \'S1033\'");
                goto label40;
            }
            if ((temp459 == 2)) {
                this.Manager.Comment("reaching state \'S116\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp456;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp456 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S269\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp456, "return of NetrServerReqChallenge, state S269");
                this.Manager.Comment("reaching state \'S422\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp457;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp457 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S575\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp457, "return of NetrServerAuthenticate3, state S575");
                this.Manager.Comment("reaching state \'S728\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp458;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonWithFlags(PrimaryDc,Client,True,NetlogonInt" +
                        "eractiveInformation,Valid,NetlogonValidationSamInfo,0)\'");
                temp458 = this.INrpcServerAdapterInstance.NetrLogonSamLogonWithFlags(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo, 0u);
                this.Manager.Checkpoint("MS-NRPC_R10189");
                this.Manager.Checkpoint("MS-NRPC_R1240");
                this.Manager.Comment("reaching state \'S881\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonWithFlags/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp458, "return of NetrLogonSamLogonWithFlags, state S881");
                this.Manager.Comment("reaching state \'S1034\'");
                goto label40;
            }
            throw new InvalidOperationException("never reached");
        label40:
;
            this.Manager.EndTest();
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS8GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S9");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS8GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S9");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS8GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S9");
        }
        #endregion
        
        #region Test Starting in S80
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS80() {
            this.Manager.BeginTest("Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS80");
            this.Manager.Comment("reaching state \'S80\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp460;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp460);
            this.Manager.AddReturn(GetPlatformInfo, null, temp460);
            this.Manager.Comment("reaching state \'S81\'");
            int temp470 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlags.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS80GetPlatformChecker)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlags.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS80GetPlatformChecker1)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlags.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS80GetPlatformChecker2)));
            if ((temp470 == 0)) {
                this.Manager.Comment("reaching state \'S222\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp461;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp461 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S375\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp461, "return of NetrServerReqChallenge, state S375");
                this.Manager.Comment("reaching state \'S528\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp462;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp462 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S681\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp462, "return of NetrServerAuthenticate3, state S681");
                this.Manager.Comment("reaching state \'S834\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp463;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonWithFlags(PrimaryDc,Client,True,NetlogonInt" +
                        "eractiveTransitiveInformation,Valid,NetlogonValidationSamInfo4,0)\'");
                temp463 = this.INrpcServerAdapterInstance.NetrLogonSamLogonWithFlags(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonInteractiveTransitiveInformation, ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo4, 0u);
                this.Manager.Checkpoint("MS-NRPC_R10190");
                this.Manager.Checkpoint("MS-NRPC_R1240");
                this.Manager.Comment("reaching state \'S987\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonWithFlags/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp463, "return of NetrLogonSamLogonWithFlags, state S987");
                this.Manager.Comment("reaching state \'S1140\'");
                goto label41;
            }
            if ((temp470 == 1)) {
                this.Manager.Comment("reaching state \'S223\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp464;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp464 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S376\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp464, "return of NetrServerReqChallenge, state S376");
                this.Manager.Comment("reaching state \'S529\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp465;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp465 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S682\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp465, "return of NetrServerAuthenticate3, state S682");
                this.Manager.Comment("reaching state \'S835\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp466;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonWithFlags(PrimaryDc,Client,True,NetlogonInt" +
                        "eractiveInformation,Valid,NetlogonValidationSamInfo,0)\'");
                temp466 = this.INrpcServerAdapterInstance.NetrLogonSamLogonWithFlags(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo, 0u);
                this.Manager.Checkpoint("MS-NRPC_R10189");
                this.Manager.Checkpoint("MS-NRPC_R1240");
                this.Manager.Comment("reaching state \'S988\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonWithFlags/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp466, "return of NetrLogonSamLogonWithFlags, state S988");
                this.Manager.Comment("reaching state \'S1141\'");
                goto label41;
            }
            if ((temp470 == 2)) {
                this.Manager.Comment("reaching state \'S224\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp467;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp467 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S377\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp467, "return of NetrServerReqChallenge, state S377");
                this.Manager.Comment("reaching state \'S530\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp468;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp468 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S683\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp468, "return of NetrServerAuthenticate3, state S683");
                this.Manager.Comment("reaching state \'S836\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp469;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonWithFlags(PrimaryDc,Client,True,NetlogonInt" +
                        "eractiveInformation,Valid,NetlogonValidationSamInfo,0)\'");
                temp469 = this.INrpcServerAdapterInstance.NetrLogonSamLogonWithFlags(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo, 0u);
                this.Manager.Checkpoint("MS-NRPC_R10189");
                this.Manager.Checkpoint("MS-NRPC_R1240");
                this.Manager.Comment("reaching state \'S989\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonWithFlags/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp469, "return of NetrLogonSamLogonWithFlags, state S989");
                this.Manager.Comment("reaching state \'S1142\'");
                goto label41;
            }
            throw new InvalidOperationException("never reached");
        label41:
;
            this.Manager.EndTest();
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS80GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S81");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS80GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S81");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS80GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S81");
        }
        #endregion
        
        #region Test Starting in S82
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS82() {
            this.Manager.BeginTest("Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS82");
            this.Manager.Comment("reaching state \'S82\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp471;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp471);
            this.Manager.AddReturn(GetPlatformInfo, null, temp471);
            this.Manager.Comment("reaching state \'S83\'");
            int temp481 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlags.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS82GetPlatformChecker)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlags.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS82GetPlatformChecker1)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlags.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS82GetPlatformChecker2)));
            if ((temp481 == 0)) {
                this.Manager.Comment("reaching state \'S225\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp472;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp472 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S378\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp472, "return of NetrServerReqChallenge, state S378");
                this.Manager.Comment("reaching state \'S531\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp473;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp473 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S684\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp473, "return of NetrServerAuthenticate3, state S684");
                this.Manager.Comment("reaching state \'S837\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp474;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonWithFlags(PrimaryDc,Client,True,NetlogonInt" +
                        "eractiveTransitiveInformation,Valid,NetlogonValidationSamInfo2,0)\'");
                temp474 = this.INrpcServerAdapterInstance.NetrLogonSamLogonWithFlags(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonInteractiveTransitiveInformation, ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo2, 0u);
                this.Manager.Checkpoint("MS-NRPC_R10190");
                this.Manager.Checkpoint("MS-NRPC_R1240");
                this.Manager.Comment("reaching state \'S990\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonWithFlags/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp474, "return of NetrLogonSamLogonWithFlags, state S990");
                this.Manager.Comment("reaching state \'S1143\'");
                goto label42;
            }
            if ((temp481 == 1)) {
                this.Manager.Comment("reaching state \'S226\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp475;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp475 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S379\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp475, "return of NetrServerReqChallenge, state S379");
                this.Manager.Comment("reaching state \'S532\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp476;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp476 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S685\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp476, "return of NetrServerAuthenticate3, state S685");
                this.Manager.Comment("reaching state \'S838\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp477;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonWithFlags(PrimaryDc,Client,True,NetlogonInt" +
                        "eractiveInformation,Valid,NetlogonValidationSamInfo,0)\'");
                temp477 = this.INrpcServerAdapterInstance.NetrLogonSamLogonWithFlags(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo, 0u);
                this.Manager.Checkpoint("MS-NRPC_R10189");
                this.Manager.Checkpoint("MS-NRPC_R1240");
                this.Manager.Comment("reaching state \'S991\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonWithFlags/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp477, "return of NetrLogonSamLogonWithFlags, state S991");
                this.Manager.Comment("reaching state \'S1144\'");
                goto label42;
            }
            if ((temp481 == 2)) {
                this.Manager.Comment("reaching state \'S227\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp478;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp478 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S380\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp478, "return of NetrServerReqChallenge, state S380");
                this.Manager.Comment("reaching state \'S533\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp479;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp479 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S686\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp479, "return of NetrServerAuthenticate3, state S686");
                this.Manager.Comment("reaching state \'S839\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp480;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonWithFlags(PrimaryDc,Client,True,NetlogonInt" +
                        "eractiveInformation,Valid,NetlogonValidationSamInfo,0)\'");
                temp480 = this.INrpcServerAdapterInstance.NetrLogonSamLogonWithFlags(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo, 0u);
                this.Manager.Checkpoint("MS-NRPC_R10189");
                this.Manager.Checkpoint("MS-NRPC_R1240");
                this.Manager.Comment("reaching state \'S992\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonWithFlags/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp480, "return of NetrLogonSamLogonWithFlags, state S992");
                this.Manager.Comment("reaching state \'S1145\'");
                goto label42;
            }
            throw new InvalidOperationException("never reached");
        label42:
;
            this.Manager.EndTest();
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS82GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S83");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS82GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S83");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS82GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S83");
        }
        #endregion
        
        #region Test Starting in S84
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS84() {
            this.Manager.BeginTest("Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS84");
            this.Manager.Comment("reaching state \'S84\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp482;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp482);
            this.Manager.AddReturn(GetPlatformInfo, null, temp482);
            this.Manager.Comment("reaching state \'S85\'");
            int temp492 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlags.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS84GetPlatformChecker)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlags.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS84GetPlatformChecker1)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlags.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS84GetPlatformChecker2)));
            if ((temp492 == 0)) {
                this.Manager.Comment("reaching state \'S228\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp483;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp483 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S381\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp483, "return of NetrServerReqChallenge, state S381");
                this.Manager.Comment("reaching state \'S534\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp484;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp484 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S687\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp484, "return of NetrServerAuthenticate3, state S687");
                this.Manager.Comment("reaching state \'S840\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp485;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonWithFlags(NonDcServer,Client,True,NetlogonN" +
                        "etworkTransitiveInformation,Valid,NetlogonValidationSamInfo2,0)\'");
                temp485 = this.INrpcServerAdapterInstance.NetrLogonSamLogonWithFlags(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonNetworkTransitiveInformation, ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo2, 0u);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R1241");
                this.Manager.Comment("reaching state \'S993\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonWithFlags/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp485, "return of NetrLogonSamLogonWithFlags, state S993");
                this.Manager.Comment("reaching state \'S1146\'");
                goto label43;
            }
            if ((temp492 == 1)) {
                this.Manager.Comment("reaching state \'S229\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp486;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp486 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S382\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp486, "return of NetrServerReqChallenge, state S382");
                this.Manager.Comment("reaching state \'S535\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp487;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp487 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S688\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp487, "return of NetrServerAuthenticate3, state S688");
                this.Manager.Comment("reaching state \'S841\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp488;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonWithFlags(PrimaryDc,Client,True,NetlogonInt" +
                        "eractiveInformation,Valid,NetlogonValidationSamInfo,0)\'");
                temp488 = this.INrpcServerAdapterInstance.NetrLogonSamLogonWithFlags(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo, 0u);
                this.Manager.Checkpoint("MS-NRPC_R10189");
                this.Manager.Checkpoint("MS-NRPC_R1240");
                this.Manager.Comment("reaching state \'S994\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonWithFlags/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp488, "return of NetrLogonSamLogonWithFlags, state S994");
                this.Manager.Comment("reaching state \'S1147\'");
                goto label43;
            }
            if ((temp492 == 2)) {
                this.Manager.Comment("reaching state \'S230\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp489;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp489 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S383\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp489, "return of NetrServerReqChallenge, state S383");
                this.Manager.Comment("reaching state \'S536\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp490;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp490 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S689\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp490, "return of NetrServerAuthenticate3, state S689");
                this.Manager.Comment("reaching state \'S842\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp491;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonWithFlags(PrimaryDc,Client,True,NetlogonInt" +
                        "eractiveInformation,Valid,NetlogonValidationSamInfo,0)\'");
                temp491 = this.INrpcServerAdapterInstance.NetrLogonSamLogonWithFlags(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo, 0u);
                this.Manager.Checkpoint("MS-NRPC_R10189");
                this.Manager.Checkpoint("MS-NRPC_R1240");
                this.Manager.Comment("reaching state \'S995\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonWithFlags/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp491, "return of NetrLogonSamLogonWithFlags, state S995");
                this.Manager.Comment("reaching state \'S1148\'");
                goto label43;
            }
            throw new InvalidOperationException("never reached");
        label43:
;
            this.Manager.EndTest();
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS84GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S85");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS84GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S85");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS84GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S85");
        }
        #endregion
        
        #region Test Starting in S86
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS86() {
            this.Manager.BeginTest("Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS86");
            this.Manager.Comment("reaching state \'S86\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp493;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp493);
            this.Manager.AddReturn(GetPlatformInfo, null, temp493);
            this.Manager.Comment("reaching state \'S87\'");
            int temp503 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlags.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS86GetPlatformChecker)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlags.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS86GetPlatformChecker1)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlags.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS86GetPlatformChecker2)));
            if ((temp503 == 0)) {
                this.Manager.Comment("reaching state \'S231\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp494;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp494 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S384\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp494, "return of NetrServerReqChallenge, state S384");
                this.Manager.Comment("reaching state \'S537\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp495;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp495 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S690\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp495, "return of NetrServerAuthenticate3, state S690");
                this.Manager.Comment("reaching state \'S843\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp496;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonWithFlags(PrimaryDc,Client,True,NetlogonSer" +
                        "viceTransitiveInformation,Valid,NetlogonValidationSamInfo4,0)\'");
                temp496 = this.INrpcServerAdapterInstance.NetrLogonSamLogonWithFlags(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonServiceTransitiveInformation, ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo4, 0u);
                this.Manager.Checkpoint("MS-NRPC_R10194");
                this.Manager.Checkpoint("MS-NRPC_R1240");
                this.Manager.Comment("reaching state \'S996\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonWithFlags/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp496, "return of NetrLogonSamLogonWithFlags, state S996");
                this.Manager.Comment("reaching state \'S1149\'");
                goto label44;
            }
            if ((temp503 == 1)) {
                this.Manager.Comment("reaching state \'S232\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp497;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp497 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S385\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp497, "return of NetrServerReqChallenge, state S385");
                this.Manager.Comment("reaching state \'S538\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp498;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp498 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S691\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp498, "return of NetrServerAuthenticate3, state S691");
                this.Manager.Comment("reaching state \'S844\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp499;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonWithFlags(PrimaryDc,Client,True,NetlogonInt" +
                        "eractiveInformation,Valid,NetlogonValidationSamInfo,0)\'");
                temp499 = this.INrpcServerAdapterInstance.NetrLogonSamLogonWithFlags(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo, 0u);
                this.Manager.Checkpoint("MS-NRPC_R10189");
                this.Manager.Checkpoint("MS-NRPC_R1240");
                this.Manager.Comment("reaching state \'S997\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonWithFlags/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp499, "return of NetrLogonSamLogonWithFlags, state S997");
                this.Manager.Comment("reaching state \'S1150\'");
                goto label44;
            }
            if ((temp503 == 2)) {
                this.Manager.Comment("reaching state \'S233\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp500;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp500 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S386\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp500, "return of NetrServerReqChallenge, state S386");
                this.Manager.Comment("reaching state \'S539\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp501;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp501 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S692\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp501, "return of NetrServerAuthenticate3, state S692");
                this.Manager.Comment("reaching state \'S845\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp502;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonWithFlags(PrimaryDc,Client,True,NetlogonInt" +
                        "eractiveInformation,Valid,NetlogonValidationSamInfo,0)\'");
                temp502 = this.INrpcServerAdapterInstance.NetrLogonSamLogonWithFlags(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo, 0u);
                this.Manager.Checkpoint("MS-NRPC_R10189");
                this.Manager.Checkpoint("MS-NRPC_R1240");
                this.Manager.Comment("reaching state \'S998\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonWithFlags/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp502, "return of NetrLogonSamLogonWithFlags, state S998");
                this.Manager.Comment("reaching state \'S1151\'");
                goto label44;
            }
            throw new InvalidOperationException("never reached");
        label44:
;
            this.Manager.EndTest();
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS86GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S87");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS86GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S87");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS86GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S87");
        }
        #endregion
        
        #region Test Starting in S88
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS88() {
            this.Manager.BeginTest("Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS88");
            this.Manager.Comment("reaching state \'S88\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp504;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp504);
            this.Manager.AddReturn(GetPlatformInfo, null, temp504);
            this.Manager.Comment("reaching state \'S89\'");
            int temp514 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlags.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS88GetPlatformChecker)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlags.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS88GetPlatformChecker1)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlags.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS88GetPlatformChecker2)));
            if ((temp514 == 0)) {
                this.Manager.Comment("reaching state \'S234\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp505;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp505 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S387\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp505, "return of NetrServerReqChallenge, state S387");
                this.Manager.Comment("reaching state \'S540\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp506;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp506 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S693\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp506, "return of NetrServerAuthenticate3, state S693");
                this.Manager.Comment("reaching state \'S846\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp507;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonWithFlags(PrimaryDc,Client,True,NetlogonSer" +
                        "viceTransitiveInformation,Valid,NetlogonValidationSamInfo2,0)\'");
                temp507 = this.INrpcServerAdapterInstance.NetrLogonSamLogonWithFlags(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonServiceTransitiveInformation, ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo2, 0u);
                this.Manager.Checkpoint("MS-NRPC_R10194");
                this.Manager.Checkpoint("MS-NRPC_R1240");
                this.Manager.Comment("reaching state \'S999\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonWithFlags/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp507, "return of NetrLogonSamLogonWithFlags, state S999");
                this.Manager.Comment("reaching state \'S1152\'");
                goto label45;
            }
            if ((temp514 == 1)) {
                this.Manager.Comment("reaching state \'S235\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp508;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp508 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S388\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp508, "return of NetrServerReqChallenge, state S388");
                this.Manager.Comment("reaching state \'S541\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp509;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp509 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S694\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp509, "return of NetrServerAuthenticate3, state S694");
                this.Manager.Comment("reaching state \'S847\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp510;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonWithFlags(PrimaryDc,Client,True,NetlogonInt" +
                        "eractiveInformation,Valid,NetlogonValidationSamInfo,0)\'");
                temp510 = this.INrpcServerAdapterInstance.NetrLogonSamLogonWithFlags(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo, 0u);
                this.Manager.Checkpoint("MS-NRPC_R10189");
                this.Manager.Checkpoint("MS-NRPC_R1240");
                this.Manager.Comment("reaching state \'S1000\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonWithFlags/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp510, "return of NetrLogonSamLogonWithFlags, state S1000");
                this.Manager.Comment("reaching state \'S1153\'");
                goto label45;
            }
            if ((temp514 == 2)) {
                this.Manager.Comment("reaching state \'S236\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp511;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp511 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S389\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp511, "return of NetrServerReqChallenge, state S389");
                this.Manager.Comment("reaching state \'S542\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp512;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp512 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S695\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp512, "return of NetrServerAuthenticate3, state S695");
                this.Manager.Comment("reaching state \'S848\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp513;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonWithFlags(PrimaryDc,Client,True,NetlogonInt" +
                        "eractiveInformation,Valid,NetlogonValidationSamInfo,0)\'");
                temp513 = this.INrpcServerAdapterInstance.NetrLogonSamLogonWithFlags(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo, 0u);
                this.Manager.Checkpoint("MS-NRPC_R10189");
                this.Manager.Checkpoint("MS-NRPC_R1240");
                this.Manager.Comment("reaching state \'S1001\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonWithFlags/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp513, "return of NetrLogonSamLogonWithFlags, state S1001");
                this.Manager.Comment("reaching state \'S1154\'");
                goto label45;
            }
            throw new InvalidOperationException("never reached");
        label45:
;
            this.Manager.EndTest();
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS88GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S89");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS88GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S89");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS88GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S89");
        }
        #endregion
        
        #region Test Starting in S90
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS90() {
            this.Manager.BeginTest("Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS90");
            this.Manager.Comment("reaching state \'S90\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp515;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp515);
            this.Manager.AddReturn(GetPlatformInfo, null, temp515);
            this.Manager.Comment("reaching state \'S91\'");
            int temp525 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlags.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS90GetPlatformChecker)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlags.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS90GetPlatformChecker1)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlags.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS90GetPlatformChecker2)));
            if ((temp525 == 0)) {
                this.Manager.Comment("reaching state \'S237\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp516;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp516 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S390\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp516, "return of NetrServerReqChallenge, state S390");
                this.Manager.Comment("reaching state \'S543\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp517;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp517 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S696\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp517, "return of NetrServerAuthenticate3, state S696");
                this.Manager.Comment("reaching state \'S849\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp518;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonWithFlags(PrimaryDc,Client,True,NetlogonNet" +
                        "workTransitiveInformation,Valid,NetlogonValidationSamInfo4,0)\'");
                temp518 = this.INrpcServerAdapterInstance.NetrLogonSamLogonWithFlags(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonNetworkTransitiveInformation, ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo4, 0u);
                this.Manager.Checkpoint("MS-NRPC_R1240");
                this.Manager.Comment("reaching state \'S1002\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonWithFlags/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp518, "return of NetrLogonSamLogonWithFlags, state S1002");
                this.Manager.Comment("reaching state \'S1155\'");
                goto label46;
            }
            if ((temp525 == 1)) {
                this.Manager.Comment("reaching state \'S238\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp519;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp519 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S391\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp519, "return of NetrServerReqChallenge, state S391");
                this.Manager.Comment("reaching state \'S544\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp520;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp520 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S697\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp520, "return of NetrServerAuthenticate3, state S697");
                this.Manager.Comment("reaching state \'S850\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp521;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonWithFlags(PrimaryDc,Client,True,NetlogonInt" +
                        "eractiveInformation,Valid,NetlogonValidationSamInfo,0)\'");
                temp521 = this.INrpcServerAdapterInstance.NetrLogonSamLogonWithFlags(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo, 0u);
                this.Manager.Checkpoint("MS-NRPC_R10189");
                this.Manager.Checkpoint("MS-NRPC_R1240");
                this.Manager.Comment("reaching state \'S1003\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonWithFlags/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp521, "return of NetrLogonSamLogonWithFlags, state S1003");
                this.Manager.Comment("reaching state \'S1156\'");
                goto label46;
            }
            if ((temp525 == 2)) {
                this.Manager.Comment("reaching state \'S239\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp522;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp522 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S392\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp522, "return of NetrServerReqChallenge, state S392");
                this.Manager.Comment("reaching state \'S545\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp523;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp523 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S698\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp523, "return of NetrServerAuthenticate3, state S698");
                this.Manager.Comment("reaching state \'S851\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp524;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonWithFlags(PrimaryDc,Client,True,NetlogonInt" +
                        "eractiveInformation,Valid,NetlogonValidationSamInfo,0)\'");
                temp524 = this.INrpcServerAdapterInstance.NetrLogonSamLogonWithFlags(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo, 0u);
                this.Manager.Checkpoint("MS-NRPC_R10189");
                this.Manager.Checkpoint("MS-NRPC_R1240");
                this.Manager.Comment("reaching state \'S1004\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonWithFlags/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp524, "return of NetrLogonSamLogonWithFlags, state S1004");
                this.Manager.Comment("reaching state \'S1157\'");
                goto label46;
            }
            throw new InvalidOperationException("never reached");
        label46:
;
            this.Manager.EndTest();
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS90GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S91");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS90GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S91");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS90GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S91");
        }
        #endregion
        
        #region Test Starting in S92
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS92() {
            this.Manager.BeginTest("Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS92");
            this.Manager.Comment("reaching state \'S92\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp526;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp526);
            this.Manager.AddReturn(GetPlatformInfo, null, temp526);
            this.Manager.Comment("reaching state \'S93\'");
            int temp536 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlags.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS92GetPlatformChecker)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlags.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS92GetPlatformChecker1)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlags.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS92GetPlatformChecker2)));
            if ((temp536 == 0)) {
                this.Manager.Comment("reaching state \'S240\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp527;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp527 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S393\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp527, "return of NetrServerReqChallenge, state S393");
                this.Manager.Comment("reaching state \'S546\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp528;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp528 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S699\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp528, "return of NetrServerAuthenticate3, state S699");
                this.Manager.Comment("reaching state \'S852\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp529;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonWithFlags(PrimaryDc,Client,True,NetlogonNet" +
                        "workTransitiveInformation,Valid,NetlogonValidationSamInfo2,2147483648)\'");
                temp529 = this.INrpcServerAdapterInstance.NetrLogonSamLogonWithFlags(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonNetworkTransitiveInformation, ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo2, 2147483648u);
                this.Manager.Checkpoint("MS-NRPC_R1240");
                this.Manager.Comment("reaching state \'S1005\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonWithFlags/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp529, "return of NetrLogonSamLogonWithFlags, state S1005");
                this.Manager.Comment("reaching state \'S1158\'");
                goto label47;
            }
            if ((temp536 == 1)) {
                this.Manager.Comment("reaching state \'S241\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp530;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp530 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S394\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp530, "return of NetrServerReqChallenge, state S394");
                this.Manager.Comment("reaching state \'S547\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp531;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp531 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S700\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp531, "return of NetrServerAuthenticate3, state S700");
                this.Manager.Comment("reaching state \'S853\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp532;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonWithFlags(PrimaryDc,Client,True,NetlogonInt" +
                        "eractiveInformation,Valid,NetlogonValidationSamInfo,0)\'");
                temp532 = this.INrpcServerAdapterInstance.NetrLogonSamLogonWithFlags(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo, 0u);
                this.Manager.Checkpoint("MS-NRPC_R10189");
                this.Manager.Checkpoint("MS-NRPC_R1240");
                this.Manager.Comment("reaching state \'S1006\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonWithFlags/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp532, "return of NetrLogonSamLogonWithFlags, state S1006");
                this.Manager.Comment("reaching state \'S1159\'");
                goto label47;
            }
            if ((temp536 == 2)) {
                this.Manager.Comment("reaching state \'S242\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp533;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp533 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S395\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp533, "return of NetrServerReqChallenge, state S395");
                this.Manager.Comment("reaching state \'S548\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp534;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp534 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S701\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp534, "return of NetrServerAuthenticate3, state S701");
                this.Manager.Comment("reaching state \'S854\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp535;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonWithFlags(PrimaryDc,Client,True,NetlogonInt" +
                        "eractiveInformation,Valid,NetlogonValidationSamInfo,0)\'");
                temp535 = this.INrpcServerAdapterInstance.NetrLogonSamLogonWithFlags(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo, 0u);
                this.Manager.Checkpoint("MS-NRPC_R10189");
                this.Manager.Checkpoint("MS-NRPC_R1240");
                this.Manager.Comment("reaching state \'S1007\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonWithFlags/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp535, "return of NetrLogonSamLogonWithFlags, state S1007");
                this.Manager.Comment("reaching state \'S1160\'");
                goto label47;
            }
            throw new InvalidOperationException("never reached");
        label47:
;
            this.Manager.EndTest();
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS92GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S93");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS92GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S93");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS92GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S93");
        }
        #endregion
        
        #region Test Starting in S94
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS94() {
            this.Manager.BeginTest("Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS94");
            this.Manager.Comment("reaching state \'S94\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp537;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp537);
            this.Manager.AddReturn(GetPlatformInfo, null, temp537);
            this.Manager.Comment("reaching state \'S95\'");
            int temp547 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlags.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS94GetPlatformChecker)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlags.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS94GetPlatformChecker1)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlags.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS94GetPlatformChecker2)));
            if ((temp547 == 0)) {
                this.Manager.Comment("reaching state \'S243\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp538;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp538 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S396\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp538, "return of NetrServerReqChallenge, state S396");
                this.Manager.Comment("reaching state \'S549\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp539;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp539 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S702\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp539, "return of NetrServerAuthenticate3, state S702");
                this.Manager.Comment("reaching state \'S855\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp540;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonWithFlags(PrimaryDc,Client,True,NetlogonNet" +
                        "workTransitiveInformation,Valid,NetlogonValidationSamInfo2,4)\'");
                temp540 = this.INrpcServerAdapterInstance.NetrLogonSamLogonWithFlags(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonNetworkTransitiveInformation, ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo2, 4u);
                this.Manager.Checkpoint("MS-NRPC_R1240");
                this.Manager.Comment("reaching state \'S1008\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonWithFlags/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp540, "return of NetrLogonSamLogonWithFlags, state S1008");
                this.Manager.Comment("reaching state \'S1161\'");
                goto label48;
            }
            if ((temp547 == 1)) {
                this.Manager.Comment("reaching state \'S244\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp541;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp541 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S397\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp541, "return of NetrServerReqChallenge, state S397");
                this.Manager.Comment("reaching state \'S550\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp542;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp542 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S703\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp542, "return of NetrServerAuthenticate3, state S703");
                this.Manager.Comment("reaching state \'S856\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp543;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonWithFlags(PrimaryDc,Client,True,NetlogonInt" +
                        "eractiveInformation,Valid,NetlogonValidationSamInfo,0)\'");
                temp543 = this.INrpcServerAdapterInstance.NetrLogonSamLogonWithFlags(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo, 0u);
                this.Manager.Checkpoint("MS-NRPC_R10189");
                this.Manager.Checkpoint("MS-NRPC_R1240");
                this.Manager.Comment("reaching state \'S1009\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonWithFlags/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp543, "return of NetrLogonSamLogonWithFlags, state S1009");
                this.Manager.Comment("reaching state \'S1162\'");
                goto label48;
            }
            if ((temp547 == 2)) {
                this.Manager.Comment("reaching state \'S245\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp544;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp544 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S398\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp544, "return of NetrServerReqChallenge, state S398");
                this.Manager.Comment("reaching state \'S551\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp545;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp545 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S704\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp545, "return of NetrServerAuthenticate3, state S704");
                this.Manager.Comment("reaching state \'S857\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp546;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonWithFlags(PrimaryDc,Client,True,NetlogonInt" +
                        "eractiveInformation,Valid,NetlogonValidationSamInfo,0)\'");
                temp546 = this.INrpcServerAdapterInstance.NetrLogonSamLogonWithFlags(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo, 0u);
                this.Manager.Checkpoint("MS-NRPC_R10189");
                this.Manager.Checkpoint("MS-NRPC_R1240");
                this.Manager.Comment("reaching state \'S1010\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonWithFlags/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp546, "return of NetrLogonSamLogonWithFlags, state S1010");
                this.Manager.Comment("reaching state \'S1163\'");
                goto label48;
            }
            throw new InvalidOperationException("never reached");
        label48:
;
            this.Manager.EndTest();
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS94GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S95");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS94GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S95");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS94GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S95");
        }
        #endregion
        
        #region Test Starting in S96
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS96() {
            this.Manager.BeginTest("Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS96");
            this.Manager.Comment("reaching state \'S96\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp548;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp548);
            this.Manager.AddReturn(GetPlatformInfo, null, temp548);
            this.Manager.Comment("reaching state \'S97\'");
            int temp558 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlags.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS96GetPlatformChecker)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlags.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS96GetPlatformChecker1)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlags.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS96GetPlatformChecker2)));
            if ((temp558 == 0)) {
                this.Manager.Comment("reaching state \'S246\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp549;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp549 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S399\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp549, "return of NetrServerReqChallenge, state S399");
                this.Manager.Comment("reaching state \'S552\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp550;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp550 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S705\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp550, "return of NetrServerAuthenticate3, state S705");
                this.Manager.Comment("reaching state \'S858\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp551;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonWithFlags(PrimaryDc,Client,True,NetlogonNet" +
                        "workTransitiveInformation,Valid,NetlogonValidationSamInfo2,8)\'");
                temp551 = this.INrpcServerAdapterInstance.NetrLogonSamLogonWithFlags(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonNetworkTransitiveInformation, ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo2, 8u);
                this.Manager.Checkpoint("MS-NRPC_R1240");
                this.Manager.Comment("reaching state \'S1011\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonWithFlags/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp551, "return of NetrLogonSamLogonWithFlags, state S1011");
                this.Manager.Comment("reaching state \'S1164\'");
                goto label49;
            }
            if ((temp558 == 1)) {
                this.Manager.Comment("reaching state \'S247\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp552;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp552 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S400\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp552, "return of NetrServerReqChallenge, state S400");
                this.Manager.Comment("reaching state \'S553\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp553;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp553 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S706\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp553, "return of NetrServerAuthenticate3, state S706");
                this.Manager.Comment("reaching state \'S859\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp554;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonWithFlags(PrimaryDc,Client,True,NetlogonInt" +
                        "eractiveInformation,Valid,NetlogonValidationSamInfo,0)\'");
                temp554 = this.INrpcServerAdapterInstance.NetrLogonSamLogonWithFlags(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo, 0u);
                this.Manager.Checkpoint("MS-NRPC_R10189");
                this.Manager.Checkpoint("MS-NRPC_R1240");
                this.Manager.Comment("reaching state \'S1012\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonWithFlags/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp554, "return of NetrLogonSamLogonWithFlags, state S1012");
                this.Manager.Comment("reaching state \'S1165\'");
                goto label49;
            }
            if ((temp558 == 2)) {
                this.Manager.Comment("reaching state \'S248\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp555;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp555 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S401\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp555, "return of NetrServerReqChallenge, state S401");
                this.Manager.Comment("reaching state \'S554\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp556;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp556 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S707\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp556, "return of NetrServerAuthenticate3, state S707");
                this.Manager.Comment("reaching state \'S860\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp557;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonWithFlags(PrimaryDc,Client,True,NetlogonInt" +
                        "eractiveInformation,Valid,NetlogonValidationSamInfo,0)\'");
                temp557 = this.INrpcServerAdapterInstance.NetrLogonSamLogonWithFlags(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo, 0u);
                this.Manager.Checkpoint("MS-NRPC_R10189");
                this.Manager.Checkpoint("MS-NRPC_R1240");
                this.Manager.Comment("reaching state \'S1013\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonWithFlags/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp557, "return of NetrLogonSamLogonWithFlags, state S1013");
                this.Manager.Comment("reaching state \'S1166\'");
                goto label49;
            }
            throw new InvalidOperationException("never reached");
        label49:
;
            this.Manager.EndTest();
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS96GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S97");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS96GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S97");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS96GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S97");
        }
        #endregion
        
        #region Test Starting in S98
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS98() {
            this.Manager.BeginTest("Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS98");
            this.Manager.Comment("reaching state \'S98\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp559;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp559);
            this.Manager.AddReturn(GetPlatformInfo, null, temp559);
            this.Manager.Comment("reaching state \'S99\'");
            int temp569 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlags.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS98GetPlatformChecker)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlags.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS98GetPlatformChecker1)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlags.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS98GetPlatformChecker2)));
            if ((temp569 == 0)) {
                this.Manager.Comment("reaching state \'S249\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp560;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp560 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S402\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp560, "return of NetrServerReqChallenge, state S402");
                this.Manager.Comment("reaching state \'S555\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp561;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp561 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S708\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp561, "return of NetrServerAuthenticate3, state S708");
                this.Manager.Comment("reaching state \'S861\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp562;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonWithFlags(PrimaryDc,Client,True,NetlogonNet" +
                        "workTransitiveInformation,Valid,NetlogonValidationSamInfo2,1)\'");
                temp562 = this.INrpcServerAdapterInstance.NetrLogonSamLogonWithFlags(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonNetworkTransitiveInformation, ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo2, 1u);
                this.Manager.Checkpoint("MS-NRPC_R1241");
                this.Manager.Comment("reaching state \'S1014\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonWithFlags/STATUS_NO_SUCH_USER\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NO_SUCH_USER, temp562, "return of NetrLogonSamLogonWithFlags, state S1014");
                this.Manager.Comment("reaching state \'S1167\'");
                goto label50;
            }
            if ((temp569 == 1)) {
                this.Manager.Comment("reaching state \'S250\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp563;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp563 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S403\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp563, "return of NetrServerReqChallenge, state S403");
                this.Manager.Comment("reaching state \'S556\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp564;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp564 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S709\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp564, "return of NetrServerAuthenticate3, state S709");
                this.Manager.Comment("reaching state \'S862\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp565;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonWithFlags(PrimaryDc,Client,True,NetlogonInt" +
                        "eractiveInformation,Valid,NetlogonValidationSamInfo,0)\'");
                temp565 = this.INrpcServerAdapterInstance.NetrLogonSamLogonWithFlags(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo, 0u);
                this.Manager.Checkpoint("MS-NRPC_R10189");
                this.Manager.Checkpoint("MS-NRPC_R1240");
                this.Manager.Comment("reaching state \'S1015\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonWithFlags/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp565, "return of NetrLogonSamLogonWithFlags, state S1015");
                this.Manager.Comment("reaching state \'S1168\'");
                goto label50;
            }
            if ((temp569 == 2)) {
                this.Manager.Comment("reaching state \'S251\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp566;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp566 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S404\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp566, "return of NetrServerReqChallenge, state S404");
                this.Manager.Comment("reaching state \'S557\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp567;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp567 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S710\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp567, "return of NetrServerAuthenticate3, state S710");
                this.Manager.Comment("reaching state \'S863\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp568;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonWithFlags(PrimaryDc,Client,True,NetlogonInt" +
                        "eractiveInformation,Valid,NetlogonValidationSamInfo,0)\'");
                temp568 = this.INrpcServerAdapterInstance.NetrLogonSamLogonWithFlags(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo, 0u);
                this.Manager.Checkpoint("MS-NRPC_R10189");
                this.Manager.Checkpoint("MS-NRPC_R1240");
                this.Manager.Comment("reaching state \'S1016\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonWithFlags/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp568, "return of NetrLogonSamLogonWithFlags, state S1016");
                this.Manager.Comment("reaching state \'S1169\'");
                goto label50;
            }
            throw new InvalidOperationException("never reached");
        label50:
;
            this.Manager.EndTest();
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS98GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S99");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS98GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S99");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonWithFlagsS98GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S99");
        }
        #endregion
    }
}
