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
    public partial class Test_PassThroughAuthenticate_NetrLogonSamLogonEx : PtfTestClassBase {
        
        public Test_PassThroughAuthenticate_NetrLogonSamLogonEx() {
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
        public void Test_PassThroughAuthenticate_NetrLogonSamLogonExS0() {
            this.Manager.BeginTest("Test_PassThroughAuthenticate_NetrLogonSamLogonExS0");
            this.Manager.Comment("reaching state \'S0\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp0;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp0);
            this.Manager.AddReturn(GetPlatformInfo, null, temp0);
            this.Manager.Comment("reaching state \'S1\'");
            int temp13 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonEx.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonExS0GetPlatformChecker)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonEx.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonExS0GetPlatformChecker1)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonEx.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonExS0GetPlatformChecker2)));
            if ((temp13 == 0)) {
                this.Manager.Comment("reaching state \'S126\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp1;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp1 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S315\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp1, "return of NetrServerReqChallenge, state S315");
                this.Manager.Comment("reaching state \'S504\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp2;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp2 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S693\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp2, "return of NetrServerAuthenticate3, state S693");
                this.Manager.Comment("reaching state \'S882\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp3;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonEx(True,PrimaryDc,Client,NetlogonNetworkTra" +
                        "nsitiveInformation,Valid,NetlogonValidationSamInfo2,2147483648)\'");
                temp3 = this.INrpcServerAdapterInstance.NetrLogonSamLogonEx(true, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonNetworkTransitiveInformation, ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo2, 2147483648u);
                this.Manager.Checkpoint("MS-NRPC_R1160");
                this.Manager.Comment("reaching state \'S1071\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonEx/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp3, "return of NetrLogonSamLogonEx, state S1071");
                this.Manager.Comment("reaching state \'S1260\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp4;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp4 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S1449\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp4, "return of NetrServerReqChallenge, state S1449");
                this.Manager.Comment("reaching state \'S1452\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp5;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp5 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S1455\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp5, "return of NetrServerAuthenticate3, state S1455");
                this.Manager.Comment("reaching state \'S1458\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp6;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonEx(True,PrimaryDc,Client,NetlogonNetworkTra" +
                        "nsitiveInformation,Valid,NetlogonValidationSamInfo2,0)\'");
                temp6 = this.INrpcServerAdapterInstance.NetrLogonSamLogonEx(true, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonNetworkTransitiveInformation, ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo2, 0u);
                this.Manager.Checkpoint("MS-NRPC_R1154");
                this.Manager.Checkpoint("MS-NRPC_R1160");
                this.Manager.Comment("reaching state \'S1461\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonEx/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp6, "return of NetrLogonSamLogonEx, state S1461");
                this.Manager.Comment("reaching state \'S1464\'");
                goto label0;
            }
            if ((temp13 == 1)) {
                this.Manager.Comment("reaching state \'S127\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp7;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp7 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S316\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp7, "return of NetrServerReqChallenge, state S316");
                this.Manager.Comment("reaching state \'S505\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp8;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp8 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S694\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp8, "return of NetrServerAuthenticate3, state S694");
                this.Manager.Comment("reaching state \'S883\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp9;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonEx(True,PrimaryDc,Client,NetlogonNetworkTra" +
                        "nsitiveInformation,Valid,NetlogonValidationSamInfo4,0)\'");
                temp9 = this.INrpcServerAdapterInstance.NetrLogonSamLogonEx(true, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonNetworkTransitiveInformation, ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo4, 0u);
                this.Manager.Checkpoint("MS-NRPC_R1160");
                this.Manager.Comment("reaching state \'S1072\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonEx/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp9, "return of NetrLogonSamLogonEx, state S1072");
                this.Manager.Comment("reaching state \'S1261\'");
                goto label0;
            }
            if ((temp13 == 2)) {
                this.Manager.Comment("reaching state \'S128\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp10;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp10 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S317\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp10, "return of NetrServerReqChallenge, state S317");
                this.Manager.Comment("reaching state \'S506\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp11;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp11 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S695\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp11, "return of NetrServerAuthenticate3, state S695");
                this.Manager.Comment("reaching state \'S884\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp12;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonEx(True,PrimaryDc,Client,NetlogonNetworkTra" +
                        "nsitiveInformation,Valid,NetlogonValidationSamInfo4,0)\'");
                temp12 = this.INrpcServerAdapterInstance.NetrLogonSamLogonEx(true, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonNetworkTransitiveInformation, ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo4, 0u);
                this.Manager.Checkpoint("MS-NRPC_R1160");
                this.Manager.Comment("reaching state \'S1073\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonEx/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp12, "return of NetrLogonSamLogonEx, state S1073");
                this.Manager.Comment("reaching state \'S1262\'");
                goto label0;
            }
            throw new InvalidOperationException("never reached");
        label0:
;
            this.Manager.EndTest();
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonExS0GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S1");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonExS0GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S1");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonExS0GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S1");
        }
        #endregion
        
        #region Test Starting in S10
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_PassThroughAuthenticate_NetrLogonSamLogonExS10() {
            this.Manager.BeginTest("Test_PassThroughAuthenticate_NetrLogonSamLogonExS10");
            this.Manager.Comment("reaching state \'S10\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp14;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp14);
            this.Manager.AddReturn(GetPlatformInfo, null, temp14);
            this.Manager.Comment("reaching state \'S11\'");
            int temp24 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonEx.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonExS10GetPlatformChecker)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonEx.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonExS10GetPlatformChecker1)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonEx.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonExS10GetPlatformChecker2)));
            if ((temp24 == 0)) {
                this.Manager.Comment("reaching state \'S141\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp15;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp15 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S330\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp15, "return of NetrServerReqChallenge, state S330");
                this.Manager.Comment("reaching state \'S519\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp16;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp16 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S708\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp16, "return of NetrServerAuthenticate3, state S708");
                this.Manager.Comment("reaching state \'S897\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp17;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonEx(True,PrimaryDc,Client,NetlogonNetworkInf" +
                        "ormation,Valid,NetlogonValidationSamInfo2,0)\'");
                temp17 = this.INrpcServerAdapterInstance.NetrLogonSamLogonEx(true, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonNetworkInformation, ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo2, 0u);
                this.Manager.Checkpoint("MS-NRPC_R1160");
                this.Manager.Comment("reaching state \'S1086\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonEx/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp17, "return of NetrLogonSamLogonEx, state S1086");
                this.Manager.Comment("reaching state \'S1275\'");
                goto label1;
            }
            if ((temp24 == 1)) {
                this.Manager.Comment("reaching state \'S142\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp18;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp18 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S331\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp18, "return of NetrServerReqChallenge, state S331");
                this.Manager.Comment("reaching state \'S520\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp19;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp19 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S709\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp19, "return of NetrServerAuthenticate3, state S709");
                this.Manager.Comment("reaching state \'S898\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp20;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonEx(True,PrimaryDc,Client,NetlogonNetworkInf" +
                        "ormation,Valid,NetlogonValidationSamInfo2,0)\'");
                temp20 = this.INrpcServerAdapterInstance.NetrLogonSamLogonEx(true, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonNetworkInformation, ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo2, 0u);
                this.Manager.Checkpoint("MS-NRPC_R1160");
                this.Manager.Comment("reaching state \'S1087\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonEx/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp20, "return of NetrLogonSamLogonEx, state S1087");
                this.Manager.Comment("reaching state \'S1276\'");
                goto label1;
            }
            if ((temp24 == 2)) {
                this.Manager.Comment("reaching state \'S143\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp21;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp21 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S332\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp21, "return of NetrServerReqChallenge, state S332");
                this.Manager.Comment("reaching state \'S521\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp22;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp22 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S710\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp22, "return of NetrServerAuthenticate3, state S710");
                this.Manager.Comment("reaching state \'S899\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp23;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonEx(True,PrimaryDc,Client,NetlogonInteractiv" +
                        "eInformation,Valid,NetlogonValidationSamInfo,0)\'");
                temp23 = this.INrpcServerAdapterInstance.NetrLogonSamLogonEx(true, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo, 0u);
                this.Manager.Checkpoint("MS-NRPC_R1174");
                this.Manager.Checkpoint("MS-NRPC_R1160");
                this.Manager.Comment("reaching state \'S1088\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonEx/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp23, "return of NetrLogonSamLogonEx, state S1088");
                this.Manager.Comment("reaching state \'S1277\'");
                goto label1;
            }
            throw new InvalidOperationException("never reached");
        label1:
;
            this.Manager.EndTest();
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonExS10GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S11");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonExS10GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S11");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonExS10GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S11");
        }
        #endregion
        
        #region Test Starting in S100
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_PassThroughAuthenticate_NetrLogonSamLogonExS100() {
            this.Manager.BeginTest("Test_PassThroughAuthenticate_NetrLogonSamLogonExS100");
            this.Manager.Comment("reaching state \'S100\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp25;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp25);
            this.Manager.AddReturn(GetPlatformInfo, null, temp25);
            this.Manager.Comment("reaching state \'S101\'");
            int temp35 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonEx.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonExS100GetPlatformChecker)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonEx.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonExS100GetPlatformChecker1)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonEx.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonExS100GetPlatformChecker2)));
            if ((temp35 == 0)) {
                this.Manager.Comment("reaching state \'S276\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp26;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp26 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S465\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp26, "return of NetrServerReqChallenge, state S465");
                this.Manager.Comment("reaching state \'S654\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp27;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp27 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S843\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp27, "return of NetrServerAuthenticate3, state S843");
                this.Manager.Comment("reaching state \'S1032\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp28;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonEx(True,PrimaryDc,Client,NetlogonServiceTra" +
                        "nsitiveInformation,Valid,NetlogonValidationSamInfo2,0)\'");
                temp28 = this.INrpcServerAdapterInstance.NetrLogonSamLogonEx(true, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonServiceTransitiveInformation, ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo2, 0u);
                this.Manager.Checkpoint("MS-NRPC_R101177");
                this.Manager.Checkpoint("MS-NRPC_R1160");
                this.Manager.Comment("reaching state \'S1221\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonEx/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp28, "return of NetrLogonSamLogonEx, state S1221");
                this.Manager.Comment("reaching state \'S1410\'");
                goto label2;
            }
            if ((temp35 == 1)) {
                this.Manager.Comment("reaching state \'S277\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp29;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp29 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S466\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp29, "return of NetrServerReqChallenge, state S466");
                this.Manager.Comment("reaching state \'S655\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp30;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp30 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S844\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp30, "return of NetrServerAuthenticate3, state S844");
                this.Manager.Comment("reaching state \'S1033\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp31;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonEx(True,PrimaryDc,Client,NetlogonInteractiv" +
                        "eInformation,Valid,NetlogonValidationSamInfo,0)\'");
                temp31 = this.INrpcServerAdapterInstance.NetrLogonSamLogonEx(true, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo, 0u);
                this.Manager.Checkpoint("MS-NRPC_R1174");
                this.Manager.Checkpoint("MS-NRPC_R1160");
                this.Manager.Comment("reaching state \'S1222\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonEx/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp31, "return of NetrLogonSamLogonEx, state S1222");
                this.Manager.Comment("reaching state \'S1411\'");
                goto label2;
            }
            if ((temp35 == 2)) {
                this.Manager.Comment("reaching state \'S278\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp32;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp32 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S467\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp32, "return of NetrServerReqChallenge, state S467");
                this.Manager.Comment("reaching state \'S656\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp33;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp33 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S845\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp33, "return of NetrServerAuthenticate3, state S845");
                this.Manager.Comment("reaching state \'S1034\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp34;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonEx(True,PrimaryDc,Client,NetlogonInteractiv" +
                        "eInformation,Valid,NetlogonValidationSamInfo,0)\'");
                temp34 = this.INrpcServerAdapterInstance.NetrLogonSamLogonEx(true, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo, 0u);
                this.Manager.Checkpoint("MS-NRPC_R1174");
                this.Manager.Checkpoint("MS-NRPC_R1160");
                this.Manager.Comment("reaching state \'S1223\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonEx/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp34, "return of NetrLogonSamLogonEx, state S1223");
                this.Manager.Comment("reaching state \'S1412\'");
                goto label2;
            }
            throw new InvalidOperationException("never reached");
        label2:
;
            this.Manager.EndTest();
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonExS100GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S101");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonExS100GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S101");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonExS100GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S101");
        }
        #endregion
        
        #region Test Starting in S102
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_PassThroughAuthenticate_NetrLogonSamLogonExS102() {
            this.Manager.BeginTest("Test_PassThroughAuthenticate_NetrLogonSamLogonExS102");
            this.Manager.Comment("reaching state \'S102\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp36;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp36);
            this.Manager.AddReturn(GetPlatformInfo, null, temp36);
            this.Manager.Comment("reaching state \'S103\'");
            int temp46 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonEx.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonExS102GetPlatformChecker)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonEx.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonExS102GetPlatformChecker1)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonEx.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonExS102GetPlatformChecker2)));
            if ((temp46 == 0)) {
                this.Manager.Comment("reaching state \'S279\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp37;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp37 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S468\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp37, "return of NetrServerReqChallenge, state S468");
                this.Manager.Comment("reaching state \'S657\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp38;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp38 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S846\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp38, "return of NetrServerAuthenticate3, state S846");
                this.Manager.Comment("reaching state \'S1035\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp39;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonEx(True,PrimaryDc,Client,NetlogonNetworkTra" +
                        "nsitiveInformation,Valid,NetlogonValidationSamInfo2,2)\'");
                temp39 = this.INrpcServerAdapterInstance.NetrLogonSamLogonEx(true, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonNetworkTransitiveInformation, ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo2, 2u);
                this.Manager.Checkpoint("MS-NRPC_R1169");
                this.Manager.Checkpoint("MS-NRPC_R1161");
                this.Manager.Comment("reaching state \'S1224\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonEx/STATUS_NO_SUCH_USER\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NO_SUCH_USER, temp39, "return of NetrLogonSamLogonEx, state S1224");
                this.Manager.Comment("reaching state \'S1413\'");
                goto label3;
            }
            if ((temp46 == 1)) {
                this.Manager.Comment("reaching state \'S280\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp40;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp40 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S469\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp40, "return of NetrServerReqChallenge, state S469");
                this.Manager.Comment("reaching state \'S658\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp41;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp41 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S847\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp41, "return of NetrServerAuthenticate3, state S847");
                this.Manager.Comment("reaching state \'S1036\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp42;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonEx(True,PrimaryDc,Client,NetlogonInteractiv" +
                        "eInformation,Valid,NetlogonValidationSamInfo,0)\'");
                temp42 = this.INrpcServerAdapterInstance.NetrLogonSamLogonEx(true, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo, 0u);
                this.Manager.Checkpoint("MS-NRPC_R1174");
                this.Manager.Checkpoint("MS-NRPC_R1160");
                this.Manager.Comment("reaching state \'S1225\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonEx/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp42, "return of NetrLogonSamLogonEx, state S1225");
                this.Manager.Comment("reaching state \'S1414\'");
                goto label3;
            }
            if ((temp46 == 2)) {
                this.Manager.Comment("reaching state \'S281\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp43;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp43 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S470\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp43, "return of NetrServerReqChallenge, state S470");
                this.Manager.Comment("reaching state \'S659\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp44;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp44 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S848\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp44, "return of NetrServerAuthenticate3, state S848");
                this.Manager.Comment("reaching state \'S1037\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp45;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonEx(True,PrimaryDc,Client,NetlogonInteractiv" +
                        "eInformation,Valid,NetlogonValidationSamInfo,0)\'");
                temp45 = this.INrpcServerAdapterInstance.NetrLogonSamLogonEx(true, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo, 0u);
                this.Manager.Checkpoint("MS-NRPC_R1174");
                this.Manager.Checkpoint("MS-NRPC_R1160");
                this.Manager.Comment("reaching state \'S1226\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonEx/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp45, "return of NetrLogonSamLogonEx, state S1226");
                this.Manager.Comment("reaching state \'S1415\'");
                goto label3;
            }
            throw new InvalidOperationException("never reached");
        label3:
;
            this.Manager.EndTest();
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonExS102GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S103");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonExS102GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S103");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonExS102GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S103");
        }
        #endregion
        
        #region Test Starting in S104
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_PassThroughAuthenticate_NetrLogonSamLogonExS104() {
            this.Manager.BeginTest("Test_PassThroughAuthenticate_NetrLogonSamLogonExS104");
            this.Manager.Comment("reaching state \'S104\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp47;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp47);
            this.Manager.AddReturn(GetPlatformInfo, null, temp47);
            this.Manager.Comment("reaching state \'S105\'");
            int temp57 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonEx.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonExS104GetPlatformChecker)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonEx.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonExS104GetPlatformChecker1)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonEx.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonExS104GetPlatformChecker2)));
            if ((temp57 == 0)) {
                this.Manager.Comment("reaching state \'S282\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp48;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp48 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S471\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp48, "return of NetrServerReqChallenge, state S471");
                this.Manager.Comment("reaching state \'S660\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp49;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp49 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S849\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp49, "return of NetrServerAuthenticate3, state S849");
                this.Manager.Comment("reaching state \'S1038\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp50;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonEx(True,PrimaryDc,Client,NetlogonNetworkTra" +
                        "nsitiveInformation,Valid,NetlogonValidationSamInfo2,4)\'");
                temp50 = this.INrpcServerAdapterInstance.NetrLogonSamLogonEx(true, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonNetworkTransitiveInformation, ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo2, 4u);
                this.Manager.Checkpoint("MS-NRPC_R1160");
                this.Manager.Comment("reaching state \'S1227\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonEx/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp50, "return of NetrLogonSamLogonEx, state S1227");
                this.Manager.Comment("reaching state \'S1416\'");
                goto label4;
            }
            if ((temp57 == 1)) {
                this.Manager.Comment("reaching state \'S283\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp51;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp51 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S472\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp51, "return of NetrServerReqChallenge, state S472");
                this.Manager.Comment("reaching state \'S661\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp52;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp52 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S850\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp52, "return of NetrServerAuthenticate3, state S850");
                this.Manager.Comment("reaching state \'S1039\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp53;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonEx(True,PrimaryDc,Client,NetlogonInteractiv" +
                        "eInformation,Valid,NetlogonValidationSamInfo,0)\'");
                temp53 = this.INrpcServerAdapterInstance.NetrLogonSamLogonEx(true, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo, 0u);
                this.Manager.Checkpoint("MS-NRPC_R1174");
                this.Manager.Checkpoint("MS-NRPC_R1160");
                this.Manager.Comment("reaching state \'S1228\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonEx/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp53, "return of NetrLogonSamLogonEx, state S1228");
                this.Manager.Comment("reaching state \'S1417\'");
                goto label4;
            }
            if ((temp57 == 2)) {
                this.Manager.Comment("reaching state \'S284\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp54;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp54 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S473\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp54, "return of NetrServerReqChallenge, state S473");
                this.Manager.Comment("reaching state \'S662\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp55;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp55 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S851\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp55, "return of NetrServerAuthenticate3, state S851");
                this.Manager.Comment("reaching state \'S1040\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp56;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonEx(True,PrimaryDc,Client,NetlogonInteractiv" +
                        "eInformation,Valid,NetlogonValidationSamInfo,0)\'");
                temp56 = this.INrpcServerAdapterInstance.NetrLogonSamLogonEx(true, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo, 0u);
                this.Manager.Checkpoint("MS-NRPC_R1174");
                this.Manager.Checkpoint("MS-NRPC_R1160");
                this.Manager.Comment("reaching state \'S1229\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonEx/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp56, "return of NetrLogonSamLogonEx, state S1229");
                this.Manager.Comment("reaching state \'S1418\'");
                goto label4;
            }
            throw new InvalidOperationException("never reached");
        label4:
;
            this.Manager.EndTest();
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonExS104GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S105");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonExS104GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S105");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonExS104GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S105");
        }
        #endregion
        
        #region Test Starting in S106
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_PassThroughAuthenticate_NetrLogonSamLogonExS106() {
            this.Manager.BeginTest("Test_PassThroughAuthenticate_NetrLogonSamLogonExS106");
            this.Manager.Comment("reaching state \'S106\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp58;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp58);
            this.Manager.AddReturn(GetPlatformInfo, null, temp58);
            this.Manager.Comment("reaching state \'S107\'");
            int temp68 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonEx.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonExS106GetPlatformChecker)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonEx.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonExS106GetPlatformChecker1)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonEx.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonExS106GetPlatformChecker2)));
            if ((temp68 == 0)) {
                this.Manager.Comment("reaching state \'S285\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp59;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp59 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S474\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp59, "return of NetrServerReqChallenge, state S474");
                this.Manager.Comment("reaching state \'S663\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp60;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp60 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S852\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp60, "return of NetrServerAuthenticate3, state S852");
                this.Manager.Comment("reaching state \'S1041\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp61;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonEx(True,PrimaryDc,Client,NetlogonNetworkTra" +
                        "nsitiveInformation,Valid,NetlogonValidationSamInfo2,8)\'");
                temp61 = this.INrpcServerAdapterInstance.NetrLogonSamLogonEx(true, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonNetworkTransitiveInformation, ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo2, 8u);
                this.Manager.Checkpoint("MS-NRPC_R1160");
                this.Manager.Comment("reaching state \'S1230\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonEx/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp61, "return of NetrLogonSamLogonEx, state S1230");
                this.Manager.Comment("reaching state \'S1419\'");
                goto label5;
            }
            if ((temp68 == 1)) {
                this.Manager.Comment("reaching state \'S286\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp62;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp62 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S475\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp62, "return of NetrServerReqChallenge, state S475");
                this.Manager.Comment("reaching state \'S664\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp63;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp63 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S853\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp63, "return of NetrServerAuthenticate3, state S853");
                this.Manager.Comment("reaching state \'S1042\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp64;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonEx(True,PrimaryDc,Client,NetlogonInteractiv" +
                        "eInformation,Valid,NetlogonValidationSamInfo,0)\'");
                temp64 = this.INrpcServerAdapterInstance.NetrLogonSamLogonEx(true, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo, 0u);
                this.Manager.Checkpoint("MS-NRPC_R1174");
                this.Manager.Checkpoint("MS-NRPC_R1160");
                this.Manager.Comment("reaching state \'S1231\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonEx/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp64, "return of NetrLogonSamLogonEx, state S1231");
                this.Manager.Comment("reaching state \'S1420\'");
                goto label5;
            }
            if ((temp68 == 2)) {
                this.Manager.Comment("reaching state \'S287\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp65;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp65 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S476\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp65, "return of NetrServerReqChallenge, state S476");
                this.Manager.Comment("reaching state \'S665\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp66;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp66 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S854\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp66, "return of NetrServerAuthenticate3, state S854");
                this.Manager.Comment("reaching state \'S1043\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp67;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonEx(True,PrimaryDc,Client,NetlogonInteractiv" +
                        "eInformation,Valid,NetlogonValidationSamInfo,0)\'");
                temp67 = this.INrpcServerAdapterInstance.NetrLogonSamLogonEx(true, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo, 0u);
                this.Manager.Checkpoint("MS-NRPC_R1174");
                this.Manager.Checkpoint("MS-NRPC_R1160");
                this.Manager.Comment("reaching state \'S1232\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonEx/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp67, "return of NetrLogonSamLogonEx, state S1232");
                this.Manager.Comment("reaching state \'S1421\'");
                goto label5;
            }
            throw new InvalidOperationException("never reached");
        label5:
;
            this.Manager.EndTest();
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonExS106GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S107");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonExS106GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S107");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonExS106GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S107");
        }
        #endregion
        
        #region Test Starting in S108
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_PassThroughAuthenticate_NetrLogonSamLogonExS108() {
            this.Manager.BeginTest("Test_PassThroughAuthenticate_NetrLogonSamLogonExS108");
            this.Manager.Comment("reaching state \'S108\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp69;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp69);
            this.Manager.AddReturn(GetPlatformInfo, null, temp69);
            this.Manager.Comment("reaching state \'S109\'");
            int temp79 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonEx.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonExS108GetPlatformChecker)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonEx.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonExS108GetPlatformChecker1)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonEx.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonExS108GetPlatformChecker2)));
            if ((temp79 == 0)) {
                this.Manager.Comment("reaching state \'S288\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp70;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp70 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S477\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp70, "return of NetrServerReqChallenge, state S477");
                this.Manager.Comment("reaching state \'S666\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp71;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp71 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S855\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp71, "return of NetrServerAuthenticate3, state S855");
                this.Manager.Comment("reaching state \'S1044\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp72;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonEx(True,PrimaryDc,Client,NetlogonNetworkTra" +
                        "nsitiveInformation,Valid,NetlogonValidationSamInfo2,2147483648)\'");
                temp72 = this.INrpcServerAdapterInstance.NetrLogonSamLogonEx(true, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonNetworkTransitiveInformation, ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo2, 2147483648u);
                this.Manager.Checkpoint("MS-NRPC_R1160");
                this.Manager.Comment("reaching state \'S1233\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonEx/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp72, "return of NetrLogonSamLogonEx, state S1233");
                this.Manager.Comment("reaching state \'S1422\'");
                goto label6;
            }
            if ((temp79 == 1)) {
                this.Manager.Comment("reaching state \'S289\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp73;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp73 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S478\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp73, "return of NetrServerReqChallenge, state S478");
                this.Manager.Comment("reaching state \'S667\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp74;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp74 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S856\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp74, "return of NetrServerAuthenticate3, state S856");
                this.Manager.Comment("reaching state \'S1045\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp75;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonEx(True,PrimaryDc,Client,NetlogonInteractiv" +
                        "eInformation,Valid,NetlogonValidationSamInfo,0)\'");
                temp75 = this.INrpcServerAdapterInstance.NetrLogonSamLogonEx(true, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo, 0u);
                this.Manager.Checkpoint("MS-NRPC_R1174");
                this.Manager.Checkpoint("MS-NRPC_R1160");
                this.Manager.Comment("reaching state \'S1234\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonEx/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp75, "return of NetrLogonSamLogonEx, state S1234");
                this.Manager.Comment("reaching state \'S1423\'");
                goto label6;
            }
            if ((temp79 == 2)) {
                this.Manager.Comment("reaching state \'S290\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp76;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp76 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S479\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp76, "return of NetrServerReqChallenge, state S479");
                this.Manager.Comment("reaching state \'S668\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp77;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp77 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S857\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp77, "return of NetrServerAuthenticate3, state S857");
                this.Manager.Comment("reaching state \'S1046\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp78;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonEx(True,PrimaryDc,Client,NetlogonInteractiv" +
                        "eInformation,Valid,NetlogonValidationSamInfo,0)\'");
                temp78 = this.INrpcServerAdapterInstance.NetrLogonSamLogonEx(true, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo, 0u);
                this.Manager.Checkpoint("MS-NRPC_R1174");
                this.Manager.Checkpoint("MS-NRPC_R1160");
                this.Manager.Comment("reaching state \'S1235\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonEx/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp78, "return of NetrLogonSamLogonEx, state S1235");
                this.Manager.Comment("reaching state \'S1424\'");
                goto label6;
            }
            throw new InvalidOperationException("never reached");
        label6:
;
            this.Manager.EndTest();
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonExS108GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S109");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonExS108GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S109");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonExS108GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S109");
        }
        #endregion
        
        #region Test Starting in S110
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_PassThroughAuthenticate_NetrLogonSamLogonExS110() {
            this.Manager.BeginTest("Test_PassThroughAuthenticate_NetrLogonSamLogonExS110");
            this.Manager.Comment("reaching state \'S110\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp80;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp80);
            this.Manager.AddReturn(GetPlatformInfo, null, temp80);
            this.Manager.Comment("reaching state \'S111\'");
            int temp90 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonEx.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonExS110GetPlatformChecker)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonEx.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonExS110GetPlatformChecker1)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonEx.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonExS110GetPlatformChecker2)));
            if ((temp90 == 0)) {
                this.Manager.Comment("reaching state \'S291\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp81;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp81 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S480\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp81, "return of NetrServerReqChallenge, state S480");
                this.Manager.Comment("reaching state \'S669\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp82;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp82 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S858\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp82, "return of NetrServerAuthenticate3, state S858");
                this.Manager.Comment("reaching state \'S1047\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp83;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonEx(True,PrimaryDc,Client,NetlogonInteractiv" +
                        "eTransitiveInformation,Valid,NetlogonValidationGenericInfo2,0)\'");
                temp83 = this.INrpcServerAdapterInstance.NetrLogonSamLogonEx(true, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonInteractiveTransitiveInformation, ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationGenericInfo2, 0u);
                this.Manager.Checkpoint("MS-NRPC_R1161");
                this.Manager.Comment("reaching state \'S1236\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonEx/STATUS_INVALID_INFO_CLASS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_INVALID_INFO_CLASS, temp83, "return of NetrLogonSamLogonEx, state S1236");
                this.Manager.Comment("reaching state \'S1425\'");
                goto label7;
            }
            if ((temp90 == 1)) {
                this.Manager.Comment("reaching state \'S292\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp84;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp84 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S481\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp84, "return of NetrServerReqChallenge, state S481");
                this.Manager.Comment("reaching state \'S670\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp85;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp85 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S859\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp85, "return of NetrServerAuthenticate3, state S859");
                this.Manager.Comment("reaching state \'S1048\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp86;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonEx(True,PrimaryDc,Client,NetlogonInteractiv" +
                        "eInformation,Valid,NetlogonValidationSamInfo,0)\'");
                temp86 = this.INrpcServerAdapterInstance.NetrLogonSamLogonEx(true, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo, 0u);
                this.Manager.Checkpoint("MS-NRPC_R1174");
                this.Manager.Checkpoint("MS-NRPC_R1160");
                this.Manager.Comment("reaching state \'S1237\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonEx/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp86, "return of NetrLogonSamLogonEx, state S1237");
                this.Manager.Comment("reaching state \'S1426\'");
                goto label7;
            }
            if ((temp90 == 2)) {
                this.Manager.Comment("reaching state \'S293\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp87;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp87 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S482\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp87, "return of NetrServerReqChallenge, state S482");
                this.Manager.Comment("reaching state \'S671\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp88;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp88 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S860\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp88, "return of NetrServerAuthenticate3, state S860");
                this.Manager.Comment("reaching state \'S1049\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp89;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonEx(True,PrimaryDc,Client,NetlogonInteractiv" +
                        "eInformation,Valid,NetlogonValidationSamInfo,0)\'");
                temp89 = this.INrpcServerAdapterInstance.NetrLogonSamLogonEx(true, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo, 0u);
                this.Manager.Checkpoint("MS-NRPC_R1174");
                this.Manager.Checkpoint("MS-NRPC_R1160");
                this.Manager.Comment("reaching state \'S1238\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonEx/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp89, "return of NetrLogonSamLogonEx, state S1238");
                this.Manager.Comment("reaching state \'S1427\'");
                goto label7;
            }
            throw new InvalidOperationException("never reached");
        label7:
;
            this.Manager.EndTest();
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonExS110GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S111");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonExS110GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S111");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonExS110GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S111");
        }
        #endregion
        
        #region Test Starting in S112
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_PassThroughAuthenticate_NetrLogonSamLogonExS112() {
            this.Manager.BeginTest("Test_PassThroughAuthenticate_NetrLogonSamLogonExS112");
            this.Manager.Comment("reaching state \'S112\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp91;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp91);
            this.Manager.AddReturn(GetPlatformInfo, null, temp91);
            this.Manager.Comment("reaching state \'S113\'");
            int temp101 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonEx.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonExS112GetPlatformChecker)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonEx.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonExS112GetPlatformChecker1)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonEx.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonExS112GetPlatformChecker2)));
            if ((temp101 == 0)) {
                this.Manager.Comment("reaching state \'S294\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp92;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp92 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S483\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp92, "return of NetrServerReqChallenge, state S483");
                this.Manager.Comment("reaching state \'S672\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp93;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp93 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S861\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp93, "return of NetrServerAuthenticate3, state S861");
                this.Manager.Comment("reaching state \'S1050\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp94;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonEx(True,PrimaryDc,Client,NetlogonInteractiv" +
                        "eTransitiveInformation,Valid,NetlogonValidationSamInfo4,0)\'");
                temp94 = this.INrpcServerAdapterInstance.NetrLogonSamLogonEx(true, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonInteractiveTransitiveInformation, ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo4, 0u);
                this.Manager.Checkpoint("MS-NRPC_R1175");
                this.Manager.Checkpoint("MS-NRPC_R1160");
                this.Manager.Comment("reaching state \'S1239\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonEx/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp94, "return of NetrLogonSamLogonEx, state S1239");
                this.Manager.Comment("reaching state \'S1428\'");
                goto label8;
            }
            if ((temp101 == 1)) {
                this.Manager.Comment("reaching state \'S295\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp95;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp95 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S484\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp95, "return of NetrServerReqChallenge, state S484");
                this.Manager.Comment("reaching state \'S673\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp96;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp96 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S862\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp96, "return of NetrServerAuthenticate3, state S862");
                this.Manager.Comment("reaching state \'S1051\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp97;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonEx(True,PrimaryDc,Client,NetlogonInteractiv" +
                        "eInformation,Valid,NetlogonValidationSamInfo,0)\'");
                temp97 = this.INrpcServerAdapterInstance.NetrLogonSamLogonEx(true, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo, 0u);
                this.Manager.Checkpoint("MS-NRPC_R1174");
                this.Manager.Checkpoint("MS-NRPC_R1160");
                this.Manager.Comment("reaching state \'S1240\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonEx/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp97, "return of NetrLogonSamLogonEx, state S1240");
                this.Manager.Comment("reaching state \'S1429\'");
                goto label8;
            }
            if ((temp101 == 2)) {
                this.Manager.Comment("reaching state \'S296\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp98;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp98 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S485\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp98, "return of NetrServerReqChallenge, state S485");
                this.Manager.Comment("reaching state \'S674\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp99;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp99 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S863\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp99, "return of NetrServerAuthenticate3, state S863");
                this.Manager.Comment("reaching state \'S1052\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp100;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonEx(True,PrimaryDc,Client,NetlogonInteractiv" +
                        "eInformation,Valid,NetlogonValidationSamInfo,0)\'");
                temp100 = this.INrpcServerAdapterInstance.NetrLogonSamLogonEx(true, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo, 0u);
                this.Manager.Checkpoint("MS-NRPC_R1174");
                this.Manager.Checkpoint("MS-NRPC_R1160");
                this.Manager.Comment("reaching state \'S1241\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonEx/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp100, "return of NetrLogonSamLogonEx, state S1241");
                this.Manager.Comment("reaching state \'S1430\'");
                goto label8;
            }
            throw new InvalidOperationException("never reached");
        label8:
;
            this.Manager.EndTest();
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonExS112GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S113");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonExS112GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S113");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonExS112GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S113");
        }
        #endregion
        
        #region Test Starting in S114
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_PassThroughAuthenticate_NetrLogonSamLogonExS114() {
            this.Manager.BeginTest("Test_PassThroughAuthenticate_NetrLogonSamLogonExS114");
            this.Manager.Comment("reaching state \'S114\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp102;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp102);
            this.Manager.AddReturn(GetPlatformInfo, null, temp102);
            this.Manager.Comment("reaching state \'S115\'");
            int temp112 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonEx.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonExS114GetPlatformChecker)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonEx.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonExS114GetPlatformChecker1)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonEx.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonExS114GetPlatformChecker2)));
            if ((temp112 == 0)) {
                this.Manager.Comment("reaching state \'S297\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp103;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp103 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S486\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp103, "return of NetrServerReqChallenge, state S486");
                this.Manager.Comment("reaching state \'S675\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp104;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp104 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S864\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp104, "return of NetrServerAuthenticate3, state S864");
                this.Manager.Comment("reaching state \'S1053\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp105;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonEx(True,InvalidComputer,Client,NetlogonNetw" +
                        "orkTransitiveInformation,Valid,NetlogonValidationSamInfo2,0)\'");
                temp105 = this.INrpcServerAdapterInstance.NetrLogonSamLogonEx(true, ((Microsoft.Protocols.TestSuites.Nrpc.ComputerType)(1)), Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonNetworkTransitiveInformation, ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo2, 0u);
                this.Manager.Checkpoint("MS-NRPC_R1172");
                this.Manager.Checkpoint("MS-NRPC_R1161");
                this.Manager.Comment("reaching state \'S1242\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonEx/STATUS_INVALID_COMPUTER_NAME\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_INVALID_COMPUTER_NAME, temp105, "return of NetrLogonSamLogonEx, state S1242");
                this.Manager.Comment("reaching state \'S1431\'");
                goto label9;
            }
            if ((temp112 == 1)) {
                this.Manager.Comment("reaching state \'S298\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp106;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp106 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S487\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp106, "return of NetrServerReqChallenge, state S487");
                this.Manager.Comment("reaching state \'S676\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp107;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp107 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S865\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp107, "return of NetrServerAuthenticate3, state S865");
                this.Manager.Comment("reaching state \'S1054\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp108;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonEx(True,PrimaryDc,Client,NetlogonInteractiv" +
                        "eInformation,Valid,NetlogonValidationSamInfo,0)\'");
                temp108 = this.INrpcServerAdapterInstance.NetrLogonSamLogonEx(true, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo, 0u);
                this.Manager.Checkpoint("MS-NRPC_R1174");
                this.Manager.Checkpoint("MS-NRPC_R1160");
                this.Manager.Comment("reaching state \'S1243\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonEx/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp108, "return of NetrLogonSamLogonEx, state S1243");
                this.Manager.Comment("reaching state \'S1432\'");
                goto label9;
            }
            if ((temp112 == 2)) {
                this.Manager.Comment("reaching state \'S299\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp109;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp109 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S488\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp109, "return of NetrServerReqChallenge, state S488");
                this.Manager.Comment("reaching state \'S677\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp110;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp110 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S866\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp110, "return of NetrServerAuthenticate3, state S866");
                this.Manager.Comment("reaching state \'S1055\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp111;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonEx(True,PrimaryDc,Client,NetlogonInteractiv" +
                        "eInformation,Valid,NetlogonValidationSamInfo,0)\'");
                temp111 = this.INrpcServerAdapterInstance.NetrLogonSamLogonEx(true, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo, 0u);
                this.Manager.Checkpoint("MS-NRPC_R1174");
                this.Manager.Checkpoint("MS-NRPC_R1160");
                this.Manager.Comment("reaching state \'S1244\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonEx/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp111, "return of NetrLogonSamLogonEx, state S1244");
                this.Manager.Comment("reaching state \'S1433\'");
                goto label9;
            }
            throw new InvalidOperationException("never reached");
        label9:
;
            this.Manager.EndTest();
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonExS114GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S115");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonExS114GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S115");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonExS114GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S115");
        }
        #endregion
        
        #region Test Starting in S116
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_PassThroughAuthenticate_NetrLogonSamLogonExS116() {
            this.Manager.BeginTest("Test_PassThroughAuthenticate_NetrLogonSamLogonExS116");
            this.Manager.Comment("reaching state \'S116\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp113;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp113);
            this.Manager.AddReturn(GetPlatformInfo, null, temp113);
            this.Manager.Comment("reaching state \'S117\'");
            int temp123 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonEx.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonExS116GetPlatformChecker)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonEx.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonExS116GetPlatformChecker1)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonEx.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonExS116GetPlatformChecker2)));
            if ((temp123 == 0)) {
                this.Manager.Comment("reaching state \'S300\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp114;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp114 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S489\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp114, "return of NetrServerReqChallenge, state S489");
                this.Manager.Comment("reaching state \'S678\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp115;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp115 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S867\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp115, "return of NetrServerAuthenticate3, state S867");
                this.Manager.Comment("reaching state \'S1056\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp116;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonEx(True,NonDcServer,Client,NetlogonNetworkT" +
                        "ransitiveInformation,Valid,NetlogonValidationSamInfo2,0)\'");
                temp116 = this.INrpcServerAdapterInstance.NetrLogonSamLogonEx(true, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonNetworkTransitiveInformation, ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo2, 0u);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R101162");
                this.Manager.Comment("reaching state \'S1245\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonEx/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp116, "return of NetrLogonSamLogonEx, state S1245");
                this.Manager.Comment("reaching state \'S1434\'");
                goto label10;
            }
            if ((temp123 == 1)) {
                this.Manager.Comment("reaching state \'S301\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp117;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp117 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S490\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp117, "return of NetrServerReqChallenge, state S490");
                this.Manager.Comment("reaching state \'S679\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp118;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp118 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S868\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp118, "return of NetrServerAuthenticate3, state S868");
                this.Manager.Comment("reaching state \'S1057\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp119;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonEx(True,PrimaryDc,Client,NetlogonInteractiv" +
                        "eInformation,Valid,NetlogonValidationSamInfo,0)\'");
                temp119 = this.INrpcServerAdapterInstance.NetrLogonSamLogonEx(true, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo, 0u);
                this.Manager.Checkpoint("MS-NRPC_R1174");
                this.Manager.Checkpoint("MS-NRPC_R1160");
                this.Manager.Comment("reaching state \'S1246\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonEx/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp119, "return of NetrLogonSamLogonEx, state S1246");
                this.Manager.Comment("reaching state \'S1435\'");
                goto label10;
            }
            if ((temp123 == 2)) {
                this.Manager.Comment("reaching state \'S302\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp120;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp120 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S491\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp120, "return of NetrServerReqChallenge, state S491");
                this.Manager.Comment("reaching state \'S680\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp121;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp121 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S869\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp121, "return of NetrServerAuthenticate3, state S869");
                this.Manager.Comment("reaching state \'S1058\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp122;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonEx(True,PrimaryDc,Client,NetlogonInteractiv" +
                        "eInformation,Valid,NetlogonValidationSamInfo,0)\'");
                temp122 = this.INrpcServerAdapterInstance.NetrLogonSamLogonEx(true, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo, 0u);
                this.Manager.Checkpoint("MS-NRPC_R1174");
                this.Manager.Checkpoint("MS-NRPC_R1160");
                this.Manager.Comment("reaching state \'S1247\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonEx/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp122, "return of NetrLogonSamLogonEx, state S1247");
                this.Manager.Comment("reaching state \'S1436\'");
                goto label10;
            }
            throw new InvalidOperationException("never reached");
        label10:
;
            this.Manager.EndTest();
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonExS116GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S117");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonExS116GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S117");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonExS116GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S117");
        }
        #endregion
        
        #region Test Starting in S118
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_PassThroughAuthenticate_NetrLogonSamLogonExS118() {
            this.Manager.BeginTest("Test_PassThroughAuthenticate_NetrLogonSamLogonExS118");
            this.Manager.Comment("reaching state \'S118\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp124;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp124);
            this.Manager.AddReturn(GetPlatformInfo, null, temp124);
            this.Manager.Comment("reaching state \'S119\'");
            int temp134 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonEx.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonExS118GetPlatformChecker)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonEx.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonExS118GetPlatformChecker1)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonEx.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonExS118GetPlatformChecker2)));
            if ((temp134 == 0)) {
                this.Manager.Comment("reaching state \'S303\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp125;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp125 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S492\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp125, "return of NetrServerReqChallenge, state S492");
                this.Manager.Comment("reaching state \'S681\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp126;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp126 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S870\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp126, "return of NetrServerAuthenticate3, state S870");
                this.Manager.Comment("reaching state \'S1059\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp127;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonEx(True,PrimaryDc,Client,NetlogonServiceTra" +
                        "nsitiveInformation,Valid,NetlogonValidationGenericInfo2,0)\'");
                temp127 = this.INrpcServerAdapterInstance.NetrLogonSamLogonEx(true, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonServiceTransitiveInformation, ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationGenericInfo2, 0u);
                this.Manager.Checkpoint("MS-NRPC_R1161");
                this.Manager.Comment("reaching state \'S1248\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonEx/STATUS_INVALID_INFO_CLASS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_INVALID_INFO_CLASS, temp127, "return of NetrLogonSamLogonEx, state S1248");
                this.Manager.Comment("reaching state \'S1437\'");
                goto label11;
            }
            if ((temp134 == 1)) {
                this.Manager.Comment("reaching state \'S304\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp128;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp128 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S493\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp128, "return of NetrServerReqChallenge, state S493");
                this.Manager.Comment("reaching state \'S682\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp129;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp129 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S871\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp129, "return of NetrServerAuthenticate3, state S871");
                this.Manager.Comment("reaching state \'S1060\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp130;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonEx(True,PrimaryDc,Client,NetlogonInteractiv" +
                        "eInformation,Valid,NetlogonValidationSamInfo,0)\'");
                temp130 = this.INrpcServerAdapterInstance.NetrLogonSamLogonEx(true, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo, 0u);
                this.Manager.Checkpoint("MS-NRPC_R1174");
                this.Manager.Checkpoint("MS-NRPC_R1160");
                this.Manager.Comment("reaching state \'S1249\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonEx/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp130, "return of NetrLogonSamLogonEx, state S1249");
                this.Manager.Comment("reaching state \'S1438\'");
                goto label11;
            }
            if ((temp134 == 2)) {
                this.Manager.Comment("reaching state \'S305\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp131;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp131 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S494\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp131, "return of NetrServerReqChallenge, state S494");
                this.Manager.Comment("reaching state \'S683\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp132;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp132 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S872\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp132, "return of NetrServerAuthenticate3, state S872");
                this.Manager.Comment("reaching state \'S1061\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp133;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonEx(True,PrimaryDc,Client,NetlogonInteractiv" +
                        "eInformation,Valid,NetlogonValidationSamInfo,0)\'");
                temp133 = this.INrpcServerAdapterInstance.NetrLogonSamLogonEx(true, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo, 0u);
                this.Manager.Checkpoint("MS-NRPC_R1174");
                this.Manager.Checkpoint("MS-NRPC_R1160");
                this.Manager.Comment("reaching state \'S1250\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonEx/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp133, "return of NetrLogonSamLogonEx, state S1250");
                this.Manager.Comment("reaching state \'S1439\'");
                goto label11;
            }
            throw new InvalidOperationException("never reached");
        label11:
;
            this.Manager.EndTest();
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonExS118GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S119");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonExS118GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S119");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonExS118GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S119");
        }
        #endregion
        
        #region Test Starting in S12
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_PassThroughAuthenticate_NetrLogonSamLogonExS12() {
            this.Manager.BeginTest("Test_PassThroughAuthenticate_NetrLogonSamLogonExS12");
            this.Manager.Comment("reaching state \'S12\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp135;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp135);
            this.Manager.AddReturn(GetPlatformInfo, null, temp135);
            this.Manager.Comment("reaching state \'S13\'");
            int temp145 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonEx.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonExS12GetPlatformChecker)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonEx.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonExS12GetPlatformChecker1)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonEx.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonExS12GetPlatformChecker2)));
            if ((temp145 == 0)) {
                this.Manager.Comment("reaching state \'S144\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp136;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp136 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S333\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp136, "return of NetrServerReqChallenge, state S333");
                this.Manager.Comment("reaching state \'S522\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp137;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp137 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S711\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp137, "return of NetrServerAuthenticate3, state S711");
                this.Manager.Comment("reaching state \'S900\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp138;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonEx(True,PrimaryDc,Client,NetlogonInteractiv" +
                        "eInformation,Valid,NetlogonValidationGenericInfo2,0)\'");
                temp138 = this.INrpcServerAdapterInstance.NetrLogonSamLogonEx(true, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationGenericInfo2, 0u);
                this.Manager.Checkpoint("MS-NRPC_R1161");
                this.Manager.Comment("reaching state \'S1089\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonEx/STATUS_INVALID_INFO_CLASS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_INVALID_INFO_CLASS, temp138, "return of NetrLogonSamLogonEx, state S1089");
                this.Manager.Comment("reaching state \'S1278\'");
                goto label12;
            }
            if ((temp145 == 1)) {
                this.Manager.Comment("reaching state \'S145\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp139;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp139 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S334\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp139, "return of NetrServerReqChallenge, state S334");
                this.Manager.Comment("reaching state \'S523\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp140;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp140 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S712\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp140, "return of NetrServerAuthenticate3, state S712");
                this.Manager.Comment("reaching state \'S901\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp141;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonEx(True,PrimaryDc,Client,NetlogonInteractiv" +
                        "eInformation,Valid,NetlogonValidationGenericInfo2,0)\'");
                temp141 = this.INrpcServerAdapterInstance.NetrLogonSamLogonEx(true, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationGenericInfo2, 0u);
                this.Manager.Checkpoint("MS-NRPC_R1161");
                this.Manager.Comment("reaching state \'S1090\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonEx/STATUS_INVALID_INFO_CLASS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_INVALID_INFO_CLASS, temp141, "return of NetrLogonSamLogonEx, state S1090");
                this.Manager.Comment("reaching state \'S1279\'");
                goto label12;
            }
            if ((temp145 == 2)) {
                this.Manager.Comment("reaching state \'S146\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp142;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp142 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S335\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp142, "return of NetrServerReqChallenge, state S335");
                this.Manager.Comment("reaching state \'S524\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp143;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp143 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S713\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp143, "return of NetrServerAuthenticate3, state S713");
                this.Manager.Comment("reaching state \'S902\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp144;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonEx(True,PrimaryDc,Client,NetlogonInteractiv" +
                        "eInformation,Valid,NetlogonValidationSamInfo,0)\'");
                temp144 = this.INrpcServerAdapterInstance.NetrLogonSamLogonEx(true, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo, 0u);
                this.Manager.Checkpoint("MS-NRPC_R1174");
                this.Manager.Checkpoint("MS-NRPC_R1160");
                this.Manager.Comment("reaching state \'S1091\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonEx/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp144, "return of NetrLogonSamLogonEx, state S1091");
                this.Manager.Comment("reaching state \'S1280\'");
                goto label12;
            }
            throw new InvalidOperationException("never reached");
        label12:
;
            this.Manager.EndTest();
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonExS12GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S13");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonExS12GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S13");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonExS12GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S13");
        }
        #endregion
        
        #region Test Starting in S120
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_PassThroughAuthenticate_NetrLogonSamLogonExS120() {
            this.Manager.BeginTest("Test_PassThroughAuthenticate_NetrLogonSamLogonExS120");
            this.Manager.Comment("reaching state \'S120\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp146;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp146);
            this.Manager.AddReturn(GetPlatformInfo, null, temp146);
            this.Manager.Comment("reaching state \'S121\'");
            int temp156 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonEx.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonExS120GetPlatformChecker)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonEx.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonExS120GetPlatformChecker1)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonEx.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonExS120GetPlatformChecker2)));
            if ((temp156 == 0)) {
                this.Manager.Comment("reaching state \'S306\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp147;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp147 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S495\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp147, "return of NetrServerReqChallenge, state S495");
                this.Manager.Comment("reaching state \'S684\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp148;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp148 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S873\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp148, "return of NetrServerAuthenticate3, state S873");
                this.Manager.Comment("reaching state \'S1062\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp149;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonEx(True,PrimaryDc,Client,NetlogonServiceTra" +
                        "nsitiveInformation,Valid,NetlogonValidationSamInfo4,0)\'");
                temp149 = this.INrpcServerAdapterInstance.NetrLogonSamLogonEx(true, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonServiceTransitiveInformation, ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo4, 0u);
                this.Manager.Checkpoint("MS-NRPC_R101177");
                this.Manager.Checkpoint("MS-NRPC_R1160");
                this.Manager.Comment("reaching state \'S1251\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonEx/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp149, "return of NetrLogonSamLogonEx, state S1251");
                this.Manager.Comment("reaching state \'S1440\'");
                goto label13;
            }
            if ((temp156 == 1)) {
                this.Manager.Comment("reaching state \'S307\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp150;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp150 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S496\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp150, "return of NetrServerReqChallenge, state S496");
                this.Manager.Comment("reaching state \'S685\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp151;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp151 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S874\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp151, "return of NetrServerAuthenticate3, state S874");
                this.Manager.Comment("reaching state \'S1063\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp152;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonEx(True,PrimaryDc,Client,NetlogonInteractiv" +
                        "eInformation,Valid,NetlogonValidationSamInfo,0)\'");
                temp152 = this.INrpcServerAdapterInstance.NetrLogonSamLogonEx(true, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo, 0u);
                this.Manager.Checkpoint("MS-NRPC_R1174");
                this.Manager.Checkpoint("MS-NRPC_R1160");
                this.Manager.Comment("reaching state \'S1252\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonEx/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp152, "return of NetrLogonSamLogonEx, state S1252");
                this.Manager.Comment("reaching state \'S1441\'");
                goto label13;
            }
            if ((temp156 == 2)) {
                this.Manager.Comment("reaching state \'S308\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp153;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp153 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S497\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp153, "return of NetrServerReqChallenge, state S497");
                this.Manager.Comment("reaching state \'S686\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp154;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp154 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S875\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp154, "return of NetrServerAuthenticate3, state S875");
                this.Manager.Comment("reaching state \'S1064\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp155;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonEx(True,PrimaryDc,Client,NetlogonInteractiv" +
                        "eInformation,Valid,NetlogonValidationSamInfo,0)\'");
                temp155 = this.INrpcServerAdapterInstance.NetrLogonSamLogonEx(true, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo, 0u);
                this.Manager.Checkpoint("MS-NRPC_R1174");
                this.Manager.Checkpoint("MS-NRPC_R1160");
                this.Manager.Comment("reaching state \'S1253\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonEx/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp155, "return of NetrLogonSamLogonEx, state S1253");
                this.Manager.Comment("reaching state \'S1442\'");
                goto label13;
            }
            throw new InvalidOperationException("never reached");
        label13:
;
            this.Manager.EndTest();
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonExS120GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S121");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonExS120GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S121");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonExS120GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S121");
        }
        #endregion
        
        #region Test Starting in S122
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_PassThroughAuthenticate_NetrLogonSamLogonExS122() {
            this.Manager.BeginTest("Test_PassThroughAuthenticate_NetrLogonSamLogonExS122");
            this.Manager.Comment("reaching state \'S122\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp157;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp157);
            this.Manager.AddReturn(GetPlatformInfo, null, temp157);
            this.Manager.Comment("reaching state \'S123\'");
            int temp167 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonEx.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonExS122GetPlatformChecker)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonEx.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonExS122GetPlatformChecker1)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonEx.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonExS122GetPlatformChecker2)));
            if ((temp167 == 0)) {
                this.Manager.Comment("reaching state \'S309\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp158;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp158 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S498\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp158, "return of NetrServerReqChallenge, state S498");
                this.Manager.Comment("reaching state \'S687\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp159;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp159 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S876\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp159, "return of NetrServerAuthenticate3, state S876");
                this.Manager.Comment("reaching state \'S1065\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp160;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonEx(True,PrimaryDc,Client,NetlogonNetworkTra" +
                        "nsitiveInformation,Valid,NetlogonValidationGenericInfo2,0)\'");
                temp160 = this.INrpcServerAdapterInstance.NetrLogonSamLogonEx(true, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonNetworkTransitiveInformation, ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationGenericInfo2, 0u);
                this.Manager.Checkpoint("MS-NRPC_R1161");
                this.Manager.Comment("reaching state \'S1254\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonEx/STATUS_INVALID_INFO_CLASS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_INVALID_INFO_CLASS, temp160, "return of NetrLogonSamLogonEx, state S1254");
                this.Manager.Comment("reaching state \'S1443\'");
                goto label14;
            }
            if ((temp167 == 1)) {
                this.Manager.Comment("reaching state \'S310\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp161;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp161 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S499\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp161, "return of NetrServerReqChallenge, state S499");
                this.Manager.Comment("reaching state \'S688\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp162;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp162 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S877\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp162, "return of NetrServerAuthenticate3, state S877");
                this.Manager.Comment("reaching state \'S1066\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp163;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonEx(True,PrimaryDc,Client,NetlogonInteractiv" +
                        "eInformation,Valid,NetlogonValidationSamInfo,0)\'");
                temp163 = this.INrpcServerAdapterInstance.NetrLogonSamLogonEx(true, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo, 0u);
                this.Manager.Checkpoint("MS-NRPC_R1174");
                this.Manager.Checkpoint("MS-NRPC_R1160");
                this.Manager.Comment("reaching state \'S1255\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonEx/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp163, "return of NetrLogonSamLogonEx, state S1255");
                this.Manager.Comment("reaching state \'S1444\'");
                goto label14;
            }
            if ((temp167 == 2)) {
                this.Manager.Comment("reaching state \'S311\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp164;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp164 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S500\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp164, "return of NetrServerReqChallenge, state S500");
                this.Manager.Comment("reaching state \'S689\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp165;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp165 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S878\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp165, "return of NetrServerAuthenticate3, state S878");
                this.Manager.Comment("reaching state \'S1067\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp166;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonEx(True,PrimaryDc,Client,NetlogonInteractiv" +
                        "eInformation,Valid,NetlogonValidationSamInfo,0)\'");
                temp166 = this.INrpcServerAdapterInstance.NetrLogonSamLogonEx(true, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo, 0u);
                this.Manager.Checkpoint("MS-NRPC_R1174");
                this.Manager.Checkpoint("MS-NRPC_R1160");
                this.Manager.Comment("reaching state \'S1256\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonEx/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp166, "return of NetrLogonSamLogonEx, state S1256");
                this.Manager.Comment("reaching state \'S1445\'");
                goto label14;
            }
            throw new InvalidOperationException("never reached");
        label14:
;
            this.Manager.EndTest();
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonExS122GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S123");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonExS122GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S123");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonExS122GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S123");
        }
        #endregion
        
        #region Test Starting in S124
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_PassThroughAuthenticate_NetrLogonSamLogonExS124() {
            this.Manager.BeginTest("Test_PassThroughAuthenticate_NetrLogonSamLogonExS124");
            this.Manager.Comment("reaching state \'S124\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp168;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp168);
            this.Manager.AddReturn(GetPlatformInfo, null, temp168);
            this.Manager.Comment("reaching state \'S125\'");
            int temp178 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonEx.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonExS124GetPlatformChecker)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonEx.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonExS124GetPlatformChecker1)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonEx.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonExS124GetPlatformChecker2)));
            if ((temp178 == 0)) {
                this.Manager.Comment("reaching state \'S312\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp169;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp169 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S501\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp169, "return of NetrServerReqChallenge, state S501");
                this.Manager.Comment("reaching state \'S690\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp170;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp170 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S879\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp170, "return of NetrServerAuthenticate3, state S879");
                this.Manager.Comment("reaching state \'S1068\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp171;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonEx(True,PrimaryDc,Client,NetlogonNetworkTra" +
                        "nsitiveInformation,Valid,NetlogonValidationSamInfo4,0)\'");
                temp171 = this.INrpcServerAdapterInstance.NetrLogonSamLogonEx(true, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonNetworkTransitiveInformation, ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo4, 0u);
                this.Manager.Checkpoint("MS-NRPC_R1160");
                this.Manager.Comment("reaching state \'S1257\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonEx/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp171, "return of NetrLogonSamLogonEx, state S1257");
                this.Manager.Comment("reaching state \'S1446\'");
                goto label15;
            }
            if ((temp178 == 1)) {
                this.Manager.Comment("reaching state \'S313\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp172;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp172 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S502\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp172, "return of NetrServerReqChallenge, state S502");
                this.Manager.Comment("reaching state \'S691\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp173;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp173 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S880\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp173, "return of NetrServerAuthenticate3, state S880");
                this.Manager.Comment("reaching state \'S1069\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp174;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonEx(True,PrimaryDc,Client,NetlogonInteractiv" +
                        "eInformation,Valid,NetlogonValidationSamInfo,0)\'");
                temp174 = this.INrpcServerAdapterInstance.NetrLogonSamLogonEx(true, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo, 0u);
                this.Manager.Checkpoint("MS-NRPC_R1174");
                this.Manager.Checkpoint("MS-NRPC_R1160");
                this.Manager.Comment("reaching state \'S1258\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonEx/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp174, "return of NetrLogonSamLogonEx, state S1258");
                this.Manager.Comment("reaching state \'S1447\'");
                goto label15;
            }
            if ((temp178 == 2)) {
                this.Manager.Comment("reaching state \'S314\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp175;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp175 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S503\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp175, "return of NetrServerReqChallenge, state S503");
                this.Manager.Comment("reaching state \'S692\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp176;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp176 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S881\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp176, "return of NetrServerAuthenticate3, state S881");
                this.Manager.Comment("reaching state \'S1070\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp177;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonEx(True,PrimaryDc,Client,NetlogonInteractiv" +
                        "eInformation,Valid,NetlogonValidationSamInfo,0)\'");
                temp177 = this.INrpcServerAdapterInstance.NetrLogonSamLogonEx(true, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo, 0u);
                this.Manager.Checkpoint("MS-NRPC_R1174");
                this.Manager.Checkpoint("MS-NRPC_R1160");
                this.Manager.Comment("reaching state \'S1259\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonEx/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp177, "return of NetrLogonSamLogonEx, state S1259");
                this.Manager.Comment("reaching state \'S1448\'");
                goto label15;
            }
            throw new InvalidOperationException("never reached");
        label15:
;
            this.Manager.EndTest();
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonExS124GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S125");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonExS124GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S125");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonExS124GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S125");
        }
        #endregion
        
        #region Test Starting in S14
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_PassThroughAuthenticate_NetrLogonSamLogonExS14() {
            this.Manager.BeginTest("Test_PassThroughAuthenticate_NetrLogonSamLogonExS14");
            this.Manager.Comment("reaching state \'S14\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp179;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp179);
            this.Manager.AddReturn(GetPlatformInfo, null, temp179);
            this.Manager.Comment("reaching state \'S15\'");
            int temp189 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonEx.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonExS14GetPlatformChecker)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonEx.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonExS14GetPlatformChecker1)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonEx.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonExS14GetPlatformChecker2)));
            if ((temp189 == 0)) {
                this.Manager.Comment("reaching state \'S147\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp180;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp180 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S336\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp180, "return of NetrServerReqChallenge, state S336");
                this.Manager.Comment("reaching state \'S525\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp181;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp181 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S714\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp181, "return of NetrServerAuthenticate3, state S714");
                this.Manager.Comment("reaching state \'S903\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp182;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonEx(True,PrimaryDc,Client,NetlogonInteractiv" +
                        "eInformation,Valid,NetlogonValidationSamInfo4,0)\'");
                temp182 = this.INrpcServerAdapterInstance.NetrLogonSamLogonEx(true, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo4, 0u);
                this.Manager.Checkpoint("MS-NRPC_R1174");
                this.Manager.Checkpoint("MS-NRPC_R1160");
                this.Manager.Comment("reaching state \'S1092\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonEx/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp182, "return of NetrLogonSamLogonEx, state S1092");
                this.Manager.Comment("reaching state \'S1281\'");
                goto label16;
            }
            if ((temp189 == 1)) {
                this.Manager.Comment("reaching state \'S148\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp183;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp183 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S337\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp183, "return of NetrServerReqChallenge, state S337");
                this.Manager.Comment("reaching state \'S526\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp184;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp184 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S715\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp184, "return of NetrServerAuthenticate3, state S715");
                this.Manager.Comment("reaching state \'S904\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp185;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonEx(True,PrimaryDc,Client,NetlogonInteractiv" +
                        "eInformation,Valid,NetlogonValidationSamInfo4,0)\'");
                temp185 = this.INrpcServerAdapterInstance.NetrLogonSamLogonEx(true, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo4, 0u);
                this.Manager.Checkpoint("MS-NRPC_R1174");
                this.Manager.Checkpoint("MS-NRPC_R1160");
                this.Manager.Comment("reaching state \'S1093\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonEx/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp185, "return of NetrLogonSamLogonEx, state S1093");
                this.Manager.Comment("reaching state \'S1282\'");
                goto label16;
            }
            if ((temp189 == 2)) {
                this.Manager.Comment("reaching state \'S149\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp186;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp186 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S338\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp186, "return of NetrServerReqChallenge, state S338");
                this.Manager.Comment("reaching state \'S527\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp187;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp187 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S716\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp187, "return of NetrServerAuthenticate3, state S716");
                this.Manager.Comment("reaching state \'S905\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp188;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonEx(True,PrimaryDc,Client,NetlogonInteractiv" +
                        "eInformation,Valid,NetlogonValidationSamInfo,0)\'");
                temp188 = this.INrpcServerAdapterInstance.NetrLogonSamLogonEx(true, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo, 0u);
                this.Manager.Checkpoint("MS-NRPC_R1174");
                this.Manager.Checkpoint("MS-NRPC_R1160");
                this.Manager.Comment("reaching state \'S1094\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonEx/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp188, "return of NetrLogonSamLogonEx, state S1094");
                this.Manager.Comment("reaching state \'S1283\'");
                goto label16;
            }
            throw new InvalidOperationException("never reached");
        label16:
;
            this.Manager.EndTest();
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonExS14GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S15");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonExS14GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S15");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonExS14GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S15");
        }
        #endregion
        
        #region Test Starting in S16
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_PassThroughAuthenticate_NetrLogonSamLogonExS16() {
            this.Manager.BeginTest("Test_PassThroughAuthenticate_NetrLogonSamLogonExS16");
            this.Manager.Comment("reaching state \'S16\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp190;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp190);
            this.Manager.AddReturn(GetPlatformInfo, null, temp190);
            this.Manager.Comment("reaching state \'S17\'");
            int temp200 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonEx.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonExS16GetPlatformChecker)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonEx.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonExS16GetPlatformChecker1)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonEx.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonExS16GetPlatformChecker2)));
            if ((temp200 == 0)) {
                this.Manager.Comment("reaching state \'S150\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp191;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp191 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S339\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp191, "return of NetrServerReqChallenge, state S339");
                this.Manager.Comment("reaching state \'S528\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp192;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp192 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S717\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp192, "return of NetrServerAuthenticate3, state S717");
                this.Manager.Comment("reaching state \'S906\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp193;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonEx(True,PrimaryDc,Client,NetlogonInteractiv" +
                        "eTransitiveInformation,Valid,NetlogonValidationSamInfo,0)\'");
                temp193 = this.INrpcServerAdapterInstance.NetrLogonSamLogonEx(true, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonInteractiveTransitiveInformation, ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo, 0u);
                this.Manager.Checkpoint("MS-NRPC_R1175");
                this.Manager.Checkpoint("MS-NRPC_R1160");
                this.Manager.Comment("reaching state \'S1095\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonEx/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp193, "return of NetrLogonSamLogonEx, state S1095");
                this.Manager.Comment("reaching state \'S1284\'");
                goto label17;
            }
            if ((temp200 == 1)) {
                this.Manager.Comment("reaching state \'S151\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp194;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp194 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S340\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp194, "return of NetrServerReqChallenge, state S340");
                this.Manager.Comment("reaching state \'S529\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp195;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp195 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S718\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp195, "return of NetrServerAuthenticate3, state S718");
                this.Manager.Comment("reaching state \'S907\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp196;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonEx(True,PrimaryDc,Client,NetlogonInteractiv" +
                        "eTransitiveInformation,Valid,NetlogonValidationSamInfo,0)\'");
                temp196 = this.INrpcServerAdapterInstance.NetrLogonSamLogonEx(true, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonInteractiveTransitiveInformation, ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo, 0u);
                this.Manager.Checkpoint("MS-NRPC_R1175");
                this.Manager.Checkpoint("MS-NRPC_R1160");
                this.Manager.Comment("reaching state \'S1096\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonEx/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp196, "return of NetrLogonSamLogonEx, state S1096");
                this.Manager.Comment("reaching state \'S1285\'");
                goto label17;
            }
            if ((temp200 == 2)) {
                this.Manager.Comment("reaching state \'S152\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp197;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp197 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S341\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp197, "return of NetrServerReqChallenge, state S341");
                this.Manager.Comment("reaching state \'S530\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp198;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp198 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S719\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp198, "return of NetrServerAuthenticate3, state S719");
                this.Manager.Comment("reaching state \'S908\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp199;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonEx(True,PrimaryDc,Client,NetlogonInteractiv" +
                        "eInformation,Valid,NetlogonValidationSamInfo,0)\'");
                temp199 = this.INrpcServerAdapterInstance.NetrLogonSamLogonEx(true, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo, 0u);
                this.Manager.Checkpoint("MS-NRPC_R1174");
                this.Manager.Checkpoint("MS-NRPC_R1160");
                this.Manager.Comment("reaching state \'S1097\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonEx/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp199, "return of NetrLogonSamLogonEx, state S1097");
                this.Manager.Comment("reaching state \'S1286\'");
                goto label17;
            }
            throw new InvalidOperationException("never reached");
        label17:
;
            this.Manager.EndTest();
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonExS16GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S17");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonExS16GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S17");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonExS16GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S17");
        }
        #endregion
        
        #region Test Starting in S18
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_PassThroughAuthenticate_NetrLogonSamLogonExS18() {
            this.Manager.BeginTest("Test_PassThroughAuthenticate_NetrLogonSamLogonExS18");
            this.Manager.Comment("reaching state \'S18\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp201;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp201);
            this.Manager.AddReturn(GetPlatformInfo, null, temp201);
            this.Manager.Comment("reaching state \'S19\'");
            int temp211 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonEx.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonExS18GetPlatformChecker)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonEx.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonExS18GetPlatformChecker1)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonEx.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonExS18GetPlatformChecker2)));
            if ((temp211 == 0)) {
                this.Manager.Comment("reaching state \'S153\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp202;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp202 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S342\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp202, "return of NetrServerReqChallenge, state S342");
                this.Manager.Comment("reaching state \'S531\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp203;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp203 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S720\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp203, "return of NetrServerAuthenticate3, state S720");
                this.Manager.Comment("reaching state \'S909\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp204;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonEx(True,PrimaryDc,Client,NetlogonServiceInf" +
                        "ormation,Valid,NetlogonValidationSamInfo2,0)\'");
                temp204 = this.INrpcServerAdapterInstance.NetrLogonSamLogonEx(true, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonServiceInformation, ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo2, 0u);
                this.Manager.Checkpoint("MS-NRPC_R1177");
                this.Manager.Checkpoint("MS-NRPC_R1160");
                this.Manager.Comment("reaching state \'S1098\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonEx/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp204, "return of NetrLogonSamLogonEx, state S1098");
                this.Manager.Comment("reaching state \'S1287\'");
                goto label18;
            }
            if ((temp211 == 1)) {
                this.Manager.Comment("reaching state \'S154\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp205;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp205 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S343\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp205, "return of NetrServerReqChallenge, state S343");
                this.Manager.Comment("reaching state \'S532\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp206;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp206 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S721\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp206, "return of NetrServerAuthenticate3, state S721");
                this.Manager.Comment("reaching state \'S910\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp207;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonEx(True,PrimaryDc,Client,NetlogonServiceInf" +
                        "ormation,Valid,NetlogonValidationSamInfo2,0)\'");
                temp207 = this.INrpcServerAdapterInstance.NetrLogonSamLogonEx(true, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonServiceInformation, ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo2, 0u);
                this.Manager.Checkpoint("MS-NRPC_R1177");
                this.Manager.Checkpoint("MS-NRPC_R1160");
                this.Manager.Comment("reaching state \'S1099\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonEx/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp207, "return of NetrLogonSamLogonEx, state S1099");
                this.Manager.Comment("reaching state \'S1288\'");
                goto label18;
            }
            if ((temp211 == 2)) {
                this.Manager.Comment("reaching state \'S155\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp208;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp208 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S344\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp208, "return of NetrServerReqChallenge, state S344");
                this.Manager.Comment("reaching state \'S533\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp209;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp209 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S722\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp209, "return of NetrServerAuthenticate3, state S722");
                this.Manager.Comment("reaching state \'S911\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp210;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonEx(True,PrimaryDc,Client,NetlogonInteractiv" +
                        "eInformation,Valid,NetlogonValidationSamInfo,0)\'");
                temp210 = this.INrpcServerAdapterInstance.NetrLogonSamLogonEx(true, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo, 0u);
                this.Manager.Checkpoint("MS-NRPC_R1174");
                this.Manager.Checkpoint("MS-NRPC_R1160");
                this.Manager.Comment("reaching state \'S1100\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonEx/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp210, "return of NetrLogonSamLogonEx, state S1100");
                this.Manager.Comment("reaching state \'S1289\'");
                goto label18;
            }
            throw new InvalidOperationException("never reached");
        label18:
;
            this.Manager.EndTest();
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonExS18GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S19");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonExS18GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S19");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonExS18GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S19");
        }
        #endregion
        
        #region Test Starting in S2
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_PassThroughAuthenticate_NetrLogonSamLogonExS2() {
            this.Manager.BeginTest("Test_PassThroughAuthenticate_NetrLogonSamLogonExS2");
            this.Manager.Comment("reaching state \'S2\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp212;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp212);
            this.Manager.AddReturn(GetPlatformInfo, null, temp212);
            this.Manager.Comment("reaching state \'S3\'");
            int temp222 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonEx.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonExS2GetPlatformChecker)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonEx.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonExS2GetPlatformChecker1)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonEx.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonExS2GetPlatformChecker2)));
            if ((temp222 == 0)) {
                this.Manager.Comment("reaching state \'S129\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp213;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp213 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S318\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp213, "return of NetrServerReqChallenge, state S318");
                this.Manager.Comment("reaching state \'S507\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp214;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp214 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S696\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp214, "return of NetrServerAuthenticate3, state S696");
                this.Manager.Comment("reaching state \'S885\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp215;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonEx(True,PrimaryDc,Client,NetlogonNetworkInf" +
                        "ormation,Valid,NetlogonValidationSamInfo,0)\'");
                temp215 = this.INrpcServerAdapterInstance.NetrLogonSamLogonEx(true, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonNetworkInformation, ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo, 0u);
                this.Manager.Checkpoint("MS-NRPC_R1160");
                this.Manager.Comment("reaching state \'S1074\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonEx/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp215, "return of NetrLogonSamLogonEx, state S1074");
                this.Manager.Comment("reaching state \'S1263\'");
                goto label19;
            }
            if ((temp222 == 1)) {
                this.Manager.Comment("reaching state \'S130\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp216;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp216 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S319\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp216, "return of NetrServerReqChallenge, state S319");
                this.Manager.Comment("reaching state \'S508\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp217;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp217 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S697\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp217, "return of NetrServerAuthenticate3, state S697");
                this.Manager.Comment("reaching state \'S886\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp218;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonEx(True,PrimaryDc,Client,NetlogonInteractiv" +
                        "eInformation,Valid,NetlogonValidationSamInfo,0)\'");
                temp218 = this.INrpcServerAdapterInstance.NetrLogonSamLogonEx(true, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo, 0u);
                this.Manager.Checkpoint("MS-NRPC_R1174");
                this.Manager.Checkpoint("MS-NRPC_R1160");
                this.Manager.Comment("reaching state \'S1075\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonEx/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp218, "return of NetrLogonSamLogonEx, state S1075");
                this.Manager.Comment("reaching state \'S1264\'");
                goto label19;
            }
            if ((temp222 == 2)) {
                this.Manager.Comment("reaching state \'S131\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp219;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp219 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S320\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp219, "return of NetrServerReqChallenge, state S320");
                this.Manager.Comment("reaching state \'S509\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp220;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp220 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S698\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp220, "return of NetrServerAuthenticate3, state S698");
                this.Manager.Comment("reaching state \'S887\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp221;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonEx(True,PrimaryDc,Client,NetlogonInteractiv" +
                        "eInformation,Valid,NetlogonValidationSamInfo,0)\'");
                temp221 = this.INrpcServerAdapterInstance.NetrLogonSamLogonEx(true, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo, 0u);
                this.Manager.Checkpoint("MS-NRPC_R1174");
                this.Manager.Checkpoint("MS-NRPC_R1160");
                this.Manager.Comment("reaching state \'S1076\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonEx/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp221, "return of NetrLogonSamLogonEx, state S1076");
                this.Manager.Comment("reaching state \'S1265\'");
                goto label19;
            }
            throw new InvalidOperationException("never reached");
        label19:
;
            this.Manager.EndTest();
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonExS2GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S3");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonExS2GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S3");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonExS2GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S3");
        }
        #endregion
        
        #region Test Starting in S20
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_PassThroughAuthenticate_NetrLogonSamLogonExS20() {
            this.Manager.BeginTest("Test_PassThroughAuthenticate_NetrLogonSamLogonExS20");
            this.Manager.Comment("reaching state \'S20\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp223;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp223);
            this.Manager.AddReturn(GetPlatformInfo, null, temp223);
            this.Manager.Comment("reaching state \'S21\'");
            int temp233 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonEx.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonExS20GetPlatformChecker)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonEx.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonExS20GetPlatformChecker1)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonEx.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonExS20GetPlatformChecker2)));
            if ((temp233 == 0)) {
                this.Manager.Comment("reaching state \'S156\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp224;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp224 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S345\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp224, "return of NetrServerReqChallenge, state S345");
                this.Manager.Comment("reaching state \'S534\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp225;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp225 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S723\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp225, "return of NetrServerAuthenticate3, state S723");
                this.Manager.Comment("reaching state \'S912\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp226;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonEx(True,PrimaryDc,Client,NetlogonNetworkInf" +
                        "ormation,Valid,NetlogonValidationGenericInfo2,0)\'");
                temp226 = this.INrpcServerAdapterInstance.NetrLogonSamLogonEx(true, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonNetworkInformation, ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationGenericInfo2, 0u);
                this.Manager.Checkpoint("MS-NRPC_R101196");
                this.Manager.Checkpoint("MS-NRPC_R1161");
                this.Manager.Comment("reaching state \'S1101\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonEx/STATUS_INVALID_INFO_CLASS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_INVALID_INFO_CLASS, temp226, "return of NetrLogonSamLogonEx, state S1101");
                this.Manager.Comment("reaching state \'S1290\'");
                goto label20;
            }
            if ((temp233 == 1)) {
                this.Manager.Comment("reaching state \'S157\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp227;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp227 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S346\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp227, "return of NetrServerReqChallenge, state S346");
                this.Manager.Comment("reaching state \'S535\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp228;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp228 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S724\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp228, "return of NetrServerAuthenticate3, state S724");
                this.Manager.Comment("reaching state \'S913\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp229;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonEx(True,PrimaryDc,Client,NetlogonNetworkInf" +
                        "ormation,Valid,NetlogonValidationGenericInfo2,0)\'");
                temp229 = this.INrpcServerAdapterInstance.NetrLogonSamLogonEx(true, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonNetworkInformation, ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationGenericInfo2, 0u);
                this.Manager.Checkpoint("MS-NRPC_R101196");
                this.Manager.Checkpoint("MS-NRPC_R1161");
                this.Manager.Comment("reaching state \'S1102\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonEx/STATUS_INVALID_INFO_CLASS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_INVALID_INFO_CLASS, temp229, "return of NetrLogonSamLogonEx, state S1102");
                this.Manager.Comment("reaching state \'S1291\'");
                goto label20;
            }
            if ((temp233 == 2)) {
                this.Manager.Comment("reaching state \'S158\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp230;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp230 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S347\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp230, "return of NetrServerReqChallenge, state S347");
                this.Manager.Comment("reaching state \'S536\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp231;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp231 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S725\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp231, "return of NetrServerAuthenticate3, state S725");
                this.Manager.Comment("reaching state \'S914\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp232;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonEx(True,PrimaryDc,Client,NetlogonInteractiv" +
                        "eInformation,Valid,NetlogonValidationSamInfo,0)\'");
                temp232 = this.INrpcServerAdapterInstance.NetrLogonSamLogonEx(true, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo, 0u);
                this.Manager.Checkpoint("MS-NRPC_R1174");
                this.Manager.Checkpoint("MS-NRPC_R1160");
                this.Manager.Comment("reaching state \'S1103\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonEx/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp232, "return of NetrLogonSamLogonEx, state S1103");
                this.Manager.Comment("reaching state \'S1292\'");
                goto label20;
            }
            throw new InvalidOperationException("never reached");
        label20:
;
            this.Manager.EndTest();
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonExS20GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S21");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonExS20GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S21");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonExS20GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S21");
        }
        #endregion
        
        #region Test Starting in S22
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_PassThroughAuthenticate_NetrLogonSamLogonExS22() {
            this.Manager.BeginTest("Test_PassThroughAuthenticate_NetrLogonSamLogonExS22");
            this.Manager.Comment("reaching state \'S22\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp234;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp234);
            this.Manager.AddReturn(GetPlatformInfo, null, temp234);
            this.Manager.Comment("reaching state \'S23\'");
            int temp244 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonEx.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonExS22GetPlatformChecker)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonEx.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonExS22GetPlatformChecker1)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonEx.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonExS22GetPlatformChecker2)));
            if ((temp244 == 0)) {
                this.Manager.Comment("reaching state \'S159\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp235;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp235 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S348\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp235, "return of NetrServerReqChallenge, state S348");
                this.Manager.Comment("reaching state \'S537\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp236;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp236 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S726\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp236, "return of NetrServerAuthenticate3, state S726");
                this.Manager.Comment("reaching state \'S915\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp237;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonEx(True,PrimaryDc,Client,NetlogonNetworkInf" +
                        "ormation,Valid,NetlogonValidationSamInfo4,0)\'");
                temp237 = this.INrpcServerAdapterInstance.NetrLogonSamLogonEx(true, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonNetworkInformation, ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo4, 0u);
                this.Manager.Checkpoint("MS-NRPC_R1160");
                this.Manager.Comment("reaching state \'S1104\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonEx/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp237, "return of NetrLogonSamLogonEx, state S1104");
                this.Manager.Comment("reaching state \'S1293\'");
                goto label21;
            }
            if ((temp244 == 1)) {
                this.Manager.Comment("reaching state \'S160\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp238;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp238 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S349\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp238, "return of NetrServerReqChallenge, state S349");
                this.Manager.Comment("reaching state \'S538\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp239;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp239 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S727\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp239, "return of NetrServerAuthenticate3, state S727");
                this.Manager.Comment("reaching state \'S916\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp240;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonEx(True,PrimaryDc,Client,NetlogonNetworkInf" +
                        "ormation,Valid,NetlogonValidationSamInfo4,0)\'");
                temp240 = this.INrpcServerAdapterInstance.NetrLogonSamLogonEx(true, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonNetworkInformation, ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo4, 0u);
                this.Manager.Checkpoint("MS-NRPC_R1160");
                this.Manager.Comment("reaching state \'S1105\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonEx/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp240, "return of NetrLogonSamLogonEx, state S1105");
                this.Manager.Comment("reaching state \'S1294\'");
                goto label21;
            }
            if ((temp244 == 2)) {
                this.Manager.Comment("reaching state \'S161\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp241;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp241 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S350\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp241, "return of NetrServerReqChallenge, state S350");
                this.Manager.Comment("reaching state \'S539\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp242;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp242 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S728\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp242, "return of NetrServerAuthenticate3, state S728");
                this.Manager.Comment("reaching state \'S917\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp243;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonEx(True,PrimaryDc,Client,NetlogonInteractiv" +
                        "eInformation,Valid,NetlogonValidationSamInfo,0)\'");
                temp243 = this.INrpcServerAdapterInstance.NetrLogonSamLogonEx(true, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo, 0u);
                this.Manager.Checkpoint("MS-NRPC_R1174");
                this.Manager.Checkpoint("MS-NRPC_R1160");
                this.Manager.Comment("reaching state \'S1106\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonEx/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp243, "return of NetrLogonSamLogonEx, state S1106");
                this.Manager.Comment("reaching state \'S1295\'");
                goto label21;
            }
            throw new InvalidOperationException("never reached");
        label21:
;
            this.Manager.EndTest();
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonExS22GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S23");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonExS22GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S23");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonExS22GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S23");
        }
        #endregion
        
        #region Test Starting in S24
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_PassThroughAuthenticate_NetrLogonSamLogonExS24() {
            this.Manager.BeginTest("Test_PassThroughAuthenticate_NetrLogonSamLogonExS24");
            this.Manager.Comment("reaching state \'S24\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp245;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp245);
            this.Manager.AddReturn(GetPlatformInfo, null, temp245);
            this.Manager.Comment("reaching state \'S25\'");
            int temp255 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonEx.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonExS24GetPlatformChecker)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonEx.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonExS24GetPlatformChecker1)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonEx.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonExS24GetPlatformChecker2)));
            if ((temp255 == 0)) {
                this.Manager.Comment("reaching state \'S162\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp246;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp246 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S351\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp246, "return of NetrServerReqChallenge, state S351");
                this.Manager.Comment("reaching state \'S540\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp247;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp247 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S729\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp247, "return of NetrServerAuthenticate3, state S729");
                this.Manager.Comment("reaching state \'S918\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp248;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonEx(True,PrimaryDc,Client,NetlogonServiceTra" +
                        "nsitiveInformation,Valid,NetlogonValidationSamInfo,0)\'");
                temp248 = this.INrpcServerAdapterInstance.NetrLogonSamLogonEx(true, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonServiceTransitiveInformation, ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo, 0u);
                this.Manager.Checkpoint("MS-NRPC_R101177");
                this.Manager.Checkpoint("MS-NRPC_R1160");
                this.Manager.Comment("reaching state \'S1107\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonEx/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp248, "return of NetrLogonSamLogonEx, state S1107");
                this.Manager.Comment("reaching state \'S1296\'");
                goto label22;
            }
            if ((temp255 == 1)) {
                this.Manager.Comment("reaching state \'S163\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp249;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp249 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S352\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp249, "return of NetrServerReqChallenge, state S352");
                this.Manager.Comment("reaching state \'S541\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp250;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp250 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S730\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp250, "return of NetrServerAuthenticate3, state S730");
                this.Manager.Comment("reaching state \'S919\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp251;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonEx(True,PrimaryDc,Client,NetlogonServiceTra" +
                        "nsitiveInformation,Valid,NetlogonValidationSamInfo,0)\'");
                temp251 = this.INrpcServerAdapterInstance.NetrLogonSamLogonEx(true, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonServiceTransitiveInformation, ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo, 0u);
                this.Manager.Checkpoint("MS-NRPC_R101177");
                this.Manager.Checkpoint("MS-NRPC_R1160");
                this.Manager.Comment("reaching state \'S1108\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonEx/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp251, "return of NetrLogonSamLogonEx, state S1108");
                this.Manager.Comment("reaching state \'S1297\'");
                goto label22;
            }
            if ((temp255 == 2)) {
                this.Manager.Comment("reaching state \'S164\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp252;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp252 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S353\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp252, "return of NetrServerReqChallenge, state S353");
                this.Manager.Comment("reaching state \'S542\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp253;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp253 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S731\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp253, "return of NetrServerAuthenticate3, state S731");
                this.Manager.Comment("reaching state \'S920\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp254;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonEx(True,PrimaryDc,Client,NetlogonInteractiv" +
                        "eInformation,Valid,NetlogonValidationSamInfo,0)\'");
                temp254 = this.INrpcServerAdapterInstance.NetrLogonSamLogonEx(true, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo, 0u);
                this.Manager.Checkpoint("MS-NRPC_R1174");
                this.Manager.Checkpoint("MS-NRPC_R1160");
                this.Manager.Comment("reaching state \'S1109\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonEx/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp254, "return of NetrLogonSamLogonEx, state S1109");
                this.Manager.Comment("reaching state \'S1298\'");
                goto label22;
            }
            throw new InvalidOperationException("never reached");
        label22:
;
            this.Manager.EndTest();
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonExS24GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S25");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonExS24GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S25");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonExS24GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S25");
        }
        #endregion
        
        #region Test Starting in S26
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_PassThroughAuthenticate_NetrLogonSamLogonExS26() {
            this.Manager.BeginTest("Test_PassThroughAuthenticate_NetrLogonSamLogonExS26");
            this.Manager.Comment("reaching state \'S26\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp256;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp256);
            this.Manager.AddReturn(GetPlatformInfo, null, temp256);
            this.Manager.Comment("reaching state \'S27\'");
            int temp266 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonEx.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonExS26GetPlatformChecker)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonEx.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonExS26GetPlatformChecker1)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonEx.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonExS26GetPlatformChecker2)));
            if ((temp266 == 0)) {
                this.Manager.Comment("reaching state \'S165\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp257;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp257 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S354\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp257, "return of NetrServerReqChallenge, state S354");
                this.Manager.Comment("reaching state \'S543\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp258;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp258 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S732\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp258, "return of NetrServerAuthenticate3, state S732");
                this.Manager.Comment("reaching state \'S921\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp259;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonEx(True,PrimaryDc,Client,NetlogonNetworkTra" +
                        "nsitiveInformation,Valid,NetlogonValidationSamInfo2,0)\'");
                temp259 = this.INrpcServerAdapterInstance.NetrLogonSamLogonEx(true, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonNetworkTransitiveInformation, ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo2, 0u);
                this.Manager.Checkpoint("MS-NRPC_R1160");
                this.Manager.Comment("reaching state \'S1110\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonEx/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp259, "return of NetrLogonSamLogonEx, state S1110");
                this.Manager.Comment("reaching state \'S1299\'");
                goto label23;
            }
            if ((temp266 == 1)) {
                this.Manager.Comment("reaching state \'S166\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp260;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp260 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S355\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp260, "return of NetrServerReqChallenge, state S355");
                this.Manager.Comment("reaching state \'S544\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp261;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp261 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S733\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp261, "return of NetrServerAuthenticate3, state S733");
                this.Manager.Comment("reaching state \'S922\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp262;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonEx(True,PrimaryDc,Client,NetlogonNetworkTra" +
                        "nsitiveInformation,Valid,NetlogonValidationSamInfo2,0)\'");
                temp262 = this.INrpcServerAdapterInstance.NetrLogonSamLogonEx(true, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonNetworkTransitiveInformation, ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo2, 0u);
                this.Manager.Checkpoint("MS-NRPC_R1160");
                this.Manager.Comment("reaching state \'S1111\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonEx/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp262, "return of NetrLogonSamLogonEx, state S1111");
                this.Manager.Comment("reaching state \'S1300\'");
                goto label23;
            }
            if ((temp266 == 2)) {
                this.Manager.Comment("reaching state \'S167\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp263;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp263 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S356\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp263, "return of NetrServerReqChallenge, state S356");
                this.Manager.Comment("reaching state \'S545\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp264;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp264 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S734\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp264, "return of NetrServerAuthenticate3, state S734");
                this.Manager.Comment("reaching state \'S923\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp265;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonEx(True,PrimaryDc,Client,NetlogonInteractiv" +
                        "eInformation,Valid,NetlogonValidationSamInfo,0)\'");
                temp265 = this.INrpcServerAdapterInstance.NetrLogonSamLogonEx(true, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo, 0u);
                this.Manager.Checkpoint("MS-NRPC_R1174");
                this.Manager.Checkpoint("MS-NRPC_R1160");
                this.Manager.Comment("reaching state \'S1112\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonEx/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp265, "return of NetrLogonSamLogonEx, state S1112");
                this.Manager.Comment("reaching state \'S1301\'");
                goto label23;
            }
            throw new InvalidOperationException("never reached");
        label23:
;
            this.Manager.EndTest();
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonExS26GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S27");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonExS26GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S27");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonExS26GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S27");
        }
        #endregion
        
        #region Test Starting in S28
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_PassThroughAuthenticate_NetrLogonSamLogonExS28() {
            this.Manager.BeginTest("Test_PassThroughAuthenticate_NetrLogonSamLogonExS28");
            this.Manager.Comment("reaching state \'S28\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp267;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp267);
            this.Manager.AddReturn(GetPlatformInfo, null, temp267);
            this.Manager.Comment("reaching state \'S29\'");
            int temp277 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonEx.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonExS28GetPlatformChecker)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonEx.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonExS28GetPlatformChecker1)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonEx.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonExS28GetPlatformChecker2)));
            if ((temp277 == 0)) {
                this.Manager.Comment("reaching state \'S168\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp268;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp268 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S357\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp268, "return of NetrServerReqChallenge, state S357");
                this.Manager.Comment("reaching state \'S546\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp269;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp269 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S735\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp269, "return of NetrServerAuthenticate3, state S735");
                this.Manager.Comment("reaching state \'S924\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp270;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonEx(False,PrimaryDc,Client,NetlogonNetworkTr" +
                        "ansitiveInformation,Valid,NetlogonValidationSamInfo2,0)\'");
                temp270 = this.INrpcServerAdapterInstance.NetrLogonSamLogonEx(false, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonNetworkTransitiveInformation, ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo2, 0u);
                this.Manager.Checkpoint("MS-NRPC_R1167");
                this.Manager.Checkpoint("MS-NRPC_R1161");
                this.Manager.Comment("reaching state \'S1113\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonEx/STATUS_ACCESS_DENIED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp270, "return of NetrLogonSamLogonEx, state S1113");
                this.Manager.Comment("reaching state \'S1302\'");
                goto label24;
            }
            if ((temp277 == 1)) {
                this.Manager.Comment("reaching state \'S169\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp271;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp271 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S358\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp271, "return of NetrServerReqChallenge, state S358");
                this.Manager.Comment("reaching state \'S547\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp272;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp272 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S736\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp272, "return of NetrServerAuthenticate3, state S736");
                this.Manager.Comment("reaching state \'S925\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp273;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonEx(False,PrimaryDc,Client,NetlogonNetworkTr" +
                        "ansitiveInformation,Valid,NetlogonValidationSamInfo2,0)\'");
                temp273 = this.INrpcServerAdapterInstance.NetrLogonSamLogonEx(false, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonNetworkTransitiveInformation, ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo2, 0u);
                this.Manager.Checkpoint("MS-NRPC_R1167");
                this.Manager.Checkpoint("MS-NRPC_R1161");
                this.Manager.Comment("reaching state \'S1114\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonEx/STATUS_ACCESS_DENIED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp273, "return of NetrLogonSamLogonEx, state S1114");
                this.Manager.Comment("reaching state \'S1303\'");
                goto label24;
            }
            if ((temp277 == 2)) {
                this.Manager.Comment("reaching state \'S170\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp274;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp274 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S359\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp274, "return of NetrServerReqChallenge, state S359");
                this.Manager.Comment("reaching state \'S548\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp275;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp275 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S737\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp275, "return of NetrServerAuthenticate3, state S737");
                this.Manager.Comment("reaching state \'S926\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp276;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonEx(True,PrimaryDc,Client,NetlogonInteractiv" +
                        "eInformation,Valid,NetlogonValidationSamInfo,0)\'");
                temp276 = this.INrpcServerAdapterInstance.NetrLogonSamLogonEx(true, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo, 0u);
                this.Manager.Checkpoint("MS-NRPC_R1174");
                this.Manager.Checkpoint("MS-NRPC_R1160");
                this.Manager.Comment("reaching state \'S1115\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonEx/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp276, "return of NetrLogonSamLogonEx, state S1115");
                this.Manager.Comment("reaching state \'S1304\'");
                goto label24;
            }
            throw new InvalidOperationException("never reached");
        label24:
;
            this.Manager.EndTest();
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonExS28GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S29");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonExS28GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S29");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonExS28GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S29");
        }
        #endregion
        
        #region Test Starting in S30
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_PassThroughAuthenticate_NetrLogonSamLogonExS30() {
            this.Manager.BeginTest("Test_PassThroughAuthenticate_NetrLogonSamLogonExS30");
            this.Manager.Comment("reaching state \'S30\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp278;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp278);
            this.Manager.AddReturn(GetPlatformInfo, null, temp278);
            this.Manager.Comment("reaching state \'S31\'");
            int temp288 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonEx.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonExS30GetPlatformChecker)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonEx.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonExS30GetPlatformChecker1)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonEx.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonExS30GetPlatformChecker2)));
            if ((temp288 == 0)) {
                this.Manager.Comment("reaching state \'S171\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp279;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp279 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S360\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp279, "return of NetrServerReqChallenge, state S360");
                this.Manager.Comment("reaching state \'S549\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp280;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp280 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S738\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp280, "return of NetrServerAuthenticate3, state S738");
                this.Manager.Comment("reaching state \'S927\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp281;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonEx(True,PrimaryDc,Client,NetlogonNetworkTra" +
                        "nsitiveInformation,Valid,NetlogonValidationSamInfo,0)\'");
                temp281 = this.INrpcServerAdapterInstance.NetrLogonSamLogonEx(true, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonNetworkTransitiveInformation, ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo, 0u);
                this.Manager.Checkpoint("MS-NRPC_R1160");
                this.Manager.Comment("reaching state \'S1116\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonEx/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp281, "return of NetrLogonSamLogonEx, state S1116");
                this.Manager.Comment("reaching state \'S1305\'");
                goto label25;
            }
            if ((temp288 == 1)) {
                this.Manager.Comment("reaching state \'S172\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp282;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp282 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S361\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp282, "return of NetrServerReqChallenge, state S361");
                this.Manager.Comment("reaching state \'S550\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp283;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp283 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S739\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp283, "return of NetrServerAuthenticate3, state S739");
                this.Manager.Comment("reaching state \'S928\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp284;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonEx(True,PrimaryDc,Client,NetlogonNetworkTra" +
                        "nsitiveInformation,Valid,NetlogonValidationSamInfo,0)\'");
                temp284 = this.INrpcServerAdapterInstance.NetrLogonSamLogonEx(true, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonNetworkTransitiveInformation, ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo, 0u);
                this.Manager.Checkpoint("MS-NRPC_R1160");
                this.Manager.Comment("reaching state \'S1117\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonEx/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp284, "return of NetrLogonSamLogonEx, state S1117");
                this.Manager.Comment("reaching state \'S1306\'");
                goto label25;
            }
            if ((temp288 == 2)) {
                this.Manager.Comment("reaching state \'S173\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp285;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp285 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S362\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp285, "return of NetrServerReqChallenge, state S362");
                this.Manager.Comment("reaching state \'S551\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp286;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp286 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S740\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp286, "return of NetrServerAuthenticate3, state S740");
                this.Manager.Comment("reaching state \'S929\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp287;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonEx(True,PrimaryDc,Client,NetlogonInteractiv" +
                        "eInformation,Valid,NetlogonValidationSamInfo,0)\'");
                temp287 = this.INrpcServerAdapterInstance.NetrLogonSamLogonEx(true, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo, 0u);
                this.Manager.Checkpoint("MS-NRPC_R1174");
                this.Manager.Checkpoint("MS-NRPC_R1160");
                this.Manager.Comment("reaching state \'S1118\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonEx/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp287, "return of NetrLogonSamLogonEx, state S1118");
                this.Manager.Comment("reaching state \'S1307\'");
                goto label25;
            }
            throw new InvalidOperationException("never reached");
        label25:
;
            this.Manager.EndTest();
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonExS30GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S31");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonExS30GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S31");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonExS30GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S31");
        }
        #endregion
        
        #region Test Starting in S32
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_PassThroughAuthenticate_NetrLogonSamLogonExS32() {
            this.Manager.BeginTest("Test_PassThroughAuthenticate_NetrLogonSamLogonExS32");
            this.Manager.Comment("reaching state \'S32\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp289;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp289);
            this.Manager.AddReturn(GetPlatformInfo, null, temp289);
            this.Manager.Comment("reaching state \'S33\'");
            int temp299 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonEx.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonExS32GetPlatformChecker)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonEx.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonExS32GetPlatformChecker1)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonEx.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonExS32GetPlatformChecker2)));
            if ((temp299 == 0)) {
                this.Manager.Comment("reaching state \'S174\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp290;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp290 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S363\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp290, "return of NetrServerReqChallenge, state S363");
                this.Manager.Comment("reaching state \'S552\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp291;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp291 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S741\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp291, "return of NetrServerAuthenticate3, state S741");
                this.Manager.Comment("reaching state \'S930\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp292;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonEx(True,PrimaryDc,Client,NetlogonInteractiv" +
                        "eTransitiveInformation,Valid,NetlogonValidationSamInfo2,0)\'");
                temp292 = this.INrpcServerAdapterInstance.NetrLogonSamLogonEx(true, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonInteractiveTransitiveInformation, ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo2, 0u);
                this.Manager.Checkpoint("MS-NRPC_R1175");
                this.Manager.Checkpoint("MS-NRPC_R1160");
                this.Manager.Comment("reaching state \'S1119\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonEx/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp292, "return of NetrLogonSamLogonEx, state S1119");
                this.Manager.Comment("reaching state \'S1308\'");
                goto label26;
            }
            if ((temp299 == 1)) {
                this.Manager.Comment("reaching state \'S175\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp293;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp293 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S364\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp293, "return of NetrServerReqChallenge, state S364");
                this.Manager.Comment("reaching state \'S553\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp294;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp294 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S742\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp294, "return of NetrServerAuthenticate3, state S742");
                this.Manager.Comment("reaching state \'S931\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp295;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonEx(True,PrimaryDc,Client,NetlogonInteractiv" +
                        "eTransitiveInformation,Valid,NetlogonValidationSamInfo2,0)\'");
                temp295 = this.INrpcServerAdapterInstance.NetrLogonSamLogonEx(true, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonInteractiveTransitiveInformation, ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo2, 0u);
                this.Manager.Checkpoint("MS-NRPC_R1175");
                this.Manager.Checkpoint("MS-NRPC_R1160");
                this.Manager.Comment("reaching state \'S1120\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonEx/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp295, "return of NetrLogonSamLogonEx, state S1120");
                this.Manager.Comment("reaching state \'S1309\'");
                goto label26;
            }
            if ((temp299 == 2)) {
                this.Manager.Comment("reaching state \'S176\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp296;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp296 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S365\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp296, "return of NetrServerReqChallenge, state S365");
                this.Manager.Comment("reaching state \'S554\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp297;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp297 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S743\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp297, "return of NetrServerAuthenticate3, state S743");
                this.Manager.Comment("reaching state \'S932\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp298;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonEx(True,PrimaryDc,Client,NetlogonInteractiv" +
                        "eInformation,Valid,NetlogonValidationSamInfo,0)\'");
                temp298 = this.INrpcServerAdapterInstance.NetrLogonSamLogonEx(true, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo, 0u);
                this.Manager.Checkpoint("MS-NRPC_R1174");
                this.Manager.Checkpoint("MS-NRPC_R1160");
                this.Manager.Comment("reaching state \'S1121\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonEx/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp298, "return of NetrLogonSamLogonEx, state S1121");
                this.Manager.Comment("reaching state \'S1310\'");
                goto label26;
            }
            throw new InvalidOperationException("never reached");
        label26:
;
            this.Manager.EndTest();
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonExS32GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S33");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonExS32GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S33");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonExS32GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S33");
        }
        #endregion
        
        #region Test Starting in S34
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_PassThroughAuthenticate_NetrLogonSamLogonExS34() {
            this.Manager.BeginTest("Test_PassThroughAuthenticate_NetrLogonSamLogonExS34");
            this.Manager.Comment("reaching state \'S34\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp300;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp300);
            this.Manager.AddReturn(GetPlatformInfo, null, temp300);
            this.Manager.Comment("reaching state \'S35\'");
            int temp310 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonEx.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonExS34GetPlatformChecker)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonEx.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonExS34GetPlatformChecker1)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonEx.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonExS34GetPlatformChecker2)));
            if ((temp310 == 0)) {
                this.Manager.Comment("reaching state \'S177\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp301;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp301 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S366\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp301, "return of NetrServerReqChallenge, state S366");
                this.Manager.Comment("reaching state \'S555\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp302;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp302 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S744\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp302, "return of NetrServerAuthenticate3, state S744");
                this.Manager.Comment("reaching state \'S933\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp303;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonEx(True,PrimaryDc,Client,NetlogonServiceInf" +
                        "ormation,Valid,NetlogonValidationGenericInfo2,0)\'");
                temp303 = this.INrpcServerAdapterInstance.NetrLogonSamLogonEx(true, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonServiceInformation, ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationGenericInfo2, 0u);
                this.Manager.Checkpoint("MS-NRPC_R1161");
                this.Manager.Comment("reaching state \'S1122\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonEx/STATUS_INVALID_INFO_CLASS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_INVALID_INFO_CLASS, temp303, "return of NetrLogonSamLogonEx, state S1122");
                this.Manager.Comment("reaching state \'S1311\'");
                goto label27;
            }
            if ((temp310 == 1)) {
                this.Manager.Comment("reaching state \'S178\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp304;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp304 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S367\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp304, "return of NetrServerReqChallenge, state S367");
                this.Manager.Comment("reaching state \'S556\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp305;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp305 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S745\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp305, "return of NetrServerAuthenticate3, state S745");
                this.Manager.Comment("reaching state \'S934\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp306;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonEx(True,PrimaryDc,Client,NetlogonServiceInf" +
                        "ormation,Valid,NetlogonValidationGenericInfo2,0)\'");
                temp306 = this.INrpcServerAdapterInstance.NetrLogonSamLogonEx(true, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonServiceInformation, ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationGenericInfo2, 0u);
                this.Manager.Checkpoint("MS-NRPC_R1161");
                this.Manager.Comment("reaching state \'S1123\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonEx/STATUS_INVALID_INFO_CLASS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_INVALID_INFO_CLASS, temp306, "return of NetrLogonSamLogonEx, state S1123");
                this.Manager.Comment("reaching state \'S1312\'");
                goto label27;
            }
            if ((temp310 == 2)) {
                this.Manager.Comment("reaching state \'S179\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp307;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp307 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S368\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp307, "return of NetrServerReqChallenge, state S368");
                this.Manager.Comment("reaching state \'S557\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp308;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp308 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S746\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp308, "return of NetrServerAuthenticate3, state S746");
                this.Manager.Comment("reaching state \'S935\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp309;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonEx(True,PrimaryDc,Client,NetlogonInteractiv" +
                        "eInformation,Valid,NetlogonValidationSamInfo,0)\'");
                temp309 = this.INrpcServerAdapterInstance.NetrLogonSamLogonEx(true, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo, 0u);
                this.Manager.Checkpoint("MS-NRPC_R1174");
                this.Manager.Checkpoint("MS-NRPC_R1160");
                this.Manager.Comment("reaching state \'S1124\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonEx/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp309, "return of NetrLogonSamLogonEx, state S1124");
                this.Manager.Comment("reaching state \'S1313\'");
                goto label27;
            }
            throw new InvalidOperationException("never reached");
        label27:
;
            this.Manager.EndTest();
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonExS34GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S35");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonExS34GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S35");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonExS34GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S35");
        }
        #endregion
        
        #region Test Starting in S36
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_PassThroughAuthenticate_NetrLogonSamLogonExS36() {
            this.Manager.BeginTest("Test_PassThroughAuthenticate_NetrLogonSamLogonExS36");
            this.Manager.Comment("reaching state \'S36\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp311;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp311);
            this.Manager.AddReturn(GetPlatformInfo, null, temp311);
            this.Manager.Comment("reaching state \'S37\'");
            int temp321 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonEx.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonExS36GetPlatformChecker)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonEx.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonExS36GetPlatformChecker1)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonEx.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonExS36GetPlatformChecker2)));
            if ((temp321 == 0)) {
                this.Manager.Comment("reaching state \'S180\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp312;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp312 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S369\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp312, "return of NetrServerReqChallenge, state S369");
                this.Manager.Comment("reaching state \'S558\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp313;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp313 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S747\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp313, "return of NetrServerAuthenticate3, state S747");
                this.Manager.Comment("reaching state \'S936\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp314;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonEx(True,PrimaryDc,Client,NetlogonServiceInf" +
                        "ormation,Valid,NetlogonValidationSamInfo4,0)\'");
                temp314 = this.INrpcServerAdapterInstance.NetrLogonSamLogonEx(true, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonServiceInformation, ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo4, 0u);
                this.Manager.Checkpoint("MS-NRPC_R1177");
                this.Manager.Checkpoint("MS-NRPC_R1160");
                this.Manager.Comment("reaching state \'S1125\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonEx/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp314, "return of NetrLogonSamLogonEx, state S1125");
                this.Manager.Comment("reaching state \'S1314\'");
                goto label28;
            }
            if ((temp321 == 1)) {
                this.Manager.Comment("reaching state \'S181\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp315;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp315 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S370\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp315, "return of NetrServerReqChallenge, state S370");
                this.Manager.Comment("reaching state \'S559\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp316;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp316 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S748\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp316, "return of NetrServerAuthenticate3, state S748");
                this.Manager.Comment("reaching state \'S937\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp317;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonEx(True,PrimaryDc,Client,NetlogonServiceInf" +
                        "ormation,Valid,NetlogonValidationSamInfo4,0)\'");
                temp317 = this.INrpcServerAdapterInstance.NetrLogonSamLogonEx(true, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonServiceInformation, ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo4, 0u);
                this.Manager.Checkpoint("MS-NRPC_R1177");
                this.Manager.Checkpoint("MS-NRPC_R1160");
                this.Manager.Comment("reaching state \'S1126\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonEx/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp317, "return of NetrLogonSamLogonEx, state S1126");
                this.Manager.Comment("reaching state \'S1315\'");
                goto label28;
            }
            if ((temp321 == 2)) {
                this.Manager.Comment("reaching state \'S182\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp318;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp318 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S371\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp318, "return of NetrServerReqChallenge, state S371");
                this.Manager.Comment("reaching state \'S560\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp319;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp319 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S749\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp319, "return of NetrServerAuthenticate3, state S749");
                this.Manager.Comment("reaching state \'S938\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp320;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonEx(True,PrimaryDc,Client,NetlogonInteractiv" +
                        "eInformation,Valid,NetlogonValidationSamInfo,0)\'");
                temp320 = this.INrpcServerAdapterInstance.NetrLogonSamLogonEx(true, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo, 0u);
                this.Manager.Checkpoint("MS-NRPC_R1174");
                this.Manager.Checkpoint("MS-NRPC_R1160");
                this.Manager.Comment("reaching state \'S1127\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonEx/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp320, "return of NetrLogonSamLogonEx, state S1127");
                this.Manager.Comment("reaching state \'S1316\'");
                goto label28;
            }
            throw new InvalidOperationException("never reached");
        label28:
;
            this.Manager.EndTest();
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonExS36GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S37");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonExS36GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S37");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonExS36GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S37");
        }
        #endregion
        
        #region Test Starting in S38
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_PassThroughAuthenticate_NetrLogonSamLogonExS38() {
            this.Manager.BeginTest("Test_PassThroughAuthenticate_NetrLogonSamLogonExS38");
            this.Manager.Comment("reaching state \'S38\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp322;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp322);
            this.Manager.AddReturn(GetPlatformInfo, null, temp322);
            this.Manager.Comment("reaching state \'S39\'");
            int temp332 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonEx.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonExS38GetPlatformChecker)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonEx.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonExS38GetPlatformChecker1)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonEx.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonExS38GetPlatformChecker2)));
            if ((temp332 == 0)) {
                this.Manager.Comment("reaching state \'S183\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp323;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp323 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S372\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp323, "return of NetrServerReqChallenge, state S372");
                this.Manager.Comment("reaching state \'S561\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp324;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp324 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S750\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp324, "return of NetrServerAuthenticate3, state S750");
                this.Manager.Comment("reaching state \'S939\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp325;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonEx(True,PrimaryDc,Client,NetlogonServiceTra" +
                        "nsitiveInformation,Valid,NetlogonValidationSamInfo2,0)\'");
                temp325 = this.INrpcServerAdapterInstance.NetrLogonSamLogonEx(true, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonServiceTransitiveInformation, ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo2, 0u);
                this.Manager.Checkpoint("MS-NRPC_R101177");
                this.Manager.Checkpoint("MS-NRPC_R1160");
                this.Manager.Comment("reaching state \'S1128\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonEx/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp325, "return of NetrLogonSamLogonEx, state S1128");
                this.Manager.Comment("reaching state \'S1317\'");
                goto label29;
            }
            if ((temp332 == 1)) {
                this.Manager.Comment("reaching state \'S184\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp326;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp326 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S373\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp326, "return of NetrServerReqChallenge, state S373");
                this.Manager.Comment("reaching state \'S562\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp327;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp327 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S751\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp327, "return of NetrServerAuthenticate3, state S751");
                this.Manager.Comment("reaching state \'S940\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp328;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonEx(True,PrimaryDc,Client,NetlogonServiceTra" +
                        "nsitiveInformation,Valid,NetlogonValidationSamInfo2,0)\'");
                temp328 = this.INrpcServerAdapterInstance.NetrLogonSamLogonEx(true, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonServiceTransitiveInformation, ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo2, 0u);
                this.Manager.Checkpoint("MS-NRPC_R101177");
                this.Manager.Checkpoint("MS-NRPC_R1160");
                this.Manager.Comment("reaching state \'S1129\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonEx/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp328, "return of NetrLogonSamLogonEx, state S1129");
                this.Manager.Comment("reaching state \'S1318\'");
                goto label29;
            }
            if ((temp332 == 2)) {
                this.Manager.Comment("reaching state \'S185\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp329;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp329 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S374\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp329, "return of NetrServerReqChallenge, state S374");
                this.Manager.Comment("reaching state \'S563\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp330;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp330 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S752\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp330, "return of NetrServerAuthenticate3, state S752");
                this.Manager.Comment("reaching state \'S941\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp331;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonEx(True,PrimaryDc,Client,NetlogonInteractiv" +
                        "eInformation,Valid,NetlogonValidationSamInfo,0)\'");
                temp331 = this.INrpcServerAdapterInstance.NetrLogonSamLogonEx(true, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo, 0u);
                this.Manager.Checkpoint("MS-NRPC_R1174");
                this.Manager.Checkpoint("MS-NRPC_R1160");
                this.Manager.Comment("reaching state \'S1130\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonEx/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp331, "return of NetrLogonSamLogonEx, state S1130");
                this.Manager.Comment("reaching state \'S1319\'");
                goto label29;
            }
            throw new InvalidOperationException("never reached");
        label29:
;
            this.Manager.EndTest();
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonExS38GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S39");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonExS38GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S39");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonExS38GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S39");
        }
        #endregion
        
        #region Test Starting in S4
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_PassThroughAuthenticate_NetrLogonSamLogonExS4() {
            this.Manager.BeginTest("Test_PassThroughAuthenticate_NetrLogonSamLogonExS4");
            this.Manager.Comment("reaching state \'S4\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp333;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp333);
            this.Manager.AddReturn(GetPlatformInfo, null, temp333);
            this.Manager.Comment("reaching state \'S5\'");
            int temp343 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonEx.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonExS4GetPlatformChecker)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonEx.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonExS4GetPlatformChecker1)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonEx.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonExS4GetPlatformChecker2)));
            if ((temp343 == 0)) {
                this.Manager.Comment("reaching state \'S132\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp334;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp334 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S321\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp334, "return of NetrServerReqChallenge, state S321");
                this.Manager.Comment("reaching state \'S510\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp335;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp335 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S699\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp335, "return of NetrServerAuthenticate3, state S699");
                this.Manager.Comment("reaching state \'S888\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp336;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonEx(True,PrimaryDc,Client,NetlogonInteractiv" +
                        "eInformation,Valid,NetlogonValidationSamInfo,0)\'");
                temp336 = this.INrpcServerAdapterInstance.NetrLogonSamLogonEx(true, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo, 0u);
                this.Manager.Checkpoint("MS-NRPC_R1174");
                this.Manager.Checkpoint("MS-NRPC_R1160");
                this.Manager.Comment("reaching state \'S1077\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonEx/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp336, "return of NetrLogonSamLogonEx, state S1077");
                this.Manager.Comment("reaching state \'S1266\'");
                goto label30;
            }
            if ((temp343 == 1)) {
                this.Manager.Comment("reaching state \'S133\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp337;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp337 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S322\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp337, "return of NetrServerReqChallenge, state S322");
                this.Manager.Comment("reaching state \'S511\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp338;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp338 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S700\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp338, "return of NetrServerAuthenticate3, state S700");
                this.Manager.Comment("reaching state \'S889\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp339;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonEx(True,PrimaryDc,Client,NetlogonNetworkInf" +
                        "ormation,Valid,NetlogonValidationSamInfo,0)\'");
                temp339 = this.INrpcServerAdapterInstance.NetrLogonSamLogonEx(true, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonNetworkInformation, ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo, 0u);
                this.Manager.Checkpoint("MS-NRPC_R1160");
                this.Manager.Comment("reaching state \'S1078\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonEx/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp339, "return of NetrLogonSamLogonEx, state S1078");
                this.Manager.Comment("reaching state \'S1267\'");
                goto label30;
            }
            if ((temp343 == 2)) {
                this.Manager.Comment("reaching state \'S134\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp340;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp340 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S323\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp340, "return of NetrServerReqChallenge, state S323");
                this.Manager.Comment("reaching state \'S512\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp341;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp341 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S701\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp341, "return of NetrServerAuthenticate3, state S701");
                this.Manager.Comment("reaching state \'S890\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp342;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonEx(True,PrimaryDc,Client,NetlogonInteractiv" +
                        "eInformation,Valid,NetlogonValidationSamInfo,0)\'");
                temp342 = this.INrpcServerAdapterInstance.NetrLogonSamLogonEx(true, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo, 0u);
                this.Manager.Checkpoint("MS-NRPC_R1174");
                this.Manager.Checkpoint("MS-NRPC_R1160");
                this.Manager.Comment("reaching state \'S1079\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonEx/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp342, "return of NetrLogonSamLogonEx, state S1079");
                this.Manager.Comment("reaching state \'S1268\'");
                goto label30;
            }
            throw new InvalidOperationException("never reached");
        label30:
;
            this.Manager.EndTest();
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonExS4GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S5");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonExS4GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S5");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonExS4GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S5");
        }
        #endregion
        
        #region Test Starting in S40
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_PassThroughAuthenticate_NetrLogonSamLogonExS40() {
            this.Manager.BeginTest("Test_PassThroughAuthenticate_NetrLogonSamLogonExS40");
            this.Manager.Comment("reaching state \'S40\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp344;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp344);
            this.Manager.AddReturn(GetPlatformInfo, null, temp344);
            this.Manager.Comment("reaching state \'S41\'");
            int temp354 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonEx.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonExS40GetPlatformChecker)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonEx.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonExS40GetPlatformChecker1)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonEx.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonExS40GetPlatformChecker2)));
            if ((temp354 == 0)) {
                this.Manager.Comment("reaching state \'S186\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp345;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp345 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S375\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp345, "return of NetrServerReqChallenge, state S375");
                this.Manager.Comment("reaching state \'S564\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp346;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp346 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S753\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp346, "return of NetrServerAuthenticate3, state S753");
                this.Manager.Comment("reaching state \'S942\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp347;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonEx(True,PrimaryDc,Client,NetlogonNetworkTra" +
                        "nsitiveInformation,Valid,NetlogonValidationSamInfo2,2)\'");
                temp347 = this.INrpcServerAdapterInstance.NetrLogonSamLogonEx(true, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonNetworkTransitiveInformation, ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo2, 2u);
                this.Manager.Checkpoint("MS-NRPC_R1169");
                this.Manager.Checkpoint("MS-NRPC_R1161");
                this.Manager.Comment("reaching state \'S1131\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonEx/STATUS_NO_SUCH_USER\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NO_SUCH_USER, temp347, "return of NetrLogonSamLogonEx, state S1131");
                this.Manager.Comment("reaching state \'S1320\'");
                goto label31;
            }
            if ((temp354 == 1)) {
                this.Manager.Comment("reaching state \'S187\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp348;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp348 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S376\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp348, "return of NetrServerReqChallenge, state S376");
                this.Manager.Comment("reaching state \'S565\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp349;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp349 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S754\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp349, "return of NetrServerAuthenticate3, state S754");
                this.Manager.Comment("reaching state \'S943\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp350;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonEx(True,PrimaryDc,Client,NetlogonNetworkTra" +
                        "nsitiveInformation,Valid,NetlogonValidationSamInfo2,2)\'");
                temp350 = this.INrpcServerAdapterInstance.NetrLogonSamLogonEx(true, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonNetworkTransitiveInformation, ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo2, 2u);
                this.Manager.Checkpoint("MS-NRPC_R1169");
                this.Manager.Checkpoint("MS-NRPC_R1161");
                this.Manager.Comment("reaching state \'S1132\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonEx/STATUS_NO_SUCH_USER\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NO_SUCH_USER, temp350, "return of NetrLogonSamLogonEx, state S1132");
                this.Manager.Comment("reaching state \'S1321\'");
                goto label31;
            }
            if ((temp354 == 2)) {
                this.Manager.Comment("reaching state \'S188\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp351;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp351 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S377\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp351, "return of NetrServerReqChallenge, state S377");
                this.Manager.Comment("reaching state \'S566\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp352;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp352 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S755\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp352, "return of NetrServerAuthenticate3, state S755");
                this.Manager.Comment("reaching state \'S944\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp353;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonEx(True,PrimaryDc,Client,NetlogonInteractiv" +
                        "eInformation,Valid,NetlogonValidationSamInfo,0)\'");
                temp353 = this.INrpcServerAdapterInstance.NetrLogonSamLogonEx(true, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo, 0u);
                this.Manager.Checkpoint("MS-NRPC_R1174");
                this.Manager.Checkpoint("MS-NRPC_R1160");
                this.Manager.Comment("reaching state \'S1133\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonEx/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp353, "return of NetrLogonSamLogonEx, state S1133");
                this.Manager.Comment("reaching state \'S1322\'");
                goto label31;
            }
            throw new InvalidOperationException("never reached");
        label31:
;
            this.Manager.EndTest();
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonExS40GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S41");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonExS40GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S41");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonExS40GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S41");
        }
        #endregion
        
        #region Test Starting in S42
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_PassThroughAuthenticate_NetrLogonSamLogonExS42() {
            this.Manager.BeginTest("Test_PassThroughAuthenticate_NetrLogonSamLogonExS42");
            this.Manager.Comment("reaching state \'S42\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp355;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp355);
            this.Manager.AddReturn(GetPlatformInfo, null, temp355);
            this.Manager.Comment("reaching state \'S43\'");
            int temp365 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonEx.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonExS42GetPlatformChecker)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonEx.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonExS42GetPlatformChecker1)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonEx.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonExS42GetPlatformChecker2)));
            if ((temp365 == 0)) {
                this.Manager.Comment("reaching state \'S189\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp356;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp356 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S378\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp356, "return of NetrServerReqChallenge, state S378");
                this.Manager.Comment("reaching state \'S567\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp357;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp357 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S756\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp357, "return of NetrServerAuthenticate3, state S756");
                this.Manager.Comment("reaching state \'S945\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp358;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonEx(True,PrimaryDc,Client,NetlogonNetworkTra" +
                        "nsitiveInformation,Valid,NetlogonValidationSamInfo2,4)\'");
                temp358 = this.INrpcServerAdapterInstance.NetrLogonSamLogonEx(true, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonNetworkTransitiveInformation, ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo2, 4u);
                this.Manager.Checkpoint("MS-NRPC_R1160");
                this.Manager.Comment("reaching state \'S1134\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonEx/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp358, "return of NetrLogonSamLogonEx, state S1134");
                this.Manager.Comment("reaching state \'S1323\'");
                goto label32;
            }
            if ((temp365 == 1)) {
                this.Manager.Comment("reaching state \'S190\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp359;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp359 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S379\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp359, "return of NetrServerReqChallenge, state S379");
                this.Manager.Comment("reaching state \'S568\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp360;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp360 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S757\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp360, "return of NetrServerAuthenticate3, state S757");
                this.Manager.Comment("reaching state \'S946\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp361;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonEx(True,PrimaryDc,Client,NetlogonNetworkTra" +
                        "nsitiveInformation,Valid,NetlogonValidationSamInfo2,4)\'");
                temp361 = this.INrpcServerAdapterInstance.NetrLogonSamLogonEx(true, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonNetworkTransitiveInformation, ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo2, 4u);
                this.Manager.Checkpoint("MS-NRPC_R1160");
                this.Manager.Comment("reaching state \'S1135\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonEx/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp361, "return of NetrLogonSamLogonEx, state S1135");
                this.Manager.Comment("reaching state \'S1324\'");
                goto label32;
            }
            if ((temp365 == 2)) {
                this.Manager.Comment("reaching state \'S191\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp362;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp362 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S380\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp362, "return of NetrServerReqChallenge, state S380");
                this.Manager.Comment("reaching state \'S569\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp363;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp363 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S758\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp363, "return of NetrServerAuthenticate3, state S758");
                this.Manager.Comment("reaching state \'S947\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp364;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonEx(True,PrimaryDc,Client,NetlogonInteractiv" +
                        "eInformation,Valid,NetlogonValidationSamInfo,0)\'");
                temp364 = this.INrpcServerAdapterInstance.NetrLogonSamLogonEx(true, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo, 0u);
                this.Manager.Checkpoint("MS-NRPC_R1174");
                this.Manager.Checkpoint("MS-NRPC_R1160");
                this.Manager.Comment("reaching state \'S1136\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonEx/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp364, "return of NetrLogonSamLogonEx, state S1136");
                this.Manager.Comment("reaching state \'S1325\'");
                goto label32;
            }
            throw new InvalidOperationException("never reached");
        label32:
;
            this.Manager.EndTest();
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonExS42GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S43");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonExS42GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S43");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonExS42GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S43");
        }
        #endregion
        
        #region Test Starting in S44
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_PassThroughAuthenticate_NetrLogonSamLogonExS44() {
            this.Manager.BeginTest("Test_PassThroughAuthenticate_NetrLogonSamLogonExS44");
            this.Manager.Comment("reaching state \'S44\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp366;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp366);
            this.Manager.AddReturn(GetPlatformInfo, null, temp366);
            this.Manager.Comment("reaching state \'S45\'");
            int temp376 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonEx.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonExS44GetPlatformChecker)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonEx.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonExS44GetPlatformChecker1)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonEx.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonExS44GetPlatformChecker2)));
            if ((temp376 == 0)) {
                this.Manager.Comment("reaching state \'S192\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp367;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp367 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S381\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp367, "return of NetrServerReqChallenge, state S381");
                this.Manager.Comment("reaching state \'S570\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp368;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp368 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S759\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp368, "return of NetrServerAuthenticate3, state S759");
                this.Manager.Comment("reaching state \'S948\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp369;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonEx(True,PrimaryDc,Client,NetlogonNetworkTra" +
                        "nsitiveInformation,Valid,NetlogonValidationSamInfo2,8)\'");
                temp369 = this.INrpcServerAdapterInstance.NetrLogonSamLogonEx(true, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonNetworkTransitiveInformation, ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo2, 8u);
                this.Manager.Checkpoint("MS-NRPC_R1160");
                this.Manager.Comment("reaching state \'S1137\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonEx/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp369, "return of NetrLogonSamLogonEx, state S1137");
                this.Manager.Comment("reaching state \'S1326\'");
                goto label33;
            }
            if ((temp376 == 1)) {
                this.Manager.Comment("reaching state \'S193\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp370;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp370 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S382\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp370, "return of NetrServerReqChallenge, state S382");
                this.Manager.Comment("reaching state \'S571\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp371;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp371 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S760\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp371, "return of NetrServerAuthenticate3, state S760");
                this.Manager.Comment("reaching state \'S949\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp372;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonEx(True,PrimaryDc,Client,NetlogonNetworkTra" +
                        "nsitiveInformation,Valid,NetlogonValidationSamInfo2,8)\'");
                temp372 = this.INrpcServerAdapterInstance.NetrLogonSamLogonEx(true, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonNetworkTransitiveInformation, ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo2, 8u);
                this.Manager.Checkpoint("MS-NRPC_R1160");
                this.Manager.Comment("reaching state \'S1138\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonEx/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp372, "return of NetrLogonSamLogonEx, state S1138");
                this.Manager.Comment("reaching state \'S1327\'");
                goto label33;
            }
            if ((temp376 == 2)) {
                this.Manager.Comment("reaching state \'S194\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp373;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp373 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S383\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp373, "return of NetrServerReqChallenge, state S383");
                this.Manager.Comment("reaching state \'S572\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp374;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp374 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S761\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp374, "return of NetrServerAuthenticate3, state S761");
                this.Manager.Comment("reaching state \'S950\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp375;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonEx(True,PrimaryDc,Client,NetlogonInteractiv" +
                        "eInformation,Valid,NetlogonValidationSamInfo,0)\'");
                temp375 = this.INrpcServerAdapterInstance.NetrLogonSamLogonEx(true, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo, 0u);
                this.Manager.Checkpoint("MS-NRPC_R1174");
                this.Manager.Checkpoint("MS-NRPC_R1160");
                this.Manager.Comment("reaching state \'S1139\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonEx/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp375, "return of NetrLogonSamLogonEx, state S1139");
                this.Manager.Comment("reaching state \'S1328\'");
                goto label33;
            }
            throw new InvalidOperationException("never reached");
        label33:
;
            this.Manager.EndTest();
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonExS44GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S45");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonExS44GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S45");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonExS44GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S45");
        }
        #endregion
        
        #region Test Starting in S46
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_PassThroughAuthenticate_NetrLogonSamLogonExS46() {
            this.Manager.BeginTest("Test_PassThroughAuthenticate_NetrLogonSamLogonExS46");
            this.Manager.Comment("reaching state \'S46\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp377;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp377);
            this.Manager.AddReturn(GetPlatformInfo, null, temp377);
            this.Manager.Comment("reaching state \'S47\'");
            int temp387 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonEx.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonExS46GetPlatformChecker)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonEx.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonExS46GetPlatformChecker1)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonEx.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonExS46GetPlatformChecker2)));
            if ((temp387 == 0)) {
                this.Manager.Comment("reaching state \'S195\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp378;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp378 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S384\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp378, "return of NetrServerReqChallenge, state S384");
                this.Manager.Comment("reaching state \'S573\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp379;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp379 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S762\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp379, "return of NetrServerAuthenticate3, state S762");
                this.Manager.Comment("reaching state \'S951\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp380;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonEx(True,PrimaryDc,Client,NetlogonNetworkTra" +
                        "nsitiveInformation,Valid,NetlogonValidationSamInfo2,2147483648)\'");
                temp380 = this.INrpcServerAdapterInstance.NetrLogonSamLogonEx(true, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonNetworkTransitiveInformation, ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo2, 2147483648u);
                this.Manager.Checkpoint("MS-NRPC_R1160");
                this.Manager.Comment("reaching state \'S1140\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonEx/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp380, "return of NetrLogonSamLogonEx, state S1140");
                this.Manager.Comment("reaching state \'S1329\'");
                goto label34;
            }
            if ((temp387 == 1)) {
                this.Manager.Comment("reaching state \'S196\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp381;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp381 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S385\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp381, "return of NetrServerReqChallenge, state S385");
                this.Manager.Comment("reaching state \'S574\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp382;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp382 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S763\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp382, "return of NetrServerAuthenticate3, state S763");
                this.Manager.Comment("reaching state \'S952\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp383;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonEx(True,PrimaryDc,Client,NetlogonNetworkTra" +
                        "nsitiveInformation,Valid,NetlogonValidationSamInfo2,2147483648)\'");
                temp383 = this.INrpcServerAdapterInstance.NetrLogonSamLogonEx(true, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonNetworkTransitiveInformation, ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo2, 2147483648u);
                this.Manager.Checkpoint("MS-NRPC_R1160");
                this.Manager.Comment("reaching state \'S1141\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonEx/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp383, "return of NetrLogonSamLogonEx, state S1141");
                this.Manager.Comment("reaching state \'S1330\'");
                goto label34;
            }
            if ((temp387 == 2)) {
                this.Manager.Comment("reaching state \'S197\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp384;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp384 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S386\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp384, "return of NetrServerReqChallenge, state S386");
                this.Manager.Comment("reaching state \'S575\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp385;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp385 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S764\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp385, "return of NetrServerAuthenticate3, state S764");
                this.Manager.Comment("reaching state \'S953\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp386;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonEx(True,PrimaryDc,Client,NetlogonInteractiv" +
                        "eInformation,Valid,NetlogonValidationSamInfo,0)\'");
                temp386 = this.INrpcServerAdapterInstance.NetrLogonSamLogonEx(true, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo, 0u);
                this.Manager.Checkpoint("MS-NRPC_R1174");
                this.Manager.Checkpoint("MS-NRPC_R1160");
                this.Manager.Comment("reaching state \'S1142\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonEx/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp386, "return of NetrLogonSamLogonEx, state S1142");
                this.Manager.Comment("reaching state \'S1331\'");
                goto label34;
            }
            throw new InvalidOperationException("never reached");
        label34:
;
            this.Manager.EndTest();
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonExS46GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S47");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonExS46GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S47");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonExS46GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S47");
        }
        #endregion
        
        #region Test Starting in S48
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_PassThroughAuthenticate_NetrLogonSamLogonExS48() {
            this.Manager.BeginTest("Test_PassThroughAuthenticate_NetrLogonSamLogonExS48");
            this.Manager.Comment("reaching state \'S48\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp388;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp388);
            this.Manager.AddReturn(GetPlatformInfo, null, temp388);
            this.Manager.Comment("reaching state \'S49\'");
            int temp398 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonEx.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonExS48GetPlatformChecker)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonEx.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonExS48GetPlatformChecker1)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonEx.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonExS48GetPlatformChecker2)));
            if ((temp398 == 0)) {
                this.Manager.Comment("reaching state \'S198\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp389;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp389 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S387\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp389, "return of NetrServerReqChallenge, state S387");
                this.Manager.Comment("reaching state \'S576\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp390;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp390 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S765\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp390, "return of NetrServerAuthenticate3, state S765");
                this.Manager.Comment("reaching state \'S954\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp391;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonEx(True,PrimaryDc,Client,NetlogonInteractiv" +
                        "eTransitiveInformation,Valid,NetlogonValidationGenericInfo2,0)\'");
                temp391 = this.INrpcServerAdapterInstance.NetrLogonSamLogonEx(true, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonInteractiveTransitiveInformation, ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationGenericInfo2, 0u);
                this.Manager.Checkpoint("MS-NRPC_R1161");
                this.Manager.Comment("reaching state \'S1143\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonEx/STATUS_INVALID_INFO_CLASS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_INVALID_INFO_CLASS, temp391, "return of NetrLogonSamLogonEx, state S1143");
                this.Manager.Comment("reaching state \'S1332\'");
                goto label35;
            }
            if ((temp398 == 1)) {
                this.Manager.Comment("reaching state \'S199\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp392;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp392 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S388\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp392, "return of NetrServerReqChallenge, state S388");
                this.Manager.Comment("reaching state \'S577\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp393;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp393 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S766\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp393, "return of NetrServerAuthenticate3, state S766");
                this.Manager.Comment("reaching state \'S955\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp394;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonEx(True,PrimaryDc,Client,NetlogonInteractiv" +
                        "eTransitiveInformation,Valid,NetlogonValidationGenericInfo2,0)\'");
                temp394 = this.INrpcServerAdapterInstance.NetrLogonSamLogonEx(true, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonInteractiveTransitiveInformation, ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationGenericInfo2, 0u);
                this.Manager.Checkpoint("MS-NRPC_R1161");
                this.Manager.Comment("reaching state \'S1144\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonEx/STATUS_INVALID_INFO_CLASS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_INVALID_INFO_CLASS, temp394, "return of NetrLogonSamLogonEx, state S1144");
                this.Manager.Comment("reaching state \'S1333\'");
                goto label35;
            }
            if ((temp398 == 2)) {
                this.Manager.Comment("reaching state \'S200\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp395;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp395 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S389\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp395, "return of NetrServerReqChallenge, state S389");
                this.Manager.Comment("reaching state \'S578\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp396;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp396 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S767\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp396, "return of NetrServerAuthenticate3, state S767");
                this.Manager.Comment("reaching state \'S956\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp397;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonEx(True,PrimaryDc,Client,NetlogonInteractiv" +
                        "eInformation,Valid,NetlogonValidationSamInfo,0)\'");
                temp397 = this.INrpcServerAdapterInstance.NetrLogonSamLogonEx(true, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo, 0u);
                this.Manager.Checkpoint("MS-NRPC_R1174");
                this.Manager.Checkpoint("MS-NRPC_R1160");
                this.Manager.Comment("reaching state \'S1145\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonEx/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp397, "return of NetrLogonSamLogonEx, state S1145");
                this.Manager.Comment("reaching state \'S1334\'");
                goto label35;
            }
            throw new InvalidOperationException("never reached");
        label35:
;
            this.Manager.EndTest();
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonExS48GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S49");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonExS48GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S49");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonExS48GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S49");
        }
        #endregion
        
        #region Test Starting in S50
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_PassThroughAuthenticate_NetrLogonSamLogonExS50() {
            this.Manager.BeginTest("Test_PassThroughAuthenticate_NetrLogonSamLogonExS50");
            this.Manager.Comment("reaching state \'S50\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp399;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp399);
            this.Manager.AddReturn(GetPlatformInfo, null, temp399);
            this.Manager.Comment("reaching state \'S51\'");
            int temp409 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonEx.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonExS50GetPlatformChecker)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonEx.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonExS50GetPlatformChecker1)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonEx.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonExS50GetPlatformChecker2)));
            if ((temp409 == 0)) {
                this.Manager.Comment("reaching state \'S201\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp400;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp400 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S390\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp400, "return of NetrServerReqChallenge, state S390");
                this.Manager.Comment("reaching state \'S579\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp401;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp401 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S768\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp401, "return of NetrServerAuthenticate3, state S768");
                this.Manager.Comment("reaching state \'S957\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp402;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonEx(True,PrimaryDc,Client,NetlogonInteractiv" +
                        "eTransitiveInformation,Valid,NetlogonValidationSamInfo4,0)\'");
                temp402 = this.INrpcServerAdapterInstance.NetrLogonSamLogonEx(true, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonInteractiveTransitiveInformation, ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo4, 0u);
                this.Manager.Checkpoint("MS-NRPC_R1175");
                this.Manager.Checkpoint("MS-NRPC_R1160");
                this.Manager.Comment("reaching state \'S1146\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonEx/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp402, "return of NetrLogonSamLogonEx, state S1146");
                this.Manager.Comment("reaching state \'S1335\'");
                goto label36;
            }
            if ((temp409 == 1)) {
                this.Manager.Comment("reaching state \'S202\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp403;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp403 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S391\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp403, "return of NetrServerReqChallenge, state S391");
                this.Manager.Comment("reaching state \'S580\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp404;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp404 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S769\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp404, "return of NetrServerAuthenticate3, state S769");
                this.Manager.Comment("reaching state \'S958\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp405;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonEx(True,PrimaryDc,Client,NetlogonInteractiv" +
                        "eTransitiveInformation,Valid,NetlogonValidationSamInfo4,0)\'");
                temp405 = this.INrpcServerAdapterInstance.NetrLogonSamLogonEx(true, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonInteractiveTransitiveInformation, ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo4, 0u);
                this.Manager.Checkpoint("MS-NRPC_R1175");
                this.Manager.Checkpoint("MS-NRPC_R1160");
                this.Manager.Comment("reaching state \'S1147\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonEx/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp405, "return of NetrLogonSamLogonEx, state S1147");
                this.Manager.Comment("reaching state \'S1336\'");
                goto label36;
            }
            if ((temp409 == 2)) {
                this.Manager.Comment("reaching state \'S203\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp406;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp406 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S392\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp406, "return of NetrServerReqChallenge, state S392");
                this.Manager.Comment("reaching state \'S581\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp407;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp407 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S770\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp407, "return of NetrServerAuthenticate3, state S770");
                this.Manager.Comment("reaching state \'S959\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp408;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonEx(True,PrimaryDc,Client,NetlogonInteractiv" +
                        "eInformation,Valid,NetlogonValidationSamInfo,0)\'");
                temp408 = this.INrpcServerAdapterInstance.NetrLogonSamLogonEx(true, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo, 0u);
                this.Manager.Checkpoint("MS-NRPC_R1174");
                this.Manager.Checkpoint("MS-NRPC_R1160");
                this.Manager.Comment("reaching state \'S1148\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonEx/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp408, "return of NetrLogonSamLogonEx, state S1148");
                this.Manager.Comment("reaching state \'S1337\'");
                goto label36;
            }
            throw new InvalidOperationException("never reached");
        label36:
;
            this.Manager.EndTest();
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonExS50GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S51");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonExS50GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S51");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonExS50GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S51");
        }
        #endregion
        
        #region Test Starting in S52
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_PassThroughAuthenticate_NetrLogonSamLogonExS52() {
            this.Manager.BeginTest("Test_PassThroughAuthenticate_NetrLogonSamLogonExS52");
            this.Manager.Comment("reaching state \'S52\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp410;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp410);
            this.Manager.AddReturn(GetPlatformInfo, null, temp410);
            this.Manager.Comment("reaching state \'S53\'");
            int temp420 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonEx.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonExS52GetPlatformChecker)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonEx.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonExS52GetPlatformChecker1)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonEx.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonExS52GetPlatformChecker2)));
            if ((temp420 == 0)) {
                this.Manager.Comment("reaching state \'S204\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp411;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp411 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S393\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp411, "return of NetrServerReqChallenge, state S393");
                this.Manager.Comment("reaching state \'S582\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp412;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp412 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S771\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp412, "return of NetrServerAuthenticate3, state S771");
                this.Manager.Comment("reaching state \'S960\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp413;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonEx(True,InvalidComputer,Client,NetlogonNetw" +
                        "orkTransitiveInformation,Valid,NetlogonValidationSamInfo2,0)\'");
                temp413 = this.INrpcServerAdapterInstance.NetrLogonSamLogonEx(true, ((Microsoft.Protocols.TestSuites.Nrpc.ComputerType)(1)), Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonNetworkTransitiveInformation, ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo2, 0u);
                this.Manager.Checkpoint("MS-NRPC_R1172");
                this.Manager.Checkpoint("MS-NRPC_R1161");
                this.Manager.Comment("reaching state \'S1149\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonEx/STATUS_INVALID_COMPUTER_NAME\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_INVALID_COMPUTER_NAME, temp413, "return of NetrLogonSamLogonEx, state S1149");
                this.Manager.Comment("reaching state \'S1338\'");
                goto label37;
            }
            if ((temp420 == 1)) {
                this.Manager.Comment("reaching state \'S205\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp414;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp414 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S394\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp414, "return of NetrServerReqChallenge, state S394");
                this.Manager.Comment("reaching state \'S583\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp415;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp415 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S772\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp415, "return of NetrServerAuthenticate3, state S772");
                this.Manager.Comment("reaching state \'S961\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp416;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonEx(True,InvalidComputer,Client,NetlogonNetw" +
                        "orkTransitiveInformation,Valid,NetlogonValidationSamInfo2,0)\'");
                temp416 = this.INrpcServerAdapterInstance.NetrLogonSamLogonEx(true, ((Microsoft.Protocols.TestSuites.Nrpc.ComputerType)(1)), Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonNetworkTransitiveInformation, ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo2, 0u);
                this.Manager.Checkpoint("MS-NRPC_R1172");
                this.Manager.Checkpoint("MS-NRPC_R1161");
                this.Manager.Comment("reaching state \'S1150\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonEx/STATUS_INVALID_COMPUTER_NAME\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_INVALID_COMPUTER_NAME, temp416, "return of NetrLogonSamLogonEx, state S1150");
                this.Manager.Comment("reaching state \'S1339\'");
                goto label37;
            }
            if ((temp420 == 2)) {
                this.Manager.Comment("reaching state \'S206\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp417;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp417 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S395\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp417, "return of NetrServerReqChallenge, state S395");
                this.Manager.Comment("reaching state \'S584\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp418;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp418 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S773\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp418, "return of NetrServerAuthenticate3, state S773");
                this.Manager.Comment("reaching state \'S962\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp419;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonEx(True,PrimaryDc,Client,NetlogonInteractiv" +
                        "eInformation,Valid,NetlogonValidationSamInfo,0)\'");
                temp419 = this.INrpcServerAdapterInstance.NetrLogonSamLogonEx(true, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo, 0u);
                this.Manager.Checkpoint("MS-NRPC_R1174");
                this.Manager.Checkpoint("MS-NRPC_R1160");
                this.Manager.Comment("reaching state \'S1151\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonEx/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp419, "return of NetrLogonSamLogonEx, state S1151");
                this.Manager.Comment("reaching state \'S1340\'");
                goto label37;
            }
            throw new InvalidOperationException("never reached");
        label37:
;
            this.Manager.EndTest();
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonExS52GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S53");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonExS52GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S53");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonExS52GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S53");
        }
        #endregion
        
        #region Test Starting in S54
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_PassThroughAuthenticate_NetrLogonSamLogonExS54() {
            this.Manager.BeginTest("Test_PassThroughAuthenticate_NetrLogonSamLogonExS54");
            this.Manager.Comment("reaching state \'S54\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp421;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp421);
            this.Manager.AddReturn(GetPlatformInfo, null, temp421);
            this.Manager.Comment("reaching state \'S55\'");
            int temp431 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonEx.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonExS54GetPlatformChecker)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonEx.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonExS54GetPlatformChecker1)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonEx.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonExS54GetPlatformChecker2)));
            if ((temp431 == 0)) {
                this.Manager.Comment("reaching state \'S207\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp422;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp422 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S396\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp422, "return of NetrServerReqChallenge, state S396");
                this.Manager.Comment("reaching state \'S585\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp423;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp423 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S774\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp423, "return of NetrServerAuthenticate3, state S774");
                this.Manager.Comment("reaching state \'S963\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp424;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonEx(True,NonDcServer,Client,NetlogonNetworkT" +
                        "ransitiveInformation,Valid,NetlogonValidationSamInfo2,0)\'");
                temp424 = this.INrpcServerAdapterInstance.NetrLogonSamLogonEx(true, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonNetworkTransitiveInformation, ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo2, 0u);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R101162");
                this.Manager.Comment("reaching state \'S1152\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonEx/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp424, "return of NetrLogonSamLogonEx, state S1152");
                this.Manager.Comment("reaching state \'S1341\'");
                goto label38;
            }
            if ((temp431 == 1)) {
                this.Manager.Comment("reaching state \'S208\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp425;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp425 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S397\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp425, "return of NetrServerReqChallenge, state S397");
                this.Manager.Comment("reaching state \'S586\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp426;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp426 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S775\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp426, "return of NetrServerAuthenticate3, state S775");
                this.Manager.Comment("reaching state \'S964\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp427;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonEx(True,NonDcServer,Client,NetlogonNetworkT" +
                        "ransitiveInformation,Valid,NetlogonValidationSamInfo2,0)\'");
                temp427 = this.INrpcServerAdapterInstance.NetrLogonSamLogonEx(true, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonNetworkTransitiveInformation, ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo2, 0u);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R101162");
                this.Manager.Comment("reaching state \'S1153\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonEx/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp427, "return of NetrLogonSamLogonEx, state S1153");
                this.Manager.Comment("reaching state \'S1342\'");
                goto label38;
            }
            if ((temp431 == 2)) {
                this.Manager.Comment("reaching state \'S209\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp428;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp428 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S398\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp428, "return of NetrServerReqChallenge, state S398");
                this.Manager.Comment("reaching state \'S587\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp429;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp429 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S776\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp429, "return of NetrServerAuthenticate3, state S776");
                this.Manager.Comment("reaching state \'S965\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp430;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonEx(True,PrimaryDc,Client,NetlogonInteractiv" +
                        "eInformation,Valid,NetlogonValidationSamInfo,0)\'");
                temp430 = this.INrpcServerAdapterInstance.NetrLogonSamLogonEx(true, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo, 0u);
                this.Manager.Checkpoint("MS-NRPC_R1174");
                this.Manager.Checkpoint("MS-NRPC_R1160");
                this.Manager.Comment("reaching state \'S1154\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonEx/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp430, "return of NetrLogonSamLogonEx, state S1154");
                this.Manager.Comment("reaching state \'S1343\'");
                goto label38;
            }
            throw new InvalidOperationException("never reached");
        label38:
;
            this.Manager.EndTest();
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonExS54GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S55");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonExS54GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S55");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonExS54GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S55");
        }
        #endregion
        
        #region Test Starting in S56
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_PassThroughAuthenticate_NetrLogonSamLogonExS56() {
            this.Manager.BeginTest("Test_PassThroughAuthenticate_NetrLogonSamLogonExS56");
            this.Manager.Comment("reaching state \'S56\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp432;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp432);
            this.Manager.AddReturn(GetPlatformInfo, null, temp432);
            this.Manager.Comment("reaching state \'S57\'");
            int temp442 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonEx.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonExS56GetPlatformChecker)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonEx.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonExS56GetPlatformChecker1)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonEx.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonExS56GetPlatformChecker2)));
            if ((temp442 == 0)) {
                this.Manager.Comment("reaching state \'S210\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp433;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp433 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S399\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp433, "return of NetrServerReqChallenge, state S399");
                this.Manager.Comment("reaching state \'S588\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp434;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp434 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S777\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp434, "return of NetrServerAuthenticate3, state S777");
                this.Manager.Comment("reaching state \'S966\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp435;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonEx(True,PrimaryDc,Client,NetlogonServiceTra" +
                        "nsitiveInformation,Valid,NetlogonValidationGenericInfo2,0)\'");
                temp435 = this.INrpcServerAdapterInstance.NetrLogonSamLogonEx(true, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonServiceTransitiveInformation, ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationGenericInfo2, 0u);
                this.Manager.Checkpoint("MS-NRPC_R1161");
                this.Manager.Comment("reaching state \'S1155\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonEx/STATUS_INVALID_INFO_CLASS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_INVALID_INFO_CLASS, temp435, "return of NetrLogonSamLogonEx, state S1155");
                this.Manager.Comment("reaching state \'S1344\'");
                goto label39;
            }
            if ((temp442 == 1)) {
                this.Manager.Comment("reaching state \'S211\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp436;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp436 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S400\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp436, "return of NetrServerReqChallenge, state S400");
                this.Manager.Comment("reaching state \'S589\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp437;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp437 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S778\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp437, "return of NetrServerAuthenticate3, state S778");
                this.Manager.Comment("reaching state \'S967\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp438;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonEx(True,PrimaryDc,Client,NetlogonServiceTra" +
                        "nsitiveInformation,Valid,NetlogonValidationGenericInfo2,0)\'");
                temp438 = this.INrpcServerAdapterInstance.NetrLogonSamLogonEx(true, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonServiceTransitiveInformation, ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationGenericInfo2, 0u);
                this.Manager.Checkpoint("MS-NRPC_R1161");
                this.Manager.Comment("reaching state \'S1156\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonEx/STATUS_INVALID_INFO_CLASS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_INVALID_INFO_CLASS, temp438, "return of NetrLogonSamLogonEx, state S1156");
                this.Manager.Comment("reaching state \'S1345\'");
                goto label39;
            }
            if ((temp442 == 2)) {
                this.Manager.Comment("reaching state \'S212\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp439;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp439 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S401\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp439, "return of NetrServerReqChallenge, state S401");
                this.Manager.Comment("reaching state \'S590\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp440;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp440 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S779\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp440, "return of NetrServerAuthenticate3, state S779");
                this.Manager.Comment("reaching state \'S968\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp441;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonEx(True,PrimaryDc,Client,NetlogonInteractiv" +
                        "eInformation,Valid,NetlogonValidationSamInfo,0)\'");
                temp441 = this.INrpcServerAdapterInstance.NetrLogonSamLogonEx(true, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo, 0u);
                this.Manager.Checkpoint("MS-NRPC_R1174");
                this.Manager.Checkpoint("MS-NRPC_R1160");
                this.Manager.Comment("reaching state \'S1157\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonEx/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp441, "return of NetrLogonSamLogonEx, state S1157");
                this.Manager.Comment("reaching state \'S1346\'");
                goto label39;
            }
            throw new InvalidOperationException("never reached");
        label39:
;
            this.Manager.EndTest();
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonExS56GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S57");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonExS56GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S57");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonExS56GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S57");
        }
        #endregion
        
        #region Test Starting in S58
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_PassThroughAuthenticate_NetrLogonSamLogonExS58() {
            this.Manager.BeginTest("Test_PassThroughAuthenticate_NetrLogonSamLogonExS58");
            this.Manager.Comment("reaching state \'S58\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp443;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp443);
            this.Manager.AddReturn(GetPlatformInfo, null, temp443);
            this.Manager.Comment("reaching state \'S59\'");
            int temp453 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonEx.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonExS58GetPlatformChecker)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonEx.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonExS58GetPlatformChecker1)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonEx.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonExS58GetPlatformChecker2)));
            if ((temp453 == 0)) {
                this.Manager.Comment("reaching state \'S213\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp444;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp444 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S402\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp444, "return of NetrServerReqChallenge, state S402");
                this.Manager.Comment("reaching state \'S591\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp445;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp445 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S780\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp445, "return of NetrServerAuthenticate3, state S780");
                this.Manager.Comment("reaching state \'S969\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp446;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonEx(True,PrimaryDc,Client,NetlogonServiceTra" +
                        "nsitiveInformation,Valid,NetlogonValidationSamInfo4,0)\'");
                temp446 = this.INrpcServerAdapterInstance.NetrLogonSamLogonEx(true, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonServiceTransitiveInformation, ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo4, 0u);
                this.Manager.Checkpoint("MS-NRPC_R101177");
                this.Manager.Checkpoint("MS-NRPC_R1160");
                this.Manager.Comment("reaching state \'S1158\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonEx/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp446, "return of NetrLogonSamLogonEx, state S1158");
                this.Manager.Comment("reaching state \'S1347\'");
                goto label40;
            }
            if ((temp453 == 1)) {
                this.Manager.Comment("reaching state \'S214\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp447;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp447 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S403\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp447, "return of NetrServerReqChallenge, state S403");
                this.Manager.Comment("reaching state \'S592\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp448;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp448 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S781\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp448, "return of NetrServerAuthenticate3, state S781");
                this.Manager.Comment("reaching state \'S970\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp449;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonEx(True,PrimaryDc,Client,NetlogonServiceTra" +
                        "nsitiveInformation,Valid,NetlogonValidationSamInfo4,0)\'");
                temp449 = this.INrpcServerAdapterInstance.NetrLogonSamLogonEx(true, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonServiceTransitiveInformation, ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo4, 0u);
                this.Manager.Checkpoint("MS-NRPC_R101177");
                this.Manager.Checkpoint("MS-NRPC_R1160");
                this.Manager.Comment("reaching state \'S1159\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonEx/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp449, "return of NetrLogonSamLogonEx, state S1159");
                this.Manager.Comment("reaching state \'S1348\'");
                goto label40;
            }
            if ((temp453 == 2)) {
                this.Manager.Comment("reaching state \'S215\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp450;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp450 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S404\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp450, "return of NetrServerReqChallenge, state S404");
                this.Manager.Comment("reaching state \'S593\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp451;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp451 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S782\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp451, "return of NetrServerAuthenticate3, state S782");
                this.Manager.Comment("reaching state \'S971\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp452;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonEx(True,PrimaryDc,Client,NetlogonInteractiv" +
                        "eInformation,Valid,NetlogonValidationSamInfo,0)\'");
                temp452 = this.INrpcServerAdapterInstance.NetrLogonSamLogonEx(true, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo, 0u);
                this.Manager.Checkpoint("MS-NRPC_R1174");
                this.Manager.Checkpoint("MS-NRPC_R1160");
                this.Manager.Comment("reaching state \'S1160\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonEx/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp452, "return of NetrLogonSamLogonEx, state S1160");
                this.Manager.Comment("reaching state \'S1349\'");
                goto label40;
            }
            throw new InvalidOperationException("never reached");
        label40:
;
            this.Manager.EndTest();
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonExS58GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S59");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonExS58GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S59");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonExS58GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S59");
        }
        #endregion
        
        #region Test Starting in S6
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_PassThroughAuthenticate_NetrLogonSamLogonExS6() {
            this.Manager.BeginTest("Test_PassThroughAuthenticate_NetrLogonSamLogonExS6");
            this.Manager.Comment("reaching state \'S6\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp454;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp454);
            this.Manager.AddReturn(GetPlatformInfo, null, temp454);
            this.Manager.Comment("reaching state \'S7\'");
            int temp464 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonEx.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonExS6GetPlatformChecker)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonEx.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonExS6GetPlatformChecker1)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonEx.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonExS6GetPlatformChecker2)));
            if ((temp464 == 0)) {
                this.Manager.Comment("reaching state \'S135\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp455;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp455 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S324\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp455, "return of NetrServerReqChallenge, state S324");
                this.Manager.Comment("reaching state \'S513\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp456;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp456 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S702\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp456, "return of NetrServerAuthenticate3, state S702");
                this.Manager.Comment("reaching state \'S891\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp457;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonEx(True,PrimaryDc,Client,NetlogonInteractiv" +
                        "eInformation,Valid,NetlogonValidationSamInfo2,0)\'");
                temp457 = this.INrpcServerAdapterInstance.NetrLogonSamLogonEx(true, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo2, 0u);
                this.Manager.Checkpoint("MS-NRPC_R1174");
                this.Manager.Checkpoint("MS-NRPC_R1160");
                this.Manager.Comment("reaching state \'S1080\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonEx/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp457, "return of NetrLogonSamLogonEx, state S1080");
                this.Manager.Comment("reaching state \'S1269\'");
                goto label41;
            }
            if ((temp464 == 1)) {
                this.Manager.Comment("reaching state \'S136\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp458;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp458 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S325\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp458, "return of NetrServerReqChallenge, state S325");
                this.Manager.Comment("reaching state \'S514\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp459;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp459 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S703\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp459, "return of NetrServerAuthenticate3, state S703");
                this.Manager.Comment("reaching state \'S892\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp460;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonEx(True,PrimaryDc,Client,NetlogonInteractiv" +
                        "eInformation,Valid,NetlogonValidationSamInfo2,0)\'");
                temp460 = this.INrpcServerAdapterInstance.NetrLogonSamLogonEx(true, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo2, 0u);
                this.Manager.Checkpoint("MS-NRPC_R1174");
                this.Manager.Checkpoint("MS-NRPC_R1160");
                this.Manager.Comment("reaching state \'S1081\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonEx/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp460, "return of NetrLogonSamLogonEx, state S1081");
                this.Manager.Comment("reaching state \'S1270\'");
                goto label41;
            }
            if ((temp464 == 2)) {
                this.Manager.Comment("reaching state \'S137\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp461;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp461 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S326\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp461, "return of NetrServerReqChallenge, state S326");
                this.Manager.Comment("reaching state \'S515\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp462;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp462 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S704\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp462, "return of NetrServerAuthenticate3, state S704");
                this.Manager.Comment("reaching state \'S893\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp463;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonEx(True,PrimaryDc,Client,NetlogonInteractiv" +
                        "eInformation,Valid,NetlogonValidationSamInfo,0)\'");
                temp463 = this.INrpcServerAdapterInstance.NetrLogonSamLogonEx(true, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo, 0u);
                this.Manager.Checkpoint("MS-NRPC_R1174");
                this.Manager.Checkpoint("MS-NRPC_R1160");
                this.Manager.Comment("reaching state \'S1082\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonEx/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp463, "return of NetrLogonSamLogonEx, state S1082");
                this.Manager.Comment("reaching state \'S1271\'");
                goto label41;
            }
            throw new InvalidOperationException("never reached");
        label41:
;
            this.Manager.EndTest();
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonExS6GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S7");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonExS6GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S7");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonExS6GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S7");
        }
        #endregion
        
        #region Test Starting in S60
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_PassThroughAuthenticate_NetrLogonSamLogonExS60() {
            this.Manager.BeginTest("Test_PassThroughAuthenticate_NetrLogonSamLogonExS60");
            this.Manager.Comment("reaching state \'S60\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp465;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp465);
            this.Manager.AddReturn(GetPlatformInfo, null, temp465);
            this.Manager.Comment("reaching state \'S61\'");
            int temp475 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonEx.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonExS60GetPlatformChecker)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonEx.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonExS60GetPlatformChecker1)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonEx.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonExS60GetPlatformChecker2)));
            if ((temp475 == 0)) {
                this.Manager.Comment("reaching state \'S216\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp466;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp466 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S405\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp466, "return of NetrServerReqChallenge, state S405");
                this.Manager.Comment("reaching state \'S594\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp467;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp467 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S783\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp467, "return of NetrServerAuthenticate3, state S783");
                this.Manager.Comment("reaching state \'S972\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp468;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonEx(True,PrimaryDc,Client,NetlogonNetworkTra" +
                        "nsitiveInformation,Valid,NetlogonValidationGenericInfo2,0)\'");
                temp468 = this.INrpcServerAdapterInstance.NetrLogonSamLogonEx(true, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonNetworkTransitiveInformation, ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationGenericInfo2, 0u);
                this.Manager.Checkpoint("MS-NRPC_R1161");
                this.Manager.Comment("reaching state \'S1161\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonEx/STATUS_INVALID_INFO_CLASS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_INVALID_INFO_CLASS, temp468, "return of NetrLogonSamLogonEx, state S1161");
                this.Manager.Comment("reaching state \'S1350\'");
                goto label42;
            }
            if ((temp475 == 1)) {
                this.Manager.Comment("reaching state \'S217\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp469;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp469 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S406\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp469, "return of NetrServerReqChallenge, state S406");
                this.Manager.Comment("reaching state \'S595\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp470;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp470 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S784\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp470, "return of NetrServerAuthenticate3, state S784");
                this.Manager.Comment("reaching state \'S973\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp471;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonEx(True,PrimaryDc,Client,NetlogonNetworkTra" +
                        "nsitiveInformation,Valid,NetlogonValidationGenericInfo2,0)\'");
                temp471 = this.INrpcServerAdapterInstance.NetrLogonSamLogonEx(true, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonNetworkTransitiveInformation, ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationGenericInfo2, 0u);
                this.Manager.Checkpoint("MS-NRPC_R1161");
                this.Manager.Comment("reaching state \'S1162\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonEx/STATUS_INVALID_INFO_CLASS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_INVALID_INFO_CLASS, temp471, "return of NetrLogonSamLogonEx, state S1162");
                this.Manager.Comment("reaching state \'S1351\'");
                goto label42;
            }
            if ((temp475 == 2)) {
                this.Manager.Comment("reaching state \'S218\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp472;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp472 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S407\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp472, "return of NetrServerReqChallenge, state S407");
                this.Manager.Comment("reaching state \'S596\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp473;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp473 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S785\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp473, "return of NetrServerAuthenticate3, state S785");
                this.Manager.Comment("reaching state \'S974\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp474;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonEx(True,PrimaryDc,Client,NetlogonInteractiv" +
                        "eInformation,Valid,NetlogonValidationSamInfo,0)\'");
                temp474 = this.INrpcServerAdapterInstance.NetrLogonSamLogonEx(true, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo, 0u);
                this.Manager.Checkpoint("MS-NRPC_R1174");
                this.Manager.Checkpoint("MS-NRPC_R1160");
                this.Manager.Comment("reaching state \'S1163\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonEx/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp474, "return of NetrLogonSamLogonEx, state S1163");
                this.Manager.Comment("reaching state \'S1352\'");
                goto label42;
            }
            throw new InvalidOperationException("never reached");
        label42:
;
            this.Manager.EndTest();
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonExS60GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S61");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonExS60GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S61");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonExS60GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S61");
        }
        #endregion
        
        #region Test Starting in S62
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_PassThroughAuthenticate_NetrLogonSamLogonExS62() {
            this.Manager.BeginTest("Test_PassThroughAuthenticate_NetrLogonSamLogonExS62");
            this.Manager.Comment("reaching state \'S62\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp476;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp476);
            this.Manager.AddReturn(GetPlatformInfo, null, temp476);
            this.Manager.Comment("reaching state \'S63\'");
            int temp492 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonEx.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonExS62GetPlatformChecker)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonEx.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonExS62GetPlatformChecker1)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonEx.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonExS62GetPlatformChecker2)));
            if ((temp492 == 0)) {
                this.Manager.Comment("reaching state \'S219\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp477;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp477 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S408\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp477, "return of NetrServerReqChallenge, state S408");
                this.Manager.Comment("reaching state \'S597\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp478;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp478 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S786\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp478, "return of NetrServerAuthenticate3, state S786");
                this.Manager.Comment("reaching state \'S975\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp479;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonEx(True,PrimaryDc,Client,NetlogonNetworkTra" +
                        "nsitiveInformation,Valid,NetlogonValidationSamInfo2,2147483648)\'");
                temp479 = this.INrpcServerAdapterInstance.NetrLogonSamLogonEx(true, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonNetworkTransitiveInformation, ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo2, 2147483648u);
                this.Manager.Checkpoint("MS-NRPC_R1160");
                this.Manager.Comment("reaching state \'S1164\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonEx/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp479, "return of NetrLogonSamLogonEx, state S1164");
                this.Manager.Comment("reaching state \'S1353\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp480;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp480 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S1450\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp480, "return of NetrServerReqChallenge, state S1450");
                this.Manager.Comment("reaching state \'S1453\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp481;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp481 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S1456\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp481, "return of NetrServerAuthenticate3, state S1456");
                this.Manager.Comment("reaching state \'S1459\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp482;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonEx(True,PrimaryDc,Client,NetlogonNetworkTra" +
                        "nsitiveInformation,Valid,NetlogonValidationSamInfo2,0)\'");
                temp482 = this.INrpcServerAdapterInstance.NetrLogonSamLogonEx(true, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonNetworkTransitiveInformation, ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo2, 0u);
                this.Manager.Checkpoint("MS-NRPC_R1154");
                this.Manager.Checkpoint("MS-NRPC_R1160");
                this.Manager.Comment("reaching state \'S1462\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonEx/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp482, "return of NetrLogonSamLogonEx, state S1462");
                this.Manager.Comment("reaching state \'S1465\'");
                goto label43;
            }
            if ((temp492 == 1)) {
                this.Manager.Comment("reaching state \'S220\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp483;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp483 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S409\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp483, "return of NetrServerReqChallenge, state S409");
                this.Manager.Comment("reaching state \'S598\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp484;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp484 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S787\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp484, "return of NetrServerAuthenticate3, state S787");
                this.Manager.Comment("reaching state \'S976\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp485;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonEx(True,PrimaryDc,Client,NetlogonNetworkTra" +
                        "nsitiveInformation,Valid,NetlogonValidationSamInfo2,2147483648)\'");
                temp485 = this.INrpcServerAdapterInstance.NetrLogonSamLogonEx(true, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonNetworkTransitiveInformation, ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo2, 2147483648u);
                this.Manager.Checkpoint("MS-NRPC_R1160");
                this.Manager.Comment("reaching state \'S1165\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonEx/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp485, "return of NetrLogonSamLogonEx, state S1165");
                this.Manager.Comment("reaching state \'S1354\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp486;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp486 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S1451\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp486, "return of NetrServerReqChallenge, state S1451");
                this.Manager.Comment("reaching state \'S1454\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp487;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp487 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S1457\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp487, "return of NetrServerAuthenticate3, state S1457");
                this.Manager.Comment("reaching state \'S1460\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp488;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonEx(True,PrimaryDc,Client,NetlogonNetworkTra" +
                        "nsitiveInformation,Valid,NetlogonValidationSamInfo2,0)\'");
                temp488 = this.INrpcServerAdapterInstance.NetrLogonSamLogonEx(true, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonNetworkTransitiveInformation, ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo2, 0u);
                this.Manager.Checkpoint("MS-NRPC_R1154");
                this.Manager.Checkpoint("MS-NRPC_R1160");
                this.Manager.Comment("reaching state \'S1463\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonEx/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp488, "return of NetrLogonSamLogonEx, state S1463");
                this.Manager.Comment("reaching state \'S1466\'");
                goto label43;
            }
            if ((temp492 == 2)) {
                this.Manager.Comment("reaching state \'S221\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp489;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp489 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S410\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp489, "return of NetrServerReqChallenge, state S410");
                this.Manager.Comment("reaching state \'S599\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp490;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp490 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S788\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp490, "return of NetrServerAuthenticate3, state S788");
                this.Manager.Comment("reaching state \'S977\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp491;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonEx(True,PrimaryDc,Client,NetlogonInteractiv" +
                        "eInformation,Valid,NetlogonValidationSamInfo,0)\'");
                temp491 = this.INrpcServerAdapterInstance.NetrLogonSamLogonEx(true, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo, 0u);
                this.Manager.Checkpoint("MS-NRPC_R1174");
                this.Manager.Checkpoint("MS-NRPC_R1160");
                this.Manager.Comment("reaching state \'S1166\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonEx/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp491, "return of NetrLogonSamLogonEx, state S1166");
                this.Manager.Comment("reaching state \'S1355\'");
                goto label43;
            }
            throw new InvalidOperationException("never reached");
        label43:
;
            this.Manager.EndTest();
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonExS62GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S63");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonExS62GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S63");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonExS62GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S63");
        }
        #endregion
        
        #region Test Starting in S64
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_PassThroughAuthenticate_NetrLogonSamLogonExS64() {
            this.Manager.BeginTest("Test_PassThroughAuthenticate_NetrLogonSamLogonExS64");
            this.Manager.Comment("reaching state \'S64\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp493;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp493);
            this.Manager.AddReturn(GetPlatformInfo, null, temp493);
            this.Manager.Comment("reaching state \'S65\'");
            int temp503 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonEx.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonExS64GetPlatformChecker)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonEx.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonExS64GetPlatformChecker1)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonEx.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonExS64GetPlatformChecker2)));
            if ((temp503 == 0)) {
                this.Manager.Comment("reaching state \'S222\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp494;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp494 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S411\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp494, "return of NetrServerReqChallenge, state S411");
                this.Manager.Comment("reaching state \'S600\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp495;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp495 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S789\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp495, "return of NetrServerAuthenticate3, state S789");
                this.Manager.Comment("reaching state \'S978\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp496;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonEx(True,PrimaryDc,Client,NetlogonInteractiv" +
                        "eInformation,Valid,NetlogonValidationSamInfo,0)\'");
                temp496 = this.INrpcServerAdapterInstance.NetrLogonSamLogonEx(true, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo, 0u);
                this.Manager.Checkpoint("MS-NRPC_R1174");
                this.Manager.Checkpoint("MS-NRPC_R1160");
                this.Manager.Comment("reaching state \'S1167\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonEx/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp496, "return of NetrLogonSamLogonEx, state S1167");
                this.Manager.Comment("reaching state \'S1356\'");
                goto label44;
            }
            if ((temp503 == 1)) {
                this.Manager.Comment("reaching state \'S223\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp497;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp497 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S412\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp497, "return of NetrServerReqChallenge, state S412");
                this.Manager.Comment("reaching state \'S601\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp498;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp498 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S790\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp498, "return of NetrServerAuthenticate3, state S790");
                this.Manager.Comment("reaching state \'S979\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp499;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonEx(True,PrimaryDc,Client,NetlogonInteractiv" +
                        "eInformation,Valid,NetlogonValidationSamInfo,0)\'");
                temp499 = this.INrpcServerAdapterInstance.NetrLogonSamLogonEx(true, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo, 0u);
                this.Manager.Checkpoint("MS-NRPC_R1174");
                this.Manager.Checkpoint("MS-NRPC_R1160");
                this.Manager.Comment("reaching state \'S1168\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonEx/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp499, "return of NetrLogonSamLogonEx, state S1168");
                this.Manager.Comment("reaching state \'S1357\'");
                goto label44;
            }
            if ((temp503 == 2)) {
                this.Manager.Comment("reaching state \'S224\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp500;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp500 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S413\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp500, "return of NetrServerReqChallenge, state S413");
                this.Manager.Comment("reaching state \'S602\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp501;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp501 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S791\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp501, "return of NetrServerAuthenticate3, state S791");
                this.Manager.Comment("reaching state \'S980\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp502;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonEx(True,PrimaryDc,Client,NetlogonInteractiv" +
                        "eInformation,Valid,NetlogonValidationSamInfo,0)\'");
                temp502 = this.INrpcServerAdapterInstance.NetrLogonSamLogonEx(true, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo, 0u);
                this.Manager.Checkpoint("MS-NRPC_R1174");
                this.Manager.Checkpoint("MS-NRPC_R1160");
                this.Manager.Comment("reaching state \'S1169\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonEx/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp502, "return of NetrLogonSamLogonEx, state S1169");
                this.Manager.Comment("reaching state \'S1358\'");
                goto label44;
            }
            throw new InvalidOperationException("never reached");
        label44:
;
            this.Manager.EndTest();
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonExS64GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S65");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonExS64GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S65");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonExS64GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S65");
        }
        #endregion
        
        #region Test Starting in S66
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_PassThroughAuthenticate_NetrLogonSamLogonExS66() {
            this.Manager.BeginTest("Test_PassThroughAuthenticate_NetrLogonSamLogonExS66");
            this.Manager.Comment("reaching state \'S66\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp504;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp504);
            this.Manager.AddReturn(GetPlatformInfo, null, temp504);
            this.Manager.Comment("reaching state \'S67\'");
            int temp514 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonEx.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonExS66GetPlatformChecker)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonEx.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonExS66GetPlatformChecker1)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonEx.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonExS66GetPlatformChecker2)));
            if ((temp514 == 0)) {
                this.Manager.Comment("reaching state \'S225\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp505;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp505 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S414\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp505, "return of NetrServerReqChallenge, state S414");
                this.Manager.Comment("reaching state \'S603\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp506;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp506 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S792\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp506, "return of NetrServerAuthenticate3, state S792");
                this.Manager.Comment("reaching state \'S981\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp507;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonEx(True,PrimaryDc,Client,NetlogonNetworkInf" +
                        "ormation,Valid,NetlogonValidationSamInfo,0)\'");
                temp507 = this.INrpcServerAdapterInstance.NetrLogonSamLogonEx(true, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonNetworkInformation, ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo, 0u);
                this.Manager.Checkpoint("MS-NRPC_R1160");
                this.Manager.Comment("reaching state \'S1170\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonEx/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp507, "return of NetrLogonSamLogonEx, state S1170");
                this.Manager.Comment("reaching state \'S1359\'");
                goto label45;
            }
            if ((temp514 == 1)) {
                this.Manager.Comment("reaching state \'S226\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp508;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp508 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S415\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp508, "return of NetrServerReqChallenge, state S415");
                this.Manager.Comment("reaching state \'S604\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp509;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp509 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S793\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp509, "return of NetrServerAuthenticate3, state S793");
                this.Manager.Comment("reaching state \'S982\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp510;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonEx(True,PrimaryDc,Client,NetlogonInteractiv" +
                        "eInformation,Valid,NetlogonValidationSamInfo,0)\'");
                temp510 = this.INrpcServerAdapterInstance.NetrLogonSamLogonEx(true, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo, 0u);
                this.Manager.Checkpoint("MS-NRPC_R1174");
                this.Manager.Checkpoint("MS-NRPC_R1160");
                this.Manager.Comment("reaching state \'S1171\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonEx/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp510, "return of NetrLogonSamLogonEx, state S1171");
                this.Manager.Comment("reaching state \'S1360\'");
                goto label45;
            }
            if ((temp514 == 2)) {
                this.Manager.Comment("reaching state \'S227\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp511;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp511 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S416\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp511, "return of NetrServerReqChallenge, state S416");
                this.Manager.Comment("reaching state \'S605\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp512;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp512 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S794\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp512, "return of NetrServerAuthenticate3, state S794");
                this.Manager.Comment("reaching state \'S983\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp513;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonEx(True,PrimaryDc,Client,NetlogonInteractiv" +
                        "eInformation,Valid,NetlogonValidationSamInfo,0)\'");
                temp513 = this.INrpcServerAdapterInstance.NetrLogonSamLogonEx(true, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo, 0u);
                this.Manager.Checkpoint("MS-NRPC_R1174");
                this.Manager.Checkpoint("MS-NRPC_R1160");
                this.Manager.Comment("reaching state \'S1172\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonEx/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp513, "return of NetrLogonSamLogonEx, state S1172");
                this.Manager.Comment("reaching state \'S1361\'");
                goto label45;
            }
            throw new InvalidOperationException("never reached");
        label45:
;
            this.Manager.EndTest();
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonExS66GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S67");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonExS66GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S67");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonExS66GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S67");
        }
        #endregion
        
        #region Test Starting in S68
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_PassThroughAuthenticate_NetrLogonSamLogonExS68() {
            this.Manager.BeginTest("Test_PassThroughAuthenticate_NetrLogonSamLogonExS68");
            this.Manager.Comment("reaching state \'S68\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp515;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp515);
            this.Manager.AddReturn(GetPlatformInfo, null, temp515);
            this.Manager.Comment("reaching state \'S69\'");
            int temp525 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonEx.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonExS68GetPlatformChecker)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonEx.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonExS68GetPlatformChecker1)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonEx.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonExS68GetPlatformChecker2)));
            if ((temp525 == 0)) {
                this.Manager.Comment("reaching state \'S228\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp516;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp516 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S417\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp516, "return of NetrServerReqChallenge, state S417");
                this.Manager.Comment("reaching state \'S606\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp517;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp517 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S795\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp517, "return of NetrServerAuthenticate3, state S795");
                this.Manager.Comment("reaching state \'S984\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp518;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonEx(True,PrimaryDc,Client,NetlogonInteractiv" +
                        "eInformation,Valid,NetlogonValidationSamInfo2,0)\'");
                temp518 = this.INrpcServerAdapterInstance.NetrLogonSamLogonEx(true, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo2, 0u);
                this.Manager.Checkpoint("MS-NRPC_R1174");
                this.Manager.Checkpoint("MS-NRPC_R1160");
                this.Manager.Comment("reaching state \'S1173\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonEx/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp518, "return of NetrLogonSamLogonEx, state S1173");
                this.Manager.Comment("reaching state \'S1362\'");
                goto label46;
            }
            if ((temp525 == 1)) {
                this.Manager.Comment("reaching state \'S229\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp519;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp519 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S418\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp519, "return of NetrServerReqChallenge, state S418");
                this.Manager.Comment("reaching state \'S607\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp520;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp520 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S796\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp520, "return of NetrServerAuthenticate3, state S796");
                this.Manager.Comment("reaching state \'S985\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp521;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonEx(True,PrimaryDc,Client,NetlogonInteractiv" +
                        "eInformation,Valid,NetlogonValidationSamInfo,0)\'");
                temp521 = this.INrpcServerAdapterInstance.NetrLogonSamLogonEx(true, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo, 0u);
                this.Manager.Checkpoint("MS-NRPC_R1174");
                this.Manager.Checkpoint("MS-NRPC_R1160");
                this.Manager.Comment("reaching state \'S1174\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonEx/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp521, "return of NetrLogonSamLogonEx, state S1174");
                this.Manager.Comment("reaching state \'S1363\'");
                goto label46;
            }
            if ((temp525 == 2)) {
                this.Manager.Comment("reaching state \'S230\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp522;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp522 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S419\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp522, "return of NetrServerReqChallenge, state S419");
                this.Manager.Comment("reaching state \'S608\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp523;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp523 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S797\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp523, "return of NetrServerAuthenticate3, state S797");
                this.Manager.Comment("reaching state \'S986\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp524;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonEx(True,PrimaryDc,Client,NetlogonInteractiv" +
                        "eInformation,Valid,NetlogonValidationSamInfo,0)\'");
                temp524 = this.INrpcServerAdapterInstance.NetrLogonSamLogonEx(true, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo, 0u);
                this.Manager.Checkpoint("MS-NRPC_R1174");
                this.Manager.Checkpoint("MS-NRPC_R1160");
                this.Manager.Comment("reaching state \'S1175\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonEx/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp524, "return of NetrLogonSamLogonEx, state S1175");
                this.Manager.Comment("reaching state \'S1364\'");
                goto label46;
            }
            throw new InvalidOperationException("never reached");
        label46:
;
            this.Manager.EndTest();
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonExS68GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S69");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonExS68GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S69");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonExS68GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S69");
        }
        #endregion
        
        #region Test Starting in S70
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_PassThroughAuthenticate_NetrLogonSamLogonExS70() {
            this.Manager.BeginTest("Test_PassThroughAuthenticate_NetrLogonSamLogonExS70");
            this.Manager.Comment("reaching state \'S70\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp526;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp526);
            this.Manager.AddReturn(GetPlatformInfo, null, temp526);
            this.Manager.Comment("reaching state \'S71\'");
            int temp536 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonEx.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonExS70GetPlatformChecker)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonEx.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonExS70GetPlatformChecker1)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonEx.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonExS70GetPlatformChecker2)));
            if ((temp536 == 0)) {
                this.Manager.Comment("reaching state \'S231\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp527;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp527 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S420\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp527, "return of NetrServerReqChallenge, state S420");
                this.Manager.Comment("reaching state \'S609\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp528;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp528 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S798\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp528, "return of NetrServerAuthenticate3, state S798");
                this.Manager.Comment("reaching state \'S987\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp529;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonEx(True,PrimaryDc,Client,NetlogonServiceInf" +
                        "ormation,Valid,NetlogonValidationSamInfo,0)\'");
                temp529 = this.INrpcServerAdapterInstance.NetrLogonSamLogonEx(true, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonServiceInformation, ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo, 0u);
                this.Manager.Checkpoint("MS-NRPC_R1177");
                this.Manager.Checkpoint("MS-NRPC_R1160");
                this.Manager.Comment("reaching state \'S1176\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonEx/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp529, "return of NetrLogonSamLogonEx, state S1176");
                this.Manager.Comment("reaching state \'S1365\'");
                goto label47;
            }
            if ((temp536 == 1)) {
                this.Manager.Comment("reaching state \'S232\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp530;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp530 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S421\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp530, "return of NetrServerReqChallenge, state S421");
                this.Manager.Comment("reaching state \'S610\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp531;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp531 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S799\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp531, "return of NetrServerAuthenticate3, state S799");
                this.Manager.Comment("reaching state \'S988\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp532;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonEx(True,PrimaryDc,Client,NetlogonInteractiv" +
                        "eInformation,Valid,NetlogonValidationSamInfo,0)\'");
                temp532 = this.INrpcServerAdapterInstance.NetrLogonSamLogonEx(true, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo, 0u);
                this.Manager.Checkpoint("MS-NRPC_R1174");
                this.Manager.Checkpoint("MS-NRPC_R1160");
                this.Manager.Comment("reaching state \'S1177\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonEx/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp532, "return of NetrLogonSamLogonEx, state S1177");
                this.Manager.Comment("reaching state \'S1366\'");
                goto label47;
            }
            if ((temp536 == 2)) {
                this.Manager.Comment("reaching state \'S233\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp533;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp533 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S422\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp533, "return of NetrServerReqChallenge, state S422");
                this.Manager.Comment("reaching state \'S611\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp534;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp534 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S800\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp534, "return of NetrServerAuthenticate3, state S800");
                this.Manager.Comment("reaching state \'S989\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp535;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonEx(True,PrimaryDc,Client,NetlogonInteractiv" +
                        "eInformation,Valid,NetlogonValidationSamInfo,0)\'");
                temp535 = this.INrpcServerAdapterInstance.NetrLogonSamLogonEx(true, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo, 0u);
                this.Manager.Checkpoint("MS-NRPC_R1174");
                this.Manager.Checkpoint("MS-NRPC_R1160");
                this.Manager.Comment("reaching state \'S1178\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonEx/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp535, "return of NetrLogonSamLogonEx, state S1178");
                this.Manager.Comment("reaching state \'S1367\'");
                goto label47;
            }
            throw new InvalidOperationException("never reached");
        label47:
;
            this.Manager.EndTest();
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonExS70GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S71");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonExS70GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S71");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonExS70GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S71");
        }
        #endregion
        
        #region Test Starting in S72
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_PassThroughAuthenticate_NetrLogonSamLogonExS72() {
            this.Manager.BeginTest("Test_PassThroughAuthenticate_NetrLogonSamLogonExS72");
            this.Manager.Comment("reaching state \'S72\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp537;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp537);
            this.Manager.AddReturn(GetPlatformInfo, null, temp537);
            this.Manager.Comment("reaching state \'S73\'");
            int temp547 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonEx.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonExS72GetPlatformChecker)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonEx.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonExS72GetPlatformChecker1)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonEx.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonExS72GetPlatformChecker2)));
            if ((temp547 == 0)) {
                this.Manager.Comment("reaching state \'S234\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp538;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp538 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S423\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp538, "return of NetrServerReqChallenge, state S423");
                this.Manager.Comment("reaching state \'S612\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp539;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp539 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S801\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp539, "return of NetrServerAuthenticate3, state S801");
                this.Manager.Comment("reaching state \'S990\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp540;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonEx(True,PrimaryDc,Client,NetlogonNetworkInf" +
                        "ormation,Valid,NetlogonValidationSamInfo2,0)\'");
                temp540 = this.INrpcServerAdapterInstance.NetrLogonSamLogonEx(true, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonNetworkInformation, ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo2, 0u);
                this.Manager.Checkpoint("MS-NRPC_R1160");
                this.Manager.Comment("reaching state \'S1179\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonEx/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp540, "return of NetrLogonSamLogonEx, state S1179");
                this.Manager.Comment("reaching state \'S1368\'");
                goto label48;
            }
            if ((temp547 == 1)) {
                this.Manager.Comment("reaching state \'S235\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp541;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp541 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S424\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp541, "return of NetrServerReqChallenge, state S424");
                this.Manager.Comment("reaching state \'S613\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp542;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp542 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S802\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp542, "return of NetrServerAuthenticate3, state S802");
                this.Manager.Comment("reaching state \'S991\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp543;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonEx(True,PrimaryDc,Client,NetlogonInteractiv" +
                        "eInformation,Valid,NetlogonValidationSamInfo,0)\'");
                temp543 = this.INrpcServerAdapterInstance.NetrLogonSamLogonEx(true, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo, 0u);
                this.Manager.Checkpoint("MS-NRPC_R1174");
                this.Manager.Checkpoint("MS-NRPC_R1160");
                this.Manager.Comment("reaching state \'S1180\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonEx/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp543, "return of NetrLogonSamLogonEx, state S1180");
                this.Manager.Comment("reaching state \'S1369\'");
                goto label48;
            }
            if ((temp547 == 2)) {
                this.Manager.Comment("reaching state \'S236\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp544;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp544 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S425\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp544, "return of NetrServerReqChallenge, state S425");
                this.Manager.Comment("reaching state \'S614\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp545;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp545 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S803\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp545, "return of NetrServerAuthenticate3, state S803");
                this.Manager.Comment("reaching state \'S992\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp546;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonEx(True,PrimaryDc,Client,NetlogonInteractiv" +
                        "eInformation,Valid,NetlogonValidationSamInfo,0)\'");
                temp546 = this.INrpcServerAdapterInstance.NetrLogonSamLogonEx(true, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo, 0u);
                this.Manager.Checkpoint("MS-NRPC_R1174");
                this.Manager.Checkpoint("MS-NRPC_R1160");
                this.Manager.Comment("reaching state \'S1181\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonEx/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp546, "return of NetrLogonSamLogonEx, state S1181");
                this.Manager.Comment("reaching state \'S1370\'");
                goto label48;
            }
            throw new InvalidOperationException("never reached");
        label48:
;
            this.Manager.EndTest();
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonExS72GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S73");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonExS72GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S73");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonExS72GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S73");
        }
        #endregion
        
        #region Test Starting in S74
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_PassThroughAuthenticate_NetrLogonSamLogonExS74() {
            this.Manager.BeginTest("Test_PassThroughAuthenticate_NetrLogonSamLogonExS74");
            this.Manager.Comment("reaching state \'S74\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp548;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp548);
            this.Manager.AddReturn(GetPlatformInfo, null, temp548);
            this.Manager.Comment("reaching state \'S75\'");
            int temp558 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonEx.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonExS74GetPlatformChecker)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonEx.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonExS74GetPlatformChecker1)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonEx.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonExS74GetPlatformChecker2)));
            if ((temp558 == 0)) {
                this.Manager.Comment("reaching state \'S237\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp549;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp549 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S426\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp549, "return of NetrServerReqChallenge, state S426");
                this.Manager.Comment("reaching state \'S615\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp550;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp550 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S804\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp550, "return of NetrServerAuthenticate3, state S804");
                this.Manager.Comment("reaching state \'S993\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp551;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonEx(True,PrimaryDc,Client,NetlogonInteractiv" +
                        "eInformation,Valid,NetlogonValidationGenericInfo2,0)\'");
                temp551 = this.INrpcServerAdapterInstance.NetrLogonSamLogonEx(true, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationGenericInfo2, 0u);
                this.Manager.Checkpoint("MS-NRPC_R1161");
                this.Manager.Comment("reaching state \'S1182\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonEx/STATUS_INVALID_INFO_CLASS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_INVALID_INFO_CLASS, temp551, "return of NetrLogonSamLogonEx, state S1182");
                this.Manager.Comment("reaching state \'S1371\'");
                goto label49;
            }
            if ((temp558 == 1)) {
                this.Manager.Comment("reaching state \'S238\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp552;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp552 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S427\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp552, "return of NetrServerReqChallenge, state S427");
                this.Manager.Comment("reaching state \'S616\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp553;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp553 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S805\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp553, "return of NetrServerAuthenticate3, state S805");
                this.Manager.Comment("reaching state \'S994\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp554;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonEx(True,PrimaryDc,Client,NetlogonInteractiv" +
                        "eInformation,Valid,NetlogonValidationSamInfo,0)\'");
                temp554 = this.INrpcServerAdapterInstance.NetrLogonSamLogonEx(true, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo, 0u);
                this.Manager.Checkpoint("MS-NRPC_R1174");
                this.Manager.Checkpoint("MS-NRPC_R1160");
                this.Manager.Comment("reaching state \'S1183\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonEx/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp554, "return of NetrLogonSamLogonEx, state S1183");
                this.Manager.Comment("reaching state \'S1372\'");
                goto label49;
            }
            if ((temp558 == 2)) {
                this.Manager.Comment("reaching state \'S239\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp555;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp555 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S428\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp555, "return of NetrServerReqChallenge, state S428");
                this.Manager.Comment("reaching state \'S617\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp556;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp556 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S806\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp556, "return of NetrServerAuthenticate3, state S806");
                this.Manager.Comment("reaching state \'S995\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp557;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonEx(True,PrimaryDc,Client,NetlogonInteractiv" +
                        "eInformation,Valid,NetlogonValidationSamInfo,0)\'");
                temp557 = this.INrpcServerAdapterInstance.NetrLogonSamLogonEx(true, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo, 0u);
                this.Manager.Checkpoint("MS-NRPC_R1174");
                this.Manager.Checkpoint("MS-NRPC_R1160");
                this.Manager.Comment("reaching state \'S1184\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonEx/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp557, "return of NetrLogonSamLogonEx, state S1184");
                this.Manager.Comment("reaching state \'S1373\'");
                goto label49;
            }
            throw new InvalidOperationException("never reached");
        label49:
;
            this.Manager.EndTest();
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonExS74GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S75");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonExS74GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S75");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonExS74GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S75");
        }
        #endregion
        
        #region Test Starting in S76
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_PassThroughAuthenticate_NetrLogonSamLogonExS76() {
            this.Manager.BeginTest("Test_PassThroughAuthenticate_NetrLogonSamLogonExS76");
            this.Manager.Comment("reaching state \'S76\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp559;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp559);
            this.Manager.AddReturn(GetPlatformInfo, null, temp559);
            this.Manager.Comment("reaching state \'S77\'");
            int temp569 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonEx.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonExS76GetPlatformChecker)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonEx.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonExS76GetPlatformChecker1)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonEx.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonExS76GetPlatformChecker2)));
            if ((temp569 == 0)) {
                this.Manager.Comment("reaching state \'S240\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp560;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp560 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S429\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp560, "return of NetrServerReqChallenge, state S429");
                this.Manager.Comment("reaching state \'S618\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp561;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp561 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S807\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp561, "return of NetrServerAuthenticate3, state S807");
                this.Manager.Comment("reaching state \'S996\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp562;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonEx(True,PrimaryDc,Client,NetlogonInteractiv" +
                        "eInformation,Valid,NetlogonValidationSamInfo4,0)\'");
                temp562 = this.INrpcServerAdapterInstance.NetrLogonSamLogonEx(true, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo4, 0u);
                this.Manager.Checkpoint("MS-NRPC_R1174");
                this.Manager.Checkpoint("MS-NRPC_R1160");
                this.Manager.Comment("reaching state \'S1185\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonEx/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp562, "return of NetrLogonSamLogonEx, state S1185");
                this.Manager.Comment("reaching state \'S1374\'");
                goto label50;
            }
            if ((temp569 == 1)) {
                this.Manager.Comment("reaching state \'S241\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp563;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp563 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S430\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp563, "return of NetrServerReqChallenge, state S430");
                this.Manager.Comment("reaching state \'S619\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp564;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp564 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S808\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp564, "return of NetrServerAuthenticate3, state S808");
                this.Manager.Comment("reaching state \'S997\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp565;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonEx(True,PrimaryDc,Client,NetlogonInteractiv" +
                        "eInformation,Valid,NetlogonValidationSamInfo,0)\'");
                temp565 = this.INrpcServerAdapterInstance.NetrLogonSamLogonEx(true, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo, 0u);
                this.Manager.Checkpoint("MS-NRPC_R1174");
                this.Manager.Checkpoint("MS-NRPC_R1160");
                this.Manager.Comment("reaching state \'S1186\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonEx/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp565, "return of NetrLogonSamLogonEx, state S1186");
                this.Manager.Comment("reaching state \'S1375\'");
                goto label50;
            }
            if ((temp569 == 2)) {
                this.Manager.Comment("reaching state \'S242\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp566;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp566 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S431\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp566, "return of NetrServerReqChallenge, state S431");
                this.Manager.Comment("reaching state \'S620\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp567;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp567 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S809\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp567, "return of NetrServerAuthenticate3, state S809");
                this.Manager.Comment("reaching state \'S998\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp568;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonEx(True,PrimaryDc,Client,NetlogonInteractiv" +
                        "eInformation,Valid,NetlogonValidationSamInfo,0)\'");
                temp568 = this.INrpcServerAdapterInstance.NetrLogonSamLogonEx(true, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo, 0u);
                this.Manager.Checkpoint("MS-NRPC_R1174");
                this.Manager.Checkpoint("MS-NRPC_R1160");
                this.Manager.Comment("reaching state \'S1187\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonEx/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp568, "return of NetrLogonSamLogonEx, state S1187");
                this.Manager.Comment("reaching state \'S1376\'");
                goto label50;
            }
            throw new InvalidOperationException("never reached");
        label50:
;
            this.Manager.EndTest();
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonExS76GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S77");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonExS76GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S77");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonExS76GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S77");
        }
        #endregion
        
        #region Test Starting in S78
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_PassThroughAuthenticate_NetrLogonSamLogonExS78() {
            this.Manager.BeginTest("Test_PassThroughAuthenticate_NetrLogonSamLogonExS78");
            this.Manager.Comment("reaching state \'S78\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp570;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp570);
            this.Manager.AddReturn(GetPlatformInfo, null, temp570);
            this.Manager.Comment("reaching state \'S79\'");
            int temp580 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonEx.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonExS78GetPlatformChecker)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonEx.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonExS78GetPlatformChecker1)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonEx.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonExS78GetPlatformChecker2)));
            if ((temp580 == 0)) {
                this.Manager.Comment("reaching state \'S243\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp571;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp571 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S432\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp571, "return of NetrServerReqChallenge, state S432");
                this.Manager.Comment("reaching state \'S621\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp572;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp572 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S810\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp572, "return of NetrServerAuthenticate3, state S810");
                this.Manager.Comment("reaching state \'S999\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp573;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonEx(True,PrimaryDc,Client,NetlogonInteractiv" +
                        "eTransitiveInformation,Valid,NetlogonValidationSamInfo,0)\'");
                temp573 = this.INrpcServerAdapterInstance.NetrLogonSamLogonEx(true, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonInteractiveTransitiveInformation, ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo, 0u);
                this.Manager.Checkpoint("MS-NRPC_R1175");
                this.Manager.Checkpoint("MS-NRPC_R1160");
                this.Manager.Comment("reaching state \'S1188\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonEx/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp573, "return of NetrLogonSamLogonEx, state S1188");
                this.Manager.Comment("reaching state \'S1377\'");
                goto label51;
            }
            if ((temp580 == 1)) {
                this.Manager.Comment("reaching state \'S244\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp574;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp574 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S433\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp574, "return of NetrServerReqChallenge, state S433");
                this.Manager.Comment("reaching state \'S622\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp575;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp575 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S811\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp575, "return of NetrServerAuthenticate3, state S811");
                this.Manager.Comment("reaching state \'S1000\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp576;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonEx(True,PrimaryDc,Client,NetlogonInteractiv" +
                        "eInformation,Valid,NetlogonValidationSamInfo,0)\'");
                temp576 = this.INrpcServerAdapterInstance.NetrLogonSamLogonEx(true, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo, 0u);
                this.Manager.Checkpoint("MS-NRPC_R1174");
                this.Manager.Checkpoint("MS-NRPC_R1160");
                this.Manager.Comment("reaching state \'S1189\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonEx/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp576, "return of NetrLogonSamLogonEx, state S1189");
                this.Manager.Comment("reaching state \'S1378\'");
                goto label51;
            }
            if ((temp580 == 2)) {
                this.Manager.Comment("reaching state \'S245\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp577;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp577 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S434\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp577, "return of NetrServerReqChallenge, state S434");
                this.Manager.Comment("reaching state \'S623\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp578;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp578 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S812\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp578, "return of NetrServerAuthenticate3, state S812");
                this.Manager.Comment("reaching state \'S1001\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp579;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonEx(True,PrimaryDc,Client,NetlogonInteractiv" +
                        "eInformation,Valid,NetlogonValidationSamInfo,0)\'");
                temp579 = this.INrpcServerAdapterInstance.NetrLogonSamLogonEx(true, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo, 0u);
                this.Manager.Checkpoint("MS-NRPC_R1174");
                this.Manager.Checkpoint("MS-NRPC_R1160");
                this.Manager.Comment("reaching state \'S1190\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonEx/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp579, "return of NetrLogonSamLogonEx, state S1190");
                this.Manager.Comment("reaching state \'S1379\'");
                goto label51;
            }
            throw new InvalidOperationException("never reached");
        label51:
;
            this.Manager.EndTest();
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonExS78GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S79");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonExS78GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S79");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonExS78GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S79");
        }
        #endregion
        
        #region Test Starting in S8
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_PassThroughAuthenticate_NetrLogonSamLogonExS8() {
            this.Manager.BeginTest("Test_PassThroughAuthenticate_NetrLogonSamLogonExS8");
            this.Manager.Comment("reaching state \'S8\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp581;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp581);
            this.Manager.AddReturn(GetPlatformInfo, null, temp581);
            this.Manager.Comment("reaching state \'S9\'");
            int temp591 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonEx.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonExS8GetPlatformChecker)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonEx.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonExS8GetPlatformChecker1)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonEx.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonExS8GetPlatformChecker2)));
            if ((temp591 == 0)) {
                this.Manager.Comment("reaching state \'S138\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp582;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp582 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S327\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp582, "return of NetrServerReqChallenge, state S327");
                this.Manager.Comment("reaching state \'S516\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp583;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp583 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S705\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp583, "return of NetrServerAuthenticate3, state S705");
                this.Manager.Comment("reaching state \'S894\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp584;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonEx(True,PrimaryDc,Client,NetlogonServiceInf" +
                        "ormation,Valid,NetlogonValidationSamInfo,0)\'");
                temp584 = this.INrpcServerAdapterInstance.NetrLogonSamLogonEx(true, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonServiceInformation, ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo, 0u);
                this.Manager.Checkpoint("MS-NRPC_R1177");
                this.Manager.Checkpoint("MS-NRPC_R1160");
                this.Manager.Comment("reaching state \'S1083\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonEx/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp584, "return of NetrLogonSamLogonEx, state S1083");
                this.Manager.Comment("reaching state \'S1272\'");
                goto label52;
            }
            if ((temp591 == 1)) {
                this.Manager.Comment("reaching state \'S139\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp585;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp585 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S328\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp585, "return of NetrServerReqChallenge, state S328");
                this.Manager.Comment("reaching state \'S517\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp586;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp586 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S706\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp586, "return of NetrServerAuthenticate3, state S706");
                this.Manager.Comment("reaching state \'S895\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp587;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonEx(True,PrimaryDc,Client,NetlogonServiceInf" +
                        "ormation,Valid,NetlogonValidationSamInfo,0)\'");
                temp587 = this.INrpcServerAdapterInstance.NetrLogonSamLogonEx(true, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonServiceInformation, ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo, 0u);
                this.Manager.Checkpoint("MS-NRPC_R1177");
                this.Manager.Checkpoint("MS-NRPC_R1160");
                this.Manager.Comment("reaching state \'S1084\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonEx/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp587, "return of NetrLogonSamLogonEx, state S1084");
                this.Manager.Comment("reaching state \'S1273\'");
                goto label52;
            }
            if ((temp591 == 2)) {
                this.Manager.Comment("reaching state \'S140\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp588;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp588 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S329\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp588, "return of NetrServerReqChallenge, state S329");
                this.Manager.Comment("reaching state \'S518\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp589;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp589 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S707\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp589, "return of NetrServerAuthenticate3, state S707");
                this.Manager.Comment("reaching state \'S896\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp590;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonEx(True,PrimaryDc,Client,NetlogonInteractiv" +
                        "eInformation,Valid,NetlogonValidationSamInfo,0)\'");
                temp590 = this.INrpcServerAdapterInstance.NetrLogonSamLogonEx(true, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo, 0u);
                this.Manager.Checkpoint("MS-NRPC_R1174");
                this.Manager.Checkpoint("MS-NRPC_R1160");
                this.Manager.Comment("reaching state \'S1085\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonEx/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp590, "return of NetrLogonSamLogonEx, state S1085");
                this.Manager.Comment("reaching state \'S1274\'");
                goto label52;
            }
            throw new InvalidOperationException("never reached");
        label52:
;
            this.Manager.EndTest();
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonExS8GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S9");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonExS8GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S9");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonExS8GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S9");
        }
        #endregion
        
        #region Test Starting in S80
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_PassThroughAuthenticate_NetrLogonSamLogonExS80() {
            this.Manager.BeginTest("Test_PassThroughAuthenticate_NetrLogonSamLogonExS80");
            this.Manager.Comment("reaching state \'S80\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp592;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp592);
            this.Manager.AddReturn(GetPlatformInfo, null, temp592);
            this.Manager.Comment("reaching state \'S81\'");
            int temp602 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonEx.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonExS80GetPlatformChecker)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonEx.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonExS80GetPlatformChecker1)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonEx.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonExS80GetPlatformChecker2)));
            if ((temp602 == 0)) {
                this.Manager.Comment("reaching state \'S246\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp593;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp593 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S435\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp593, "return of NetrServerReqChallenge, state S435");
                this.Manager.Comment("reaching state \'S624\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp594;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp594 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S813\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp594, "return of NetrServerAuthenticate3, state S813");
                this.Manager.Comment("reaching state \'S1002\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp595;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonEx(True,PrimaryDc,Client,NetlogonServiceInf" +
                        "ormation,Valid,NetlogonValidationSamInfo2,0)\'");
                temp595 = this.INrpcServerAdapterInstance.NetrLogonSamLogonEx(true, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonServiceInformation, ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo2, 0u);
                this.Manager.Checkpoint("MS-NRPC_R1177");
                this.Manager.Checkpoint("MS-NRPC_R1160");
                this.Manager.Comment("reaching state \'S1191\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonEx/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp595, "return of NetrLogonSamLogonEx, state S1191");
                this.Manager.Comment("reaching state \'S1380\'");
                goto label53;
            }
            if ((temp602 == 1)) {
                this.Manager.Comment("reaching state \'S247\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp596;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp596 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S436\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp596, "return of NetrServerReqChallenge, state S436");
                this.Manager.Comment("reaching state \'S625\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp597;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp597 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S814\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp597, "return of NetrServerAuthenticate3, state S814");
                this.Manager.Comment("reaching state \'S1003\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp598;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonEx(True,PrimaryDc,Client,NetlogonInteractiv" +
                        "eInformation,Valid,NetlogonValidationSamInfo,0)\'");
                temp598 = this.INrpcServerAdapterInstance.NetrLogonSamLogonEx(true, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo, 0u);
                this.Manager.Checkpoint("MS-NRPC_R1174");
                this.Manager.Checkpoint("MS-NRPC_R1160");
                this.Manager.Comment("reaching state \'S1192\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonEx/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp598, "return of NetrLogonSamLogonEx, state S1192");
                this.Manager.Comment("reaching state \'S1381\'");
                goto label53;
            }
            if ((temp602 == 2)) {
                this.Manager.Comment("reaching state \'S248\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp599;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp599 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S437\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp599, "return of NetrServerReqChallenge, state S437");
                this.Manager.Comment("reaching state \'S626\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp600;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp600 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S815\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp600, "return of NetrServerAuthenticate3, state S815");
                this.Manager.Comment("reaching state \'S1004\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp601;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonEx(True,PrimaryDc,Client,NetlogonInteractiv" +
                        "eInformation,Valid,NetlogonValidationSamInfo,0)\'");
                temp601 = this.INrpcServerAdapterInstance.NetrLogonSamLogonEx(true, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo, 0u);
                this.Manager.Checkpoint("MS-NRPC_R1174");
                this.Manager.Checkpoint("MS-NRPC_R1160");
                this.Manager.Comment("reaching state \'S1193\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonEx/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp601, "return of NetrLogonSamLogonEx, state S1193");
                this.Manager.Comment("reaching state \'S1382\'");
                goto label53;
            }
            throw new InvalidOperationException("never reached");
        label53:
;
            this.Manager.EndTest();
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonExS80GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S81");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonExS80GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S81");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonExS80GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S81");
        }
        #endregion
        
        #region Test Starting in S82
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_PassThroughAuthenticate_NetrLogonSamLogonExS82() {
            this.Manager.BeginTest("Test_PassThroughAuthenticate_NetrLogonSamLogonExS82");
            this.Manager.Comment("reaching state \'S82\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp603;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp603);
            this.Manager.AddReturn(GetPlatformInfo, null, temp603);
            this.Manager.Comment("reaching state \'S83\'");
            int temp613 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonEx.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonExS82GetPlatformChecker)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonEx.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonExS82GetPlatformChecker1)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonEx.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonExS82GetPlatformChecker2)));
            if ((temp613 == 0)) {
                this.Manager.Comment("reaching state \'S249\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp604;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp604 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S438\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp604, "return of NetrServerReqChallenge, state S438");
                this.Manager.Comment("reaching state \'S627\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp605;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp605 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S816\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp605, "return of NetrServerAuthenticate3, state S816");
                this.Manager.Comment("reaching state \'S1005\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp606;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonEx(True,PrimaryDc,Client,NetlogonNetworkInf" +
                        "ormation,Valid,NetlogonValidationGenericInfo2,0)\'");
                temp606 = this.INrpcServerAdapterInstance.NetrLogonSamLogonEx(true, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonNetworkInformation, ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationGenericInfo2, 0u);
                this.Manager.Checkpoint("MS-NRPC_R101196");
                this.Manager.Checkpoint("MS-NRPC_R1161");
                this.Manager.Comment("reaching state \'S1194\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonEx/STATUS_INVALID_INFO_CLASS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_INVALID_INFO_CLASS, temp606, "return of NetrLogonSamLogonEx, state S1194");
                this.Manager.Comment("reaching state \'S1383\'");
                goto label54;
            }
            if ((temp613 == 1)) {
                this.Manager.Comment("reaching state \'S250\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp607;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp607 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S439\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp607, "return of NetrServerReqChallenge, state S439");
                this.Manager.Comment("reaching state \'S628\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp608;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp608 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S817\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp608, "return of NetrServerAuthenticate3, state S817");
                this.Manager.Comment("reaching state \'S1006\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp609;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonEx(True,PrimaryDc,Client,NetlogonInteractiv" +
                        "eInformation,Valid,NetlogonValidationSamInfo,0)\'");
                temp609 = this.INrpcServerAdapterInstance.NetrLogonSamLogonEx(true, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo, 0u);
                this.Manager.Checkpoint("MS-NRPC_R1174");
                this.Manager.Checkpoint("MS-NRPC_R1160");
                this.Manager.Comment("reaching state \'S1195\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonEx/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp609, "return of NetrLogonSamLogonEx, state S1195");
                this.Manager.Comment("reaching state \'S1384\'");
                goto label54;
            }
            if ((temp613 == 2)) {
                this.Manager.Comment("reaching state \'S251\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp610;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp610 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S440\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp610, "return of NetrServerReqChallenge, state S440");
                this.Manager.Comment("reaching state \'S629\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp611;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp611 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S818\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp611, "return of NetrServerAuthenticate3, state S818");
                this.Manager.Comment("reaching state \'S1007\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp612;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonEx(True,PrimaryDc,Client,NetlogonInteractiv" +
                        "eInformation,Valid,NetlogonValidationSamInfo,0)\'");
                temp612 = this.INrpcServerAdapterInstance.NetrLogonSamLogonEx(true, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo, 0u);
                this.Manager.Checkpoint("MS-NRPC_R1174");
                this.Manager.Checkpoint("MS-NRPC_R1160");
                this.Manager.Comment("reaching state \'S1196\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonEx/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp612, "return of NetrLogonSamLogonEx, state S1196");
                this.Manager.Comment("reaching state \'S1385\'");
                goto label54;
            }
            throw new InvalidOperationException("never reached");
        label54:
;
            this.Manager.EndTest();
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonExS82GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S83");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonExS82GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S83");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonExS82GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S83");
        }
        #endregion
        
        #region Test Starting in S84
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_PassThroughAuthenticate_NetrLogonSamLogonExS84() {
            this.Manager.BeginTest("Test_PassThroughAuthenticate_NetrLogonSamLogonExS84");
            this.Manager.Comment("reaching state \'S84\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp614;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp614);
            this.Manager.AddReturn(GetPlatformInfo, null, temp614);
            this.Manager.Comment("reaching state \'S85\'");
            int temp624 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonEx.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonExS84GetPlatformChecker)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonEx.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonExS84GetPlatformChecker1)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonEx.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonExS84GetPlatformChecker2)));
            if ((temp624 == 0)) {
                this.Manager.Comment("reaching state \'S252\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp615;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp615 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S441\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp615, "return of NetrServerReqChallenge, state S441");
                this.Manager.Comment("reaching state \'S630\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp616;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp616 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S819\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp616, "return of NetrServerAuthenticate3, state S819");
                this.Manager.Comment("reaching state \'S1008\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp617;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonEx(True,PrimaryDc,Client,NetlogonNetworkInf" +
                        "ormation,Valid,NetlogonValidationSamInfo4,0)\'");
                temp617 = this.INrpcServerAdapterInstance.NetrLogonSamLogonEx(true, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonNetworkInformation, ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo4, 0u);
                this.Manager.Checkpoint("MS-NRPC_R1160");
                this.Manager.Comment("reaching state \'S1197\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonEx/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp617, "return of NetrLogonSamLogonEx, state S1197");
                this.Manager.Comment("reaching state \'S1386\'");
                goto label55;
            }
            if ((temp624 == 1)) {
                this.Manager.Comment("reaching state \'S253\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp618;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp618 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S442\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp618, "return of NetrServerReqChallenge, state S442");
                this.Manager.Comment("reaching state \'S631\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp619;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp619 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S820\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp619, "return of NetrServerAuthenticate3, state S820");
                this.Manager.Comment("reaching state \'S1009\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp620;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonEx(True,PrimaryDc,Client,NetlogonInteractiv" +
                        "eInformation,Valid,NetlogonValidationSamInfo,0)\'");
                temp620 = this.INrpcServerAdapterInstance.NetrLogonSamLogonEx(true, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo, 0u);
                this.Manager.Checkpoint("MS-NRPC_R1174");
                this.Manager.Checkpoint("MS-NRPC_R1160");
                this.Manager.Comment("reaching state \'S1198\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonEx/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp620, "return of NetrLogonSamLogonEx, state S1198");
                this.Manager.Comment("reaching state \'S1387\'");
                goto label55;
            }
            if ((temp624 == 2)) {
                this.Manager.Comment("reaching state \'S254\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp621;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp621 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S443\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp621, "return of NetrServerReqChallenge, state S443");
                this.Manager.Comment("reaching state \'S632\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp622;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp622 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S821\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp622, "return of NetrServerAuthenticate3, state S821");
                this.Manager.Comment("reaching state \'S1010\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp623;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonEx(True,PrimaryDc,Client,NetlogonInteractiv" +
                        "eInformation,Valid,NetlogonValidationSamInfo,0)\'");
                temp623 = this.INrpcServerAdapterInstance.NetrLogonSamLogonEx(true, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo, 0u);
                this.Manager.Checkpoint("MS-NRPC_R1174");
                this.Manager.Checkpoint("MS-NRPC_R1160");
                this.Manager.Comment("reaching state \'S1199\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonEx/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp623, "return of NetrLogonSamLogonEx, state S1199");
                this.Manager.Comment("reaching state \'S1388\'");
                goto label55;
            }
            throw new InvalidOperationException("never reached");
        label55:
;
            this.Manager.EndTest();
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonExS84GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S85");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonExS84GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S85");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonExS84GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S85");
        }
        #endregion
        
        #region Test Starting in S86
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_PassThroughAuthenticate_NetrLogonSamLogonExS86() {
            this.Manager.BeginTest("Test_PassThroughAuthenticate_NetrLogonSamLogonExS86");
            this.Manager.Comment("reaching state \'S86\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp625;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp625);
            this.Manager.AddReturn(GetPlatformInfo, null, temp625);
            this.Manager.Comment("reaching state \'S87\'");
            int temp635 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonEx.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonExS86GetPlatformChecker)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonEx.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonExS86GetPlatformChecker1)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonEx.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonExS86GetPlatformChecker2)));
            if ((temp635 == 0)) {
                this.Manager.Comment("reaching state \'S255\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp626;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp626 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S444\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp626, "return of NetrServerReqChallenge, state S444");
                this.Manager.Comment("reaching state \'S633\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp627;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp627 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S822\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp627, "return of NetrServerAuthenticate3, state S822");
                this.Manager.Comment("reaching state \'S1011\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp628;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonEx(True,PrimaryDc,Client,NetlogonServiceTra" +
                        "nsitiveInformation,Valid,NetlogonValidationSamInfo,0)\'");
                temp628 = this.INrpcServerAdapterInstance.NetrLogonSamLogonEx(true, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonServiceTransitiveInformation, ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo, 0u);
                this.Manager.Checkpoint("MS-NRPC_R101177");
                this.Manager.Checkpoint("MS-NRPC_R1160");
                this.Manager.Comment("reaching state \'S1200\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonEx/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp628, "return of NetrLogonSamLogonEx, state S1200");
                this.Manager.Comment("reaching state \'S1389\'");
                goto label56;
            }
            if ((temp635 == 1)) {
                this.Manager.Comment("reaching state \'S256\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp629;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp629 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S445\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp629, "return of NetrServerReqChallenge, state S445");
                this.Manager.Comment("reaching state \'S634\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp630;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp630 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S823\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp630, "return of NetrServerAuthenticate3, state S823");
                this.Manager.Comment("reaching state \'S1012\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp631;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonEx(True,PrimaryDc,Client,NetlogonInteractiv" +
                        "eInformation,Valid,NetlogonValidationSamInfo,0)\'");
                temp631 = this.INrpcServerAdapterInstance.NetrLogonSamLogonEx(true, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo, 0u);
                this.Manager.Checkpoint("MS-NRPC_R1174");
                this.Manager.Checkpoint("MS-NRPC_R1160");
                this.Manager.Comment("reaching state \'S1201\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonEx/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp631, "return of NetrLogonSamLogonEx, state S1201");
                this.Manager.Comment("reaching state \'S1390\'");
                goto label56;
            }
            if ((temp635 == 2)) {
                this.Manager.Comment("reaching state \'S257\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp632;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp632 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S446\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp632, "return of NetrServerReqChallenge, state S446");
                this.Manager.Comment("reaching state \'S635\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp633;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp633 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S824\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp633, "return of NetrServerAuthenticate3, state S824");
                this.Manager.Comment("reaching state \'S1013\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp634;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonEx(True,PrimaryDc,Client,NetlogonInteractiv" +
                        "eInformation,Valid,NetlogonValidationSamInfo,0)\'");
                temp634 = this.INrpcServerAdapterInstance.NetrLogonSamLogonEx(true, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo, 0u);
                this.Manager.Checkpoint("MS-NRPC_R1174");
                this.Manager.Checkpoint("MS-NRPC_R1160");
                this.Manager.Comment("reaching state \'S1202\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonEx/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp634, "return of NetrLogonSamLogonEx, state S1202");
                this.Manager.Comment("reaching state \'S1391\'");
                goto label56;
            }
            throw new InvalidOperationException("never reached");
        label56:
;
            this.Manager.EndTest();
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonExS86GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S87");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonExS86GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S87");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonExS86GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S87");
        }
        #endregion
        
        #region Test Starting in S88
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_PassThroughAuthenticate_NetrLogonSamLogonExS88() {
            this.Manager.BeginTest("Test_PassThroughAuthenticate_NetrLogonSamLogonExS88");
            this.Manager.Comment("reaching state \'S88\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp636;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp636);
            this.Manager.AddReturn(GetPlatformInfo, null, temp636);
            this.Manager.Comment("reaching state \'S89\'");
            int temp646 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonEx.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonExS88GetPlatformChecker)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonEx.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonExS88GetPlatformChecker1)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonEx.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonExS88GetPlatformChecker2)));
            if ((temp646 == 0)) {
                this.Manager.Comment("reaching state \'S258\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp637;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp637 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S447\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp637, "return of NetrServerReqChallenge, state S447");
                this.Manager.Comment("reaching state \'S636\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp638;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp638 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S825\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp638, "return of NetrServerAuthenticate3, state S825");
                this.Manager.Comment("reaching state \'S1014\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp639;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonEx(True,PrimaryDc,Client,NetlogonNetworkTra" +
                        "nsitiveInformation,Valid,NetlogonValidationSamInfo2,0)\'");
                temp639 = this.INrpcServerAdapterInstance.NetrLogonSamLogonEx(true, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonNetworkTransitiveInformation, ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo2, 0u);
                this.Manager.Checkpoint("MS-NRPC_R1160");
                this.Manager.Comment("reaching state \'S1203\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonEx/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp639, "return of NetrLogonSamLogonEx, state S1203");
                this.Manager.Comment("reaching state \'S1392\'");
                goto label57;
            }
            if ((temp646 == 1)) {
                this.Manager.Comment("reaching state \'S259\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp640;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp640 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S448\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp640, "return of NetrServerReqChallenge, state S448");
                this.Manager.Comment("reaching state \'S637\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp641;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp641 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S826\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp641, "return of NetrServerAuthenticate3, state S826");
                this.Manager.Comment("reaching state \'S1015\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp642;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonEx(True,PrimaryDc,Client,NetlogonInteractiv" +
                        "eInformation,Valid,NetlogonValidationSamInfo,0)\'");
                temp642 = this.INrpcServerAdapterInstance.NetrLogonSamLogonEx(true, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo, 0u);
                this.Manager.Checkpoint("MS-NRPC_R1174");
                this.Manager.Checkpoint("MS-NRPC_R1160");
                this.Manager.Comment("reaching state \'S1204\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonEx/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp642, "return of NetrLogonSamLogonEx, state S1204");
                this.Manager.Comment("reaching state \'S1393\'");
                goto label57;
            }
            if ((temp646 == 2)) {
                this.Manager.Comment("reaching state \'S260\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp643;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp643 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S449\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp643, "return of NetrServerReqChallenge, state S449");
                this.Manager.Comment("reaching state \'S638\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp644;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp644 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S827\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp644, "return of NetrServerAuthenticate3, state S827");
                this.Manager.Comment("reaching state \'S1016\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp645;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonEx(True,PrimaryDc,Client,NetlogonInteractiv" +
                        "eInformation,Valid,NetlogonValidationSamInfo,0)\'");
                temp645 = this.INrpcServerAdapterInstance.NetrLogonSamLogonEx(true, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo, 0u);
                this.Manager.Checkpoint("MS-NRPC_R1174");
                this.Manager.Checkpoint("MS-NRPC_R1160");
                this.Manager.Comment("reaching state \'S1205\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonEx/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp645, "return of NetrLogonSamLogonEx, state S1205");
                this.Manager.Comment("reaching state \'S1394\'");
                goto label57;
            }
            throw new InvalidOperationException("never reached");
        label57:
;
            this.Manager.EndTest();
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonExS88GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S89");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonExS88GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S89");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonExS88GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S89");
        }
        #endregion
        
        #region Test Starting in S90
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_PassThroughAuthenticate_NetrLogonSamLogonExS90() {
            this.Manager.BeginTest("Test_PassThroughAuthenticate_NetrLogonSamLogonExS90");
            this.Manager.Comment("reaching state \'S90\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp647;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp647);
            this.Manager.AddReturn(GetPlatformInfo, null, temp647);
            this.Manager.Comment("reaching state \'S91\'");
            int temp657 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonEx.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonExS90GetPlatformChecker)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonEx.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonExS90GetPlatformChecker1)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonEx.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonExS90GetPlatformChecker2)));
            if ((temp657 == 0)) {
                this.Manager.Comment("reaching state \'S261\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp648;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp648 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S450\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp648, "return of NetrServerReqChallenge, state S450");
                this.Manager.Comment("reaching state \'S639\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp649;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp649 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S828\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp649, "return of NetrServerAuthenticate3, state S828");
                this.Manager.Comment("reaching state \'S1017\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp650;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonEx(False,PrimaryDc,Client,NetlogonNetworkTr" +
                        "ansitiveInformation,Valid,NetlogonValidationSamInfo2,0)\'");
                temp650 = this.INrpcServerAdapterInstance.NetrLogonSamLogonEx(false, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonNetworkTransitiveInformation, ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo2, 0u);
                this.Manager.Checkpoint("MS-NRPC_R1167");
                this.Manager.Checkpoint("MS-NRPC_R1161");
                this.Manager.Comment("reaching state \'S1206\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonEx/STATUS_ACCESS_DENIED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp650, "return of NetrLogonSamLogonEx, state S1206");
                this.Manager.Comment("reaching state \'S1395\'");
                goto label58;
            }
            if ((temp657 == 1)) {
                this.Manager.Comment("reaching state \'S262\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp651;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp651 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S451\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp651, "return of NetrServerReqChallenge, state S451");
                this.Manager.Comment("reaching state \'S640\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp652;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp652 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S829\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp652, "return of NetrServerAuthenticate3, state S829");
                this.Manager.Comment("reaching state \'S1018\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp653;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonEx(True,PrimaryDc,Client,NetlogonInteractiv" +
                        "eInformation,Valid,NetlogonValidationSamInfo,0)\'");
                temp653 = this.INrpcServerAdapterInstance.NetrLogonSamLogonEx(true, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo, 0u);
                this.Manager.Checkpoint("MS-NRPC_R1174");
                this.Manager.Checkpoint("MS-NRPC_R1160");
                this.Manager.Comment("reaching state \'S1207\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonEx/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp653, "return of NetrLogonSamLogonEx, state S1207");
                this.Manager.Comment("reaching state \'S1396\'");
                goto label58;
            }
            if ((temp657 == 2)) {
                this.Manager.Comment("reaching state \'S263\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp654;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp654 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S452\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp654, "return of NetrServerReqChallenge, state S452");
                this.Manager.Comment("reaching state \'S641\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp655;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp655 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S830\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp655, "return of NetrServerAuthenticate3, state S830");
                this.Manager.Comment("reaching state \'S1019\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp656;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonEx(True,PrimaryDc,Client,NetlogonInteractiv" +
                        "eInformation,Valid,NetlogonValidationSamInfo,0)\'");
                temp656 = this.INrpcServerAdapterInstance.NetrLogonSamLogonEx(true, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo, 0u);
                this.Manager.Checkpoint("MS-NRPC_R1174");
                this.Manager.Checkpoint("MS-NRPC_R1160");
                this.Manager.Comment("reaching state \'S1208\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonEx/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp656, "return of NetrLogonSamLogonEx, state S1208");
                this.Manager.Comment("reaching state \'S1397\'");
                goto label58;
            }
            throw new InvalidOperationException("never reached");
        label58:
;
            this.Manager.EndTest();
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonExS90GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S91");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonExS90GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S91");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonExS90GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S91");
        }
        #endregion
        
        #region Test Starting in S92
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_PassThroughAuthenticate_NetrLogonSamLogonExS92() {
            this.Manager.BeginTest("Test_PassThroughAuthenticate_NetrLogonSamLogonExS92");
            this.Manager.Comment("reaching state \'S92\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp658;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp658);
            this.Manager.AddReturn(GetPlatformInfo, null, temp658);
            this.Manager.Comment("reaching state \'S93\'");
            int temp668 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonEx.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonExS92GetPlatformChecker)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonEx.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonExS92GetPlatformChecker1)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonEx.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonExS92GetPlatformChecker2)));
            if ((temp668 == 0)) {
                this.Manager.Comment("reaching state \'S264\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp659;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp659 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S453\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp659, "return of NetrServerReqChallenge, state S453");
                this.Manager.Comment("reaching state \'S642\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp660;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp660 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S831\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp660, "return of NetrServerAuthenticate3, state S831");
                this.Manager.Comment("reaching state \'S1020\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp661;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonEx(True,PrimaryDc,Client,NetlogonNetworkTra" +
                        "nsitiveInformation,Valid,NetlogonValidationSamInfo,0)\'");
                temp661 = this.INrpcServerAdapterInstance.NetrLogonSamLogonEx(true, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonNetworkTransitiveInformation, ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo, 0u);
                this.Manager.Checkpoint("MS-NRPC_R1160");
                this.Manager.Comment("reaching state \'S1209\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonEx/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp661, "return of NetrLogonSamLogonEx, state S1209");
                this.Manager.Comment("reaching state \'S1398\'");
                goto label59;
            }
            if ((temp668 == 1)) {
                this.Manager.Comment("reaching state \'S265\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp662;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp662 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S454\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp662, "return of NetrServerReqChallenge, state S454");
                this.Manager.Comment("reaching state \'S643\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp663;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp663 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S832\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp663, "return of NetrServerAuthenticate3, state S832");
                this.Manager.Comment("reaching state \'S1021\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp664;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonEx(True,PrimaryDc,Client,NetlogonInteractiv" +
                        "eInformation,Valid,NetlogonValidationSamInfo,0)\'");
                temp664 = this.INrpcServerAdapterInstance.NetrLogonSamLogonEx(true, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo, 0u);
                this.Manager.Checkpoint("MS-NRPC_R1174");
                this.Manager.Checkpoint("MS-NRPC_R1160");
                this.Manager.Comment("reaching state \'S1210\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonEx/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp664, "return of NetrLogonSamLogonEx, state S1210");
                this.Manager.Comment("reaching state \'S1399\'");
                goto label59;
            }
            if ((temp668 == 2)) {
                this.Manager.Comment("reaching state \'S266\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp665;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp665 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S455\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp665, "return of NetrServerReqChallenge, state S455");
                this.Manager.Comment("reaching state \'S644\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp666;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp666 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S833\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp666, "return of NetrServerAuthenticate3, state S833");
                this.Manager.Comment("reaching state \'S1022\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp667;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonEx(True,PrimaryDc,Client,NetlogonInteractiv" +
                        "eInformation,Valid,NetlogonValidationSamInfo,0)\'");
                temp667 = this.INrpcServerAdapterInstance.NetrLogonSamLogonEx(true, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo, 0u);
                this.Manager.Checkpoint("MS-NRPC_R1174");
                this.Manager.Checkpoint("MS-NRPC_R1160");
                this.Manager.Comment("reaching state \'S1211\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonEx/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp667, "return of NetrLogonSamLogonEx, state S1211");
                this.Manager.Comment("reaching state \'S1400\'");
                goto label59;
            }
            throw new InvalidOperationException("never reached");
        label59:
;
            this.Manager.EndTest();
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonExS92GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S93");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonExS92GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S93");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonExS92GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S93");
        }
        #endregion
        
        #region Test Starting in S94
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_PassThroughAuthenticate_NetrLogonSamLogonExS94() {
            this.Manager.BeginTest("Test_PassThroughAuthenticate_NetrLogonSamLogonExS94");
            this.Manager.Comment("reaching state \'S94\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp669;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp669);
            this.Manager.AddReturn(GetPlatformInfo, null, temp669);
            this.Manager.Comment("reaching state \'S95\'");
            int temp679 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonEx.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonExS94GetPlatformChecker)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonEx.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonExS94GetPlatformChecker1)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonEx.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonExS94GetPlatformChecker2)));
            if ((temp679 == 0)) {
                this.Manager.Comment("reaching state \'S267\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp670;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp670 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S456\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp670, "return of NetrServerReqChallenge, state S456");
                this.Manager.Comment("reaching state \'S645\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp671;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp671 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S834\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp671, "return of NetrServerAuthenticate3, state S834");
                this.Manager.Comment("reaching state \'S1023\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp672;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonEx(True,PrimaryDc,Client,NetlogonInteractiv" +
                        "eTransitiveInformation,Valid,NetlogonValidationSamInfo2,0)\'");
                temp672 = this.INrpcServerAdapterInstance.NetrLogonSamLogonEx(true, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonInteractiveTransitiveInformation, ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo2, 0u);
                this.Manager.Checkpoint("MS-NRPC_R1175");
                this.Manager.Checkpoint("MS-NRPC_R1160");
                this.Manager.Comment("reaching state \'S1212\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonEx/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp672, "return of NetrLogonSamLogonEx, state S1212");
                this.Manager.Comment("reaching state \'S1401\'");
                goto label60;
            }
            if ((temp679 == 1)) {
                this.Manager.Comment("reaching state \'S268\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp673;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp673 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S457\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp673, "return of NetrServerReqChallenge, state S457");
                this.Manager.Comment("reaching state \'S646\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp674;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp674 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S835\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp674, "return of NetrServerAuthenticate3, state S835");
                this.Manager.Comment("reaching state \'S1024\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp675;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonEx(True,PrimaryDc,Client,NetlogonInteractiv" +
                        "eInformation,Valid,NetlogonValidationSamInfo,0)\'");
                temp675 = this.INrpcServerAdapterInstance.NetrLogonSamLogonEx(true, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo, 0u);
                this.Manager.Checkpoint("MS-NRPC_R1174");
                this.Manager.Checkpoint("MS-NRPC_R1160");
                this.Manager.Comment("reaching state \'S1213\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonEx/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp675, "return of NetrLogonSamLogonEx, state S1213");
                this.Manager.Comment("reaching state \'S1402\'");
                goto label60;
            }
            if ((temp679 == 2)) {
                this.Manager.Comment("reaching state \'S269\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp676;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp676 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S458\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp676, "return of NetrServerReqChallenge, state S458");
                this.Manager.Comment("reaching state \'S647\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp677;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp677 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S836\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp677, "return of NetrServerAuthenticate3, state S836");
                this.Manager.Comment("reaching state \'S1025\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp678;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonEx(True,PrimaryDc,Client,NetlogonInteractiv" +
                        "eInformation,Valid,NetlogonValidationSamInfo,0)\'");
                temp678 = this.INrpcServerAdapterInstance.NetrLogonSamLogonEx(true, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo, 0u);
                this.Manager.Checkpoint("MS-NRPC_R1174");
                this.Manager.Checkpoint("MS-NRPC_R1160");
                this.Manager.Comment("reaching state \'S1214\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonEx/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp678, "return of NetrLogonSamLogonEx, state S1214");
                this.Manager.Comment("reaching state \'S1403\'");
                goto label60;
            }
            throw new InvalidOperationException("never reached");
        label60:
;
            this.Manager.EndTest();
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonExS94GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S95");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonExS94GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S95");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonExS94GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S95");
        }
        #endregion
        
        #region Test Starting in S96
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_PassThroughAuthenticate_NetrLogonSamLogonExS96() {
            this.Manager.BeginTest("Test_PassThroughAuthenticate_NetrLogonSamLogonExS96");
            this.Manager.Comment("reaching state \'S96\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp680;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp680);
            this.Manager.AddReturn(GetPlatformInfo, null, temp680);
            this.Manager.Comment("reaching state \'S97\'");
            int temp690 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonEx.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonExS96GetPlatformChecker)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonEx.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonExS96GetPlatformChecker1)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonEx.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonExS96GetPlatformChecker2)));
            if ((temp690 == 0)) {
                this.Manager.Comment("reaching state \'S270\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp681;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp681 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S459\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp681, "return of NetrServerReqChallenge, state S459");
                this.Manager.Comment("reaching state \'S648\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp682;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp682 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S837\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp682, "return of NetrServerAuthenticate3, state S837");
                this.Manager.Comment("reaching state \'S1026\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp683;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonEx(True,PrimaryDc,Client,NetlogonServiceInf" +
                        "ormation,Valid,NetlogonValidationGenericInfo2,0)\'");
                temp683 = this.INrpcServerAdapterInstance.NetrLogonSamLogonEx(true, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonServiceInformation, ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationGenericInfo2, 0u);
                this.Manager.Checkpoint("MS-NRPC_R1161");
                this.Manager.Comment("reaching state \'S1215\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonEx/STATUS_INVALID_INFO_CLASS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_INVALID_INFO_CLASS, temp683, "return of NetrLogonSamLogonEx, state S1215");
                this.Manager.Comment("reaching state \'S1404\'");
                goto label61;
            }
            if ((temp690 == 1)) {
                this.Manager.Comment("reaching state \'S271\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp684;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp684 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S460\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp684, "return of NetrServerReqChallenge, state S460");
                this.Manager.Comment("reaching state \'S649\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp685;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp685 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S838\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp685, "return of NetrServerAuthenticate3, state S838");
                this.Manager.Comment("reaching state \'S1027\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp686;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonEx(True,PrimaryDc,Client,NetlogonInteractiv" +
                        "eInformation,Valid,NetlogonValidationSamInfo,0)\'");
                temp686 = this.INrpcServerAdapterInstance.NetrLogonSamLogonEx(true, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo, 0u);
                this.Manager.Checkpoint("MS-NRPC_R1174");
                this.Manager.Checkpoint("MS-NRPC_R1160");
                this.Manager.Comment("reaching state \'S1216\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonEx/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp686, "return of NetrLogonSamLogonEx, state S1216");
                this.Manager.Comment("reaching state \'S1405\'");
                goto label61;
            }
            if ((temp690 == 2)) {
                this.Manager.Comment("reaching state \'S272\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp687;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp687 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S461\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp687, "return of NetrServerReqChallenge, state S461");
                this.Manager.Comment("reaching state \'S650\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp688;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp688 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S839\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp688, "return of NetrServerAuthenticate3, state S839");
                this.Manager.Comment("reaching state \'S1028\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp689;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonEx(True,PrimaryDc,Client,NetlogonInteractiv" +
                        "eInformation,Valid,NetlogonValidationSamInfo,0)\'");
                temp689 = this.INrpcServerAdapterInstance.NetrLogonSamLogonEx(true, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo, 0u);
                this.Manager.Checkpoint("MS-NRPC_R1174");
                this.Manager.Checkpoint("MS-NRPC_R1160");
                this.Manager.Comment("reaching state \'S1217\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonEx/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp689, "return of NetrLogonSamLogonEx, state S1217");
                this.Manager.Comment("reaching state \'S1406\'");
                goto label61;
            }
            throw new InvalidOperationException("never reached");
        label61:
;
            this.Manager.EndTest();
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonExS96GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S97");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonExS96GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S97");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonExS96GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S97");
        }
        #endregion
        
        #region Test Starting in S98
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_PassThroughAuthenticate_NetrLogonSamLogonExS98() {
            this.Manager.BeginTest("Test_PassThroughAuthenticate_NetrLogonSamLogonExS98");
            this.Manager.Comment("reaching state \'S98\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp691;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp691);
            this.Manager.AddReturn(GetPlatformInfo, null, temp691);
            this.Manager.Comment("reaching state \'S99\'");
            int temp701 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonEx.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonExS98GetPlatformChecker)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonEx.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonExS98GetPlatformChecker1)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogonEx.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonExS98GetPlatformChecker2)));
            if ((temp701 == 0)) {
                this.Manager.Comment("reaching state \'S273\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp692;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp692 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S462\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp692, "return of NetrServerReqChallenge, state S462");
                this.Manager.Comment("reaching state \'S651\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp693;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp693 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S840\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp693, "return of NetrServerAuthenticate3, state S840");
                this.Manager.Comment("reaching state \'S1029\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp694;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonEx(True,PrimaryDc,Client,NetlogonServiceInf" +
                        "ormation,Valid,NetlogonValidationSamInfo4,0)\'");
                temp694 = this.INrpcServerAdapterInstance.NetrLogonSamLogonEx(true, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonServiceInformation, ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo4, 0u);
                this.Manager.Checkpoint("MS-NRPC_R1177");
                this.Manager.Checkpoint("MS-NRPC_R1160");
                this.Manager.Comment("reaching state \'S1218\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonEx/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp694, "return of NetrLogonSamLogonEx, state S1218");
                this.Manager.Comment("reaching state \'S1407\'");
                goto label62;
            }
            if ((temp701 == 1)) {
                this.Manager.Comment("reaching state \'S274\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp695;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp695 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S463\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp695, "return of NetrServerReqChallenge, state S463");
                this.Manager.Comment("reaching state \'S652\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp696;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp696 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S841\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp696, "return of NetrServerAuthenticate3, state S841");
                this.Manager.Comment("reaching state \'S1030\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp697;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonEx(True,PrimaryDc,Client,NetlogonInteractiv" +
                        "eInformation,Valid,NetlogonValidationSamInfo,0)\'");
                temp697 = this.INrpcServerAdapterInstance.NetrLogonSamLogonEx(true, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo, 0u);
                this.Manager.Checkpoint("MS-NRPC_R1174");
                this.Manager.Checkpoint("MS-NRPC_R1160");
                this.Manager.Comment("reaching state \'S1219\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonEx/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp697, "return of NetrLogonSamLogonEx, state S1219");
                this.Manager.Comment("reaching state \'S1408\'");
                goto label62;
            }
            if ((temp701 == 2)) {
                this.Manager.Comment("reaching state \'S275\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp698;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp698 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S464\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp698, "return of NetrServerReqChallenge, state S464");
                this.Manager.Comment("reaching state \'S653\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp699;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp699 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S842\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp699, "return of NetrServerAuthenticate3, state S842");
                this.Manager.Comment("reaching state \'S1031\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp700;
                this.Manager.Comment("executing step \'call NetrLogonSamLogonEx(True,PrimaryDc,Client,NetlogonInteractiv" +
                        "eInformation,Valid,NetlogonValidationSamInfo,0)\'");
                temp700 = this.INrpcServerAdapterInstance.NetrLogonSamLogonEx(true, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo, 0u);
                this.Manager.Checkpoint("MS-NRPC_R1174");
                this.Manager.Checkpoint("MS-NRPC_R1160");
                this.Manager.Comment("reaching state \'S1220\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogonEx/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp700, "return of NetrLogonSamLogonEx, state S1220");
                this.Manager.Comment("reaching state \'S1409\'");
                goto label62;
            }
            throw new InvalidOperationException("never reached");
        label62:
;
            this.Manager.EndTest();
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonExS98GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S99");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonExS98GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S99");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonExS98GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S99");
        }
        #endregion
    }
}
