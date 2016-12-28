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
    public partial class Test_EstablishSecureChannel_NetrServerAuthenticate3 : PtfTestClassBase {
        
        public Test_EstablishSecureChannel_NetrServerAuthenticate3() {
            this.SetSwitch("ProceedControlTimeout", "100");
            this.SetSwitch("QuiescenceTimeout", "30000");
        }
        
        #region Expect Delegates
        public delegate void GetPlatformDelegate1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform);
        
        public delegate void NetrServerReqChallengeDelegate1(Microsoft.Protocols.TestSuites.Nrpc.HRESULT @return);
        
        public delegate void NetrServerAuthenticate3Delegate1(Microsoft.Protocols.TestSuites.Nrpc.HRESULT @return);
        #endregion
        
        #region Event Metadata
        static System.Reflection.MethodBase GetPlatformInfo = TestManagerHelpers.GetMethodInfo(typeof(Microsoft.Protocols.TestSuites.Nrpc.INrpcServerAdapter), "GetPlatform", typeof(Microsoft.Protocols.TestSuites.Nrpc.PlatformType).MakeByRefType());
        
        static System.Reflection.MethodBase NetrServerAuthenticate3Info = TestManagerHelpers.GetMethodInfo(typeof(Microsoft.Protocols.TestSuites.Nrpc.INrpcServerAdapter), "NetrServerAuthenticate3", typeof(Microsoft.Protocols.TestSuites.Nrpc.ComputerType), typeof(Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType), typeof(Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE), typeof(Microsoft.Protocols.TestSuites.Nrpc.ComputerType), typeof(bool), typeof(uint));
        
        static System.Reflection.MethodBase NetrServerReqChallengeInfo = TestManagerHelpers.GetMethodInfo(typeof(Microsoft.Protocols.TestSuites.Nrpc.INrpcServerAdapter), "NetrServerReqChallenge", typeof(Microsoft.Protocols.TestSuites.Nrpc.ComputerType), typeof(Microsoft.Protocols.TestSuites.Nrpc.ComputerType));
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
        public void Test_EstablishSecureChannel_NetrServerAuthenticate3S0() {
            this.Manager.BeginTest("Test_EstablishSecureChannel_NetrServerAuthenticate3S0");
            this.Manager.Comment("reaching state \'S0\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp0;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp0);
            this.Manager.AddReturn(GetPlatformInfo, null, temp0);
            this.Manager.Comment("reaching state \'S1\'");
            int temp11 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate3.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate3S0GetPlatformChecker)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate3.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate3S0GetPlatformChecker1)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate3.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate3S0GetPlatformChecker2)));
            if ((temp11 == 0)) {
                this.Manager.Comment("reaching state \'S120\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S300\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S480\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp1;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp1 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S660\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp1, "return of NetrServerReqChallenge, state S660");
                this.Manager.Comment("reaching state \'S840\'");
                this.Manager.Comment("executing step \'call ConfigServerRejectMD5Client(True)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServerRejectMD5Client(true);
                this.Manager.Comment("reaching state \'S1020\'");
                this.Manager.Comment("checking step \'return ConfigServerRejectMD5Client\'");
                this.Manager.Comment("reaching state \'S1112\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp2;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16388)\'");
                temp2 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16388u);
                this.Manager.Checkpoint("MS-NRPC_R203371");
                this.Manager.Checkpoint("MS-NRPC_R103456");
                this.Manager.Comment("reaching state \'S1204\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/STATUS_DOWNGRADE_DETECTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_DOWNGRADE_DETECTED, temp2, "return of NetrServerAuthenticate3, state S1204");
                this.Manager.Comment("reaching state \'S1293\'");
                this.Manager.Comment("executing step \'call ConfigServerRejectMD5Client(False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServerRejectMD5Client(false);
                this.Manager.Comment("reaching state \'S1382\'");
                this.Manager.Comment("checking step \'return ConfigServerRejectMD5Client\'");
                this.Manager.Comment("reaching state \'S1441\'");
                goto label0;
            }
            if ((temp11 == 1)) {
                this.Manager.Comment("reaching state \'S121\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S301\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S481\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp3;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp3 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S661\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp3, "return of NetrServerReqChallenge, state S661");
                this.Manager.Comment("reaching state \'S841\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp4;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,InvalidAccount,Workstation" +
                        "SecureChannel,Client,True,16388)\'");
                temp4 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16388u);
                this.Manager.Checkpoint("MS-NRPC_R103373");
                this.Manager.Checkpoint("MS-NRPC_R103456");
                this.Manager.Comment("reaching state \'S1021\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/STATUS_NO_TRUST_SAM_ACCOUNT\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NO_TRUST_SAM_ACCOUNT, temp4, "return of NetrServerAuthenticate3, state S1021");
                this.Manager.Comment("reaching state \'S1113\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp5;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp5 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S1205\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp5, "return of NetrServerReqChallenge, state S1205");
                this.Manager.Comment("reaching state \'S1294\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp6;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,InvalidAccount,Workstation" +
                        "SecureChannel,Client,True,16388)\'");
                temp6 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16388u);
                this.Manager.Checkpoint("MS-NRPC_R103373");
                this.Manager.Checkpoint("MS-NRPC_R103456");
                this.Manager.Comment("reaching state \'S1383\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/STATUS_NO_TRUST_SAM_ACCOUNT\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NO_TRUST_SAM_ACCOUNT, temp6, "return of NetrServerAuthenticate3, state S1383");
                this.Manager.Comment("reaching state \'S1442\'");
                goto label0;
            }
            if ((temp11 == 2)) {
                this.Manager.Comment("reaching state \'S122\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S302\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S482\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp7;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp7 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S662\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp7, "return of NetrServerReqChallenge, state S662");
                this.Manager.Comment("reaching state \'S842\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp8;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,InvalidAccount,Workstation" +
                        "SecureChannel,Client,True,16388)\'");
                temp8 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16388u);
                this.Manager.Checkpoint("MS-NRPC_R103373");
                this.Manager.Checkpoint("MS-NRPC_R103456");
                this.Manager.Comment("reaching state \'S1022\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/STATUS_NO_TRUST_SAM_ACCOUNT\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NO_TRUST_SAM_ACCOUNT, temp8, "return of NetrServerAuthenticate3, state S1022");
                this.Manager.Comment("reaching state \'S1114\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp9;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp9 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S1206\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp9, "return of NetrServerReqChallenge, state S1206");
                this.Manager.Comment("reaching state \'S1295\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp10;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,InvalidAccount,Workstation" +
                        "SecureChannel,Client,True,16388)\'");
                temp10 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16388u);
                this.Manager.Checkpoint("MS-NRPC_R103373");
                this.Manager.Checkpoint("MS-NRPC_R103456");
                this.Manager.Comment("reaching state \'S1384\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/STATUS_NO_TRUST_SAM_ACCOUNT\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NO_TRUST_SAM_ACCOUNT, temp10, "return of NetrServerAuthenticate3, state S1384");
                this.Manager.Comment("reaching state \'S1443\'");
                goto label0;
            }
            throw new InvalidOperationException("never reached");
        label0:
;
            this.Manager.EndTest();
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate3S0GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S1");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate3S0GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S1");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate3S0GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S1");
        }
        #endregion
        
        #region Test Starting in S10
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_EstablishSecureChannel_NetrServerAuthenticate3S10() {
            this.Manager.BeginTest("Test_EstablishSecureChannel_NetrServerAuthenticate3S10");
            this.Manager.Comment("reaching state \'S10\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp12;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp12);
            this.Manager.AddReturn(GetPlatformInfo, null, temp12);
            this.Manager.Comment("reaching state \'S11\'");
            int temp22 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate3.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate3S10GetPlatformChecker)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate3.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate3S10GetPlatformChecker1)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate3.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate3S10GetPlatformChecker2)));
            if ((temp22 == 0)) {
                this.Manager.Comment("reaching state \'S135\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S315\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S495\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp13;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp13 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S675\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp13, "return of NetrServerReqChallenge, state S675");
                this.Manager.Comment("reaching state \'S855\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp14;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,12)\'");
                temp14 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 12u);
                this.Manager.Checkpoint("MS-NRPC_R103371");
                this.Manager.Checkpoint("MS-NRPC_R103456");
                this.Manager.Comment("reaching state \'S1031\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/STATUS_DOWNGRADE_DETECTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_DOWNGRADE_DETECTED, temp14, "return of NetrServerAuthenticate3, state S1031");
                this.Manager.Comment("reaching state \'S1123\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp15;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp15 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S1215\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp15, "return of NetrServerReqChallenge, state S1215");
                this.Manager.Comment("reaching state \'S1304\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp16;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,4)\'");
                temp16 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 4u);
                this.Manager.Checkpoint("MS-NRPC_R103371");
                this.Manager.Checkpoint("MS-NRPC_R890");
                this.Manager.Checkpoint("MS-NRPC_R103456");
                this.Manager.AddReturn(NetrServerAuthenticate3Info, null, temp16);
                Test_EstablishSecureChannel_NetrServerAuthenticate3S1385();
                goto label1;
            }
            if ((temp22 == 1)) {
                this.Manager.Comment("reaching state \'S136\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S316\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S496\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp17;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp17 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S676\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp17, "return of NetrServerReqChallenge, state S676");
                this.Manager.Comment("reaching state \'S856\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp18;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,12)\'");
                temp18 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 12u);
                this.Manager.Checkpoint("MS-NRPC_R103371");
                this.Manager.Checkpoint("MS-NRPC_R103456");
                this.Manager.Comment("reaching state \'S1032\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/STATUS_DOWNGRADE_DETECTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_DOWNGRADE_DETECTED, temp18, "return of NetrServerAuthenticate3, state S1032");
                this.Manager.Comment("reaching state \'S1124\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp19;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp19 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S1216\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp19, "return of NetrServerReqChallenge, state S1216");
                this.Manager.Comment("reaching state \'S1305\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp20;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,4)\'");
                temp20 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 4u);
                this.Manager.Checkpoint("MS-NRPC_R103371");
                this.Manager.Checkpoint("MS-NRPC_R890");
                this.Manager.Checkpoint("MS-NRPC_R103456");
                this.Manager.AddReturn(NetrServerAuthenticate3Info, null, temp20);
                Test_EstablishSecureChannel_NetrServerAuthenticate3S1386();
                goto label1;
            }
            if ((temp22 == 2)) {
                this.Manager.Comment("reaching state \'S137\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S317\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S497\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp21;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(NonDcServer,Client)\'");
                temp21 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103509");
                this.Manager.Checkpoint("MS-NRPC_R103341");
                this.Manager.Comment("reaching state \'S677\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp21, "return of NetrServerReqChallenge, state S677");
                this.Manager.Comment("reaching state \'S857\'");
                goto label1;
            }
            throw new InvalidOperationException("never reached");
        label1:
;
            this.Manager.EndTest();
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate3S10GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S11");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate3S1385() {
            this.Manager.Comment("reaching state \'S1385\'");
            this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate3.NetrServerAuthenticate3Info, null, new NetrServerAuthenticate3Delegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate3S10NetrServerAuthenticate3Checker)));
            this.Manager.Comment("reaching state \'S1444\'");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate3S10NetrServerAuthenticate3Checker(Microsoft.Protocols.TestSuites.Nrpc.HRESULT @return) {
            this.Manager.Comment("checking step \'return NetrServerAuthenticate3/STATUS_DOWNGRADE_DETECTED\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_DOWNGRADE_DETECTED, @return, "return of NetrServerAuthenticate3, state S1385");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate3S10GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S11");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate3S1386() {
            this.Manager.Comment("reaching state \'S1386\'");
            this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate3.NetrServerAuthenticate3Info, null, new NetrServerAuthenticate3Delegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate3S10NetrServerAuthenticate3Checker1)));
            this.Manager.Comment("reaching state \'S1445\'");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate3S10NetrServerAuthenticate3Checker1(Microsoft.Protocols.TestSuites.Nrpc.HRESULT @return) {
            this.Manager.Comment("checking step \'return NetrServerAuthenticate3/STATUS_DOWNGRADE_DETECTED\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_DOWNGRADE_DETECTED, @return, "return of NetrServerAuthenticate3, state S1386");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate3S10GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S11");
        }
        #endregion
        
        #region Test Starting in S100
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_EstablishSecureChannel_NetrServerAuthenticate3S100() {
            this.Manager.BeginTest("Test_EstablishSecureChannel_NetrServerAuthenticate3S100");
            this.Manager.Comment("reaching state \'S100\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp23;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp23);
            this.Manager.AddReturn(GetPlatformInfo, null, temp23);
            this.Manager.Comment("reaching state \'S101\'");
            int temp30 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate3.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate3S100GetPlatformChecker)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate3.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate3S100GetPlatformChecker1)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate3.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate3S100GetPlatformChecker2)));
            if ((temp30 == 0)) {
                this.Manager.Comment("reaching state \'S270\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S450\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S630\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp24;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp24 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S810\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp24, "return of NetrServerReqChallenge, state S810");
                this.Manager.Comment("reaching state \'S990\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp25;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,524292)\'");
                temp25 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 524292u);
                this.Manager.Checkpoint("MS-NRPC_R103371");
                this.Manager.Checkpoint("MS-NRPC_R103456");
                this.Manager.Comment("reaching state \'S1102\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/STATUS_DOWNGRADE_DETECTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_DOWNGRADE_DETECTED, temp25, "return of NetrServerAuthenticate3, state S1102");
                this.Manager.Comment("reaching state \'S1194\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp26;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp26 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S1284\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp26, "return of NetrServerReqChallenge, state S1284");
                this.Manager.Comment("reaching state \'S1373\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp27;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,524292)\'");
                temp27 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 524292u);
                this.Manager.Checkpoint("MS-NRPC_R103371");
                this.Manager.Checkpoint("MS-NRPC_R103456");
                this.Manager.Comment("reaching state \'S1433\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/STATUS_DOWNGRADE_DETECTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_DOWNGRADE_DETECTED, temp27, "return of NetrServerAuthenticate3, state S1433");
                this.Manager.Comment("reaching state \'S1492\'");
                goto label2;
            }
            if ((temp30 == 1)) {
                this.Manager.Comment("reaching state \'S271\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S451\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S631\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp28;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(NonDcServer,Client)\'");
                temp28 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103509");
                this.Manager.Checkpoint("MS-NRPC_R103341");
                this.Manager.Comment("reaching state \'S811\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp28, "return of NetrServerReqChallenge, state S811");
                this.Manager.Comment("reaching state \'S991\'");
                goto label2;
            }
            if ((temp30 == 2)) {
                this.Manager.Comment("reaching state \'S272\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S452\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S632\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp29;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(NonDcServer,Client)\'");
                temp29 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103509");
                this.Manager.Checkpoint("MS-NRPC_R103341");
                this.Manager.Comment("reaching state \'S812\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp29, "return of NetrServerReqChallenge, state S812");
                this.Manager.Comment("reaching state \'S992\'");
                goto label2;
            }
            throw new InvalidOperationException("never reached");
        label2:
;
            this.Manager.EndTest();
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate3S100GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S101");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate3S100GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S101");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate3S100GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S101");
        }
        #endregion
        
        #region Test Starting in S102
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_EstablishSecureChannel_NetrServerAuthenticate3S102() {
            this.Manager.BeginTest("Test_EstablishSecureChannel_NetrServerAuthenticate3S102");
            this.Manager.Comment("reaching state \'S102\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp31;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp31);
            this.Manager.AddReturn(GetPlatformInfo, null, temp31);
            this.Manager.Comment("reaching state \'S103\'");
            int temp38 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate3.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate3S102GetPlatformChecker)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate3.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate3S102GetPlatformChecker1)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate3.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate3S102GetPlatformChecker2)));
            if ((temp38 == 0)) {
                this.Manager.Comment("reaching state \'S273\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S453\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S633\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp32;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp32 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S813\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp32, "return of NetrServerReqChallenge, state S813");
                this.Manager.Comment("reaching state \'S993\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp33;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,1048580)\'");
                temp33 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 1048580u);
                this.Manager.Checkpoint("MS-NRPC_R103371");
                this.Manager.Checkpoint("MS-NRPC_R103456");
                this.Manager.Comment("reaching state \'S1103\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/STATUS_DOWNGRADE_DETECTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_DOWNGRADE_DETECTED, temp33, "return of NetrServerAuthenticate3, state S1103");
                this.Manager.Comment("reaching state \'S1195\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp34;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp34 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S1285\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp34, "return of NetrServerReqChallenge, state S1285");
                this.Manager.Comment("reaching state \'S1374\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp35;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,1048580)\'");
                temp35 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 1048580u);
                this.Manager.Checkpoint("MS-NRPC_R103371");
                this.Manager.Checkpoint("MS-NRPC_R103456");
                this.Manager.Comment("reaching state \'S1434\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/STATUS_DOWNGRADE_DETECTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_DOWNGRADE_DETECTED, temp35, "return of NetrServerAuthenticate3, state S1434");
                this.Manager.Comment("reaching state \'S1493\'");
                goto label3;
            }
            if ((temp38 == 1)) {
                this.Manager.Comment("reaching state \'S274\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S454\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S634\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp36;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(NonDcServer,Client)\'");
                temp36 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103509");
                this.Manager.Checkpoint("MS-NRPC_R103341");
                this.Manager.Comment("reaching state \'S814\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp36, "return of NetrServerReqChallenge, state S814");
                this.Manager.Comment("reaching state \'S994\'");
                goto label3;
            }
            if ((temp38 == 2)) {
                this.Manager.Comment("reaching state \'S275\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S455\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S635\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp37;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(NonDcServer,Client)\'");
                temp37 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103509");
                this.Manager.Checkpoint("MS-NRPC_R103341");
                this.Manager.Comment("reaching state \'S815\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp37, "return of NetrServerReqChallenge, state S815");
                this.Manager.Comment("reaching state \'S995\'");
                goto label3;
            }
            throw new InvalidOperationException("never reached");
        label3:
;
            this.Manager.EndTest();
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate3S102GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S103");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate3S102GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S103");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate3S102GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S103");
        }
        #endregion
        
        #region Test Starting in S104
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_EstablishSecureChannel_NetrServerAuthenticate3S104() {
            this.Manager.BeginTest("Test_EstablishSecureChannel_NetrServerAuthenticate3S104");
            this.Manager.Comment("reaching state \'S104\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp39;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp39);
            this.Manager.AddReturn(GetPlatformInfo, null, temp39);
            this.Manager.Comment("reaching state \'S105\'");
            int temp46 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate3.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate3S104GetPlatformChecker)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate3.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate3S104GetPlatformChecker1)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate3.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate3S104GetPlatformChecker2)));
            if ((temp46 == 0)) {
                this.Manager.Comment("reaching state \'S276\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S456\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S636\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp40;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp40 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S816\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp40, "return of NetrServerReqChallenge, state S816");
                this.Manager.Comment("reaching state \'S996\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp41;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,2097156)\'");
                temp41 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 2097156u);
                this.Manager.Checkpoint("MS-NRPC_R103371");
                this.Manager.Checkpoint("MS-NRPC_R103456");
                this.Manager.Comment("reaching state \'S1104\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/STATUS_DOWNGRADE_DETECTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_DOWNGRADE_DETECTED, temp41, "return of NetrServerAuthenticate3, state S1104");
                this.Manager.Comment("reaching state \'S1196\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp42;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp42 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S1286\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp42, "return of NetrServerReqChallenge, state S1286");
                this.Manager.Comment("reaching state \'S1375\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp43;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,2097156)\'");
                temp43 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 2097156u);
                this.Manager.Checkpoint("MS-NRPC_R103371");
                this.Manager.Checkpoint("MS-NRPC_R103456");
                this.Manager.Comment("reaching state \'S1435\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/STATUS_DOWNGRADE_DETECTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_DOWNGRADE_DETECTED, temp43, "return of NetrServerAuthenticate3, state S1435");
                this.Manager.Comment("reaching state \'S1494\'");
                goto label4;
            }
            if ((temp46 == 1)) {
                this.Manager.Comment("reaching state \'S277\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S457\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S637\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp44;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(NonDcServer,Client)\'");
                temp44 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103509");
                this.Manager.Checkpoint("MS-NRPC_R103341");
                this.Manager.Comment("reaching state \'S817\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp44, "return of NetrServerReqChallenge, state S817");
                this.Manager.Comment("reaching state \'S997\'");
                goto label4;
            }
            if ((temp46 == 2)) {
                this.Manager.Comment("reaching state \'S278\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S458\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S638\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp45;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(NonDcServer,Client)\'");
                temp45 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103509");
                this.Manager.Checkpoint("MS-NRPC_R103341");
                this.Manager.Comment("reaching state \'S818\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp45, "return of NetrServerReqChallenge, state S818");
                this.Manager.Comment("reaching state \'S998\'");
                goto label4;
            }
            throw new InvalidOperationException("never reached");
        label4:
;
            this.Manager.EndTest();
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate3S104GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S105");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate3S104GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S105");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate3S104GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S105");
        }
        #endregion
        
        #region Test Starting in S106
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_EstablishSecureChannel_NetrServerAuthenticate3S106() {
            this.Manager.BeginTest("Test_EstablishSecureChannel_NetrServerAuthenticate3S106");
            this.Manager.Comment("reaching state \'S106\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp47;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp47);
            this.Manager.AddReturn(GetPlatformInfo, null, temp47);
            this.Manager.Comment("reaching state \'S107\'");
            int temp54 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate3.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate3S106GetPlatformChecker)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate3.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate3S106GetPlatformChecker1)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate3.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate3S106GetPlatformChecker2)));
            if ((temp54 == 0)) {
                this.Manager.Comment("reaching state \'S279\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S459\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S639\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp48;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp48 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S819\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp48, "return of NetrServerReqChallenge, state S819");
                this.Manager.Comment("reaching state \'S999\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp49;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16777220)\'");
                temp49 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16777220u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S1105\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp49, "return of NetrServerAuthenticate3, state S1105");
                this.Manager.Comment("reaching state \'S1197\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp50;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp50 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S1287\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp50, "return of NetrServerReqChallenge, state S1287");
                this.Manager.Comment("reaching state \'S1376\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp51;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16777220)\'");
                temp51 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16777220u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S1436\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp51, "return of NetrServerAuthenticate3, state S1436");
                this.Manager.Comment("reaching state \'S1495\'");
                goto label5;
            }
            if ((temp54 == 1)) {
                this.Manager.Comment("reaching state \'S280\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S460\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S640\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp52;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(NonDcServer,Client)\'");
                temp52 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103509");
                this.Manager.Checkpoint("MS-NRPC_R103341");
                this.Manager.Comment("reaching state \'S820\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp52, "return of NetrServerReqChallenge, state S820");
                this.Manager.Comment("reaching state \'S1000\'");
                goto label5;
            }
            if ((temp54 == 2)) {
                this.Manager.Comment("reaching state \'S281\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S461\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S641\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp53;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(NonDcServer,Client)\'");
                temp53 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103509");
                this.Manager.Checkpoint("MS-NRPC_R103341");
                this.Manager.Comment("reaching state \'S821\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp53, "return of NetrServerReqChallenge, state S821");
                this.Manager.Comment("reaching state \'S1001\'");
                goto label5;
            }
            throw new InvalidOperationException("never reached");
        label5:
;
            this.Manager.EndTest();
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate3S106GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S107");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate3S106GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S107");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate3S106GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S107");
        }
        #endregion
        
        #region Test Starting in S108
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_EstablishSecureChannel_NetrServerAuthenticate3S108() {
            this.Manager.BeginTest("Test_EstablishSecureChannel_NetrServerAuthenticate3S108");
            this.Manager.Comment("reaching state \'S108\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp55;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp55);
            this.Manager.AddReturn(GetPlatformInfo, null, temp55);
            this.Manager.Comment("reaching state \'S109\'");
            int temp62 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate3.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate3S108GetPlatformChecker)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate3.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate3S108GetPlatformChecker1)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate3.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate3S108GetPlatformChecker2)));
            if ((temp62 == 0)) {
                this.Manager.Comment("reaching state \'S282\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S462\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S642\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp56;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp56 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S822\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp56, "return of NetrServerReqChallenge, state S822");
                this.Manager.Comment("reaching state \'S1002\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp57;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,1073741828)\'");
                temp57 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 1073741828u);
                this.Manager.Checkpoint("MS-NRPC_R103371");
                this.Manager.Checkpoint("MS-NRPC_R103456");
                this.Manager.Comment("reaching state \'S1106\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/STATUS_DOWNGRADE_DETECTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_DOWNGRADE_DETECTED, temp57, "return of NetrServerAuthenticate3, state S1106");
                this.Manager.Comment("reaching state \'S1198\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp58;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp58 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S1288\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp58, "return of NetrServerReqChallenge, state S1288");
                this.Manager.Comment("reaching state \'S1377\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp59;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,1073741828)\'");
                temp59 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 1073741828u);
                this.Manager.Checkpoint("MS-NRPC_R103371");
                this.Manager.Checkpoint("MS-NRPC_R103456");
                this.Manager.Comment("reaching state \'S1437\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/STATUS_DOWNGRADE_DETECTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_DOWNGRADE_DETECTED, temp59, "return of NetrServerAuthenticate3, state S1437");
                this.Manager.Comment("reaching state \'S1496\'");
                goto label6;
            }
            if ((temp62 == 1)) {
                this.Manager.Comment("reaching state \'S283\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S463\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S643\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp60;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(NonDcServer,Client)\'");
                temp60 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103509");
                this.Manager.Checkpoint("MS-NRPC_R103341");
                this.Manager.Comment("reaching state \'S823\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp60, "return of NetrServerReqChallenge, state S823");
                this.Manager.Comment("reaching state \'S1003\'");
                goto label6;
            }
            if ((temp62 == 2)) {
                this.Manager.Comment("reaching state \'S284\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S464\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S644\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp61;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(NonDcServer,Client)\'");
                temp61 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103509");
                this.Manager.Checkpoint("MS-NRPC_R103341");
                this.Manager.Comment("reaching state \'S824\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp61, "return of NetrServerReqChallenge, state S824");
                this.Manager.Comment("reaching state \'S1004\'");
                goto label6;
            }
            throw new InvalidOperationException("never reached");
        label6:
;
            this.Manager.EndTest();
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate3S108GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S109");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate3S108GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S109");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate3S108GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S109");
        }
        #endregion
        
        #region Test Starting in S110
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_EstablishSecureChannel_NetrServerAuthenticate3S110() {
            this.Manager.BeginTest("Test_EstablishSecureChannel_NetrServerAuthenticate3S110");
            this.Manager.Comment("reaching state \'S110\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp63;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp63);
            this.Manager.AddReturn(GetPlatformInfo, null, temp63);
            this.Manager.Comment("reaching state \'S111\'");
            int temp70 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate3.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate3S110GetPlatformChecker)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate3.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate3S110GetPlatformChecker1)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate3.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate3S110GetPlatformChecker2)));
            if ((temp70 == 0)) {
                this.Manager.Comment("reaching state \'S285\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S465\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S645\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp64;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp64 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S825\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp64, "return of NetrServerReqChallenge, state S825");
                this.Manager.Comment("reaching state \'S1005\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp65;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,536870916)\'");
                temp65 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 536870916u);
                this.Manager.Checkpoint("MS-NRPC_R103371");
                this.Manager.Checkpoint("MS-NRPC_R103456");
                this.Manager.Comment("reaching state \'S1107\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/STATUS_DOWNGRADE_DETECTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_DOWNGRADE_DETECTED, temp65, "return of NetrServerAuthenticate3, state S1107");
                this.Manager.Comment("reaching state \'S1199\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp66;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp66 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S1289\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp66, "return of NetrServerReqChallenge, state S1289");
                this.Manager.Comment("reaching state \'S1378\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp67;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,4)\'");
                temp67 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 4u);
                this.Manager.Checkpoint("MS-NRPC_R103371");
                this.Manager.Checkpoint("MS-NRPC_R894");
                this.Manager.Checkpoint("MS-NRPC_R103456");
                this.Manager.AddReturn(NetrServerAuthenticate3Info, null, temp67);
                Test_EstablishSecureChannel_NetrServerAuthenticate3S1422();
                goto label7;
            }
            if ((temp70 == 1)) {
                this.Manager.Comment("reaching state \'S286\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S466\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S646\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp68;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(NonDcServer,Client)\'");
                temp68 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103509");
                this.Manager.Checkpoint("MS-NRPC_R103341");
                this.Manager.Comment("reaching state \'S826\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp68, "return of NetrServerReqChallenge, state S826");
                this.Manager.Comment("reaching state \'S1006\'");
                goto label7;
            }
            if ((temp70 == 2)) {
                this.Manager.Comment("reaching state \'S287\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S467\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S647\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp69;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(NonDcServer,Client)\'");
                temp69 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103509");
                this.Manager.Checkpoint("MS-NRPC_R103341");
                this.Manager.Comment("reaching state \'S827\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp69, "return of NetrServerReqChallenge, state S827");
                this.Manager.Comment("reaching state \'S1007\'");
                goto label7;
            }
            throw new InvalidOperationException("never reached");
        label7:
;
            this.Manager.EndTest();
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate3S110GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S111");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate3S1422() {
            this.Manager.Comment("reaching state \'S1422\'");
            this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate3.NetrServerAuthenticate3Info, null, new NetrServerAuthenticate3Delegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate3S110NetrServerAuthenticate3Checker)));
            this.Manager.Comment("reaching state \'S1481\'");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate3S110NetrServerAuthenticate3Checker(Microsoft.Protocols.TestSuites.Nrpc.HRESULT @return) {
            this.Manager.Comment("checking step \'return NetrServerAuthenticate3/STATUS_DOWNGRADE_DETECTED\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_DOWNGRADE_DETECTED, @return, "return of NetrServerAuthenticate3, state S1422");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate3S110GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S111");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate3S110GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S111");
        }
        #endregion
        
        #region Test Starting in S112
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_EstablishSecureChannel_NetrServerAuthenticate3S112() {
            this.Manager.BeginTest("Test_EstablishSecureChannel_NetrServerAuthenticate3S112");
            this.Manager.Comment("reaching state \'S112\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp71;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp71);
            this.Manager.AddReturn(GetPlatformInfo, null, temp71);
            this.Manager.Comment("reaching state \'S113\'");
            int temp78 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate3.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate3S112GetPlatformChecker)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate3.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate3S112GetPlatformChecker1)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate3.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate3S112GetPlatformChecker2)));
            if ((temp78 == 0)) {
                this.Manager.Comment("reaching state \'S288\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S468\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S648\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp72;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp72 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S828\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp72, "return of NetrServerReqChallenge, state S828");
                this.Manager.Comment("reaching state \'S1008\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp73;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16388)\'");
                temp73 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16388u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S1108\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp73, "return of NetrServerAuthenticate3, state S1108");
                this.Manager.Comment("reaching state \'S1200\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp74;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp74 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S1290\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp74, "return of NetrServerReqChallenge, state S1290");
                this.Manager.Comment("reaching state \'S1379\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp75;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16388)\'");
                temp75 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16388u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S1438\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp75, "return of NetrServerAuthenticate3, state S1438");
                this.Manager.Comment("reaching state \'S1497\'");
                goto label8;
            }
            if ((temp78 == 1)) {
                this.Manager.Comment("reaching state \'S289\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S469\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S649\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp76;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(NonDcServer,Client)\'");
                temp76 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103509");
                this.Manager.Checkpoint("MS-NRPC_R103341");
                this.Manager.Comment("reaching state \'S829\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp76, "return of NetrServerReqChallenge, state S829");
                this.Manager.Comment("reaching state \'S1009\'");
                goto label8;
            }
            if ((temp78 == 2)) {
                this.Manager.Comment("reaching state \'S290\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S470\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S650\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp77;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(NonDcServer,Client)\'");
                temp77 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103509");
                this.Manager.Checkpoint("MS-NRPC_R103341");
                this.Manager.Comment("reaching state \'S830\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp77, "return of NetrServerReqChallenge, state S830");
                this.Manager.Comment("reaching state \'S1010\'");
                goto label8;
            }
            throw new InvalidOperationException("never reached");
        label8:
;
            this.Manager.EndTest();
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate3S112GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S113");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate3S112GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S113");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate3S112GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S113");
        }
        #endregion
        
        #region Test Starting in S114
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_EstablishSecureChannel_NetrServerAuthenticate3S114() {
            this.Manager.BeginTest("Test_EstablishSecureChannel_NetrServerAuthenticate3S114");
            this.Manager.Comment("reaching state \'S114\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp79;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp79);
            this.Manager.AddReturn(GetPlatformInfo, null, temp79);
            this.Manager.Comment("reaching state \'S115\'");
            int temp86 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate3.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate3S114GetPlatformChecker)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate3.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate3S114GetPlatformChecker1)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate3.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate3S114GetPlatformChecker2)));
            if ((temp86 == 0)) {
                this.Manager.Comment("reaching state \'S291\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S471\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S651\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp80;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp80 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S831\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp80, "return of NetrServerReqChallenge, state S831");
                this.Manager.Comment("reaching state \'S1011\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp81;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,False,16388)\'");
                temp81 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, false, 16388u);
                this.Manager.Checkpoint("MS-NRPC_R103376");
                this.Manager.Checkpoint("MS-NRPC_R103456");
                this.Manager.Comment("reaching state \'S1109\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/STATUS_ACCESS_DENIED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp81, "return of NetrServerAuthenticate3, state S1109");
                this.Manager.Comment("reaching state \'S1201\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp82;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp82 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S1291\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp82, "return of NetrServerReqChallenge, state S1291");
                this.Manager.Comment("reaching state \'S1380\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp83;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,False,16388)\'");
                temp83 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, false, 16388u);
                this.Manager.Checkpoint("MS-NRPC_R103376");
                this.Manager.Checkpoint("MS-NRPC_R103456");
                this.Manager.Comment("reaching state \'S1439\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/STATUS_ACCESS_DENIED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp83, "return of NetrServerAuthenticate3, state S1439");
                this.Manager.Comment("reaching state \'S1498\'");
                goto label9;
            }
            if ((temp86 == 1)) {
                this.Manager.Comment("reaching state \'S292\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S472\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S652\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp84;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(NonDcServer,Client)\'");
                temp84 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103509");
                this.Manager.Checkpoint("MS-NRPC_R103341");
                this.Manager.Comment("reaching state \'S832\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp84, "return of NetrServerReqChallenge, state S832");
                this.Manager.Comment("reaching state \'S1012\'");
                goto label9;
            }
            if ((temp86 == 2)) {
                this.Manager.Comment("reaching state \'S293\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S473\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S653\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp85;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(NonDcServer,Client)\'");
                temp85 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103509");
                this.Manager.Checkpoint("MS-NRPC_R103341");
                this.Manager.Comment("reaching state \'S833\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp85, "return of NetrServerReqChallenge, state S833");
                this.Manager.Comment("reaching state \'S1013\'");
                goto label9;
            }
            throw new InvalidOperationException("never reached");
        label9:
;
            this.Manager.EndTest();
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate3S114GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S115");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate3S114GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S115");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate3S114GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S115");
        }
        #endregion
        
        #region Test Starting in S116
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_EstablishSecureChannel_NetrServerAuthenticate3S116() {
            this.Manager.BeginTest("Test_EstablishSecureChannel_NetrServerAuthenticate3S116");
            this.Manager.Comment("reaching state \'S116\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp87;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp87);
            this.Manager.AddReturn(GetPlatformInfo, null, temp87);
            this.Manager.Comment("reaching state \'S117\'");
            int temp94 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate3.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate3S116GetPlatformChecker)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate3.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate3S116GetPlatformChecker1)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate3.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate3S116GetPlatformChecker2)));
            if ((temp94 == 0)) {
                this.Manager.Comment("reaching state \'S294\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S474\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S654\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp88;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(NonDcServer,Client)\'");
                temp88 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103509");
                this.Manager.Checkpoint("MS-NRPC_R103341");
                this.Manager.Comment("reaching state \'S834\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp88, "return of NetrServerReqChallenge, state S834");
                this.Manager.Comment("reaching state \'S1014\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp89;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(NonDcServer,DomainMemberComputerAcco" +
                        "unt,WorkstationSecureChannel,Client,True,16388)\'");
                temp89 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16388u);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103513");
                this.Manager.Checkpoint("MS-NRPC_R103456");
                this.Manager.Comment("reaching state \'S1110\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp89, "return of NetrServerAuthenticate3, state S1110");
                this.Manager.Comment("reaching state \'S1202\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp90;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp90 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.AddReturn(NetrServerReqChallengeInfo, null, temp90);
                Test_EstablishSecureChannel_NetrServerAuthenticate3S1265();
                goto label10;
            }
            if ((temp94 == 1)) {
                this.Manager.Comment("reaching state \'S295\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S475\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S655\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp92;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(NonDcServer,Client)\'");
                temp92 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103509");
                this.Manager.Checkpoint("MS-NRPC_R103341");
                this.Manager.Comment("reaching state \'S835\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp92, "return of NetrServerReqChallenge, state S835");
                this.Manager.Comment("reaching state \'S1015\'");
                goto label10;
            }
            if ((temp94 == 2)) {
                this.Manager.Comment("reaching state \'S296\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S476\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S656\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp93;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(NonDcServer,Client)\'");
                temp93 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103509");
                this.Manager.Checkpoint("MS-NRPC_R103341");
                this.Manager.Comment("reaching state \'S836\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp93, "return of NetrServerReqChallenge, state S836");
                this.Manager.Comment("reaching state \'S1016\'");
                goto label10;
            }
            throw new InvalidOperationException("never reached");
        label10:
;
            this.Manager.EndTest();
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate3S116GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S117");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate3S1265() {
            this.Manager.Comment("reaching state \'S1265\'");
            this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate3.NetrServerReqChallengeInfo, null, new NetrServerReqChallengeDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate3S116NetrServerReqChallengeChecker)));
            this.Manager.Comment("reaching state \'S1354\'");
            Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp91;
            this.Manager.Comment("executing step \'call NetrServerAuthenticate3(NonDcServer,DomainMemberComputerAcco" +
                    "unt,WorkstationSecureChannel,Client,True,16388)\'");
            temp91 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16388u);
            this.Manager.Checkpoint("MS-NRPC_R425");
            this.Manager.Checkpoint("MS-NRPC_R103513");
            this.Manager.Checkpoint("MS-NRPC_R103456");
            this.Manager.Comment("reaching state \'S1423\'");
            this.Manager.Comment("checking step \'return NetrServerAuthenticate3/STATUS_NOT_SUPPORTED\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp91, "return of NetrServerAuthenticate3, state S1423");
            this.Manager.Comment("reaching state \'S1482\'");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate3S116NetrServerReqChallengeChecker(Microsoft.Protocols.TestSuites.Nrpc.HRESULT @return) {
            this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), @return, "return of NetrServerReqChallenge, state S1265");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate3S116GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S117");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate3S116GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S117");
        }
        #endregion
        
        #region Test Starting in S118
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_EstablishSecureChannel_NetrServerAuthenticate3S118() {
            this.Manager.BeginTest("Test_EstablishSecureChannel_NetrServerAuthenticate3S118");
            this.Manager.Comment("reaching state \'S118\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp95;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp95);
            this.Manager.AddReturn(GetPlatformInfo, null, temp95);
            this.Manager.Comment("reaching state \'S119\'");
            int temp102 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate3.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate3S118GetPlatformChecker)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate3.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate3S118GetPlatformChecker1)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate3.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate3S118GetPlatformChecker2)));
            if ((temp102 == 0)) {
                this.Manager.Comment("reaching state \'S297\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S477\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S657\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp96;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp96 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S837\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp96, "return of NetrServerReqChallenge, state S837");
                this.Manager.Comment("reaching state \'S1017\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp97;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,InvalidAccount,Workstation" +
                        "SecureChannel,Client,True,16388)\'");
                temp97 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16388u);
                this.Manager.Checkpoint("MS-NRPC_R103373");
                this.Manager.Checkpoint("MS-NRPC_R103456");
                this.Manager.Comment("reaching state \'S1111\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/STATUS_NO_TRUST_SAM_ACCOUNT\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NO_TRUST_SAM_ACCOUNT, temp97, "return of NetrServerAuthenticate3, state S1111");
                this.Manager.Comment("reaching state \'S1203\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp98;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp98 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S1292\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp98, "return of NetrServerReqChallenge, state S1292");
                this.Manager.Comment("reaching state \'S1381\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp99;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,InvalidAccount,Workstation" +
                        "SecureChannel,Client,True,16388)\'");
                temp99 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType)(1)), Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16388u);
                this.Manager.Checkpoint("MS-NRPC_R103373");
                this.Manager.Checkpoint("MS-NRPC_R103456");
                this.Manager.Comment("reaching state \'S1440\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/STATUS_NO_TRUST_SAM_ACCOUNT\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NO_TRUST_SAM_ACCOUNT, temp99, "return of NetrServerAuthenticate3, state S1440");
                this.Manager.Comment("reaching state \'S1499\'");
                goto label11;
            }
            if ((temp102 == 1)) {
                this.Manager.Comment("reaching state \'S298\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S478\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S658\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp100;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(NonDcServer,Client)\'");
                temp100 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103509");
                this.Manager.Checkpoint("MS-NRPC_R103341");
                this.Manager.Comment("reaching state \'S838\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp100, "return of NetrServerReqChallenge, state S838");
                this.Manager.Comment("reaching state \'S1018\'");
                goto label11;
            }
            if ((temp102 == 2)) {
                this.Manager.Comment("reaching state \'S299\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S479\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S659\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp101;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(NonDcServer,Client)\'");
                temp101 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103509");
                this.Manager.Checkpoint("MS-NRPC_R103341");
                this.Manager.Comment("reaching state \'S839\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp101, "return of NetrServerReqChallenge, state S839");
                this.Manager.Comment("reaching state \'S1019\'");
                goto label11;
            }
            throw new InvalidOperationException("never reached");
        label11:
;
            this.Manager.EndTest();
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate3S118GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S119");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate3S118GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S119");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate3S118GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S119");
        }
        #endregion
        
        #region Test Starting in S12
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_EstablishSecureChannel_NetrServerAuthenticate3S12() {
            this.Manager.BeginTest("Test_EstablishSecureChannel_NetrServerAuthenticate3S12");
            this.Manager.Comment("reaching state \'S12\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp103;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp103);
            this.Manager.AddReturn(GetPlatformInfo, null, temp103);
            this.Manager.Comment("reaching state \'S13\'");
            int temp113 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate3.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate3S12GetPlatformChecker)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate3.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate3S12GetPlatformChecker1)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate3.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate3S12GetPlatformChecker2)));
            if ((temp113 == 0)) {
                this.Manager.Comment("reaching state \'S138\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S318\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S498\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp104;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp104 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S678\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp104, "return of NetrServerReqChallenge, state S678");
                this.Manager.Comment("reaching state \'S858\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp105;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,20)\'");
                temp105 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 20u);
                this.Manager.Checkpoint("MS-NRPC_R103371");
                this.Manager.Checkpoint("MS-NRPC_R103456");
                this.Manager.Comment("reaching state \'S1033\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/STATUS_DOWNGRADE_DETECTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_DOWNGRADE_DETECTED, temp105, "return of NetrServerAuthenticate3, state S1033");
                this.Manager.Comment("reaching state \'S1125\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp106;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp106 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S1217\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp106, "return of NetrServerReqChallenge, state S1217");
                this.Manager.Comment("reaching state \'S1306\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp107;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,4)\'");
                temp107 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 4u);
                this.Manager.Checkpoint("MS-NRPC_R103371");
                this.Manager.Checkpoint("MS-NRPC_R104868");
                this.Manager.Checkpoint("MS-NRPC_R103456");
                this.Manager.AddReturn(NetrServerAuthenticate3Info, null, temp107);
                Test_EstablishSecureChannel_NetrServerAuthenticate3S1385();
                goto label12;
            }
            if ((temp113 == 1)) {
                this.Manager.Comment("reaching state \'S139\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S319\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S499\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp108;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp108 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S679\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp108, "return of NetrServerReqChallenge, state S679");
                this.Manager.Comment("reaching state \'S859\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp109;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,20)\'");
                temp109 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 20u);
                this.Manager.Checkpoint("MS-NRPC_R103371");
                this.Manager.Checkpoint("MS-NRPC_R103456");
                this.Manager.Comment("reaching state \'S1034\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/STATUS_DOWNGRADE_DETECTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_DOWNGRADE_DETECTED, temp109, "return of NetrServerAuthenticate3, state S1034");
                this.Manager.Comment("reaching state \'S1126\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp110;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp110 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S1218\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp110, "return of NetrServerReqChallenge, state S1218");
                this.Manager.Comment("reaching state \'S1307\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp111;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,4)\'");
                temp111 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 4u);
                this.Manager.Checkpoint("MS-NRPC_R103371");
                this.Manager.Checkpoint("MS-NRPC_R104868");
                this.Manager.Checkpoint("MS-NRPC_R103456");
                this.Manager.AddReturn(NetrServerAuthenticate3Info, null, temp111);
                Test_EstablishSecureChannel_NetrServerAuthenticate3S1386();
                goto label12;
            }
            if ((temp113 == 2)) {
                this.Manager.Comment("reaching state \'S140\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S320\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S500\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp112;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(NonDcServer,Client)\'");
                temp112 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103509");
                this.Manager.Checkpoint("MS-NRPC_R103341");
                this.Manager.Comment("reaching state \'S680\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp112, "return of NetrServerReqChallenge, state S680");
                this.Manager.Comment("reaching state \'S860\'");
                goto label12;
            }
            throw new InvalidOperationException("never reached");
        label12:
;
            this.Manager.EndTest();
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate3S12GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S13");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate3S12GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S13");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate3S12GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S13");
        }
        #endregion
        
        #region Test Starting in S14
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_EstablishSecureChannel_NetrServerAuthenticate3S14() {
            this.Manager.BeginTest("Test_EstablishSecureChannel_NetrServerAuthenticate3S14");
            this.Manager.Comment("reaching state \'S14\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp114;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp114);
            this.Manager.AddReturn(GetPlatformInfo, null, temp114);
            this.Manager.Comment("reaching state \'S15\'");
            int temp124 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate3.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate3S14GetPlatformChecker)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate3.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate3S14GetPlatformChecker1)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate3.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate3S14GetPlatformChecker2)));
            if ((temp124 == 0)) {
                this.Manager.Comment("reaching state \'S141\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S321\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S501\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp115;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp115 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S681\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp115, "return of NetrServerReqChallenge, state S681");
                this.Manager.Comment("reaching state \'S861\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp116;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,36)\'");
                temp116 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 36u);
                this.Manager.Checkpoint("MS-NRPC_R103371");
                this.Manager.Checkpoint("MS-NRPC_R103456");
                this.Manager.Comment("reaching state \'S1035\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/STATUS_DOWNGRADE_DETECTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_DOWNGRADE_DETECTED, temp116, "return of NetrServerAuthenticate3, state S1035");
                this.Manager.Comment("reaching state \'S1127\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp117;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp117 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S1219\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp117, "return of NetrServerReqChallenge, state S1219");
                this.Manager.Comment("reaching state \'S1308\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp118;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,4)\'");
                temp118 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 4u);
                this.Manager.Checkpoint("MS-NRPC_R103371");
                this.Manager.Checkpoint("MS-NRPC_R104869");
                this.Manager.Checkpoint("MS-NRPC_R103456");
                this.Manager.AddReturn(NetrServerAuthenticate3Info, null, temp118);
                Test_EstablishSecureChannel_NetrServerAuthenticate3S1385();
                goto label13;
            }
            if ((temp124 == 1)) {
                this.Manager.Comment("reaching state \'S142\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S322\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S502\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp119;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp119 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S682\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp119, "return of NetrServerReqChallenge, state S682");
                this.Manager.Comment("reaching state \'S862\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp120;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,36)\'");
                temp120 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 36u);
                this.Manager.Checkpoint("MS-NRPC_R103371");
                this.Manager.Checkpoint("MS-NRPC_R103456");
                this.Manager.Comment("reaching state \'S1036\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/STATUS_DOWNGRADE_DETECTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_DOWNGRADE_DETECTED, temp120, "return of NetrServerAuthenticate3, state S1036");
                this.Manager.Comment("reaching state \'S1128\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp121;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp121 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S1220\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp121, "return of NetrServerReqChallenge, state S1220");
                this.Manager.Comment("reaching state \'S1309\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp122;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,4)\'");
                temp122 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 4u);
                this.Manager.Checkpoint("MS-NRPC_R103371");
                this.Manager.Checkpoint("MS-NRPC_R104869");
                this.Manager.Checkpoint("MS-NRPC_R103456");
                this.Manager.AddReturn(NetrServerAuthenticate3Info, null, temp122);
                Test_EstablishSecureChannel_NetrServerAuthenticate3S1386();
                goto label13;
            }
            if ((temp124 == 2)) {
                this.Manager.Comment("reaching state \'S143\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S323\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S503\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp123;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(NonDcServer,Client)\'");
                temp123 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103509");
                this.Manager.Checkpoint("MS-NRPC_R103341");
                this.Manager.Comment("reaching state \'S683\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp123, "return of NetrServerReqChallenge, state S683");
                this.Manager.Comment("reaching state \'S863\'");
                goto label13;
            }
            throw new InvalidOperationException("never reached");
        label13:
;
            this.Manager.EndTest();
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate3S14GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S15");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate3S14GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S15");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate3S14GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S15");
        }
        #endregion
        
        #region Test Starting in S16
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_EstablishSecureChannel_NetrServerAuthenticate3S16() {
            this.Manager.BeginTest("Test_EstablishSecureChannel_NetrServerAuthenticate3S16");
            this.Manager.Comment("reaching state \'S16\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp125;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp125);
            this.Manager.AddReturn(GetPlatformInfo, null, temp125);
            this.Manager.Comment("reaching state \'S17\'");
            int temp135 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate3.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate3S16GetPlatformChecker)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate3.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate3S16GetPlatformChecker1)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate3.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate3S16GetPlatformChecker2)));
            if ((temp135 == 0)) {
                this.Manager.Comment("reaching state \'S144\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S324\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S504\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp126;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp126 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S684\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp126, "return of NetrServerReqChallenge, state S684");
                this.Manager.Comment("reaching state \'S864\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp127;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,68)\'");
                temp127 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 68u);
                this.Manager.Checkpoint("MS-NRPC_R103371");
                this.Manager.Checkpoint("MS-NRPC_R103456");
                this.Manager.Comment("reaching state \'S1037\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/STATUS_DOWNGRADE_DETECTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_DOWNGRADE_DETECTED, temp127, "return of NetrServerAuthenticate3, state S1037");
                this.Manager.Comment("reaching state \'S1129\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp128;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp128 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S1221\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp128, "return of NetrServerReqChallenge, state S1221");
                this.Manager.Comment("reaching state \'S1310\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp129;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,68)\'");
                temp129 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 68u);
                this.Manager.Checkpoint("MS-NRPC_R103371");
                this.Manager.Checkpoint("MS-NRPC_R103456");
                this.Manager.Comment("reaching state \'S1389\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/STATUS_DOWNGRADE_DETECTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_DOWNGRADE_DETECTED, temp129, "return of NetrServerAuthenticate3, state S1389");
                this.Manager.Comment("reaching state \'S1448\'");
                goto label14;
            }
            if ((temp135 == 1)) {
                this.Manager.Comment("reaching state \'S145\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S325\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S505\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp130;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp130 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S685\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp130, "return of NetrServerReqChallenge, state S685");
                this.Manager.Comment("reaching state \'S865\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp131;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,68)\'");
                temp131 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 68u);
                this.Manager.Checkpoint("MS-NRPC_R103371");
                this.Manager.Checkpoint("MS-NRPC_R103456");
                this.Manager.Comment("reaching state \'S1038\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/STATUS_DOWNGRADE_DETECTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_DOWNGRADE_DETECTED, temp131, "return of NetrServerAuthenticate3, state S1038");
                this.Manager.Comment("reaching state \'S1130\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp132;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp132 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S1222\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp132, "return of NetrServerReqChallenge, state S1222");
                this.Manager.Comment("reaching state \'S1311\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp133;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,68)\'");
                temp133 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 68u);
                this.Manager.Checkpoint("MS-NRPC_R103371");
                this.Manager.Checkpoint("MS-NRPC_R103456");
                this.Manager.Comment("reaching state \'S1390\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/STATUS_DOWNGRADE_DETECTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_DOWNGRADE_DETECTED, temp133, "return of NetrServerAuthenticate3, state S1390");
                this.Manager.Comment("reaching state \'S1449\'");
                goto label14;
            }
            if ((temp135 == 2)) {
                this.Manager.Comment("reaching state \'S146\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S326\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S506\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp134;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(NonDcServer,Client)\'");
                temp134 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103509");
                this.Manager.Checkpoint("MS-NRPC_R103341");
                this.Manager.Comment("reaching state \'S686\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp134, "return of NetrServerReqChallenge, state S686");
                this.Manager.Comment("reaching state \'S866\'");
                goto label14;
            }
            throw new InvalidOperationException("never reached");
        label14:
;
            this.Manager.EndTest();
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate3S16GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S17");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate3S16GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S17");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate3S16GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S17");
        }
        #endregion
        
        #region Test Starting in S18
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_EstablishSecureChannel_NetrServerAuthenticate3S18() {
            this.Manager.BeginTest("Test_EstablishSecureChannel_NetrServerAuthenticate3S18");
            this.Manager.Comment("reaching state \'S18\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp136;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp136);
            this.Manager.AddReturn(GetPlatformInfo, null, temp136);
            this.Manager.Comment("reaching state \'S19\'");
            int temp146 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate3.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate3S18GetPlatformChecker)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate3.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate3S18GetPlatformChecker1)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate3.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate3S18GetPlatformChecker2)));
            if ((temp146 == 0)) {
                this.Manager.Comment("reaching state \'S147\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S327\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S507\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp137;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp137 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S687\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp137, "return of NetrServerReqChallenge, state S687");
                this.Manager.Comment("reaching state \'S867\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp138;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,132)\'");
                temp138 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 132u);
                this.Manager.Checkpoint("MS-NRPC_R103371");
                this.Manager.Checkpoint("MS-NRPC_R103456");
                this.Manager.Comment("reaching state \'S1039\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/STATUS_DOWNGRADE_DETECTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_DOWNGRADE_DETECTED, temp138, "return of NetrServerAuthenticate3, state S1039");
                this.Manager.Comment("reaching state \'S1131\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp139;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp139 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S1223\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp139, "return of NetrServerReqChallenge, state S1223");
                this.Manager.Comment("reaching state \'S1312\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp140;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,132)\'");
                temp140 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 132u);
                this.Manager.Checkpoint("MS-NRPC_R103371");
                this.Manager.Checkpoint("MS-NRPC_R103456");
                this.Manager.Comment("reaching state \'S1391\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/STATUS_DOWNGRADE_DETECTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_DOWNGRADE_DETECTED, temp140, "return of NetrServerAuthenticate3, state S1391");
                this.Manager.Comment("reaching state \'S1450\'");
                goto label15;
            }
            if ((temp146 == 1)) {
                this.Manager.Comment("reaching state \'S148\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S328\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S508\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp141;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp141 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S688\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp141, "return of NetrServerReqChallenge, state S688");
                this.Manager.Comment("reaching state \'S868\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp142;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,132)\'");
                temp142 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 132u);
                this.Manager.Checkpoint("MS-NRPC_R103371");
                this.Manager.Checkpoint("MS-NRPC_R103456");
                this.Manager.Comment("reaching state \'S1040\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/STATUS_DOWNGRADE_DETECTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_DOWNGRADE_DETECTED, temp142, "return of NetrServerAuthenticate3, state S1040");
                this.Manager.Comment("reaching state \'S1132\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp143;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp143 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S1224\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp143, "return of NetrServerReqChallenge, state S1224");
                this.Manager.Comment("reaching state \'S1313\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp144;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,132)\'");
                temp144 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 132u);
                this.Manager.Checkpoint("MS-NRPC_R103371");
                this.Manager.Checkpoint("MS-NRPC_R103456");
                this.Manager.Comment("reaching state \'S1392\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/STATUS_DOWNGRADE_DETECTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_DOWNGRADE_DETECTED, temp144, "return of NetrServerAuthenticate3, state S1392");
                this.Manager.Comment("reaching state \'S1451\'");
                goto label15;
            }
            if ((temp146 == 2)) {
                this.Manager.Comment("reaching state \'S149\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S329\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S509\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp145;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(NonDcServer,Client)\'");
                temp145 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103509");
                this.Manager.Checkpoint("MS-NRPC_R103341");
                this.Manager.Comment("reaching state \'S689\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp145, "return of NetrServerReqChallenge, state S689");
                this.Manager.Comment("reaching state \'S869\'");
                goto label15;
            }
            throw new InvalidOperationException("never reached");
        label15:
;
            this.Manager.EndTest();
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate3S18GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S19");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate3S18GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S19");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate3S18GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S19");
        }
        #endregion
        
        #region Test Starting in S2
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_EstablishSecureChannel_NetrServerAuthenticate3S2() {
            this.Manager.BeginTest("Test_EstablishSecureChannel_NetrServerAuthenticate3S2");
            this.Manager.Comment("reaching state \'S2\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp147;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp147);
            this.Manager.AddReturn(GetPlatformInfo, null, temp147);
            this.Manager.Comment("reaching state \'S3\'");
            int temp157 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate3.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate3S2GetPlatformChecker)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate3.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate3S2GetPlatformChecker1)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate3.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate3S2GetPlatformChecker2)));
            if ((temp157 == 0)) {
                this.Manager.Comment("reaching state \'S123\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S303\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S483\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp148;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp148 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S663\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp148, "return of NetrServerReqChallenge, state S663");
                this.Manager.Comment("reaching state \'S843\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp149;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,5)\'");
                temp149 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 5u);
                this.Manager.Checkpoint("MS-NRPC_R103371");
                this.Manager.Checkpoint("MS-NRPC_R103456");
                this.Manager.Comment("reaching state \'S1023\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/STATUS_DOWNGRADE_DETECTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_DOWNGRADE_DETECTED, temp149, "return of NetrServerAuthenticate3, state S1023");
                this.Manager.Comment("reaching state \'S1115\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp150;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp150 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S1207\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp150, "return of NetrServerReqChallenge, state S1207");
                this.Manager.Comment("reaching state \'S1296\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp151;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,4)\'");
                temp151 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 4u);
                this.Manager.Checkpoint("MS-NRPC_R103371");
                this.Manager.Checkpoint("MS-NRPC_R888");
                this.Manager.Checkpoint("MS-NRPC_R103456");
                this.Manager.AddReturn(NetrServerAuthenticate3Info, null, temp151);
                Test_EstablishSecureChannel_NetrServerAuthenticate3S1385();
                goto label16;
            }
            if ((temp157 == 1)) {
                this.Manager.Comment("reaching state \'S124\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S304\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S484\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp152;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp152 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S664\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp152, "return of NetrServerReqChallenge, state S664");
                this.Manager.Comment("reaching state \'S844\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp153;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,5)\'");
                temp153 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 5u);
                this.Manager.Checkpoint("MS-NRPC_R103371");
                this.Manager.Checkpoint("MS-NRPC_R103456");
                this.Manager.Comment("reaching state \'S1024\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/STATUS_DOWNGRADE_DETECTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_DOWNGRADE_DETECTED, temp153, "return of NetrServerAuthenticate3, state S1024");
                this.Manager.Comment("reaching state \'S1116\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp154;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp154 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S1208\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp154, "return of NetrServerReqChallenge, state S1208");
                this.Manager.Comment("reaching state \'S1297\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp155;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,4)\'");
                temp155 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 4u);
                this.Manager.Checkpoint("MS-NRPC_R103371");
                this.Manager.Checkpoint("MS-NRPC_R888");
                this.Manager.Checkpoint("MS-NRPC_R103456");
                this.Manager.AddReturn(NetrServerAuthenticate3Info, null, temp155);
                Test_EstablishSecureChannel_NetrServerAuthenticate3S1386();
                goto label16;
            }
            if ((temp157 == 2)) {
                this.Manager.Comment("reaching state \'S125\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S305\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S485\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp156;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(NonDcServer,Client)\'");
                temp156 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103509");
                this.Manager.Checkpoint("MS-NRPC_R103341");
                this.Manager.Comment("reaching state \'S665\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp156, "return of NetrServerReqChallenge, state S665");
                this.Manager.Comment("reaching state \'S845\'");
                goto label16;
            }
            throw new InvalidOperationException("never reached");
        label16:
;
            this.Manager.EndTest();
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate3S2GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S3");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate3S2GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S3");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate3S2GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S3");
        }
        #endregion
        
        #region Test Starting in S20
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_EstablishSecureChannel_NetrServerAuthenticate3S20() {
            this.Manager.BeginTest("Test_EstablishSecureChannel_NetrServerAuthenticate3S20");
            this.Manager.Comment("reaching state \'S20\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp158;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp158);
            this.Manager.AddReturn(GetPlatformInfo, null, temp158);
            this.Manager.Comment("reaching state \'S21\'");
            int temp168 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate3.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate3S20GetPlatformChecker)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate3.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate3S20GetPlatformChecker1)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate3.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate3S20GetPlatformChecker2)));
            if ((temp168 == 0)) {
                this.Manager.Comment("reaching state \'S150\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S330\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S510\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp159;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp159 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S690\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp159, "return of NetrServerReqChallenge, state S690");
                this.Manager.Comment("reaching state \'S870\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp160;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,260)\'");
                temp160 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 260u);
                this.Manager.Checkpoint("MS-NRPC_R103371");
                this.Manager.Checkpoint("MS-NRPC_R103456");
                this.Manager.Comment("reaching state \'S1041\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/STATUS_DOWNGRADE_DETECTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_DOWNGRADE_DETECTED, temp160, "return of NetrServerAuthenticate3, state S1041");
                this.Manager.Comment("reaching state \'S1133\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp161;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp161 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S1225\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp161, "return of NetrServerReqChallenge, state S1225");
                this.Manager.Comment("reaching state \'S1314\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp162;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,260)\'");
                temp162 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 260u);
                this.Manager.Checkpoint("MS-NRPC_R103371");
                this.Manager.Checkpoint("MS-NRPC_R103456");
                this.Manager.Comment("reaching state \'S1393\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/STATUS_DOWNGRADE_DETECTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_DOWNGRADE_DETECTED, temp162, "return of NetrServerAuthenticate3, state S1393");
                this.Manager.Comment("reaching state \'S1452\'");
                goto label17;
            }
            if ((temp168 == 1)) {
                this.Manager.Comment("reaching state \'S151\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S331\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S511\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp163;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp163 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S691\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp163, "return of NetrServerReqChallenge, state S691");
                this.Manager.Comment("reaching state \'S871\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp164;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,260)\'");
                temp164 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 260u);
                this.Manager.Checkpoint("MS-NRPC_R103371");
                this.Manager.Checkpoint("MS-NRPC_R103456");
                this.Manager.Comment("reaching state \'S1042\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/STATUS_DOWNGRADE_DETECTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_DOWNGRADE_DETECTED, temp164, "return of NetrServerAuthenticate3, state S1042");
                this.Manager.Comment("reaching state \'S1134\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp165;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp165 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S1226\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp165, "return of NetrServerReqChallenge, state S1226");
                this.Manager.Comment("reaching state \'S1315\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp166;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,260)\'");
                temp166 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 260u);
                this.Manager.Checkpoint("MS-NRPC_R103371");
                this.Manager.Checkpoint("MS-NRPC_R103456");
                this.Manager.Comment("reaching state \'S1394\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/STATUS_DOWNGRADE_DETECTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_DOWNGRADE_DETECTED, temp166, "return of NetrServerAuthenticate3, state S1394");
                this.Manager.Comment("reaching state \'S1453\'");
                goto label17;
            }
            if ((temp168 == 2)) {
                this.Manager.Comment("reaching state \'S152\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S332\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S512\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp167;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(NonDcServer,Client)\'");
                temp167 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103509");
                this.Manager.Checkpoint("MS-NRPC_R103341");
                this.Manager.Comment("reaching state \'S692\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp167, "return of NetrServerReqChallenge, state S692");
                this.Manager.Comment("reaching state \'S872\'");
                goto label17;
            }
            throw new InvalidOperationException("never reached");
        label17:
;
            this.Manager.EndTest();
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate3S20GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S21");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate3S20GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S21");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate3S20GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S21");
        }
        #endregion
        
        #region Test Starting in S22
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_EstablishSecureChannel_NetrServerAuthenticate3S22() {
            this.Manager.BeginTest("Test_EstablishSecureChannel_NetrServerAuthenticate3S22");
            this.Manager.Comment("reaching state \'S22\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp169;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp169);
            this.Manager.AddReturn(GetPlatformInfo, null, temp169);
            this.Manager.Comment("reaching state \'S23\'");
            int temp179 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate3.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate3S22GetPlatformChecker)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate3.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate3S22GetPlatformChecker1)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate3.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate3S22GetPlatformChecker2)));
            if ((temp179 == 0)) {
                this.Manager.Comment("reaching state \'S153\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S333\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S513\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp170;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp170 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S693\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp170, "return of NetrServerReqChallenge, state S693");
                this.Manager.Comment("reaching state \'S873\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp171;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,516)\'");
                temp171 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 516u);
                this.Manager.Checkpoint("MS-NRPC_R103371");
                this.Manager.Checkpoint("MS-NRPC_R103456");
                this.Manager.Comment("reaching state \'S1043\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/STATUS_DOWNGRADE_DETECTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_DOWNGRADE_DETECTED, temp171, "return of NetrServerAuthenticate3, state S1043");
                this.Manager.Comment("reaching state \'S1135\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp172;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp172 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S1227\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp172, "return of NetrServerReqChallenge, state S1227");
                this.Manager.Comment("reaching state \'S1316\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp173;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,516)\'");
                temp173 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 516u);
                this.Manager.Checkpoint("MS-NRPC_R103371");
                this.Manager.Checkpoint("MS-NRPC_R103456");
                this.Manager.Comment("reaching state \'S1395\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/STATUS_DOWNGRADE_DETECTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_DOWNGRADE_DETECTED, temp173, "return of NetrServerAuthenticate3, state S1395");
                this.Manager.Comment("reaching state \'S1454\'");
                goto label18;
            }
            if ((temp179 == 1)) {
                this.Manager.Comment("reaching state \'S154\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S334\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S514\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp174;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp174 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S694\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp174, "return of NetrServerReqChallenge, state S694");
                this.Manager.Comment("reaching state \'S874\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp175;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,516)\'");
                temp175 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 516u);
                this.Manager.Checkpoint("MS-NRPC_R103371");
                this.Manager.Checkpoint("MS-NRPC_R103456");
                this.Manager.Comment("reaching state \'S1044\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/STATUS_DOWNGRADE_DETECTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_DOWNGRADE_DETECTED, temp175, "return of NetrServerAuthenticate3, state S1044");
                this.Manager.Comment("reaching state \'S1136\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp176;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp176 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S1228\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp176, "return of NetrServerReqChallenge, state S1228");
                this.Manager.Comment("reaching state \'S1317\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp177;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,516)\'");
                temp177 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 516u);
                this.Manager.Checkpoint("MS-NRPC_R103371");
                this.Manager.Checkpoint("MS-NRPC_R103456");
                this.Manager.Comment("reaching state \'S1396\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/STATUS_DOWNGRADE_DETECTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_DOWNGRADE_DETECTED, temp177, "return of NetrServerAuthenticate3, state S1396");
                this.Manager.Comment("reaching state \'S1455\'");
                goto label18;
            }
            if ((temp179 == 2)) {
                this.Manager.Comment("reaching state \'S155\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S335\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S515\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp178;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(NonDcServer,Client)\'");
                temp178 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103509");
                this.Manager.Checkpoint("MS-NRPC_R103341");
                this.Manager.Comment("reaching state \'S695\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp178, "return of NetrServerReqChallenge, state S695");
                this.Manager.Comment("reaching state \'S875\'");
                goto label18;
            }
            throw new InvalidOperationException("never reached");
        label18:
;
            this.Manager.EndTest();
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate3S22GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S23");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate3S22GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S23");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate3S22GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S23");
        }
        #endregion
        
        #region Test Starting in S24
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_EstablishSecureChannel_NetrServerAuthenticate3S24() {
            this.Manager.BeginTest("Test_EstablishSecureChannel_NetrServerAuthenticate3S24");
            this.Manager.Comment("reaching state \'S24\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp180;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp180);
            this.Manager.AddReturn(GetPlatformInfo, null, temp180);
            this.Manager.Comment("reaching state \'S25\'");
            int temp190 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate3.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate3S24GetPlatformChecker)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate3.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate3S24GetPlatformChecker1)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate3.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate3S24GetPlatformChecker2)));
            if ((temp190 == 0)) {
                this.Manager.Comment("reaching state \'S156\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S336\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S516\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp181;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp181 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S696\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp181, "return of NetrServerReqChallenge, state S696");
                this.Manager.Comment("reaching state \'S876\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp182;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,1028)\'");
                temp182 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 1028u);
                this.Manager.Checkpoint("MS-NRPC_R103371");
                this.Manager.Checkpoint("MS-NRPC_R103456");
                this.Manager.Comment("reaching state \'S1045\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/STATUS_DOWNGRADE_DETECTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_DOWNGRADE_DETECTED, temp182, "return of NetrServerAuthenticate3, state S1045");
                this.Manager.Comment("reaching state \'S1137\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp183;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp183 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S1229\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp183, "return of NetrServerReqChallenge, state S1229");
                this.Manager.Comment("reaching state \'S1318\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp184;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,1028)\'");
                temp184 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 1028u);
                this.Manager.Checkpoint("MS-NRPC_R103371");
                this.Manager.Checkpoint("MS-NRPC_R103456");
                this.Manager.Comment("reaching state \'S1397\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/STATUS_DOWNGRADE_DETECTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_DOWNGRADE_DETECTED, temp184, "return of NetrServerAuthenticate3, state S1397");
                this.Manager.Comment("reaching state \'S1456\'");
                goto label19;
            }
            if ((temp190 == 1)) {
                this.Manager.Comment("reaching state \'S157\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S337\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S517\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp185;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp185 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S697\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp185, "return of NetrServerReqChallenge, state S697");
                this.Manager.Comment("reaching state \'S877\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp186;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,1028)\'");
                temp186 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 1028u);
                this.Manager.Checkpoint("MS-NRPC_R103371");
                this.Manager.Checkpoint("MS-NRPC_R103456");
                this.Manager.Comment("reaching state \'S1046\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/STATUS_DOWNGRADE_DETECTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_DOWNGRADE_DETECTED, temp186, "return of NetrServerAuthenticate3, state S1046");
                this.Manager.Comment("reaching state \'S1138\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp187;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp187 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S1230\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp187, "return of NetrServerReqChallenge, state S1230");
                this.Manager.Comment("reaching state \'S1319\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp188;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,1028)\'");
                temp188 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 1028u);
                this.Manager.Checkpoint("MS-NRPC_R103371");
                this.Manager.Checkpoint("MS-NRPC_R103456");
                this.Manager.Comment("reaching state \'S1398\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/STATUS_DOWNGRADE_DETECTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_DOWNGRADE_DETECTED, temp188, "return of NetrServerAuthenticate3, state S1398");
                this.Manager.Comment("reaching state \'S1457\'");
                goto label19;
            }
            if ((temp190 == 2)) {
                this.Manager.Comment("reaching state \'S158\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S338\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S518\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp189;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(NonDcServer,Client)\'");
                temp189 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103509");
                this.Manager.Checkpoint("MS-NRPC_R103341");
                this.Manager.Comment("reaching state \'S698\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp189, "return of NetrServerReqChallenge, state S698");
                this.Manager.Comment("reaching state \'S878\'");
                goto label19;
            }
            throw new InvalidOperationException("never reached");
        label19:
;
            this.Manager.EndTest();
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate3S24GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S25");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate3S24GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S25");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate3S24GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S25");
        }
        #endregion
        
        #region Test Starting in S26
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_EstablishSecureChannel_NetrServerAuthenticate3S26() {
            this.Manager.BeginTest("Test_EstablishSecureChannel_NetrServerAuthenticate3S26");
            this.Manager.Comment("reaching state \'S26\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp191;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp191);
            this.Manager.AddReturn(GetPlatformInfo, null, temp191);
            this.Manager.Comment("reaching state \'S27\'");
            int temp201 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate3.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate3S26GetPlatformChecker)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate3.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate3S26GetPlatformChecker1)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate3.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate3S26GetPlatformChecker2)));
            if ((temp201 == 0)) {
                this.Manager.Comment("reaching state \'S159\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S339\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S519\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp192;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp192 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S699\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp192, "return of NetrServerReqChallenge, state S699");
                this.Manager.Comment("reaching state \'S879\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp193;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,2052)\'");
                temp193 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 2052u);
                this.Manager.Checkpoint("MS-NRPC_R103371");
                this.Manager.Checkpoint("MS-NRPC_R103456");
                this.Manager.Comment("reaching state \'S1047\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/STATUS_DOWNGRADE_DETECTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_DOWNGRADE_DETECTED, temp193, "return of NetrServerAuthenticate3, state S1047");
                this.Manager.Comment("reaching state \'S1139\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp194;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp194 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S1231\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp194, "return of NetrServerReqChallenge, state S1231");
                this.Manager.Comment("reaching state \'S1320\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp195;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,2052)\'");
                temp195 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 2052u);
                this.Manager.Checkpoint("MS-NRPC_R103371");
                this.Manager.Checkpoint("MS-NRPC_R103456");
                this.Manager.Comment("reaching state \'S1399\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/STATUS_DOWNGRADE_DETECTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_DOWNGRADE_DETECTED, temp195, "return of NetrServerAuthenticate3, state S1399");
                this.Manager.Comment("reaching state \'S1458\'");
                goto label20;
            }
            if ((temp201 == 1)) {
                this.Manager.Comment("reaching state \'S160\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S340\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S520\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp196;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp196 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S700\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp196, "return of NetrServerReqChallenge, state S700");
                this.Manager.Comment("reaching state \'S880\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp197;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,2052)\'");
                temp197 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 2052u);
                this.Manager.Checkpoint("MS-NRPC_R103371");
                this.Manager.Checkpoint("MS-NRPC_R103456");
                this.Manager.Comment("reaching state \'S1048\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/STATUS_DOWNGRADE_DETECTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_DOWNGRADE_DETECTED, temp197, "return of NetrServerAuthenticate3, state S1048");
                this.Manager.Comment("reaching state \'S1140\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp198;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp198 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S1232\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp198, "return of NetrServerReqChallenge, state S1232");
                this.Manager.Comment("reaching state \'S1321\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp199;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,2052)\'");
                temp199 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 2052u);
                this.Manager.Checkpoint("MS-NRPC_R103371");
                this.Manager.Checkpoint("MS-NRPC_R103456");
                this.Manager.Comment("reaching state \'S1400\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/STATUS_DOWNGRADE_DETECTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_DOWNGRADE_DETECTED, temp199, "return of NetrServerAuthenticate3, state S1400");
                this.Manager.Comment("reaching state \'S1459\'");
                goto label20;
            }
            if ((temp201 == 2)) {
                this.Manager.Comment("reaching state \'S161\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S341\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S521\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp200;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(NonDcServer,Client)\'");
                temp200 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103509");
                this.Manager.Checkpoint("MS-NRPC_R103341");
                this.Manager.Comment("reaching state \'S701\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp200, "return of NetrServerReqChallenge, state S701");
                this.Manager.Comment("reaching state \'S881\'");
                goto label20;
            }
            throw new InvalidOperationException("never reached");
        label20:
;
            this.Manager.EndTest();
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate3S26GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S27");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate3S26GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S27");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate3S26GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S27");
        }
        #endregion
        
        #region Test Starting in S28
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_EstablishSecureChannel_NetrServerAuthenticate3S28() {
            this.Manager.BeginTest("Test_EstablishSecureChannel_NetrServerAuthenticate3S28");
            this.Manager.Comment("reaching state \'S28\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp202;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp202);
            this.Manager.AddReturn(GetPlatformInfo, null, temp202);
            this.Manager.Comment("reaching state \'S29\'");
            int temp212 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate3.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate3S28GetPlatformChecker)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate3.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate3S28GetPlatformChecker1)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate3.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate3S28GetPlatformChecker2)));
            if ((temp212 == 0)) {
                this.Manager.Comment("reaching state \'S162\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S342\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S522\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp203;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp203 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S702\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp203, "return of NetrServerReqChallenge, state S702");
                this.Manager.Comment("reaching state \'S882\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp204;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,4100)\'");
                temp204 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 4100u);
                this.Manager.Checkpoint("MS-NRPC_R103371");
                this.Manager.Checkpoint("MS-NRPC_R103456");
                this.Manager.Comment("reaching state \'S1049\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/STATUS_DOWNGRADE_DETECTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_DOWNGRADE_DETECTED, temp204, "return of NetrServerAuthenticate3, state S1049");
                this.Manager.Comment("reaching state \'S1141\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp205;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp205 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S1233\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp205, "return of NetrServerReqChallenge, state S1233");
                this.Manager.Comment("reaching state \'S1322\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp206;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,4)\'");
                temp206 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 4u);
                this.Manager.Checkpoint("MS-NRPC_R103371");
                this.Manager.Checkpoint("MS-NRPC_R891");
                this.Manager.Checkpoint("MS-NRPC_R103456");
                this.Manager.AddReturn(NetrServerAuthenticate3Info, null, temp206);
                Test_EstablishSecureChannel_NetrServerAuthenticate3S1385();
                goto label21;
            }
            if ((temp212 == 1)) {
                this.Manager.Comment("reaching state \'S163\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S343\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S523\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp207;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp207 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S703\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp207, "return of NetrServerReqChallenge, state S703");
                this.Manager.Comment("reaching state \'S883\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp208;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,4100)\'");
                temp208 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 4100u);
                this.Manager.Checkpoint("MS-NRPC_R103371");
                this.Manager.Checkpoint("MS-NRPC_R103456");
                this.Manager.Comment("reaching state \'S1050\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/STATUS_DOWNGRADE_DETECTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_DOWNGRADE_DETECTED, temp208, "return of NetrServerAuthenticate3, state S1050");
                this.Manager.Comment("reaching state \'S1142\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp209;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp209 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S1234\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp209, "return of NetrServerReqChallenge, state S1234");
                this.Manager.Comment("reaching state \'S1323\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp210;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,4)\'");
                temp210 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 4u);
                this.Manager.Checkpoint("MS-NRPC_R103371");
                this.Manager.Checkpoint("MS-NRPC_R891");
                this.Manager.Checkpoint("MS-NRPC_R103456");
                this.Manager.AddReturn(NetrServerAuthenticate3Info, null, temp210);
                Test_EstablishSecureChannel_NetrServerAuthenticate3S1386();
                goto label21;
            }
            if ((temp212 == 2)) {
                this.Manager.Comment("reaching state \'S164\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S344\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S524\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp211;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(NonDcServer,Client)\'");
                temp211 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103509");
                this.Manager.Checkpoint("MS-NRPC_R103341");
                this.Manager.Comment("reaching state \'S704\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp211, "return of NetrServerReqChallenge, state S704");
                this.Manager.Comment("reaching state \'S884\'");
                goto label21;
            }
            throw new InvalidOperationException("never reached");
        label21:
;
            this.Manager.EndTest();
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate3S28GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S29");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate3S28GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S29");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate3S28GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S29");
        }
        #endregion
        
        #region Test Starting in S30
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_EstablishSecureChannel_NetrServerAuthenticate3S30() {
            this.Manager.BeginTest("Test_EstablishSecureChannel_NetrServerAuthenticate3S30");
            this.Manager.Comment("reaching state \'S30\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp213;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp213);
            this.Manager.AddReturn(GetPlatformInfo, null, temp213);
            this.Manager.Comment("reaching state \'S31\'");
            int temp223 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate3.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate3S30GetPlatformChecker)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate3.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate3S30GetPlatformChecker1)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate3.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate3S30GetPlatformChecker2)));
            if ((temp223 == 0)) {
                this.Manager.Comment("reaching state \'S165\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S345\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S525\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp214;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp214 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S705\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp214, "return of NetrServerReqChallenge, state S705");
                this.Manager.Comment("reaching state \'S885\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp215;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,8196)\'");
                temp215 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 8196u);
                this.Manager.Checkpoint("MS-NRPC_R103371");
                this.Manager.Checkpoint("MS-NRPC_R103456");
                this.Manager.Comment("reaching state \'S1051\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/STATUS_DOWNGRADE_DETECTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_DOWNGRADE_DETECTED, temp215, "return of NetrServerAuthenticate3, state S1051");
                this.Manager.Comment("reaching state \'S1143\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp216;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp216 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S1235\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp216, "return of NetrServerReqChallenge, state S1235");
                this.Manager.Comment("reaching state \'S1324\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp217;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,4)\'");
                temp217 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 4u);
                this.Manager.Checkpoint("MS-NRPC_R103371");
                this.Manager.Checkpoint("MS-NRPC_R892");
                this.Manager.Checkpoint("MS-NRPC_R103456");
                this.Manager.AddReturn(NetrServerAuthenticate3Info, null, temp217);
                Test_EstablishSecureChannel_NetrServerAuthenticate3S1385();
                goto label22;
            }
            if ((temp223 == 1)) {
                this.Manager.Comment("reaching state \'S166\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S346\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S526\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp218;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp218 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S706\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp218, "return of NetrServerReqChallenge, state S706");
                this.Manager.Comment("reaching state \'S886\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp219;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,8196)\'");
                temp219 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 8196u);
                this.Manager.Checkpoint("MS-NRPC_R103371");
                this.Manager.Checkpoint("MS-NRPC_R103456");
                this.Manager.Comment("reaching state \'S1052\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/STATUS_DOWNGRADE_DETECTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_DOWNGRADE_DETECTED, temp219, "return of NetrServerAuthenticate3, state S1052");
                this.Manager.Comment("reaching state \'S1144\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp220;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp220 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S1236\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp220, "return of NetrServerReqChallenge, state S1236");
                this.Manager.Comment("reaching state \'S1325\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp221;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,4)\'");
                temp221 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 4u);
                this.Manager.Checkpoint("MS-NRPC_R103371");
                this.Manager.Checkpoint("MS-NRPC_R892");
                this.Manager.Checkpoint("MS-NRPC_R103456");
                this.Manager.AddReturn(NetrServerAuthenticate3Info, null, temp221);
                Test_EstablishSecureChannel_NetrServerAuthenticate3S1386();
                goto label22;
            }
            if ((temp223 == 2)) {
                this.Manager.Comment("reaching state \'S167\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S347\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S527\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp222;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(NonDcServer,Client)\'");
                temp222 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103509");
                this.Manager.Checkpoint("MS-NRPC_R103341");
                this.Manager.Comment("reaching state \'S707\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp222, "return of NetrServerReqChallenge, state S707");
                this.Manager.Comment("reaching state \'S887\'");
                goto label22;
            }
            throw new InvalidOperationException("never reached");
        label22:
;
            this.Manager.EndTest();
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate3S30GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S31");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate3S30GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S31");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate3S30GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S31");
        }
        #endregion
        
        #region Test Starting in S32
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_EstablishSecureChannel_NetrServerAuthenticate3S32() {
            this.Manager.BeginTest("Test_EstablishSecureChannel_NetrServerAuthenticate3S32");
            this.Manager.Comment("reaching state \'S32\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp224;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp224);
            this.Manager.AddReturn(GetPlatformInfo, null, temp224);
            this.Manager.Comment("reaching state \'S33\'");
            int temp234 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate3.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate3S32GetPlatformChecker)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate3.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate3S32GetPlatformChecker1)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate3.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate3S32GetPlatformChecker2)));
            if ((temp234 == 0)) {
                this.Manager.Comment("reaching state \'S168\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S348\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S528\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp225;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp225 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S708\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp225, "return of NetrServerReqChallenge, state S708");
                this.Manager.Comment("reaching state \'S888\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp226;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,32772)\'");
                temp226 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 32772u);
                this.Manager.Checkpoint("MS-NRPC_R103371");
                this.Manager.Checkpoint("MS-NRPC_R103456");
                this.Manager.Comment("reaching state \'S1053\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/STATUS_DOWNGRADE_DETECTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_DOWNGRADE_DETECTED, temp226, "return of NetrServerAuthenticate3, state S1053");
                this.Manager.Comment("reaching state \'S1145\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp227;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp227 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S1237\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp227, "return of NetrServerReqChallenge, state S1237");
                this.Manager.Comment("reaching state \'S1326\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp228;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,32772)\'");
                temp228 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 32772u);
                this.Manager.Checkpoint("MS-NRPC_R103371");
                this.Manager.Checkpoint("MS-NRPC_R103456");
                this.Manager.Comment("reaching state \'S1401\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/STATUS_DOWNGRADE_DETECTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_DOWNGRADE_DETECTED, temp228, "return of NetrServerAuthenticate3, state S1401");
                this.Manager.Comment("reaching state \'S1460\'");
                goto label23;
            }
            if ((temp234 == 1)) {
                this.Manager.Comment("reaching state \'S169\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S349\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S529\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp229;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp229 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S709\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp229, "return of NetrServerReqChallenge, state S709");
                this.Manager.Comment("reaching state \'S889\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp230;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,32772)\'");
                temp230 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 32772u);
                this.Manager.Checkpoint("MS-NRPC_R103371");
                this.Manager.Checkpoint("MS-NRPC_R103456");
                this.Manager.Comment("reaching state \'S1054\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/STATUS_DOWNGRADE_DETECTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_DOWNGRADE_DETECTED, temp230, "return of NetrServerAuthenticate3, state S1054");
                this.Manager.Comment("reaching state \'S1146\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp231;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp231 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S1238\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp231, "return of NetrServerReqChallenge, state S1238");
                this.Manager.Comment("reaching state \'S1327\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp232;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,32772)\'");
                temp232 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 32772u);
                this.Manager.Checkpoint("MS-NRPC_R103371");
                this.Manager.Checkpoint("MS-NRPC_R103456");
                this.Manager.Comment("reaching state \'S1402\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/STATUS_DOWNGRADE_DETECTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_DOWNGRADE_DETECTED, temp232, "return of NetrServerAuthenticate3, state S1402");
                this.Manager.Comment("reaching state \'S1461\'");
                goto label23;
            }
            if ((temp234 == 2)) {
                this.Manager.Comment("reaching state \'S170\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S350\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S530\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp233;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(NonDcServer,Client)\'");
                temp233 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103509");
                this.Manager.Checkpoint("MS-NRPC_R103341");
                this.Manager.Comment("reaching state \'S710\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp233, "return of NetrServerReqChallenge, state S710");
                this.Manager.Comment("reaching state \'S890\'");
                goto label23;
            }
            throw new InvalidOperationException("never reached");
        label23:
;
            this.Manager.EndTest();
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate3S32GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S33");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate3S32GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S33");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate3S32GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S33");
        }
        #endregion
        
        #region Test Starting in S34
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_EstablishSecureChannel_NetrServerAuthenticate3S34() {
            this.Manager.BeginTest("Test_EstablishSecureChannel_NetrServerAuthenticate3S34");
            this.Manager.Comment("reaching state \'S34\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp235;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp235);
            this.Manager.AddReturn(GetPlatformInfo, null, temp235);
            this.Manager.Comment("reaching state \'S35\'");
            int temp245 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate3.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate3S34GetPlatformChecker)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate3.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate3S34GetPlatformChecker1)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate3.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate3S34GetPlatformChecker2)));
            if ((temp245 == 0)) {
                this.Manager.Comment("reaching state \'S171\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S351\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S531\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp236;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp236 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S711\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp236, "return of NetrServerReqChallenge, state S711");
                this.Manager.Comment("reaching state \'S891\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp237;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,2147483652)\'");
                temp237 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 2147483652u);
                this.Manager.Checkpoint("MS-NRPC_R103371");
                this.Manager.Checkpoint("MS-NRPC_R103456");
                this.Manager.Comment("reaching state \'S1055\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/STATUS_DOWNGRADE_DETECTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_DOWNGRADE_DETECTED, temp237, "return of NetrServerAuthenticate3, state S1055");
                this.Manager.Comment("reaching state \'S1147\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp238;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp238 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S1239\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp238, "return of NetrServerReqChallenge, state S1239");
                this.Manager.Comment("reaching state \'S1328\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp239;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,4)\'");
                temp239 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 4u);
                this.Manager.Checkpoint("MS-NRPC_R103371");
                this.Manager.Checkpoint("MS-NRPC_R897");
                this.Manager.Checkpoint("MS-NRPC_R103456");
                this.Manager.AddReturn(NetrServerAuthenticate3Info, null, temp239);
                Test_EstablishSecureChannel_NetrServerAuthenticate3S1385();
                goto label24;
            }
            if ((temp245 == 1)) {
                this.Manager.Comment("reaching state \'S172\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S352\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S532\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp240;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp240 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S712\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp240, "return of NetrServerReqChallenge, state S712");
                this.Manager.Comment("reaching state \'S892\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp241;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,65540)\'");
                temp241 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 65540u);
                this.Manager.Checkpoint("MS-NRPC_R103371");
                this.Manager.Checkpoint("MS-NRPC_R103456");
                this.Manager.Comment("reaching state \'S1056\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/STATUS_DOWNGRADE_DETECTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_DOWNGRADE_DETECTED, temp241, "return of NetrServerAuthenticate3, state S1056");
                this.Manager.Comment("reaching state \'S1148\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp242;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp242 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S1240\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp242, "return of NetrServerReqChallenge, state S1240");
                this.Manager.Comment("reaching state \'S1329\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp243;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,4)\'");
                temp243 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 4u);
                this.Manager.Checkpoint("MS-NRPC_R103371");
                this.Manager.Checkpoint("MS-NRPC_R893");
                this.Manager.Checkpoint("MS-NRPC_R103456");
                this.Manager.AddReturn(NetrServerAuthenticate3Info, null, temp243);
                Test_EstablishSecureChannel_NetrServerAuthenticate3S1386();
                goto label24;
            }
            if ((temp245 == 2)) {
                this.Manager.Comment("reaching state \'S173\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S353\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S533\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp244;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(NonDcServer,Client)\'");
                temp244 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103509");
                this.Manager.Checkpoint("MS-NRPC_R103341");
                this.Manager.Comment("reaching state \'S713\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp244, "return of NetrServerReqChallenge, state S713");
                this.Manager.Comment("reaching state \'S893\'");
                goto label24;
            }
            throw new InvalidOperationException("never reached");
        label24:
;
            this.Manager.EndTest();
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate3S34GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S35");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate3S34GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S35");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate3S34GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S35");
        }
        #endregion
        
        #region Test Starting in S36
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_EstablishSecureChannel_NetrServerAuthenticate3S36() {
            this.Manager.BeginTest("Test_EstablishSecureChannel_NetrServerAuthenticate3S36");
            this.Manager.Comment("reaching state \'S36\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp246;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp246);
            this.Manager.AddReturn(GetPlatformInfo, null, temp246);
            this.Manager.Comment("reaching state \'S37\'");
            int temp256 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate3.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate3S36GetPlatformChecker)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate3.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate3S36GetPlatformChecker1)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate3.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate3S36GetPlatformChecker2)));
            if ((temp256 == 0)) {
                this.Manager.Comment("reaching state \'S174\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S354\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S534\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp247;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp247 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S714\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp247, "return of NetrServerReqChallenge, state S714");
                this.Manager.Comment("reaching state \'S894\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp248;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,2097156)\'");
                temp248 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 2097156u);
                this.Manager.Checkpoint("MS-NRPC_R103371");
                this.Manager.Checkpoint("MS-NRPC_R103456");
                this.Manager.Comment("reaching state \'S1057\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/STATUS_DOWNGRADE_DETECTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_DOWNGRADE_DETECTED, temp248, "return of NetrServerAuthenticate3, state S1057");
                this.Manager.Comment("reaching state \'S1149\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp249;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp249 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S1241\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp249, "return of NetrServerReqChallenge, state S1241");
                this.Manager.Comment("reaching state \'S1330\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp250;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,2097156)\'");
                temp250 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 2097156u);
                this.Manager.Checkpoint("MS-NRPC_R103371");
                this.Manager.Checkpoint("MS-NRPC_R103456");
                this.Manager.Comment("reaching state \'S1403\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/STATUS_DOWNGRADE_DETECTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_DOWNGRADE_DETECTED, temp250, "return of NetrServerAuthenticate3, state S1403");
                this.Manager.Comment("reaching state \'S1462\'");
                goto label25;
            }
            if ((temp256 == 1)) {
                this.Manager.Comment("reaching state \'S175\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S355\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S535\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp251;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp251 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S715\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp251, "return of NetrServerReqChallenge, state S715");
                this.Manager.Comment("reaching state \'S895\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp252;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,2147483652)\'");
                temp252 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 2147483652u);
                this.Manager.Checkpoint("MS-NRPC_R103371");
                this.Manager.Checkpoint("MS-NRPC_R103456");
                this.Manager.Comment("reaching state \'S1058\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/STATUS_DOWNGRADE_DETECTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_DOWNGRADE_DETECTED, temp252, "return of NetrServerAuthenticate3, state S1058");
                this.Manager.Comment("reaching state \'S1150\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp253;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp253 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S1242\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp253, "return of NetrServerReqChallenge, state S1242");
                this.Manager.Comment("reaching state \'S1331\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp254;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,4)\'");
                temp254 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 4u);
                this.Manager.Checkpoint("MS-NRPC_R103371");
                this.Manager.Checkpoint("MS-NRPC_R897");
                this.Manager.Checkpoint("MS-NRPC_R103456");
                this.Manager.AddReturn(NetrServerAuthenticate3Info, null, temp254);
                Test_EstablishSecureChannel_NetrServerAuthenticate3S1386();
                goto label25;
            }
            if ((temp256 == 2)) {
                this.Manager.Comment("reaching state \'S176\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S356\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S536\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp255;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(NonDcServer,Client)\'");
                temp255 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103509");
                this.Manager.Checkpoint("MS-NRPC_R103341");
                this.Manager.Comment("reaching state \'S716\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp255, "return of NetrServerReqChallenge, state S716");
                this.Manager.Comment("reaching state \'S896\'");
                goto label25;
            }
            throw new InvalidOperationException("never reached");
        label25:
;
            this.Manager.EndTest();
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate3S36GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S37");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate3S36GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S37");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate3S36GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S37");
        }
        #endregion
        
        #region Test Starting in S38
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_EstablishSecureChannel_NetrServerAuthenticate3S38() {
            this.Manager.BeginTest("Test_EstablishSecureChannel_NetrServerAuthenticate3S38");
            this.Manager.Comment("reaching state \'S38\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp257;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp257);
            this.Manager.AddReturn(GetPlatformInfo, null, temp257);
            this.Manager.Comment("reaching state \'S39\'");
            int temp267 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate3.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate3S38GetPlatformChecker)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate3.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate3S38GetPlatformChecker1)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate3.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate3S38GetPlatformChecker2)));
            if ((temp267 == 0)) {
                this.Manager.Comment("reaching state \'S177\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S357\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S537\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp258;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp258 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S717\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp258, "return of NetrServerReqChallenge, state S717");
                this.Manager.Comment("reaching state \'S897\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp259;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,1073741828)\'");
                temp259 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 1073741828u);
                this.Manager.Checkpoint("MS-NRPC_R103371");
                this.Manager.Checkpoint("MS-NRPC_R103456");
                this.Manager.Comment("reaching state \'S1059\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/STATUS_DOWNGRADE_DETECTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_DOWNGRADE_DETECTED, temp259, "return of NetrServerAuthenticate3, state S1059");
                this.Manager.Comment("reaching state \'S1151\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp260;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp260 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S1243\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp260, "return of NetrServerReqChallenge, state S1243");
                this.Manager.Comment("reaching state \'S1332\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp261;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,1073741828)\'");
                temp261 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 1073741828u);
                this.Manager.Checkpoint("MS-NRPC_R103371");
                this.Manager.Checkpoint("MS-NRPC_R103456");
                this.Manager.Comment("reaching state \'S1404\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/STATUS_DOWNGRADE_DETECTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_DOWNGRADE_DETECTED, temp261, "return of NetrServerAuthenticate3, state S1404");
                this.Manager.Comment("reaching state \'S1463\'");
                goto label26;
            }
            if ((temp267 == 1)) {
                this.Manager.Comment("reaching state \'S178\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S358\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S538\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp262;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp262 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S718\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp262, "return of NetrServerReqChallenge, state S718");
                this.Manager.Comment("reaching state \'S898\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp263;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,131076)\'");
                temp263 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 131076u);
                this.Manager.Checkpoint("MS-NRPC_R103371");
                this.Manager.Checkpoint("MS-NRPC_R103456");
                this.Manager.Comment("reaching state \'S1060\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/STATUS_DOWNGRADE_DETECTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_DOWNGRADE_DETECTED, temp263, "return of NetrServerAuthenticate3, state S1060");
                this.Manager.Comment("reaching state \'S1152\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp264;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp264 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S1244\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp264, "return of NetrServerReqChallenge, state S1244");
                this.Manager.Comment("reaching state \'S1333\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp265;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,131076)\'");
                temp265 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 131076u);
                this.Manager.Checkpoint("MS-NRPC_R103371");
                this.Manager.Checkpoint("MS-NRPC_R103456");
                this.Manager.Comment("reaching state \'S1405\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/STATUS_DOWNGRADE_DETECTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_DOWNGRADE_DETECTED, temp265, "return of NetrServerAuthenticate3, state S1405");
                this.Manager.Comment("reaching state \'S1464\'");
                goto label26;
            }
            if ((temp267 == 2)) {
                this.Manager.Comment("reaching state \'S179\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S359\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S539\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp266;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(NonDcServer,Client)\'");
                temp266 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103509");
                this.Manager.Checkpoint("MS-NRPC_R103341");
                this.Manager.Comment("reaching state \'S719\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp266, "return of NetrServerReqChallenge, state S719");
                this.Manager.Comment("reaching state \'S899\'");
                goto label26;
            }
            throw new InvalidOperationException("never reached");
        label26:
;
            this.Manager.EndTest();
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate3S38GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S39");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate3S38GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S39");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate3S38GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S39");
        }
        #endregion
        
        #region Test Starting in S4
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_EstablishSecureChannel_NetrServerAuthenticate3S4() {
            this.Manager.BeginTest("Test_EstablishSecureChannel_NetrServerAuthenticate3S4");
            this.Manager.Comment("reaching state \'S4\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp268;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp268);
            this.Manager.AddReturn(GetPlatformInfo, null, temp268);
            this.Manager.Comment("reaching state \'S5\'");
            int temp278 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate3.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate3S4GetPlatformChecker)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate3.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate3S4GetPlatformChecker1)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate3.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate3S4GetPlatformChecker2)));
            if ((temp278 == 0)) {
                this.Manager.Comment("reaching state \'S126\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S306\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S486\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp269;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp269 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S666\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp269, "return of NetrServerReqChallenge, state S666");
                this.Manager.Comment("reaching state \'S846\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp270;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(NonDcServer,DomainMemberComputerAcco" +
                        "unt,WorkstationSecureChannel,Client,True,16388)\'");
                temp270 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16388u);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103513");
                this.Manager.Checkpoint("MS-NRPC_R103456");
                this.Manager.Comment("reaching state \'S1025\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp270, "return of NetrServerAuthenticate3, state S1025");
                this.Manager.Comment("reaching state \'S1117\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp271;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp271 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.AddReturn(NetrServerReqChallengeInfo, null, temp271);
                Test_EstablishSecureChannel_NetrServerAuthenticate3S1209();
                goto label27;
            }
            if ((temp278 == 1)) {
                this.Manager.Comment("reaching state \'S127\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S307\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S487\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp273;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp273 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S667\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp273, "return of NetrServerReqChallenge, state S667");
                this.Manager.Comment("reaching state \'S847\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp274;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(NonDcServer,DomainMemberComputerAcco" +
                        "unt,WorkstationSecureChannel,Client,True,16388)\'");
                temp274 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16388u);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103513");
                this.Manager.Checkpoint("MS-NRPC_R103456");
                this.Manager.Comment("reaching state \'S1026\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp274, "return of NetrServerAuthenticate3, state S1026");
                this.Manager.Comment("reaching state \'S1118\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp275;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp275 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.AddReturn(NetrServerReqChallengeInfo, null, temp275);
                Test_EstablishSecureChannel_NetrServerAuthenticate3S1210();
                goto label27;
            }
            if ((temp278 == 2)) {
                this.Manager.Comment("reaching state \'S128\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S308\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S488\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp277;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(NonDcServer,Client)\'");
                temp277 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103509");
                this.Manager.Checkpoint("MS-NRPC_R103341");
                this.Manager.Comment("reaching state \'S668\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp277, "return of NetrServerReqChallenge, state S668");
                this.Manager.Comment("reaching state \'S848\'");
                goto label27;
            }
            throw new InvalidOperationException("never reached");
        label27:
;
            this.Manager.EndTest();
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate3S4GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S5");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate3S1209() {
            this.Manager.Comment("reaching state \'S1209\'");
            this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate3.NetrServerReqChallengeInfo, null, new NetrServerReqChallengeDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate3S4NetrServerReqChallengeChecker)));
            this.Manager.Comment("reaching state \'S1298\'");
            Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp272;
            this.Manager.Comment("executing step \'call NetrServerAuthenticate3(NonDcServer,DomainMemberComputerAcco" +
                    "unt,WorkstationSecureChannel,Client,True,16388)\'");
            temp272 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16388u);
            this.Manager.Checkpoint("MS-NRPC_R425");
            this.Manager.Checkpoint("MS-NRPC_R103513");
            this.Manager.Checkpoint("MS-NRPC_R103456");
            this.Manager.Comment("reaching state \'S1387\'");
            this.Manager.Comment("checking step \'return NetrServerAuthenticate3/STATUS_NOT_SUPPORTED\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp272, "return of NetrServerAuthenticate3, state S1387");
            this.Manager.Comment("reaching state \'S1446\'");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate3S4NetrServerReqChallengeChecker(Microsoft.Protocols.TestSuites.Nrpc.HRESULT @return) {
            this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), @return, "return of NetrServerReqChallenge, state S1209");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate3S4GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S5");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate3S1210() {
            this.Manager.Comment("reaching state \'S1210\'");
            this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate3.NetrServerReqChallengeInfo, null, new NetrServerReqChallengeDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate3S4NetrServerReqChallengeChecker1)));
            this.Manager.Comment("reaching state \'S1299\'");
            Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp276;
            this.Manager.Comment("executing step \'call NetrServerAuthenticate3(NonDcServer,DomainMemberComputerAcco" +
                    "unt,WorkstationSecureChannel,Client,True,16388)\'");
            temp276 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16388u);
            this.Manager.Checkpoint("MS-NRPC_R425");
            this.Manager.Checkpoint("MS-NRPC_R103513");
            this.Manager.Checkpoint("MS-NRPC_R103456");
            this.Manager.Comment("reaching state \'S1388\'");
            this.Manager.Comment("checking step \'return NetrServerAuthenticate3/STATUS_NOT_SUPPORTED\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp276, "return of NetrServerAuthenticate3, state S1388");
            this.Manager.Comment("reaching state \'S1447\'");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate3S4NetrServerReqChallengeChecker1(Microsoft.Protocols.TestSuites.Nrpc.HRESULT @return) {
            this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), @return, "return of NetrServerReqChallenge, state S1210");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate3S4GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S5");
        }
        #endregion
        
        #region Test Starting in S40
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_EstablishSecureChannel_NetrServerAuthenticate3S40() {
            this.Manager.BeginTest("Test_EstablishSecureChannel_NetrServerAuthenticate3S40");
            this.Manager.Comment("reaching state \'S40\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp279;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp279);
            this.Manager.AddReturn(GetPlatformInfo, null, temp279);
            this.Manager.Comment("reaching state \'S41\'");
            int temp289 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate3.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate3S40GetPlatformChecker)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate3.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate3S40GetPlatformChecker1)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate3.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate3S40GetPlatformChecker2)));
            if ((temp289 == 0)) {
                this.Manager.Comment("reaching state \'S180\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S360\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S540\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp280;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp280 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S720\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp280, "return of NetrServerReqChallenge, state S720");
                this.Manager.Comment("reaching state \'S900\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp281;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,536870916)\'");
                temp281 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 536870916u);
                this.Manager.Checkpoint("MS-NRPC_R103371");
                this.Manager.Checkpoint("MS-NRPC_R103456");
                this.Manager.Comment("reaching state \'S1061\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/STATUS_DOWNGRADE_DETECTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_DOWNGRADE_DETECTED, temp281, "return of NetrServerAuthenticate3, state S1061");
                this.Manager.Comment("reaching state \'S1153\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp282;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp282 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S1245\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp282, "return of NetrServerReqChallenge, state S1245");
                this.Manager.Comment("reaching state \'S1334\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp283;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,4)\'");
                temp283 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 4u);
                this.Manager.Checkpoint("MS-NRPC_R103371");
                this.Manager.Checkpoint("MS-NRPC_R894");
                this.Manager.Checkpoint("MS-NRPC_R103456");
                this.Manager.AddReturn(NetrServerAuthenticate3Info, null, temp283);
                Test_EstablishSecureChannel_NetrServerAuthenticate3S1385();
                goto label28;
            }
            if ((temp289 == 1)) {
                this.Manager.Comment("reaching state \'S181\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S361\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S541\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp284;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp284 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S721\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp284, "return of NetrServerReqChallenge, state S721");
                this.Manager.Comment("reaching state \'S901\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp285;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,262148)\'");
                temp285 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 262148u);
                this.Manager.Checkpoint("MS-NRPC_R103371");
                this.Manager.Checkpoint("MS-NRPC_R103456");
                this.Manager.Comment("reaching state \'S1062\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/STATUS_DOWNGRADE_DETECTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_DOWNGRADE_DETECTED, temp285, "return of NetrServerAuthenticate3, state S1062");
                this.Manager.Comment("reaching state \'S1154\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp286;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp286 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S1246\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp286, "return of NetrServerReqChallenge, state S1246");
                this.Manager.Comment("reaching state \'S1335\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp287;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,262148)\'");
                temp287 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 262148u);
                this.Manager.Checkpoint("MS-NRPC_R103371");
                this.Manager.Checkpoint("MS-NRPC_R103456");
                this.Manager.Comment("reaching state \'S1406\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/STATUS_DOWNGRADE_DETECTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_DOWNGRADE_DETECTED, temp287, "return of NetrServerAuthenticate3, state S1406");
                this.Manager.Comment("reaching state \'S1465\'");
                goto label28;
            }
            if ((temp289 == 2)) {
                this.Manager.Comment("reaching state \'S182\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S362\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S542\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp288;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(NonDcServer,Client)\'");
                temp288 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103509");
                this.Manager.Checkpoint("MS-NRPC_R103341");
                this.Manager.Comment("reaching state \'S722\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp288, "return of NetrServerReqChallenge, state S722");
                this.Manager.Comment("reaching state \'S902\'");
                goto label28;
            }
            throw new InvalidOperationException("never reached");
        label28:
;
            this.Manager.EndTest();
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate3S40GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S41");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate3S40GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S41");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate3S40GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S41");
        }
        #endregion
        
        #region Test Starting in S42
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_EstablishSecureChannel_NetrServerAuthenticate3S42() {
            this.Manager.BeginTest("Test_EstablishSecureChannel_NetrServerAuthenticate3S42");
            this.Manager.Comment("reaching state \'S42\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp290;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp290);
            this.Manager.AddReturn(GetPlatformInfo, null, temp290);
            this.Manager.Comment("reaching state \'S43\'");
            int temp300 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate3.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate3S42GetPlatformChecker)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate3.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate3S42GetPlatformChecker1)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate3.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate3S42GetPlatformChecker2)));
            if ((temp300 == 0)) {
                this.Manager.Comment("reaching state \'S183\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S363\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S543\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp291;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp291 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S723\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp291, "return of NetrServerReqChallenge, state S723");
                this.Manager.Comment("reaching state \'S903\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp292;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,65540)\'");
                temp292 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 65540u);
                this.Manager.Checkpoint("MS-NRPC_R103371");
                this.Manager.Checkpoint("MS-NRPC_R103456");
                this.Manager.Comment("reaching state \'S1063\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/STATUS_DOWNGRADE_DETECTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_DOWNGRADE_DETECTED, temp292, "return of NetrServerAuthenticate3, state S1063");
                this.Manager.Comment("reaching state \'S1155\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp293;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp293 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S1247\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp293, "return of NetrServerReqChallenge, state S1247");
                this.Manager.Comment("reaching state \'S1336\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp294;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,4)\'");
                temp294 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 4u);
                this.Manager.Checkpoint("MS-NRPC_R103371");
                this.Manager.Checkpoint("MS-NRPC_R893");
                this.Manager.Checkpoint("MS-NRPC_R103456");
                this.Manager.AddReturn(NetrServerAuthenticate3Info, null, temp294);
                Test_EstablishSecureChannel_NetrServerAuthenticate3S1385();
                goto label29;
            }
            if ((temp300 == 1)) {
                this.Manager.Comment("reaching state \'S184\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S364\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S544\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp295;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp295 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S724\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp295, "return of NetrServerReqChallenge, state S724");
                this.Manager.Comment("reaching state \'S904\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp296;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,524292)\'");
                temp296 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 524292u);
                this.Manager.Checkpoint("MS-NRPC_R103371");
                this.Manager.Checkpoint("MS-NRPC_R103456");
                this.Manager.Comment("reaching state \'S1064\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/STATUS_DOWNGRADE_DETECTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_DOWNGRADE_DETECTED, temp296, "return of NetrServerAuthenticate3, state S1064");
                this.Manager.Comment("reaching state \'S1156\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp297;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp297 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S1248\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp297, "return of NetrServerReqChallenge, state S1248");
                this.Manager.Comment("reaching state \'S1337\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp298;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,524292)\'");
                temp298 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 524292u);
                this.Manager.Checkpoint("MS-NRPC_R103371");
                this.Manager.Checkpoint("MS-NRPC_R103456");
                this.Manager.Comment("reaching state \'S1407\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/STATUS_DOWNGRADE_DETECTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_DOWNGRADE_DETECTED, temp298, "return of NetrServerAuthenticate3, state S1407");
                this.Manager.Comment("reaching state \'S1466\'");
                goto label29;
            }
            if ((temp300 == 2)) {
                this.Manager.Comment("reaching state \'S185\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S365\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S545\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp299;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(NonDcServer,Client)\'");
                temp299 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103509");
                this.Manager.Checkpoint("MS-NRPC_R103341");
                this.Manager.Comment("reaching state \'S725\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp299, "return of NetrServerReqChallenge, state S725");
                this.Manager.Comment("reaching state \'S905\'");
                goto label29;
            }
            throw new InvalidOperationException("never reached");
        label29:
;
            this.Manager.EndTest();
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate3S42GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S43");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate3S42GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S43");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate3S42GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S43");
        }
        #endregion
        
        #region Test Starting in S44
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_EstablishSecureChannel_NetrServerAuthenticate3S44() {
            this.Manager.BeginTest("Test_EstablishSecureChannel_NetrServerAuthenticate3S44");
            this.Manager.Comment("reaching state \'S44\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp301;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp301);
            this.Manager.AddReturn(GetPlatformInfo, null, temp301);
            this.Manager.Comment("reaching state \'S45\'");
            int temp311 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate3.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate3S44GetPlatformChecker)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate3.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate3S44GetPlatformChecker1)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate3.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate3S44GetPlatformChecker2)));
            if ((temp311 == 0)) {
                this.Manager.Comment("reaching state \'S186\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S366\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S546\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp302;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp302 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S726\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp302, "return of NetrServerReqChallenge, state S726");
                this.Manager.Comment("reaching state \'S906\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp303;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,131076)\'");
                temp303 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 131076u);
                this.Manager.Checkpoint("MS-NRPC_R103371");
                this.Manager.Checkpoint("MS-NRPC_R103456");
                this.Manager.Comment("reaching state \'S1065\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/STATUS_DOWNGRADE_DETECTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_DOWNGRADE_DETECTED, temp303, "return of NetrServerAuthenticate3, state S1065");
                this.Manager.Comment("reaching state \'S1157\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp304;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp304 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S1249\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp304, "return of NetrServerReqChallenge, state S1249");
                this.Manager.Comment("reaching state \'S1338\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp305;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,131076)\'");
                temp305 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 131076u);
                this.Manager.Checkpoint("MS-NRPC_R103371");
                this.Manager.Checkpoint("MS-NRPC_R103456");
                this.Manager.Comment("reaching state \'S1408\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/STATUS_DOWNGRADE_DETECTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_DOWNGRADE_DETECTED, temp305, "return of NetrServerAuthenticate3, state S1408");
                this.Manager.Comment("reaching state \'S1467\'");
                goto label30;
            }
            if ((temp311 == 1)) {
                this.Manager.Comment("reaching state \'S187\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S367\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S547\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp306;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp306 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S727\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp306, "return of NetrServerReqChallenge, state S727");
                this.Manager.Comment("reaching state \'S907\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp307;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,1048580)\'");
                temp307 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 1048580u);
                this.Manager.Checkpoint("MS-NRPC_R103371");
                this.Manager.Checkpoint("MS-NRPC_R103456");
                this.Manager.Comment("reaching state \'S1066\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/STATUS_DOWNGRADE_DETECTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_DOWNGRADE_DETECTED, temp307, "return of NetrServerAuthenticate3, state S1066");
                this.Manager.Comment("reaching state \'S1158\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp308;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp308 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S1250\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp308, "return of NetrServerReqChallenge, state S1250");
                this.Manager.Comment("reaching state \'S1339\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp309;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,1048580)\'");
                temp309 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 1048580u);
                this.Manager.Checkpoint("MS-NRPC_R103371");
                this.Manager.Checkpoint("MS-NRPC_R103456");
                this.Manager.Comment("reaching state \'S1409\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/STATUS_DOWNGRADE_DETECTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_DOWNGRADE_DETECTED, temp309, "return of NetrServerAuthenticate3, state S1409");
                this.Manager.Comment("reaching state \'S1468\'");
                goto label30;
            }
            if ((temp311 == 2)) {
                this.Manager.Comment("reaching state \'S188\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S368\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S548\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp310;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(NonDcServer,Client)\'");
                temp310 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103509");
                this.Manager.Checkpoint("MS-NRPC_R103341");
                this.Manager.Comment("reaching state \'S728\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp310, "return of NetrServerReqChallenge, state S728");
                this.Manager.Comment("reaching state \'S908\'");
                goto label30;
            }
            throw new InvalidOperationException("never reached");
        label30:
;
            this.Manager.EndTest();
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate3S44GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S45");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate3S44GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S45");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate3S44GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S45");
        }
        #endregion
        
        #region Test Starting in S46
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_EstablishSecureChannel_NetrServerAuthenticate3S46() {
            this.Manager.BeginTest("Test_EstablishSecureChannel_NetrServerAuthenticate3S46");
            this.Manager.Comment("reaching state \'S46\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp312;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp312);
            this.Manager.AddReturn(GetPlatformInfo, null, temp312);
            this.Manager.Comment("reaching state \'S47\'");
            int temp322 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate3.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate3S46GetPlatformChecker)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate3.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate3S46GetPlatformChecker1)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate3.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate3S46GetPlatformChecker2)));
            if ((temp322 == 0)) {
                this.Manager.Comment("reaching state \'S189\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S369\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S549\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp313;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp313 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S729\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp313, "return of NetrServerReqChallenge, state S729");
                this.Manager.Comment("reaching state \'S909\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp314;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,262148)\'");
                temp314 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 262148u);
                this.Manager.Checkpoint("MS-NRPC_R103371");
                this.Manager.Checkpoint("MS-NRPC_R103456");
                this.Manager.Comment("reaching state \'S1067\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/STATUS_DOWNGRADE_DETECTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_DOWNGRADE_DETECTED, temp314, "return of NetrServerAuthenticate3, state S1067");
                this.Manager.Comment("reaching state \'S1159\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp315;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp315 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S1251\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp315, "return of NetrServerReqChallenge, state S1251");
                this.Manager.Comment("reaching state \'S1340\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp316;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,262148)\'");
                temp316 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 262148u);
                this.Manager.Checkpoint("MS-NRPC_R103371");
                this.Manager.Checkpoint("MS-NRPC_R103456");
                this.Manager.Comment("reaching state \'S1410\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/STATUS_DOWNGRADE_DETECTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_DOWNGRADE_DETECTED, temp316, "return of NetrServerAuthenticate3, state S1410");
                this.Manager.Comment("reaching state \'S1469\'");
                goto label31;
            }
            if ((temp322 == 1)) {
                this.Manager.Comment("reaching state \'S190\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S370\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S550\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp317;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp317 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S730\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp317, "return of NetrServerReqChallenge, state S730");
                this.Manager.Comment("reaching state \'S910\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp318;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,2097156)\'");
                temp318 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 2097156u);
                this.Manager.Checkpoint("MS-NRPC_R103371");
                this.Manager.Checkpoint("MS-NRPC_R103456");
                this.Manager.Comment("reaching state \'S1068\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/STATUS_DOWNGRADE_DETECTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_DOWNGRADE_DETECTED, temp318, "return of NetrServerAuthenticate3, state S1068");
                this.Manager.Comment("reaching state \'S1160\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp319;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp319 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S1252\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp319, "return of NetrServerReqChallenge, state S1252");
                this.Manager.Comment("reaching state \'S1341\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp320;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,2097156)\'");
                temp320 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 2097156u);
                this.Manager.Checkpoint("MS-NRPC_R103371");
                this.Manager.Checkpoint("MS-NRPC_R103456");
                this.Manager.Comment("reaching state \'S1411\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/STATUS_DOWNGRADE_DETECTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_DOWNGRADE_DETECTED, temp320, "return of NetrServerAuthenticate3, state S1411");
                this.Manager.Comment("reaching state \'S1470\'");
                goto label31;
            }
            if ((temp322 == 2)) {
                this.Manager.Comment("reaching state \'S191\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S371\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S551\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp321;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(NonDcServer,Client)\'");
                temp321 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103509");
                this.Manager.Checkpoint("MS-NRPC_R103341");
                this.Manager.Comment("reaching state \'S731\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp321, "return of NetrServerReqChallenge, state S731");
                this.Manager.Comment("reaching state \'S911\'");
                goto label31;
            }
            throw new InvalidOperationException("never reached");
        label31:
;
            this.Manager.EndTest();
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate3S46GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S47");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate3S46GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S47");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate3S46GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S47");
        }
        #endregion
        
        #region Test Starting in S48
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_EstablishSecureChannel_NetrServerAuthenticate3S48() {
            this.Manager.BeginTest("Test_EstablishSecureChannel_NetrServerAuthenticate3S48");
            this.Manager.Comment("reaching state \'S48\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp323;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp323);
            this.Manager.AddReturn(GetPlatformInfo, null, temp323);
            this.Manager.Comment("reaching state \'S49\'");
            int temp333 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate3.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate3S48GetPlatformChecker)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate3.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate3S48GetPlatformChecker1)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate3.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate3S48GetPlatformChecker2)));
            if ((temp333 == 0)) {
                this.Manager.Comment("reaching state \'S192\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S372\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S552\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp324;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp324 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S732\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp324, "return of NetrServerReqChallenge, state S732");
                this.Manager.Comment("reaching state \'S912\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp325;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,524292)\'");
                temp325 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 524292u);
                this.Manager.Checkpoint("MS-NRPC_R103371");
                this.Manager.Checkpoint("MS-NRPC_R103456");
                this.Manager.Comment("reaching state \'S1069\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/STATUS_DOWNGRADE_DETECTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_DOWNGRADE_DETECTED, temp325, "return of NetrServerAuthenticate3, state S1069");
                this.Manager.Comment("reaching state \'S1161\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp326;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp326 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S1253\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp326, "return of NetrServerReqChallenge, state S1253");
                this.Manager.Comment("reaching state \'S1342\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp327;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,524292)\'");
                temp327 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 524292u);
                this.Manager.Checkpoint("MS-NRPC_R103371");
                this.Manager.Checkpoint("MS-NRPC_R103456");
                this.Manager.Comment("reaching state \'S1412\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/STATUS_DOWNGRADE_DETECTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_DOWNGRADE_DETECTED, temp327, "return of NetrServerAuthenticate3, state S1412");
                this.Manager.Comment("reaching state \'S1471\'");
                goto label32;
            }
            if ((temp333 == 1)) {
                this.Manager.Comment("reaching state \'S193\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S373\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S553\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp328;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp328 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S733\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp328, "return of NetrServerReqChallenge, state S733");
                this.Manager.Comment("reaching state \'S913\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp329;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16777220)\'");
                temp329 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16777220u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S1070\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp329, "return of NetrServerAuthenticate3, state S1070");
                this.Manager.Comment("reaching state \'S1162\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp330;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp330 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S1254\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp330, "return of NetrServerReqChallenge, state S1254");
                this.Manager.Comment("reaching state \'S1343\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp331;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16777220)\'");
                temp331 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16777220u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S1413\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp331, "return of NetrServerAuthenticate3, state S1413");
                this.Manager.Comment("reaching state \'S1472\'");
                goto label32;
            }
            if ((temp333 == 2)) {
                this.Manager.Comment("reaching state \'S194\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S374\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S554\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp332;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(NonDcServer,Client)\'");
                temp332 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103509");
                this.Manager.Checkpoint("MS-NRPC_R103341");
                this.Manager.Comment("reaching state \'S734\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp332, "return of NetrServerReqChallenge, state S734");
                this.Manager.Comment("reaching state \'S914\'");
                goto label32;
            }
            throw new InvalidOperationException("never reached");
        label32:
;
            this.Manager.EndTest();
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate3S48GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S49");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate3S48GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S49");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate3S48GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S49");
        }
        #endregion
        
        #region Test Starting in S50
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_EstablishSecureChannel_NetrServerAuthenticate3S50() {
            this.Manager.BeginTest("Test_EstablishSecureChannel_NetrServerAuthenticate3S50");
            this.Manager.Comment("reaching state \'S50\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp334;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp334);
            this.Manager.AddReturn(GetPlatformInfo, null, temp334);
            this.Manager.Comment("reaching state \'S51\'");
            int temp344 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate3.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate3S50GetPlatformChecker)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate3.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate3S50GetPlatformChecker1)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate3.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate3S50GetPlatformChecker2)));
            if ((temp344 == 0)) {
                this.Manager.Comment("reaching state \'S195\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S375\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S555\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp335;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp335 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S735\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp335, "return of NetrServerReqChallenge, state S735");
                this.Manager.Comment("reaching state \'S915\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp336;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,1048580)\'");
                temp336 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 1048580u);
                this.Manager.Checkpoint("MS-NRPC_R103371");
                this.Manager.Checkpoint("MS-NRPC_R103456");
                this.Manager.Comment("reaching state \'S1071\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/STATUS_DOWNGRADE_DETECTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_DOWNGRADE_DETECTED, temp336, "return of NetrServerAuthenticate3, state S1071");
                this.Manager.Comment("reaching state \'S1163\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp337;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp337 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S1255\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp337, "return of NetrServerReqChallenge, state S1255");
                this.Manager.Comment("reaching state \'S1344\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp338;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,1048580)\'");
                temp338 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 1048580u);
                this.Manager.Checkpoint("MS-NRPC_R103371");
                this.Manager.Checkpoint("MS-NRPC_R103456");
                this.Manager.Comment("reaching state \'S1414\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/STATUS_DOWNGRADE_DETECTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_DOWNGRADE_DETECTED, temp338, "return of NetrServerAuthenticate3, state S1414");
                this.Manager.Comment("reaching state \'S1473\'");
                goto label33;
            }
            if ((temp344 == 1)) {
                this.Manager.Comment("reaching state \'S196\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S376\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S556\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp339;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp339 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S736\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp339, "return of NetrServerReqChallenge, state S736");
                this.Manager.Comment("reaching state \'S916\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp340;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,1073741828)\'");
                temp340 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 1073741828u);
                this.Manager.Checkpoint("MS-NRPC_R103371");
                this.Manager.Checkpoint("MS-NRPC_R103456");
                this.Manager.Comment("reaching state \'S1072\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/STATUS_DOWNGRADE_DETECTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_DOWNGRADE_DETECTED, temp340, "return of NetrServerAuthenticate3, state S1072");
                this.Manager.Comment("reaching state \'S1164\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp341;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp341 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S1256\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp341, "return of NetrServerReqChallenge, state S1256");
                this.Manager.Comment("reaching state \'S1345\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp342;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,1073741828)\'");
                temp342 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 1073741828u);
                this.Manager.Checkpoint("MS-NRPC_R103371");
                this.Manager.Checkpoint("MS-NRPC_R103456");
                this.Manager.Comment("reaching state \'S1415\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/STATUS_DOWNGRADE_DETECTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_DOWNGRADE_DETECTED, temp342, "return of NetrServerAuthenticate3, state S1415");
                this.Manager.Comment("reaching state \'S1474\'");
                goto label33;
            }
            if ((temp344 == 2)) {
                this.Manager.Comment("reaching state \'S197\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S377\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S557\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp343;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(NonDcServer,Client)\'");
                temp343 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103509");
                this.Manager.Checkpoint("MS-NRPC_R103341");
                this.Manager.Comment("reaching state \'S737\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp343, "return of NetrServerReqChallenge, state S737");
                this.Manager.Comment("reaching state \'S917\'");
                goto label33;
            }
            throw new InvalidOperationException("never reached");
        label33:
;
            this.Manager.EndTest();
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate3S50GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S51");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate3S50GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S51");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate3S50GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S51");
        }
        #endregion
        
        #region Test Starting in S52
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_EstablishSecureChannel_NetrServerAuthenticate3S52() {
            this.Manager.BeginTest("Test_EstablishSecureChannel_NetrServerAuthenticate3S52");
            this.Manager.Comment("reaching state \'S52\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp345;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp345);
            this.Manager.AddReturn(GetPlatformInfo, null, temp345);
            this.Manager.Comment("reaching state \'S53\'");
            int temp355 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate3.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate3S52GetPlatformChecker)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate3.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate3S52GetPlatformChecker1)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate3.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate3S52GetPlatformChecker2)));
            if ((temp355 == 0)) {
                this.Manager.Comment("reaching state \'S198\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S378\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S558\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp346;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp346 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S738\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp346, "return of NetrServerReqChallenge, state S738");
                this.Manager.Comment("reaching state \'S918\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp347;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16388)\'");
                temp347 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16388u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S1073\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp347, "return of NetrServerAuthenticate3, state S1073");
                this.Manager.Comment("reaching state \'S1165\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp348;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp348 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S1257\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp348, "return of NetrServerReqChallenge, state S1257");
                this.Manager.Comment("reaching state \'S1346\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp349;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16388)\'");
                temp349 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16388u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S1416\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp349, "return of NetrServerAuthenticate3, state S1416");
                this.Manager.Comment("reaching state \'S1475\'");
                goto label34;
            }
            if ((temp355 == 1)) {
                this.Manager.Comment("reaching state \'S199\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S379\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S559\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp350;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp350 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S739\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp350, "return of NetrServerReqChallenge, state S739");
                this.Manager.Comment("reaching state \'S919\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp351;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,536870916)\'");
                temp351 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 536870916u);
                this.Manager.Checkpoint("MS-NRPC_R103371");
                this.Manager.Checkpoint("MS-NRPC_R103456");
                this.Manager.Comment("reaching state \'S1074\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/STATUS_DOWNGRADE_DETECTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_DOWNGRADE_DETECTED, temp351, "return of NetrServerAuthenticate3, state S1074");
                this.Manager.Comment("reaching state \'S1166\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp352;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp352 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S1258\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp352, "return of NetrServerReqChallenge, state S1258");
                this.Manager.Comment("reaching state \'S1347\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp353;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,4)\'");
                temp353 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 4u);
                this.Manager.Checkpoint("MS-NRPC_R103371");
                this.Manager.Checkpoint("MS-NRPC_R894");
                this.Manager.Checkpoint("MS-NRPC_R103456");
                this.Manager.AddReturn(NetrServerAuthenticate3Info, null, temp353);
                Test_EstablishSecureChannel_NetrServerAuthenticate3S1386();
                goto label34;
            }
            if ((temp355 == 2)) {
                this.Manager.Comment("reaching state \'S200\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S380\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S560\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp354;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(NonDcServer,Client)\'");
                temp354 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103509");
                this.Manager.Checkpoint("MS-NRPC_R103341");
                this.Manager.Comment("reaching state \'S740\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp354, "return of NetrServerReqChallenge, state S740");
                this.Manager.Comment("reaching state \'S920\'");
                goto label34;
            }
            throw new InvalidOperationException("never reached");
        label34:
;
            this.Manager.EndTest();
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate3S52GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S53");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate3S52GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S53");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate3S52GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S53");
        }
        #endregion
        
        #region Test Starting in S54
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_EstablishSecureChannel_NetrServerAuthenticate3S54() {
            this.Manager.BeginTest("Test_EstablishSecureChannel_NetrServerAuthenticate3S54");
            this.Manager.Comment("reaching state \'S54\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp356;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp356);
            this.Manager.AddReturn(GetPlatformInfo, null, temp356);
            this.Manager.Comment("reaching state \'S55\'");
            int temp366 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate3.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate3S54GetPlatformChecker)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate3.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate3S54GetPlatformChecker1)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate3.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate3S54GetPlatformChecker2)));
            if ((temp366 == 0)) {
                this.Manager.Comment("reaching state \'S201\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S381\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S561\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp357;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp357 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S741\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp357, "return of NetrServerReqChallenge, state S741");
                this.Manager.Comment("reaching state \'S921\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp358;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,False,16388)\'");
                temp358 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, false, 16388u);
                this.Manager.Checkpoint("MS-NRPC_R103376");
                this.Manager.Checkpoint("MS-NRPC_R103456");
                this.Manager.Comment("reaching state \'S1075\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/STATUS_ACCESS_DENIED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp358, "return of NetrServerAuthenticate3, state S1075");
                this.Manager.Comment("reaching state \'S1167\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp359;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp359 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S1259\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp359, "return of NetrServerReqChallenge, state S1259");
                this.Manager.Comment("reaching state \'S1348\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp360;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,False,16388)\'");
                temp360 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, false, 16388u);
                this.Manager.Checkpoint("MS-NRPC_R103376");
                this.Manager.Checkpoint("MS-NRPC_R103456");
                this.Manager.Comment("reaching state \'S1417\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/STATUS_ACCESS_DENIED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp360, "return of NetrServerAuthenticate3, state S1417");
                this.Manager.Comment("reaching state \'S1476\'");
                goto label35;
            }
            if ((temp366 == 1)) {
                this.Manager.Comment("reaching state \'S202\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S382\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S562\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp361;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp361 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S742\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp361, "return of NetrServerReqChallenge, state S742");
                this.Manager.Comment("reaching state \'S922\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp362;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16388)\'");
                temp362 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16388u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S1076\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp362, "return of NetrServerAuthenticate3, state S1076");
                this.Manager.Comment("reaching state \'S1168\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp363;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp363 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S1260\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp363, "return of NetrServerReqChallenge, state S1260");
                this.Manager.Comment("reaching state \'S1349\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp364;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16388)\'");
                temp364 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16388u);
                this.Manager.Checkpoint("MS-NRPC_R103455");
                this.Manager.Comment("reaching state \'S1418\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp364, "return of NetrServerAuthenticate3, state S1418");
                this.Manager.Comment("reaching state \'S1477\'");
                goto label35;
            }
            if ((temp366 == 2)) {
                this.Manager.Comment("reaching state \'S203\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S383\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S563\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp365;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(NonDcServer,Client)\'");
                temp365 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103509");
                this.Manager.Checkpoint("MS-NRPC_R103341");
                this.Manager.Comment("reaching state \'S743\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp365, "return of NetrServerReqChallenge, state S743");
                this.Manager.Comment("reaching state \'S923\'");
                goto label35;
            }
            throw new InvalidOperationException("never reached");
        label35:
;
            this.Manager.EndTest();
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate3S54GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S55");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate3S54GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S55");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate3S54GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S55");
        }
        #endregion
        
        #region Test Starting in S56
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_EstablishSecureChannel_NetrServerAuthenticate3S56() {
            this.Manager.BeginTest("Test_EstablishSecureChannel_NetrServerAuthenticate3S56");
            this.Manager.Comment("reaching state \'S56\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp367;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp367);
            this.Manager.AddReturn(GetPlatformInfo, null, temp367);
            this.Manager.Comment("reaching state \'S57\'");
            int temp376 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate3.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate3S56GetPlatformChecker)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate3.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate3S56GetPlatformChecker1)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate3.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate3S56GetPlatformChecker2)));
            if ((temp376 == 0)) {
                this.Manager.Comment("reaching state \'S204\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S384\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S564\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp368;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(NonDcServer,Client)\'");
                temp368 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103509");
                this.Manager.Checkpoint("MS-NRPC_R103341");
                this.Manager.Comment("reaching state \'S744\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp368, "return of NetrServerReqChallenge, state S744");
                this.Manager.Comment("reaching state \'S924\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp369;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(NonDcServer,DomainMemberComputerAcco" +
                        "unt,WorkstationSecureChannel,Client,True,16388)\'");
                temp369 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16388u);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103513");
                this.Manager.Checkpoint("MS-NRPC_R103456");
                this.Manager.Comment("reaching state \'S1077\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp369, "return of NetrServerAuthenticate3, state S1077");
                this.Manager.Comment("reaching state \'S1169\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp370;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp370 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.AddReturn(NetrServerReqChallengeInfo, null, temp370);
                Test_EstablishSecureChannel_NetrServerAuthenticate3S1209();
                goto label36;
            }
            if ((temp376 == 1)) {
                this.Manager.Comment("reaching state \'S205\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S385\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S565\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp371;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp371 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S745\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp371, "return of NetrServerReqChallenge, state S745");
                this.Manager.Comment("reaching state \'S925\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp372;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,False,16388)\'");
                temp372 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, false, 16388u);
                this.Manager.Checkpoint("MS-NRPC_R103376");
                this.Manager.Checkpoint("MS-NRPC_R103456");
                this.Manager.Comment("reaching state \'S1078\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/STATUS_ACCESS_DENIED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp372, "return of NetrServerAuthenticate3, state S1078");
                this.Manager.Comment("reaching state \'S1170\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp373;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp373 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S1261\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp373, "return of NetrServerReqChallenge, state S1261");
                this.Manager.Comment("reaching state \'S1350\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp374;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,False,16388)\'");
                temp374 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, false, 16388u);
                this.Manager.Checkpoint("MS-NRPC_R103376");
                this.Manager.Checkpoint("MS-NRPC_R103456");
                this.Manager.Comment("reaching state \'S1419\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/STATUS_ACCESS_DENIED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp374, "return of NetrServerAuthenticate3, state S1419");
                this.Manager.Comment("reaching state \'S1478\'");
                goto label36;
            }
            if ((temp376 == 2)) {
                this.Manager.Comment("reaching state \'S206\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S386\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S566\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp375;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(NonDcServer,Client)\'");
                temp375 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103509");
                this.Manager.Checkpoint("MS-NRPC_R103341");
                this.Manager.Comment("reaching state \'S746\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp375, "return of NetrServerReqChallenge, state S746");
                this.Manager.Comment("reaching state \'S926\'");
                goto label36;
            }
            throw new InvalidOperationException("never reached");
        label36:
;
            this.Manager.EndTest();
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate3S56GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S57");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate3S56GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S57");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate3S56GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S57");
        }
        #endregion
        
        #region Test Starting in S58
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_EstablishSecureChannel_NetrServerAuthenticate3S58() {
            this.Manager.BeginTest("Test_EstablishSecureChannel_NetrServerAuthenticate3S58");
            this.Manager.Comment("reaching state \'S58\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp377;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp377);
            this.Manager.AddReturn(GetPlatformInfo, null, temp377);
            this.Manager.Comment("reaching state \'S59\'");
            int temp384 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate3.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate3S58GetPlatformChecker)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate3.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate3S58GetPlatformChecker1)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate3.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate3S58GetPlatformChecker2)));
            if ((temp384 == 0)) {
                this.Manager.Comment("reaching state \'S207\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S387\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S567\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp378;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp378 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S747\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp378, "return of NetrServerReqChallenge, state S747");
                this.Manager.Comment("reaching state \'S927\'");
                this.Manager.Comment("executing step \'call ConfigServerRejectMD5Client(True)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServerRejectMD5Client(true);
                this.Manager.Comment("reaching state \'S1079\'");
                this.Manager.Comment("checking step \'return ConfigServerRejectMD5Client\'");
                this.Manager.Comment("reaching state \'S1171\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp379;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16388)\'");
                temp379 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16388u);
                this.Manager.Checkpoint("MS-NRPC_R203371");
                this.Manager.Checkpoint("MS-NRPC_R103456");
                this.Manager.Comment("reaching state \'S1262\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/STATUS_DOWNGRADE_DETECTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_DOWNGRADE_DETECTED, temp379, "return of NetrServerAuthenticate3, state S1262");
                this.Manager.Comment("reaching state \'S1351\'");
                this.Manager.Comment("executing step \'call ConfigServerRejectMD5Client(False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServerRejectMD5Client(false);
                this.Manager.Comment("reaching state \'S1420\'");
                this.Manager.Comment("checking step \'return ConfigServerRejectMD5Client\'");
                this.Manager.Comment("reaching state \'S1479\'");
                goto label37;
            }
            if ((temp384 == 1)) {
                this.Manager.Comment("reaching state \'S208\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S388\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S568\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp380;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(NonDcServer,Client)\'");
                temp380 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103509");
                this.Manager.Checkpoint("MS-NRPC_R103341");
                this.Manager.Comment("reaching state \'S748\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp380, "return of NetrServerReqChallenge, state S748");
                this.Manager.Comment("reaching state \'S928\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp381;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(NonDcServer,DomainMemberComputerAcco" +
                        "unt,WorkstationSecureChannel,Client,True,16388)\'");
                temp381 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16388u);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103513");
                this.Manager.Checkpoint("MS-NRPC_R103456");
                this.Manager.Comment("reaching state \'S1080\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp381, "return of NetrServerAuthenticate3, state S1080");
                this.Manager.Comment("reaching state \'S1172\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp382;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp382 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.AddReturn(NetrServerReqChallengeInfo, null, temp382);
                Test_EstablishSecureChannel_NetrServerAuthenticate3S1210();
                goto label37;
            }
            if ((temp384 == 2)) {
                this.Manager.Comment("reaching state \'S209\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S389\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S569\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp383;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(NonDcServer,Client)\'");
                temp383 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103509");
                this.Manager.Checkpoint("MS-NRPC_R103341");
                this.Manager.Comment("reaching state \'S749\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp383, "return of NetrServerReqChallenge, state S749");
                this.Manager.Comment("reaching state \'S929\'");
                goto label37;
            }
            throw new InvalidOperationException("never reached");
        label37:
;
            this.Manager.EndTest();
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate3S58GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S59");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate3S58GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S59");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate3S58GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S59");
        }
        #endregion
        
        #region Test Starting in S6
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_EstablishSecureChannel_NetrServerAuthenticate3S6() {
            this.Manager.BeginTest("Test_EstablishSecureChannel_NetrServerAuthenticate3S6");
            this.Manager.Comment("reaching state \'S6\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp385;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp385);
            this.Manager.AddReturn(GetPlatformInfo, null, temp385);
            this.Manager.Comment("reaching state \'S7\'");
            int temp395 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate3.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate3S6GetPlatformChecker)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate3.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate3S6GetPlatformChecker1)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate3.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate3S6GetPlatformChecker2)));
            if ((temp395 == 0)) {
                this.Manager.Comment("reaching state \'S129\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S309\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S489\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp386;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp386 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S669\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp386, "return of NetrServerReqChallenge, state S669");
                this.Manager.Comment("reaching state \'S849\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp387;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,6)\'");
                temp387 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 6u);
                this.Manager.Checkpoint("MS-NRPC_R103371");
                this.Manager.Checkpoint("MS-NRPC_R103456");
                this.Manager.Comment("reaching state \'S1027\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/STATUS_DOWNGRADE_DETECTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_DOWNGRADE_DETECTED, temp387, "return of NetrServerAuthenticate3, state S1027");
                this.Manager.Comment("reaching state \'S1119\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp388;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp388 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S1211\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp388, "return of NetrServerReqChallenge, state S1211");
                this.Manager.Comment("reaching state \'S1300\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp389;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,4)\'");
                temp389 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 4u);
                this.Manager.Checkpoint("MS-NRPC_R103371");
                this.Manager.Checkpoint("MS-NRPC_R889");
                this.Manager.Checkpoint("MS-NRPC_R103456");
                this.Manager.AddReturn(NetrServerAuthenticate3Info, null, temp389);
                Test_EstablishSecureChannel_NetrServerAuthenticate3S1385();
                goto label38;
            }
            if ((temp395 == 1)) {
                this.Manager.Comment("reaching state \'S130\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S310\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S490\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp390;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp390 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S670\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp390, "return of NetrServerReqChallenge, state S670");
                this.Manager.Comment("reaching state \'S850\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp391;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,6)\'");
                temp391 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 6u);
                this.Manager.Checkpoint("MS-NRPC_R103371");
                this.Manager.Checkpoint("MS-NRPC_R103456");
                this.Manager.Comment("reaching state \'S1028\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/STATUS_DOWNGRADE_DETECTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_DOWNGRADE_DETECTED, temp391, "return of NetrServerAuthenticate3, state S1028");
                this.Manager.Comment("reaching state \'S1120\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp392;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp392 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S1212\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp392, "return of NetrServerReqChallenge, state S1212");
                this.Manager.Comment("reaching state \'S1301\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp393;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,4)\'");
                temp393 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 4u);
                this.Manager.Checkpoint("MS-NRPC_R103371");
                this.Manager.Checkpoint("MS-NRPC_R889");
                this.Manager.Checkpoint("MS-NRPC_R103456");
                this.Manager.AddReturn(NetrServerAuthenticate3Info, null, temp393);
                Test_EstablishSecureChannel_NetrServerAuthenticate3S1386();
                goto label38;
            }
            if ((temp395 == 2)) {
                this.Manager.Comment("reaching state \'S131\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S311\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S491\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp394;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(NonDcServer,Client)\'");
                temp394 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103509");
                this.Manager.Checkpoint("MS-NRPC_R103341");
                this.Manager.Comment("reaching state \'S671\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp394, "return of NetrServerReqChallenge, state S671");
                this.Manager.Comment("reaching state \'S851\'");
                goto label38;
            }
            throw new InvalidOperationException("never reached");
        label38:
;
            this.Manager.EndTest();
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate3S6GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S7");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate3S6GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S7");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate3S6GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S7");
        }
        #endregion
        
        #region Test Starting in S60
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_EstablishSecureChannel_NetrServerAuthenticate3S60() {
            this.Manager.BeginTest("Test_EstablishSecureChannel_NetrServerAuthenticate3S60");
            this.Manager.Comment("reaching state \'S60\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp396;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp396);
            this.Manager.AddReturn(GetPlatformInfo, null, temp396);
            this.Manager.Comment("reaching state \'S61\'");
            int temp404 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate3.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate3S60GetPlatformChecker)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate3.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate3S60GetPlatformChecker1)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate3.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate3S60GetPlatformChecker2)));
            if ((temp404 == 0)) {
                this.Manager.Comment("reaching state \'S210\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S390\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S570\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp397;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp397 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S750\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp397, "return of NetrServerReqChallenge, state S750");
                this.Manager.Comment("reaching state \'S930\'");
                this.Manager.Comment("executing step \'call ConfigServerRejectMD5Client(True)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServerRejectMD5Client(true);
                this.Manager.Comment("reaching state \'S1081\'");
                this.Manager.Comment("checking step \'return ConfigServerRejectMD5Client\'");
                this.Manager.Comment("reaching state \'S1173\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp398;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16388)\'");
                temp398 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16388u);
                this.Manager.Checkpoint("MS-NRPC_R203371");
                this.Manager.Checkpoint("MS-NRPC_R103456");
                this.Manager.Comment("reaching state \'S1263\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/STATUS_DOWNGRADE_DETECTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_DOWNGRADE_DETECTED, temp398, "return of NetrServerAuthenticate3, state S1263");
                this.Manager.Comment("reaching state \'S1352\'");
                this.Manager.Comment("executing step \'call ConfigServerRejectMD5Client(False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServerRejectMD5Client(false);
                this.Manager.Comment("reaching state \'S1421\'");
                this.Manager.Comment("checking step \'return ConfigServerRejectMD5Client\'");
                this.Manager.Comment("reaching state \'S1480\'");
                goto label39;
            }
            if ((temp404 == 1)) {
                this.Manager.Comment("reaching state \'S211\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S391\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S571\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp399;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp399 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S751\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp399, "return of NetrServerReqChallenge, state S751");
                this.Manager.Comment("reaching state \'S931\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp400;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,5)\'");
                temp400 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 5u);
                this.Manager.Checkpoint("MS-NRPC_R103371");
                this.Manager.Checkpoint("MS-NRPC_R103456");
                this.Manager.Comment("reaching state \'S1082\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/STATUS_DOWNGRADE_DETECTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_DOWNGRADE_DETECTED, temp400, "return of NetrServerAuthenticate3, state S1082");
                this.Manager.Comment("reaching state \'S1174\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp401;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp401 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S1264\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp401, "return of NetrServerReqChallenge, state S1264");
                this.Manager.Comment("reaching state \'S1353\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp402;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,4)\'");
                temp402 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 4u);
                this.Manager.Checkpoint("MS-NRPC_R103371");
                this.Manager.Checkpoint("MS-NRPC_R888");
                this.Manager.Checkpoint("MS-NRPC_R103456");
                this.Manager.AddReturn(NetrServerAuthenticate3Info, null, temp402);
                Test_EstablishSecureChannel_NetrServerAuthenticate3S1422();
                goto label39;
            }
            if ((temp404 == 2)) {
                this.Manager.Comment("reaching state \'S212\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S392\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S572\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp403;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(NonDcServer,Client)\'");
                temp403 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103509");
                this.Manager.Checkpoint("MS-NRPC_R103341");
                this.Manager.Comment("reaching state \'S752\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp403, "return of NetrServerReqChallenge, state S752");
                this.Manager.Comment("reaching state \'S932\'");
                goto label39;
            }
            throw new InvalidOperationException("never reached");
        label39:
;
            this.Manager.EndTest();
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate3S60GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S61");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate3S60GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S61");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate3S60GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S61");
        }
        #endregion
        
        #region Test Starting in S62
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_EstablishSecureChannel_NetrServerAuthenticate3S62() {
            this.Manager.BeginTest("Test_EstablishSecureChannel_NetrServerAuthenticate3S62");
            this.Manager.Comment("reaching state \'S62\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp405;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp405);
            this.Manager.AddReturn(GetPlatformInfo, null, temp405);
            this.Manager.Comment("reaching state \'S63\'");
            int temp411 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate3.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate3S62GetPlatformChecker)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate3.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate3S62GetPlatformChecker1)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate3.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate3S62GetPlatformChecker2)));
            if ((temp411 == 0)) {
                this.Manager.Comment("reaching state \'S213\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S393\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S573\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp406;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp406 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S753\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp406, "return of NetrServerReqChallenge, state S753");
                this.Manager.Comment("reaching state \'S933\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp407;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(NonDcServer,DomainMemberComputerAcco" +
                        "unt,WorkstationSecureChannel,Client,True,16388)\'");
                temp407 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 16388u);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103513");
                this.Manager.Checkpoint("MS-NRPC_R103456");
                this.Manager.Comment("reaching state \'S1083\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp407, "return of NetrServerAuthenticate3, state S1083");
                this.Manager.Comment("reaching state \'S1175\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp408;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp408 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.AddReturn(NetrServerReqChallengeInfo, null, temp408);
                Test_EstablishSecureChannel_NetrServerAuthenticate3S1265();
                goto label40;
            }
            if ((temp411 == 1)) {
                this.Manager.Comment("reaching state \'S214\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S394\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S574\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp409;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(NonDcServer,Client)\'");
                temp409 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103509");
                this.Manager.Checkpoint("MS-NRPC_R103341");
                this.Manager.Comment("reaching state \'S754\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp409, "return of NetrServerReqChallenge, state S754");
                this.Manager.Comment("reaching state \'S934\'");
                goto label40;
            }
            if ((temp411 == 2)) {
                this.Manager.Comment("reaching state \'S215\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S395\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S575\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp410;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(NonDcServer,Client)\'");
                temp410 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103509");
                this.Manager.Checkpoint("MS-NRPC_R103341");
                this.Manager.Comment("reaching state \'S755\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp410, "return of NetrServerReqChallenge, state S755");
                this.Manager.Comment("reaching state \'S935\'");
                goto label40;
            }
            throw new InvalidOperationException("never reached");
        label40:
;
            this.Manager.EndTest();
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate3S62GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S63");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate3S62GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S63");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate3S62GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S63");
        }
        #endregion
        
        #region Test Starting in S64
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_EstablishSecureChannel_NetrServerAuthenticate3S64() {
            this.Manager.BeginTest("Test_EstablishSecureChannel_NetrServerAuthenticate3S64");
            this.Manager.Comment("reaching state \'S64\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp412;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp412);
            this.Manager.AddReturn(GetPlatformInfo, null, temp412);
            this.Manager.Comment("reaching state \'S65\'");
            int temp419 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate3.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate3S64GetPlatformChecker)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate3.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate3S64GetPlatformChecker1)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate3.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate3S64GetPlatformChecker2)));
            if ((temp419 == 0)) {
                this.Manager.Comment("reaching state \'S216\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S396\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S576\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp413;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp413 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S756\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp413, "return of NetrServerReqChallenge, state S756");
                this.Manager.Comment("reaching state \'S936\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp414;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,6)\'");
                temp414 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 6u);
                this.Manager.Checkpoint("MS-NRPC_R103371");
                this.Manager.Checkpoint("MS-NRPC_R103456");
                this.Manager.Comment("reaching state \'S1084\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/STATUS_DOWNGRADE_DETECTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_DOWNGRADE_DETECTED, temp414, "return of NetrServerAuthenticate3, state S1084");
                this.Manager.Comment("reaching state \'S1176\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp415;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp415 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S1266\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp415, "return of NetrServerReqChallenge, state S1266");
                this.Manager.Comment("reaching state \'S1355\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp416;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,4)\'");
                temp416 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 4u);
                this.Manager.Checkpoint("MS-NRPC_R103371");
                this.Manager.Checkpoint("MS-NRPC_R889");
                this.Manager.Checkpoint("MS-NRPC_R103456");
                this.Manager.AddReturn(NetrServerAuthenticate3Info, null, temp416);
                Test_EstablishSecureChannel_NetrServerAuthenticate3S1422();
                goto label41;
            }
            if ((temp419 == 1)) {
                this.Manager.Comment("reaching state \'S217\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S397\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S577\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp417;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(NonDcServer,Client)\'");
                temp417 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103509");
                this.Manager.Checkpoint("MS-NRPC_R103341");
                this.Manager.Comment("reaching state \'S757\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp417, "return of NetrServerReqChallenge, state S757");
                this.Manager.Comment("reaching state \'S937\'");
                goto label41;
            }
            if ((temp419 == 2)) {
                this.Manager.Comment("reaching state \'S218\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S398\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S578\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp418;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(NonDcServer,Client)\'");
                temp418 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103509");
                this.Manager.Checkpoint("MS-NRPC_R103341");
                this.Manager.Comment("reaching state \'S758\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp418, "return of NetrServerReqChallenge, state S758");
                this.Manager.Comment("reaching state \'S938\'");
                goto label41;
            }
            throw new InvalidOperationException("never reached");
        label41:
;
            this.Manager.EndTest();
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate3S64GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S65");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate3S64GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S65");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate3S64GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S65");
        }
        #endregion
        
        #region Test Starting in S66
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_EstablishSecureChannel_NetrServerAuthenticate3S66() {
            this.Manager.BeginTest("Test_EstablishSecureChannel_NetrServerAuthenticate3S66");
            this.Manager.Comment("reaching state \'S66\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp420;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp420);
            this.Manager.AddReturn(GetPlatformInfo, null, temp420);
            this.Manager.Comment("reaching state \'S67\'");
            int temp427 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate3.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate3S66GetPlatformChecker)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate3.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate3S66GetPlatformChecker1)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate3.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate3S66GetPlatformChecker2)));
            if ((temp427 == 0)) {
                this.Manager.Comment("reaching state \'S219\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S399\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S579\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp421;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp421 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S759\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp421, "return of NetrServerReqChallenge, state S759");
                this.Manager.Comment("reaching state \'S939\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp422;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,4)\'");
                temp422 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 4u);
                this.Manager.Checkpoint("MS-NRPC_R103371");
                this.Manager.Checkpoint("MS-NRPC_R103456");
                this.Manager.Comment("reaching state \'S1085\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/STATUS_DOWNGRADE_DETECTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_DOWNGRADE_DETECTED, temp422, "return of NetrServerAuthenticate3, state S1085");
                this.Manager.Comment("reaching state \'S1177\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp423;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp423 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S1267\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp423, "return of NetrServerReqChallenge, state S1267");
                this.Manager.Comment("reaching state \'S1356\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp424;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,4)\'");
                temp424 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 4u);
                this.Manager.Checkpoint("MS-NRPC_R103371");
                this.Manager.Checkpoint("MS-NRPC_R103456");
                this.Manager.AddReturn(NetrServerAuthenticate3Info, null, temp424);
                Test_EstablishSecureChannel_NetrServerAuthenticate3S1422();
                goto label42;
            }
            if ((temp427 == 1)) {
                this.Manager.Comment("reaching state \'S220\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S400\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S580\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp425;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(NonDcServer,Client)\'");
                temp425 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103509");
                this.Manager.Checkpoint("MS-NRPC_R103341");
                this.Manager.Comment("reaching state \'S760\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp425, "return of NetrServerReqChallenge, state S760");
                this.Manager.Comment("reaching state \'S940\'");
                goto label42;
            }
            if ((temp427 == 2)) {
                this.Manager.Comment("reaching state \'S221\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S401\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S581\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp426;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(NonDcServer,Client)\'");
                temp426 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103509");
                this.Manager.Checkpoint("MS-NRPC_R103341");
                this.Manager.Comment("reaching state \'S761\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp426, "return of NetrServerReqChallenge, state S761");
                this.Manager.Comment("reaching state \'S941\'");
                goto label42;
            }
            throw new InvalidOperationException("never reached");
        label42:
;
            this.Manager.EndTest();
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate3S66GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S67");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate3S66GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S67");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate3S66GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S67");
        }
        #endregion
        
        #region Test Starting in S68
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_EstablishSecureChannel_NetrServerAuthenticate3S68() {
            this.Manager.BeginTest("Test_EstablishSecureChannel_NetrServerAuthenticate3S68");
            this.Manager.Comment("reaching state \'S68\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp428;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp428);
            this.Manager.AddReturn(GetPlatformInfo, null, temp428);
            this.Manager.Comment("reaching state \'S69\'");
            int temp435 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate3.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate3S68GetPlatformChecker)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate3.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate3S68GetPlatformChecker1)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate3.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate3S68GetPlatformChecker2)));
            if ((temp435 == 0)) {
                this.Manager.Comment("reaching state \'S222\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S402\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S582\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp429;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp429 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S762\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp429, "return of NetrServerReqChallenge, state S762");
                this.Manager.Comment("reaching state \'S942\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp430;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,12)\'");
                temp430 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 12u);
                this.Manager.Checkpoint("MS-NRPC_R103371");
                this.Manager.Checkpoint("MS-NRPC_R103456");
                this.Manager.Comment("reaching state \'S1086\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/STATUS_DOWNGRADE_DETECTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_DOWNGRADE_DETECTED, temp430, "return of NetrServerAuthenticate3, state S1086");
                this.Manager.Comment("reaching state \'S1178\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp431;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp431 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S1268\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp431, "return of NetrServerReqChallenge, state S1268");
                this.Manager.Comment("reaching state \'S1357\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp432;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,4)\'");
                temp432 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 4u);
                this.Manager.Checkpoint("MS-NRPC_R103371");
                this.Manager.Checkpoint("MS-NRPC_R890");
                this.Manager.Checkpoint("MS-NRPC_R103456");
                this.Manager.AddReturn(NetrServerAuthenticate3Info, null, temp432);
                Test_EstablishSecureChannel_NetrServerAuthenticate3S1422();
                goto label43;
            }
            if ((temp435 == 1)) {
                this.Manager.Comment("reaching state \'S223\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S403\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S583\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp433;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(NonDcServer,Client)\'");
                temp433 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103509");
                this.Manager.Checkpoint("MS-NRPC_R103341");
                this.Manager.Comment("reaching state \'S763\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp433, "return of NetrServerReqChallenge, state S763");
                this.Manager.Comment("reaching state \'S943\'");
                goto label43;
            }
            if ((temp435 == 2)) {
                this.Manager.Comment("reaching state \'S224\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S404\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S584\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp434;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(NonDcServer,Client)\'");
                temp434 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103509");
                this.Manager.Checkpoint("MS-NRPC_R103341");
                this.Manager.Comment("reaching state \'S764\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp434, "return of NetrServerReqChallenge, state S764");
                this.Manager.Comment("reaching state \'S944\'");
                goto label43;
            }
            throw new InvalidOperationException("never reached");
        label43:
;
            this.Manager.EndTest();
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate3S68GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S69");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate3S68GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S69");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate3S68GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S69");
        }
        #endregion
        
        #region Test Starting in S70
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_EstablishSecureChannel_NetrServerAuthenticate3S70() {
            this.Manager.BeginTest("Test_EstablishSecureChannel_NetrServerAuthenticate3S70");
            this.Manager.Comment("reaching state \'S70\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp436;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp436);
            this.Manager.AddReturn(GetPlatformInfo, null, temp436);
            this.Manager.Comment("reaching state \'S71\'");
            int temp443 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate3.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate3S70GetPlatformChecker)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate3.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate3S70GetPlatformChecker1)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate3.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate3S70GetPlatformChecker2)));
            if ((temp443 == 0)) {
                this.Manager.Comment("reaching state \'S225\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S405\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S585\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp437;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp437 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S765\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp437, "return of NetrServerReqChallenge, state S765");
                this.Manager.Comment("reaching state \'S945\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp438;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,20)\'");
                temp438 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 20u);
                this.Manager.Checkpoint("MS-NRPC_R103371");
                this.Manager.Checkpoint("MS-NRPC_R103456");
                this.Manager.Comment("reaching state \'S1087\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/STATUS_DOWNGRADE_DETECTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_DOWNGRADE_DETECTED, temp438, "return of NetrServerAuthenticate3, state S1087");
                this.Manager.Comment("reaching state \'S1179\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp439;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp439 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S1269\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp439, "return of NetrServerReqChallenge, state S1269");
                this.Manager.Comment("reaching state \'S1358\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp440;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,4)\'");
                temp440 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 4u);
                this.Manager.Checkpoint("MS-NRPC_R103371");
                this.Manager.Checkpoint("MS-NRPC_R104868");
                this.Manager.Checkpoint("MS-NRPC_R103456");
                this.Manager.AddReturn(NetrServerAuthenticate3Info, null, temp440);
                Test_EstablishSecureChannel_NetrServerAuthenticate3S1422();
                goto label44;
            }
            if ((temp443 == 1)) {
                this.Manager.Comment("reaching state \'S226\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S406\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S586\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp441;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(NonDcServer,Client)\'");
                temp441 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103509");
                this.Manager.Checkpoint("MS-NRPC_R103341");
                this.Manager.Comment("reaching state \'S766\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp441, "return of NetrServerReqChallenge, state S766");
                this.Manager.Comment("reaching state \'S946\'");
                goto label44;
            }
            if ((temp443 == 2)) {
                this.Manager.Comment("reaching state \'S227\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S407\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S587\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp442;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(NonDcServer,Client)\'");
                temp442 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103509");
                this.Manager.Checkpoint("MS-NRPC_R103341");
                this.Manager.Comment("reaching state \'S767\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp442, "return of NetrServerReqChallenge, state S767");
                this.Manager.Comment("reaching state \'S947\'");
                goto label44;
            }
            throw new InvalidOperationException("never reached");
        label44:
;
            this.Manager.EndTest();
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate3S70GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S71");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate3S70GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S71");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate3S70GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S71");
        }
        #endregion
        
        #region Test Starting in S72
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_EstablishSecureChannel_NetrServerAuthenticate3S72() {
            this.Manager.BeginTest("Test_EstablishSecureChannel_NetrServerAuthenticate3S72");
            this.Manager.Comment("reaching state \'S72\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp444;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp444);
            this.Manager.AddReturn(GetPlatformInfo, null, temp444);
            this.Manager.Comment("reaching state \'S73\'");
            int temp451 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate3.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate3S72GetPlatformChecker)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate3.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate3S72GetPlatformChecker1)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate3.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate3S72GetPlatformChecker2)));
            if ((temp451 == 0)) {
                this.Manager.Comment("reaching state \'S228\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S408\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S588\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp445;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp445 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S768\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp445, "return of NetrServerReqChallenge, state S768");
                this.Manager.Comment("reaching state \'S948\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp446;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,36)\'");
                temp446 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 36u);
                this.Manager.Checkpoint("MS-NRPC_R103371");
                this.Manager.Checkpoint("MS-NRPC_R103456");
                this.Manager.Comment("reaching state \'S1088\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/STATUS_DOWNGRADE_DETECTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_DOWNGRADE_DETECTED, temp446, "return of NetrServerAuthenticate3, state S1088");
                this.Manager.Comment("reaching state \'S1180\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp447;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp447 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S1270\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp447, "return of NetrServerReqChallenge, state S1270");
                this.Manager.Comment("reaching state \'S1359\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp448;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,4)\'");
                temp448 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 4u);
                this.Manager.Checkpoint("MS-NRPC_R103371");
                this.Manager.Checkpoint("MS-NRPC_R104869");
                this.Manager.Checkpoint("MS-NRPC_R103456");
                this.Manager.AddReturn(NetrServerAuthenticate3Info, null, temp448);
                Test_EstablishSecureChannel_NetrServerAuthenticate3S1422();
                goto label45;
            }
            if ((temp451 == 1)) {
                this.Manager.Comment("reaching state \'S229\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S409\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S589\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp449;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(NonDcServer,Client)\'");
                temp449 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103509");
                this.Manager.Checkpoint("MS-NRPC_R103341");
                this.Manager.Comment("reaching state \'S769\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp449, "return of NetrServerReqChallenge, state S769");
                this.Manager.Comment("reaching state \'S949\'");
                goto label45;
            }
            if ((temp451 == 2)) {
                this.Manager.Comment("reaching state \'S230\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S410\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S590\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp450;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(NonDcServer,Client)\'");
                temp450 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103509");
                this.Manager.Checkpoint("MS-NRPC_R103341");
                this.Manager.Comment("reaching state \'S770\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp450, "return of NetrServerReqChallenge, state S770");
                this.Manager.Comment("reaching state \'S950\'");
                goto label45;
            }
            throw new InvalidOperationException("never reached");
        label45:
;
            this.Manager.EndTest();
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate3S72GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S73");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate3S72GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S73");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate3S72GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S73");
        }
        #endregion
        
        #region Test Starting in S74
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_EstablishSecureChannel_NetrServerAuthenticate3S74() {
            this.Manager.BeginTest("Test_EstablishSecureChannel_NetrServerAuthenticate3S74");
            this.Manager.Comment("reaching state \'S74\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp452;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp452);
            this.Manager.AddReturn(GetPlatformInfo, null, temp452);
            this.Manager.Comment("reaching state \'S75\'");
            int temp459 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate3.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate3S74GetPlatformChecker)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate3.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate3S74GetPlatformChecker1)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate3.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate3S74GetPlatformChecker2)));
            if ((temp459 == 0)) {
                this.Manager.Comment("reaching state \'S231\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S411\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S591\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp453;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp453 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S771\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp453, "return of NetrServerReqChallenge, state S771");
                this.Manager.Comment("reaching state \'S951\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp454;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,68)\'");
                temp454 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 68u);
                this.Manager.Checkpoint("MS-NRPC_R103371");
                this.Manager.Checkpoint("MS-NRPC_R103456");
                this.Manager.Comment("reaching state \'S1089\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/STATUS_DOWNGRADE_DETECTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_DOWNGRADE_DETECTED, temp454, "return of NetrServerAuthenticate3, state S1089");
                this.Manager.Comment("reaching state \'S1181\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp455;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp455 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S1271\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp455, "return of NetrServerReqChallenge, state S1271");
                this.Manager.Comment("reaching state \'S1360\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp456;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,68)\'");
                temp456 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 68u);
                this.Manager.Checkpoint("MS-NRPC_R103371");
                this.Manager.Checkpoint("MS-NRPC_R103456");
                this.Manager.Comment("reaching state \'S1424\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/STATUS_DOWNGRADE_DETECTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_DOWNGRADE_DETECTED, temp456, "return of NetrServerAuthenticate3, state S1424");
                this.Manager.Comment("reaching state \'S1483\'");
                goto label46;
            }
            if ((temp459 == 1)) {
                this.Manager.Comment("reaching state \'S232\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S412\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S592\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp457;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(NonDcServer,Client)\'");
                temp457 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103509");
                this.Manager.Checkpoint("MS-NRPC_R103341");
                this.Manager.Comment("reaching state \'S772\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp457, "return of NetrServerReqChallenge, state S772");
                this.Manager.Comment("reaching state \'S952\'");
                goto label46;
            }
            if ((temp459 == 2)) {
                this.Manager.Comment("reaching state \'S233\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S413\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S593\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp458;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(NonDcServer,Client)\'");
                temp458 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103509");
                this.Manager.Checkpoint("MS-NRPC_R103341");
                this.Manager.Comment("reaching state \'S773\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp458, "return of NetrServerReqChallenge, state S773");
                this.Manager.Comment("reaching state \'S953\'");
                goto label46;
            }
            throw new InvalidOperationException("never reached");
        label46:
;
            this.Manager.EndTest();
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate3S74GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S75");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate3S74GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S75");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate3S74GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S75");
        }
        #endregion
        
        #region Test Starting in S76
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_EstablishSecureChannel_NetrServerAuthenticate3S76() {
            this.Manager.BeginTest("Test_EstablishSecureChannel_NetrServerAuthenticate3S76");
            this.Manager.Comment("reaching state \'S76\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp460;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp460);
            this.Manager.AddReturn(GetPlatformInfo, null, temp460);
            this.Manager.Comment("reaching state \'S77\'");
            int temp467 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate3.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate3S76GetPlatformChecker)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate3.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate3S76GetPlatformChecker1)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate3.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate3S76GetPlatformChecker2)));
            if ((temp467 == 0)) {
                this.Manager.Comment("reaching state \'S234\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S414\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S594\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp461;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp461 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S774\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp461, "return of NetrServerReqChallenge, state S774");
                this.Manager.Comment("reaching state \'S954\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp462;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,132)\'");
                temp462 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 132u);
                this.Manager.Checkpoint("MS-NRPC_R103371");
                this.Manager.Checkpoint("MS-NRPC_R103456");
                this.Manager.Comment("reaching state \'S1090\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/STATUS_DOWNGRADE_DETECTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_DOWNGRADE_DETECTED, temp462, "return of NetrServerAuthenticate3, state S1090");
                this.Manager.Comment("reaching state \'S1182\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp463;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp463 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S1272\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp463, "return of NetrServerReqChallenge, state S1272");
                this.Manager.Comment("reaching state \'S1361\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp464;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,132)\'");
                temp464 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 132u);
                this.Manager.Checkpoint("MS-NRPC_R103371");
                this.Manager.Checkpoint("MS-NRPC_R103456");
                this.Manager.Comment("reaching state \'S1425\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/STATUS_DOWNGRADE_DETECTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_DOWNGRADE_DETECTED, temp464, "return of NetrServerAuthenticate3, state S1425");
                this.Manager.Comment("reaching state \'S1484\'");
                goto label47;
            }
            if ((temp467 == 1)) {
                this.Manager.Comment("reaching state \'S235\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S415\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S595\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp465;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(NonDcServer,Client)\'");
                temp465 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103509");
                this.Manager.Checkpoint("MS-NRPC_R103341");
                this.Manager.Comment("reaching state \'S775\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp465, "return of NetrServerReqChallenge, state S775");
                this.Manager.Comment("reaching state \'S955\'");
                goto label47;
            }
            if ((temp467 == 2)) {
                this.Manager.Comment("reaching state \'S236\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S416\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S596\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp466;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(NonDcServer,Client)\'");
                temp466 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103509");
                this.Manager.Checkpoint("MS-NRPC_R103341");
                this.Manager.Comment("reaching state \'S776\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp466, "return of NetrServerReqChallenge, state S776");
                this.Manager.Comment("reaching state \'S956\'");
                goto label47;
            }
            throw new InvalidOperationException("never reached");
        label47:
;
            this.Manager.EndTest();
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate3S76GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S77");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate3S76GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S77");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate3S76GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S77");
        }
        #endregion
        
        #region Test Starting in S78
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_EstablishSecureChannel_NetrServerAuthenticate3S78() {
            this.Manager.BeginTest("Test_EstablishSecureChannel_NetrServerAuthenticate3S78");
            this.Manager.Comment("reaching state \'S78\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp468;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp468);
            this.Manager.AddReturn(GetPlatformInfo, null, temp468);
            this.Manager.Comment("reaching state \'S79\'");
            int temp475 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate3.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate3S78GetPlatformChecker)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate3.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate3S78GetPlatformChecker1)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate3.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate3S78GetPlatformChecker2)));
            if ((temp475 == 0)) {
                this.Manager.Comment("reaching state \'S237\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S417\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S597\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp469;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp469 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S777\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp469, "return of NetrServerReqChallenge, state S777");
                this.Manager.Comment("reaching state \'S957\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp470;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,260)\'");
                temp470 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 260u);
                this.Manager.Checkpoint("MS-NRPC_R103371");
                this.Manager.Checkpoint("MS-NRPC_R103456");
                this.Manager.Comment("reaching state \'S1091\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/STATUS_DOWNGRADE_DETECTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_DOWNGRADE_DETECTED, temp470, "return of NetrServerAuthenticate3, state S1091");
                this.Manager.Comment("reaching state \'S1183\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp471;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp471 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S1273\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp471, "return of NetrServerReqChallenge, state S1273");
                this.Manager.Comment("reaching state \'S1362\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp472;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,260)\'");
                temp472 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 260u);
                this.Manager.Checkpoint("MS-NRPC_R103371");
                this.Manager.Checkpoint("MS-NRPC_R103456");
                this.Manager.Comment("reaching state \'S1426\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/STATUS_DOWNGRADE_DETECTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_DOWNGRADE_DETECTED, temp472, "return of NetrServerAuthenticate3, state S1426");
                this.Manager.Comment("reaching state \'S1485\'");
                goto label48;
            }
            if ((temp475 == 1)) {
                this.Manager.Comment("reaching state \'S238\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S418\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S598\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp473;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(NonDcServer,Client)\'");
                temp473 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103509");
                this.Manager.Checkpoint("MS-NRPC_R103341");
                this.Manager.Comment("reaching state \'S778\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp473, "return of NetrServerReqChallenge, state S778");
                this.Manager.Comment("reaching state \'S958\'");
                goto label48;
            }
            if ((temp475 == 2)) {
                this.Manager.Comment("reaching state \'S239\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S419\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S599\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp474;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(NonDcServer,Client)\'");
                temp474 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103509");
                this.Manager.Checkpoint("MS-NRPC_R103341");
                this.Manager.Comment("reaching state \'S779\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp474, "return of NetrServerReqChallenge, state S779");
                this.Manager.Comment("reaching state \'S959\'");
                goto label48;
            }
            throw new InvalidOperationException("never reached");
        label48:
;
            this.Manager.EndTest();
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate3S78GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S79");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate3S78GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S79");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate3S78GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S79");
        }
        #endregion
        
        #region Test Starting in S8
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_EstablishSecureChannel_NetrServerAuthenticate3S8() {
            this.Manager.BeginTest("Test_EstablishSecureChannel_NetrServerAuthenticate3S8");
            this.Manager.Comment("reaching state \'S8\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp476;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp476);
            this.Manager.AddReturn(GetPlatformInfo, null, temp476);
            this.Manager.Comment("reaching state \'S9\'");
            int temp486 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate3.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate3S8GetPlatformChecker)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate3.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate3S8GetPlatformChecker1)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate3.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate3S8GetPlatformChecker2)));
            if ((temp486 == 0)) {
                this.Manager.Comment("reaching state \'S132\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S312\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S492\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp477;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp477 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S672\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp477, "return of NetrServerReqChallenge, state S672");
                this.Manager.Comment("reaching state \'S852\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp478;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,4)\'");
                temp478 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 4u);
                this.Manager.Checkpoint("MS-NRPC_R103371");
                this.Manager.Checkpoint("MS-NRPC_R103456");
                this.Manager.Comment("reaching state \'S1029\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/STATUS_DOWNGRADE_DETECTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_DOWNGRADE_DETECTED, temp478, "return of NetrServerAuthenticate3, state S1029");
                this.Manager.Comment("reaching state \'S1121\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp479;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp479 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S1213\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp479, "return of NetrServerReqChallenge, state S1213");
                this.Manager.Comment("reaching state \'S1302\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp480;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,4)\'");
                temp480 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 4u);
                this.Manager.Checkpoint("MS-NRPC_R103371");
                this.Manager.Checkpoint("MS-NRPC_R103456");
                this.Manager.AddReturn(NetrServerAuthenticate3Info, null, temp480);
                Test_EstablishSecureChannel_NetrServerAuthenticate3S1385();
                goto label49;
            }
            if ((temp486 == 1)) {
                this.Manager.Comment("reaching state \'S133\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S313\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S493\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp481;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp481 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S673\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp481, "return of NetrServerReqChallenge, state S673");
                this.Manager.Comment("reaching state \'S853\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp482;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,4)\'");
                temp482 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 4u);
                this.Manager.Checkpoint("MS-NRPC_R103371");
                this.Manager.Checkpoint("MS-NRPC_R103456");
                this.Manager.Comment("reaching state \'S1030\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/STATUS_DOWNGRADE_DETECTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_DOWNGRADE_DETECTED, temp482, "return of NetrServerAuthenticate3, state S1030");
                this.Manager.Comment("reaching state \'S1122\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp483;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp483 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S1214\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp483, "return of NetrServerReqChallenge, state S1214");
                this.Manager.Comment("reaching state \'S1303\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp484;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,4)\'");
                temp484 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 4u);
                this.Manager.Checkpoint("MS-NRPC_R103371");
                this.Manager.Checkpoint("MS-NRPC_R103456");
                this.Manager.AddReturn(NetrServerAuthenticate3Info, null, temp484);
                Test_EstablishSecureChannel_NetrServerAuthenticate3S1386();
                goto label49;
            }
            if ((temp486 == 2)) {
                this.Manager.Comment("reaching state \'S134\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S314\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S494\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp485;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(NonDcServer,Client)\'");
                temp485 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103509");
                this.Manager.Checkpoint("MS-NRPC_R103341");
                this.Manager.Comment("reaching state \'S674\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp485, "return of NetrServerReqChallenge, state S674");
                this.Manager.Comment("reaching state \'S854\'");
                goto label49;
            }
            throw new InvalidOperationException("never reached");
        label49:
;
            this.Manager.EndTest();
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate3S8GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S9");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate3S8GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S9");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate3S8GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S9");
        }
        #endregion
        
        #region Test Starting in S80
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_EstablishSecureChannel_NetrServerAuthenticate3S80() {
            this.Manager.BeginTest("Test_EstablishSecureChannel_NetrServerAuthenticate3S80");
            this.Manager.Comment("reaching state \'S80\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp487;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp487);
            this.Manager.AddReturn(GetPlatformInfo, null, temp487);
            this.Manager.Comment("reaching state \'S81\'");
            int temp494 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate3.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate3S80GetPlatformChecker)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate3.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate3S80GetPlatformChecker1)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate3.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate3S80GetPlatformChecker2)));
            if ((temp494 == 0)) {
                this.Manager.Comment("reaching state \'S240\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S420\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S600\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp488;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp488 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S780\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp488, "return of NetrServerReqChallenge, state S780");
                this.Manager.Comment("reaching state \'S960\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp489;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,516)\'");
                temp489 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 516u);
                this.Manager.Checkpoint("MS-NRPC_R103371");
                this.Manager.Checkpoint("MS-NRPC_R103456");
                this.Manager.Comment("reaching state \'S1092\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/STATUS_DOWNGRADE_DETECTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_DOWNGRADE_DETECTED, temp489, "return of NetrServerAuthenticate3, state S1092");
                this.Manager.Comment("reaching state \'S1184\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp490;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp490 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S1274\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp490, "return of NetrServerReqChallenge, state S1274");
                this.Manager.Comment("reaching state \'S1363\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp491;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,516)\'");
                temp491 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 516u);
                this.Manager.Checkpoint("MS-NRPC_R103371");
                this.Manager.Checkpoint("MS-NRPC_R103456");
                this.Manager.Comment("reaching state \'S1427\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/STATUS_DOWNGRADE_DETECTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_DOWNGRADE_DETECTED, temp491, "return of NetrServerAuthenticate3, state S1427");
                this.Manager.Comment("reaching state \'S1486\'");
                goto label50;
            }
            if ((temp494 == 1)) {
                this.Manager.Comment("reaching state \'S241\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S421\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S601\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp492;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(NonDcServer,Client)\'");
                temp492 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103509");
                this.Manager.Checkpoint("MS-NRPC_R103341");
                this.Manager.Comment("reaching state \'S781\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp492, "return of NetrServerReqChallenge, state S781");
                this.Manager.Comment("reaching state \'S961\'");
                goto label50;
            }
            if ((temp494 == 2)) {
                this.Manager.Comment("reaching state \'S242\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S422\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S602\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp493;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(NonDcServer,Client)\'");
                temp493 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103509");
                this.Manager.Checkpoint("MS-NRPC_R103341");
                this.Manager.Comment("reaching state \'S782\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp493, "return of NetrServerReqChallenge, state S782");
                this.Manager.Comment("reaching state \'S962\'");
                goto label50;
            }
            throw new InvalidOperationException("never reached");
        label50:
;
            this.Manager.EndTest();
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate3S80GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S81");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate3S80GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S81");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate3S80GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S81");
        }
        #endregion
        
        #region Test Starting in S82
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_EstablishSecureChannel_NetrServerAuthenticate3S82() {
            this.Manager.BeginTest("Test_EstablishSecureChannel_NetrServerAuthenticate3S82");
            this.Manager.Comment("reaching state \'S82\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp495;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp495);
            this.Manager.AddReturn(GetPlatformInfo, null, temp495);
            this.Manager.Comment("reaching state \'S83\'");
            int temp502 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate3.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate3S82GetPlatformChecker)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate3.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate3S82GetPlatformChecker1)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate3.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate3S82GetPlatformChecker2)));
            if ((temp502 == 0)) {
                this.Manager.Comment("reaching state \'S243\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S423\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S603\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp496;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp496 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S783\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp496, "return of NetrServerReqChallenge, state S783");
                this.Manager.Comment("reaching state \'S963\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp497;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,1028)\'");
                temp497 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 1028u);
                this.Manager.Checkpoint("MS-NRPC_R103371");
                this.Manager.Checkpoint("MS-NRPC_R103456");
                this.Manager.Comment("reaching state \'S1093\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/STATUS_DOWNGRADE_DETECTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_DOWNGRADE_DETECTED, temp497, "return of NetrServerAuthenticate3, state S1093");
                this.Manager.Comment("reaching state \'S1185\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp498;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp498 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S1275\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp498, "return of NetrServerReqChallenge, state S1275");
                this.Manager.Comment("reaching state \'S1364\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp499;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,1028)\'");
                temp499 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 1028u);
                this.Manager.Checkpoint("MS-NRPC_R103371");
                this.Manager.Checkpoint("MS-NRPC_R103456");
                this.Manager.Comment("reaching state \'S1428\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/STATUS_DOWNGRADE_DETECTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_DOWNGRADE_DETECTED, temp499, "return of NetrServerAuthenticate3, state S1428");
                this.Manager.Comment("reaching state \'S1487\'");
                goto label51;
            }
            if ((temp502 == 1)) {
                this.Manager.Comment("reaching state \'S244\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S424\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S604\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp500;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(NonDcServer,Client)\'");
                temp500 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103509");
                this.Manager.Checkpoint("MS-NRPC_R103341");
                this.Manager.Comment("reaching state \'S784\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp500, "return of NetrServerReqChallenge, state S784");
                this.Manager.Comment("reaching state \'S964\'");
                goto label51;
            }
            if ((temp502 == 2)) {
                this.Manager.Comment("reaching state \'S245\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S425\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S605\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp501;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(NonDcServer,Client)\'");
                temp501 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103509");
                this.Manager.Checkpoint("MS-NRPC_R103341");
                this.Manager.Comment("reaching state \'S785\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp501, "return of NetrServerReqChallenge, state S785");
                this.Manager.Comment("reaching state \'S965\'");
                goto label51;
            }
            throw new InvalidOperationException("never reached");
        label51:
;
            this.Manager.EndTest();
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate3S82GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S83");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate3S82GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S83");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate3S82GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S83");
        }
        #endregion
        
        #region Test Starting in S84
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_EstablishSecureChannel_NetrServerAuthenticate3S84() {
            this.Manager.BeginTest("Test_EstablishSecureChannel_NetrServerAuthenticate3S84");
            this.Manager.Comment("reaching state \'S84\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp503;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp503);
            this.Manager.AddReturn(GetPlatformInfo, null, temp503);
            this.Manager.Comment("reaching state \'S85\'");
            int temp510 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate3.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate3S84GetPlatformChecker)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate3.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate3S84GetPlatformChecker1)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate3.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate3S84GetPlatformChecker2)));
            if ((temp510 == 0)) {
                this.Manager.Comment("reaching state \'S246\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S426\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S606\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp504;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp504 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S786\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp504, "return of NetrServerReqChallenge, state S786");
                this.Manager.Comment("reaching state \'S966\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp505;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,2052)\'");
                temp505 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 2052u);
                this.Manager.Checkpoint("MS-NRPC_R103371");
                this.Manager.Checkpoint("MS-NRPC_R103456");
                this.Manager.Comment("reaching state \'S1094\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/STATUS_DOWNGRADE_DETECTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_DOWNGRADE_DETECTED, temp505, "return of NetrServerAuthenticate3, state S1094");
                this.Manager.Comment("reaching state \'S1186\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp506;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp506 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S1276\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp506, "return of NetrServerReqChallenge, state S1276");
                this.Manager.Comment("reaching state \'S1365\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp507;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,2052)\'");
                temp507 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 2052u);
                this.Manager.Checkpoint("MS-NRPC_R103371");
                this.Manager.Checkpoint("MS-NRPC_R103456");
                this.Manager.Comment("reaching state \'S1429\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/STATUS_DOWNGRADE_DETECTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_DOWNGRADE_DETECTED, temp507, "return of NetrServerAuthenticate3, state S1429");
                this.Manager.Comment("reaching state \'S1488\'");
                goto label52;
            }
            if ((temp510 == 1)) {
                this.Manager.Comment("reaching state \'S247\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S427\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S607\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp508;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(NonDcServer,Client)\'");
                temp508 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103509");
                this.Manager.Checkpoint("MS-NRPC_R103341");
                this.Manager.Comment("reaching state \'S787\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp508, "return of NetrServerReqChallenge, state S787");
                this.Manager.Comment("reaching state \'S967\'");
                goto label52;
            }
            if ((temp510 == 2)) {
                this.Manager.Comment("reaching state \'S248\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S428\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S608\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp509;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(NonDcServer,Client)\'");
                temp509 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103509");
                this.Manager.Checkpoint("MS-NRPC_R103341");
                this.Manager.Comment("reaching state \'S788\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp509, "return of NetrServerReqChallenge, state S788");
                this.Manager.Comment("reaching state \'S968\'");
                goto label52;
            }
            throw new InvalidOperationException("never reached");
        label52:
;
            this.Manager.EndTest();
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate3S84GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S85");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate3S84GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S85");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate3S84GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S85");
        }
        #endregion
        
        #region Test Starting in S86
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_EstablishSecureChannel_NetrServerAuthenticate3S86() {
            this.Manager.BeginTest("Test_EstablishSecureChannel_NetrServerAuthenticate3S86");
            this.Manager.Comment("reaching state \'S86\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp511;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp511);
            this.Manager.AddReturn(GetPlatformInfo, null, temp511);
            this.Manager.Comment("reaching state \'S87\'");
            int temp518 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate3.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate3S86GetPlatformChecker)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate3.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate3S86GetPlatformChecker1)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate3.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate3S86GetPlatformChecker2)));
            if ((temp518 == 0)) {
                this.Manager.Comment("reaching state \'S249\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S429\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S609\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp512;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp512 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S789\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp512, "return of NetrServerReqChallenge, state S789");
                this.Manager.Comment("reaching state \'S969\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp513;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,4100)\'");
                temp513 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 4100u);
                this.Manager.Checkpoint("MS-NRPC_R103371");
                this.Manager.Checkpoint("MS-NRPC_R103456");
                this.Manager.Comment("reaching state \'S1095\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/STATUS_DOWNGRADE_DETECTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_DOWNGRADE_DETECTED, temp513, "return of NetrServerAuthenticate3, state S1095");
                this.Manager.Comment("reaching state \'S1187\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp514;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp514 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S1277\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp514, "return of NetrServerReqChallenge, state S1277");
                this.Manager.Comment("reaching state \'S1366\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp515;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,4)\'");
                temp515 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 4u);
                this.Manager.Checkpoint("MS-NRPC_R103371");
                this.Manager.Checkpoint("MS-NRPC_R891");
                this.Manager.Checkpoint("MS-NRPC_R103456");
                this.Manager.AddReturn(NetrServerAuthenticate3Info, null, temp515);
                Test_EstablishSecureChannel_NetrServerAuthenticate3S1422();
                goto label53;
            }
            if ((temp518 == 1)) {
                this.Manager.Comment("reaching state \'S250\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S430\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S610\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp516;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(NonDcServer,Client)\'");
                temp516 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103509");
                this.Manager.Checkpoint("MS-NRPC_R103341");
                this.Manager.Comment("reaching state \'S790\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp516, "return of NetrServerReqChallenge, state S790");
                this.Manager.Comment("reaching state \'S970\'");
                goto label53;
            }
            if ((temp518 == 2)) {
                this.Manager.Comment("reaching state \'S251\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S431\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S611\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp517;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(NonDcServer,Client)\'");
                temp517 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103509");
                this.Manager.Checkpoint("MS-NRPC_R103341");
                this.Manager.Comment("reaching state \'S791\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp517, "return of NetrServerReqChallenge, state S791");
                this.Manager.Comment("reaching state \'S971\'");
                goto label53;
            }
            throw new InvalidOperationException("never reached");
        label53:
;
            this.Manager.EndTest();
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate3S86GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S87");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate3S86GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S87");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate3S86GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S87");
        }
        #endregion
        
        #region Test Starting in S88
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_EstablishSecureChannel_NetrServerAuthenticate3S88() {
            this.Manager.BeginTest("Test_EstablishSecureChannel_NetrServerAuthenticate3S88");
            this.Manager.Comment("reaching state \'S88\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp519;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp519);
            this.Manager.AddReturn(GetPlatformInfo, null, temp519);
            this.Manager.Comment("reaching state \'S89\'");
            int temp526 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate3.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate3S88GetPlatformChecker)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate3.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate3S88GetPlatformChecker1)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate3.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate3S88GetPlatformChecker2)));
            if ((temp526 == 0)) {
                this.Manager.Comment("reaching state \'S252\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S432\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S612\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp520;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp520 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S792\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp520, "return of NetrServerReqChallenge, state S792");
                this.Manager.Comment("reaching state \'S972\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp521;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,8196)\'");
                temp521 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 8196u);
                this.Manager.Checkpoint("MS-NRPC_R103371");
                this.Manager.Checkpoint("MS-NRPC_R103456");
                this.Manager.Comment("reaching state \'S1096\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/STATUS_DOWNGRADE_DETECTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_DOWNGRADE_DETECTED, temp521, "return of NetrServerAuthenticate3, state S1096");
                this.Manager.Comment("reaching state \'S1188\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp522;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp522 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S1278\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp522, "return of NetrServerReqChallenge, state S1278");
                this.Manager.Comment("reaching state \'S1367\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp523;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,4)\'");
                temp523 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 4u);
                this.Manager.Checkpoint("MS-NRPC_R103371");
                this.Manager.Checkpoint("MS-NRPC_R892");
                this.Manager.Checkpoint("MS-NRPC_R103456");
                this.Manager.AddReturn(NetrServerAuthenticate3Info, null, temp523);
                Test_EstablishSecureChannel_NetrServerAuthenticate3S1422();
                goto label54;
            }
            if ((temp526 == 1)) {
                this.Manager.Comment("reaching state \'S253\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S433\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S613\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp524;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(NonDcServer,Client)\'");
                temp524 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103509");
                this.Manager.Checkpoint("MS-NRPC_R103341");
                this.Manager.Comment("reaching state \'S793\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp524, "return of NetrServerReqChallenge, state S793");
                this.Manager.Comment("reaching state \'S973\'");
                goto label54;
            }
            if ((temp526 == 2)) {
                this.Manager.Comment("reaching state \'S254\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S434\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S614\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp525;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(NonDcServer,Client)\'");
                temp525 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103509");
                this.Manager.Checkpoint("MS-NRPC_R103341");
                this.Manager.Comment("reaching state \'S794\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp525, "return of NetrServerReqChallenge, state S794");
                this.Manager.Comment("reaching state \'S974\'");
                goto label54;
            }
            throw new InvalidOperationException("never reached");
        label54:
;
            this.Manager.EndTest();
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate3S88GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S89");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate3S88GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S89");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate3S88GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S89");
        }
        #endregion
        
        #region Test Starting in S90
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_EstablishSecureChannel_NetrServerAuthenticate3S90() {
            this.Manager.BeginTest("Test_EstablishSecureChannel_NetrServerAuthenticate3S90");
            this.Manager.Comment("reaching state \'S90\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp527;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp527);
            this.Manager.AddReturn(GetPlatformInfo, null, temp527);
            this.Manager.Comment("reaching state \'S91\'");
            int temp534 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate3.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate3S90GetPlatformChecker)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate3.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate3S90GetPlatformChecker1)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate3.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate3S90GetPlatformChecker2)));
            if ((temp534 == 0)) {
                this.Manager.Comment("reaching state \'S255\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S435\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S615\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp528;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp528 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S795\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp528, "return of NetrServerReqChallenge, state S795");
                this.Manager.Comment("reaching state \'S975\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp529;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,32772)\'");
                temp529 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 32772u);
                this.Manager.Checkpoint("MS-NRPC_R103371");
                this.Manager.Checkpoint("MS-NRPC_R103456");
                this.Manager.Comment("reaching state \'S1097\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/STATUS_DOWNGRADE_DETECTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_DOWNGRADE_DETECTED, temp529, "return of NetrServerAuthenticate3, state S1097");
                this.Manager.Comment("reaching state \'S1189\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp530;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp530 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S1279\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp530, "return of NetrServerReqChallenge, state S1279");
                this.Manager.Comment("reaching state \'S1368\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp531;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,32772)\'");
                temp531 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 32772u);
                this.Manager.Checkpoint("MS-NRPC_R103371");
                this.Manager.Checkpoint("MS-NRPC_R103456");
                this.Manager.Comment("reaching state \'S1430\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/STATUS_DOWNGRADE_DETECTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_DOWNGRADE_DETECTED, temp531, "return of NetrServerAuthenticate3, state S1430");
                this.Manager.Comment("reaching state \'S1489\'");
                goto label55;
            }
            if ((temp534 == 1)) {
                this.Manager.Comment("reaching state \'S256\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S436\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S616\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp532;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(NonDcServer,Client)\'");
                temp532 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103509");
                this.Manager.Checkpoint("MS-NRPC_R103341");
                this.Manager.Comment("reaching state \'S796\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp532, "return of NetrServerReqChallenge, state S796");
                this.Manager.Comment("reaching state \'S976\'");
                goto label55;
            }
            if ((temp534 == 2)) {
                this.Manager.Comment("reaching state \'S257\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S437\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S617\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp533;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(NonDcServer,Client)\'");
                temp533 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103509");
                this.Manager.Checkpoint("MS-NRPC_R103341");
                this.Manager.Comment("reaching state \'S797\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp533, "return of NetrServerReqChallenge, state S797");
                this.Manager.Comment("reaching state \'S977\'");
                goto label55;
            }
            throw new InvalidOperationException("never reached");
        label55:
;
            this.Manager.EndTest();
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate3S90GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S91");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate3S90GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S91");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate3S90GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S91");
        }
        #endregion
        
        #region Test Starting in S92
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_EstablishSecureChannel_NetrServerAuthenticate3S92() {
            this.Manager.BeginTest("Test_EstablishSecureChannel_NetrServerAuthenticate3S92");
            this.Manager.Comment("reaching state \'S92\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp535;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp535);
            this.Manager.AddReturn(GetPlatformInfo, null, temp535);
            this.Manager.Comment("reaching state \'S93\'");
            int temp542 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate3.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate3S92GetPlatformChecker)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate3.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate3S92GetPlatformChecker1)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate3.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate3S92GetPlatformChecker2)));
            if ((temp542 == 0)) {
                this.Manager.Comment("reaching state \'S258\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S438\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S618\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp536;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp536 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S798\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp536, "return of NetrServerReqChallenge, state S798");
                this.Manager.Comment("reaching state \'S978\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp537;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,65540)\'");
                temp537 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 65540u);
                this.Manager.Checkpoint("MS-NRPC_R103371");
                this.Manager.Checkpoint("MS-NRPC_R103456");
                this.Manager.Comment("reaching state \'S1098\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/STATUS_DOWNGRADE_DETECTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_DOWNGRADE_DETECTED, temp537, "return of NetrServerAuthenticate3, state S1098");
                this.Manager.Comment("reaching state \'S1190\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp538;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp538 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S1280\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp538, "return of NetrServerReqChallenge, state S1280");
                this.Manager.Comment("reaching state \'S1369\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp539;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,4)\'");
                temp539 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 4u);
                this.Manager.Checkpoint("MS-NRPC_R103371");
                this.Manager.Checkpoint("MS-NRPC_R893");
                this.Manager.Checkpoint("MS-NRPC_R103456");
                this.Manager.AddReturn(NetrServerAuthenticate3Info, null, temp539);
                Test_EstablishSecureChannel_NetrServerAuthenticate3S1422();
                goto label56;
            }
            if ((temp542 == 1)) {
                this.Manager.Comment("reaching state \'S259\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S439\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S619\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp540;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(NonDcServer,Client)\'");
                temp540 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103509");
                this.Manager.Checkpoint("MS-NRPC_R103341");
                this.Manager.Comment("reaching state \'S799\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp540, "return of NetrServerReqChallenge, state S799");
                this.Manager.Comment("reaching state \'S979\'");
                goto label56;
            }
            if ((temp542 == 2)) {
                this.Manager.Comment("reaching state \'S260\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S440\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S620\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp541;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(NonDcServer,Client)\'");
                temp541 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103509");
                this.Manager.Checkpoint("MS-NRPC_R103341");
                this.Manager.Comment("reaching state \'S800\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp541, "return of NetrServerReqChallenge, state S800");
                this.Manager.Comment("reaching state \'S980\'");
                goto label56;
            }
            throw new InvalidOperationException("never reached");
        label56:
;
            this.Manager.EndTest();
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate3S92GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S93");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate3S92GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S93");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate3S92GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S93");
        }
        #endregion
        
        #region Test Starting in S94
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_EstablishSecureChannel_NetrServerAuthenticate3S94() {
            this.Manager.BeginTest("Test_EstablishSecureChannel_NetrServerAuthenticate3S94");
            this.Manager.Comment("reaching state \'S94\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp543;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp543);
            this.Manager.AddReturn(GetPlatformInfo, null, temp543);
            this.Manager.Comment("reaching state \'S95\'");
            int temp550 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate3.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate3S94GetPlatformChecker)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate3.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate3S94GetPlatformChecker1)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate3.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate3S94GetPlatformChecker2)));
            if ((temp550 == 0)) {
                this.Manager.Comment("reaching state \'S261\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S441\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S621\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp544;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp544 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S801\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp544, "return of NetrServerReqChallenge, state S801");
                this.Manager.Comment("reaching state \'S981\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp545;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,2147483652)\'");
                temp545 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 2147483652u);
                this.Manager.Checkpoint("MS-NRPC_R103371");
                this.Manager.Checkpoint("MS-NRPC_R103456");
                this.Manager.Comment("reaching state \'S1099\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/STATUS_DOWNGRADE_DETECTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_DOWNGRADE_DETECTED, temp545, "return of NetrServerAuthenticate3, state S1099");
                this.Manager.Comment("reaching state \'S1191\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp546;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp546 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S1281\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp546, "return of NetrServerReqChallenge, state S1281");
                this.Manager.Comment("reaching state \'S1370\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp547;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,4)\'");
                temp547 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 4u);
                this.Manager.Checkpoint("MS-NRPC_R103371");
                this.Manager.Checkpoint("MS-NRPC_R897");
                this.Manager.Checkpoint("MS-NRPC_R103456");
                this.Manager.AddReturn(NetrServerAuthenticate3Info, null, temp547);
                Test_EstablishSecureChannel_NetrServerAuthenticate3S1422();
                goto label57;
            }
            if ((temp550 == 1)) {
                this.Manager.Comment("reaching state \'S262\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S442\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S622\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp548;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(NonDcServer,Client)\'");
                temp548 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103509");
                this.Manager.Checkpoint("MS-NRPC_R103341");
                this.Manager.Comment("reaching state \'S802\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp548, "return of NetrServerReqChallenge, state S802");
                this.Manager.Comment("reaching state \'S982\'");
                goto label57;
            }
            if ((temp550 == 2)) {
                this.Manager.Comment("reaching state \'S263\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S443\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S623\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp549;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(NonDcServer,Client)\'");
                temp549 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103509");
                this.Manager.Checkpoint("MS-NRPC_R103341");
                this.Manager.Comment("reaching state \'S803\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp549, "return of NetrServerReqChallenge, state S803");
                this.Manager.Comment("reaching state \'S983\'");
                goto label57;
            }
            throw new InvalidOperationException("never reached");
        label57:
;
            this.Manager.EndTest();
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate3S94GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S95");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate3S94GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S95");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate3S94GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S95");
        }
        #endregion
        
        #region Test Starting in S96
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_EstablishSecureChannel_NetrServerAuthenticate3S96() {
            this.Manager.BeginTest("Test_EstablishSecureChannel_NetrServerAuthenticate3S96");
            this.Manager.Comment("reaching state \'S96\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp551;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp551);
            this.Manager.AddReturn(GetPlatformInfo, null, temp551);
            this.Manager.Comment("reaching state \'S97\'");
            int temp558 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate3.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate3S96GetPlatformChecker)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate3.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate3S96GetPlatformChecker1)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate3.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate3S96GetPlatformChecker2)));
            if ((temp558 == 0)) {
                this.Manager.Comment("reaching state \'S264\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S444\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S624\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp552;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp552 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S804\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp552, "return of NetrServerReqChallenge, state S804");
                this.Manager.Comment("reaching state \'S984\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp553;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,131076)\'");
                temp553 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 131076u);
                this.Manager.Checkpoint("MS-NRPC_R103371");
                this.Manager.Checkpoint("MS-NRPC_R103456");
                this.Manager.Comment("reaching state \'S1100\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/STATUS_DOWNGRADE_DETECTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_DOWNGRADE_DETECTED, temp553, "return of NetrServerAuthenticate3, state S1100");
                this.Manager.Comment("reaching state \'S1192\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp554;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp554 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S1282\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp554, "return of NetrServerReqChallenge, state S1282");
                this.Manager.Comment("reaching state \'S1371\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp555;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,131076)\'");
                temp555 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 131076u);
                this.Manager.Checkpoint("MS-NRPC_R103371");
                this.Manager.Checkpoint("MS-NRPC_R103456");
                this.Manager.Comment("reaching state \'S1431\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/STATUS_DOWNGRADE_DETECTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_DOWNGRADE_DETECTED, temp555, "return of NetrServerAuthenticate3, state S1431");
                this.Manager.Comment("reaching state \'S1490\'");
                goto label58;
            }
            if ((temp558 == 1)) {
                this.Manager.Comment("reaching state \'S265\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S445\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S625\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp556;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(NonDcServer,Client)\'");
                temp556 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103509");
                this.Manager.Checkpoint("MS-NRPC_R103341");
                this.Manager.Comment("reaching state \'S805\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp556, "return of NetrServerReqChallenge, state S805");
                this.Manager.Comment("reaching state \'S985\'");
                goto label58;
            }
            if ((temp558 == 2)) {
                this.Manager.Comment("reaching state \'S266\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S446\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S626\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp557;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(NonDcServer,Client)\'");
                temp557 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103509");
                this.Manager.Checkpoint("MS-NRPC_R103341");
                this.Manager.Comment("reaching state \'S806\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp557, "return of NetrServerReqChallenge, state S806");
                this.Manager.Comment("reaching state \'S986\'");
                goto label58;
            }
            throw new InvalidOperationException("never reached");
        label58:
;
            this.Manager.EndTest();
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate3S96GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S97");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate3S96GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S97");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate3S96GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S97");
        }
        #endregion
        
        #region Test Starting in S98
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_EstablishSecureChannel_NetrServerAuthenticate3S98() {
            this.Manager.BeginTest("Test_EstablishSecureChannel_NetrServerAuthenticate3S98");
            this.Manager.Comment("reaching state \'S98\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp559;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp559);
            this.Manager.AddReturn(GetPlatformInfo, null, temp559);
            this.Manager.Comment("reaching state \'S99\'");
            int temp566 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate3.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate3S98GetPlatformChecker)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate3.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate3S98GetPlatformChecker1)), new ExpectedReturn(Test_EstablishSecureChannel_NetrServerAuthenticate3.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_EstablishSecureChannel_NetrServerAuthenticate3S98GetPlatformChecker2)));
            if ((temp566 == 0)) {
                this.Manager.Comment("reaching state \'S267\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S447\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S627\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp560;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp560 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S807\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp560, "return of NetrServerReqChallenge, state S807");
                this.Manager.Comment("reaching state \'S987\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp561;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,262148)\'");
                temp561 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 262148u);
                this.Manager.Checkpoint("MS-NRPC_R103371");
                this.Manager.Checkpoint("MS-NRPC_R103456");
                this.Manager.Comment("reaching state \'S1101\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/STATUS_DOWNGRADE_DETECTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_DOWNGRADE_DETECTED, temp561, "return of NetrServerAuthenticate3, state S1101");
                this.Manager.Comment("reaching state \'S1193\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp562;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp562 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S1283\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp562, "return of NetrServerReqChallenge, state S1283");
                this.Manager.Comment("reaching state \'S1372\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp563;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate3(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,262148)\'");
                temp563 = this.INrpcServerAdapterInstance.NetrServerAuthenticate3(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client, true, 262148u);
                this.Manager.Checkpoint("MS-NRPC_R103371");
                this.Manager.Checkpoint("MS-NRPC_R103456");
                this.Manager.Comment("reaching state \'S1432\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate3/STATUS_DOWNGRADE_DETECTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_DOWNGRADE_DETECTED, temp563, "return of NetrServerAuthenticate3, state S1432");
                this.Manager.Comment("reaching state \'S1491\'");
                goto label59;
            }
            if ((temp566 == 1)) {
                this.Manager.Comment("reaching state \'S268\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S448\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S628\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp564;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(NonDcServer,Client)\'");
                temp564 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103509");
                this.Manager.Checkpoint("MS-NRPC_R103341");
                this.Manager.Comment("reaching state \'S808\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp564, "return of NetrServerReqChallenge, state S808");
                this.Manager.Comment("reaching state \'S988\'");
                goto label59;
            }
            if ((temp566 == 2)) {
                this.Manager.Comment("reaching state \'S269\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S449\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S629\'");
                Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp565;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(NonDcServer,Client)\'");
                temp565 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R425");
                this.Manager.Checkpoint("MS-NRPC_R103509");
                this.Manager.Checkpoint("MS-NRPC_R103341");
                this.Manager.Comment("reaching state \'S809\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/STATUS_NOT_SUPPORTED\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_NOT_SUPPORTED, temp565, "return of NetrServerReqChallenge, state S809");
                this.Manager.Comment("reaching state \'S989\'");
                goto label59;
            }
            throw new InvalidOperationException("never reached");
        label59:
;
            this.Manager.EndTest();
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate3S98GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S99");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate3S98GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S99");
        }
        
        private void Test_EstablishSecureChannel_NetrServerAuthenticate3S98GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S99");
        }
        #endregion
    }
}
