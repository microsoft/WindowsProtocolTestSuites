// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc {
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Reflection;
    using Microsoft.SpecExplorer.Runtime.Testing;
    using Microsoft.Protocols.TestTools;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Spec Explorer", "3.5.3146.0")]
    [TestClassAttribute()]
    public partial class BVT_Test_EstablishSecureChannel_NetrServerAuthenticate2 : PtfTestClassBase {
        
        public BVT_Test_EstablishSecureChannel_NetrServerAuthenticate2() {
            this.SetSwitch("ProceedControlTimeout", "100");
            this.SetSwitch("QuiescenceTimeout", "30000");
        }
        
        #region Expect Delegates
        public delegate void GetPlatformDelegate1(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType sutPlatform);
        #endregion
        
        #region Event Metadata
        static System.Reflection.MethodBase GetPlatformInfo = TestManagerHelpers.GetMethodInfo(typeof(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.INrpcServerAdapter), "GetPlatform", typeof(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType).MakeByRefType());
        #endregion
        
        #region Adapter Instances
        private Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.INrpcServerAdapter INrpcServerAdapterInstance;
        
        private Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.INrpcServerSutControlAdapter INrpcServerSutControlAdapterInstance;
        #endregion
        
        #region Class Initialization and Cleanup
        [ClassInitializeAttribute()]
        public static void ClassInitialize(TestContext context) {
            PtfTestClassBase.Initialize(context);
        }
        
        [ClassCleanupAttribute()]
        public static void ClassCleanup() {
            PtfTestClassBase.Cleanup();
        }
        #endregion
        
        #region Test Initialization and Cleanup
        protected override void TestInitialize() {
            this.InitializeTestManager();
            this.INrpcServerAdapterInstance = ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.INrpcServerAdapter)(this.Manager.GetAdapter(typeof(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.INrpcServerAdapter))));
            this.INrpcServerSutControlAdapterInstance = ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.INrpcServerSutControlAdapter)(this.Manager.GetAdapter(typeof(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.INrpcServerSutControlAdapter))));
        }
        
        protected override void TestCleanup() {
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
        public void NRPC_BVT_Test_EstablishSecureChannel_NetrServerAuthenticate2S0()
        {
            this.Manager.BeginTest("BVT_Test_EstablishSecureChannel_NetrServerAuthenticate2S0");
            this.Manager.Comment("reaching state \'S0\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType temp0;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp0);
            this.Manager.AddReturn(GetPlatformInfo, null, temp0);
            this.Manager.Comment("reaching state \'S1\'");
            int temp5 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(BVT_Test_EstablishSecureChannel_NetrServerAuthenticate2.GetPlatformInfo, null, new GetPlatformDelegate1(this.BVT_Test_EstablishSecureChannel_NetrServerAuthenticate2S0GetPlatformChecker)), new ExpectedReturn(BVT_Test_EstablishSecureChannel_NetrServerAuthenticate2.GetPlatformInfo, null, new GetPlatformDelegate1(this.BVT_Test_EstablishSecureChannel_NetrServerAuthenticate2S0GetPlatformChecker1)));
            if ((temp5 == 0)) {
                this.Manager.Comment("reaching state \'S4\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S8\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S12\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp1;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp1 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S16\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp1, "return of NetrServerReqChallenge, state S16");
                this.Manager.Comment("reaching state \'S20\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp2;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate2(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16384)\'");
                temp2 = this.INrpcServerAdapterInstance.NetrServerAuthenticate2(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.Client, true, 16384u);
                this.Manager.Checkpoint("MS-NRPC_R103478");
                this.Manager.Comment("reaching state \'S24\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate2/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp2, "return of NetrServerAuthenticate2, state S24");
                this.Manager.Comment("reaching state \'S28\'");
                goto label0;
            }
            if ((temp5 == 1)) {
                this.Manager.Comment("reaching state \'S5\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S9\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S13\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp3;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp3 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S17\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp3, "return of NetrServerReqChallenge, state S17");
                this.Manager.Comment("reaching state \'S21\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp4;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate2(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16384)\'");
                temp4 = this.INrpcServerAdapterInstance.NetrServerAuthenticate2(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.Client, true, 16384u);
                this.Manager.Checkpoint("MS-NRPC_R103478");
                this.Manager.Comment("reaching state \'S25\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate2/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp4, "return of NetrServerAuthenticate2, state S25");
                this.Manager.Comment("reaching state \'S29\'");
                goto label0;
            }
            throw new InvalidOperationException("never reached");
        label0:
;
            this.Manager.EndTest();
        }
        
        private void BVT_Test_EstablishSecureChannel_NetrServerAuthenticate2S0GetPlatformChecker(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S1");
        }
        
        private void BVT_Test_EstablishSecureChannel_NetrServerAuthenticate2S0GetPlatformChecker1(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S1");
        }
        #endregion
        
        #region Test Starting in S2
        [TestMethod]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("BVT")]
        [TestCategory("PDC")]
        [TestCategory("MS-NRPC")]
        public void NRPC_BVT_Test_EstablishSecureChannel_NetrServerAuthenticate2S2()
        {
            this.Manager.BeginTest("BVT_Test_EstablishSecureChannel_NetrServerAuthenticate2S2");
            this.Manager.Comment("reaching state \'S2\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType temp6;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp6);
            this.Manager.AddReturn(GetPlatformInfo, null, temp6);
            this.Manager.Comment("reaching state \'S3\'");
            int temp11 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(BVT_Test_EstablishSecureChannel_NetrServerAuthenticate2.GetPlatformInfo, null, new GetPlatformDelegate1(this.BVT_Test_EstablishSecureChannel_NetrServerAuthenticate2S2GetPlatformChecker)), new ExpectedReturn(BVT_Test_EstablishSecureChannel_NetrServerAuthenticate2.GetPlatformInfo, null, new GetPlatformDelegate1(this.BVT_Test_EstablishSecureChannel_NetrServerAuthenticate2S2GetPlatformChecker1)));
            if ((temp11 == 0)) {
                this.Manager.Comment("reaching state \'S6\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S10\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S14\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp7;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp7 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S18\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp7, "return of NetrServerReqChallenge, state S18");
                this.Manager.Comment("reaching state \'S22\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp8;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate2(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16777216)\'");
                temp8 = this.INrpcServerAdapterInstance.NetrServerAuthenticate2(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.Client, true, 16777216u);
                this.Manager.Checkpoint("MS-NRPC_R103478");
                this.Manager.Comment("reaching state \'S26\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate2/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp8, "return of NetrServerAuthenticate2, state S26");
                this.Manager.Comment("reaching state \'S30\'");
                goto label1;
            }
            if ((temp11 == 1)) {
                this.Manager.Comment("reaching state \'S7\'");
                this.Manager.Comment("executing step \'call ConfigServer(False,False)\'");
                this.INrpcServerSutControlAdapterInstance.ConfigServer(false, false);
                this.Manager.Comment("reaching state \'S11\'");
                this.Manager.Comment("checking step \'return ConfigServer\'");
                this.Manager.Comment("reaching state \'S15\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp9;
                this.Manager.Comment("executing step \'call NetrServerReqChallenge(PrimaryDc,Client)\'");
                temp9 = this.INrpcServerAdapterInstance.NetrServerReqChallenge(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.Client);
                this.Manager.Checkpoint("MS-NRPC_R103340");
                this.Manager.Comment("reaching state \'S19\'");
                this.Manager.Comment("checking step \'return NetrServerReqChallenge/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp9, "return of NetrServerReqChallenge, state S19");
                this.Manager.Comment("reaching state \'S23\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT temp10;
                this.Manager.Comment("executing step \'call NetrServerAuthenticate2(PrimaryDc,DomainMemberComputerAccoun" +
                        "t,WorkstationSecureChannel,Client,True,16384)\'");
                temp10 = this.INrpcServerAdapterInstance.NetrServerAuthenticate2(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.AccounterNameType.DomainMemberComputerAccount, Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc._NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.ComputerType.Client, true, 16384u);
                this.Manager.Checkpoint("MS-NRPC_R103478");
                this.Manager.Comment("reaching state \'S27\'");
                this.Manager.Comment("checking step \'return NetrServerAuthenticate2/ERROR_SUCCESS\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.HRESULT)(0)), temp10, "return of NetrServerAuthenticate2, state S27");
                this.Manager.Comment("reaching state \'S31\'");
                goto label1;
            }
            throw new InvalidOperationException("never reached");
        label1:
;
            this.Manager.EndTest();
        }
        
        private void BVT_Test_EstablishSecureChannel_NetrServerAuthenticate2S2GetPlatformChecker(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S3");
        }
        
        private void BVT_Test_EstablishSecureChannel_NetrServerAuthenticate2S2GetPlatformChecker1(Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S3");
        }
        #endregion
    }
}
