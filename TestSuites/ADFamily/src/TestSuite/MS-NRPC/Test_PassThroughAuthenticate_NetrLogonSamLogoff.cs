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
    public partial class Test_PassThroughAuthenticate_NetrLogonSamLogoff : PtfTestClassBase {
        
        public Test_PassThroughAuthenticate_NetrLogonSamLogoff() {
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
        public void Test_PassThroughAuthenticate_NetrLogonSamLogoffS0() {
            this.Manager.BeginTest("Test_PassThroughAuthenticate_NetrLogonSamLogoffS0");
            this.Manager.Comment("reaching state \'S0\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp0;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp0);
            this.Manager.AddReturn(GetPlatformInfo, null, temp0);
            this.Manager.Comment("reaching state \'S1\'");
            int temp10 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogoff.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogoffS0GetPlatformChecker)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogoff.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogoffS0GetPlatformChecker1)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogoff.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogoffS0GetPlatformChecker2)));
            if ((temp10 == 0)) {
                this.Manager.Comment("reaching state \'S46\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp1;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp1 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S115\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp1, "return of NetrServerReqChallenge, state S115");
                this.Manager.Comment("reaching state \'S184\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp2;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp2 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S253\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp2, "return of NetrServerAuthenticate3, state S253");
                this.Manager.Comment("reaching state \'S322\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp3;
                this.Manager.Comment("executing step \'call NetrLogonSamLogoff(PrimaryDc,Client,Null,False,NetlogonInter" +
                        "activeInformation,Valid)\'");
                temp3 = this.INrpcServerAdapterInstance.NetrLogonSamLogoff(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, ((Microsoft.Protocols.TestSuites.Nrpc.AuthenticatorType)(0)), false, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)));
                this.Manager.Checkpoint("MS-NRPC_R1269");
                this.Manager.Checkpoint("MS-NRPC_R1258");
                this.Manager.Comment("reaching state \'S391\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogoff/STATUS_INVALID_PARAMETER\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_INVALID_PARAMETER, temp3, "return of NetrLogonSamLogoff, state S391");
                Test_PassThroughAuthenticate_NetrLogonSamLogoffS460();
                goto label0;
            }
            if ((temp10 == 1)) {
                this.Manager.Comment("reaching state \'S47\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp4;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp4 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S116\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp4, "return of NetrServerReqChallenge, state S116");
                this.Manager.Comment("reaching state \'S185\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp5;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp5 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S254\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp5, "return of NetrServerAuthenticate3, state S254");
                this.Manager.Comment("reaching state \'S323\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp6;
                this.Manager.Comment("executing step \'call NetrLogonSamLogoff(PrimaryDc,Client,Null,False,NetlogonInter" +
                        "activeInformation,Valid)\'");
                temp6 = this.INrpcServerAdapterInstance.NetrLogonSamLogoff(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, ((Microsoft.Protocols.TestSuites.Nrpc.AuthenticatorType)(0)), false, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)));
                this.Manager.Checkpoint("MS-NRPC_R1269");
                this.Manager.Checkpoint("MS-NRPC_R1258");
                this.Manager.Comment("reaching state \'S392\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogoff/STATUS_INVALID_PARAMETER\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_INVALID_PARAMETER, temp6, "return of NetrLogonSamLogoff, state S392");
                Test_PassThroughAuthenticate_NetrLogonSamLogoffS461();
                goto label0;
            }
            if ((temp10 == 2)) {
                this.Manager.Comment("reaching state \'S48\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp7;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp7 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S117\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp7, "return of NetrServerReqChallenge, state S117");
                this.Manager.Comment("reaching state \'S186\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp8;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp8 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S255\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp8, "return of NetrServerAuthenticate3, state S255");
                this.Manager.Comment("reaching state \'S324\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp9;
                this.Manager.Comment("executing step \'call NetrLogonSamLogoff(PrimaryDc,Client,Null,False,NetlogonInter" +
                        "activeInformation,Valid)\'");
                temp9 = this.INrpcServerAdapterInstance.NetrLogonSamLogoff(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, ((Microsoft.Protocols.TestSuites.Nrpc.AuthenticatorType)(0)), false, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)));
                this.Manager.Checkpoint("MS-NRPC_R1269");
                this.Manager.Checkpoint("MS-NRPC_R1258");
                this.Manager.Comment("reaching state \'S393\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogoff/STATUS_INVALID_PARAMETER\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_INVALID_PARAMETER, temp9, "return of NetrLogonSamLogoff, state S393");
                Test_PassThroughAuthenticate_NetrLogonSamLogoffS462();
                goto label0;
            }
            throw new InvalidOperationException("never reached");
        label0:
;
            this.Manager.EndTest();
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogoffS0GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S1");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogoffS460() {
            this.Manager.Comment("reaching state \'S460\'");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogoffS0GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S1");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogoffS461() {
            this.Manager.Comment("reaching state \'S461\'");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogoffS0GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S1");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogoffS462() {
            this.Manager.Comment("reaching state \'S462\'");
        }
        #endregion
        
        #region Test Starting in S10
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_PassThroughAuthenticate_NetrLogonSamLogoffS10() {
            this.Manager.BeginTest("Test_PassThroughAuthenticate_NetrLogonSamLogoffS10");
            this.Manager.Comment("reaching state \'S10\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp11;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp11);
            this.Manager.AddReturn(GetPlatformInfo, null, temp11);
            this.Manager.Comment("reaching state \'S11\'");
            int temp21 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogoff.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogoffS10GetPlatformChecker)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogoff.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogoffS10GetPlatformChecker1)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogoff.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogoffS10GetPlatformChecker2)));
            if ((temp21 == 0)) {
                this.Manager.Comment("reaching state \'S61\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp12;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp12 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S130\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp12, "return of NetrServerReqChallenge, state S130");
                this.Manager.Comment("reaching state \'S199\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp13;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp13 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S268\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp13, "return of NetrServerAuthenticate3, state S268");
                this.Manager.Comment("reaching state \'S337\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp14;
                this.Manager.Comment("executing step \'call NetrLogonSamLogoff(PrimaryDc,Client,Valid,False,NetlogonServ" +
                        "iceTransitiveInformation,Valid)\'");
                temp14 = this.INrpcServerAdapterInstance.NetrLogonSamLogoff(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, ((Microsoft.Protocols.TestSuites.Nrpc.AuthenticatorType)(1)), false, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonServiceTransitiveInformation, ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)));
                this.Manager.Checkpoint("MS-NRPC_R1272");
                this.Manager.Checkpoint("MS-NRPC_R1258");
                this.Manager.Comment("reaching state \'S406\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogoff/STATUS_INVALID_INFO_CLASS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_INVALID_INFO_CLASS, temp14, "return of NetrLogonSamLogoff, state S406");
                Test_PassThroughAuthenticate_NetrLogonSamLogoffS462();
                goto label1;
            }
            if ((temp21 == 1)) {
                this.Manager.Comment("reaching state \'S62\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp15;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp15 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S131\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp15, "return of NetrServerReqChallenge, state S131");
                this.Manager.Comment("reaching state \'S200\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp16;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp16 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S269\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp16, "return of NetrServerAuthenticate3, state S269");
                this.Manager.Comment("reaching state \'S338\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp17;
                this.Manager.Comment("executing step \'call NetrLogonSamLogoff(PrimaryDc,Client,Valid,False,NetlogonServ" +
                        "iceTransitiveInformation,Valid)\'");
                temp17 = this.INrpcServerAdapterInstance.NetrLogonSamLogoff(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, ((Microsoft.Protocols.TestSuites.Nrpc.AuthenticatorType)(1)), false, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonServiceTransitiveInformation, ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)));
                this.Manager.Checkpoint("MS-NRPC_R1272");
                this.Manager.Checkpoint("MS-NRPC_R1258");
                this.Manager.Comment("reaching state \'S407\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogoff/STATUS_INVALID_INFO_CLASS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_INVALID_INFO_CLASS, temp17, "return of NetrLogonSamLogoff, state S407");
                Test_PassThroughAuthenticate_NetrLogonSamLogoffS461();
                goto label1;
            }
            if ((temp21 == 2)) {
                this.Manager.Comment("reaching state \'S63\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp18;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp18 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S132\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp18, "return of NetrServerReqChallenge, state S132");
                this.Manager.Comment("reaching state \'S201\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp19;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp19 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S270\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp19, "return of NetrServerAuthenticate3, state S270");
                this.Manager.Comment("reaching state \'S339\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp20;
                this.Manager.Comment("executing step \'call NetrLogonSamLogoff(NonDcServer,Client,Valid,False,NetlogonIn" +
                        "teractiveInformation,Valid)\'");
                temp20 = this.INrpcServerAdapterInstance.NetrLogonSamLogoff(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, ((Microsoft.Protocols.TestSuites.Nrpc.AuthenticatorType)(1)), false, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)));
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R1533");
                this.Manager.Checkpoint("MS-NRPC_R1258");
                this.Manager.Comment("reaching state \'S408\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogoff/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp20, "return of NetrLogonSamLogoff, state S408");
                this.Manager.Comment("reaching state \'S467\'");
                goto label1;
            }
            throw new InvalidOperationException("never reached");
        label1:
;
            this.Manager.EndTest();
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogoffS10GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S11");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogoffS10GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S11");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogoffS10GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S11");
        }
        #endregion
        
        #region Test Starting in S12
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_PassThroughAuthenticate_NetrLogonSamLogoffS12() {
            this.Manager.BeginTest("Test_PassThroughAuthenticate_NetrLogonSamLogoffS12");
            this.Manager.Comment("reaching state \'S12\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp22;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp22);
            this.Manager.AddReturn(GetPlatformInfo, null, temp22);
            this.Manager.Comment("reaching state \'S13\'");
            int temp32 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogoff.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogoffS12GetPlatformChecker)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogoff.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogoffS12GetPlatformChecker1)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogoff.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogoffS12GetPlatformChecker2)));
            if ((temp32 == 0)) {
                this.Manager.Comment("reaching state \'S64\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp23;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp23 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S133\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp23, "return of NetrServerReqChallenge, state S133");
                this.Manager.Comment("reaching state \'S202\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp24;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp24 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S271\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp24, "return of NetrServerAuthenticate3, state S271");
                this.Manager.Comment("reaching state \'S340\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp25;
                this.Manager.Comment("executing step \'call NetrLogonSamLogoff(PrimaryDc,Client,Valid,False,NetlogonNetw" +
                        "orkTransitiveInformation,Valid)\'");
                temp25 = this.INrpcServerAdapterInstance.NetrLogonSamLogoff(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, ((Microsoft.Protocols.TestSuites.Nrpc.AuthenticatorType)(1)), false, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonNetworkTransitiveInformation, ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)));
                this.Manager.Checkpoint("MS-NRPC_R1272");
                this.Manager.Checkpoint("MS-NRPC_R1258");
                this.Manager.Comment("reaching state \'S409\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogoff/STATUS_INVALID_INFO_CLASS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_INVALID_INFO_CLASS, temp25, "return of NetrLogonSamLogoff, state S409");
                Test_PassThroughAuthenticate_NetrLogonSamLogoffS462();
                goto label2;
            }
            if ((temp32 == 1)) {
                this.Manager.Comment("reaching state \'S65\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp26;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp26 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S134\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp26, "return of NetrServerReqChallenge, state S134");
                this.Manager.Comment("reaching state \'S203\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp27;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp27 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S272\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp27, "return of NetrServerAuthenticate3, state S272");
                this.Manager.Comment("reaching state \'S341\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp28;
                this.Manager.Comment("executing step \'call NetrLogonSamLogoff(PrimaryDc,Client,Valid,False,NetlogonNetw" +
                        "orkTransitiveInformation,Valid)\'");
                temp28 = this.INrpcServerAdapterInstance.NetrLogonSamLogoff(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, ((Microsoft.Protocols.TestSuites.Nrpc.AuthenticatorType)(1)), false, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonNetworkTransitiveInformation, ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)));
                this.Manager.Checkpoint("MS-NRPC_R1272");
                this.Manager.Checkpoint("MS-NRPC_R1258");
                this.Manager.Comment("reaching state \'S410\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogoff/STATUS_INVALID_INFO_CLASS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_INVALID_INFO_CLASS, temp28, "return of NetrLogonSamLogoff, state S410");
                Test_PassThroughAuthenticate_NetrLogonSamLogoffS461();
                goto label2;
            }
            if ((temp32 == 2)) {
                this.Manager.Comment("reaching state \'S66\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp29;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp29 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S135\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp29, "return of NetrServerReqChallenge, state S135");
                this.Manager.Comment("reaching state \'S204\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp30;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp30 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S273\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp30, "return of NetrServerAuthenticate3, state S273");
                this.Manager.Comment("reaching state \'S342\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp31;
                this.Manager.Comment("executing step \'call NetrLogonSamLogoff(NonDcServer,Client,Valid,False,NetlogonIn" +
                        "teractiveInformation,Valid)\'");
                temp31 = this.INrpcServerAdapterInstance.NetrLogonSamLogoff(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, ((Microsoft.Protocols.TestSuites.Nrpc.AuthenticatorType)(1)), false, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)));
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R1533");
                this.Manager.Checkpoint("MS-NRPC_R1258");
                this.Manager.Comment("reaching state \'S411\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogoff/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp31, "return of NetrLogonSamLogoff, state S411");
                this.Manager.Comment("reaching state \'S468\'");
                goto label2;
            }
            throw new InvalidOperationException("never reached");
        label2:
;
            this.Manager.EndTest();
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogoffS12GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S13");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogoffS12GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S13");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogoffS12GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S13");
        }
        #endregion
        
        #region Test Starting in S14
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_PassThroughAuthenticate_NetrLogonSamLogoffS14() {
            this.Manager.BeginTest("Test_PassThroughAuthenticate_NetrLogonSamLogoffS14");
            this.Manager.Comment("reaching state \'S14\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp33;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp33);
            this.Manager.AddReturn(GetPlatformInfo, null, temp33);
            this.Manager.Comment("reaching state \'S15\'");
            int temp43 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogoff.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogoffS14GetPlatformChecker)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogoff.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogoffS14GetPlatformChecker1)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogoff.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogoffS14GetPlatformChecker2)));
            if ((temp43 == 0)) {
                this.Manager.Comment("reaching state \'S67\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp34;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp34 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S136\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp34, "return of NetrServerReqChallenge, state S136");
                this.Manager.Comment("reaching state \'S205\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp35;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp35 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S274\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp35, "return of NetrServerAuthenticate3, state S274");
                this.Manager.Comment("reaching state \'S343\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp36;
                this.Manager.Comment("executing step \'call NetrLogonSamLogoff(PrimaryDc,Client,Valid,False,NetlogonNetw" +
                        "orkInformation,Valid)\'");
                temp36 = this.INrpcServerAdapterInstance.NetrLogonSamLogoff(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, ((Microsoft.Protocols.TestSuites.Nrpc.AuthenticatorType)(1)), false, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonNetworkInformation, ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)));
                this.Manager.Checkpoint("MS-NRPC_R1272");
                this.Manager.Checkpoint("MS-NRPC_R1258");
                this.Manager.Comment("reaching state \'S412\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogoff/STATUS_INVALID_INFO_CLASS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_INVALID_INFO_CLASS, temp36, "return of NetrLogonSamLogoff, state S412");
                Test_PassThroughAuthenticate_NetrLogonSamLogoffS462();
                goto label3;
            }
            if ((temp43 == 1)) {
                this.Manager.Comment("reaching state \'S68\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp37;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp37 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S137\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp37, "return of NetrServerReqChallenge, state S137");
                this.Manager.Comment("reaching state \'S206\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp38;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp38 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S275\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp38, "return of NetrServerAuthenticate3, state S275");
                this.Manager.Comment("reaching state \'S344\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp39;
                this.Manager.Comment("executing step \'call NetrLogonSamLogoff(PrimaryDc,Client,Valid,False,NetlogonNetw" +
                        "orkInformation,Valid)\'");
                temp39 = this.INrpcServerAdapterInstance.NetrLogonSamLogoff(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, ((Microsoft.Protocols.TestSuites.Nrpc.AuthenticatorType)(1)), false, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonNetworkInformation, ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)));
                this.Manager.Checkpoint("MS-NRPC_R1272");
                this.Manager.Checkpoint("MS-NRPC_R1258");
                this.Manager.Comment("reaching state \'S413\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogoff/STATUS_INVALID_INFO_CLASS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_INVALID_INFO_CLASS, temp39, "return of NetrLogonSamLogoff, state S413");
                Test_PassThroughAuthenticate_NetrLogonSamLogoffS461();
                goto label3;
            }
            if ((temp43 == 2)) {
                this.Manager.Comment("reaching state \'S69\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp40;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp40 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S138\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp40, "return of NetrServerReqChallenge, state S138");
                this.Manager.Comment("reaching state \'S207\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp41;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp41 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S276\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp41, "return of NetrServerAuthenticate3, state S276");
                this.Manager.Comment("reaching state \'S345\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp42;
                this.Manager.Comment("executing step \'call NetrLogonSamLogoff(NonDcServer,Client,Valid,False,NetlogonIn" +
                        "teractiveInformation,Valid)\'");
                temp42 = this.INrpcServerAdapterInstance.NetrLogonSamLogoff(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, ((Microsoft.Protocols.TestSuites.Nrpc.AuthenticatorType)(1)), false, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)));
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R1533");
                this.Manager.Checkpoint("MS-NRPC_R1258");
                this.Manager.Comment("reaching state \'S414\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogoff/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp42, "return of NetrLogonSamLogoff, state S414");
                this.Manager.Comment("reaching state \'S469\'");
                goto label3;
            }
            throw new InvalidOperationException("never reached");
        label3:
;
            this.Manager.EndTest();
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogoffS14GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S15");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogoffS14GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S15");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogoffS14GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S15");
        }
        #endregion
        
        #region Test Starting in S16
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_PassThroughAuthenticate_NetrLogonSamLogoffS16() {
            this.Manager.BeginTest("Test_PassThroughAuthenticate_NetrLogonSamLogoffS16");
            this.Manager.Comment("reaching state \'S16\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp44;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp44);
            this.Manager.AddReturn(GetPlatformInfo, null, temp44);
            this.Manager.Comment("reaching state \'S17\'");
            int temp54 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogoff.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogoffS16GetPlatformChecker)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogoff.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogoffS16GetPlatformChecker1)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogoff.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogoffS16GetPlatformChecker2)));
            if ((temp54 == 0)) {
                this.Manager.Comment("reaching state \'S70\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp45;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp45 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S139\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp45, "return of NetrServerReqChallenge, state S139");
                this.Manager.Comment("reaching state \'S208\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp46;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp46 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S277\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp46, "return of NetrServerAuthenticate3, state S277");
                this.Manager.Comment("reaching state \'S346\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp47;
                this.Manager.Comment("executing step \'call NetrLogonSamLogoff(PrimaryDc,Client,Valid,False,NetlogonServ" +
                        "iceInformation,Valid)\'");
                temp47 = this.INrpcServerAdapterInstance.NetrLogonSamLogoff(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, ((Microsoft.Protocols.TestSuites.Nrpc.AuthenticatorType)(1)), false, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonServiceInformation, ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)));
                this.Manager.Checkpoint("MS-NRPC_R1272");
                this.Manager.Checkpoint("MS-NRPC_R1258");
                this.Manager.Comment("reaching state \'S415\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogoff/STATUS_INVALID_INFO_CLASS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_INVALID_INFO_CLASS, temp47, "return of NetrLogonSamLogoff, state S415");
                Test_PassThroughAuthenticate_NetrLogonSamLogoffS462();
                goto label4;
            }
            if ((temp54 == 1)) {
                this.Manager.Comment("reaching state \'S71\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp48;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp48 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S140\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp48, "return of NetrServerReqChallenge, state S140");
                this.Manager.Comment("reaching state \'S209\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp49;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp49 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S278\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp49, "return of NetrServerAuthenticate3, state S278");
                this.Manager.Comment("reaching state \'S347\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp50;
                this.Manager.Comment("executing step \'call NetrLogonSamLogoff(PrimaryDc,Client,Valid,False,NetlogonServ" +
                        "iceInformation,Valid)\'");
                temp50 = this.INrpcServerAdapterInstance.NetrLogonSamLogoff(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, ((Microsoft.Protocols.TestSuites.Nrpc.AuthenticatorType)(1)), false, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonServiceInformation, ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)));
                this.Manager.Checkpoint("MS-NRPC_R1272");
                this.Manager.Checkpoint("MS-NRPC_R1258");
                this.Manager.Comment("reaching state \'S416\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogoff/STATUS_INVALID_INFO_CLASS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_INVALID_INFO_CLASS, temp50, "return of NetrLogonSamLogoff, state S416");
                Test_PassThroughAuthenticate_NetrLogonSamLogoffS461();
                goto label4;
            }
            if ((temp54 == 2)) {
                this.Manager.Comment("reaching state \'S72\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp51;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp51 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S141\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp51, "return of NetrServerReqChallenge, state S141");
                this.Manager.Comment("reaching state \'S210\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp52;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp52 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S279\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp52, "return of NetrServerAuthenticate3, state S279");
                this.Manager.Comment("reaching state \'S348\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp53;
                this.Manager.Comment("executing step \'call NetrLogonSamLogoff(NonDcServer,Client,Valid,False,NetlogonIn" +
                        "teractiveInformation,Valid)\'");
                temp53 = this.INrpcServerAdapterInstance.NetrLogonSamLogoff(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, ((Microsoft.Protocols.TestSuites.Nrpc.AuthenticatorType)(1)), false, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)));
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R1533");
                this.Manager.Checkpoint("MS-NRPC_R1258");
                this.Manager.Comment("reaching state \'S417\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogoff/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp53, "return of NetrLogonSamLogoff, state S417");
                this.Manager.Comment("reaching state \'S470\'");
                goto label4;
            }
            throw new InvalidOperationException("never reached");
        label4:
;
            this.Manager.EndTest();
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogoffS16GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S17");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogoffS16GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S17");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogoffS16GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S17");
        }
        #endregion
        
        #region Test Starting in S18
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_PassThroughAuthenticate_NetrLogonSamLogoffS18() {
            this.Manager.BeginTest("Test_PassThroughAuthenticate_NetrLogonSamLogoffS18");
            this.Manager.Comment("reaching state \'S18\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp55;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp55);
            this.Manager.AddReturn(GetPlatformInfo, null, temp55);
            this.Manager.Comment("reaching state \'S19\'");
            int temp65 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogoff.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogoffS18GetPlatformChecker)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogoff.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogoffS18GetPlatformChecker1)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogoff.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogoffS18GetPlatformChecker2)));
            if ((temp65 == 0)) {
                this.Manager.Comment("reaching state \'S73\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp56;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp56 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S142\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp56, "return of NetrServerReqChallenge, state S142");
                this.Manager.Comment("reaching state \'S211\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp57;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp57 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S280\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp57, "return of NetrServerAuthenticate3, state S280");
                this.Manager.Comment("reaching state \'S349\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp58;
                this.Manager.Comment("executing step \'call NetrLogonSamLogoff(PrimaryDc,Client,Valid,False,NetlogonInte" +
                        "ractiveTransitiveInformation,Valid)\'");
                temp58 = this.INrpcServerAdapterInstance.NetrLogonSamLogoff(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, ((Microsoft.Protocols.TestSuites.Nrpc.AuthenticatorType)(1)), false, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonInteractiveTransitiveInformation, ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)));
                this.Manager.Checkpoint("MS-NRPC_R1272");
                this.Manager.Checkpoint("MS-NRPC_R1258");
                this.Manager.Comment("reaching state \'S418\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogoff/STATUS_INVALID_INFO_CLASS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_INVALID_INFO_CLASS, temp58, "return of NetrLogonSamLogoff, state S418");
                Test_PassThroughAuthenticate_NetrLogonSamLogoffS462();
                goto label5;
            }
            if ((temp65 == 1)) {
                this.Manager.Comment("reaching state \'S74\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp59;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp59 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S143\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp59, "return of NetrServerReqChallenge, state S143");
                this.Manager.Comment("reaching state \'S212\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp60;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp60 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S281\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp60, "return of NetrServerAuthenticate3, state S281");
                this.Manager.Comment("reaching state \'S350\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp61;
                this.Manager.Comment("executing step \'call NetrLogonSamLogoff(PrimaryDc,Client,Valid,False,NetlogonInte" +
                        "ractiveTransitiveInformation,Valid)\'");
                temp61 = this.INrpcServerAdapterInstance.NetrLogonSamLogoff(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, ((Microsoft.Protocols.TestSuites.Nrpc.AuthenticatorType)(1)), false, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonInteractiveTransitiveInformation, ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)));
                this.Manager.Checkpoint("MS-NRPC_R1272");
                this.Manager.Checkpoint("MS-NRPC_R1258");
                this.Manager.Comment("reaching state \'S419\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogoff/STATUS_INVALID_INFO_CLASS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_INVALID_INFO_CLASS, temp61, "return of NetrLogonSamLogoff, state S419");
                Test_PassThroughAuthenticate_NetrLogonSamLogoffS461();
                goto label5;
            }
            if ((temp65 == 2)) {
                this.Manager.Comment("reaching state \'S75\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp62;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp62 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S144\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp62, "return of NetrServerReqChallenge, state S144");
                this.Manager.Comment("reaching state \'S213\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp63;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp63 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S282\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp63, "return of NetrServerAuthenticate3, state S282");
                this.Manager.Comment("reaching state \'S351\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp64;
                this.Manager.Comment("executing step \'call NetrLogonSamLogoff(NonDcServer,Client,Valid,False,NetlogonIn" +
                        "teractiveInformation,Valid)\'");
                temp64 = this.INrpcServerAdapterInstance.NetrLogonSamLogoff(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, ((Microsoft.Protocols.TestSuites.Nrpc.AuthenticatorType)(1)), false, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)));
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R1533");
                this.Manager.Checkpoint("MS-NRPC_R1258");
                this.Manager.Comment("reaching state \'S420\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogoff/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp64, "return of NetrLogonSamLogoff, state S420");
                this.Manager.Comment("reaching state \'S471\'");
                goto label5;
            }
            throw new InvalidOperationException("never reached");
        label5:
;
            this.Manager.EndTest();
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogoffS18GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S19");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogoffS18GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S19");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogoffS18GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S19");
        }
        #endregion
        
        #region Test Starting in S2
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_PassThroughAuthenticate_NetrLogonSamLogoffS2() {
            this.Manager.BeginTest("Test_PassThroughAuthenticate_NetrLogonSamLogoffS2");
            this.Manager.Comment("reaching state \'S2\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp66;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp66);
            this.Manager.AddReturn(GetPlatformInfo, null, temp66);
            this.Manager.Comment("reaching state \'S3\'");
            int temp76 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogoff.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogoffS2GetPlatformChecker)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogoff.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogoffS2GetPlatformChecker1)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogoff.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogoffS2GetPlatformChecker2)));
            if ((temp76 == 0)) {
                this.Manager.Comment("reaching state \'S49\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp67;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp67 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S118\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp67, "return of NetrServerReqChallenge, state S118");
                this.Manager.Comment("reaching state \'S187\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp68;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp68 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S256\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp68, "return of NetrServerAuthenticate3, state S256");
                this.Manager.Comment("reaching state \'S325\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp69;
                this.Manager.Comment("executing step \'call NetrLogonSamLogoff(NonDcServer,Client,Valid,False,NetlogonIn" +
                        "teractiveInformation,Valid)\'");
                temp69 = this.INrpcServerAdapterInstance.NetrLogonSamLogoff(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, ((Microsoft.Protocols.TestSuites.Nrpc.AuthenticatorType)(1)), false, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)));
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R1533");
                this.Manager.Checkpoint("MS-NRPC_R1258");
                this.Manager.Comment("reaching state \'S394\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogoff/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp69, "return of NetrLogonSamLogoff, state S394");
                Test_PassThroughAuthenticate_NetrLogonSamLogoffS462();
                goto label6;
            }
            if ((temp76 == 1)) {
                this.Manager.Comment("reaching state \'S50\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp70;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp70 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S119\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp70, "return of NetrServerReqChallenge, state S119");
                this.Manager.Comment("reaching state \'S188\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp71;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp71 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S257\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp71, "return of NetrServerAuthenticate3, state S257");
                this.Manager.Comment("reaching state \'S326\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp72;
                this.Manager.Comment("executing step \'call NetrLogonSamLogoff(NonDcServer,Client,Valid,False,NetlogonIn" +
                        "teractiveInformation,Valid)\'");
                temp72 = this.INrpcServerAdapterInstance.NetrLogonSamLogoff(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, ((Microsoft.Protocols.TestSuites.Nrpc.AuthenticatorType)(1)), false, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)));
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R1533");
                this.Manager.Checkpoint("MS-NRPC_R1258");
                this.Manager.Comment("reaching state \'S395\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogoff/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp72, "return of NetrLogonSamLogoff, state S395");
                Test_PassThroughAuthenticate_NetrLogonSamLogoffS461();
                goto label6;
            }
            if ((temp76 == 2)) {
                this.Manager.Comment("reaching state \'S51\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp73;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp73 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S120\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp73, "return of NetrServerReqChallenge, state S120");
                this.Manager.Comment("reaching state \'S189\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp74;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp74 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S258\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp74, "return of NetrServerAuthenticate3, state S258");
                this.Manager.Comment("reaching state \'S327\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp75;
                this.Manager.Comment("executing step \'call NetrLogonSamLogoff(NonDcServer,Client,Valid,False,NetlogonIn" +
                        "teractiveInformation,Valid)\'");
                temp75 = this.INrpcServerAdapterInstance.NetrLogonSamLogoff(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, ((Microsoft.Protocols.TestSuites.Nrpc.AuthenticatorType)(1)), false, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)));
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R1533");
                this.Manager.Checkpoint("MS-NRPC_R1258");
                this.Manager.Comment("reaching state \'S396\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogoff/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp75, "return of NetrLogonSamLogoff, state S396");
                this.Manager.Comment("reaching state \'S463\'");
                goto label6;
            }
            throw new InvalidOperationException("never reached");
        label6:
;
            this.Manager.EndTest();
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogoffS2GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S3");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogoffS2GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S3");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogoffS2GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S3");
        }
        #endregion
        
        #region Test Starting in S20
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_PassThroughAuthenticate_NetrLogonSamLogoffS20() {
            this.Manager.BeginTest("Test_PassThroughAuthenticate_NetrLogonSamLogoffS20");
            this.Manager.Comment("reaching state \'S20\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp77;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp77);
            this.Manager.AddReturn(GetPlatformInfo, null, temp77);
            this.Manager.Comment("reaching state \'S21\'");
            int temp87 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogoff.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogoffS20GetPlatformChecker)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogoff.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogoffS20GetPlatformChecker1)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogoff.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogoffS20GetPlatformChecker2)));
            if ((temp87 == 0)) {
                this.Manager.Comment("reaching state \'S76\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp78;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp78 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S145\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp78, "return of NetrServerReqChallenge, state S145");
                this.Manager.Comment("reaching state \'S214\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp79;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp79 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S283\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp79, "return of NetrServerAuthenticate3, state S283");
                this.Manager.Comment("reaching state \'S352\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp80;
                this.Manager.Comment("executing step \'call NetrLogonSamLogoff(PrimaryDc,Client,Valid,True,NetlogonInter" +
                        "activeInformation,Valid)\'");
                temp80 = this.INrpcServerAdapterInstance.NetrLogonSamLogoff(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, ((Microsoft.Protocols.TestSuites.Nrpc.AuthenticatorType)(1)), true, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)));
                this.Manager.Checkpoint("MS-NRPC_R1270");
                this.Manager.Checkpoint("MS-NRPC_R1258");
                this.Manager.Comment("reaching state \'S421\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogoff/STATUS_INVALID_PARAMETER\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_INVALID_PARAMETER, temp80, "return of NetrLogonSamLogoff, state S421");
                Test_PassThroughAuthenticate_NetrLogonSamLogoffS462();
                goto label7;
            }
            if ((temp87 == 1)) {
                this.Manager.Comment("reaching state \'S77\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp81;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp81 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S146\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp81, "return of NetrServerReqChallenge, state S146");
                this.Manager.Comment("reaching state \'S215\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp82;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp82 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S284\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp82, "return of NetrServerAuthenticate3, state S284");
                this.Manager.Comment("reaching state \'S353\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp83;
                this.Manager.Comment("executing step \'call NetrLogonSamLogoff(PrimaryDc,Client,Valid,True,NetlogonInter" +
                        "activeInformation,Valid)\'");
                temp83 = this.INrpcServerAdapterInstance.NetrLogonSamLogoff(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, ((Microsoft.Protocols.TestSuites.Nrpc.AuthenticatorType)(1)), true, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)));
                this.Manager.Checkpoint("MS-NRPC_R1270");
                this.Manager.Checkpoint("MS-NRPC_R1258");
                this.Manager.Comment("reaching state \'S422\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogoff/STATUS_INVALID_PARAMETER\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_INVALID_PARAMETER, temp83, "return of NetrLogonSamLogoff, state S422");
                Test_PassThroughAuthenticate_NetrLogonSamLogoffS461();
                goto label7;
            }
            if ((temp87 == 2)) {
                this.Manager.Comment("reaching state \'S78\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp84;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp84 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S147\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp84, "return of NetrServerReqChallenge, state S147");
                this.Manager.Comment("reaching state \'S216\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp85;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp85 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S285\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp85, "return of NetrServerAuthenticate3, state S285");
                this.Manager.Comment("reaching state \'S354\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp86;
                this.Manager.Comment("executing step \'call NetrLogonSamLogoff(NonDcServer,Client,Valid,False,NetlogonIn" +
                        "teractiveInformation,Valid)\'");
                temp86 = this.INrpcServerAdapterInstance.NetrLogonSamLogoff(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, ((Microsoft.Protocols.TestSuites.Nrpc.AuthenticatorType)(1)), false, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)));
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R1533");
                this.Manager.Checkpoint("MS-NRPC_R1258");
                this.Manager.Comment("reaching state \'S423\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogoff/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp86, "return of NetrLogonSamLogoff, state S423");
                this.Manager.Comment("reaching state \'S472\'");
                goto label7;
            }
            throw new InvalidOperationException("never reached");
        label7:
;
            this.Manager.EndTest();
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogoffS20GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S21");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogoffS20GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S21");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogoffS20GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S21");
        }
        #endregion
        
        #region Test Starting in S22
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_PassThroughAuthenticate_NetrLogonSamLogoffS22() {
            this.Manager.BeginTest("Test_PassThroughAuthenticate_NetrLogonSamLogoffS22");
            this.Manager.Comment("reaching state \'S22\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp88;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp88);
            this.Manager.AddReturn(GetPlatformInfo, null, temp88);
            this.Manager.Comment("reaching state \'S23\'");
            int temp98 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogoff.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogoffS22GetPlatformChecker)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogoff.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogoffS22GetPlatformChecker1)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogoff.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogoffS22GetPlatformChecker2)));
            if ((temp98 == 0)) {
                this.Manager.Comment("reaching state \'S79\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp89;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp89 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S148\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp89, "return of NetrServerReqChallenge, state S148");
                this.Manager.Comment("reaching state \'S217\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp90;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp90 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S286\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp90, "return of NetrServerAuthenticate3, state S286");
                this.Manager.Comment("reaching state \'S355\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp91;
                this.Manager.Comment("executing step \'call NetrLogonSamLogoff(PrimaryDc,Client,Invalid,False,NetlogonIn" +
                        "teractiveInformation,Valid)\'");
                temp91 = this.INrpcServerAdapterInstance.NetrLogonSamLogoff(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, Microsoft.Protocols.TestSuites.Nrpc.AuthenticatorType.Invalid, false, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)));
                this.Manager.Checkpoint("MS-NRPC_R1265");
                this.Manager.Checkpoint("MS-NRPC_R1258");
                this.Manager.Comment("reaching state \'S424\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogoff/STATUS_ACCESS_DENIED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp91, "return of NetrLogonSamLogoff, state S424");
                Test_PassThroughAuthenticate_NetrLogonSamLogoffS462();
                goto label8;
            }
            if ((temp98 == 1)) {
                this.Manager.Comment("reaching state \'S80\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp92;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp92 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S149\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp92, "return of NetrServerReqChallenge, state S149");
                this.Manager.Comment("reaching state \'S218\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp93;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp93 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S287\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp93, "return of NetrServerAuthenticate3, state S287");
                this.Manager.Comment("reaching state \'S356\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp94;
                this.Manager.Comment("executing step \'call NetrLogonSamLogoff(PrimaryDc,Client,Invalid,False,NetlogonIn" +
                        "teractiveInformation,Valid)\'");
                temp94 = this.INrpcServerAdapterInstance.NetrLogonSamLogoff(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, Microsoft.Protocols.TestSuites.Nrpc.AuthenticatorType.Invalid, false, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)));
                this.Manager.Checkpoint("MS-NRPC_R1265");
                this.Manager.Checkpoint("MS-NRPC_R1258");
                this.Manager.Comment("reaching state \'S425\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogoff/STATUS_ACCESS_DENIED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp94, "return of NetrLogonSamLogoff, state S425");
                Test_PassThroughAuthenticate_NetrLogonSamLogoffS461();
                goto label8;
            }
            if ((temp98 == 2)) {
                this.Manager.Comment("reaching state \'S81\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp95;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp95 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S150\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp95, "return of NetrServerReqChallenge, state S150");
                this.Manager.Comment("reaching state \'S219\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp96;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp96 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S288\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp96, "return of NetrServerAuthenticate3, state S288");
                this.Manager.Comment("reaching state \'S357\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp97;
                this.Manager.Comment("executing step \'call NetrLogonSamLogoff(NonDcServer,Client,Valid,False,NetlogonIn" +
                        "teractiveInformation,Valid)\'");
                temp97 = this.INrpcServerAdapterInstance.NetrLogonSamLogoff(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, ((Microsoft.Protocols.TestSuites.Nrpc.AuthenticatorType)(1)), false, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)));
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R1533");
                this.Manager.Checkpoint("MS-NRPC_R1258");
                this.Manager.Comment("reaching state \'S426\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogoff/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp97, "return of NetrLogonSamLogoff, state S426");
                this.Manager.Comment("reaching state \'S473\'");
                goto label8;
            }
            throw new InvalidOperationException("never reached");
        label8:
;
            this.Manager.EndTest();
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogoffS22GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S23");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogoffS22GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S23");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogoffS22GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S23");
        }
        #endregion
        
        #region Test Starting in S24
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_PassThroughAuthenticate_NetrLogonSamLogoffS24() {
            this.Manager.BeginTest("Test_PassThroughAuthenticate_NetrLogonSamLogoffS24");
            this.Manager.Comment("reaching state \'S24\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp99;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp99);
            this.Manager.AddReturn(GetPlatformInfo, null, temp99);
            this.Manager.Comment("reaching state \'S25\'");
            int temp109 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogoff.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogoffS24GetPlatformChecker)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogoff.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogoffS24GetPlatformChecker1)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogoff.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogoffS24GetPlatformChecker2)));
            if ((temp109 == 0)) {
                this.Manager.Comment("reaching state \'S82\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp100;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp100 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S151\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp100, "return of NetrServerReqChallenge, state S151");
                this.Manager.Comment("reaching state \'S220\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp101;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp101 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S289\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp101, "return of NetrServerAuthenticate3, state S289");
                this.Manager.Comment("reaching state \'S358\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp102;
                this.Manager.Comment("executing step \'call NetrLogonSamLogoff(NonDcServer,Client,Valid,False,NetlogonIn" +
                        "teractiveInformation,Valid)\'");
                temp102 = this.INrpcServerAdapterInstance.NetrLogonSamLogoff(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, ((Microsoft.Protocols.TestSuites.Nrpc.AuthenticatorType)(1)), false, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)));
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R1533");
                this.Manager.Checkpoint("MS-NRPC_R1258");
                this.Manager.Comment("reaching state \'S427\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogoff/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp102, "return of NetrLogonSamLogoff, state S427");
                Test_PassThroughAuthenticate_NetrLogonSamLogoffS460();
                goto label9;
            }
            if ((temp109 == 1)) {
                this.Manager.Comment("reaching state \'S83\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp103;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp103 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S152\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp103, "return of NetrServerReqChallenge, state S152");
                this.Manager.Comment("reaching state \'S221\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp104;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp104 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S290\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp104, "return of NetrServerAuthenticate3, state S290");
                this.Manager.Comment("reaching state \'S359\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp105;
                this.Manager.Comment("executing step \'call NetrLogonSamLogoff(NonDcServer,Client,Valid,False,NetlogonIn" +
                        "teractiveInformation,Valid)\'");
                temp105 = this.INrpcServerAdapterInstance.NetrLogonSamLogoff(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, ((Microsoft.Protocols.TestSuites.Nrpc.AuthenticatorType)(1)), false, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)));
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R1533");
                this.Manager.Checkpoint("MS-NRPC_R1258");
                this.Manager.Comment("reaching state \'S428\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogoff/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp105, "return of NetrLogonSamLogoff, state S428");
                this.Manager.Comment("reaching state \'S474\'");
                goto label9;
            }
            if ((temp109 == 2)) {
                this.Manager.Comment("reaching state \'S84\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp106;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp106 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S153\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp106, "return of NetrServerReqChallenge, state S153");
                this.Manager.Comment("reaching state \'S222\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp107;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp107 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S291\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp107, "return of NetrServerAuthenticate3, state S291");
                this.Manager.Comment("reaching state \'S360\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp108;
                this.Manager.Comment("executing step \'call NetrLogonSamLogoff(NonDcServer,Client,Valid,False,NetlogonIn" +
                        "teractiveInformation,Valid)\'");
                temp108 = this.INrpcServerAdapterInstance.NetrLogonSamLogoff(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, ((Microsoft.Protocols.TestSuites.Nrpc.AuthenticatorType)(1)), false, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)));
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R1533");
                this.Manager.Checkpoint("MS-NRPC_R1258");
                this.Manager.Comment("reaching state \'S429\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogoff/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp108, "return of NetrLogonSamLogoff, state S429");
                this.Manager.Comment("reaching state \'S475\'");
                goto label9;
            }
            throw new InvalidOperationException("never reached");
        label9:
;
            this.Manager.EndTest();
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogoffS24GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S25");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogoffS24GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S25");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogoffS24GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S25");
        }
        #endregion
        
        #region Test Starting in S26
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_PassThroughAuthenticate_NetrLogonSamLogoffS26() {
            this.Manager.BeginTest("Test_PassThroughAuthenticate_NetrLogonSamLogoffS26");
            this.Manager.Comment("reaching state \'S26\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp110;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp110);
            this.Manager.AddReturn(GetPlatformInfo, null, temp110);
            this.Manager.Comment("reaching state \'S27\'");
            int temp120 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogoff.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogoffS26GetPlatformChecker)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogoff.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogoffS26GetPlatformChecker1)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogoff.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogoffS26GetPlatformChecker2)));
            if ((temp120 == 0)) {
                this.Manager.Comment("reaching state \'S85\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp111;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp111 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S154\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp111, "return of NetrServerReqChallenge, state S154");
                this.Manager.Comment("reaching state \'S223\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp112;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp112 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S292\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp112, "return of NetrServerAuthenticate3, state S292");
                this.Manager.Comment("reaching state \'S361\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp113;
                this.Manager.Comment("executing step \'call NetrLogonSamLogoff(PrimaryDc,Null,Valid,False,NetlogonIntera" +
                        "ctiveInformation,Valid)\'");
                temp113 = this.INrpcServerAdapterInstance.NetrLogonSamLogoff(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.ComputerType)(0)), ((Microsoft.Protocols.TestSuites.Nrpc.AuthenticatorType)(1)), false, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)));
                this.Manager.Checkpoint("MS-NRPC_R1267");
                this.Manager.Checkpoint("MS-NRPC_R1258");
                this.Manager.Comment("reaching state \'S430\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogoff/STATUS_INVALID_PARAMETER\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_INVALID_PARAMETER, temp113, "return of NetrLogonSamLogoff, state S430");
                Test_PassThroughAuthenticate_NetrLogonSamLogoffS460();
                goto label10;
            }
            if ((temp120 == 1)) {
                this.Manager.Comment("reaching state \'S86\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp114;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp114 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S155\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp114, "return of NetrServerReqChallenge, state S155");
                this.Manager.Comment("reaching state \'S224\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp115;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp115 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S293\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp115, "return of NetrServerAuthenticate3, state S293");
                this.Manager.Comment("reaching state \'S362\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp116;
                this.Manager.Comment("executing step \'call NetrLogonSamLogoff(NonDcServer,Client,Valid,False,NetlogonIn" +
                        "teractiveInformation,Valid)\'");
                temp116 = this.INrpcServerAdapterInstance.NetrLogonSamLogoff(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, ((Microsoft.Protocols.TestSuites.Nrpc.AuthenticatorType)(1)), false, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)));
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R1533");
                this.Manager.Checkpoint("MS-NRPC_R1258");
                this.Manager.Comment("reaching state \'S431\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogoff/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp116, "return of NetrLogonSamLogoff, state S431");
                this.Manager.Comment("reaching state \'S476\'");
                goto label10;
            }
            if ((temp120 == 2)) {
                this.Manager.Comment("reaching state \'S87\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp117;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp117 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S156\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp117, "return of NetrServerReqChallenge, state S156");
                this.Manager.Comment("reaching state \'S225\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp118;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp118 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S294\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp118, "return of NetrServerAuthenticate3, state S294");
                this.Manager.Comment("reaching state \'S363\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp119;
                this.Manager.Comment("executing step \'call NetrLogonSamLogoff(NonDcServer,Client,Valid,False,NetlogonIn" +
                        "teractiveInformation,Valid)\'");
                temp119 = this.INrpcServerAdapterInstance.NetrLogonSamLogoff(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, ((Microsoft.Protocols.TestSuites.Nrpc.AuthenticatorType)(1)), false, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)));
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R1533");
                this.Manager.Checkpoint("MS-NRPC_R1258");
                this.Manager.Comment("reaching state \'S432\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogoff/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp119, "return of NetrLogonSamLogoff, state S432");
                this.Manager.Comment("reaching state \'S477\'");
                goto label10;
            }
            throw new InvalidOperationException("never reached");
        label10:
;
            this.Manager.EndTest();
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogoffS26GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S27");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogoffS26GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S27");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogoffS26GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S27");
        }
        #endregion
        
        #region Test Starting in S28
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_PassThroughAuthenticate_NetrLogonSamLogoffS28() {
            this.Manager.BeginTest("Test_PassThroughAuthenticate_NetrLogonSamLogoffS28");
            this.Manager.Comment("reaching state \'S28\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp121;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp121);
            this.Manager.AddReturn(GetPlatformInfo, null, temp121);
            this.Manager.Comment("reaching state \'S29\'");
            int temp131 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogoff.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogoffS28GetPlatformChecker)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogoff.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogoffS28GetPlatformChecker1)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogoff.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogoffS28GetPlatformChecker2)));
            if ((temp131 == 0)) {
                this.Manager.Comment("reaching state \'S88\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp122;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp122 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S157\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp122, "return of NetrServerReqChallenge, state S157");
                this.Manager.Comment("reaching state \'S226\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp123;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp123 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S295\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp123, "return of NetrServerAuthenticate3, state S295");
                this.Manager.Comment("reaching state \'S364\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp124;
                this.Manager.Comment("executing step \'call NetrLogonSamLogoff(PrimaryDc,Client,Valid,False,NetlogonInte" +
                        "ractiveInformation,Null)\'");
                temp124 = this.INrpcServerAdapterInstance.NetrLogonSamLogoff(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, ((Microsoft.Protocols.TestSuites.Nrpc.AuthenticatorType)(1)), false, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(0)));
                this.Manager.Checkpoint("MS-NRPC_R1260");
                this.Manager.Checkpoint("MS-NRPC_R1258");
                this.Manager.Comment("reaching state \'S433\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogoff/STATUS_INVALID_PARAMETER\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_INVALID_PARAMETER, temp124, "return of NetrLogonSamLogoff, state S433");
                Test_PassThroughAuthenticate_NetrLogonSamLogoffS460();
                goto label11;
            }
            if ((temp131 == 1)) {
                this.Manager.Comment("reaching state \'S89\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp125;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp125 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S158\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp125, "return of NetrServerReqChallenge, state S158");
                this.Manager.Comment("reaching state \'S227\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp126;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp126 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S296\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp126, "return of NetrServerAuthenticate3, state S296");
                this.Manager.Comment("reaching state \'S365\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp127;
                this.Manager.Comment("executing step \'call NetrLogonSamLogoff(NonDcServer,Client,Valid,False,NetlogonIn" +
                        "teractiveInformation,Valid)\'");
                temp127 = this.INrpcServerAdapterInstance.NetrLogonSamLogoff(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, ((Microsoft.Protocols.TestSuites.Nrpc.AuthenticatorType)(1)), false, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)));
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R1533");
                this.Manager.Checkpoint("MS-NRPC_R1258");
                this.Manager.Comment("reaching state \'S434\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogoff/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp127, "return of NetrLogonSamLogoff, state S434");
                this.Manager.Comment("reaching state \'S478\'");
                goto label11;
            }
            if ((temp131 == 2)) {
                this.Manager.Comment("reaching state \'S90\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp128;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp128 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S159\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp128, "return of NetrServerReqChallenge, state S159");
                this.Manager.Comment("reaching state \'S228\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp129;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp129 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S297\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp129, "return of NetrServerAuthenticate3, state S297");
                this.Manager.Comment("reaching state \'S366\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp130;
                this.Manager.Comment("executing step \'call NetrLogonSamLogoff(NonDcServer,Client,Valid,False,NetlogonIn" +
                        "teractiveInformation,Valid)\'");
                temp130 = this.INrpcServerAdapterInstance.NetrLogonSamLogoff(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, ((Microsoft.Protocols.TestSuites.Nrpc.AuthenticatorType)(1)), false, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)));
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R1533");
                this.Manager.Checkpoint("MS-NRPC_R1258");
                this.Manager.Comment("reaching state \'S435\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogoff/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp130, "return of NetrLogonSamLogoff, state S435");
                this.Manager.Comment("reaching state \'S479\'");
                goto label11;
            }
            throw new InvalidOperationException("never reached");
        label11:
;
            this.Manager.EndTest();
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogoffS28GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S29");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogoffS28GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S29");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogoffS28GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S29");
        }
        #endregion
        
        #region Test Starting in S30
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_PassThroughAuthenticate_NetrLogonSamLogoffS30() {
            this.Manager.BeginTest("Test_PassThroughAuthenticate_NetrLogonSamLogoffS30");
            this.Manager.Comment("reaching state \'S30\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp132;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp132);
            this.Manager.AddReturn(GetPlatformInfo, null, temp132);
            this.Manager.Comment("reaching state \'S31\'");
            int temp142 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogoff.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogoffS30GetPlatformChecker)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogoff.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogoffS30GetPlatformChecker1)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogoff.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogoffS30GetPlatformChecker2)));
            if ((temp142 == 0)) {
                this.Manager.Comment("reaching state \'S91\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp133;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp133 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S160\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp133, "return of NetrServerReqChallenge, state S160");
                this.Manager.Comment("reaching state \'S229\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp134;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp134 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S298\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp134, "return of NetrServerAuthenticate3, state S298");
                this.Manager.Comment("reaching state \'S367\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp135;
                this.Manager.Comment("executing step \'call NetrLogonSamLogoff(PrimaryDc,Client,Valid,False,NetlogonInte" +
                        "ractiveInformation,Valid)\'");
                temp135 = this.INrpcServerAdapterInstance.NetrLogonSamLogoff(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, ((Microsoft.Protocols.TestSuites.Nrpc.AuthenticatorType)(1)), false, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)));
                this.Manager.Checkpoint("MS-NRPC_R1257");
                this.Manager.Comment("reaching state \'S436\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogoff/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp135, "return of NetrLogonSamLogoff, state S436");
                Test_PassThroughAuthenticate_NetrLogonSamLogoffS460();
                goto label12;
            }
            if ((temp142 == 1)) {
                this.Manager.Comment("reaching state \'S92\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp136;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp136 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S161\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp136, "return of NetrServerReqChallenge, state S161");
                this.Manager.Comment("reaching state \'S230\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp137;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp137 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S299\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp137, "return of NetrServerAuthenticate3, state S299");
                this.Manager.Comment("reaching state \'S368\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp138;
                this.Manager.Comment("executing step \'call NetrLogonSamLogoff(NonDcServer,Client,Valid,False,NetlogonIn" +
                        "teractiveInformation,Valid)\'");
                temp138 = this.INrpcServerAdapterInstance.NetrLogonSamLogoff(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, ((Microsoft.Protocols.TestSuites.Nrpc.AuthenticatorType)(1)), false, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)));
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R1533");
                this.Manager.Checkpoint("MS-NRPC_R1258");
                this.Manager.Comment("reaching state \'S437\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogoff/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp138, "return of NetrLogonSamLogoff, state S437");
                this.Manager.Comment("reaching state \'S480\'");
                goto label12;
            }
            if ((temp142 == 2)) {
                this.Manager.Comment("reaching state \'S93\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp139;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp139 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S162\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp139, "return of NetrServerReqChallenge, state S162");
                this.Manager.Comment("reaching state \'S231\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp140;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp140 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S300\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp140, "return of NetrServerAuthenticate3, state S300");
                this.Manager.Comment("reaching state \'S369\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp141;
                this.Manager.Comment("executing step \'call NetrLogonSamLogoff(NonDcServer,Client,Valid,False,NetlogonIn" +
                        "teractiveInformation,Valid)\'");
                temp141 = this.INrpcServerAdapterInstance.NetrLogonSamLogoff(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, ((Microsoft.Protocols.TestSuites.Nrpc.AuthenticatorType)(1)), false, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)));
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R1533");
                this.Manager.Checkpoint("MS-NRPC_R1258");
                this.Manager.Comment("reaching state \'S438\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogoff/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp141, "return of NetrLogonSamLogoff, state S438");
                this.Manager.Comment("reaching state \'S481\'");
                goto label12;
            }
            throw new InvalidOperationException("never reached");
        label12:
;
            this.Manager.EndTest();
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogoffS30GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S31");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogoffS30GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S31");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogoffS30GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S31");
        }
        #endregion
        
        #region Test Starting in S32
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_PassThroughAuthenticate_NetrLogonSamLogoffS32() {
            this.Manager.BeginTest("Test_PassThroughAuthenticate_NetrLogonSamLogoffS32");
            this.Manager.Comment("reaching state \'S32\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp143;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp143);
            this.Manager.AddReturn(GetPlatformInfo, null, temp143);
            this.Manager.Comment("reaching state \'S33\'");
            int temp153 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogoff.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogoffS32GetPlatformChecker)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogoff.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogoffS32GetPlatformChecker1)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogoff.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogoffS32GetPlatformChecker2)));
            if ((temp153 == 0)) {
                this.Manager.Comment("reaching state \'S94\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp144;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp144 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S163\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp144, "return of NetrServerReqChallenge, state S163");
                this.Manager.Comment("reaching state \'S232\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp145;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp145 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S301\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp145, "return of NetrServerAuthenticate3, state S301");
                this.Manager.Comment("reaching state \'S370\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp146;
                this.Manager.Comment("executing step \'call NetrLogonSamLogoff(PrimaryDc,Client,Valid,False,NetlogonServ" +
                        "iceTransitiveInformation,Valid)\'");
                temp146 = this.INrpcServerAdapterInstance.NetrLogonSamLogoff(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, ((Microsoft.Protocols.TestSuites.Nrpc.AuthenticatorType)(1)), false, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonServiceTransitiveInformation, ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)));
                this.Manager.Checkpoint("MS-NRPC_R1272");
                this.Manager.Checkpoint("MS-NRPC_R1258");
                this.Manager.Comment("reaching state \'S439\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogoff/STATUS_INVALID_INFO_CLASS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_INVALID_INFO_CLASS, temp146, "return of NetrLogonSamLogoff, state S439");
                Test_PassThroughAuthenticate_NetrLogonSamLogoffS460();
                goto label13;
            }
            if ((temp153 == 1)) {
                this.Manager.Comment("reaching state \'S95\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp147;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp147 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S164\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp147, "return of NetrServerReqChallenge, state S164");
                this.Manager.Comment("reaching state \'S233\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp148;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp148 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S302\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp148, "return of NetrServerAuthenticate3, state S302");
                this.Manager.Comment("reaching state \'S371\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp149;
                this.Manager.Comment("executing step \'call NetrLogonSamLogoff(NonDcServer,Client,Valid,False,NetlogonIn" +
                        "teractiveInformation,Valid)\'");
                temp149 = this.INrpcServerAdapterInstance.NetrLogonSamLogoff(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, ((Microsoft.Protocols.TestSuites.Nrpc.AuthenticatorType)(1)), false, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)));
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R1533");
                this.Manager.Checkpoint("MS-NRPC_R1258");
                this.Manager.Comment("reaching state \'S440\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogoff/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp149, "return of NetrLogonSamLogoff, state S440");
                this.Manager.Comment("reaching state \'S482\'");
                goto label13;
            }
            if ((temp153 == 2)) {
                this.Manager.Comment("reaching state \'S96\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp150;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp150 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S165\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp150, "return of NetrServerReqChallenge, state S165");
                this.Manager.Comment("reaching state \'S234\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp151;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp151 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S303\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp151, "return of NetrServerAuthenticate3, state S303");
                this.Manager.Comment("reaching state \'S372\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp152;
                this.Manager.Comment("executing step \'call NetrLogonSamLogoff(NonDcServer,Client,Valid,False,NetlogonIn" +
                        "teractiveInformation,Valid)\'");
                temp152 = this.INrpcServerAdapterInstance.NetrLogonSamLogoff(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, ((Microsoft.Protocols.TestSuites.Nrpc.AuthenticatorType)(1)), false, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)));
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R1533");
                this.Manager.Checkpoint("MS-NRPC_R1258");
                this.Manager.Comment("reaching state \'S441\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogoff/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp152, "return of NetrLogonSamLogoff, state S441");
                this.Manager.Comment("reaching state \'S483\'");
                goto label13;
            }
            throw new InvalidOperationException("never reached");
        label13:
;
            this.Manager.EndTest();
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogoffS32GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S33");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogoffS32GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S33");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogoffS32GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S33");
        }
        #endregion
        
        #region Test Starting in S34
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_PassThroughAuthenticate_NetrLogonSamLogoffS34() {
            this.Manager.BeginTest("Test_PassThroughAuthenticate_NetrLogonSamLogoffS34");
            this.Manager.Comment("reaching state \'S34\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp154;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp154);
            this.Manager.AddReturn(GetPlatformInfo, null, temp154);
            this.Manager.Comment("reaching state \'S35\'");
            int temp164 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogoff.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogoffS34GetPlatformChecker)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogoff.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogoffS34GetPlatformChecker1)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogoff.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogoffS34GetPlatformChecker2)));
            if ((temp164 == 0)) {
                this.Manager.Comment("reaching state \'S97\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp155;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp155 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S166\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp155, "return of NetrServerReqChallenge, state S166");
                this.Manager.Comment("reaching state \'S235\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp156;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp156 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S304\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp156, "return of NetrServerAuthenticate3, state S304");
                this.Manager.Comment("reaching state \'S373\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp157;
                this.Manager.Comment("executing step \'call NetrLogonSamLogoff(PrimaryDc,Client,Valid,False,NetlogonNetw" +
                        "orkTransitiveInformation,Valid)\'");
                temp157 = this.INrpcServerAdapterInstance.NetrLogonSamLogoff(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, ((Microsoft.Protocols.TestSuites.Nrpc.AuthenticatorType)(1)), false, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonNetworkTransitiveInformation, ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)));
                this.Manager.Checkpoint("MS-NRPC_R1272");
                this.Manager.Checkpoint("MS-NRPC_R1258");
                this.Manager.Comment("reaching state \'S442\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogoff/STATUS_INVALID_INFO_CLASS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_INVALID_INFO_CLASS, temp157, "return of NetrLogonSamLogoff, state S442");
                Test_PassThroughAuthenticate_NetrLogonSamLogoffS460();
                goto label14;
            }
            if ((temp164 == 1)) {
                this.Manager.Comment("reaching state \'S98\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp158;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp158 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S167\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp158, "return of NetrServerReqChallenge, state S167");
                this.Manager.Comment("reaching state \'S236\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp159;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp159 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S305\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp159, "return of NetrServerAuthenticate3, state S305");
                this.Manager.Comment("reaching state \'S374\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp160;
                this.Manager.Comment("executing step \'call NetrLogonSamLogoff(NonDcServer,Client,Valid,False,NetlogonIn" +
                        "teractiveInformation,Valid)\'");
                temp160 = this.INrpcServerAdapterInstance.NetrLogonSamLogoff(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, ((Microsoft.Protocols.TestSuites.Nrpc.AuthenticatorType)(1)), false, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)));
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R1533");
                this.Manager.Checkpoint("MS-NRPC_R1258");
                this.Manager.Comment("reaching state \'S443\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogoff/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp160, "return of NetrLogonSamLogoff, state S443");
                this.Manager.Comment("reaching state \'S484\'");
                goto label14;
            }
            if ((temp164 == 2)) {
                this.Manager.Comment("reaching state \'S99\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp161;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp161 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S168\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp161, "return of NetrServerReqChallenge, state S168");
                this.Manager.Comment("reaching state \'S237\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp162;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp162 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S306\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp162, "return of NetrServerAuthenticate3, state S306");
                this.Manager.Comment("reaching state \'S375\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp163;
                this.Manager.Comment("executing step \'call NetrLogonSamLogoff(NonDcServer,Client,Valid,False,NetlogonIn" +
                        "teractiveInformation,Valid)\'");
                temp163 = this.INrpcServerAdapterInstance.NetrLogonSamLogoff(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, ((Microsoft.Protocols.TestSuites.Nrpc.AuthenticatorType)(1)), false, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)));
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R1533");
                this.Manager.Checkpoint("MS-NRPC_R1258");
                this.Manager.Comment("reaching state \'S444\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogoff/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp163, "return of NetrLogonSamLogoff, state S444");
                this.Manager.Comment("reaching state \'S485\'");
                goto label14;
            }
            throw new InvalidOperationException("never reached");
        label14:
;
            this.Manager.EndTest();
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogoffS34GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S35");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogoffS34GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S35");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogoffS34GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S35");
        }
        #endregion
        
        #region Test Starting in S36
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_PassThroughAuthenticate_NetrLogonSamLogoffS36() {
            this.Manager.BeginTest("Test_PassThroughAuthenticate_NetrLogonSamLogoffS36");
            this.Manager.Comment("reaching state \'S36\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp165;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp165);
            this.Manager.AddReturn(GetPlatformInfo, null, temp165);
            this.Manager.Comment("reaching state \'S37\'");
            int temp175 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogoff.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogoffS36GetPlatformChecker)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogoff.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogoffS36GetPlatformChecker1)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogoff.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogoffS36GetPlatformChecker2)));
            if ((temp175 == 0)) {
                this.Manager.Comment("reaching state \'S100\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp166;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp166 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S169\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp166, "return of NetrServerReqChallenge, state S169");
                this.Manager.Comment("reaching state \'S238\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp167;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp167 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S307\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp167, "return of NetrServerAuthenticate3, state S307");
                this.Manager.Comment("reaching state \'S376\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp168;
                this.Manager.Comment("executing step \'call NetrLogonSamLogoff(PrimaryDc,Client,Valid,False,NetlogonNetw" +
                        "orkInformation,Valid)\'");
                temp168 = this.INrpcServerAdapterInstance.NetrLogonSamLogoff(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, ((Microsoft.Protocols.TestSuites.Nrpc.AuthenticatorType)(1)), false, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonNetworkInformation, ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)));
                this.Manager.Checkpoint("MS-NRPC_R1272");
                this.Manager.Checkpoint("MS-NRPC_R1258");
                this.Manager.Comment("reaching state \'S445\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogoff/STATUS_INVALID_INFO_CLASS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_INVALID_INFO_CLASS, temp168, "return of NetrLogonSamLogoff, state S445");
                Test_PassThroughAuthenticate_NetrLogonSamLogoffS460();
                goto label15;
            }
            if ((temp175 == 1)) {
                this.Manager.Comment("reaching state \'S101\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp169;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp169 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S170\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp169, "return of NetrServerReqChallenge, state S170");
                this.Manager.Comment("reaching state \'S239\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp170;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp170 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S308\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp170, "return of NetrServerAuthenticate3, state S308");
                this.Manager.Comment("reaching state \'S377\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp171;
                this.Manager.Comment("executing step \'call NetrLogonSamLogoff(NonDcServer,Client,Valid,False,NetlogonIn" +
                        "teractiveInformation,Valid)\'");
                temp171 = this.INrpcServerAdapterInstance.NetrLogonSamLogoff(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, ((Microsoft.Protocols.TestSuites.Nrpc.AuthenticatorType)(1)), false, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)));
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R1533");
                this.Manager.Checkpoint("MS-NRPC_R1258");
                this.Manager.Comment("reaching state \'S446\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogoff/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp171, "return of NetrLogonSamLogoff, state S446");
                this.Manager.Comment("reaching state \'S486\'");
                goto label15;
            }
            if ((temp175 == 2)) {
                this.Manager.Comment("reaching state \'S102\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp172;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp172 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S171\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp172, "return of NetrServerReqChallenge, state S171");
                this.Manager.Comment("reaching state \'S240\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp173;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp173 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S309\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp173, "return of NetrServerAuthenticate3, state S309");
                this.Manager.Comment("reaching state \'S378\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp174;
                this.Manager.Comment("executing step \'call NetrLogonSamLogoff(NonDcServer,Client,Valid,False,NetlogonIn" +
                        "teractiveInformation,Valid)\'");
                temp174 = this.INrpcServerAdapterInstance.NetrLogonSamLogoff(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, ((Microsoft.Protocols.TestSuites.Nrpc.AuthenticatorType)(1)), false, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)));
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R1533");
                this.Manager.Checkpoint("MS-NRPC_R1258");
                this.Manager.Comment("reaching state \'S447\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogoff/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp174, "return of NetrLogonSamLogoff, state S447");
                this.Manager.Comment("reaching state \'S487\'");
                goto label15;
            }
            throw new InvalidOperationException("never reached");
        label15:
;
            this.Manager.EndTest();
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogoffS36GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S37");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogoffS36GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S37");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogoffS36GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S37");
        }
        #endregion
        
        #region Test Starting in S38
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_PassThroughAuthenticate_NetrLogonSamLogoffS38() {
            this.Manager.BeginTest("Test_PassThroughAuthenticate_NetrLogonSamLogoffS38");
            this.Manager.Comment("reaching state \'S38\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp176;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp176);
            this.Manager.AddReturn(GetPlatformInfo, null, temp176);
            this.Manager.Comment("reaching state \'S39\'");
            int temp186 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogoff.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogoffS38GetPlatformChecker)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogoff.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogoffS38GetPlatformChecker1)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogoff.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogoffS38GetPlatformChecker2)));
            if ((temp186 == 0)) {
                this.Manager.Comment("reaching state \'S103\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp177;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp177 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S172\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp177, "return of NetrServerReqChallenge, state S172");
                this.Manager.Comment("reaching state \'S241\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp178;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp178 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S310\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp178, "return of NetrServerAuthenticate3, state S310");
                this.Manager.Comment("reaching state \'S379\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp179;
                this.Manager.Comment("executing step \'call NetrLogonSamLogoff(PrimaryDc,Client,Valid,False,NetlogonServ" +
                        "iceInformation,Valid)\'");
                temp179 = this.INrpcServerAdapterInstance.NetrLogonSamLogoff(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, ((Microsoft.Protocols.TestSuites.Nrpc.AuthenticatorType)(1)), false, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonServiceInformation, ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)));
                this.Manager.Checkpoint("MS-NRPC_R1272");
                this.Manager.Checkpoint("MS-NRPC_R1258");
                this.Manager.Comment("reaching state \'S448\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogoff/STATUS_INVALID_INFO_CLASS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_INVALID_INFO_CLASS, temp179, "return of NetrLogonSamLogoff, state S448");
                Test_PassThroughAuthenticate_NetrLogonSamLogoffS460();
                goto label16;
            }
            if ((temp186 == 1)) {
                this.Manager.Comment("reaching state \'S104\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp180;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp180 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S173\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp180, "return of NetrServerReqChallenge, state S173");
                this.Manager.Comment("reaching state \'S242\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp181;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp181 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S311\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp181, "return of NetrServerAuthenticate3, state S311");
                this.Manager.Comment("reaching state \'S380\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp182;
                this.Manager.Comment("executing step \'call NetrLogonSamLogoff(NonDcServer,Client,Valid,False,NetlogonIn" +
                        "teractiveInformation,Valid)\'");
                temp182 = this.INrpcServerAdapterInstance.NetrLogonSamLogoff(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, ((Microsoft.Protocols.TestSuites.Nrpc.AuthenticatorType)(1)), false, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)));
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R1533");
                this.Manager.Checkpoint("MS-NRPC_R1258");
                this.Manager.Comment("reaching state \'S449\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogoff/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp182, "return of NetrLogonSamLogoff, state S449");
                this.Manager.Comment("reaching state \'S488\'");
                goto label16;
            }
            if ((temp186 == 2)) {
                this.Manager.Comment("reaching state \'S105\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp183;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp183 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S174\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp183, "return of NetrServerReqChallenge, state S174");
                this.Manager.Comment("reaching state \'S243\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp184;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp184 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S312\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp184, "return of NetrServerAuthenticate3, state S312");
                this.Manager.Comment("reaching state \'S381\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp185;
                this.Manager.Comment("executing step \'call NetrLogonSamLogoff(NonDcServer,Client,Valid,False,NetlogonIn" +
                        "teractiveInformation,Valid)\'");
                temp185 = this.INrpcServerAdapterInstance.NetrLogonSamLogoff(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, ((Microsoft.Protocols.TestSuites.Nrpc.AuthenticatorType)(1)), false, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)));
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R1533");
                this.Manager.Checkpoint("MS-NRPC_R1258");
                this.Manager.Comment("reaching state \'S450\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogoff/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp185, "return of NetrLogonSamLogoff, state S450");
                this.Manager.Comment("reaching state \'S489\'");
                goto label16;
            }
            throw new InvalidOperationException("never reached");
        label16:
;
            this.Manager.EndTest();
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogoffS38GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S39");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogoffS38GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S39");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogoffS38GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S39");
        }
        #endregion
        
        #region Test Starting in S4
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_PassThroughAuthenticate_NetrLogonSamLogoffS4() {
            this.Manager.BeginTest("Test_PassThroughAuthenticate_NetrLogonSamLogoffS4");
            this.Manager.Comment("reaching state \'S4\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp187;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp187);
            this.Manager.AddReturn(GetPlatformInfo, null, temp187);
            this.Manager.Comment("reaching state \'S5\'");
            int temp197 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogoff.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogoffS4GetPlatformChecker)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogoff.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogoffS4GetPlatformChecker1)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogoff.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogoffS4GetPlatformChecker2)));
            if ((temp197 == 0)) {
                this.Manager.Comment("reaching state \'S52\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp188;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp188 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S121\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp188, "return of NetrServerReqChallenge, state S121");
                this.Manager.Comment("reaching state \'S190\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp189;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp189 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S259\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp189, "return of NetrServerAuthenticate3, state S259");
                this.Manager.Comment("reaching state \'S328\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp190;
                this.Manager.Comment("executing step \'call NetrLogonSamLogoff(PrimaryDc,Null,Valid,False,NetlogonIntera" +
                        "ctiveInformation,Valid)\'");
                temp190 = this.INrpcServerAdapterInstance.NetrLogonSamLogoff(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.ComputerType)(0)), ((Microsoft.Protocols.TestSuites.Nrpc.AuthenticatorType)(1)), false, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)));
                this.Manager.Checkpoint("MS-NRPC_R1267");
                this.Manager.Checkpoint("MS-NRPC_R1258");
                this.Manager.Comment("reaching state \'S397\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogoff/STATUS_INVALID_PARAMETER\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_INVALID_PARAMETER, temp190, "return of NetrLogonSamLogoff, state S397");
                Test_PassThroughAuthenticate_NetrLogonSamLogoffS462();
                goto label17;
            }
            if ((temp197 == 1)) {
                this.Manager.Comment("reaching state \'S53\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp191;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp191 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S122\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp191, "return of NetrServerReqChallenge, state S122");
                this.Manager.Comment("reaching state \'S191\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp192;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp192 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S260\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp192, "return of NetrServerAuthenticate3, state S260");
                this.Manager.Comment("reaching state \'S329\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp193;
                this.Manager.Comment("executing step \'call NetrLogonSamLogoff(PrimaryDc,Null,Valid,False,NetlogonIntera" +
                        "ctiveInformation,Valid)\'");
                temp193 = this.INrpcServerAdapterInstance.NetrLogonSamLogoff(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.ComputerType)(0)), ((Microsoft.Protocols.TestSuites.Nrpc.AuthenticatorType)(1)), false, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)));
                this.Manager.Checkpoint("MS-NRPC_R1267");
                this.Manager.Checkpoint("MS-NRPC_R1258");
                this.Manager.Comment("reaching state \'S398\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogoff/STATUS_INVALID_PARAMETER\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_INVALID_PARAMETER, temp193, "return of NetrLogonSamLogoff, state S398");
                Test_PassThroughAuthenticate_NetrLogonSamLogoffS461();
                goto label17;
            }
            if ((temp197 == 2)) {
                this.Manager.Comment("reaching state \'S54\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp194;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp194 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S123\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp194, "return of NetrServerReqChallenge, state S123");
                this.Manager.Comment("reaching state \'S192\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp195;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp195 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S261\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp195, "return of NetrServerAuthenticate3, state S261");
                this.Manager.Comment("reaching state \'S330\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp196;
                this.Manager.Comment("executing step \'call NetrLogonSamLogoff(NonDcServer,Client,Valid,False,NetlogonIn" +
                        "teractiveInformation,Valid)\'");
                temp196 = this.INrpcServerAdapterInstance.NetrLogonSamLogoff(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, ((Microsoft.Protocols.TestSuites.Nrpc.AuthenticatorType)(1)), false, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)));
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R1533");
                this.Manager.Checkpoint("MS-NRPC_R1258");
                this.Manager.Comment("reaching state \'S399\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogoff/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp196, "return of NetrLogonSamLogoff, state S399");
                this.Manager.Comment("reaching state \'S464\'");
                goto label17;
            }
            throw new InvalidOperationException("never reached");
        label17:
;
            this.Manager.EndTest();
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogoffS4GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S5");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogoffS4GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S5");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogoffS4GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S5");
        }
        #endregion
        
        #region Test Starting in S40
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_PassThroughAuthenticate_NetrLogonSamLogoffS40() {
            this.Manager.BeginTest("Test_PassThroughAuthenticate_NetrLogonSamLogoffS40");
            this.Manager.Comment("reaching state \'S40\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp198;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp198);
            this.Manager.AddReturn(GetPlatformInfo, null, temp198);
            this.Manager.Comment("reaching state \'S41\'");
            int temp208 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogoff.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogoffS40GetPlatformChecker)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogoff.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogoffS40GetPlatformChecker1)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogoff.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogoffS40GetPlatformChecker2)));
            if ((temp208 == 0)) {
                this.Manager.Comment("reaching state \'S106\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp199;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp199 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S175\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp199, "return of NetrServerReqChallenge, state S175");
                this.Manager.Comment("reaching state \'S244\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp200;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp200 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S313\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp200, "return of NetrServerAuthenticate3, state S313");
                this.Manager.Comment("reaching state \'S382\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp201;
                this.Manager.Comment("executing step \'call NetrLogonSamLogoff(PrimaryDc,Client,Valid,False,NetlogonInte" +
                        "ractiveTransitiveInformation,Valid)\'");
                temp201 = this.INrpcServerAdapterInstance.NetrLogonSamLogoff(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, ((Microsoft.Protocols.TestSuites.Nrpc.AuthenticatorType)(1)), false, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS.NetlogonInteractiveTransitiveInformation, ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)));
                this.Manager.Checkpoint("MS-NRPC_R1272");
                this.Manager.Checkpoint("MS-NRPC_R1258");
                this.Manager.Comment("reaching state \'S451\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogoff/STATUS_INVALID_INFO_CLASS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_INVALID_INFO_CLASS, temp201, "return of NetrLogonSamLogoff, state S451");
                Test_PassThroughAuthenticate_NetrLogonSamLogoffS460();
                goto label18;
            }
            if ((temp208 == 1)) {
                this.Manager.Comment("reaching state \'S107\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp202;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp202 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S176\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp202, "return of NetrServerReqChallenge, state S176");
                this.Manager.Comment("reaching state \'S245\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp203;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp203 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S314\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp203, "return of NetrServerAuthenticate3, state S314");
                this.Manager.Comment("reaching state \'S383\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp204;
                this.Manager.Comment("executing step \'call NetrLogonSamLogoff(NonDcServer,Client,Valid,False,NetlogonIn" +
                        "teractiveInformation,Valid)\'");
                temp204 = this.INrpcServerAdapterInstance.NetrLogonSamLogoff(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, ((Microsoft.Protocols.TestSuites.Nrpc.AuthenticatorType)(1)), false, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)));
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R1533");
                this.Manager.Checkpoint("MS-NRPC_R1258");
                this.Manager.Comment("reaching state \'S452\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogoff/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp204, "return of NetrLogonSamLogoff, state S452");
                this.Manager.Comment("reaching state \'S490\'");
                goto label18;
            }
            if ((temp208 == 2)) {
                this.Manager.Comment("reaching state \'S108\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp205;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp205 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S177\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp205, "return of NetrServerReqChallenge, state S177");
                this.Manager.Comment("reaching state \'S246\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp206;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp206 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S315\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp206, "return of NetrServerAuthenticate3, state S315");
                this.Manager.Comment("reaching state \'S384\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp207;
                this.Manager.Comment("executing step \'call NetrLogonSamLogoff(NonDcServer,Client,Valid,False,NetlogonIn" +
                        "teractiveInformation,Valid)\'");
                temp207 = this.INrpcServerAdapterInstance.NetrLogonSamLogoff(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, ((Microsoft.Protocols.TestSuites.Nrpc.AuthenticatorType)(1)), false, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)));
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R1533");
                this.Manager.Checkpoint("MS-NRPC_R1258");
                this.Manager.Comment("reaching state \'S453\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogoff/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp207, "return of NetrLogonSamLogoff, state S453");
                this.Manager.Comment("reaching state \'S491\'");
                goto label18;
            }
            throw new InvalidOperationException("never reached");
        label18:
;
            this.Manager.EndTest();
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogoffS40GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S41");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogoffS40GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S41");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogoffS40GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S41");
        }
        #endregion
        
        #region Test Starting in S42
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_PassThroughAuthenticate_NetrLogonSamLogoffS42() {
            this.Manager.BeginTest("Test_PassThroughAuthenticate_NetrLogonSamLogoffS42");
            this.Manager.Comment("reaching state \'S42\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp209;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp209);
            this.Manager.AddReturn(GetPlatformInfo, null, temp209);
            this.Manager.Comment("reaching state \'S43\'");
            int temp219 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogoff.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogoffS42GetPlatformChecker)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogoff.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogoffS42GetPlatformChecker1)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogoff.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogoffS42GetPlatformChecker2)));
            if ((temp219 == 0)) {
                this.Manager.Comment("reaching state \'S109\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp210;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp210 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S178\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp210, "return of NetrServerReqChallenge, state S178");
                this.Manager.Comment("reaching state \'S247\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp211;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp211 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S316\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp211, "return of NetrServerAuthenticate3, state S316");
                this.Manager.Comment("reaching state \'S385\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp212;
                this.Manager.Comment("executing step \'call NetrLogonSamLogoff(PrimaryDc,Client,Valid,True,NetlogonInter" +
                        "activeInformation,Valid)\'");
                temp212 = this.INrpcServerAdapterInstance.NetrLogonSamLogoff(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, ((Microsoft.Protocols.TestSuites.Nrpc.AuthenticatorType)(1)), true, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)));
                this.Manager.Checkpoint("MS-NRPC_R1270");
                this.Manager.Checkpoint("MS-NRPC_R1258");
                this.Manager.Comment("reaching state \'S454\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogoff/STATUS_INVALID_PARAMETER\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_INVALID_PARAMETER, temp212, "return of NetrLogonSamLogoff, state S454");
                Test_PassThroughAuthenticate_NetrLogonSamLogoffS460();
                goto label19;
            }
            if ((temp219 == 1)) {
                this.Manager.Comment("reaching state \'S110\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp213;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp213 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S179\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp213, "return of NetrServerReqChallenge, state S179");
                this.Manager.Comment("reaching state \'S248\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp214;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp214 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S317\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp214, "return of NetrServerAuthenticate3, state S317");
                this.Manager.Comment("reaching state \'S386\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp215;
                this.Manager.Comment("executing step \'call NetrLogonSamLogoff(NonDcServer,Client,Valid,False,NetlogonIn" +
                        "teractiveInformation,Valid)\'");
                temp215 = this.INrpcServerAdapterInstance.NetrLogonSamLogoff(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, ((Microsoft.Protocols.TestSuites.Nrpc.AuthenticatorType)(1)), false, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)));
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R1533");
                this.Manager.Checkpoint("MS-NRPC_R1258");
                this.Manager.Comment("reaching state \'S455\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogoff/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp215, "return of NetrLogonSamLogoff, state S455");
                this.Manager.Comment("reaching state \'S492\'");
                goto label19;
            }
            if ((temp219 == 2)) {
                this.Manager.Comment("reaching state \'S111\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp216;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp216 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S180\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp216, "return of NetrServerReqChallenge, state S180");
                this.Manager.Comment("reaching state \'S249\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp217;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp217 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S318\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp217, "return of NetrServerAuthenticate3, state S318");
                this.Manager.Comment("reaching state \'S387\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp218;
                this.Manager.Comment("executing step \'call NetrLogonSamLogoff(NonDcServer,Client,Valid,False,NetlogonIn" +
                        "teractiveInformation,Valid)\'");
                temp218 = this.INrpcServerAdapterInstance.NetrLogonSamLogoff(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, ((Microsoft.Protocols.TestSuites.Nrpc.AuthenticatorType)(1)), false, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)));
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R1533");
                this.Manager.Checkpoint("MS-NRPC_R1258");
                this.Manager.Comment("reaching state \'S456\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogoff/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp218, "return of NetrLogonSamLogoff, state S456");
                this.Manager.Comment("reaching state \'S493\'");
                goto label19;
            }
            throw new InvalidOperationException("never reached");
        label19:
;
            this.Manager.EndTest();
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogoffS42GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S43");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogoffS42GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S43");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogoffS42GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S43");
        }
        #endregion
        
        #region Test Starting in S44
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_PassThroughAuthenticate_NetrLogonSamLogoffS44() {
            this.Manager.BeginTest("Test_PassThroughAuthenticate_NetrLogonSamLogoffS44");
            this.Manager.Comment("reaching state \'S44\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp220;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp220);
            this.Manager.AddReturn(GetPlatformInfo, null, temp220);
            this.Manager.Comment("reaching state \'S45\'");
            int temp230 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogoff.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogoffS44GetPlatformChecker)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogoff.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogoffS44GetPlatformChecker1)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogoff.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogoffS44GetPlatformChecker2)));
            if ((temp230 == 0)) {
                this.Manager.Comment("reaching state \'S112\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp221;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp221 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S181\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp221, "return of NetrServerReqChallenge, state S181");
                this.Manager.Comment("reaching state \'S250\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp222;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp222 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S319\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp222, "return of NetrServerAuthenticate3, state S319");
                this.Manager.Comment("reaching state \'S388\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp223;
                this.Manager.Comment("executing step \'call NetrLogonSamLogoff(PrimaryDc,Client,Invalid,False,NetlogonIn" +
                        "teractiveInformation,Valid)\'");
                temp223 = this.INrpcServerAdapterInstance.NetrLogonSamLogoff(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, Microsoft.Protocols.TestSuites.Nrpc.AuthenticatorType.Invalid, false, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)));
                this.Manager.Checkpoint("MS-NRPC_R1265");
                this.Manager.Checkpoint("MS-NRPC_R1258");
                this.Manager.Comment("reaching state \'S457\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogoff/STATUS_ACCESS_DENIED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp223, "return of NetrLogonSamLogoff, state S457");
                Test_PassThroughAuthenticate_NetrLogonSamLogoffS460();
                goto label20;
            }
            if ((temp230 == 1)) {
                this.Manager.Comment("reaching state \'S113\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp224;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp224 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S182\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp224, "return of NetrServerReqChallenge, state S182");
                this.Manager.Comment("reaching state \'S251\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp225;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp225 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S320\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp225, "return of NetrServerAuthenticate3, state S320");
                this.Manager.Comment("reaching state \'S389\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp226;
                this.Manager.Comment("executing step \'call NetrLogonSamLogoff(NonDcServer,Client,Valid,False,NetlogonIn" +
                        "teractiveInformation,Valid)\'");
                temp226 = this.INrpcServerAdapterInstance.NetrLogonSamLogoff(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, ((Microsoft.Protocols.TestSuites.Nrpc.AuthenticatorType)(1)), false, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)));
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R1533");
                this.Manager.Checkpoint("MS-NRPC_R1258");
                this.Manager.Comment("reaching state \'S458\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogoff/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp226, "return of NetrLogonSamLogoff, state S458");
                this.Manager.Comment("reaching state \'S494\'");
                goto label20;
            }
            if ((temp230 == 2)) {
                this.Manager.Comment("reaching state \'S114\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp227;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp227 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S183\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp227, "return of NetrServerReqChallenge, state S183");
                this.Manager.Comment("reaching state \'S252\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp228;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp228 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S321\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp228, "return of NetrServerAuthenticate3, state S321");
                this.Manager.Comment("reaching state \'S390\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp229;
                this.Manager.Comment("executing step \'call NetrLogonSamLogoff(NonDcServer,Client,Valid,False,NetlogonIn" +
                        "teractiveInformation,Valid)\'");
                temp229 = this.INrpcServerAdapterInstance.NetrLogonSamLogoff(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, ((Microsoft.Protocols.TestSuites.Nrpc.AuthenticatorType)(1)), false, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)));
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R1533");
                this.Manager.Checkpoint("MS-NRPC_R1258");
                this.Manager.Comment("reaching state \'S459\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogoff/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp229, "return of NetrLogonSamLogoff, state S459");
                this.Manager.Comment("reaching state \'S495\'");
                goto label20;
            }
            throw new InvalidOperationException("never reached");
        label20:
;
            this.Manager.EndTest();
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogoffS44GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S45");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogoffS44GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S45");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogoffS44GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S45");
        }
        #endregion
        
        #region Test Starting in S6
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_PassThroughAuthenticate_NetrLogonSamLogoffS6() {
            this.Manager.BeginTest("Test_PassThroughAuthenticate_NetrLogonSamLogoffS6");
            this.Manager.Comment("reaching state \'S6\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp231;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp231);
            this.Manager.AddReturn(GetPlatformInfo, null, temp231);
            this.Manager.Comment("reaching state \'S7\'");
            int temp241 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogoff.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogoffS6GetPlatformChecker)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogoff.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogoffS6GetPlatformChecker1)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogoff.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogoffS6GetPlatformChecker2)));
            if ((temp241 == 0)) {
                this.Manager.Comment("reaching state \'S55\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp232;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp232 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S124\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp232, "return of NetrServerReqChallenge, state S124");
                this.Manager.Comment("reaching state \'S193\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp233;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp233 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S262\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp233, "return of NetrServerAuthenticate3, state S262");
                this.Manager.Comment("reaching state \'S331\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp234;
                this.Manager.Comment("executing step \'call NetrLogonSamLogoff(PrimaryDc,Client,Valid,False,NetlogonInte" +
                        "ractiveInformation,Null)\'");
                temp234 = this.INrpcServerAdapterInstance.NetrLogonSamLogoff(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, ((Microsoft.Protocols.TestSuites.Nrpc.AuthenticatorType)(1)), false, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(0)));
                this.Manager.Checkpoint("MS-NRPC_R1260");
                this.Manager.Checkpoint("MS-NRPC_R1258");
                this.Manager.Comment("reaching state \'S400\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogoff/STATUS_INVALID_PARAMETER\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_INVALID_PARAMETER, temp234, "return of NetrLogonSamLogoff, state S400");
                Test_PassThroughAuthenticate_NetrLogonSamLogoffS462();
                goto label21;
            }
            if ((temp241 == 1)) {
                this.Manager.Comment("reaching state \'S56\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp235;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp235 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S125\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp235, "return of NetrServerReqChallenge, state S125");
                this.Manager.Comment("reaching state \'S194\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp236;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp236 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S263\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp236, "return of NetrServerAuthenticate3, state S263");
                this.Manager.Comment("reaching state \'S332\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp237;
                this.Manager.Comment("executing step \'call NetrLogonSamLogoff(PrimaryDc,Client,Valid,False,NetlogonInte" +
                        "ractiveInformation,Null)\'");
                temp237 = this.INrpcServerAdapterInstance.NetrLogonSamLogoff(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, ((Microsoft.Protocols.TestSuites.Nrpc.AuthenticatorType)(1)), false, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(0)));
                this.Manager.Checkpoint("MS-NRPC_R1260");
                this.Manager.Checkpoint("MS-NRPC_R1258");
                this.Manager.Comment("reaching state \'S401\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogoff/STATUS_INVALID_PARAMETER\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_INVALID_PARAMETER, temp237, "return of NetrLogonSamLogoff, state S401");
                Test_PassThroughAuthenticate_NetrLogonSamLogoffS461();
                goto label21;
            }
            if ((temp241 == 2)) {
                this.Manager.Comment("reaching state \'S57\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp238;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp238 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S126\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp238, "return of NetrServerReqChallenge, state S126");
                this.Manager.Comment("reaching state \'S195\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp239;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp239 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S264\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp239, "return of NetrServerAuthenticate3, state S264");
                this.Manager.Comment("reaching state \'S333\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp240;
                this.Manager.Comment("executing step \'call NetrLogonSamLogoff(NonDcServer,Client,Valid,False,NetlogonIn" +
                        "teractiveInformation,Valid)\'");
                temp240 = this.INrpcServerAdapterInstance.NetrLogonSamLogoff(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, ((Microsoft.Protocols.TestSuites.Nrpc.AuthenticatorType)(1)), false, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)));
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R1533");
                this.Manager.Checkpoint("MS-NRPC_R1258");
                this.Manager.Comment("reaching state \'S402\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogoff/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp240, "return of NetrLogonSamLogoff, state S402");
                this.Manager.Comment("reaching state \'S465\'");
                goto label21;
            }
            throw new InvalidOperationException("never reached");
        label21:
;
            this.Manager.EndTest();
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogoffS6GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S7");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogoffS6GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S7");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogoffS6GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S7");
        }
        #endregion
        
        #region Test Starting in S8
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_PassThroughAuthenticate_NetrLogonSamLogoffS8() {
            this.Manager.BeginTest("Test_PassThroughAuthenticate_NetrLogonSamLogoffS8");
            this.Manager.Comment("reaching state \'S8\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp242;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp242);
            this.Manager.AddReturn(GetPlatformInfo, null, temp242);
            this.Manager.Comment("reaching state \'S9\'");
            int temp252 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogoff.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogoffS8GetPlatformChecker)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogoff.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogoffS8GetPlatformChecker1)), new ExpectedReturn(Test_PassThroughAuthenticate_NetrLogonSamLogoff.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_PassThroughAuthenticate_NetrLogonSamLogoffS8GetPlatformChecker2)));
            if ((temp252 == 0)) {
                this.Manager.Comment("reaching state \'S58\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp243;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp243 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S127\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp243, "return of NetrServerReqChallenge, state S127");
                this.Manager.Comment("reaching state \'S196\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp244;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp244 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S265\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp244, "return of NetrServerAuthenticate3, state S265");
                this.Manager.Comment("reaching state \'S334\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp245;
                this.Manager.Comment("executing step \'call NetrLogonSamLogoff(PrimaryDc,Client,Valid,False,NetlogonInte" +
                        "ractiveInformation,Valid)\'");
                temp245 = this.INrpcServerAdapterInstance.NetrLogonSamLogoff(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, ((Microsoft.Protocols.TestSuites.Nrpc.AuthenticatorType)(1)), false, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)));
                this.Manager.Checkpoint("MS-NRPC_R1257");
                this.Manager.Comment("reaching state \'S403\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogoff/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp245, "return of NetrLogonSamLogoff, state S403");
                Test_PassThroughAuthenticate_NetrLogonSamLogoffS462();
                goto label22;
            }
            if ((temp252 == 1)) {
                this.Manager.Comment("reaching state \'S59\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp246;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp246 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S128\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp246, "return of NetrServerReqChallenge, state S128");
                this.Manager.Comment("reaching state \'S197\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp247;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp247 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S266\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp247, "return of NetrServerAuthenticate3, state S266");
                this.Manager.Comment("reaching state \'S335\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp248;
                this.Manager.Comment("executing step \'call NetrLogonSamLogoff(PrimaryDc,Client,Valid,False,NetlogonInte" +
                        "ractiveInformation,Valid)\'");
                temp248 = this.INrpcServerAdapterInstance.NetrLogonSamLogoff(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, ((Microsoft.Protocols.TestSuites.Nrpc.AuthenticatorType)(1)), false, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)));
                this.Manager.Checkpoint("MS-NRPC_R1257");
                this.Manager.Comment("reaching state \'S404\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogoff/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp248, "return of NetrLogonSamLogoff, state S404");
                Test_PassThroughAuthenticate_NetrLogonSamLogoffS461();
                goto label22;
            }
            if ((temp252 == 2)) {
                this.Manager.Comment("reaching state \'S60\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp249;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp249 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S129\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp249, "return of NetrServerReqChallenge, state S129");
                this.Manager.Comment("reaching state \'S198\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp250;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16644)\'");
                temp250 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16644u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S267\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp250, "return of NetrServerAuthenticate3, state S267");
                this.Manager.Comment("reaching state \'S336\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp251;
                this.Manager.Comment("executing step \'call NetrLogonSamLogoff(NonDcServer,Client,Valid,False,NetlogonIn" +
                        "teractiveInformation,Valid)\'");
                temp251 = this.INrpcServerAdapterInstance.NetrLogonSamLogoff(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, ((Microsoft.Protocols.TestSuites.Nrpc.AuthenticatorType)(1)), false, ((Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_LOGON_INFO_CLASS)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.LogonInformationType)(1)));
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R1533");
                this.Manager.Checkpoint("MS-NRPC_R1258");
                this.Manager.Comment("reaching state \'S405\'");
                this.Manager.Comment("checking step \'return NetrLogonSamLogoff/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp251, "return of NetrLogonSamLogoff, state S405");
                this.Manager.Comment("reaching state \'S466\'");
                goto label22;
            }
            throw new InvalidOperationException("never reached");
        label22:
;
            this.Manager.EndTest();
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogoffS8GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S9");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogoffS8GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S9");
        }
        
        private void Test_PassThroughAuthenticate_NetrLogonSamLogoffS8GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S9");
        }
        #endregion
    }
}
