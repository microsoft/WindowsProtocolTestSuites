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
    public partial class Test_AuthenticateMessage : PtfTestClassBase {
        
        public Test_AuthenticateMessage() {
            this.SetSwitch("ProceedControlTimeout", "100");
            this.SetSwitch("QuiescenceTimeout", "30000");
        }
        
        #region Expect Delegates
        public delegate void GetPlatformDelegate1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform);
        
        public delegate void GetClientAccountTypeDelegate1(bool isAdministrator);
        #endregion
        
        #region Event Metadata
        static System.Reflection.MethodBase GetPlatformInfo = TestManagerHelpers.GetMethodInfo(typeof(Microsoft.Protocols.TestSuites.Nrpc.INrpcServerAdapter), "GetPlatform", typeof(Microsoft.Protocols.TestSuites.Nrpc.PlatformType).MakeByRefType());
        
        static System.Reflection.MethodBase GetClientAccountTypeInfo = TestManagerHelpers.GetMethodInfo(typeof(Microsoft.Protocols.TestSuites.Nrpc.INrpcServerAdapter), "GetClientAccountType", typeof(bool).MakeByRefType());
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
        public void Test_AuthenticateMessageS0() {
            this.Manager.BeginTest("Test_AuthenticateMessageS0");
            this.Manager.Comment("reaching state \'S0\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp0;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp0);
            this.Manager.AddReturn(GetPlatformInfo, null, temp0);
            this.Manager.Comment("reaching state \'S1\'");
            int temp13 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_AuthenticateMessageS0GetPlatformChecker)), new ExpectedReturn(Test_AuthenticateMessage.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_AuthenticateMessageS0GetPlatformChecker1)), new ExpectedReturn(Test_AuthenticateMessage.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_AuthenticateMessageS0GetPlatformChecker2)));
            if ((temp13 == 0)) {
                this.Manager.Comment("reaching state \'S116\'");
                bool temp1;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp1);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp1);
                this.Manager.Comment("reaching state \'S290\'");
                int temp4 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS0GetClientAccountTypeChecker)), new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS0GetClientAccountTypeChecker1)));
                if ((temp4 == 0)) {
                    this.Manager.Comment("reaching state \'S464\'");
                    this.Manager.Comment("executing step \'call ChangeNonDCMachineAccountStatus(False)\'");
                    this.INrpcServerSutControlAdapterInstance.ChangeNonDCMachineAccountStatus(false);
                    this.Manager.Comment("reaching state \'S812\'");
                    this.Manager.Comment("checking step \'return ChangeNonDCMachineAccountStatus\'");
                    this.Manager.Comment("reaching state \'S1160\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp2;
                    this.Manager.Comment("executing step \'call NetrLogonComputeServerDigest(PrimaryDc,RidOfNonDcMachineAcco" +
                            "unt,MessageOne)\'");
                    temp2 = this.INrpcServerAdapterInstance.NetrLogonComputeServerDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.RidType.RidOfNonDcMachineAccount, ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103757");
                    this.Manager.Checkpoint("MS-NRPC_R103743");
                    this.Manager.Comment("reaching state \'S1376\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeServerDigest/ERROR_NO_SUCH_USER\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SUCH_USER, temp2, "return of NetrLogonComputeServerDigest, state S1376");
                    this.Manager.Comment("reaching state \'S1442\'");
                    this.Manager.Comment("executing step \'call ChangeNonDCMachineAccountStatus(True)\'");
                    this.INrpcServerSutControlAdapterInstance.ChangeNonDCMachineAccountStatus(true);
                    this.Manager.Comment("reaching state \'S1460\'");
                    this.Manager.Comment("checking step \'return ChangeNonDCMachineAccountStatus\'");
                    Test_AuthenticateMessageS1285();
                    goto label0;
                }
                if ((temp4 == 1)) {
                    this.Manager.Comment("reaching state \'S465\'");
                    this.Manager.Comment("executing step \'call ChangeNonDCMachineAccountStatus(False)\'");
                    this.INrpcServerSutControlAdapterInstance.ChangeNonDCMachineAccountStatus(false);
                    this.Manager.Comment("reaching state \'S813\'");
                    this.Manager.Comment("checking step \'return ChangeNonDCMachineAccountStatus\'");
                    this.Manager.Comment("reaching state \'S1161\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp3;
                    this.Manager.Comment("executing step \'call NetrLogonComputeServerDigest(PrimaryDc,RidOfNonDcMachineAcco" +
                            "unt,MessageOne)\'");
                    temp3 = this.INrpcServerAdapterInstance.NetrLogonComputeServerDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.RidType.RidOfNonDcMachineAccount, ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103757");
                    this.Manager.Checkpoint("MS-NRPC_R103743");
                    this.Manager.Comment("reaching state \'S1377\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeServerDigest/ERROR_NO_SUCH_USER\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SUCH_USER, temp3, "return of NetrLogonComputeServerDigest, state S1377");
                    this.Manager.Comment("reaching state \'S1443\'");
                    this.Manager.Comment("executing step \'call ChangeNonDCMachineAccountStatus(True)\'");
                    this.INrpcServerSutControlAdapterInstance.ChangeNonDCMachineAccountStatus(true);
                    this.Manager.Comment("reaching state \'S1461\'");
                    this.Manager.Comment("checking step \'return ChangeNonDCMachineAccountStatus\'");
                    Test_AuthenticateMessageS1284();
                    goto label0;
                }
                throw new InvalidOperationException("never reached");
            label0:
;
                goto label3;
            }
            if ((temp13 == 1)) {
                this.Manager.Comment("reaching state \'S117\'");
                bool temp5;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp5);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp5);
                this.Manager.Comment("reaching state \'S291\'");
                int temp8 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS0GetClientAccountTypeChecker2)), new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS0GetClientAccountTypeChecker3)));
                if ((temp8 == 0)) {
                    this.Manager.Comment("reaching state \'S466\'");
                    this.Manager.Comment("executing step \'call ChangeNonDCMachineAccountStatus(False)\'");
                    this.INrpcServerSutControlAdapterInstance.ChangeNonDCMachineAccountStatus(false);
                    this.Manager.Comment("reaching state \'S814\'");
                    this.Manager.Comment("checking step \'return ChangeNonDCMachineAccountStatus\'");
                    this.Manager.Comment("reaching state \'S1162\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp6;
                    this.Manager.Comment("executing step \'call NetrLogonComputeServerDigest(PrimaryDc,RidOfNonDcMachineAcco" +
                            "unt,MessageOne)\'");
                    temp6 = this.INrpcServerAdapterInstance.NetrLogonComputeServerDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.RidType.RidOfNonDcMachineAccount, ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103757");
                    this.Manager.Checkpoint("MS-NRPC_R103743");
                    this.Manager.Comment("reaching state \'S1378\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeServerDigest/ERROR_NO_SUCH_USER\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SUCH_USER, temp6, "return of NetrLogonComputeServerDigest, state S1378");
                    this.Manager.Comment("reaching state \'S1444\'");
                    this.Manager.Comment("executing step \'call ChangeNonDCMachineAccountStatus(True)\'");
                    this.INrpcServerSutControlAdapterInstance.ChangeNonDCMachineAccountStatus(true);
                    this.Manager.Comment("reaching state \'S1462\'");
                    this.Manager.Comment("checking step \'return ChangeNonDCMachineAccountStatus\'");
                    Test_AuthenticateMessageS1245();
                    goto label1;
                }
                if ((temp8 == 1)) {
                    this.Manager.Comment("reaching state \'S467\'");
                    this.Manager.Comment("executing step \'call ChangeNonDCMachineAccountStatus(False)\'");
                    this.INrpcServerSutControlAdapterInstance.ChangeNonDCMachineAccountStatus(false);
                    this.Manager.Comment("reaching state \'S815\'");
                    this.Manager.Comment("checking step \'return ChangeNonDCMachineAccountStatus\'");
                    this.Manager.Comment("reaching state \'S1163\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp7;
                    this.Manager.Comment("executing step \'call NetrLogonComputeServerDigest(PrimaryDc,RidOfNonDcMachineAcco" +
                            "unt,MessageOne)\'");
                    temp7 = this.INrpcServerAdapterInstance.NetrLogonComputeServerDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.RidType.RidOfNonDcMachineAccount, ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103757");
                    this.Manager.Checkpoint("MS-NRPC_R103743");
                    this.Manager.Comment("reaching state \'S1379\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeServerDigest/ERROR_NO_SUCH_USER\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SUCH_USER, temp7, "return of NetrLogonComputeServerDigest, state S1379");
                    this.Manager.Comment("reaching state \'S1445\'");
                    this.Manager.Comment("executing step \'call ChangeNonDCMachineAccountStatus(True)\'");
                    this.INrpcServerSutControlAdapterInstance.ChangeNonDCMachineAccountStatus(true);
                    this.Manager.Comment("reaching state \'S1463\'");
                    this.Manager.Comment("checking step \'return ChangeNonDCMachineAccountStatus\'");
                    Test_AuthenticateMessageS1244();
                    goto label1;
                }
                throw new InvalidOperationException("never reached");
            label1:
;
                goto label3;
            }
            if ((temp13 == 2)) {
                this.Manager.Comment("reaching state \'S118\'");
                bool temp9;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp9);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp9);
                this.Manager.Comment("reaching state \'S292\'");
                int temp12 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS0GetClientAccountTypeChecker4)), new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS0GetClientAccountTypeChecker5)));
                if ((temp12 == 0)) {
                    this.Manager.Comment("reaching state \'S468\'");
                    this.Manager.Comment("executing step \'call ChangeNonDCMachineAccountStatus(False)\'");
                    this.INrpcServerSutControlAdapterInstance.ChangeNonDCMachineAccountStatus(false);
                    this.Manager.Comment("reaching state \'S816\'");
                    this.Manager.Comment("checking step \'return ChangeNonDCMachineAccountStatus\'");
                    this.Manager.Comment("reaching state \'S1164\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp10;
                    this.Manager.Comment("executing step \'call NetrLogonComputeServerDigest(PrimaryDc,RidOfNonDcMachineAcco" +
                            "unt,MessageOne)\'");
                    temp10 = this.INrpcServerAdapterInstance.NetrLogonComputeServerDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.RidType.RidOfNonDcMachineAccount, ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103757");
                    this.Manager.Checkpoint("MS-NRPC_R103743");
                    this.Manager.Comment("reaching state \'S1380\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeServerDigest/ERROR_NO_SUCH_USER\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SUCH_USER, temp10, "return of NetrLogonComputeServerDigest, state S1380");
                    this.Manager.Comment("reaching state \'S1446\'");
                    this.Manager.Comment("executing step \'call ChangeNonDCMachineAccountStatus(True)\'");
                    this.INrpcServerSutControlAdapterInstance.ChangeNonDCMachineAccountStatus(true);
                    this.Manager.Comment("reaching state \'S1464\'");
                    this.Manager.Comment("checking step \'return ChangeNonDCMachineAccountStatus\'");
                    Test_AuthenticateMessageS1217();
                    goto label2;
                }
                if ((temp12 == 1)) {
                    this.Manager.Comment("reaching state \'S469\'");
                    this.Manager.Comment("executing step \'call ChangeNonDCMachineAccountStatus(False)\'");
                    this.INrpcServerSutControlAdapterInstance.ChangeNonDCMachineAccountStatus(false);
                    this.Manager.Comment("reaching state \'S817\'");
                    this.Manager.Comment("checking step \'return ChangeNonDCMachineAccountStatus\'");
                    this.Manager.Comment("reaching state \'S1165\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp11;
                    this.Manager.Comment("executing step \'call NetrLogonComputeServerDigest(PrimaryDc,RidOfNonDcMachineAcco" +
                            "unt,MessageOne)\'");
                    temp11 = this.INrpcServerAdapterInstance.NetrLogonComputeServerDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.RidType.RidOfNonDcMachineAccount, ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103757");
                    this.Manager.Checkpoint("MS-NRPC_R103743");
                    this.Manager.Comment("reaching state \'S1381\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeServerDigest/ERROR_NO_SUCH_USER\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SUCH_USER, temp11, "return of NetrLogonComputeServerDigest, state S1381");
                    this.Manager.Comment("reaching state \'S1447\'");
                    this.Manager.Comment("executing step \'call ChangeNonDCMachineAccountStatus(True)\'");
                    this.INrpcServerSutControlAdapterInstance.ChangeNonDCMachineAccountStatus(true);
                    this.Manager.Comment("reaching state \'S1465\'");
                    this.Manager.Comment("checking step \'return ChangeNonDCMachineAccountStatus\'");
                    Test_AuthenticateMessageS1216();
                    goto label2;
                }
                throw new InvalidOperationException("never reached");
            label2:
;
                goto label3;
            }
            throw new InvalidOperationException("never reached");
        label3:
;
            this.Manager.EndTest();
        }
        
        private void Test_AuthenticateMessageS0GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S1");
        }
        
        private void Test_AuthenticateMessageS0GetClientAccountTypeChecker(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S290");
        }
        
        private void Test_AuthenticateMessageS1285() {
            this.Manager.Comment("reaching state \'S1285\'");
        }
        
        private void Test_AuthenticateMessageS0GetClientAccountTypeChecker1(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S290");
        }
        
        private void Test_AuthenticateMessageS1284() {
            this.Manager.Comment("reaching state \'S1284\'");
        }
        
        private void Test_AuthenticateMessageS0GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S1");
        }
        
        private void Test_AuthenticateMessageS0GetClientAccountTypeChecker2(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S291");
        }
        
        private void Test_AuthenticateMessageS1245() {
            this.Manager.Comment("reaching state \'S1245\'");
        }
        
        private void Test_AuthenticateMessageS0GetClientAccountTypeChecker3(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S291");
        }
        
        private void Test_AuthenticateMessageS1244() {
            this.Manager.Comment("reaching state \'S1244\'");
        }
        
        private void Test_AuthenticateMessageS0GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S1");
        }
        
        private void Test_AuthenticateMessageS0GetClientAccountTypeChecker4(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S292");
        }
        
        private void Test_AuthenticateMessageS1217() {
            this.Manager.Comment("reaching state \'S1217\'");
        }
        
        private void Test_AuthenticateMessageS0GetClientAccountTypeChecker5(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S292");
        }
        
        private void Test_AuthenticateMessageS1216() {
            this.Manager.Comment("reaching state \'S1216\'");
        }
        #endregion
        
        #region Test Starting in S10
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_AuthenticateMessageS10() {
            this.Manager.BeginTest("Test_AuthenticateMessageS10");
            this.Manager.Comment("reaching state \'S10\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp14;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp14);
            this.Manager.AddReturn(GetPlatformInfo, null, temp14);
            this.Manager.Comment("reaching state \'S11\'");
            int temp27 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_AuthenticateMessageS10GetPlatformChecker)), new ExpectedReturn(Test_AuthenticateMessage.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_AuthenticateMessageS10GetPlatformChecker1)), new ExpectedReturn(Test_AuthenticateMessage.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_AuthenticateMessageS10GetPlatformChecker2)));
            if ((temp27 == 0)) {
                this.Manager.Comment("reaching state \'S131\'");
                bool temp15;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp15);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp15);
                this.Manager.Comment("reaching state \'S305\'");
                int temp18 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS10GetClientAccountTypeChecker)), new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS10GetClientAccountTypeChecker1)));
                if ((temp18 == 0)) {
                    this.Manager.Comment("reaching state \'S494\'");
                    this.Manager.Comment("executing step \'call SwitchUserAccount(False)\'");
                    this.INrpcServerAdapterInstance.SwitchUserAccount(false);
                    this.Manager.Comment("reaching state \'S842\'");
                    this.Manager.Comment("checking step \'return SwitchUserAccount\'");
                    this.Manager.Comment("reaching state \'S1190\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp16;
                    this.Manager.Comment("executing step \'call NetrLogonComputeClientDigest(PrimaryDc,NetBiosFormatDomainNa" +
                            "me,MessageOne)\'");
                    temp16 = this.INrpcServerAdapterInstance.NetrLogonComputeClientDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.NetBiosFormatDomainName, ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103791");
                    this.Manager.Checkpoint("MS-NRPC_R103790");
                    this.Manager.Comment("reaching state \'S1398\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeClientDigest/ERROR_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_ACCESS_DENIED, temp16, "return of NetrLogonComputeClientDigest, state S1398");
                    Test_AuthenticateMessageS1448();
                    goto label4;
                }
                if ((temp18 == 1)) {
                    this.Manager.Comment("reaching state \'S495\'");
                    this.Manager.Comment("executing step \'call SwitchUserAccount(False)\'");
                    this.INrpcServerAdapterInstance.SwitchUserAccount(false);
                    this.Manager.Comment("reaching state \'S843\'");
                    this.Manager.Comment("checking step \'return SwitchUserAccount\'");
                    this.Manager.Comment("reaching state \'S1191\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp17;
                    this.Manager.Comment("executing step \'call NetrLogonComputeClientDigest(PrimaryDc,NetBiosFormatDomainNa" +
                            "me,MessageOne)\'");
                    temp17 = this.INrpcServerAdapterInstance.NetrLogonComputeClientDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.NetBiosFormatDomainName, ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103791");
                    this.Manager.Checkpoint("MS-NRPC_R103790");
                    this.Manager.Comment("reaching state \'S1399\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeClientDigest/ERROR_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_ACCESS_DENIED, temp17, "return of NetrLogonComputeClientDigest, state S1399");
                    Test_AuthenticateMessageS1449();
                    goto label4;
                }
                throw new InvalidOperationException("never reached");
            label4:
;
                goto label7;
            }
            if ((temp27 == 1)) {
                this.Manager.Comment("reaching state \'S132\'");
                bool temp19;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp19);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp19);
                this.Manager.Comment("reaching state \'S306\'");
                int temp22 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS10GetClientAccountTypeChecker2)), new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS10GetClientAccountTypeChecker3)));
                if ((temp22 == 0)) {
                    this.Manager.Comment("reaching state \'S496\'");
                    this.Manager.Comment("executing step \'call SwitchUserAccount(False)\'");
                    this.INrpcServerAdapterInstance.SwitchUserAccount(false);
                    this.Manager.Comment("reaching state \'S844\'");
                    this.Manager.Comment("checking step \'return SwitchUserAccount\'");
                    this.Manager.Comment("reaching state \'S1192\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp20;
                    this.Manager.Comment("executing step \'call NetrLogonComputeClientDigest(PrimaryDc,NetBiosFormatDomainNa" +
                            "me,MessageOne)\'");
                    temp20 = this.INrpcServerAdapterInstance.NetrLogonComputeClientDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.NetBiosFormatDomainName, ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103791");
                    this.Manager.Checkpoint("MS-NRPC_R103790");
                    this.Manager.Comment("reaching state \'S1400\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeClientDigest/ERROR_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_ACCESS_DENIED, temp20, "return of NetrLogonComputeClientDigest, state S1400");
                    Test_AuthenticateMessageS1450();
                    goto label5;
                }
                if ((temp22 == 1)) {
                    this.Manager.Comment("reaching state \'S497\'");
                    this.Manager.Comment("executing step \'call SwitchUserAccount(False)\'");
                    this.INrpcServerAdapterInstance.SwitchUserAccount(false);
                    this.Manager.Comment("reaching state \'S845\'");
                    this.Manager.Comment("checking step \'return SwitchUserAccount\'");
                    this.Manager.Comment("reaching state \'S1193\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp21;
                    this.Manager.Comment("executing step \'call NetrLogonComputeClientDigest(PrimaryDc,NetBiosFormatDomainNa" +
                            "me,MessageOne)\'");
                    temp21 = this.INrpcServerAdapterInstance.NetrLogonComputeClientDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.NetBiosFormatDomainName, ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103791");
                    this.Manager.Checkpoint("MS-NRPC_R103790");
                    this.Manager.Comment("reaching state \'S1401\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeClientDigest/ERROR_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_ACCESS_DENIED, temp21, "return of NetrLogonComputeClientDigest, state S1401");
                    Test_AuthenticateMessageS1451();
                    goto label5;
                }
                throw new InvalidOperationException("never reached");
            label5:
;
                goto label7;
            }
            if ((temp27 == 2)) {
                this.Manager.Comment("reaching state \'S133\'");
                bool temp23;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp23);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp23);
                this.Manager.Comment("reaching state \'S307\'");
                int temp26 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS10GetClientAccountTypeChecker4)), new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS10GetClientAccountTypeChecker5)));
                if ((temp26 == 0)) {
                    this.Manager.Comment("reaching state \'S498\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp24;
                    this.Manager.Comment("executing step \'call NetrLogonComputeServerDigest(PrimaryDc,RidOfNonMachineAccoun" +
                            "t,MessageOne)\'");
                    temp24 = this.INrpcServerAdapterInstance.NetrLogonComputeServerDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.RidType)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103756");
                    this.Manager.Checkpoint("MS-NRPC_R103743");
                    this.Manager.Comment("reaching state \'S846\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeServerDigest/ERROR_NO_SUCH_USER\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SUCH_USER, temp24, "return of NetrLogonComputeServerDigest, state S846");
                    this.Manager.Comment("reaching state \'S1194\'");
                    goto label6;
                }
                if ((temp26 == 1)) {
                    this.Manager.Comment("reaching state \'S499\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp25;
                    this.Manager.Comment("executing step \'call NetrLogonComputeServerDigest(PrimaryDc,RidOfNonMachineAccoun" +
                            "t,MessageOne)\'");
                    temp25 = this.INrpcServerAdapterInstance.NetrLogonComputeServerDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.RidType)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103756");
                    this.Manager.Checkpoint("MS-NRPC_R103743");
                    this.Manager.Comment("reaching state \'S847\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeServerDigest/ERROR_NO_SUCH_USER\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SUCH_USER, temp25, "return of NetrLogonComputeServerDigest, state S847");
                    this.Manager.Comment("reaching state \'S1195\'");
                    goto label6;
                }
                throw new InvalidOperationException("never reached");
            label6:
;
                goto label7;
            }
            throw new InvalidOperationException("never reached");
        label7:
;
            this.Manager.EndTest();
        }
        
        private void Test_AuthenticateMessageS10GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S11");
        }
        
        private void Test_AuthenticateMessageS10GetClientAccountTypeChecker(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S305");
        }
        
        private void Test_AuthenticateMessageS1448() {
            this.Manager.Comment("reaching state \'S1448\'");
            this.Manager.Comment("executing step \'call SwitchUserAccount(True)\'");
            this.INrpcServerAdapterInstance.SwitchUserAccount(true);
            this.Manager.Comment("reaching state \'S1466\'");
            this.Manager.Comment("checking step \'return SwitchUserAccount\'");
            Test_AuthenticateMessageS1216();
        }
        
        private void Test_AuthenticateMessageS10GetClientAccountTypeChecker1(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S305");
        }
        
        private void Test_AuthenticateMessageS1449() {
            this.Manager.Comment("reaching state \'S1449\'");
            this.Manager.Comment("executing step \'call SwitchUserAccount(True)\'");
            this.INrpcServerAdapterInstance.SwitchUserAccount(true);
            this.Manager.Comment("reaching state \'S1467\'");
            this.Manager.Comment("checking step \'return SwitchUserAccount\'");
            Test_AuthenticateMessageS1217();
        }
        
        private void Test_AuthenticateMessageS10GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S11");
        }
        
        private void Test_AuthenticateMessageS10GetClientAccountTypeChecker2(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S306");
        }
        
        private void Test_AuthenticateMessageS1450() {
            this.Manager.Comment("reaching state \'S1450\'");
            this.Manager.Comment("executing step \'call SwitchUserAccount(True)\'");
            this.INrpcServerAdapterInstance.SwitchUserAccount(true);
            this.Manager.Comment("reaching state \'S1468\'");
            this.Manager.Comment("checking step \'return SwitchUserAccount\'");
            Test_AuthenticateMessageS1244();
        }
        
        private void Test_AuthenticateMessageS10GetClientAccountTypeChecker3(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S306");
        }
        
        private void Test_AuthenticateMessageS1451() {
            this.Manager.Comment("reaching state \'S1451\'");
            this.Manager.Comment("executing step \'call SwitchUserAccount(True)\'");
            this.INrpcServerAdapterInstance.SwitchUserAccount(true);
            this.Manager.Comment("reaching state \'S1469\'");
            this.Manager.Comment("checking step \'return SwitchUserAccount\'");
            Test_AuthenticateMessageS1245();
        }
        
        private void Test_AuthenticateMessageS10GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S11");
        }
        
        private void Test_AuthenticateMessageS10GetClientAccountTypeChecker4(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S307");
        }
        
        private void Test_AuthenticateMessageS10GetClientAccountTypeChecker5(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S307");
        }
        #endregion
        
        #region Test Starting in S100
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_AuthenticateMessageS100() {
            this.Manager.BeginTest("Test_AuthenticateMessageS100");
            this.Manager.Comment("reaching state \'S100\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp28;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp28);
            this.Manager.AddReturn(GetPlatformInfo, null, temp28);
            this.Manager.Comment("reaching state \'S101\'");
            int temp41 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_AuthenticateMessageS100GetPlatformChecker)), new ExpectedReturn(Test_AuthenticateMessage.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_AuthenticateMessageS100GetPlatformChecker1)), new ExpectedReturn(Test_AuthenticateMessage.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_AuthenticateMessageS100GetPlatformChecker2)));
            if ((temp41 == 0)) {
                this.Manager.Comment("reaching state \'S266\'");
                bool temp29;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp29);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp29);
                this.Manager.Comment("reaching state \'S440\'");
                int temp32 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS100GetClientAccountTypeChecker)), new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS100GetClientAccountTypeChecker1)));
                if ((temp32 == 0)) {
                    this.Manager.Comment("reaching state \'S764\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp30;
                    this.Manager.Comment("executing step \'call NetrLogonSetServiceBits(PrimaryDc,1,64)\'");
                    temp30 = this.INrpcServerAdapterInstance.NetrLogonSetServiceBits(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 1u, 64u);
                    this.Manager.Comment("reaching state \'S1112\'");
                    this.Manager.Comment("checking step \'return NetrLogonSetServiceBits/ERROR_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_ACCESS_DENIED, temp30, "return of NetrLogonSetServiceBits, state S1112");
                    Test_AuthenticateMessageS1216();
                    goto label8;
                }
                if ((temp32 == 1)) {
                    this.Manager.Comment("reaching state \'S765\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp31;
                    this.Manager.Comment("executing step \'call NetrLogonSetServiceBits(PrimaryDc,512,512)\'");
                    temp31 = this.INrpcServerAdapterInstance.NetrLogonSetServiceBits(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 512u, 512u);
                    this.Manager.Comment("reaching state \'S1113\'");
                    this.Manager.Comment("checking step \'return NetrLogonSetServiceBits/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp31, "return of NetrLogonSetServiceBits, state S1113");
                    Test_AuthenticateMessageS1217();
                    goto label8;
                }
                throw new InvalidOperationException("never reached");
            label8:
;
                goto label11;
            }
            if ((temp41 == 1)) {
                this.Manager.Comment("reaching state \'S267\'");
                bool temp33;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp33);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp33);
                this.Manager.Comment("reaching state \'S441\'");
                int temp36 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS100GetClientAccountTypeChecker2)), new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS100GetClientAccountTypeChecker3)));
                if ((temp36 == 0)) {
                    this.Manager.Comment("reaching state \'S766\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp34;
                    this.Manager.Comment("executing step \'call NetrLogonComputeServerDigest(PrimaryDc,RidOfNonMachineAccoun" +
                            "t,MessageOne)\'");
                    temp34 = this.INrpcServerAdapterInstance.NetrLogonComputeServerDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.RidType)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103756");
                    this.Manager.Checkpoint("MS-NRPC_R103743");
                    this.Manager.Comment("reaching state \'S1114\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeServerDigest/ERROR_NO_SUCH_USER\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SUCH_USER, temp34, "return of NetrLogonComputeServerDigest, state S1114");
                    this.Manager.Comment("reaching state \'S1344\'");
                    goto label9;
                }
                if ((temp36 == 1)) {
                    this.Manager.Comment("reaching state \'S767\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp35;
                    this.Manager.Comment("executing step \'call NetrLogonComputeServerDigest(PrimaryDc,RidOfNonMachineAccoun" +
                            "t,MessageOne)\'");
                    temp35 = this.INrpcServerAdapterInstance.NetrLogonComputeServerDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.RidType)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103756");
                    this.Manager.Checkpoint("MS-NRPC_R103743");
                    this.Manager.Comment("reaching state \'S1115\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeServerDigest/ERROR_NO_SUCH_USER\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SUCH_USER, temp35, "return of NetrLogonComputeServerDigest, state S1115");
                    this.Manager.Comment("reaching state \'S1345\'");
                    goto label9;
                }
                throw new InvalidOperationException("never reached");
            label9:
;
                goto label11;
            }
            if ((temp41 == 2)) {
                this.Manager.Comment("reaching state \'S268\'");
                bool temp37;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp37);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp37);
                this.Manager.Comment("reaching state \'S442\'");
                int temp40 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS100GetClientAccountTypeChecker4)), new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS100GetClientAccountTypeChecker5)));
                if ((temp40 == 0)) {
                    this.Manager.Comment("reaching state \'S768\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp38;
                    this.Manager.Comment("executing step \'call NetrLogonComputeServerDigest(PrimaryDc,RidOfNonMachineAccoun" +
                            "t,MessageOne)\'");
                    temp38 = this.INrpcServerAdapterInstance.NetrLogonComputeServerDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.RidType)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103756");
                    this.Manager.Checkpoint("MS-NRPC_R103743");
                    this.Manager.Comment("reaching state \'S1116\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeServerDigest/ERROR_NO_SUCH_USER\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SUCH_USER, temp38, "return of NetrLogonComputeServerDigest, state S1116");
                    this.Manager.Comment("reaching state \'S1346\'");
                    goto label10;
                }
                if ((temp40 == 1)) {
                    this.Manager.Comment("reaching state \'S769\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp39;
                    this.Manager.Comment("executing step \'call NetrLogonComputeServerDigest(PrimaryDc,RidOfNonMachineAccoun" +
                            "t,MessageOne)\'");
                    temp39 = this.INrpcServerAdapterInstance.NetrLogonComputeServerDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.RidType)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103756");
                    this.Manager.Checkpoint("MS-NRPC_R103743");
                    this.Manager.Comment("reaching state \'S1117\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeServerDigest/ERROR_NO_SUCH_USER\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SUCH_USER, temp39, "return of NetrLogonComputeServerDigest, state S1117");
                    this.Manager.Comment("reaching state \'S1347\'");
                    goto label10;
                }
                throw new InvalidOperationException("never reached");
            label10:
;
                goto label11;
            }
            throw new InvalidOperationException("never reached");
        label11:
;
            this.Manager.EndTest();
        }
        
        private void Test_AuthenticateMessageS100GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S101");
        }
        
        private void Test_AuthenticateMessageS100GetClientAccountTypeChecker(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S440");
        }
        
        private void Test_AuthenticateMessageS100GetClientAccountTypeChecker1(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S440");
        }
        
        private void Test_AuthenticateMessageS100GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S101");
        }
        
        private void Test_AuthenticateMessageS100GetClientAccountTypeChecker2(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S441");
        }
        
        private void Test_AuthenticateMessageS100GetClientAccountTypeChecker3(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S441");
        }
        
        private void Test_AuthenticateMessageS100GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S101");
        }
        
        private void Test_AuthenticateMessageS100GetClientAccountTypeChecker4(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S442");
        }
        
        private void Test_AuthenticateMessageS100GetClientAccountTypeChecker5(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S442");
        }
        #endregion
        
        #region Test Starting in S102
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_AuthenticateMessageS102() {
            this.Manager.BeginTest("Test_AuthenticateMessageS102");
            this.Manager.Comment("reaching state \'S102\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp42;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp42);
            this.Manager.AddReturn(GetPlatformInfo, null, temp42);
            this.Manager.Comment("reaching state \'S103\'");
            int temp55 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_AuthenticateMessageS102GetPlatformChecker)), new ExpectedReturn(Test_AuthenticateMessage.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_AuthenticateMessageS102GetPlatformChecker1)), new ExpectedReturn(Test_AuthenticateMessage.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_AuthenticateMessageS102GetPlatformChecker2)));
            if ((temp55 == 0)) {
                this.Manager.Comment("reaching state \'S269\'");
                bool temp43;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp43);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp43);
                this.Manager.Comment("reaching state \'S443\'");
                int temp46 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS102GetClientAccountTypeChecker)), new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS102GetClientAccountTypeChecker1)));
                if ((temp46 == 0)) {
                    this.Manager.Comment("reaching state \'S770\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp44;
                    this.Manager.Comment("executing step \'call NetrLogonSetServiceBits(PrimaryDc,64,512)\'");
                    temp44 = this.INrpcServerAdapterInstance.NetrLogonSetServiceBits(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 64u, 512u);
                    this.Manager.Comment("reaching state \'S1118\'");
                    this.Manager.Comment("checking step \'return NetrLogonSetServiceBits/ERROR_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_ACCESS_DENIED, temp44, "return of NetrLogonSetServiceBits, state S1118");
                    Test_AuthenticateMessageS1216();
                    goto label12;
                }
                if ((temp46 == 1)) {
                    this.Manager.Comment("reaching state \'S771\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp45;
                    this.Manager.Comment("executing step \'call NetrLogonSetServiceBits(PrimaryDc,512,0)\'");
                    temp45 = this.INrpcServerAdapterInstance.NetrLogonSetServiceBits(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 512u, 0u);
                    this.Manager.Comment("reaching state \'S1119\'");
                    this.Manager.Comment("checking step \'return NetrLogonSetServiceBits/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp45, "return of NetrLogonSetServiceBits, state S1119");
                    Test_AuthenticateMessageS1217();
                    goto label12;
                }
                throw new InvalidOperationException("never reached");
            label12:
;
                goto label15;
            }
            if ((temp55 == 1)) {
                this.Manager.Comment("reaching state \'S270\'");
                bool temp47;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp47);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp47);
                this.Manager.Comment("reaching state \'S444\'");
                int temp50 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS102GetClientAccountTypeChecker2)), new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS102GetClientAccountTypeChecker3)));
                if ((temp50 == 0)) {
                    this.Manager.Comment("reaching state \'S772\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp48;
                    this.Manager.Comment("executing step \'call NetrLogonComputeServerDigest(PrimaryDc,RidOfNonMachineAccoun" +
                            "t,MessageOne)\'");
                    temp48 = this.INrpcServerAdapterInstance.NetrLogonComputeServerDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.RidType)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103756");
                    this.Manager.Checkpoint("MS-NRPC_R103743");
                    this.Manager.Comment("reaching state \'S1120\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeServerDigest/ERROR_NO_SUCH_USER\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SUCH_USER, temp48, "return of NetrLogonComputeServerDigest, state S1120");
                    this.Manager.Comment("reaching state \'S1348\'");
                    goto label13;
                }
                if ((temp50 == 1)) {
                    this.Manager.Comment("reaching state \'S773\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp49;
                    this.Manager.Comment("executing step \'call NetrLogonComputeServerDigest(PrimaryDc,RidOfNonMachineAccoun" +
                            "t,MessageOne)\'");
                    temp49 = this.INrpcServerAdapterInstance.NetrLogonComputeServerDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.RidType)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103756");
                    this.Manager.Checkpoint("MS-NRPC_R103743");
                    this.Manager.Comment("reaching state \'S1121\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeServerDigest/ERROR_NO_SUCH_USER\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SUCH_USER, temp49, "return of NetrLogonComputeServerDigest, state S1121");
                    this.Manager.Comment("reaching state \'S1349\'");
                    goto label13;
                }
                throw new InvalidOperationException("never reached");
            label13:
;
                goto label15;
            }
            if ((temp55 == 2)) {
                this.Manager.Comment("reaching state \'S271\'");
                bool temp51;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp51);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp51);
                this.Manager.Comment("reaching state \'S445\'");
                int temp54 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS102GetClientAccountTypeChecker4)), new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS102GetClientAccountTypeChecker5)));
                if ((temp54 == 0)) {
                    this.Manager.Comment("reaching state \'S774\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp52;
                    this.Manager.Comment("executing step \'call NetrLogonComputeServerDigest(PrimaryDc,RidOfNonMachineAccoun" +
                            "t,MessageOne)\'");
                    temp52 = this.INrpcServerAdapterInstance.NetrLogonComputeServerDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.RidType)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103756");
                    this.Manager.Checkpoint("MS-NRPC_R103743");
                    this.Manager.Comment("reaching state \'S1122\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeServerDigest/ERROR_NO_SUCH_USER\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SUCH_USER, temp52, "return of NetrLogonComputeServerDigest, state S1122");
                    this.Manager.Comment("reaching state \'S1350\'");
                    goto label14;
                }
                if ((temp54 == 1)) {
                    this.Manager.Comment("reaching state \'S775\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp53;
                    this.Manager.Comment("executing step \'call NetrLogonComputeServerDigest(PrimaryDc,RidOfNonMachineAccoun" +
                            "t,MessageOne)\'");
                    temp53 = this.INrpcServerAdapterInstance.NetrLogonComputeServerDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.RidType)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103756");
                    this.Manager.Checkpoint("MS-NRPC_R103743");
                    this.Manager.Comment("reaching state \'S1123\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeServerDigest/ERROR_NO_SUCH_USER\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SUCH_USER, temp53, "return of NetrLogonComputeServerDigest, state S1123");
                    this.Manager.Comment("reaching state \'S1351\'");
                    goto label14;
                }
                throw new InvalidOperationException("never reached");
            label14:
;
                goto label15;
            }
            throw new InvalidOperationException("never reached");
        label15:
;
            this.Manager.EndTest();
        }
        
        private void Test_AuthenticateMessageS102GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S103");
        }
        
        private void Test_AuthenticateMessageS102GetClientAccountTypeChecker(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S443");
        }
        
        private void Test_AuthenticateMessageS102GetClientAccountTypeChecker1(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S443");
        }
        
        private void Test_AuthenticateMessageS102GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S103");
        }
        
        private void Test_AuthenticateMessageS102GetClientAccountTypeChecker2(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S444");
        }
        
        private void Test_AuthenticateMessageS102GetClientAccountTypeChecker3(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S444");
        }
        
        private void Test_AuthenticateMessageS102GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S103");
        }
        
        private void Test_AuthenticateMessageS102GetClientAccountTypeChecker4(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S445");
        }
        
        private void Test_AuthenticateMessageS102GetClientAccountTypeChecker5(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S445");
        }
        #endregion
        
        #region Test Starting in S104
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_AuthenticateMessageS104() {
            this.Manager.BeginTest("Test_AuthenticateMessageS104");
            this.Manager.Comment("reaching state \'S104\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp56;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp56);
            this.Manager.AddReturn(GetPlatformInfo, null, temp56);
            this.Manager.Comment("reaching state \'S105\'");
            int temp69 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_AuthenticateMessageS104GetPlatformChecker)), new ExpectedReturn(Test_AuthenticateMessage.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_AuthenticateMessageS104GetPlatformChecker1)), new ExpectedReturn(Test_AuthenticateMessage.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_AuthenticateMessageS104GetPlatformChecker2)));
            if ((temp69 == 0)) {
                this.Manager.Comment("reaching state \'S272\'");
                bool temp57;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp57);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp57);
                this.Manager.Comment("reaching state \'S446\'");
                int temp60 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS104GetClientAccountTypeChecker)), new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS104GetClientAccountTypeChecker1)));
                if ((temp60 == 0)) {
                    this.Manager.Comment("reaching state \'S776\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp58;
                    this.Manager.Comment("executing step \'call NetrLogonSetServiceBits(PrimaryDc,512,512)\'");
                    temp58 = this.INrpcServerAdapterInstance.NetrLogonSetServiceBits(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 512u, 512u);
                    this.Manager.Comment("reaching state \'S1124\'");
                    this.Manager.Comment("checking step \'return NetrLogonSetServiceBits/ERROR_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_ACCESS_DENIED, temp58, "return of NetrLogonSetServiceBits, state S1124");
                    Test_AuthenticateMessageS1216();
                    goto label16;
                }
                if ((temp60 == 1)) {
                    this.Manager.Comment("reaching state \'S777\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp59;
                    this.Manager.Comment("executing step \'call NetrLogonSetServiceBits(PrimaryDc,8192,512)\'");
                    temp59 = this.INrpcServerAdapterInstance.NetrLogonSetServiceBits(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 8192u, 512u);
                    this.Manager.Comment("reaching state \'S1125\'");
                    this.Manager.Comment("checking step \'return NetrLogonSetServiceBits/STATUS_INVALID_PARAMETER\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_INVALID_PARAMETER, temp59, "return of NetrLogonSetServiceBits, state S1125");
                    Test_AuthenticateMessageS1217();
                    goto label16;
                }
                throw new InvalidOperationException("never reached");
            label16:
;
                goto label19;
            }
            if ((temp69 == 1)) {
                this.Manager.Comment("reaching state \'S273\'");
                bool temp61;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp61);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp61);
                this.Manager.Comment("reaching state \'S447\'");
                int temp64 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS104GetClientAccountTypeChecker2)), new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS104GetClientAccountTypeChecker3)));
                if ((temp64 == 0)) {
                    this.Manager.Comment("reaching state \'S778\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp62;
                    this.Manager.Comment("executing step \'call NetrLogonComputeServerDigest(PrimaryDc,RidOfNonMachineAccoun" +
                            "t,MessageOne)\'");
                    temp62 = this.INrpcServerAdapterInstance.NetrLogonComputeServerDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.RidType)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103756");
                    this.Manager.Checkpoint("MS-NRPC_R103743");
                    this.Manager.Comment("reaching state \'S1126\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeServerDigest/ERROR_NO_SUCH_USER\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SUCH_USER, temp62, "return of NetrLogonComputeServerDigest, state S1126");
                    this.Manager.Comment("reaching state \'S1352\'");
                    goto label17;
                }
                if ((temp64 == 1)) {
                    this.Manager.Comment("reaching state \'S779\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp63;
                    this.Manager.Comment("executing step \'call NetrLogonComputeServerDigest(PrimaryDc,RidOfNonMachineAccoun" +
                            "t,MessageOne)\'");
                    temp63 = this.INrpcServerAdapterInstance.NetrLogonComputeServerDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.RidType)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103756");
                    this.Manager.Checkpoint("MS-NRPC_R103743");
                    this.Manager.Comment("reaching state \'S1127\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeServerDigest/ERROR_NO_SUCH_USER\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SUCH_USER, temp63, "return of NetrLogonComputeServerDigest, state S1127");
                    this.Manager.Comment("reaching state \'S1353\'");
                    goto label17;
                }
                throw new InvalidOperationException("never reached");
            label17:
;
                goto label19;
            }
            if ((temp69 == 2)) {
                this.Manager.Comment("reaching state \'S274\'");
                bool temp65;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp65);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp65);
                this.Manager.Comment("reaching state \'S448\'");
                int temp68 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS104GetClientAccountTypeChecker4)), new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS104GetClientAccountTypeChecker5)));
                if ((temp68 == 0)) {
                    this.Manager.Comment("reaching state \'S780\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp66;
                    this.Manager.Comment("executing step \'call NetrLogonComputeServerDigest(PrimaryDc,RidOfNonMachineAccoun" +
                            "t,MessageOne)\'");
                    temp66 = this.INrpcServerAdapterInstance.NetrLogonComputeServerDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.RidType)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103756");
                    this.Manager.Checkpoint("MS-NRPC_R103743");
                    this.Manager.Comment("reaching state \'S1128\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeServerDigest/ERROR_NO_SUCH_USER\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SUCH_USER, temp66, "return of NetrLogonComputeServerDigest, state S1128");
                    this.Manager.Comment("reaching state \'S1354\'");
                    goto label18;
                }
                if ((temp68 == 1)) {
                    this.Manager.Comment("reaching state \'S781\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp67;
                    this.Manager.Comment("executing step \'call NetrLogonComputeServerDigest(PrimaryDc,RidOfNonMachineAccoun" +
                            "t,MessageOne)\'");
                    temp67 = this.INrpcServerAdapterInstance.NetrLogonComputeServerDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.RidType)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103756");
                    this.Manager.Checkpoint("MS-NRPC_R103743");
                    this.Manager.Comment("reaching state \'S1129\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeServerDigest/ERROR_NO_SUCH_USER\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SUCH_USER, temp67, "return of NetrLogonComputeServerDigest, state S1129");
                    this.Manager.Comment("reaching state \'S1355\'");
                    goto label18;
                }
                throw new InvalidOperationException("never reached");
            label18:
;
                goto label19;
            }
            throw new InvalidOperationException("never reached");
        label19:
;
            this.Manager.EndTest();
        }
        
        private void Test_AuthenticateMessageS104GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S105");
        }
        
        private void Test_AuthenticateMessageS104GetClientAccountTypeChecker(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S446");
        }
        
        private void Test_AuthenticateMessageS104GetClientAccountTypeChecker1(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S446");
        }
        
        private void Test_AuthenticateMessageS104GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S105");
        }
        
        private void Test_AuthenticateMessageS104GetClientAccountTypeChecker2(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S447");
        }
        
        private void Test_AuthenticateMessageS104GetClientAccountTypeChecker3(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S447");
        }
        
        private void Test_AuthenticateMessageS104GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S105");
        }
        
        private void Test_AuthenticateMessageS104GetClientAccountTypeChecker4(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S448");
        }
        
        private void Test_AuthenticateMessageS104GetClientAccountTypeChecker5(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S448");
        }
        #endregion
        
        #region Test Starting in S106
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_AuthenticateMessageS106() {
            this.Manager.BeginTest("Test_AuthenticateMessageS106");
            this.Manager.Comment("reaching state \'S106\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp70;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp70);
            this.Manager.AddReturn(GetPlatformInfo, null, temp70);
            this.Manager.Comment("reaching state \'S107\'");
            int temp83 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_AuthenticateMessageS106GetPlatformChecker)), new ExpectedReturn(Test_AuthenticateMessage.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_AuthenticateMessageS106GetPlatformChecker1)), new ExpectedReturn(Test_AuthenticateMessage.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_AuthenticateMessageS106GetPlatformChecker2)));
            if ((temp83 == 0)) {
                this.Manager.Comment("reaching state \'S275\'");
                bool temp71;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp71);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp71);
                this.Manager.Comment("reaching state \'S449\'");
                int temp74 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS106GetClientAccountTypeChecker)), new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS106GetClientAccountTypeChecker1)));
                if ((temp74 == 0)) {
                    this.Manager.Comment("reaching state \'S782\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp72;
                    this.Manager.Comment("executing step \'call NetrLogonSetServiceBits(PrimaryDc,8192,64)\'");
                    temp72 = this.INrpcServerAdapterInstance.NetrLogonSetServiceBits(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 8192u, 64u);
                    this.Manager.Comment("reaching state \'S1130\'");
                    this.Manager.Comment("checking step \'return NetrLogonSetServiceBits/ERROR_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_ACCESS_DENIED, temp72, "return of NetrLogonSetServiceBits, state S1130");
                    Test_AuthenticateMessageS1216();
                    goto label20;
                }
                if ((temp74 == 1)) {
                    this.Manager.Comment("reaching state \'S783\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp73;
                    this.Manager.Comment("executing step \'call NetrLogonSetServiceBits(PrimaryDc,64,0)\'");
                    temp73 = this.INrpcServerAdapterInstance.NetrLogonSetServiceBits(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 64u, 0u);
                    this.Manager.Comment("reaching state \'S1131\'");
                    this.Manager.Comment("checking step \'return NetrLogonSetServiceBits/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp73, "return of NetrLogonSetServiceBits, state S1131");
                    Test_AuthenticateMessageS1217();
                    goto label20;
                }
                throw new InvalidOperationException("never reached");
            label20:
;
                goto label23;
            }
            if ((temp83 == 1)) {
                this.Manager.Comment("reaching state \'S276\'");
                bool temp75;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp75);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp75);
                this.Manager.Comment("reaching state \'S450\'");
                int temp78 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS106GetClientAccountTypeChecker2)), new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS106GetClientAccountTypeChecker3)));
                if ((temp78 == 0)) {
                    this.Manager.Comment("reaching state \'S784\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp76;
                    this.Manager.Comment("executing step \'call NetrLogonComputeServerDigest(PrimaryDc,RidOfNonMachineAccoun" +
                            "t,MessageOne)\'");
                    temp76 = this.INrpcServerAdapterInstance.NetrLogonComputeServerDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.RidType)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103756");
                    this.Manager.Checkpoint("MS-NRPC_R103743");
                    this.Manager.Comment("reaching state \'S1132\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeServerDigest/ERROR_NO_SUCH_USER\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SUCH_USER, temp76, "return of NetrLogonComputeServerDigest, state S1132");
                    this.Manager.Comment("reaching state \'S1356\'");
                    goto label21;
                }
                if ((temp78 == 1)) {
                    this.Manager.Comment("reaching state \'S785\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp77;
                    this.Manager.Comment("executing step \'call NetrLogonComputeServerDigest(PrimaryDc,RidOfNonMachineAccoun" +
                            "t,MessageOne)\'");
                    temp77 = this.INrpcServerAdapterInstance.NetrLogonComputeServerDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.RidType)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103756");
                    this.Manager.Checkpoint("MS-NRPC_R103743");
                    this.Manager.Comment("reaching state \'S1133\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeServerDigest/ERROR_NO_SUCH_USER\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SUCH_USER, temp77, "return of NetrLogonComputeServerDigest, state S1133");
                    this.Manager.Comment("reaching state \'S1357\'");
                    goto label21;
                }
                throw new InvalidOperationException("never reached");
            label21:
;
                goto label23;
            }
            if ((temp83 == 2)) {
                this.Manager.Comment("reaching state \'S277\'");
                bool temp79;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp79);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp79);
                this.Manager.Comment("reaching state \'S451\'");
                int temp82 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS106GetClientAccountTypeChecker4)), new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS106GetClientAccountTypeChecker5)));
                if ((temp82 == 0)) {
                    this.Manager.Comment("reaching state \'S786\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp80;
                    this.Manager.Comment("executing step \'call NetrLogonComputeServerDigest(PrimaryDc,RidOfNonMachineAccoun" +
                            "t,MessageOne)\'");
                    temp80 = this.INrpcServerAdapterInstance.NetrLogonComputeServerDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.RidType)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103756");
                    this.Manager.Checkpoint("MS-NRPC_R103743");
                    this.Manager.Comment("reaching state \'S1134\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeServerDigest/ERROR_NO_SUCH_USER\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SUCH_USER, temp80, "return of NetrLogonComputeServerDigest, state S1134");
                    this.Manager.Comment("reaching state \'S1358\'");
                    goto label22;
                }
                if ((temp82 == 1)) {
                    this.Manager.Comment("reaching state \'S787\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp81;
                    this.Manager.Comment("executing step \'call NetrLogonComputeServerDigest(PrimaryDc,RidOfNonMachineAccoun" +
                            "t,MessageOne)\'");
                    temp81 = this.INrpcServerAdapterInstance.NetrLogonComputeServerDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.RidType)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103756");
                    this.Manager.Checkpoint("MS-NRPC_R103743");
                    this.Manager.Comment("reaching state \'S1135\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeServerDigest/ERROR_NO_SUCH_USER\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SUCH_USER, temp81, "return of NetrLogonComputeServerDigest, state S1135");
                    this.Manager.Comment("reaching state \'S1359\'");
                    goto label22;
                }
                throw new InvalidOperationException("never reached");
            label22:
;
                goto label23;
            }
            throw new InvalidOperationException("never reached");
        label23:
;
            this.Manager.EndTest();
        }
        
        private void Test_AuthenticateMessageS106GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S107");
        }
        
        private void Test_AuthenticateMessageS106GetClientAccountTypeChecker(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S449");
        }
        
        private void Test_AuthenticateMessageS106GetClientAccountTypeChecker1(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S449");
        }
        
        private void Test_AuthenticateMessageS106GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S107");
        }
        
        private void Test_AuthenticateMessageS106GetClientAccountTypeChecker2(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S450");
        }
        
        private void Test_AuthenticateMessageS106GetClientAccountTypeChecker3(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S450");
        }
        
        private void Test_AuthenticateMessageS106GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S107");
        }
        
        private void Test_AuthenticateMessageS106GetClientAccountTypeChecker4(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S451");
        }
        
        private void Test_AuthenticateMessageS106GetClientAccountTypeChecker5(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S451");
        }
        #endregion
        
        #region Test Starting in S108
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_AuthenticateMessageS108() {
            this.Manager.BeginTest("Test_AuthenticateMessageS108");
            this.Manager.Comment("reaching state \'S108\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp84;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp84);
            this.Manager.AddReturn(GetPlatformInfo, null, temp84);
            this.Manager.Comment("reaching state \'S109\'");
            int temp97 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_AuthenticateMessageS108GetPlatformChecker)), new ExpectedReturn(Test_AuthenticateMessage.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_AuthenticateMessageS108GetPlatformChecker1)), new ExpectedReturn(Test_AuthenticateMessage.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_AuthenticateMessageS108GetPlatformChecker2)));
            if ((temp97 == 0)) {
                this.Manager.Comment("reaching state \'S278\'");
                bool temp85;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp85);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp85);
                this.Manager.Comment("reaching state \'S452\'");
                int temp88 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS108GetClientAccountTypeChecker)), new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS108GetClientAccountTypeChecker1)));
                if ((temp88 == 0)) {
                    this.Manager.Comment("reaching state \'S788\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp86;
                    this.Manager.Comment("executing step \'call NetrLogonSetServiceBits(PrimaryDc,64,64)\'");
                    temp86 = this.INrpcServerAdapterInstance.NetrLogonSetServiceBits(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 64u, 64u);
                    this.Manager.Comment("reaching state \'S1136\'");
                    this.Manager.Comment("checking step \'return NetrLogonSetServiceBits/ERROR_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_ACCESS_DENIED, temp86, "return of NetrLogonSetServiceBits, state S1136");
                    Test_AuthenticateMessageS1216();
                    goto label24;
                }
                if ((temp88 == 1)) {
                    this.Manager.Comment("reaching state \'S789\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp87;
                    this.Manager.Comment("executing step \'call NetrLogonSetServiceBits(PrimaryDc,64,512)\'");
                    temp87 = this.INrpcServerAdapterInstance.NetrLogonSetServiceBits(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 64u, 512u);
                    this.Manager.Comment("reaching state \'S1137\'");
                    this.Manager.Comment("checking step \'return NetrLogonSetServiceBits/STATUS_INVALID_PARAMETER\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_INVALID_PARAMETER, temp87, "return of NetrLogonSetServiceBits, state S1137");
                    Test_AuthenticateMessageS1217();
                    goto label24;
                }
                throw new InvalidOperationException("never reached");
            label24:
;
                goto label27;
            }
            if ((temp97 == 1)) {
                this.Manager.Comment("reaching state \'S279\'");
                bool temp89;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp89);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp89);
                this.Manager.Comment("reaching state \'S453\'");
                int temp92 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS108GetClientAccountTypeChecker2)), new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS108GetClientAccountTypeChecker3)));
                if ((temp92 == 0)) {
                    this.Manager.Comment("reaching state \'S790\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp90;
                    this.Manager.Comment("executing step \'call NetrLogonComputeServerDigest(PrimaryDc,RidOfNonMachineAccoun" +
                            "t,MessageOne)\'");
                    temp90 = this.INrpcServerAdapterInstance.NetrLogonComputeServerDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.RidType)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103756");
                    this.Manager.Checkpoint("MS-NRPC_R103743");
                    this.Manager.Comment("reaching state \'S1138\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeServerDigest/ERROR_NO_SUCH_USER\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SUCH_USER, temp90, "return of NetrLogonComputeServerDigest, state S1138");
                    this.Manager.Comment("reaching state \'S1360\'");
                    goto label25;
                }
                if ((temp92 == 1)) {
                    this.Manager.Comment("reaching state \'S791\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp91;
                    this.Manager.Comment("executing step \'call NetrLogonComputeServerDigest(PrimaryDc,RidOfNonMachineAccoun" +
                            "t,MessageOne)\'");
                    temp91 = this.INrpcServerAdapterInstance.NetrLogonComputeServerDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.RidType)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103756");
                    this.Manager.Checkpoint("MS-NRPC_R103743");
                    this.Manager.Comment("reaching state \'S1139\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeServerDigest/ERROR_NO_SUCH_USER\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SUCH_USER, temp91, "return of NetrLogonComputeServerDigest, state S1139");
                    this.Manager.Comment("reaching state \'S1361\'");
                    goto label25;
                }
                throw new InvalidOperationException("never reached");
            label25:
;
                goto label27;
            }
            if ((temp97 == 2)) {
                this.Manager.Comment("reaching state \'S280\'");
                bool temp93;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp93);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp93);
                this.Manager.Comment("reaching state \'S454\'");
                int temp96 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS108GetClientAccountTypeChecker4)), new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS108GetClientAccountTypeChecker5)));
                if ((temp96 == 0)) {
                    this.Manager.Comment("reaching state \'S792\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp94;
                    this.Manager.Comment("executing step \'call NetrLogonComputeServerDigest(PrimaryDc,RidOfNonMachineAccoun" +
                            "t,MessageOne)\'");
                    temp94 = this.INrpcServerAdapterInstance.NetrLogonComputeServerDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.RidType)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103756");
                    this.Manager.Checkpoint("MS-NRPC_R103743");
                    this.Manager.Comment("reaching state \'S1140\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeServerDigest/ERROR_NO_SUCH_USER\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SUCH_USER, temp94, "return of NetrLogonComputeServerDigest, state S1140");
                    this.Manager.Comment("reaching state \'S1362\'");
                    goto label26;
                }
                if ((temp96 == 1)) {
                    this.Manager.Comment("reaching state \'S793\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp95;
                    this.Manager.Comment("executing step \'call NetrLogonComputeServerDigest(PrimaryDc,RidOfNonMachineAccoun" +
                            "t,MessageOne)\'");
                    temp95 = this.INrpcServerAdapterInstance.NetrLogonComputeServerDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.RidType)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103756");
                    this.Manager.Checkpoint("MS-NRPC_R103743");
                    this.Manager.Comment("reaching state \'S1141\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeServerDigest/ERROR_NO_SUCH_USER\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SUCH_USER, temp95, "return of NetrLogonComputeServerDigest, state S1141");
                    this.Manager.Comment("reaching state \'S1363\'");
                    goto label26;
                }
                throw new InvalidOperationException("never reached");
            label26:
;
                goto label27;
            }
            throw new InvalidOperationException("never reached");
        label27:
;
            this.Manager.EndTest();
        }
        
        private void Test_AuthenticateMessageS108GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S109");
        }
        
        private void Test_AuthenticateMessageS108GetClientAccountTypeChecker(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S452");
        }
        
        private void Test_AuthenticateMessageS108GetClientAccountTypeChecker1(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S452");
        }
        
        private void Test_AuthenticateMessageS108GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S109");
        }
        
        private void Test_AuthenticateMessageS108GetClientAccountTypeChecker2(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S453");
        }
        
        private void Test_AuthenticateMessageS108GetClientAccountTypeChecker3(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S453");
        }
        
        private void Test_AuthenticateMessageS108GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S109");
        }
        
        private void Test_AuthenticateMessageS108GetClientAccountTypeChecker4(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S454");
        }
        
        private void Test_AuthenticateMessageS108GetClientAccountTypeChecker5(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S454");
        }
        #endregion
        
        #region Test Starting in S110
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_AuthenticateMessageS110() {
            this.Manager.BeginTest("Test_AuthenticateMessageS110");
            this.Manager.Comment("reaching state \'S110\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp98;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp98);
            this.Manager.AddReturn(GetPlatformInfo, null, temp98);
            this.Manager.Comment("reaching state \'S111\'");
            int temp111 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_AuthenticateMessageS110GetPlatformChecker)), new ExpectedReturn(Test_AuthenticateMessage.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_AuthenticateMessageS110GetPlatformChecker1)), new ExpectedReturn(Test_AuthenticateMessage.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_AuthenticateMessageS110GetPlatformChecker2)));
            if ((temp111 == 0)) {
                this.Manager.Comment("reaching state \'S281\'");
                bool temp99;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp99);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp99);
                this.Manager.Comment("reaching state \'S455\'");
                int temp102 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS110GetClientAccountTypeChecker)), new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS110GetClientAccountTypeChecker1)));
                if ((temp102 == 0)) {
                    this.Manager.Comment("reaching state \'S794\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp100;
                    this.Manager.Comment("executing step \'call NetrLogonSetServiceBits(NonDcServer,0,0)\'");
                    temp100 = this.INrpcServerAdapterInstance.NetrLogonSetServiceBits(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 0u, 0u);
                    this.Manager.Comment("reaching state \'S1142\'");
                    this.Manager.Comment("checking step \'return NetrLogonSetServiceBits/ERROR_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_ACCESS_DENIED, temp100, "return of NetrLogonSetServiceBits, state S1142");
                    Test_AuthenticateMessageS1216();
                    goto label28;
                }
                if ((temp102 == 1)) {
                    this.Manager.Comment("reaching state \'S795\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp101;
                    this.Manager.Comment("executing step \'call NetrLogonSetServiceBits(NonDcServer,0,0)\'");
                    temp101 = this.INrpcServerAdapterInstance.NetrLogonSetServiceBits(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, 0u, 0u);
                    this.Manager.Comment("reaching state \'S1143\'");
                    this.Manager.Comment("checking step \'return NetrLogonSetServiceBits/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp101, "return of NetrLogonSetServiceBits, state S1143");
                    Test_AuthenticateMessageS1217();
                    goto label28;
                }
                throw new InvalidOperationException("never reached");
            label28:
;
                goto label31;
            }
            if ((temp111 == 1)) {
                this.Manager.Comment("reaching state \'S282\'");
                bool temp103;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp103);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp103);
                this.Manager.Comment("reaching state \'S456\'");
                int temp106 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS110GetClientAccountTypeChecker2)), new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS110GetClientAccountTypeChecker3)));
                if ((temp106 == 0)) {
                    this.Manager.Comment("reaching state \'S796\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp104;
                    this.Manager.Comment("executing step \'call NetrLogonComputeServerDigest(PrimaryDc,RidOfNonMachineAccoun" +
                            "t,MessageOne)\'");
                    temp104 = this.INrpcServerAdapterInstance.NetrLogonComputeServerDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.RidType)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103756");
                    this.Manager.Checkpoint("MS-NRPC_R103743");
                    this.Manager.Comment("reaching state \'S1144\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeServerDigest/ERROR_NO_SUCH_USER\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SUCH_USER, temp104, "return of NetrLogonComputeServerDigest, state S1144");
                    this.Manager.Comment("reaching state \'S1364\'");
                    goto label29;
                }
                if ((temp106 == 1)) {
                    this.Manager.Comment("reaching state \'S797\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp105;
                    this.Manager.Comment("executing step \'call NetrLogonComputeServerDigest(PrimaryDc,RidOfNonMachineAccoun" +
                            "t,MessageOne)\'");
                    temp105 = this.INrpcServerAdapterInstance.NetrLogonComputeServerDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.RidType)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103756");
                    this.Manager.Checkpoint("MS-NRPC_R103743");
                    this.Manager.Comment("reaching state \'S1145\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeServerDigest/ERROR_NO_SUCH_USER\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SUCH_USER, temp105, "return of NetrLogonComputeServerDigest, state S1145");
                    this.Manager.Comment("reaching state \'S1365\'");
                    goto label29;
                }
                throw new InvalidOperationException("never reached");
            label29:
;
                goto label31;
            }
            if ((temp111 == 2)) {
                this.Manager.Comment("reaching state \'S283\'");
                bool temp107;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp107);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp107);
                this.Manager.Comment("reaching state \'S457\'");
                int temp110 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS110GetClientAccountTypeChecker4)), new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS110GetClientAccountTypeChecker5)));
                if ((temp110 == 0)) {
                    this.Manager.Comment("reaching state \'S798\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp108;
                    this.Manager.Comment("executing step \'call NetrLogonComputeServerDigest(PrimaryDc,RidOfNonMachineAccoun" +
                            "t,MessageOne)\'");
                    temp108 = this.INrpcServerAdapterInstance.NetrLogonComputeServerDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.RidType)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103756");
                    this.Manager.Checkpoint("MS-NRPC_R103743");
                    this.Manager.Comment("reaching state \'S1146\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeServerDigest/ERROR_NO_SUCH_USER\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SUCH_USER, temp108, "return of NetrLogonComputeServerDigest, state S1146");
                    this.Manager.Comment("reaching state \'S1366\'");
                    goto label30;
                }
                if ((temp110 == 1)) {
                    this.Manager.Comment("reaching state \'S799\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp109;
                    this.Manager.Comment("executing step \'call NetrLogonComputeServerDigest(PrimaryDc,RidOfNonMachineAccoun" +
                            "t,MessageOne)\'");
                    temp109 = this.INrpcServerAdapterInstance.NetrLogonComputeServerDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.RidType)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103756");
                    this.Manager.Checkpoint("MS-NRPC_R103743");
                    this.Manager.Comment("reaching state \'S1147\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeServerDigest/ERROR_NO_SUCH_USER\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SUCH_USER, temp109, "return of NetrLogonComputeServerDigest, state S1147");
                    this.Manager.Comment("reaching state \'S1367\'");
                    goto label30;
                }
                throw new InvalidOperationException("never reached");
            label30:
;
                goto label31;
            }
            throw new InvalidOperationException("never reached");
        label31:
;
            this.Manager.EndTest();
        }
        
        private void Test_AuthenticateMessageS110GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S111");
        }
        
        private void Test_AuthenticateMessageS110GetClientAccountTypeChecker(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S455");
        }
        
        private void Test_AuthenticateMessageS110GetClientAccountTypeChecker1(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S455");
        }
        
        private void Test_AuthenticateMessageS110GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S111");
        }
        
        private void Test_AuthenticateMessageS110GetClientAccountTypeChecker2(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S456");
        }
        
        private void Test_AuthenticateMessageS110GetClientAccountTypeChecker3(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S456");
        }
        
        private void Test_AuthenticateMessageS110GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S111");
        }
        
        private void Test_AuthenticateMessageS110GetClientAccountTypeChecker4(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S457");
        }
        
        private void Test_AuthenticateMessageS110GetClientAccountTypeChecker5(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S457");
        }
        #endregion
        
        #region Test Starting in S112
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_AuthenticateMessageS112() {
            this.Manager.BeginTest("Test_AuthenticateMessageS112");
            this.Manager.Comment("reaching state \'S112\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp112;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp112);
            this.Manager.AddReturn(GetPlatformInfo, null, temp112);
            this.Manager.Comment("reaching state \'S113\'");
            int temp125 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_AuthenticateMessageS112GetPlatformChecker)), new ExpectedReturn(Test_AuthenticateMessage.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_AuthenticateMessageS112GetPlatformChecker1)), new ExpectedReturn(Test_AuthenticateMessage.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_AuthenticateMessageS112GetPlatformChecker2)));
            if ((temp125 == 0)) {
                this.Manager.Comment("reaching state \'S284\'");
                bool temp113;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp113);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp113);
                this.Manager.Comment("reaching state \'S458\'");
                int temp116 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS112GetClientAccountTypeChecker)), new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS112GetClientAccountTypeChecker1)));
                if ((temp116 == 0)) {
                    this.Manager.Comment("reaching state \'S800\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp114;
                    this.Manager.Comment("executing step \'call NetrLogonGetTimeServiceParentDomain(NonDcServer)\'");
                    temp114 = this.INrpcServerAdapterInstance.NetrLogonGetTimeServiceParentDomain(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer);
                    this.Manager.Comment("reaching state \'S1148\'");
                    this.Manager.Comment("checking step \'return NetrLogonGetTimeServiceParentDomain/ERROR_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_ACCESS_DENIED, temp114, "return of NetrLogonGetTimeServiceParentDomain, state S1148");
                    Test_AuthenticateMessageS1216();
                    goto label32;
                }
                if ((temp116 == 1)) {
                    this.Manager.Comment("reaching state \'S801\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp115;
                    this.Manager.Comment("executing step \'call NetrLogonGetTimeServiceParentDomain(NonDcServer)\'");
                    temp115 = this.INrpcServerAdapterInstance.NetrLogonGetTimeServiceParentDomain(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer);
                    this.Manager.Comment("reaching state \'S1149\'");
                    this.Manager.Comment("checking step \'return NetrLogonGetTimeServiceParentDomain/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp115, "return of NetrLogonGetTimeServiceParentDomain, state S1149");
                    Test_AuthenticateMessageS1217();
                    goto label32;
                }
                throw new InvalidOperationException("never reached");
            label32:
;
                goto label35;
            }
            if ((temp125 == 1)) {
                this.Manager.Comment("reaching state \'S285\'");
                bool temp117;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp117);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp117);
                this.Manager.Comment("reaching state \'S459\'");
                int temp120 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS112GetClientAccountTypeChecker2)), new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS112GetClientAccountTypeChecker3)));
                if ((temp120 == 0)) {
                    this.Manager.Comment("reaching state \'S802\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp118;
                    this.Manager.Comment("executing step \'call NetrLogonComputeServerDigest(PrimaryDc,RidOfNonMachineAccoun" +
                            "t,MessageOne)\'");
                    temp118 = this.INrpcServerAdapterInstance.NetrLogonComputeServerDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.RidType)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103756");
                    this.Manager.Checkpoint("MS-NRPC_R103743");
                    this.Manager.Comment("reaching state \'S1150\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeServerDigest/ERROR_NO_SUCH_USER\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SUCH_USER, temp118, "return of NetrLogonComputeServerDigest, state S1150");
                    this.Manager.Comment("reaching state \'S1368\'");
                    goto label33;
                }
                if ((temp120 == 1)) {
                    this.Manager.Comment("reaching state \'S803\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp119;
                    this.Manager.Comment("executing step \'call NetrLogonComputeServerDigest(PrimaryDc,RidOfNonMachineAccoun" +
                            "t,MessageOne)\'");
                    temp119 = this.INrpcServerAdapterInstance.NetrLogonComputeServerDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.RidType)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103756");
                    this.Manager.Checkpoint("MS-NRPC_R103743");
                    this.Manager.Comment("reaching state \'S1151\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeServerDigest/ERROR_NO_SUCH_USER\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SUCH_USER, temp119, "return of NetrLogonComputeServerDigest, state S1151");
                    this.Manager.Comment("reaching state \'S1369\'");
                    goto label33;
                }
                throw new InvalidOperationException("never reached");
            label33:
;
                goto label35;
            }
            if ((temp125 == 2)) {
                this.Manager.Comment("reaching state \'S286\'");
                bool temp121;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp121);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp121);
                this.Manager.Comment("reaching state \'S460\'");
                int temp124 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS112GetClientAccountTypeChecker4)), new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS112GetClientAccountTypeChecker5)));
                if ((temp124 == 0)) {
                    this.Manager.Comment("reaching state \'S804\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp122;
                    this.Manager.Comment("executing step \'call NetrLogonComputeServerDigest(PrimaryDc,RidOfNonMachineAccoun" +
                            "t,MessageOne)\'");
                    temp122 = this.INrpcServerAdapterInstance.NetrLogonComputeServerDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.RidType)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103756");
                    this.Manager.Checkpoint("MS-NRPC_R103743");
                    this.Manager.Comment("reaching state \'S1152\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeServerDigest/ERROR_NO_SUCH_USER\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SUCH_USER, temp122, "return of NetrLogonComputeServerDigest, state S1152");
                    this.Manager.Comment("reaching state \'S1370\'");
                    goto label34;
                }
                if ((temp124 == 1)) {
                    this.Manager.Comment("reaching state \'S805\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp123;
                    this.Manager.Comment("executing step \'call NetrLogonComputeServerDigest(PrimaryDc,RidOfNonMachineAccoun" +
                            "t,MessageOne)\'");
                    temp123 = this.INrpcServerAdapterInstance.NetrLogonComputeServerDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.RidType)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103756");
                    this.Manager.Checkpoint("MS-NRPC_R103743");
                    this.Manager.Comment("reaching state \'S1153\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeServerDigest/ERROR_NO_SUCH_USER\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SUCH_USER, temp123, "return of NetrLogonComputeServerDigest, state S1153");
                    this.Manager.Comment("reaching state \'S1371\'");
                    goto label34;
                }
                throw new InvalidOperationException("never reached");
            label34:
;
                goto label35;
            }
            throw new InvalidOperationException("never reached");
        label35:
;
            this.Manager.EndTest();
        }
        
        private void Test_AuthenticateMessageS112GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S113");
        }
        
        private void Test_AuthenticateMessageS112GetClientAccountTypeChecker(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S458");
        }
        
        private void Test_AuthenticateMessageS112GetClientAccountTypeChecker1(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S458");
        }
        
        private void Test_AuthenticateMessageS112GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S113");
        }
        
        private void Test_AuthenticateMessageS112GetClientAccountTypeChecker2(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S459");
        }
        
        private void Test_AuthenticateMessageS112GetClientAccountTypeChecker3(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S459");
        }
        
        private void Test_AuthenticateMessageS112GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S113");
        }
        
        private void Test_AuthenticateMessageS112GetClientAccountTypeChecker4(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S460");
        }
        
        private void Test_AuthenticateMessageS112GetClientAccountTypeChecker5(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S460");
        }
        #endregion
        
        #region Test Starting in S114
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_AuthenticateMessageS114() {
            this.Manager.BeginTest("Test_AuthenticateMessageS114");
            this.Manager.Comment("reaching state \'S114\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp126;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp126);
            this.Manager.AddReturn(GetPlatformInfo, null, temp126);
            this.Manager.Comment("reaching state \'S115\'");
            int temp139 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_AuthenticateMessageS114GetPlatformChecker)), new ExpectedReturn(Test_AuthenticateMessage.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_AuthenticateMessageS114GetPlatformChecker1)), new ExpectedReturn(Test_AuthenticateMessage.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_AuthenticateMessageS114GetPlatformChecker2)));
            if ((temp139 == 0)) {
                this.Manager.Comment("reaching state \'S287\'");
                bool temp127;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp127);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp127);
                this.Manager.Comment("reaching state \'S461\'");
                int temp130 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS114GetClientAccountTypeChecker)), new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS114GetClientAccountTypeChecker1)));
                if ((temp130 == 0)) {
                    this.Manager.Comment("reaching state \'S806\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp128;
                    this.Manager.Comment("executing step \'call NetrLogonGetTimeServiceParentDomain(PrimaryDc)\'");
                    temp128 = this.INrpcServerAdapterInstance.NetrLogonGetTimeServiceParentDomain(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc);
                    this.Manager.Comment("reaching state \'S1154\'");
                    this.Manager.Comment("checking step \'return NetrLogonGetTimeServiceParentDomain/ERROR_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_ACCESS_DENIED, temp128, "return of NetrLogonGetTimeServiceParentDomain, state S1154");
                    Test_AuthenticateMessageS1216();
                    goto label36;
                }
                if ((temp130 == 1)) {
                    this.Manager.Comment("reaching state \'S807\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp129;
                    this.Manager.Comment("executing step \'call NetrLogonGetTimeServiceParentDomain(PrimaryDc)\'");
                    temp129 = this.INrpcServerAdapterInstance.NetrLogonGetTimeServiceParentDomain(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc);
                    this.Manager.Comment("reaching state \'S1155\'");
                    this.Manager.Comment("checking step \'return NetrLogonGetTimeServiceParentDomain/ERROR_NO_SUCH_DOMAIN\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SUCH_DOMAIN, temp129, "return of NetrLogonGetTimeServiceParentDomain, state S1155");
                    Test_AuthenticateMessageS1217();
                    goto label36;
                }
                throw new InvalidOperationException("never reached");
            label36:
;
                goto label39;
            }
            if ((temp139 == 1)) {
                this.Manager.Comment("reaching state \'S288\'");
                bool temp131;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp131);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp131);
                this.Manager.Comment("reaching state \'S462\'");
                int temp134 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS114GetClientAccountTypeChecker2)), new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS114GetClientAccountTypeChecker3)));
                if ((temp134 == 0)) {
                    this.Manager.Comment("reaching state \'S808\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp132;
                    this.Manager.Comment("executing step \'call NetrLogonComputeServerDigest(PrimaryDc,RidOfNonMachineAccoun" +
                            "t,MessageOne)\'");
                    temp132 = this.INrpcServerAdapterInstance.NetrLogonComputeServerDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.RidType)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103756");
                    this.Manager.Checkpoint("MS-NRPC_R103743");
                    this.Manager.Comment("reaching state \'S1156\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeServerDigest/ERROR_NO_SUCH_USER\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SUCH_USER, temp132, "return of NetrLogonComputeServerDigest, state S1156");
                    this.Manager.Comment("reaching state \'S1372\'");
                    goto label37;
                }
                if ((temp134 == 1)) {
                    this.Manager.Comment("reaching state \'S809\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp133;
                    this.Manager.Comment("executing step \'call NetrLogonComputeServerDigest(PrimaryDc,RidOfNonMachineAccoun" +
                            "t,MessageOne)\'");
                    temp133 = this.INrpcServerAdapterInstance.NetrLogonComputeServerDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.RidType)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103756");
                    this.Manager.Checkpoint("MS-NRPC_R103743");
                    this.Manager.Comment("reaching state \'S1157\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeServerDigest/ERROR_NO_SUCH_USER\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SUCH_USER, temp133, "return of NetrLogonComputeServerDigest, state S1157");
                    this.Manager.Comment("reaching state \'S1373\'");
                    goto label37;
                }
                throw new InvalidOperationException("never reached");
            label37:
;
                goto label39;
            }
            if ((temp139 == 2)) {
                this.Manager.Comment("reaching state \'S289\'");
                bool temp135;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp135);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp135);
                this.Manager.Comment("reaching state \'S463\'");
                int temp138 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS114GetClientAccountTypeChecker4)), new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS114GetClientAccountTypeChecker5)));
                if ((temp138 == 0)) {
                    this.Manager.Comment("reaching state \'S810\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp136;
                    this.Manager.Comment("executing step \'call NetrLogonComputeServerDigest(PrimaryDc,RidOfNonMachineAccoun" +
                            "t,MessageOne)\'");
                    temp136 = this.INrpcServerAdapterInstance.NetrLogonComputeServerDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.RidType)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103756");
                    this.Manager.Checkpoint("MS-NRPC_R103743");
                    this.Manager.Comment("reaching state \'S1158\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeServerDigest/ERROR_NO_SUCH_USER\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SUCH_USER, temp136, "return of NetrLogonComputeServerDigest, state S1158");
                    this.Manager.Comment("reaching state \'S1374\'");
                    goto label38;
                }
                if ((temp138 == 1)) {
                    this.Manager.Comment("reaching state \'S811\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp137;
                    this.Manager.Comment("executing step \'call NetrLogonComputeServerDigest(PrimaryDc,RidOfNonMachineAccoun" +
                            "t,MessageOne)\'");
                    temp137 = this.INrpcServerAdapterInstance.NetrLogonComputeServerDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.RidType)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103756");
                    this.Manager.Checkpoint("MS-NRPC_R103743");
                    this.Manager.Comment("reaching state \'S1159\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeServerDigest/ERROR_NO_SUCH_USER\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SUCH_USER, temp137, "return of NetrLogonComputeServerDigest, state S1159");
                    this.Manager.Comment("reaching state \'S1375\'");
                    goto label38;
                }
                throw new InvalidOperationException("never reached");
            label38:
;
                goto label39;
            }
            throw new InvalidOperationException("never reached");
        label39:
;
            this.Manager.EndTest();
        }
        
        private void Test_AuthenticateMessageS114GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S115");
        }
        
        private void Test_AuthenticateMessageS114GetClientAccountTypeChecker(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S461");
        }
        
        private void Test_AuthenticateMessageS114GetClientAccountTypeChecker1(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S461");
        }
        
        private void Test_AuthenticateMessageS114GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S115");
        }
        
        private void Test_AuthenticateMessageS114GetClientAccountTypeChecker2(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S462");
        }
        
        private void Test_AuthenticateMessageS114GetClientAccountTypeChecker3(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S462");
        }
        
        private void Test_AuthenticateMessageS114GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S115");
        }
        
        private void Test_AuthenticateMessageS114GetClientAccountTypeChecker4(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S463");
        }
        
        private void Test_AuthenticateMessageS114GetClientAccountTypeChecker5(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S463");
        }
        #endregion
        
        #region Test Starting in S12
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_AuthenticateMessageS12() {
            this.Manager.BeginTest("Test_AuthenticateMessageS12");
            this.Manager.Comment("reaching state \'S12\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp140;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp140);
            this.Manager.AddReturn(GetPlatformInfo, null, temp140);
            this.Manager.Comment("reaching state \'S13\'");
            int temp153 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_AuthenticateMessageS12GetPlatformChecker)), new ExpectedReturn(Test_AuthenticateMessage.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_AuthenticateMessageS12GetPlatformChecker1)), new ExpectedReturn(Test_AuthenticateMessage.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_AuthenticateMessageS12GetPlatformChecker2)));
            if ((temp153 == 0)) {
                this.Manager.Comment("reaching state \'S134\'");
                bool temp141;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp141);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp141);
                this.Manager.Comment("reaching state \'S308\'");
                int temp144 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS12GetClientAccountTypeChecker)), new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS12GetClientAccountTypeChecker1)));
                if ((temp144 == 0)) {
                    this.Manager.Comment("reaching state \'S500\'");
                    this.Manager.Comment("executing step \'call SwitchUserAccount(False)\'");
                    this.INrpcServerAdapterInstance.SwitchUserAccount(false);
                    this.Manager.Comment("reaching state \'S848\'");
                    this.Manager.Comment("checking step \'return SwitchUserAccount\'");
                    this.Manager.Comment("reaching state \'S1196\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp142;
                    this.Manager.Comment("executing step \'call NetrLogonComputeClientDigest(PrimaryDc,FqdnFormatDomainName," +
                            "MessageOne)\'");
                    temp142 = this.INrpcServerAdapterInstance.NetrLogonComputeClientDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.FqdnFormatDomainName, ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103791");
                    this.Manager.Checkpoint("MS-NRPC_R103790");
                    this.Manager.Comment("reaching state \'S1402\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeClientDigest/ERROR_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_ACCESS_DENIED, temp142, "return of NetrLogonComputeClientDigest, state S1402");
                    Test_AuthenticateMessageS1448();
                    goto label40;
                }
                if ((temp144 == 1)) {
                    this.Manager.Comment("reaching state \'S501\'");
                    this.Manager.Comment("executing step \'call SwitchUserAccount(False)\'");
                    this.INrpcServerAdapterInstance.SwitchUserAccount(false);
                    this.Manager.Comment("reaching state \'S849\'");
                    this.Manager.Comment("checking step \'return SwitchUserAccount\'");
                    this.Manager.Comment("reaching state \'S1197\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp143;
                    this.Manager.Comment("executing step \'call NetrLogonComputeClientDigest(PrimaryDc,FqdnFormatDomainName," +
                            "MessageOne)\'");
                    temp143 = this.INrpcServerAdapterInstance.NetrLogonComputeClientDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.FqdnFormatDomainName, ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103791");
                    this.Manager.Checkpoint("MS-NRPC_R103790");
                    this.Manager.Comment("reaching state \'S1403\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeClientDigest/ERROR_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_ACCESS_DENIED, temp143, "return of NetrLogonComputeClientDigest, state S1403");
                    Test_AuthenticateMessageS1449();
                    goto label40;
                }
                throw new InvalidOperationException("never reached");
            label40:
;
                goto label43;
            }
            if ((temp153 == 1)) {
                this.Manager.Comment("reaching state \'S135\'");
                bool temp145;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp145);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp145);
                this.Manager.Comment("reaching state \'S309\'");
                int temp148 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS12GetClientAccountTypeChecker2)), new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS12GetClientAccountTypeChecker3)));
                if ((temp148 == 0)) {
                    this.Manager.Comment("reaching state \'S502\'");
                    this.Manager.Comment("executing step \'call SwitchUserAccount(False)\'");
                    this.INrpcServerAdapterInstance.SwitchUserAccount(false);
                    this.Manager.Comment("reaching state \'S850\'");
                    this.Manager.Comment("checking step \'return SwitchUserAccount\'");
                    this.Manager.Comment("reaching state \'S1198\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp146;
                    this.Manager.Comment("executing step \'call NetrLogonComputeClientDigest(PrimaryDc,FqdnFormatDomainName," +
                            "MessageOne)\'");
                    temp146 = this.INrpcServerAdapterInstance.NetrLogonComputeClientDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.FqdnFormatDomainName, ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103791");
                    this.Manager.Checkpoint("MS-NRPC_R103790");
                    this.Manager.Comment("reaching state \'S1404\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeClientDigest/ERROR_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_ACCESS_DENIED, temp146, "return of NetrLogonComputeClientDigest, state S1404");
                    Test_AuthenticateMessageS1450();
                    goto label41;
                }
                if ((temp148 == 1)) {
                    this.Manager.Comment("reaching state \'S503\'");
                    this.Manager.Comment("executing step \'call SwitchUserAccount(False)\'");
                    this.INrpcServerAdapterInstance.SwitchUserAccount(false);
                    this.Manager.Comment("reaching state \'S851\'");
                    this.Manager.Comment("checking step \'return SwitchUserAccount\'");
                    this.Manager.Comment("reaching state \'S1199\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp147;
                    this.Manager.Comment("executing step \'call NetrLogonComputeClientDigest(PrimaryDc,FqdnFormatDomainName," +
                            "MessageOne)\'");
                    temp147 = this.INrpcServerAdapterInstance.NetrLogonComputeClientDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.FqdnFormatDomainName, ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103791");
                    this.Manager.Checkpoint("MS-NRPC_R103790");
                    this.Manager.Comment("reaching state \'S1405\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeClientDigest/ERROR_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_ACCESS_DENIED, temp147, "return of NetrLogonComputeClientDigest, state S1405");
                    Test_AuthenticateMessageS1451();
                    goto label41;
                }
                throw new InvalidOperationException("never reached");
            label41:
;
                goto label43;
            }
            if ((temp153 == 2)) {
                this.Manager.Comment("reaching state \'S136\'");
                bool temp149;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp149);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp149);
                this.Manager.Comment("reaching state \'S310\'");
                int temp152 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS12GetClientAccountTypeChecker4)), new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS12GetClientAccountTypeChecker5)));
                if ((temp152 == 0)) {
                    this.Manager.Comment("reaching state \'S504\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp150;
                    this.Manager.Comment("executing step \'call NetrLogonComputeServerDigest(PrimaryDc,RidOfNonMachineAccoun" +
                            "t,MessageOne)\'");
                    temp150 = this.INrpcServerAdapterInstance.NetrLogonComputeServerDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.RidType)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103756");
                    this.Manager.Checkpoint("MS-NRPC_R103743");
                    this.Manager.Comment("reaching state \'S852\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeServerDigest/ERROR_NO_SUCH_USER\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SUCH_USER, temp150, "return of NetrLogonComputeServerDigest, state S852");
                    this.Manager.Comment("reaching state \'S1200\'");
                    goto label42;
                }
                if ((temp152 == 1)) {
                    this.Manager.Comment("reaching state \'S505\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp151;
                    this.Manager.Comment("executing step \'call NetrLogonComputeServerDigest(PrimaryDc,RidOfNonMachineAccoun" +
                            "t,MessageOne)\'");
                    temp151 = this.INrpcServerAdapterInstance.NetrLogonComputeServerDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.RidType)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103756");
                    this.Manager.Checkpoint("MS-NRPC_R103743");
                    this.Manager.Comment("reaching state \'S853\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeServerDigest/ERROR_NO_SUCH_USER\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SUCH_USER, temp151, "return of NetrLogonComputeServerDigest, state S853");
                    this.Manager.Comment("reaching state \'S1201\'");
                    goto label42;
                }
                throw new InvalidOperationException("never reached");
            label42:
;
                goto label43;
            }
            throw new InvalidOperationException("never reached");
        label43:
;
            this.Manager.EndTest();
        }
        
        private void Test_AuthenticateMessageS12GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S13");
        }
        
        private void Test_AuthenticateMessageS12GetClientAccountTypeChecker(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S308");
        }
        
        private void Test_AuthenticateMessageS12GetClientAccountTypeChecker1(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S308");
        }
        
        private void Test_AuthenticateMessageS12GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S13");
        }
        
        private void Test_AuthenticateMessageS12GetClientAccountTypeChecker2(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S309");
        }
        
        private void Test_AuthenticateMessageS12GetClientAccountTypeChecker3(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S309");
        }
        
        private void Test_AuthenticateMessageS12GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S13");
        }
        
        private void Test_AuthenticateMessageS12GetClientAccountTypeChecker4(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S310");
        }
        
        private void Test_AuthenticateMessageS12GetClientAccountTypeChecker5(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S310");
        }
        #endregion
        
        #region Test Starting in S14
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_AuthenticateMessageS14() {
            this.Manager.BeginTest("Test_AuthenticateMessageS14");
            this.Manager.Comment("reaching state \'S14\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp154;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp154);
            this.Manager.AddReturn(GetPlatformInfo, null, temp154);
            this.Manager.Comment("reaching state \'S15\'");
            int temp167 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_AuthenticateMessageS14GetPlatformChecker)), new ExpectedReturn(Test_AuthenticateMessage.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_AuthenticateMessageS14GetPlatformChecker1)), new ExpectedReturn(Test_AuthenticateMessage.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_AuthenticateMessageS14GetPlatformChecker2)));
            if ((temp167 == 0)) {
                this.Manager.Comment("reaching state \'S137\'");
                bool temp155;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp155);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp155);
                this.Manager.Comment("reaching state \'S311\'");
                int temp158 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS14GetClientAccountTypeChecker)), new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS14GetClientAccountTypeChecker1)));
                if ((temp158 == 0)) {
                    this.Manager.Comment("reaching state \'S506\'");
                    this.Manager.Comment("executing step \'call SwitchUserAccount(False)\'");
                    this.INrpcServerAdapterInstance.SwitchUserAccount(false);
                    this.Manager.Comment("reaching state \'S854\'");
                    this.Manager.Comment("checking step \'return SwitchUserAccount\'");
                    this.Manager.Comment("reaching state \'S1202\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp156;
                    this.Manager.Comment("executing step \'call NetrLogonComputeServerDigest(PrimaryDc,NonExist,MessageOne)\'" +
                            "");
                    temp156 = this.INrpcServerAdapterInstance.NetrLogonComputeServerDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.RidType)(0)), ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103746");
                    this.Manager.Checkpoint("MS-NRPC_R103743");
                    this.Manager.Comment("reaching state \'S1406\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeServerDigest/ERROR_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_ACCESS_DENIED, temp156, "return of NetrLogonComputeServerDigest, state S1406");
                    Test_AuthenticateMessageS1452();
                    goto label44;
                }
                if ((temp158 == 1)) {
                    this.Manager.Comment("reaching state \'S507\'");
                    this.Manager.Comment("executing step \'call SwitchUserAccount(False)\'");
                    this.INrpcServerAdapterInstance.SwitchUserAccount(false);
                    this.Manager.Comment("reaching state \'S855\'");
                    this.Manager.Comment("checking step \'return SwitchUserAccount\'");
                    this.Manager.Comment("reaching state \'S1203\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp157;
                    this.Manager.Comment("executing step \'call NetrLogonComputeServerDigest(PrimaryDc,NonExist,MessageOne)\'" +
                            "");
                    temp157 = this.INrpcServerAdapterInstance.NetrLogonComputeServerDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.RidType)(0)), ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103746");
                    this.Manager.Checkpoint("MS-NRPC_R103743");
                    this.Manager.Comment("reaching state \'S1407\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeServerDigest/ERROR_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_ACCESS_DENIED, temp157, "return of NetrLogonComputeServerDigest, state S1407");
                    Test_AuthenticateMessageS1453();
                    goto label44;
                }
                throw new InvalidOperationException("never reached");
            label44:
;
                goto label47;
            }
            if ((temp167 == 1)) {
                this.Manager.Comment("reaching state \'S138\'");
                bool temp159;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp159);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp159);
                this.Manager.Comment("reaching state \'S312\'");
                int temp162 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS14GetClientAccountTypeChecker2)), new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS14GetClientAccountTypeChecker3)));
                if ((temp162 == 0)) {
                    this.Manager.Comment("reaching state \'S508\'");
                    this.Manager.Comment("executing step \'call SwitchUserAccount(False)\'");
                    this.INrpcServerAdapterInstance.SwitchUserAccount(false);
                    this.Manager.Comment("reaching state \'S856\'");
                    this.Manager.Comment("checking step \'return SwitchUserAccount\'");
                    this.Manager.Comment("reaching state \'S1204\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp160;
                    this.Manager.Comment("executing step \'call NetrLogonComputeServerDigest(PrimaryDc,NonExist,MessageOne)\'" +
                            "");
                    temp160 = this.INrpcServerAdapterInstance.NetrLogonComputeServerDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.RidType)(0)), ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103746");
                    this.Manager.Checkpoint("MS-NRPC_R103743");
                    this.Manager.Comment("reaching state \'S1408\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeServerDigest/ERROR_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_ACCESS_DENIED, temp160, "return of NetrLogonComputeServerDigest, state S1408");
                    Test_AuthenticateMessageS1454();
                    goto label45;
                }
                if ((temp162 == 1)) {
                    this.Manager.Comment("reaching state \'S509\'");
                    this.Manager.Comment("executing step \'call SwitchUserAccount(False)\'");
                    this.INrpcServerAdapterInstance.SwitchUserAccount(false);
                    this.Manager.Comment("reaching state \'S857\'");
                    this.Manager.Comment("checking step \'return SwitchUserAccount\'");
                    this.Manager.Comment("reaching state \'S1205\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp161;
                    this.Manager.Comment("executing step \'call NetrLogonComputeServerDigest(PrimaryDc,NonExist,MessageOne)\'" +
                            "");
                    temp161 = this.INrpcServerAdapterInstance.NetrLogonComputeServerDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.RidType)(0)), ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103746");
                    this.Manager.Checkpoint("MS-NRPC_R103743");
                    this.Manager.Comment("reaching state \'S1409\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeServerDigest/ERROR_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_ACCESS_DENIED, temp161, "return of NetrLogonComputeServerDigest, state S1409");
                    Test_AuthenticateMessageS1455();
                    goto label45;
                }
                throw new InvalidOperationException("never reached");
            label45:
;
                goto label47;
            }
            if ((temp167 == 2)) {
                this.Manager.Comment("reaching state \'S139\'");
                bool temp163;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp163);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp163);
                this.Manager.Comment("reaching state \'S313\'");
                int temp166 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS14GetClientAccountTypeChecker4)), new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS14GetClientAccountTypeChecker5)));
                if ((temp166 == 0)) {
                    this.Manager.Comment("reaching state \'S510\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp164;
                    this.Manager.Comment("executing step \'call NetrLogonComputeServerDigest(PrimaryDc,RidOfNonMachineAccoun" +
                            "t,MessageOne)\'");
                    temp164 = this.INrpcServerAdapterInstance.NetrLogonComputeServerDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.RidType)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103756");
                    this.Manager.Checkpoint("MS-NRPC_R103743");
                    this.Manager.Comment("reaching state \'S858\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeServerDigest/ERROR_NO_SUCH_USER\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SUCH_USER, temp164, "return of NetrLogonComputeServerDigest, state S858");
                    this.Manager.Comment("reaching state \'S1206\'");
                    goto label46;
                }
                if ((temp166 == 1)) {
                    this.Manager.Comment("reaching state \'S511\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp165;
                    this.Manager.Comment("executing step \'call NetrLogonComputeServerDigest(PrimaryDc,RidOfNonMachineAccoun" +
                            "t,MessageOne)\'");
                    temp165 = this.INrpcServerAdapterInstance.NetrLogonComputeServerDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.RidType)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103756");
                    this.Manager.Checkpoint("MS-NRPC_R103743");
                    this.Manager.Comment("reaching state \'S859\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeServerDigest/ERROR_NO_SUCH_USER\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SUCH_USER, temp165, "return of NetrLogonComputeServerDigest, state S859");
                    this.Manager.Comment("reaching state \'S1207\'");
                    goto label46;
                }
                throw new InvalidOperationException("never reached");
            label46:
;
                goto label47;
            }
            throw new InvalidOperationException("never reached");
        label47:
;
            this.Manager.EndTest();
        }
        
        private void Test_AuthenticateMessageS14GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S15");
        }
        
        private void Test_AuthenticateMessageS14GetClientAccountTypeChecker(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S311");
        }
        
        private void Test_AuthenticateMessageS1452() {
            this.Manager.Comment("reaching state \'S1452\'");
            this.Manager.Comment("executing step \'call SwitchUserAccount(True)\'");
            this.INrpcServerAdapterInstance.SwitchUserAccount(true);
            this.Manager.Comment("reaching state \'S1470\'");
            this.Manager.Comment("checking step \'return SwitchUserAccount\'");
            Test_AuthenticateMessageS1216();
        }
        
        private void Test_AuthenticateMessageS14GetClientAccountTypeChecker1(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S311");
        }
        
        private void Test_AuthenticateMessageS1453() {
            this.Manager.Comment("reaching state \'S1453\'");
            this.Manager.Comment("executing step \'call SwitchUserAccount(True)\'");
            this.INrpcServerAdapterInstance.SwitchUserAccount(true);
            this.Manager.Comment("reaching state \'S1471\'");
            this.Manager.Comment("checking step \'return SwitchUserAccount\'");
            Test_AuthenticateMessageS1217();
        }
        
        private void Test_AuthenticateMessageS14GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S15");
        }
        
        private void Test_AuthenticateMessageS14GetClientAccountTypeChecker2(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S312");
        }
        
        private void Test_AuthenticateMessageS1454() {
            this.Manager.Comment("reaching state \'S1454\'");
            this.Manager.Comment("executing step \'call SwitchUserAccount(True)\'");
            this.INrpcServerAdapterInstance.SwitchUserAccount(true);
            this.Manager.Comment("reaching state \'S1472\'");
            this.Manager.Comment("checking step \'return SwitchUserAccount\'");
            Test_AuthenticateMessageS1244();
        }
        
        private void Test_AuthenticateMessageS14GetClientAccountTypeChecker3(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S312");
        }
        
        private void Test_AuthenticateMessageS1455() {
            this.Manager.Comment("reaching state \'S1455\'");
            this.Manager.Comment("executing step \'call SwitchUserAccount(True)\'");
            this.INrpcServerAdapterInstance.SwitchUserAccount(true);
            this.Manager.Comment("reaching state \'S1473\'");
            this.Manager.Comment("checking step \'return SwitchUserAccount\'");
            Test_AuthenticateMessageS1245();
        }
        
        private void Test_AuthenticateMessageS14GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S15");
        }
        
        private void Test_AuthenticateMessageS14GetClientAccountTypeChecker4(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S313");
        }
        
        private void Test_AuthenticateMessageS14GetClientAccountTypeChecker5(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S313");
        }
        #endregion
        
        #region Test Starting in S16
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_AuthenticateMessageS16() {
            this.Manager.BeginTest("Test_AuthenticateMessageS16");
            this.Manager.Comment("reaching state \'S16\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp168;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp168);
            this.Manager.AddReturn(GetPlatformInfo, null, temp168);
            this.Manager.Comment("reaching state \'S17\'");
            int temp181 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_AuthenticateMessageS16GetPlatformChecker)), new ExpectedReturn(Test_AuthenticateMessage.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_AuthenticateMessageS16GetPlatformChecker1)), new ExpectedReturn(Test_AuthenticateMessage.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_AuthenticateMessageS16GetPlatformChecker2)));
            if ((temp181 == 0)) {
                this.Manager.Comment("reaching state \'S140\'");
                bool temp169;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp169);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp169);
                this.Manager.Comment("reaching state \'S314\'");
                int temp172 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS16GetClientAccountTypeChecker)), new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS16GetClientAccountTypeChecker1)));
                if ((temp172 == 0)) {
                    this.Manager.Comment("reaching state \'S512\'");
                    this.Manager.Comment("executing step \'call SwitchUserAccount(False)\'");
                    this.INrpcServerAdapterInstance.SwitchUserAccount(false);
                    this.Manager.Comment("reaching state \'S860\'");
                    this.Manager.Comment("checking step \'return SwitchUserAccount\'");
                    this.Manager.Comment("reaching state \'S1208\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp170;
                    this.Manager.Comment("executing step \'call NetrLogonComputeServerDigest(PrimaryDc,RidOfNonMachineAccoun" +
                            "t,MessageOne)\'");
                    temp170 = this.INrpcServerAdapterInstance.NetrLogonComputeServerDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.RidType)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103746");
                    this.Manager.Checkpoint("MS-NRPC_R103743");
                    this.Manager.Comment("reaching state \'S1410\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeServerDigest/ERROR_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_ACCESS_DENIED, temp170, "return of NetrLogonComputeServerDigest, state S1410");
                    Test_AuthenticateMessageS1452();
                    goto label48;
                }
                if ((temp172 == 1)) {
                    this.Manager.Comment("reaching state \'S513\'");
                    this.Manager.Comment("executing step \'call SwitchUserAccount(False)\'");
                    this.INrpcServerAdapterInstance.SwitchUserAccount(false);
                    this.Manager.Comment("reaching state \'S861\'");
                    this.Manager.Comment("checking step \'return SwitchUserAccount\'");
                    this.Manager.Comment("reaching state \'S1209\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp171;
                    this.Manager.Comment("executing step \'call NetrLogonComputeServerDigest(PrimaryDc,RidOfNonMachineAccoun" +
                            "t,MessageOne)\'");
                    temp171 = this.INrpcServerAdapterInstance.NetrLogonComputeServerDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.RidType)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103746");
                    this.Manager.Checkpoint("MS-NRPC_R103743");
                    this.Manager.Comment("reaching state \'S1411\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeServerDigest/ERROR_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_ACCESS_DENIED, temp171, "return of NetrLogonComputeServerDigest, state S1411");
                    Test_AuthenticateMessageS1453();
                    goto label48;
                }
                throw new InvalidOperationException("never reached");
            label48:
;
                goto label51;
            }
            if ((temp181 == 1)) {
                this.Manager.Comment("reaching state \'S141\'");
                bool temp173;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp173);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp173);
                this.Manager.Comment("reaching state \'S315\'");
                int temp176 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS16GetClientAccountTypeChecker2)), new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS16GetClientAccountTypeChecker3)));
                if ((temp176 == 0)) {
                    this.Manager.Comment("reaching state \'S514\'");
                    this.Manager.Comment("executing step \'call SwitchUserAccount(False)\'");
                    this.INrpcServerAdapterInstance.SwitchUserAccount(false);
                    this.Manager.Comment("reaching state \'S862\'");
                    this.Manager.Comment("checking step \'return SwitchUserAccount\'");
                    this.Manager.Comment("reaching state \'S1210\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp174;
                    this.Manager.Comment("executing step \'call NetrLogonComputeServerDigest(PrimaryDc,RidOfNonMachineAccoun" +
                            "t,MessageOne)\'");
                    temp174 = this.INrpcServerAdapterInstance.NetrLogonComputeServerDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.RidType)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103746");
                    this.Manager.Checkpoint("MS-NRPC_R103743");
                    this.Manager.Comment("reaching state \'S1412\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeServerDigest/ERROR_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_ACCESS_DENIED, temp174, "return of NetrLogonComputeServerDigest, state S1412");
                    Test_AuthenticateMessageS1454();
                    goto label49;
                }
                if ((temp176 == 1)) {
                    this.Manager.Comment("reaching state \'S515\'");
                    this.Manager.Comment("executing step \'call SwitchUserAccount(False)\'");
                    this.INrpcServerAdapterInstance.SwitchUserAccount(false);
                    this.Manager.Comment("reaching state \'S863\'");
                    this.Manager.Comment("checking step \'return SwitchUserAccount\'");
                    this.Manager.Comment("reaching state \'S1211\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp175;
                    this.Manager.Comment("executing step \'call NetrLogonComputeServerDigest(PrimaryDc,RidOfNonMachineAccoun" +
                            "t,MessageOne)\'");
                    temp175 = this.INrpcServerAdapterInstance.NetrLogonComputeServerDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.RidType)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103746");
                    this.Manager.Checkpoint("MS-NRPC_R103743");
                    this.Manager.Comment("reaching state \'S1413\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeServerDigest/ERROR_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_ACCESS_DENIED, temp175, "return of NetrLogonComputeServerDigest, state S1413");
                    Test_AuthenticateMessageS1455();
                    goto label49;
                }
                throw new InvalidOperationException("never reached");
            label49:
;
                goto label51;
            }
            if ((temp181 == 2)) {
                this.Manager.Comment("reaching state \'S142\'");
                bool temp177;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp177);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp177);
                this.Manager.Comment("reaching state \'S316\'");
                int temp180 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS16GetClientAccountTypeChecker4)), new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS16GetClientAccountTypeChecker5)));
                if ((temp180 == 0)) {
                    this.Manager.Comment("reaching state \'S516\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp178;
                    this.Manager.Comment("executing step \'call NetrLogonComputeServerDigest(PrimaryDc,RidOfNonMachineAccoun" +
                            "t,MessageOne)\'");
                    temp178 = this.INrpcServerAdapterInstance.NetrLogonComputeServerDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.RidType)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103756");
                    this.Manager.Checkpoint("MS-NRPC_R103743");
                    this.Manager.Comment("reaching state \'S864\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeServerDigest/ERROR_NO_SUCH_USER\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SUCH_USER, temp178, "return of NetrLogonComputeServerDigest, state S864");
                    this.Manager.Comment("reaching state \'S1212\'");
                    goto label50;
                }
                if ((temp180 == 1)) {
                    this.Manager.Comment("reaching state \'S517\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp179;
                    this.Manager.Comment("executing step \'call NetrLogonComputeServerDigest(PrimaryDc,RidOfNonMachineAccoun" +
                            "t,MessageOne)\'");
                    temp179 = this.INrpcServerAdapterInstance.NetrLogonComputeServerDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.RidType)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103756");
                    this.Manager.Checkpoint("MS-NRPC_R103743");
                    this.Manager.Comment("reaching state \'S865\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeServerDigest/ERROR_NO_SUCH_USER\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SUCH_USER, temp179, "return of NetrLogonComputeServerDigest, state S865");
                    this.Manager.Comment("reaching state \'S1213\'");
                    goto label50;
                }
                throw new InvalidOperationException("never reached");
            label50:
;
                goto label51;
            }
            throw new InvalidOperationException("never reached");
        label51:
;
            this.Manager.EndTest();
        }
        
        private void Test_AuthenticateMessageS16GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S17");
        }
        
        private void Test_AuthenticateMessageS16GetClientAccountTypeChecker(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S314");
        }
        
        private void Test_AuthenticateMessageS16GetClientAccountTypeChecker1(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S314");
        }
        
        private void Test_AuthenticateMessageS16GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S17");
        }
        
        private void Test_AuthenticateMessageS16GetClientAccountTypeChecker2(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S315");
        }
        
        private void Test_AuthenticateMessageS16GetClientAccountTypeChecker3(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S315");
        }
        
        private void Test_AuthenticateMessageS16GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S17");
        }
        
        private void Test_AuthenticateMessageS16GetClientAccountTypeChecker4(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S316");
        }
        
        private void Test_AuthenticateMessageS16GetClientAccountTypeChecker5(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S316");
        }
        #endregion
        
        #region Test Starting in S18
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_AuthenticateMessageS18() {
            this.Manager.BeginTest("Test_AuthenticateMessageS18");
            this.Manager.Comment("reaching state \'S18\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp182;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp182);
            this.Manager.AddReturn(GetPlatformInfo, null, temp182);
            this.Manager.Comment("reaching state \'S19\'");
            int temp195 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_AuthenticateMessageS18GetPlatformChecker)), new ExpectedReturn(Test_AuthenticateMessage.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_AuthenticateMessageS18GetPlatformChecker1)), new ExpectedReturn(Test_AuthenticateMessage.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_AuthenticateMessageS18GetPlatformChecker2)));
            if ((temp195 == 0)) {
                this.Manager.Comment("reaching state \'S143\'");
                bool temp183;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp183);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp183);
                this.Manager.Comment("reaching state \'S317\'");
                int temp186 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS18GetClientAccountTypeChecker)), new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS18GetClientAccountTypeChecker1)));
                if ((temp186 == 0)) {
                    this.Manager.Comment("reaching state \'S518\'");
                    this.Manager.Comment("executing step \'call SwitchUserAccount(False)\'");
                    this.INrpcServerAdapterInstance.SwitchUserAccount(false);
                    this.Manager.Comment("reaching state \'S866\'");
                    this.Manager.Comment("checking step \'return SwitchUserAccount\'");
                    this.Manager.Comment("reaching state \'S1214\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp184;
                    this.Manager.Comment("executing step \'call NetrLogonComputeClientDigest(PrimaryDc,Null,MessageOne)\'");
                    temp184 = this.INrpcServerAdapterInstance.NetrLogonComputeClientDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.DomainNameType)(0)), ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103791");
                    this.Manager.Checkpoint("MS-NRPC_R103790");
                    this.Manager.Comment("reaching state \'S1414\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeClientDigest/ERROR_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_ACCESS_DENIED, temp184, "return of NetrLogonComputeClientDigest, state S1414");
                    Test_AuthenticateMessageS1456();
                    goto label52;
                }
                if ((temp186 == 1)) {
                    this.Manager.Comment("reaching state \'S519\'");
                    this.Manager.Comment("executing step \'call SwitchUserAccount(False)\'");
                    this.INrpcServerAdapterInstance.SwitchUserAccount(false);
                    this.Manager.Comment("reaching state \'S867\'");
                    this.Manager.Comment("checking step \'return SwitchUserAccount\'");
                    this.Manager.Comment("reaching state \'S1215\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp185;
                    this.Manager.Comment("executing step \'call NetrLogonComputeClientDigest(PrimaryDc,Null,MessageOne)\'");
                    temp185 = this.INrpcServerAdapterInstance.NetrLogonComputeClientDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.DomainNameType)(0)), ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103791");
                    this.Manager.Checkpoint("MS-NRPC_R103790");
                    this.Manager.Comment("reaching state \'S1415\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeClientDigest/ERROR_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_ACCESS_DENIED, temp185, "return of NetrLogonComputeClientDigest, state S1415");
                    Test_AuthenticateMessageS1457();
                    goto label52;
                }
                throw new InvalidOperationException("never reached");
            label52:
;
                goto label55;
            }
            if ((temp195 == 1)) {
                this.Manager.Comment("reaching state \'S144\'");
                bool temp187;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp187);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp187);
                this.Manager.Comment("reaching state \'S318\'");
                int temp190 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS18GetClientAccountTypeChecker2)), new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS18GetClientAccountTypeChecker3)));
                if ((temp190 == 0)) {
                    this.Manager.Comment("reaching state \'S520\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp188;
                    this.Manager.Comment("executing step \'call NetrLogonGetTrustRid(NonDcServer,FqdnFormatDomainName)\'");
                    temp188 = this.INrpcServerAdapterInstance.NetrLogonGetTrustRid(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.FqdnFormatDomainName);
                    this.Manager.Comment("reaching state \'S868\'");
                    this.Manager.Comment("checking step \'return NetrLogonGetTrustRid/ERROR_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_ACCESS_DENIED, temp188, "return of NetrLogonGetTrustRid, state S868");
                    Test_AuthenticateMessageS1216();
                    goto label53;
                }
                if ((temp190 == 1)) {
                    this.Manager.Comment("reaching state \'S521\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp189;
                    this.Manager.Comment("executing step \'call NetrLogonGetTrustRid(NonDcServer,FqdnFormatDomainName)\'");
                    temp189 = this.INrpcServerAdapterInstance.NetrLogonGetTrustRid(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.FqdnFormatDomainName);
                    this.Manager.Comment("reaching state \'S869\'");
                    this.Manager.Comment("checking step \'return NetrLogonGetTrustRid/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp189, "return of NetrLogonGetTrustRid, state S869");
                    Test_AuthenticateMessageS1217();
                    goto label53;
                }
                throw new InvalidOperationException("never reached");
            label53:
;
                goto label55;
            }
            if ((temp195 == 2)) {
                this.Manager.Comment("reaching state \'S145\'");
                bool temp191;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp191);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp191);
                this.Manager.Comment("reaching state \'S319\'");
                int temp194 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS18GetClientAccountTypeChecker4)), new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS18GetClientAccountTypeChecker5)));
                if ((temp194 == 0)) {
                    this.Manager.Comment("reaching state \'S522\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp192;
                    this.Manager.Comment("executing step \'call NetrLogonComputeServerDigest(PrimaryDc,RidOfNonMachineAccoun" +
                            "t,MessageOne)\'");
                    temp192 = this.INrpcServerAdapterInstance.NetrLogonComputeServerDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.RidType)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103756");
                    this.Manager.Checkpoint("MS-NRPC_R103743");
                    this.Manager.Comment("reaching state \'S870\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeServerDigest/ERROR_NO_SUCH_USER\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SUCH_USER, temp192, "return of NetrLogonComputeServerDigest, state S870");
                    this.Manager.Comment("reaching state \'S1218\'");
                    goto label54;
                }
                if ((temp194 == 1)) {
                    this.Manager.Comment("reaching state \'S523\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp193;
                    this.Manager.Comment("executing step \'call NetrLogonComputeServerDigest(PrimaryDc,RidOfNonMachineAccoun" +
                            "t,MessageOne)\'");
                    temp193 = this.INrpcServerAdapterInstance.NetrLogonComputeServerDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.RidType)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103756");
                    this.Manager.Checkpoint("MS-NRPC_R103743");
                    this.Manager.Comment("reaching state \'S871\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeServerDigest/ERROR_NO_SUCH_USER\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SUCH_USER, temp193, "return of NetrLogonComputeServerDigest, state S871");
                    this.Manager.Comment("reaching state \'S1219\'");
                    goto label54;
                }
                throw new InvalidOperationException("never reached");
            label54:
;
                goto label55;
            }
            throw new InvalidOperationException("never reached");
        label55:
;
            this.Manager.EndTest();
        }
        
        private void Test_AuthenticateMessageS18GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S19");
        }
        
        private void Test_AuthenticateMessageS18GetClientAccountTypeChecker(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S317");
        }
        
        private void Test_AuthenticateMessageS1456() {
            this.Manager.Comment("reaching state \'S1456\'");
            this.Manager.Comment("executing step \'call SwitchUserAccount(True)\'");
            this.INrpcServerAdapterInstance.SwitchUserAccount(true);
            this.Manager.Comment("reaching state \'S1474\'");
            this.Manager.Comment("checking step \'return SwitchUserAccount\'");
            Test_AuthenticateMessageS1284();
        }
        
        private void Test_AuthenticateMessageS18GetClientAccountTypeChecker1(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S317");
        }
        
        private void Test_AuthenticateMessageS1457() {
            this.Manager.Comment("reaching state \'S1457\'");
            this.Manager.Comment("executing step \'call SwitchUserAccount(True)\'");
            this.INrpcServerAdapterInstance.SwitchUserAccount(true);
            this.Manager.Comment("reaching state \'S1475\'");
            this.Manager.Comment("checking step \'return SwitchUserAccount\'");
            Test_AuthenticateMessageS1285();
        }
        
        private void Test_AuthenticateMessageS18GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S19");
        }
        
        private void Test_AuthenticateMessageS18GetClientAccountTypeChecker2(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S318");
        }
        
        private void Test_AuthenticateMessageS18GetClientAccountTypeChecker3(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S318");
        }
        
        private void Test_AuthenticateMessageS18GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S19");
        }
        
        private void Test_AuthenticateMessageS18GetClientAccountTypeChecker4(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S319");
        }
        
        private void Test_AuthenticateMessageS18GetClientAccountTypeChecker5(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S319");
        }
        #endregion
        
        #region Test Starting in S2
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_AuthenticateMessageS2() {
            this.Manager.BeginTest("Test_AuthenticateMessageS2");
            this.Manager.Comment("reaching state \'S2\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp196;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp196);
            this.Manager.AddReturn(GetPlatformInfo, null, temp196);
            this.Manager.Comment("reaching state \'S3\'");
            int temp209 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_AuthenticateMessageS2GetPlatformChecker)), new ExpectedReturn(Test_AuthenticateMessage.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_AuthenticateMessageS2GetPlatformChecker1)), new ExpectedReturn(Test_AuthenticateMessage.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_AuthenticateMessageS2GetPlatformChecker2)));
            if ((temp209 == 0)) {
                this.Manager.Comment("reaching state \'S119\'");
                bool temp197;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp197);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp197);
                this.Manager.Comment("reaching state \'S293\'");
                int temp200 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS2GetClientAccountTypeChecker)), new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS2GetClientAccountTypeChecker1)));
                if ((temp200 == 0)) {
                    this.Manager.Comment("reaching state \'S470\'");
                    this.Manager.Comment("executing step \'call SwitchUserAccount(False)\'");
                    this.INrpcServerAdapterInstance.SwitchUserAccount(false);
                    this.Manager.Comment("reaching state \'S818\'");
                    this.Manager.Comment("checking step \'return SwitchUserAccount\'");
                    this.Manager.Comment("reaching state \'S1166\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp198;
                    this.Manager.Comment("executing step \'call NetrLogonComputeClientDigest(PrimaryDc,Null,MessageOne)\'");
                    temp198 = this.INrpcServerAdapterInstance.NetrLogonComputeClientDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.DomainNameType)(0)), ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103791");
                    this.Manager.Checkpoint("MS-NRPC_R103790");
                    this.Manager.Comment("reaching state \'S1382\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeClientDigest/ERROR_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_ACCESS_DENIED, temp198, "return of NetrLogonComputeClientDigest, state S1382");
                    Test_AuthenticateMessageS1448();
                    goto label56;
                }
                if ((temp200 == 1)) {
                    this.Manager.Comment("reaching state \'S471\'");
                    this.Manager.Comment("executing step \'call SwitchUserAccount(False)\'");
                    this.INrpcServerAdapterInstance.SwitchUserAccount(false);
                    this.Manager.Comment("reaching state \'S819\'");
                    this.Manager.Comment("checking step \'return SwitchUserAccount\'");
                    this.Manager.Comment("reaching state \'S1167\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp199;
                    this.Manager.Comment("executing step \'call NetrLogonComputeClientDigest(PrimaryDc,Null,MessageOne)\'");
                    temp199 = this.INrpcServerAdapterInstance.NetrLogonComputeClientDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.DomainNameType)(0)), ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103791");
                    this.Manager.Checkpoint("MS-NRPC_R103790");
                    this.Manager.Comment("reaching state \'S1383\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeClientDigest/ERROR_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_ACCESS_DENIED, temp199, "return of NetrLogonComputeClientDigest, state S1383");
                    Test_AuthenticateMessageS1449();
                    goto label56;
                }
                throw new InvalidOperationException("never reached");
            label56:
;
                goto label59;
            }
            if ((temp209 == 1)) {
                this.Manager.Comment("reaching state \'S120\'");
                bool temp201;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp201);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp201);
                this.Manager.Comment("reaching state \'S294\'");
                int temp204 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS2GetClientAccountTypeChecker2)), new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS2GetClientAccountTypeChecker3)));
                if ((temp204 == 0)) {
                    this.Manager.Comment("reaching state \'S472\'");
                    this.Manager.Comment("executing step \'call SwitchUserAccount(False)\'");
                    this.INrpcServerAdapterInstance.SwitchUserAccount(false);
                    this.Manager.Comment("reaching state \'S820\'");
                    this.Manager.Comment("checking step \'return SwitchUserAccount\'");
                    this.Manager.Comment("reaching state \'S1168\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp202;
                    this.Manager.Comment("executing step \'call NetrLogonComputeClientDigest(PrimaryDc,Null,MessageOne)\'");
                    temp202 = this.INrpcServerAdapterInstance.NetrLogonComputeClientDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.DomainNameType)(0)), ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103791");
                    this.Manager.Checkpoint("MS-NRPC_R103790");
                    this.Manager.Comment("reaching state \'S1384\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeClientDigest/ERROR_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_ACCESS_DENIED, temp202, "return of NetrLogonComputeClientDigest, state S1384");
                    Test_AuthenticateMessageS1450();
                    goto label57;
                }
                if ((temp204 == 1)) {
                    this.Manager.Comment("reaching state \'S473\'");
                    this.Manager.Comment("executing step \'call SwitchUserAccount(False)\'");
                    this.INrpcServerAdapterInstance.SwitchUserAccount(false);
                    this.Manager.Comment("reaching state \'S821\'");
                    this.Manager.Comment("checking step \'return SwitchUserAccount\'");
                    this.Manager.Comment("reaching state \'S1169\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp203;
                    this.Manager.Comment("executing step \'call NetrLogonComputeClientDigest(PrimaryDc,Null,MessageOne)\'");
                    temp203 = this.INrpcServerAdapterInstance.NetrLogonComputeClientDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.DomainNameType)(0)), ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103791");
                    this.Manager.Checkpoint("MS-NRPC_R103790");
                    this.Manager.Comment("reaching state \'S1385\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeClientDigest/ERROR_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_ACCESS_DENIED, temp203, "return of NetrLogonComputeClientDigest, state S1385");
                    Test_AuthenticateMessageS1451();
                    goto label57;
                }
                throw new InvalidOperationException("never reached");
            label57:
;
                goto label59;
            }
            if ((temp209 == 2)) {
                this.Manager.Comment("reaching state \'S121\'");
                bool temp205;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp205);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp205);
                this.Manager.Comment("reaching state \'S295\'");
                int temp208 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS2GetClientAccountTypeChecker4)), new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS2GetClientAccountTypeChecker5)));
                if ((temp208 == 0)) {
                    this.Manager.Comment("reaching state \'S474\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp206;
                    this.Manager.Comment("executing step \'call NetrLogonComputeServerDigest(PrimaryDc,RidOfNonMachineAccoun" +
                            "t,MessageOne)\'");
                    temp206 = this.INrpcServerAdapterInstance.NetrLogonComputeServerDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.RidType)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103756");
                    this.Manager.Checkpoint("MS-NRPC_R103743");
                    this.Manager.Comment("reaching state \'S822\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeServerDigest/ERROR_NO_SUCH_USER\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SUCH_USER, temp206, "return of NetrLogonComputeServerDigest, state S822");
                    this.Manager.Comment("reaching state \'S1170\'");
                    goto label58;
                }
                if ((temp208 == 1)) {
                    this.Manager.Comment("reaching state \'S475\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp207;
                    this.Manager.Comment("executing step \'call NetrLogonComputeServerDigest(PrimaryDc,RidOfNonMachineAccoun" +
                            "t,MessageOne)\'");
                    temp207 = this.INrpcServerAdapterInstance.NetrLogonComputeServerDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.RidType)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103756");
                    this.Manager.Checkpoint("MS-NRPC_R103743");
                    this.Manager.Comment("reaching state \'S823\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeServerDigest/ERROR_NO_SUCH_USER\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SUCH_USER, temp207, "return of NetrLogonComputeServerDigest, state S823");
                    this.Manager.Comment("reaching state \'S1171\'");
                    goto label58;
                }
                throw new InvalidOperationException("never reached");
            label58:
;
                goto label59;
            }
            throw new InvalidOperationException("never reached");
        label59:
;
            this.Manager.EndTest();
        }
        
        private void Test_AuthenticateMessageS2GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S3");
        }
        
        private void Test_AuthenticateMessageS2GetClientAccountTypeChecker(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S293");
        }
        
        private void Test_AuthenticateMessageS2GetClientAccountTypeChecker1(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S293");
        }
        
        private void Test_AuthenticateMessageS2GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S3");
        }
        
        private void Test_AuthenticateMessageS2GetClientAccountTypeChecker2(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S294");
        }
        
        private void Test_AuthenticateMessageS2GetClientAccountTypeChecker3(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S294");
        }
        
        private void Test_AuthenticateMessageS2GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S3");
        }
        
        private void Test_AuthenticateMessageS2GetClientAccountTypeChecker4(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S295");
        }
        
        private void Test_AuthenticateMessageS2GetClientAccountTypeChecker5(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S295");
        }
        #endregion
        
        #region Test Starting in S20
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_AuthenticateMessageS20() {
            this.Manager.BeginTest("Test_AuthenticateMessageS20");
            this.Manager.Comment("reaching state \'S20\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp210;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp210);
            this.Manager.AddReturn(GetPlatformInfo, null, temp210);
            this.Manager.Comment("reaching state \'S21\'");
            int temp223 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_AuthenticateMessageS20GetPlatformChecker)), new ExpectedReturn(Test_AuthenticateMessage.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_AuthenticateMessageS20GetPlatformChecker1)), new ExpectedReturn(Test_AuthenticateMessage.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_AuthenticateMessageS20GetPlatformChecker2)));
            if ((temp223 == 0)) {
                this.Manager.Comment("reaching state \'S146\'");
                bool temp211;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp211);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp211);
                this.Manager.Comment("reaching state \'S320\'");
                int temp214 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS20GetClientAccountTypeChecker)), new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS20GetClientAccountTypeChecker1)));
                if ((temp214 == 0)) {
                    this.Manager.Comment("reaching state \'S524\'");
                    this.Manager.Comment("executing step \'call SwitchUserAccount(False)\'");
                    this.INrpcServerAdapterInstance.SwitchUserAccount(false);
                    this.Manager.Comment("reaching state \'S872\'");
                    this.Manager.Comment("checking step \'return SwitchUserAccount\'");
                    this.Manager.Comment("reaching state \'S1220\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp212;
                    this.Manager.Comment("executing step \'call NetrLogonComputeClientDigest(PrimaryDc,EmptyDomainName,Messa" +
                            "geOne)\'");
                    temp212 = this.INrpcServerAdapterInstance.NetrLogonComputeClientDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.DomainNameType)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103791");
                    this.Manager.Checkpoint("MS-NRPC_R103790");
                    this.Manager.Comment("reaching state \'S1416\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeClientDigest/ERROR_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_ACCESS_DENIED, temp212, "return of NetrLogonComputeClientDigest, state S1416");
                    Test_AuthenticateMessageS1456();
                    goto label60;
                }
                if ((temp214 == 1)) {
                    this.Manager.Comment("reaching state \'S525\'");
                    this.Manager.Comment("executing step \'call SwitchUserAccount(False)\'");
                    this.INrpcServerAdapterInstance.SwitchUserAccount(false);
                    this.Manager.Comment("reaching state \'S873\'");
                    this.Manager.Comment("checking step \'return SwitchUserAccount\'");
                    this.Manager.Comment("reaching state \'S1221\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp213;
                    this.Manager.Comment("executing step \'call NetrLogonComputeClientDigest(PrimaryDc,EmptyDomainName,Messa" +
                            "geOne)\'");
                    temp213 = this.INrpcServerAdapterInstance.NetrLogonComputeClientDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.DomainNameType)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103791");
                    this.Manager.Checkpoint("MS-NRPC_R103790");
                    this.Manager.Comment("reaching state \'S1417\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeClientDigest/ERROR_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_ACCESS_DENIED, temp213, "return of NetrLogonComputeClientDigest, state S1417");
                    Test_AuthenticateMessageS1457();
                    goto label60;
                }
                throw new InvalidOperationException("never reached");
            label60:
;
                goto label63;
            }
            if ((temp223 == 1)) {
                this.Manager.Comment("reaching state \'S147\'");
                bool temp215;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp215);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp215);
                this.Manager.Comment("reaching state \'S321\'");
                int temp218 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS20GetClientAccountTypeChecker2)), new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS20GetClientAccountTypeChecker3)));
                if ((temp218 == 0)) {
                    this.Manager.Comment("reaching state \'S526\'");
                    this.Manager.Comment("executing step \'call SwitchUserAccount(False)\'");
                    this.INrpcServerAdapterInstance.SwitchUserAccount(false);
                    this.Manager.Comment("reaching state \'S874\'");
                    this.Manager.Comment("checking step \'return SwitchUserAccount\'");
                    this.Manager.Comment("reaching state \'S1222\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp216;
                    this.Manager.Comment("executing step \'call NetrLogonComputeClientDigest(PrimaryDc,TrustedDomainName,Mes" +
                            "sageOne)\'");
                    temp216 = this.INrpcServerAdapterInstance.NetrLogonComputeClientDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.TrustedDomainName, ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103791");
                    this.Manager.Checkpoint("MS-NRPC_R103790");
                    this.Manager.Comment("reaching state \'S1418\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeClientDigest/ERROR_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_ACCESS_DENIED, temp216, "return of NetrLogonComputeClientDigest, state S1418");
                    Test_AuthenticateMessageS1448();
                    goto label61;
                }
                if ((temp218 == 1)) {
                    this.Manager.Comment("reaching state \'S527\'");
                    this.Manager.Comment("executing step \'call SwitchUserAccount(False)\'");
                    this.INrpcServerAdapterInstance.SwitchUserAccount(false);
                    this.Manager.Comment("reaching state \'S875\'");
                    this.Manager.Comment("checking step \'return SwitchUserAccount\'");
                    this.Manager.Comment("reaching state \'S1223\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp217;
                    this.Manager.Comment("executing step \'call NetrLogonComputeClientDigest(PrimaryDc,TrustedDomainName,Mes" +
                            "sageOne)\'");
                    temp217 = this.INrpcServerAdapterInstance.NetrLogonComputeClientDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.TrustedDomainName, ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103791");
                    this.Manager.Checkpoint("MS-NRPC_R103790");
                    this.Manager.Comment("reaching state \'S1419\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeClientDigest/ERROR_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_ACCESS_DENIED, temp217, "return of NetrLogonComputeClientDigest, state S1419");
                    Test_AuthenticateMessageS1449();
                    goto label61;
                }
                throw new InvalidOperationException("never reached");
            label61:
;
                goto label63;
            }
            if ((temp223 == 2)) {
                this.Manager.Comment("reaching state \'S148\'");
                bool temp219;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp219);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp219);
                this.Manager.Comment("reaching state \'S322\'");
                int temp222 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS20GetClientAccountTypeChecker4)), new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS20GetClientAccountTypeChecker5)));
                if ((temp222 == 0)) {
                    this.Manager.Comment("reaching state \'S528\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp220;
                    this.Manager.Comment("executing step \'call NetrLogonComputeServerDigest(PrimaryDc,RidOfNonMachineAccoun" +
                            "t,MessageOne)\'");
                    temp220 = this.INrpcServerAdapterInstance.NetrLogonComputeServerDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.RidType)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103756");
                    this.Manager.Checkpoint("MS-NRPC_R103743");
                    this.Manager.Comment("reaching state \'S876\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeServerDigest/ERROR_NO_SUCH_USER\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SUCH_USER, temp220, "return of NetrLogonComputeServerDigest, state S876");
                    this.Manager.Comment("reaching state \'S1224\'");
                    goto label62;
                }
                if ((temp222 == 1)) {
                    this.Manager.Comment("reaching state \'S529\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp221;
                    this.Manager.Comment("executing step \'call NetrLogonComputeServerDigest(PrimaryDc,RidOfNonMachineAccoun" +
                            "t,MessageOne)\'");
                    temp221 = this.INrpcServerAdapterInstance.NetrLogonComputeServerDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.RidType)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103756");
                    this.Manager.Checkpoint("MS-NRPC_R103743");
                    this.Manager.Comment("reaching state \'S877\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeServerDigest/ERROR_NO_SUCH_USER\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SUCH_USER, temp221, "return of NetrLogonComputeServerDigest, state S877");
                    this.Manager.Comment("reaching state \'S1225\'");
                    goto label62;
                }
                throw new InvalidOperationException("never reached");
            label62:
;
                goto label63;
            }
            throw new InvalidOperationException("never reached");
        label63:
;
            this.Manager.EndTest();
        }
        
        private void Test_AuthenticateMessageS20GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S21");
        }
        
        private void Test_AuthenticateMessageS20GetClientAccountTypeChecker(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S320");
        }
        
        private void Test_AuthenticateMessageS20GetClientAccountTypeChecker1(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S320");
        }
        
        private void Test_AuthenticateMessageS20GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S21");
        }
        
        private void Test_AuthenticateMessageS20GetClientAccountTypeChecker2(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S321");
        }
        
        private void Test_AuthenticateMessageS20GetClientAccountTypeChecker3(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S321");
        }
        
        private void Test_AuthenticateMessageS20GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S21");
        }
        
        private void Test_AuthenticateMessageS20GetClientAccountTypeChecker4(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S322");
        }
        
        private void Test_AuthenticateMessageS20GetClientAccountTypeChecker5(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S322");
        }
        #endregion
        
        #region Test Starting in S22
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_AuthenticateMessageS22() {
            this.Manager.BeginTest("Test_AuthenticateMessageS22");
            this.Manager.Comment("reaching state \'S22\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp224;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp224);
            this.Manager.AddReturn(GetPlatformInfo, null, temp224);
            this.Manager.Comment("reaching state \'S23\'");
            int temp237 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_AuthenticateMessageS22GetPlatformChecker)), new ExpectedReturn(Test_AuthenticateMessage.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_AuthenticateMessageS22GetPlatformChecker1)), new ExpectedReturn(Test_AuthenticateMessage.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_AuthenticateMessageS22GetPlatformChecker2)));
            if ((temp237 == 0)) {
                this.Manager.Comment("reaching state \'S149\'");
                bool temp225;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp225);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp225);
                this.Manager.Comment("reaching state \'S323\'");
                int temp228 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS22GetClientAccountTypeChecker)), new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS22GetClientAccountTypeChecker1)));
                if ((temp228 == 0)) {
                    this.Manager.Comment("reaching state \'S530\'");
                    this.Manager.Comment("executing step \'call SwitchUserAccount(False)\'");
                    this.INrpcServerAdapterInstance.SwitchUserAccount(false);
                    this.Manager.Comment("reaching state \'S878\'");
                    this.Manager.Comment("checking step \'return SwitchUserAccount\'");
                    this.Manager.Comment("reaching state \'S1226\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp226;
                    this.Manager.Comment("executing step \'call NetrLogonComputeClientDigest(PrimaryDc,InvalidDomainName,Mes" +
                            "sageOne)\'");
                    temp226 = this.INrpcServerAdapterInstance.NetrLogonComputeClientDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.InvalidDomainName, ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103791");
                    this.Manager.Checkpoint("MS-NRPC_R103790");
                    this.Manager.Comment("reaching state \'S1420\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeClientDigest/ERROR_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_ACCESS_DENIED, temp226, "return of NetrLogonComputeClientDigest, state S1420");
                    Test_AuthenticateMessageS1456();
                    goto label64;
                }
                if ((temp228 == 1)) {
                    this.Manager.Comment("reaching state \'S531\'");
                    this.Manager.Comment("executing step \'call SwitchUserAccount(False)\'");
                    this.INrpcServerAdapterInstance.SwitchUserAccount(false);
                    this.Manager.Comment("reaching state \'S879\'");
                    this.Manager.Comment("checking step \'return SwitchUserAccount\'");
                    this.Manager.Comment("reaching state \'S1227\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp227;
                    this.Manager.Comment("executing step \'call NetrLogonComputeClientDigest(PrimaryDc,InvalidDomainName,Mes" +
                            "sageOne)\'");
                    temp227 = this.INrpcServerAdapterInstance.NetrLogonComputeClientDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.InvalidDomainName, ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103791");
                    this.Manager.Checkpoint("MS-NRPC_R103790");
                    this.Manager.Comment("reaching state \'S1421\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeClientDigest/ERROR_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_ACCESS_DENIED, temp227, "return of NetrLogonComputeClientDigest, state S1421");
                    Test_AuthenticateMessageS1457();
                    goto label64;
                }
                throw new InvalidOperationException("never reached");
            label64:
;
                goto label67;
            }
            if ((temp237 == 1)) {
                this.Manager.Comment("reaching state \'S150\'");
                bool temp229;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp229);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp229);
                this.Manager.Comment("reaching state \'S324\'");
                int temp232 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS22GetClientAccountTypeChecker2)), new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS22GetClientAccountTypeChecker3)));
                if ((temp232 == 0)) {
                    this.Manager.Comment("reaching state \'S532\'");
                    this.Manager.Comment("executing step \'call SwitchUserAccount(False)\'");
                    this.INrpcServerAdapterInstance.SwitchUserAccount(false);
                    this.Manager.Comment("reaching state \'S880\'");
                    this.Manager.Comment("checking step \'return SwitchUserAccount\'");
                    this.Manager.Comment("reaching state \'S1228\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp230;
                    this.Manager.Comment("executing step \'call NetrLogonComputeServerDigest(PrimaryDc,RidOfNonDcMachineAcco" +
                            "unt,MessageOne)\'");
                    temp230 = this.INrpcServerAdapterInstance.NetrLogonComputeServerDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.RidType.RidOfNonDcMachineAccount, ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103746");
                    this.Manager.Checkpoint("MS-NRPC_R103743");
                    this.Manager.Comment("reaching state \'S1422\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeServerDigest/ERROR_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_ACCESS_DENIED, temp230, "return of NetrLogonComputeServerDigest, state S1422");
                    Test_AuthenticateMessageS1452();
                    goto label65;
                }
                if ((temp232 == 1)) {
                    this.Manager.Comment("reaching state \'S533\'");
                    this.Manager.Comment("executing step \'call SwitchUserAccount(False)\'");
                    this.INrpcServerAdapterInstance.SwitchUserAccount(false);
                    this.Manager.Comment("reaching state \'S881\'");
                    this.Manager.Comment("checking step \'return SwitchUserAccount\'");
                    this.Manager.Comment("reaching state \'S1229\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp231;
                    this.Manager.Comment("executing step \'call NetrLogonComputeServerDigest(PrimaryDc,RidOfNonDcMachineAcco" +
                            "unt,MessageOne)\'");
                    temp231 = this.INrpcServerAdapterInstance.NetrLogonComputeServerDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.RidType.RidOfNonDcMachineAccount, ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103746");
                    this.Manager.Checkpoint("MS-NRPC_R103743");
                    this.Manager.Comment("reaching state \'S1423\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeServerDigest/ERROR_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_ACCESS_DENIED, temp231, "return of NetrLogonComputeServerDigest, state S1423");
                    Test_AuthenticateMessageS1453();
                    goto label65;
                }
                throw new InvalidOperationException("never reached");
            label65:
;
                goto label67;
            }
            if ((temp237 == 2)) {
                this.Manager.Comment("reaching state \'S151\'");
                bool temp233;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp233);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp233);
                this.Manager.Comment("reaching state \'S325\'");
                int temp236 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS22GetClientAccountTypeChecker4)), new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS22GetClientAccountTypeChecker5)));
                if ((temp236 == 0)) {
                    this.Manager.Comment("reaching state \'S534\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp234;
                    this.Manager.Comment("executing step \'call NetrLogonComputeServerDigest(PrimaryDc,RidOfNonMachineAccoun" +
                            "t,MessageOne)\'");
                    temp234 = this.INrpcServerAdapterInstance.NetrLogonComputeServerDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.RidType)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103756");
                    this.Manager.Checkpoint("MS-NRPC_R103743");
                    this.Manager.Comment("reaching state \'S882\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeServerDigest/ERROR_NO_SUCH_USER\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SUCH_USER, temp234, "return of NetrLogonComputeServerDigest, state S882");
                    this.Manager.Comment("reaching state \'S1230\'");
                    goto label66;
                }
                if ((temp236 == 1)) {
                    this.Manager.Comment("reaching state \'S535\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp235;
                    this.Manager.Comment("executing step \'call NetrLogonComputeServerDigest(PrimaryDc,RidOfNonMachineAccoun" +
                            "t,MessageOne)\'");
                    temp235 = this.INrpcServerAdapterInstance.NetrLogonComputeServerDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.RidType)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103756");
                    this.Manager.Checkpoint("MS-NRPC_R103743");
                    this.Manager.Comment("reaching state \'S883\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeServerDigest/ERROR_NO_SUCH_USER\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SUCH_USER, temp235, "return of NetrLogonComputeServerDigest, state S883");
                    this.Manager.Comment("reaching state \'S1231\'");
                    goto label66;
                }
                throw new InvalidOperationException("never reached");
            label66:
;
                goto label67;
            }
            throw new InvalidOperationException("never reached");
        label67:
;
            this.Manager.EndTest();
        }
        
        private void Test_AuthenticateMessageS22GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S23");
        }
        
        private void Test_AuthenticateMessageS22GetClientAccountTypeChecker(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S323");
        }
        
        private void Test_AuthenticateMessageS22GetClientAccountTypeChecker1(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S323");
        }
        
        private void Test_AuthenticateMessageS22GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S23");
        }
        
        private void Test_AuthenticateMessageS22GetClientAccountTypeChecker2(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S324");
        }
        
        private void Test_AuthenticateMessageS22GetClientAccountTypeChecker3(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S324");
        }
        
        private void Test_AuthenticateMessageS22GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S23");
        }
        
        private void Test_AuthenticateMessageS22GetClientAccountTypeChecker4(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S325");
        }
        
        private void Test_AuthenticateMessageS22GetClientAccountTypeChecker5(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S325");
        }
        #endregion
        
        #region Test Starting in S24
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_AuthenticateMessageS24() {
            this.Manager.BeginTest("Test_AuthenticateMessageS24");
            this.Manager.Comment("reaching state \'S24\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp238;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp238);
            this.Manager.AddReturn(GetPlatformInfo, null, temp238);
            this.Manager.Comment("reaching state \'S25\'");
            int temp251 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_AuthenticateMessageS24GetPlatformChecker)), new ExpectedReturn(Test_AuthenticateMessage.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_AuthenticateMessageS24GetPlatformChecker1)), new ExpectedReturn(Test_AuthenticateMessage.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_AuthenticateMessageS24GetPlatformChecker2)));
            if ((temp251 == 0)) {
                this.Manager.Comment("reaching state \'S152\'");
                bool temp239;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp239);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp239);
                this.Manager.Comment("reaching state \'S326\'");
                int temp242 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS24GetClientAccountTypeChecker)), new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS24GetClientAccountTypeChecker1)));
                if ((temp242 == 0)) {
                    this.Manager.Comment("reaching state \'S536\'");
                    this.Manager.Comment("executing step \'call SwitchUserAccount(False)\'");
                    this.INrpcServerAdapterInstance.SwitchUserAccount(false);
                    this.Manager.Comment("reaching state \'S884\'");
                    this.Manager.Comment("checking step \'return SwitchUserAccount\'");
                    this.Manager.Comment("reaching state \'S1232\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp240;
                    this.Manager.Comment("executing step \'call NetrLogonComputeClientDigest(PrimaryDc,InvalidFormatDomainNa" +
                            "me,MessageOne)\'");
                    temp240 = this.INrpcServerAdapterInstance.NetrLogonComputeClientDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.InvalidFormatDomainName, ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103791");
                    this.Manager.Checkpoint("MS-NRPC_R103790");
                    this.Manager.Comment("reaching state \'S1424\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeClientDigest/ERROR_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_ACCESS_DENIED, temp240, "return of NetrLogonComputeClientDigest, state S1424");
                    Test_AuthenticateMessageS1456();
                    goto label68;
                }
                if ((temp242 == 1)) {
                    this.Manager.Comment("reaching state \'S537\'");
                    this.Manager.Comment("executing step \'call SwitchUserAccount(False)\'");
                    this.INrpcServerAdapterInstance.SwitchUserAccount(false);
                    this.Manager.Comment("reaching state \'S885\'");
                    this.Manager.Comment("checking step \'return SwitchUserAccount\'");
                    this.Manager.Comment("reaching state \'S1233\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp241;
                    this.Manager.Comment("executing step \'call NetrLogonComputeClientDigest(PrimaryDc,InvalidFormatDomainNa" +
                            "me,MessageOne)\'");
                    temp241 = this.INrpcServerAdapterInstance.NetrLogonComputeClientDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.InvalidFormatDomainName, ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103791");
                    this.Manager.Checkpoint("MS-NRPC_R103790");
                    this.Manager.Comment("reaching state \'S1425\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeClientDigest/ERROR_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_ACCESS_DENIED, temp241, "return of NetrLogonComputeClientDigest, state S1425");
                    Test_AuthenticateMessageS1457();
                    goto label68;
                }
                throw new InvalidOperationException("never reached");
            label68:
;
                goto label71;
            }
            if ((temp251 == 1)) {
                this.Manager.Comment("reaching state \'S153\'");
                bool temp243;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp243);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp243);
                this.Manager.Comment("reaching state \'S327\'");
                int temp246 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS24GetClientAccountTypeChecker2)), new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS24GetClientAccountTypeChecker3)));
                if ((temp246 == 0)) {
                    this.Manager.Comment("reaching state \'S538\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp244;
                    this.Manager.Comment("executing step \'call NetrLogonGetTrustRid(PrimaryDc,Null)\'");
                    temp244 = this.INrpcServerAdapterInstance.NetrLogonGetTrustRid(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.DomainNameType)(0)));
                    this.Manager.Comment("reaching state \'S886\'");
                    this.Manager.Comment("checking step \'return NetrLogonGetTrustRid/ERROR_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_ACCESS_DENIED, temp244, "return of NetrLogonGetTrustRid, state S886");
                    Test_AuthenticateMessageS1216();
                    goto label69;
                }
                if ((temp246 == 1)) {
                    this.Manager.Comment("reaching state \'S539\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp245;
                    this.Manager.Comment("executing step \'call NetrLogonGetTrustRid(PrimaryDc,Null)\'");
                    temp245 = this.INrpcServerAdapterInstance.NetrLogonGetTrustRid(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.DomainNameType)(0)));
                    this.Manager.Comment("reaching state \'S887\'");
                    this.Manager.Comment("checking step \'return NetrLogonGetTrustRid/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp245, "return of NetrLogonGetTrustRid, state S887");
                    Test_AuthenticateMessageS1217();
                    goto label69;
                }
                throw new InvalidOperationException("never reached");
            label69:
;
                goto label71;
            }
            if ((temp251 == 2)) {
                this.Manager.Comment("reaching state \'S154\'");
                bool temp247;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp247);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp247);
                this.Manager.Comment("reaching state \'S328\'");
                int temp250 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS24GetClientAccountTypeChecker4)), new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS24GetClientAccountTypeChecker5)));
                if ((temp250 == 0)) {
                    this.Manager.Comment("reaching state \'S540\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp248;
                    this.Manager.Comment("executing step \'call NetrLogonComputeServerDigest(PrimaryDc,RidOfNonMachineAccoun" +
                            "t,MessageOne)\'");
                    temp248 = this.INrpcServerAdapterInstance.NetrLogonComputeServerDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.RidType)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103756");
                    this.Manager.Checkpoint("MS-NRPC_R103743");
                    this.Manager.Comment("reaching state \'S888\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeServerDigest/ERROR_NO_SUCH_USER\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SUCH_USER, temp248, "return of NetrLogonComputeServerDigest, state S888");
                    this.Manager.Comment("reaching state \'S1234\'");
                    goto label70;
                }
                if ((temp250 == 1)) {
                    this.Manager.Comment("reaching state \'S541\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp249;
                    this.Manager.Comment("executing step \'call NetrLogonComputeServerDigest(PrimaryDc,RidOfNonMachineAccoun" +
                            "t,MessageOne)\'");
                    temp249 = this.INrpcServerAdapterInstance.NetrLogonComputeServerDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.RidType)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103756");
                    this.Manager.Checkpoint("MS-NRPC_R103743");
                    this.Manager.Comment("reaching state \'S889\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeServerDigest/ERROR_NO_SUCH_USER\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SUCH_USER, temp249, "return of NetrLogonComputeServerDigest, state S889");
                    this.Manager.Comment("reaching state \'S1235\'");
                    goto label70;
                }
                throw new InvalidOperationException("never reached");
            label70:
;
                goto label71;
            }
            throw new InvalidOperationException("never reached");
        label71:
;
            this.Manager.EndTest();
        }
        
        private void Test_AuthenticateMessageS24GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S25");
        }
        
        private void Test_AuthenticateMessageS24GetClientAccountTypeChecker(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S326");
        }
        
        private void Test_AuthenticateMessageS24GetClientAccountTypeChecker1(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S326");
        }
        
        private void Test_AuthenticateMessageS24GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S25");
        }
        
        private void Test_AuthenticateMessageS24GetClientAccountTypeChecker2(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S327");
        }
        
        private void Test_AuthenticateMessageS24GetClientAccountTypeChecker3(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S327");
        }
        
        private void Test_AuthenticateMessageS24GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S25");
        }
        
        private void Test_AuthenticateMessageS24GetClientAccountTypeChecker4(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S328");
        }
        
        private void Test_AuthenticateMessageS24GetClientAccountTypeChecker5(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S328");
        }
        #endregion
        
        #region Test Starting in S26
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_AuthenticateMessageS26() {
            this.Manager.BeginTest("Test_AuthenticateMessageS26");
            this.Manager.Comment("reaching state \'S26\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp252;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp252);
            this.Manager.AddReturn(GetPlatformInfo, null, temp252);
            this.Manager.Comment("reaching state \'S27\'");
            int temp265 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_AuthenticateMessageS26GetPlatformChecker)), new ExpectedReturn(Test_AuthenticateMessage.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_AuthenticateMessageS26GetPlatformChecker1)), new ExpectedReturn(Test_AuthenticateMessage.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_AuthenticateMessageS26GetPlatformChecker2)));
            if ((temp265 == 0)) {
                this.Manager.Comment("reaching state \'S155\'");
                bool temp253;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp253);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp253);
                this.Manager.Comment("reaching state \'S329\'");
                int temp256 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS26GetClientAccountTypeChecker)), new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS26GetClientAccountTypeChecker1)));
                if ((temp256 == 0)) {
                    this.Manager.Comment("reaching state \'S542\'");
                    this.Manager.Comment("executing step \'call SwitchUserAccount(False)\'");
                    this.INrpcServerAdapterInstance.SwitchUserAccount(false);
                    this.Manager.Comment("reaching state \'S890\'");
                    this.Manager.Comment("checking step \'return SwitchUserAccount\'");
                    this.Manager.Comment("reaching state \'S1236\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp254;
                    this.Manager.Comment("executing step \'call NetrLogonComputeClientDigest(PrimaryDc,NetBiosFormatDomainNa" +
                            "me,MessageOne)\'");
                    temp254 = this.INrpcServerAdapterInstance.NetrLogonComputeClientDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.NetBiosFormatDomainName, ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103791");
                    this.Manager.Checkpoint("MS-NRPC_R103790");
                    this.Manager.Comment("reaching state \'S1426\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeClientDigest/ERROR_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_ACCESS_DENIED, temp254, "return of NetrLogonComputeClientDigest, state S1426");
                    Test_AuthenticateMessageS1456();
                    goto label72;
                }
                if ((temp256 == 1)) {
                    this.Manager.Comment("reaching state \'S543\'");
                    this.Manager.Comment("executing step \'call SwitchUserAccount(False)\'");
                    this.INrpcServerAdapterInstance.SwitchUserAccount(false);
                    this.Manager.Comment("reaching state \'S891\'");
                    this.Manager.Comment("checking step \'return SwitchUserAccount\'");
                    this.Manager.Comment("reaching state \'S1237\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp255;
                    this.Manager.Comment("executing step \'call NetrLogonComputeClientDigest(PrimaryDc,NetBiosFormatDomainNa" +
                            "me,MessageOne)\'");
                    temp255 = this.INrpcServerAdapterInstance.NetrLogonComputeClientDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.NetBiosFormatDomainName, ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103791");
                    this.Manager.Checkpoint("MS-NRPC_R103790");
                    this.Manager.Comment("reaching state \'S1427\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeClientDigest/ERROR_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_ACCESS_DENIED, temp255, "return of NetrLogonComputeClientDigest, state S1427");
                    Test_AuthenticateMessageS1457();
                    goto label72;
                }
                throw new InvalidOperationException("never reached");
            label72:
;
                goto label75;
            }
            if ((temp265 == 1)) {
                this.Manager.Comment("reaching state \'S156\'");
                bool temp257;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp257);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp257);
                this.Manager.Comment("reaching state \'S330\'");
                int temp260 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS26GetClientAccountTypeChecker2)), new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS26GetClientAccountTypeChecker3)));
                if ((temp260 == 0)) {
                    this.Manager.Comment("reaching state \'S544\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp258;
                    this.Manager.Comment("executing step \'call NetrLogonGetTrustRid(PrimaryDc,EmptyDomainName)\'");
                    temp258 = this.INrpcServerAdapterInstance.NetrLogonGetTrustRid(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.DomainNameType)(1)));
                    this.Manager.Comment("reaching state \'S892\'");
                    this.Manager.Comment("checking step \'return NetrLogonGetTrustRid/ERROR_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_ACCESS_DENIED, temp258, "return of NetrLogonGetTrustRid, state S892");
                    Test_AuthenticateMessageS1216();
                    goto label73;
                }
                if ((temp260 == 1)) {
                    this.Manager.Comment("reaching state \'S545\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp259;
                    this.Manager.Comment("executing step \'call NetrLogonGetTrustRid(PrimaryDc,EmptyDomainName)\'");
                    temp259 = this.INrpcServerAdapterInstance.NetrLogonGetTrustRid(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.DomainNameType)(1)));
                    this.Manager.Comment("reaching state \'S893\'");
                    this.Manager.Comment("checking step \'return NetrLogonGetTrustRid/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp259, "return of NetrLogonGetTrustRid, state S893");
                    Test_AuthenticateMessageS1217();
                    goto label73;
                }
                throw new InvalidOperationException("never reached");
            label73:
;
                goto label75;
            }
            if ((temp265 == 2)) {
                this.Manager.Comment("reaching state \'S157\'");
                bool temp261;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp261);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp261);
                this.Manager.Comment("reaching state \'S331\'");
                int temp264 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS26GetClientAccountTypeChecker4)), new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS26GetClientAccountTypeChecker5)));
                if ((temp264 == 0)) {
                    this.Manager.Comment("reaching state \'S546\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp262;
                    this.Manager.Comment("executing step \'call NetrLogonComputeServerDigest(PrimaryDc,RidOfNonMachineAccoun" +
                            "t,MessageOne)\'");
                    temp262 = this.INrpcServerAdapterInstance.NetrLogonComputeServerDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.RidType)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103756");
                    this.Manager.Checkpoint("MS-NRPC_R103743");
                    this.Manager.Comment("reaching state \'S894\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeServerDigest/ERROR_NO_SUCH_USER\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SUCH_USER, temp262, "return of NetrLogonComputeServerDigest, state S894");
                    this.Manager.Comment("reaching state \'S1238\'");
                    goto label74;
                }
                if ((temp264 == 1)) {
                    this.Manager.Comment("reaching state \'S547\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp263;
                    this.Manager.Comment("executing step \'call NetrLogonComputeServerDigest(PrimaryDc,RidOfNonMachineAccoun" +
                            "t,MessageOne)\'");
                    temp263 = this.INrpcServerAdapterInstance.NetrLogonComputeServerDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.RidType)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103756");
                    this.Manager.Checkpoint("MS-NRPC_R103743");
                    this.Manager.Comment("reaching state \'S895\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeServerDigest/ERROR_NO_SUCH_USER\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SUCH_USER, temp263, "return of NetrLogonComputeServerDigest, state S895");
                    this.Manager.Comment("reaching state \'S1239\'");
                    goto label74;
                }
                throw new InvalidOperationException("never reached");
            label74:
;
                goto label75;
            }
            throw new InvalidOperationException("never reached");
        label75:
;
            this.Manager.EndTest();
        }
        
        private void Test_AuthenticateMessageS26GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S27");
        }
        
        private void Test_AuthenticateMessageS26GetClientAccountTypeChecker(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S329");
        }
        
        private void Test_AuthenticateMessageS26GetClientAccountTypeChecker1(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S329");
        }
        
        private void Test_AuthenticateMessageS26GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S27");
        }
        
        private void Test_AuthenticateMessageS26GetClientAccountTypeChecker2(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S330");
        }
        
        private void Test_AuthenticateMessageS26GetClientAccountTypeChecker3(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S330");
        }
        
        private void Test_AuthenticateMessageS26GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S27");
        }
        
        private void Test_AuthenticateMessageS26GetClientAccountTypeChecker4(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S331");
        }
        
        private void Test_AuthenticateMessageS26GetClientAccountTypeChecker5(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S331");
        }
        #endregion
        
        #region Test Starting in S28
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_AuthenticateMessageS28() {
            this.Manager.BeginTest("Test_AuthenticateMessageS28");
            this.Manager.Comment("reaching state \'S28\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp266;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp266);
            this.Manager.AddReturn(GetPlatformInfo, null, temp266);
            this.Manager.Comment("reaching state \'S29\'");
            int temp279 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_AuthenticateMessageS28GetPlatformChecker)), new ExpectedReturn(Test_AuthenticateMessage.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_AuthenticateMessageS28GetPlatformChecker1)), new ExpectedReturn(Test_AuthenticateMessage.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_AuthenticateMessageS28GetPlatformChecker2)));
            if ((temp279 == 0)) {
                this.Manager.Comment("reaching state \'S158\'");
                bool temp267;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp267);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp267);
                this.Manager.Comment("reaching state \'S332\'");
                int temp270 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS28GetClientAccountTypeChecker)), new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS28GetClientAccountTypeChecker1)));
                if ((temp270 == 0)) {
                    this.Manager.Comment("reaching state \'S548\'");
                    this.Manager.Comment("executing step \'call SwitchUserAccount(False)\'");
                    this.INrpcServerAdapterInstance.SwitchUserAccount(false);
                    this.Manager.Comment("reaching state \'S896\'");
                    this.Manager.Comment("checking step \'return SwitchUserAccount\'");
                    this.Manager.Comment("reaching state \'S1240\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp268;
                    this.Manager.Comment("executing step \'call NetrLogonComputeClientDigest(PrimaryDc,FqdnFormatDomainName," +
                            "MessageOne)\'");
                    temp268 = this.INrpcServerAdapterInstance.NetrLogonComputeClientDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.FqdnFormatDomainName, ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103791");
                    this.Manager.Checkpoint("MS-NRPC_R103790");
                    this.Manager.Comment("reaching state \'S1428\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeClientDigest/ERROR_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_ACCESS_DENIED, temp268, "return of NetrLogonComputeClientDigest, state S1428");
                    Test_AuthenticateMessageS1456();
                    goto label76;
                }
                if ((temp270 == 1)) {
                    this.Manager.Comment("reaching state \'S549\'");
                    this.Manager.Comment("executing step \'call SwitchUserAccount(False)\'");
                    this.INrpcServerAdapterInstance.SwitchUserAccount(false);
                    this.Manager.Comment("reaching state \'S897\'");
                    this.Manager.Comment("checking step \'return SwitchUserAccount\'");
                    this.Manager.Comment("reaching state \'S1241\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp269;
                    this.Manager.Comment("executing step \'call NetrLogonComputeClientDigest(PrimaryDc,FqdnFormatDomainName," +
                            "MessageOne)\'");
                    temp269 = this.INrpcServerAdapterInstance.NetrLogonComputeClientDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.FqdnFormatDomainName, ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103791");
                    this.Manager.Checkpoint("MS-NRPC_R103790");
                    this.Manager.Comment("reaching state \'S1429\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeClientDigest/ERROR_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_ACCESS_DENIED, temp269, "return of NetrLogonComputeClientDigest, state S1429");
                    Test_AuthenticateMessageS1457();
                    goto label76;
                }
                throw new InvalidOperationException("never reached");
            label76:
;
                goto label79;
            }
            if ((temp279 == 1)) {
                this.Manager.Comment("reaching state \'S159\'");
                bool temp271;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp271);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp271);
                this.Manager.Comment("reaching state \'S333\'");
                int temp274 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS28GetClientAccountTypeChecker2)), new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS28GetClientAccountTypeChecker3)));
                if ((temp274 == 0)) {
                    this.Manager.Comment("reaching state \'S550\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp272;
                    this.Manager.Comment("executing step \'call NetrLogonGetTrustRid(PrimaryDc,InvalidDomainName)\'");
                    temp272 = this.INrpcServerAdapterInstance.NetrLogonGetTrustRid(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.InvalidDomainName);
                    this.Manager.Comment("reaching state \'S898\'");
                    this.Manager.Comment("checking step \'return NetrLogonGetTrustRid/ERROR_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_ACCESS_DENIED, temp272, "return of NetrLogonGetTrustRid, state S898");
                    Test_AuthenticateMessageS1216();
                    goto label77;
                }
                if ((temp274 == 1)) {
                    this.Manager.Comment("reaching state \'S551\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp273;
                    this.Manager.Comment("executing step \'call NetrLogonGetTrustRid(PrimaryDc,InvalidDomainName)\'");
                    temp273 = this.INrpcServerAdapterInstance.NetrLogonGetTrustRid(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.InvalidDomainName);
                    this.Manager.Comment("reaching state \'S899\'");
                    this.Manager.Comment("checking step \'return NetrLogonGetTrustRid/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp273, "return of NetrLogonGetTrustRid, state S899");
                    Test_AuthenticateMessageS1217();
                    goto label77;
                }
                throw new InvalidOperationException("never reached");
            label77:
;
                goto label79;
            }
            if ((temp279 == 2)) {
                this.Manager.Comment("reaching state \'S160\'");
                bool temp275;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp275);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp275);
                this.Manager.Comment("reaching state \'S334\'");
                int temp278 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS28GetClientAccountTypeChecker4)), new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS28GetClientAccountTypeChecker5)));
                if ((temp278 == 0)) {
                    this.Manager.Comment("reaching state \'S552\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp276;
                    this.Manager.Comment("executing step \'call NetrLogonComputeServerDigest(PrimaryDc,RidOfNonMachineAccoun" +
                            "t,MessageOne)\'");
                    temp276 = this.INrpcServerAdapterInstance.NetrLogonComputeServerDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.RidType)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103756");
                    this.Manager.Checkpoint("MS-NRPC_R103743");
                    this.Manager.Comment("reaching state \'S900\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeServerDigest/ERROR_NO_SUCH_USER\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SUCH_USER, temp276, "return of NetrLogonComputeServerDigest, state S900");
                    this.Manager.Comment("reaching state \'S1242\'");
                    goto label78;
                }
                if ((temp278 == 1)) {
                    this.Manager.Comment("reaching state \'S553\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp277;
                    this.Manager.Comment("executing step \'call NetrLogonComputeServerDigest(PrimaryDc,RidOfNonMachineAccoun" +
                            "t,MessageOne)\'");
                    temp277 = this.INrpcServerAdapterInstance.NetrLogonComputeServerDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.RidType)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103756");
                    this.Manager.Checkpoint("MS-NRPC_R103743");
                    this.Manager.Comment("reaching state \'S901\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeServerDigest/ERROR_NO_SUCH_USER\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SUCH_USER, temp277, "return of NetrLogonComputeServerDigest, state S901");
                    this.Manager.Comment("reaching state \'S1243\'");
                    goto label78;
                }
                throw new InvalidOperationException("never reached");
            label78:
;
                goto label79;
            }
            throw new InvalidOperationException("never reached");
        label79:
;
            this.Manager.EndTest();
        }
        
        private void Test_AuthenticateMessageS28GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S29");
        }
        
        private void Test_AuthenticateMessageS28GetClientAccountTypeChecker(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S332");
        }
        
        private void Test_AuthenticateMessageS28GetClientAccountTypeChecker1(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S332");
        }
        
        private void Test_AuthenticateMessageS28GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S29");
        }
        
        private void Test_AuthenticateMessageS28GetClientAccountTypeChecker2(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S333");
        }
        
        private void Test_AuthenticateMessageS28GetClientAccountTypeChecker3(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S333");
        }
        
        private void Test_AuthenticateMessageS28GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S29");
        }
        
        private void Test_AuthenticateMessageS28GetClientAccountTypeChecker4(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S334");
        }
        
        private void Test_AuthenticateMessageS28GetClientAccountTypeChecker5(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S334");
        }
        #endregion
        
        #region Test Starting in S30
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_AuthenticateMessageS30() {
            this.Manager.BeginTest("Test_AuthenticateMessageS30");
            this.Manager.Comment("reaching state \'S30\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp280;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp280);
            this.Manager.AddReturn(GetPlatformInfo, null, temp280);
            this.Manager.Comment("reaching state \'S31\'");
            int temp293 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_AuthenticateMessageS30GetPlatformChecker)), new ExpectedReturn(Test_AuthenticateMessage.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_AuthenticateMessageS30GetPlatformChecker1)), new ExpectedReturn(Test_AuthenticateMessage.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_AuthenticateMessageS30GetPlatformChecker2)));
            if ((temp293 == 0)) {
                this.Manager.Comment("reaching state \'S161\'");
                bool temp281;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp281);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp281);
                this.Manager.Comment("reaching state \'S335\'");
                int temp284 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS30GetClientAccountTypeChecker)), new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS30GetClientAccountTypeChecker1)));
                if ((temp284 == 0)) {
                    this.Manager.Comment("reaching state \'S554\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp282;
                    this.Manager.Comment("executing step \'call NetrLogonGetTrustRid(PrimaryDc,InvalidFormatDomainName)\'");
                    temp282 = this.INrpcServerAdapterInstance.NetrLogonGetTrustRid(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.InvalidFormatDomainName);
                    this.Manager.Comment("reaching state \'S902\'");
                    this.Manager.Comment("checking step \'return NetrLogonGetTrustRid/ERROR_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_ACCESS_DENIED, temp282, "return of NetrLogonGetTrustRid, state S902");
                    Test_AuthenticateMessageS1216();
                    goto label80;
                }
                if ((temp284 == 1)) {
                    this.Manager.Comment("reaching state \'S555\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp283;
                    this.Manager.Comment("executing step \'call NetrLogonGetTrustRid(PrimaryDc,InvalidFormatDomainName)\'");
                    temp283 = this.INrpcServerAdapterInstance.NetrLogonGetTrustRid(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.InvalidFormatDomainName);
                    this.Manager.Comment("reaching state \'S903\'");
                    this.Manager.Comment("checking step \'return NetrLogonGetTrustRid/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp283, "return of NetrLogonGetTrustRid, state S903");
                    Test_AuthenticateMessageS1217();
                    goto label80;
                }
                throw new InvalidOperationException("never reached");
            label80:
;
                goto label83;
            }
            if ((temp293 == 1)) {
                this.Manager.Comment("reaching state \'S162\'");
                bool temp285;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp285);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp285);
                this.Manager.Comment("reaching state \'S336\'");
                int temp288 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS30GetClientAccountTypeChecker2)), new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS30GetClientAccountTypeChecker3)));
                if ((temp288 == 0)) {
                    this.Manager.Comment("reaching state \'S556\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp286;
                    this.Manager.Comment("executing step \'call NetrLogonComputeServerDigest(PrimaryDc,RidOfNonMachineAccoun" +
                            "t,MessageOne)\'");
                    temp286 = this.INrpcServerAdapterInstance.NetrLogonComputeServerDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.RidType)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103756");
                    this.Manager.Checkpoint("MS-NRPC_R103743");
                    this.Manager.Comment("reaching state \'S904\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeServerDigest/ERROR_NO_SUCH_USER\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SUCH_USER, temp286, "return of NetrLogonComputeServerDigest, state S904");
                    Test_AuthenticateMessageS1244();
                    goto label81;
                }
                if ((temp288 == 1)) {
                    this.Manager.Comment("reaching state \'S557\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp287;
                    this.Manager.Comment("executing step \'call NetrLogonComputeServerDigest(PrimaryDc,RidOfNonMachineAccoun" +
                            "t,MessageOne)\'");
                    temp287 = this.INrpcServerAdapterInstance.NetrLogonComputeServerDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.RidType)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103756");
                    this.Manager.Checkpoint("MS-NRPC_R103743");
                    this.Manager.Comment("reaching state \'S905\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeServerDigest/ERROR_NO_SUCH_USER\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SUCH_USER, temp287, "return of NetrLogonComputeServerDigest, state S905");
                    Test_AuthenticateMessageS1245();
                    goto label81;
                }
                throw new InvalidOperationException("never reached");
            label81:
;
                goto label83;
            }
            if ((temp293 == 2)) {
                this.Manager.Comment("reaching state \'S163\'");
                bool temp289;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp289);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp289);
                this.Manager.Comment("reaching state \'S337\'");
                int temp292 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS30GetClientAccountTypeChecker4)), new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS30GetClientAccountTypeChecker5)));
                if ((temp292 == 0)) {
                    this.Manager.Comment("reaching state \'S558\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp290;
                    this.Manager.Comment("executing step \'call NetrLogonComputeServerDigest(PrimaryDc,RidOfNonMachineAccoun" +
                            "t,MessageOne)\'");
                    temp290 = this.INrpcServerAdapterInstance.NetrLogonComputeServerDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.RidType)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103756");
                    this.Manager.Checkpoint("MS-NRPC_R103743");
                    this.Manager.Comment("reaching state \'S906\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeServerDigest/ERROR_NO_SUCH_USER\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SUCH_USER, temp290, "return of NetrLogonComputeServerDigest, state S906");
                    this.Manager.Comment("reaching state \'S1246\'");
                    goto label82;
                }
                if ((temp292 == 1)) {
                    this.Manager.Comment("reaching state \'S559\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp291;
                    this.Manager.Comment("executing step \'call NetrLogonComputeServerDigest(PrimaryDc,RidOfNonMachineAccoun" +
                            "t,MessageOne)\'");
                    temp291 = this.INrpcServerAdapterInstance.NetrLogonComputeServerDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.RidType)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103756");
                    this.Manager.Checkpoint("MS-NRPC_R103743");
                    this.Manager.Comment("reaching state \'S907\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeServerDigest/ERROR_NO_SUCH_USER\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SUCH_USER, temp291, "return of NetrLogonComputeServerDigest, state S907");
                    this.Manager.Comment("reaching state \'S1247\'");
                    goto label82;
                }
                throw new InvalidOperationException("never reached");
            label82:
;
                goto label83;
            }
            throw new InvalidOperationException("never reached");
        label83:
;
            this.Manager.EndTest();
        }
        
        private void Test_AuthenticateMessageS30GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S31");
        }
        
        private void Test_AuthenticateMessageS30GetClientAccountTypeChecker(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S335");
        }
        
        private void Test_AuthenticateMessageS30GetClientAccountTypeChecker1(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S335");
        }
        
        private void Test_AuthenticateMessageS30GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S31");
        }
        
        private void Test_AuthenticateMessageS30GetClientAccountTypeChecker2(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S336");
        }
        
        private void Test_AuthenticateMessageS30GetClientAccountTypeChecker3(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S336");
        }
        
        private void Test_AuthenticateMessageS30GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S31");
        }
        
        private void Test_AuthenticateMessageS30GetClientAccountTypeChecker4(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S337");
        }
        
        private void Test_AuthenticateMessageS30GetClientAccountTypeChecker5(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S337");
        }
        #endregion
        
        #region Test Starting in S32
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_AuthenticateMessageS32() {
            this.Manager.BeginTest("Test_AuthenticateMessageS32");
            this.Manager.Comment("reaching state \'S32\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp294;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp294);
            this.Manager.AddReturn(GetPlatformInfo, null, temp294);
            this.Manager.Comment("reaching state \'S33\'");
            int temp307 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_AuthenticateMessageS32GetPlatformChecker)), new ExpectedReturn(Test_AuthenticateMessage.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_AuthenticateMessageS32GetPlatformChecker1)), new ExpectedReturn(Test_AuthenticateMessage.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_AuthenticateMessageS32GetPlatformChecker2)));
            if ((temp307 == 0)) {
                this.Manager.Comment("reaching state \'S164\'");
                bool temp295;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp295);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp295);
                this.Manager.Comment("reaching state \'S338\'");
                int temp298 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS32GetClientAccountTypeChecker)), new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS32GetClientAccountTypeChecker1)));
                if ((temp298 == 0)) {
                    this.Manager.Comment("reaching state \'S560\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp296;
                    this.Manager.Comment("executing step \'call NetrLogonGetTrustRid(PrimaryDc,NetBiosFormatDomainName)\'");
                    temp296 = this.INrpcServerAdapterInstance.NetrLogonGetTrustRid(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.NetBiosFormatDomainName);
                    this.Manager.Comment("reaching state \'S908\'");
                    this.Manager.Comment("checking step \'return NetrLogonGetTrustRid/ERROR_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_ACCESS_DENIED, temp296, "return of NetrLogonGetTrustRid, state S908");
                    Test_AuthenticateMessageS1216();
                    goto label84;
                }
                if ((temp298 == 1)) {
                    this.Manager.Comment("reaching state \'S561\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp297;
                    this.Manager.Comment("executing step \'call NetrLogonGetTrustRid(PrimaryDc,NetBiosFormatDomainName)\'");
                    temp297 = this.INrpcServerAdapterInstance.NetrLogonGetTrustRid(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.NetBiosFormatDomainName);
                    this.Manager.Comment("reaching state \'S909\'");
                    this.Manager.Comment("checking step \'return NetrLogonGetTrustRid/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp297, "return of NetrLogonGetTrustRid, state S909");
                    Test_AuthenticateMessageS1217();
                    goto label84;
                }
                throw new InvalidOperationException("never reached");
            label84:
;
                goto label87;
            }
            if ((temp307 == 1)) {
                this.Manager.Comment("reaching state \'S165\'");
                bool temp299;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp299);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp299);
                this.Manager.Comment("reaching state \'S339\'");
                int temp302 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS32GetClientAccountTypeChecker2)), new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS32GetClientAccountTypeChecker3)));
                if ((temp302 == 0)) {
                    this.Manager.Comment("reaching state \'S562\'");
                    this.Manager.Comment("executing step \'call SwitchUserAccount(False)\'");
                    this.INrpcServerAdapterInstance.SwitchUserAccount(false);
                    this.Manager.Comment("reaching state \'S910\'");
                    this.Manager.Comment("checking step \'return SwitchUserAccount\'");
                    this.Manager.Comment("reaching state \'S1248\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp300;
                    this.Manager.Comment("executing step \'call NetrLogonComputeClientDigest(PrimaryDc,TrustedDomainName,Mes" +
                            "sageOne)\'");
                    temp300 = this.INrpcServerAdapterInstance.NetrLogonComputeClientDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.TrustedDomainName, ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103791");
                    this.Manager.Checkpoint("MS-NRPC_R103790");
                    this.Manager.Comment("reaching state \'S1430\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeClientDigest/ERROR_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_ACCESS_DENIED, temp300, "return of NetrLogonComputeClientDigest, state S1430");
                    Test_AuthenticateMessageS1450();
                    goto label85;
                }
                if ((temp302 == 1)) {
                    this.Manager.Comment("reaching state \'S563\'");
                    this.Manager.Comment("executing step \'call SwitchUserAccount(False)\'");
                    this.INrpcServerAdapterInstance.SwitchUserAccount(false);
                    this.Manager.Comment("reaching state \'S911\'");
                    this.Manager.Comment("checking step \'return SwitchUserAccount\'");
                    this.Manager.Comment("reaching state \'S1249\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp301;
                    this.Manager.Comment("executing step \'call NetrLogonComputeClientDigest(PrimaryDc,TrustedDomainName,Mes" +
                            "sageOne)\'");
                    temp301 = this.INrpcServerAdapterInstance.NetrLogonComputeClientDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.TrustedDomainName, ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103791");
                    this.Manager.Checkpoint("MS-NRPC_R103790");
                    this.Manager.Comment("reaching state \'S1431\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeClientDigest/ERROR_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_ACCESS_DENIED, temp301, "return of NetrLogonComputeClientDigest, state S1431");
                    Test_AuthenticateMessageS1451();
                    goto label85;
                }
                throw new InvalidOperationException("never reached");
            label85:
;
                goto label87;
            }
            if ((temp307 == 2)) {
                this.Manager.Comment("reaching state \'S166\'");
                bool temp303;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp303);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp303);
                this.Manager.Comment("reaching state \'S340\'");
                int temp306 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS32GetClientAccountTypeChecker4)), new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS32GetClientAccountTypeChecker5)));
                if ((temp306 == 0)) {
                    this.Manager.Comment("reaching state \'S564\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp304;
                    this.Manager.Comment("executing step \'call NetrLogonComputeServerDigest(PrimaryDc,RidOfNonMachineAccoun" +
                            "t,MessageOne)\'");
                    temp304 = this.INrpcServerAdapterInstance.NetrLogonComputeServerDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.RidType)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103756");
                    this.Manager.Checkpoint("MS-NRPC_R103743");
                    this.Manager.Comment("reaching state \'S912\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeServerDigest/ERROR_NO_SUCH_USER\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SUCH_USER, temp304, "return of NetrLogonComputeServerDigest, state S912");
                    this.Manager.Comment("reaching state \'S1250\'");
                    goto label86;
                }
                if ((temp306 == 1)) {
                    this.Manager.Comment("reaching state \'S565\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp305;
                    this.Manager.Comment("executing step \'call NetrLogonComputeServerDigest(PrimaryDc,RidOfNonMachineAccoun" +
                            "t,MessageOne)\'");
                    temp305 = this.INrpcServerAdapterInstance.NetrLogonComputeServerDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.RidType)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103756");
                    this.Manager.Checkpoint("MS-NRPC_R103743");
                    this.Manager.Comment("reaching state \'S913\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeServerDigest/ERROR_NO_SUCH_USER\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SUCH_USER, temp305, "return of NetrLogonComputeServerDigest, state S913");
                    this.Manager.Comment("reaching state \'S1251\'");
                    goto label86;
                }
                throw new InvalidOperationException("never reached");
            label86:
;
                goto label87;
            }
            throw new InvalidOperationException("never reached");
        label87:
;
            this.Manager.EndTest();
        }
        
        private void Test_AuthenticateMessageS32GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S33");
        }
        
        private void Test_AuthenticateMessageS32GetClientAccountTypeChecker(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S338");
        }
        
        private void Test_AuthenticateMessageS32GetClientAccountTypeChecker1(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S338");
        }
        
        private void Test_AuthenticateMessageS32GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S33");
        }
        
        private void Test_AuthenticateMessageS32GetClientAccountTypeChecker2(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S339");
        }
        
        private void Test_AuthenticateMessageS32GetClientAccountTypeChecker3(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S339");
        }
        
        private void Test_AuthenticateMessageS32GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S33");
        }
        
        private void Test_AuthenticateMessageS32GetClientAccountTypeChecker4(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S340");
        }
        
        private void Test_AuthenticateMessageS32GetClientAccountTypeChecker5(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S340");
        }
        #endregion
        
        #region Test Starting in S34
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_AuthenticateMessageS34() {
            this.Manager.BeginTest("Test_AuthenticateMessageS34");
            this.Manager.Comment("reaching state \'S34\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp308;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp308);
            this.Manager.AddReturn(GetPlatformInfo, null, temp308);
            this.Manager.Comment("reaching state \'S35\'");
            int temp321 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_AuthenticateMessageS34GetPlatformChecker)), new ExpectedReturn(Test_AuthenticateMessage.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_AuthenticateMessageS34GetPlatformChecker1)), new ExpectedReturn(Test_AuthenticateMessage.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_AuthenticateMessageS34GetPlatformChecker2)));
            if ((temp321 == 0)) {
                this.Manager.Comment("reaching state \'S167\'");
                bool temp309;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp309);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp309);
                this.Manager.Comment("reaching state \'S341\'");
                int temp312 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS34GetClientAccountTypeChecker)), new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS34GetClientAccountTypeChecker1)));
                if ((temp312 == 0)) {
                    this.Manager.Comment("reaching state \'S566\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp310;
                    this.Manager.Comment("executing step \'call NetrLogonGetTrustRid(PrimaryDc,FqdnFormatDomainName)\'");
                    temp310 = this.INrpcServerAdapterInstance.NetrLogonGetTrustRid(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.FqdnFormatDomainName);
                    this.Manager.Comment("reaching state \'S914\'");
                    this.Manager.Comment("checking step \'return NetrLogonGetTrustRid/ERROR_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_ACCESS_DENIED, temp310, "return of NetrLogonGetTrustRid, state S914");
                    Test_AuthenticateMessageS1216();
                    goto label88;
                }
                if ((temp312 == 1)) {
                    this.Manager.Comment("reaching state \'S567\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp311;
                    this.Manager.Comment("executing step \'call NetrLogonGetTrustRid(PrimaryDc,FqdnFormatDomainName)\'");
                    temp311 = this.INrpcServerAdapterInstance.NetrLogonGetTrustRid(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.FqdnFormatDomainName);
                    this.Manager.Comment("reaching state \'S915\'");
                    this.Manager.Comment("checking step \'return NetrLogonGetTrustRid/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp311, "return of NetrLogonGetTrustRid, state S915");
                    Test_AuthenticateMessageS1217();
                    goto label88;
                }
                throw new InvalidOperationException("never reached");
            label88:
;
                goto label91;
            }
            if ((temp321 == 1)) {
                this.Manager.Comment("reaching state \'S168\'");
                bool temp313;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp313);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp313);
                this.Manager.Comment("reaching state \'S342\'");
                int temp316 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS34GetClientAccountTypeChecker2)), new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS34GetClientAccountTypeChecker3)));
                if ((temp316 == 0)) {
                    this.Manager.Comment("reaching state \'S568\'");
                    this.Manager.Comment("executing step \'call SwitchUserAccount(False)\'");
                    this.INrpcServerAdapterInstance.SwitchUserAccount(false);
                    this.Manager.Comment("reaching state \'S916\'");
                    this.Manager.Comment("checking step \'return SwitchUserAccount\'");
                    this.Manager.Comment("reaching state \'S1252\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp314;
                    this.Manager.Comment("executing step \'call NetrLogonComputeServerDigest(PrimaryDc,RidOfNonDcMachineAcco" +
                            "unt,MessageOne)\'");
                    temp314 = this.INrpcServerAdapterInstance.NetrLogonComputeServerDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.RidType.RidOfNonDcMachineAccount, ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103746");
                    this.Manager.Checkpoint("MS-NRPC_R103743");
                    this.Manager.Comment("reaching state \'S1432\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeServerDigest/ERROR_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_ACCESS_DENIED, temp314, "return of NetrLogonComputeServerDigest, state S1432");
                    Test_AuthenticateMessageS1454();
                    goto label89;
                }
                if ((temp316 == 1)) {
                    this.Manager.Comment("reaching state \'S569\'");
                    this.Manager.Comment("executing step \'call SwitchUserAccount(False)\'");
                    this.INrpcServerAdapterInstance.SwitchUserAccount(false);
                    this.Manager.Comment("reaching state \'S917\'");
                    this.Manager.Comment("checking step \'return SwitchUserAccount\'");
                    this.Manager.Comment("reaching state \'S1253\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp315;
                    this.Manager.Comment("executing step \'call NetrLogonComputeServerDigest(PrimaryDc,RidOfNonDcMachineAcco" +
                            "unt,MessageOne)\'");
                    temp315 = this.INrpcServerAdapterInstance.NetrLogonComputeServerDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.RidType.RidOfNonDcMachineAccount, ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103746");
                    this.Manager.Checkpoint("MS-NRPC_R103743");
                    this.Manager.Comment("reaching state \'S1433\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeServerDigest/ERROR_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_ACCESS_DENIED, temp315, "return of NetrLogonComputeServerDigest, state S1433");
                    Test_AuthenticateMessageS1455();
                    goto label89;
                }
                throw new InvalidOperationException("never reached");
            label89:
;
                goto label91;
            }
            if ((temp321 == 2)) {
                this.Manager.Comment("reaching state \'S169\'");
                bool temp317;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp317);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp317);
                this.Manager.Comment("reaching state \'S343\'");
                int temp320 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS34GetClientAccountTypeChecker4)), new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS34GetClientAccountTypeChecker5)));
                if ((temp320 == 0)) {
                    this.Manager.Comment("reaching state \'S570\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp318;
                    this.Manager.Comment("executing step \'call NetrLogonComputeServerDigest(PrimaryDc,RidOfNonMachineAccoun" +
                            "t,MessageOne)\'");
                    temp318 = this.INrpcServerAdapterInstance.NetrLogonComputeServerDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.RidType)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103756");
                    this.Manager.Checkpoint("MS-NRPC_R103743");
                    this.Manager.Comment("reaching state \'S918\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeServerDigest/ERROR_NO_SUCH_USER\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SUCH_USER, temp318, "return of NetrLogonComputeServerDigest, state S918");
                    this.Manager.Comment("reaching state \'S1254\'");
                    goto label90;
                }
                if ((temp320 == 1)) {
                    this.Manager.Comment("reaching state \'S571\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp319;
                    this.Manager.Comment("executing step \'call NetrLogonComputeServerDigest(PrimaryDc,RidOfNonMachineAccoun" +
                            "t,MessageOne)\'");
                    temp319 = this.INrpcServerAdapterInstance.NetrLogonComputeServerDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.RidType)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103756");
                    this.Manager.Checkpoint("MS-NRPC_R103743");
                    this.Manager.Comment("reaching state \'S919\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeServerDigest/ERROR_NO_SUCH_USER\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SUCH_USER, temp319, "return of NetrLogonComputeServerDigest, state S919");
                    this.Manager.Comment("reaching state \'S1255\'");
                    goto label90;
                }
                throw new InvalidOperationException("never reached");
            label90:
;
                goto label91;
            }
            throw new InvalidOperationException("never reached");
        label91:
;
            this.Manager.EndTest();
        }
        
        private void Test_AuthenticateMessageS34GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S35");
        }
        
        private void Test_AuthenticateMessageS34GetClientAccountTypeChecker(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S341");
        }
        
        private void Test_AuthenticateMessageS34GetClientAccountTypeChecker1(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S341");
        }
        
        private void Test_AuthenticateMessageS34GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S35");
        }
        
        private void Test_AuthenticateMessageS34GetClientAccountTypeChecker2(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S342");
        }
        
        private void Test_AuthenticateMessageS34GetClientAccountTypeChecker3(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S342");
        }
        
        private void Test_AuthenticateMessageS34GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S35");
        }
        
        private void Test_AuthenticateMessageS34GetClientAccountTypeChecker4(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S343");
        }
        
        private void Test_AuthenticateMessageS34GetClientAccountTypeChecker5(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S343");
        }
        #endregion
        
        #region Test Starting in S36
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_AuthenticateMessageS36() {
            this.Manager.BeginTest("Test_AuthenticateMessageS36");
            this.Manager.Comment("reaching state \'S36\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp322;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp322);
            this.Manager.AddReturn(GetPlatformInfo, null, temp322);
            this.Manager.Comment("reaching state \'S37\'");
            int temp335 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_AuthenticateMessageS36GetPlatformChecker)), new ExpectedReturn(Test_AuthenticateMessage.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_AuthenticateMessageS36GetPlatformChecker1)), new ExpectedReturn(Test_AuthenticateMessage.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_AuthenticateMessageS36GetPlatformChecker2)));
            if ((temp335 == 0)) {
                this.Manager.Comment("reaching state \'S170\'");
                bool temp323;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp323);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp323);
                this.Manager.Comment("reaching state \'S344\'");
                int temp326 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS36GetClientAccountTypeChecker)), new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS36GetClientAccountTypeChecker1)));
                if ((temp326 == 0)) {
                    this.Manager.Comment("reaching state \'S572\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp324;
                    this.Manager.Comment("executing step \'call NetrLogonGetTrustRid(PrimaryDc,TrustedDomainName)\'");
                    temp324 = this.INrpcServerAdapterInstance.NetrLogonGetTrustRid(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.TrustedDomainName);
                    this.Manager.Comment("reaching state \'S920\'");
                    this.Manager.Comment("checking step \'return NetrLogonGetTrustRid/ERROR_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_ACCESS_DENIED, temp324, "return of NetrLogonGetTrustRid, state S920");
                    Test_AuthenticateMessageS1216();
                    goto label92;
                }
                if ((temp326 == 1)) {
                    this.Manager.Comment("reaching state \'S573\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp325;
                    this.Manager.Comment("executing step \'call NetrLogonGetTrustRid(PrimaryDc,TrustedDomainName)\'");
                    temp325 = this.INrpcServerAdapterInstance.NetrLogonGetTrustRid(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.TrustedDomainName);
                    this.Manager.Comment("reaching state \'S921\'");
                    this.Manager.Comment("checking step \'return NetrLogonGetTrustRid/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp325, "return of NetrLogonGetTrustRid, state S921");
                    Test_AuthenticateMessageS1217();
                    goto label92;
                }
                throw new InvalidOperationException("never reached");
            label92:
;
                goto label95;
            }
            if ((temp335 == 1)) {
                this.Manager.Comment("reaching state \'S171\'");
                bool temp327;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp327);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp327);
                this.Manager.Comment("reaching state \'S345\'");
                int temp330 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS36GetClientAccountTypeChecker2)), new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS36GetClientAccountTypeChecker3)));
                if ((temp330 == 0)) {
                    this.Manager.Comment("reaching state \'S574\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp328;
                    this.Manager.Comment("executing step \'call NetrLogonComputeServerDigest(PrimaryDc,NonExist,MessageOne)\'" +
                            "");
                    temp328 = this.INrpcServerAdapterInstance.NetrLogonComputeServerDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.RidType)(0)), ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103755");
                    this.Manager.Checkpoint("MS-NRPC_R103743");
                    this.Manager.Comment("reaching state \'S922\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeServerDigest/ERROR_NO_SUCH_USER\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SUCH_USER, temp328, "return of NetrLogonComputeServerDigest, state S922");
                    Test_AuthenticateMessageS1244();
                    goto label93;
                }
                if ((temp330 == 1)) {
                    this.Manager.Comment("reaching state \'S575\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp329;
                    this.Manager.Comment("executing step \'call NetrLogonComputeServerDigest(PrimaryDc,NonExist,MessageOne)\'" +
                            "");
                    temp329 = this.INrpcServerAdapterInstance.NetrLogonComputeServerDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.RidType)(0)), ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103755");
                    this.Manager.Checkpoint("MS-NRPC_R103743");
                    this.Manager.Comment("reaching state \'S923\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeServerDigest/ERROR_NO_SUCH_USER\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SUCH_USER, temp329, "return of NetrLogonComputeServerDigest, state S923");
                    Test_AuthenticateMessageS1245();
                    goto label93;
                }
                throw new InvalidOperationException("never reached");
            label93:
;
                goto label95;
            }
            if ((temp335 == 2)) {
                this.Manager.Comment("reaching state \'S172\'");
                bool temp331;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp331);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp331);
                this.Manager.Comment("reaching state \'S346\'");
                int temp334 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS36GetClientAccountTypeChecker4)), new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS36GetClientAccountTypeChecker5)));
                if ((temp334 == 0)) {
                    this.Manager.Comment("reaching state \'S576\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp332;
                    this.Manager.Comment("executing step \'call NetrLogonComputeServerDigest(PrimaryDc,RidOfNonMachineAccoun" +
                            "t,MessageOne)\'");
                    temp332 = this.INrpcServerAdapterInstance.NetrLogonComputeServerDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.RidType)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103756");
                    this.Manager.Checkpoint("MS-NRPC_R103743");
                    this.Manager.Comment("reaching state \'S924\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeServerDigest/ERROR_NO_SUCH_USER\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SUCH_USER, temp332, "return of NetrLogonComputeServerDigest, state S924");
                    this.Manager.Comment("reaching state \'S1256\'");
                    goto label94;
                }
                if ((temp334 == 1)) {
                    this.Manager.Comment("reaching state \'S577\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp333;
                    this.Manager.Comment("executing step \'call NetrLogonComputeServerDigest(PrimaryDc,RidOfNonMachineAccoun" +
                            "t,MessageOne)\'");
                    temp333 = this.INrpcServerAdapterInstance.NetrLogonComputeServerDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.RidType)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103756");
                    this.Manager.Checkpoint("MS-NRPC_R103743");
                    this.Manager.Comment("reaching state \'S925\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeServerDigest/ERROR_NO_SUCH_USER\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SUCH_USER, temp333, "return of NetrLogonComputeServerDigest, state S925");
                    this.Manager.Comment("reaching state \'S1257\'");
                    goto label94;
                }
                throw new InvalidOperationException("never reached");
            label94:
;
                goto label95;
            }
            throw new InvalidOperationException("never reached");
        label95:
;
            this.Manager.EndTest();
        }
        
        private void Test_AuthenticateMessageS36GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S37");
        }
        
        private void Test_AuthenticateMessageS36GetClientAccountTypeChecker(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S344");
        }
        
        private void Test_AuthenticateMessageS36GetClientAccountTypeChecker1(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S344");
        }
        
        private void Test_AuthenticateMessageS36GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S37");
        }
        
        private void Test_AuthenticateMessageS36GetClientAccountTypeChecker2(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S345");
        }
        
        private void Test_AuthenticateMessageS36GetClientAccountTypeChecker3(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S345");
        }
        
        private void Test_AuthenticateMessageS36GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S37");
        }
        
        private void Test_AuthenticateMessageS36GetClientAccountTypeChecker4(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S346");
        }
        
        private void Test_AuthenticateMessageS36GetClientAccountTypeChecker5(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S346");
        }
        #endregion
        
        #region Test Starting in S38
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_AuthenticateMessageS38() {
            this.Manager.BeginTest("Test_AuthenticateMessageS38");
            this.Manager.Comment("reaching state \'S38\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp336;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp336);
            this.Manager.AddReturn(GetPlatformInfo, null, temp336);
            this.Manager.Comment("reaching state \'S39\'");
            int temp349 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_AuthenticateMessageS38GetPlatformChecker)), new ExpectedReturn(Test_AuthenticateMessage.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_AuthenticateMessageS38GetPlatformChecker1)), new ExpectedReturn(Test_AuthenticateMessage.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_AuthenticateMessageS38GetPlatformChecker2)));
            if ((temp349 == 0)) {
                this.Manager.Comment("reaching state \'S173\'");
                bool temp337;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp337);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp337);
                this.Manager.Comment("reaching state \'S347\'");
                int temp340 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS38GetClientAccountTypeChecker)), new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS38GetClientAccountTypeChecker1)));
                if ((temp340 == 0)) {
                    this.Manager.Comment("reaching state \'S578\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp338;
                    this.Manager.Comment("executing step \'call NetrLogonComputeServerDigest(PrimaryDc,RidOfNonMachineAccoun" +
                            "t,MessageOne)\'");
                    temp338 = this.INrpcServerAdapterInstance.NetrLogonComputeServerDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.RidType)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103756");
                    this.Manager.Checkpoint("MS-NRPC_R103743");
                    this.Manager.Comment("reaching state \'S926\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeServerDigest/ERROR_NO_SUCH_USER\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SUCH_USER, temp338, "return of NetrLogonComputeServerDigest, state S926");
                    Test_AuthenticateMessageS1216();
                    goto label96;
                }
                if ((temp340 == 1)) {
                    this.Manager.Comment("reaching state \'S579\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp339;
                    this.Manager.Comment("executing step \'call NetrLogonComputeServerDigest(PrimaryDc,RidOfNonMachineAccoun" +
                            "t,MessageOne)\'");
                    temp339 = this.INrpcServerAdapterInstance.NetrLogonComputeServerDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.RidType)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103756");
                    this.Manager.Checkpoint("MS-NRPC_R103743");
                    this.Manager.Comment("reaching state \'S927\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeServerDigest/ERROR_NO_SUCH_USER\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SUCH_USER, temp339, "return of NetrLogonComputeServerDigest, state S927");
                    Test_AuthenticateMessageS1217();
                    goto label96;
                }
                throw new InvalidOperationException("never reached");
            label96:
;
                goto label99;
            }
            if ((temp349 == 1)) {
                this.Manager.Comment("reaching state \'S174\'");
                bool temp341;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp341);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp341);
                this.Manager.Comment("reaching state \'S348\'");
                int temp344 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS38GetClientAccountTypeChecker2)), new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS38GetClientAccountTypeChecker3)));
                if ((temp344 == 0)) {
                    this.Manager.Comment("reaching state \'S580\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp342;
                    this.Manager.Comment("executing step \'call NetrLogonComputeServerDigest(PrimaryDc,RidOfNonDcMachineAcco" +
                            "unt,MessageOne)\'");
                    temp342 = this.INrpcServerAdapterInstance.NetrLogonComputeServerDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.RidType.RidOfNonDcMachineAccount, ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103742");
                    this.Manager.Comment("reaching state \'S928\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeServerDigest/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp342, "return of NetrLogonComputeServerDigest, state S928");
                    Test_AuthenticateMessageS1244();
                    goto label97;
                }
                if ((temp344 == 1)) {
                    this.Manager.Comment("reaching state \'S581\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp343;
                    this.Manager.Comment("executing step \'call NetrLogonComputeServerDigest(PrimaryDc,RidOfNonDcMachineAcco" +
                            "unt,MessageOne)\'");
                    temp343 = this.INrpcServerAdapterInstance.NetrLogonComputeServerDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.RidType.RidOfNonDcMachineAccount, ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103742");
                    this.Manager.Comment("reaching state \'S929\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeServerDigest/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp343, "return of NetrLogonComputeServerDigest, state S929");
                    Test_AuthenticateMessageS1245();
                    goto label97;
                }
                throw new InvalidOperationException("never reached");
            label97:
;
                goto label99;
            }
            if ((temp349 == 2)) {
                this.Manager.Comment("reaching state \'S175\'");
                bool temp345;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp345);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp345);
                this.Manager.Comment("reaching state \'S349\'");
                int temp348 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS38GetClientAccountTypeChecker4)), new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS38GetClientAccountTypeChecker5)));
                if ((temp348 == 0)) {
                    this.Manager.Comment("reaching state \'S582\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp346;
                    this.Manager.Comment("executing step \'call NetrLogonComputeServerDigest(PrimaryDc,RidOfNonMachineAccoun" +
                            "t,MessageOne)\'");
                    temp346 = this.INrpcServerAdapterInstance.NetrLogonComputeServerDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.RidType)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103756");
                    this.Manager.Checkpoint("MS-NRPC_R103743");
                    this.Manager.Comment("reaching state \'S930\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeServerDigest/ERROR_NO_SUCH_USER\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SUCH_USER, temp346, "return of NetrLogonComputeServerDigest, state S930");
                    this.Manager.Comment("reaching state \'S1258\'");
                    goto label98;
                }
                if ((temp348 == 1)) {
                    this.Manager.Comment("reaching state \'S583\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp347;
                    this.Manager.Comment("executing step \'call NetrLogonComputeServerDigest(PrimaryDc,RidOfNonMachineAccoun" +
                            "t,MessageOne)\'");
                    temp347 = this.INrpcServerAdapterInstance.NetrLogonComputeServerDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.RidType)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103756");
                    this.Manager.Checkpoint("MS-NRPC_R103743");
                    this.Manager.Comment("reaching state \'S931\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeServerDigest/ERROR_NO_SUCH_USER\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SUCH_USER, temp347, "return of NetrLogonComputeServerDigest, state S931");
                    this.Manager.Comment("reaching state \'S1259\'");
                    goto label98;
                }
                throw new InvalidOperationException("never reached");
            label98:
;
                goto label99;
            }
            throw new InvalidOperationException("never reached");
        label99:
;
            this.Manager.EndTest();
        }
        
        private void Test_AuthenticateMessageS38GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S39");
        }
        
        private void Test_AuthenticateMessageS38GetClientAccountTypeChecker(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S347");
        }
        
        private void Test_AuthenticateMessageS38GetClientAccountTypeChecker1(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S347");
        }
        
        private void Test_AuthenticateMessageS38GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S39");
        }
        
        private void Test_AuthenticateMessageS38GetClientAccountTypeChecker2(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S348");
        }
        
        private void Test_AuthenticateMessageS38GetClientAccountTypeChecker3(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S348");
        }
        
        private void Test_AuthenticateMessageS38GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S39");
        }
        
        private void Test_AuthenticateMessageS38GetClientAccountTypeChecker4(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S349");
        }
        
        private void Test_AuthenticateMessageS38GetClientAccountTypeChecker5(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S349");
        }
        #endregion
        
        #region Test Starting in S4
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_AuthenticateMessageS4() {
            this.Manager.BeginTest("Test_AuthenticateMessageS4");
            this.Manager.Comment("reaching state \'S4\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp350;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp350);
            this.Manager.AddReturn(GetPlatformInfo, null, temp350);
            this.Manager.Comment("reaching state \'S5\'");
            int temp363 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_AuthenticateMessageS4GetPlatformChecker)), new ExpectedReturn(Test_AuthenticateMessage.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_AuthenticateMessageS4GetPlatformChecker1)), new ExpectedReturn(Test_AuthenticateMessage.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_AuthenticateMessageS4GetPlatformChecker2)));
            if ((temp363 == 0)) {
                this.Manager.Comment("reaching state \'S122\'");
                bool temp351;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp351);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp351);
                this.Manager.Comment("reaching state \'S296\'");
                int temp354 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS4GetClientAccountTypeChecker)), new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS4GetClientAccountTypeChecker1)));
                if ((temp354 == 0)) {
                    this.Manager.Comment("reaching state \'S476\'");
                    this.Manager.Comment("executing step \'call SwitchUserAccount(False)\'");
                    this.INrpcServerAdapterInstance.SwitchUserAccount(false);
                    this.Manager.Comment("reaching state \'S824\'");
                    this.Manager.Comment("checking step \'return SwitchUserAccount\'");
                    this.Manager.Comment("reaching state \'S1172\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp352;
                    this.Manager.Comment("executing step \'call NetrLogonComputeClientDigest(PrimaryDc,EmptyDomainName,Messa" +
                            "geOne)\'");
                    temp352 = this.INrpcServerAdapterInstance.NetrLogonComputeClientDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.DomainNameType)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103791");
                    this.Manager.Checkpoint("MS-NRPC_R103790");
                    this.Manager.Comment("reaching state \'S1386\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeClientDigest/ERROR_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_ACCESS_DENIED, temp352, "return of NetrLogonComputeClientDigest, state S1386");
                    Test_AuthenticateMessageS1448();
                    goto label100;
                }
                if ((temp354 == 1)) {
                    this.Manager.Comment("reaching state \'S477\'");
                    this.Manager.Comment("executing step \'call SwitchUserAccount(False)\'");
                    this.INrpcServerAdapterInstance.SwitchUserAccount(false);
                    this.Manager.Comment("reaching state \'S825\'");
                    this.Manager.Comment("checking step \'return SwitchUserAccount\'");
                    this.Manager.Comment("reaching state \'S1173\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp353;
                    this.Manager.Comment("executing step \'call NetrLogonComputeClientDigest(PrimaryDc,EmptyDomainName,Messa" +
                            "geOne)\'");
                    temp353 = this.INrpcServerAdapterInstance.NetrLogonComputeClientDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.DomainNameType)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103791");
                    this.Manager.Checkpoint("MS-NRPC_R103790");
                    this.Manager.Comment("reaching state \'S1387\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeClientDigest/ERROR_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_ACCESS_DENIED, temp353, "return of NetrLogonComputeClientDigest, state S1387");
                    Test_AuthenticateMessageS1449();
                    goto label100;
                }
                throw new InvalidOperationException("never reached");
            label100:
;
                goto label103;
            }
            if ((temp363 == 1)) {
                this.Manager.Comment("reaching state \'S123\'");
                bool temp355;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp355);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp355);
                this.Manager.Comment("reaching state \'S297\'");
                int temp358 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS4GetClientAccountTypeChecker2)), new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS4GetClientAccountTypeChecker3)));
                if ((temp358 == 0)) {
                    this.Manager.Comment("reaching state \'S478\'");
                    this.Manager.Comment("executing step \'call SwitchUserAccount(False)\'");
                    this.INrpcServerAdapterInstance.SwitchUserAccount(false);
                    this.Manager.Comment("reaching state \'S826\'");
                    this.Manager.Comment("checking step \'return SwitchUserAccount\'");
                    this.Manager.Comment("reaching state \'S1174\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp356;
                    this.Manager.Comment("executing step \'call NetrLogonComputeClientDigest(PrimaryDc,EmptyDomainName,Messa" +
                            "geOne)\'");
                    temp356 = this.INrpcServerAdapterInstance.NetrLogonComputeClientDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.DomainNameType)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103791");
                    this.Manager.Checkpoint("MS-NRPC_R103790");
                    this.Manager.Comment("reaching state \'S1388\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeClientDigest/ERROR_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_ACCESS_DENIED, temp356, "return of NetrLogonComputeClientDigest, state S1388");
                    Test_AuthenticateMessageS1450();
                    goto label101;
                }
                if ((temp358 == 1)) {
                    this.Manager.Comment("reaching state \'S479\'");
                    this.Manager.Comment("executing step \'call SwitchUserAccount(False)\'");
                    this.INrpcServerAdapterInstance.SwitchUserAccount(false);
                    this.Manager.Comment("reaching state \'S827\'");
                    this.Manager.Comment("checking step \'return SwitchUserAccount\'");
                    this.Manager.Comment("reaching state \'S1175\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp357;
                    this.Manager.Comment("executing step \'call NetrLogonComputeClientDigest(PrimaryDc,EmptyDomainName,Messa" +
                            "geOne)\'");
                    temp357 = this.INrpcServerAdapterInstance.NetrLogonComputeClientDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.DomainNameType)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103791");
                    this.Manager.Checkpoint("MS-NRPC_R103790");
                    this.Manager.Comment("reaching state \'S1389\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeClientDigest/ERROR_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_ACCESS_DENIED, temp357, "return of NetrLogonComputeClientDigest, state S1389");
                    Test_AuthenticateMessageS1451();
                    goto label101;
                }
                throw new InvalidOperationException("never reached");
            label101:
;
                goto label103;
            }
            if ((temp363 == 2)) {
                this.Manager.Comment("reaching state \'S124\'");
                bool temp359;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp359);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp359);
                this.Manager.Comment("reaching state \'S298\'");
                int temp362 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS4GetClientAccountTypeChecker4)), new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS4GetClientAccountTypeChecker5)));
                if ((temp362 == 0)) {
                    this.Manager.Comment("reaching state \'S480\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp360;
                    this.Manager.Comment("executing step \'call NetrLogonComputeServerDigest(PrimaryDc,RidOfNonMachineAccoun" +
                            "t,MessageOne)\'");
                    temp360 = this.INrpcServerAdapterInstance.NetrLogonComputeServerDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.RidType)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103756");
                    this.Manager.Checkpoint("MS-NRPC_R103743");
                    this.Manager.Comment("reaching state \'S828\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeServerDigest/ERROR_NO_SUCH_USER\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SUCH_USER, temp360, "return of NetrLogonComputeServerDigest, state S828");
                    this.Manager.Comment("reaching state \'S1176\'");
                    goto label102;
                }
                if ((temp362 == 1)) {
                    this.Manager.Comment("reaching state \'S481\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp361;
                    this.Manager.Comment("executing step \'call NetrLogonComputeServerDigest(PrimaryDc,RidOfNonMachineAccoun" +
                            "t,MessageOne)\'");
                    temp361 = this.INrpcServerAdapterInstance.NetrLogonComputeServerDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.RidType)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103756");
                    this.Manager.Checkpoint("MS-NRPC_R103743");
                    this.Manager.Comment("reaching state \'S829\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeServerDigest/ERROR_NO_SUCH_USER\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SUCH_USER, temp361, "return of NetrLogonComputeServerDigest, state S829");
                    this.Manager.Comment("reaching state \'S1177\'");
                    goto label102;
                }
                throw new InvalidOperationException("never reached");
            label102:
;
                goto label103;
            }
            throw new InvalidOperationException("never reached");
        label103:
;
            this.Manager.EndTest();
        }
        
        private void Test_AuthenticateMessageS4GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S5");
        }
        
        private void Test_AuthenticateMessageS4GetClientAccountTypeChecker(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S296");
        }
        
        private void Test_AuthenticateMessageS4GetClientAccountTypeChecker1(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S296");
        }
        
        private void Test_AuthenticateMessageS4GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S5");
        }
        
        private void Test_AuthenticateMessageS4GetClientAccountTypeChecker2(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S297");
        }
        
        private void Test_AuthenticateMessageS4GetClientAccountTypeChecker3(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S297");
        }
        
        private void Test_AuthenticateMessageS4GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S5");
        }
        
        private void Test_AuthenticateMessageS4GetClientAccountTypeChecker4(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S298");
        }
        
        private void Test_AuthenticateMessageS4GetClientAccountTypeChecker5(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S298");
        }
        #endregion
        
        #region Test Starting in S40
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_AuthenticateMessageS40() {
            this.Manager.BeginTest("Test_AuthenticateMessageS40");
            this.Manager.Comment("reaching state \'S40\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp364;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp364);
            this.Manager.AddReturn(GetPlatformInfo, null, temp364);
            this.Manager.Comment("reaching state \'S41\'");
            int temp377 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_AuthenticateMessageS40GetPlatformChecker)), new ExpectedReturn(Test_AuthenticateMessage.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_AuthenticateMessageS40GetPlatformChecker1)), new ExpectedReturn(Test_AuthenticateMessage.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_AuthenticateMessageS40GetPlatformChecker2)));
            if ((temp377 == 0)) {
                this.Manager.Comment("reaching state \'S176\'");
                bool temp365;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp365);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp365);
                this.Manager.Comment("reaching state \'S350\'");
                int temp368 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS40GetClientAccountTypeChecker)), new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS40GetClientAccountTypeChecker1)));
                if ((temp368 == 0)) {
                    this.Manager.Comment("reaching state \'S584\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp366;
                    this.Manager.Comment("executing step \'call NetrLogonComputeServerDigest(PrimaryDc,NonExist,MessageOne)\'" +
                            "");
                    temp366 = this.INrpcServerAdapterInstance.NetrLogonComputeServerDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.RidType)(0)), ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103755");
                    this.Manager.Checkpoint("MS-NRPC_R103743");
                    this.Manager.Comment("reaching state \'S932\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeServerDigest/ERROR_NO_SUCH_USER\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SUCH_USER, temp366, "return of NetrLogonComputeServerDigest, state S932");
                    Test_AuthenticateMessageS1216();
                    goto label104;
                }
                if ((temp368 == 1)) {
                    this.Manager.Comment("reaching state \'S585\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp367;
                    this.Manager.Comment("executing step \'call NetrLogonComputeServerDigest(PrimaryDc,NonExist,MessageOne)\'" +
                            "");
                    temp367 = this.INrpcServerAdapterInstance.NetrLogonComputeServerDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.RidType)(0)), ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103755");
                    this.Manager.Checkpoint("MS-NRPC_R103743");
                    this.Manager.Comment("reaching state \'S933\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeServerDigest/ERROR_NO_SUCH_USER\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SUCH_USER, temp367, "return of NetrLogonComputeServerDigest, state S933");
                    Test_AuthenticateMessageS1217();
                    goto label104;
                }
                throw new InvalidOperationException("never reached");
            label104:
;
                goto label107;
            }
            if ((temp377 == 1)) {
                this.Manager.Comment("reaching state \'S177\'");
                bool temp369;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp369);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp369);
                this.Manager.Comment("reaching state \'S351\'");
                int temp372 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS40GetClientAccountTypeChecker2)), new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS40GetClientAccountTypeChecker3)));
                if ((temp372 == 0)) {
                    this.Manager.Comment("reaching state \'S586\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp370;
                    this.Manager.Comment("executing step \'call NetrLogonComputeClientDigest(NonDcServer,FqdnFormatDomainNam" +
                            "e,MessageOne)\'");
                    temp370 = this.INrpcServerAdapterInstance.NetrLogonComputeClientDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.FqdnFormatDomainName, ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103789");
                    this.Manager.Comment("reaching state \'S934\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeClientDigest/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp370, "return of NetrLogonComputeClientDigest, state S934");
                    Test_AuthenticateMessageS1244();
                    goto label105;
                }
                if ((temp372 == 1)) {
                    this.Manager.Comment("reaching state \'S587\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp371;
                    this.Manager.Comment("executing step \'call NetrLogonComputeClientDigest(NonDcServer,FqdnFormatDomainNam" +
                            "e,MessageOne)\'");
                    temp371 = this.INrpcServerAdapterInstance.NetrLogonComputeClientDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.FqdnFormatDomainName, ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103789");
                    this.Manager.Comment("reaching state \'S935\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeClientDigest/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp371, "return of NetrLogonComputeClientDigest, state S935");
                    Test_AuthenticateMessageS1245();
                    goto label105;
                }
                throw new InvalidOperationException("never reached");
            label105:
;
                goto label107;
            }
            if ((temp377 == 2)) {
                this.Manager.Comment("reaching state \'S178\'");
                bool temp373;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp373);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp373);
                this.Manager.Comment("reaching state \'S352\'");
                int temp376 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS40GetClientAccountTypeChecker4)), new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS40GetClientAccountTypeChecker5)));
                if ((temp376 == 0)) {
                    this.Manager.Comment("reaching state \'S588\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp374;
                    this.Manager.Comment("executing step \'call NetrLogonComputeServerDigest(PrimaryDc,RidOfNonMachineAccoun" +
                            "t,MessageOne)\'");
                    temp374 = this.INrpcServerAdapterInstance.NetrLogonComputeServerDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.RidType)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103756");
                    this.Manager.Checkpoint("MS-NRPC_R103743");
                    this.Manager.Comment("reaching state \'S936\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeServerDigest/ERROR_NO_SUCH_USER\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SUCH_USER, temp374, "return of NetrLogonComputeServerDigest, state S936");
                    this.Manager.Comment("reaching state \'S1260\'");
                    goto label106;
                }
                if ((temp376 == 1)) {
                    this.Manager.Comment("reaching state \'S589\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp375;
                    this.Manager.Comment("executing step \'call NetrLogonComputeServerDigest(PrimaryDc,RidOfNonMachineAccoun" +
                            "t,MessageOne)\'");
                    temp375 = this.INrpcServerAdapterInstance.NetrLogonComputeServerDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.RidType)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103756");
                    this.Manager.Checkpoint("MS-NRPC_R103743");
                    this.Manager.Comment("reaching state \'S937\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeServerDigest/ERROR_NO_SUCH_USER\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SUCH_USER, temp375, "return of NetrLogonComputeServerDigest, state S937");
                    this.Manager.Comment("reaching state \'S1261\'");
                    goto label106;
                }
                throw new InvalidOperationException("never reached");
            label106:
;
                goto label107;
            }
            throw new InvalidOperationException("never reached");
        label107:
;
            this.Manager.EndTest();
        }
        
        private void Test_AuthenticateMessageS40GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S41");
        }
        
        private void Test_AuthenticateMessageS40GetClientAccountTypeChecker(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S350");
        }
        
        private void Test_AuthenticateMessageS40GetClientAccountTypeChecker1(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S350");
        }
        
        private void Test_AuthenticateMessageS40GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S41");
        }
        
        private void Test_AuthenticateMessageS40GetClientAccountTypeChecker2(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S351");
        }
        
        private void Test_AuthenticateMessageS40GetClientAccountTypeChecker3(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S351");
        }
        
        private void Test_AuthenticateMessageS40GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S41");
        }
        
        private void Test_AuthenticateMessageS40GetClientAccountTypeChecker4(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S352");
        }
        
        private void Test_AuthenticateMessageS40GetClientAccountTypeChecker5(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S352");
        }
        #endregion
        
        #region Test Starting in S42
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_AuthenticateMessageS42() {
            this.Manager.BeginTest("Test_AuthenticateMessageS42");
            this.Manager.Comment("reaching state \'S42\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp378;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp378);
            this.Manager.AddReturn(GetPlatformInfo, null, temp378);
            this.Manager.Comment("reaching state \'S43\'");
            int temp391 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_AuthenticateMessageS42GetPlatformChecker)), new ExpectedReturn(Test_AuthenticateMessage.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_AuthenticateMessageS42GetPlatformChecker1)), new ExpectedReturn(Test_AuthenticateMessage.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_AuthenticateMessageS42GetPlatformChecker2)));
            if ((temp391 == 0)) {
                this.Manager.Comment("reaching state \'S179\'");
                bool temp379;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp379);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp379);
                this.Manager.Comment("reaching state \'S353\'");
                int temp382 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS42GetClientAccountTypeChecker)), new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS42GetClientAccountTypeChecker1)));
                if ((temp382 == 0)) {
                    this.Manager.Comment("reaching state \'S590\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp380;
                    this.Manager.Comment("executing step \'call NetrLogonComputeServerDigest(PrimaryDc,RidOfNonDcMachineAcco" +
                            "unt,MessageOne)\'");
                    temp380 = this.INrpcServerAdapterInstance.NetrLogonComputeServerDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.RidType.RidOfNonDcMachineAccount, ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103742");
                    this.Manager.Comment("reaching state \'S938\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeServerDigest/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp380, "return of NetrLogonComputeServerDigest, state S938");
                    Test_AuthenticateMessageS1216();
                    goto label108;
                }
                if ((temp382 == 1)) {
                    this.Manager.Comment("reaching state \'S591\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp381;
                    this.Manager.Comment("executing step \'call NetrLogonComputeServerDigest(PrimaryDc,RidOfNonDcMachineAcco" +
                            "unt,MessageOne)\'");
                    temp381 = this.INrpcServerAdapterInstance.NetrLogonComputeServerDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.RidType.RidOfNonDcMachineAccount, ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103742");
                    this.Manager.Comment("reaching state \'S939\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeServerDigest/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp381, "return of NetrLogonComputeServerDigest, state S939");
                    Test_AuthenticateMessageS1217();
                    goto label108;
                }
                throw new InvalidOperationException("never reached");
            label108:
;
                goto label111;
            }
            if ((temp391 == 1)) {
                this.Manager.Comment("reaching state \'S180\'");
                bool temp383;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp383);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp383);
                this.Manager.Comment("reaching state \'S354\'");
                int temp386 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS42GetClientAccountTypeChecker2)), new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS42GetClientAccountTypeChecker3)));
                if ((temp386 == 0)) {
                    this.Manager.Comment("reaching state \'S592\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp384;
                    this.Manager.Comment("executing step \'call NetrLogonComputeClientDigest(PrimaryDc,Null,MessageOne)\'");
                    temp384 = this.INrpcServerAdapterInstance.NetrLogonComputeClientDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.DomainNameType)(0)), ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103789");
                    this.Manager.Comment("reaching state \'S940\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeClientDigest/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp384, "return of NetrLogonComputeClientDigest, state S940");
                    Test_AuthenticateMessageS1244();
                    goto label109;
                }
                if ((temp386 == 1)) {
                    this.Manager.Comment("reaching state \'S593\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp385;
                    this.Manager.Comment("executing step \'call NetrLogonComputeClientDigest(PrimaryDc,Null,MessageOne)\'");
                    temp385 = this.INrpcServerAdapterInstance.NetrLogonComputeClientDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.DomainNameType)(0)), ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103789");
                    this.Manager.Comment("reaching state \'S941\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeClientDigest/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp385, "return of NetrLogonComputeClientDigest, state S941");
                    Test_AuthenticateMessageS1245();
                    goto label109;
                }
                throw new InvalidOperationException("never reached");
            label109:
;
                goto label111;
            }
            if ((temp391 == 2)) {
                this.Manager.Comment("reaching state \'S181\'");
                bool temp387;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp387);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp387);
                this.Manager.Comment("reaching state \'S355\'");
                int temp390 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS42GetClientAccountTypeChecker4)), new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS42GetClientAccountTypeChecker5)));
                if ((temp390 == 0)) {
                    this.Manager.Comment("reaching state \'S594\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp388;
                    this.Manager.Comment("executing step \'call NetrLogonComputeServerDigest(PrimaryDc,RidOfNonMachineAccoun" +
                            "t,MessageOne)\'");
                    temp388 = this.INrpcServerAdapterInstance.NetrLogonComputeServerDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.RidType)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103756");
                    this.Manager.Checkpoint("MS-NRPC_R103743");
                    this.Manager.Comment("reaching state \'S942\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeServerDigest/ERROR_NO_SUCH_USER\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SUCH_USER, temp388, "return of NetrLogonComputeServerDigest, state S942");
                    this.Manager.Comment("reaching state \'S1262\'");
                    goto label110;
                }
                if ((temp390 == 1)) {
                    this.Manager.Comment("reaching state \'S595\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp389;
                    this.Manager.Comment("executing step \'call NetrLogonComputeServerDigest(PrimaryDc,RidOfNonMachineAccoun" +
                            "t,MessageOne)\'");
                    temp389 = this.INrpcServerAdapterInstance.NetrLogonComputeServerDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.RidType)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103756");
                    this.Manager.Checkpoint("MS-NRPC_R103743");
                    this.Manager.Comment("reaching state \'S943\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeServerDigest/ERROR_NO_SUCH_USER\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SUCH_USER, temp389, "return of NetrLogonComputeServerDigest, state S943");
                    this.Manager.Comment("reaching state \'S1263\'");
                    goto label110;
                }
                throw new InvalidOperationException("never reached");
            label110:
;
                goto label111;
            }
            throw new InvalidOperationException("never reached");
        label111:
;
            this.Manager.EndTest();
        }
        
        private void Test_AuthenticateMessageS42GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S43");
        }
        
        private void Test_AuthenticateMessageS42GetClientAccountTypeChecker(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S353");
        }
        
        private void Test_AuthenticateMessageS42GetClientAccountTypeChecker1(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S353");
        }
        
        private void Test_AuthenticateMessageS42GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S43");
        }
        
        private void Test_AuthenticateMessageS42GetClientAccountTypeChecker2(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S354");
        }
        
        private void Test_AuthenticateMessageS42GetClientAccountTypeChecker3(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S354");
        }
        
        private void Test_AuthenticateMessageS42GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S43");
        }
        
        private void Test_AuthenticateMessageS42GetClientAccountTypeChecker4(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S355");
        }
        
        private void Test_AuthenticateMessageS42GetClientAccountTypeChecker5(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S355");
        }
        #endregion
        
        #region Test Starting in S44
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_AuthenticateMessageS44() {
            this.Manager.BeginTest("Test_AuthenticateMessageS44");
            this.Manager.Comment("reaching state \'S44\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp392;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp392);
            this.Manager.AddReturn(GetPlatformInfo, null, temp392);
            this.Manager.Comment("reaching state \'S45\'");
            int temp405 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_AuthenticateMessageS44GetPlatformChecker)), new ExpectedReturn(Test_AuthenticateMessage.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_AuthenticateMessageS44GetPlatformChecker1)), new ExpectedReturn(Test_AuthenticateMessage.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_AuthenticateMessageS44GetPlatformChecker2)));
            if ((temp405 == 0)) {
                this.Manager.Comment("reaching state \'S182\'");
                bool temp393;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp393);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp393);
                this.Manager.Comment("reaching state \'S356\'");
                int temp396 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS44GetClientAccountTypeChecker)), new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS44GetClientAccountTypeChecker1)));
                if ((temp396 == 0)) {
                    this.Manager.Comment("reaching state \'S596\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp394;
                    this.Manager.Comment("executing step \'call NetrLogonComputeClientDigest(NonDcServer,FqdnFormatDomainNam" +
                            "e,MessageOne)\'");
                    temp394 = this.INrpcServerAdapterInstance.NetrLogonComputeClientDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.FqdnFormatDomainName, ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103789");
                    this.Manager.Comment("reaching state \'S944\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeClientDigest/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp394, "return of NetrLogonComputeClientDigest, state S944");
                    Test_AuthenticateMessageS1216();
                    goto label112;
                }
                if ((temp396 == 1)) {
                    this.Manager.Comment("reaching state \'S597\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp395;
                    this.Manager.Comment("executing step \'call NetrLogonComputeClientDigest(NonDcServer,FqdnFormatDomainNam" +
                            "e,MessageOne)\'");
                    temp395 = this.INrpcServerAdapterInstance.NetrLogonComputeClientDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.FqdnFormatDomainName, ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103789");
                    this.Manager.Comment("reaching state \'S945\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeClientDigest/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp395, "return of NetrLogonComputeClientDigest, state S945");
                    Test_AuthenticateMessageS1217();
                    goto label112;
                }
                throw new InvalidOperationException("never reached");
            label112:
;
                goto label115;
            }
            if ((temp405 == 1)) {
                this.Manager.Comment("reaching state \'S183\'");
                bool temp397;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp397);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp397);
                this.Manager.Comment("reaching state \'S357\'");
                int temp400 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS44GetClientAccountTypeChecker2)), new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS44GetClientAccountTypeChecker3)));
                if ((temp400 == 0)) {
                    this.Manager.Comment("reaching state \'S598\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp398;
                    this.Manager.Comment("executing step \'call NetrLogonComputeClientDigest(PrimaryDc,NetBiosFormatDomainNa" +
                            "me,MessageOne)\'");
                    temp398 = this.INrpcServerAdapterInstance.NetrLogonComputeClientDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.NetBiosFormatDomainName, ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103789");
                    this.Manager.Comment("reaching state \'S946\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeClientDigest/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp398, "return of NetrLogonComputeClientDigest, state S946");
                    Test_AuthenticateMessageS1244();
                    goto label113;
                }
                if ((temp400 == 1)) {
                    this.Manager.Comment("reaching state \'S599\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp399;
                    this.Manager.Comment("executing step \'call NetrLogonComputeClientDigest(PrimaryDc,NetBiosFormatDomainNa" +
                            "me,MessageOne)\'");
                    temp399 = this.INrpcServerAdapterInstance.NetrLogonComputeClientDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.NetBiosFormatDomainName, ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103789");
                    this.Manager.Comment("reaching state \'S947\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeClientDigest/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp399, "return of NetrLogonComputeClientDigest, state S947");
                    Test_AuthenticateMessageS1245();
                    goto label113;
                }
                throw new InvalidOperationException("never reached");
            label113:
;
                goto label115;
            }
            if ((temp405 == 2)) {
                this.Manager.Comment("reaching state \'S184\'");
                bool temp401;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp401);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp401);
                this.Manager.Comment("reaching state \'S358\'");
                int temp404 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS44GetClientAccountTypeChecker4)), new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS44GetClientAccountTypeChecker5)));
                if ((temp404 == 0)) {
                    this.Manager.Comment("reaching state \'S600\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp402;
                    this.Manager.Comment("executing step \'call NetrLogonComputeServerDigest(PrimaryDc,RidOfNonMachineAccoun" +
                            "t,MessageOne)\'");
                    temp402 = this.INrpcServerAdapterInstance.NetrLogonComputeServerDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.RidType)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103756");
                    this.Manager.Checkpoint("MS-NRPC_R103743");
                    this.Manager.Comment("reaching state \'S948\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeServerDigest/ERROR_NO_SUCH_USER\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SUCH_USER, temp402, "return of NetrLogonComputeServerDigest, state S948");
                    this.Manager.Comment("reaching state \'S1264\'");
                    goto label114;
                }
                if ((temp404 == 1)) {
                    this.Manager.Comment("reaching state \'S601\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp403;
                    this.Manager.Comment("executing step \'call NetrLogonComputeServerDigest(PrimaryDc,RidOfNonMachineAccoun" +
                            "t,MessageOne)\'");
                    temp403 = this.INrpcServerAdapterInstance.NetrLogonComputeServerDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.RidType)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103756");
                    this.Manager.Checkpoint("MS-NRPC_R103743");
                    this.Manager.Comment("reaching state \'S949\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeServerDigest/ERROR_NO_SUCH_USER\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SUCH_USER, temp403, "return of NetrLogonComputeServerDigest, state S949");
                    this.Manager.Comment("reaching state \'S1265\'");
                    goto label114;
                }
                throw new InvalidOperationException("never reached");
            label114:
;
                goto label115;
            }
            throw new InvalidOperationException("never reached");
        label115:
;
            this.Manager.EndTest();
        }
        
        private void Test_AuthenticateMessageS44GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S45");
        }
        
        private void Test_AuthenticateMessageS44GetClientAccountTypeChecker(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S356");
        }
        
        private void Test_AuthenticateMessageS44GetClientAccountTypeChecker1(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S356");
        }
        
        private void Test_AuthenticateMessageS44GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S45");
        }
        
        private void Test_AuthenticateMessageS44GetClientAccountTypeChecker2(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S357");
        }
        
        private void Test_AuthenticateMessageS44GetClientAccountTypeChecker3(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S357");
        }
        
        private void Test_AuthenticateMessageS44GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S45");
        }
        
        private void Test_AuthenticateMessageS44GetClientAccountTypeChecker4(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S358");
        }
        
        private void Test_AuthenticateMessageS44GetClientAccountTypeChecker5(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S358");
        }
        #endregion
        
        #region Test Starting in S46
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_AuthenticateMessageS46() {
            this.Manager.BeginTest("Test_AuthenticateMessageS46");
            this.Manager.Comment("reaching state \'S46\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp406;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp406);
            this.Manager.AddReturn(GetPlatformInfo, null, temp406);
            this.Manager.Comment("reaching state \'S47\'");
            int temp419 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_AuthenticateMessageS46GetPlatformChecker)), new ExpectedReturn(Test_AuthenticateMessage.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_AuthenticateMessageS46GetPlatformChecker1)), new ExpectedReturn(Test_AuthenticateMessage.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_AuthenticateMessageS46GetPlatformChecker2)));
            if ((temp419 == 0)) {
                this.Manager.Comment("reaching state \'S185\'");
                bool temp407;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp407);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp407);
                this.Manager.Comment("reaching state \'S359\'");
                int temp410 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS46GetClientAccountTypeChecker)), new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS46GetClientAccountTypeChecker1)));
                if ((temp410 == 0)) {
                    this.Manager.Comment("reaching state \'S602\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp408;
                    this.Manager.Comment("executing step \'call NetrLogonComputeClientDigest(PrimaryDc,Null,MessageOne)\'");
                    temp408 = this.INrpcServerAdapterInstance.NetrLogonComputeClientDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.DomainNameType)(0)), ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103789");
                    this.Manager.Comment("reaching state \'S950\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeClientDigest/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp408, "return of NetrLogonComputeClientDigest, state S950");
                    Test_AuthenticateMessageS1216();
                    goto label116;
                }
                if ((temp410 == 1)) {
                    this.Manager.Comment("reaching state \'S603\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp409;
                    this.Manager.Comment("executing step \'call NetrLogonComputeClientDigest(PrimaryDc,Null,MessageOne)\'");
                    temp409 = this.INrpcServerAdapterInstance.NetrLogonComputeClientDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.DomainNameType)(0)), ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103789");
                    this.Manager.Comment("reaching state \'S951\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeClientDigest/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp409, "return of NetrLogonComputeClientDigest, state S951");
                    Test_AuthenticateMessageS1217();
                    goto label116;
                }
                throw new InvalidOperationException("never reached");
            label116:
;
                goto label119;
            }
            if ((temp419 == 1)) {
                this.Manager.Comment("reaching state \'S186\'");
                bool temp411;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp411);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp411);
                this.Manager.Comment("reaching state \'S360\'");
                int temp414 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS46GetClientAccountTypeChecker2)), new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS46GetClientAccountTypeChecker3)));
                if ((temp414 == 0)) {
                    this.Manager.Comment("reaching state \'S604\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp412;
                    this.Manager.Comment("executing step \'call NetrLogonComputeClientDigest(PrimaryDc,TrustedDomainName,Mes" +
                            "sageOne)\'");
                    temp412 = this.INrpcServerAdapterInstance.NetrLogonComputeClientDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.TrustedDomainName, ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103789");
                    this.Manager.Comment("reaching state \'S952\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeClientDigest/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp412, "return of NetrLogonComputeClientDigest, state S952");
                    Test_AuthenticateMessageS1244();
                    goto label117;
                }
                if ((temp414 == 1)) {
                    this.Manager.Comment("reaching state \'S605\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp413;
                    this.Manager.Comment("executing step \'call NetrLogonComputeClientDigest(PrimaryDc,TrustedDomainName,Mes" +
                            "sageOne)\'");
                    temp413 = this.INrpcServerAdapterInstance.NetrLogonComputeClientDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.TrustedDomainName, ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103789");
                    this.Manager.Comment("reaching state \'S953\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeClientDigest/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp413, "return of NetrLogonComputeClientDigest, state S953");
                    Test_AuthenticateMessageS1245();
                    goto label117;
                }
                throw new InvalidOperationException("never reached");
            label117:
;
                goto label119;
            }
            if ((temp419 == 2)) {
                this.Manager.Comment("reaching state \'S187\'");
                bool temp415;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp415);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp415);
                this.Manager.Comment("reaching state \'S361\'");
                int temp418 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS46GetClientAccountTypeChecker4)), new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS46GetClientAccountTypeChecker5)));
                if ((temp418 == 0)) {
                    this.Manager.Comment("reaching state \'S606\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp416;
                    this.Manager.Comment("executing step \'call NetrLogonComputeServerDigest(PrimaryDc,RidOfNonMachineAccoun" +
                            "t,MessageOne)\'");
                    temp416 = this.INrpcServerAdapterInstance.NetrLogonComputeServerDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.RidType)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103756");
                    this.Manager.Checkpoint("MS-NRPC_R103743");
                    this.Manager.Comment("reaching state \'S954\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeServerDigest/ERROR_NO_SUCH_USER\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SUCH_USER, temp416, "return of NetrLogonComputeServerDigest, state S954");
                    this.Manager.Comment("reaching state \'S1266\'");
                    goto label118;
                }
                if ((temp418 == 1)) {
                    this.Manager.Comment("reaching state \'S607\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp417;
                    this.Manager.Comment("executing step \'call NetrLogonComputeServerDigest(PrimaryDc,RidOfNonMachineAccoun" +
                            "t,MessageOne)\'");
                    temp417 = this.INrpcServerAdapterInstance.NetrLogonComputeServerDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.RidType)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103756");
                    this.Manager.Checkpoint("MS-NRPC_R103743");
                    this.Manager.Comment("reaching state \'S955\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeServerDigest/ERROR_NO_SUCH_USER\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SUCH_USER, temp417, "return of NetrLogonComputeServerDigest, state S955");
                    this.Manager.Comment("reaching state \'S1267\'");
                    goto label118;
                }
                throw new InvalidOperationException("never reached");
            label118:
;
                goto label119;
            }
            throw new InvalidOperationException("never reached");
        label119:
;
            this.Manager.EndTest();
        }
        
        private void Test_AuthenticateMessageS46GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S47");
        }
        
        private void Test_AuthenticateMessageS46GetClientAccountTypeChecker(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S359");
        }
        
        private void Test_AuthenticateMessageS46GetClientAccountTypeChecker1(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S359");
        }
        
        private void Test_AuthenticateMessageS46GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S47");
        }
        
        private void Test_AuthenticateMessageS46GetClientAccountTypeChecker2(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S360");
        }
        
        private void Test_AuthenticateMessageS46GetClientAccountTypeChecker3(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S360");
        }
        
        private void Test_AuthenticateMessageS46GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S47");
        }
        
        private void Test_AuthenticateMessageS46GetClientAccountTypeChecker4(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S361");
        }
        
        private void Test_AuthenticateMessageS46GetClientAccountTypeChecker5(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S361");
        }
        #endregion
        
        #region Test Starting in S48
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_AuthenticateMessageS48() {
            this.Manager.BeginTest("Test_AuthenticateMessageS48");
            this.Manager.Comment("reaching state \'S48\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp420;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp420);
            this.Manager.AddReturn(GetPlatformInfo, null, temp420);
            this.Manager.Comment("reaching state \'S49\'");
            int temp433 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_AuthenticateMessageS48GetPlatformChecker)), new ExpectedReturn(Test_AuthenticateMessage.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_AuthenticateMessageS48GetPlatformChecker1)), new ExpectedReturn(Test_AuthenticateMessage.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_AuthenticateMessageS48GetPlatformChecker2)));
            if ((temp433 == 0)) {
                this.Manager.Comment("reaching state \'S188\'");
                bool temp421;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp421);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp421);
                this.Manager.Comment("reaching state \'S362\'");
                int temp424 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS48GetClientAccountTypeChecker)), new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS48GetClientAccountTypeChecker1)));
                if ((temp424 == 0)) {
                    this.Manager.Comment("reaching state \'S608\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp422;
                    this.Manager.Comment("executing step \'call NetrLogonComputeClientDigest(PrimaryDc,NetBiosFormatDomainNa" +
                            "me,MessageOne)\'");
                    temp422 = this.INrpcServerAdapterInstance.NetrLogonComputeClientDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.NetBiosFormatDomainName, ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103789");
                    this.Manager.Comment("reaching state \'S956\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeClientDigest/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp422, "return of NetrLogonComputeClientDigest, state S956");
                    Test_AuthenticateMessageS1216();
                    goto label120;
                }
                if ((temp424 == 1)) {
                    this.Manager.Comment("reaching state \'S609\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp423;
                    this.Manager.Comment("executing step \'call NetrLogonComputeClientDigest(PrimaryDc,NetBiosFormatDomainNa" +
                            "me,MessageOne)\'");
                    temp423 = this.INrpcServerAdapterInstance.NetrLogonComputeClientDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.NetBiosFormatDomainName, ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103789");
                    this.Manager.Comment("reaching state \'S957\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeClientDigest/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp423, "return of NetrLogonComputeClientDigest, state S957");
                    Test_AuthenticateMessageS1217();
                    goto label120;
                }
                throw new InvalidOperationException("never reached");
            label120:
;
                goto label123;
            }
            if ((temp433 == 1)) {
                this.Manager.Comment("reaching state \'S189\'");
                bool temp425;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp425);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp425);
                this.Manager.Comment("reaching state \'S363\'");
                int temp428 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS48GetClientAccountTypeChecker2)), new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS48GetClientAccountTypeChecker3)));
                if ((temp428 == 0)) {
                    this.Manager.Comment("reaching state \'S610\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp426;
                    this.Manager.Comment("executing step \'call NetrLogonComputeClientDigest(PrimaryDc,FqdnFormatDomainName," +
                            "MessageOne)\'");
                    temp426 = this.INrpcServerAdapterInstance.NetrLogonComputeClientDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.FqdnFormatDomainName, ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103789");
                    this.Manager.Comment("reaching state \'S958\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeClientDigest/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp426, "return of NetrLogonComputeClientDigest, state S958");
                    Test_AuthenticateMessageS1244();
                    goto label121;
                }
                if ((temp428 == 1)) {
                    this.Manager.Comment("reaching state \'S611\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp427;
                    this.Manager.Comment("executing step \'call NetrLogonComputeClientDigest(PrimaryDc,FqdnFormatDomainName," +
                            "MessageOne)\'");
                    temp427 = this.INrpcServerAdapterInstance.NetrLogonComputeClientDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.FqdnFormatDomainName, ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103789");
                    this.Manager.Comment("reaching state \'S959\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeClientDigest/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp427, "return of NetrLogonComputeClientDigest, state S959");
                    Test_AuthenticateMessageS1245();
                    goto label121;
                }
                throw new InvalidOperationException("never reached");
            label121:
;
                goto label123;
            }
            if ((temp433 == 2)) {
                this.Manager.Comment("reaching state \'S190\'");
                bool temp429;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp429);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp429);
                this.Manager.Comment("reaching state \'S364\'");
                int temp432 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS48GetClientAccountTypeChecker4)), new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS48GetClientAccountTypeChecker5)));
                if ((temp432 == 0)) {
                    this.Manager.Comment("reaching state \'S612\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp430;
                    this.Manager.Comment("executing step \'call NetrLogonComputeServerDigest(PrimaryDc,RidOfNonMachineAccoun" +
                            "t,MessageOne)\'");
                    temp430 = this.INrpcServerAdapterInstance.NetrLogonComputeServerDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.RidType)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103756");
                    this.Manager.Checkpoint("MS-NRPC_R103743");
                    this.Manager.Comment("reaching state \'S960\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeServerDigest/ERROR_NO_SUCH_USER\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SUCH_USER, temp430, "return of NetrLogonComputeServerDigest, state S960");
                    this.Manager.Comment("reaching state \'S1268\'");
                    goto label122;
                }
                if ((temp432 == 1)) {
                    this.Manager.Comment("reaching state \'S613\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp431;
                    this.Manager.Comment("executing step \'call NetrLogonComputeServerDigest(PrimaryDc,RidOfNonMachineAccoun" +
                            "t,MessageOne)\'");
                    temp431 = this.INrpcServerAdapterInstance.NetrLogonComputeServerDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.RidType)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103756");
                    this.Manager.Checkpoint("MS-NRPC_R103743");
                    this.Manager.Comment("reaching state \'S961\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeServerDigest/ERROR_NO_SUCH_USER\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SUCH_USER, temp431, "return of NetrLogonComputeServerDigest, state S961");
                    this.Manager.Comment("reaching state \'S1269\'");
                    goto label122;
                }
                throw new InvalidOperationException("never reached");
            label122:
;
                goto label123;
            }
            throw new InvalidOperationException("never reached");
        label123:
;
            this.Manager.EndTest();
        }
        
        private void Test_AuthenticateMessageS48GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S49");
        }
        
        private void Test_AuthenticateMessageS48GetClientAccountTypeChecker(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S362");
        }
        
        private void Test_AuthenticateMessageS48GetClientAccountTypeChecker1(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S362");
        }
        
        private void Test_AuthenticateMessageS48GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S49");
        }
        
        private void Test_AuthenticateMessageS48GetClientAccountTypeChecker2(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S363");
        }
        
        private void Test_AuthenticateMessageS48GetClientAccountTypeChecker3(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S363");
        }
        
        private void Test_AuthenticateMessageS48GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S49");
        }
        
        private void Test_AuthenticateMessageS48GetClientAccountTypeChecker4(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S364");
        }
        
        private void Test_AuthenticateMessageS48GetClientAccountTypeChecker5(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S364");
        }
        #endregion
        
        #region Test Starting in S50
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_AuthenticateMessageS50() {
            this.Manager.BeginTest("Test_AuthenticateMessageS50");
            this.Manager.Comment("reaching state \'S50\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp434;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp434);
            this.Manager.AddReturn(GetPlatformInfo, null, temp434);
            this.Manager.Comment("reaching state \'S51\'");
            int temp447 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_AuthenticateMessageS50GetPlatformChecker)), new ExpectedReturn(Test_AuthenticateMessage.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_AuthenticateMessageS50GetPlatformChecker1)), new ExpectedReturn(Test_AuthenticateMessage.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_AuthenticateMessageS50GetPlatformChecker2)));
            if ((temp447 == 0)) {
                this.Manager.Comment("reaching state \'S191\'");
                bool temp435;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp435);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp435);
                this.Manager.Comment("reaching state \'S365\'");
                int temp438 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS50GetClientAccountTypeChecker)), new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS50GetClientAccountTypeChecker1)));
                if ((temp438 == 0)) {
                    this.Manager.Comment("reaching state \'S614\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp436;
                    this.Manager.Comment("executing step \'call NetrLogonComputeClientDigest(PrimaryDc,TrustedDomainName,Mes" +
                            "sageOne)\'");
                    temp436 = this.INrpcServerAdapterInstance.NetrLogonComputeClientDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.TrustedDomainName, ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103789");
                    this.Manager.Comment("reaching state \'S962\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeClientDigest/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp436, "return of NetrLogonComputeClientDigest, state S962");
                    Test_AuthenticateMessageS1216();
                    goto label124;
                }
                if ((temp438 == 1)) {
                    this.Manager.Comment("reaching state \'S615\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp437;
                    this.Manager.Comment("executing step \'call NetrLogonComputeClientDigest(PrimaryDc,TrustedDomainName,Mes" +
                            "sageOne)\'");
                    temp437 = this.INrpcServerAdapterInstance.NetrLogonComputeClientDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.TrustedDomainName, ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103789");
                    this.Manager.Comment("reaching state \'S963\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeClientDigest/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp437, "return of NetrLogonComputeClientDigest, state S963");
                    Test_AuthenticateMessageS1217();
                    goto label124;
                }
                throw new InvalidOperationException("never reached");
            label124:
;
                goto label127;
            }
            if ((temp447 == 1)) {
                this.Manager.Comment("reaching state \'S192\'");
                bool temp439;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp439);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp439);
                this.Manager.Comment("reaching state \'S366\'");
                int temp442 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS50GetClientAccountTypeChecker2)), new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS50GetClientAccountTypeChecker3)));
                if ((temp442 == 0)) {
                    this.Manager.Comment("reaching state \'S616\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp440;
                    this.Manager.Comment("executing step \'call NetrLogonComputeClientDigest(PrimaryDc,EmptyDomainName,Messa" +
                            "geOne)\'");
                    temp440 = this.INrpcServerAdapterInstance.NetrLogonComputeClientDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.DomainNameType)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103789");
                    this.Manager.Comment("reaching state \'S964\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeClientDigest/ERROR_NO_SUCH_DOMAIN\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SUCH_DOMAIN, temp440, "return of NetrLogonComputeClientDigest, state S964");
                    Test_AuthenticateMessageS1244();
                    goto label125;
                }
                if ((temp442 == 1)) {
                    this.Manager.Comment("reaching state \'S617\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp441;
                    this.Manager.Comment("executing step \'call NetrLogonComputeClientDigest(PrimaryDc,EmptyDomainName,Messa" +
                            "geOne)\'");
                    temp441 = this.INrpcServerAdapterInstance.NetrLogonComputeClientDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.DomainNameType)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103789");
                    this.Manager.Comment("reaching state \'S965\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeClientDigest/ERROR_NO_SUCH_DOMAIN\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SUCH_DOMAIN, temp441, "return of NetrLogonComputeClientDigest, state S965");
                    Test_AuthenticateMessageS1245();
                    goto label125;
                }
                throw new InvalidOperationException("never reached");
            label125:
;
                goto label127;
            }
            if ((temp447 == 2)) {
                this.Manager.Comment("reaching state \'S193\'");
                bool temp443;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp443);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp443);
                this.Manager.Comment("reaching state \'S367\'");
                int temp446 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS50GetClientAccountTypeChecker4)), new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS50GetClientAccountTypeChecker5)));
                if ((temp446 == 0)) {
                    this.Manager.Comment("reaching state \'S618\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp444;
                    this.Manager.Comment("executing step \'call NetrLogonComputeServerDigest(PrimaryDc,RidOfNonMachineAccoun" +
                            "t,MessageOne)\'");
                    temp444 = this.INrpcServerAdapterInstance.NetrLogonComputeServerDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.RidType)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103756");
                    this.Manager.Checkpoint("MS-NRPC_R103743");
                    this.Manager.Comment("reaching state \'S966\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeServerDigest/ERROR_NO_SUCH_USER\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SUCH_USER, temp444, "return of NetrLogonComputeServerDigest, state S966");
                    this.Manager.Comment("reaching state \'S1270\'");
                    goto label126;
                }
                if ((temp446 == 1)) {
                    this.Manager.Comment("reaching state \'S619\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp445;
                    this.Manager.Comment("executing step \'call NetrLogonComputeServerDigest(PrimaryDc,RidOfNonMachineAccoun" +
                            "t,MessageOne)\'");
                    temp445 = this.INrpcServerAdapterInstance.NetrLogonComputeServerDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.RidType)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103756");
                    this.Manager.Checkpoint("MS-NRPC_R103743");
                    this.Manager.Comment("reaching state \'S967\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeServerDigest/ERROR_NO_SUCH_USER\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SUCH_USER, temp445, "return of NetrLogonComputeServerDigest, state S967");
                    this.Manager.Comment("reaching state \'S1271\'");
                    goto label126;
                }
                throw new InvalidOperationException("never reached");
            label126:
;
                goto label127;
            }
            throw new InvalidOperationException("never reached");
        label127:
;
            this.Manager.EndTest();
        }
        
        private void Test_AuthenticateMessageS50GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S51");
        }
        
        private void Test_AuthenticateMessageS50GetClientAccountTypeChecker(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S365");
        }
        
        private void Test_AuthenticateMessageS50GetClientAccountTypeChecker1(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S365");
        }
        
        private void Test_AuthenticateMessageS50GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S51");
        }
        
        private void Test_AuthenticateMessageS50GetClientAccountTypeChecker2(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S366");
        }
        
        private void Test_AuthenticateMessageS50GetClientAccountTypeChecker3(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S366");
        }
        
        private void Test_AuthenticateMessageS50GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S51");
        }
        
        private void Test_AuthenticateMessageS50GetClientAccountTypeChecker4(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S367");
        }
        
        private void Test_AuthenticateMessageS50GetClientAccountTypeChecker5(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S367");
        }
        #endregion
        
        #region Test Starting in S52
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_AuthenticateMessageS52() {
            this.Manager.BeginTest("Test_AuthenticateMessageS52");
            this.Manager.Comment("reaching state \'S52\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp448;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp448);
            this.Manager.AddReturn(GetPlatformInfo, null, temp448);
            this.Manager.Comment("reaching state \'S53\'");
            int temp461 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_AuthenticateMessageS52GetPlatformChecker)), new ExpectedReturn(Test_AuthenticateMessage.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_AuthenticateMessageS52GetPlatformChecker1)), new ExpectedReturn(Test_AuthenticateMessage.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_AuthenticateMessageS52GetPlatformChecker2)));
            if ((temp461 == 0)) {
                this.Manager.Comment("reaching state \'S194\'");
                bool temp449;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp449);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp449);
                this.Manager.Comment("reaching state \'S368\'");
                int temp452 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS52GetClientAccountTypeChecker)), new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS52GetClientAccountTypeChecker1)));
                if ((temp452 == 0)) {
                    this.Manager.Comment("reaching state \'S620\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp450;
                    this.Manager.Comment("executing step \'call NetrLogonComputeClientDigest(PrimaryDc,FqdnFormatDomainName," +
                            "MessageOne)\'");
                    temp450 = this.INrpcServerAdapterInstance.NetrLogonComputeClientDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.FqdnFormatDomainName, ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103789");
                    this.Manager.Comment("reaching state \'S968\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeClientDigest/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp450, "return of NetrLogonComputeClientDigest, state S968");
                    Test_AuthenticateMessageS1216();
                    goto label128;
                }
                if ((temp452 == 1)) {
                    this.Manager.Comment("reaching state \'S621\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp451;
                    this.Manager.Comment("executing step \'call NetrLogonComputeClientDigest(PrimaryDc,FqdnFormatDomainName," +
                            "MessageOne)\'");
                    temp451 = this.INrpcServerAdapterInstance.NetrLogonComputeClientDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.FqdnFormatDomainName, ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103789");
                    this.Manager.Comment("reaching state \'S969\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeClientDigest/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp451, "return of NetrLogonComputeClientDigest, state S969");
                    Test_AuthenticateMessageS1217();
                    goto label128;
                }
                throw new InvalidOperationException("never reached");
            label128:
;
                goto label131;
            }
            if ((temp461 == 1)) {
                this.Manager.Comment("reaching state \'S195\'");
                bool temp453;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp453);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp453);
                this.Manager.Comment("reaching state \'S369\'");
                int temp456 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS52GetClientAccountTypeChecker2)), new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS52GetClientAccountTypeChecker3)));
                if ((temp456 == 0)) {
                    this.Manager.Comment("reaching state \'S622\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp454;
                    this.Manager.Comment("executing step \'call NetrLogonComputeClientDigest(PrimaryDc,InvalidDomainName,Mes" +
                            "sageOne)\'");
                    temp454 = this.INrpcServerAdapterInstance.NetrLogonComputeClientDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.InvalidDomainName, ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103789");
                    this.Manager.Comment("reaching state \'S970\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeClientDigest/ERROR_NO_SUCH_DOMAIN\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SUCH_DOMAIN, temp454, "return of NetrLogonComputeClientDigest, state S970");
                    Test_AuthenticateMessageS1244();
                    goto label129;
                }
                if ((temp456 == 1)) {
                    this.Manager.Comment("reaching state \'S623\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp455;
                    this.Manager.Comment("executing step \'call NetrLogonComputeClientDigest(PrimaryDc,InvalidDomainName,Mes" +
                            "sageOne)\'");
                    temp455 = this.INrpcServerAdapterInstance.NetrLogonComputeClientDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.InvalidDomainName, ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103789");
                    this.Manager.Comment("reaching state \'S971\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeClientDigest/ERROR_NO_SUCH_DOMAIN\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SUCH_DOMAIN, temp455, "return of NetrLogonComputeClientDigest, state S971");
                    Test_AuthenticateMessageS1245();
                    goto label129;
                }
                throw new InvalidOperationException("never reached");
            label129:
;
                goto label131;
            }
            if ((temp461 == 2)) {
                this.Manager.Comment("reaching state \'S196\'");
                bool temp457;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp457);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp457);
                this.Manager.Comment("reaching state \'S370\'");
                int temp460 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS52GetClientAccountTypeChecker4)), new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS52GetClientAccountTypeChecker5)));
                if ((temp460 == 0)) {
                    this.Manager.Comment("reaching state \'S624\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp458;
                    this.Manager.Comment("executing step \'call NetrLogonComputeServerDigest(PrimaryDc,RidOfNonMachineAccoun" +
                            "t,MessageOne)\'");
                    temp458 = this.INrpcServerAdapterInstance.NetrLogonComputeServerDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.RidType)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103756");
                    this.Manager.Checkpoint("MS-NRPC_R103743");
                    this.Manager.Comment("reaching state \'S972\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeServerDigest/ERROR_NO_SUCH_USER\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SUCH_USER, temp458, "return of NetrLogonComputeServerDigest, state S972");
                    this.Manager.Comment("reaching state \'S1272\'");
                    goto label130;
                }
                if ((temp460 == 1)) {
                    this.Manager.Comment("reaching state \'S625\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp459;
                    this.Manager.Comment("executing step \'call NetrLogonComputeServerDigest(PrimaryDc,RidOfNonMachineAccoun" +
                            "t,MessageOne)\'");
                    temp459 = this.INrpcServerAdapterInstance.NetrLogonComputeServerDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.RidType)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103756");
                    this.Manager.Checkpoint("MS-NRPC_R103743");
                    this.Manager.Comment("reaching state \'S973\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeServerDigest/ERROR_NO_SUCH_USER\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SUCH_USER, temp459, "return of NetrLogonComputeServerDigest, state S973");
                    this.Manager.Comment("reaching state \'S1273\'");
                    goto label130;
                }
                throw new InvalidOperationException("never reached");
            label130:
;
                goto label131;
            }
            throw new InvalidOperationException("never reached");
        label131:
;
            this.Manager.EndTest();
        }
        
        private void Test_AuthenticateMessageS52GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S53");
        }
        
        private void Test_AuthenticateMessageS52GetClientAccountTypeChecker(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S368");
        }
        
        private void Test_AuthenticateMessageS52GetClientAccountTypeChecker1(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S368");
        }
        
        private void Test_AuthenticateMessageS52GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S53");
        }
        
        private void Test_AuthenticateMessageS52GetClientAccountTypeChecker2(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S369");
        }
        
        private void Test_AuthenticateMessageS52GetClientAccountTypeChecker3(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S369");
        }
        
        private void Test_AuthenticateMessageS52GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S53");
        }
        
        private void Test_AuthenticateMessageS52GetClientAccountTypeChecker4(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S370");
        }
        
        private void Test_AuthenticateMessageS52GetClientAccountTypeChecker5(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S370");
        }
        #endregion
        
        #region Test Starting in S54
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_AuthenticateMessageS54() {
            this.Manager.BeginTest("Test_AuthenticateMessageS54");
            this.Manager.Comment("reaching state \'S54\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp462;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp462);
            this.Manager.AddReturn(GetPlatformInfo, null, temp462);
            this.Manager.Comment("reaching state \'S55\'");
            int temp475 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_AuthenticateMessageS54GetPlatformChecker)), new ExpectedReturn(Test_AuthenticateMessage.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_AuthenticateMessageS54GetPlatformChecker1)), new ExpectedReturn(Test_AuthenticateMessage.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_AuthenticateMessageS54GetPlatformChecker2)));
            if ((temp475 == 0)) {
                this.Manager.Comment("reaching state \'S197\'");
                bool temp463;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp463);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp463);
                this.Manager.Comment("reaching state \'S371\'");
                int temp466 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS54GetClientAccountTypeChecker)), new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS54GetClientAccountTypeChecker1)));
                if ((temp466 == 0)) {
                    this.Manager.Comment("reaching state \'S626\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp464;
                    this.Manager.Comment("executing step \'call NetrLogonComputeClientDigest(PrimaryDc,EmptyDomainName,Messa" +
                            "geOne)\'");
                    temp464 = this.INrpcServerAdapterInstance.NetrLogonComputeClientDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.DomainNameType)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103789");
                    this.Manager.Comment("reaching state \'S974\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeClientDigest/ERROR_NO_SUCH_DOMAIN\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SUCH_DOMAIN, temp464, "return of NetrLogonComputeClientDigest, state S974");
                    Test_AuthenticateMessageS1216();
                    goto label132;
                }
                if ((temp466 == 1)) {
                    this.Manager.Comment("reaching state \'S627\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp465;
                    this.Manager.Comment("executing step \'call NetrLogonComputeClientDigest(PrimaryDc,EmptyDomainName,Messa" +
                            "geOne)\'");
                    temp465 = this.INrpcServerAdapterInstance.NetrLogonComputeClientDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.DomainNameType)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103789");
                    this.Manager.Comment("reaching state \'S975\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeClientDigest/ERROR_NO_SUCH_DOMAIN\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SUCH_DOMAIN, temp465, "return of NetrLogonComputeClientDigest, state S975");
                    Test_AuthenticateMessageS1217();
                    goto label132;
                }
                throw new InvalidOperationException("never reached");
            label132:
;
                goto label135;
            }
            if ((temp475 == 1)) {
                this.Manager.Comment("reaching state \'S198\'");
                bool temp467;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp467);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp467);
                this.Manager.Comment("reaching state \'S372\'");
                int temp470 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS54GetClientAccountTypeChecker2)), new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS54GetClientAccountTypeChecker3)));
                if ((temp470 == 0)) {
                    this.Manager.Comment("reaching state \'S628\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp468;
                    this.Manager.Comment("executing step \'call NetrLogonComputeClientDigest(PrimaryDc,InvalidFormatDomainNa" +
                            "me,MessageOne)\'");
                    temp468 = this.INrpcServerAdapterInstance.NetrLogonComputeClientDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.InvalidFormatDomainName, ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103789");
                    this.Manager.Comment("reaching state \'S976\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeClientDigest/ERROR_NO_SUCH_DOMAIN\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SUCH_DOMAIN, temp468, "return of NetrLogonComputeClientDigest, state S976");
                    Test_AuthenticateMessageS1244();
                    goto label133;
                }
                if ((temp470 == 1)) {
                    this.Manager.Comment("reaching state \'S629\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp469;
                    this.Manager.Comment("executing step \'call NetrLogonComputeClientDigest(PrimaryDc,InvalidFormatDomainNa" +
                            "me,MessageOne)\'");
                    temp469 = this.INrpcServerAdapterInstance.NetrLogonComputeClientDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.InvalidFormatDomainName, ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103789");
                    this.Manager.Comment("reaching state \'S977\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeClientDigest/ERROR_NO_SUCH_DOMAIN\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SUCH_DOMAIN, temp469, "return of NetrLogonComputeClientDigest, state S977");
                    Test_AuthenticateMessageS1245();
                    goto label133;
                }
                throw new InvalidOperationException("never reached");
            label133:
;
                goto label135;
            }
            if ((temp475 == 2)) {
                this.Manager.Comment("reaching state \'S199\'");
                bool temp471;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp471);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp471);
                this.Manager.Comment("reaching state \'S373\'");
                int temp474 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS54GetClientAccountTypeChecker4)), new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS54GetClientAccountTypeChecker5)));
                if ((temp474 == 0)) {
                    this.Manager.Comment("reaching state \'S630\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp472;
                    this.Manager.Comment("executing step \'call NetrLogonComputeServerDigest(PrimaryDc,RidOfNonMachineAccoun" +
                            "t,MessageOne)\'");
                    temp472 = this.INrpcServerAdapterInstance.NetrLogonComputeServerDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.RidType)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103756");
                    this.Manager.Checkpoint("MS-NRPC_R103743");
                    this.Manager.Comment("reaching state \'S978\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeServerDigest/ERROR_NO_SUCH_USER\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SUCH_USER, temp472, "return of NetrLogonComputeServerDigest, state S978");
                    this.Manager.Comment("reaching state \'S1274\'");
                    goto label134;
                }
                if ((temp474 == 1)) {
                    this.Manager.Comment("reaching state \'S631\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp473;
                    this.Manager.Comment("executing step \'call NetrLogonComputeServerDigest(PrimaryDc,RidOfNonMachineAccoun" +
                            "t,MessageOne)\'");
                    temp473 = this.INrpcServerAdapterInstance.NetrLogonComputeServerDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.RidType)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103756");
                    this.Manager.Checkpoint("MS-NRPC_R103743");
                    this.Manager.Comment("reaching state \'S979\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeServerDigest/ERROR_NO_SUCH_USER\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SUCH_USER, temp473, "return of NetrLogonComputeServerDigest, state S979");
                    this.Manager.Comment("reaching state \'S1275\'");
                    goto label134;
                }
                throw new InvalidOperationException("never reached");
            label134:
;
                goto label135;
            }
            throw new InvalidOperationException("never reached");
        label135:
;
            this.Manager.EndTest();
        }
        
        private void Test_AuthenticateMessageS54GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S55");
        }
        
        private void Test_AuthenticateMessageS54GetClientAccountTypeChecker(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S371");
        }
        
        private void Test_AuthenticateMessageS54GetClientAccountTypeChecker1(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S371");
        }
        
        private void Test_AuthenticateMessageS54GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S55");
        }
        
        private void Test_AuthenticateMessageS54GetClientAccountTypeChecker2(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S372");
        }
        
        private void Test_AuthenticateMessageS54GetClientAccountTypeChecker3(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S372");
        }
        
        private void Test_AuthenticateMessageS54GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S55");
        }
        
        private void Test_AuthenticateMessageS54GetClientAccountTypeChecker4(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S373");
        }
        
        private void Test_AuthenticateMessageS54GetClientAccountTypeChecker5(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S373");
        }
        #endregion
        
        #region Test Starting in S56
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_AuthenticateMessageS56() {
            this.Manager.BeginTest("Test_AuthenticateMessageS56");
            this.Manager.Comment("reaching state \'S56\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp476;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp476);
            this.Manager.AddReturn(GetPlatformInfo, null, temp476);
            this.Manager.Comment("reaching state \'S57\'");
            int temp489 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_AuthenticateMessageS56GetPlatformChecker)), new ExpectedReturn(Test_AuthenticateMessage.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_AuthenticateMessageS56GetPlatformChecker1)), new ExpectedReturn(Test_AuthenticateMessage.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_AuthenticateMessageS56GetPlatformChecker2)));
            if ((temp489 == 0)) {
                this.Manager.Comment("reaching state \'S200\'");
                bool temp477;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp477);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp477);
                this.Manager.Comment("reaching state \'S374\'");
                int temp480 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS56GetClientAccountTypeChecker)), new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS56GetClientAccountTypeChecker1)));
                if ((temp480 == 0)) {
                    this.Manager.Comment("reaching state \'S632\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp478;
                    this.Manager.Comment("executing step \'call NetrLogonComputeClientDigest(PrimaryDc,InvalidDomainName,Mes" +
                            "sageOne)\'");
                    temp478 = this.INrpcServerAdapterInstance.NetrLogonComputeClientDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.InvalidDomainName, ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103789");
                    this.Manager.Comment("reaching state \'S980\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeClientDigest/ERROR_NO_SUCH_DOMAIN\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SUCH_DOMAIN, temp478, "return of NetrLogonComputeClientDigest, state S980");
                    Test_AuthenticateMessageS1216();
                    goto label136;
                }
                if ((temp480 == 1)) {
                    this.Manager.Comment("reaching state \'S633\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp479;
                    this.Manager.Comment("executing step \'call NetrLogonComputeClientDigest(PrimaryDc,InvalidDomainName,Mes" +
                            "sageOne)\'");
                    temp479 = this.INrpcServerAdapterInstance.NetrLogonComputeClientDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.InvalidDomainName, ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103789");
                    this.Manager.Comment("reaching state \'S981\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeClientDigest/ERROR_NO_SUCH_DOMAIN\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SUCH_DOMAIN, temp479, "return of NetrLogonComputeClientDigest, state S981");
                    Test_AuthenticateMessageS1217();
                    goto label136;
                }
                throw new InvalidOperationException("never reached");
            label136:
;
                goto label139;
            }
            if ((temp489 == 1)) {
                this.Manager.Comment("reaching state \'S201\'");
                bool temp481;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp481);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp481);
                this.Manager.Comment("reaching state \'S375\'");
                int temp484 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS56GetClientAccountTypeChecker2)), new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS56GetClientAccountTypeChecker3)));
                if ((temp484 == 0)) {
                    this.Manager.Comment("reaching state \'S634\'");
                    this.Manager.Comment("executing step \'call SwitchUserAccount(False)\'");
                    this.INrpcServerAdapterInstance.SwitchUserAccount(false);
                    this.Manager.Comment("reaching state \'S982\'");
                    this.Manager.Comment("checking step \'return SwitchUserAccount\'");
                    this.Manager.Comment("reaching state \'S1276\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp482;
                    this.Manager.Comment("executing step \'call NetrLogonComputeServerDigest(PrimaryDc,NonExist,MessageOne)\'" +
                            "");
                    temp482 = this.INrpcServerAdapterInstance.NetrLogonComputeServerDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.RidType)(0)), ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103746");
                    this.Manager.Checkpoint("MS-NRPC_R103743");
                    this.Manager.Comment("reaching state \'S1434\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeServerDigest/ERROR_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_ACCESS_DENIED, temp482, "return of NetrLogonComputeServerDigest, state S1434");
                    Test_AuthenticateMessageS1458();
                    goto label137;
                }
                if ((temp484 == 1)) {
                    this.Manager.Comment("reaching state \'S635\'");
                    this.Manager.Comment("executing step \'call SwitchUserAccount(False)\'");
                    this.INrpcServerAdapterInstance.SwitchUserAccount(false);
                    this.Manager.Comment("reaching state \'S983\'");
                    this.Manager.Comment("checking step \'return SwitchUserAccount\'");
                    this.Manager.Comment("reaching state \'S1277\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp483;
                    this.Manager.Comment("executing step \'call NetrLogonComputeServerDigest(PrimaryDc,NonExist,MessageOne)\'" +
                            "");
                    temp483 = this.INrpcServerAdapterInstance.NetrLogonComputeServerDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.RidType)(0)), ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103746");
                    this.Manager.Checkpoint("MS-NRPC_R103743");
                    this.Manager.Comment("reaching state \'S1435\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeServerDigest/ERROR_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_ACCESS_DENIED, temp483, "return of NetrLogonComputeServerDigest, state S1435");
                    Test_AuthenticateMessageS1459();
                    goto label137;
                }
                throw new InvalidOperationException("never reached");
            label137:
;
                goto label139;
            }
            if ((temp489 == 2)) {
                this.Manager.Comment("reaching state \'S202\'");
                bool temp485;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp485);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp485);
                this.Manager.Comment("reaching state \'S376\'");
                int temp488 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS56GetClientAccountTypeChecker4)), new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS56GetClientAccountTypeChecker5)));
                if ((temp488 == 0)) {
                    this.Manager.Comment("reaching state \'S636\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp486;
                    this.Manager.Comment("executing step \'call NetrLogonComputeServerDigest(PrimaryDc,RidOfNonMachineAccoun" +
                            "t,MessageOne)\'");
                    temp486 = this.INrpcServerAdapterInstance.NetrLogonComputeServerDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.RidType)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103756");
                    this.Manager.Checkpoint("MS-NRPC_R103743");
                    this.Manager.Comment("reaching state \'S984\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeServerDigest/ERROR_NO_SUCH_USER\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SUCH_USER, temp486, "return of NetrLogonComputeServerDigest, state S984");
                    this.Manager.Comment("reaching state \'S1278\'");
                    goto label138;
                }
                if ((temp488 == 1)) {
                    this.Manager.Comment("reaching state \'S637\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp487;
                    this.Manager.Comment("executing step \'call NetrLogonComputeServerDigest(PrimaryDc,RidOfNonMachineAccoun" +
                            "t,MessageOne)\'");
                    temp487 = this.INrpcServerAdapterInstance.NetrLogonComputeServerDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.RidType)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103756");
                    this.Manager.Checkpoint("MS-NRPC_R103743");
                    this.Manager.Comment("reaching state \'S985\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeServerDigest/ERROR_NO_SUCH_USER\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SUCH_USER, temp487, "return of NetrLogonComputeServerDigest, state S985");
                    this.Manager.Comment("reaching state \'S1279\'");
                    goto label138;
                }
                throw new InvalidOperationException("never reached");
            label138:
;
                goto label139;
            }
            throw new InvalidOperationException("never reached");
        label139:
;
            this.Manager.EndTest();
        }
        
        private void Test_AuthenticateMessageS56GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S57");
        }
        
        private void Test_AuthenticateMessageS56GetClientAccountTypeChecker(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S374");
        }
        
        private void Test_AuthenticateMessageS56GetClientAccountTypeChecker1(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S374");
        }
        
        private void Test_AuthenticateMessageS56GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S57");
        }
        
        private void Test_AuthenticateMessageS56GetClientAccountTypeChecker2(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S375");
        }
        
        private void Test_AuthenticateMessageS1458() {
            this.Manager.Comment("reaching state \'S1458\'");
            this.Manager.Comment("executing step \'call SwitchUserAccount(True)\'");
            this.INrpcServerAdapterInstance.SwitchUserAccount(true);
            this.Manager.Comment("reaching state \'S1476\'");
            this.Manager.Comment("checking step \'return SwitchUserAccount\'");
            Test_AuthenticateMessageS1284();
        }
        
        private void Test_AuthenticateMessageS56GetClientAccountTypeChecker3(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S375");
        }
        
        private void Test_AuthenticateMessageS1459() {
            this.Manager.Comment("reaching state \'S1459\'");
            this.Manager.Comment("executing step \'call SwitchUserAccount(True)\'");
            this.INrpcServerAdapterInstance.SwitchUserAccount(true);
            this.Manager.Comment("reaching state \'S1477\'");
            this.Manager.Comment("checking step \'return SwitchUserAccount\'");
            Test_AuthenticateMessageS1285();
        }
        
        private void Test_AuthenticateMessageS56GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S57");
        }
        
        private void Test_AuthenticateMessageS56GetClientAccountTypeChecker4(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S376");
        }
        
        private void Test_AuthenticateMessageS56GetClientAccountTypeChecker5(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S376");
        }
        #endregion
        
        #region Test Starting in S58
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_AuthenticateMessageS58() {
            this.Manager.BeginTest("Test_AuthenticateMessageS58");
            this.Manager.Comment("reaching state \'S58\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp490;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp490);
            this.Manager.AddReturn(GetPlatformInfo, null, temp490);
            this.Manager.Comment("reaching state \'S59\'");
            int temp503 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_AuthenticateMessageS58GetPlatformChecker)), new ExpectedReturn(Test_AuthenticateMessage.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_AuthenticateMessageS58GetPlatformChecker1)), new ExpectedReturn(Test_AuthenticateMessage.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_AuthenticateMessageS58GetPlatformChecker2)));
            if ((temp503 == 0)) {
                this.Manager.Comment("reaching state \'S203\'");
                bool temp491;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp491);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp491);
                this.Manager.Comment("reaching state \'S377\'");
                int temp494 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS58GetClientAccountTypeChecker)), new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS58GetClientAccountTypeChecker1)));
                if ((temp494 == 0)) {
                    this.Manager.Comment("reaching state \'S638\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp492;
                    this.Manager.Comment("executing step \'call NetrLogonComputeClientDigest(PrimaryDc,InvalidFormatDomainNa" +
                            "me,MessageOne)\'");
                    temp492 = this.INrpcServerAdapterInstance.NetrLogonComputeClientDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.InvalidFormatDomainName, ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103789");
                    this.Manager.Comment("reaching state \'S986\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeClientDigest/ERROR_NO_SUCH_DOMAIN\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SUCH_DOMAIN, temp492, "return of NetrLogonComputeClientDigest, state S986");
                    Test_AuthenticateMessageS1216();
                    goto label140;
                }
                if ((temp494 == 1)) {
                    this.Manager.Comment("reaching state \'S639\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp493;
                    this.Manager.Comment("executing step \'call NetrLogonComputeClientDigest(PrimaryDc,InvalidFormatDomainNa" +
                            "me,MessageOne)\'");
                    temp493 = this.INrpcServerAdapterInstance.NetrLogonComputeClientDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.InvalidFormatDomainName, ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103789");
                    this.Manager.Comment("reaching state \'S987\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeClientDigest/ERROR_NO_SUCH_DOMAIN\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SUCH_DOMAIN, temp493, "return of NetrLogonComputeClientDigest, state S987");
                    Test_AuthenticateMessageS1217();
                    goto label140;
                }
                throw new InvalidOperationException("never reached");
            label140:
;
                goto label143;
            }
            if ((temp503 == 1)) {
                this.Manager.Comment("reaching state \'S204\'");
                bool temp495;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp495);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp495);
                this.Manager.Comment("reaching state \'S378\'");
                int temp498 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS58GetClientAccountTypeChecker2)), new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS58GetClientAccountTypeChecker3)));
                if ((temp498 == 0)) {
                    this.Manager.Comment("reaching state \'S640\'");
                    this.Manager.Comment("executing step \'call SwitchUserAccount(False)\'");
                    this.INrpcServerAdapterInstance.SwitchUserAccount(false);
                    this.Manager.Comment("reaching state \'S988\'");
                    this.Manager.Comment("checking step \'return SwitchUserAccount\'");
                    this.Manager.Comment("reaching state \'S1280\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp496;
                    this.Manager.Comment("executing step \'call NetrLogonComputeServerDigest(PrimaryDc,RidOfNonMachineAccoun" +
                            "t,MessageOne)\'");
                    temp496 = this.INrpcServerAdapterInstance.NetrLogonComputeServerDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.RidType)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103746");
                    this.Manager.Checkpoint("MS-NRPC_R103743");
                    this.Manager.Comment("reaching state \'S1436\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeServerDigest/ERROR_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_ACCESS_DENIED, temp496, "return of NetrLogonComputeServerDigest, state S1436");
                    Test_AuthenticateMessageS1458();
                    goto label141;
                }
                if ((temp498 == 1)) {
                    this.Manager.Comment("reaching state \'S641\'");
                    this.Manager.Comment("executing step \'call SwitchUserAccount(False)\'");
                    this.INrpcServerAdapterInstance.SwitchUserAccount(false);
                    this.Manager.Comment("reaching state \'S989\'");
                    this.Manager.Comment("checking step \'return SwitchUserAccount\'");
                    this.Manager.Comment("reaching state \'S1281\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp497;
                    this.Manager.Comment("executing step \'call NetrLogonComputeServerDigest(PrimaryDc,RidOfNonMachineAccoun" +
                            "t,MessageOne)\'");
                    temp497 = this.INrpcServerAdapterInstance.NetrLogonComputeServerDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.RidType)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103746");
                    this.Manager.Checkpoint("MS-NRPC_R103743");
                    this.Manager.Comment("reaching state \'S1437\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeServerDigest/ERROR_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_ACCESS_DENIED, temp497, "return of NetrLogonComputeServerDigest, state S1437");
                    Test_AuthenticateMessageS1459();
                    goto label141;
                }
                throw new InvalidOperationException("never reached");
            label141:
;
                goto label143;
            }
            if ((temp503 == 2)) {
                this.Manager.Comment("reaching state \'S205\'");
                bool temp499;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp499);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp499);
                this.Manager.Comment("reaching state \'S379\'");
                int temp502 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS58GetClientAccountTypeChecker4)), new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS58GetClientAccountTypeChecker5)));
                if ((temp502 == 0)) {
                    this.Manager.Comment("reaching state \'S642\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp500;
                    this.Manager.Comment("executing step \'call NetrLogonComputeServerDigest(PrimaryDc,RidOfNonMachineAccoun" +
                            "t,MessageOne)\'");
                    temp500 = this.INrpcServerAdapterInstance.NetrLogonComputeServerDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.RidType)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103756");
                    this.Manager.Checkpoint("MS-NRPC_R103743");
                    this.Manager.Comment("reaching state \'S990\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeServerDigest/ERROR_NO_SUCH_USER\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SUCH_USER, temp500, "return of NetrLogonComputeServerDigest, state S990");
                    this.Manager.Comment("reaching state \'S1282\'");
                    goto label142;
                }
                if ((temp502 == 1)) {
                    this.Manager.Comment("reaching state \'S643\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp501;
                    this.Manager.Comment("executing step \'call NetrLogonComputeServerDigest(PrimaryDc,RidOfNonMachineAccoun" +
                            "t,MessageOne)\'");
                    temp501 = this.INrpcServerAdapterInstance.NetrLogonComputeServerDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.RidType)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103756");
                    this.Manager.Checkpoint("MS-NRPC_R103743");
                    this.Manager.Comment("reaching state \'S991\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeServerDigest/ERROR_NO_SUCH_USER\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SUCH_USER, temp501, "return of NetrLogonComputeServerDigest, state S991");
                    this.Manager.Comment("reaching state \'S1283\'");
                    goto label142;
                }
                throw new InvalidOperationException("never reached");
            label142:
;
                goto label143;
            }
            throw new InvalidOperationException("never reached");
        label143:
;
            this.Manager.EndTest();
        }
        
        private void Test_AuthenticateMessageS58GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S59");
        }
        
        private void Test_AuthenticateMessageS58GetClientAccountTypeChecker(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S377");
        }
        
        private void Test_AuthenticateMessageS58GetClientAccountTypeChecker1(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S377");
        }
        
        private void Test_AuthenticateMessageS58GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S59");
        }
        
        private void Test_AuthenticateMessageS58GetClientAccountTypeChecker2(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S378");
        }
        
        private void Test_AuthenticateMessageS58GetClientAccountTypeChecker3(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S378");
        }
        
        private void Test_AuthenticateMessageS58GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S59");
        }
        
        private void Test_AuthenticateMessageS58GetClientAccountTypeChecker4(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S379");
        }
        
        private void Test_AuthenticateMessageS58GetClientAccountTypeChecker5(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S379");
        }
        #endregion
        
        #region Test Starting in S6
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_AuthenticateMessageS6() {
            this.Manager.BeginTest("Test_AuthenticateMessageS6");
            this.Manager.Comment("reaching state \'S6\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp504;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp504);
            this.Manager.AddReturn(GetPlatformInfo, null, temp504);
            this.Manager.Comment("reaching state \'S7\'");
            int temp517 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_AuthenticateMessageS6GetPlatformChecker)), new ExpectedReturn(Test_AuthenticateMessage.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_AuthenticateMessageS6GetPlatformChecker1)), new ExpectedReturn(Test_AuthenticateMessage.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_AuthenticateMessageS6GetPlatformChecker2)));
            if ((temp517 == 0)) {
                this.Manager.Comment("reaching state \'S125\'");
                bool temp505;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp505);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp505);
                this.Manager.Comment("reaching state \'S299\'");
                int temp508 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS6GetClientAccountTypeChecker)), new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS6GetClientAccountTypeChecker1)));
                if ((temp508 == 0)) {
                    this.Manager.Comment("reaching state \'S482\'");
                    this.Manager.Comment("executing step \'call SwitchUserAccount(False)\'");
                    this.INrpcServerAdapterInstance.SwitchUserAccount(false);
                    this.Manager.Comment("reaching state \'S830\'");
                    this.Manager.Comment("checking step \'return SwitchUserAccount\'");
                    this.Manager.Comment("reaching state \'S1178\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp506;
                    this.Manager.Comment("executing step \'call NetrLogonComputeClientDigest(PrimaryDc,InvalidDomainName,Mes" +
                            "sageOne)\'");
                    temp506 = this.INrpcServerAdapterInstance.NetrLogonComputeClientDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.InvalidDomainName, ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103791");
                    this.Manager.Checkpoint("MS-NRPC_R103790");
                    this.Manager.Comment("reaching state \'S1390\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeClientDigest/ERROR_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_ACCESS_DENIED, temp506, "return of NetrLogonComputeClientDigest, state S1390");
                    Test_AuthenticateMessageS1448();
                    goto label144;
                }
                if ((temp508 == 1)) {
                    this.Manager.Comment("reaching state \'S483\'");
                    this.Manager.Comment("executing step \'call SwitchUserAccount(False)\'");
                    this.INrpcServerAdapterInstance.SwitchUserAccount(false);
                    this.Manager.Comment("reaching state \'S831\'");
                    this.Manager.Comment("checking step \'return SwitchUserAccount\'");
                    this.Manager.Comment("reaching state \'S1179\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp507;
                    this.Manager.Comment("executing step \'call NetrLogonComputeClientDigest(PrimaryDc,InvalidDomainName,Mes" +
                            "sageOne)\'");
                    temp507 = this.INrpcServerAdapterInstance.NetrLogonComputeClientDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.InvalidDomainName, ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103791");
                    this.Manager.Checkpoint("MS-NRPC_R103790");
                    this.Manager.Comment("reaching state \'S1391\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeClientDigest/ERROR_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_ACCESS_DENIED, temp507, "return of NetrLogonComputeClientDigest, state S1391");
                    Test_AuthenticateMessageS1449();
                    goto label144;
                }
                throw new InvalidOperationException("never reached");
            label144:
;
                goto label147;
            }
            if ((temp517 == 1)) {
                this.Manager.Comment("reaching state \'S126\'");
                bool temp509;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp509);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp509);
                this.Manager.Comment("reaching state \'S300\'");
                int temp512 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS6GetClientAccountTypeChecker2)), new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS6GetClientAccountTypeChecker3)));
                if ((temp512 == 0)) {
                    this.Manager.Comment("reaching state \'S484\'");
                    this.Manager.Comment("executing step \'call SwitchUserAccount(False)\'");
                    this.INrpcServerAdapterInstance.SwitchUserAccount(false);
                    this.Manager.Comment("reaching state \'S832\'");
                    this.Manager.Comment("checking step \'return SwitchUserAccount\'");
                    this.Manager.Comment("reaching state \'S1180\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp510;
                    this.Manager.Comment("executing step \'call NetrLogonComputeClientDigest(PrimaryDc,InvalidDomainName,Mes" +
                            "sageOne)\'");
                    temp510 = this.INrpcServerAdapterInstance.NetrLogonComputeClientDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.InvalidDomainName, ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103791");
                    this.Manager.Checkpoint("MS-NRPC_R103790");
                    this.Manager.Comment("reaching state \'S1392\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeClientDigest/ERROR_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_ACCESS_DENIED, temp510, "return of NetrLogonComputeClientDigest, state S1392");
                    Test_AuthenticateMessageS1450();
                    goto label145;
                }
                if ((temp512 == 1)) {
                    this.Manager.Comment("reaching state \'S485\'");
                    this.Manager.Comment("executing step \'call SwitchUserAccount(False)\'");
                    this.INrpcServerAdapterInstance.SwitchUserAccount(false);
                    this.Manager.Comment("reaching state \'S833\'");
                    this.Manager.Comment("checking step \'return SwitchUserAccount\'");
                    this.Manager.Comment("reaching state \'S1181\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp511;
                    this.Manager.Comment("executing step \'call NetrLogonComputeClientDigest(PrimaryDc,InvalidDomainName,Mes" +
                            "sageOne)\'");
                    temp511 = this.INrpcServerAdapterInstance.NetrLogonComputeClientDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.InvalidDomainName, ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103791");
                    this.Manager.Checkpoint("MS-NRPC_R103790");
                    this.Manager.Comment("reaching state \'S1393\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeClientDigest/ERROR_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_ACCESS_DENIED, temp511, "return of NetrLogonComputeClientDigest, state S1393");
                    Test_AuthenticateMessageS1451();
                    goto label145;
                }
                throw new InvalidOperationException("never reached");
            label145:
;
                goto label147;
            }
            if ((temp517 == 2)) {
                this.Manager.Comment("reaching state \'S127\'");
                bool temp513;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp513);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp513);
                this.Manager.Comment("reaching state \'S301\'");
                int temp516 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS6GetClientAccountTypeChecker4)), new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS6GetClientAccountTypeChecker5)));
                if ((temp516 == 0)) {
                    this.Manager.Comment("reaching state \'S486\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp514;
                    this.Manager.Comment("executing step \'call NetrLogonComputeServerDigest(PrimaryDc,RidOfNonMachineAccoun" +
                            "t,MessageOne)\'");
                    temp514 = this.INrpcServerAdapterInstance.NetrLogonComputeServerDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.RidType)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103756");
                    this.Manager.Checkpoint("MS-NRPC_R103743");
                    this.Manager.Comment("reaching state \'S834\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeServerDigest/ERROR_NO_SUCH_USER\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SUCH_USER, temp514, "return of NetrLogonComputeServerDigest, state S834");
                    this.Manager.Comment("reaching state \'S1182\'");
                    goto label146;
                }
                if ((temp516 == 1)) {
                    this.Manager.Comment("reaching state \'S487\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp515;
                    this.Manager.Comment("executing step \'call NetrLogonComputeServerDigest(PrimaryDc,RidOfNonMachineAccoun" +
                            "t,MessageOne)\'");
                    temp515 = this.INrpcServerAdapterInstance.NetrLogonComputeServerDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.RidType)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103756");
                    this.Manager.Checkpoint("MS-NRPC_R103743");
                    this.Manager.Comment("reaching state \'S835\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeServerDigest/ERROR_NO_SUCH_USER\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SUCH_USER, temp515, "return of NetrLogonComputeServerDigest, state S835");
                    this.Manager.Comment("reaching state \'S1183\'");
                    goto label146;
                }
                throw new InvalidOperationException("never reached");
            label146:
;
                goto label147;
            }
            throw new InvalidOperationException("never reached");
        label147:
;
            this.Manager.EndTest();
        }
        
        private void Test_AuthenticateMessageS6GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S7");
        }
        
        private void Test_AuthenticateMessageS6GetClientAccountTypeChecker(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S299");
        }
        
        private void Test_AuthenticateMessageS6GetClientAccountTypeChecker1(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S299");
        }
        
        private void Test_AuthenticateMessageS6GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S7");
        }
        
        private void Test_AuthenticateMessageS6GetClientAccountTypeChecker2(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S300");
        }
        
        private void Test_AuthenticateMessageS6GetClientAccountTypeChecker3(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S300");
        }
        
        private void Test_AuthenticateMessageS6GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S7");
        }
        
        private void Test_AuthenticateMessageS6GetClientAccountTypeChecker4(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S301");
        }
        
        private void Test_AuthenticateMessageS6GetClientAccountTypeChecker5(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S301");
        }
        #endregion
        
        #region Test Starting in S60
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_AuthenticateMessageS60() {
            this.Manager.BeginTest("Test_AuthenticateMessageS60");
            this.Manager.Comment("reaching state \'S60\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp518;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp518);
            this.Manager.AddReturn(GetPlatformInfo, null, temp518);
            this.Manager.Comment("reaching state \'S61\'");
            int temp531 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_AuthenticateMessageS60GetPlatformChecker)), new ExpectedReturn(Test_AuthenticateMessage.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_AuthenticateMessageS60GetPlatformChecker1)), new ExpectedReturn(Test_AuthenticateMessage.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_AuthenticateMessageS60GetPlatformChecker2)));
            if ((temp531 == 0)) {
                this.Manager.Comment("reaching state \'S206\'");
                bool temp519;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp519);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp519);
                this.Manager.Comment("reaching state \'S380\'");
                int temp522 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS60GetClientAccountTypeChecker)), new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS60GetClientAccountTypeChecker1)));
                if ((temp522 == 0)) {
                    this.Manager.Comment("reaching state \'S644\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp520;
                    this.Manager.Comment("executing step \'call NetrLogonSetServiceBits(PrimaryDc,0,0)\'");
                    temp520 = this.INrpcServerAdapterInstance.NetrLogonSetServiceBits(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 0u, 0u);
                    this.Manager.Comment("reaching state \'S992\'");
                    this.Manager.Comment("checking step \'return NetrLogonSetServiceBits/ERROR_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_ACCESS_DENIED, temp520, "return of NetrLogonSetServiceBits, state S992");
                    Test_AuthenticateMessageS1216();
                    goto label148;
                }
                if ((temp522 == 1)) {
                    this.Manager.Comment("reaching state \'S645\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp521;
                    this.Manager.Comment("executing step \'call NetrLogonSetServiceBits(PrimaryDc,512,8192)\'");
                    temp521 = this.INrpcServerAdapterInstance.NetrLogonSetServiceBits(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 512u, 8192u);
                    this.Manager.Comment("reaching state \'S993\'");
                    this.Manager.Comment("checking step \'return NetrLogonSetServiceBits/STATUS_INVALID_PARAMETER\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_INVALID_PARAMETER, temp521, "return of NetrLogonSetServiceBits, state S993");
                    Test_AuthenticateMessageS1217();
                    goto label148;
                }
                throw new InvalidOperationException("never reached");
            label148:
;
                goto label151;
            }
            if ((temp531 == 1)) {
                this.Manager.Comment("reaching state \'S207\'");
                bool temp523;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp523);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp523);
                this.Manager.Comment("reaching state \'S381\'");
                int temp526 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS60GetClientAccountTypeChecker2)), new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS60GetClientAccountTypeChecker3)));
                if ((temp526 == 0)) {
                    this.Manager.Comment("reaching state \'S646\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp524;
                    this.Manager.Comment("executing step \'call NetrLogonComputeServerDigest(PrimaryDc,RidOfNonMachineAccoun" +
                            "t,MessageOne)\'");
                    temp524 = this.INrpcServerAdapterInstance.NetrLogonComputeServerDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.RidType)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103756");
                    this.Manager.Checkpoint("MS-NRPC_R103743");
                    this.Manager.Comment("reaching state \'S994\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeServerDigest/ERROR_NO_SUCH_USER\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SUCH_USER, temp524, "return of NetrLogonComputeServerDigest, state S994");
                    Test_AuthenticateMessageS1284();
                    goto label149;
                }
                if ((temp526 == 1)) {
                    this.Manager.Comment("reaching state \'S647\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp525;
                    this.Manager.Comment("executing step \'call NetrLogonComputeServerDigest(PrimaryDc,RidOfNonMachineAccoun" +
                            "t,MessageOne)\'");
                    temp525 = this.INrpcServerAdapterInstance.NetrLogonComputeServerDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.RidType)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103756");
                    this.Manager.Checkpoint("MS-NRPC_R103743");
                    this.Manager.Comment("reaching state \'S995\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeServerDigest/ERROR_NO_SUCH_USER\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SUCH_USER, temp525, "return of NetrLogonComputeServerDigest, state S995");
                    Test_AuthenticateMessageS1285();
                    goto label149;
                }
                throw new InvalidOperationException("never reached");
            label149:
;
                goto label151;
            }
            if ((temp531 == 2)) {
                this.Manager.Comment("reaching state \'S208\'");
                bool temp527;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp527);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp527);
                this.Manager.Comment("reaching state \'S382\'");
                int temp530 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS60GetClientAccountTypeChecker4)), new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS60GetClientAccountTypeChecker5)));
                if ((temp530 == 0)) {
                    this.Manager.Comment("reaching state \'S648\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp528;
                    this.Manager.Comment("executing step \'call NetrLogonComputeServerDigest(PrimaryDc,RidOfNonMachineAccoun" +
                            "t,MessageOne)\'");
                    temp528 = this.INrpcServerAdapterInstance.NetrLogonComputeServerDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.RidType)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103756");
                    this.Manager.Checkpoint("MS-NRPC_R103743");
                    this.Manager.Comment("reaching state \'S996\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeServerDigest/ERROR_NO_SUCH_USER\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SUCH_USER, temp528, "return of NetrLogonComputeServerDigest, state S996");
                    this.Manager.Comment("reaching state \'S1286\'");
                    goto label150;
                }
                if ((temp530 == 1)) {
                    this.Manager.Comment("reaching state \'S649\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp529;
                    this.Manager.Comment("executing step \'call NetrLogonComputeServerDigest(PrimaryDc,RidOfNonMachineAccoun" +
                            "t,MessageOne)\'");
                    temp529 = this.INrpcServerAdapterInstance.NetrLogonComputeServerDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.RidType)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103756");
                    this.Manager.Checkpoint("MS-NRPC_R103743");
                    this.Manager.Comment("reaching state \'S997\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeServerDigest/ERROR_NO_SUCH_USER\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SUCH_USER, temp529, "return of NetrLogonComputeServerDigest, state S997");
                    this.Manager.Comment("reaching state \'S1287\'");
                    goto label150;
                }
                throw new InvalidOperationException("never reached");
            label150:
;
                goto label151;
            }
            throw new InvalidOperationException("never reached");
        label151:
;
            this.Manager.EndTest();
        }
        
        private void Test_AuthenticateMessageS60GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S61");
        }
        
        private void Test_AuthenticateMessageS60GetClientAccountTypeChecker(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S380");
        }
        
        private void Test_AuthenticateMessageS60GetClientAccountTypeChecker1(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S380");
        }
        
        private void Test_AuthenticateMessageS60GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S61");
        }
        
        private void Test_AuthenticateMessageS60GetClientAccountTypeChecker2(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S381");
        }
        
        private void Test_AuthenticateMessageS60GetClientAccountTypeChecker3(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S381");
        }
        
        private void Test_AuthenticateMessageS60GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S61");
        }
        
        private void Test_AuthenticateMessageS60GetClientAccountTypeChecker4(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S382");
        }
        
        private void Test_AuthenticateMessageS60GetClientAccountTypeChecker5(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S382");
        }
        #endregion
        
        #region Test Starting in S62
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_AuthenticateMessageS62() {
            this.Manager.BeginTest("Test_AuthenticateMessageS62");
            this.Manager.Comment("reaching state \'S62\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp532;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp532);
            this.Manager.AddReturn(GetPlatformInfo, null, temp532);
            this.Manager.Comment("reaching state \'S63\'");
            int temp545 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_AuthenticateMessageS62GetPlatformChecker)), new ExpectedReturn(Test_AuthenticateMessage.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_AuthenticateMessageS62GetPlatformChecker1)), new ExpectedReturn(Test_AuthenticateMessage.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_AuthenticateMessageS62GetPlatformChecker2)));
            if ((temp545 == 0)) {
                this.Manager.Comment("reaching state \'S209\'");
                bool temp533;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp533);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp533);
                this.Manager.Comment("reaching state \'S383\'");
                int temp536 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS62GetClientAccountTypeChecker)), new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS62GetClientAccountTypeChecker1)));
                if ((temp536 == 0)) {
                    this.Manager.Comment("reaching state \'S650\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp534;
                    this.Manager.Comment("executing step \'call NetrLogonSetServiceBits(PrimaryDc,64,0)\'");
                    temp534 = this.INrpcServerAdapterInstance.NetrLogonSetServiceBits(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 64u, 0u);
                    this.Manager.Comment("reaching state \'S998\'");
                    this.Manager.Comment("checking step \'return NetrLogonSetServiceBits/ERROR_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_ACCESS_DENIED, temp534, "return of NetrLogonSetServiceBits, state S998");
                    Test_AuthenticateMessageS1216();
                    goto label152;
                }
                if ((temp536 == 1)) {
                    this.Manager.Comment("reaching state \'S651\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp535;
                    this.Manager.Comment("executing step \'call NetrLogonSetServiceBits(PrimaryDc,512,1)\'");
                    temp535 = this.INrpcServerAdapterInstance.NetrLogonSetServiceBits(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 512u, 1u);
                    this.Manager.Comment("reaching state \'S999\'");
                    this.Manager.Comment("checking step \'return NetrLogonSetServiceBits/STATUS_INVALID_PARAMETER\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_INVALID_PARAMETER, temp535, "return of NetrLogonSetServiceBits, state S999");
                    Test_AuthenticateMessageS1217();
                    goto label152;
                }
                throw new InvalidOperationException("never reached");
            label152:
;
                goto label155;
            }
            if ((temp545 == 1)) {
                this.Manager.Comment("reaching state \'S210\'");
                bool temp537;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp537);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp537);
                this.Manager.Comment("reaching state \'S384\'");
                int temp540 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS62GetClientAccountTypeChecker2)), new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS62GetClientAccountTypeChecker3)));
                if ((temp540 == 0)) {
                    this.Manager.Comment("reaching state \'S652\'");
                    this.Manager.Comment("executing step \'call SwitchUserAccount(False)\'");
                    this.INrpcServerAdapterInstance.SwitchUserAccount(false);
                    this.Manager.Comment("reaching state \'S1000\'");
                    this.Manager.Comment("checking step \'return SwitchUserAccount\'");
                    this.Manager.Comment("reaching state \'S1288\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp538;
                    this.Manager.Comment("executing step \'call NetrLogonComputeClientDigest(PrimaryDc,TrustedDomainName,Mes" +
                            "sageOne)\'");
                    temp538 = this.INrpcServerAdapterInstance.NetrLogonComputeClientDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.TrustedDomainName, ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103791");
                    this.Manager.Checkpoint("MS-NRPC_R103790");
                    this.Manager.Comment("reaching state \'S1438\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeClientDigest/ERROR_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_ACCESS_DENIED, temp538, "return of NetrLogonComputeClientDigest, state S1438");
                    Test_AuthenticateMessageS1456();
                    goto label153;
                }
                if ((temp540 == 1)) {
                    this.Manager.Comment("reaching state \'S653\'");
                    this.Manager.Comment("executing step \'call SwitchUserAccount(False)\'");
                    this.INrpcServerAdapterInstance.SwitchUserAccount(false);
                    this.Manager.Comment("reaching state \'S1001\'");
                    this.Manager.Comment("checking step \'return SwitchUserAccount\'");
                    this.Manager.Comment("reaching state \'S1289\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp539;
                    this.Manager.Comment("executing step \'call NetrLogonComputeClientDigest(PrimaryDc,TrustedDomainName,Mes" +
                            "sageOne)\'");
                    temp539 = this.INrpcServerAdapterInstance.NetrLogonComputeClientDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.TrustedDomainName, ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103791");
                    this.Manager.Checkpoint("MS-NRPC_R103790");
                    this.Manager.Comment("reaching state \'S1439\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeClientDigest/ERROR_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_ACCESS_DENIED, temp539, "return of NetrLogonComputeClientDigest, state S1439");
                    Test_AuthenticateMessageS1457();
                    goto label153;
                }
                throw new InvalidOperationException("never reached");
            label153:
;
                goto label155;
            }
            if ((temp545 == 2)) {
                this.Manager.Comment("reaching state \'S211\'");
                bool temp541;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp541);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp541);
                this.Manager.Comment("reaching state \'S385\'");
                int temp544 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS62GetClientAccountTypeChecker4)), new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS62GetClientAccountTypeChecker5)));
                if ((temp544 == 0)) {
                    this.Manager.Comment("reaching state \'S654\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp542;
                    this.Manager.Comment("executing step \'call NetrLogonComputeServerDigest(PrimaryDc,RidOfNonMachineAccoun" +
                            "t,MessageOne)\'");
                    temp542 = this.INrpcServerAdapterInstance.NetrLogonComputeServerDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.RidType)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103756");
                    this.Manager.Checkpoint("MS-NRPC_R103743");
                    this.Manager.Comment("reaching state \'S1002\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeServerDigest/ERROR_NO_SUCH_USER\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SUCH_USER, temp542, "return of NetrLogonComputeServerDigest, state S1002");
                    this.Manager.Comment("reaching state \'S1290\'");
                    goto label154;
                }
                if ((temp544 == 1)) {
                    this.Manager.Comment("reaching state \'S655\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp543;
                    this.Manager.Comment("executing step \'call NetrLogonComputeServerDigest(PrimaryDc,RidOfNonMachineAccoun" +
                            "t,MessageOne)\'");
                    temp543 = this.INrpcServerAdapterInstance.NetrLogonComputeServerDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.RidType)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103756");
                    this.Manager.Checkpoint("MS-NRPC_R103743");
                    this.Manager.Comment("reaching state \'S1003\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeServerDigest/ERROR_NO_SUCH_USER\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SUCH_USER, temp543, "return of NetrLogonComputeServerDigest, state S1003");
                    this.Manager.Comment("reaching state \'S1291\'");
                    goto label154;
                }
                throw new InvalidOperationException("never reached");
            label154:
;
                goto label155;
            }
            throw new InvalidOperationException("never reached");
        label155:
;
            this.Manager.EndTest();
        }
        
        private void Test_AuthenticateMessageS62GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S63");
        }
        
        private void Test_AuthenticateMessageS62GetClientAccountTypeChecker(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S383");
        }
        
        private void Test_AuthenticateMessageS62GetClientAccountTypeChecker1(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S383");
        }
        
        private void Test_AuthenticateMessageS62GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S63");
        }
        
        private void Test_AuthenticateMessageS62GetClientAccountTypeChecker2(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S384");
        }
        
        private void Test_AuthenticateMessageS62GetClientAccountTypeChecker3(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S384");
        }
        
        private void Test_AuthenticateMessageS62GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S63");
        }
        
        private void Test_AuthenticateMessageS62GetClientAccountTypeChecker4(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S385");
        }
        
        private void Test_AuthenticateMessageS62GetClientAccountTypeChecker5(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S385");
        }
        #endregion
        
        #region Test Starting in S64
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_AuthenticateMessageS64() {
            this.Manager.BeginTest("Test_AuthenticateMessageS64");
            this.Manager.Comment("reaching state \'S64\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp546;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp546);
            this.Manager.AddReturn(GetPlatformInfo, null, temp546);
            this.Manager.Comment("reaching state \'S65\'");
            int temp559 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_AuthenticateMessageS64GetPlatformChecker)), new ExpectedReturn(Test_AuthenticateMessage.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_AuthenticateMessageS64GetPlatformChecker1)), new ExpectedReturn(Test_AuthenticateMessage.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_AuthenticateMessageS64GetPlatformChecker2)));
            if ((temp559 == 0)) {
                this.Manager.Comment("reaching state \'S212\'");
                bool temp547;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp547);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp547);
                this.Manager.Comment("reaching state \'S386\'");
                int temp550 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS64GetClientAccountTypeChecker)), new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS64GetClientAccountTypeChecker1)));
                if ((temp550 == 0)) {
                    this.Manager.Comment("reaching state \'S656\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp548;
                    this.Manager.Comment("executing step \'call NetrLogonSetServiceBits(PrimaryDc,512,0)\'");
                    temp548 = this.INrpcServerAdapterInstance.NetrLogonSetServiceBits(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 512u, 0u);
                    this.Manager.Comment("reaching state \'S1004\'");
                    this.Manager.Comment("checking step \'return NetrLogonSetServiceBits/ERROR_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_ACCESS_DENIED, temp548, "return of NetrLogonSetServiceBits, state S1004");
                    Test_AuthenticateMessageS1216();
                    goto label156;
                }
                if ((temp550 == 1)) {
                    this.Manager.Comment("reaching state \'S657\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp549;
                    this.Manager.Comment("executing step \'call NetrLogonSetServiceBits(PrimaryDc,1,1)\'");
                    temp549 = this.INrpcServerAdapterInstance.NetrLogonSetServiceBits(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 1u, 1u);
                    this.Manager.Comment("reaching state \'S1005\'");
                    this.Manager.Comment("checking step \'return NetrLogonSetServiceBits/STATUS_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp549, "return of NetrLogonSetServiceBits, state S1005");
                    Test_AuthenticateMessageS1217();
                    goto label156;
                }
                throw new InvalidOperationException("never reached");
            label156:
;
                goto label159;
            }
            if ((temp559 == 1)) {
                this.Manager.Comment("reaching state \'S213\'");
                bool temp551;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp551);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp551);
                this.Manager.Comment("reaching state \'S387\'");
                int temp554 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS64GetClientAccountTypeChecker2)), new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS64GetClientAccountTypeChecker3)));
                if ((temp554 == 0)) {
                    this.Manager.Comment("reaching state \'S658\'");
                    this.Manager.Comment("executing step \'call SwitchUserAccount(False)\'");
                    this.INrpcServerAdapterInstance.SwitchUserAccount(false);
                    this.Manager.Comment("reaching state \'S1006\'");
                    this.Manager.Comment("checking step \'return SwitchUserAccount\'");
                    this.Manager.Comment("reaching state \'S1292\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp552;
                    this.Manager.Comment("executing step \'call NetrLogonComputeServerDigest(PrimaryDc,RidOfNonDcMachineAcco" +
                            "unt,MessageOne)\'");
                    temp552 = this.INrpcServerAdapterInstance.NetrLogonComputeServerDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.RidType.RidOfNonDcMachineAccount, ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103746");
                    this.Manager.Checkpoint("MS-NRPC_R103743");
                    this.Manager.Comment("reaching state \'S1440\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeServerDigest/ERROR_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_ACCESS_DENIED, temp552, "return of NetrLogonComputeServerDigest, state S1440");
                    Test_AuthenticateMessageS1458();
                    goto label157;
                }
                if ((temp554 == 1)) {
                    this.Manager.Comment("reaching state \'S659\'");
                    this.Manager.Comment("executing step \'call SwitchUserAccount(False)\'");
                    this.INrpcServerAdapterInstance.SwitchUserAccount(false);
                    this.Manager.Comment("reaching state \'S1007\'");
                    this.Manager.Comment("checking step \'return SwitchUserAccount\'");
                    this.Manager.Comment("reaching state \'S1293\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp553;
                    this.Manager.Comment("executing step \'call NetrLogonComputeServerDigest(PrimaryDc,RidOfNonDcMachineAcco" +
                            "unt,MessageOne)\'");
                    temp553 = this.INrpcServerAdapterInstance.NetrLogonComputeServerDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.RidType.RidOfNonDcMachineAccount, ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103746");
                    this.Manager.Checkpoint("MS-NRPC_R103743");
                    this.Manager.Comment("reaching state \'S1441\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeServerDigest/ERROR_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_ACCESS_DENIED, temp553, "return of NetrLogonComputeServerDigest, state S1441");
                    Test_AuthenticateMessageS1459();
                    goto label157;
                }
                throw new InvalidOperationException("never reached");
            label157:
;
                goto label159;
            }
            if ((temp559 == 2)) {
                this.Manager.Comment("reaching state \'S214\'");
                bool temp555;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp555);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp555);
                this.Manager.Comment("reaching state \'S388\'");
                int temp558 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS64GetClientAccountTypeChecker4)), new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS64GetClientAccountTypeChecker5)));
                if ((temp558 == 0)) {
                    this.Manager.Comment("reaching state \'S660\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp556;
                    this.Manager.Comment("executing step \'call NetrLogonComputeServerDigest(PrimaryDc,RidOfNonMachineAccoun" +
                            "t,MessageOne)\'");
                    temp556 = this.INrpcServerAdapterInstance.NetrLogonComputeServerDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.RidType)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103756");
                    this.Manager.Checkpoint("MS-NRPC_R103743");
                    this.Manager.Comment("reaching state \'S1008\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeServerDigest/ERROR_NO_SUCH_USER\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SUCH_USER, temp556, "return of NetrLogonComputeServerDigest, state S1008");
                    this.Manager.Comment("reaching state \'S1294\'");
                    goto label158;
                }
                if ((temp558 == 1)) {
                    this.Manager.Comment("reaching state \'S661\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp557;
                    this.Manager.Comment("executing step \'call NetrLogonComputeServerDigest(PrimaryDc,RidOfNonMachineAccoun" +
                            "t,MessageOne)\'");
                    temp557 = this.INrpcServerAdapterInstance.NetrLogonComputeServerDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.RidType)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103756");
                    this.Manager.Checkpoint("MS-NRPC_R103743");
                    this.Manager.Comment("reaching state \'S1009\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeServerDigest/ERROR_NO_SUCH_USER\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SUCH_USER, temp557, "return of NetrLogonComputeServerDigest, state S1009");
                    this.Manager.Comment("reaching state \'S1295\'");
                    goto label158;
                }
                throw new InvalidOperationException("never reached");
            label158:
;
                goto label159;
            }
            throw new InvalidOperationException("never reached");
        label159:
;
            this.Manager.EndTest();
        }
        
        private void Test_AuthenticateMessageS64GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S65");
        }
        
        private void Test_AuthenticateMessageS64GetClientAccountTypeChecker(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S386");
        }
        
        private void Test_AuthenticateMessageS64GetClientAccountTypeChecker1(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S386");
        }
        
        private void Test_AuthenticateMessageS64GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S65");
        }
        
        private void Test_AuthenticateMessageS64GetClientAccountTypeChecker2(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S387");
        }
        
        private void Test_AuthenticateMessageS64GetClientAccountTypeChecker3(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S387");
        }
        
        private void Test_AuthenticateMessageS64GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S65");
        }
        
        private void Test_AuthenticateMessageS64GetClientAccountTypeChecker4(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S388");
        }
        
        private void Test_AuthenticateMessageS64GetClientAccountTypeChecker5(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S388");
        }
        #endregion
        
        #region Test Starting in S66
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_AuthenticateMessageS66() {
            this.Manager.BeginTest("Test_AuthenticateMessageS66");
            this.Manager.Comment("reaching state \'S66\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp560;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp560);
            this.Manager.AddReturn(GetPlatformInfo, null, temp560);
            this.Manager.Comment("reaching state \'S67\'");
            int temp573 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_AuthenticateMessageS66GetPlatformChecker)), new ExpectedReturn(Test_AuthenticateMessage.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_AuthenticateMessageS66GetPlatformChecker1)), new ExpectedReturn(Test_AuthenticateMessage.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_AuthenticateMessageS66GetPlatformChecker2)));
            if ((temp573 == 0)) {
                this.Manager.Comment("reaching state \'S215\'");
                bool temp561;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp561);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp561);
                this.Manager.Comment("reaching state \'S389\'");
                int temp564 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS66GetClientAccountTypeChecker)), new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS66GetClientAccountTypeChecker1)));
                if ((temp564 == 0)) {
                    this.Manager.Comment("reaching state \'S662\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp562;
                    this.Manager.Comment("executing step \'call NetrLogonSetServiceBits(PrimaryDc,8192,0)\'");
                    temp562 = this.INrpcServerAdapterInstance.NetrLogonSetServiceBits(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 8192u, 0u);
                    this.Manager.Comment("reaching state \'S1010\'");
                    this.Manager.Comment("checking step \'return NetrLogonSetServiceBits/ERROR_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_ACCESS_DENIED, temp562, "return of NetrLogonSetServiceBits, state S1010");
                    Test_AuthenticateMessageS1216();
                    goto label160;
                }
                if ((temp564 == 1)) {
                    this.Manager.Comment("reaching state \'S663\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp563;
                    this.Manager.Comment("executing step \'call NetrLogonSetServiceBits(PrimaryDc,0,1)\'");
                    temp563 = this.INrpcServerAdapterInstance.NetrLogonSetServiceBits(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 0u, 1u);
                    this.Manager.Comment("reaching state \'S1011\'");
                    this.Manager.Comment("checking step \'return NetrLogonSetServiceBits/STATUS_INVALID_PARAMETER\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_INVALID_PARAMETER, temp563, "return of NetrLogonSetServiceBits, state S1011");
                    Test_AuthenticateMessageS1217();
                    goto label160;
                }
                throw new InvalidOperationException("never reached");
            label160:
;
                goto label163;
            }
            if ((temp573 == 1)) {
                this.Manager.Comment("reaching state \'S216\'");
                bool temp565;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp565);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp565);
                this.Manager.Comment("reaching state \'S390\'");
                int temp568 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS66GetClientAccountTypeChecker2)), new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS66GetClientAccountTypeChecker3)));
                if ((temp568 == 0)) {
                    this.Manager.Comment("reaching state \'S664\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp566;
                    this.Manager.Comment("executing step \'call NetrLogonComputeServerDigest(PrimaryDc,NonExist,MessageOne)\'" +
                            "");
                    temp566 = this.INrpcServerAdapterInstance.NetrLogonComputeServerDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.RidType)(0)), ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103755");
                    this.Manager.Checkpoint("MS-NRPC_R103743");
                    this.Manager.Comment("reaching state \'S1012\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeServerDigest/ERROR_NO_SUCH_USER\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SUCH_USER, temp566, "return of NetrLogonComputeServerDigest, state S1012");
                    Test_AuthenticateMessageS1284();
                    goto label161;
                }
                if ((temp568 == 1)) {
                    this.Manager.Comment("reaching state \'S665\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp567;
                    this.Manager.Comment("executing step \'call NetrLogonComputeServerDigest(PrimaryDc,NonExist,MessageOne)\'" +
                            "");
                    temp567 = this.INrpcServerAdapterInstance.NetrLogonComputeServerDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.RidType)(0)), ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103755");
                    this.Manager.Checkpoint("MS-NRPC_R103743");
                    this.Manager.Comment("reaching state \'S1013\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeServerDigest/ERROR_NO_SUCH_USER\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SUCH_USER, temp567, "return of NetrLogonComputeServerDigest, state S1013");
                    Test_AuthenticateMessageS1285();
                    goto label161;
                }
                throw new InvalidOperationException("never reached");
            label161:
;
                goto label163;
            }
            if ((temp573 == 2)) {
                this.Manager.Comment("reaching state \'S217\'");
                bool temp569;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp569);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp569);
                this.Manager.Comment("reaching state \'S391\'");
                int temp572 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS66GetClientAccountTypeChecker4)), new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS66GetClientAccountTypeChecker5)));
                if ((temp572 == 0)) {
                    this.Manager.Comment("reaching state \'S666\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp570;
                    this.Manager.Comment("executing step \'call NetrLogonComputeServerDigest(PrimaryDc,RidOfNonMachineAccoun" +
                            "t,MessageOne)\'");
                    temp570 = this.INrpcServerAdapterInstance.NetrLogonComputeServerDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.RidType)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103756");
                    this.Manager.Checkpoint("MS-NRPC_R103743");
                    this.Manager.Comment("reaching state \'S1014\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeServerDigest/ERROR_NO_SUCH_USER\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SUCH_USER, temp570, "return of NetrLogonComputeServerDigest, state S1014");
                    this.Manager.Comment("reaching state \'S1296\'");
                    goto label162;
                }
                if ((temp572 == 1)) {
                    this.Manager.Comment("reaching state \'S667\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp571;
                    this.Manager.Comment("executing step \'call NetrLogonComputeServerDigest(PrimaryDc,RidOfNonMachineAccoun" +
                            "t,MessageOne)\'");
                    temp571 = this.INrpcServerAdapterInstance.NetrLogonComputeServerDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.RidType)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103756");
                    this.Manager.Checkpoint("MS-NRPC_R103743");
                    this.Manager.Comment("reaching state \'S1015\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeServerDigest/ERROR_NO_SUCH_USER\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SUCH_USER, temp571, "return of NetrLogonComputeServerDigest, state S1015");
                    this.Manager.Comment("reaching state \'S1297\'");
                    goto label162;
                }
                throw new InvalidOperationException("never reached");
            label162:
;
                goto label163;
            }
            throw new InvalidOperationException("never reached");
        label163:
;
            this.Manager.EndTest();
        }
        
        private void Test_AuthenticateMessageS66GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S67");
        }
        
        private void Test_AuthenticateMessageS66GetClientAccountTypeChecker(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S389");
        }
        
        private void Test_AuthenticateMessageS66GetClientAccountTypeChecker1(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S389");
        }
        
        private void Test_AuthenticateMessageS66GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S67");
        }
        
        private void Test_AuthenticateMessageS66GetClientAccountTypeChecker2(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S390");
        }
        
        private void Test_AuthenticateMessageS66GetClientAccountTypeChecker3(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S390");
        }
        
        private void Test_AuthenticateMessageS66GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S67");
        }
        
        private void Test_AuthenticateMessageS66GetClientAccountTypeChecker4(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S391");
        }
        
        private void Test_AuthenticateMessageS66GetClientAccountTypeChecker5(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S391");
        }
        #endregion
        
        #region Test Starting in S68
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_AuthenticateMessageS68() {
            this.Manager.BeginTest("Test_AuthenticateMessageS68");
            this.Manager.Comment("reaching state \'S68\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp574;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp574);
            this.Manager.AddReturn(GetPlatformInfo, null, temp574);
            this.Manager.Comment("reaching state \'S69\'");
            int temp587 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_AuthenticateMessageS68GetPlatformChecker)), new ExpectedReturn(Test_AuthenticateMessage.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_AuthenticateMessageS68GetPlatformChecker1)), new ExpectedReturn(Test_AuthenticateMessage.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_AuthenticateMessageS68GetPlatformChecker2)));
            if ((temp587 == 0)) {
                this.Manager.Comment("reaching state \'S218\'");
                bool temp575;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp575);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp575);
                this.Manager.Comment("reaching state \'S392\'");
                int temp578 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS68GetClientAccountTypeChecker)), new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS68GetClientAccountTypeChecker1)));
                if ((temp578 == 0)) {
                    this.Manager.Comment("reaching state \'S668\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp576;
                    this.Manager.Comment("executing step \'call NetrLogonSetServiceBits(PrimaryDc,1,0)\'");
                    temp576 = this.INrpcServerAdapterInstance.NetrLogonSetServiceBits(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 1u, 0u);
                    this.Manager.Comment("reaching state \'S1016\'");
                    this.Manager.Comment("checking step \'return NetrLogonSetServiceBits/ERROR_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_ACCESS_DENIED, temp576, "return of NetrLogonSetServiceBits, state S1016");
                    Test_AuthenticateMessageS1216();
                    goto label164;
                }
                if ((temp578 == 1)) {
                    this.Manager.Comment("reaching state \'S669\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp577;
                    this.Manager.Comment("executing step \'call NetrLogonSetServiceBits(PrimaryDc,8192,1)\'");
                    temp577 = this.INrpcServerAdapterInstance.NetrLogonSetServiceBits(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 8192u, 1u);
                    this.Manager.Comment("reaching state \'S1017\'");
                    this.Manager.Comment("checking step \'return NetrLogonSetServiceBits/STATUS_INVALID_PARAMETER\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_INVALID_PARAMETER, temp577, "return of NetrLogonSetServiceBits, state S1017");
                    Test_AuthenticateMessageS1217();
                    goto label164;
                }
                throw new InvalidOperationException("never reached");
            label164:
;
                goto label167;
            }
            if ((temp587 == 1)) {
                this.Manager.Comment("reaching state \'S219\'");
                bool temp579;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp579);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp579);
                this.Manager.Comment("reaching state \'S393\'");
                int temp582 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS68GetClientAccountTypeChecker2)), new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS68GetClientAccountTypeChecker3)));
                if ((temp582 == 0)) {
                    this.Manager.Comment("reaching state \'S670\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp580;
                    this.Manager.Comment("executing step \'call NetrLogonComputeServerDigest(PrimaryDc,RidOfNonDcMachineAcco" +
                            "unt,MessageOne)\'");
                    temp580 = this.INrpcServerAdapterInstance.NetrLogonComputeServerDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.RidType.RidOfNonDcMachineAccount, ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103742");
                    this.Manager.Comment("reaching state \'S1018\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeServerDigest/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp580, "return of NetrLogonComputeServerDigest, state S1018");
                    Test_AuthenticateMessageS1284();
                    goto label165;
                }
                if ((temp582 == 1)) {
                    this.Manager.Comment("reaching state \'S671\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp581;
                    this.Manager.Comment("executing step \'call NetrLogonComputeServerDigest(PrimaryDc,RidOfNonDcMachineAcco" +
                            "unt,MessageOne)\'");
                    temp581 = this.INrpcServerAdapterInstance.NetrLogonComputeServerDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.RidType.RidOfNonDcMachineAccount, ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103742");
                    this.Manager.Comment("reaching state \'S1019\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeServerDigest/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp581, "return of NetrLogonComputeServerDigest, state S1019");
                    Test_AuthenticateMessageS1285();
                    goto label165;
                }
                throw new InvalidOperationException("never reached");
            label165:
;
                goto label167;
            }
            if ((temp587 == 2)) {
                this.Manager.Comment("reaching state \'S220\'");
                bool temp583;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp583);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp583);
                this.Manager.Comment("reaching state \'S394\'");
                int temp586 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS68GetClientAccountTypeChecker4)), new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS68GetClientAccountTypeChecker5)));
                if ((temp586 == 0)) {
                    this.Manager.Comment("reaching state \'S672\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp584;
                    this.Manager.Comment("executing step \'call NetrLogonComputeServerDigest(PrimaryDc,RidOfNonMachineAccoun" +
                            "t,MessageOne)\'");
                    temp584 = this.INrpcServerAdapterInstance.NetrLogonComputeServerDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.RidType)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103756");
                    this.Manager.Checkpoint("MS-NRPC_R103743");
                    this.Manager.Comment("reaching state \'S1020\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeServerDigest/ERROR_NO_SUCH_USER\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SUCH_USER, temp584, "return of NetrLogonComputeServerDigest, state S1020");
                    this.Manager.Comment("reaching state \'S1298\'");
                    goto label166;
                }
                if ((temp586 == 1)) {
                    this.Manager.Comment("reaching state \'S673\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp585;
                    this.Manager.Comment("executing step \'call NetrLogonComputeServerDigest(PrimaryDc,RidOfNonMachineAccoun" +
                            "t,MessageOne)\'");
                    temp585 = this.INrpcServerAdapterInstance.NetrLogonComputeServerDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.RidType)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103756");
                    this.Manager.Checkpoint("MS-NRPC_R103743");
                    this.Manager.Comment("reaching state \'S1021\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeServerDigest/ERROR_NO_SUCH_USER\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SUCH_USER, temp585, "return of NetrLogonComputeServerDigest, state S1021");
                    this.Manager.Comment("reaching state \'S1299\'");
                    goto label166;
                }
                throw new InvalidOperationException("never reached");
            label166:
;
                goto label167;
            }
            throw new InvalidOperationException("never reached");
        label167:
;
            this.Manager.EndTest();
        }
        
        private void Test_AuthenticateMessageS68GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S69");
        }
        
        private void Test_AuthenticateMessageS68GetClientAccountTypeChecker(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S392");
        }
        
        private void Test_AuthenticateMessageS68GetClientAccountTypeChecker1(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S392");
        }
        
        private void Test_AuthenticateMessageS68GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S69");
        }
        
        private void Test_AuthenticateMessageS68GetClientAccountTypeChecker2(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S393");
        }
        
        private void Test_AuthenticateMessageS68GetClientAccountTypeChecker3(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S393");
        }
        
        private void Test_AuthenticateMessageS68GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S69");
        }
        
        private void Test_AuthenticateMessageS68GetClientAccountTypeChecker4(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S394");
        }
        
        private void Test_AuthenticateMessageS68GetClientAccountTypeChecker5(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S394");
        }
        #endregion
        
        #region Test Starting in S70
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_AuthenticateMessageS70() {
            this.Manager.BeginTest("Test_AuthenticateMessageS70");
            this.Manager.Comment("reaching state \'S70\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp588;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp588);
            this.Manager.AddReturn(GetPlatformInfo, null, temp588);
            this.Manager.Comment("reaching state \'S71\'");
            int temp601 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_AuthenticateMessageS70GetPlatformChecker)), new ExpectedReturn(Test_AuthenticateMessage.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_AuthenticateMessageS70GetPlatformChecker1)), new ExpectedReturn(Test_AuthenticateMessage.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_AuthenticateMessageS70GetPlatformChecker2)));
            if ((temp601 == 0)) {
                this.Manager.Comment("reaching state \'S221\'");
                bool temp589;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp589);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp589);
                this.Manager.Comment("reaching state \'S395\'");
                int temp592 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS70GetClientAccountTypeChecker)), new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS70GetClientAccountTypeChecker1)));
                if ((temp592 == 0)) {
                    this.Manager.Comment("reaching state \'S674\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp590;
                    this.Manager.Comment("executing step \'call NetrLogonSetServiceBits(PrimaryDc,512,64)\'");
                    temp590 = this.INrpcServerAdapterInstance.NetrLogonSetServiceBits(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 512u, 64u);
                    this.Manager.Comment("reaching state \'S1022\'");
                    this.Manager.Comment("checking step \'return NetrLogonSetServiceBits/ERROR_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_ACCESS_DENIED, temp590, "return of NetrLogonSetServiceBits, state S1022");
                    Test_AuthenticateMessageS1216();
                    goto label168;
                }
                if ((temp592 == 1)) {
                    this.Manager.Comment("reaching state \'S675\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp591;
                    this.Manager.Comment("executing step \'call NetrLogonSetServiceBits(PrimaryDc,64,1)\'");
                    temp591 = this.INrpcServerAdapterInstance.NetrLogonSetServiceBits(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 64u, 1u);
                    this.Manager.Comment("reaching state \'S1023\'");
                    this.Manager.Comment("checking step \'return NetrLogonSetServiceBits/STATUS_INVALID_PARAMETER\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_INVALID_PARAMETER, temp591, "return of NetrLogonSetServiceBits, state S1023");
                    Test_AuthenticateMessageS1217();
                    goto label168;
                }
                throw new InvalidOperationException("never reached");
            label168:
;
                goto label171;
            }
            if ((temp601 == 1)) {
                this.Manager.Comment("reaching state \'S222\'");
                bool temp593;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp593);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp593);
                this.Manager.Comment("reaching state \'S396\'");
                int temp596 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS70GetClientAccountTypeChecker2)), new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS70GetClientAccountTypeChecker3)));
                if ((temp596 == 0)) {
                    this.Manager.Comment("reaching state \'S676\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp594;
                    this.Manager.Comment("executing step \'call NetrLogonComputeClientDigest(NonDcServer,FqdnFormatDomainNam" +
                            "e,MessageOne)\'");
                    temp594 = this.INrpcServerAdapterInstance.NetrLogonComputeClientDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.FqdnFormatDomainName, ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103789");
                    this.Manager.Comment("reaching state \'S1024\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeClientDigest/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp594, "return of NetrLogonComputeClientDigest, state S1024");
                    Test_AuthenticateMessageS1284();
                    goto label169;
                }
                if ((temp596 == 1)) {
                    this.Manager.Comment("reaching state \'S677\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp595;
                    this.Manager.Comment("executing step \'call NetrLogonComputeClientDigest(NonDcServer,FqdnFormatDomainNam" +
                            "e,MessageOne)\'");
                    temp595 = this.INrpcServerAdapterInstance.NetrLogonComputeClientDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.NonDcServer, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.FqdnFormatDomainName, ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103789");
                    this.Manager.Comment("reaching state \'S1025\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeClientDigest/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp595, "return of NetrLogonComputeClientDigest, state S1025");
                    Test_AuthenticateMessageS1285();
                    goto label169;
                }
                throw new InvalidOperationException("never reached");
            label169:
;
                goto label171;
            }
            if ((temp601 == 2)) {
                this.Manager.Comment("reaching state \'S223\'");
                bool temp597;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp597);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp597);
                this.Manager.Comment("reaching state \'S397\'");
                int temp600 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS70GetClientAccountTypeChecker4)), new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS70GetClientAccountTypeChecker5)));
                if ((temp600 == 0)) {
                    this.Manager.Comment("reaching state \'S678\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp598;
                    this.Manager.Comment("executing step \'call NetrLogonComputeServerDigest(PrimaryDc,RidOfNonMachineAccoun" +
                            "t,MessageOne)\'");
                    temp598 = this.INrpcServerAdapterInstance.NetrLogonComputeServerDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.RidType)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103756");
                    this.Manager.Checkpoint("MS-NRPC_R103743");
                    this.Manager.Comment("reaching state \'S1026\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeServerDigest/ERROR_NO_SUCH_USER\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SUCH_USER, temp598, "return of NetrLogonComputeServerDigest, state S1026");
                    this.Manager.Comment("reaching state \'S1300\'");
                    goto label170;
                }
                if ((temp600 == 1)) {
                    this.Manager.Comment("reaching state \'S679\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp599;
                    this.Manager.Comment("executing step \'call NetrLogonComputeServerDigest(PrimaryDc,RidOfNonMachineAccoun" +
                            "t,MessageOne)\'");
                    temp599 = this.INrpcServerAdapterInstance.NetrLogonComputeServerDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.RidType)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103756");
                    this.Manager.Checkpoint("MS-NRPC_R103743");
                    this.Manager.Comment("reaching state \'S1027\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeServerDigest/ERROR_NO_SUCH_USER\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SUCH_USER, temp599, "return of NetrLogonComputeServerDigest, state S1027");
                    this.Manager.Comment("reaching state \'S1301\'");
                    goto label170;
                }
                throw new InvalidOperationException("never reached");
            label170:
;
                goto label171;
            }
            throw new InvalidOperationException("never reached");
        label171:
;
            this.Manager.EndTest();
        }
        
        private void Test_AuthenticateMessageS70GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S71");
        }
        
        private void Test_AuthenticateMessageS70GetClientAccountTypeChecker(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S395");
        }
        
        private void Test_AuthenticateMessageS70GetClientAccountTypeChecker1(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S395");
        }
        
        private void Test_AuthenticateMessageS70GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S71");
        }
        
        private void Test_AuthenticateMessageS70GetClientAccountTypeChecker2(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S396");
        }
        
        private void Test_AuthenticateMessageS70GetClientAccountTypeChecker3(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S396");
        }
        
        private void Test_AuthenticateMessageS70GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S71");
        }
        
        private void Test_AuthenticateMessageS70GetClientAccountTypeChecker4(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S397");
        }
        
        private void Test_AuthenticateMessageS70GetClientAccountTypeChecker5(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S397");
        }
        #endregion
        
        #region Test Starting in S72
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_AuthenticateMessageS72() {
            this.Manager.BeginTest("Test_AuthenticateMessageS72");
            this.Manager.Comment("reaching state \'S72\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp602;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp602);
            this.Manager.AddReturn(GetPlatformInfo, null, temp602);
            this.Manager.Comment("reaching state \'S73\'");
            int temp615 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_AuthenticateMessageS72GetPlatformChecker)), new ExpectedReturn(Test_AuthenticateMessage.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_AuthenticateMessageS72GetPlatformChecker1)), new ExpectedReturn(Test_AuthenticateMessage.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_AuthenticateMessageS72GetPlatformChecker2)));
            if ((temp615 == 0)) {
                this.Manager.Comment("reaching state \'S224\'");
                bool temp603;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp603);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp603);
                this.Manager.Comment("reaching state \'S398\'");
                int temp606 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS72GetClientAccountTypeChecker)), new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS72GetClientAccountTypeChecker1)));
                if ((temp606 == 0)) {
                    this.Manager.Comment("reaching state \'S680\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp604;
                    this.Manager.Comment("executing step \'call NetrLogonSetServiceBits(PrimaryDc,512,8192)\'");
                    temp604 = this.INrpcServerAdapterInstance.NetrLogonSetServiceBits(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 512u, 8192u);
                    this.Manager.Comment("reaching state \'S1028\'");
                    this.Manager.Comment("checking step \'return NetrLogonSetServiceBits/ERROR_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_ACCESS_DENIED, temp604, "return of NetrLogonSetServiceBits, state S1028");
                    Test_AuthenticateMessageS1216();
                    goto label172;
                }
                if ((temp606 == 1)) {
                    this.Manager.Comment("reaching state \'S681\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp605;
                    this.Manager.Comment("executing step \'call NetrLogonSetServiceBits(PrimaryDc,1,8192)\'");
                    temp605 = this.INrpcServerAdapterInstance.NetrLogonSetServiceBits(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 1u, 8192u);
                    this.Manager.Comment("reaching state \'S1029\'");
                    this.Manager.Comment("checking step \'return NetrLogonSetServiceBits/STATUS_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp605, "return of NetrLogonSetServiceBits, state S1029");
                    Test_AuthenticateMessageS1217();
                    goto label172;
                }
                throw new InvalidOperationException("never reached");
            label172:
;
                goto label175;
            }
            if ((temp615 == 1)) {
                this.Manager.Comment("reaching state \'S225\'");
                bool temp607;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp607);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp607);
                this.Manager.Comment("reaching state \'S399\'");
                int temp610 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS72GetClientAccountTypeChecker2)), new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS72GetClientAccountTypeChecker3)));
                if ((temp610 == 0)) {
                    this.Manager.Comment("reaching state \'S682\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp608;
                    this.Manager.Comment("executing step \'call NetrLogonComputeClientDigest(PrimaryDc,Null,MessageOne)\'");
                    temp608 = this.INrpcServerAdapterInstance.NetrLogonComputeClientDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.DomainNameType)(0)), ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103789");
                    this.Manager.Comment("reaching state \'S1030\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeClientDigest/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp608, "return of NetrLogonComputeClientDigest, state S1030");
                    Test_AuthenticateMessageS1284();
                    goto label173;
                }
                if ((temp610 == 1)) {
                    this.Manager.Comment("reaching state \'S683\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp609;
                    this.Manager.Comment("executing step \'call NetrLogonComputeClientDigest(PrimaryDc,Null,MessageOne)\'");
                    temp609 = this.INrpcServerAdapterInstance.NetrLogonComputeClientDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.DomainNameType)(0)), ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103789");
                    this.Manager.Comment("reaching state \'S1031\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeClientDigest/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp609, "return of NetrLogonComputeClientDigest, state S1031");
                    Test_AuthenticateMessageS1285();
                    goto label173;
                }
                throw new InvalidOperationException("never reached");
            label173:
;
                goto label175;
            }
            if ((temp615 == 2)) {
                this.Manager.Comment("reaching state \'S226\'");
                bool temp611;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp611);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp611);
                this.Manager.Comment("reaching state \'S400\'");
                int temp614 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS72GetClientAccountTypeChecker4)), new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS72GetClientAccountTypeChecker5)));
                if ((temp614 == 0)) {
                    this.Manager.Comment("reaching state \'S684\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp612;
                    this.Manager.Comment("executing step \'call NetrLogonComputeServerDigest(PrimaryDc,RidOfNonMachineAccoun" +
                            "t,MessageOne)\'");
                    temp612 = this.INrpcServerAdapterInstance.NetrLogonComputeServerDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.RidType)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103756");
                    this.Manager.Checkpoint("MS-NRPC_R103743");
                    this.Manager.Comment("reaching state \'S1032\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeServerDigest/ERROR_NO_SUCH_USER\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SUCH_USER, temp612, "return of NetrLogonComputeServerDigest, state S1032");
                    this.Manager.Comment("reaching state \'S1302\'");
                    goto label174;
                }
                if ((temp614 == 1)) {
                    this.Manager.Comment("reaching state \'S685\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp613;
                    this.Manager.Comment("executing step \'call NetrLogonComputeServerDigest(PrimaryDc,RidOfNonMachineAccoun" +
                            "t,MessageOne)\'");
                    temp613 = this.INrpcServerAdapterInstance.NetrLogonComputeServerDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.RidType)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103756");
                    this.Manager.Checkpoint("MS-NRPC_R103743");
                    this.Manager.Comment("reaching state \'S1033\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeServerDigest/ERROR_NO_SUCH_USER\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SUCH_USER, temp613, "return of NetrLogonComputeServerDigest, state S1033");
                    this.Manager.Comment("reaching state \'S1303\'");
                    goto label174;
                }
                throw new InvalidOperationException("never reached");
            label174:
;
                goto label175;
            }
            throw new InvalidOperationException("never reached");
        label175:
;
            this.Manager.EndTest();
        }
        
        private void Test_AuthenticateMessageS72GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S73");
        }
        
        private void Test_AuthenticateMessageS72GetClientAccountTypeChecker(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S398");
        }
        
        private void Test_AuthenticateMessageS72GetClientAccountTypeChecker1(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S398");
        }
        
        private void Test_AuthenticateMessageS72GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S73");
        }
        
        private void Test_AuthenticateMessageS72GetClientAccountTypeChecker2(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S399");
        }
        
        private void Test_AuthenticateMessageS72GetClientAccountTypeChecker3(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S399");
        }
        
        private void Test_AuthenticateMessageS72GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S73");
        }
        
        private void Test_AuthenticateMessageS72GetClientAccountTypeChecker4(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S400");
        }
        
        private void Test_AuthenticateMessageS72GetClientAccountTypeChecker5(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S400");
        }
        #endregion
        
        #region Test Starting in S74
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_AuthenticateMessageS74() {
            this.Manager.BeginTest("Test_AuthenticateMessageS74");
            this.Manager.Comment("reaching state \'S74\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp616;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp616);
            this.Manager.AddReturn(GetPlatformInfo, null, temp616);
            this.Manager.Comment("reaching state \'S75\'");
            int temp629 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_AuthenticateMessageS74GetPlatformChecker)), new ExpectedReturn(Test_AuthenticateMessage.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_AuthenticateMessageS74GetPlatformChecker1)), new ExpectedReturn(Test_AuthenticateMessage.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_AuthenticateMessageS74GetPlatformChecker2)));
            if ((temp629 == 0)) {
                this.Manager.Comment("reaching state \'S227\'");
                bool temp617;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp617);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp617);
                this.Manager.Comment("reaching state \'S401\'");
                int temp620 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS74GetClientAccountTypeChecker)), new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS74GetClientAccountTypeChecker1)));
                if ((temp620 == 0)) {
                    this.Manager.Comment("reaching state \'S686\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp618;
                    this.Manager.Comment("executing step \'call NetrLogonSetServiceBits(PrimaryDc,8192,8192)\'");
                    temp618 = this.INrpcServerAdapterInstance.NetrLogonSetServiceBits(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 8192u, 8192u);
                    this.Manager.Comment("reaching state \'S1034\'");
                    this.Manager.Comment("checking step \'return NetrLogonSetServiceBits/ERROR_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_ACCESS_DENIED, temp618, "return of NetrLogonSetServiceBits, state S1034");
                    Test_AuthenticateMessageS1216();
                    goto label176;
                }
                if ((temp620 == 1)) {
                    this.Manager.Comment("reaching state \'S687\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp619;
                    this.Manager.Comment("executing step \'call NetrLogonSetServiceBits(PrimaryDc,1,64)\'");
                    temp619 = this.INrpcServerAdapterInstance.NetrLogonSetServiceBits(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 1u, 64u);
                    this.Manager.Comment("reaching state \'S1035\'");
                    this.Manager.Comment("checking step \'return NetrLogonSetServiceBits/STATUS_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp619, "return of NetrLogonSetServiceBits, state S1035");
                    Test_AuthenticateMessageS1217();
                    goto label176;
                }
                throw new InvalidOperationException("never reached");
            label176:
;
                goto label179;
            }
            if ((temp629 == 1)) {
                this.Manager.Comment("reaching state \'S228\'");
                bool temp621;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp621);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp621);
                this.Manager.Comment("reaching state \'S402\'");
                int temp624 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS74GetClientAccountTypeChecker2)), new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS74GetClientAccountTypeChecker3)));
                if ((temp624 == 0)) {
                    this.Manager.Comment("reaching state \'S688\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp622;
                    this.Manager.Comment("executing step \'call NetrLogonComputeClientDigest(PrimaryDc,NetBiosFormatDomainNa" +
                            "me,MessageOne)\'");
                    temp622 = this.INrpcServerAdapterInstance.NetrLogonComputeClientDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.NetBiosFormatDomainName, ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103789");
                    this.Manager.Comment("reaching state \'S1036\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeClientDigest/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp622, "return of NetrLogonComputeClientDigest, state S1036");
                    Test_AuthenticateMessageS1284();
                    goto label177;
                }
                if ((temp624 == 1)) {
                    this.Manager.Comment("reaching state \'S689\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp623;
                    this.Manager.Comment("executing step \'call NetrLogonComputeClientDigest(PrimaryDc,NetBiosFormatDomainNa" +
                            "me,MessageOne)\'");
                    temp623 = this.INrpcServerAdapterInstance.NetrLogonComputeClientDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.NetBiosFormatDomainName, ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103789");
                    this.Manager.Comment("reaching state \'S1037\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeClientDigest/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp623, "return of NetrLogonComputeClientDigest, state S1037");
                    Test_AuthenticateMessageS1285();
                    goto label177;
                }
                throw new InvalidOperationException("never reached");
            label177:
;
                goto label179;
            }
            if ((temp629 == 2)) {
                this.Manager.Comment("reaching state \'S229\'");
                bool temp625;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp625);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp625);
                this.Manager.Comment("reaching state \'S403\'");
                int temp628 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS74GetClientAccountTypeChecker4)), new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS74GetClientAccountTypeChecker5)));
                if ((temp628 == 0)) {
                    this.Manager.Comment("reaching state \'S690\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp626;
                    this.Manager.Comment("executing step \'call NetrLogonComputeServerDigest(PrimaryDc,RidOfNonMachineAccoun" +
                            "t,MessageOne)\'");
                    temp626 = this.INrpcServerAdapterInstance.NetrLogonComputeServerDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.RidType)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103756");
                    this.Manager.Checkpoint("MS-NRPC_R103743");
                    this.Manager.Comment("reaching state \'S1038\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeServerDigest/ERROR_NO_SUCH_USER\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SUCH_USER, temp626, "return of NetrLogonComputeServerDigest, state S1038");
                    this.Manager.Comment("reaching state \'S1304\'");
                    goto label178;
                }
                if ((temp628 == 1)) {
                    this.Manager.Comment("reaching state \'S691\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp627;
                    this.Manager.Comment("executing step \'call NetrLogonComputeServerDigest(PrimaryDc,RidOfNonMachineAccoun" +
                            "t,MessageOne)\'");
                    temp627 = this.INrpcServerAdapterInstance.NetrLogonComputeServerDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.RidType)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103756");
                    this.Manager.Checkpoint("MS-NRPC_R103743");
                    this.Manager.Comment("reaching state \'S1039\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeServerDigest/ERROR_NO_SUCH_USER\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SUCH_USER, temp627, "return of NetrLogonComputeServerDigest, state S1039");
                    this.Manager.Comment("reaching state \'S1305\'");
                    goto label178;
                }
                throw new InvalidOperationException("never reached");
            label178:
;
                goto label179;
            }
            throw new InvalidOperationException("never reached");
        label179:
;
            this.Manager.EndTest();
        }
        
        private void Test_AuthenticateMessageS74GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S75");
        }
        
        private void Test_AuthenticateMessageS74GetClientAccountTypeChecker(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S401");
        }
        
        private void Test_AuthenticateMessageS74GetClientAccountTypeChecker1(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S401");
        }
        
        private void Test_AuthenticateMessageS74GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S75");
        }
        
        private void Test_AuthenticateMessageS74GetClientAccountTypeChecker2(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S402");
        }
        
        private void Test_AuthenticateMessageS74GetClientAccountTypeChecker3(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S402");
        }
        
        private void Test_AuthenticateMessageS74GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S75");
        }
        
        private void Test_AuthenticateMessageS74GetClientAccountTypeChecker4(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S403");
        }
        
        private void Test_AuthenticateMessageS74GetClientAccountTypeChecker5(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S403");
        }
        #endregion
        
        #region Test Starting in S76
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_AuthenticateMessageS76() {
            this.Manager.BeginTest("Test_AuthenticateMessageS76");
            this.Manager.Comment("reaching state \'S76\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp630;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp630);
            this.Manager.AddReturn(GetPlatformInfo, null, temp630);
            this.Manager.Comment("reaching state \'S77\'");
            int temp643 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_AuthenticateMessageS76GetPlatformChecker)), new ExpectedReturn(Test_AuthenticateMessage.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_AuthenticateMessageS76GetPlatformChecker1)), new ExpectedReturn(Test_AuthenticateMessage.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_AuthenticateMessageS76GetPlatformChecker2)));
            if ((temp643 == 0)) {
                this.Manager.Comment("reaching state \'S230\'");
                bool temp631;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp631);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp631);
                this.Manager.Comment("reaching state \'S404\'");
                int temp634 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS76GetClientAccountTypeChecker)), new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS76GetClientAccountTypeChecker1)));
                if ((temp634 == 0)) {
                    this.Manager.Comment("reaching state \'S692\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp632;
                    this.Manager.Comment("executing step \'call NetrLogonSetServiceBits(PrimaryDc,1,8192)\'");
                    temp632 = this.INrpcServerAdapterInstance.NetrLogonSetServiceBits(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 1u, 8192u);
                    this.Manager.Comment("reaching state \'S1040\'");
                    this.Manager.Comment("checking step \'return NetrLogonSetServiceBits/ERROR_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_ACCESS_DENIED, temp632, "return of NetrLogonSetServiceBits, state S1040");
                    Test_AuthenticateMessageS1216();
                    goto label180;
                }
                if ((temp634 == 1)) {
                    this.Manager.Comment("reaching state \'S693\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp633;
                    this.Manager.Comment("executing step \'call NetrLogonSetServiceBits(PrimaryDc,1,0)\'");
                    temp633 = this.INrpcServerAdapterInstance.NetrLogonSetServiceBits(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 1u, 0u);
                    this.Manager.Comment("reaching state \'S1041\'");
                    this.Manager.Comment("checking step \'return NetrLogonSetServiceBits/STATUS_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp633, "return of NetrLogonSetServiceBits, state S1041");
                    Test_AuthenticateMessageS1217();
                    goto label180;
                }
                throw new InvalidOperationException("never reached");
            label180:
;
                goto label183;
            }
            if ((temp643 == 1)) {
                this.Manager.Comment("reaching state \'S231\'");
                bool temp635;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp635);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp635);
                this.Manager.Comment("reaching state \'S405\'");
                int temp638 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS76GetClientAccountTypeChecker2)), new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS76GetClientAccountTypeChecker3)));
                if ((temp638 == 0)) {
                    this.Manager.Comment("reaching state \'S694\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp636;
                    this.Manager.Comment("executing step \'call NetrLogonComputeClientDigest(PrimaryDc,TrustedDomainName,Mes" +
                            "sageOne)\'");
                    temp636 = this.INrpcServerAdapterInstance.NetrLogonComputeClientDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.TrustedDomainName, ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103789");
                    this.Manager.Comment("reaching state \'S1042\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeClientDigest/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp636, "return of NetrLogonComputeClientDigest, state S1042");
                    Test_AuthenticateMessageS1284();
                    goto label181;
                }
                if ((temp638 == 1)) {
                    this.Manager.Comment("reaching state \'S695\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp637;
                    this.Manager.Comment("executing step \'call NetrLogonComputeClientDigest(PrimaryDc,TrustedDomainName,Mes" +
                            "sageOne)\'");
                    temp637 = this.INrpcServerAdapterInstance.NetrLogonComputeClientDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.TrustedDomainName, ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103789");
                    this.Manager.Comment("reaching state \'S1043\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeClientDigest/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp637, "return of NetrLogonComputeClientDigest, state S1043");
                    Test_AuthenticateMessageS1285();
                    goto label181;
                }
                throw new InvalidOperationException("never reached");
            label181:
;
                goto label183;
            }
            if ((temp643 == 2)) {
                this.Manager.Comment("reaching state \'S232\'");
                bool temp639;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp639);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp639);
                this.Manager.Comment("reaching state \'S406\'");
                int temp642 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS76GetClientAccountTypeChecker4)), new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS76GetClientAccountTypeChecker5)));
                if ((temp642 == 0)) {
                    this.Manager.Comment("reaching state \'S696\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp640;
                    this.Manager.Comment("executing step \'call NetrLogonComputeServerDigest(PrimaryDc,RidOfNonMachineAccoun" +
                            "t,MessageOne)\'");
                    temp640 = this.INrpcServerAdapterInstance.NetrLogonComputeServerDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.RidType)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103756");
                    this.Manager.Checkpoint("MS-NRPC_R103743");
                    this.Manager.Comment("reaching state \'S1044\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeServerDigest/ERROR_NO_SUCH_USER\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SUCH_USER, temp640, "return of NetrLogonComputeServerDigest, state S1044");
                    this.Manager.Comment("reaching state \'S1306\'");
                    goto label182;
                }
                if ((temp642 == 1)) {
                    this.Manager.Comment("reaching state \'S697\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp641;
                    this.Manager.Comment("executing step \'call NetrLogonComputeServerDigest(PrimaryDc,RidOfNonMachineAccoun" +
                            "t,MessageOne)\'");
                    temp641 = this.INrpcServerAdapterInstance.NetrLogonComputeServerDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.RidType)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103756");
                    this.Manager.Checkpoint("MS-NRPC_R103743");
                    this.Manager.Comment("reaching state \'S1045\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeServerDigest/ERROR_NO_SUCH_USER\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SUCH_USER, temp641, "return of NetrLogonComputeServerDigest, state S1045");
                    this.Manager.Comment("reaching state \'S1307\'");
                    goto label182;
                }
                throw new InvalidOperationException("never reached");
            label182:
;
                goto label183;
            }
            throw new InvalidOperationException("never reached");
        label183:
;
            this.Manager.EndTest();
        }
        
        private void Test_AuthenticateMessageS76GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S77");
        }
        
        private void Test_AuthenticateMessageS76GetClientAccountTypeChecker(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S404");
        }
        
        private void Test_AuthenticateMessageS76GetClientAccountTypeChecker1(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S404");
        }
        
        private void Test_AuthenticateMessageS76GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S77");
        }
        
        private void Test_AuthenticateMessageS76GetClientAccountTypeChecker2(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S405");
        }
        
        private void Test_AuthenticateMessageS76GetClientAccountTypeChecker3(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S405");
        }
        
        private void Test_AuthenticateMessageS76GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S77");
        }
        
        private void Test_AuthenticateMessageS76GetClientAccountTypeChecker4(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S406");
        }
        
        private void Test_AuthenticateMessageS76GetClientAccountTypeChecker5(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S406");
        }
        #endregion
        
        #region Test Starting in S78
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_AuthenticateMessageS78() {
            this.Manager.BeginTest("Test_AuthenticateMessageS78");
            this.Manager.Comment("reaching state \'S78\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp644;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp644);
            this.Manager.AddReturn(GetPlatformInfo, null, temp644);
            this.Manager.Comment("reaching state \'S79\'");
            int temp657 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_AuthenticateMessageS78GetPlatformChecker)), new ExpectedReturn(Test_AuthenticateMessage.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_AuthenticateMessageS78GetPlatformChecker1)), new ExpectedReturn(Test_AuthenticateMessage.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_AuthenticateMessageS78GetPlatformChecker2)));
            if ((temp657 == 0)) {
                this.Manager.Comment("reaching state \'S233\'");
                bool temp645;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp645);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp645);
                this.Manager.Comment("reaching state \'S407\'");
                int temp648 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS78GetClientAccountTypeChecker)), new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS78GetClientAccountTypeChecker1)));
                if ((temp648 == 0)) {
                    this.Manager.Comment("reaching state \'S698\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp646;
                    this.Manager.Comment("executing step \'call NetrLogonSetServiceBits(PrimaryDc,64,8192)\'");
                    temp646 = this.INrpcServerAdapterInstance.NetrLogonSetServiceBits(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 64u, 8192u);
                    this.Manager.Comment("reaching state \'S1046\'");
                    this.Manager.Comment("checking step \'return NetrLogonSetServiceBits/ERROR_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_ACCESS_DENIED, temp646, "return of NetrLogonSetServiceBits, state S1046");
                    Test_AuthenticateMessageS1216();
                    goto label184;
                }
                if ((temp648 == 1)) {
                    this.Manager.Comment("reaching state \'S699\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp647;
                    this.Manager.Comment("executing step \'call NetrLogonSetServiceBits(PrimaryDc,1,512)\'");
                    temp647 = this.INrpcServerAdapterInstance.NetrLogonSetServiceBits(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 1u, 512u);
                    this.Manager.Comment("reaching state \'S1047\'");
                    this.Manager.Comment("checking step \'return NetrLogonSetServiceBits/STATUS_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_ACCESS_DENIED, temp647, "return of NetrLogonSetServiceBits, state S1047");
                    Test_AuthenticateMessageS1217();
                    goto label184;
                }
                throw new InvalidOperationException("never reached");
            label184:
;
                goto label187;
            }
            if ((temp657 == 1)) {
                this.Manager.Comment("reaching state \'S234\'");
                bool temp649;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp649);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp649);
                this.Manager.Comment("reaching state \'S408\'");
                int temp652 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS78GetClientAccountTypeChecker2)), new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS78GetClientAccountTypeChecker3)));
                if ((temp652 == 0)) {
                    this.Manager.Comment("reaching state \'S700\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp650;
                    this.Manager.Comment("executing step \'call NetrLogonComputeClientDigest(PrimaryDc,FqdnFormatDomainName," +
                            "MessageOne)\'");
                    temp650 = this.INrpcServerAdapterInstance.NetrLogonComputeClientDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.FqdnFormatDomainName, ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103789");
                    this.Manager.Comment("reaching state \'S1048\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeClientDigest/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp650, "return of NetrLogonComputeClientDigest, state S1048");
                    Test_AuthenticateMessageS1284();
                    goto label185;
                }
                if ((temp652 == 1)) {
                    this.Manager.Comment("reaching state \'S701\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp651;
                    this.Manager.Comment("executing step \'call NetrLogonComputeClientDigest(PrimaryDc,FqdnFormatDomainName," +
                            "MessageOne)\'");
                    temp651 = this.INrpcServerAdapterInstance.NetrLogonComputeClientDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.FqdnFormatDomainName, ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103789");
                    this.Manager.Comment("reaching state \'S1049\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeClientDigest/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp651, "return of NetrLogonComputeClientDigest, state S1049");
                    Test_AuthenticateMessageS1285();
                    goto label185;
                }
                throw new InvalidOperationException("never reached");
            label185:
;
                goto label187;
            }
            if ((temp657 == 2)) {
                this.Manager.Comment("reaching state \'S235\'");
                bool temp653;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp653);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp653);
                this.Manager.Comment("reaching state \'S409\'");
                int temp656 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS78GetClientAccountTypeChecker4)), new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS78GetClientAccountTypeChecker5)));
                if ((temp656 == 0)) {
                    this.Manager.Comment("reaching state \'S702\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp654;
                    this.Manager.Comment("executing step \'call NetrLogonComputeServerDigest(PrimaryDc,RidOfNonMachineAccoun" +
                            "t,MessageOne)\'");
                    temp654 = this.INrpcServerAdapterInstance.NetrLogonComputeServerDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.RidType)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103756");
                    this.Manager.Checkpoint("MS-NRPC_R103743");
                    this.Manager.Comment("reaching state \'S1050\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeServerDigest/ERROR_NO_SUCH_USER\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SUCH_USER, temp654, "return of NetrLogonComputeServerDigest, state S1050");
                    this.Manager.Comment("reaching state \'S1308\'");
                    goto label186;
                }
                if ((temp656 == 1)) {
                    this.Manager.Comment("reaching state \'S703\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp655;
                    this.Manager.Comment("executing step \'call NetrLogonComputeServerDigest(PrimaryDc,RidOfNonMachineAccoun" +
                            "t,MessageOne)\'");
                    temp655 = this.INrpcServerAdapterInstance.NetrLogonComputeServerDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.RidType)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103756");
                    this.Manager.Checkpoint("MS-NRPC_R103743");
                    this.Manager.Comment("reaching state \'S1051\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeServerDigest/ERROR_NO_SUCH_USER\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SUCH_USER, temp655, "return of NetrLogonComputeServerDigest, state S1051");
                    this.Manager.Comment("reaching state \'S1309\'");
                    goto label186;
                }
                throw new InvalidOperationException("never reached");
            label186:
;
                goto label187;
            }
            throw new InvalidOperationException("never reached");
        label187:
;
            this.Manager.EndTest();
        }
        
        private void Test_AuthenticateMessageS78GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S79");
        }
        
        private void Test_AuthenticateMessageS78GetClientAccountTypeChecker(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S407");
        }
        
        private void Test_AuthenticateMessageS78GetClientAccountTypeChecker1(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S407");
        }
        
        private void Test_AuthenticateMessageS78GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S79");
        }
        
        private void Test_AuthenticateMessageS78GetClientAccountTypeChecker2(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S408");
        }
        
        private void Test_AuthenticateMessageS78GetClientAccountTypeChecker3(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S408");
        }
        
        private void Test_AuthenticateMessageS78GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S79");
        }
        
        private void Test_AuthenticateMessageS78GetClientAccountTypeChecker4(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S409");
        }
        
        private void Test_AuthenticateMessageS78GetClientAccountTypeChecker5(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S409");
        }
        #endregion
        
        #region Test Starting in S8
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_AuthenticateMessageS8() {
            this.Manager.BeginTest("Test_AuthenticateMessageS8");
            this.Manager.Comment("reaching state \'S8\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp658;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp658);
            this.Manager.AddReturn(GetPlatformInfo, null, temp658);
            this.Manager.Comment("reaching state \'S9\'");
            int temp671 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_AuthenticateMessageS8GetPlatformChecker)), new ExpectedReturn(Test_AuthenticateMessage.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_AuthenticateMessageS8GetPlatformChecker1)), new ExpectedReturn(Test_AuthenticateMessage.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_AuthenticateMessageS8GetPlatformChecker2)));
            if ((temp671 == 0)) {
                this.Manager.Comment("reaching state \'S128\'");
                bool temp659;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp659);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp659);
                this.Manager.Comment("reaching state \'S302\'");
                int temp662 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS8GetClientAccountTypeChecker)), new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS8GetClientAccountTypeChecker1)));
                if ((temp662 == 0)) {
                    this.Manager.Comment("reaching state \'S488\'");
                    this.Manager.Comment("executing step \'call SwitchUserAccount(False)\'");
                    this.INrpcServerAdapterInstance.SwitchUserAccount(false);
                    this.Manager.Comment("reaching state \'S836\'");
                    this.Manager.Comment("checking step \'return SwitchUserAccount\'");
                    this.Manager.Comment("reaching state \'S1184\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp660;
                    this.Manager.Comment("executing step \'call NetrLogonComputeClientDigest(PrimaryDc,InvalidFormatDomainNa" +
                            "me,MessageOne)\'");
                    temp660 = this.INrpcServerAdapterInstance.NetrLogonComputeClientDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.InvalidFormatDomainName, ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103791");
                    this.Manager.Checkpoint("MS-NRPC_R103790");
                    this.Manager.Comment("reaching state \'S1394\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeClientDigest/ERROR_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_ACCESS_DENIED, temp660, "return of NetrLogonComputeClientDigest, state S1394");
                    Test_AuthenticateMessageS1448();
                    goto label188;
                }
                if ((temp662 == 1)) {
                    this.Manager.Comment("reaching state \'S489\'");
                    this.Manager.Comment("executing step \'call SwitchUserAccount(False)\'");
                    this.INrpcServerAdapterInstance.SwitchUserAccount(false);
                    this.Manager.Comment("reaching state \'S837\'");
                    this.Manager.Comment("checking step \'return SwitchUserAccount\'");
                    this.Manager.Comment("reaching state \'S1185\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp661;
                    this.Manager.Comment("executing step \'call NetrLogonComputeClientDigest(PrimaryDc,InvalidFormatDomainNa" +
                            "me,MessageOne)\'");
                    temp661 = this.INrpcServerAdapterInstance.NetrLogonComputeClientDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.InvalidFormatDomainName, ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103791");
                    this.Manager.Checkpoint("MS-NRPC_R103790");
                    this.Manager.Comment("reaching state \'S1395\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeClientDigest/ERROR_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_ACCESS_DENIED, temp661, "return of NetrLogonComputeClientDigest, state S1395");
                    Test_AuthenticateMessageS1449();
                    goto label188;
                }
                throw new InvalidOperationException("never reached");
            label188:
;
                goto label191;
            }
            if ((temp671 == 1)) {
                this.Manager.Comment("reaching state \'S129\'");
                bool temp663;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp663);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp663);
                this.Manager.Comment("reaching state \'S303\'");
                int temp666 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS8GetClientAccountTypeChecker2)), new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS8GetClientAccountTypeChecker3)));
                if ((temp666 == 0)) {
                    this.Manager.Comment("reaching state \'S490\'");
                    this.Manager.Comment("executing step \'call SwitchUserAccount(False)\'");
                    this.INrpcServerAdapterInstance.SwitchUserAccount(false);
                    this.Manager.Comment("reaching state \'S838\'");
                    this.Manager.Comment("checking step \'return SwitchUserAccount\'");
                    this.Manager.Comment("reaching state \'S1186\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp664;
                    this.Manager.Comment("executing step \'call NetrLogonComputeClientDigest(PrimaryDc,InvalidFormatDomainNa" +
                            "me,MessageOne)\'");
                    temp664 = this.INrpcServerAdapterInstance.NetrLogonComputeClientDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.InvalidFormatDomainName, ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103791");
                    this.Manager.Checkpoint("MS-NRPC_R103790");
                    this.Manager.Comment("reaching state \'S1396\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeClientDigest/ERROR_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_ACCESS_DENIED, temp664, "return of NetrLogonComputeClientDigest, state S1396");
                    Test_AuthenticateMessageS1450();
                    goto label189;
                }
                if ((temp666 == 1)) {
                    this.Manager.Comment("reaching state \'S491\'");
                    this.Manager.Comment("executing step \'call SwitchUserAccount(False)\'");
                    this.INrpcServerAdapterInstance.SwitchUserAccount(false);
                    this.Manager.Comment("reaching state \'S839\'");
                    this.Manager.Comment("checking step \'return SwitchUserAccount\'");
                    this.Manager.Comment("reaching state \'S1187\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp665;
                    this.Manager.Comment("executing step \'call NetrLogonComputeClientDigest(PrimaryDc,InvalidFormatDomainNa" +
                            "me,MessageOne)\'");
                    temp665 = this.INrpcServerAdapterInstance.NetrLogonComputeClientDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.InvalidFormatDomainName, ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103791");
                    this.Manager.Checkpoint("MS-NRPC_R103790");
                    this.Manager.Comment("reaching state \'S1397\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeClientDigest/ERROR_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_ACCESS_DENIED, temp665, "return of NetrLogonComputeClientDigest, state S1397");
                    Test_AuthenticateMessageS1451();
                    goto label189;
                }
                throw new InvalidOperationException("never reached");
            label189:
;
                goto label191;
            }
            if ((temp671 == 2)) {
                this.Manager.Comment("reaching state \'S130\'");
                bool temp667;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp667);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp667);
                this.Manager.Comment("reaching state \'S304\'");
                int temp670 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS8GetClientAccountTypeChecker4)), new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS8GetClientAccountTypeChecker5)));
                if ((temp670 == 0)) {
                    this.Manager.Comment("reaching state \'S492\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp668;
                    this.Manager.Comment("executing step \'call NetrLogonComputeServerDigest(PrimaryDc,RidOfNonMachineAccoun" +
                            "t,MessageOne)\'");
                    temp668 = this.INrpcServerAdapterInstance.NetrLogonComputeServerDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.RidType)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103756");
                    this.Manager.Checkpoint("MS-NRPC_R103743");
                    this.Manager.Comment("reaching state \'S840\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeServerDigest/ERROR_NO_SUCH_USER\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SUCH_USER, temp668, "return of NetrLogonComputeServerDigest, state S840");
                    this.Manager.Comment("reaching state \'S1188\'");
                    goto label190;
                }
                if ((temp670 == 1)) {
                    this.Manager.Comment("reaching state \'S493\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp669;
                    this.Manager.Comment("executing step \'call NetrLogonComputeServerDigest(PrimaryDc,RidOfNonMachineAccoun" +
                            "t,MessageOne)\'");
                    temp669 = this.INrpcServerAdapterInstance.NetrLogonComputeServerDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.RidType)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103756");
                    this.Manager.Checkpoint("MS-NRPC_R103743");
                    this.Manager.Comment("reaching state \'S841\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeServerDigest/ERROR_NO_SUCH_USER\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SUCH_USER, temp669, "return of NetrLogonComputeServerDigest, state S841");
                    this.Manager.Comment("reaching state \'S1189\'");
                    goto label190;
                }
                throw new InvalidOperationException("never reached");
            label190:
;
                goto label191;
            }
            throw new InvalidOperationException("never reached");
        label191:
;
            this.Manager.EndTest();
        }
        
        private void Test_AuthenticateMessageS8GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S9");
        }
        
        private void Test_AuthenticateMessageS8GetClientAccountTypeChecker(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S302");
        }
        
        private void Test_AuthenticateMessageS8GetClientAccountTypeChecker1(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S302");
        }
        
        private void Test_AuthenticateMessageS8GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S9");
        }
        
        private void Test_AuthenticateMessageS8GetClientAccountTypeChecker2(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S303");
        }
        
        private void Test_AuthenticateMessageS8GetClientAccountTypeChecker3(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S303");
        }
        
        private void Test_AuthenticateMessageS8GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S9");
        }
        
        private void Test_AuthenticateMessageS8GetClientAccountTypeChecker4(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S304");
        }
        
        private void Test_AuthenticateMessageS8GetClientAccountTypeChecker5(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S304");
        }
        #endregion
        
        #region Test Starting in S80
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_AuthenticateMessageS80() {
            this.Manager.BeginTest("Test_AuthenticateMessageS80");
            this.Manager.Comment("reaching state \'S80\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp672;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp672);
            this.Manager.AddReturn(GetPlatformInfo, null, temp672);
            this.Manager.Comment("reaching state \'S81\'");
            int temp685 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_AuthenticateMessageS80GetPlatformChecker)), new ExpectedReturn(Test_AuthenticateMessage.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_AuthenticateMessageS80GetPlatformChecker1)), new ExpectedReturn(Test_AuthenticateMessage.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_AuthenticateMessageS80GetPlatformChecker2)));
            if ((temp685 == 0)) {
                this.Manager.Comment("reaching state \'S236\'");
                bool temp673;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp673);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp673);
                this.Manager.Comment("reaching state \'S410\'");
                int temp676 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS80GetClientAccountTypeChecker)), new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS80GetClientAccountTypeChecker1)));
                if ((temp676 == 0)) {
                    this.Manager.Comment("reaching state \'S704\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp674;
                    this.Manager.Comment("executing step \'call NetrLogonSetServiceBits(PrimaryDc,0,8192)\'");
                    temp674 = this.INrpcServerAdapterInstance.NetrLogonSetServiceBits(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 0u, 8192u);
                    this.Manager.Comment("reaching state \'S1052\'");
                    this.Manager.Comment("checking step \'return NetrLogonSetServiceBits/ERROR_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_ACCESS_DENIED, temp674, "return of NetrLogonSetServiceBits, state S1052");
                    Test_AuthenticateMessageS1216();
                    goto label192;
                }
                if ((temp676 == 1)) {
                    this.Manager.Comment("reaching state \'S705\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp675;
                    this.Manager.Comment("executing step \'call NetrLogonSetServiceBits(PrimaryDc,0,8192)\'");
                    temp675 = this.INrpcServerAdapterInstance.NetrLogonSetServiceBits(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 0u, 8192u);
                    this.Manager.Comment("reaching state \'S1053\'");
                    this.Manager.Comment("checking step \'return NetrLogonSetServiceBits/STATUS_INVALID_PARAMETER\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_INVALID_PARAMETER, temp675, "return of NetrLogonSetServiceBits, state S1053");
                    Test_AuthenticateMessageS1217();
                    goto label192;
                }
                throw new InvalidOperationException("never reached");
            label192:
;
                goto label195;
            }
            if ((temp685 == 1)) {
                this.Manager.Comment("reaching state \'S237\'");
                bool temp677;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp677);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp677);
                this.Manager.Comment("reaching state \'S411\'");
                int temp680 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS80GetClientAccountTypeChecker2)), new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS80GetClientAccountTypeChecker3)));
                if ((temp680 == 0)) {
                    this.Manager.Comment("reaching state \'S706\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp678;
                    this.Manager.Comment("executing step \'call NetrLogonComputeClientDigest(PrimaryDc,EmptyDomainName,Messa" +
                            "geOne)\'");
                    temp678 = this.INrpcServerAdapterInstance.NetrLogonComputeClientDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.DomainNameType)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103789");
                    this.Manager.Comment("reaching state \'S1054\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeClientDigest/ERROR_NO_SUCH_DOMAIN\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SUCH_DOMAIN, temp678, "return of NetrLogonComputeClientDigest, state S1054");
                    Test_AuthenticateMessageS1284();
                    goto label193;
                }
                if ((temp680 == 1)) {
                    this.Manager.Comment("reaching state \'S707\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp679;
                    this.Manager.Comment("executing step \'call NetrLogonComputeClientDigest(PrimaryDc,EmptyDomainName,Messa" +
                            "geOne)\'");
                    temp679 = this.INrpcServerAdapterInstance.NetrLogonComputeClientDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.DomainNameType)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103789");
                    this.Manager.Comment("reaching state \'S1055\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeClientDigest/ERROR_NO_SUCH_DOMAIN\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SUCH_DOMAIN, temp679, "return of NetrLogonComputeClientDigest, state S1055");
                    Test_AuthenticateMessageS1285();
                    goto label193;
                }
                throw new InvalidOperationException("never reached");
            label193:
;
                goto label195;
            }
            if ((temp685 == 2)) {
                this.Manager.Comment("reaching state \'S238\'");
                bool temp681;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp681);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp681);
                this.Manager.Comment("reaching state \'S412\'");
                int temp684 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS80GetClientAccountTypeChecker4)), new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS80GetClientAccountTypeChecker5)));
                if ((temp684 == 0)) {
                    this.Manager.Comment("reaching state \'S708\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp682;
                    this.Manager.Comment("executing step \'call NetrLogonComputeServerDigest(PrimaryDc,RidOfNonMachineAccoun" +
                            "t,MessageOne)\'");
                    temp682 = this.INrpcServerAdapterInstance.NetrLogonComputeServerDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.RidType)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103756");
                    this.Manager.Checkpoint("MS-NRPC_R103743");
                    this.Manager.Comment("reaching state \'S1056\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeServerDigest/ERROR_NO_SUCH_USER\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SUCH_USER, temp682, "return of NetrLogonComputeServerDigest, state S1056");
                    this.Manager.Comment("reaching state \'S1310\'");
                    goto label194;
                }
                if ((temp684 == 1)) {
                    this.Manager.Comment("reaching state \'S709\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp683;
                    this.Manager.Comment("executing step \'call NetrLogonComputeServerDigest(PrimaryDc,RidOfNonMachineAccoun" +
                            "t,MessageOne)\'");
                    temp683 = this.INrpcServerAdapterInstance.NetrLogonComputeServerDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.RidType)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103756");
                    this.Manager.Checkpoint("MS-NRPC_R103743");
                    this.Manager.Comment("reaching state \'S1057\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeServerDigest/ERROR_NO_SUCH_USER\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SUCH_USER, temp683, "return of NetrLogonComputeServerDigest, state S1057");
                    this.Manager.Comment("reaching state \'S1311\'");
                    goto label194;
                }
                throw new InvalidOperationException("never reached");
            label194:
;
                goto label195;
            }
            throw new InvalidOperationException("never reached");
        label195:
;
            this.Manager.EndTest();
        }
        
        private void Test_AuthenticateMessageS80GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S81");
        }
        
        private void Test_AuthenticateMessageS80GetClientAccountTypeChecker(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S410");
        }
        
        private void Test_AuthenticateMessageS80GetClientAccountTypeChecker1(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S410");
        }
        
        private void Test_AuthenticateMessageS80GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S81");
        }
        
        private void Test_AuthenticateMessageS80GetClientAccountTypeChecker2(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S411");
        }
        
        private void Test_AuthenticateMessageS80GetClientAccountTypeChecker3(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S411");
        }
        
        private void Test_AuthenticateMessageS80GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S81");
        }
        
        private void Test_AuthenticateMessageS80GetClientAccountTypeChecker4(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S412");
        }
        
        private void Test_AuthenticateMessageS80GetClientAccountTypeChecker5(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S412");
        }
        #endregion
        
        #region Test Starting in S82
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_AuthenticateMessageS82() {
            this.Manager.BeginTest("Test_AuthenticateMessageS82");
            this.Manager.Comment("reaching state \'S82\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp686;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp686);
            this.Manager.AddReturn(GetPlatformInfo, null, temp686);
            this.Manager.Comment("reaching state \'S83\'");
            int temp699 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_AuthenticateMessageS82GetPlatformChecker)), new ExpectedReturn(Test_AuthenticateMessage.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_AuthenticateMessageS82GetPlatformChecker1)), new ExpectedReturn(Test_AuthenticateMessage.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_AuthenticateMessageS82GetPlatformChecker2)));
            if ((temp699 == 0)) {
                this.Manager.Comment("reaching state \'S239\'");
                bool temp687;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp687);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp687);
                this.Manager.Comment("reaching state \'S413\'");
                int temp690 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS82GetClientAccountTypeChecker)), new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS82GetClientAccountTypeChecker1)));
                if ((temp690 == 0)) {
                    this.Manager.Comment("reaching state \'S710\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp688;
                    this.Manager.Comment("executing step \'call NetrLogonSetServiceBits(PrimaryDc,8192,512)\'");
                    temp688 = this.INrpcServerAdapterInstance.NetrLogonSetServiceBits(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 8192u, 512u);
                    this.Manager.Comment("reaching state \'S1058\'");
                    this.Manager.Comment("checking step \'return NetrLogonSetServiceBits/ERROR_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_ACCESS_DENIED, temp688, "return of NetrLogonSetServiceBits, state S1058");
                    Test_AuthenticateMessageS1216();
                    goto label196;
                }
                if ((temp690 == 1)) {
                    this.Manager.Comment("reaching state \'S711\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp689;
                    this.Manager.Comment("executing step \'call NetrLogonSetServiceBits(PrimaryDc,8192,8192)\'");
                    temp689 = this.INrpcServerAdapterInstance.NetrLogonSetServiceBits(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 8192u, 8192u);
                    this.Manager.Comment("reaching state \'S1059\'");
                    this.Manager.Comment("checking step \'return NetrLogonSetServiceBits/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp689, "return of NetrLogonSetServiceBits, state S1059");
                    Test_AuthenticateMessageS1217();
                    goto label196;
                }
                throw new InvalidOperationException("never reached");
            label196:
;
                goto label199;
            }
            if ((temp699 == 1)) {
                this.Manager.Comment("reaching state \'S240\'");
                bool temp691;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp691);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp691);
                this.Manager.Comment("reaching state \'S414\'");
                int temp694 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS82GetClientAccountTypeChecker2)), new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS82GetClientAccountTypeChecker3)));
                if ((temp694 == 0)) {
                    this.Manager.Comment("reaching state \'S712\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp692;
                    this.Manager.Comment("executing step \'call NetrLogonComputeClientDigest(PrimaryDc,InvalidDomainName,Mes" +
                            "sageOne)\'");
                    temp692 = this.INrpcServerAdapterInstance.NetrLogonComputeClientDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.InvalidDomainName, ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103789");
                    this.Manager.Comment("reaching state \'S1060\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeClientDigest/ERROR_NO_SUCH_DOMAIN\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SUCH_DOMAIN, temp692, "return of NetrLogonComputeClientDigest, state S1060");
                    Test_AuthenticateMessageS1284();
                    goto label197;
                }
                if ((temp694 == 1)) {
                    this.Manager.Comment("reaching state \'S713\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp693;
                    this.Manager.Comment("executing step \'call NetrLogonComputeClientDigest(PrimaryDc,InvalidDomainName,Mes" +
                            "sageOne)\'");
                    temp693 = this.INrpcServerAdapterInstance.NetrLogonComputeClientDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.InvalidDomainName, ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103789");
                    this.Manager.Comment("reaching state \'S1061\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeClientDigest/ERROR_NO_SUCH_DOMAIN\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SUCH_DOMAIN, temp693, "return of NetrLogonComputeClientDigest, state S1061");
                    Test_AuthenticateMessageS1285();
                    goto label197;
                }
                throw new InvalidOperationException("never reached");
            label197:
;
                goto label199;
            }
            if ((temp699 == 2)) {
                this.Manager.Comment("reaching state \'S241\'");
                bool temp695;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp695);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp695);
                this.Manager.Comment("reaching state \'S415\'");
                int temp698 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS82GetClientAccountTypeChecker4)), new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS82GetClientAccountTypeChecker5)));
                if ((temp698 == 0)) {
                    this.Manager.Comment("reaching state \'S714\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp696;
                    this.Manager.Comment("executing step \'call NetrLogonComputeServerDigest(PrimaryDc,RidOfNonMachineAccoun" +
                            "t,MessageOne)\'");
                    temp696 = this.INrpcServerAdapterInstance.NetrLogonComputeServerDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.RidType)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103756");
                    this.Manager.Checkpoint("MS-NRPC_R103743");
                    this.Manager.Comment("reaching state \'S1062\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeServerDigest/ERROR_NO_SUCH_USER\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SUCH_USER, temp696, "return of NetrLogonComputeServerDigest, state S1062");
                    this.Manager.Comment("reaching state \'S1312\'");
                    goto label198;
                }
                if ((temp698 == 1)) {
                    this.Manager.Comment("reaching state \'S715\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp697;
                    this.Manager.Comment("executing step \'call NetrLogonComputeServerDigest(PrimaryDc,RidOfNonMachineAccoun" +
                            "t,MessageOne)\'");
                    temp697 = this.INrpcServerAdapterInstance.NetrLogonComputeServerDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.RidType)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103756");
                    this.Manager.Checkpoint("MS-NRPC_R103743");
                    this.Manager.Comment("reaching state \'S1063\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeServerDigest/ERROR_NO_SUCH_USER\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SUCH_USER, temp697, "return of NetrLogonComputeServerDigest, state S1063");
                    this.Manager.Comment("reaching state \'S1313\'");
                    goto label198;
                }
                throw new InvalidOperationException("never reached");
            label198:
;
                goto label199;
            }
            throw new InvalidOperationException("never reached");
        label199:
;
            this.Manager.EndTest();
        }
        
        private void Test_AuthenticateMessageS82GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S83");
        }
        
        private void Test_AuthenticateMessageS82GetClientAccountTypeChecker(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S413");
        }
        
        private void Test_AuthenticateMessageS82GetClientAccountTypeChecker1(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S413");
        }
        
        private void Test_AuthenticateMessageS82GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S83");
        }
        
        private void Test_AuthenticateMessageS82GetClientAccountTypeChecker2(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S414");
        }
        
        private void Test_AuthenticateMessageS82GetClientAccountTypeChecker3(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S414");
        }
        
        private void Test_AuthenticateMessageS82GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S83");
        }
        
        private void Test_AuthenticateMessageS82GetClientAccountTypeChecker4(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S415");
        }
        
        private void Test_AuthenticateMessageS82GetClientAccountTypeChecker5(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S415");
        }
        #endregion
        
        #region Test Starting in S84
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_AuthenticateMessageS84() {
            this.Manager.BeginTest("Test_AuthenticateMessageS84");
            this.Manager.Comment("reaching state \'S84\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp700;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp700);
            this.Manager.AddReturn(GetPlatformInfo, null, temp700);
            this.Manager.Comment("reaching state \'S85\'");
            int temp713 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_AuthenticateMessageS84GetPlatformChecker)), new ExpectedReturn(Test_AuthenticateMessage.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_AuthenticateMessageS84GetPlatformChecker1)), new ExpectedReturn(Test_AuthenticateMessage.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_AuthenticateMessageS84GetPlatformChecker2)));
            if ((temp713 == 0)) {
                this.Manager.Comment("reaching state \'S242\'");
                bool temp701;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp701);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp701);
                this.Manager.Comment("reaching state \'S416\'");
                int temp704 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS84GetClientAccountTypeChecker)), new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS84GetClientAccountTypeChecker1)));
                if ((temp704 == 0)) {
                    this.Manager.Comment("reaching state \'S716\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp702;
                    this.Manager.Comment("executing step \'call NetrLogonSetServiceBits(PrimaryDc,8192,1)\'");
                    temp702 = this.INrpcServerAdapterInstance.NetrLogonSetServiceBits(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 8192u, 1u);
                    this.Manager.Comment("reaching state \'S1064\'");
                    this.Manager.Comment("checking step \'return NetrLogonSetServiceBits/ERROR_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_ACCESS_DENIED, temp702, "return of NetrLogonSetServiceBits, state S1064");
                    Test_AuthenticateMessageS1216();
                    goto label200;
                }
                if ((temp704 == 1)) {
                    this.Manager.Comment("reaching state \'S717\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp703;
                    this.Manager.Comment("executing step \'call NetrLogonSetServiceBits(PrimaryDc,64,8192)\'");
                    temp703 = this.INrpcServerAdapterInstance.NetrLogonSetServiceBits(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 64u, 8192u);
                    this.Manager.Comment("reaching state \'S1065\'");
                    this.Manager.Comment("checking step \'return NetrLogonSetServiceBits/STATUS_INVALID_PARAMETER\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_INVALID_PARAMETER, temp703, "return of NetrLogonSetServiceBits, state S1065");
                    Test_AuthenticateMessageS1217();
                    goto label200;
                }
                throw new InvalidOperationException("never reached");
            label200:
;
                goto label203;
            }
            if ((temp713 == 1)) {
                this.Manager.Comment("reaching state \'S243\'");
                bool temp705;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp705);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp705);
                this.Manager.Comment("reaching state \'S417\'");
                int temp708 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS84GetClientAccountTypeChecker2)), new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS84GetClientAccountTypeChecker3)));
                if ((temp708 == 0)) {
                    this.Manager.Comment("reaching state \'S718\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp706;
                    this.Manager.Comment("executing step \'call NetrLogonComputeClientDigest(PrimaryDc,InvalidFormatDomainNa" +
                            "me,MessageOne)\'");
                    temp706 = this.INrpcServerAdapterInstance.NetrLogonComputeClientDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.InvalidFormatDomainName, ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103789");
                    this.Manager.Comment("reaching state \'S1066\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeClientDigest/ERROR_NO_SUCH_DOMAIN\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SUCH_DOMAIN, temp706, "return of NetrLogonComputeClientDigest, state S1066");
                    Test_AuthenticateMessageS1284();
                    goto label201;
                }
                if ((temp708 == 1)) {
                    this.Manager.Comment("reaching state \'S719\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp707;
                    this.Manager.Comment("executing step \'call NetrLogonComputeClientDigest(PrimaryDc,InvalidFormatDomainNa" +
                            "me,MessageOne)\'");
                    temp707 = this.INrpcServerAdapterInstance.NetrLogonComputeClientDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.InvalidFormatDomainName, ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103789");
                    this.Manager.Comment("reaching state \'S1067\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeClientDigest/ERROR_NO_SUCH_DOMAIN\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SUCH_DOMAIN, temp707, "return of NetrLogonComputeClientDigest, state S1067");
                    Test_AuthenticateMessageS1285();
                    goto label201;
                }
                throw new InvalidOperationException("never reached");
            label201:
;
                goto label203;
            }
            if ((temp713 == 2)) {
                this.Manager.Comment("reaching state \'S244\'");
                bool temp709;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp709);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp709);
                this.Manager.Comment("reaching state \'S418\'");
                int temp712 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS84GetClientAccountTypeChecker4)), new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS84GetClientAccountTypeChecker5)));
                if ((temp712 == 0)) {
                    this.Manager.Comment("reaching state \'S720\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp710;
                    this.Manager.Comment("executing step \'call NetrLogonComputeServerDigest(PrimaryDc,RidOfNonMachineAccoun" +
                            "t,MessageOne)\'");
                    temp710 = this.INrpcServerAdapterInstance.NetrLogonComputeServerDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.RidType)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103756");
                    this.Manager.Checkpoint("MS-NRPC_R103743");
                    this.Manager.Comment("reaching state \'S1068\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeServerDigest/ERROR_NO_SUCH_USER\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SUCH_USER, temp710, "return of NetrLogonComputeServerDigest, state S1068");
                    this.Manager.Comment("reaching state \'S1314\'");
                    goto label202;
                }
                if ((temp712 == 1)) {
                    this.Manager.Comment("reaching state \'S721\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp711;
                    this.Manager.Comment("executing step \'call NetrLogonComputeServerDigest(PrimaryDc,RidOfNonMachineAccoun" +
                            "t,MessageOne)\'");
                    temp711 = this.INrpcServerAdapterInstance.NetrLogonComputeServerDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.RidType)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103756");
                    this.Manager.Checkpoint("MS-NRPC_R103743");
                    this.Manager.Comment("reaching state \'S1069\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeServerDigest/ERROR_NO_SUCH_USER\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SUCH_USER, temp711, "return of NetrLogonComputeServerDigest, state S1069");
                    this.Manager.Comment("reaching state \'S1315\'");
                    goto label202;
                }
                throw new InvalidOperationException("never reached");
            label202:
;
                goto label203;
            }
            throw new InvalidOperationException("never reached");
        label203:
;
            this.Manager.EndTest();
        }
        
        private void Test_AuthenticateMessageS84GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S85");
        }
        
        private void Test_AuthenticateMessageS84GetClientAccountTypeChecker(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S416");
        }
        
        private void Test_AuthenticateMessageS84GetClientAccountTypeChecker1(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S416");
        }
        
        private void Test_AuthenticateMessageS84GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S85");
        }
        
        private void Test_AuthenticateMessageS84GetClientAccountTypeChecker2(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S417");
        }
        
        private void Test_AuthenticateMessageS84GetClientAccountTypeChecker3(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S417");
        }
        
        private void Test_AuthenticateMessageS84GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S85");
        }
        
        private void Test_AuthenticateMessageS84GetClientAccountTypeChecker4(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S418");
        }
        
        private void Test_AuthenticateMessageS84GetClientAccountTypeChecker5(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S418");
        }
        #endregion
        
        #region Test Starting in S86
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_AuthenticateMessageS86() {
            this.Manager.BeginTest("Test_AuthenticateMessageS86");
            this.Manager.Comment("reaching state \'S86\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp714;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp714);
            this.Manager.AddReturn(GetPlatformInfo, null, temp714);
            this.Manager.Comment("reaching state \'S87\'");
            int temp727 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_AuthenticateMessageS86GetPlatformChecker)), new ExpectedReturn(Test_AuthenticateMessage.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_AuthenticateMessageS86GetPlatformChecker1)), new ExpectedReturn(Test_AuthenticateMessage.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_AuthenticateMessageS86GetPlatformChecker2)));
            if ((temp727 == 0)) {
                this.Manager.Comment("reaching state \'S245\'");
                bool temp715;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp715);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp715);
                this.Manager.Comment("reaching state \'S419\'");
                int temp718 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS86GetClientAccountTypeChecker)), new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS86GetClientAccountTypeChecker1)));
                if ((temp718 == 0)) {
                    this.Manager.Comment("reaching state \'S722\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp716;
                    this.Manager.Comment("executing step \'call NetrLogonSetServiceBits(PrimaryDc,512,1)\'");
                    temp716 = this.INrpcServerAdapterInstance.NetrLogonSetServiceBits(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 512u, 1u);
                    this.Manager.Comment("reaching state \'S1070\'");
                    this.Manager.Comment("checking step \'return NetrLogonSetServiceBits/ERROR_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_ACCESS_DENIED, temp716, "return of NetrLogonSetServiceBits, state S1070");
                    Test_AuthenticateMessageS1216();
                    goto label204;
                }
                if ((temp718 == 1)) {
                    this.Manager.Comment("reaching state \'S723\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp717;
                    this.Manager.Comment("executing step \'call NetrLogonSetServiceBits(PrimaryDc,0,64)\'");
                    temp717 = this.INrpcServerAdapterInstance.NetrLogonSetServiceBits(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 0u, 64u);
                    this.Manager.Comment("reaching state \'S1071\'");
                    this.Manager.Comment("checking step \'return NetrLogonSetServiceBits/STATUS_INVALID_PARAMETER\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_INVALID_PARAMETER, temp717, "return of NetrLogonSetServiceBits, state S1071");
                    Test_AuthenticateMessageS1217();
                    goto label204;
                }
                throw new InvalidOperationException("never reached");
            label204:
;
                goto label207;
            }
            if ((temp727 == 1)) {
                this.Manager.Comment("reaching state \'S246\'");
                bool temp719;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp719);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp719);
                this.Manager.Comment("reaching state \'S420\'");
                int temp722 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS86GetClientAccountTypeChecker2)), new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS86GetClientAccountTypeChecker3)));
                if ((temp722 == 0)) {
                    this.Manager.Comment("reaching state \'S724\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp720;
                    this.Manager.Comment("executing step \'call NetrLogonComputeServerDigest(PrimaryDc,RidOfNonMachineAccoun" +
                            "t,MessageOne)\'");
                    temp720 = this.INrpcServerAdapterInstance.NetrLogonComputeServerDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.RidType)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103756");
                    this.Manager.Checkpoint("MS-NRPC_R103743");
                    this.Manager.Comment("reaching state \'S1072\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeServerDigest/ERROR_NO_SUCH_USER\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SUCH_USER, temp720, "return of NetrLogonComputeServerDigest, state S1072");
                    this.Manager.Comment("reaching state \'S1316\'");
                    goto label205;
                }
                if ((temp722 == 1)) {
                    this.Manager.Comment("reaching state \'S725\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp721;
                    this.Manager.Comment("executing step \'call NetrLogonComputeServerDigest(PrimaryDc,RidOfNonMachineAccoun" +
                            "t,MessageOne)\'");
                    temp721 = this.INrpcServerAdapterInstance.NetrLogonComputeServerDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.RidType)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103756");
                    this.Manager.Checkpoint("MS-NRPC_R103743");
                    this.Manager.Comment("reaching state \'S1073\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeServerDigest/ERROR_NO_SUCH_USER\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SUCH_USER, temp721, "return of NetrLogonComputeServerDigest, state S1073");
                    this.Manager.Comment("reaching state \'S1317\'");
                    goto label205;
                }
                throw new InvalidOperationException("never reached");
            label205:
;
                goto label207;
            }
            if ((temp727 == 2)) {
                this.Manager.Comment("reaching state \'S247\'");
                bool temp723;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp723);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp723);
                this.Manager.Comment("reaching state \'S421\'");
                int temp726 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS86GetClientAccountTypeChecker4)), new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS86GetClientAccountTypeChecker5)));
                if ((temp726 == 0)) {
                    this.Manager.Comment("reaching state \'S726\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp724;
                    this.Manager.Comment("executing step \'call NetrLogonComputeServerDigest(PrimaryDc,RidOfNonMachineAccoun" +
                            "t,MessageOne)\'");
                    temp724 = this.INrpcServerAdapterInstance.NetrLogonComputeServerDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.RidType)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103756");
                    this.Manager.Checkpoint("MS-NRPC_R103743");
                    this.Manager.Comment("reaching state \'S1074\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeServerDigest/ERROR_NO_SUCH_USER\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SUCH_USER, temp724, "return of NetrLogonComputeServerDigest, state S1074");
                    this.Manager.Comment("reaching state \'S1318\'");
                    goto label206;
                }
                if ((temp726 == 1)) {
                    this.Manager.Comment("reaching state \'S727\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp725;
                    this.Manager.Comment("executing step \'call NetrLogonComputeServerDigest(PrimaryDc,RidOfNonMachineAccoun" +
                            "t,MessageOne)\'");
                    temp725 = this.INrpcServerAdapterInstance.NetrLogonComputeServerDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.RidType)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103756");
                    this.Manager.Checkpoint("MS-NRPC_R103743");
                    this.Manager.Comment("reaching state \'S1075\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeServerDigest/ERROR_NO_SUCH_USER\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SUCH_USER, temp725, "return of NetrLogonComputeServerDigest, state S1075");
                    this.Manager.Comment("reaching state \'S1319\'");
                    goto label206;
                }
                throw new InvalidOperationException("never reached");
            label206:
;
                goto label207;
            }
            throw new InvalidOperationException("never reached");
        label207:
;
            this.Manager.EndTest();
        }
        
        private void Test_AuthenticateMessageS86GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S87");
        }
        
        private void Test_AuthenticateMessageS86GetClientAccountTypeChecker(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S419");
        }
        
        private void Test_AuthenticateMessageS86GetClientAccountTypeChecker1(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S419");
        }
        
        private void Test_AuthenticateMessageS86GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S87");
        }
        
        private void Test_AuthenticateMessageS86GetClientAccountTypeChecker2(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S420");
        }
        
        private void Test_AuthenticateMessageS86GetClientAccountTypeChecker3(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S420");
        }
        
        private void Test_AuthenticateMessageS86GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S87");
        }
        
        private void Test_AuthenticateMessageS86GetClientAccountTypeChecker4(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S421");
        }
        
        private void Test_AuthenticateMessageS86GetClientAccountTypeChecker5(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S421");
        }
        #endregion
        
        #region Test Starting in S88
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_AuthenticateMessageS88() {
            this.Manager.BeginTest("Test_AuthenticateMessageS88");
            this.Manager.Comment("reaching state \'S88\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp728;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp728);
            this.Manager.AddReturn(GetPlatformInfo, null, temp728);
            this.Manager.Comment("reaching state \'S89\'");
            int temp741 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_AuthenticateMessageS88GetPlatformChecker)), new ExpectedReturn(Test_AuthenticateMessage.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_AuthenticateMessageS88GetPlatformChecker1)), new ExpectedReturn(Test_AuthenticateMessage.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_AuthenticateMessageS88GetPlatformChecker2)));
            if ((temp741 == 0)) {
                this.Manager.Comment("reaching state \'S248\'");
                bool temp729;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp729);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp729);
                this.Manager.Comment("reaching state \'S422\'");
                int temp732 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS88GetClientAccountTypeChecker)), new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS88GetClientAccountTypeChecker1)));
                if ((temp732 == 0)) {
                    this.Manager.Comment("reaching state \'S728\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp730;
                    this.Manager.Comment("executing step \'call NetrLogonSetServiceBits(PrimaryDc,0,1)\'");
                    temp730 = this.INrpcServerAdapterInstance.NetrLogonSetServiceBits(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 0u, 1u);
                    this.Manager.Comment("reaching state \'S1076\'");
                    this.Manager.Comment("checking step \'return NetrLogonSetServiceBits/ERROR_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_ACCESS_DENIED, temp730, "return of NetrLogonSetServiceBits, state S1076");
                    Test_AuthenticateMessageS1216();
                    goto label208;
                }
                if ((temp732 == 1)) {
                    this.Manager.Comment("reaching state \'S729\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp731;
                    this.Manager.Comment("executing step \'call NetrLogonSetServiceBits(PrimaryDc,0,0)\'");
                    temp731 = this.INrpcServerAdapterInstance.NetrLogonSetServiceBits(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 0u, 0u);
                    this.Manager.Comment("reaching state \'S1077\'");
                    this.Manager.Comment("checking step \'return NetrLogonSetServiceBits/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp731, "return of NetrLogonSetServiceBits, state S1077");
                    Test_AuthenticateMessageS1217();
                    goto label208;
                }
                throw new InvalidOperationException("never reached");
            label208:
;
                goto label211;
            }
            if ((temp741 == 1)) {
                this.Manager.Comment("reaching state \'S249\'");
                bool temp733;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp733);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp733);
                this.Manager.Comment("reaching state \'S423\'");
                int temp736 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS88GetClientAccountTypeChecker2)), new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS88GetClientAccountTypeChecker3)));
                if ((temp736 == 0)) {
                    this.Manager.Comment("reaching state \'S730\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp734;
                    this.Manager.Comment("executing step \'call NetrLogonComputeServerDigest(PrimaryDc,RidOfNonMachineAccoun" +
                            "t,MessageOne)\'");
                    temp734 = this.INrpcServerAdapterInstance.NetrLogonComputeServerDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.RidType)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103756");
                    this.Manager.Checkpoint("MS-NRPC_R103743");
                    this.Manager.Comment("reaching state \'S1078\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeServerDigest/ERROR_NO_SUCH_USER\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SUCH_USER, temp734, "return of NetrLogonComputeServerDigest, state S1078");
                    this.Manager.Comment("reaching state \'S1320\'");
                    goto label209;
                }
                if ((temp736 == 1)) {
                    this.Manager.Comment("reaching state \'S731\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp735;
                    this.Manager.Comment("executing step \'call NetrLogonComputeServerDigest(PrimaryDc,RidOfNonMachineAccoun" +
                            "t,MessageOne)\'");
                    temp735 = this.INrpcServerAdapterInstance.NetrLogonComputeServerDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.RidType)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103756");
                    this.Manager.Checkpoint("MS-NRPC_R103743");
                    this.Manager.Comment("reaching state \'S1079\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeServerDigest/ERROR_NO_SUCH_USER\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SUCH_USER, temp735, "return of NetrLogonComputeServerDigest, state S1079");
                    this.Manager.Comment("reaching state \'S1321\'");
                    goto label209;
                }
                throw new InvalidOperationException("never reached");
            label209:
;
                goto label211;
            }
            if ((temp741 == 2)) {
                this.Manager.Comment("reaching state \'S250\'");
                bool temp737;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp737);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp737);
                this.Manager.Comment("reaching state \'S424\'");
                int temp740 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS88GetClientAccountTypeChecker4)), new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS88GetClientAccountTypeChecker5)));
                if ((temp740 == 0)) {
                    this.Manager.Comment("reaching state \'S732\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp738;
                    this.Manager.Comment("executing step \'call NetrLogonComputeServerDigest(PrimaryDc,RidOfNonMachineAccoun" +
                            "t,MessageOne)\'");
                    temp738 = this.INrpcServerAdapterInstance.NetrLogonComputeServerDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.RidType)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103756");
                    this.Manager.Checkpoint("MS-NRPC_R103743");
                    this.Manager.Comment("reaching state \'S1080\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeServerDigest/ERROR_NO_SUCH_USER\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SUCH_USER, temp738, "return of NetrLogonComputeServerDigest, state S1080");
                    this.Manager.Comment("reaching state \'S1322\'");
                    goto label210;
                }
                if ((temp740 == 1)) {
                    this.Manager.Comment("reaching state \'S733\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp739;
                    this.Manager.Comment("executing step \'call NetrLogonComputeServerDigest(PrimaryDc,RidOfNonMachineAccoun" +
                            "t,MessageOne)\'");
                    temp739 = this.INrpcServerAdapterInstance.NetrLogonComputeServerDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.RidType)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103756");
                    this.Manager.Checkpoint("MS-NRPC_R103743");
                    this.Manager.Comment("reaching state \'S1081\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeServerDigest/ERROR_NO_SUCH_USER\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SUCH_USER, temp739, "return of NetrLogonComputeServerDigest, state S1081");
                    this.Manager.Comment("reaching state \'S1323\'");
                    goto label210;
                }
                throw new InvalidOperationException("never reached");
            label210:
;
                goto label211;
            }
            throw new InvalidOperationException("never reached");
        label211:
;
            this.Manager.EndTest();
        }
        
        private void Test_AuthenticateMessageS88GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S89");
        }
        
        private void Test_AuthenticateMessageS88GetClientAccountTypeChecker(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S422");
        }
        
        private void Test_AuthenticateMessageS88GetClientAccountTypeChecker1(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S422");
        }
        
        private void Test_AuthenticateMessageS88GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S89");
        }
        
        private void Test_AuthenticateMessageS88GetClientAccountTypeChecker2(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S423");
        }
        
        private void Test_AuthenticateMessageS88GetClientAccountTypeChecker3(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S423");
        }
        
        private void Test_AuthenticateMessageS88GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S89");
        }
        
        private void Test_AuthenticateMessageS88GetClientAccountTypeChecker4(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S424");
        }
        
        private void Test_AuthenticateMessageS88GetClientAccountTypeChecker5(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S424");
        }
        #endregion
        
        #region Test Starting in S90
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_AuthenticateMessageS90() {
            this.Manager.BeginTest("Test_AuthenticateMessageS90");
            this.Manager.Comment("reaching state \'S90\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp742;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp742);
            this.Manager.AddReturn(GetPlatformInfo, null, temp742);
            this.Manager.Comment("reaching state \'S91\'");
            int temp755 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_AuthenticateMessageS90GetPlatformChecker)), new ExpectedReturn(Test_AuthenticateMessage.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_AuthenticateMessageS90GetPlatformChecker1)), new ExpectedReturn(Test_AuthenticateMessage.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_AuthenticateMessageS90GetPlatformChecker2)));
            if ((temp755 == 0)) {
                this.Manager.Comment("reaching state \'S251\'");
                bool temp743;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp743);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp743);
                this.Manager.Comment("reaching state \'S425\'");
                int temp746 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS90GetClientAccountTypeChecker)), new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS90GetClientAccountTypeChecker1)));
                if ((temp746 == 0)) {
                    this.Manager.Comment("reaching state \'S734\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp744;
                    this.Manager.Comment("executing step \'call NetrLogonSetServiceBits(PrimaryDc,64,1)\'");
                    temp744 = this.INrpcServerAdapterInstance.NetrLogonSetServiceBits(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 64u, 1u);
                    this.Manager.Comment("reaching state \'S1082\'");
                    this.Manager.Comment("checking step \'return NetrLogonSetServiceBits/ERROR_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_ACCESS_DENIED, temp744, "return of NetrLogonSetServiceBits, state S1082");
                    Test_AuthenticateMessageS1216();
                    goto label212;
                }
                if ((temp746 == 1)) {
                    this.Manager.Comment("reaching state \'S735\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp745;
                    this.Manager.Comment("executing step \'call NetrLogonSetServiceBits(PrimaryDc,0,512)\'");
                    temp745 = this.INrpcServerAdapterInstance.NetrLogonSetServiceBits(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 0u, 512u);
                    this.Manager.Comment("reaching state \'S1083\'");
                    this.Manager.Comment("checking step \'return NetrLogonSetServiceBits/STATUS_INVALID_PARAMETER\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_INVALID_PARAMETER, temp745, "return of NetrLogonSetServiceBits, state S1083");
                    Test_AuthenticateMessageS1217();
                    goto label212;
                }
                throw new InvalidOperationException("never reached");
            label212:
;
                goto label215;
            }
            if ((temp755 == 1)) {
                this.Manager.Comment("reaching state \'S252\'");
                bool temp747;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp747);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp747);
                this.Manager.Comment("reaching state \'S426\'");
                int temp750 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS90GetClientAccountTypeChecker2)), new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS90GetClientAccountTypeChecker3)));
                if ((temp750 == 0)) {
                    this.Manager.Comment("reaching state \'S736\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp748;
                    this.Manager.Comment("executing step \'call NetrLogonComputeServerDigest(PrimaryDc,RidOfNonMachineAccoun" +
                            "t,MessageOne)\'");
                    temp748 = this.INrpcServerAdapterInstance.NetrLogonComputeServerDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.RidType)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103756");
                    this.Manager.Checkpoint("MS-NRPC_R103743");
                    this.Manager.Comment("reaching state \'S1084\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeServerDigest/ERROR_NO_SUCH_USER\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SUCH_USER, temp748, "return of NetrLogonComputeServerDigest, state S1084");
                    this.Manager.Comment("reaching state \'S1324\'");
                    goto label213;
                }
                if ((temp750 == 1)) {
                    this.Manager.Comment("reaching state \'S737\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp749;
                    this.Manager.Comment("executing step \'call NetrLogonComputeServerDigest(PrimaryDc,RidOfNonMachineAccoun" +
                            "t,MessageOne)\'");
                    temp749 = this.INrpcServerAdapterInstance.NetrLogonComputeServerDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.RidType)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103756");
                    this.Manager.Checkpoint("MS-NRPC_R103743");
                    this.Manager.Comment("reaching state \'S1085\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeServerDigest/ERROR_NO_SUCH_USER\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SUCH_USER, temp749, "return of NetrLogonComputeServerDigest, state S1085");
                    this.Manager.Comment("reaching state \'S1325\'");
                    goto label213;
                }
                throw new InvalidOperationException("never reached");
            label213:
;
                goto label215;
            }
            if ((temp755 == 2)) {
                this.Manager.Comment("reaching state \'S253\'");
                bool temp751;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp751);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp751);
                this.Manager.Comment("reaching state \'S427\'");
                int temp754 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS90GetClientAccountTypeChecker4)), new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS90GetClientAccountTypeChecker5)));
                if ((temp754 == 0)) {
                    this.Manager.Comment("reaching state \'S738\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp752;
                    this.Manager.Comment("executing step \'call NetrLogonComputeServerDigest(PrimaryDc,RidOfNonMachineAccoun" +
                            "t,MessageOne)\'");
                    temp752 = this.INrpcServerAdapterInstance.NetrLogonComputeServerDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.RidType)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103756");
                    this.Manager.Checkpoint("MS-NRPC_R103743");
                    this.Manager.Comment("reaching state \'S1086\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeServerDigest/ERROR_NO_SUCH_USER\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SUCH_USER, temp752, "return of NetrLogonComputeServerDigest, state S1086");
                    this.Manager.Comment("reaching state \'S1326\'");
                    goto label214;
                }
                if ((temp754 == 1)) {
                    this.Manager.Comment("reaching state \'S739\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp753;
                    this.Manager.Comment("executing step \'call NetrLogonComputeServerDigest(PrimaryDc,RidOfNonMachineAccoun" +
                            "t,MessageOne)\'");
                    temp753 = this.INrpcServerAdapterInstance.NetrLogonComputeServerDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.RidType)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103756");
                    this.Manager.Checkpoint("MS-NRPC_R103743");
                    this.Manager.Comment("reaching state \'S1087\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeServerDigest/ERROR_NO_SUCH_USER\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SUCH_USER, temp753, "return of NetrLogonComputeServerDigest, state S1087");
                    this.Manager.Comment("reaching state \'S1327\'");
                    goto label214;
                }
                throw new InvalidOperationException("never reached");
            label214:
;
                goto label215;
            }
            throw new InvalidOperationException("never reached");
        label215:
;
            this.Manager.EndTest();
        }
        
        private void Test_AuthenticateMessageS90GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S91");
        }
        
        private void Test_AuthenticateMessageS90GetClientAccountTypeChecker(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S425");
        }
        
        private void Test_AuthenticateMessageS90GetClientAccountTypeChecker1(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S425");
        }
        
        private void Test_AuthenticateMessageS90GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S91");
        }
        
        private void Test_AuthenticateMessageS90GetClientAccountTypeChecker2(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S426");
        }
        
        private void Test_AuthenticateMessageS90GetClientAccountTypeChecker3(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S426");
        }
        
        private void Test_AuthenticateMessageS90GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S91");
        }
        
        private void Test_AuthenticateMessageS90GetClientAccountTypeChecker4(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S427");
        }
        
        private void Test_AuthenticateMessageS90GetClientAccountTypeChecker5(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S427");
        }
        #endregion
        
        #region Test Starting in S92
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_AuthenticateMessageS92() {
            this.Manager.BeginTest("Test_AuthenticateMessageS92");
            this.Manager.Comment("reaching state \'S92\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp756;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp756);
            this.Manager.AddReturn(GetPlatformInfo, null, temp756);
            this.Manager.Comment("reaching state \'S93\'");
            int temp769 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_AuthenticateMessageS92GetPlatformChecker)), new ExpectedReturn(Test_AuthenticateMessage.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_AuthenticateMessageS92GetPlatformChecker1)), new ExpectedReturn(Test_AuthenticateMessage.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_AuthenticateMessageS92GetPlatformChecker2)));
            if ((temp769 == 0)) {
                this.Manager.Comment("reaching state \'S254\'");
                bool temp757;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp757);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp757);
                this.Manager.Comment("reaching state \'S428\'");
                int temp760 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS92GetClientAccountTypeChecker)), new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS92GetClientAccountTypeChecker1)));
                if ((temp760 == 0)) {
                    this.Manager.Comment("reaching state \'S740\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp758;
                    this.Manager.Comment("executing step \'call NetrLogonSetServiceBits(PrimaryDc,1,1)\'");
                    temp758 = this.INrpcServerAdapterInstance.NetrLogonSetServiceBits(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 1u, 1u);
                    this.Manager.Comment("reaching state \'S1088\'");
                    this.Manager.Comment("checking step \'return NetrLogonSetServiceBits/ERROR_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_ACCESS_DENIED, temp758, "return of NetrLogonSetServiceBits, state S1088");
                    Test_AuthenticateMessageS1216();
                    goto label216;
                }
                if ((temp760 == 1)) {
                    this.Manager.Comment("reaching state \'S741\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp759;
                    this.Manager.Comment("executing step \'call NetrLogonSetServiceBits(PrimaryDc,8192,64)\'");
                    temp759 = this.INrpcServerAdapterInstance.NetrLogonSetServiceBits(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 8192u, 64u);
                    this.Manager.Comment("reaching state \'S1089\'");
                    this.Manager.Comment("checking step \'return NetrLogonSetServiceBits/STATUS_INVALID_PARAMETER\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_INVALID_PARAMETER, temp759, "return of NetrLogonSetServiceBits, state S1089");
                    Test_AuthenticateMessageS1217();
                    goto label216;
                }
                throw new InvalidOperationException("never reached");
            label216:
;
                goto label219;
            }
            if ((temp769 == 1)) {
                this.Manager.Comment("reaching state \'S255\'");
                bool temp761;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp761);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp761);
                this.Manager.Comment("reaching state \'S429\'");
                int temp764 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS92GetClientAccountTypeChecker2)), new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS92GetClientAccountTypeChecker3)));
                if ((temp764 == 0)) {
                    this.Manager.Comment("reaching state \'S742\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp762;
                    this.Manager.Comment("executing step \'call NetrLogonComputeServerDigest(PrimaryDc,RidOfNonMachineAccoun" +
                            "t,MessageOne)\'");
                    temp762 = this.INrpcServerAdapterInstance.NetrLogonComputeServerDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.RidType)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103756");
                    this.Manager.Checkpoint("MS-NRPC_R103743");
                    this.Manager.Comment("reaching state \'S1090\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeServerDigest/ERROR_NO_SUCH_USER\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SUCH_USER, temp762, "return of NetrLogonComputeServerDigest, state S1090");
                    this.Manager.Comment("reaching state \'S1328\'");
                    goto label217;
                }
                if ((temp764 == 1)) {
                    this.Manager.Comment("reaching state \'S743\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp763;
                    this.Manager.Comment("executing step \'call NetrLogonComputeServerDigest(PrimaryDc,RidOfNonMachineAccoun" +
                            "t,MessageOne)\'");
                    temp763 = this.INrpcServerAdapterInstance.NetrLogonComputeServerDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.RidType)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103756");
                    this.Manager.Checkpoint("MS-NRPC_R103743");
                    this.Manager.Comment("reaching state \'S1091\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeServerDigest/ERROR_NO_SUCH_USER\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SUCH_USER, temp763, "return of NetrLogonComputeServerDigest, state S1091");
                    this.Manager.Comment("reaching state \'S1329\'");
                    goto label217;
                }
                throw new InvalidOperationException("never reached");
            label217:
;
                goto label219;
            }
            if ((temp769 == 2)) {
                this.Manager.Comment("reaching state \'S256\'");
                bool temp765;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp765);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp765);
                this.Manager.Comment("reaching state \'S430\'");
                int temp768 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS92GetClientAccountTypeChecker4)), new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS92GetClientAccountTypeChecker5)));
                if ((temp768 == 0)) {
                    this.Manager.Comment("reaching state \'S744\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp766;
                    this.Manager.Comment("executing step \'call NetrLogonComputeServerDigest(PrimaryDc,RidOfNonMachineAccoun" +
                            "t,MessageOne)\'");
                    temp766 = this.INrpcServerAdapterInstance.NetrLogonComputeServerDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.RidType)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103756");
                    this.Manager.Checkpoint("MS-NRPC_R103743");
                    this.Manager.Comment("reaching state \'S1092\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeServerDigest/ERROR_NO_SUCH_USER\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SUCH_USER, temp766, "return of NetrLogonComputeServerDigest, state S1092");
                    this.Manager.Comment("reaching state \'S1330\'");
                    goto label218;
                }
                if ((temp768 == 1)) {
                    this.Manager.Comment("reaching state \'S745\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp767;
                    this.Manager.Comment("executing step \'call NetrLogonComputeServerDigest(PrimaryDc,RidOfNonMachineAccoun" +
                            "t,MessageOne)\'");
                    temp767 = this.INrpcServerAdapterInstance.NetrLogonComputeServerDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.RidType)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103756");
                    this.Manager.Checkpoint("MS-NRPC_R103743");
                    this.Manager.Comment("reaching state \'S1093\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeServerDigest/ERROR_NO_SUCH_USER\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SUCH_USER, temp767, "return of NetrLogonComputeServerDigest, state S1093");
                    this.Manager.Comment("reaching state \'S1331\'");
                    goto label218;
                }
                throw new InvalidOperationException("never reached");
            label218:
;
                goto label219;
            }
            throw new InvalidOperationException("never reached");
        label219:
;
            this.Manager.EndTest();
        }
        
        private void Test_AuthenticateMessageS92GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S93");
        }
        
        private void Test_AuthenticateMessageS92GetClientAccountTypeChecker(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S428");
        }
        
        private void Test_AuthenticateMessageS92GetClientAccountTypeChecker1(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S428");
        }
        
        private void Test_AuthenticateMessageS92GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S93");
        }
        
        private void Test_AuthenticateMessageS92GetClientAccountTypeChecker2(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S429");
        }
        
        private void Test_AuthenticateMessageS92GetClientAccountTypeChecker3(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S429");
        }
        
        private void Test_AuthenticateMessageS92GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S93");
        }
        
        private void Test_AuthenticateMessageS92GetClientAccountTypeChecker4(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S430");
        }
        
        private void Test_AuthenticateMessageS92GetClientAccountTypeChecker5(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S430");
        }
        #endregion
        
        #region Test Starting in S94
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_AuthenticateMessageS94() {
            this.Manager.BeginTest("Test_AuthenticateMessageS94");
            this.Manager.Comment("reaching state \'S94\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp770;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp770);
            this.Manager.AddReturn(GetPlatformInfo, null, temp770);
            this.Manager.Comment("reaching state \'S95\'");
            int temp783 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_AuthenticateMessageS94GetPlatformChecker)), new ExpectedReturn(Test_AuthenticateMessage.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_AuthenticateMessageS94GetPlatformChecker1)), new ExpectedReturn(Test_AuthenticateMessage.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_AuthenticateMessageS94GetPlatformChecker2)));
            if ((temp783 == 0)) {
                this.Manager.Comment("reaching state \'S257\'");
                bool temp771;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp771);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp771);
                this.Manager.Comment("reaching state \'S431\'");
                int temp774 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS94GetClientAccountTypeChecker)), new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS94GetClientAccountTypeChecker1)));
                if ((temp774 == 0)) {
                    this.Manager.Comment("reaching state \'S746\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp772;
                    this.Manager.Comment("executing step \'call NetrLogonSetServiceBits(PrimaryDc,0,512)\'");
                    temp772 = this.INrpcServerAdapterInstance.NetrLogonSetServiceBits(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 0u, 512u);
                    this.Manager.Comment("reaching state \'S1094\'");
                    this.Manager.Comment("checking step \'return NetrLogonSetServiceBits/ERROR_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_ACCESS_DENIED, temp772, "return of NetrLogonSetServiceBits, state S1094");
                    Test_AuthenticateMessageS1216();
                    goto label220;
                }
                if ((temp774 == 1)) {
                    this.Manager.Comment("reaching state \'S747\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp773;
                    this.Manager.Comment("executing step \'call NetrLogonSetServiceBits(PrimaryDc,64,64)\'");
                    temp773 = this.INrpcServerAdapterInstance.NetrLogonSetServiceBits(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 64u, 64u);
                    this.Manager.Comment("reaching state \'S1095\'");
                    this.Manager.Comment("checking step \'return NetrLogonSetServiceBits/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp773, "return of NetrLogonSetServiceBits, state S1095");
                    Test_AuthenticateMessageS1217();
                    goto label220;
                }
                throw new InvalidOperationException("never reached");
            label220:
;
                goto label223;
            }
            if ((temp783 == 1)) {
                this.Manager.Comment("reaching state \'S258\'");
                bool temp775;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp775);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp775);
                this.Manager.Comment("reaching state \'S432\'");
                int temp778 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS94GetClientAccountTypeChecker2)), new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS94GetClientAccountTypeChecker3)));
                if ((temp778 == 0)) {
                    this.Manager.Comment("reaching state \'S748\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp776;
                    this.Manager.Comment("executing step \'call NetrLogonComputeServerDigest(PrimaryDc,RidOfNonMachineAccoun" +
                            "t,MessageOne)\'");
                    temp776 = this.INrpcServerAdapterInstance.NetrLogonComputeServerDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.RidType)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103756");
                    this.Manager.Checkpoint("MS-NRPC_R103743");
                    this.Manager.Comment("reaching state \'S1096\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeServerDigest/ERROR_NO_SUCH_USER\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SUCH_USER, temp776, "return of NetrLogonComputeServerDigest, state S1096");
                    this.Manager.Comment("reaching state \'S1332\'");
                    goto label221;
                }
                if ((temp778 == 1)) {
                    this.Manager.Comment("reaching state \'S749\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp777;
                    this.Manager.Comment("executing step \'call NetrLogonComputeServerDigest(PrimaryDc,RidOfNonMachineAccoun" +
                            "t,MessageOne)\'");
                    temp777 = this.INrpcServerAdapterInstance.NetrLogonComputeServerDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.RidType)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103756");
                    this.Manager.Checkpoint("MS-NRPC_R103743");
                    this.Manager.Comment("reaching state \'S1097\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeServerDigest/ERROR_NO_SUCH_USER\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SUCH_USER, temp777, "return of NetrLogonComputeServerDigest, state S1097");
                    this.Manager.Comment("reaching state \'S1333\'");
                    goto label221;
                }
                throw new InvalidOperationException("never reached");
            label221:
;
                goto label223;
            }
            if ((temp783 == 2)) {
                this.Manager.Comment("reaching state \'S259\'");
                bool temp779;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp779);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp779);
                this.Manager.Comment("reaching state \'S433\'");
                int temp782 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS94GetClientAccountTypeChecker4)), new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS94GetClientAccountTypeChecker5)));
                if ((temp782 == 0)) {
                    this.Manager.Comment("reaching state \'S750\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp780;
                    this.Manager.Comment("executing step \'call NetrLogonComputeServerDigest(PrimaryDc,RidOfNonMachineAccoun" +
                            "t,MessageOne)\'");
                    temp780 = this.INrpcServerAdapterInstance.NetrLogonComputeServerDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.RidType)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103756");
                    this.Manager.Checkpoint("MS-NRPC_R103743");
                    this.Manager.Comment("reaching state \'S1098\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeServerDigest/ERROR_NO_SUCH_USER\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SUCH_USER, temp780, "return of NetrLogonComputeServerDigest, state S1098");
                    this.Manager.Comment("reaching state \'S1334\'");
                    goto label222;
                }
                if ((temp782 == 1)) {
                    this.Manager.Comment("reaching state \'S751\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp781;
                    this.Manager.Comment("executing step \'call NetrLogonComputeServerDigest(PrimaryDc,RidOfNonMachineAccoun" +
                            "t,MessageOne)\'");
                    temp781 = this.INrpcServerAdapterInstance.NetrLogonComputeServerDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.RidType)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103756");
                    this.Manager.Checkpoint("MS-NRPC_R103743");
                    this.Manager.Comment("reaching state \'S1099\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeServerDigest/ERROR_NO_SUCH_USER\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SUCH_USER, temp781, "return of NetrLogonComputeServerDigest, state S1099");
                    this.Manager.Comment("reaching state \'S1335\'");
                    goto label222;
                }
                throw new InvalidOperationException("never reached");
            label222:
;
                goto label223;
            }
            throw new InvalidOperationException("never reached");
        label223:
;
            this.Manager.EndTest();
        }
        
        private void Test_AuthenticateMessageS94GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S95");
        }
        
        private void Test_AuthenticateMessageS94GetClientAccountTypeChecker(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S431");
        }
        
        private void Test_AuthenticateMessageS94GetClientAccountTypeChecker1(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S431");
        }
        
        private void Test_AuthenticateMessageS94GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S95");
        }
        
        private void Test_AuthenticateMessageS94GetClientAccountTypeChecker2(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S432");
        }
        
        private void Test_AuthenticateMessageS94GetClientAccountTypeChecker3(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S432");
        }
        
        private void Test_AuthenticateMessageS94GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S95");
        }
        
        private void Test_AuthenticateMessageS94GetClientAccountTypeChecker4(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S433");
        }
        
        private void Test_AuthenticateMessageS94GetClientAccountTypeChecker5(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S433");
        }
        #endregion
        
        #region Test Starting in S96
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_AuthenticateMessageS96() {
            this.Manager.BeginTest("Test_AuthenticateMessageS96");
            this.Manager.Comment("reaching state \'S96\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp784;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp784);
            this.Manager.AddReturn(GetPlatformInfo, null, temp784);
            this.Manager.Comment("reaching state \'S97\'");
            int temp797 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_AuthenticateMessageS96GetPlatformChecker)), new ExpectedReturn(Test_AuthenticateMessage.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_AuthenticateMessageS96GetPlatformChecker1)), new ExpectedReturn(Test_AuthenticateMessage.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_AuthenticateMessageS96GetPlatformChecker2)));
            if ((temp797 == 0)) {
                this.Manager.Comment("reaching state \'S260\'");
                bool temp785;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp785);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp785);
                this.Manager.Comment("reaching state \'S434\'");
                int temp788 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS96GetClientAccountTypeChecker)), new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS96GetClientAccountTypeChecker1)));
                if ((temp788 == 0)) {
                    this.Manager.Comment("reaching state \'S752\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp786;
                    this.Manager.Comment("executing step \'call NetrLogonSetServiceBits(PrimaryDc,0,64)\'");
                    temp786 = this.INrpcServerAdapterInstance.NetrLogonSetServiceBits(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 0u, 64u);
                    this.Manager.Comment("reaching state \'S1100\'");
                    this.Manager.Comment("checking step \'return NetrLogonSetServiceBits/ERROR_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_ACCESS_DENIED, temp786, "return of NetrLogonSetServiceBits, state S1100");
                    Test_AuthenticateMessageS1216();
                    goto label224;
                }
                if ((temp788 == 1)) {
                    this.Manager.Comment("reaching state \'S753\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp787;
                    this.Manager.Comment("executing step \'call NetrLogonSetServiceBits(PrimaryDc,512,64)\'");
                    temp787 = this.INrpcServerAdapterInstance.NetrLogonSetServiceBits(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 512u, 64u);
                    this.Manager.Comment("reaching state \'S1101\'");
                    this.Manager.Comment("checking step \'return NetrLogonSetServiceBits/STATUS_INVALID_PARAMETER\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.STATUS_INVALID_PARAMETER, temp787, "return of NetrLogonSetServiceBits, state S1101");
                    Test_AuthenticateMessageS1217();
                    goto label224;
                }
                throw new InvalidOperationException("never reached");
            label224:
;
                goto label227;
            }
            if ((temp797 == 1)) {
                this.Manager.Comment("reaching state \'S261\'");
                bool temp789;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp789);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp789);
                this.Manager.Comment("reaching state \'S435\'");
                int temp792 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS96GetClientAccountTypeChecker2)), new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS96GetClientAccountTypeChecker3)));
                if ((temp792 == 0)) {
                    this.Manager.Comment("reaching state \'S754\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp790;
                    this.Manager.Comment("executing step \'call NetrLogonComputeServerDigest(PrimaryDc,RidOfNonMachineAccoun" +
                            "t,MessageOne)\'");
                    temp790 = this.INrpcServerAdapterInstance.NetrLogonComputeServerDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.RidType)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103756");
                    this.Manager.Checkpoint("MS-NRPC_R103743");
                    this.Manager.Comment("reaching state \'S1102\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeServerDigest/ERROR_NO_SUCH_USER\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SUCH_USER, temp790, "return of NetrLogonComputeServerDigest, state S1102");
                    this.Manager.Comment("reaching state \'S1336\'");
                    goto label225;
                }
                if ((temp792 == 1)) {
                    this.Manager.Comment("reaching state \'S755\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp791;
                    this.Manager.Comment("executing step \'call NetrLogonComputeServerDigest(PrimaryDc,RidOfNonMachineAccoun" +
                            "t,MessageOne)\'");
                    temp791 = this.INrpcServerAdapterInstance.NetrLogonComputeServerDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.RidType)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103756");
                    this.Manager.Checkpoint("MS-NRPC_R103743");
                    this.Manager.Comment("reaching state \'S1103\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeServerDigest/ERROR_NO_SUCH_USER\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SUCH_USER, temp791, "return of NetrLogonComputeServerDigest, state S1103");
                    this.Manager.Comment("reaching state \'S1337\'");
                    goto label225;
                }
                throw new InvalidOperationException("never reached");
            label225:
;
                goto label227;
            }
            if ((temp797 == 2)) {
                this.Manager.Comment("reaching state \'S262\'");
                bool temp793;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp793);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp793);
                this.Manager.Comment("reaching state \'S436\'");
                int temp796 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS96GetClientAccountTypeChecker4)), new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS96GetClientAccountTypeChecker5)));
                if ((temp796 == 0)) {
                    this.Manager.Comment("reaching state \'S756\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp794;
                    this.Manager.Comment("executing step \'call NetrLogonComputeServerDigest(PrimaryDc,RidOfNonMachineAccoun" +
                            "t,MessageOne)\'");
                    temp794 = this.INrpcServerAdapterInstance.NetrLogonComputeServerDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.RidType)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103756");
                    this.Manager.Checkpoint("MS-NRPC_R103743");
                    this.Manager.Comment("reaching state \'S1104\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeServerDigest/ERROR_NO_SUCH_USER\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SUCH_USER, temp794, "return of NetrLogonComputeServerDigest, state S1104");
                    this.Manager.Comment("reaching state \'S1338\'");
                    goto label226;
                }
                if ((temp796 == 1)) {
                    this.Manager.Comment("reaching state \'S757\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp795;
                    this.Manager.Comment("executing step \'call NetrLogonComputeServerDigest(PrimaryDc,RidOfNonMachineAccoun" +
                            "t,MessageOne)\'");
                    temp795 = this.INrpcServerAdapterInstance.NetrLogonComputeServerDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.RidType)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103756");
                    this.Manager.Checkpoint("MS-NRPC_R103743");
                    this.Manager.Comment("reaching state \'S1105\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeServerDigest/ERROR_NO_SUCH_USER\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SUCH_USER, temp795, "return of NetrLogonComputeServerDigest, state S1105");
                    this.Manager.Comment("reaching state \'S1339\'");
                    goto label226;
                }
                throw new InvalidOperationException("never reached");
            label226:
;
                goto label227;
            }
            throw new InvalidOperationException("never reached");
        label227:
;
            this.Manager.EndTest();
        }
        
        private void Test_AuthenticateMessageS96GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S97");
        }
        
        private void Test_AuthenticateMessageS96GetClientAccountTypeChecker(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S434");
        }
        
        private void Test_AuthenticateMessageS96GetClientAccountTypeChecker1(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S434");
        }
        
        private void Test_AuthenticateMessageS96GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S97");
        }
        
        private void Test_AuthenticateMessageS96GetClientAccountTypeChecker2(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S435");
        }
        
        private void Test_AuthenticateMessageS96GetClientAccountTypeChecker3(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S435");
        }
        
        private void Test_AuthenticateMessageS96GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S97");
        }
        
        private void Test_AuthenticateMessageS96GetClientAccountTypeChecker4(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S436");
        }
        
        private void Test_AuthenticateMessageS96GetClientAccountTypeChecker5(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S436");
        }
        #endregion
        
        #region Test Starting in S98
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_AuthenticateMessageS98() {
            this.Manager.BeginTest("Test_AuthenticateMessageS98");
            this.Manager.Comment("reaching state \'S98\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp798;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp798);
            this.Manager.AddReturn(GetPlatformInfo, null, temp798);
            this.Manager.Comment("reaching state \'S99\'");
            int temp811 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_AuthenticateMessageS98GetPlatformChecker)), new ExpectedReturn(Test_AuthenticateMessage.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_AuthenticateMessageS98GetPlatformChecker1)), new ExpectedReturn(Test_AuthenticateMessage.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_AuthenticateMessageS98GetPlatformChecker2)));
            if ((temp811 == 0)) {
                this.Manager.Comment("reaching state \'S263\'");
                bool temp799;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp799);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp799);
                this.Manager.Comment("reaching state \'S437\'");
                int temp802 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS98GetClientAccountTypeChecker)), new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS98GetClientAccountTypeChecker1)));
                if ((temp802 == 0)) {
                    this.Manager.Comment("reaching state \'S758\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp800;
                    this.Manager.Comment("executing step \'call NetrLogonSetServiceBits(PrimaryDc,1,512)\'");
                    temp800 = this.INrpcServerAdapterInstance.NetrLogonSetServiceBits(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 1u, 512u);
                    this.Manager.Comment("reaching state \'S1106\'");
                    this.Manager.Comment("checking step \'return NetrLogonSetServiceBits/ERROR_ACCESS_DENIED\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_ACCESS_DENIED, temp800, "return of NetrLogonSetServiceBits, state S1106");
                    Test_AuthenticateMessageS1216();
                    goto label228;
                }
                if ((temp802 == 1)) {
                    this.Manager.Comment("reaching state \'S759\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp801;
                    this.Manager.Comment("executing step \'call NetrLogonSetServiceBits(PrimaryDc,8192,0)\'");
                    temp801 = this.INrpcServerAdapterInstance.NetrLogonSetServiceBits(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, 8192u, 0u);
                    this.Manager.Comment("reaching state \'S1107\'");
                    this.Manager.Comment("checking step \'return NetrLogonSetServiceBits/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp801, "return of NetrLogonSetServiceBits, state S1107");
                    Test_AuthenticateMessageS1217();
                    goto label228;
                }
                throw new InvalidOperationException("never reached");
            label228:
;
                goto label231;
            }
            if ((temp811 == 1)) {
                this.Manager.Comment("reaching state \'S264\'");
                bool temp803;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp803);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp803);
                this.Manager.Comment("reaching state \'S438\'");
                int temp806 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS98GetClientAccountTypeChecker2)), new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS98GetClientAccountTypeChecker3)));
                if ((temp806 == 0)) {
                    this.Manager.Comment("reaching state \'S760\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp804;
                    this.Manager.Comment("executing step \'call NetrLogonComputeServerDigest(PrimaryDc,RidOfNonMachineAccoun" +
                            "t,MessageOne)\'");
                    temp804 = this.INrpcServerAdapterInstance.NetrLogonComputeServerDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.RidType)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103756");
                    this.Manager.Checkpoint("MS-NRPC_R103743");
                    this.Manager.Comment("reaching state \'S1108\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeServerDigest/ERROR_NO_SUCH_USER\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SUCH_USER, temp804, "return of NetrLogonComputeServerDigest, state S1108");
                    this.Manager.Comment("reaching state \'S1340\'");
                    goto label229;
                }
                if ((temp806 == 1)) {
                    this.Manager.Comment("reaching state \'S761\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp805;
                    this.Manager.Comment("executing step \'call NetrLogonComputeServerDigest(PrimaryDc,RidOfNonMachineAccoun" +
                            "t,MessageOne)\'");
                    temp805 = this.INrpcServerAdapterInstance.NetrLogonComputeServerDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.RidType)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103756");
                    this.Manager.Checkpoint("MS-NRPC_R103743");
                    this.Manager.Comment("reaching state \'S1109\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeServerDigest/ERROR_NO_SUCH_USER\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SUCH_USER, temp805, "return of NetrLogonComputeServerDigest, state S1109");
                    this.Manager.Comment("reaching state \'S1341\'");
                    goto label229;
                }
                throw new InvalidOperationException("never reached");
            label229:
;
                goto label231;
            }
            if ((temp811 == 2)) {
                this.Manager.Comment("reaching state \'S265\'");
                bool temp807;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp807);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp807);
                this.Manager.Comment("reaching state \'S439\'");
                int temp810 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS98GetClientAccountTypeChecker4)), new ExpectedReturn(Test_AuthenticateMessage.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_AuthenticateMessageS98GetClientAccountTypeChecker5)));
                if ((temp810 == 0)) {
                    this.Manager.Comment("reaching state \'S762\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp808;
                    this.Manager.Comment("executing step \'call NetrLogonComputeServerDigest(PrimaryDc,RidOfNonMachineAccoun" +
                            "t,MessageOne)\'");
                    temp808 = this.INrpcServerAdapterInstance.NetrLogonComputeServerDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.RidType)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103756");
                    this.Manager.Checkpoint("MS-NRPC_R103743");
                    this.Manager.Comment("reaching state \'S1110\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeServerDigest/ERROR_NO_SUCH_USER\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SUCH_USER, temp808, "return of NetrLogonComputeServerDigest, state S1110");
                    this.Manager.Comment("reaching state \'S1342\'");
                    goto label230;
                }
                if ((temp810 == 1)) {
                    this.Manager.Comment("reaching state \'S763\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp809;
                    this.Manager.Comment("executing step \'call NetrLogonComputeServerDigest(PrimaryDc,RidOfNonMachineAccoun" +
                            "t,MessageOne)\'");
                    temp809 = this.INrpcServerAdapterInstance.NetrLogonComputeServerDigest(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, ((Microsoft.Protocols.TestSuites.Nrpc.RidType)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.MessageType)(0)));
                    this.Manager.Checkpoint("MS-NRPC_R103756");
                    this.Manager.Checkpoint("MS-NRPC_R103743");
                    this.Manager.Comment("reaching state \'S1111\'");
                    this.Manager.Comment("checking step \'return NetrLogonComputeServerDigest/ERROR_NO_SUCH_USER\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_NO_SUCH_USER, temp809, "return of NetrLogonComputeServerDigest, state S1111");
                    this.Manager.Comment("reaching state \'S1343\'");
                    goto label230;
                }
                throw new InvalidOperationException("never reached");
            label230:
;
                goto label231;
            }
            throw new InvalidOperationException("never reached");
        label231:
;
            this.Manager.EndTest();
        }
        
        private void Test_AuthenticateMessageS98GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S99");
        }
        
        private void Test_AuthenticateMessageS98GetClientAccountTypeChecker(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S437");
        }
        
        private void Test_AuthenticateMessageS98GetClientAccountTypeChecker1(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S437");
        }
        
        private void Test_AuthenticateMessageS98GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S99");
        }
        
        private void Test_AuthenticateMessageS98GetClientAccountTypeChecker2(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S438");
        }
        
        private void Test_AuthenticateMessageS98GetClientAccountTypeChecker3(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S438");
        }
        
        private void Test_AuthenticateMessageS98GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S99");
        }
        
        private void Test_AuthenticateMessageS98GetClientAccountTypeChecker4(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S439");
        }
        
        private void Test_AuthenticateMessageS98GetClientAccountTypeChecker5(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S439");
        }
        #endregion
    }
}
