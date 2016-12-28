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
    public partial class Test_EstablishSecureChannel_NetrServerAuthenticate2 : PtfTestClassBase {
        
        public Test_EstablishSecureChannel_NetrServerAuthenticate2() {
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
        
        private Microsoft.Protocols.TestSuites.Nrpc.INrpcServerSutControlAdapter INrpcServerSutControlAdapterInstance;
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
            this.INrpcServerSutControlAdapterInstance = ((Microsoft.Protocols.TestSuites.Nrpc.INrpcServerSutControlAdapter)(this.Manager.GetAdapter(typeof(Microsoft.Protocols.TestSuites.Nrpc.INrpcServerSutControlAdapter))));
        }
        
        protected override void TestCleanup() {
            base.TestCleanup();
            this.CleanupTestManager();
        }
        #endregion
        
        #region Test Starting in S0
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_EstablishSecureChannel_NetrServerAuthenticate2S0() {
            this.Manager.BeginTest("Test_EstablishSecureChannel_NetrServerAuthenticate2S0");
            this.Manager.Comment("reaching state \'S0\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp0;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp0);
            this.Manager.AddReturn(GetPlatformInfo, null, temp0);
            this.Manager.Comment("reaching state \'S1\'");
            int temp7 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate2.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate2S0GetPlatformChecker)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate2.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate2S0GetPlatformChecker1)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate2.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate2S0GetPlatformChecker2)));
            if ((temp7 == 0)) {
                this.Manager.Comment("reaching state \'S112\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S280\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S448\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp1;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp1 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S616\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp1, "return of NetrServerReqChallenge, state S616");
                this.Manager.Comment("reaching state \'S784\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp2;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate2(PrimaryDc,InvalidAccount,Workstation" +
                        "SecureChannel,Client,True,16384)\'");
                temp2 = this.INrpcServerAdapterInstance.NetrServerAuthenticate2(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16384u);
                this.Manager.Checkpoint("MS-NRPC_R103489");
                this.Manager.Checkpoint("MS-NRPC_R103479");
                this.Manager.Comment("reaching state \'S952\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate2/STATUS_NO_TRUST_SAM_ACCOUNT\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NO_TRUST_SAM_ACCOUNT, temp2, "return of NetrServerAuthenticate2, state S952");
                this.Manager.Comment("reaching state \'S1037\'");
                goto label0;
            }
            if ((temp7 == 1)) {
                this.Manager.Comment("reaching state \'S113\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S281\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S449\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp3;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp3 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S617\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp3, "return of NetrServerReqChallenge, state S617");
                this.Manager.Comment("reaching state \'S785\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp4;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate2(PrimaryDc,InvalidAccount,Workstation" +
                        "SecureChannel,Client,True,16384)\'");
                temp4 = this.INrpcServerAdapterInstance.NetrServerAuthenticate2(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16384u);
                this.Manager.Checkpoint("MS-NRPC_R103489");
                this.Manager.Checkpoint("MS-NRPC_R103479");
                this.Manager.Comment("reaching state \'S953\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate2/STATUS_NO_TRUST_SAM_ACCOUNT\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NO_TRUST_SAM_ACCOUNT, temp4, "return of NetrServerAuthenticate2, state S953");
                this.Manager.Comment("reaching state \'S1038\'");
                goto label0;
            }
            if ((temp7 == 2)) {
                this.Manager.Comment("reaching state \'S114\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S282\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S450\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp5;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp5 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S618\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp5, "return of NetrServerReqChallenge, state S618");
                this.Manager.Comment("reaching state \'S786\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp6;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate2(PrimaryDc,InvalidAccount,Workstation" +
                        "SecureChannel,Client,True,16384)\'");
                temp6 = this.INrpcServerAdapterInstance.NetrServerAuthenticate2(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16384u);
                this.Manager.Checkpoint("MS-NRPC_R103489");
                this.Manager.Checkpoint("MS-NRPC_R103479");
                this.Manager.Comment("reaching state \'S954\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate2/STATUS_NO_TRUST_SAM_ACCOUNT\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NO_TRUST_SAM_ACCOUNT, temp6, "return of NetrServerAuthenticate2, state S954");
                this.Manager.Comment("reaching state \'S1039\'");
                goto label0;
            }
            throw new InvalidOperationException("never reached");
        label0:
;
            this.Manager.EndTest();
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate2S0GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S1");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate2S0GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S1");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate2S0GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S1");
        }
        #endregion
        
        #region Test Starting in S10
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_EstablishSecureChannel_NetrServerAuthenticate2S10() {
            this.Manager.BeginTest("Test_EstablishSecureChannel_NetrServerAuthenticate2S10");
            this.Manager.Comment("reaching state \'S10\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp8;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp8);
            this.Manager.AddReturn(GetPlatformInfo, null, temp8);
            this.Manager.Comment("reaching state \'S11\'");
            int temp14 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate2.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate2S10GetPlatformChecker)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate2.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate2S10GetPlatformChecker1)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate2.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate2S10GetPlatformChecker2)));
            if ((temp14 == 0)) {
                this.Manager.Comment("reaching state \'S127\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S295\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S463\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp9;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp9 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S631\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp9, "return of NetrServerReqChallenge, state S631");
                this.Manager.Comment("reaching state \'S799\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp10;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate2(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,8)\'");
                temp10 = this.INrpcServerAdapterInstance.NetrServerAuthenticate2(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 8u);
                this.Manager.Checkpoint("MS-NRPC_R103479");
                this.Manager.Comment("reaching state \'S963\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate2/STATUS_DOWNGRADE_DETECTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_DOWNGRADE_DETECTED, temp10, "return of NetrServerAuthenticate2, state S963");
                this.Manager.Comment("reaching state \'S1048\'");
                goto label1;
            }
            if ((temp14 == 1)) {
                this.Manager.Comment("reaching state \'S128\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S296\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S464\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp11;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp11 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S632\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp11, "return of NetrServerReqChallenge, state S632");
                this.Manager.Comment("reaching state \'S800\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp12;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate2(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,8)\'");
                temp12 = this.INrpcServerAdapterInstance.NetrServerAuthenticate2(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 8u);
                this.Manager.Checkpoint("MS-NRPC_R103479");
                this.Manager.Comment("reaching state \'S964\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate2/STATUS_DOWNGRADE_DETECTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_DOWNGRADE_DETECTED, temp12, "return of NetrServerAuthenticate2, state S964");
                this.Manager.Comment("reaching state \'S1049\'");
                goto label1;
            }
            if ((temp14 == 2)) {
                this.Manager.Comment("reaching state \'S129\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S297\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S465\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp13;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(NonDcServer,Client)\'");
                temp13 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103509");
                this.Manager.Checkpoint("MS-NRPC_R103341");
                this.Manager.Comment("reaching state \'S633\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp13, "return of NetrServerReqChallenge, state S633");
                this.Manager.Comment("reaching state \'S801\'");
                goto label1;
            }
            throw new InvalidOperationException("never reached");
        label1:
;
            this.Manager.EndTest();
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate2S10GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S11");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate2S10GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S11");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate2S10GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S11");
        }
        #endregion
        
        #region Test Starting in S100
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_EstablishSecureChannel_NetrServerAuthenticate2S100() {
            this.Manager.BeginTest("Test_EstablishSecureChannel_NetrServerAuthenticate2S100");
            this.Manager.Comment("reaching state \'S100\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp15;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp15);
            this.Manager.AddReturn(GetPlatformInfo, null, temp15);
            this.Manager.Comment("reaching state \'S101\'");
            int temp20 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate2.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate2S100GetPlatformChecker)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate2.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate2S100GetPlatformChecker1)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate2.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate2S100GetPlatformChecker2)));
            if ((temp20 == 0)) {
                this.Manager.Comment("reaching state \'S262\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S430\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S598\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp16;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp16 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S766\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp16, "return of NetrServerReqChallenge, state S766");
                this.Manager.Comment("reaching state \'S934\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp17;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate2(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,2097152)\'");
                temp17 = this.INrpcServerAdapterInstance.NetrServerAuthenticate2(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 2097152u);
                this.Manager.Checkpoint("MS-NRPC_R103479");
                this.Manager.Comment("reaching state \'S1031\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate2/STATUS_DOWNGRADE_DETECTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_DOWNGRADE_DETECTED, temp17, "return of NetrServerAuthenticate2, state S1031");
                this.Manager.Comment("reaching state \'S1116\'");
                goto label2;
            }
            if ((temp20 == 1)) {
                this.Manager.Comment("reaching state \'S263\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S431\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S599\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp18;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(NonDcServer,Client)\'");
                temp18 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103509");
                this.Manager.Checkpoint("MS-NRPC_R103341");
                this.Manager.Comment("reaching state \'S767\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp18, "return of NetrServerReqChallenge, state S767");
                this.Manager.Comment("reaching state \'S935\'");
                goto label2;
            }
            if ((temp20 == 2)) {
                this.Manager.Comment("reaching state \'S264\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S432\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S600\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp19;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(NonDcServer,Client)\'");
                temp19 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103509");
                this.Manager.Checkpoint("MS-NRPC_R103341");
                this.Manager.Comment("reaching state \'S768\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp19, "return of NetrServerReqChallenge, state S768");
                this.Manager.Comment("reaching state \'S936\'");
                goto label2;
            }
            throw new InvalidOperationException("never reached");
        label2:
;
            this.Manager.EndTest();
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate2S100GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S101");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate2S100GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S101");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate2S100GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S101");
        }
        #endregion
        
        #region Test Starting in S102
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_EstablishSecureChannel_NetrServerAuthenticate2S102() {
            this.Manager.BeginTest("Test_EstablishSecureChannel_NetrServerAuthenticate2S102");
            this.Manager.Comment("reaching state \'S102\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp21;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp21);
            this.Manager.AddReturn(GetPlatformInfo, null, temp21);
            this.Manager.Comment("reaching state \'S103\'");
            int temp26 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate2.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate2S102GetPlatformChecker)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate2.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate2S102GetPlatformChecker1)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate2.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate2S102GetPlatformChecker2)));
            if ((temp26 == 0)) {
                this.Manager.Comment("reaching state \'S265\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S433\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S601\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp22;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp22 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S769\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp22, "return of NetrServerReqChallenge, state S769");
                this.Manager.Comment("reaching state \'S937\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp23;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate2(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16777216)\'");
                temp23 = this.INrpcServerAdapterInstance.NetrServerAuthenticate2(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16777216u);
                this.Manager.Checkpoint("MS-NRPC_R103478");
                this.Manager.Comment("reaching state \'S1032\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate2/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp23, "return of NetrServerAuthenticate2, state S1032");
                this.Manager.Comment("reaching state \'S1117\'");
                goto label3;
            }
            if ((temp26 == 1)) {
                this.Manager.Comment("reaching state \'S266\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S434\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S602\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp24;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(NonDcServer,Client)\'");
                temp24 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103509");
                this.Manager.Checkpoint("MS-NRPC_R103341");
                this.Manager.Comment("reaching state \'S770\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp24, "return of NetrServerReqChallenge, state S770");
                this.Manager.Comment("reaching state \'S938\'");
                goto label3;
            }
            if ((temp26 == 2)) {
                this.Manager.Comment("reaching state \'S267\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S435\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S603\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp25;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(NonDcServer,Client)\'");
                temp25 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103509");
                this.Manager.Checkpoint("MS-NRPC_R103341");
                this.Manager.Comment("reaching state \'S771\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp25, "return of NetrServerReqChallenge, state S771");
                this.Manager.Comment("reaching state \'S939\'");
                goto label3;
            }
            throw new InvalidOperationException("never reached");
        label3:
;
            this.Manager.EndTest();
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate2S102GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S103");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate2S102GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S103");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate2S102GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S103");
        }
        #endregion
        
        #region Test Starting in S104
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_EstablishSecureChannel_NetrServerAuthenticate2S104() {
            this.Manager.BeginTest("Test_EstablishSecureChannel_NetrServerAuthenticate2S104");
            this.Manager.Comment("reaching state \'S104\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp27;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp27);
            this.Manager.AddReturn(GetPlatformInfo, null, temp27);
            this.Manager.Comment("reaching state \'S105\'");
            int temp32 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate2.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate2S104GetPlatformChecker)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate2.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate2S104GetPlatformChecker1)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate2.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate2S104GetPlatformChecker2)));
            if ((temp32 == 0)) {
                this.Manager.Comment("reaching state \'S268\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S436\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S604\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp28;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp28 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S772\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp28, "return of NetrServerReqChallenge, state S772");
                this.Manager.Comment("reaching state \'S940\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp29;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate2(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,536870912)\'");
                temp29 = this.INrpcServerAdapterInstance.NetrServerAuthenticate2(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 536870912u);
                this.Manager.Checkpoint("MS-NRPC_R103479");
                this.Manager.Comment("reaching state \'S1033\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate2/STATUS_DOWNGRADE_DETECTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_DOWNGRADE_DETECTED, temp29, "return of NetrServerAuthenticate2, state S1033");
                this.Manager.Comment("reaching state \'S1118\'");
                goto label4;
            }
            if ((temp32 == 1)) {
                this.Manager.Comment("reaching state \'S269\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S437\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S605\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp30;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(NonDcServer,Client)\'");
                temp30 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103509");
                this.Manager.Checkpoint("MS-NRPC_R103341");
                this.Manager.Comment("reaching state \'S773\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp30, "return of NetrServerReqChallenge, state S773");
                this.Manager.Comment("reaching state \'S941\'");
                goto label4;
            }
            if ((temp32 == 2)) {
                this.Manager.Comment("reaching state \'S270\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S438\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S606\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp31;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(NonDcServer,Client)\'");
                temp31 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103509");
                this.Manager.Checkpoint("MS-NRPC_R103341");
                this.Manager.Comment("reaching state \'S774\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp31, "return of NetrServerReqChallenge, state S774");
                this.Manager.Comment("reaching state \'S942\'");
                goto label4;
            }
            throw new InvalidOperationException("never reached");
        label4:
;
            this.Manager.EndTest();
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate2S104GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S105");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate2S104GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S105");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate2S104GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S105");
        }
        #endregion
        
        #region Test Starting in S106
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_EstablishSecureChannel_NetrServerAuthenticate2S106() {
            this.Manager.BeginTest("Test_EstablishSecureChannel_NetrServerAuthenticate2S106");
            this.Manager.Comment("reaching state \'S106\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp33;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp33);
            this.Manager.AddReturn(GetPlatformInfo, null, temp33);
            this.Manager.Comment("reaching state \'S107\'");
            int temp38 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate2.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate2S106GetPlatformChecker)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate2.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate2S106GetPlatformChecker1)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate2.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate2S106GetPlatformChecker2)));
            if ((temp38 == 0)) {
                this.Manager.Comment("reaching state \'S271\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S439\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S607\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp34;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp34 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S775\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp34, "return of NetrServerReqChallenge, state S775");
                this.Manager.Comment("reaching state \'S943\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp35;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate2(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16384)\'");
                temp35 = this.INrpcServerAdapterInstance.NetrServerAuthenticate2(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16384u);
                this.Manager.Checkpoint("MS-NRPC_R103478");
                this.Manager.Comment("reaching state \'S1034\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate2/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp35, "return of NetrServerAuthenticate2, state S1034");
                this.Manager.Comment("reaching state \'S1119\'");
                goto label5;
            }
            if ((temp38 == 1)) {
                this.Manager.Comment("reaching state \'S272\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S440\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S608\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp36;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(NonDcServer,Client)\'");
                temp36 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103509");
                this.Manager.Checkpoint("MS-NRPC_R103341");
                this.Manager.Comment("reaching state \'S776\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp36, "return of NetrServerReqChallenge, state S776");
                this.Manager.Comment("reaching state \'S944\'");
                goto label5;
            }
            if ((temp38 == 2)) {
                this.Manager.Comment("reaching state \'S273\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S441\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S609\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp37;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(NonDcServer,Client)\'");
                temp37 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103509");
                this.Manager.Checkpoint("MS-NRPC_R103341");
                this.Manager.Comment("reaching state \'S777\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp37, "return of NetrServerReqChallenge, state S777");
                this.Manager.Comment("reaching state \'S945\'");
                goto label5;
            }
            throw new InvalidOperationException("never reached");
        label5:
;
            this.Manager.EndTest();
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate2S106GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S107");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate2S106GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S107");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate2S106GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S107");
        }
        #endregion
        
        #region Test Starting in S108
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_EstablishSecureChannel_NetrServerAuthenticate2S108() {
            this.Manager.BeginTest("Test_EstablishSecureChannel_NetrServerAuthenticate2S108");
            this.Manager.Comment("reaching state \'S108\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp39;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp39);
            this.Manager.AddReturn(GetPlatformInfo, null, temp39);
            this.Manager.Comment("reaching state \'S109\'");
            int temp44 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate2.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate2S108GetPlatformChecker)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate2.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate2S108GetPlatformChecker1)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate2.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate2S108GetPlatformChecker2)));
            if ((temp44 == 0)) {
                this.Manager.Comment("reaching state \'S274\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S442\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S610\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp40;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp40 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S778\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp40, "return of NetrServerReqChallenge, state S778");
                this.Manager.Comment("reaching state \'S946\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp41;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate2(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,False,16384)\'");
                temp41 = this.INrpcServerAdapterInstance.NetrServerAuthenticate2(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, false, 16384u);
                this.Manager.Checkpoint("MS-NRPC_R103492");
                this.Manager.Checkpoint("MS-NRPC_R103479");
                this.Manager.Comment("reaching state \'S1035\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate2/STATUS_ACCESS_DENIED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp41, "return of NetrServerAuthenticate2, state S1035");
                this.Manager.Comment("reaching state \'S1120\'");
                goto label6;
            }
            if ((temp44 == 1)) {
                this.Manager.Comment("reaching state \'S275\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S443\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S611\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp42;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(NonDcServer,Client)\'");
                temp42 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103509");
                this.Manager.Checkpoint("MS-NRPC_R103341");
                this.Manager.Comment("reaching state \'S779\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp42, "return of NetrServerReqChallenge, state S779");
                this.Manager.Comment("reaching state \'S947\'");
                goto label6;
            }
            if ((temp44 == 2)) {
                this.Manager.Comment("reaching state \'S276\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S444\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S612\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp43;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(NonDcServer,Client)\'");
                temp43 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103509");
                this.Manager.Checkpoint("MS-NRPC_R103341");
                this.Manager.Comment("reaching state \'S780\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp43, "return of NetrServerReqChallenge, state S780");
                this.Manager.Comment("reaching state \'S948\'");
                goto label6;
            }
            throw new InvalidOperationException("never reached");
        label6:
;
            this.Manager.EndTest();
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate2S108GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S109");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate2S108GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S109");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate2S108GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S109");
        }
        #endregion
        
        #region Test Starting in S110
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_EstablishSecureChannel_NetrServerAuthenticate2S110() {
            this.Manager.BeginTest("Test_EstablishSecureChannel_NetrServerAuthenticate2S110");
            this.Manager.Comment("reaching state \'S110\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp45;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp45);
            this.Manager.AddReturn(GetPlatformInfo, null, temp45);
            this.Manager.Comment("reaching state \'S111\'");
            int temp50 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate2.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate2S110GetPlatformChecker)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate2.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate2S110GetPlatformChecker1)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate2.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate2S110GetPlatformChecker2)));
            if ((temp50 == 0)) {
                this.Manager.Comment("reaching state \'S277\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S445\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S613\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp46;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(NonDcServer,Client)\'");
                temp46 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103509");
                this.Manager.Checkpoint("MS-NRPC_R103341");
                this.Manager.Comment("reaching state \'S781\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp46, "return of NetrServerReqChallenge, state S781");
                this.Manager.Comment("reaching state \'S949\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp47;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate2(NonDcServer,DomainMemberComputerAcco" +
                        "unt,WorkstationSecureChannel,Client,True,16384)\'");
                temp47 = this.INrpcServerAdapterInstance.NetrServerAuthenticate2(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16384u);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R104904");
                this.Manager.Checkpoint("MS-NRPC_R103479");
                this.Manager.Comment("reaching state \'S1036\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate2/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp47, "return of NetrServerAuthenticate2, state S1036");
                this.Manager.Comment("reaching state \'S1121\'");
                goto label7;
            }
            if ((temp50 == 1)) {
                this.Manager.Comment("reaching state \'S278\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S446\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S614\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp48;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(NonDcServer,Client)\'");
                temp48 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103509");
                this.Manager.Checkpoint("MS-NRPC_R103341");
                this.Manager.Comment("reaching state \'S782\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp48, "return of NetrServerReqChallenge, state S782");
                this.Manager.Comment("reaching state \'S950\'");
                goto label7;
            }
            if ((temp50 == 2)) {
                this.Manager.Comment("reaching state \'S279\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S447\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S615\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp49;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(NonDcServer,Client)\'");
                temp49 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103509");
                this.Manager.Checkpoint("MS-NRPC_R103341");
                this.Manager.Comment("reaching state \'S783\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp49, "return of NetrServerReqChallenge, state S783");
                this.Manager.Comment("reaching state \'S951\'");
                goto label7;
            }
            throw new InvalidOperationException("never reached");
        label7:
;
            this.Manager.EndTest();
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate2S110GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S111");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate2S110GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S111");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate2S110GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S111");
        }
        #endregion
        
        #region Test Starting in S12
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_EstablishSecureChannel_NetrServerAuthenticate2S12() {
            this.Manager.BeginTest("Test_EstablishSecureChannel_NetrServerAuthenticate2S12");
            this.Manager.Comment("reaching state \'S12\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp51;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp51);
            this.Manager.AddReturn(GetPlatformInfo, null, temp51);
            this.Manager.Comment("reaching state \'S13\'");
            int temp57 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate2.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate2S12GetPlatformChecker)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate2.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate2S12GetPlatformChecker1)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate2.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate2S12GetPlatformChecker2)));
            if ((temp57 == 0)) {
                this.Manager.Comment("reaching state \'S130\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S298\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S466\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp52;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp52 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S634\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp52, "return of NetrServerReqChallenge, state S634");
                this.Manager.Comment("reaching state \'S802\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp53;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate2(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16)\'");
                temp53 = this.INrpcServerAdapterInstance.NetrServerAuthenticate2(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16u);
                this.Manager.Checkpoint("MS-NRPC_R103479");
                this.Manager.Comment("reaching state \'S965\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate2/STATUS_DOWNGRADE_DETECTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_DOWNGRADE_DETECTED, temp53, "return of NetrServerAuthenticate2, state S965");
                this.Manager.Comment("reaching state \'S1050\'");
                goto label8;
            }
            if ((temp57 == 1)) {
                this.Manager.Comment("reaching state \'S131\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S299\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S467\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp54;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp54 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S635\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp54, "return of NetrServerReqChallenge, state S635");
                this.Manager.Comment("reaching state \'S803\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp55;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate2(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16)\'");
                temp55 = this.INrpcServerAdapterInstance.NetrServerAuthenticate2(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16u);
                this.Manager.Checkpoint("MS-NRPC_R103479");
                this.Manager.Comment("reaching state \'S966\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate2/STATUS_DOWNGRADE_DETECTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_DOWNGRADE_DETECTED, temp55, "return of NetrServerAuthenticate2, state S966");
                this.Manager.Comment("reaching state \'S1051\'");
                goto label8;
            }
            if ((temp57 == 2)) {
                this.Manager.Comment("reaching state \'S132\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S300\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S468\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp56;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(NonDcServer,Client)\'");
                temp56 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103509");
                this.Manager.Checkpoint("MS-NRPC_R103341");
                this.Manager.Comment("reaching state \'S636\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp56, "return of NetrServerReqChallenge, state S636");
                this.Manager.Comment("reaching state \'S804\'");
                goto label8;
            }
            throw new InvalidOperationException("never reached");
        label8:
;
            this.Manager.EndTest();
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate2S12GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S13");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate2S12GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S13");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate2S12GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S13");
        }
        #endregion
        
        #region Test Starting in S14
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_EstablishSecureChannel_NetrServerAuthenticate2S14() {
            this.Manager.BeginTest("Test_EstablishSecureChannel_NetrServerAuthenticate2S14");
            this.Manager.Comment("reaching state \'S14\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp58;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp58);
            this.Manager.AddReturn(GetPlatformInfo, null, temp58);
            this.Manager.Comment("reaching state \'S15\'");
            int temp64 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate2.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate2S14GetPlatformChecker)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate2.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate2S14GetPlatformChecker1)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate2.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate2S14GetPlatformChecker2)));
            if ((temp64 == 0)) {
                this.Manager.Comment("reaching state \'S133\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S301\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S469\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp59;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp59 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S637\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp59, "return of NetrServerReqChallenge, state S637");
                this.Manager.Comment("reaching state \'S805\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp60;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate2(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,32)\'");
                temp60 = this.INrpcServerAdapterInstance.NetrServerAuthenticate2(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 32u);
                this.Manager.Checkpoint("MS-NRPC_R103479");
                this.Manager.Comment("reaching state \'S967\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate2/STATUS_DOWNGRADE_DETECTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_DOWNGRADE_DETECTED, temp60, "return of NetrServerAuthenticate2, state S967");
                this.Manager.Comment("reaching state \'S1052\'");
                goto label9;
            }
            if ((temp64 == 1)) {
                this.Manager.Comment("reaching state \'S134\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S302\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S470\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp61;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp61 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S638\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp61, "return of NetrServerReqChallenge, state S638");
                this.Manager.Comment("reaching state \'S806\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp62;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate2(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,32)\'");
                temp62 = this.INrpcServerAdapterInstance.NetrServerAuthenticate2(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 32u);
                this.Manager.Checkpoint("MS-NRPC_R103479");
                this.Manager.Comment("reaching state \'S968\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate2/STATUS_DOWNGRADE_DETECTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_DOWNGRADE_DETECTED, temp62, "return of NetrServerAuthenticate2, state S968");
                this.Manager.Comment("reaching state \'S1053\'");
                goto label9;
            }
            if ((temp64 == 2)) {
                this.Manager.Comment("reaching state \'S135\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S303\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S471\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp63;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(NonDcServer,Client)\'");
                temp63 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103509");
                this.Manager.Checkpoint("MS-NRPC_R103341");
                this.Manager.Comment("reaching state \'S639\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp63, "return of NetrServerReqChallenge, state S639");
                this.Manager.Comment("reaching state \'S807\'");
                goto label9;
            }
            throw new InvalidOperationException("never reached");
        label9:
;
            this.Manager.EndTest();
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate2S14GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S15");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate2S14GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S15");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate2S14GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S15");
        }
        #endregion
        
        #region Test Starting in S16
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_EstablishSecureChannel_NetrServerAuthenticate2S16() {
            this.Manager.BeginTest("Test_EstablishSecureChannel_NetrServerAuthenticate2S16");
            this.Manager.Comment("reaching state \'S16\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp65;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp65);
            this.Manager.AddReturn(GetPlatformInfo, null, temp65);
            this.Manager.Comment("reaching state \'S17\'");
            int temp71 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate2.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate2S16GetPlatformChecker)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate2.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate2S16GetPlatformChecker1)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate2.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate2S16GetPlatformChecker2)));
            if ((temp71 == 0)) {
                this.Manager.Comment("reaching state \'S136\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S304\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S472\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp66;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp66 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S640\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp66, "return of NetrServerReqChallenge, state S640");
                this.Manager.Comment("reaching state \'S808\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp67;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate2(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,128)\'");
                temp67 = this.INrpcServerAdapterInstance.NetrServerAuthenticate2(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 128u);
                this.Manager.Checkpoint("MS-NRPC_R103479");
                this.Manager.Comment("reaching state \'S969\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate2/STATUS_DOWNGRADE_DETECTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_DOWNGRADE_DETECTED, temp67, "return of NetrServerAuthenticate2, state S969");
                this.Manager.Comment("reaching state \'S1054\'");
                goto label10;
            }
            if ((temp71 == 1)) {
                this.Manager.Comment("reaching state \'S137\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S305\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S473\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp68;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp68 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S641\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp68, "return of NetrServerReqChallenge, state S641");
                this.Manager.Comment("reaching state \'S809\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp69;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate2(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,128)\'");
                temp69 = this.INrpcServerAdapterInstance.NetrServerAuthenticate2(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 128u);
                this.Manager.Checkpoint("MS-NRPC_R103479");
                this.Manager.Comment("reaching state \'S970\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate2/STATUS_DOWNGRADE_DETECTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_DOWNGRADE_DETECTED, temp69, "return of NetrServerAuthenticate2, state S970");
                this.Manager.Comment("reaching state \'S1055\'");
                goto label10;
            }
            if ((temp71 == 2)) {
                this.Manager.Comment("reaching state \'S138\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S306\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S474\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp70;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(NonDcServer,Client)\'");
                temp70 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103509");
                this.Manager.Checkpoint("MS-NRPC_R103341");
                this.Manager.Comment("reaching state \'S642\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp70, "return of NetrServerReqChallenge, state S642");
                this.Manager.Comment("reaching state \'S810\'");
                goto label10;
            }
            throw new InvalidOperationException("never reached");
        label10:
;
            this.Manager.EndTest();
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate2S16GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S17");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate2S16GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S17");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate2S16GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S17");
        }
        #endregion
        
        #region Test Starting in S18
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_EstablishSecureChannel_NetrServerAuthenticate2S18() {
            this.Manager.BeginTest("Test_EstablishSecureChannel_NetrServerAuthenticate2S18");
            this.Manager.Comment("reaching state \'S18\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp72;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp72);
            this.Manager.AddReturn(GetPlatformInfo, null, temp72);
            this.Manager.Comment("reaching state \'S19\'");
            int temp78 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate2.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate2S18GetPlatformChecker)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate2.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate2S18GetPlatformChecker1)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate2.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate2S18GetPlatformChecker2)));
            if ((temp78 == 0)) {
                this.Manager.Comment("reaching state \'S139\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S307\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S475\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp73;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp73 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S643\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp73, "return of NetrServerReqChallenge, state S643");
                this.Manager.Comment("reaching state \'S811\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp74;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate2(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,256)\'");
                temp74 = this.INrpcServerAdapterInstance.NetrServerAuthenticate2(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 256u);
                this.Manager.Checkpoint("MS-NRPC_R103479");
                this.Manager.Comment("reaching state \'S971\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate2/STATUS_DOWNGRADE_DETECTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_DOWNGRADE_DETECTED, temp74, "return of NetrServerAuthenticate2, state S971");
                this.Manager.Comment("reaching state \'S1056\'");
                goto label11;
            }
            if ((temp78 == 1)) {
                this.Manager.Comment("reaching state \'S140\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S308\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S476\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp75;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp75 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S644\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp75, "return of NetrServerReqChallenge, state S644");
                this.Manager.Comment("reaching state \'S812\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp76;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate2(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,256)\'");
                temp76 = this.INrpcServerAdapterInstance.NetrServerAuthenticate2(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 256u);
                this.Manager.Checkpoint("MS-NRPC_R103479");
                this.Manager.Comment("reaching state \'S972\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate2/STATUS_DOWNGRADE_DETECTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_DOWNGRADE_DETECTED, temp76, "return of NetrServerAuthenticate2, state S972");
                this.Manager.Comment("reaching state \'S1057\'");
                goto label11;
            }
            if ((temp78 == 2)) {
                this.Manager.Comment("reaching state \'S141\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S309\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S477\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp77;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(NonDcServer,Client)\'");
                temp77 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103509");
                this.Manager.Checkpoint("MS-NRPC_R103341");
                this.Manager.Comment("reaching state \'S645\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp77, "return of NetrServerReqChallenge, state S645");
                this.Manager.Comment("reaching state \'S813\'");
                goto label11;
            }
            throw new InvalidOperationException("never reached");
        label11:
;
            this.Manager.EndTest();
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate2S18GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S19");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate2S18GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S19");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate2S18GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S19");
        }
        #endregion
        
        #region Test Starting in S2
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_EstablishSecureChannel_NetrServerAuthenticate2S2() {
            this.Manager.BeginTest("Test_EstablishSecureChannel_NetrServerAuthenticate2S2");
            this.Manager.Comment("reaching state \'S2\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp79;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp79);
            this.Manager.AddReturn(GetPlatformInfo, null, temp79);
            this.Manager.Comment("reaching state \'S3\'");
            int temp85 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate2.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate2S2GetPlatformChecker)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate2.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate2S2GetPlatformChecker1)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate2.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate2S2GetPlatformChecker2)));
            if ((temp85 == 0)) {
                this.Manager.Comment("reaching state \'S115\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S283\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S451\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp80;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp80 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S619\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp80, "return of NetrServerReqChallenge, state S619");
                this.Manager.Comment("reaching state \'S787\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp81;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate2(NonDcServer,DomainMemberComputerAcco" +
                        "unt,WorkstationSecureChannel,Client,True,16384)\'");
                temp81 = this.INrpcServerAdapterInstance.NetrServerAuthenticate2(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16384u);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R104904");
                this.Manager.Checkpoint("MS-NRPC_R103479");
                this.Manager.Comment("reaching state \'S955\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate2/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp81, "return of NetrServerAuthenticate2, state S955");
                this.Manager.Comment("reaching state \'S1040\'");
                goto label12;
            }
            if ((temp85 == 1)) {
                this.Manager.Comment("reaching state \'S116\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S284\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S452\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp82;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp82 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S620\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp82, "return of NetrServerReqChallenge, state S620");
                this.Manager.Comment("reaching state \'S788\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp83;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate2(NonDcServer,DomainMemberComputerAcco" +
                        "unt,WorkstationSecureChannel,Client,True,16384)\'");
                temp83 = this.INrpcServerAdapterInstance.NetrServerAuthenticate2(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16384u);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R104904");
                this.Manager.Checkpoint("MS-NRPC_R103479");
                this.Manager.Comment("reaching state \'S956\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate2/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp83, "return of NetrServerAuthenticate2, state S956");
                this.Manager.Comment("reaching state \'S1041\'");
                goto label12;
            }
            if ((temp85 == 2)) {
                this.Manager.Comment("reaching state \'S117\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S285\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S453\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp84;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(NonDcServer,Client)\'");
                temp84 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103509");
                this.Manager.Checkpoint("MS-NRPC_R103341");
                this.Manager.Comment("reaching state \'S621\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp84, "return of NetrServerReqChallenge, state S621");
                this.Manager.Comment("reaching state \'S789\'");
                goto label12;
            }
            throw new InvalidOperationException("never reached");
        label12:
;
            this.Manager.EndTest();
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate2S2GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S3");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate2S2GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S3");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate2S2GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S3");
        }
        #endregion
        
        #region Test Starting in S20
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_EstablishSecureChannel_NetrServerAuthenticate2S20() {
            this.Manager.BeginTest("Test_EstablishSecureChannel_NetrServerAuthenticate2S20");
            this.Manager.Comment("reaching state \'S20\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp86;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp86);
            this.Manager.AddReturn(GetPlatformInfo, null, temp86);
            this.Manager.Comment("reaching state \'S21\'");
            int temp92 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate2.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate2S20GetPlatformChecker)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate2.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate2S20GetPlatformChecker1)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate2.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate2S20GetPlatformChecker2)));
            if ((temp92 == 0)) {
                this.Manager.Comment("reaching state \'S142\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S310\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S478\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp87;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp87 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S646\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp87, "return of NetrServerReqChallenge, state S646");
                this.Manager.Comment("reaching state \'S814\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp88;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate2(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,512)\'");
                temp88 = this.INrpcServerAdapterInstance.NetrServerAuthenticate2(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 512u);
                this.Manager.Checkpoint("MS-NRPC_R103479");
                this.Manager.Comment("reaching state \'S973\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate2/STATUS_DOWNGRADE_DETECTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_DOWNGRADE_DETECTED, temp88, "return of NetrServerAuthenticate2, state S973");
                this.Manager.Comment("reaching state \'S1058\'");
                goto label13;
            }
            if ((temp92 == 1)) {
                this.Manager.Comment("reaching state \'S143\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S311\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S479\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp89;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp89 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S647\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp89, "return of NetrServerReqChallenge, state S647");
                this.Manager.Comment("reaching state \'S815\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp90;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate2(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,512)\'");
                temp90 = this.INrpcServerAdapterInstance.NetrServerAuthenticate2(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 512u);
                this.Manager.Checkpoint("MS-NRPC_R103479");
                this.Manager.Comment("reaching state \'S974\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate2/STATUS_DOWNGRADE_DETECTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_DOWNGRADE_DETECTED, temp90, "return of NetrServerAuthenticate2, state S974");
                this.Manager.Comment("reaching state \'S1059\'");
                goto label13;
            }
            if ((temp92 == 2)) {
                this.Manager.Comment("reaching state \'S144\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S312\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S480\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp91;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(NonDcServer,Client)\'");
                temp91 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103509");
                this.Manager.Checkpoint("MS-NRPC_R103341");
                this.Manager.Comment("reaching state \'S648\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp91, "return of NetrServerReqChallenge, state S648");
                this.Manager.Comment("reaching state \'S816\'");
                goto label13;
            }
            throw new InvalidOperationException("never reached");
        label13:
;
            this.Manager.EndTest();
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate2S20GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S21");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate2S20GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S21");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate2S20GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S21");
        }
        #endregion
        
        #region Test Starting in S22
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_EstablishSecureChannel_NetrServerAuthenticate2S22() {
            this.Manager.BeginTest("Test_EstablishSecureChannel_NetrServerAuthenticate2S22");
            this.Manager.Comment("reaching state \'S22\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp93;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp93);
            this.Manager.AddReturn(GetPlatformInfo, null, temp93);
            this.Manager.Comment("reaching state \'S23\'");
            int temp99 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate2.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate2S22GetPlatformChecker)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate2.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate2S22GetPlatformChecker1)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate2.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate2S22GetPlatformChecker2)));
            if ((temp99 == 0)) {
                this.Manager.Comment("reaching state \'S145\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S313\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S481\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp94;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp94 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S649\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp94, "return of NetrServerReqChallenge, state S649");
                this.Manager.Comment("reaching state \'S817\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp95;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate2(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,1024)\'");
                temp95 = this.INrpcServerAdapterInstance.NetrServerAuthenticate2(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 1024u);
                this.Manager.Checkpoint("MS-NRPC_R103479");
                this.Manager.Comment("reaching state \'S975\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate2/STATUS_DOWNGRADE_DETECTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_DOWNGRADE_DETECTED, temp95, "return of NetrServerAuthenticate2, state S975");
                this.Manager.Comment("reaching state \'S1060\'");
                goto label14;
            }
            if ((temp99 == 1)) {
                this.Manager.Comment("reaching state \'S146\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S314\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S482\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp96;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp96 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S650\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp96, "return of NetrServerReqChallenge, state S650");
                this.Manager.Comment("reaching state \'S818\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp97;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate2(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,1024)\'");
                temp97 = this.INrpcServerAdapterInstance.NetrServerAuthenticate2(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 1024u);
                this.Manager.Checkpoint("MS-NRPC_R103479");
                this.Manager.Comment("reaching state \'S976\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate2/STATUS_DOWNGRADE_DETECTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_DOWNGRADE_DETECTED, temp97, "return of NetrServerAuthenticate2, state S976");
                this.Manager.Comment("reaching state \'S1061\'");
                goto label14;
            }
            if ((temp99 == 2)) {
                this.Manager.Comment("reaching state \'S147\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S315\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S483\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp98;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(NonDcServer,Client)\'");
                temp98 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103509");
                this.Manager.Checkpoint("MS-NRPC_R103341");
                this.Manager.Comment("reaching state \'S651\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp98, "return of NetrServerReqChallenge, state S651");
                this.Manager.Comment("reaching state \'S819\'");
                goto label14;
            }
            throw new InvalidOperationException("never reached");
        label14:
;
            this.Manager.EndTest();
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate2S22GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S23");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate2S22GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S23");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate2S22GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S23");
        }
        #endregion
        
        #region Test Starting in S24
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_EstablishSecureChannel_NetrServerAuthenticate2S24() {
            this.Manager.BeginTest("Test_EstablishSecureChannel_NetrServerAuthenticate2S24");
            this.Manager.Comment("reaching state \'S24\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp100;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp100);
            this.Manager.AddReturn(GetPlatformInfo, null, temp100);
            this.Manager.Comment("reaching state \'S25\'");
            int temp106 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate2.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate2S24GetPlatformChecker)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate2.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate2S24GetPlatformChecker1)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate2.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate2S24GetPlatformChecker2)));
            if ((temp106 == 0)) {
                this.Manager.Comment("reaching state \'S148\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S316\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S484\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp101;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp101 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S652\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp101, "return of NetrServerReqChallenge, state S652");
                this.Manager.Comment("reaching state \'S820\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp102;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate2(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,2048)\'");
                temp102 = this.INrpcServerAdapterInstance.NetrServerAuthenticate2(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 2048u);
                this.Manager.Checkpoint("MS-NRPC_R103479");
                this.Manager.Comment("reaching state \'S977\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate2/STATUS_DOWNGRADE_DETECTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_DOWNGRADE_DETECTED, temp102, "return of NetrServerAuthenticate2, state S977");
                this.Manager.Comment("reaching state \'S1062\'");
                goto label15;
            }
            if ((temp106 == 1)) {
                this.Manager.Comment("reaching state \'S149\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S317\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S485\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp103;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp103 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S653\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp103, "return of NetrServerReqChallenge, state S653");
                this.Manager.Comment("reaching state \'S821\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp104;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate2(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,2048)\'");
                temp104 = this.INrpcServerAdapterInstance.NetrServerAuthenticate2(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 2048u);
                this.Manager.Checkpoint("MS-NRPC_R103479");
                this.Manager.Comment("reaching state \'S978\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate2/STATUS_DOWNGRADE_DETECTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_DOWNGRADE_DETECTED, temp104, "return of NetrServerAuthenticate2, state S978");
                this.Manager.Comment("reaching state \'S1063\'");
                goto label15;
            }
            if ((temp106 == 2)) {
                this.Manager.Comment("reaching state \'S150\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S318\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S486\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp105;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(NonDcServer,Client)\'");
                temp105 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103509");
                this.Manager.Checkpoint("MS-NRPC_R103341");
                this.Manager.Comment("reaching state \'S654\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp105, "return of NetrServerReqChallenge, state S654");
                this.Manager.Comment("reaching state \'S822\'");
                goto label15;
            }
            throw new InvalidOperationException("never reached");
        label15:
;
            this.Manager.EndTest();
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate2S24GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S25");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate2S24GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S25");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate2S24GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S25");
        }
        #endregion
        
        #region Test Starting in S26
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_EstablishSecureChannel_NetrServerAuthenticate2S26() {
            this.Manager.BeginTest("Test_EstablishSecureChannel_NetrServerAuthenticate2S26");
            this.Manager.Comment("reaching state \'S26\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp107;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp107);
            this.Manager.AddReturn(GetPlatformInfo, null, temp107);
            this.Manager.Comment("reaching state \'S27\'");
            int temp113 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate2.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate2S26GetPlatformChecker)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate2.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate2S26GetPlatformChecker1)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate2.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate2S26GetPlatformChecker2)));
            if ((temp113 == 0)) {
                this.Manager.Comment("reaching state \'S151\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S319\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S487\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp108;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp108 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S655\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp108, "return of NetrServerReqChallenge, state S655");
                this.Manager.Comment("reaching state \'S823\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp109;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate2(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,4096)\'");
                temp109 = this.INrpcServerAdapterInstance.NetrServerAuthenticate2(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 4096u);
                this.Manager.Checkpoint("MS-NRPC_R103479");
                this.Manager.Comment("reaching state \'S979\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate2/STATUS_DOWNGRADE_DETECTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_DOWNGRADE_DETECTED, temp109, "return of NetrServerAuthenticate2, state S979");
                this.Manager.Comment("reaching state \'S1064\'");
                goto label16;
            }
            if ((temp113 == 1)) {
                this.Manager.Comment("reaching state \'S152\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S320\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S488\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp110;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp110 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S656\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp110, "return of NetrServerReqChallenge, state S656");
                this.Manager.Comment("reaching state \'S824\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp111;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate2(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,4096)\'");
                temp111 = this.INrpcServerAdapterInstance.NetrServerAuthenticate2(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 4096u);
                this.Manager.Checkpoint("MS-NRPC_R103479");
                this.Manager.Comment("reaching state \'S980\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate2/STATUS_DOWNGRADE_DETECTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_DOWNGRADE_DETECTED, temp111, "return of NetrServerAuthenticate2, state S980");
                this.Manager.Comment("reaching state \'S1065\'");
                goto label16;
            }
            if ((temp113 == 2)) {
                this.Manager.Comment("reaching state \'S153\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S321\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S489\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp112;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(NonDcServer,Client)\'");
                temp112 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103509");
                this.Manager.Checkpoint("MS-NRPC_R103341");
                this.Manager.Comment("reaching state \'S657\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp112, "return of NetrServerReqChallenge, state S657");
                this.Manager.Comment("reaching state \'S825\'");
                goto label16;
            }
            throw new InvalidOperationException("never reached");
        label16:
;
            this.Manager.EndTest();
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate2S26GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S27");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate2S26GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S27");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate2S26GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S27");
        }
        #endregion
        
        #region Test Starting in S28
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_EstablishSecureChannel_NetrServerAuthenticate2S28() {
            this.Manager.BeginTest("Test_EstablishSecureChannel_NetrServerAuthenticate2S28");
            this.Manager.Comment("reaching state \'S28\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp114;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp114);
            this.Manager.AddReturn(GetPlatformInfo, null, temp114);
            this.Manager.Comment("reaching state \'S29\'");
            int temp120 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate2.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate2S28GetPlatformChecker)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate2.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate2S28GetPlatformChecker1)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate2.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate2S28GetPlatformChecker2)));
            if ((temp120 == 0)) {
                this.Manager.Comment("reaching state \'S154\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S322\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S490\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp115;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp115 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S658\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp115, "return of NetrServerReqChallenge, state S658");
                this.Manager.Comment("reaching state \'S826\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp116;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate2(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,8192)\'");
                temp116 = this.INrpcServerAdapterInstance.NetrServerAuthenticate2(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 8192u);
                this.Manager.Checkpoint("MS-NRPC_R103479");
                this.Manager.Comment("reaching state \'S981\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate2/STATUS_DOWNGRADE_DETECTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_DOWNGRADE_DETECTED, temp116, "return of NetrServerAuthenticate2, state S981");
                this.Manager.Comment("reaching state \'S1066\'");
                goto label17;
            }
            if ((temp120 == 1)) {
                this.Manager.Comment("reaching state \'S155\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S323\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S491\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp117;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp117 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S659\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp117, "return of NetrServerReqChallenge, state S659");
                this.Manager.Comment("reaching state \'S827\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp118;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate2(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,8192)\'");
                temp118 = this.INrpcServerAdapterInstance.NetrServerAuthenticate2(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 8192u);
                this.Manager.Checkpoint("MS-NRPC_R103479");
                this.Manager.Comment("reaching state \'S982\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate2/STATUS_DOWNGRADE_DETECTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_DOWNGRADE_DETECTED, temp118, "return of NetrServerAuthenticate2, state S982");
                this.Manager.Comment("reaching state \'S1067\'");
                goto label17;
            }
            if ((temp120 == 2)) {
                this.Manager.Comment("reaching state \'S156\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S324\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S492\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp119;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(NonDcServer,Client)\'");
                temp119 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103509");
                this.Manager.Checkpoint("MS-NRPC_R103341");
                this.Manager.Comment("reaching state \'S660\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp119, "return of NetrServerReqChallenge, state S660");
                this.Manager.Comment("reaching state \'S828\'");
                goto label17;
            }
            throw new InvalidOperationException("never reached");
        label17:
;
            this.Manager.EndTest();
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate2S28GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S29");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate2S28GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S29");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate2S28GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S29");
        }
        #endregion
        
        #region Test Starting in S30
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_EstablishSecureChannel_NetrServerAuthenticate2S30() {
            this.Manager.BeginTest("Test_EstablishSecureChannel_NetrServerAuthenticate2S30");
            this.Manager.Comment("reaching state \'S30\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp121;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp121);
            this.Manager.AddReturn(GetPlatformInfo, null, temp121);
            this.Manager.Comment("reaching state \'S31\'");
            int temp127 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate2.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate2S30GetPlatformChecker)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate2.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate2S30GetPlatformChecker1)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate2.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate2S30GetPlatformChecker2)));
            if ((temp127 == 0)) {
                this.Manager.Comment("reaching state \'S157\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S325\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S493\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp122;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp122 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S661\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp122, "return of NetrServerReqChallenge, state S661");
                this.Manager.Comment("reaching state \'S829\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp123;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate2(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,32768)\'");
                temp123 = this.INrpcServerAdapterInstance.NetrServerAuthenticate2(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 32768u);
                this.Manager.Checkpoint("MS-NRPC_R103479");
                this.Manager.Comment("reaching state \'S983\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate2/STATUS_DOWNGRADE_DETECTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_DOWNGRADE_DETECTED, temp123, "return of NetrServerAuthenticate2, state S983");
                this.Manager.Comment("reaching state \'S1068\'");
                goto label18;
            }
            if ((temp127 == 1)) {
                this.Manager.Comment("reaching state \'S158\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S326\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S494\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp124;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp124 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S662\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp124, "return of NetrServerReqChallenge, state S662");
                this.Manager.Comment("reaching state \'S830\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp125;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate2(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,32768)\'");
                temp125 = this.INrpcServerAdapterInstance.NetrServerAuthenticate2(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 32768u);
                this.Manager.Checkpoint("MS-NRPC_R103479");
                this.Manager.Comment("reaching state \'S984\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate2/STATUS_DOWNGRADE_DETECTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_DOWNGRADE_DETECTED, temp125, "return of NetrServerAuthenticate2, state S984");
                this.Manager.Comment("reaching state \'S1069\'");
                goto label18;
            }
            if ((temp127 == 2)) {
                this.Manager.Comment("reaching state \'S159\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S327\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S495\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp126;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(NonDcServer,Client)\'");
                temp126 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103509");
                this.Manager.Checkpoint("MS-NRPC_R103341");
                this.Manager.Comment("reaching state \'S663\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp126, "return of NetrServerReqChallenge, state S663");
                this.Manager.Comment("reaching state \'S831\'");
                goto label18;
            }
            throw new InvalidOperationException("never reached");
        label18:
;
            this.Manager.EndTest();
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate2S30GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S31");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate2S30GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S31");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate2S30GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S31");
        }
        #endregion
        
        #region Test Starting in S32
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_EstablishSecureChannel_NetrServerAuthenticate2S32() {
            this.Manager.BeginTest("Test_EstablishSecureChannel_NetrServerAuthenticate2S32");
            this.Manager.Comment("reaching state \'S32\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp128;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp128);
            this.Manager.AddReturn(GetPlatformInfo, null, temp128);
            this.Manager.Comment("reaching state \'S33\'");
            int temp134 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate2.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate2S32GetPlatformChecker)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate2.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate2S32GetPlatformChecker1)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate2.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate2S32GetPlatformChecker2)));
            if ((temp134 == 0)) {
                this.Manager.Comment("reaching state \'S160\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S328\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S496\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp129;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp129 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S664\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp129, "return of NetrServerReqChallenge, state S664");
                this.Manager.Comment("reaching state \'S832\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp130;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate2(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,65536)\'");
                temp130 = this.INrpcServerAdapterInstance.NetrServerAuthenticate2(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 65536u);
                this.Manager.Checkpoint("MS-NRPC_R103479");
                this.Manager.Comment("reaching state \'S985\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate2/STATUS_DOWNGRADE_DETECTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_DOWNGRADE_DETECTED, temp130, "return of NetrServerAuthenticate2, state S985");
                this.Manager.Comment("reaching state \'S1070\'");
                goto label19;
            }
            if ((temp134 == 1)) {
                this.Manager.Comment("reaching state \'S161\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S329\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S497\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp131;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp131 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S665\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp131, "return of NetrServerReqChallenge, state S665");
                this.Manager.Comment("reaching state \'S833\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp132;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate2(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,65536)\'");
                temp132 = this.INrpcServerAdapterInstance.NetrServerAuthenticate2(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 65536u);
                this.Manager.Checkpoint("MS-NRPC_R103479");
                this.Manager.Comment("reaching state \'S986\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate2/STATUS_DOWNGRADE_DETECTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_DOWNGRADE_DETECTED, temp132, "return of NetrServerAuthenticate2, state S986");
                this.Manager.Comment("reaching state \'S1071\'");
                goto label19;
            }
            if ((temp134 == 2)) {
                this.Manager.Comment("reaching state \'S162\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S330\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S498\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp133;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(NonDcServer,Client)\'");
                temp133 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103509");
                this.Manager.Checkpoint("MS-NRPC_R103341");
                this.Manager.Comment("reaching state \'S666\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp133, "return of NetrServerReqChallenge, state S666");
                this.Manager.Comment("reaching state \'S834\'");
                goto label19;
            }
            throw new InvalidOperationException("never reached");
        label19:
;
            this.Manager.EndTest();
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate2S32GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S33");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate2S32GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S33");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate2S32GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S33");
        }
        #endregion
        
        #region Test Starting in S34
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_EstablishSecureChannel_NetrServerAuthenticate2S34() {
            this.Manager.BeginTest("Test_EstablishSecureChannel_NetrServerAuthenticate2S34");
            this.Manager.Comment("reaching state \'S34\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp135;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp135);
            this.Manager.AddReturn(GetPlatformInfo, null, temp135);
            this.Manager.Comment("reaching state \'S35\'");
            int temp141 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate2.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate2S34GetPlatformChecker)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate2.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate2S34GetPlatformChecker1)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate2.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate2S34GetPlatformChecker2)));
            if ((temp141 == 0)) {
                this.Manager.Comment("reaching state \'S163\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S331\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S499\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp136;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp136 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S667\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp136, "return of NetrServerReqChallenge, state S667");
                this.Manager.Comment("reaching state \'S835\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp137;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate2(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,64)\'");
                temp137 = this.INrpcServerAdapterInstance.NetrServerAuthenticate2(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 64u);
                this.Manager.Checkpoint("MS-NRPC_R103479");
                this.Manager.Comment("reaching state \'S987\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate2/STATUS_DOWNGRADE_DETECTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_DOWNGRADE_DETECTED, temp137, "return of NetrServerAuthenticate2, state S987");
                this.Manager.Comment("reaching state \'S1072\'");
                goto label20;
            }
            if ((temp141 == 1)) {
                this.Manager.Comment("reaching state \'S164\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S332\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S500\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp138;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp138 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S668\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp138, "return of NetrServerReqChallenge, state S668");
                this.Manager.Comment("reaching state \'S836\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp139;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate2(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,64)\'");
                temp139 = this.INrpcServerAdapterInstance.NetrServerAuthenticate2(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 64u);
                this.Manager.Checkpoint("MS-NRPC_R103479");
                this.Manager.Comment("reaching state \'S988\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate2/STATUS_DOWNGRADE_DETECTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_DOWNGRADE_DETECTED, temp139, "return of NetrServerAuthenticate2, state S988");
                this.Manager.Comment("reaching state \'S1073\'");
                goto label20;
            }
            if ((temp141 == 2)) {
                this.Manager.Comment("reaching state \'S165\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S333\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S501\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp140;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(NonDcServer,Client)\'");
                temp140 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103509");
                this.Manager.Checkpoint("MS-NRPC_R103341");
                this.Manager.Comment("reaching state \'S669\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp140, "return of NetrServerReqChallenge, state S669");
                this.Manager.Comment("reaching state \'S837\'");
                goto label20;
            }
            throw new InvalidOperationException("never reached");
        label20:
;
            this.Manager.EndTest();
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate2S34GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S35");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate2S34GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S35");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate2S34GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S35");
        }
        #endregion
        
        #region Test Starting in S36
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_EstablishSecureChannel_NetrServerAuthenticate2S36() {
            this.Manager.BeginTest("Test_EstablishSecureChannel_NetrServerAuthenticate2S36");
            this.Manager.Comment("reaching state \'S36\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp142;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp142);
            this.Manager.AddReturn(GetPlatformInfo, null, temp142);
            this.Manager.Comment("reaching state \'S37\'");
            int temp148 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate2.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate2S36GetPlatformChecker)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate2.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate2S36GetPlatformChecker1)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate2.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate2S36GetPlatformChecker2)));
            if ((temp148 == 0)) {
                this.Manager.Comment("reaching state \'S166\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S334\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S502\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp143;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp143 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S670\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp143, "return of NetrServerReqChallenge, state S670");
                this.Manager.Comment("reaching state \'S838\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp144;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate2(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,131072)\'");
                temp144 = this.INrpcServerAdapterInstance.NetrServerAuthenticate2(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 131072u);
                this.Manager.Checkpoint("MS-NRPC_R103479");
                this.Manager.Comment("reaching state \'S989\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate2/STATUS_DOWNGRADE_DETECTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_DOWNGRADE_DETECTED, temp144, "return of NetrServerAuthenticate2, state S989");
                this.Manager.Comment("reaching state \'S1074\'");
                goto label21;
            }
            if ((temp148 == 1)) {
                this.Manager.Comment("reaching state \'S167\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S335\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S503\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp145;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp145 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S671\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp145, "return of NetrServerReqChallenge, state S671");
                this.Manager.Comment("reaching state \'S839\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp146;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate2(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,131072)\'");
                temp146 = this.INrpcServerAdapterInstance.NetrServerAuthenticate2(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 131072u);
                this.Manager.Checkpoint("MS-NRPC_R103479");
                this.Manager.Comment("reaching state \'S990\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate2/STATUS_DOWNGRADE_DETECTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_DOWNGRADE_DETECTED, temp146, "return of NetrServerAuthenticate2, state S990");
                this.Manager.Comment("reaching state \'S1075\'");
                goto label21;
            }
            if ((temp148 == 2)) {
                this.Manager.Comment("reaching state \'S168\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S336\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S504\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp147;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(NonDcServer,Client)\'");
                temp147 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103509");
                this.Manager.Checkpoint("MS-NRPC_R103341");
                this.Manager.Comment("reaching state \'S672\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp147, "return of NetrServerReqChallenge, state S672");
                this.Manager.Comment("reaching state \'S840\'");
                goto label21;
            }
            throw new InvalidOperationException("never reached");
        label21:
;
            this.Manager.EndTest();
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate2S36GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S37");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate2S36GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S37");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate2S36GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S37");
        }
        #endregion
        
        #region Test Starting in S38
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_EstablishSecureChannel_NetrServerAuthenticate2S38() {
            this.Manager.BeginTest("Test_EstablishSecureChannel_NetrServerAuthenticate2S38");
            this.Manager.Comment("reaching state \'S38\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp149;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp149);
            this.Manager.AddReturn(GetPlatformInfo, null, temp149);
            this.Manager.Comment("reaching state \'S39\'");
            int temp155 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate2.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate2S38GetPlatformChecker)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate2.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate2S38GetPlatformChecker1)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate2.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate2S38GetPlatformChecker2)));
            if ((temp155 == 0)) {
                this.Manager.Comment("reaching state \'S169\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S337\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S505\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp150;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp150 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S673\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp150, "return of NetrServerReqChallenge, state S673");
                this.Manager.Comment("reaching state \'S841\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp151;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate2(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,262144)\'");
                temp151 = this.INrpcServerAdapterInstance.NetrServerAuthenticate2(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 262144u);
                this.Manager.Checkpoint("MS-NRPC_R103479");
                this.Manager.Comment("reaching state \'S991\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate2/STATUS_DOWNGRADE_DETECTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_DOWNGRADE_DETECTED, temp151, "return of NetrServerAuthenticate2, state S991");
                this.Manager.Comment("reaching state \'S1076\'");
                goto label22;
            }
            if ((temp155 == 1)) {
                this.Manager.Comment("reaching state \'S170\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S338\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S506\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp152;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp152 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S674\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp152, "return of NetrServerReqChallenge, state S674");
                this.Manager.Comment("reaching state \'S842\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp153;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate2(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,262144)\'");
                temp153 = this.INrpcServerAdapterInstance.NetrServerAuthenticate2(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 262144u);
                this.Manager.Checkpoint("MS-NRPC_R103479");
                this.Manager.Comment("reaching state \'S992\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate2/STATUS_DOWNGRADE_DETECTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_DOWNGRADE_DETECTED, temp153, "return of NetrServerAuthenticate2, state S992");
                this.Manager.Comment("reaching state \'S1077\'");
                goto label22;
            }
            if ((temp155 == 2)) {
                this.Manager.Comment("reaching state \'S171\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S339\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S507\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp154;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(NonDcServer,Client)\'");
                temp154 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103509");
                this.Manager.Checkpoint("MS-NRPC_R103341");
                this.Manager.Comment("reaching state \'S675\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp154, "return of NetrServerReqChallenge, state S675");
                this.Manager.Comment("reaching state \'S843\'");
                goto label22;
            }
            throw new InvalidOperationException("never reached");
        label22:
;
            this.Manager.EndTest();
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate2S38GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S39");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate2S38GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S39");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate2S38GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S39");
        }
        #endregion
        
        #region Test Starting in S4
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_EstablishSecureChannel_NetrServerAuthenticate2S4() {
            this.Manager.BeginTest("Test_EstablishSecureChannel_NetrServerAuthenticate2S4");
            this.Manager.Comment("reaching state \'S4\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp156;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp156);
            this.Manager.AddReturn(GetPlatformInfo, null, temp156);
            this.Manager.Comment("reaching state \'S5\'");
            int temp162 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate2.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate2S4GetPlatformChecker)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate2.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate2S4GetPlatformChecker1)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate2.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate2S4GetPlatformChecker2)));
            if ((temp162 == 0)) {
                this.Manager.Comment("reaching state \'S118\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S286\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S454\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp157;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp157 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S622\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp157, "return of NetrServerReqChallenge, state S622");
                this.Manager.Comment("reaching state \'S790\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp158;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate2(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,1)\'");
                temp158 = this.INrpcServerAdapterInstance.NetrServerAuthenticate2(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 1u);
                this.Manager.Checkpoint("MS-NRPC_R103479");
                this.Manager.Comment("reaching state \'S957\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate2/STATUS_DOWNGRADE_DETECTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_DOWNGRADE_DETECTED, temp158, "return of NetrServerAuthenticate2, state S957");
                this.Manager.Comment("reaching state \'S1042\'");
                goto label23;
            }
            if ((temp162 == 1)) {
                this.Manager.Comment("reaching state \'S119\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S287\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S455\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp159;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp159 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S623\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp159, "return of NetrServerReqChallenge, state S623");
                this.Manager.Comment("reaching state \'S791\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp160;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate2(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,1)\'");
                temp160 = this.INrpcServerAdapterInstance.NetrServerAuthenticate2(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 1u);
                this.Manager.Checkpoint("MS-NRPC_R103479");
                this.Manager.Comment("reaching state \'S958\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate2/STATUS_DOWNGRADE_DETECTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_DOWNGRADE_DETECTED, temp160, "return of NetrServerAuthenticate2, state S958");
                this.Manager.Comment("reaching state \'S1043\'");
                goto label23;
            }
            if ((temp162 == 2)) {
                this.Manager.Comment("reaching state \'S120\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S288\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S456\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp161;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(NonDcServer,Client)\'");
                temp161 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103509");
                this.Manager.Checkpoint("MS-NRPC_R103341");
                this.Manager.Comment("reaching state \'S624\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp161, "return of NetrServerReqChallenge, state S624");
                this.Manager.Comment("reaching state \'S792\'");
                goto label23;
            }
            throw new InvalidOperationException("never reached");
        label23:
;
            this.Manager.EndTest();
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate2S4GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S5");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate2S4GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S5");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate2S4GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S5");
        }
        #endregion
        
        #region Test Starting in S40
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_EstablishSecureChannel_NetrServerAuthenticate2S40() {
            this.Manager.BeginTest("Test_EstablishSecureChannel_NetrServerAuthenticate2S40");
            this.Manager.Comment("reaching state \'S40\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp163;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp163);
            this.Manager.AddReturn(GetPlatformInfo, null, temp163);
            this.Manager.Comment("reaching state \'S41\'");
            int temp169 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate2.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate2S40GetPlatformChecker)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate2.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate2S40GetPlatformChecker1)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate2.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate2S40GetPlatformChecker2)));
            if ((temp169 == 0)) {
                this.Manager.Comment("reaching state \'S172\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S340\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S508\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp164;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp164 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S676\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp164, "return of NetrServerReqChallenge, state S676");
                this.Manager.Comment("reaching state \'S844\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp165;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate2(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,524288)\'");
                temp165 = this.INrpcServerAdapterInstance.NetrServerAuthenticate2(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 524288u);
                this.Manager.Checkpoint("MS-NRPC_R103479");
                this.Manager.Comment("reaching state \'S993\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate2/STATUS_DOWNGRADE_DETECTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_DOWNGRADE_DETECTED, temp165, "return of NetrServerAuthenticate2, state S993");
                this.Manager.Comment("reaching state \'S1078\'");
                goto label24;
            }
            if ((temp169 == 1)) {
                this.Manager.Comment("reaching state \'S173\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S341\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S509\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp166;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp166 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S677\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp166, "return of NetrServerReqChallenge, state S677");
                this.Manager.Comment("reaching state \'S845\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp167;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate2(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,524288)\'");
                temp167 = this.INrpcServerAdapterInstance.NetrServerAuthenticate2(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 524288u);
                this.Manager.Checkpoint("MS-NRPC_R103479");
                this.Manager.Comment("reaching state \'S994\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate2/STATUS_DOWNGRADE_DETECTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_DOWNGRADE_DETECTED, temp167, "return of NetrServerAuthenticate2, state S994");
                this.Manager.Comment("reaching state \'S1079\'");
                goto label24;
            }
            if ((temp169 == 2)) {
                this.Manager.Comment("reaching state \'S174\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S342\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S510\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp168;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(NonDcServer,Client)\'");
                temp168 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103509");
                this.Manager.Checkpoint("MS-NRPC_R103341");
                this.Manager.Comment("reaching state \'S678\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp168, "return of NetrServerReqChallenge, state S678");
                this.Manager.Comment("reaching state \'S846\'");
                goto label24;
            }
            throw new InvalidOperationException("never reached");
        label24:
;
            this.Manager.EndTest();
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate2S40GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S41");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate2S40GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S41");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate2S40GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S41");
        }
        #endregion
        
        #region Test Starting in S42
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_EstablishSecureChannel_NetrServerAuthenticate2S42() {
            this.Manager.BeginTest("Test_EstablishSecureChannel_NetrServerAuthenticate2S42");
            this.Manager.Comment("reaching state \'S42\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp170;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp170);
            this.Manager.AddReturn(GetPlatformInfo, null, temp170);
            this.Manager.Comment("reaching state \'S43\'");
            int temp176 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate2.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate2S42GetPlatformChecker)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate2.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate2S42GetPlatformChecker1)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate2.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate2S42GetPlatformChecker2)));
            if ((temp176 == 0)) {
                this.Manager.Comment("reaching state \'S175\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S343\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S511\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp171;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp171 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S679\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp171, "return of NetrServerReqChallenge, state S679");
                this.Manager.Comment("reaching state \'S847\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp172;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate2(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,1048576)\'");
                temp172 = this.INrpcServerAdapterInstance.NetrServerAuthenticate2(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 1048576u);
                this.Manager.Checkpoint("MS-NRPC_R103479");
                this.Manager.Comment("reaching state \'S995\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate2/STATUS_DOWNGRADE_DETECTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_DOWNGRADE_DETECTED, temp172, "return of NetrServerAuthenticate2, state S995");
                this.Manager.Comment("reaching state \'S1080\'");
                goto label25;
            }
            if ((temp176 == 1)) {
                this.Manager.Comment("reaching state \'S176\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S344\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S512\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp173;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp173 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S680\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp173, "return of NetrServerReqChallenge, state S680");
                this.Manager.Comment("reaching state \'S848\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp174;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate2(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,1048576)\'");
                temp174 = this.INrpcServerAdapterInstance.NetrServerAuthenticate2(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 1048576u);
                this.Manager.Checkpoint("MS-NRPC_R103479");
                this.Manager.Comment("reaching state \'S996\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate2/STATUS_DOWNGRADE_DETECTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_DOWNGRADE_DETECTED, temp174, "return of NetrServerAuthenticate2, state S996");
                this.Manager.Comment("reaching state \'S1081\'");
                goto label25;
            }
            if ((temp176 == 2)) {
                this.Manager.Comment("reaching state \'S177\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S345\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S513\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp175;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(NonDcServer,Client)\'");
                temp175 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103509");
                this.Manager.Checkpoint("MS-NRPC_R103341");
                this.Manager.Comment("reaching state \'S681\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp175, "return of NetrServerReqChallenge, state S681");
                this.Manager.Comment("reaching state \'S849\'");
                goto label25;
            }
            throw new InvalidOperationException("never reached");
        label25:
;
            this.Manager.EndTest();
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate2S42GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S43");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate2S42GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S43");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate2S42GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S43");
        }
        #endregion
        
        #region Test Starting in S44
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_EstablishSecureChannel_NetrServerAuthenticate2S44() {
            this.Manager.BeginTest("Test_EstablishSecureChannel_NetrServerAuthenticate2S44");
            this.Manager.Comment("reaching state \'S44\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp177;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp177);
            this.Manager.AddReturn(GetPlatformInfo, null, temp177);
            this.Manager.Comment("reaching state \'S45\'");
            int temp183 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate2.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate2S44GetPlatformChecker)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate2.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate2S44GetPlatformChecker1)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate2.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate2S44GetPlatformChecker2)));
            if ((temp183 == 0)) {
                this.Manager.Comment("reaching state \'S178\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S346\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S514\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp178;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp178 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S682\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp178, "return of NetrServerReqChallenge, state S682");
                this.Manager.Comment("reaching state \'S850\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp179;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate2(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,2097152)\'");
                temp179 = this.INrpcServerAdapterInstance.NetrServerAuthenticate2(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 2097152u);
                this.Manager.Checkpoint("MS-NRPC_R103479");
                this.Manager.Comment("reaching state \'S997\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate2/STATUS_DOWNGRADE_DETECTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_DOWNGRADE_DETECTED, temp179, "return of NetrServerAuthenticate2, state S997");
                this.Manager.Comment("reaching state \'S1082\'");
                goto label26;
            }
            if ((temp183 == 1)) {
                this.Manager.Comment("reaching state \'S179\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S347\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S515\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp180;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp180 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S683\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp180, "return of NetrServerReqChallenge, state S683");
                this.Manager.Comment("reaching state \'S851\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp181;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate2(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,2097152)\'");
                temp181 = this.INrpcServerAdapterInstance.NetrServerAuthenticate2(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 2097152u);
                this.Manager.Checkpoint("MS-NRPC_R103479");
                this.Manager.Comment("reaching state \'S998\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate2/STATUS_DOWNGRADE_DETECTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_DOWNGRADE_DETECTED, temp181, "return of NetrServerAuthenticate2, state S998");
                this.Manager.Comment("reaching state \'S1083\'");
                goto label26;
            }
            if ((temp183 == 2)) {
                this.Manager.Comment("reaching state \'S180\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S348\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S516\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp182;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(NonDcServer,Client)\'");
                temp182 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103509");
                this.Manager.Checkpoint("MS-NRPC_R103341");
                this.Manager.Comment("reaching state \'S684\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp182, "return of NetrServerReqChallenge, state S684");
                this.Manager.Comment("reaching state \'S852\'");
                goto label26;
            }
            throw new InvalidOperationException("never reached");
        label26:
;
            this.Manager.EndTest();
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate2S44GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S45");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate2S44GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S45");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate2S44GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S45");
        }
        #endregion
        
        #region Test Starting in S46
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_EstablishSecureChannel_NetrServerAuthenticate2S46() {
            this.Manager.BeginTest("Test_EstablishSecureChannel_NetrServerAuthenticate2S46");
            this.Manager.Comment("reaching state \'S46\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp184;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp184);
            this.Manager.AddReturn(GetPlatformInfo, null, temp184);
            this.Manager.Comment("reaching state \'S47\'");
            int temp190 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate2.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate2S46GetPlatformChecker)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate2.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate2S46GetPlatformChecker1)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate2.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate2S46GetPlatformChecker2)));
            if ((temp190 == 0)) {
                this.Manager.Comment("reaching state \'S181\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S349\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S517\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp185;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp185 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S685\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp185, "return of NetrServerReqChallenge, state S685");
                this.Manager.Comment("reaching state \'S853\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp186;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate2(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,536870912)\'");
                temp186 = this.INrpcServerAdapterInstance.NetrServerAuthenticate2(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 536870912u);
                this.Manager.Checkpoint("MS-NRPC_R103479");
                this.Manager.Comment("reaching state \'S999\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate2/STATUS_DOWNGRADE_DETECTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_DOWNGRADE_DETECTED, temp186, "return of NetrServerAuthenticate2, state S999");
                this.Manager.Comment("reaching state \'S1084\'");
                goto label27;
            }
            if ((temp190 == 1)) {
                this.Manager.Comment("reaching state \'S182\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S350\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S518\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp187;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp187 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S686\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp187, "return of NetrServerReqChallenge, state S686");
                this.Manager.Comment("reaching state \'S854\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp188;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate2(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,536870912)\'");
                temp188 = this.INrpcServerAdapterInstance.NetrServerAuthenticate2(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 536870912u);
                this.Manager.Checkpoint("MS-NRPC_R103479");
                this.Manager.Comment("reaching state \'S1000\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate2/STATUS_DOWNGRADE_DETECTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_DOWNGRADE_DETECTED, temp188, "return of NetrServerAuthenticate2, state S1000");
                this.Manager.Comment("reaching state \'S1085\'");
                goto label27;
            }
            if ((temp190 == 2)) {
                this.Manager.Comment("reaching state \'S183\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S351\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S519\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp189;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(NonDcServer,Client)\'");
                temp189 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103509");
                this.Manager.Checkpoint("MS-NRPC_R103341");
                this.Manager.Comment("reaching state \'S687\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp189, "return of NetrServerReqChallenge, state S687");
                this.Manager.Comment("reaching state \'S855\'");
                goto label27;
            }
            throw new InvalidOperationException("never reached");
        label27:
;
            this.Manager.EndTest();
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate2S46GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S47");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate2S46GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S47");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate2S46GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S47");
        }
        #endregion
        
        #region Test Starting in S48
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_EstablishSecureChannel_NetrServerAuthenticate2S48() {
            this.Manager.BeginTest("Test_EstablishSecureChannel_NetrServerAuthenticate2S48");
            this.Manager.Comment("reaching state \'S48\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp191;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp191);
            this.Manager.AddReturn(GetPlatformInfo, null, temp191);
            this.Manager.Comment("reaching state \'S49\'");
            int temp197 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate2.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate2S48GetPlatformChecker)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate2.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate2S48GetPlatformChecker1)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate2.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate2S48GetPlatformChecker2)));
            if ((temp197 == 0)) {
                this.Manager.Comment("reaching state \'S184\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S352\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S520\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp192;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp192 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S688\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp192, "return of NetrServerReqChallenge, state S688");
                this.Manager.Comment("reaching state \'S856\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp193;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate2(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,1073741824)\'");
                temp193 = this.INrpcServerAdapterInstance.NetrServerAuthenticate2(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 1073741824u);
                this.Manager.Checkpoint("MS-NRPC_R103479");
                this.Manager.Comment("reaching state \'S1001\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate2/STATUS_DOWNGRADE_DETECTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_DOWNGRADE_DETECTED, temp193, "return of NetrServerAuthenticate2, state S1001");
                this.Manager.Comment("reaching state \'S1086\'");
                goto label28;
            }
            if ((temp197 == 1)) {
                this.Manager.Comment("reaching state \'S185\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S353\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S521\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp194;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp194 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S689\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp194, "return of NetrServerReqChallenge, state S689");
                this.Manager.Comment("reaching state \'S857\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp195;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate2(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,1073741824)\'");
                temp195 = this.INrpcServerAdapterInstance.NetrServerAuthenticate2(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 1073741824u);
                this.Manager.Checkpoint("MS-NRPC_R103479");
                this.Manager.Comment("reaching state \'S1002\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate2/STATUS_DOWNGRADE_DETECTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_DOWNGRADE_DETECTED, temp195, "return of NetrServerAuthenticate2, state S1002");
                this.Manager.Comment("reaching state \'S1087\'");
                goto label28;
            }
            if ((temp197 == 2)) {
                this.Manager.Comment("reaching state \'S186\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S354\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S522\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp196;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(NonDcServer,Client)\'");
                temp196 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103509");
                this.Manager.Checkpoint("MS-NRPC_R103341");
                this.Manager.Comment("reaching state \'S690\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp196, "return of NetrServerReqChallenge, state S690");
                this.Manager.Comment("reaching state \'S858\'");
                goto label28;
            }
            throw new InvalidOperationException("never reached");
        label28:
;
            this.Manager.EndTest();
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate2S48GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S49");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate2S48GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S49");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate2S48GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S49");
        }
        #endregion
        
        #region Test Starting in S50
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_EstablishSecureChannel_NetrServerAuthenticate2S50() {
            this.Manager.BeginTest("Test_EstablishSecureChannel_NetrServerAuthenticate2S50");
            this.Manager.Comment("reaching state \'S50\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp198;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp198);
            this.Manager.AddReturn(GetPlatformInfo, null, temp198);
            this.Manager.Comment("reaching state \'S51\'");
            int temp204 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate2.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate2S50GetPlatformChecker)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate2.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate2S50GetPlatformChecker1)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate2.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate2S50GetPlatformChecker2)));
            if ((temp204 == 0)) {
                this.Manager.Comment("reaching state \'S187\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S355\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S523\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp199;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp199 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S691\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp199, "return of NetrServerReqChallenge, state S691");
                this.Manager.Comment("reaching state \'S859\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp200;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate2(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16384)\'");
                temp200 = this.INrpcServerAdapterInstance.NetrServerAuthenticate2(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16384u);
                this.Manager.Checkpoint("MS-NRPC_R103478");
                this.Manager.Comment("reaching state \'S1003\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate2/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp200, "return of NetrServerAuthenticate2, state S1003");
                this.Manager.Comment("reaching state \'S1088\'");
                goto label29;
            }
            if ((temp204 == 1)) {
                this.Manager.Comment("reaching state \'S188\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S356\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S524\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp201;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp201 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S692\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp201, "return of NetrServerReqChallenge, state S692");
                this.Manager.Comment("reaching state \'S860\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp202;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate2(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16384)\'");
                temp202 = this.INrpcServerAdapterInstance.NetrServerAuthenticate2(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16384u);
                this.Manager.Checkpoint("MS-NRPC_R103478");
                this.Manager.Comment("reaching state \'S1004\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate2/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp202, "return of NetrServerAuthenticate2, state S1004");
                this.Manager.Comment("reaching state \'S1089\'");
                goto label29;
            }
            if ((temp204 == 2)) {
                this.Manager.Comment("reaching state \'S189\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S357\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S525\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp203;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(NonDcServer,Client)\'");
                temp203 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103509");
                this.Manager.Checkpoint("MS-NRPC_R103341");
                this.Manager.Comment("reaching state \'S693\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp203, "return of NetrServerReqChallenge, state S693");
                this.Manager.Comment("reaching state \'S861\'");
                goto label29;
            }
            throw new InvalidOperationException("never reached");
        label29:
;
            this.Manager.EndTest();
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate2S50GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S51");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate2S50GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S51");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate2S50GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S51");
        }
        #endregion
        
        #region Test Starting in S52
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_EstablishSecureChannel_NetrServerAuthenticate2S52() {
            this.Manager.BeginTest("Test_EstablishSecureChannel_NetrServerAuthenticate2S52");
            this.Manager.Comment("reaching state \'S52\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp205;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp205);
            this.Manager.AddReturn(GetPlatformInfo, null, temp205);
            this.Manager.Comment("reaching state \'S53\'");
            int temp211 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate2.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate2S52GetPlatformChecker)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate2.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate2S52GetPlatformChecker1)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate2.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate2S52GetPlatformChecker2)));
            if ((temp211 == 0)) {
                this.Manager.Comment("reaching state \'S190\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S358\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S526\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp206;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp206 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S694\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp206, "return of NetrServerReqChallenge, state S694");
                this.Manager.Comment("reaching state \'S862\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp207;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate2(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,False,16384)\'");
                temp207 = this.INrpcServerAdapterInstance.NetrServerAuthenticate2(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, false, 16384u);
                this.Manager.Checkpoint("MS-NRPC_R103492");
                this.Manager.Checkpoint("MS-NRPC_R103479");
                this.Manager.Comment("reaching state \'S1005\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate2/STATUS_ACCESS_DENIED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp207, "return of NetrServerAuthenticate2, state S1005");
                this.Manager.Comment("reaching state \'S1090\'");
                goto label30;
            }
            if ((temp211 == 1)) {
                this.Manager.Comment("reaching state \'S191\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S359\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S527\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp208;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp208 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S695\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp208, "return of NetrServerReqChallenge, state S695");
                this.Manager.Comment("reaching state \'S863\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp209;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate2(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,False,16384)\'");
                temp209 = this.INrpcServerAdapterInstance.NetrServerAuthenticate2(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, false, 16384u);
                this.Manager.Checkpoint("MS-NRPC_R103492");
                this.Manager.Checkpoint("MS-NRPC_R103479");
                this.Manager.Comment("reaching state \'S1006\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate2/STATUS_ACCESS_DENIED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp209, "return of NetrServerAuthenticate2, state S1006");
                this.Manager.Comment("reaching state \'S1091\'");
                goto label30;
            }
            if ((temp211 == 2)) {
                this.Manager.Comment("reaching state \'S192\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S360\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S528\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp210;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(NonDcServer,Client)\'");
                temp210 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103509");
                this.Manager.Checkpoint("MS-NRPC_R103341");
                this.Manager.Comment("reaching state \'S696\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp210, "return of NetrServerReqChallenge, state S696");
                this.Manager.Comment("reaching state \'S864\'");
                goto label30;
            }
            throw new InvalidOperationException("never reached");
        label30:
;
            this.Manager.EndTest();
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate2S52GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S53");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate2S52GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S53");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate2S52GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S53");
        }
        #endregion
        
        #region Test Starting in S54
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_EstablishSecureChannel_NetrServerAuthenticate2S54() {
            this.Manager.BeginTest("Test_EstablishSecureChannel_NetrServerAuthenticate2S54");
            this.Manager.Comment("reaching state \'S54\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp212;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp212);
            this.Manager.AddReturn(GetPlatformInfo, null, temp212);
            this.Manager.Comment("reaching state \'S55\'");
            int temp218 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate2.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate2S54GetPlatformChecker)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate2.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate2S54GetPlatformChecker1)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate2.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate2S54GetPlatformChecker2)));
            if ((temp218 == 0)) {
                this.Manager.Comment("reaching state \'S193\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S361\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S529\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp213;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(NonDcServer,Client)\'");
                temp213 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103509");
                this.Manager.Checkpoint("MS-NRPC_R103341");
                this.Manager.Comment("reaching state \'S697\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp213, "return of NetrServerReqChallenge, state S697");
                this.Manager.Comment("reaching state \'S865\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp214;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate2(NonDcServer,DomainMemberComputerAcco" +
                        "unt,WorkstationSecureChannel,Client,True,16384)\'");
                temp214 = this.INrpcServerAdapterInstance.NetrServerAuthenticate2(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16384u);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R104904");
                this.Manager.Checkpoint("MS-NRPC_R103479");
                this.Manager.Comment("reaching state \'S1007\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate2/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp214, "return of NetrServerAuthenticate2, state S1007");
                this.Manager.Comment("reaching state \'S1092\'");
                goto label31;
            }
            if ((temp218 == 1)) {
                this.Manager.Comment("reaching state \'S194\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S362\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S530\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp215;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(NonDcServer,Client)\'");
                temp215 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103509");
                this.Manager.Checkpoint("MS-NRPC_R103341");
                this.Manager.Comment("reaching state \'S698\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp215, "return of NetrServerReqChallenge, state S698");
                this.Manager.Comment("reaching state \'S866\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp216;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate2(NonDcServer,DomainMemberComputerAcco" +
                        "unt,WorkstationSecureChannel,Client,True,16384)\'");
                temp216 = this.INrpcServerAdapterInstance.NetrServerAuthenticate2(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16384u);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R104904");
                this.Manager.Checkpoint("MS-NRPC_R103479");
                this.Manager.Comment("reaching state \'S1008\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate2/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp216, "return of NetrServerAuthenticate2, state S1008");
                this.Manager.Comment("reaching state \'S1093\'");
                goto label31;
            }
            if ((temp218 == 2)) {
                this.Manager.Comment("reaching state \'S195\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S363\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S531\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp217;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(NonDcServer,Client)\'");
                temp217 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103509");
                this.Manager.Checkpoint("MS-NRPC_R103341");
                this.Manager.Comment("reaching state \'S699\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp217, "return of NetrServerReqChallenge, state S699");
                this.Manager.Comment("reaching state \'S867\'");
                goto label31;
            }
            throw new InvalidOperationException("never reached");
        label31:
;
            this.Manager.EndTest();
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate2S54GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S55");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate2S54GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S55");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate2S54GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S55");
        }
        #endregion
        
        #region Test Starting in S56
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_EstablishSecureChannel_NetrServerAuthenticate2S56() {
            this.Manager.BeginTest("Test_EstablishSecureChannel_NetrServerAuthenticate2S56");
            this.Manager.Comment("reaching state \'S56\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp219;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp219);
            this.Manager.AddReturn(GetPlatformInfo, null, temp219);
            this.Manager.Comment("reaching state \'S57\'");
            int temp224 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate2.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate2S56GetPlatformChecker)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate2.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate2S56GetPlatformChecker1)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate2.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate2S56GetPlatformChecker2)));
            if ((temp224 == 0)) {
                this.Manager.Comment("reaching state \'S196\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S364\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S532\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp220;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp220 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S700\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp220, "return of NetrServerReqChallenge, state S700");
                this.Manager.Comment("reaching state \'S868\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp221;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate2(NonDcServer,DomainMemberComputerAcco" +
                        "unt,WorkstationSecureChannel,Client,True,16384)\'");
                temp221 = this.INrpcServerAdapterInstance.NetrServerAuthenticate2(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16384u);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R104904");
                this.Manager.Checkpoint("MS-NRPC_R103479");
                this.Manager.Comment("reaching state \'S1009\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate2/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp221, "return of NetrServerAuthenticate2, state S1009");
                this.Manager.Comment("reaching state \'S1094\'");
                goto label32;
            }
            if ((temp224 == 1)) {
                this.Manager.Comment("reaching state \'S197\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S365\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S533\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp222;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(NonDcServer,Client)\'");
                temp222 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103509");
                this.Manager.Checkpoint("MS-NRPC_R103341");
                this.Manager.Comment("reaching state \'S701\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp222, "return of NetrServerReqChallenge, state S701");
                this.Manager.Comment("reaching state \'S869\'");
                goto label32;
            }
            if ((temp224 == 2)) {
                this.Manager.Comment("reaching state \'S198\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S366\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S534\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp223;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(NonDcServer,Client)\'");
                temp223 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103509");
                this.Manager.Checkpoint("MS-NRPC_R103341");
                this.Manager.Comment("reaching state \'S702\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp223, "return of NetrServerReqChallenge, state S702");
                this.Manager.Comment("reaching state \'S870\'");
                goto label32;
            }
            throw new InvalidOperationException("never reached");
        label32:
;
            this.Manager.EndTest();
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate2S56GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S57");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate2S56GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S57");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate2S56GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S57");
        }
        #endregion
        
        #region Test Starting in S58
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_EstablishSecureChannel_NetrServerAuthenticate2S58() {
            this.Manager.BeginTest("Test_EstablishSecureChannel_NetrServerAuthenticate2S58");
            this.Manager.Comment("reaching state \'S58\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp225;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp225);
            this.Manager.AddReturn(GetPlatformInfo, null, temp225);
            this.Manager.Comment("reaching state \'S59\'");
            int temp230 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate2.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate2S58GetPlatformChecker)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate2.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate2S58GetPlatformChecker1)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate2.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate2S58GetPlatformChecker2)));
            if ((temp230 == 0)) {
                this.Manager.Comment("reaching state \'S199\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S367\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S535\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp226;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp226 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S703\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp226, "return of NetrServerReqChallenge, state S703");
                this.Manager.Comment("reaching state \'S871\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp227;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate2(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,1)\'");
                temp227 = this.INrpcServerAdapterInstance.NetrServerAuthenticate2(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 1u);
                this.Manager.Checkpoint("MS-NRPC_R103479");
                this.Manager.Comment("reaching state \'S1010\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate2/STATUS_DOWNGRADE_DETECTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_DOWNGRADE_DETECTED, temp227, "return of NetrServerAuthenticate2, state S1010");
                this.Manager.Comment("reaching state \'S1095\'");
                goto label33;
            }
            if ((temp230 == 1)) {
                this.Manager.Comment("reaching state \'S200\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S368\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S536\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp228;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(NonDcServer,Client)\'");
                temp228 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103509");
                this.Manager.Checkpoint("MS-NRPC_R103341");
                this.Manager.Comment("reaching state \'S704\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp228, "return of NetrServerReqChallenge, state S704");
                this.Manager.Comment("reaching state \'S872\'");
                goto label33;
            }
            if ((temp230 == 2)) {
                this.Manager.Comment("reaching state \'S201\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S369\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S537\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp229;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(NonDcServer,Client)\'");
                temp229 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103509");
                this.Manager.Checkpoint("MS-NRPC_R103341");
                this.Manager.Comment("reaching state \'S705\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp229, "return of NetrServerReqChallenge, state S705");
                this.Manager.Comment("reaching state \'S873\'");
                goto label33;
            }
            throw new InvalidOperationException("never reached");
        label33:
;
            this.Manager.EndTest();
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate2S58GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S59");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate2S58GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S59");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate2S58GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S59");
        }
        #endregion
        
        #region Test Starting in S6
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_EstablishSecureChannel_NetrServerAuthenticate2S6() {
            this.Manager.BeginTest("Test_EstablishSecureChannel_NetrServerAuthenticate2S6");
            this.Manager.Comment("reaching state \'S6\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp231;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp231);
            this.Manager.AddReturn(GetPlatformInfo, null, temp231);
            this.Manager.Comment("reaching state \'S7\'");
            int temp237 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate2.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate2S6GetPlatformChecker)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate2.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate2S6GetPlatformChecker1)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate2.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate2S6GetPlatformChecker2)));
            if ((temp237 == 0)) {
                this.Manager.Comment("reaching state \'S121\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S289\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S457\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp232;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp232 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S625\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp232, "return of NetrServerReqChallenge, state S625");
                this.Manager.Comment("reaching state \'S793\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp233;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate2(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,2)\'");
                temp233 = this.INrpcServerAdapterInstance.NetrServerAuthenticate2(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 2u);
                this.Manager.Checkpoint("MS-NRPC_R103479");
                this.Manager.Comment("reaching state \'S959\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate2/STATUS_DOWNGRADE_DETECTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_DOWNGRADE_DETECTED, temp233, "return of NetrServerAuthenticate2, state S959");
                this.Manager.Comment("reaching state \'S1044\'");
                goto label34;
            }
            if ((temp237 == 1)) {
                this.Manager.Comment("reaching state \'S122\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S290\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S458\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp234;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp234 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S626\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp234, "return of NetrServerReqChallenge, state S626");
                this.Manager.Comment("reaching state \'S794\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp235;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate2(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,2)\'");
                temp235 = this.INrpcServerAdapterInstance.NetrServerAuthenticate2(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 2u);
                this.Manager.Checkpoint("MS-NRPC_R103479");
                this.Manager.Comment("reaching state \'S960\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate2/STATUS_DOWNGRADE_DETECTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_DOWNGRADE_DETECTED, temp235, "return of NetrServerAuthenticate2, state S960");
                this.Manager.Comment("reaching state \'S1045\'");
                goto label34;
            }
            if ((temp237 == 2)) {
                this.Manager.Comment("reaching state \'S123\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S291\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S459\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp236;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(NonDcServer,Client)\'");
                temp236 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103509");
                this.Manager.Checkpoint("MS-NRPC_R103341");
                this.Manager.Comment("reaching state \'S627\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp236, "return of NetrServerReqChallenge, state S627");
                this.Manager.Comment("reaching state \'S795\'");
                goto label34;
            }
            throw new InvalidOperationException("never reached");
        label34:
;
            this.Manager.EndTest();
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate2S6GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S7");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate2S6GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S7");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate2S6GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S7");
        }
        #endregion
        
        #region Test Starting in S60
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_EstablishSecureChannel_NetrServerAuthenticate2S60() {
            this.Manager.BeginTest("Test_EstablishSecureChannel_NetrServerAuthenticate2S60");
            this.Manager.Comment("reaching state \'S60\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp238;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp238);
            this.Manager.AddReturn(GetPlatformInfo, null, temp238);
            this.Manager.Comment("reaching state \'S61\'");
            int temp243 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate2.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate2S60GetPlatformChecker)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate2.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate2S60GetPlatformChecker1)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate2.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate2S60GetPlatformChecker2)));
            if ((temp243 == 0)) {
                this.Manager.Comment("reaching state \'S202\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S370\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S538\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp239;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp239 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S706\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp239, "return of NetrServerReqChallenge, state S706");
                this.Manager.Comment("reaching state \'S874\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp240;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate2(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,2)\'");
                temp240 = this.INrpcServerAdapterInstance.NetrServerAuthenticate2(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 2u);
                this.Manager.Checkpoint("MS-NRPC_R103479");
                this.Manager.Comment("reaching state \'S1011\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate2/STATUS_DOWNGRADE_DETECTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_DOWNGRADE_DETECTED, temp240, "return of NetrServerAuthenticate2, state S1011");
                this.Manager.Comment("reaching state \'S1096\'");
                goto label35;
            }
            if ((temp243 == 1)) {
                this.Manager.Comment("reaching state \'S203\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S371\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S539\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp241;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(NonDcServer,Client)\'");
                temp241 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103509");
                this.Manager.Checkpoint("MS-NRPC_R103341");
                this.Manager.Comment("reaching state \'S707\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp241, "return of NetrServerReqChallenge, state S707");
                this.Manager.Comment("reaching state \'S875\'");
                goto label35;
            }
            if ((temp243 == 2)) {
                this.Manager.Comment("reaching state \'S204\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S372\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S540\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp242;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(NonDcServer,Client)\'");
                temp242 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103509");
                this.Manager.Checkpoint("MS-NRPC_R103341");
                this.Manager.Comment("reaching state \'S708\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp242, "return of NetrServerReqChallenge, state S708");
                this.Manager.Comment("reaching state \'S876\'");
                goto label35;
            }
            throw new InvalidOperationException("never reached");
        label35:
;
            this.Manager.EndTest();
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate2S60GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S61");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate2S60GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S61");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate2S60GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S61");
        }
        #endregion
        
        #region Test Starting in S62
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_EstablishSecureChannel_NetrServerAuthenticate2S62() {
            this.Manager.BeginTest("Test_EstablishSecureChannel_NetrServerAuthenticate2S62");
            this.Manager.Comment("reaching state \'S62\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp244;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp244);
            this.Manager.AddReturn(GetPlatformInfo, null, temp244);
            this.Manager.Comment("reaching state \'S63\'");
            int temp249 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate2.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate2S62GetPlatformChecker)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate2.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate2S62GetPlatformChecker1)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate2.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate2S62GetPlatformChecker2)));
            if ((temp249 == 0)) {
                this.Manager.Comment("reaching state \'S205\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S373\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S541\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp245;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp245 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S709\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp245, "return of NetrServerReqChallenge, state S709");
                this.Manager.Comment("reaching state \'S877\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp246;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate2(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,4)\'");
                temp246 = this.INrpcServerAdapterInstance.NetrServerAuthenticate2(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 4u);
                this.Manager.Checkpoint("MS-NRPC_R103479");
                this.Manager.Comment("reaching state \'S1012\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate2/STATUS_DOWNGRADE_DETECTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_DOWNGRADE_DETECTED, temp246, "return of NetrServerAuthenticate2, state S1012");
                this.Manager.Comment("reaching state \'S1097\'");
                goto label36;
            }
            if ((temp249 == 1)) {
                this.Manager.Comment("reaching state \'S206\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S374\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S542\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp247;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(NonDcServer,Client)\'");
                temp247 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103509");
                this.Manager.Checkpoint("MS-NRPC_R103341");
                this.Manager.Comment("reaching state \'S710\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp247, "return of NetrServerReqChallenge, state S710");
                this.Manager.Comment("reaching state \'S878\'");
                goto label36;
            }
            if ((temp249 == 2)) {
                this.Manager.Comment("reaching state \'S207\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S375\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S543\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp248;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(NonDcServer,Client)\'");
                temp248 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103509");
                this.Manager.Checkpoint("MS-NRPC_R103341");
                this.Manager.Comment("reaching state \'S711\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp248, "return of NetrServerReqChallenge, state S711");
                this.Manager.Comment("reaching state \'S879\'");
                goto label36;
            }
            throw new InvalidOperationException("never reached");
        label36:
;
            this.Manager.EndTest();
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate2S62GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S63");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate2S62GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S63");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate2S62GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S63");
        }
        #endregion
        
        #region Test Starting in S64
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_EstablishSecureChannel_NetrServerAuthenticate2S64() {
            this.Manager.BeginTest("Test_EstablishSecureChannel_NetrServerAuthenticate2S64");
            this.Manager.Comment("reaching state \'S64\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp250;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp250);
            this.Manager.AddReturn(GetPlatformInfo, null, temp250);
            this.Manager.Comment("reaching state \'S65\'");
            int temp255 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate2.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate2S64GetPlatformChecker)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate2.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate2S64GetPlatformChecker1)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate2.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate2S64GetPlatformChecker2)));
            if ((temp255 == 0)) {
                this.Manager.Comment("reaching state \'S208\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S376\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S544\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp251;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp251 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S712\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp251, "return of NetrServerReqChallenge, state S712");
                this.Manager.Comment("reaching state \'S880\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp252;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate2(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,8)\'");
                temp252 = this.INrpcServerAdapterInstance.NetrServerAuthenticate2(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 8u);
                this.Manager.Checkpoint("MS-NRPC_R103479");
                this.Manager.Comment("reaching state \'S1013\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate2/STATUS_DOWNGRADE_DETECTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_DOWNGRADE_DETECTED, temp252, "return of NetrServerAuthenticate2, state S1013");
                this.Manager.Comment("reaching state \'S1098\'");
                goto label37;
            }
            if ((temp255 == 1)) {
                this.Manager.Comment("reaching state \'S209\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S377\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S545\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp253;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(NonDcServer,Client)\'");
                temp253 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103509");
                this.Manager.Checkpoint("MS-NRPC_R103341");
                this.Manager.Comment("reaching state \'S713\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp253, "return of NetrServerReqChallenge, state S713");
                this.Manager.Comment("reaching state \'S881\'");
                goto label37;
            }
            if ((temp255 == 2)) {
                this.Manager.Comment("reaching state \'S210\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S378\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S546\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp254;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(NonDcServer,Client)\'");
                temp254 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103509");
                this.Manager.Checkpoint("MS-NRPC_R103341");
                this.Manager.Comment("reaching state \'S714\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp254, "return of NetrServerReqChallenge, state S714");
                this.Manager.Comment("reaching state \'S882\'");
                goto label37;
            }
            throw new InvalidOperationException("never reached");
        label37:
;
            this.Manager.EndTest();
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate2S64GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S65");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate2S64GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S65");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate2S64GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S65");
        }
        #endregion
        
        #region Test Starting in S66
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_EstablishSecureChannel_NetrServerAuthenticate2S66() {
            this.Manager.BeginTest("Test_EstablishSecureChannel_NetrServerAuthenticate2S66");
            this.Manager.Comment("reaching state \'S66\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp256;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp256);
            this.Manager.AddReturn(GetPlatformInfo, null, temp256);
            this.Manager.Comment("reaching state \'S67\'");
            int temp261 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate2.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate2S66GetPlatformChecker)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate2.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate2S66GetPlatformChecker1)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate2.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate2S66GetPlatformChecker2)));
            if ((temp261 == 0)) {
                this.Manager.Comment("reaching state \'S211\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S379\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S547\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp257;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp257 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S715\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp257, "return of NetrServerReqChallenge, state S715");
                this.Manager.Comment("reaching state \'S883\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp258;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate2(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16)\'");
                temp258 = this.INrpcServerAdapterInstance.NetrServerAuthenticate2(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16u);
                this.Manager.Checkpoint("MS-NRPC_R103479");
                this.Manager.Comment("reaching state \'S1014\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate2/STATUS_DOWNGRADE_DETECTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_DOWNGRADE_DETECTED, temp258, "return of NetrServerAuthenticate2, state S1014");
                this.Manager.Comment("reaching state \'S1099\'");
                goto label38;
            }
            if ((temp261 == 1)) {
                this.Manager.Comment("reaching state \'S212\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S380\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S548\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp259;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(NonDcServer,Client)\'");
                temp259 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103509");
                this.Manager.Checkpoint("MS-NRPC_R103341");
                this.Manager.Comment("reaching state \'S716\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp259, "return of NetrServerReqChallenge, state S716");
                this.Manager.Comment("reaching state \'S884\'");
                goto label38;
            }
            if ((temp261 == 2)) {
                this.Manager.Comment("reaching state \'S213\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S381\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S549\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp260;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(NonDcServer,Client)\'");
                temp260 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103509");
                this.Manager.Checkpoint("MS-NRPC_R103341");
                this.Manager.Comment("reaching state \'S717\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp260, "return of NetrServerReqChallenge, state S717");
                this.Manager.Comment("reaching state \'S885\'");
                goto label38;
            }
            throw new InvalidOperationException("never reached");
        label38:
;
            this.Manager.EndTest();
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate2S66GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S67");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate2S66GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S67");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate2S66GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S67");
        }
        #endregion
        
        #region Test Starting in S68
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_EstablishSecureChannel_NetrServerAuthenticate2S68() {
            this.Manager.BeginTest("Test_EstablishSecureChannel_NetrServerAuthenticate2S68");
            this.Manager.Comment("reaching state \'S68\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp262;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp262);
            this.Manager.AddReturn(GetPlatformInfo, null, temp262);
            this.Manager.Comment("reaching state \'S69\'");
            int temp267 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate2.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate2S68GetPlatformChecker)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate2.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate2S68GetPlatformChecker1)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate2.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate2S68GetPlatformChecker2)));
            if ((temp267 == 0)) {
                this.Manager.Comment("reaching state \'S214\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S382\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S550\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp263;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp263 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S718\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp263, "return of NetrServerReqChallenge, state S718");
                this.Manager.Comment("reaching state \'S886\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp264;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate2(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,32)\'");
                temp264 = this.INrpcServerAdapterInstance.NetrServerAuthenticate2(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 32u);
                this.Manager.Checkpoint("MS-NRPC_R103479");
                this.Manager.Comment("reaching state \'S1015\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate2/STATUS_DOWNGRADE_DETECTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_DOWNGRADE_DETECTED, temp264, "return of NetrServerAuthenticate2, state S1015");
                this.Manager.Comment("reaching state \'S1100\'");
                goto label39;
            }
            if ((temp267 == 1)) {
                this.Manager.Comment("reaching state \'S215\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S383\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S551\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp265;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(NonDcServer,Client)\'");
                temp265 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103509");
                this.Manager.Checkpoint("MS-NRPC_R103341");
                this.Manager.Comment("reaching state \'S719\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp265, "return of NetrServerReqChallenge, state S719");
                this.Manager.Comment("reaching state \'S887\'");
                goto label39;
            }
            if ((temp267 == 2)) {
                this.Manager.Comment("reaching state \'S216\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S384\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S552\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp266;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(NonDcServer,Client)\'");
                temp266 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103509");
                this.Manager.Checkpoint("MS-NRPC_R103341");
                this.Manager.Comment("reaching state \'S720\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp266, "return of NetrServerReqChallenge, state S720");
                this.Manager.Comment("reaching state \'S888\'");
                goto label39;
            }
            throw new InvalidOperationException("never reached");
        label39:
;
            this.Manager.EndTest();
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate2S68GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S69");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate2S68GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S69");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate2S68GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S69");
        }
        #endregion
        
        #region Test Starting in S70
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_EstablishSecureChannel_NetrServerAuthenticate2S70() {
            this.Manager.BeginTest("Test_EstablishSecureChannel_NetrServerAuthenticate2S70");
            this.Manager.Comment("reaching state \'S70\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp268;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp268);
            this.Manager.AddReturn(GetPlatformInfo, null, temp268);
            this.Manager.Comment("reaching state \'S71\'");
            int temp273 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate2.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate2S70GetPlatformChecker)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate2.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate2S70GetPlatformChecker1)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate2.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate2S70GetPlatformChecker2)));
            if ((temp273 == 0)) {
                this.Manager.Comment("reaching state \'S217\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S385\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S553\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp269;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp269 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S721\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp269, "return of NetrServerReqChallenge, state S721");
                this.Manager.Comment("reaching state \'S889\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp270;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate2(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,64)\'");
                temp270 = this.INrpcServerAdapterInstance.NetrServerAuthenticate2(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 64u);
                this.Manager.Checkpoint("MS-NRPC_R103479");
                this.Manager.Comment("reaching state \'S1016\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate2/STATUS_DOWNGRADE_DETECTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_DOWNGRADE_DETECTED, temp270, "return of NetrServerAuthenticate2, state S1016");
                this.Manager.Comment("reaching state \'S1101\'");
                goto label40;
            }
            if ((temp273 == 1)) {
                this.Manager.Comment("reaching state \'S218\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S386\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S554\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp271;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(NonDcServer,Client)\'");
                temp271 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103509");
                this.Manager.Checkpoint("MS-NRPC_R103341");
                this.Manager.Comment("reaching state \'S722\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp271, "return of NetrServerReqChallenge, state S722");
                this.Manager.Comment("reaching state \'S890\'");
                goto label40;
            }
            if ((temp273 == 2)) {
                this.Manager.Comment("reaching state \'S219\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S387\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S555\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp272;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(NonDcServer,Client)\'");
                temp272 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103509");
                this.Manager.Checkpoint("MS-NRPC_R103341");
                this.Manager.Comment("reaching state \'S723\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp272, "return of NetrServerReqChallenge, state S723");
                this.Manager.Comment("reaching state \'S891\'");
                goto label40;
            }
            throw new InvalidOperationException("never reached");
        label40:
;
            this.Manager.EndTest();
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate2S70GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S71");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate2S70GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S71");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate2S70GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S71");
        }
        #endregion
        
        #region Test Starting in S72
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_EstablishSecureChannel_NetrServerAuthenticate2S72() {
            this.Manager.BeginTest("Test_EstablishSecureChannel_NetrServerAuthenticate2S72");
            this.Manager.Comment("reaching state \'S72\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp274;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp274);
            this.Manager.AddReturn(GetPlatformInfo, null, temp274);
            this.Manager.Comment("reaching state \'S73\'");
            int temp279 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate2.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate2S72GetPlatformChecker)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate2.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate2S72GetPlatformChecker1)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate2.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate2S72GetPlatformChecker2)));
            if ((temp279 == 0)) {
                this.Manager.Comment("reaching state \'S220\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S388\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S556\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp275;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp275 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S724\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp275, "return of NetrServerReqChallenge, state S724");
                this.Manager.Comment("reaching state \'S892\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp276;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate2(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,128)\'");
                temp276 = this.INrpcServerAdapterInstance.NetrServerAuthenticate2(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 128u);
                this.Manager.Checkpoint("MS-NRPC_R103479");
                this.Manager.Comment("reaching state \'S1017\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate2/STATUS_DOWNGRADE_DETECTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_DOWNGRADE_DETECTED, temp276, "return of NetrServerAuthenticate2, state S1017");
                this.Manager.Comment("reaching state \'S1102\'");
                goto label41;
            }
            if ((temp279 == 1)) {
                this.Manager.Comment("reaching state \'S221\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S389\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S557\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp277;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(NonDcServer,Client)\'");
                temp277 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103509");
                this.Manager.Checkpoint("MS-NRPC_R103341");
                this.Manager.Comment("reaching state \'S725\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp277, "return of NetrServerReqChallenge, state S725");
                this.Manager.Comment("reaching state \'S893\'");
                goto label41;
            }
            if ((temp279 == 2)) {
                this.Manager.Comment("reaching state \'S222\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S390\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S558\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp278;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(NonDcServer,Client)\'");
                temp278 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103509");
                this.Manager.Checkpoint("MS-NRPC_R103341");
                this.Manager.Comment("reaching state \'S726\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp278, "return of NetrServerReqChallenge, state S726");
                this.Manager.Comment("reaching state \'S894\'");
                goto label41;
            }
            throw new InvalidOperationException("never reached");
        label41:
;
            this.Manager.EndTest();
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate2S72GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S73");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate2S72GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S73");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate2S72GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S73");
        }
        #endregion
        
        #region Test Starting in S74
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_EstablishSecureChannel_NetrServerAuthenticate2S74() {
            this.Manager.BeginTest("Test_EstablishSecureChannel_NetrServerAuthenticate2S74");
            this.Manager.Comment("reaching state \'S74\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp280;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp280);
            this.Manager.AddReturn(GetPlatformInfo, null, temp280);
            this.Manager.Comment("reaching state \'S75\'");
            int temp285 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate2.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate2S74GetPlatformChecker)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate2.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate2S74GetPlatformChecker1)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate2.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate2S74GetPlatformChecker2)));
            if ((temp285 == 0)) {
                this.Manager.Comment("reaching state \'S223\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S391\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S559\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp281;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp281 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S727\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp281, "return of NetrServerReqChallenge, state S727");
                this.Manager.Comment("reaching state \'S895\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp282;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate2(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,256)\'");
                temp282 = this.INrpcServerAdapterInstance.NetrServerAuthenticate2(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 256u);
                this.Manager.Checkpoint("MS-NRPC_R103479");
                this.Manager.Comment("reaching state \'S1018\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate2/STATUS_DOWNGRADE_DETECTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_DOWNGRADE_DETECTED, temp282, "return of NetrServerAuthenticate2, state S1018");
                this.Manager.Comment("reaching state \'S1103\'");
                goto label42;
            }
            if ((temp285 == 1)) {
                this.Manager.Comment("reaching state \'S224\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S392\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S560\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp283;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(NonDcServer,Client)\'");
                temp283 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103509");
                this.Manager.Checkpoint("MS-NRPC_R103341");
                this.Manager.Comment("reaching state \'S728\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp283, "return of NetrServerReqChallenge, state S728");
                this.Manager.Comment("reaching state \'S896\'");
                goto label42;
            }
            if ((temp285 == 2)) {
                this.Manager.Comment("reaching state \'S225\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S393\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S561\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp284;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(NonDcServer,Client)\'");
                temp284 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103509");
                this.Manager.Checkpoint("MS-NRPC_R103341");
                this.Manager.Comment("reaching state \'S729\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp284, "return of NetrServerReqChallenge, state S729");
                this.Manager.Comment("reaching state \'S897\'");
                goto label42;
            }
            throw new InvalidOperationException("never reached");
        label42:
;
            this.Manager.EndTest();
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate2S74GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S75");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate2S74GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S75");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate2S74GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S75");
        }
        #endregion
        
        #region Test Starting in S76
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_EstablishSecureChannel_NetrServerAuthenticate2S76() {
            this.Manager.BeginTest("Test_EstablishSecureChannel_NetrServerAuthenticate2S76");
            this.Manager.Comment("reaching state \'S76\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp286;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp286);
            this.Manager.AddReturn(GetPlatformInfo, null, temp286);
            this.Manager.Comment("reaching state \'S77\'");
            int temp291 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate2.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate2S76GetPlatformChecker)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate2.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate2S76GetPlatformChecker1)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate2.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate2S76GetPlatformChecker2)));
            if ((temp291 == 0)) {
                this.Manager.Comment("reaching state \'S226\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S394\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S562\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp287;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp287 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S730\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp287, "return of NetrServerReqChallenge, state S730");
                this.Manager.Comment("reaching state \'S898\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp288;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate2(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,512)\'");
                temp288 = this.INrpcServerAdapterInstance.NetrServerAuthenticate2(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 512u);
                this.Manager.Checkpoint("MS-NRPC_R103479");
                this.Manager.Comment("reaching state \'S1019\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate2/STATUS_DOWNGRADE_DETECTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_DOWNGRADE_DETECTED, temp288, "return of NetrServerAuthenticate2, state S1019");
                this.Manager.Comment("reaching state \'S1104\'");
                goto label43;
            }
            if ((temp291 == 1)) {
                this.Manager.Comment("reaching state \'S227\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S395\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S563\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp289;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(NonDcServer,Client)\'");
                temp289 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103509");
                this.Manager.Checkpoint("MS-NRPC_R103341");
                this.Manager.Comment("reaching state \'S731\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp289, "return of NetrServerReqChallenge, state S731");
                this.Manager.Comment("reaching state \'S899\'");
                goto label43;
            }
            if ((temp291 == 2)) {
                this.Manager.Comment("reaching state \'S228\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S396\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S564\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp290;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(NonDcServer,Client)\'");
                temp290 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103509");
                this.Manager.Checkpoint("MS-NRPC_R103341");
                this.Manager.Comment("reaching state \'S732\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp290, "return of NetrServerReqChallenge, state S732");
                this.Manager.Comment("reaching state \'S900\'");
                goto label43;
            }
            throw new InvalidOperationException("never reached");
        label43:
;
            this.Manager.EndTest();
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate2S76GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S77");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate2S76GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S77");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate2S76GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S77");
        }
        #endregion
        
        #region Test Starting in S78
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_EstablishSecureChannel_NetrServerAuthenticate2S78() {
            this.Manager.BeginTest("Test_EstablishSecureChannel_NetrServerAuthenticate2S78");
            this.Manager.Comment("reaching state \'S78\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp292;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp292);
            this.Manager.AddReturn(GetPlatformInfo, null, temp292);
            this.Manager.Comment("reaching state \'S79\'");
            int temp297 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate2.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate2S78GetPlatformChecker)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate2.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate2S78GetPlatformChecker1)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate2.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate2S78GetPlatformChecker2)));
            if ((temp297 == 0)) {
                this.Manager.Comment("reaching state \'S229\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S397\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S565\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp293;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp293 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S733\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp293, "return of NetrServerReqChallenge, state S733");
                this.Manager.Comment("reaching state \'S901\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp294;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate2(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,1024)\'");
                temp294 = this.INrpcServerAdapterInstance.NetrServerAuthenticate2(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 1024u);
                this.Manager.Checkpoint("MS-NRPC_R103479");
                this.Manager.Comment("reaching state \'S1020\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate2/STATUS_DOWNGRADE_DETECTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_DOWNGRADE_DETECTED, temp294, "return of NetrServerAuthenticate2, state S1020");
                this.Manager.Comment("reaching state \'S1105\'");
                goto label44;
            }
            if ((temp297 == 1)) {
                this.Manager.Comment("reaching state \'S230\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S398\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S566\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp295;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(NonDcServer,Client)\'");
                temp295 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103509");
                this.Manager.Checkpoint("MS-NRPC_R103341");
                this.Manager.Comment("reaching state \'S734\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp295, "return of NetrServerReqChallenge, state S734");
                this.Manager.Comment("reaching state \'S902\'");
                goto label44;
            }
            if ((temp297 == 2)) {
                this.Manager.Comment("reaching state \'S231\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S399\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S567\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp296;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(NonDcServer,Client)\'");
                temp296 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103509");
                this.Manager.Checkpoint("MS-NRPC_R103341");
                this.Manager.Comment("reaching state \'S735\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp296, "return of NetrServerReqChallenge, state S735");
                this.Manager.Comment("reaching state \'S903\'");
                goto label44;
            }
            throw new InvalidOperationException("never reached");
        label44:
;
            this.Manager.EndTest();
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate2S78GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S79");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate2S78GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S79");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate2S78GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S79");
        }
        #endregion
        
        #region Test Starting in S8
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_EstablishSecureChannel_NetrServerAuthenticate2S8() {
            this.Manager.BeginTest("Test_EstablishSecureChannel_NetrServerAuthenticate2S8");
            this.Manager.Comment("reaching state \'S8\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp298;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp298);
            this.Manager.AddReturn(GetPlatformInfo, null, temp298);
            this.Manager.Comment("reaching state \'S9\'");
            int temp304 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate2.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate2S8GetPlatformChecker)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate2.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate2S8GetPlatformChecker1)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate2.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate2S8GetPlatformChecker2)));
            if ((temp304 == 0)) {
                this.Manager.Comment("reaching state \'S124\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S292\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S460\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp299;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp299 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S628\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp299, "return of NetrServerReqChallenge, state S628");
                this.Manager.Comment("reaching state \'S796\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp300;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate2(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,4)\'");
                temp300 = this.INrpcServerAdapterInstance.NetrServerAuthenticate2(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 4u);
                this.Manager.Checkpoint("MS-NRPC_R103479");
                this.Manager.Comment("reaching state \'S961\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate2/STATUS_DOWNGRADE_DETECTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_DOWNGRADE_DETECTED, temp300, "return of NetrServerAuthenticate2, state S961");
                this.Manager.Comment("reaching state \'S1046\'");
                goto label45;
            }
            if ((temp304 == 1)) {
                this.Manager.Comment("reaching state \'S125\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S293\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S461\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp301;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp301 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S629\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp301, "return of NetrServerReqChallenge, state S629");
                this.Manager.Comment("reaching state \'S797\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp302;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate2(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,4)\'");
                temp302 = this.INrpcServerAdapterInstance.NetrServerAuthenticate2(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 4u);
                this.Manager.Checkpoint("MS-NRPC_R103479");
                this.Manager.Comment("reaching state \'S962\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate2/STATUS_DOWNGRADE_DETECTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_DOWNGRADE_DETECTED, temp302, "return of NetrServerAuthenticate2, state S962");
                this.Manager.Comment("reaching state \'S1047\'");
                goto label45;
            }
            if ((temp304 == 2)) {
                this.Manager.Comment("reaching state \'S126\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S294\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S462\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp303;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(NonDcServer,Client)\'");
                temp303 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103509");
                this.Manager.Checkpoint("MS-NRPC_R103341");
                this.Manager.Comment("reaching state \'S630\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp303, "return of NetrServerReqChallenge, state S630");
                this.Manager.Comment("reaching state \'S798\'");
                goto label45;
            }
            throw new InvalidOperationException("never reached");
        label45:
;
            this.Manager.EndTest();
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate2S8GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S9");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate2S8GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S9");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate2S8GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S9");
        }
        #endregion
        
        #region Test Starting in S80
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_EstablishSecureChannel_NetrServerAuthenticate2S80() {
            this.Manager.BeginTest("Test_EstablishSecureChannel_NetrServerAuthenticate2S80");
            this.Manager.Comment("reaching state \'S80\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp305;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp305);
            this.Manager.AddReturn(GetPlatformInfo, null, temp305);
            this.Manager.Comment("reaching state \'S81\'");
            int temp310 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate2.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate2S80GetPlatformChecker)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate2.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate2S80GetPlatformChecker1)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate2.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate2S80GetPlatformChecker2)));
            if ((temp310 == 0)) {
                this.Manager.Comment("reaching state \'S232\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S400\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S568\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp306;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp306 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S736\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp306, "return of NetrServerReqChallenge, state S736");
                this.Manager.Comment("reaching state \'S904\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp307;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate2(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,2048)\'");
                temp307 = this.INrpcServerAdapterInstance.NetrServerAuthenticate2(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 2048u);
                this.Manager.Checkpoint("MS-NRPC_R103479");
                this.Manager.Comment("reaching state \'S1021\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate2/STATUS_DOWNGRADE_DETECTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_DOWNGRADE_DETECTED, temp307, "return of NetrServerAuthenticate2, state S1021");
                this.Manager.Comment("reaching state \'S1106\'");
                goto label46;
            }
            if ((temp310 == 1)) {
                this.Manager.Comment("reaching state \'S233\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S401\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S569\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp308;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(NonDcServer,Client)\'");
                temp308 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103509");
                this.Manager.Checkpoint("MS-NRPC_R103341");
                this.Manager.Comment("reaching state \'S737\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp308, "return of NetrServerReqChallenge, state S737");
                this.Manager.Comment("reaching state \'S905\'");
                goto label46;
            }
            if ((temp310 == 2)) {
                this.Manager.Comment("reaching state \'S234\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S402\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S570\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp309;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(NonDcServer,Client)\'");
                temp309 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103509");
                this.Manager.Checkpoint("MS-NRPC_R103341");
                this.Manager.Comment("reaching state \'S738\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp309, "return of NetrServerReqChallenge, state S738");
                this.Manager.Comment("reaching state \'S906\'");
                goto label46;
            }
            throw new InvalidOperationException("never reached");
        label46:
;
            this.Manager.EndTest();
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate2S80GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S81");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate2S80GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S81");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate2S80GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S81");
        }
        #endregion
        
        #region Test Starting in S82
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_EstablishSecureChannel_NetrServerAuthenticate2S82() {
            this.Manager.BeginTest("Test_EstablishSecureChannel_NetrServerAuthenticate2S82");
            this.Manager.Comment("reaching state \'S82\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp311;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp311);
            this.Manager.AddReturn(GetPlatformInfo, null, temp311);
            this.Manager.Comment("reaching state \'S83\'");
            int temp316 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate2.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate2S82GetPlatformChecker)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate2.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate2S82GetPlatformChecker1)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate2.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate2S82GetPlatformChecker2)));
            if ((temp316 == 0)) {
                this.Manager.Comment("reaching state \'S235\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S403\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S571\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp312;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp312 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S739\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp312, "return of NetrServerReqChallenge, state S739");
                this.Manager.Comment("reaching state \'S907\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp313;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate2(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,4096)\'");
                temp313 = this.INrpcServerAdapterInstance.NetrServerAuthenticate2(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 4096u);
                this.Manager.Checkpoint("MS-NRPC_R103479");
                this.Manager.Comment("reaching state \'S1022\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate2/STATUS_DOWNGRADE_DETECTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_DOWNGRADE_DETECTED, temp313, "return of NetrServerAuthenticate2, state S1022");
                this.Manager.Comment("reaching state \'S1107\'");
                goto label47;
            }
            if ((temp316 == 1)) {
                this.Manager.Comment("reaching state \'S236\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S404\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S572\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp314;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(NonDcServer,Client)\'");
                temp314 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103509");
                this.Manager.Checkpoint("MS-NRPC_R103341");
                this.Manager.Comment("reaching state \'S740\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp314, "return of NetrServerReqChallenge, state S740");
                this.Manager.Comment("reaching state \'S908\'");
                goto label47;
            }
            if ((temp316 == 2)) {
                this.Manager.Comment("reaching state \'S237\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S405\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S573\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp315;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(NonDcServer,Client)\'");
                temp315 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103509");
                this.Manager.Checkpoint("MS-NRPC_R103341");
                this.Manager.Comment("reaching state \'S741\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp315, "return of NetrServerReqChallenge, state S741");
                this.Manager.Comment("reaching state \'S909\'");
                goto label47;
            }
            throw new InvalidOperationException("never reached");
        label47:
;
            this.Manager.EndTest();
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate2S82GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S83");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate2S82GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S83");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate2S82GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S83");
        }
        #endregion
        
        #region Test Starting in S84
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_EstablishSecureChannel_NetrServerAuthenticate2S84() {
            this.Manager.BeginTest("Test_EstablishSecureChannel_NetrServerAuthenticate2S84");
            this.Manager.Comment("reaching state \'S84\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp317;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp317);
            this.Manager.AddReturn(GetPlatformInfo, null, temp317);
            this.Manager.Comment("reaching state \'S85\'");
            int temp322 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate2.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate2S84GetPlatformChecker)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate2.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate2S84GetPlatformChecker1)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate2.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate2S84GetPlatformChecker2)));
            if ((temp322 == 0)) {
                this.Manager.Comment("reaching state \'S238\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S406\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S574\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp318;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp318 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S742\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp318, "return of NetrServerReqChallenge, state S742");
                this.Manager.Comment("reaching state \'S910\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp319;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate2(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,8192)\'");
                temp319 = this.INrpcServerAdapterInstance.NetrServerAuthenticate2(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 8192u);
                this.Manager.Checkpoint("MS-NRPC_R103479");
                this.Manager.Comment("reaching state \'S1023\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate2/STATUS_DOWNGRADE_DETECTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_DOWNGRADE_DETECTED, temp319, "return of NetrServerAuthenticate2, state S1023");
                this.Manager.Comment("reaching state \'S1108\'");
                goto label48;
            }
            if ((temp322 == 1)) {
                this.Manager.Comment("reaching state \'S239\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S407\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S575\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp320;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(NonDcServer,Client)\'");
                temp320 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103509");
                this.Manager.Checkpoint("MS-NRPC_R103341");
                this.Manager.Comment("reaching state \'S743\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp320, "return of NetrServerReqChallenge, state S743");
                this.Manager.Comment("reaching state \'S911\'");
                goto label48;
            }
            if ((temp322 == 2)) {
                this.Manager.Comment("reaching state \'S240\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S408\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S576\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp321;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(NonDcServer,Client)\'");
                temp321 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103509");
                this.Manager.Checkpoint("MS-NRPC_R103341");
                this.Manager.Comment("reaching state \'S744\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp321, "return of NetrServerReqChallenge, state S744");
                this.Manager.Comment("reaching state \'S912\'");
                goto label48;
            }
            throw new InvalidOperationException("never reached");
        label48:
;
            this.Manager.EndTest();
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate2S84GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S85");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate2S84GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S85");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate2S84GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S85");
        }
        #endregion
        
        #region Test Starting in S86
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_EstablishSecureChannel_NetrServerAuthenticate2S86() {
            this.Manager.BeginTest("Test_EstablishSecureChannel_NetrServerAuthenticate2S86");
            this.Manager.Comment("reaching state \'S86\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp323;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp323);
            this.Manager.AddReturn(GetPlatformInfo, null, temp323);
            this.Manager.Comment("reaching state \'S87\'");
            int temp328 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate2.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate2S86GetPlatformChecker)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate2.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate2S86GetPlatformChecker1)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate2.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate2S86GetPlatformChecker2)));
            if ((temp328 == 0)) {
                this.Manager.Comment("reaching state \'S241\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S409\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S577\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp324;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp324 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S745\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp324, "return of NetrServerReqChallenge, state S745");
                this.Manager.Comment("reaching state \'S913\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp325;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate2(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,32768)\'");
                temp325 = this.INrpcServerAdapterInstance.NetrServerAuthenticate2(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 32768u);
                this.Manager.Checkpoint("MS-NRPC_R103479");
                this.Manager.Comment("reaching state \'S1024\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate2/STATUS_DOWNGRADE_DETECTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_DOWNGRADE_DETECTED, temp325, "return of NetrServerAuthenticate2, state S1024");
                this.Manager.Comment("reaching state \'S1109\'");
                goto label49;
            }
            if ((temp328 == 1)) {
                this.Manager.Comment("reaching state \'S242\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S410\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S578\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp326;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(NonDcServer,Client)\'");
                temp326 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103509");
                this.Manager.Checkpoint("MS-NRPC_R103341");
                this.Manager.Comment("reaching state \'S746\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp326, "return of NetrServerReqChallenge, state S746");
                this.Manager.Comment("reaching state \'S914\'");
                goto label49;
            }
            if ((temp328 == 2)) {
                this.Manager.Comment("reaching state \'S243\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S411\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S579\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp327;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(NonDcServer,Client)\'");
                temp327 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103509");
                this.Manager.Checkpoint("MS-NRPC_R103341");
                this.Manager.Comment("reaching state \'S747\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp327, "return of NetrServerReqChallenge, state S747");
                this.Manager.Comment("reaching state \'S915\'");
                goto label49;
            }
            throw new InvalidOperationException("never reached");
        label49:
;
            this.Manager.EndTest();
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate2S86GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S87");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate2S86GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S87");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate2S86GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S87");
        }
        #endregion
        
        #region Test Starting in S88
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_EstablishSecureChannel_NetrServerAuthenticate2S88() {
            this.Manager.BeginTest("Test_EstablishSecureChannel_NetrServerAuthenticate2S88");
            this.Manager.Comment("reaching state \'S88\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp329;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp329);
            this.Manager.AddReturn(GetPlatformInfo, null, temp329);
            this.Manager.Comment("reaching state \'S89\'");
            int temp334 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate2.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate2S88GetPlatformChecker)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate2.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate2S88GetPlatformChecker1)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate2.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate2S88GetPlatformChecker2)));
            if ((temp334 == 0)) {
                this.Manager.Comment("reaching state \'S244\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S412\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S580\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp330;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp330 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S748\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp330, "return of NetrServerReqChallenge, state S748");
                this.Manager.Comment("reaching state \'S916\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp331;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate2(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,65536)\'");
                temp331 = this.INrpcServerAdapterInstance.NetrServerAuthenticate2(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 65536u);
                this.Manager.Checkpoint("MS-NRPC_R103479");
                this.Manager.Comment("reaching state \'S1025\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate2/STATUS_DOWNGRADE_DETECTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_DOWNGRADE_DETECTED, temp331, "return of NetrServerAuthenticate2, state S1025");
                this.Manager.Comment("reaching state \'S1110\'");
                goto label50;
            }
            if ((temp334 == 1)) {
                this.Manager.Comment("reaching state \'S245\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S413\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S581\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp332;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(NonDcServer,Client)\'");
                temp332 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103509");
                this.Manager.Checkpoint("MS-NRPC_R103341");
                this.Manager.Comment("reaching state \'S749\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp332, "return of NetrServerReqChallenge, state S749");
                this.Manager.Comment("reaching state \'S917\'");
                goto label50;
            }
            if ((temp334 == 2)) {
                this.Manager.Comment("reaching state \'S246\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S414\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S582\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp333;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(NonDcServer,Client)\'");
                temp333 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103509");
                this.Manager.Checkpoint("MS-NRPC_R103341");
                this.Manager.Comment("reaching state \'S750\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp333, "return of NetrServerReqChallenge, state S750");
                this.Manager.Comment("reaching state \'S918\'");
                goto label50;
            }
            throw new InvalidOperationException("never reached");
        label50:
;
            this.Manager.EndTest();
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate2S88GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S89");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate2S88GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S89");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate2S88GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S89");
        }
        #endregion
        
        #region Test Starting in S90
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_EstablishSecureChannel_NetrServerAuthenticate2S90() {
            this.Manager.BeginTest("Test_EstablishSecureChannel_NetrServerAuthenticate2S90");
            this.Manager.Comment("reaching state \'S90\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp335;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp335);
            this.Manager.AddReturn(GetPlatformInfo, null, temp335);
            this.Manager.Comment("reaching state \'S91\'");
            int temp340 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate2.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate2S90GetPlatformChecker)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate2.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate2S90GetPlatformChecker1)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate2.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate2S90GetPlatformChecker2)));
            if ((temp340 == 0)) {
                this.Manager.Comment("reaching state \'S247\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S415\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S583\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp336;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp336 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S751\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp336, "return of NetrServerReqChallenge, state S751");
                this.Manager.Comment("reaching state \'S919\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp337;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate2(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,1073741824)\'");
                temp337 = this.INrpcServerAdapterInstance.NetrServerAuthenticate2(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 1073741824u);
                this.Manager.Checkpoint("MS-NRPC_R103479");
                this.Manager.Comment("reaching state \'S1026\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate2/STATUS_DOWNGRADE_DETECTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_DOWNGRADE_DETECTED, temp337, "return of NetrServerAuthenticate2, state S1026");
                this.Manager.Comment("reaching state \'S1111\'");
                goto label51;
            }
            if ((temp340 == 1)) {
                this.Manager.Comment("reaching state \'S248\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S416\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S584\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp338;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(NonDcServer,Client)\'");
                temp338 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103509");
                this.Manager.Checkpoint("MS-NRPC_R103341");
                this.Manager.Comment("reaching state \'S752\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp338, "return of NetrServerReqChallenge, state S752");
                this.Manager.Comment("reaching state \'S920\'");
                goto label51;
            }
            if ((temp340 == 2)) {
                this.Manager.Comment("reaching state \'S249\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S417\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S585\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp339;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(NonDcServer,Client)\'");
                temp339 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103509");
                this.Manager.Checkpoint("MS-NRPC_R103341");
                this.Manager.Comment("reaching state \'S753\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp339, "return of NetrServerReqChallenge, state S753");
                this.Manager.Comment("reaching state \'S921\'");
                goto label51;
            }
            throw new InvalidOperationException("never reached");
        label51:
;
            this.Manager.EndTest();
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate2S90GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S91");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate2S90GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S91");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate2S90GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S91");
        }
        #endregion
        
        #region Test Starting in S92
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_EstablishSecureChannel_NetrServerAuthenticate2S92() {
            this.Manager.BeginTest("Test_EstablishSecureChannel_NetrServerAuthenticate2S92");
            this.Manager.Comment("reaching state \'S92\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp341;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp341);
            this.Manager.AddReturn(GetPlatformInfo, null, temp341);
            this.Manager.Comment("reaching state \'S93\'");
            int temp346 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate2.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate2S92GetPlatformChecker)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate2.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate2S92GetPlatformChecker1)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate2.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate2S92GetPlatformChecker2)));
            if ((temp346 == 0)) {
                this.Manager.Comment("reaching state \'S250\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S418\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S586\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp342;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp342 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S754\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp342, "return of NetrServerReqChallenge, state S754");
                this.Manager.Comment("reaching state \'S922\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp343;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate2(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,131072)\'");
                temp343 = this.INrpcServerAdapterInstance.NetrServerAuthenticate2(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 131072u);
                this.Manager.Checkpoint("MS-NRPC_R103479");
                this.Manager.Comment("reaching state \'S1027\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate2/STATUS_DOWNGRADE_DETECTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_DOWNGRADE_DETECTED, temp343, "return of NetrServerAuthenticate2, state S1027");
                this.Manager.Comment("reaching state \'S1112\'");
                goto label52;
            }
            if ((temp346 == 1)) {
                this.Manager.Comment("reaching state \'S251\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S419\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S587\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp344;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(NonDcServer,Client)\'");
                temp344 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103509");
                this.Manager.Checkpoint("MS-NRPC_R103341");
                this.Manager.Comment("reaching state \'S755\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp344, "return of NetrServerReqChallenge, state S755");
                this.Manager.Comment("reaching state \'S923\'");
                goto label52;
            }
            if ((temp346 == 2)) {
                this.Manager.Comment("reaching state \'S252\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S420\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S588\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp345;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(NonDcServer,Client)\'");
                temp345 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103509");
                this.Manager.Checkpoint("MS-NRPC_R103341");
                this.Manager.Comment("reaching state \'S756\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp345, "return of NetrServerReqChallenge, state S756");
                this.Manager.Comment("reaching state \'S924\'");
                goto label52;
            }
            throw new InvalidOperationException("never reached");
        label52:
;
            this.Manager.EndTest();
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate2S92GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S93");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate2S92GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S93");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate2S92GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S93");
        }
        #endregion
        
        #region Test Starting in S94
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_EstablishSecureChannel_NetrServerAuthenticate2S94() {
            this.Manager.BeginTest("Test_EstablishSecureChannel_NetrServerAuthenticate2S94");
            this.Manager.Comment("reaching state \'S94\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp347;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp347);
            this.Manager.AddReturn(GetPlatformInfo, null, temp347);
            this.Manager.Comment("reaching state \'S95\'");
            int temp352 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate2.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate2S94GetPlatformChecker)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate2.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate2S94GetPlatformChecker1)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate2.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate2S94GetPlatformChecker2)));
            if ((temp352 == 0)) {
                this.Manager.Comment("reaching state \'S253\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S421\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S589\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp348;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp348 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S757\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp348, "return of NetrServerReqChallenge, state S757");
                this.Manager.Comment("reaching state \'S925\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp349;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate2(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,262144)\'");
                temp349 = this.INrpcServerAdapterInstance.NetrServerAuthenticate2(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 262144u);
                this.Manager.Checkpoint("MS-NRPC_R103479");
                this.Manager.Comment("reaching state \'S1028\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate2/STATUS_DOWNGRADE_DETECTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_DOWNGRADE_DETECTED, temp349, "return of NetrServerAuthenticate2, state S1028");
                this.Manager.Comment("reaching state \'S1113\'");
                goto label53;
            }
            if ((temp352 == 1)) {
                this.Manager.Comment("reaching state \'S254\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S422\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S590\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp350;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(NonDcServer,Client)\'");
                temp350 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103509");
                this.Manager.Checkpoint("MS-NRPC_R103341");
                this.Manager.Comment("reaching state \'S758\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp350, "return of NetrServerReqChallenge, state S758");
                this.Manager.Comment("reaching state \'S926\'");
                goto label53;
            }
            if ((temp352 == 2)) {
                this.Manager.Comment("reaching state \'S255\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S423\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S591\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp351;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(NonDcServer,Client)\'");
                temp351 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103509");
                this.Manager.Checkpoint("MS-NRPC_R103341");
                this.Manager.Comment("reaching state \'S759\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp351, "return of NetrServerReqChallenge, state S759");
                this.Manager.Comment("reaching state \'S927\'");
                goto label53;
            }
            throw new InvalidOperationException("never reached");
        label53:
;
            this.Manager.EndTest();
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate2S94GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S95");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate2S94GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S95");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate2S94GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S95");
        }
        #endregion
        
        #region Test Starting in S96
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_EstablishSecureChannel_NetrServerAuthenticate2S96() {
            this.Manager.BeginTest("Test_EstablishSecureChannel_NetrServerAuthenticate2S96");
            this.Manager.Comment("reaching state \'S96\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp353;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp353);
            this.Manager.AddReturn(GetPlatformInfo, null, temp353);
            this.Manager.Comment("reaching state \'S97\'");
            int temp358 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate2.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate2S96GetPlatformChecker)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate2.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate2S96GetPlatformChecker1)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate2.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate2S96GetPlatformChecker2)));
            if ((temp358 == 0)) {
                this.Manager.Comment("reaching state \'S256\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S424\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S592\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp354;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp354 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S760\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp354, "return of NetrServerReqChallenge, state S760");
                this.Manager.Comment("reaching state \'S928\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp355;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate2(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,524288)\'");
                temp355 = this.INrpcServerAdapterInstance.NetrServerAuthenticate2(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 524288u);
                this.Manager.Checkpoint("MS-NRPC_R103479");
                this.Manager.Comment("reaching state \'S1029\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate2/STATUS_DOWNGRADE_DETECTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_DOWNGRADE_DETECTED, temp355, "return of NetrServerAuthenticate2, state S1029");
                this.Manager.Comment("reaching state \'S1114\'");
                goto label54;
            }
            if ((temp358 == 1)) {
                this.Manager.Comment("reaching state \'S257\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S425\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S593\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp356;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(NonDcServer,Client)\'");
                temp356 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103509");
                this.Manager.Checkpoint("MS-NRPC_R103341");
                this.Manager.Comment("reaching state \'S761\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp356, "return of NetrServerReqChallenge, state S761");
                this.Manager.Comment("reaching state \'S929\'");
                goto label54;
            }
            if ((temp358 == 2)) {
                this.Manager.Comment("reaching state \'S258\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S426\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S594\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp357;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(NonDcServer,Client)\'");
                temp357 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103509");
                this.Manager.Checkpoint("MS-NRPC_R103341");
                this.Manager.Comment("reaching state \'S762\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp357, "return of NetrServerReqChallenge, state S762");
                this.Manager.Comment("reaching state \'S930\'");
                goto label54;
            }
            throw new InvalidOperationException("never reached");
        label54:
;
            this.Manager.EndTest();
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate2S96GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S97");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate2S96GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S97");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate2S96GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S97");
        }
        #endregion
        
        #region Test Starting in S98
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_EstablishSecureChannel_NetrServerAuthenticate2S98() {
            this.Manager.BeginTest("Test_EstablishSecureChannel_NetrServerAuthenticate2S98");
            this.Manager.Comment("reaching state \'S98\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp359;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp359);
            this.Manager.AddReturn(GetPlatformInfo, null, temp359);
            this.Manager.Comment("reaching state \'S99\'");
            int temp364 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate2.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate2S98GetPlatformChecker)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate2.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate2S98GetPlatformChecker1)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate2.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate2S98GetPlatformChecker2)));
            if ((temp364 == 0)) {
                this.Manager.Comment("reaching state \'S259\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S427\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S595\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp360;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp360 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S763\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp360, "return of NetrServerReqChallenge, state S763");
                this.Manager.Comment("reaching state \'S931\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp361;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate2(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,1048576)\'");
                temp361 = this.INrpcServerAdapterInstance.NetrServerAuthenticate2(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 1048576u);
                this.Manager.Checkpoint("MS-NRPC_R103479");
                this.Manager.Comment("reaching state \'S1030\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate2/STATUS_DOWNGRADE_DETECTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_DOWNGRADE_DETECTED, temp361, "return of NetrServerAuthenticate2, state S1030");
                this.Manager.Comment("reaching state \'S1115\'");
                goto label55;
            }
            if ((temp364 == 1)) {
                this.Manager.Comment("reaching state \'S260\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S428\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S596\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp362;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(NonDcServer,Client)\'");
                temp362 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103509");
                this.Manager.Checkpoint("MS-NRPC_R103341");
                this.Manager.Comment("reaching state \'S764\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp362, "return of NetrServerReqChallenge, state S764");
                this.Manager.Comment("reaching state \'S932\'");
                goto label55;
            }
            if ((temp364 == 2)) {
                this.Manager.Comment("reaching state \'S261\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S429\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S597\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp363;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(NonDcServer,Client)\'");
                temp363 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103509");
                this.Manager.Checkpoint("MS-NRPC_R103341");
                this.Manager.Comment("reaching state \'S765\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp363, "return of NetrServerReqChallenge, state S765");
                this.Manager.Comment("reaching state \'S933\'");
                goto label55;
            }
            throw new InvalidOperationException("never reached");
        label55:
;
            this.Manager.EndTest();
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate2S98GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S99");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate2S98GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S99");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate2S98GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S99");
        }
        #endregion
    }
}
