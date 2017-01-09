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
    public partial class Test_EstablishSecureChannel_NetrServerAuthenticate : PtfTestClassBase {
        
        public Test_EstablishSecureChannel_NetrServerAuthenticate() {
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
        public void Test_EstablishSecureChannel_NetrServerAuthenticateS0() {
            this.Manager.BeginTest("Test_EstablishSecureChannel_NetrServerAuthenticateS0");
            this.Manager.Comment("reaching state \'S0\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp0;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp0);
            this.Manager.AddReturn(GetPlatformInfo, null, temp0);
            this.Manager.Comment("reaching state \'S1\'");
            int temp7 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticateS0GetPlatformChecker)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticateS0GetPlatformChecker1)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticateS0GetPlatformChecker2)));
            if ((temp7 == 0)) {
                this.Manager.Comment("reaching state \'S18\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S45\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S72\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp1;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp1 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S99\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp1, "return of NetrServerReqChallenge, state S99");
                this.Manager.Comment("reaching state \'S126\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp2;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate(PrimaryDc,InvalidAccount,WorkstationS" +
                        "ecureChannel,Client,True)\'");
                temp2 = this.INrpcServerAdapterInstance.NetrServerAuthenticate(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true);
                this.Manager.Checkpoint("MS-NRPC_R1404");
                this.Manager.Comment("reaching state \'S153\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate/STATUS_DOWNGRADE_DETECTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_DOWNGRADE_DETECTED, temp2, "return of NetrServerAuthenticate, state S153");
                this.Manager.Comment("reaching state \'S168\'");
                goto label0;
            }
            if ((temp7 == 1)) {
                this.Manager.Comment("reaching state \'S19\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S46\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S73\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp3;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp3 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S100\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp3, "return of NetrServerReqChallenge, state S100");
                this.Manager.Comment("reaching state \'S127\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp4;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate(PrimaryDc,InvalidAccount,WorkstationS" +
                        "ecureChannel,Client,True)\'");
                temp4 = this.INrpcServerAdapterInstance.NetrServerAuthenticate(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true);
                this.Manager.Checkpoint("MS-NRPC_R1404");
                this.Manager.Comment("reaching state \'S154\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate/STATUS_DOWNGRADE_DETECTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_DOWNGRADE_DETECTED, temp4, "return of NetrServerAuthenticate, state S154");
                this.Manager.Comment("reaching state \'S169\'");
                goto label0;
            }
            if ((temp7 == 2)) {
                this.Manager.Comment("reaching state \'S20\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S47\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S74\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp5;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp5 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S101\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp5, "return of NetrServerReqChallenge, state S101");
                this.Manager.Comment("reaching state \'S128\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp6;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate(PrimaryDc,InvalidAccount,WorkstationS" +
                        "ecureChannel,Client,True)\'");
                temp6 = this.INrpcServerAdapterInstance.NetrServerAuthenticate(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true);
                this.Manager.Checkpoint("MS-NRPC_R1404");
                this.Manager.Comment("reaching state \'S155\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate/STATUS_DOWNGRADE_DETECTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_DOWNGRADE_DETECTED, temp6, "return of NetrServerAuthenticate, state S155");
                this.Manager.Comment("reaching state \'S170\'");
                goto label0;
            }
            throw new InvalidOperationException("never reached");
        label0:
;
            this.Manager.EndTest();
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticateS0GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S1");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticateS0GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S1");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticateS0GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S1");
        }
        #endregion
        
        #region Test Starting in S10
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_EstablishSecureChannel_NetrServerAuthenticateS10() {
            this.Manager.BeginTest("Test_EstablishSecureChannel_NetrServerAuthenticateS10");
            this.Manager.Comment("reaching state \'S10\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp8;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp8);
            this.Manager.AddReturn(GetPlatformInfo, null, temp8);
            this.Manager.Comment("reaching state \'S11\'");
            int temp13 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticateS10GetPlatformChecker)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticateS10GetPlatformChecker1)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticateS10GetPlatformChecker2)));
            if ((temp13 == 0)) {
                this.Manager.Comment("reaching state \'S33\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S60\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S87\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp9;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp9 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S114\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp9, "return of NetrServerReqChallenge, state S114");
                this.Manager.Comment("reaching state \'S141\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp10;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate(NonDcServer,DomainMemberComputerAccou" +
                        "nt,WorkstationSecureChannel,Client,True)\'");
                temp10 = this.INrpcServerAdapterInstance.NetrServerAuthenticate(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R104907");
                this.Manager.Checkpoint("MS-NRPC_R1404");
                this.Manager.Comment("reaching state \'S164\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp10, "return of NetrServerAuthenticate, state S164");
                this.Manager.Comment("reaching state \'S179\'");
                goto label1;
            }
            if ((temp13 == 1)) {
                this.Manager.Comment("reaching state \'S34\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S61\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S88\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp11;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(NonDcServer,Client)\'");
                temp11 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103509");
                this.Manager.Checkpoint("MS-NRPC_R103341");
                this.Manager.Comment("reaching state \'S115\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp11, "return of NetrServerReqChallenge, state S115");
                this.Manager.Comment("reaching state \'S142\'");
                goto label1;
            }
            if ((temp13 == 2)) {
                this.Manager.Comment("reaching state \'S35\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S62\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S89\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp12;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(NonDcServer,Client)\'");
                temp12 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103509");
                this.Manager.Checkpoint("MS-NRPC_R103341");
                this.Manager.Comment("reaching state \'S116\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp12, "return of NetrServerReqChallenge, state S116");
                this.Manager.Comment("reaching state \'S143\'");
                goto label1;
            }
            throw new InvalidOperationException("never reached");
        label1:
;
            this.Manager.EndTest();
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticateS10GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S11");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticateS10GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S11");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticateS10GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S11");
        }
        #endregion
        
        #region Test Starting in S12
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_EstablishSecureChannel_NetrServerAuthenticateS12() {
            this.Manager.BeginTest("Test_EstablishSecureChannel_NetrServerAuthenticateS12");
            this.Manager.Comment("reaching state \'S12\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp14;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp14);
            this.Manager.AddReturn(GetPlatformInfo, null, temp14);
            this.Manager.Comment("reaching state \'S13\'");
            int temp19 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticateS12GetPlatformChecker)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticateS12GetPlatformChecker1)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticateS12GetPlatformChecker2)));
            if ((temp19 == 0)) {
                this.Manager.Comment("reaching state \'S36\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S63\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S90\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp15;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp15 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S117\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp15, "return of NetrServerReqChallenge, state S117");
                this.Manager.Comment("reaching state \'S144\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp16;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate(PrimaryDc,DomainMemberComputerAccount" +
                        ",WorkstationSecureChannel,Client,False)\'");
                temp16 = this.INrpcServerAdapterInstance.NetrServerAuthenticate(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, false);
                this.Manager.Checkpoint("MS-NRPC_R1404");
                this.Manager.Comment("reaching state \'S165\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate/STATUS_DOWNGRADE_DETECTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_DOWNGRADE_DETECTED, temp16, "return of NetrServerAuthenticate, state S165");
                this.Manager.Comment("reaching state \'S180\'");
                goto label2;
            }
            if ((temp19 == 1)) {
                this.Manager.Comment("reaching state \'S37\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S64\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S91\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp17;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(NonDcServer,Client)\'");
                temp17 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103509");
                this.Manager.Checkpoint("MS-NRPC_R103341");
                this.Manager.Comment("reaching state \'S118\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp17, "return of NetrServerReqChallenge, state S118");
                this.Manager.Comment("reaching state \'S145\'");
                goto label2;
            }
            if ((temp19 == 2)) {
                this.Manager.Comment("reaching state \'S38\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S65\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S92\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp18;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(NonDcServer,Client)\'");
                temp18 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103509");
                this.Manager.Checkpoint("MS-NRPC_R103341");
                this.Manager.Comment("reaching state \'S119\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp18, "return of NetrServerReqChallenge, state S119");
                this.Manager.Comment("reaching state \'S146\'");
                goto label2;
            }
            throw new InvalidOperationException("never reached");
        label2:
;
            this.Manager.EndTest();
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticateS12GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S13");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticateS12GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S13");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticateS12GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S13");
        }
        #endregion
        
        #region Test Starting in S14
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_EstablishSecureChannel_NetrServerAuthenticateS14() {
            this.Manager.BeginTest("Test_EstablishSecureChannel_NetrServerAuthenticateS14");
            this.Manager.Comment("reaching state \'S14\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp20;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp20);
            this.Manager.AddReturn(GetPlatformInfo, null, temp20);
            this.Manager.Comment("reaching state \'S15\'");
            int temp25 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticateS14GetPlatformChecker)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticateS14GetPlatformChecker1)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticateS14GetPlatformChecker2)));
            if ((temp25 == 0)) {
                this.Manager.Comment("reaching state \'S39\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S66\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S93\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp21;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp21 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S120\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp21, "return of NetrServerReqChallenge, state S120");
                this.Manager.Comment("reaching state \'S147\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp22;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate(PrimaryDc,DomainMemberComputerAccount" +
                        ",WorkstationSecureChannel,Client,True)\'");
                temp22 = this.INrpcServerAdapterInstance.NetrServerAuthenticate(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true);
                this.Manager.Checkpoint("MS-NRPC_R1404");
                this.Manager.Comment("reaching state \'S166\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate/STATUS_DOWNGRADE_DETECTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_DOWNGRADE_DETECTED, temp22, "return of NetrServerAuthenticate, state S166");
                this.Manager.Comment("reaching state \'S181\'");
                goto label3;
            }
            if ((temp25 == 1)) {
                this.Manager.Comment("reaching state \'S40\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S67\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S94\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp23;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(NonDcServer,Client)\'");
                temp23 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103509");
                this.Manager.Checkpoint("MS-NRPC_R103341");
                this.Manager.Comment("reaching state \'S121\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp23, "return of NetrServerReqChallenge, state S121");
                this.Manager.Comment("reaching state \'S148\'");
                goto label3;
            }
            if ((temp25 == 2)) {
                this.Manager.Comment("reaching state \'S41\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S68\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S95\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp24;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(NonDcServer,Client)\'");
                temp24 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103509");
                this.Manager.Checkpoint("MS-NRPC_R103341");
                this.Manager.Comment("reaching state \'S122\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp24, "return of NetrServerReqChallenge, state S122");
                this.Manager.Comment("reaching state \'S149\'");
                goto label3;
            }
            throw new InvalidOperationException("never reached");
        label3:
;
            this.Manager.EndTest();
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticateS14GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S15");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticateS14GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S15");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticateS14GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S15");
        }
        #endregion
        
        #region Test Starting in S16
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_EstablishSecureChannel_NetrServerAuthenticateS16() {
            this.Manager.BeginTest("Test_EstablishSecureChannel_NetrServerAuthenticateS16");
            this.Manager.Comment("reaching state \'S16\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp26;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp26);
            this.Manager.AddReturn(GetPlatformInfo, null, temp26);
            this.Manager.Comment("reaching state \'S17\'");
            int temp31 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticateS16GetPlatformChecker)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticateS16GetPlatformChecker1)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticateS16GetPlatformChecker2)));
            if ((temp31 == 0)) {
                this.Manager.Comment("reaching state \'S42\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S69\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S96\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp27;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(NonDcServer,Client)\'");
                temp27 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103509");
                this.Manager.Checkpoint("MS-NRPC_R103341");
                this.Manager.Comment("reaching state \'S123\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp27, "return of NetrServerReqChallenge, state S123");
                this.Manager.Comment("reaching state \'S150\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp28;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate(NonDcServer,DomainMemberComputerAccou" +
                        "nt,WorkstationSecureChannel,Client,True)\'");
                temp28 = this.INrpcServerAdapterInstance.NetrServerAuthenticate(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R104907");
                this.Manager.Checkpoint("MS-NRPC_R1404");
                this.Manager.Comment("reaching state \'S167\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp28, "return of NetrServerAuthenticate, state S167");
                this.Manager.Comment("reaching state \'S182\'");
                goto label4;
            }
            if ((temp31 == 1)) {
                this.Manager.Comment("reaching state \'S43\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S70\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S97\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp29;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(NonDcServer,Client)\'");
                temp29 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103509");
                this.Manager.Checkpoint("MS-NRPC_R103341");
                this.Manager.Comment("reaching state \'S124\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp29, "return of NetrServerReqChallenge, state S124");
                this.Manager.Comment("reaching state \'S151\'");
                goto label4;
            }
            if ((temp31 == 2)) {
                this.Manager.Comment("reaching state \'S44\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S71\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S98\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp30;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(NonDcServer,Client)\'");
                temp30 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103509");
                this.Manager.Checkpoint("MS-NRPC_R103341");
                this.Manager.Comment("reaching state \'S125\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp30, "return of NetrServerReqChallenge, state S125");
                this.Manager.Comment("reaching state \'S152\'");
                goto label4;
            }
            throw new InvalidOperationException("never reached");
        label4:
;
            this.Manager.EndTest();
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticateS16GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S17");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticateS16GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S17");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticateS16GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S17");
        }
        #endregion
        
        #region Test Starting in S2
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_EstablishSecureChannel_NetrServerAuthenticateS2() {
            this.Manager.BeginTest("Test_EstablishSecureChannel_NetrServerAuthenticateS2");
            this.Manager.Comment("reaching state \'S2\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp32;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp32);
            this.Manager.AddReturn(GetPlatformInfo, null, temp32);
            this.Manager.Comment("reaching state \'S3\'");
            int temp38 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticateS2GetPlatformChecker)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticateS2GetPlatformChecker1)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticateS2GetPlatformChecker2)));
            if ((temp38 == 0)) {
                this.Manager.Comment("reaching state \'S21\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S48\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S75\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp33;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp33 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S102\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp33, "return of NetrServerReqChallenge, state S102");
                this.Manager.Comment("reaching state \'S129\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp34;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate(NonDcServer,DomainMemberComputerAccou" +
                        "nt,WorkstationSecureChannel,Client,True)\'");
                temp34 = this.INrpcServerAdapterInstance.NetrServerAuthenticate(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R104907");
                this.Manager.Checkpoint("MS-NRPC_R1404");
                this.Manager.Comment("reaching state \'S156\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp34, "return of NetrServerAuthenticate, state S156");
                this.Manager.Comment("reaching state \'S171\'");
                goto label5;
            }
            if ((temp38 == 1)) {
                this.Manager.Comment("reaching state \'S22\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S49\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S76\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp35;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp35 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S103\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp35, "return of NetrServerReqChallenge, state S103");
                this.Manager.Comment("reaching state \'S130\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp36;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate(PrimaryDc,DomainMemberComputerAccount" +
                        ",WorkstationSecureChannel,Client,False)\'");
                temp36 = this.INrpcServerAdapterInstance.NetrServerAuthenticate(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, false);
                this.Manager.Checkpoint("MS-NRPC_R1404");
                this.Manager.Comment("reaching state \'S157\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate/STATUS_DOWNGRADE_DETECTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_DOWNGRADE_DETECTED, temp36, "return of NetrServerAuthenticate, state S157");
                this.Manager.Comment("reaching state \'S172\'");
                goto label5;
            }
            if ((temp38 == 2)) {
                this.Manager.Comment("reaching state \'S23\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S50\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S77\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp37;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(NonDcServer,Client)\'");
                temp37 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103509");
                this.Manager.Checkpoint("MS-NRPC_R103341");
                this.Manager.Comment("reaching state \'S104\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp37, "return of NetrServerReqChallenge, state S104");
                this.Manager.Comment("reaching state \'S131\'");
                goto label5;
            }
            throw new InvalidOperationException("never reached");
        label5:
;
            this.Manager.EndTest();
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticateS2GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S3");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticateS2GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S3");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticateS2GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S3");
        }
        #endregion
        
        #region Test Starting in S4
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_EstablishSecureChannel_NetrServerAuthenticateS4() {
            this.Manager.BeginTest("Test_EstablishSecureChannel_NetrServerAuthenticateS4");
            this.Manager.Comment("reaching state \'S4\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp39;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp39);
            this.Manager.AddReturn(GetPlatformInfo, null, temp39);
            this.Manager.Comment("reaching state \'S5\'");
            int temp45 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticateS4GetPlatformChecker)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticateS4GetPlatformChecker1)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticateS4GetPlatformChecker2)));
            if ((temp45 == 0)) {
                this.Manager.Comment("reaching state \'S24\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S51\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S78\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp40;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp40 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S105\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp40, "return of NetrServerReqChallenge, state S105");
                this.Manager.Comment("reaching state \'S132\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp41;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate(PrimaryDc,DomainMemberComputerAccount" +
                        ",WorkstationSecureChannel,Client,False)\'");
                temp41 = this.INrpcServerAdapterInstance.NetrServerAuthenticate(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, false);
                this.Manager.Checkpoint("MS-NRPC_R1404");
                this.Manager.Comment("reaching state \'S158\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate/STATUS_DOWNGRADE_DETECTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_DOWNGRADE_DETECTED, temp41, "return of NetrServerAuthenticate, state S158");
                this.Manager.Comment("reaching state \'S173\'");
                goto label6;
            }
            if ((temp45 == 1)) {
                this.Manager.Comment("reaching state \'S25\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S52\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S79\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp42;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp42 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S106\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp42, "return of NetrServerReqChallenge, state S106");
                this.Manager.Comment("reaching state \'S133\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp43;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate(NonDcServer,DomainMemberComputerAccou" +
                        "nt,WorkstationSecureChannel,Client,True)\'");
                temp43 = this.INrpcServerAdapterInstance.NetrServerAuthenticate(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R104907");
                this.Manager.Checkpoint("MS-NRPC_R1404");
                this.Manager.Comment("reaching state \'S159\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp43, "return of NetrServerAuthenticate, state S159");
                this.Manager.Comment("reaching state \'S174\'");
                goto label6;
            }
            if ((temp45 == 2)) {
                this.Manager.Comment("reaching state \'S26\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S53\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S80\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp44;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(NonDcServer,Client)\'");
                temp44 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103509");
                this.Manager.Checkpoint("MS-NRPC_R103341");
                this.Manager.Comment("reaching state \'S107\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp44, "return of NetrServerReqChallenge, state S107");
                this.Manager.Comment("reaching state \'S134\'");
                goto label6;
            }
            throw new InvalidOperationException("never reached");
        label6:
;
            this.Manager.EndTest();
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticateS4GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S5");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticateS4GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S5");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticateS4GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S5");
        }
        #endregion
        
        #region Test Starting in S6
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_EstablishSecureChannel_NetrServerAuthenticateS6() {
            this.Manager.BeginTest("Test_EstablishSecureChannel_NetrServerAuthenticateS6");
            this.Manager.Comment("reaching state \'S6\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp46;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp46);
            this.Manager.AddReturn(GetPlatformInfo, null, temp46);
            this.Manager.Comment("reaching state \'S7\'");
            int temp52 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticateS6GetPlatformChecker)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticateS6GetPlatformChecker1)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticateS6GetPlatformChecker2)));
            if ((temp52 == 0)) {
                this.Manager.Comment("reaching state \'S27\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S54\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S81\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp47;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp47 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S108\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp47, "return of NetrServerReqChallenge, state S108");
                this.Manager.Comment("reaching state \'S135\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp48;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate(PrimaryDc,DomainMemberComputerAccount" +
                        ",WorkstationSecureChannel,Client,True)\'");
                temp48 = this.INrpcServerAdapterInstance.NetrServerAuthenticate(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true);
                this.Manager.Checkpoint("MS-NRPC_R1404");
                this.Manager.Comment("reaching state \'S160\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate/STATUS_DOWNGRADE_DETECTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_DOWNGRADE_DETECTED, temp48, "return of NetrServerAuthenticate, state S160");
                this.Manager.Comment("reaching state \'S175\'");
                goto label7;
            }
            if ((temp52 == 1)) {
                this.Manager.Comment("reaching state \'S28\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S55\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S82\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp49;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp49 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S109\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp49, "return of NetrServerReqChallenge, state S109");
                this.Manager.Comment("reaching state \'S136\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp50;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate(PrimaryDc,DomainMemberComputerAccount" +
                        ",WorkstationSecureChannel,Client,True)\'");
                temp50 = this.INrpcServerAdapterInstance.NetrServerAuthenticate(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true);
                this.Manager.Checkpoint("MS-NRPC_R1404");
                this.Manager.Comment("reaching state \'S161\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate/STATUS_DOWNGRADE_DETECTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_DOWNGRADE_DETECTED, temp50, "return of NetrServerAuthenticate, state S161");
                this.Manager.Comment("reaching state \'S176\'");
                goto label7;
            }
            if ((temp52 == 2)) {
                this.Manager.Comment("reaching state \'S29\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S56\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S83\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp51;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(NonDcServer,Client)\'");
                temp51 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103509");
                this.Manager.Checkpoint("MS-NRPC_R103341");
                this.Manager.Comment("reaching state \'S110\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp51, "return of NetrServerReqChallenge, state S110");
                this.Manager.Comment("reaching state \'S137\'");
                goto label7;
            }
            throw new InvalidOperationException("never reached");
        label7:
;
            this.Manager.EndTest();
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticateS6GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S7");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticateS6GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S7");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticateS6GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S7");
        }
        #endregion
        
        #region Test Starting in S8
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_EstablishSecureChannel_NetrServerAuthenticateS8() {
            this.Manager.BeginTest("Test_EstablishSecureChannel_NetrServerAuthenticateS8");
            this.Manager.Comment("reaching state \'S8\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp53;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp53);
            this.Manager.AddReturn(GetPlatformInfo, null, temp53);
            this.Manager.Comment("reaching state \'S9\'");
            int temp59 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticateS8GetPlatformChecker)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticateS8GetPlatformChecker1)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticateS8GetPlatformChecker2)));
            if ((temp59 == 0)) {
                this.Manager.Comment("reaching state \'S30\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S57\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S84\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp54;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(NonDcServer,Client)\'");
                temp54 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103509");
                this.Manager.Checkpoint("MS-NRPC_R103341");
                this.Manager.Comment("reaching state \'S111\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp54, "return of NetrServerReqChallenge, state S111");
                this.Manager.Comment("reaching state \'S138\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp55;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate(NonDcServer,DomainMemberComputerAccou" +
                        "nt,WorkstationSecureChannel,Client,True)\'");
                temp55 = this.INrpcServerAdapterInstance.NetrServerAuthenticate(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R104907");
                this.Manager.Checkpoint("MS-NRPC_R1404");
                this.Manager.Comment("reaching state \'S162\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp55, "return of NetrServerAuthenticate, state S162");
                this.Manager.Comment("reaching state \'S177\'");
                goto label8;
            }
            if ((temp59 == 1)) {
                this.Manager.Comment("reaching state \'S31\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S58\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S85\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp56;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(NonDcServer,Client)\'");
                temp56 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103509");
                this.Manager.Checkpoint("MS-NRPC_R103341");
                this.Manager.Comment("reaching state \'S112\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp56, "return of NetrServerReqChallenge, state S112");
                this.Manager.Comment("reaching state \'S139\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp57;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate(NonDcServer,DomainMemberComputerAccou" +
                        "nt,WorkstationSecureChannel,Client,True)\'");
                temp57 = this.INrpcServerAdapterInstance.NetrServerAuthenticate(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R104907");
                this.Manager.Checkpoint("MS-NRPC_R1404");
                this.Manager.Comment("reaching state \'S163\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp57, "return of NetrServerAuthenticate, state S163");
                this.Manager.Comment("reaching state \'S178\'");
                goto label8;
            }
            if ((temp59 == 2)) {
                this.Manager.Comment("reaching state \'S32\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S59\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S86\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp58;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(NonDcServer,Client)\'");
                temp58 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103509");
                this.Manager.Checkpoint("MS-NRPC_R103341");
                this.Manager.Comment("reaching state \'S113\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp58, "return of NetrServerReqChallenge, state S113");
                this.Manager.Comment("reaching state \'S140\'");
                goto label8;
            }
            throw new InvalidOperationException("never reached");
        label8:
;
            this.Manager.EndTest();
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticateS8GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S9");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticateS8GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S9");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticateS8GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S9");
        }
        #endregion
    }
}
