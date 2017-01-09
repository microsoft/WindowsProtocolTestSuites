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
    public partial class Test_PassThroughAuthenticate_NetrLogonSamLogon : PtfTestClassBase {
        
        public Test_PassThroughAuthenticate_NetrLogonSamLogon() {
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
        public void Test_PassThroughAuthenticate_NetrLogonSamLogonS0() {
            this.Manager.BeginTest("Test_PassThroughAuthenticate_NetrLogonSamLogonS0");
            this.Manager.Comment("reaching state \'S0\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp0;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp0);
            this.Manager.AddReturn(GetPlatformInfo, null, temp0);
            this.Manager.Comment("reaching state \'S1\'");
            int temp10 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogon.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonS0GetPlatformChecker)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogon.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonS0GetPlatformChecker1)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogon.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonS0GetPlatformChecker2)));
            if ((temp10 == 0)) {
                this.Manager.Comment("reaching state \'S22\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp1;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp1 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S55\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp1, "return of NetrServerReqChallenge, state S55");
                this.Manager.Comment("reaching state \'S88\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp2;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp2 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S121\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp2, "return of NetrServerAuthenticate3, state S121");
                this.Manager.Comment("reaching state \'S154\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp3;
                this.Manager.Comment("executing step \'call NetrLogonSamLogon(PrimaryDc,Client,True,NetlogonNetworkInfor" +
                        "mation,Valid,NetlogonValidationSamInfo)\'");
                temp3 = this.INrpcServerAdapterInstance.NetrLogonSamLogon(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonNetworkInformation, ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo);
                this.Manager.Comment("reaching state \'S187\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogon/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp3, "return of NetrLogonSamLogon, state S187");
                Test_PassThroughAuthenticate_NetrLogonSamLogonS220();
                goto label0;
            }
            if ((temp10 == 1)) {
                this.Manager.Comment("reaching state \'S23\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp4;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp4 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S56\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp4, "return of NetrServerReqChallenge, state S56");
                this.Manager.Comment("reaching state \'S89\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp5;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp5 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S122\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp5, "return of NetrServerAuthenticate3, state S122");
                this.Manager.Comment("reaching state \'S155\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp6;
                this.Manager.Comment("executing step \'call NetrLogonSamLogon(PrimaryDc,Client,True,NetlogonNetworkInfor" +
                        "mation,Valid,NetlogonValidationSamInfo)\'");
                temp6 = this.INrpcServerAdapterInstance.NetrLogonSamLogon(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonNetworkInformation, ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo);
                this.Manager.Comment("reaching state \'S188\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogon/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp6, "return of NetrLogonSamLogon, state S188");
                Test_PassThroughAuthenticate_NetrLogonSamLogonS221();
                goto label0;
            }
            if ((temp10 == 2)) {
                this.Manager.Comment("reaching state \'S24\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp7;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp7 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S57\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp7, "return of NetrServerReqChallenge, state S57");
                this.Manager.Comment("reaching state \'S90\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp8;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp8 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S123\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp8, "return of NetrServerAuthenticate3, state S123");
                this.Manager.Comment("reaching state \'S156\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp9;
                this.Manager.Comment("executing step \'call NetrLogonSamLogon(PrimaryDc,Client,True,NetlogonNetworkInfor" +
                        "mation,Valid,NetlogonValidationSamInfo)\'");
                temp9 = this.INrpcServerAdapterInstance.NetrLogonSamLogon(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonNetworkInformation, ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo);
                this.Manager.Comment("reaching state \'S189\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogon/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp9, "return of NetrLogonSamLogon, state S189");
                Test_PassThroughAuthenticate_NetrLogonSamLogonS222();
                goto label0;
            }
            throw new InvalidOperationException("never reached");
        label0:
;
            this.Manager.EndTest();
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonS0GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S1");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonS220() {
            this.Manager.Comment("reaching state \'S220\'");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonS0GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S1");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonS221() {
            this.Manager.Comment("reaching state \'S221\'");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonS0GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S1");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonS222() {
            this.Manager.Comment("reaching state \'S222\'");
        }
        #endregion
        
        #region Test Starting in S10
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_PassThroughAuthenticate_NetrLogonSamLogonS10() {
            this.Manager.BeginTest("Test_PassThroughAuthenticate_NetrLogonSamLogonS10");
            this.Manager.Comment("reaching state \'S10\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp11;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp11);
            this.Manager.AddReturn(GetPlatformInfo, null, temp11);
            this.Manager.Comment("reaching state \'S11\'");
            int temp21 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogon.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonS10GetPlatformChecker)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogon.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonS10GetPlatformChecker1)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogon.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonS10GetPlatformChecker2)));
            if ((temp21 == 0)) {
                this.Manager.Comment("reaching state \'S37\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp12;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp12 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S70\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp12, "return of NetrServerReqChallenge, state S70");
                this.Manager.Comment("reaching state \'S103\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp13;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp13 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S136\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp13, "return of NetrServerAuthenticate3, state S136");
                this.Manager.Comment("reaching state \'S169\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp14;
                this.Manager.Comment("executing step \'call NetrLogonSamLogon(PrimaryDc,Client,True,NetlogonInteractiveT" +
                        "ransitiveInformation,Valid,NetlogonValidationSamInfo)\'");
                temp14 = this.INrpcServerAdapterInstance.NetrLogonSamLogon(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonInteractiveTransitiveInformation, ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo);
                this.Manager.Comment("reaching state \'S202\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogon/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp14, "return of NetrLogonSamLogon, state S202");
                Test_PassThroughAuthenticate_NetrLogonSamLogonS222();
                goto label1;
            }
            if ((temp21 == 1)) {
                this.Manager.Comment("reaching state \'S38\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp15;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp15 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S71\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp15, "return of NetrServerReqChallenge, state S71");
                this.Manager.Comment("reaching state \'S104\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp16;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp16 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S137\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp16, "return of NetrServerAuthenticate3, state S137");
                this.Manager.Comment("reaching state \'S170\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp17;
                this.Manager.Comment("executing step \'call NetrLogonSamLogon(PrimaryDc,Client,True,NetlogonInteractiveT" +
                        "ransitiveInformation,Valid,NetlogonValidationSamInfo)\'");
                temp17 = this.INrpcServerAdapterInstance.NetrLogonSamLogon(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonInteractiveTransitiveInformation, ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo);
                this.Manager.Comment("reaching state \'S203\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogon/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp17, "return of NetrLogonSamLogon, state S203");
                Test_PassThroughAuthenticate_NetrLogonSamLogonS221();
                goto label1;
            }
            if ((temp21 == 2)) {
                this.Manager.Comment("reaching state \'S39\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp18;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp18 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S72\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp18, "return of NetrServerReqChallenge, state S72");
                this.Manager.Comment("reaching state \'S105\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp19;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp19 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S138\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp19, "return of NetrServerAuthenticate3, state S138");
                this.Manager.Comment("reaching state \'S171\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp20;
                this.Manager.Comment("executing step \'call NetrLogonSamLogon(PrimaryDc,Client,True,NetlogonNetworkTrans" +
                        "itiveInformation,Valid,NetlogonValidationSamInfo)\'");
                temp20 = this.INrpcServerAdapterInstance.NetrLogonSamLogon(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonNetworkTransitiveInformation, ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo);
                this.Manager.Comment("reaching state \'S204\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogon/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp20, "return of NetrLogonSamLogon, state S204");
                this.Manager.Comment("reaching state \'S227\'");
                goto label1;
            }
            throw new InvalidOperationException("never reached");
        label1:
;
            this.Manager.EndTest();
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonS10GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S11");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonS10GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S11");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonS10GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S11");
        }
        #endregion
        
        #region Test Starting in S12
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_PassThroughAuthenticate_NetrLogonSamLogonS12() {
            this.Manager.BeginTest("Test_PassThroughAuthenticate_NetrLogonSamLogonS12");
            this.Manager.Comment("reaching state \'S12\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp22;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp22);
            this.Manager.AddReturn(GetPlatformInfo, null, temp22);
            this.Manager.Comment("reaching state \'S13\'");
            int temp32 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogon.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonS12GetPlatformChecker)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogon.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonS12GetPlatformChecker1)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogon.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonS12GetPlatformChecker2)));
            if ((temp32 == 0)) {
                this.Manager.Comment("reaching state \'S40\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp23;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp23 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S73\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp23, "return of NetrServerReqChallenge, state S73");
                this.Manager.Comment("reaching state \'S106\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp24;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp24 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S139\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp24, "return of NetrServerAuthenticate3, state S139");
                this.Manager.Comment("reaching state \'S172\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp25;
                this.Manager.Comment("executing step \'call NetrLogonSamLogon(PrimaryDc,Client,True,NetlogonNetworkTrans" +
                        "itiveInformation,Valid,NetlogonValidationSamInfo)\'");
                temp25 = this.INrpcServerAdapterInstance.NetrLogonSamLogon(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonNetworkTransitiveInformation, ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo);
                this.Manager.Comment("reaching state \'S205\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogon/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp25, "return of NetrLogonSamLogon, state S205");
                Test_PassThroughAuthenticate_NetrLogonSamLogonS220();
                goto label2;
            }
            if ((temp32 == 1)) {
                this.Manager.Comment("reaching state \'S41\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp26;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp26 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S74\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp26, "return of NetrServerReqChallenge, state S74");
                this.Manager.Comment("reaching state \'S107\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp27;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp27 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S140\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp27, "return of NetrServerAuthenticate3, state S140");
                this.Manager.Comment("reaching state \'S173\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp28;
                this.Manager.Comment("executing step \'call NetrLogonSamLogon(PrimaryDc,Client,True,NetlogonNetworkTrans" +
                        "itiveInformation,Valid,NetlogonValidationSamInfo)\'");
                temp28 = this.INrpcServerAdapterInstance.NetrLogonSamLogon(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonNetworkTransitiveInformation, ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo);
                this.Manager.Comment("reaching state \'S206\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogon/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp28, "return of NetrLogonSamLogon, state S206");
                this.Manager.Comment("reaching state \'S228\'");
                goto label2;
            }
            if ((temp32 == 2)) {
                this.Manager.Comment("reaching state \'S42\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp29;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp29 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S75\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp29, "return of NetrServerReqChallenge, state S75");
                this.Manager.Comment("reaching state \'S108\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp30;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp30 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S141\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp30, "return of NetrServerAuthenticate3, state S141");
                this.Manager.Comment("reaching state \'S174\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp31;
                this.Manager.Comment("executing step \'call NetrLogonSamLogon(PrimaryDc,Client,True,NetlogonNetworkTrans" +
                        "itiveInformation,Valid,NetlogonValidationSamInfo)\'");
                temp31 = this.INrpcServerAdapterInstance.NetrLogonSamLogon(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonNetworkTransitiveInformation, ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo);
                this.Manager.Comment("reaching state \'S207\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogon/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp31, "return of NetrLogonSamLogon, state S207");
                this.Manager.Comment("reaching state \'S229\'");
                goto label2;
            }
            throw new InvalidOperationException("never reached");
        label2:
;
            this.Manager.EndTest();
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonS12GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S13");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonS12GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S13");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonS12GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S13");
        }
        #endregion
        
        #region Test Starting in S14
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_PassThroughAuthenticate_NetrLogonSamLogonS14() {
            this.Manager.BeginTest("Test_PassThroughAuthenticate_NetrLogonSamLogonS14");
            this.Manager.Comment("reaching state \'S14\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp33;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp33);
            this.Manager.AddReturn(GetPlatformInfo, null, temp33);
            this.Manager.Comment("reaching state \'S15\'");
            int temp43 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogon.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonS14GetPlatformChecker)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogon.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonS14GetPlatformChecker1)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogon.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonS14GetPlatformChecker2)));
            if ((temp43 == 0)) {
                this.Manager.Comment("reaching state \'S43\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp34;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp34 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S76\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp34, "return of NetrServerReqChallenge, state S76");
                this.Manager.Comment("reaching state \'S109\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp35;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp35 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S142\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp35, "return of NetrServerAuthenticate3, state S142");
                this.Manager.Comment("reaching state \'S175\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp36;
                this.Manager.Comment("executing step \'call NetrLogonSamLogon(PrimaryDc,Client,True,NetlogonInteractiveI" +
                        "nformation,Valid,NetlogonValidationSamInfo)\'");
                temp36 = this.INrpcServerAdapterInstance.NetrLogonSamLogon(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo);
                this.Manager.Comment("reaching state \'S208\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogon/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp36, "return of NetrLogonSamLogon, state S208");
                Test_PassThroughAuthenticate_NetrLogonSamLogonS220();
                goto label3;
            }
            if ((temp43 == 1)) {
                this.Manager.Comment("reaching state \'S44\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp37;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp37 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S77\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp37, "return of NetrServerReqChallenge, state S77");
                this.Manager.Comment("reaching state \'S110\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp38;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp38 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S143\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp38, "return of NetrServerAuthenticate3, state S143");
                this.Manager.Comment("reaching state \'S176\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp39;
                this.Manager.Comment("executing step \'call NetrLogonSamLogon(PrimaryDc,Client,True,NetlogonNetworkTrans" +
                        "itiveInformation,Valid,NetlogonValidationSamInfo)\'");
                temp39 = this.INrpcServerAdapterInstance.NetrLogonSamLogon(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonNetworkTransitiveInformation, ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo);
                this.Manager.Comment("reaching state \'S209\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogon/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp39, "return of NetrLogonSamLogon, state S209");
                this.Manager.Comment("reaching state \'S230\'");
                goto label3;
            }
            if ((temp43 == 2)) {
                this.Manager.Comment("reaching state \'S45\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp40;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp40 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S78\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp40, "return of NetrServerReqChallenge, state S78");
                this.Manager.Comment("reaching state \'S111\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp41;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp41 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S144\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp41, "return of NetrServerAuthenticate3, state S144");
                this.Manager.Comment("reaching state \'S177\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp42;
                this.Manager.Comment("executing step \'call NetrLogonSamLogon(PrimaryDc,Client,True,NetlogonNetworkTrans" +
                        "itiveInformation,Valid,NetlogonValidationSamInfo)\'");
                temp42 = this.INrpcServerAdapterInstance.NetrLogonSamLogon(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonNetworkTransitiveInformation, ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo);
                this.Manager.Comment("reaching state \'S210\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogon/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp42, "return of NetrLogonSamLogon, state S210");
                this.Manager.Comment("reaching state \'S231\'");
                goto label3;
            }
            throw new InvalidOperationException("never reached");
        label3:
;
            this.Manager.EndTest();
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonS14GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S15");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonS14GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S15");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonS14GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S15");
        }
        #endregion
        
        #region Test Starting in S16
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_PassThroughAuthenticate_NetrLogonSamLogonS16() {
            this.Manager.BeginTest("Test_PassThroughAuthenticate_NetrLogonSamLogonS16");
            this.Manager.Comment("reaching state \'S16\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp44;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp44);
            this.Manager.AddReturn(GetPlatformInfo, null, temp44);
            this.Manager.Comment("reaching state \'S17\'");
            int temp54 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogon.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonS16GetPlatformChecker)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogon.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonS16GetPlatformChecker1)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogon.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonS16GetPlatformChecker2)));
            if ((temp54 == 0)) {
                this.Manager.Comment("reaching state \'S46\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp45;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp45 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S79\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp45, "return of NetrServerReqChallenge, state S79");
                this.Manager.Comment("reaching state \'S112\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp46;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp46 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S145\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp46, "return of NetrServerAuthenticate3, state S145");
                this.Manager.Comment("reaching state \'S178\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp47;
                this.Manager.Comment("executing step \'call NetrLogonSamLogon(PrimaryDc,Client,True,NetlogonServiceInfor" +
                        "mation,Valid,NetlogonValidationSamInfo)\'");
                temp47 = this.INrpcServerAdapterInstance.NetrLogonSamLogon(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonServiceInformation, ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo);
                this.Manager.Comment("reaching state \'S211\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogon/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp47, "return of NetrLogonSamLogon, state S211");
                Test_PassThroughAuthenticate_NetrLogonSamLogonS220();
                goto label4;
            }
            if ((temp54 == 1)) {
                this.Manager.Comment("reaching state \'S47\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp48;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp48 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S80\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp48, "return of NetrServerReqChallenge, state S80");
                this.Manager.Comment("reaching state \'S113\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp49;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp49 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S146\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp49, "return of NetrServerAuthenticate3, state S146");
                this.Manager.Comment("reaching state \'S179\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp50;
                this.Manager.Comment("executing step \'call NetrLogonSamLogon(PrimaryDc,Client,True,NetlogonNetworkTrans" +
                        "itiveInformation,Valid,NetlogonValidationSamInfo)\'");
                temp50 = this.INrpcServerAdapterInstance.NetrLogonSamLogon(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonNetworkTransitiveInformation, ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo);
                this.Manager.Comment("reaching state \'S212\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogon/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp50, "return of NetrLogonSamLogon, state S212");
                this.Manager.Comment("reaching state \'S232\'");
                goto label4;
            }
            if ((temp54 == 2)) {
                this.Manager.Comment("reaching state \'S48\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp51;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp51 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S81\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp51, "return of NetrServerReqChallenge, state S81");
                this.Manager.Comment("reaching state \'S114\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp52;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp52 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S147\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp52, "return of NetrServerAuthenticate3, state S147");
                this.Manager.Comment("reaching state \'S180\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp53;
                this.Manager.Comment("executing step \'call NetrLogonSamLogon(PrimaryDc,Client,True,NetlogonNetworkTrans" +
                        "itiveInformation,Valid,NetlogonValidationSamInfo)\'");
                temp53 = this.INrpcServerAdapterInstance.NetrLogonSamLogon(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonNetworkTransitiveInformation, ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo);
                this.Manager.Comment("reaching state \'S213\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogon/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp53, "return of NetrLogonSamLogon, state S213");
                this.Manager.Comment("reaching state \'S233\'");
                goto label4;
            }
            throw new InvalidOperationException("never reached");
        label4:
;
            this.Manager.EndTest();
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonS16GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S17");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonS16GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S17");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonS16GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S17");
        }
        #endregion
        
        #region Test Starting in S18
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_PassThroughAuthenticate_NetrLogonSamLogonS18() {
            this.Manager.BeginTest("Test_PassThroughAuthenticate_NetrLogonSamLogonS18");
            this.Manager.Comment("reaching state \'S18\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp55;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp55);
            this.Manager.AddReturn(GetPlatformInfo, null, temp55);
            this.Manager.Comment("reaching state \'S19\'");
            int temp65 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogon.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonS18GetPlatformChecker)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogon.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonS18GetPlatformChecker1)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogon.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonS18GetPlatformChecker2)));
            if ((temp65 == 0)) {
                this.Manager.Comment("reaching state \'S49\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp56;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp56 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S82\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp56, "return of NetrServerReqChallenge, state S82");
                this.Manager.Comment("reaching state \'S115\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp57;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp57 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S148\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp57, "return of NetrServerAuthenticate3, state S148");
                this.Manager.Comment("reaching state \'S181\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp58;
                this.Manager.Comment("executing step \'call NetrLogonSamLogon(PrimaryDc,Client,True,NetlogonServiceTrans" +
                        "itiveInformation,Valid,NetlogonValidationSamInfo)\'");
                temp58 = this.INrpcServerAdapterInstance.NetrLogonSamLogon(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonServiceTransitiveInformation, ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo);
                this.Manager.Comment("reaching state \'S214\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogon/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp58, "return of NetrLogonSamLogon, state S214");
                Test_PassThroughAuthenticate_NetrLogonSamLogonS220();
                goto label5;
            }
            if ((temp65 == 1)) {
                this.Manager.Comment("reaching state \'S50\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp59;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp59 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S83\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp59, "return of NetrServerReqChallenge, state S83");
                this.Manager.Comment("reaching state \'S116\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp60;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp60 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S149\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp60, "return of NetrServerAuthenticate3, state S149");
                this.Manager.Comment("reaching state \'S182\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp61;
                this.Manager.Comment("executing step \'call NetrLogonSamLogon(PrimaryDc,Client,True,NetlogonNetworkTrans" +
                        "itiveInformation,Valid,NetlogonValidationSamInfo)\'");
                temp61 = this.INrpcServerAdapterInstance.NetrLogonSamLogon(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonNetworkTransitiveInformation, ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo);
                this.Manager.Comment("reaching state \'S215\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogon/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp61, "return of NetrLogonSamLogon, state S215");
                this.Manager.Comment("reaching state \'S234\'");
                goto label5;
            }
            if ((temp65 == 2)) {
                this.Manager.Comment("reaching state \'S51\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp62;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp62 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S84\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp62, "return of NetrServerReqChallenge, state S84");
                this.Manager.Comment("reaching state \'S117\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp63;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp63 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S150\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp63, "return of NetrServerAuthenticate3, state S150");
                this.Manager.Comment("reaching state \'S183\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp64;
                this.Manager.Comment("executing step \'call NetrLogonSamLogon(PrimaryDc,Client,True,NetlogonNetworkTrans" +
                        "itiveInformation,Valid,NetlogonValidationSamInfo)\'");
                temp64 = this.INrpcServerAdapterInstance.NetrLogonSamLogon(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonNetworkTransitiveInformation, ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo);
                this.Manager.Comment("reaching state \'S216\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogon/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp64, "return of NetrLogonSamLogon, state S216");
                this.Manager.Comment("reaching state \'S235\'");
                goto label5;
            }
            throw new InvalidOperationException("never reached");
        label5:
;
            this.Manager.EndTest();
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonS18GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S19");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonS18GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S19");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonS18GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S19");
        }
        #endregion
        
        #region Test Starting in S2
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_PassThroughAuthenticate_NetrLogonSamLogonS2() {
            this.Manager.BeginTest("Test_PassThroughAuthenticate_NetrLogonSamLogonS2");
            this.Manager.Comment("reaching state \'S2\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp66;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp66);
            this.Manager.AddReturn(GetPlatformInfo, null, temp66);
            this.Manager.Comment("reaching state \'S3\'");
            int temp76 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogon.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonS2GetPlatformChecker)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogon.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonS2GetPlatformChecker1)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogon.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonS2GetPlatformChecker2)));
            if ((temp76 == 0)) {
                this.Manager.Comment("reaching state \'S25\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp67;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp67 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S58\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp67, "return of NetrServerReqChallenge, state S58");
                this.Manager.Comment("reaching state \'S91\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp68;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp68 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S124\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp68, "return of NetrServerAuthenticate3, state S124");
                this.Manager.Comment("reaching state \'S157\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp69;
                this.Manager.Comment("executing step \'call NetrLogonSamLogon(PrimaryDc,Client,True,NetlogonNetworkTrans" +
                        "itiveInformation,Valid,NetlogonValidationSamInfo)\'");
                temp69 = this.INrpcServerAdapterInstance.NetrLogonSamLogon(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonNetworkTransitiveInformation, ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo);
                this.Manager.Comment("reaching state \'S190\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogon/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp69, "return of NetrLogonSamLogon, state S190");
                Test_PassThroughAuthenticate_NetrLogonSamLogonS222();
                goto label6;
            }
            if ((temp76 == 1)) {
                this.Manager.Comment("reaching state \'S26\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp70;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp70 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S59\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp70, "return of NetrServerReqChallenge, state S59");
                this.Manager.Comment("reaching state \'S92\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp71;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp71 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S125\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp71, "return of NetrServerAuthenticate3, state S125");
                this.Manager.Comment("reaching state \'S158\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp72;
                this.Manager.Comment("executing step \'call NetrLogonSamLogon(PrimaryDc,Client,True,NetlogonNetworkTrans" +
                        "itiveInformation,Valid,NetlogonValidationSamInfo)\'");
                temp72 = this.INrpcServerAdapterInstance.NetrLogonSamLogon(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonNetworkTransitiveInformation, ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo);
                this.Manager.Comment("reaching state \'S191\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogon/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp72, "return of NetrLogonSamLogon, state S191");
                Test_PassThroughAuthenticate_NetrLogonSamLogonS221();
                goto label6;
            }
            if ((temp76 == 2)) {
                this.Manager.Comment("reaching state \'S27\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp73;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp73 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S60\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp73, "return of NetrServerReqChallenge, state S60");
                this.Manager.Comment("reaching state \'S93\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp74;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp74 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S126\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp74, "return of NetrServerAuthenticate3, state S126");
                this.Manager.Comment("reaching state \'S159\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp75;
                this.Manager.Comment("executing step \'call NetrLogonSamLogon(PrimaryDc,Client,True,NetlogonNetworkTrans" +
                        "itiveInformation,Valid,NetlogonValidationSamInfo)\'");
                temp75 = this.INrpcServerAdapterInstance.NetrLogonSamLogon(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonNetworkTransitiveInformation, ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo);
                this.Manager.Comment("reaching state \'S192\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogon/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp75, "return of NetrLogonSamLogon, state S192");
                this.Manager.Comment("reaching state \'S223\'");
                goto label6;
            }
            throw new InvalidOperationException("never reached");
        label6:
;
            this.Manager.EndTest();
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonS2GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S3");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonS2GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S3");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonS2GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S3");
        }
        #endregion
        
        #region Test Starting in S20
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_PassThroughAuthenticate_NetrLogonSamLogonS20() {
            this.Manager.BeginTest("Test_PassThroughAuthenticate_NetrLogonSamLogonS20");
            this.Manager.Comment("reaching state \'S20\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp77;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp77);
            this.Manager.AddReturn(GetPlatformInfo, null, temp77);
            this.Manager.Comment("reaching state \'S21\'");
            int temp87 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogon.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonS20GetPlatformChecker)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogon.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonS20GetPlatformChecker1)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogon.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonS20GetPlatformChecker2)));
            if ((temp87 == 0)) {
                this.Manager.Comment("reaching state \'S52\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp78;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp78 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S85\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp78, "return of NetrServerReqChallenge, state S85");
                this.Manager.Comment("reaching state \'S118\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp79;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp79 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S151\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp79, "return of NetrServerAuthenticate3, state S151");
                this.Manager.Comment("reaching state \'S184\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp80;
                this.Manager.Comment("executing step \'call NetrLogonSamLogon(PrimaryDc,Client,True,NetlogonInteractiveT" +
                        "ransitiveInformation,Valid,NetlogonValidationSamInfo)\'");
                temp80 = this.INrpcServerAdapterInstance.NetrLogonSamLogon(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonInteractiveTransitiveInformation, ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo);
                this.Manager.Comment("reaching state \'S217\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogon/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp80, "return of NetrLogonSamLogon, state S217");
                Test_PassThroughAuthenticate_NetrLogonSamLogonS220();
                goto label7;
            }
            if ((temp87 == 1)) {
                this.Manager.Comment("reaching state \'S53\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp81;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp81 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S86\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp81, "return of NetrServerReqChallenge, state S86");
                this.Manager.Comment("reaching state \'S119\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp82;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp82 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S152\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp82, "return of NetrServerAuthenticate3, state S152");
                this.Manager.Comment("reaching state \'S185\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp83;
                this.Manager.Comment("executing step \'call NetrLogonSamLogon(PrimaryDc,Client,True,NetlogonNetworkTrans" +
                        "itiveInformation,Valid,NetlogonValidationSamInfo)\'");
                temp83 = this.INrpcServerAdapterInstance.NetrLogonSamLogon(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonNetworkTransitiveInformation, ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo);
                this.Manager.Comment("reaching state \'S218\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogon/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp83, "return of NetrLogonSamLogon, state S218");
                this.Manager.Comment("reaching state \'S236\'");
                goto label7;
            }
            if ((temp87 == 2)) {
                this.Manager.Comment("reaching state \'S54\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp84;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp84 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S87\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp84, "return of NetrServerReqChallenge, state S87");
                this.Manager.Comment("reaching state \'S120\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp85;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp85 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S153\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp85, "return of NetrServerAuthenticate3, state S153");
                this.Manager.Comment("reaching state \'S186\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp86;
                this.Manager.Comment("executing step \'call NetrLogonSamLogon(PrimaryDc,Client,True,NetlogonNetworkTrans" +
                        "itiveInformation,Valid,NetlogonValidationSamInfo)\'");
                temp86 = this.INrpcServerAdapterInstance.NetrLogonSamLogon(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonNetworkTransitiveInformation, ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo);
                this.Manager.Comment("reaching state \'S219\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogon/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp86, "return of NetrLogonSamLogon, state S219");
                this.Manager.Comment("reaching state \'S237\'");
                goto label7;
            }
            throw new InvalidOperationException("never reached");
        label7:
;
            this.Manager.EndTest();
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonS20GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S21");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonS20GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S21");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonS20GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S21");
        }
        #endregion
        
        #region Test Starting in S4
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_PassThroughAuthenticate_NetrLogonSamLogonS4() {
            this.Manager.BeginTest("Test_PassThroughAuthenticate_NetrLogonSamLogonS4");
            this.Manager.Comment("reaching state \'S4\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp88;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp88);
            this.Manager.AddReturn(GetPlatformInfo, null, temp88);
            this.Manager.Comment("reaching state \'S5\'");
            int temp98 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogon.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonS4GetPlatformChecker)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogon.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonS4GetPlatformChecker1)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogon.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonS4GetPlatformChecker2)));
            if ((temp98 == 0)) {
                this.Manager.Comment("reaching state \'S28\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp89;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp89 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S61\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp89, "return of NetrServerReqChallenge, state S61");
                this.Manager.Comment("reaching state \'S94\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp90;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp90 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S127\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp90, "return of NetrServerAuthenticate3, state S127");
                this.Manager.Comment("reaching state \'S160\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp91;
                this.Manager.Comment("executing step \'call NetrLogonSamLogon(PrimaryDc,Client,True,NetlogonInteractiveI" +
                        "nformation,Valid,NetlogonValidationSamInfo)\'");
                temp91 = this.INrpcServerAdapterInstance.NetrLogonSamLogon(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo);
                this.Manager.Comment("reaching state \'S193\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogon/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp91, "return of NetrLogonSamLogon, state S193");
                Test_PassThroughAuthenticate_NetrLogonSamLogonS222();
                goto label8;
            }
            if ((temp98 == 1)) {
                this.Manager.Comment("reaching state \'S29\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp92;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp92 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S62\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp92, "return of NetrServerReqChallenge, state S62");
                this.Manager.Comment("reaching state \'S95\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp93;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp93 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S128\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp93, "return of NetrServerAuthenticate3, state S128");
                this.Manager.Comment("reaching state \'S161\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp94;
                this.Manager.Comment("executing step \'call NetrLogonSamLogon(PrimaryDc,Client,True,NetlogonInteractiveI" +
                        "nformation,Valid,NetlogonValidationSamInfo)\'");
                temp94 = this.INrpcServerAdapterInstance.NetrLogonSamLogon(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo);
                this.Manager.Comment("reaching state \'S194\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogon/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp94, "return of NetrLogonSamLogon, state S194");
                Test_PassThroughAuthenticate_NetrLogonSamLogonS221();
                goto label8;
            }
            if ((temp98 == 2)) {
                this.Manager.Comment("reaching state \'S30\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp95;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp95 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S63\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp95, "return of NetrServerReqChallenge, state S63");
                this.Manager.Comment("reaching state \'S96\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp96;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp96 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S129\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp96, "return of NetrServerAuthenticate3, state S129");
                this.Manager.Comment("reaching state \'S162\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp97;
                this.Manager.Comment("executing step \'call NetrLogonSamLogon(PrimaryDc,Client,True,NetlogonNetworkTrans" +
                        "itiveInformation,Valid,NetlogonValidationSamInfo)\'");
                temp97 = this.INrpcServerAdapterInstance.NetrLogonSamLogon(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonNetworkTransitiveInformation, ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo);
                this.Manager.Comment("reaching state \'S195\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogon/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp97, "return of NetrLogonSamLogon, state S195");
                this.Manager.Comment("reaching state \'S224\'");
                goto label8;
            }
            throw new InvalidOperationException("never reached");
        label8:
;
            this.Manager.EndTest();
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonS4GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S5");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonS4GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S5");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonS4GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S5");
        }
        #endregion
        
        #region Test Starting in S6
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_PassThroughAuthenticate_NetrLogonSamLogonS6() {
            this.Manager.BeginTest("Test_PassThroughAuthenticate_NetrLogonSamLogonS6");
            this.Manager.Comment("reaching state \'S6\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp99;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp99);
            this.Manager.AddReturn(GetPlatformInfo, null, temp99);
            this.Manager.Comment("reaching state \'S7\'");
            int temp109 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogon.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonS6GetPlatformChecker)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogon.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonS6GetPlatformChecker1)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogon.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonS6GetPlatformChecker2)));
            if ((temp109 == 0)) {
                this.Manager.Comment("reaching state \'S31\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp100;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp100 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S64\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp100, "return of NetrServerReqChallenge, state S64");
                this.Manager.Comment("reaching state \'S97\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp101;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp101 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S130\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp101, "return of NetrServerAuthenticate3, state S130");
                this.Manager.Comment("reaching state \'S163\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp102;
                this.Manager.Comment("executing step \'call NetrLogonSamLogon(PrimaryDc,Client,True,NetlogonServiceInfor" +
                        "mation,Valid,NetlogonValidationSamInfo)\'");
                temp102 = this.INrpcServerAdapterInstance.NetrLogonSamLogon(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonServiceInformation, ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo);
                this.Manager.Comment("reaching state \'S196\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogon/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp102, "return of NetrLogonSamLogon, state S196");
                Test_PassThroughAuthenticate_NetrLogonSamLogonS222();
                goto label9;
            }
            if ((temp109 == 1)) {
                this.Manager.Comment("reaching state \'S32\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp103;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp103 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S65\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp103, "return of NetrServerReqChallenge, state S65");
                this.Manager.Comment("reaching state \'S98\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp104;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp104 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S131\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp104, "return of NetrServerAuthenticate3, state S131");
                this.Manager.Comment("reaching state \'S164\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp105;
                this.Manager.Comment("executing step \'call NetrLogonSamLogon(PrimaryDc,Client,True,NetlogonServiceInfor" +
                        "mation,Valid,NetlogonValidationSamInfo)\'");
                temp105 = this.INrpcServerAdapterInstance.NetrLogonSamLogon(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonServiceInformation, ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo);
                this.Manager.Comment("reaching state \'S197\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogon/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp105, "return of NetrLogonSamLogon, state S197");
                Test_PassThroughAuthenticate_NetrLogonSamLogonS221();
                goto label9;
            }
            if ((temp109 == 2)) {
                this.Manager.Comment("reaching state \'S33\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp106;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp106 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S66\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp106, "return of NetrServerReqChallenge, state S66");
                this.Manager.Comment("reaching state \'S99\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp107;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp107 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S132\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp107, "return of NetrServerAuthenticate3, state S132");
                this.Manager.Comment("reaching state \'S165\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp108;
                this.Manager.Comment("executing step \'call NetrLogonSamLogon(PrimaryDc,Client,True,NetlogonNetworkTrans" +
                        "itiveInformation,Valid,NetlogonValidationSamInfo)\'");
                temp108 = this.INrpcServerAdapterInstance.NetrLogonSamLogon(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonNetworkTransitiveInformation, ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo);
                this.Manager.Comment("reaching state \'S198\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogon/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp108, "return of NetrLogonSamLogon, state S198");
                this.Manager.Comment("reaching state \'S225\'");
                goto label9;
            }
            throw new InvalidOperationException("never reached");
        label9:
;
            this.Manager.EndTest();
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonS6GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S7");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonS6GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S7");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonS6GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S7");
        }
        #endregion
        
        #region Test Starting in S8
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_PassThroughAuthenticate_NetrLogonSamLogonS8() {
            this.Manager.BeginTest("Test_PassThroughAuthenticate_NetrLogonSamLogonS8");
            this.Manager.Comment("reaching state \'S8\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp110;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp110);
            this.Manager.AddReturn(GetPlatformInfo, null, temp110);
            this.Manager.Comment("reaching state \'S9\'");
            int temp120 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogon.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonS8GetPlatformChecker)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogon.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonS8GetPlatformChecker1)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogon.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogonS8GetPlatformChecker2)));
            if ((temp120 == 0)) {
                this.Manager.Comment("reaching state \'S34\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp111;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp111 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S67\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp111, "return of NetrServerReqChallenge, state S67");
                this.Manager.Comment("reaching state \'S100\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp112;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp112 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S133\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp112, "return of NetrServerAuthenticate3, state S133");
                this.Manager.Comment("reaching state \'S166\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp113;
                this.Manager.Comment("executing step \'call NetrLogonSamLogon(PrimaryDc,Client,True,NetlogonServiceTrans" +
                        "itiveInformation,Valid,NetlogonValidationSamInfo)\'");
                temp113 = this.INrpcServerAdapterInstance.NetrLogonSamLogon(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonServiceTransitiveInformation, ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo);
                this.Manager.Comment("reaching state \'S199\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogon/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp113, "return of NetrLogonSamLogon, state S199");
                Test_PassThroughAuthenticate_NetrLogonSamLogonS222();
                goto label10;
            }
            if ((temp120 == 1)) {
                this.Manager.Comment("reaching state \'S35\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp114;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp114 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S68\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp114, "return of NetrServerReqChallenge, state S68");
                this.Manager.Comment("reaching state \'S101\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp115;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp115 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S134\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp115, "return of NetrServerAuthenticate3, state S134");
                this.Manager.Comment("reaching state \'S167\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp116;
                this.Manager.Comment("executing step \'call NetrLogonSamLogon(PrimaryDc,Client,True,NetlogonServiceTrans" +
                        "itiveInformation,Valid,NetlogonValidationSamInfo)\'");
                temp116 = this.INrpcServerAdapterInstance.NetrLogonSamLogon(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonServiceTransitiveInformation, ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo);
                this.Manager.Comment("reaching state \'S200\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogon/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp116, "return of NetrLogonSamLogon, state S200");
                Test_PassThroughAuthenticate_NetrLogonSamLogonS221();
                goto label10;
            }
            if ((temp120 == 2)) {
                this.Manager.Comment("reaching state \'S36\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp117;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp117 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S69\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp117, "return of NetrServerReqChallenge, state S69");
                this.Manager.Comment("reaching state \'S102\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp118;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp118 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S135\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp118, "return of NetrServerAuthenticate3, state S135");
                this.Manager.Comment("reaching state \'S168\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp119;
                this.Manager.Comment("executing step \'call NetrLogonSamLogon(PrimaryDc,Client,True,NetlogonNetworkTrans" +
                        "itiveInformation,Valid,NetlogonValidationSamInfo)\'");
                temp119 = this.INrpcServerAdapterInstance.NetrLogonSamLogon(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonNetworkTransitiveInformation, ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_VALIDATION_INFO_CLASS.NetlogonValidationSamInfo);
                this.Manager.Comment("reaching state \'S201\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogon/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp119, "return of NetrLogonSamLogon, state S201");
                this.Manager.Comment("reaching state \'S226\'");
                goto label10;
            }
            throw new InvalidOperationException("never reached");
        label10:
;
            this.Manager.EndTest();
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonS8GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S9");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonS8GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S9");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogonS8GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S9");
        }
        #endregion
    }
}
