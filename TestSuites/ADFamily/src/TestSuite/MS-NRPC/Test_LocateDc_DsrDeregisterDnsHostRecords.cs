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
    public partial class Test_LocateDc_DsrDeregisterDnsHostRecords : PtfTestClassBase {
        
        public Test_LocateDc_DsrDeregisterDnsHostRecords() {
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
        public void Test_LocateDc_DsrDeregisterDnsHostRecordsS0() {
            this.Manager.BeginTest("Test_LocateDc_DsrDeregisterDnsHostRecordsS0");
            this.Manager.Comment("reaching state \'S0\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp0;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp0);
            this.Manager.AddReturn(GetPlatformInfo, null, temp0);
            this.Manager.Comment("reaching state \'S1\'");
            int temp13 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_LocateDc_DsrDeregisterDnsHostRecords.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_DsrDeregisterDnsHostRecordsS0GetPlatformChecker)), new ExpectedReturn(Test_LocateDc_DsrDeregisterDnsHostRecords.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_DsrDeregisterDnsHostRecordsS0GetPlatformChecker1)), new ExpectedReturn(Test_LocateDc_DsrDeregisterDnsHostRecords.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_DsrDeregisterDnsHostRecordsS0GetPlatformChecker2)));
            if ((temp13 == 0)) {
                this.Manager.Comment("reaching state \'S70\'");
                bool temp1;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp1);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp1);
                this.Manager.Comment("reaching state \'S151\'");
                int temp4 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_LocateDc_DsrDeregisterDnsHostRecords.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_LocateDc_DsrDeregisterDnsHostRecordsS0GetClientAccountTypeChecker)), new ExpectedReturn(Test_LocateDc_DsrDeregisterDnsHostRecords.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_LocateDc_DsrDeregisterDnsHostRecordsS0GetClientAccountTypeChecker1)));
                if ((temp4 == 0)) {
                    this.Manager.Comment("reaching state \'S232\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp2;
                    this.Manager.Comment("executing step \'call DsrDeregisterDnsHostRecords(PrimaryDc,FqdnFormatDomainName,T" +
                            "rustedDomainGuid,DsaGuidOne,PrimaryDc)\'");
                    temp2 = this.INrpcServerAdapterInstance.DsrDeregisterDnsHostRecords(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.FqdnFormatDomainName, Microsoft.Protocols.TestSuites.Nrpc.DomainGuidType.TrustedDomainGuid, ((Microsoft.Protocols.TestSuites.Nrpc.DsaGuidType)(0)), Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc);
                    this.Manager.Checkpoint("MS-NRPC_R103453");
                    this.Manager.Comment("reaching state \'S371\'");
                    this.Manager.Comment("checking step \'return DsrDeregisterDnsHostRecords/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp2, "return of DsrDeregisterDnsHostRecords, state S371");
                    Test_LocateDc_DsrDeregisterDnsHostRecordsS510();
                    goto label0;
                }
                if ((temp4 == 1)) {
                    this.Manager.Comment("reaching state \'S233\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp3;
                    this.Manager.Comment("executing step \'call DsrDeregisterDnsHostRecords(PrimaryDc,FqdnFormatDomainName,T" +
                            "rustedDomainGuid,DsaGuidOne,PrimaryDc)\'");
                    temp3 = this.INrpcServerAdapterInstance.DsrDeregisterDnsHostRecords(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.FqdnFormatDomainName, Microsoft.Protocols.TestSuites.Nrpc.DomainGuidType.TrustedDomainGuid, ((Microsoft.Protocols.TestSuites.Nrpc.DsaGuidType)(0)), Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc);
                    this.Manager.Checkpoint("MS-NRPC_R103453");
                    this.Manager.Comment("reaching state \'S372\'");
                    this.Manager.Comment("checking step \'return DsrDeregisterDnsHostRecords/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp3, "return of DsrDeregisterDnsHostRecords, state S372");
                    Test_LocateDc_DsrDeregisterDnsHostRecordsS511();
                    goto label0;
                }
                throw new InvalidOperationException("never reached");
            label0:
;
                goto label3;
            }
            if ((temp13 == 1)) {
                this.Manager.Comment("reaching state \'S71\'");
                bool temp5;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp5);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp5);
                this.Manager.Comment("reaching state \'S152\'");
                int temp8 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_LocateDc_DsrDeregisterDnsHostRecords.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_LocateDc_DsrDeregisterDnsHostRecordsS0GetClientAccountTypeChecker2)), new ExpectedReturn(Test_LocateDc_DsrDeregisterDnsHostRecords.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_LocateDc_DsrDeregisterDnsHostRecordsS0GetClientAccountTypeChecker3)));
                if ((temp8 == 0)) {
                    this.Manager.Comment("reaching state \'S234\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp6;
                    this.Manager.Comment("executing step \'call DsrDeregisterDnsHostRecords(PrimaryDc,FqdnFormatDomainName,T" +
                            "rustedDomainGuid,DsaGuidOne,PrimaryDc)\'");
                    temp6 = this.INrpcServerAdapterInstance.DsrDeregisterDnsHostRecords(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.FqdnFormatDomainName, Microsoft.Protocols.TestSuites.Nrpc.DomainGuidType.TrustedDomainGuid, ((Microsoft.Protocols.TestSuites.Nrpc.DsaGuidType)(0)), Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc);
                    this.Manager.Checkpoint("MS-NRPC_R103453");
                    this.Manager.Comment("reaching state \'S373\'");
                    this.Manager.Comment("checking step \'return DsrDeregisterDnsHostRecords/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp6, "return of DsrDeregisterDnsHostRecords, state S373");
                    Test_LocateDc_DsrDeregisterDnsHostRecordsS512();
                    goto label1;
                }
                if ((temp8 == 1)) {
                    this.Manager.Comment("reaching state \'S235\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp7;
                    this.Manager.Comment("executing step \'call DsrDeregisterDnsHostRecords(PrimaryDc,FqdnFormatDomainName,T" +
                            "rustedDomainGuid,DsaGuidOne,PrimaryDc)\'");
                    temp7 = this.INrpcServerAdapterInstance.DsrDeregisterDnsHostRecords(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.FqdnFormatDomainName, Microsoft.Protocols.TestSuites.Nrpc.DomainGuidType.TrustedDomainGuid, ((Microsoft.Protocols.TestSuites.Nrpc.DsaGuidType)(0)), Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc);
                    this.Manager.Checkpoint("MS-NRPC_R103453");
                    this.Manager.Comment("reaching state \'S374\'");
                    this.Manager.Comment("checking step \'return DsrDeregisterDnsHostRecords/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp7, "return of DsrDeregisterDnsHostRecords, state S374");
                    Test_LocateDc_DsrDeregisterDnsHostRecordsS513();
                    goto label1;
                }
                throw new InvalidOperationException("never reached");
            label1:
;
                goto label3;
            }
            if ((temp13 == 2)) {
                this.Manager.Comment("reaching state \'S72\'");
                bool temp9;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp9);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp9);
                this.Manager.Comment("reaching state \'S153\'");
                int temp12 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_LocateDc_DsrDeregisterDnsHostRecords.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_LocateDc_DsrDeregisterDnsHostRecordsS0GetClientAccountTypeChecker4)), new ExpectedReturn(Test_LocateDc_DsrDeregisterDnsHostRecords.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_LocateDc_DsrDeregisterDnsHostRecordsS0GetClientAccountTypeChecker5)));
                if ((temp12 == 0)) {
                    this.Manager.Comment("reaching state \'S236\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp10;
                    this.Manager.Comment("executing step \'call DsrDeregisterDnsHostRecords(PrimaryDc,FqdnFormatDomainName,T" +
                            "rustedDomainGuid,DsaGuidOne,PrimaryDc)\'");
                    temp10 = this.INrpcServerAdapterInstance.DsrDeregisterDnsHostRecords(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.FqdnFormatDomainName, Microsoft.Protocols.TestSuites.Nrpc.DomainGuidType.TrustedDomainGuid, ((Microsoft.Protocols.TestSuites.Nrpc.DsaGuidType)(0)), Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc);
                    this.Manager.Checkpoint("MS-NRPC_R103453");
                    this.Manager.Comment("reaching state \'S375\'");
                    this.Manager.Comment("checking step \'return DsrDeregisterDnsHostRecords/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp10, "return of DsrDeregisterDnsHostRecords, state S375");
                    Test_LocateDc_DsrDeregisterDnsHostRecordsS514();
                    goto label2;
                }
                if ((temp12 == 1)) {
                    this.Manager.Comment("reaching state \'S237\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp11;
                    this.Manager.Comment("executing step \'call DsrDeregisterDnsHostRecords(PrimaryDc,FqdnFormatDomainName,T" +
                            "rustedDomainGuid,DsaGuidOne,PrimaryDc)\'");
                    temp11 = this.INrpcServerAdapterInstance.DsrDeregisterDnsHostRecords(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.FqdnFormatDomainName, Microsoft.Protocols.TestSuites.Nrpc.DomainGuidType.TrustedDomainGuid, ((Microsoft.Protocols.TestSuites.Nrpc.DsaGuidType)(0)), Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc);
                    this.Manager.Checkpoint("MS-NRPC_R103453");
                    this.Manager.Comment("reaching state \'S376\'");
                    this.Manager.Comment("checking step \'return DsrDeregisterDnsHostRecords/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp11, "return of DsrDeregisterDnsHostRecords, state S376");
                    Test_LocateDc_DsrDeregisterDnsHostRecordsS515();
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
        
        private void Test_LocateDc_DsrDeregisterDnsHostRecordsS0GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S1");
        }
        
        private void Test_LocateDc_DsrDeregisterDnsHostRecordsS0GetClientAccountTypeChecker(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S151");
        }
        
        private void Test_LocateDc_DsrDeregisterDnsHostRecordsS510() {
            this.Manager.Comment("reaching state \'S510\'");
        }
        
        private void Test_LocateDc_DsrDeregisterDnsHostRecordsS0GetClientAccountTypeChecker1(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S151");
        }
        
        private void Test_LocateDc_DsrDeregisterDnsHostRecordsS511() {
            this.Manager.Comment("reaching state \'S511\'");
        }
        
        private void Test_LocateDc_DsrDeregisterDnsHostRecordsS0GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S1");
        }
        
        private void Test_LocateDc_DsrDeregisterDnsHostRecordsS0GetClientAccountTypeChecker2(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S152");
        }
        
        private void Test_LocateDc_DsrDeregisterDnsHostRecordsS512() {
            this.Manager.Comment("reaching state \'S512\'");
        }
        
        private void Test_LocateDc_DsrDeregisterDnsHostRecordsS0GetClientAccountTypeChecker3(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S152");
        }
        
        private void Test_LocateDc_DsrDeregisterDnsHostRecordsS513() {
            this.Manager.Comment("reaching state \'S513\'");
        }
        
        private void Test_LocateDc_DsrDeregisterDnsHostRecordsS0GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S1");
        }
        
        private void Test_LocateDc_DsrDeregisterDnsHostRecordsS0GetClientAccountTypeChecker4(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S153");
        }
        
        private void Test_LocateDc_DsrDeregisterDnsHostRecordsS514() {
            this.Manager.Comment("reaching state \'S514\'");
        }
        
        private void Test_LocateDc_DsrDeregisterDnsHostRecordsS0GetClientAccountTypeChecker5(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S153");
        }
        
        private void Test_LocateDc_DsrDeregisterDnsHostRecordsS515() {
            this.Manager.Comment("reaching state \'S515\'");
        }
        #endregion
        
        #region Test Starting in S10
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_LocateDc_DsrDeregisterDnsHostRecordsS10() {
            this.Manager.BeginTest("Test_LocateDc_DsrDeregisterDnsHostRecordsS10");
            this.Manager.Comment("reaching state \'S10\'");
            this.Manager.Comment("executing step \'call SwitchUserAccount(False)\'");
            this.INrpcServerAdapterInstance.SwitchUserAccount(false);
            this.Manager.Comment("reaching state \'S11\'");
            this.Manager.Comment("checking step \'return SwitchUserAccount\'");
            this.Manager.Comment("reaching state \'S77\'");
            Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp14;
            this.Manager.Comment("executing step \'call DsrDeregisterDnsHostRecords(PrimaryDc,NetBiosFormatDomainNam" +
                    "e,Null,DsaGuidOne,PrimaryDc)\'");
            temp14 = this.INrpcServerAdapterInstance.DsrDeregisterDnsHostRecords(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.NetBiosFormatDomainName, ((Microsoft.Protocols.TestSuites.Nrpc.DomainGuidType)(0)), ((Microsoft.Protocols.TestSuites.Nrpc.DsaGuidType)(0)), Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc);
            this.Manager.Checkpoint("MS-NRPC_R103328");
            this.Manager.Checkpoint("MS-NRPC_R103454");
            this.Manager.Comment("reaching state \'S158\'");
            this.Manager.Comment("checking step \'return DsrDeregisterDnsHostRecords/ERROR_ACCESS_DENIED\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_ACCESS_DENIED, temp14, "return of DsrDeregisterDnsHostRecords, state S158");
            Test_LocateDc_DsrDeregisterDnsHostRecordsS238();
            this.Manager.EndTest();
        }
        
        private void Test_LocateDc_DsrDeregisterDnsHostRecordsS238() {
            this.Manager.Comment("reaching state \'S238\'");
            this.Manager.Comment("executing step \'call SwitchUserAccount(True)\'");
            this.INrpcServerAdapterInstance.SwitchUserAccount(true);
            this.Manager.Comment("reaching state \'S377\'");
            this.Manager.Comment("checking step \'return SwitchUserAccount\'");
            this.Manager.Comment("reaching state \'S516\'");
            this.Manager.Comment("executing step \'call RestartNetlogonService()\'");
            this.INrpcServerSutControlAdapterInstance.RestartNetlogonService();
            this.Manager.Comment("reaching state \'S583\'");
            this.Manager.Comment("checking step \'return RestartNetlogonService\'");
            this.Manager.Comment("reaching state \'S584\'");
        }
        #endregion
        
        #region Test Starting in S12
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_LocateDc_DsrDeregisterDnsHostRecordsS12() {
            this.Manager.BeginTest("Test_LocateDc_DsrDeregisterDnsHostRecordsS12");
            this.Manager.Comment("reaching state \'S12\'");
            this.Manager.Comment("executing step \'call SwitchUserAccount(False)\'");
            this.INrpcServerAdapterInstance.SwitchUserAccount(false);
            this.Manager.Comment("reaching state \'S13\'");
            this.Manager.Comment("checking step \'return SwitchUserAccount\'");
            this.Manager.Comment("reaching state \'S78\'");
            Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp15;
            this.Manager.Comment("executing step \'call DsrDeregisterDnsHostRecords(PrimaryDc,NetBiosFormatDomainNam" +
                    "e,InvalidDomainGuid,DsaGuidOne,PrimaryDc)\'");
            temp15 = this.INrpcServerAdapterInstance.DsrDeregisterDnsHostRecords(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.NetBiosFormatDomainName, ((Microsoft.Protocols.TestSuites.Nrpc.DomainGuidType)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.DsaGuidType)(0)), Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc);
            this.Manager.Checkpoint("MS-NRPC_R103328");
            this.Manager.Checkpoint("MS-NRPC_R103454");
            this.Manager.Comment("reaching state \'S159\'");
            this.Manager.Comment("checking step \'return DsrDeregisterDnsHostRecords/ERROR_ACCESS_DENIED\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_ACCESS_DENIED, temp15, "return of DsrDeregisterDnsHostRecords, state S159");
            Test_LocateDc_DsrDeregisterDnsHostRecordsS238();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S14
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_LocateDc_DsrDeregisterDnsHostRecordsS14() {
            this.Manager.BeginTest("Test_LocateDc_DsrDeregisterDnsHostRecordsS14");
            this.Manager.Comment("reaching state \'S14\'");
            this.Manager.Comment("executing step \'call SwitchUserAccount(False)\'");
            this.INrpcServerAdapterInstance.SwitchUserAccount(false);
            this.Manager.Comment("reaching state \'S15\'");
            this.Manager.Comment("checking step \'return SwitchUserAccount\'");
            this.Manager.Comment("reaching state \'S79\'");
            Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp16;
            this.Manager.Comment("executing step \'call DsrDeregisterDnsHostRecords(PrimaryDc,NetBiosFormatDomainNam" +
                    "e,PrimaryDomainGuid,DsaGuidOne,PrimaryDc)\'");
            temp16 = this.INrpcServerAdapterInstance.DsrDeregisterDnsHostRecords(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.NetBiosFormatDomainName, Microsoft.Protocols.TestSuites.Nrpc.DomainGuidType.PrimaryDomainGuid, ((Microsoft.Protocols.TestSuites.Nrpc.DsaGuidType)(0)), Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc);
            this.Manager.Checkpoint("MS-NRPC_R103328");
            this.Manager.Checkpoint("MS-NRPC_R103454");
            this.Manager.Comment("reaching state \'S160\'");
            this.Manager.Comment("checking step \'return DsrDeregisterDnsHostRecords/ERROR_ACCESS_DENIED\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_ACCESS_DENIED, temp16, "return of DsrDeregisterDnsHostRecords, state S160");
            Test_LocateDc_DsrDeregisterDnsHostRecordsS238();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S16
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_LocateDc_DsrDeregisterDnsHostRecordsS16() {
            this.Manager.BeginTest("Test_LocateDc_DsrDeregisterDnsHostRecordsS16");
            this.Manager.Comment("reaching state \'S16\'");
            this.Manager.Comment("executing step \'call SwitchUserAccount(False)\'");
            this.INrpcServerAdapterInstance.SwitchUserAccount(false);
            this.Manager.Comment("reaching state \'S17\'");
            this.Manager.Comment("checking step \'return SwitchUserAccount\'");
            this.Manager.Comment("reaching state \'S80\'");
            Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp17;
            this.Manager.Comment("executing step \'call DsrDeregisterDnsHostRecords(PrimaryDc,NetBiosFormatDomainNam" +
                    "e,TrustedDomainGuid,DsaGuidOne,PrimaryDc)\'");
            temp17 = this.INrpcServerAdapterInstance.DsrDeregisterDnsHostRecords(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.NetBiosFormatDomainName, Microsoft.Protocols.TestSuites.Nrpc.DomainGuidType.TrustedDomainGuid, ((Microsoft.Protocols.TestSuites.Nrpc.DsaGuidType)(0)), Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc);
            this.Manager.Checkpoint("MS-NRPC_R103328");
            this.Manager.Checkpoint("MS-NRPC_R103454");
            this.Manager.Comment("reaching state \'S161\'");
            this.Manager.Comment("checking step \'return DsrDeregisterDnsHostRecords/ERROR_ACCESS_DENIED\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_ACCESS_DENIED, temp17, "return of DsrDeregisterDnsHostRecords, state S161");
            Test_LocateDc_DsrDeregisterDnsHostRecordsS238();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S18
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_LocateDc_DsrDeregisterDnsHostRecordsS18() {
            this.Manager.BeginTest("Test_LocateDc_DsrDeregisterDnsHostRecordsS18");
            this.Manager.Comment("reaching state \'S18\'");
            this.Manager.Comment("executing step \'call SwitchUserAccount(False)\'");
            this.INrpcServerAdapterInstance.SwitchUserAccount(false);
            this.Manager.Comment("reaching state \'S19\'");
            this.Manager.Comment("checking step \'return SwitchUserAccount\'");
            this.Manager.Comment("reaching state \'S81\'");
            Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp18;
            this.Manager.Comment("executing step \'call DsrDeregisterDnsHostRecords(PrimaryDc,FqdnFormatDomainName,N" +
                    "ull,DsaGuidOne,PrimaryDc)\'");
            temp18 = this.INrpcServerAdapterInstance.DsrDeregisterDnsHostRecords(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.FqdnFormatDomainName, ((Microsoft.Protocols.TestSuites.Nrpc.DomainGuidType)(0)), ((Microsoft.Protocols.TestSuites.Nrpc.DsaGuidType)(0)), Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc);
            this.Manager.Checkpoint("MS-NRPC_R103328");
            this.Manager.Checkpoint("MS-NRPC_R103454");
            this.Manager.Comment("reaching state \'S162\'");
            this.Manager.Comment("checking step \'return DsrDeregisterDnsHostRecords/ERROR_ACCESS_DENIED\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_ACCESS_DENIED, temp18, "return of DsrDeregisterDnsHostRecords, state S162");
            Test_LocateDc_DsrDeregisterDnsHostRecordsS238();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S2
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_LocateDc_DsrDeregisterDnsHostRecordsS2() {
            this.Manager.BeginTest("Test_LocateDc_DsrDeregisterDnsHostRecordsS2");
            this.Manager.Comment("reaching state \'S2\'");
            this.Manager.Comment("executing step \'call SwitchUserAccount(False)\'");
            this.INrpcServerAdapterInstance.SwitchUserAccount(false);
            this.Manager.Comment("reaching state \'S3\'");
            this.Manager.Comment("checking step \'return SwitchUserAccount\'");
            this.Manager.Comment("reaching state \'S73\'");
            Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp19;
            this.Manager.Comment("executing step \'call DsrDeregisterDnsHostRecords(PrimaryDc,InvalidDomainName,Null" +
                    ",DsaGuidOne,PrimaryDc)\'");
            temp19 = this.INrpcServerAdapterInstance.DsrDeregisterDnsHostRecords(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.InvalidDomainName, ((Microsoft.Protocols.TestSuites.Nrpc.DomainGuidType)(0)), ((Microsoft.Protocols.TestSuites.Nrpc.DsaGuidType)(0)), Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc);
            this.Manager.Checkpoint("MS-NRPC_R103328");
            this.Manager.Checkpoint("MS-NRPC_R103454");
            this.Manager.Comment("reaching state \'S154\'");
            this.Manager.Comment("checking step \'return DsrDeregisterDnsHostRecords/ERROR_ACCESS_DENIED\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_ACCESS_DENIED, temp19, "return of DsrDeregisterDnsHostRecords, state S154");
            Test_LocateDc_DsrDeregisterDnsHostRecordsS238();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S20
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_LocateDc_DsrDeregisterDnsHostRecordsS20() {
            this.Manager.BeginTest("Test_LocateDc_DsrDeregisterDnsHostRecordsS20");
            this.Manager.Comment("reaching state \'S20\'");
            this.Manager.Comment("executing step \'call SwitchUserAccount(False)\'");
            this.INrpcServerAdapterInstance.SwitchUserAccount(false);
            this.Manager.Comment("reaching state \'S21\'");
            this.Manager.Comment("checking step \'return SwitchUserAccount\'");
            this.Manager.Comment("reaching state \'S82\'");
            Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp20;
            this.Manager.Comment("executing step \'call DsrDeregisterDnsHostRecords(PrimaryDc,FqdnFormatDomainName,I" +
                    "nvalidDomainGuid,DsaGuidOne,PrimaryDc)\'");
            temp20 = this.INrpcServerAdapterInstance.DsrDeregisterDnsHostRecords(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.FqdnFormatDomainName, ((Microsoft.Protocols.TestSuites.Nrpc.DomainGuidType)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.DsaGuidType)(0)), Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc);
            this.Manager.Checkpoint("MS-NRPC_R103328");
            this.Manager.Checkpoint("MS-NRPC_R103454");
            this.Manager.Comment("reaching state \'S163\'");
            this.Manager.Comment("checking step \'return DsrDeregisterDnsHostRecords/ERROR_ACCESS_DENIED\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_ACCESS_DENIED, temp20, "return of DsrDeregisterDnsHostRecords, state S163");
            Test_LocateDc_DsrDeregisterDnsHostRecordsS238();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S22
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_LocateDc_DsrDeregisterDnsHostRecordsS22() {
            this.Manager.BeginTest("Test_LocateDc_DsrDeregisterDnsHostRecordsS22");
            this.Manager.Comment("reaching state \'S22\'");
            this.Manager.Comment("executing step \'call SwitchUserAccount(False)\'");
            this.INrpcServerAdapterInstance.SwitchUserAccount(false);
            this.Manager.Comment("reaching state \'S23\'");
            this.Manager.Comment("checking step \'return SwitchUserAccount\'");
            this.Manager.Comment("reaching state \'S83\'");
            Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp21;
            this.Manager.Comment("executing step \'call DsrDeregisterDnsHostRecords(PrimaryDc,FqdnFormatDomainName,P" +
                    "rimaryDomainGuid,DsaGuidOne,PrimaryDc)\'");
            temp21 = this.INrpcServerAdapterInstance.DsrDeregisterDnsHostRecords(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.FqdnFormatDomainName, Microsoft.Protocols.TestSuites.Nrpc.DomainGuidType.PrimaryDomainGuid, ((Microsoft.Protocols.TestSuites.Nrpc.DsaGuidType)(0)), Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc);
            this.Manager.Checkpoint("MS-NRPC_R103328");
            this.Manager.Checkpoint("MS-NRPC_R103454");
            this.Manager.Comment("reaching state \'S164\'");
            this.Manager.Comment("checking step \'return DsrDeregisterDnsHostRecords/ERROR_ACCESS_DENIED\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_ACCESS_DENIED, temp21, "return of DsrDeregisterDnsHostRecords, state S164");
            Test_LocateDc_DsrDeregisterDnsHostRecordsS238();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S24
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_LocateDc_DsrDeregisterDnsHostRecordsS24() {
            this.Manager.BeginTest("Test_LocateDc_DsrDeregisterDnsHostRecordsS24");
            this.Manager.Comment("reaching state \'S24\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp22;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp22);
            this.Manager.AddReturn(GetPlatformInfo, null, temp22);
            this.Manager.Comment("reaching state \'S25\'");
            int temp35 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_LocateDc_DsrDeregisterDnsHostRecords.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_DsrDeregisterDnsHostRecordsS24GetPlatformChecker)), new ExpectedReturn(Test_LocateDc_DsrDeregisterDnsHostRecords.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_DsrDeregisterDnsHostRecordsS24GetPlatformChecker1)), new ExpectedReturn(Test_LocateDc_DsrDeregisterDnsHostRecords.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_DsrDeregisterDnsHostRecordsS24GetPlatformChecker2)));
            if ((temp35 == 0)) {
                this.Manager.Comment("reaching state \'S84\'");
                bool temp23;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp23);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp23);
                this.Manager.Comment("reaching state \'S165\'");
                int temp26 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_LocateDc_DsrDeregisterDnsHostRecords.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_LocateDc_DsrDeregisterDnsHostRecordsS24GetClientAccountTypeChecker)), new ExpectedReturn(Test_LocateDc_DsrDeregisterDnsHostRecords.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_LocateDc_DsrDeregisterDnsHostRecordsS24GetClientAccountTypeChecker1)));
                if ((temp26 == 0)) {
                    this.Manager.Comment("reaching state \'S239\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp24;
                    this.Manager.Comment("executing step \'call DsrDeregisterDnsHostRecords(PrimaryDc,InvalidDomainName,Null" +
                            ",DsaGuidOne,PrimaryDc)\'");
                    temp24 = this.INrpcServerAdapterInstance.DsrDeregisterDnsHostRecords(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.InvalidDomainName, ((Microsoft.Protocols.TestSuites.Nrpc.DomainGuidType)(0)), ((Microsoft.Protocols.TestSuites.Nrpc.DsaGuidType)(0)), Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc);
                    this.Manager.Checkpoint("MS-NRPC_R103453");
                    this.Manager.Comment("reaching state \'S378\'");
                    this.Manager.Comment("checking step \'return DsrDeregisterDnsHostRecords/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp24, "return of DsrDeregisterDnsHostRecords, state S378");
                    Test_LocateDc_DsrDeregisterDnsHostRecordsS515();
                    goto label4;
                }
                if ((temp26 == 1)) {
                    this.Manager.Comment("reaching state \'S240\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp25;
                    this.Manager.Comment("executing step \'call DsrDeregisterDnsHostRecords(PrimaryDc,InvalidDomainName,Null" +
                            ",DsaGuidOne,PrimaryDc)\'");
                    temp25 = this.INrpcServerAdapterInstance.DsrDeregisterDnsHostRecords(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.InvalidDomainName, ((Microsoft.Protocols.TestSuites.Nrpc.DomainGuidType)(0)), ((Microsoft.Protocols.TestSuites.Nrpc.DsaGuidType)(0)), Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc);
                    this.Manager.Checkpoint("MS-NRPC_R103453");
                    this.Manager.Comment("reaching state \'S379\'");
                    this.Manager.Comment("checking step \'return DsrDeregisterDnsHostRecords/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp25, "return of DsrDeregisterDnsHostRecords, state S379");
                    Test_LocateDc_DsrDeregisterDnsHostRecordsS514();
                    goto label4;
                }
                throw new InvalidOperationException("never reached");
            label4:
;
                goto label7;
            }
            if ((temp35 == 1)) {
                this.Manager.Comment("reaching state \'S85\'");
                bool temp27;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp27);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp27);
                this.Manager.Comment("reaching state \'S166\'");
                int temp30 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_LocateDc_DsrDeregisterDnsHostRecords.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_LocateDc_DsrDeregisterDnsHostRecordsS24GetClientAccountTypeChecker2)), new ExpectedReturn(Test_LocateDc_DsrDeregisterDnsHostRecords.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_LocateDc_DsrDeregisterDnsHostRecordsS24GetClientAccountTypeChecker3)));
                if ((temp30 == 0)) {
                    this.Manager.Comment("reaching state \'S241\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp28;
                    this.Manager.Comment("executing step \'call DsrDeregisterDnsHostRecords(PrimaryDc,InvalidDomainName,Null" +
                            ",DsaGuidOne,PrimaryDc)\'");
                    temp28 = this.INrpcServerAdapterInstance.DsrDeregisterDnsHostRecords(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.InvalidDomainName, ((Microsoft.Protocols.TestSuites.Nrpc.DomainGuidType)(0)), ((Microsoft.Protocols.TestSuites.Nrpc.DsaGuidType)(0)), Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc);
                    this.Manager.Checkpoint("MS-NRPC_R103453");
                    this.Manager.Comment("reaching state \'S380\'");
                    this.Manager.Comment("checking step \'return DsrDeregisterDnsHostRecords/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp28, "return of DsrDeregisterDnsHostRecords, state S380");
                    Test_LocateDc_DsrDeregisterDnsHostRecordsS513();
                    goto label5;
                }
                if ((temp30 == 1)) {
                    this.Manager.Comment("reaching state \'S242\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp29;
                    this.Manager.Comment("executing step \'call DsrDeregisterDnsHostRecords(PrimaryDc,InvalidDomainName,Null" +
                            ",DsaGuidOne,PrimaryDc)\'");
                    temp29 = this.INrpcServerAdapterInstance.DsrDeregisterDnsHostRecords(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.InvalidDomainName, ((Microsoft.Protocols.TestSuites.Nrpc.DomainGuidType)(0)), ((Microsoft.Protocols.TestSuites.Nrpc.DsaGuidType)(0)), Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc);
                    this.Manager.Checkpoint("MS-NRPC_R103453");
                    this.Manager.Comment("reaching state \'S381\'");
                    this.Manager.Comment("checking step \'return DsrDeregisterDnsHostRecords/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp29, "return of DsrDeregisterDnsHostRecords, state S381");
                    Test_LocateDc_DsrDeregisterDnsHostRecordsS512();
                    goto label5;
                }
                throw new InvalidOperationException("never reached");
            label5:
;
                goto label7;
            }
            if ((temp35 == 2)) {
                this.Manager.Comment("reaching state \'S86\'");
                bool temp31;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp31);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp31);
                this.Manager.Comment("reaching state \'S167\'");
                int temp34 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_LocateDc_DsrDeregisterDnsHostRecords.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_LocateDc_DsrDeregisterDnsHostRecordsS24GetClientAccountTypeChecker4)), new ExpectedReturn(Test_LocateDc_DsrDeregisterDnsHostRecords.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_LocateDc_DsrDeregisterDnsHostRecordsS24GetClientAccountTypeChecker5)));
                if ((temp34 == 0)) {
                    this.Manager.Comment("reaching state \'S243\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp32;
                    this.Manager.Comment("executing step \'call DsrDeregisterDnsHostRecords(PrimaryDc,InvalidDomainName,Null" +
                            ",DsaGuidOne,PrimaryDc)\'");
                    temp32 = this.INrpcServerAdapterInstance.DsrDeregisterDnsHostRecords(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.InvalidDomainName, ((Microsoft.Protocols.TestSuites.Nrpc.DomainGuidType)(0)), ((Microsoft.Protocols.TestSuites.Nrpc.DsaGuidType)(0)), Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc);
                    this.Manager.Checkpoint("MS-NRPC_R103453");
                    this.Manager.Comment("reaching state \'S382\'");
                    this.Manager.Comment("checking step \'return DsrDeregisterDnsHostRecords/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp32, "return of DsrDeregisterDnsHostRecords, state S382");
                    this.Manager.Comment("reaching state \'S517\'");
                    goto label6;
                }
                if ((temp34 == 1)) {
                    this.Manager.Comment("reaching state \'S244\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp33;
                    this.Manager.Comment("executing step \'call DsrDeregisterDnsHostRecords(PrimaryDc,InvalidDomainName,Null" +
                            ",DsaGuidOne,PrimaryDc)\'");
                    temp33 = this.INrpcServerAdapterInstance.DsrDeregisterDnsHostRecords(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.InvalidDomainName, ((Microsoft.Protocols.TestSuites.Nrpc.DomainGuidType)(0)), ((Microsoft.Protocols.TestSuites.Nrpc.DsaGuidType)(0)), Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc);
                    this.Manager.Checkpoint("MS-NRPC_R103453");
                    this.Manager.Comment("reaching state \'S383\'");
                    this.Manager.Comment("checking step \'return DsrDeregisterDnsHostRecords/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp33, "return of DsrDeregisterDnsHostRecords, state S383");
                    this.Manager.Comment("reaching state \'S518\'");
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
        
        private void Test_LocateDc_DsrDeregisterDnsHostRecordsS24GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S25");
        }
        
        private void Test_LocateDc_DsrDeregisterDnsHostRecordsS24GetClientAccountTypeChecker(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S165");
        }
        
        private void Test_LocateDc_DsrDeregisterDnsHostRecordsS24GetClientAccountTypeChecker1(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S165");
        }
        
        private void Test_LocateDc_DsrDeregisterDnsHostRecordsS24GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S25");
        }
        
        private void Test_LocateDc_DsrDeregisterDnsHostRecordsS24GetClientAccountTypeChecker2(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S166");
        }
        
        private void Test_LocateDc_DsrDeregisterDnsHostRecordsS24GetClientAccountTypeChecker3(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S166");
        }
        
        private void Test_LocateDc_DsrDeregisterDnsHostRecordsS24GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S25");
        }
        
        private void Test_LocateDc_DsrDeregisterDnsHostRecordsS24GetClientAccountTypeChecker4(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S167");
        }
        
        private void Test_LocateDc_DsrDeregisterDnsHostRecordsS24GetClientAccountTypeChecker5(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S167");
        }
        #endregion
        
        #region Test Starting in S26
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_LocateDc_DsrDeregisterDnsHostRecordsS26() {
            this.Manager.BeginTest("Test_LocateDc_DsrDeregisterDnsHostRecordsS26");
            this.Manager.Comment("reaching state \'S26\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp36;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp36);
            this.Manager.AddReturn(GetPlatformInfo, null, temp36);
            this.Manager.Comment("reaching state \'S27\'");
            int temp49 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_LocateDc_DsrDeregisterDnsHostRecords.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_DsrDeregisterDnsHostRecordsS26GetPlatformChecker)), new ExpectedReturn(Test_LocateDc_DsrDeregisterDnsHostRecords.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_DsrDeregisterDnsHostRecordsS26GetPlatformChecker1)), new ExpectedReturn(Test_LocateDc_DsrDeregisterDnsHostRecords.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_DsrDeregisterDnsHostRecordsS26GetPlatformChecker2)));
            if ((temp49 == 0)) {
                this.Manager.Comment("reaching state \'S87\'");
                bool temp37;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp37);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp37);
                this.Manager.Comment("reaching state \'S168\'");
                int temp40 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_LocateDc_DsrDeregisterDnsHostRecords.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_LocateDc_DsrDeregisterDnsHostRecordsS26GetClientAccountTypeChecker)), new ExpectedReturn(Test_LocateDc_DsrDeregisterDnsHostRecords.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_LocateDc_DsrDeregisterDnsHostRecordsS26GetClientAccountTypeChecker1)));
                if ((temp40 == 0)) {
                    this.Manager.Comment("reaching state \'S245\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp38;
                    this.Manager.Comment("executing step \'call DsrDeregisterDnsHostRecords(PrimaryDc,InvalidDomainName,Inva" +
                            "lidDomainGuid,DsaGuidOne,PrimaryDc)\'");
                    temp38 = this.INrpcServerAdapterInstance.DsrDeregisterDnsHostRecords(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.InvalidDomainName, ((Microsoft.Protocols.TestSuites.Nrpc.DomainGuidType)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.DsaGuidType)(0)), Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc);
                    this.Manager.Checkpoint("MS-NRPC_R103453");
                    this.Manager.Comment("reaching state \'S384\'");
                    this.Manager.Comment("checking step \'return DsrDeregisterDnsHostRecords/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp38, "return of DsrDeregisterDnsHostRecords, state S384");
                    Test_LocateDc_DsrDeregisterDnsHostRecordsS515();
                    goto label8;
                }
                if ((temp40 == 1)) {
                    this.Manager.Comment("reaching state \'S246\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp39;
                    this.Manager.Comment("executing step \'call DsrDeregisterDnsHostRecords(PrimaryDc,InvalidDomainName,Inva" +
                            "lidDomainGuid,DsaGuidOne,PrimaryDc)\'");
                    temp39 = this.INrpcServerAdapterInstance.DsrDeregisterDnsHostRecords(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.InvalidDomainName, ((Microsoft.Protocols.TestSuites.Nrpc.DomainGuidType)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.DsaGuidType)(0)), Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc);
                    this.Manager.Checkpoint("MS-NRPC_R103453");
                    this.Manager.Comment("reaching state \'S385\'");
                    this.Manager.Comment("checking step \'return DsrDeregisterDnsHostRecords/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp39, "return of DsrDeregisterDnsHostRecords, state S385");
                    Test_LocateDc_DsrDeregisterDnsHostRecordsS514();
                    goto label8;
                }
                throw new InvalidOperationException("never reached");
            label8:
;
                goto label11;
            }
            if ((temp49 == 1)) {
                this.Manager.Comment("reaching state \'S88\'");
                bool temp41;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp41);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp41);
                this.Manager.Comment("reaching state \'S169\'");
                int temp44 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_LocateDc_DsrDeregisterDnsHostRecords.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_LocateDc_DsrDeregisterDnsHostRecordsS26GetClientAccountTypeChecker2)), new ExpectedReturn(Test_LocateDc_DsrDeregisterDnsHostRecords.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_LocateDc_DsrDeregisterDnsHostRecordsS26GetClientAccountTypeChecker3)));
                if ((temp44 == 0)) {
                    this.Manager.Comment("reaching state \'S247\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp42;
                    this.Manager.Comment("executing step \'call DsrDeregisterDnsHostRecords(PrimaryDc,InvalidDomainName,Inva" +
                            "lidDomainGuid,DsaGuidOne,PrimaryDc)\'");
                    temp42 = this.INrpcServerAdapterInstance.DsrDeregisterDnsHostRecords(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.InvalidDomainName, ((Microsoft.Protocols.TestSuites.Nrpc.DomainGuidType)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.DsaGuidType)(0)), Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc);
                    this.Manager.Checkpoint("MS-NRPC_R103453");
                    this.Manager.Comment("reaching state \'S386\'");
                    this.Manager.Comment("checking step \'return DsrDeregisterDnsHostRecords/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp42, "return of DsrDeregisterDnsHostRecords, state S386");
                    Test_LocateDc_DsrDeregisterDnsHostRecordsS513();
                    goto label9;
                }
                if ((temp44 == 1)) {
                    this.Manager.Comment("reaching state \'S248\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp43;
                    this.Manager.Comment("executing step \'call DsrDeregisterDnsHostRecords(PrimaryDc,InvalidDomainName,Inva" +
                            "lidDomainGuid,DsaGuidOne,PrimaryDc)\'");
                    temp43 = this.INrpcServerAdapterInstance.DsrDeregisterDnsHostRecords(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.InvalidDomainName, ((Microsoft.Protocols.TestSuites.Nrpc.DomainGuidType)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.DsaGuidType)(0)), Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc);
                    this.Manager.Checkpoint("MS-NRPC_R103453");
                    this.Manager.Comment("reaching state \'S387\'");
                    this.Manager.Comment("checking step \'return DsrDeregisterDnsHostRecords/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp43, "return of DsrDeregisterDnsHostRecords, state S387");
                    Test_LocateDc_DsrDeregisterDnsHostRecordsS512();
                    goto label9;
                }
                throw new InvalidOperationException("never reached");
            label9:
;
                goto label11;
            }
            if ((temp49 == 2)) {
                this.Manager.Comment("reaching state \'S89\'");
                bool temp45;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp45);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp45);
                this.Manager.Comment("reaching state \'S170\'");
                int temp48 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_LocateDc_DsrDeregisterDnsHostRecords.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_LocateDc_DsrDeregisterDnsHostRecordsS26GetClientAccountTypeChecker4)), new ExpectedReturn(Test_LocateDc_DsrDeregisterDnsHostRecords.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_LocateDc_DsrDeregisterDnsHostRecordsS26GetClientAccountTypeChecker5)));
                if ((temp48 == 0)) {
                    this.Manager.Comment("reaching state \'S249\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp46;
                    this.Manager.Comment("executing step \'call DsrDeregisterDnsHostRecords(PrimaryDc,InvalidDomainName,Null" +
                            ",DsaGuidOne,PrimaryDc)\'");
                    temp46 = this.INrpcServerAdapterInstance.DsrDeregisterDnsHostRecords(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.InvalidDomainName, ((Microsoft.Protocols.TestSuites.Nrpc.DomainGuidType)(0)), ((Microsoft.Protocols.TestSuites.Nrpc.DsaGuidType)(0)), Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc);
                    this.Manager.Checkpoint("MS-NRPC_R103453");
                    this.Manager.Comment("reaching state \'S388\'");
                    this.Manager.Comment("checking step \'return DsrDeregisterDnsHostRecords/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp46, "return of DsrDeregisterDnsHostRecords, state S388");
                    this.Manager.Comment("reaching state \'S519\'");
                    goto label10;
                }
                if ((temp48 == 1)) {
                    this.Manager.Comment("reaching state \'S250\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp47;
                    this.Manager.Comment("executing step \'call DsrDeregisterDnsHostRecords(PrimaryDc,InvalidDomainName,Null" +
                            ",DsaGuidOne,PrimaryDc)\'");
                    temp47 = this.INrpcServerAdapterInstance.DsrDeregisterDnsHostRecords(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.InvalidDomainName, ((Microsoft.Protocols.TestSuites.Nrpc.DomainGuidType)(0)), ((Microsoft.Protocols.TestSuites.Nrpc.DsaGuidType)(0)), Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc);
                    this.Manager.Checkpoint("MS-NRPC_R103453");
                    this.Manager.Comment("reaching state \'S389\'");
                    this.Manager.Comment("checking step \'return DsrDeregisterDnsHostRecords/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp47, "return of DsrDeregisterDnsHostRecords, state S389");
                    this.Manager.Comment("reaching state \'S520\'");
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
        
        private void Test_LocateDc_DsrDeregisterDnsHostRecordsS26GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S27");
        }
        
        private void Test_LocateDc_DsrDeregisterDnsHostRecordsS26GetClientAccountTypeChecker(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S168");
        }
        
        private void Test_LocateDc_DsrDeregisterDnsHostRecordsS26GetClientAccountTypeChecker1(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S168");
        }
        
        private void Test_LocateDc_DsrDeregisterDnsHostRecordsS26GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S27");
        }
        
        private void Test_LocateDc_DsrDeregisterDnsHostRecordsS26GetClientAccountTypeChecker2(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S169");
        }
        
        private void Test_LocateDc_DsrDeregisterDnsHostRecordsS26GetClientAccountTypeChecker3(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S169");
        }
        
        private void Test_LocateDc_DsrDeregisterDnsHostRecordsS26GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S27");
        }
        
        private void Test_LocateDc_DsrDeregisterDnsHostRecordsS26GetClientAccountTypeChecker4(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S170");
        }
        
        private void Test_LocateDc_DsrDeregisterDnsHostRecordsS26GetClientAccountTypeChecker5(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S170");
        }
        #endregion
        
        #region Test Starting in S28
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_LocateDc_DsrDeregisterDnsHostRecordsS28() {
            this.Manager.BeginTest("Test_LocateDc_DsrDeregisterDnsHostRecordsS28");
            this.Manager.Comment("reaching state \'S28\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp50;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp50);
            this.Manager.AddReturn(GetPlatformInfo, null, temp50);
            this.Manager.Comment("reaching state \'S29\'");
            int temp63 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_LocateDc_DsrDeregisterDnsHostRecords.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_DsrDeregisterDnsHostRecordsS28GetPlatformChecker)), new ExpectedReturn(Test_LocateDc_DsrDeregisterDnsHostRecords.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_DsrDeregisterDnsHostRecordsS28GetPlatformChecker1)), new ExpectedReturn(Test_LocateDc_DsrDeregisterDnsHostRecords.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_DsrDeregisterDnsHostRecordsS28GetPlatformChecker2)));
            if ((temp63 == 0)) {
                this.Manager.Comment("reaching state \'S90\'");
                bool temp51;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp51);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp51);
                this.Manager.Comment("reaching state \'S171\'");
                int temp54 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_LocateDc_DsrDeregisterDnsHostRecords.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_LocateDc_DsrDeregisterDnsHostRecordsS28GetClientAccountTypeChecker)), new ExpectedReturn(Test_LocateDc_DsrDeregisterDnsHostRecords.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_LocateDc_DsrDeregisterDnsHostRecordsS28GetClientAccountTypeChecker1)));
                if ((temp54 == 0)) {
                    this.Manager.Comment("reaching state \'S251\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp52;
                    this.Manager.Comment("executing step \'call DsrDeregisterDnsHostRecords(PrimaryDc,InvalidDomainName,Prim" +
                            "aryDomainGuid,DsaGuidOne,PrimaryDc)\'");
                    temp52 = this.INrpcServerAdapterInstance.DsrDeregisterDnsHostRecords(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.InvalidDomainName, Microsoft.Protocols.TestSuites.Nrpc.DomainGuidType.PrimaryDomainGuid, ((Microsoft.Protocols.TestSuites.Nrpc.DsaGuidType)(0)), Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc);
                    this.Manager.Checkpoint("MS-NRPC_R103453");
                    this.Manager.Comment("reaching state \'S390\'");
                    this.Manager.Comment("checking step \'return DsrDeregisterDnsHostRecords/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp52, "return of DsrDeregisterDnsHostRecords, state S390");
                    Test_LocateDc_DsrDeregisterDnsHostRecordsS515();
                    goto label12;
                }
                if ((temp54 == 1)) {
                    this.Manager.Comment("reaching state \'S252\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp53;
                    this.Manager.Comment("executing step \'call DsrDeregisterDnsHostRecords(PrimaryDc,InvalidDomainName,Prim" +
                            "aryDomainGuid,DsaGuidOne,PrimaryDc)\'");
                    temp53 = this.INrpcServerAdapterInstance.DsrDeregisterDnsHostRecords(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.InvalidDomainName, Microsoft.Protocols.TestSuites.Nrpc.DomainGuidType.PrimaryDomainGuid, ((Microsoft.Protocols.TestSuites.Nrpc.DsaGuidType)(0)), Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc);
                    this.Manager.Checkpoint("MS-NRPC_R103453");
                    this.Manager.Comment("reaching state \'S391\'");
                    this.Manager.Comment("checking step \'return DsrDeregisterDnsHostRecords/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp53, "return of DsrDeregisterDnsHostRecords, state S391");
                    Test_LocateDc_DsrDeregisterDnsHostRecordsS514();
                    goto label12;
                }
                throw new InvalidOperationException("never reached");
            label12:
;
                goto label15;
            }
            if ((temp63 == 1)) {
                this.Manager.Comment("reaching state \'S91\'");
                bool temp55;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp55);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp55);
                this.Manager.Comment("reaching state \'S172\'");
                int temp58 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_LocateDc_DsrDeregisterDnsHostRecords.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_LocateDc_DsrDeregisterDnsHostRecordsS28GetClientAccountTypeChecker2)), new ExpectedReturn(Test_LocateDc_DsrDeregisterDnsHostRecords.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_LocateDc_DsrDeregisterDnsHostRecordsS28GetClientAccountTypeChecker3)));
                if ((temp58 == 0)) {
                    this.Manager.Comment("reaching state \'S253\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp56;
                    this.Manager.Comment("executing step \'call DsrDeregisterDnsHostRecords(PrimaryDc,InvalidDomainName,Prim" +
                            "aryDomainGuid,DsaGuidOne,PrimaryDc)\'");
                    temp56 = this.INrpcServerAdapterInstance.DsrDeregisterDnsHostRecords(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.InvalidDomainName, Microsoft.Protocols.TestSuites.Nrpc.DomainGuidType.PrimaryDomainGuid, ((Microsoft.Protocols.TestSuites.Nrpc.DsaGuidType)(0)), Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc);
                    this.Manager.Checkpoint("MS-NRPC_R103453");
                    this.Manager.Comment("reaching state \'S392\'");
                    this.Manager.Comment("checking step \'return DsrDeregisterDnsHostRecords/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp56, "return of DsrDeregisterDnsHostRecords, state S392");
                    Test_LocateDc_DsrDeregisterDnsHostRecordsS513();
                    goto label13;
                }
                if ((temp58 == 1)) {
                    this.Manager.Comment("reaching state \'S254\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp57;
                    this.Manager.Comment("executing step \'call DsrDeregisterDnsHostRecords(PrimaryDc,InvalidDomainName,Prim" +
                            "aryDomainGuid,DsaGuidOne,PrimaryDc)\'");
                    temp57 = this.INrpcServerAdapterInstance.DsrDeregisterDnsHostRecords(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.InvalidDomainName, Microsoft.Protocols.TestSuites.Nrpc.DomainGuidType.PrimaryDomainGuid, ((Microsoft.Protocols.TestSuites.Nrpc.DsaGuidType)(0)), Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc);
                    this.Manager.Checkpoint("MS-NRPC_R103453");
                    this.Manager.Comment("reaching state \'S393\'");
                    this.Manager.Comment("checking step \'return DsrDeregisterDnsHostRecords/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp57, "return of DsrDeregisterDnsHostRecords, state S393");
                    Test_LocateDc_DsrDeregisterDnsHostRecordsS512();
                    goto label13;
                }
                throw new InvalidOperationException("never reached");
            label13:
;
                goto label15;
            }
            if ((temp63 == 2)) {
                this.Manager.Comment("reaching state \'S92\'");
                bool temp59;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp59);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp59);
                this.Manager.Comment("reaching state \'S173\'");
                int temp62 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_LocateDc_DsrDeregisterDnsHostRecords.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_LocateDc_DsrDeregisterDnsHostRecordsS28GetClientAccountTypeChecker4)), new ExpectedReturn(Test_LocateDc_DsrDeregisterDnsHostRecords.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_LocateDc_DsrDeregisterDnsHostRecordsS28GetClientAccountTypeChecker5)));
                if ((temp62 == 0)) {
                    this.Manager.Comment("reaching state \'S255\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp60;
                    this.Manager.Comment("executing step \'call DsrDeregisterDnsHostRecords(PrimaryDc,InvalidDomainName,Null" +
                            ",DsaGuidOne,PrimaryDc)\'");
                    temp60 = this.INrpcServerAdapterInstance.DsrDeregisterDnsHostRecords(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.InvalidDomainName, ((Microsoft.Protocols.TestSuites.Nrpc.DomainGuidType)(0)), ((Microsoft.Protocols.TestSuites.Nrpc.DsaGuidType)(0)), Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc);
                    this.Manager.Checkpoint("MS-NRPC_R103453");
                    this.Manager.Comment("reaching state \'S394\'");
                    this.Manager.Comment("checking step \'return DsrDeregisterDnsHostRecords/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp60, "return of DsrDeregisterDnsHostRecords, state S394");
                    this.Manager.Comment("reaching state \'S521\'");
                    goto label14;
                }
                if ((temp62 == 1)) {
                    this.Manager.Comment("reaching state \'S256\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp61;
                    this.Manager.Comment("executing step \'call DsrDeregisterDnsHostRecords(PrimaryDc,InvalidDomainName,Null" +
                            ",DsaGuidOne,PrimaryDc)\'");
                    temp61 = this.INrpcServerAdapterInstance.DsrDeregisterDnsHostRecords(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.InvalidDomainName, ((Microsoft.Protocols.TestSuites.Nrpc.DomainGuidType)(0)), ((Microsoft.Protocols.TestSuites.Nrpc.DsaGuidType)(0)), Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc);
                    this.Manager.Checkpoint("MS-NRPC_R103453");
                    this.Manager.Comment("reaching state \'S395\'");
                    this.Manager.Comment("checking step \'return DsrDeregisterDnsHostRecords/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp61, "return of DsrDeregisterDnsHostRecords, state S395");
                    this.Manager.Comment("reaching state \'S522\'");
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
        
        private void Test_LocateDc_DsrDeregisterDnsHostRecordsS28GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S29");
        }
        
        private void Test_LocateDc_DsrDeregisterDnsHostRecordsS28GetClientAccountTypeChecker(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S171");
        }
        
        private void Test_LocateDc_DsrDeregisterDnsHostRecordsS28GetClientAccountTypeChecker1(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S171");
        }
        
        private void Test_LocateDc_DsrDeregisterDnsHostRecordsS28GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S29");
        }
        
        private void Test_LocateDc_DsrDeregisterDnsHostRecordsS28GetClientAccountTypeChecker2(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S172");
        }
        
        private void Test_LocateDc_DsrDeregisterDnsHostRecordsS28GetClientAccountTypeChecker3(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S172");
        }
        
        private void Test_LocateDc_DsrDeregisterDnsHostRecordsS28GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S29");
        }
        
        private void Test_LocateDc_DsrDeregisterDnsHostRecordsS28GetClientAccountTypeChecker4(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S173");
        }
        
        private void Test_LocateDc_DsrDeregisterDnsHostRecordsS28GetClientAccountTypeChecker5(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S173");
        }
        #endregion
        
        #region Test Starting in S30
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_LocateDc_DsrDeregisterDnsHostRecordsS30() {
            this.Manager.BeginTest("Test_LocateDc_DsrDeregisterDnsHostRecordsS30");
            this.Manager.Comment("reaching state \'S30\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp64;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp64);
            this.Manager.AddReturn(GetPlatformInfo, null, temp64);
            this.Manager.Comment("reaching state \'S31\'");
            int temp77 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_LocateDc_DsrDeregisterDnsHostRecords.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_DsrDeregisterDnsHostRecordsS30GetPlatformChecker)), new ExpectedReturn(Test_LocateDc_DsrDeregisterDnsHostRecords.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_DsrDeregisterDnsHostRecordsS30GetPlatformChecker1)), new ExpectedReturn(Test_LocateDc_DsrDeregisterDnsHostRecords.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_DsrDeregisterDnsHostRecordsS30GetPlatformChecker2)));
            if ((temp77 == 0)) {
                this.Manager.Comment("reaching state \'S93\'");
                bool temp65;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp65);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp65);
                this.Manager.Comment("reaching state \'S174\'");
                int temp68 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_LocateDc_DsrDeregisterDnsHostRecords.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_LocateDc_DsrDeregisterDnsHostRecordsS30GetClientAccountTypeChecker)), new ExpectedReturn(Test_LocateDc_DsrDeregisterDnsHostRecords.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_LocateDc_DsrDeregisterDnsHostRecordsS30GetClientAccountTypeChecker1)));
                if ((temp68 == 0)) {
                    this.Manager.Comment("reaching state \'S257\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp66;
                    this.Manager.Comment("executing step \'call DsrDeregisterDnsHostRecords(PrimaryDc,InvalidDomainName,Trus" +
                            "tedDomainGuid,DsaGuidOne,PrimaryDc)\'");
                    temp66 = this.INrpcServerAdapterInstance.DsrDeregisterDnsHostRecords(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.InvalidDomainName, Microsoft.Protocols.TestSuites.Nrpc.DomainGuidType.TrustedDomainGuid, ((Microsoft.Protocols.TestSuites.Nrpc.DsaGuidType)(0)), Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc);
                    this.Manager.Checkpoint("MS-NRPC_R103453");
                    this.Manager.Comment("reaching state \'S396\'");
                    this.Manager.Comment("checking step \'return DsrDeregisterDnsHostRecords/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp66, "return of DsrDeregisterDnsHostRecords, state S396");
                    Test_LocateDc_DsrDeregisterDnsHostRecordsS515();
                    goto label16;
                }
                if ((temp68 == 1)) {
                    this.Manager.Comment("reaching state \'S258\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp67;
                    this.Manager.Comment("executing step \'call DsrDeregisterDnsHostRecords(PrimaryDc,InvalidDomainName,Trus" +
                            "tedDomainGuid,DsaGuidOne,PrimaryDc)\'");
                    temp67 = this.INrpcServerAdapterInstance.DsrDeregisterDnsHostRecords(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.InvalidDomainName, Microsoft.Protocols.TestSuites.Nrpc.DomainGuidType.TrustedDomainGuid, ((Microsoft.Protocols.TestSuites.Nrpc.DsaGuidType)(0)), Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc);
                    this.Manager.Checkpoint("MS-NRPC_R103453");
                    this.Manager.Comment("reaching state \'S397\'");
                    this.Manager.Comment("checking step \'return DsrDeregisterDnsHostRecords/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp67, "return of DsrDeregisterDnsHostRecords, state S397");
                    Test_LocateDc_DsrDeregisterDnsHostRecordsS514();
                    goto label16;
                }
                throw new InvalidOperationException("never reached");
            label16:
;
                goto label19;
            }
            if ((temp77 == 1)) {
                this.Manager.Comment("reaching state \'S94\'");
                bool temp69;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp69);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp69);
                this.Manager.Comment("reaching state \'S175\'");
                int temp72 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_LocateDc_DsrDeregisterDnsHostRecords.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_LocateDc_DsrDeregisterDnsHostRecordsS30GetClientAccountTypeChecker2)), new ExpectedReturn(Test_LocateDc_DsrDeregisterDnsHostRecords.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_LocateDc_DsrDeregisterDnsHostRecordsS30GetClientAccountTypeChecker3)));
                if ((temp72 == 0)) {
                    this.Manager.Comment("reaching state \'S259\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp70;
                    this.Manager.Comment("executing step \'call DsrDeregisterDnsHostRecords(PrimaryDc,InvalidDomainName,Trus" +
                            "tedDomainGuid,DsaGuidOne,PrimaryDc)\'");
                    temp70 = this.INrpcServerAdapterInstance.DsrDeregisterDnsHostRecords(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.InvalidDomainName, Microsoft.Protocols.TestSuites.Nrpc.DomainGuidType.TrustedDomainGuid, ((Microsoft.Protocols.TestSuites.Nrpc.DsaGuidType)(0)), Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc);
                    this.Manager.Checkpoint("MS-NRPC_R103453");
                    this.Manager.Comment("reaching state \'S398\'");
                    this.Manager.Comment("checking step \'return DsrDeregisterDnsHostRecords/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp70, "return of DsrDeregisterDnsHostRecords, state S398");
                    Test_LocateDc_DsrDeregisterDnsHostRecordsS513();
                    goto label17;
                }
                if ((temp72 == 1)) {
                    this.Manager.Comment("reaching state \'S260\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp71;
                    this.Manager.Comment("executing step \'call DsrDeregisterDnsHostRecords(PrimaryDc,InvalidDomainName,Trus" +
                            "tedDomainGuid,DsaGuidOne,PrimaryDc)\'");
                    temp71 = this.INrpcServerAdapterInstance.DsrDeregisterDnsHostRecords(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.InvalidDomainName, Microsoft.Protocols.TestSuites.Nrpc.DomainGuidType.TrustedDomainGuid, ((Microsoft.Protocols.TestSuites.Nrpc.DsaGuidType)(0)), Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc);
                    this.Manager.Checkpoint("MS-NRPC_R103453");
                    this.Manager.Comment("reaching state \'S399\'");
                    this.Manager.Comment("checking step \'return DsrDeregisterDnsHostRecords/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp71, "return of DsrDeregisterDnsHostRecords, state S399");
                    Test_LocateDc_DsrDeregisterDnsHostRecordsS512();
                    goto label17;
                }
                throw new InvalidOperationException("never reached");
            label17:
;
                goto label19;
            }
            if ((temp77 == 2)) {
                this.Manager.Comment("reaching state \'S95\'");
                bool temp73;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp73);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp73);
                this.Manager.Comment("reaching state \'S176\'");
                int temp76 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_LocateDc_DsrDeregisterDnsHostRecords.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_LocateDc_DsrDeregisterDnsHostRecordsS30GetClientAccountTypeChecker4)), new ExpectedReturn(Test_LocateDc_DsrDeregisterDnsHostRecords.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_LocateDc_DsrDeregisterDnsHostRecordsS30GetClientAccountTypeChecker5)));
                if ((temp76 == 0)) {
                    this.Manager.Comment("reaching state \'S261\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp74;
                    this.Manager.Comment("executing step \'call DsrDeregisterDnsHostRecords(PrimaryDc,InvalidDomainName,Null" +
                            ",DsaGuidOne,PrimaryDc)\'");
                    temp74 = this.INrpcServerAdapterInstance.DsrDeregisterDnsHostRecords(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.InvalidDomainName, ((Microsoft.Protocols.TestSuites.Nrpc.DomainGuidType)(0)), ((Microsoft.Protocols.TestSuites.Nrpc.DsaGuidType)(0)), Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc);
                    this.Manager.Checkpoint("MS-NRPC_R103453");
                    this.Manager.Comment("reaching state \'S400\'");
                    this.Manager.Comment("checking step \'return DsrDeregisterDnsHostRecords/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp74, "return of DsrDeregisterDnsHostRecords, state S400");
                    this.Manager.Comment("reaching state \'S523\'");
                    goto label18;
                }
                if ((temp76 == 1)) {
                    this.Manager.Comment("reaching state \'S262\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp75;
                    this.Manager.Comment("executing step \'call DsrDeregisterDnsHostRecords(PrimaryDc,InvalidDomainName,Null" +
                            ",DsaGuidOne,PrimaryDc)\'");
                    temp75 = this.INrpcServerAdapterInstance.DsrDeregisterDnsHostRecords(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.InvalidDomainName, ((Microsoft.Protocols.TestSuites.Nrpc.DomainGuidType)(0)), ((Microsoft.Protocols.TestSuites.Nrpc.DsaGuidType)(0)), Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc);
                    this.Manager.Checkpoint("MS-NRPC_R103453");
                    this.Manager.Comment("reaching state \'S401\'");
                    this.Manager.Comment("checking step \'return DsrDeregisterDnsHostRecords/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp75, "return of DsrDeregisterDnsHostRecords, state S401");
                    this.Manager.Comment("reaching state \'S524\'");
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
        
        private void Test_LocateDc_DsrDeregisterDnsHostRecordsS30GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S31");
        }
        
        private void Test_LocateDc_DsrDeregisterDnsHostRecordsS30GetClientAccountTypeChecker(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S174");
        }
        
        private void Test_LocateDc_DsrDeregisterDnsHostRecordsS30GetClientAccountTypeChecker1(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S174");
        }
        
        private void Test_LocateDc_DsrDeregisterDnsHostRecordsS30GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S31");
        }
        
        private void Test_LocateDc_DsrDeregisterDnsHostRecordsS30GetClientAccountTypeChecker2(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S175");
        }
        
        private void Test_LocateDc_DsrDeregisterDnsHostRecordsS30GetClientAccountTypeChecker3(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S175");
        }
        
        private void Test_LocateDc_DsrDeregisterDnsHostRecordsS30GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S31");
        }
        
        private void Test_LocateDc_DsrDeregisterDnsHostRecordsS30GetClientAccountTypeChecker4(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S176");
        }
        
        private void Test_LocateDc_DsrDeregisterDnsHostRecordsS30GetClientAccountTypeChecker5(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S176");
        }
        #endregion
        
        #region Test Starting in S32
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_LocateDc_DsrDeregisterDnsHostRecordsS32() {
            this.Manager.BeginTest("Test_LocateDc_DsrDeregisterDnsHostRecordsS32");
            this.Manager.Comment("reaching state \'S32\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp78;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp78);
            this.Manager.AddReturn(GetPlatformInfo, null, temp78);
            this.Manager.Comment("reaching state \'S33\'");
            int temp91 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_LocateDc_DsrDeregisterDnsHostRecords.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_DsrDeregisterDnsHostRecordsS32GetPlatformChecker)), new ExpectedReturn(Test_LocateDc_DsrDeregisterDnsHostRecords.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_DsrDeregisterDnsHostRecordsS32GetPlatformChecker1)), new ExpectedReturn(Test_LocateDc_DsrDeregisterDnsHostRecords.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_DsrDeregisterDnsHostRecordsS32GetPlatformChecker2)));
            if ((temp91 == 0)) {
                this.Manager.Comment("reaching state \'S96\'");
                bool temp79;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp79);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp79);
                this.Manager.Comment("reaching state \'S177\'");
                int temp82 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_LocateDc_DsrDeregisterDnsHostRecords.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_LocateDc_DsrDeregisterDnsHostRecordsS32GetClientAccountTypeChecker)), new ExpectedReturn(Test_LocateDc_DsrDeregisterDnsHostRecords.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_LocateDc_DsrDeregisterDnsHostRecordsS32GetClientAccountTypeChecker1)));
                if ((temp82 == 0)) {
                    this.Manager.Comment("reaching state \'S263\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp80;
                    this.Manager.Comment("executing step \'call DsrDeregisterDnsHostRecords(PrimaryDc,NetBiosFormatDomainNam" +
                            "e,Null,DsaGuidOne,PrimaryDc)\'");
                    temp80 = this.INrpcServerAdapterInstance.DsrDeregisterDnsHostRecords(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.NetBiosFormatDomainName, ((Microsoft.Protocols.TestSuites.Nrpc.DomainGuidType)(0)), ((Microsoft.Protocols.TestSuites.Nrpc.DsaGuidType)(0)), Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc);
                    this.Manager.Checkpoint("MS-NRPC_R103453");
                    this.Manager.Comment("reaching state \'S402\'");
                    this.Manager.Comment("checking step \'return DsrDeregisterDnsHostRecords/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp80, "return of DsrDeregisterDnsHostRecords, state S402");
                    Test_LocateDc_DsrDeregisterDnsHostRecordsS515();
                    goto label20;
                }
                if ((temp82 == 1)) {
                    this.Manager.Comment("reaching state \'S264\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp81;
                    this.Manager.Comment("executing step \'call DsrDeregisterDnsHostRecords(PrimaryDc,NetBiosFormatDomainNam" +
                            "e,Null,DsaGuidOne,PrimaryDc)\'");
                    temp81 = this.INrpcServerAdapterInstance.DsrDeregisterDnsHostRecords(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.NetBiosFormatDomainName, ((Microsoft.Protocols.TestSuites.Nrpc.DomainGuidType)(0)), ((Microsoft.Protocols.TestSuites.Nrpc.DsaGuidType)(0)), Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc);
                    this.Manager.Checkpoint("MS-NRPC_R103453");
                    this.Manager.Comment("reaching state \'S403\'");
                    this.Manager.Comment("checking step \'return DsrDeregisterDnsHostRecords/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp81, "return of DsrDeregisterDnsHostRecords, state S403");
                    Test_LocateDc_DsrDeregisterDnsHostRecordsS514();
                    goto label20;
                }
                throw new InvalidOperationException("never reached");
            label20:
;
                goto label23;
            }
            if ((temp91 == 1)) {
                this.Manager.Comment("reaching state \'S97\'");
                bool temp83;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp83);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp83);
                this.Manager.Comment("reaching state \'S178\'");
                int temp86 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_LocateDc_DsrDeregisterDnsHostRecords.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_LocateDc_DsrDeregisterDnsHostRecordsS32GetClientAccountTypeChecker2)), new ExpectedReturn(Test_LocateDc_DsrDeregisterDnsHostRecords.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_LocateDc_DsrDeregisterDnsHostRecordsS32GetClientAccountTypeChecker3)));
                if ((temp86 == 0)) {
                    this.Manager.Comment("reaching state \'S265\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp84;
                    this.Manager.Comment("executing step \'call DsrDeregisterDnsHostRecords(PrimaryDc,NetBiosFormatDomainNam" +
                            "e,Null,DsaGuidOne,PrimaryDc)\'");
                    temp84 = this.INrpcServerAdapterInstance.DsrDeregisterDnsHostRecords(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.NetBiosFormatDomainName, ((Microsoft.Protocols.TestSuites.Nrpc.DomainGuidType)(0)), ((Microsoft.Protocols.TestSuites.Nrpc.DsaGuidType)(0)), Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc);
                    this.Manager.Checkpoint("MS-NRPC_R103453");
                    this.Manager.Comment("reaching state \'S404\'");
                    this.Manager.Comment("checking step \'return DsrDeregisterDnsHostRecords/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp84, "return of DsrDeregisterDnsHostRecords, state S404");
                    Test_LocateDc_DsrDeregisterDnsHostRecordsS513();
                    goto label21;
                }
                if ((temp86 == 1)) {
                    this.Manager.Comment("reaching state \'S266\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp85;
                    this.Manager.Comment("executing step \'call DsrDeregisterDnsHostRecords(PrimaryDc,NetBiosFormatDomainNam" +
                            "e,Null,DsaGuidOne,PrimaryDc)\'");
                    temp85 = this.INrpcServerAdapterInstance.DsrDeregisterDnsHostRecords(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.NetBiosFormatDomainName, ((Microsoft.Protocols.TestSuites.Nrpc.DomainGuidType)(0)), ((Microsoft.Protocols.TestSuites.Nrpc.DsaGuidType)(0)), Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc);
                    this.Manager.Checkpoint("MS-NRPC_R103453");
                    this.Manager.Comment("reaching state \'S405\'");
                    this.Manager.Comment("checking step \'return DsrDeregisterDnsHostRecords/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp85, "return of DsrDeregisterDnsHostRecords, state S405");
                    Test_LocateDc_DsrDeregisterDnsHostRecordsS512();
                    goto label21;
                }
                throw new InvalidOperationException("never reached");
            label21:
;
                goto label23;
            }
            if ((temp91 == 2)) {
                this.Manager.Comment("reaching state \'S98\'");
                bool temp87;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp87);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp87);
                this.Manager.Comment("reaching state \'S179\'");
                int temp90 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_LocateDc_DsrDeregisterDnsHostRecords.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_LocateDc_DsrDeregisterDnsHostRecordsS32GetClientAccountTypeChecker4)), new ExpectedReturn(Test_LocateDc_DsrDeregisterDnsHostRecords.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_LocateDc_DsrDeregisterDnsHostRecordsS32GetClientAccountTypeChecker5)));
                if ((temp90 == 0)) {
                    this.Manager.Comment("reaching state \'S267\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp88;
                    this.Manager.Comment("executing step \'call DsrDeregisterDnsHostRecords(PrimaryDc,InvalidDomainName,Null" +
                            ",DsaGuidOne,PrimaryDc)\'");
                    temp88 = this.INrpcServerAdapterInstance.DsrDeregisterDnsHostRecords(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.InvalidDomainName, ((Microsoft.Protocols.TestSuites.Nrpc.DomainGuidType)(0)), ((Microsoft.Protocols.TestSuites.Nrpc.DsaGuidType)(0)), Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc);
                    this.Manager.Checkpoint("MS-NRPC_R103453");
                    this.Manager.Comment("reaching state \'S406\'");
                    this.Manager.Comment("checking step \'return DsrDeregisterDnsHostRecords/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp88, "return of DsrDeregisterDnsHostRecords, state S406");
                    this.Manager.Comment("reaching state \'S525\'");
                    goto label22;
                }
                if ((temp90 == 1)) {
                    this.Manager.Comment("reaching state \'S268\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp89;
                    this.Manager.Comment("executing step \'call DsrDeregisterDnsHostRecords(PrimaryDc,InvalidDomainName,Null" +
                            ",DsaGuidOne,PrimaryDc)\'");
                    temp89 = this.INrpcServerAdapterInstance.DsrDeregisterDnsHostRecords(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.InvalidDomainName, ((Microsoft.Protocols.TestSuites.Nrpc.DomainGuidType)(0)), ((Microsoft.Protocols.TestSuites.Nrpc.DsaGuidType)(0)), Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc);
                    this.Manager.Checkpoint("MS-NRPC_R103453");
                    this.Manager.Comment("reaching state \'S407\'");
                    this.Manager.Comment("checking step \'return DsrDeregisterDnsHostRecords/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp89, "return of DsrDeregisterDnsHostRecords, state S407");
                    this.Manager.Comment("reaching state \'S526\'");
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
        
        private void Test_LocateDc_DsrDeregisterDnsHostRecordsS32GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S33");
        }
        
        private void Test_LocateDc_DsrDeregisterDnsHostRecordsS32GetClientAccountTypeChecker(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S177");
        }
        
        private void Test_LocateDc_DsrDeregisterDnsHostRecordsS32GetClientAccountTypeChecker1(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S177");
        }
        
        private void Test_LocateDc_DsrDeregisterDnsHostRecordsS32GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S33");
        }
        
        private void Test_LocateDc_DsrDeregisterDnsHostRecordsS32GetClientAccountTypeChecker2(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S178");
        }
        
        private void Test_LocateDc_DsrDeregisterDnsHostRecordsS32GetClientAccountTypeChecker3(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S178");
        }
        
        private void Test_LocateDc_DsrDeregisterDnsHostRecordsS32GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S33");
        }
        
        private void Test_LocateDc_DsrDeregisterDnsHostRecordsS32GetClientAccountTypeChecker4(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S179");
        }
        
        private void Test_LocateDc_DsrDeregisterDnsHostRecordsS32GetClientAccountTypeChecker5(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S179");
        }
        #endregion
        
        #region Test Starting in S34
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_LocateDc_DsrDeregisterDnsHostRecordsS34() {
            this.Manager.BeginTest("Test_LocateDc_DsrDeregisterDnsHostRecordsS34");
            this.Manager.Comment("reaching state \'S34\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp92;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp92);
            this.Manager.AddReturn(GetPlatformInfo, null, temp92);
            this.Manager.Comment("reaching state \'S35\'");
            int temp105 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_LocateDc_DsrDeregisterDnsHostRecords.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_DsrDeregisterDnsHostRecordsS34GetPlatformChecker)), new ExpectedReturn(Test_LocateDc_DsrDeregisterDnsHostRecords.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_DsrDeregisterDnsHostRecordsS34GetPlatformChecker1)), new ExpectedReturn(Test_LocateDc_DsrDeregisterDnsHostRecords.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_DsrDeregisterDnsHostRecordsS34GetPlatformChecker2)));
            if ((temp105 == 0)) {
                this.Manager.Comment("reaching state \'S100\'");
                bool temp93;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp93);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp93);
                this.Manager.Comment("reaching state \'S181\'");
                int temp96 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_LocateDc_DsrDeregisterDnsHostRecords.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_LocateDc_DsrDeregisterDnsHostRecordsS34GetClientAccountTypeChecker)), new ExpectedReturn(Test_LocateDc_DsrDeregisterDnsHostRecords.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_LocateDc_DsrDeregisterDnsHostRecordsS34GetClientAccountTypeChecker1)));
                if ((temp96 == 0)) {
                    this.Manager.Comment("reaching state \'S271\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp94;
                    this.Manager.Comment("executing step \'call DsrDeregisterDnsHostRecords(PrimaryDc,NetBiosFormatDomainNam" +
                            "e,InvalidDomainGuid,DsaGuidOne,PrimaryDc)\'");
                    temp94 = this.INrpcServerAdapterInstance.DsrDeregisterDnsHostRecords(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.NetBiosFormatDomainName, ((Microsoft.Protocols.TestSuites.Nrpc.DomainGuidType)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.DsaGuidType)(0)), Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc);
                    this.Manager.Checkpoint("MS-NRPC_R103453");
                    this.Manager.Comment("reaching state \'S410\'");
                    this.Manager.Comment("checking step \'return DsrDeregisterDnsHostRecords/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp94, "return of DsrDeregisterDnsHostRecords, state S410");
                    Test_LocateDc_DsrDeregisterDnsHostRecordsS513();
                    goto label24;
                }
                if ((temp96 == 1)) {
                    this.Manager.Comment("reaching state \'S272\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp95;
                    this.Manager.Comment("executing step \'call DsrDeregisterDnsHostRecords(PrimaryDc,NetBiosFormatDomainNam" +
                            "e,InvalidDomainGuid,DsaGuidOne,PrimaryDc)\'");
                    temp95 = this.INrpcServerAdapterInstance.DsrDeregisterDnsHostRecords(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.NetBiosFormatDomainName, ((Microsoft.Protocols.TestSuites.Nrpc.DomainGuidType)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.DsaGuidType)(0)), Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc);
                    this.Manager.Checkpoint("MS-NRPC_R103453");
                    this.Manager.Comment("reaching state \'S411\'");
                    this.Manager.Comment("checking step \'return DsrDeregisterDnsHostRecords/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp95, "return of DsrDeregisterDnsHostRecords, state S411");
                    Test_LocateDc_DsrDeregisterDnsHostRecordsS512();
                    goto label24;
                }
                throw new InvalidOperationException("never reached");
            label24:
;
                goto label27;
            }
            if ((temp105 == 1)) {
                this.Manager.Comment("reaching state \'S101\'");
                bool temp97;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp97);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp97);
                this.Manager.Comment("reaching state \'S182\'");
                int temp100 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_LocateDc_DsrDeregisterDnsHostRecords.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_LocateDc_DsrDeregisterDnsHostRecordsS34GetClientAccountTypeChecker2)), new ExpectedReturn(Test_LocateDc_DsrDeregisterDnsHostRecords.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_LocateDc_DsrDeregisterDnsHostRecordsS34GetClientAccountTypeChecker3)));
                if ((temp100 == 0)) {
                    this.Manager.Comment("reaching state \'S273\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp98;
                    this.Manager.Comment("executing step \'call DsrDeregisterDnsHostRecords(PrimaryDc,InvalidDomainName,Null" +
                            ",DsaGuidOne,PrimaryDc)\'");
                    temp98 = this.INrpcServerAdapterInstance.DsrDeregisterDnsHostRecords(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.InvalidDomainName, ((Microsoft.Protocols.TestSuites.Nrpc.DomainGuidType)(0)), ((Microsoft.Protocols.TestSuites.Nrpc.DsaGuidType)(0)), Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc);
                    this.Manager.Checkpoint("MS-NRPC_R103453");
                    this.Manager.Comment("reaching state \'S412\'");
                    this.Manager.Comment("checking step \'return DsrDeregisterDnsHostRecords/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp98, "return of DsrDeregisterDnsHostRecords, state S412");
                    this.Manager.Comment("reaching state \'S527\'");
                    goto label25;
                }
                if ((temp100 == 1)) {
                    this.Manager.Comment("reaching state \'S274\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp99;
                    this.Manager.Comment("executing step \'call DsrDeregisterDnsHostRecords(PrimaryDc,InvalidDomainName,Null" +
                            ",DsaGuidOne,PrimaryDc)\'");
                    temp99 = this.INrpcServerAdapterInstance.DsrDeregisterDnsHostRecords(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.InvalidDomainName, ((Microsoft.Protocols.TestSuites.Nrpc.DomainGuidType)(0)), ((Microsoft.Protocols.TestSuites.Nrpc.DsaGuidType)(0)), Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc);
                    this.Manager.Checkpoint("MS-NRPC_R103453");
                    this.Manager.Comment("reaching state \'S413\'");
                    this.Manager.Comment("checking step \'return DsrDeregisterDnsHostRecords/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp99, "return of DsrDeregisterDnsHostRecords, state S413");
                    this.Manager.Comment("reaching state \'S528\'");
                    goto label25;
                }
                throw new InvalidOperationException("never reached");
            label25:
;
                goto label27;
            }
            if ((temp105 == 2)) {
                this.Manager.Comment("reaching state \'S99\'");
                bool temp101;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp101);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp101);
                this.Manager.Comment("reaching state \'S180\'");
                int temp104 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_LocateDc_DsrDeregisterDnsHostRecords.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_LocateDc_DsrDeregisterDnsHostRecordsS34GetClientAccountTypeChecker4)), new ExpectedReturn(Test_LocateDc_DsrDeregisterDnsHostRecords.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_LocateDc_DsrDeregisterDnsHostRecordsS34GetClientAccountTypeChecker5)));
                if ((temp104 == 0)) {
                    this.Manager.Comment("reaching state \'S269\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp102;
                    this.Manager.Comment("executing step \'call DsrDeregisterDnsHostRecords(PrimaryDc,NetBiosFormatDomainNam" +
                            "e,InvalidDomainGuid,DsaGuidOne,PrimaryDc)\'");
                    temp102 = this.INrpcServerAdapterInstance.DsrDeregisterDnsHostRecords(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.NetBiosFormatDomainName, ((Microsoft.Protocols.TestSuites.Nrpc.DomainGuidType)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.DsaGuidType)(0)), Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc);
                    this.Manager.Checkpoint("MS-NRPC_R103453");
                    this.Manager.Comment("reaching state \'S408\'");
                    this.Manager.Comment("checking step \'return DsrDeregisterDnsHostRecords/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp102, "return of DsrDeregisterDnsHostRecords, state S408");
                    Test_LocateDc_DsrDeregisterDnsHostRecordsS515();
                    goto label26;
                }
                if ((temp104 == 1)) {
                    this.Manager.Comment("reaching state \'S270\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp103;
                    this.Manager.Comment("executing step \'call DsrDeregisterDnsHostRecords(PrimaryDc,NetBiosFormatDomainNam" +
                            "e,InvalidDomainGuid,DsaGuidOne,PrimaryDc)\'");
                    temp103 = this.INrpcServerAdapterInstance.DsrDeregisterDnsHostRecords(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.NetBiosFormatDomainName, ((Microsoft.Protocols.TestSuites.Nrpc.DomainGuidType)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.DsaGuidType)(0)), Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc);
                    this.Manager.Checkpoint("MS-NRPC_R103453");
                    this.Manager.Comment("reaching state \'S409\'");
                    this.Manager.Comment("checking step \'return DsrDeregisterDnsHostRecords/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp103, "return of DsrDeregisterDnsHostRecords, state S409");
                    Test_LocateDc_DsrDeregisterDnsHostRecordsS514();
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
        
        private void Test_LocateDc_DsrDeregisterDnsHostRecordsS34GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S35");
        }
        
        private void Test_LocateDc_DsrDeregisterDnsHostRecordsS34GetClientAccountTypeChecker(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S181");
        }
        
        private void Test_LocateDc_DsrDeregisterDnsHostRecordsS34GetClientAccountTypeChecker1(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S181");
        }
        
        private void Test_LocateDc_DsrDeregisterDnsHostRecordsS34GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S35");
        }
        
        private void Test_LocateDc_DsrDeregisterDnsHostRecordsS34GetClientAccountTypeChecker2(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S182");
        }
        
        private void Test_LocateDc_DsrDeregisterDnsHostRecordsS34GetClientAccountTypeChecker3(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S182");
        }
        
        private void Test_LocateDc_DsrDeregisterDnsHostRecordsS34GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S35");
        }
        
        private void Test_LocateDc_DsrDeregisterDnsHostRecordsS34GetClientAccountTypeChecker4(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S180");
        }
        
        private void Test_LocateDc_DsrDeregisterDnsHostRecordsS34GetClientAccountTypeChecker5(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S180");
        }
        #endregion
        
        #region Test Starting in S36
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_LocateDc_DsrDeregisterDnsHostRecordsS36() {
            this.Manager.BeginTest("Test_LocateDc_DsrDeregisterDnsHostRecordsS36");
            this.Manager.Comment("reaching state \'S36\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp106;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp106);
            this.Manager.AddReturn(GetPlatformInfo, null, temp106);
            this.Manager.Comment("reaching state \'S37\'");
            int temp119 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_LocateDc_DsrDeregisterDnsHostRecords.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_DsrDeregisterDnsHostRecordsS36GetPlatformChecker)), new ExpectedReturn(Test_LocateDc_DsrDeregisterDnsHostRecords.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_DsrDeregisterDnsHostRecordsS36GetPlatformChecker1)), new ExpectedReturn(Test_LocateDc_DsrDeregisterDnsHostRecords.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_DsrDeregisterDnsHostRecordsS36GetPlatformChecker2)));
            if ((temp119 == 0)) {
                this.Manager.Comment("reaching state \'S102\'");
                bool temp107;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp107);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp107);
                this.Manager.Comment("reaching state \'S183\'");
                int temp110 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_LocateDc_DsrDeregisterDnsHostRecords.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_LocateDc_DsrDeregisterDnsHostRecordsS36GetClientAccountTypeChecker)), new ExpectedReturn(Test_LocateDc_DsrDeregisterDnsHostRecords.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_LocateDc_DsrDeregisterDnsHostRecordsS36GetClientAccountTypeChecker1)));
                if ((temp110 == 0)) {
                    this.Manager.Comment("reaching state \'S275\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp108;
                    this.Manager.Comment("executing step \'call DsrDeregisterDnsHostRecords(PrimaryDc,NetBiosFormatDomainNam" +
                            "e,PrimaryDomainGuid,DsaGuidOne,PrimaryDc)\'");
                    temp108 = this.INrpcServerAdapterInstance.DsrDeregisterDnsHostRecords(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.NetBiosFormatDomainName, Microsoft.Protocols.TestSuites.Nrpc.DomainGuidType.PrimaryDomainGuid, ((Microsoft.Protocols.TestSuites.Nrpc.DsaGuidType)(0)), Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc);
                    this.Manager.Checkpoint("MS-NRPC_R103453");
                    this.Manager.Comment("reaching state \'S414\'");
                    this.Manager.Comment("checking step \'return DsrDeregisterDnsHostRecords/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp108, "return of DsrDeregisterDnsHostRecords, state S414");
                    Test_LocateDc_DsrDeregisterDnsHostRecordsS515();
                    goto label28;
                }
                if ((temp110 == 1)) {
                    this.Manager.Comment("reaching state \'S276\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp109;
                    this.Manager.Comment("executing step \'call DsrDeregisterDnsHostRecords(PrimaryDc,NetBiosFormatDomainNam" +
                            "e,PrimaryDomainGuid,DsaGuidOne,PrimaryDc)\'");
                    temp109 = this.INrpcServerAdapterInstance.DsrDeregisterDnsHostRecords(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.NetBiosFormatDomainName, Microsoft.Protocols.TestSuites.Nrpc.DomainGuidType.PrimaryDomainGuid, ((Microsoft.Protocols.TestSuites.Nrpc.DsaGuidType)(0)), Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc);
                    this.Manager.Checkpoint("MS-NRPC_R103453");
                    this.Manager.Comment("reaching state \'S415\'");
                    this.Manager.Comment("checking step \'return DsrDeregisterDnsHostRecords/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp109, "return of DsrDeregisterDnsHostRecords, state S415");
                    Test_LocateDc_DsrDeregisterDnsHostRecordsS514();
                    goto label28;
                }
                throw new InvalidOperationException("never reached");
            label28:
;
                goto label31;
            }
            if ((temp119 == 1)) {
                this.Manager.Comment("reaching state \'S103\'");
                bool temp111;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp111);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp111);
                this.Manager.Comment("reaching state \'S184\'");
                int temp114 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_LocateDc_DsrDeregisterDnsHostRecords.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_LocateDc_DsrDeregisterDnsHostRecordsS36GetClientAccountTypeChecker2)), new ExpectedReturn(Test_LocateDc_DsrDeregisterDnsHostRecords.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_LocateDc_DsrDeregisterDnsHostRecordsS36GetClientAccountTypeChecker3)));
                if ((temp114 == 0)) {
                    this.Manager.Comment("reaching state \'S277\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp112;
                    this.Manager.Comment("executing step \'call DsrDeregisterDnsHostRecords(PrimaryDc,NetBiosFormatDomainNam" +
                            "e,PrimaryDomainGuid,DsaGuidOne,PrimaryDc)\'");
                    temp112 = this.INrpcServerAdapterInstance.DsrDeregisterDnsHostRecords(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.NetBiosFormatDomainName, Microsoft.Protocols.TestSuites.Nrpc.DomainGuidType.PrimaryDomainGuid, ((Microsoft.Protocols.TestSuites.Nrpc.DsaGuidType)(0)), Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc);
                    this.Manager.Checkpoint("MS-NRPC_R103453");
                    this.Manager.Comment("reaching state \'S416\'");
                    this.Manager.Comment("checking step \'return DsrDeregisterDnsHostRecords/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp112, "return of DsrDeregisterDnsHostRecords, state S416");
                    Test_LocateDc_DsrDeregisterDnsHostRecordsS513();
                    goto label29;
                }
                if ((temp114 == 1)) {
                    this.Manager.Comment("reaching state \'S278\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp113;
                    this.Manager.Comment("executing step \'call DsrDeregisterDnsHostRecords(PrimaryDc,NetBiosFormatDomainNam" +
                            "e,PrimaryDomainGuid,DsaGuidOne,PrimaryDc)\'");
                    temp113 = this.INrpcServerAdapterInstance.DsrDeregisterDnsHostRecords(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.NetBiosFormatDomainName, Microsoft.Protocols.TestSuites.Nrpc.DomainGuidType.PrimaryDomainGuid, ((Microsoft.Protocols.TestSuites.Nrpc.DsaGuidType)(0)), Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc);
                    this.Manager.Checkpoint("MS-NRPC_R103453");
                    this.Manager.Comment("reaching state \'S417\'");
                    this.Manager.Comment("checking step \'return DsrDeregisterDnsHostRecords/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp113, "return of DsrDeregisterDnsHostRecords, state S417");
                    Test_LocateDc_DsrDeregisterDnsHostRecordsS512();
                    goto label29;
                }
                throw new InvalidOperationException("never reached");
            label29:
;
                goto label31;
            }
            if ((temp119 == 2)) {
                this.Manager.Comment("reaching state \'S104\'");
                bool temp115;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp115);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp115);
                this.Manager.Comment("reaching state \'S185\'");
                int temp118 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_LocateDc_DsrDeregisterDnsHostRecords.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_LocateDc_DsrDeregisterDnsHostRecordsS36GetClientAccountTypeChecker4)), new ExpectedReturn(Test_LocateDc_DsrDeregisterDnsHostRecords.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_LocateDc_DsrDeregisterDnsHostRecordsS36GetClientAccountTypeChecker5)));
                if ((temp118 == 0)) {
                    this.Manager.Comment("reaching state \'S279\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp116;
                    this.Manager.Comment("executing step \'call DsrDeregisterDnsHostRecords(PrimaryDc,InvalidDomainName,Null" +
                            ",DsaGuidOne,PrimaryDc)\'");
                    temp116 = this.INrpcServerAdapterInstance.DsrDeregisterDnsHostRecords(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.InvalidDomainName, ((Microsoft.Protocols.TestSuites.Nrpc.DomainGuidType)(0)), ((Microsoft.Protocols.TestSuites.Nrpc.DsaGuidType)(0)), Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc);
                    this.Manager.Checkpoint("MS-NRPC_R103453");
                    this.Manager.Comment("reaching state \'S418\'");
                    this.Manager.Comment("checking step \'return DsrDeregisterDnsHostRecords/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp116, "return of DsrDeregisterDnsHostRecords, state S418");
                    this.Manager.Comment("reaching state \'S529\'");
                    goto label30;
                }
                if ((temp118 == 1)) {
                    this.Manager.Comment("reaching state \'S280\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp117;
                    this.Manager.Comment("executing step \'call DsrDeregisterDnsHostRecords(PrimaryDc,InvalidDomainName,Null" +
                            ",DsaGuidOne,PrimaryDc)\'");
                    temp117 = this.INrpcServerAdapterInstance.DsrDeregisterDnsHostRecords(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.InvalidDomainName, ((Microsoft.Protocols.TestSuites.Nrpc.DomainGuidType)(0)), ((Microsoft.Protocols.TestSuites.Nrpc.DsaGuidType)(0)), Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc);
                    this.Manager.Checkpoint("MS-NRPC_R103453");
                    this.Manager.Comment("reaching state \'S419\'");
                    this.Manager.Comment("checking step \'return DsrDeregisterDnsHostRecords/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp117, "return of DsrDeregisterDnsHostRecords, state S419");
                    this.Manager.Comment("reaching state \'S530\'");
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
        
        private void Test_LocateDc_DsrDeregisterDnsHostRecordsS36GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S37");
        }
        
        private void Test_LocateDc_DsrDeregisterDnsHostRecordsS36GetClientAccountTypeChecker(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S183");
        }
        
        private void Test_LocateDc_DsrDeregisterDnsHostRecordsS36GetClientAccountTypeChecker1(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S183");
        }
        
        private void Test_LocateDc_DsrDeregisterDnsHostRecordsS36GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S37");
        }
        
        private void Test_LocateDc_DsrDeregisterDnsHostRecordsS36GetClientAccountTypeChecker2(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S184");
        }
        
        private void Test_LocateDc_DsrDeregisterDnsHostRecordsS36GetClientAccountTypeChecker3(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S184");
        }
        
        private void Test_LocateDc_DsrDeregisterDnsHostRecordsS36GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S37");
        }
        
        private void Test_LocateDc_DsrDeregisterDnsHostRecordsS36GetClientAccountTypeChecker4(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S185");
        }
        
        private void Test_LocateDc_DsrDeregisterDnsHostRecordsS36GetClientAccountTypeChecker5(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S185");
        }
        #endregion
        
        #region Test Starting in S38
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_LocateDc_DsrDeregisterDnsHostRecordsS38() {
            this.Manager.BeginTest("Test_LocateDc_DsrDeregisterDnsHostRecordsS38");
            this.Manager.Comment("reaching state \'S38\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp120;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp120);
            this.Manager.AddReturn(GetPlatformInfo, null, temp120);
            this.Manager.Comment("reaching state \'S39\'");
            int temp133 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_LocateDc_DsrDeregisterDnsHostRecords.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_DsrDeregisterDnsHostRecordsS38GetPlatformChecker)), new ExpectedReturn(Test_LocateDc_DsrDeregisterDnsHostRecords.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_DsrDeregisterDnsHostRecordsS38GetPlatformChecker1)), new ExpectedReturn(Test_LocateDc_DsrDeregisterDnsHostRecords.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_DsrDeregisterDnsHostRecordsS38GetPlatformChecker2)));
            if ((temp133 == 0)) {
                this.Manager.Comment("reaching state \'S105\'");
                bool temp121;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp121);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp121);
                this.Manager.Comment("reaching state \'S186\'");
                int temp124 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_LocateDc_DsrDeregisterDnsHostRecords.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_LocateDc_DsrDeregisterDnsHostRecordsS38GetClientAccountTypeChecker)), new ExpectedReturn(Test_LocateDc_DsrDeregisterDnsHostRecords.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_LocateDc_DsrDeregisterDnsHostRecordsS38GetClientAccountTypeChecker1)));
                if ((temp124 == 0)) {
                    this.Manager.Comment("reaching state \'S281\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp122;
                    this.Manager.Comment("executing step \'call DsrDeregisterDnsHostRecords(PrimaryDc,NetBiosFormatDomainNam" +
                            "e,TrustedDomainGuid,DsaGuidOne,PrimaryDc)\'");
                    temp122 = this.INrpcServerAdapterInstance.DsrDeregisterDnsHostRecords(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.NetBiosFormatDomainName, Microsoft.Protocols.TestSuites.Nrpc.DomainGuidType.TrustedDomainGuid, ((Microsoft.Protocols.TestSuites.Nrpc.DsaGuidType)(0)), Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc);
                    this.Manager.Checkpoint("MS-NRPC_R103453");
                    this.Manager.Comment("reaching state \'S420\'");
                    this.Manager.Comment("checking step \'return DsrDeregisterDnsHostRecords/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp122, "return of DsrDeregisterDnsHostRecords, state S420");
                    Test_LocateDc_DsrDeregisterDnsHostRecordsS515();
                    goto label32;
                }
                if ((temp124 == 1)) {
                    this.Manager.Comment("reaching state \'S282\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp123;
                    this.Manager.Comment("executing step \'call DsrDeregisterDnsHostRecords(PrimaryDc,NetBiosFormatDomainNam" +
                            "e,TrustedDomainGuid,DsaGuidOne,PrimaryDc)\'");
                    temp123 = this.INrpcServerAdapterInstance.DsrDeregisterDnsHostRecords(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.NetBiosFormatDomainName, Microsoft.Protocols.TestSuites.Nrpc.DomainGuidType.TrustedDomainGuid, ((Microsoft.Protocols.TestSuites.Nrpc.DsaGuidType)(0)), Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc);
                    this.Manager.Checkpoint("MS-NRPC_R103453");
                    this.Manager.Comment("reaching state \'S421\'");
                    this.Manager.Comment("checking step \'return DsrDeregisterDnsHostRecords/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp123, "return of DsrDeregisterDnsHostRecords, state S421");
                    Test_LocateDc_DsrDeregisterDnsHostRecordsS514();
                    goto label32;
                }
                throw new InvalidOperationException("never reached");
            label32:
;
                goto label35;
            }
            if ((temp133 == 1)) {
                this.Manager.Comment("reaching state \'S106\'");
                bool temp125;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp125);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp125);
                this.Manager.Comment("reaching state \'S187\'");
                int temp128 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_LocateDc_DsrDeregisterDnsHostRecords.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_LocateDc_DsrDeregisterDnsHostRecordsS38GetClientAccountTypeChecker2)), new ExpectedReturn(Test_LocateDc_DsrDeregisterDnsHostRecords.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_LocateDc_DsrDeregisterDnsHostRecordsS38GetClientAccountTypeChecker3)));
                if ((temp128 == 0)) {
                    this.Manager.Comment("reaching state \'S283\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp126;
                    this.Manager.Comment("executing step \'call DsrDeregisterDnsHostRecords(PrimaryDc,NetBiosFormatDomainNam" +
                            "e,TrustedDomainGuid,DsaGuidOne,PrimaryDc)\'");
                    temp126 = this.INrpcServerAdapterInstance.DsrDeregisterDnsHostRecords(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.NetBiosFormatDomainName, Microsoft.Protocols.TestSuites.Nrpc.DomainGuidType.TrustedDomainGuid, ((Microsoft.Protocols.TestSuites.Nrpc.DsaGuidType)(0)), Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc);
                    this.Manager.Checkpoint("MS-NRPC_R103453");
                    this.Manager.Comment("reaching state \'S422\'");
                    this.Manager.Comment("checking step \'return DsrDeregisterDnsHostRecords/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp126, "return of DsrDeregisterDnsHostRecords, state S422");
                    Test_LocateDc_DsrDeregisterDnsHostRecordsS513();
                    goto label33;
                }
                if ((temp128 == 1)) {
                    this.Manager.Comment("reaching state \'S284\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp127;
                    this.Manager.Comment("executing step \'call DsrDeregisterDnsHostRecords(PrimaryDc,NetBiosFormatDomainNam" +
                            "e,TrustedDomainGuid,DsaGuidOne,PrimaryDc)\'");
                    temp127 = this.INrpcServerAdapterInstance.DsrDeregisterDnsHostRecords(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.NetBiosFormatDomainName, Microsoft.Protocols.TestSuites.Nrpc.DomainGuidType.TrustedDomainGuid, ((Microsoft.Protocols.TestSuites.Nrpc.DsaGuidType)(0)), Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc);
                    this.Manager.Checkpoint("MS-NRPC_R103453");
                    this.Manager.Comment("reaching state \'S423\'");
                    this.Manager.Comment("checking step \'return DsrDeregisterDnsHostRecords/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp127, "return of DsrDeregisterDnsHostRecords, state S423");
                    Test_LocateDc_DsrDeregisterDnsHostRecordsS512();
                    goto label33;
                }
                throw new InvalidOperationException("never reached");
            label33:
;
                goto label35;
            }
            if ((temp133 == 2)) {
                this.Manager.Comment("reaching state \'S107\'");
                bool temp129;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp129);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp129);
                this.Manager.Comment("reaching state \'S188\'");
                int temp132 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_LocateDc_DsrDeregisterDnsHostRecords.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_LocateDc_DsrDeregisterDnsHostRecordsS38GetClientAccountTypeChecker4)), new ExpectedReturn(Test_LocateDc_DsrDeregisterDnsHostRecords.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_LocateDc_DsrDeregisterDnsHostRecordsS38GetClientAccountTypeChecker5)));
                if ((temp132 == 0)) {
                    this.Manager.Comment("reaching state \'S285\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp130;
                    this.Manager.Comment("executing step \'call DsrDeregisterDnsHostRecords(PrimaryDc,InvalidDomainName,Null" +
                            ",DsaGuidOne,PrimaryDc)\'");
                    temp130 = this.INrpcServerAdapterInstance.DsrDeregisterDnsHostRecords(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.InvalidDomainName, ((Microsoft.Protocols.TestSuites.Nrpc.DomainGuidType)(0)), ((Microsoft.Protocols.TestSuites.Nrpc.DsaGuidType)(0)), Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc);
                    this.Manager.Checkpoint("MS-NRPC_R103453");
                    this.Manager.Comment("reaching state \'S424\'");
                    this.Manager.Comment("checking step \'return DsrDeregisterDnsHostRecords/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp130, "return of DsrDeregisterDnsHostRecords, state S424");
                    this.Manager.Comment("reaching state \'S531\'");
                    goto label34;
                }
                if ((temp132 == 1)) {
                    this.Manager.Comment("reaching state \'S286\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp131;
                    this.Manager.Comment("executing step \'call DsrDeregisterDnsHostRecords(PrimaryDc,InvalidDomainName,Null" +
                            ",DsaGuidOne,PrimaryDc)\'");
                    temp131 = this.INrpcServerAdapterInstance.DsrDeregisterDnsHostRecords(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.InvalidDomainName, ((Microsoft.Protocols.TestSuites.Nrpc.DomainGuidType)(0)), ((Microsoft.Protocols.TestSuites.Nrpc.DsaGuidType)(0)), Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc);
                    this.Manager.Checkpoint("MS-NRPC_R103453");
                    this.Manager.Comment("reaching state \'S425\'");
                    this.Manager.Comment("checking step \'return DsrDeregisterDnsHostRecords/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp131, "return of DsrDeregisterDnsHostRecords, state S425");
                    this.Manager.Comment("reaching state \'S532\'");
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
        
        private void Test_LocateDc_DsrDeregisterDnsHostRecordsS38GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S39");
        }
        
        private void Test_LocateDc_DsrDeregisterDnsHostRecordsS38GetClientAccountTypeChecker(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S186");
        }
        
        private void Test_LocateDc_DsrDeregisterDnsHostRecordsS38GetClientAccountTypeChecker1(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S186");
        }
        
        private void Test_LocateDc_DsrDeregisterDnsHostRecordsS38GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S39");
        }
        
        private void Test_LocateDc_DsrDeregisterDnsHostRecordsS38GetClientAccountTypeChecker2(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S187");
        }
        
        private void Test_LocateDc_DsrDeregisterDnsHostRecordsS38GetClientAccountTypeChecker3(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S187");
        }
        
        private void Test_LocateDc_DsrDeregisterDnsHostRecordsS38GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S39");
        }
        
        private void Test_LocateDc_DsrDeregisterDnsHostRecordsS38GetClientAccountTypeChecker4(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S188");
        }
        
        private void Test_LocateDc_DsrDeregisterDnsHostRecordsS38GetClientAccountTypeChecker5(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S188");
        }
        #endregion
        
        #region Test Starting in S4
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_LocateDc_DsrDeregisterDnsHostRecordsS4() {
            this.Manager.BeginTest("Test_LocateDc_DsrDeregisterDnsHostRecordsS4");
            this.Manager.Comment("reaching state \'S4\'");
            this.Manager.Comment("executing step \'call SwitchUserAccount(False)\'");
            this.INrpcServerAdapterInstance.SwitchUserAccount(false);
            this.Manager.Comment("reaching state \'S5\'");
            this.Manager.Comment("checking step \'return SwitchUserAccount\'");
            this.Manager.Comment("reaching state \'S74\'");
            Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp134;
            this.Manager.Comment("executing step \'call DsrDeregisterDnsHostRecords(PrimaryDc,InvalidDomainName,Inva" +
                    "lidDomainGuid,DsaGuidOne,PrimaryDc)\'");
            temp134 = this.INrpcServerAdapterInstance.DsrDeregisterDnsHostRecords(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.InvalidDomainName, ((Microsoft.Protocols.TestSuites.Nrpc.DomainGuidType)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.DsaGuidType)(0)), Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc);
            this.Manager.Checkpoint("MS-NRPC_R103328");
            this.Manager.Checkpoint("MS-NRPC_R103454");
            this.Manager.Comment("reaching state \'S155\'");
            this.Manager.Comment("checking step \'return DsrDeregisterDnsHostRecords/ERROR_ACCESS_DENIED\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_ACCESS_DENIED, temp134, "return of DsrDeregisterDnsHostRecords, state S155");
            Test_LocateDc_DsrDeregisterDnsHostRecordsS238();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S40
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_LocateDc_DsrDeregisterDnsHostRecordsS40() {
            this.Manager.BeginTest("Test_LocateDc_DsrDeregisterDnsHostRecordsS40");
            this.Manager.Comment("reaching state \'S40\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp135;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp135);
            this.Manager.AddReturn(GetPlatformInfo, null, temp135);
            this.Manager.Comment("reaching state \'S41\'");
            int temp148 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_LocateDc_DsrDeregisterDnsHostRecords.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_DsrDeregisterDnsHostRecordsS40GetPlatformChecker)), new ExpectedReturn(Test_LocateDc_DsrDeregisterDnsHostRecords.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_DsrDeregisterDnsHostRecordsS40GetPlatformChecker1)), new ExpectedReturn(Test_LocateDc_DsrDeregisterDnsHostRecords.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_DsrDeregisterDnsHostRecordsS40GetPlatformChecker2)));
            if ((temp148 == 0)) {
                this.Manager.Comment("reaching state \'S108\'");
                bool temp136;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp136);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp136);
                this.Manager.Comment("reaching state \'S189\'");
                int temp139 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_LocateDc_DsrDeregisterDnsHostRecords.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_LocateDc_DsrDeregisterDnsHostRecordsS40GetClientAccountTypeChecker)), new ExpectedReturn(Test_LocateDc_DsrDeregisterDnsHostRecords.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_LocateDc_DsrDeregisterDnsHostRecordsS40GetClientAccountTypeChecker1)));
                if ((temp139 == 0)) {
                    this.Manager.Comment("reaching state \'S287\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp137;
                    this.Manager.Comment("executing step \'call DsrDeregisterDnsHostRecords(PrimaryDc,FqdnFormatDomainName,N" +
                            "ull,DsaGuidOne,PrimaryDc)\'");
                    temp137 = this.INrpcServerAdapterInstance.DsrDeregisterDnsHostRecords(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.FqdnFormatDomainName, ((Microsoft.Protocols.TestSuites.Nrpc.DomainGuidType)(0)), ((Microsoft.Protocols.TestSuites.Nrpc.DsaGuidType)(0)), Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc);
                    this.Manager.Checkpoint("MS-NRPC_R103453");
                    this.Manager.Comment("reaching state \'S426\'");
                    this.Manager.Comment("checking step \'return DsrDeregisterDnsHostRecords/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp137, "return of DsrDeregisterDnsHostRecords, state S426");
                    Test_LocateDc_DsrDeregisterDnsHostRecordsS515();
                    goto label36;
                }
                if ((temp139 == 1)) {
                    this.Manager.Comment("reaching state \'S288\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp138;
                    this.Manager.Comment("executing step \'call DsrDeregisterDnsHostRecords(PrimaryDc,FqdnFormatDomainName,N" +
                            "ull,DsaGuidOne,PrimaryDc)\'");
                    temp138 = this.INrpcServerAdapterInstance.DsrDeregisterDnsHostRecords(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.FqdnFormatDomainName, ((Microsoft.Protocols.TestSuites.Nrpc.DomainGuidType)(0)), ((Microsoft.Protocols.TestSuites.Nrpc.DsaGuidType)(0)), Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc);
                    this.Manager.Checkpoint("MS-NRPC_R103453");
                    this.Manager.Comment("reaching state \'S427\'");
                    this.Manager.Comment("checking step \'return DsrDeregisterDnsHostRecords/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp138, "return of DsrDeregisterDnsHostRecords, state S427");
                    Test_LocateDc_DsrDeregisterDnsHostRecordsS514();
                    goto label36;
                }
                throw new InvalidOperationException("never reached");
            label36:
;
                goto label39;
            }
            if ((temp148 == 1)) {
                this.Manager.Comment("reaching state \'S109\'");
                bool temp140;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp140);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp140);
                this.Manager.Comment("reaching state \'S190\'");
                int temp143 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_LocateDc_DsrDeregisterDnsHostRecords.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_LocateDc_DsrDeregisterDnsHostRecordsS40GetClientAccountTypeChecker2)), new ExpectedReturn(Test_LocateDc_DsrDeregisterDnsHostRecords.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_LocateDc_DsrDeregisterDnsHostRecordsS40GetClientAccountTypeChecker3)));
                if ((temp143 == 0)) {
                    this.Manager.Comment("reaching state \'S289\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp141;
                    this.Manager.Comment("executing step \'call DsrDeregisterDnsHostRecords(PrimaryDc,FqdnFormatDomainName,N" +
                            "ull,DsaGuidOne,PrimaryDc)\'");
                    temp141 = this.INrpcServerAdapterInstance.DsrDeregisterDnsHostRecords(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.FqdnFormatDomainName, ((Microsoft.Protocols.TestSuites.Nrpc.DomainGuidType)(0)), ((Microsoft.Protocols.TestSuites.Nrpc.DsaGuidType)(0)), Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc);
                    this.Manager.Checkpoint("MS-NRPC_R103453");
                    this.Manager.Comment("reaching state \'S428\'");
                    this.Manager.Comment("checking step \'return DsrDeregisterDnsHostRecords/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp141, "return of DsrDeregisterDnsHostRecords, state S428");
                    Test_LocateDc_DsrDeregisterDnsHostRecordsS513();
                    goto label37;
                }
                if ((temp143 == 1)) {
                    this.Manager.Comment("reaching state \'S290\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp142;
                    this.Manager.Comment("executing step \'call DsrDeregisterDnsHostRecords(PrimaryDc,FqdnFormatDomainName,N" +
                            "ull,DsaGuidOne,PrimaryDc)\'");
                    temp142 = this.INrpcServerAdapterInstance.DsrDeregisterDnsHostRecords(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.FqdnFormatDomainName, ((Microsoft.Protocols.TestSuites.Nrpc.DomainGuidType)(0)), ((Microsoft.Protocols.TestSuites.Nrpc.DsaGuidType)(0)), Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc);
                    this.Manager.Checkpoint("MS-NRPC_R103453");
                    this.Manager.Comment("reaching state \'S429\'");
                    this.Manager.Comment("checking step \'return DsrDeregisterDnsHostRecords/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp142, "return of DsrDeregisterDnsHostRecords, state S429");
                    Test_LocateDc_DsrDeregisterDnsHostRecordsS512();
                    goto label37;
                }
                throw new InvalidOperationException("never reached");
            label37:
;
                goto label39;
            }
            if ((temp148 == 2)) {
                this.Manager.Comment("reaching state \'S110\'");
                bool temp144;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp144);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp144);
                this.Manager.Comment("reaching state \'S191\'");
                int temp147 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_LocateDc_DsrDeregisterDnsHostRecords.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_LocateDc_DsrDeregisterDnsHostRecordsS40GetClientAccountTypeChecker4)), new ExpectedReturn(Test_LocateDc_DsrDeregisterDnsHostRecords.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_LocateDc_DsrDeregisterDnsHostRecordsS40GetClientAccountTypeChecker5)));
                if ((temp147 == 0)) {
                    this.Manager.Comment("reaching state \'S291\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp145;
                    this.Manager.Comment("executing step \'call DsrDeregisterDnsHostRecords(PrimaryDc,InvalidDomainName,Null" +
                            ",DsaGuidOne,PrimaryDc)\'");
                    temp145 = this.INrpcServerAdapterInstance.DsrDeregisterDnsHostRecords(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.InvalidDomainName, ((Microsoft.Protocols.TestSuites.Nrpc.DomainGuidType)(0)), ((Microsoft.Protocols.TestSuites.Nrpc.DsaGuidType)(0)), Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc);
                    this.Manager.Checkpoint("MS-NRPC_R103453");
                    this.Manager.Comment("reaching state \'S430\'");
                    this.Manager.Comment("checking step \'return DsrDeregisterDnsHostRecords/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp145, "return of DsrDeregisterDnsHostRecords, state S430");
                    this.Manager.Comment("reaching state \'S533\'");
                    goto label38;
                }
                if ((temp147 == 1)) {
                    this.Manager.Comment("reaching state \'S292\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp146;
                    this.Manager.Comment("executing step \'call DsrDeregisterDnsHostRecords(PrimaryDc,InvalidDomainName,Null" +
                            ",DsaGuidOne,PrimaryDc)\'");
                    temp146 = this.INrpcServerAdapterInstance.DsrDeregisterDnsHostRecords(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.InvalidDomainName, ((Microsoft.Protocols.TestSuites.Nrpc.DomainGuidType)(0)), ((Microsoft.Protocols.TestSuites.Nrpc.DsaGuidType)(0)), Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc);
                    this.Manager.Checkpoint("MS-NRPC_R103453");
                    this.Manager.Comment("reaching state \'S431\'");
                    this.Manager.Comment("checking step \'return DsrDeregisterDnsHostRecords/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp146, "return of DsrDeregisterDnsHostRecords, state S431");
                    this.Manager.Comment("reaching state \'S534\'");
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
        
        private void Test_LocateDc_DsrDeregisterDnsHostRecordsS40GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S41");
        }
        
        private void Test_LocateDc_DsrDeregisterDnsHostRecordsS40GetClientAccountTypeChecker(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S189");
        }
        
        private void Test_LocateDc_DsrDeregisterDnsHostRecordsS40GetClientAccountTypeChecker1(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S189");
        }
        
        private void Test_LocateDc_DsrDeregisterDnsHostRecordsS40GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S41");
        }
        
        private void Test_LocateDc_DsrDeregisterDnsHostRecordsS40GetClientAccountTypeChecker2(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S190");
        }
        
        private void Test_LocateDc_DsrDeregisterDnsHostRecordsS40GetClientAccountTypeChecker3(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S190");
        }
        
        private void Test_LocateDc_DsrDeregisterDnsHostRecordsS40GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S41");
        }
        
        private void Test_LocateDc_DsrDeregisterDnsHostRecordsS40GetClientAccountTypeChecker4(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S191");
        }
        
        private void Test_LocateDc_DsrDeregisterDnsHostRecordsS40GetClientAccountTypeChecker5(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S191");
        }
        #endregion
        
        #region Test Starting in S42
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_LocateDc_DsrDeregisterDnsHostRecordsS42() {
            this.Manager.BeginTest("Test_LocateDc_DsrDeregisterDnsHostRecordsS42");
            this.Manager.Comment("reaching state \'S42\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp149;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp149);
            this.Manager.AddReturn(GetPlatformInfo, null, temp149);
            this.Manager.Comment("reaching state \'S43\'");
            int temp162 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_LocateDc_DsrDeregisterDnsHostRecords.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_DsrDeregisterDnsHostRecordsS42GetPlatformChecker)), new ExpectedReturn(Test_LocateDc_DsrDeregisterDnsHostRecords.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_DsrDeregisterDnsHostRecordsS42GetPlatformChecker1)), new ExpectedReturn(Test_LocateDc_DsrDeregisterDnsHostRecords.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_DsrDeregisterDnsHostRecordsS42GetPlatformChecker2)));
            if ((temp162 == 0)) {
                this.Manager.Comment("reaching state \'S111\'");
                bool temp150;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp150);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp150);
                this.Manager.Comment("reaching state \'S192\'");
                int temp153 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_LocateDc_DsrDeregisterDnsHostRecords.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_LocateDc_DsrDeregisterDnsHostRecordsS42GetClientAccountTypeChecker)), new ExpectedReturn(Test_LocateDc_DsrDeregisterDnsHostRecords.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_LocateDc_DsrDeregisterDnsHostRecordsS42GetClientAccountTypeChecker1)));
                if ((temp153 == 0)) {
                    this.Manager.Comment("reaching state \'S293\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp151;
                    this.Manager.Comment("executing step \'call DsrDeregisterDnsHostRecords(PrimaryDc,FqdnFormatDomainName,I" +
                            "nvalidDomainGuid,DsaGuidOne,PrimaryDc)\'");
                    temp151 = this.INrpcServerAdapterInstance.DsrDeregisterDnsHostRecords(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.FqdnFormatDomainName, ((Microsoft.Protocols.TestSuites.Nrpc.DomainGuidType)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.DsaGuidType)(0)), Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc);
                    this.Manager.Checkpoint("MS-NRPC_R103453");
                    this.Manager.Comment("reaching state \'S432\'");
                    this.Manager.Comment("checking step \'return DsrDeregisterDnsHostRecords/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp151, "return of DsrDeregisterDnsHostRecords, state S432");
                    Test_LocateDc_DsrDeregisterDnsHostRecordsS515();
                    goto label40;
                }
                if ((temp153 == 1)) {
                    this.Manager.Comment("reaching state \'S294\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp152;
                    this.Manager.Comment("executing step \'call DsrDeregisterDnsHostRecords(PrimaryDc,FqdnFormatDomainName,I" +
                            "nvalidDomainGuid,DsaGuidOne,PrimaryDc)\'");
                    temp152 = this.INrpcServerAdapterInstance.DsrDeregisterDnsHostRecords(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.FqdnFormatDomainName, ((Microsoft.Protocols.TestSuites.Nrpc.DomainGuidType)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.DsaGuidType)(0)), Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc);
                    this.Manager.Checkpoint("MS-NRPC_R103453");
                    this.Manager.Comment("reaching state \'S433\'");
                    this.Manager.Comment("checking step \'return DsrDeregisterDnsHostRecords/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp152, "return of DsrDeregisterDnsHostRecords, state S433");
                    Test_LocateDc_DsrDeregisterDnsHostRecordsS514();
                    goto label40;
                }
                throw new InvalidOperationException("never reached");
            label40:
;
                goto label43;
            }
            if ((temp162 == 1)) {
                this.Manager.Comment("reaching state \'S112\'");
                bool temp154;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp154);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp154);
                this.Manager.Comment("reaching state \'S193\'");
                int temp157 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_LocateDc_DsrDeregisterDnsHostRecords.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_LocateDc_DsrDeregisterDnsHostRecordsS42GetClientAccountTypeChecker2)), new ExpectedReturn(Test_LocateDc_DsrDeregisterDnsHostRecords.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_LocateDc_DsrDeregisterDnsHostRecordsS42GetClientAccountTypeChecker3)));
                if ((temp157 == 0)) {
                    this.Manager.Comment("reaching state \'S295\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp155;
                    this.Manager.Comment("executing step \'call DsrDeregisterDnsHostRecords(PrimaryDc,FqdnFormatDomainName,I" +
                            "nvalidDomainGuid,DsaGuidOne,PrimaryDc)\'");
                    temp155 = this.INrpcServerAdapterInstance.DsrDeregisterDnsHostRecords(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.FqdnFormatDomainName, ((Microsoft.Protocols.TestSuites.Nrpc.DomainGuidType)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.DsaGuidType)(0)), Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc);
                    this.Manager.Checkpoint("MS-NRPC_R103453");
                    this.Manager.Comment("reaching state \'S434\'");
                    this.Manager.Comment("checking step \'return DsrDeregisterDnsHostRecords/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp155, "return of DsrDeregisterDnsHostRecords, state S434");
                    Test_LocateDc_DsrDeregisterDnsHostRecordsS513();
                    goto label41;
                }
                if ((temp157 == 1)) {
                    this.Manager.Comment("reaching state \'S296\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp156;
                    this.Manager.Comment("executing step \'call DsrDeregisterDnsHostRecords(PrimaryDc,FqdnFormatDomainName,I" +
                            "nvalidDomainGuid,DsaGuidOne,PrimaryDc)\'");
                    temp156 = this.INrpcServerAdapterInstance.DsrDeregisterDnsHostRecords(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.FqdnFormatDomainName, ((Microsoft.Protocols.TestSuites.Nrpc.DomainGuidType)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.DsaGuidType)(0)), Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc);
                    this.Manager.Checkpoint("MS-NRPC_R103453");
                    this.Manager.Comment("reaching state \'S435\'");
                    this.Manager.Comment("checking step \'return DsrDeregisterDnsHostRecords/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp156, "return of DsrDeregisterDnsHostRecords, state S435");
                    Test_LocateDc_DsrDeregisterDnsHostRecordsS512();
                    goto label41;
                }
                throw new InvalidOperationException("never reached");
            label41:
;
                goto label43;
            }
            if ((temp162 == 2)) {
                this.Manager.Comment("reaching state \'S113\'");
                bool temp158;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp158);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp158);
                this.Manager.Comment("reaching state \'S194\'");
                int temp161 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_LocateDc_DsrDeregisterDnsHostRecords.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_LocateDc_DsrDeregisterDnsHostRecordsS42GetClientAccountTypeChecker4)), new ExpectedReturn(Test_LocateDc_DsrDeregisterDnsHostRecords.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_LocateDc_DsrDeregisterDnsHostRecordsS42GetClientAccountTypeChecker5)));
                if ((temp161 == 0)) {
                    this.Manager.Comment("reaching state \'S297\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp159;
                    this.Manager.Comment("executing step \'call DsrDeregisterDnsHostRecords(PrimaryDc,InvalidDomainName,Null" +
                            ",DsaGuidOne,PrimaryDc)\'");
                    temp159 = this.INrpcServerAdapterInstance.DsrDeregisterDnsHostRecords(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.InvalidDomainName, ((Microsoft.Protocols.TestSuites.Nrpc.DomainGuidType)(0)), ((Microsoft.Protocols.TestSuites.Nrpc.DsaGuidType)(0)), Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc);
                    this.Manager.Checkpoint("MS-NRPC_R103453");
                    this.Manager.Comment("reaching state \'S436\'");
                    this.Manager.Comment("checking step \'return DsrDeregisterDnsHostRecords/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp159, "return of DsrDeregisterDnsHostRecords, state S436");
                    this.Manager.Comment("reaching state \'S535\'");
                    goto label42;
                }
                if ((temp161 == 1)) {
                    this.Manager.Comment("reaching state \'S298\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp160;
                    this.Manager.Comment("executing step \'call DsrDeregisterDnsHostRecords(PrimaryDc,InvalidDomainName,Null" +
                            ",DsaGuidOne,PrimaryDc)\'");
                    temp160 = this.INrpcServerAdapterInstance.DsrDeregisterDnsHostRecords(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.InvalidDomainName, ((Microsoft.Protocols.TestSuites.Nrpc.DomainGuidType)(0)), ((Microsoft.Protocols.TestSuites.Nrpc.DsaGuidType)(0)), Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc);
                    this.Manager.Checkpoint("MS-NRPC_R103453");
                    this.Manager.Comment("reaching state \'S437\'");
                    this.Manager.Comment("checking step \'return DsrDeregisterDnsHostRecords/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp160, "return of DsrDeregisterDnsHostRecords, state S437");
                    this.Manager.Comment("reaching state \'S536\'");
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
        
        private void Test_LocateDc_DsrDeregisterDnsHostRecordsS42GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S43");
        }
        
        private void Test_LocateDc_DsrDeregisterDnsHostRecordsS42GetClientAccountTypeChecker(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S192");
        }
        
        private void Test_LocateDc_DsrDeregisterDnsHostRecordsS42GetClientAccountTypeChecker1(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S192");
        }
        
        private void Test_LocateDc_DsrDeregisterDnsHostRecordsS42GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S43");
        }
        
        private void Test_LocateDc_DsrDeregisterDnsHostRecordsS42GetClientAccountTypeChecker2(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S193");
        }
        
        private void Test_LocateDc_DsrDeregisterDnsHostRecordsS42GetClientAccountTypeChecker3(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S193");
        }
        
        private void Test_LocateDc_DsrDeregisterDnsHostRecordsS42GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S43");
        }
        
        private void Test_LocateDc_DsrDeregisterDnsHostRecordsS42GetClientAccountTypeChecker4(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S194");
        }
        
        private void Test_LocateDc_DsrDeregisterDnsHostRecordsS42GetClientAccountTypeChecker5(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S194");
        }
        #endregion
        
        #region Test Starting in S44
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_LocateDc_DsrDeregisterDnsHostRecordsS44() {
            this.Manager.BeginTest("Test_LocateDc_DsrDeregisterDnsHostRecordsS44");
            this.Manager.Comment("reaching state \'S44\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp163;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp163);
            this.Manager.AddReturn(GetPlatformInfo, null, temp163);
            this.Manager.Comment("reaching state \'S45\'");
            int temp176 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_LocateDc_DsrDeregisterDnsHostRecords.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_DsrDeregisterDnsHostRecordsS44GetPlatformChecker)), new ExpectedReturn(Test_LocateDc_DsrDeregisterDnsHostRecords.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_DsrDeregisterDnsHostRecordsS44GetPlatformChecker1)), new ExpectedReturn(Test_LocateDc_DsrDeregisterDnsHostRecords.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_DsrDeregisterDnsHostRecordsS44GetPlatformChecker2)));
            if ((temp176 == 0)) {
                this.Manager.Comment("reaching state \'S114\'");
                bool temp164;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp164);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp164);
                this.Manager.Comment("reaching state \'S195\'");
                int temp167 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_LocateDc_DsrDeregisterDnsHostRecords.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_LocateDc_DsrDeregisterDnsHostRecordsS44GetClientAccountTypeChecker)), new ExpectedReturn(Test_LocateDc_DsrDeregisterDnsHostRecords.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_LocateDc_DsrDeregisterDnsHostRecordsS44GetClientAccountTypeChecker1)));
                if ((temp167 == 0)) {
                    this.Manager.Comment("reaching state \'S299\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp165;
                    this.Manager.Comment("executing step \'call DsrDeregisterDnsHostRecords(PrimaryDc,FqdnFormatDomainName,P" +
                            "rimaryDomainGuid,DsaGuidOne,PrimaryDc)\'");
                    temp165 = this.INrpcServerAdapterInstance.DsrDeregisterDnsHostRecords(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.FqdnFormatDomainName, Microsoft.Protocols.TestSuites.Nrpc.DomainGuidType.PrimaryDomainGuid, ((Microsoft.Protocols.TestSuites.Nrpc.DsaGuidType)(0)), Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc);
                    this.Manager.Checkpoint("MS-NRPC_R103453");
                    this.Manager.Comment("reaching state \'S438\'");
                    this.Manager.Comment("checking step \'return DsrDeregisterDnsHostRecords/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp165, "return of DsrDeregisterDnsHostRecords, state S438");
                    Test_LocateDc_DsrDeregisterDnsHostRecordsS515();
                    goto label44;
                }
                if ((temp167 == 1)) {
                    this.Manager.Comment("reaching state \'S300\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp166;
                    this.Manager.Comment("executing step \'call DsrDeregisterDnsHostRecords(PrimaryDc,FqdnFormatDomainName,P" +
                            "rimaryDomainGuid,DsaGuidOne,PrimaryDc)\'");
                    temp166 = this.INrpcServerAdapterInstance.DsrDeregisterDnsHostRecords(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.FqdnFormatDomainName, Microsoft.Protocols.TestSuites.Nrpc.DomainGuidType.PrimaryDomainGuid, ((Microsoft.Protocols.TestSuites.Nrpc.DsaGuidType)(0)), Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc);
                    this.Manager.Checkpoint("MS-NRPC_R103453");
                    this.Manager.Comment("reaching state \'S439\'");
                    this.Manager.Comment("checking step \'return DsrDeregisterDnsHostRecords/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp166, "return of DsrDeregisterDnsHostRecords, state S439");
                    Test_LocateDc_DsrDeregisterDnsHostRecordsS514();
                    goto label44;
                }
                throw new InvalidOperationException("never reached");
            label44:
;
                goto label47;
            }
            if ((temp176 == 1)) {
                this.Manager.Comment("reaching state \'S115\'");
                bool temp168;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp168);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp168);
                this.Manager.Comment("reaching state \'S196\'");
                int temp171 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_LocateDc_DsrDeregisterDnsHostRecords.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_LocateDc_DsrDeregisterDnsHostRecordsS44GetClientAccountTypeChecker2)), new ExpectedReturn(Test_LocateDc_DsrDeregisterDnsHostRecords.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_LocateDc_DsrDeregisterDnsHostRecordsS44GetClientAccountTypeChecker3)));
                if ((temp171 == 0)) {
                    this.Manager.Comment("reaching state \'S301\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp169;
                    this.Manager.Comment("executing step \'call DsrDeregisterDnsHostRecords(PrimaryDc,FqdnFormatDomainName,P" +
                            "rimaryDomainGuid,DsaGuidOne,PrimaryDc)\'");
                    temp169 = this.INrpcServerAdapterInstance.DsrDeregisterDnsHostRecords(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.FqdnFormatDomainName, Microsoft.Protocols.TestSuites.Nrpc.DomainGuidType.PrimaryDomainGuid, ((Microsoft.Protocols.TestSuites.Nrpc.DsaGuidType)(0)), Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc);
                    this.Manager.Checkpoint("MS-NRPC_R103453");
                    this.Manager.Comment("reaching state \'S440\'");
                    this.Manager.Comment("checking step \'return DsrDeregisterDnsHostRecords/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp169, "return of DsrDeregisterDnsHostRecords, state S440");
                    Test_LocateDc_DsrDeregisterDnsHostRecordsS513();
                    goto label45;
                }
                if ((temp171 == 1)) {
                    this.Manager.Comment("reaching state \'S302\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp170;
                    this.Manager.Comment("executing step \'call DsrDeregisterDnsHostRecords(PrimaryDc,FqdnFormatDomainName,P" +
                            "rimaryDomainGuid,DsaGuidOne,PrimaryDc)\'");
                    temp170 = this.INrpcServerAdapterInstance.DsrDeregisterDnsHostRecords(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.FqdnFormatDomainName, Microsoft.Protocols.TestSuites.Nrpc.DomainGuidType.PrimaryDomainGuid, ((Microsoft.Protocols.TestSuites.Nrpc.DsaGuidType)(0)), Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc);
                    this.Manager.Checkpoint("MS-NRPC_R103453");
                    this.Manager.Comment("reaching state \'S441\'");
                    this.Manager.Comment("checking step \'return DsrDeregisterDnsHostRecords/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp170, "return of DsrDeregisterDnsHostRecords, state S441");
                    Test_LocateDc_DsrDeregisterDnsHostRecordsS512();
                    goto label45;
                }
                throw new InvalidOperationException("never reached");
            label45:
;
                goto label47;
            }
            if ((temp176 == 2)) {
                this.Manager.Comment("reaching state \'S116\'");
                bool temp172;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp172);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp172);
                this.Manager.Comment("reaching state \'S197\'");
                int temp175 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_LocateDc_DsrDeregisterDnsHostRecords.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_LocateDc_DsrDeregisterDnsHostRecordsS44GetClientAccountTypeChecker4)), new ExpectedReturn(Test_LocateDc_DsrDeregisterDnsHostRecords.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_LocateDc_DsrDeregisterDnsHostRecordsS44GetClientAccountTypeChecker5)));
                if ((temp175 == 0)) {
                    this.Manager.Comment("reaching state \'S303\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp173;
                    this.Manager.Comment("executing step \'call DsrDeregisterDnsHostRecords(PrimaryDc,InvalidDomainName,Null" +
                            ",DsaGuidOne,PrimaryDc)\'");
                    temp173 = this.INrpcServerAdapterInstance.DsrDeregisterDnsHostRecords(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.InvalidDomainName, ((Microsoft.Protocols.TestSuites.Nrpc.DomainGuidType)(0)), ((Microsoft.Protocols.TestSuites.Nrpc.DsaGuidType)(0)), Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc);
                    this.Manager.Checkpoint("MS-NRPC_R103453");
                    this.Manager.Comment("reaching state \'S442\'");
                    this.Manager.Comment("checking step \'return DsrDeregisterDnsHostRecords/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp173, "return of DsrDeregisterDnsHostRecords, state S442");
                    this.Manager.Comment("reaching state \'S537\'");
                    goto label46;
                }
                if ((temp175 == 1)) {
                    this.Manager.Comment("reaching state \'S304\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp174;
                    this.Manager.Comment("executing step \'call DsrDeregisterDnsHostRecords(PrimaryDc,InvalidDomainName,Null" +
                            ",DsaGuidOne,PrimaryDc)\'");
                    temp174 = this.INrpcServerAdapterInstance.DsrDeregisterDnsHostRecords(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.InvalidDomainName, ((Microsoft.Protocols.TestSuites.Nrpc.DomainGuidType)(0)), ((Microsoft.Protocols.TestSuites.Nrpc.DsaGuidType)(0)), Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc);
                    this.Manager.Checkpoint("MS-NRPC_R103453");
                    this.Manager.Comment("reaching state \'S443\'");
                    this.Manager.Comment("checking step \'return DsrDeregisterDnsHostRecords/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp174, "return of DsrDeregisterDnsHostRecords, state S443");
                    this.Manager.Comment("reaching state \'S538\'");
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
        
        private void Test_LocateDc_DsrDeregisterDnsHostRecordsS44GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S45");
        }
        
        private void Test_LocateDc_DsrDeregisterDnsHostRecordsS44GetClientAccountTypeChecker(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S195");
        }
        
        private void Test_LocateDc_DsrDeregisterDnsHostRecordsS44GetClientAccountTypeChecker1(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S195");
        }
        
        private void Test_LocateDc_DsrDeregisterDnsHostRecordsS44GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S45");
        }
        
        private void Test_LocateDc_DsrDeregisterDnsHostRecordsS44GetClientAccountTypeChecker2(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S196");
        }
        
        private void Test_LocateDc_DsrDeregisterDnsHostRecordsS44GetClientAccountTypeChecker3(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S196");
        }
        
        private void Test_LocateDc_DsrDeregisterDnsHostRecordsS44GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S45");
        }
        
        private void Test_LocateDc_DsrDeregisterDnsHostRecordsS44GetClientAccountTypeChecker4(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S197");
        }
        
        private void Test_LocateDc_DsrDeregisterDnsHostRecordsS44GetClientAccountTypeChecker5(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S197");
        }
        #endregion
        
        #region Test Starting in S46
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_LocateDc_DsrDeregisterDnsHostRecordsS46() {
            this.Manager.BeginTest("Test_LocateDc_DsrDeregisterDnsHostRecordsS46");
            this.Manager.Comment("reaching state \'S46\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp177;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp177);
            this.Manager.AddReturn(GetPlatformInfo, null, temp177);
            this.Manager.Comment("reaching state \'S47\'");
            int temp190 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_LocateDc_DsrDeregisterDnsHostRecords.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_DsrDeregisterDnsHostRecordsS46GetPlatformChecker)), new ExpectedReturn(Test_LocateDc_DsrDeregisterDnsHostRecords.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_DsrDeregisterDnsHostRecordsS46GetPlatformChecker1)), new ExpectedReturn(Test_LocateDc_DsrDeregisterDnsHostRecords.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_DsrDeregisterDnsHostRecordsS46GetPlatformChecker2)));
            if ((temp190 == 0)) {
                this.Manager.Comment("reaching state \'S117\'");
                bool temp178;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp178);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp178);
                this.Manager.Comment("reaching state \'S198\'");
                int temp181 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_LocateDc_DsrDeregisterDnsHostRecords.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_LocateDc_DsrDeregisterDnsHostRecordsS46GetClientAccountTypeChecker)), new ExpectedReturn(Test_LocateDc_DsrDeregisterDnsHostRecords.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_LocateDc_DsrDeregisterDnsHostRecordsS46GetClientAccountTypeChecker1)));
                if ((temp181 == 0)) {
                    this.Manager.Comment("reaching state \'S305\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp179;
                    this.Manager.Comment("executing step \'call DsrDeregisterDnsHostRecords(PrimaryDc,InvalidDomainName,Null" +
                            ",DsaGuidOne,PrimaryDc)\'");
                    temp179 = this.INrpcServerAdapterInstance.DsrDeregisterDnsHostRecords(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.InvalidDomainName, ((Microsoft.Protocols.TestSuites.Nrpc.DomainGuidType)(0)), ((Microsoft.Protocols.TestSuites.Nrpc.DsaGuidType)(0)), Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc);
                    this.Manager.Checkpoint("MS-NRPC_R103453");
                    this.Manager.Comment("reaching state \'S444\'");
                    this.Manager.Comment("checking step \'return DsrDeregisterDnsHostRecords/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp179, "return of DsrDeregisterDnsHostRecords, state S444");
                    Test_LocateDc_DsrDeregisterDnsHostRecordsS511();
                    goto label48;
                }
                if ((temp181 == 1)) {
                    this.Manager.Comment("reaching state \'S306\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp180;
                    this.Manager.Comment("executing step \'call DsrDeregisterDnsHostRecords(PrimaryDc,InvalidDomainName,Null" +
                            ",DsaGuidOne,PrimaryDc)\'");
                    temp180 = this.INrpcServerAdapterInstance.DsrDeregisterDnsHostRecords(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.InvalidDomainName, ((Microsoft.Protocols.TestSuites.Nrpc.DomainGuidType)(0)), ((Microsoft.Protocols.TestSuites.Nrpc.DsaGuidType)(0)), Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc);
                    this.Manager.Checkpoint("MS-NRPC_R103453");
                    this.Manager.Comment("reaching state \'S445\'");
                    this.Manager.Comment("checking step \'return DsrDeregisterDnsHostRecords/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp180, "return of DsrDeregisterDnsHostRecords, state S445");
                    Test_LocateDc_DsrDeregisterDnsHostRecordsS510();
                    goto label48;
                }
                throw new InvalidOperationException("never reached");
            label48:
;
                goto label51;
            }
            if ((temp190 == 1)) {
                this.Manager.Comment("reaching state \'S118\'");
                bool temp182;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp182);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp182);
                this.Manager.Comment("reaching state \'S199\'");
                int temp185 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_LocateDc_DsrDeregisterDnsHostRecords.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_LocateDc_DsrDeregisterDnsHostRecordsS46GetClientAccountTypeChecker2)), new ExpectedReturn(Test_LocateDc_DsrDeregisterDnsHostRecords.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_LocateDc_DsrDeregisterDnsHostRecordsS46GetClientAccountTypeChecker3)));
                if ((temp185 == 0)) {
                    this.Manager.Comment("reaching state \'S307\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp183;
                    this.Manager.Comment("executing step \'call DsrDeregisterDnsHostRecords(PrimaryDc,InvalidDomainName,Null" +
                            ",DsaGuidOne,PrimaryDc)\'");
                    temp183 = this.INrpcServerAdapterInstance.DsrDeregisterDnsHostRecords(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.InvalidDomainName, ((Microsoft.Protocols.TestSuites.Nrpc.DomainGuidType)(0)), ((Microsoft.Protocols.TestSuites.Nrpc.DsaGuidType)(0)), Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc);
                    this.Manager.Checkpoint("MS-NRPC_R103453");
                    this.Manager.Comment("reaching state \'S446\'");
                    this.Manager.Comment("checking step \'return DsrDeregisterDnsHostRecords/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp183, "return of DsrDeregisterDnsHostRecords, state S446");
                    this.Manager.Comment("reaching state \'S539\'");
                    goto label49;
                }
                if ((temp185 == 1)) {
                    this.Manager.Comment("reaching state \'S308\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp184;
                    this.Manager.Comment("executing step \'call DsrDeregisterDnsHostRecords(PrimaryDc,InvalidDomainName,Null" +
                            ",DsaGuidOne,PrimaryDc)\'");
                    temp184 = this.INrpcServerAdapterInstance.DsrDeregisterDnsHostRecords(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.InvalidDomainName, ((Microsoft.Protocols.TestSuites.Nrpc.DomainGuidType)(0)), ((Microsoft.Protocols.TestSuites.Nrpc.DsaGuidType)(0)), Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc);
                    this.Manager.Checkpoint("MS-NRPC_R103453");
                    this.Manager.Comment("reaching state \'S447\'");
                    this.Manager.Comment("checking step \'return DsrDeregisterDnsHostRecords/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp184, "return of DsrDeregisterDnsHostRecords, state S447");
                    this.Manager.Comment("reaching state \'S540\'");
                    goto label49;
                }
                throw new InvalidOperationException("never reached");
            label49:
;
                goto label51;
            }
            if ((temp190 == 2)) {
                this.Manager.Comment("reaching state \'S119\'");
                bool temp186;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp186);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp186);
                this.Manager.Comment("reaching state \'S200\'");
                int temp189 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_LocateDc_DsrDeregisterDnsHostRecords.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_LocateDc_DsrDeregisterDnsHostRecordsS46GetClientAccountTypeChecker4)), new ExpectedReturn(Test_LocateDc_DsrDeregisterDnsHostRecords.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_LocateDc_DsrDeregisterDnsHostRecordsS46GetClientAccountTypeChecker5)));
                if ((temp189 == 0)) {
                    this.Manager.Comment("reaching state \'S309\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp187;
                    this.Manager.Comment("executing step \'call DsrDeregisterDnsHostRecords(PrimaryDc,InvalidDomainName,Null" +
                            ",DsaGuidOne,PrimaryDc)\'");
                    temp187 = this.INrpcServerAdapterInstance.DsrDeregisterDnsHostRecords(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.InvalidDomainName, ((Microsoft.Protocols.TestSuites.Nrpc.DomainGuidType)(0)), ((Microsoft.Protocols.TestSuites.Nrpc.DsaGuidType)(0)), Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc);
                    this.Manager.Checkpoint("MS-NRPC_R103453");
                    this.Manager.Comment("reaching state \'S448\'");
                    this.Manager.Comment("checking step \'return DsrDeregisterDnsHostRecords/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp187, "return of DsrDeregisterDnsHostRecords, state S448");
                    this.Manager.Comment("reaching state \'S541\'");
                    goto label50;
                }
                if ((temp189 == 1)) {
                    this.Manager.Comment("reaching state \'S310\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp188;
                    this.Manager.Comment("executing step \'call DsrDeregisterDnsHostRecords(PrimaryDc,InvalidDomainName,Null" +
                            ",DsaGuidOne,PrimaryDc)\'");
                    temp188 = this.INrpcServerAdapterInstance.DsrDeregisterDnsHostRecords(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.InvalidDomainName, ((Microsoft.Protocols.TestSuites.Nrpc.DomainGuidType)(0)), ((Microsoft.Protocols.TestSuites.Nrpc.DsaGuidType)(0)), Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc);
                    this.Manager.Checkpoint("MS-NRPC_R103453");
                    this.Manager.Comment("reaching state \'S449\'");
                    this.Manager.Comment("checking step \'return DsrDeregisterDnsHostRecords/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp188, "return of DsrDeregisterDnsHostRecords, state S449");
                    this.Manager.Comment("reaching state \'S542\'");
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
        
        private void Test_LocateDc_DsrDeregisterDnsHostRecordsS46GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S47");
        }
        
        private void Test_LocateDc_DsrDeregisterDnsHostRecordsS46GetClientAccountTypeChecker(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S198");
        }
        
        private void Test_LocateDc_DsrDeregisterDnsHostRecordsS46GetClientAccountTypeChecker1(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S198");
        }
        
        private void Test_LocateDc_DsrDeregisterDnsHostRecordsS46GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S47");
        }
        
        private void Test_LocateDc_DsrDeregisterDnsHostRecordsS46GetClientAccountTypeChecker2(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S199");
        }
        
        private void Test_LocateDc_DsrDeregisterDnsHostRecordsS46GetClientAccountTypeChecker3(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S199");
        }
        
        private void Test_LocateDc_DsrDeregisterDnsHostRecordsS46GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S47");
        }
        
        private void Test_LocateDc_DsrDeregisterDnsHostRecordsS46GetClientAccountTypeChecker4(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S200");
        }
        
        private void Test_LocateDc_DsrDeregisterDnsHostRecordsS46GetClientAccountTypeChecker5(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S200");
        }
        #endregion
        
        #region Test Starting in S48
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_LocateDc_DsrDeregisterDnsHostRecordsS48() {
            this.Manager.BeginTest("Test_LocateDc_DsrDeregisterDnsHostRecordsS48");
            this.Manager.Comment("reaching state \'S48\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp191;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp191);
            this.Manager.AddReturn(GetPlatformInfo, null, temp191);
            this.Manager.Comment("reaching state \'S49\'");
            int temp204 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_LocateDc_DsrDeregisterDnsHostRecords.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_DsrDeregisterDnsHostRecordsS48GetPlatformChecker)), new ExpectedReturn(Test_LocateDc_DsrDeregisterDnsHostRecords.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_DsrDeregisterDnsHostRecordsS48GetPlatformChecker1)), new ExpectedReturn(Test_LocateDc_DsrDeregisterDnsHostRecords.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_DsrDeregisterDnsHostRecordsS48GetPlatformChecker2)));
            if ((temp204 == 0)) {
                this.Manager.Comment("reaching state \'S120\'");
                bool temp192;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp192);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp192);
                this.Manager.Comment("reaching state \'S201\'");
                int temp195 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_LocateDc_DsrDeregisterDnsHostRecords.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_LocateDc_DsrDeregisterDnsHostRecordsS48GetClientAccountTypeChecker)), new ExpectedReturn(Test_LocateDc_DsrDeregisterDnsHostRecords.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_LocateDc_DsrDeregisterDnsHostRecordsS48GetClientAccountTypeChecker1)));
                if ((temp195 == 0)) {
                    this.Manager.Comment("reaching state \'S311\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp193;
                    this.Manager.Comment("executing step \'call DsrDeregisterDnsHostRecords(PrimaryDc,InvalidDomainName,Inva" +
                            "lidDomainGuid,DsaGuidOne,PrimaryDc)\'");
                    temp193 = this.INrpcServerAdapterInstance.DsrDeregisterDnsHostRecords(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.InvalidDomainName, ((Microsoft.Protocols.TestSuites.Nrpc.DomainGuidType)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.DsaGuidType)(0)), Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc);
                    this.Manager.Checkpoint("MS-NRPC_R103453");
                    this.Manager.Comment("reaching state \'S450\'");
                    this.Manager.Comment("checking step \'return DsrDeregisterDnsHostRecords/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp193, "return of DsrDeregisterDnsHostRecords, state S450");
                    Test_LocateDc_DsrDeregisterDnsHostRecordsS511();
                    goto label52;
                }
                if ((temp195 == 1)) {
                    this.Manager.Comment("reaching state \'S312\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp194;
                    this.Manager.Comment("executing step \'call DsrDeregisterDnsHostRecords(PrimaryDc,InvalidDomainName,Inva" +
                            "lidDomainGuid,DsaGuidOne,PrimaryDc)\'");
                    temp194 = this.INrpcServerAdapterInstance.DsrDeregisterDnsHostRecords(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.InvalidDomainName, ((Microsoft.Protocols.TestSuites.Nrpc.DomainGuidType)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.DsaGuidType)(0)), Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc);
                    this.Manager.Checkpoint("MS-NRPC_R103453");
                    this.Manager.Comment("reaching state \'S451\'");
                    this.Manager.Comment("checking step \'return DsrDeregisterDnsHostRecords/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp194, "return of DsrDeregisterDnsHostRecords, state S451");
                    Test_LocateDc_DsrDeregisterDnsHostRecordsS510();
                    goto label52;
                }
                throw new InvalidOperationException("never reached");
            label52:
;
                goto label55;
            }
            if ((temp204 == 1)) {
                this.Manager.Comment("reaching state \'S121\'");
                bool temp196;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp196);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp196);
                this.Manager.Comment("reaching state \'S202\'");
                int temp199 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_LocateDc_DsrDeregisterDnsHostRecords.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_LocateDc_DsrDeregisterDnsHostRecordsS48GetClientAccountTypeChecker2)), new ExpectedReturn(Test_LocateDc_DsrDeregisterDnsHostRecords.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_LocateDc_DsrDeregisterDnsHostRecordsS48GetClientAccountTypeChecker3)));
                if ((temp199 == 0)) {
                    this.Manager.Comment("reaching state \'S313\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp197;
                    this.Manager.Comment("executing step \'call DsrDeregisterDnsHostRecords(PrimaryDc,InvalidDomainName,Null" +
                            ",DsaGuidOne,PrimaryDc)\'");
                    temp197 = this.INrpcServerAdapterInstance.DsrDeregisterDnsHostRecords(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.InvalidDomainName, ((Microsoft.Protocols.TestSuites.Nrpc.DomainGuidType)(0)), ((Microsoft.Protocols.TestSuites.Nrpc.DsaGuidType)(0)), Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc);
                    this.Manager.Checkpoint("MS-NRPC_R103453");
                    this.Manager.Comment("reaching state \'S452\'");
                    this.Manager.Comment("checking step \'return DsrDeregisterDnsHostRecords/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp197, "return of DsrDeregisterDnsHostRecords, state S452");
                    this.Manager.Comment("reaching state \'S543\'");
                    goto label53;
                }
                if ((temp199 == 1)) {
                    this.Manager.Comment("reaching state \'S314\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp198;
                    this.Manager.Comment("executing step \'call DsrDeregisterDnsHostRecords(PrimaryDc,InvalidDomainName,Null" +
                            ",DsaGuidOne,PrimaryDc)\'");
                    temp198 = this.INrpcServerAdapterInstance.DsrDeregisterDnsHostRecords(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.InvalidDomainName, ((Microsoft.Protocols.TestSuites.Nrpc.DomainGuidType)(0)), ((Microsoft.Protocols.TestSuites.Nrpc.DsaGuidType)(0)), Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc);
                    this.Manager.Checkpoint("MS-NRPC_R103453");
                    this.Manager.Comment("reaching state \'S453\'");
                    this.Manager.Comment("checking step \'return DsrDeregisterDnsHostRecords/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp198, "return of DsrDeregisterDnsHostRecords, state S453");
                    this.Manager.Comment("reaching state \'S544\'");
                    goto label53;
                }
                throw new InvalidOperationException("never reached");
            label53:
;
                goto label55;
            }
            if ((temp204 == 2)) {
                this.Manager.Comment("reaching state \'S122\'");
                bool temp200;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp200);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp200);
                this.Manager.Comment("reaching state \'S203\'");
                int temp203 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_LocateDc_DsrDeregisterDnsHostRecords.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_LocateDc_DsrDeregisterDnsHostRecordsS48GetClientAccountTypeChecker4)), new ExpectedReturn(Test_LocateDc_DsrDeregisterDnsHostRecords.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_LocateDc_DsrDeregisterDnsHostRecordsS48GetClientAccountTypeChecker5)));
                if ((temp203 == 0)) {
                    this.Manager.Comment("reaching state \'S315\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp201;
                    this.Manager.Comment("executing step \'call DsrDeregisterDnsHostRecords(PrimaryDc,InvalidDomainName,Null" +
                            ",DsaGuidOne,PrimaryDc)\'");
                    temp201 = this.INrpcServerAdapterInstance.DsrDeregisterDnsHostRecords(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.InvalidDomainName, ((Microsoft.Protocols.TestSuites.Nrpc.DomainGuidType)(0)), ((Microsoft.Protocols.TestSuites.Nrpc.DsaGuidType)(0)), Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc);
                    this.Manager.Checkpoint("MS-NRPC_R103453");
                    this.Manager.Comment("reaching state \'S454\'");
                    this.Manager.Comment("checking step \'return DsrDeregisterDnsHostRecords/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp201, "return of DsrDeregisterDnsHostRecords, state S454");
                    this.Manager.Comment("reaching state \'S545\'");
                    goto label54;
                }
                if ((temp203 == 1)) {
                    this.Manager.Comment("reaching state \'S316\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp202;
                    this.Manager.Comment("executing step \'call DsrDeregisterDnsHostRecords(PrimaryDc,InvalidDomainName,Null" +
                            ",DsaGuidOne,PrimaryDc)\'");
                    temp202 = this.INrpcServerAdapterInstance.DsrDeregisterDnsHostRecords(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.InvalidDomainName, ((Microsoft.Protocols.TestSuites.Nrpc.DomainGuidType)(0)), ((Microsoft.Protocols.TestSuites.Nrpc.DsaGuidType)(0)), Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc);
                    this.Manager.Checkpoint("MS-NRPC_R103453");
                    this.Manager.Comment("reaching state \'S455\'");
                    this.Manager.Comment("checking step \'return DsrDeregisterDnsHostRecords/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp202, "return of DsrDeregisterDnsHostRecords, state S455");
                    this.Manager.Comment("reaching state \'S546\'");
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
        
        private void Test_LocateDc_DsrDeregisterDnsHostRecordsS48GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S49");
        }
        
        private void Test_LocateDc_DsrDeregisterDnsHostRecordsS48GetClientAccountTypeChecker(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S201");
        }
        
        private void Test_LocateDc_DsrDeregisterDnsHostRecordsS48GetClientAccountTypeChecker1(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S201");
        }
        
        private void Test_LocateDc_DsrDeregisterDnsHostRecordsS48GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S49");
        }
        
        private void Test_LocateDc_DsrDeregisterDnsHostRecordsS48GetClientAccountTypeChecker2(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S202");
        }
        
        private void Test_LocateDc_DsrDeregisterDnsHostRecordsS48GetClientAccountTypeChecker3(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S202");
        }
        
        private void Test_LocateDc_DsrDeregisterDnsHostRecordsS48GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S49");
        }
        
        private void Test_LocateDc_DsrDeregisterDnsHostRecordsS48GetClientAccountTypeChecker4(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S203");
        }
        
        private void Test_LocateDc_DsrDeregisterDnsHostRecordsS48GetClientAccountTypeChecker5(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S203");
        }
        #endregion
        
        #region Test Starting in S50
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_LocateDc_DsrDeregisterDnsHostRecordsS50() {
            this.Manager.BeginTest("Test_LocateDc_DsrDeregisterDnsHostRecordsS50");
            this.Manager.Comment("reaching state \'S50\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp205;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp205);
            this.Manager.AddReturn(GetPlatformInfo, null, temp205);
            this.Manager.Comment("reaching state \'S51\'");
            int temp218 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_LocateDc_DsrDeregisterDnsHostRecords.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_DsrDeregisterDnsHostRecordsS50GetPlatformChecker)), new ExpectedReturn(Test_LocateDc_DsrDeregisterDnsHostRecords.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_DsrDeregisterDnsHostRecordsS50GetPlatformChecker1)), new ExpectedReturn(Test_LocateDc_DsrDeregisterDnsHostRecords.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_DsrDeregisterDnsHostRecordsS50GetPlatformChecker2)));
            if ((temp218 == 0)) {
                this.Manager.Comment("reaching state \'S123\'");
                bool temp206;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp206);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp206);
                this.Manager.Comment("reaching state \'S204\'");
                int temp209 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_LocateDc_DsrDeregisterDnsHostRecords.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_LocateDc_DsrDeregisterDnsHostRecordsS50GetClientAccountTypeChecker)), new ExpectedReturn(Test_LocateDc_DsrDeregisterDnsHostRecords.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_LocateDc_DsrDeregisterDnsHostRecordsS50GetClientAccountTypeChecker1)));
                if ((temp209 == 0)) {
                    this.Manager.Comment("reaching state \'S317\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp207;
                    this.Manager.Comment("executing step \'call DsrDeregisterDnsHostRecords(PrimaryDc,InvalidDomainName,Prim" +
                            "aryDomainGuid,DsaGuidOne,PrimaryDc)\'");
                    temp207 = this.INrpcServerAdapterInstance.DsrDeregisterDnsHostRecords(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.InvalidDomainName, Microsoft.Protocols.TestSuites.Nrpc.DomainGuidType.PrimaryDomainGuid, ((Microsoft.Protocols.TestSuites.Nrpc.DsaGuidType)(0)), Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc);
                    this.Manager.Checkpoint("MS-NRPC_R103453");
                    this.Manager.Comment("reaching state \'S456\'");
                    this.Manager.Comment("checking step \'return DsrDeregisterDnsHostRecords/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp207, "return of DsrDeregisterDnsHostRecords, state S456");
                    Test_LocateDc_DsrDeregisterDnsHostRecordsS511();
                    goto label56;
                }
                if ((temp209 == 1)) {
                    this.Manager.Comment("reaching state \'S318\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp208;
                    this.Manager.Comment("executing step \'call DsrDeregisterDnsHostRecords(PrimaryDc,InvalidDomainName,Prim" +
                            "aryDomainGuid,DsaGuidOne,PrimaryDc)\'");
                    temp208 = this.INrpcServerAdapterInstance.DsrDeregisterDnsHostRecords(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.InvalidDomainName, Microsoft.Protocols.TestSuites.Nrpc.DomainGuidType.PrimaryDomainGuid, ((Microsoft.Protocols.TestSuites.Nrpc.DsaGuidType)(0)), Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc);
                    this.Manager.Checkpoint("MS-NRPC_R103453");
                    this.Manager.Comment("reaching state \'S457\'");
                    this.Manager.Comment("checking step \'return DsrDeregisterDnsHostRecords/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp208, "return of DsrDeregisterDnsHostRecords, state S457");
                    Test_LocateDc_DsrDeregisterDnsHostRecordsS510();
                    goto label56;
                }
                throw new InvalidOperationException("never reached");
            label56:
;
                goto label59;
            }
            if ((temp218 == 1)) {
                this.Manager.Comment("reaching state \'S124\'");
                bool temp210;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp210);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp210);
                this.Manager.Comment("reaching state \'S205\'");
                int temp213 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_LocateDc_DsrDeregisterDnsHostRecords.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_LocateDc_DsrDeregisterDnsHostRecordsS50GetClientAccountTypeChecker2)), new ExpectedReturn(Test_LocateDc_DsrDeregisterDnsHostRecords.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_LocateDc_DsrDeregisterDnsHostRecordsS50GetClientAccountTypeChecker3)));
                if ((temp213 == 0)) {
                    this.Manager.Comment("reaching state \'S319\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp211;
                    this.Manager.Comment("executing step \'call DsrDeregisterDnsHostRecords(PrimaryDc,InvalidDomainName,Null" +
                            ",DsaGuidOne,PrimaryDc)\'");
                    temp211 = this.INrpcServerAdapterInstance.DsrDeregisterDnsHostRecords(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.InvalidDomainName, ((Microsoft.Protocols.TestSuites.Nrpc.DomainGuidType)(0)), ((Microsoft.Protocols.TestSuites.Nrpc.DsaGuidType)(0)), Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc);
                    this.Manager.Checkpoint("MS-NRPC_R103453");
                    this.Manager.Comment("reaching state \'S458\'");
                    this.Manager.Comment("checking step \'return DsrDeregisterDnsHostRecords/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp211, "return of DsrDeregisterDnsHostRecords, state S458");
                    this.Manager.Comment("reaching state \'S547\'");
                    goto label57;
                }
                if ((temp213 == 1)) {
                    this.Manager.Comment("reaching state \'S320\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp212;
                    this.Manager.Comment("executing step \'call DsrDeregisterDnsHostRecords(PrimaryDc,InvalidDomainName,Null" +
                            ",DsaGuidOne,PrimaryDc)\'");
                    temp212 = this.INrpcServerAdapterInstance.DsrDeregisterDnsHostRecords(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.InvalidDomainName, ((Microsoft.Protocols.TestSuites.Nrpc.DomainGuidType)(0)), ((Microsoft.Protocols.TestSuites.Nrpc.DsaGuidType)(0)), Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc);
                    this.Manager.Checkpoint("MS-NRPC_R103453");
                    this.Manager.Comment("reaching state \'S459\'");
                    this.Manager.Comment("checking step \'return DsrDeregisterDnsHostRecords/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp212, "return of DsrDeregisterDnsHostRecords, state S459");
                    this.Manager.Comment("reaching state \'S548\'");
                    goto label57;
                }
                throw new InvalidOperationException("never reached");
            label57:
;
                goto label59;
            }
            if ((temp218 == 2)) {
                this.Manager.Comment("reaching state \'S125\'");
                bool temp214;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp214);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp214);
                this.Manager.Comment("reaching state \'S206\'");
                int temp217 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_LocateDc_DsrDeregisterDnsHostRecords.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_LocateDc_DsrDeregisterDnsHostRecordsS50GetClientAccountTypeChecker4)), new ExpectedReturn(Test_LocateDc_DsrDeregisterDnsHostRecords.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_LocateDc_DsrDeregisterDnsHostRecordsS50GetClientAccountTypeChecker5)));
                if ((temp217 == 0)) {
                    this.Manager.Comment("reaching state \'S321\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp215;
                    this.Manager.Comment("executing step \'call DsrDeregisterDnsHostRecords(PrimaryDc,InvalidDomainName,Null" +
                            ",DsaGuidOne,PrimaryDc)\'");
                    temp215 = this.INrpcServerAdapterInstance.DsrDeregisterDnsHostRecords(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.InvalidDomainName, ((Microsoft.Protocols.TestSuites.Nrpc.DomainGuidType)(0)), ((Microsoft.Protocols.TestSuites.Nrpc.DsaGuidType)(0)), Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc);
                    this.Manager.Checkpoint("MS-NRPC_R103453");
                    this.Manager.Comment("reaching state \'S460\'");
                    this.Manager.Comment("checking step \'return DsrDeregisterDnsHostRecords/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp215, "return of DsrDeregisterDnsHostRecords, state S460");
                    this.Manager.Comment("reaching state \'S549\'");
                    goto label58;
                }
                if ((temp217 == 1)) {
                    this.Manager.Comment("reaching state \'S322\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp216;
                    this.Manager.Comment("executing step \'call DsrDeregisterDnsHostRecords(PrimaryDc,InvalidDomainName,Null" +
                            ",DsaGuidOne,PrimaryDc)\'");
                    temp216 = this.INrpcServerAdapterInstance.DsrDeregisterDnsHostRecords(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.InvalidDomainName, ((Microsoft.Protocols.TestSuites.Nrpc.DomainGuidType)(0)), ((Microsoft.Protocols.TestSuites.Nrpc.DsaGuidType)(0)), Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc);
                    this.Manager.Checkpoint("MS-NRPC_R103453");
                    this.Manager.Comment("reaching state \'S461\'");
                    this.Manager.Comment("checking step \'return DsrDeregisterDnsHostRecords/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp216, "return of DsrDeregisterDnsHostRecords, state S461");
                    this.Manager.Comment("reaching state \'S550\'");
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
        
        private void Test_LocateDc_DsrDeregisterDnsHostRecordsS50GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S51");
        }
        
        private void Test_LocateDc_DsrDeregisterDnsHostRecordsS50GetClientAccountTypeChecker(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S204");
        }
        
        private void Test_LocateDc_DsrDeregisterDnsHostRecordsS50GetClientAccountTypeChecker1(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S204");
        }
        
        private void Test_LocateDc_DsrDeregisterDnsHostRecordsS50GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S51");
        }
        
        private void Test_LocateDc_DsrDeregisterDnsHostRecordsS50GetClientAccountTypeChecker2(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S205");
        }
        
        private void Test_LocateDc_DsrDeregisterDnsHostRecordsS50GetClientAccountTypeChecker3(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S205");
        }
        
        private void Test_LocateDc_DsrDeregisterDnsHostRecordsS50GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S51");
        }
        
        private void Test_LocateDc_DsrDeregisterDnsHostRecordsS50GetClientAccountTypeChecker4(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S206");
        }
        
        private void Test_LocateDc_DsrDeregisterDnsHostRecordsS50GetClientAccountTypeChecker5(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S206");
        }
        #endregion
        
        #region Test Starting in S52
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_LocateDc_DsrDeregisterDnsHostRecordsS52() {
            this.Manager.BeginTest("Test_LocateDc_DsrDeregisterDnsHostRecordsS52");
            this.Manager.Comment("reaching state \'S52\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp219;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp219);
            this.Manager.AddReturn(GetPlatformInfo, null, temp219);
            this.Manager.Comment("reaching state \'S53\'");
            int temp232 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_LocateDc_DsrDeregisterDnsHostRecords.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_DsrDeregisterDnsHostRecordsS52GetPlatformChecker)), new ExpectedReturn(Test_LocateDc_DsrDeregisterDnsHostRecords.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_DsrDeregisterDnsHostRecordsS52GetPlatformChecker1)), new ExpectedReturn(Test_LocateDc_DsrDeregisterDnsHostRecords.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_DsrDeregisterDnsHostRecordsS52GetPlatformChecker2)));
            if ((temp232 == 0)) {
                this.Manager.Comment("reaching state \'S126\'");
                bool temp220;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp220);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp220);
                this.Manager.Comment("reaching state \'S207\'");
                int temp223 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_LocateDc_DsrDeregisterDnsHostRecords.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_LocateDc_DsrDeregisterDnsHostRecordsS52GetClientAccountTypeChecker)), new ExpectedReturn(Test_LocateDc_DsrDeregisterDnsHostRecords.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_LocateDc_DsrDeregisterDnsHostRecordsS52GetClientAccountTypeChecker1)));
                if ((temp223 == 0)) {
                    this.Manager.Comment("reaching state \'S323\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp221;
                    this.Manager.Comment("executing step \'call DsrDeregisterDnsHostRecords(PrimaryDc,InvalidDomainName,Trus" +
                            "tedDomainGuid,DsaGuidOne,PrimaryDc)\'");
                    temp221 = this.INrpcServerAdapterInstance.DsrDeregisterDnsHostRecords(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.InvalidDomainName, Microsoft.Protocols.TestSuites.Nrpc.DomainGuidType.TrustedDomainGuid, ((Microsoft.Protocols.TestSuites.Nrpc.DsaGuidType)(0)), Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc);
                    this.Manager.Checkpoint("MS-NRPC_R103453");
                    this.Manager.Comment("reaching state \'S462\'");
                    this.Manager.Comment("checking step \'return DsrDeregisterDnsHostRecords/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp221, "return of DsrDeregisterDnsHostRecords, state S462");
                    Test_LocateDc_DsrDeregisterDnsHostRecordsS511();
                    goto label60;
                }
                if ((temp223 == 1)) {
                    this.Manager.Comment("reaching state \'S324\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp222;
                    this.Manager.Comment("executing step \'call DsrDeregisterDnsHostRecords(PrimaryDc,InvalidDomainName,Trus" +
                            "tedDomainGuid,DsaGuidOne,PrimaryDc)\'");
                    temp222 = this.INrpcServerAdapterInstance.DsrDeregisterDnsHostRecords(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.InvalidDomainName, Microsoft.Protocols.TestSuites.Nrpc.DomainGuidType.TrustedDomainGuid, ((Microsoft.Protocols.TestSuites.Nrpc.DsaGuidType)(0)), Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc);
                    this.Manager.Checkpoint("MS-NRPC_R103453");
                    this.Manager.Comment("reaching state \'S463\'");
                    this.Manager.Comment("checking step \'return DsrDeregisterDnsHostRecords/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp222, "return of DsrDeregisterDnsHostRecords, state S463");
                    Test_LocateDc_DsrDeregisterDnsHostRecordsS510();
                    goto label60;
                }
                throw new InvalidOperationException("never reached");
            label60:
;
                goto label63;
            }
            if ((temp232 == 1)) {
                this.Manager.Comment("reaching state \'S127\'");
                bool temp224;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp224);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp224);
                this.Manager.Comment("reaching state \'S208\'");
                int temp227 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_LocateDc_DsrDeregisterDnsHostRecords.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_LocateDc_DsrDeregisterDnsHostRecordsS52GetClientAccountTypeChecker2)), new ExpectedReturn(Test_LocateDc_DsrDeregisterDnsHostRecords.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_LocateDc_DsrDeregisterDnsHostRecordsS52GetClientAccountTypeChecker3)));
                if ((temp227 == 0)) {
                    this.Manager.Comment("reaching state \'S325\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp225;
                    this.Manager.Comment("executing step \'call DsrDeregisterDnsHostRecords(PrimaryDc,InvalidDomainName,Null" +
                            ",DsaGuidOne,PrimaryDc)\'");
                    temp225 = this.INrpcServerAdapterInstance.DsrDeregisterDnsHostRecords(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.InvalidDomainName, ((Microsoft.Protocols.TestSuites.Nrpc.DomainGuidType)(0)), ((Microsoft.Protocols.TestSuites.Nrpc.DsaGuidType)(0)), Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc);
                    this.Manager.Checkpoint("MS-NRPC_R103453");
                    this.Manager.Comment("reaching state \'S464\'");
                    this.Manager.Comment("checking step \'return DsrDeregisterDnsHostRecords/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp225, "return of DsrDeregisterDnsHostRecords, state S464");
                    this.Manager.Comment("reaching state \'S551\'");
                    goto label61;
                }
                if ((temp227 == 1)) {
                    this.Manager.Comment("reaching state \'S326\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp226;
                    this.Manager.Comment("executing step \'call DsrDeregisterDnsHostRecords(PrimaryDc,InvalidDomainName,Null" +
                            ",DsaGuidOne,PrimaryDc)\'");
                    temp226 = this.INrpcServerAdapterInstance.DsrDeregisterDnsHostRecords(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.InvalidDomainName, ((Microsoft.Protocols.TestSuites.Nrpc.DomainGuidType)(0)), ((Microsoft.Protocols.TestSuites.Nrpc.DsaGuidType)(0)), Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc);
                    this.Manager.Checkpoint("MS-NRPC_R103453");
                    this.Manager.Comment("reaching state \'S465\'");
                    this.Manager.Comment("checking step \'return DsrDeregisterDnsHostRecords/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp226, "return of DsrDeregisterDnsHostRecords, state S465");
                    this.Manager.Comment("reaching state \'S552\'");
                    goto label61;
                }
                throw new InvalidOperationException("never reached");
            label61:
;
                goto label63;
            }
            if ((temp232 == 2)) {
                this.Manager.Comment("reaching state \'S128\'");
                bool temp228;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp228);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp228);
                this.Manager.Comment("reaching state \'S209\'");
                int temp231 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_LocateDc_DsrDeregisterDnsHostRecords.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_LocateDc_DsrDeregisterDnsHostRecordsS52GetClientAccountTypeChecker4)), new ExpectedReturn(Test_LocateDc_DsrDeregisterDnsHostRecords.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_LocateDc_DsrDeregisterDnsHostRecordsS52GetClientAccountTypeChecker5)));
                if ((temp231 == 0)) {
                    this.Manager.Comment("reaching state \'S327\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp229;
                    this.Manager.Comment("executing step \'call DsrDeregisterDnsHostRecords(PrimaryDc,InvalidDomainName,Null" +
                            ",DsaGuidOne,PrimaryDc)\'");
                    temp229 = this.INrpcServerAdapterInstance.DsrDeregisterDnsHostRecords(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.InvalidDomainName, ((Microsoft.Protocols.TestSuites.Nrpc.DomainGuidType)(0)), ((Microsoft.Protocols.TestSuites.Nrpc.DsaGuidType)(0)), Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc);
                    this.Manager.Checkpoint("MS-NRPC_R103453");
                    this.Manager.Comment("reaching state \'S466\'");
                    this.Manager.Comment("checking step \'return DsrDeregisterDnsHostRecords/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp229, "return of DsrDeregisterDnsHostRecords, state S466");
                    this.Manager.Comment("reaching state \'S553\'");
                    goto label62;
                }
                if ((temp231 == 1)) {
                    this.Manager.Comment("reaching state \'S328\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp230;
                    this.Manager.Comment("executing step \'call DsrDeregisterDnsHostRecords(PrimaryDc,InvalidDomainName,Null" +
                            ",DsaGuidOne,PrimaryDc)\'");
                    temp230 = this.INrpcServerAdapterInstance.DsrDeregisterDnsHostRecords(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.InvalidDomainName, ((Microsoft.Protocols.TestSuites.Nrpc.DomainGuidType)(0)), ((Microsoft.Protocols.TestSuites.Nrpc.DsaGuidType)(0)), Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc);
                    this.Manager.Checkpoint("MS-NRPC_R103453");
                    this.Manager.Comment("reaching state \'S467\'");
                    this.Manager.Comment("checking step \'return DsrDeregisterDnsHostRecords/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp230, "return of DsrDeregisterDnsHostRecords, state S467");
                    this.Manager.Comment("reaching state \'S554\'");
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
        
        private void Test_LocateDc_DsrDeregisterDnsHostRecordsS52GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S53");
        }
        
        private void Test_LocateDc_DsrDeregisterDnsHostRecordsS52GetClientAccountTypeChecker(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S207");
        }
        
        private void Test_LocateDc_DsrDeregisterDnsHostRecordsS52GetClientAccountTypeChecker1(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S207");
        }
        
        private void Test_LocateDc_DsrDeregisterDnsHostRecordsS52GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S53");
        }
        
        private void Test_LocateDc_DsrDeregisterDnsHostRecordsS52GetClientAccountTypeChecker2(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S208");
        }
        
        private void Test_LocateDc_DsrDeregisterDnsHostRecordsS52GetClientAccountTypeChecker3(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S208");
        }
        
        private void Test_LocateDc_DsrDeregisterDnsHostRecordsS52GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S53");
        }
        
        private void Test_LocateDc_DsrDeregisterDnsHostRecordsS52GetClientAccountTypeChecker4(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S209");
        }
        
        private void Test_LocateDc_DsrDeregisterDnsHostRecordsS52GetClientAccountTypeChecker5(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S209");
        }
        #endregion
        
        #region Test Starting in S54
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_LocateDc_DsrDeregisterDnsHostRecordsS54() {
            this.Manager.BeginTest("Test_LocateDc_DsrDeregisterDnsHostRecordsS54");
            this.Manager.Comment("reaching state \'S54\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp233;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp233);
            this.Manager.AddReturn(GetPlatformInfo, null, temp233);
            this.Manager.Comment("reaching state \'S55\'");
            int temp246 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_LocateDc_DsrDeregisterDnsHostRecords.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_DsrDeregisterDnsHostRecordsS54GetPlatformChecker)), new ExpectedReturn(Test_LocateDc_DsrDeregisterDnsHostRecords.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_DsrDeregisterDnsHostRecordsS54GetPlatformChecker1)), new ExpectedReturn(Test_LocateDc_DsrDeregisterDnsHostRecords.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_DsrDeregisterDnsHostRecordsS54GetPlatformChecker2)));
            if ((temp246 == 0)) {
                this.Manager.Comment("reaching state \'S129\'");
                bool temp234;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp234);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp234);
                this.Manager.Comment("reaching state \'S210\'");
                int temp237 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_LocateDc_DsrDeregisterDnsHostRecords.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_LocateDc_DsrDeregisterDnsHostRecordsS54GetClientAccountTypeChecker)), new ExpectedReturn(Test_LocateDc_DsrDeregisterDnsHostRecords.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_LocateDc_DsrDeregisterDnsHostRecordsS54GetClientAccountTypeChecker1)));
                if ((temp237 == 0)) {
                    this.Manager.Comment("reaching state \'S329\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp235;
                    this.Manager.Comment("executing step \'call DsrDeregisterDnsHostRecords(PrimaryDc,NetBiosFormatDomainNam" +
                            "e,Null,DsaGuidOne,PrimaryDc)\'");
                    temp235 = this.INrpcServerAdapterInstance.DsrDeregisterDnsHostRecords(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.NetBiosFormatDomainName, ((Microsoft.Protocols.TestSuites.Nrpc.DomainGuidType)(0)), ((Microsoft.Protocols.TestSuites.Nrpc.DsaGuidType)(0)), Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc);
                    this.Manager.Checkpoint("MS-NRPC_R103453");
                    this.Manager.Comment("reaching state \'S468\'");
                    this.Manager.Comment("checking step \'return DsrDeregisterDnsHostRecords/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp235, "return of DsrDeregisterDnsHostRecords, state S468");
                    Test_LocateDc_DsrDeregisterDnsHostRecordsS511();
                    goto label64;
                }
                if ((temp237 == 1)) {
                    this.Manager.Comment("reaching state \'S330\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp236;
                    this.Manager.Comment("executing step \'call DsrDeregisterDnsHostRecords(PrimaryDc,NetBiosFormatDomainNam" +
                            "e,Null,DsaGuidOne,PrimaryDc)\'");
                    temp236 = this.INrpcServerAdapterInstance.DsrDeregisterDnsHostRecords(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.NetBiosFormatDomainName, ((Microsoft.Protocols.TestSuites.Nrpc.DomainGuidType)(0)), ((Microsoft.Protocols.TestSuites.Nrpc.DsaGuidType)(0)), Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc);
                    this.Manager.Checkpoint("MS-NRPC_R103453");
                    this.Manager.Comment("reaching state \'S469\'");
                    this.Manager.Comment("checking step \'return DsrDeregisterDnsHostRecords/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp236, "return of DsrDeregisterDnsHostRecords, state S469");
                    Test_LocateDc_DsrDeregisterDnsHostRecordsS510();
                    goto label64;
                }
                throw new InvalidOperationException("never reached");
            label64:
;
                goto label67;
            }
            if ((temp246 == 1)) {
                this.Manager.Comment("reaching state \'S130\'");
                bool temp238;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp238);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp238);
                this.Manager.Comment("reaching state \'S211\'");
                int temp241 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_LocateDc_DsrDeregisterDnsHostRecords.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_LocateDc_DsrDeregisterDnsHostRecordsS54GetClientAccountTypeChecker2)), new ExpectedReturn(Test_LocateDc_DsrDeregisterDnsHostRecords.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_LocateDc_DsrDeregisterDnsHostRecordsS54GetClientAccountTypeChecker3)));
                if ((temp241 == 0)) {
                    this.Manager.Comment("reaching state \'S331\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp239;
                    this.Manager.Comment("executing step \'call DsrDeregisterDnsHostRecords(PrimaryDc,InvalidDomainName,Null" +
                            ",DsaGuidOne,PrimaryDc)\'");
                    temp239 = this.INrpcServerAdapterInstance.DsrDeregisterDnsHostRecords(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.InvalidDomainName, ((Microsoft.Protocols.TestSuites.Nrpc.DomainGuidType)(0)), ((Microsoft.Protocols.TestSuites.Nrpc.DsaGuidType)(0)), Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc);
                    this.Manager.Checkpoint("MS-NRPC_R103453");
                    this.Manager.Comment("reaching state \'S470\'");
                    this.Manager.Comment("checking step \'return DsrDeregisterDnsHostRecords/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp239, "return of DsrDeregisterDnsHostRecords, state S470");
                    this.Manager.Comment("reaching state \'S555\'");
                    goto label65;
                }
                if ((temp241 == 1)) {
                    this.Manager.Comment("reaching state \'S332\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp240;
                    this.Manager.Comment("executing step \'call DsrDeregisterDnsHostRecords(PrimaryDc,InvalidDomainName,Null" +
                            ",DsaGuidOne,PrimaryDc)\'");
                    temp240 = this.INrpcServerAdapterInstance.DsrDeregisterDnsHostRecords(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.InvalidDomainName, ((Microsoft.Protocols.TestSuites.Nrpc.DomainGuidType)(0)), ((Microsoft.Protocols.TestSuites.Nrpc.DsaGuidType)(0)), Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc);
                    this.Manager.Checkpoint("MS-NRPC_R103453");
                    this.Manager.Comment("reaching state \'S471\'");
                    this.Manager.Comment("checking step \'return DsrDeregisterDnsHostRecords/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp240, "return of DsrDeregisterDnsHostRecords, state S471");
                    this.Manager.Comment("reaching state \'S556\'");
                    goto label65;
                }
                throw new InvalidOperationException("never reached");
            label65:
;
                goto label67;
            }
            if ((temp246 == 2)) {
                this.Manager.Comment("reaching state \'S131\'");
                bool temp242;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp242);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp242);
                this.Manager.Comment("reaching state \'S212\'");
                int temp245 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_LocateDc_DsrDeregisterDnsHostRecords.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_LocateDc_DsrDeregisterDnsHostRecordsS54GetClientAccountTypeChecker4)), new ExpectedReturn(Test_LocateDc_DsrDeregisterDnsHostRecords.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_LocateDc_DsrDeregisterDnsHostRecordsS54GetClientAccountTypeChecker5)));
                if ((temp245 == 0)) {
                    this.Manager.Comment("reaching state \'S333\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp243;
                    this.Manager.Comment("executing step \'call DsrDeregisterDnsHostRecords(PrimaryDc,InvalidDomainName,Null" +
                            ",DsaGuidOne,PrimaryDc)\'");
                    temp243 = this.INrpcServerAdapterInstance.DsrDeregisterDnsHostRecords(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.InvalidDomainName, ((Microsoft.Protocols.TestSuites.Nrpc.DomainGuidType)(0)), ((Microsoft.Protocols.TestSuites.Nrpc.DsaGuidType)(0)), Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc);
                    this.Manager.Checkpoint("MS-NRPC_R103453");
                    this.Manager.Comment("reaching state \'S472\'");
                    this.Manager.Comment("checking step \'return DsrDeregisterDnsHostRecords/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp243, "return of DsrDeregisterDnsHostRecords, state S472");
                    this.Manager.Comment("reaching state \'S557\'");
                    goto label66;
                }
                if ((temp245 == 1)) {
                    this.Manager.Comment("reaching state \'S334\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp244;
                    this.Manager.Comment("executing step \'call DsrDeregisterDnsHostRecords(PrimaryDc,InvalidDomainName,Null" +
                            ",DsaGuidOne,PrimaryDc)\'");
                    temp244 = this.INrpcServerAdapterInstance.DsrDeregisterDnsHostRecords(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.InvalidDomainName, ((Microsoft.Protocols.TestSuites.Nrpc.DomainGuidType)(0)), ((Microsoft.Protocols.TestSuites.Nrpc.DsaGuidType)(0)), Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc);
                    this.Manager.Checkpoint("MS-NRPC_R103453");
                    this.Manager.Comment("reaching state \'S473\'");
                    this.Manager.Comment("checking step \'return DsrDeregisterDnsHostRecords/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp244, "return of DsrDeregisterDnsHostRecords, state S473");
                    this.Manager.Comment("reaching state \'S558\'");
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
        
        private void Test_LocateDc_DsrDeregisterDnsHostRecordsS54GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S55");
        }
        
        private void Test_LocateDc_DsrDeregisterDnsHostRecordsS54GetClientAccountTypeChecker(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S210");
        }
        
        private void Test_LocateDc_DsrDeregisterDnsHostRecordsS54GetClientAccountTypeChecker1(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S210");
        }
        
        private void Test_LocateDc_DsrDeregisterDnsHostRecordsS54GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S55");
        }
        
        private void Test_LocateDc_DsrDeregisterDnsHostRecordsS54GetClientAccountTypeChecker2(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S211");
        }
        
        private void Test_LocateDc_DsrDeregisterDnsHostRecordsS54GetClientAccountTypeChecker3(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S211");
        }
        
        private void Test_LocateDc_DsrDeregisterDnsHostRecordsS54GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S55");
        }
        
        private void Test_LocateDc_DsrDeregisterDnsHostRecordsS54GetClientAccountTypeChecker4(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S212");
        }
        
        private void Test_LocateDc_DsrDeregisterDnsHostRecordsS54GetClientAccountTypeChecker5(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S212");
        }
        #endregion
        
        #region Test Starting in S56
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_LocateDc_DsrDeregisterDnsHostRecordsS56() {
            this.Manager.BeginTest("Test_LocateDc_DsrDeregisterDnsHostRecordsS56");
            this.Manager.Comment("reaching state \'S56\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp247;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp247);
            this.Manager.AddReturn(GetPlatformInfo, null, temp247);
            this.Manager.Comment("reaching state \'S57\'");
            int temp260 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_LocateDc_DsrDeregisterDnsHostRecords.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_DsrDeregisterDnsHostRecordsS56GetPlatformChecker)), new ExpectedReturn(Test_LocateDc_DsrDeregisterDnsHostRecords.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_DsrDeregisterDnsHostRecordsS56GetPlatformChecker1)), new ExpectedReturn(Test_LocateDc_DsrDeregisterDnsHostRecords.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_DsrDeregisterDnsHostRecordsS56GetPlatformChecker2)));
            if ((temp260 == 0)) {
                this.Manager.Comment("reaching state \'S132\'");
                bool temp248;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp248);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp248);
                this.Manager.Comment("reaching state \'S213\'");
                int temp251 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_LocateDc_DsrDeregisterDnsHostRecords.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_LocateDc_DsrDeregisterDnsHostRecordsS56GetClientAccountTypeChecker)), new ExpectedReturn(Test_LocateDc_DsrDeregisterDnsHostRecords.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_LocateDc_DsrDeregisterDnsHostRecordsS56GetClientAccountTypeChecker1)));
                if ((temp251 == 0)) {
                    this.Manager.Comment("reaching state \'S335\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp249;
                    this.Manager.Comment("executing step \'call DsrDeregisterDnsHostRecords(PrimaryDc,NetBiosFormatDomainNam" +
                            "e,InvalidDomainGuid,DsaGuidOne,PrimaryDc)\'");
                    temp249 = this.INrpcServerAdapterInstance.DsrDeregisterDnsHostRecords(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.NetBiosFormatDomainName, ((Microsoft.Protocols.TestSuites.Nrpc.DomainGuidType)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.DsaGuidType)(0)), Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc);
                    this.Manager.Checkpoint("MS-NRPC_R103453");
                    this.Manager.Comment("reaching state \'S474\'");
                    this.Manager.Comment("checking step \'return DsrDeregisterDnsHostRecords/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp249, "return of DsrDeregisterDnsHostRecords, state S474");
                    Test_LocateDc_DsrDeregisterDnsHostRecordsS511();
                    goto label68;
                }
                if ((temp251 == 1)) {
                    this.Manager.Comment("reaching state \'S336\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp250;
                    this.Manager.Comment("executing step \'call DsrDeregisterDnsHostRecords(PrimaryDc,NetBiosFormatDomainNam" +
                            "e,InvalidDomainGuid,DsaGuidOne,PrimaryDc)\'");
                    temp250 = this.INrpcServerAdapterInstance.DsrDeregisterDnsHostRecords(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.NetBiosFormatDomainName, ((Microsoft.Protocols.TestSuites.Nrpc.DomainGuidType)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.DsaGuidType)(0)), Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc);
                    this.Manager.Checkpoint("MS-NRPC_R103453");
                    this.Manager.Comment("reaching state \'S475\'");
                    this.Manager.Comment("checking step \'return DsrDeregisterDnsHostRecords/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp250, "return of DsrDeregisterDnsHostRecords, state S475");
                    Test_LocateDc_DsrDeregisterDnsHostRecordsS510();
                    goto label68;
                }
                throw new InvalidOperationException("never reached");
            label68:
;
                goto label71;
            }
            if ((temp260 == 1)) {
                this.Manager.Comment("reaching state \'S133\'");
                bool temp252;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp252);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp252);
                this.Manager.Comment("reaching state \'S214\'");
                int temp255 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_LocateDc_DsrDeregisterDnsHostRecords.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_LocateDc_DsrDeregisterDnsHostRecordsS56GetClientAccountTypeChecker2)), new ExpectedReturn(Test_LocateDc_DsrDeregisterDnsHostRecords.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_LocateDc_DsrDeregisterDnsHostRecordsS56GetClientAccountTypeChecker3)));
                if ((temp255 == 0)) {
                    this.Manager.Comment("reaching state \'S337\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp253;
                    this.Manager.Comment("executing step \'call DsrDeregisterDnsHostRecords(PrimaryDc,InvalidDomainName,Null" +
                            ",DsaGuidOne,PrimaryDc)\'");
                    temp253 = this.INrpcServerAdapterInstance.DsrDeregisterDnsHostRecords(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.InvalidDomainName, ((Microsoft.Protocols.TestSuites.Nrpc.DomainGuidType)(0)), ((Microsoft.Protocols.TestSuites.Nrpc.DsaGuidType)(0)), Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc);
                    this.Manager.Checkpoint("MS-NRPC_R103453");
                    this.Manager.Comment("reaching state \'S476\'");
                    this.Manager.Comment("checking step \'return DsrDeregisterDnsHostRecords/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp253, "return of DsrDeregisterDnsHostRecords, state S476");
                    this.Manager.Comment("reaching state \'S559\'");
                    goto label69;
                }
                if ((temp255 == 1)) {
                    this.Manager.Comment("reaching state \'S338\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp254;
                    this.Manager.Comment("executing step \'call DsrDeregisterDnsHostRecords(PrimaryDc,InvalidDomainName,Null" +
                            ",DsaGuidOne,PrimaryDc)\'");
                    temp254 = this.INrpcServerAdapterInstance.DsrDeregisterDnsHostRecords(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.InvalidDomainName, ((Microsoft.Protocols.TestSuites.Nrpc.DomainGuidType)(0)), ((Microsoft.Protocols.TestSuites.Nrpc.DsaGuidType)(0)), Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc);
                    this.Manager.Checkpoint("MS-NRPC_R103453");
                    this.Manager.Comment("reaching state \'S477\'");
                    this.Manager.Comment("checking step \'return DsrDeregisterDnsHostRecords/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp254, "return of DsrDeregisterDnsHostRecords, state S477");
                    this.Manager.Comment("reaching state \'S560\'");
                    goto label69;
                }
                throw new InvalidOperationException("never reached");
            label69:
;
                goto label71;
            }
            if ((temp260 == 2)) {
                this.Manager.Comment("reaching state \'S134\'");
                bool temp256;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp256);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp256);
                this.Manager.Comment("reaching state \'S215\'");
                int temp259 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_LocateDc_DsrDeregisterDnsHostRecords.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_LocateDc_DsrDeregisterDnsHostRecordsS56GetClientAccountTypeChecker4)), new ExpectedReturn(Test_LocateDc_DsrDeregisterDnsHostRecords.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_LocateDc_DsrDeregisterDnsHostRecordsS56GetClientAccountTypeChecker5)));
                if ((temp259 == 0)) {
                    this.Manager.Comment("reaching state \'S339\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp257;
                    this.Manager.Comment("executing step \'call DsrDeregisterDnsHostRecords(PrimaryDc,InvalidDomainName,Null" +
                            ",DsaGuidOne,PrimaryDc)\'");
                    temp257 = this.INrpcServerAdapterInstance.DsrDeregisterDnsHostRecords(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.InvalidDomainName, ((Microsoft.Protocols.TestSuites.Nrpc.DomainGuidType)(0)), ((Microsoft.Protocols.TestSuites.Nrpc.DsaGuidType)(0)), Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc);
                    this.Manager.Checkpoint("MS-NRPC_R103453");
                    this.Manager.Comment("reaching state \'S478\'");
                    this.Manager.Comment("checking step \'return DsrDeregisterDnsHostRecords/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp257, "return of DsrDeregisterDnsHostRecords, state S478");
                    this.Manager.Comment("reaching state \'S561\'");
                    goto label70;
                }
                if ((temp259 == 1)) {
                    this.Manager.Comment("reaching state \'S340\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp258;
                    this.Manager.Comment("executing step \'call DsrDeregisterDnsHostRecords(PrimaryDc,InvalidDomainName,Null" +
                            ",DsaGuidOne,PrimaryDc)\'");
                    temp258 = this.INrpcServerAdapterInstance.DsrDeregisterDnsHostRecords(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.InvalidDomainName, ((Microsoft.Protocols.TestSuites.Nrpc.DomainGuidType)(0)), ((Microsoft.Protocols.TestSuites.Nrpc.DsaGuidType)(0)), Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc);
                    this.Manager.Checkpoint("MS-NRPC_R103453");
                    this.Manager.Comment("reaching state \'S479\'");
                    this.Manager.Comment("checking step \'return DsrDeregisterDnsHostRecords/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp258, "return of DsrDeregisterDnsHostRecords, state S479");
                    this.Manager.Comment("reaching state \'S562\'");
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
        
        private void Test_LocateDc_DsrDeregisterDnsHostRecordsS56GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S57");
        }
        
        private void Test_LocateDc_DsrDeregisterDnsHostRecordsS56GetClientAccountTypeChecker(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S213");
        }
        
        private void Test_LocateDc_DsrDeregisterDnsHostRecordsS56GetClientAccountTypeChecker1(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S213");
        }
        
        private void Test_LocateDc_DsrDeregisterDnsHostRecordsS56GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S57");
        }
        
        private void Test_LocateDc_DsrDeregisterDnsHostRecordsS56GetClientAccountTypeChecker2(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S214");
        }
        
        private void Test_LocateDc_DsrDeregisterDnsHostRecordsS56GetClientAccountTypeChecker3(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S214");
        }
        
        private void Test_LocateDc_DsrDeregisterDnsHostRecordsS56GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S57");
        }
        
        private void Test_LocateDc_DsrDeregisterDnsHostRecordsS56GetClientAccountTypeChecker4(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S215");
        }
        
        private void Test_LocateDc_DsrDeregisterDnsHostRecordsS56GetClientAccountTypeChecker5(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S215");
        }
        #endregion
        
        #region Test Starting in S58
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_LocateDc_DsrDeregisterDnsHostRecordsS58() {
            this.Manager.BeginTest("Test_LocateDc_DsrDeregisterDnsHostRecordsS58");
            this.Manager.Comment("reaching state \'S58\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp261;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp261);
            this.Manager.AddReturn(GetPlatformInfo, null, temp261);
            this.Manager.Comment("reaching state \'S59\'");
            int temp274 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_LocateDc_DsrDeregisterDnsHostRecords.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_DsrDeregisterDnsHostRecordsS58GetPlatformChecker)), new ExpectedReturn(Test_LocateDc_DsrDeregisterDnsHostRecords.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_DsrDeregisterDnsHostRecordsS58GetPlatformChecker1)), new ExpectedReturn(Test_LocateDc_DsrDeregisterDnsHostRecords.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_DsrDeregisterDnsHostRecordsS58GetPlatformChecker2)));
            if ((temp274 == 0)) {
                this.Manager.Comment("reaching state \'S135\'");
                bool temp262;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp262);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp262);
                this.Manager.Comment("reaching state \'S216\'");
                int temp265 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_LocateDc_DsrDeregisterDnsHostRecords.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_LocateDc_DsrDeregisterDnsHostRecordsS58GetClientAccountTypeChecker)), new ExpectedReturn(Test_LocateDc_DsrDeregisterDnsHostRecords.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_LocateDc_DsrDeregisterDnsHostRecordsS58GetClientAccountTypeChecker1)));
                if ((temp265 == 0)) {
                    this.Manager.Comment("reaching state \'S341\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp263;
                    this.Manager.Comment("executing step \'call DsrDeregisterDnsHostRecords(PrimaryDc,NetBiosFormatDomainNam" +
                            "e,PrimaryDomainGuid,DsaGuidOne,PrimaryDc)\'");
                    temp263 = this.INrpcServerAdapterInstance.DsrDeregisterDnsHostRecords(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.NetBiosFormatDomainName, Microsoft.Protocols.TestSuites.Nrpc.DomainGuidType.PrimaryDomainGuid, ((Microsoft.Protocols.TestSuites.Nrpc.DsaGuidType)(0)), Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc);
                    this.Manager.Checkpoint("MS-NRPC_R103453");
                    this.Manager.Comment("reaching state \'S480\'");
                    this.Manager.Comment("checking step \'return DsrDeregisterDnsHostRecords/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp263, "return of DsrDeregisterDnsHostRecords, state S480");
                    Test_LocateDc_DsrDeregisterDnsHostRecordsS511();
                    goto label72;
                }
                if ((temp265 == 1)) {
                    this.Manager.Comment("reaching state \'S342\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp264;
                    this.Manager.Comment("executing step \'call DsrDeregisterDnsHostRecords(PrimaryDc,NetBiosFormatDomainNam" +
                            "e,PrimaryDomainGuid,DsaGuidOne,PrimaryDc)\'");
                    temp264 = this.INrpcServerAdapterInstance.DsrDeregisterDnsHostRecords(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.NetBiosFormatDomainName, Microsoft.Protocols.TestSuites.Nrpc.DomainGuidType.PrimaryDomainGuid, ((Microsoft.Protocols.TestSuites.Nrpc.DsaGuidType)(0)), Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc);
                    this.Manager.Checkpoint("MS-NRPC_R103453");
                    this.Manager.Comment("reaching state \'S481\'");
                    this.Manager.Comment("checking step \'return DsrDeregisterDnsHostRecords/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp264, "return of DsrDeregisterDnsHostRecords, state S481");
                    Test_LocateDc_DsrDeregisterDnsHostRecordsS510();
                    goto label72;
                }
                throw new InvalidOperationException("never reached");
            label72:
;
                goto label75;
            }
            if ((temp274 == 1)) {
                this.Manager.Comment("reaching state \'S136\'");
                bool temp266;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp266);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp266);
                this.Manager.Comment("reaching state \'S217\'");
                int temp269 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_LocateDc_DsrDeregisterDnsHostRecords.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_LocateDc_DsrDeregisterDnsHostRecordsS58GetClientAccountTypeChecker2)), new ExpectedReturn(Test_LocateDc_DsrDeregisterDnsHostRecords.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_LocateDc_DsrDeregisterDnsHostRecordsS58GetClientAccountTypeChecker3)));
                if ((temp269 == 0)) {
                    this.Manager.Comment("reaching state \'S343\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp267;
                    this.Manager.Comment("executing step \'call DsrDeregisterDnsHostRecords(PrimaryDc,InvalidDomainName,Null" +
                            ",DsaGuidOne,PrimaryDc)\'");
                    temp267 = this.INrpcServerAdapterInstance.DsrDeregisterDnsHostRecords(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.InvalidDomainName, ((Microsoft.Protocols.TestSuites.Nrpc.DomainGuidType)(0)), ((Microsoft.Protocols.TestSuites.Nrpc.DsaGuidType)(0)), Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc);
                    this.Manager.Checkpoint("MS-NRPC_R103453");
                    this.Manager.Comment("reaching state \'S482\'");
                    this.Manager.Comment("checking step \'return DsrDeregisterDnsHostRecords/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp267, "return of DsrDeregisterDnsHostRecords, state S482");
                    this.Manager.Comment("reaching state \'S563\'");
                    goto label73;
                }
                if ((temp269 == 1)) {
                    this.Manager.Comment("reaching state \'S344\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp268;
                    this.Manager.Comment("executing step \'call DsrDeregisterDnsHostRecords(PrimaryDc,InvalidDomainName,Null" +
                            ",DsaGuidOne,PrimaryDc)\'");
                    temp268 = this.INrpcServerAdapterInstance.DsrDeregisterDnsHostRecords(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.InvalidDomainName, ((Microsoft.Protocols.TestSuites.Nrpc.DomainGuidType)(0)), ((Microsoft.Protocols.TestSuites.Nrpc.DsaGuidType)(0)), Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc);
                    this.Manager.Checkpoint("MS-NRPC_R103453");
                    this.Manager.Comment("reaching state \'S483\'");
                    this.Manager.Comment("checking step \'return DsrDeregisterDnsHostRecords/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp268, "return of DsrDeregisterDnsHostRecords, state S483");
                    this.Manager.Comment("reaching state \'S564\'");
                    goto label73;
                }
                throw new InvalidOperationException("never reached");
            label73:
;
                goto label75;
            }
            if ((temp274 == 2)) {
                this.Manager.Comment("reaching state \'S137\'");
                bool temp270;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp270);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp270);
                this.Manager.Comment("reaching state \'S218\'");
                int temp273 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_LocateDc_DsrDeregisterDnsHostRecords.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_LocateDc_DsrDeregisterDnsHostRecordsS58GetClientAccountTypeChecker4)), new ExpectedReturn(Test_LocateDc_DsrDeregisterDnsHostRecords.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_LocateDc_DsrDeregisterDnsHostRecordsS58GetClientAccountTypeChecker5)));
                if ((temp273 == 0)) {
                    this.Manager.Comment("reaching state \'S345\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp271;
                    this.Manager.Comment("executing step \'call DsrDeregisterDnsHostRecords(PrimaryDc,InvalidDomainName,Null" +
                            ",DsaGuidOne,PrimaryDc)\'");
                    temp271 = this.INrpcServerAdapterInstance.DsrDeregisterDnsHostRecords(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.InvalidDomainName, ((Microsoft.Protocols.TestSuites.Nrpc.DomainGuidType)(0)), ((Microsoft.Protocols.TestSuites.Nrpc.DsaGuidType)(0)), Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc);
                    this.Manager.Checkpoint("MS-NRPC_R103453");
                    this.Manager.Comment("reaching state \'S484\'");
                    this.Manager.Comment("checking step \'return DsrDeregisterDnsHostRecords/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp271, "return of DsrDeregisterDnsHostRecords, state S484");
                    this.Manager.Comment("reaching state \'S565\'");
                    goto label74;
                }
                if ((temp273 == 1)) {
                    this.Manager.Comment("reaching state \'S346\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp272;
                    this.Manager.Comment("executing step \'call DsrDeregisterDnsHostRecords(PrimaryDc,InvalidDomainName,Null" +
                            ",DsaGuidOne,PrimaryDc)\'");
                    temp272 = this.INrpcServerAdapterInstance.DsrDeregisterDnsHostRecords(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.InvalidDomainName, ((Microsoft.Protocols.TestSuites.Nrpc.DomainGuidType)(0)), ((Microsoft.Protocols.TestSuites.Nrpc.DsaGuidType)(0)), Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc);
                    this.Manager.Checkpoint("MS-NRPC_R103453");
                    this.Manager.Comment("reaching state \'S485\'");
                    this.Manager.Comment("checking step \'return DsrDeregisterDnsHostRecords/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp272, "return of DsrDeregisterDnsHostRecords, state S485");
                    this.Manager.Comment("reaching state \'S566\'");
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
        
        private void Test_LocateDc_DsrDeregisterDnsHostRecordsS58GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S59");
        }
        
        private void Test_LocateDc_DsrDeregisterDnsHostRecordsS58GetClientAccountTypeChecker(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S216");
        }
        
        private void Test_LocateDc_DsrDeregisterDnsHostRecordsS58GetClientAccountTypeChecker1(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S216");
        }
        
        private void Test_LocateDc_DsrDeregisterDnsHostRecordsS58GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S59");
        }
        
        private void Test_LocateDc_DsrDeregisterDnsHostRecordsS58GetClientAccountTypeChecker2(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S217");
        }
        
        private void Test_LocateDc_DsrDeregisterDnsHostRecordsS58GetClientAccountTypeChecker3(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S217");
        }
        
        private void Test_LocateDc_DsrDeregisterDnsHostRecordsS58GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S59");
        }
        
        private void Test_LocateDc_DsrDeregisterDnsHostRecordsS58GetClientAccountTypeChecker4(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S218");
        }
        
        private void Test_LocateDc_DsrDeregisterDnsHostRecordsS58GetClientAccountTypeChecker5(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S218");
        }
        #endregion
        
        #region Test Starting in S6
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_LocateDc_DsrDeregisterDnsHostRecordsS6() {
            this.Manager.BeginTest("Test_LocateDc_DsrDeregisterDnsHostRecordsS6");
            this.Manager.Comment("reaching state \'S6\'");
            this.Manager.Comment("executing step \'call SwitchUserAccount(False)\'");
            this.INrpcServerAdapterInstance.SwitchUserAccount(false);
            this.Manager.Comment("reaching state \'S7\'");
            this.Manager.Comment("checking step \'return SwitchUserAccount\'");
            this.Manager.Comment("reaching state \'S75\'");
            Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp275;
            this.Manager.Comment("executing step \'call DsrDeregisterDnsHostRecords(PrimaryDc,InvalidDomainName,Prim" +
                    "aryDomainGuid,DsaGuidOne,PrimaryDc)\'");
            temp275 = this.INrpcServerAdapterInstance.DsrDeregisterDnsHostRecords(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.InvalidDomainName, Microsoft.Protocols.TestSuites.Nrpc.DomainGuidType.PrimaryDomainGuid, ((Microsoft.Protocols.TestSuites.Nrpc.DsaGuidType)(0)), Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc);
            this.Manager.Checkpoint("MS-NRPC_R103328");
            this.Manager.Checkpoint("MS-NRPC_R103454");
            this.Manager.Comment("reaching state \'S156\'");
            this.Manager.Comment("checking step \'return DsrDeregisterDnsHostRecords/ERROR_ACCESS_DENIED\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_ACCESS_DENIED, temp275, "return of DsrDeregisterDnsHostRecords, state S156");
            Test_LocateDc_DsrDeregisterDnsHostRecordsS238();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S60
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_LocateDc_DsrDeregisterDnsHostRecordsS60() {
            this.Manager.BeginTest("Test_LocateDc_DsrDeregisterDnsHostRecordsS60");
            this.Manager.Comment("reaching state \'S60\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp276;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp276);
            this.Manager.AddReturn(GetPlatformInfo, null, temp276);
            this.Manager.Comment("reaching state \'S61\'");
            int temp289 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_LocateDc_DsrDeregisterDnsHostRecords.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_DsrDeregisterDnsHostRecordsS60GetPlatformChecker)), new ExpectedReturn(Test_LocateDc_DsrDeregisterDnsHostRecords.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_DsrDeregisterDnsHostRecordsS60GetPlatformChecker1)), new ExpectedReturn(Test_LocateDc_DsrDeregisterDnsHostRecords.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_DsrDeregisterDnsHostRecordsS60GetPlatformChecker2)));
            if ((temp289 == 0)) {
                this.Manager.Comment("reaching state \'S138\'");
                bool temp277;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp277);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp277);
                this.Manager.Comment("reaching state \'S219\'");
                int temp280 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_LocateDc_DsrDeregisterDnsHostRecords.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_LocateDc_DsrDeregisterDnsHostRecordsS60GetClientAccountTypeChecker)), new ExpectedReturn(Test_LocateDc_DsrDeregisterDnsHostRecords.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_LocateDc_DsrDeregisterDnsHostRecordsS60GetClientAccountTypeChecker1)));
                if ((temp280 == 0)) {
                    this.Manager.Comment("reaching state \'S347\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp278;
                    this.Manager.Comment("executing step \'call DsrDeregisterDnsHostRecords(PrimaryDc,NetBiosFormatDomainNam" +
                            "e,TrustedDomainGuid,DsaGuidOne,PrimaryDc)\'");
                    temp278 = this.INrpcServerAdapterInstance.DsrDeregisterDnsHostRecords(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.NetBiosFormatDomainName, Microsoft.Protocols.TestSuites.Nrpc.DomainGuidType.TrustedDomainGuid, ((Microsoft.Protocols.TestSuites.Nrpc.DsaGuidType)(0)), Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc);
                    this.Manager.Checkpoint("MS-NRPC_R103453");
                    this.Manager.Comment("reaching state \'S486\'");
                    this.Manager.Comment("checking step \'return DsrDeregisterDnsHostRecords/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp278, "return of DsrDeregisterDnsHostRecords, state S486");
                    Test_LocateDc_DsrDeregisterDnsHostRecordsS511();
                    goto label76;
                }
                if ((temp280 == 1)) {
                    this.Manager.Comment("reaching state \'S348\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp279;
                    this.Manager.Comment("executing step \'call DsrDeregisterDnsHostRecords(PrimaryDc,NetBiosFormatDomainNam" +
                            "e,TrustedDomainGuid,DsaGuidOne,PrimaryDc)\'");
                    temp279 = this.INrpcServerAdapterInstance.DsrDeregisterDnsHostRecords(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.NetBiosFormatDomainName, Microsoft.Protocols.TestSuites.Nrpc.DomainGuidType.TrustedDomainGuid, ((Microsoft.Protocols.TestSuites.Nrpc.DsaGuidType)(0)), Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc);
                    this.Manager.Checkpoint("MS-NRPC_R103453");
                    this.Manager.Comment("reaching state \'S487\'");
                    this.Manager.Comment("checking step \'return DsrDeregisterDnsHostRecords/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp279, "return of DsrDeregisterDnsHostRecords, state S487");
                    Test_LocateDc_DsrDeregisterDnsHostRecordsS510();
                    goto label76;
                }
                throw new InvalidOperationException("never reached");
            label76:
;
                goto label79;
            }
            if ((temp289 == 1)) {
                this.Manager.Comment("reaching state \'S139\'");
                bool temp281;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp281);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp281);
                this.Manager.Comment("reaching state \'S220\'");
                int temp284 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_LocateDc_DsrDeregisterDnsHostRecords.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_LocateDc_DsrDeregisterDnsHostRecordsS60GetClientAccountTypeChecker2)), new ExpectedReturn(Test_LocateDc_DsrDeregisterDnsHostRecords.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_LocateDc_DsrDeregisterDnsHostRecordsS60GetClientAccountTypeChecker3)));
                if ((temp284 == 0)) {
                    this.Manager.Comment("reaching state \'S349\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp282;
                    this.Manager.Comment("executing step \'call DsrDeregisterDnsHostRecords(PrimaryDc,InvalidDomainName,Null" +
                            ",DsaGuidOne,PrimaryDc)\'");
                    temp282 = this.INrpcServerAdapterInstance.DsrDeregisterDnsHostRecords(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.InvalidDomainName, ((Microsoft.Protocols.TestSuites.Nrpc.DomainGuidType)(0)), ((Microsoft.Protocols.TestSuites.Nrpc.DsaGuidType)(0)), Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc);
                    this.Manager.Checkpoint("MS-NRPC_R103453");
                    this.Manager.Comment("reaching state \'S488\'");
                    this.Manager.Comment("checking step \'return DsrDeregisterDnsHostRecords/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp282, "return of DsrDeregisterDnsHostRecords, state S488");
                    this.Manager.Comment("reaching state \'S567\'");
                    goto label77;
                }
                if ((temp284 == 1)) {
                    this.Manager.Comment("reaching state \'S350\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp283;
                    this.Manager.Comment("executing step \'call DsrDeregisterDnsHostRecords(PrimaryDc,InvalidDomainName,Null" +
                            ",DsaGuidOne,PrimaryDc)\'");
                    temp283 = this.INrpcServerAdapterInstance.DsrDeregisterDnsHostRecords(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.InvalidDomainName, ((Microsoft.Protocols.TestSuites.Nrpc.DomainGuidType)(0)), ((Microsoft.Protocols.TestSuites.Nrpc.DsaGuidType)(0)), Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc);
                    this.Manager.Checkpoint("MS-NRPC_R103453");
                    this.Manager.Comment("reaching state \'S489\'");
                    this.Manager.Comment("checking step \'return DsrDeregisterDnsHostRecords/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp283, "return of DsrDeregisterDnsHostRecords, state S489");
                    this.Manager.Comment("reaching state \'S568\'");
                    goto label77;
                }
                throw new InvalidOperationException("never reached");
            label77:
;
                goto label79;
            }
            if ((temp289 == 2)) {
                this.Manager.Comment("reaching state \'S140\'");
                bool temp285;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp285);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp285);
                this.Manager.Comment("reaching state \'S221\'");
                int temp288 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_LocateDc_DsrDeregisterDnsHostRecords.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_LocateDc_DsrDeregisterDnsHostRecordsS60GetClientAccountTypeChecker4)), new ExpectedReturn(Test_LocateDc_DsrDeregisterDnsHostRecords.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_LocateDc_DsrDeregisterDnsHostRecordsS60GetClientAccountTypeChecker5)));
                if ((temp288 == 0)) {
                    this.Manager.Comment("reaching state \'S351\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp286;
                    this.Manager.Comment("executing step \'call DsrDeregisterDnsHostRecords(PrimaryDc,InvalidDomainName,Null" +
                            ",DsaGuidOne,PrimaryDc)\'");
                    temp286 = this.INrpcServerAdapterInstance.DsrDeregisterDnsHostRecords(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.InvalidDomainName, ((Microsoft.Protocols.TestSuites.Nrpc.DomainGuidType)(0)), ((Microsoft.Protocols.TestSuites.Nrpc.DsaGuidType)(0)), Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc);
                    this.Manager.Checkpoint("MS-NRPC_R103453");
                    this.Manager.Comment("reaching state \'S490\'");
                    this.Manager.Comment("checking step \'return DsrDeregisterDnsHostRecords/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp286, "return of DsrDeregisterDnsHostRecords, state S490");
                    this.Manager.Comment("reaching state \'S569\'");
                    goto label78;
                }
                if ((temp288 == 1)) {
                    this.Manager.Comment("reaching state \'S352\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp287;
                    this.Manager.Comment("executing step \'call DsrDeregisterDnsHostRecords(PrimaryDc,InvalidDomainName,Null" +
                            ",DsaGuidOne,PrimaryDc)\'");
                    temp287 = this.INrpcServerAdapterInstance.DsrDeregisterDnsHostRecords(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.InvalidDomainName, ((Microsoft.Protocols.TestSuites.Nrpc.DomainGuidType)(0)), ((Microsoft.Protocols.TestSuites.Nrpc.DsaGuidType)(0)), Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc);
                    this.Manager.Checkpoint("MS-NRPC_R103453");
                    this.Manager.Comment("reaching state \'S491\'");
                    this.Manager.Comment("checking step \'return DsrDeregisterDnsHostRecords/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp287, "return of DsrDeregisterDnsHostRecords, state S491");
                    this.Manager.Comment("reaching state \'S570\'");
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
        
        private void Test_LocateDc_DsrDeregisterDnsHostRecordsS60GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S61");
        }
        
        private void Test_LocateDc_DsrDeregisterDnsHostRecordsS60GetClientAccountTypeChecker(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S219");
        }
        
        private void Test_LocateDc_DsrDeregisterDnsHostRecordsS60GetClientAccountTypeChecker1(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S219");
        }
        
        private void Test_LocateDc_DsrDeregisterDnsHostRecordsS60GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S61");
        }
        
        private void Test_LocateDc_DsrDeregisterDnsHostRecordsS60GetClientAccountTypeChecker2(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S220");
        }
        
        private void Test_LocateDc_DsrDeregisterDnsHostRecordsS60GetClientAccountTypeChecker3(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S220");
        }
        
        private void Test_LocateDc_DsrDeregisterDnsHostRecordsS60GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S61");
        }
        
        private void Test_LocateDc_DsrDeregisterDnsHostRecordsS60GetClientAccountTypeChecker4(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S221");
        }
        
        private void Test_LocateDc_DsrDeregisterDnsHostRecordsS60GetClientAccountTypeChecker5(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S221");
        }
        #endregion
        
        #region Test Starting in S62
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_LocateDc_DsrDeregisterDnsHostRecordsS62() {
            this.Manager.BeginTest("Test_LocateDc_DsrDeregisterDnsHostRecordsS62");
            this.Manager.Comment("reaching state \'S62\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp290;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp290);
            this.Manager.AddReturn(GetPlatformInfo, null, temp290);
            this.Manager.Comment("reaching state \'S63\'");
            int temp303 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_LocateDc_DsrDeregisterDnsHostRecords.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_DsrDeregisterDnsHostRecordsS62GetPlatformChecker)), new ExpectedReturn(Test_LocateDc_DsrDeregisterDnsHostRecords.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_DsrDeregisterDnsHostRecordsS62GetPlatformChecker1)), new ExpectedReturn(Test_LocateDc_DsrDeregisterDnsHostRecords.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_DsrDeregisterDnsHostRecordsS62GetPlatformChecker2)));
            if ((temp303 == 0)) {
                this.Manager.Comment("reaching state \'S141\'");
                bool temp291;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp291);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp291);
                this.Manager.Comment("reaching state \'S222\'");
                int temp294 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_LocateDc_DsrDeregisterDnsHostRecords.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_LocateDc_DsrDeregisterDnsHostRecordsS62GetClientAccountTypeChecker)), new ExpectedReturn(Test_LocateDc_DsrDeregisterDnsHostRecords.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_LocateDc_DsrDeregisterDnsHostRecordsS62GetClientAccountTypeChecker1)));
                if ((temp294 == 0)) {
                    this.Manager.Comment("reaching state \'S353\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp292;
                    this.Manager.Comment("executing step \'call DsrDeregisterDnsHostRecords(PrimaryDc,FqdnFormatDomainName,N" +
                            "ull,DsaGuidOne,PrimaryDc)\'");
                    temp292 = this.INrpcServerAdapterInstance.DsrDeregisterDnsHostRecords(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.FqdnFormatDomainName, ((Microsoft.Protocols.TestSuites.Nrpc.DomainGuidType)(0)), ((Microsoft.Protocols.TestSuites.Nrpc.DsaGuidType)(0)), Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc);
                    this.Manager.Checkpoint("MS-NRPC_R103453");
                    this.Manager.Comment("reaching state \'S492\'");
                    this.Manager.Comment("checking step \'return DsrDeregisterDnsHostRecords/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp292, "return of DsrDeregisterDnsHostRecords, state S492");
                    Test_LocateDc_DsrDeregisterDnsHostRecordsS511();
                    goto label80;
                }
                if ((temp294 == 1)) {
                    this.Manager.Comment("reaching state \'S354\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp293;
                    this.Manager.Comment("executing step \'call DsrDeregisterDnsHostRecords(PrimaryDc,FqdnFormatDomainName,N" +
                            "ull,DsaGuidOne,PrimaryDc)\'");
                    temp293 = this.INrpcServerAdapterInstance.DsrDeregisterDnsHostRecords(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.FqdnFormatDomainName, ((Microsoft.Protocols.TestSuites.Nrpc.DomainGuidType)(0)), ((Microsoft.Protocols.TestSuites.Nrpc.DsaGuidType)(0)), Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc);
                    this.Manager.Checkpoint("MS-NRPC_R103453");
                    this.Manager.Comment("reaching state \'S493\'");
                    this.Manager.Comment("checking step \'return DsrDeregisterDnsHostRecords/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp293, "return of DsrDeregisterDnsHostRecords, state S493");
                    Test_LocateDc_DsrDeregisterDnsHostRecordsS510();
                    goto label80;
                }
                throw new InvalidOperationException("never reached");
            label80:
;
                goto label83;
            }
            if ((temp303 == 1)) {
                this.Manager.Comment("reaching state \'S142\'");
                bool temp295;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp295);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp295);
                this.Manager.Comment("reaching state \'S223\'");
                int temp298 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_LocateDc_DsrDeregisterDnsHostRecords.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_LocateDc_DsrDeregisterDnsHostRecordsS62GetClientAccountTypeChecker2)), new ExpectedReturn(Test_LocateDc_DsrDeregisterDnsHostRecords.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_LocateDc_DsrDeregisterDnsHostRecordsS62GetClientAccountTypeChecker3)));
                if ((temp298 == 0)) {
                    this.Manager.Comment("reaching state \'S355\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp296;
                    this.Manager.Comment("executing step \'call DsrDeregisterDnsHostRecords(PrimaryDc,InvalidDomainName,Null" +
                            ",DsaGuidOne,PrimaryDc)\'");
                    temp296 = this.INrpcServerAdapterInstance.DsrDeregisterDnsHostRecords(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.InvalidDomainName, ((Microsoft.Protocols.TestSuites.Nrpc.DomainGuidType)(0)), ((Microsoft.Protocols.TestSuites.Nrpc.DsaGuidType)(0)), Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc);
                    this.Manager.Checkpoint("MS-NRPC_R103453");
                    this.Manager.Comment("reaching state \'S494\'");
                    this.Manager.Comment("checking step \'return DsrDeregisterDnsHostRecords/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp296, "return of DsrDeregisterDnsHostRecords, state S494");
                    this.Manager.Comment("reaching state \'S571\'");
                    goto label81;
                }
                if ((temp298 == 1)) {
                    this.Manager.Comment("reaching state \'S356\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp297;
                    this.Manager.Comment("executing step \'call DsrDeregisterDnsHostRecords(PrimaryDc,InvalidDomainName,Null" +
                            ",DsaGuidOne,PrimaryDc)\'");
                    temp297 = this.INrpcServerAdapterInstance.DsrDeregisterDnsHostRecords(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.InvalidDomainName, ((Microsoft.Protocols.TestSuites.Nrpc.DomainGuidType)(0)), ((Microsoft.Protocols.TestSuites.Nrpc.DsaGuidType)(0)), Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc);
                    this.Manager.Checkpoint("MS-NRPC_R103453");
                    this.Manager.Comment("reaching state \'S495\'");
                    this.Manager.Comment("checking step \'return DsrDeregisterDnsHostRecords/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp297, "return of DsrDeregisterDnsHostRecords, state S495");
                    this.Manager.Comment("reaching state \'S572\'");
                    goto label81;
                }
                throw new InvalidOperationException("never reached");
            label81:
;
                goto label83;
            }
            if ((temp303 == 2)) {
                this.Manager.Comment("reaching state \'S143\'");
                bool temp299;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp299);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp299);
                this.Manager.Comment("reaching state \'S224\'");
                int temp302 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_LocateDc_DsrDeregisterDnsHostRecords.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_LocateDc_DsrDeregisterDnsHostRecordsS62GetClientAccountTypeChecker4)), new ExpectedReturn(Test_LocateDc_DsrDeregisterDnsHostRecords.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_LocateDc_DsrDeregisterDnsHostRecordsS62GetClientAccountTypeChecker5)));
                if ((temp302 == 0)) {
                    this.Manager.Comment("reaching state \'S357\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp300;
                    this.Manager.Comment("executing step \'call DsrDeregisterDnsHostRecords(PrimaryDc,InvalidDomainName,Null" +
                            ",DsaGuidOne,PrimaryDc)\'");
                    temp300 = this.INrpcServerAdapterInstance.DsrDeregisterDnsHostRecords(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.InvalidDomainName, ((Microsoft.Protocols.TestSuites.Nrpc.DomainGuidType)(0)), ((Microsoft.Protocols.TestSuites.Nrpc.DsaGuidType)(0)), Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc);
                    this.Manager.Checkpoint("MS-NRPC_R103453");
                    this.Manager.Comment("reaching state \'S496\'");
                    this.Manager.Comment("checking step \'return DsrDeregisterDnsHostRecords/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp300, "return of DsrDeregisterDnsHostRecords, state S496");
                    this.Manager.Comment("reaching state \'S573\'");
                    goto label82;
                }
                if ((temp302 == 1)) {
                    this.Manager.Comment("reaching state \'S358\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp301;
                    this.Manager.Comment("executing step \'call DsrDeregisterDnsHostRecords(PrimaryDc,InvalidDomainName,Null" +
                            ",DsaGuidOne,PrimaryDc)\'");
                    temp301 = this.INrpcServerAdapterInstance.DsrDeregisterDnsHostRecords(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.InvalidDomainName, ((Microsoft.Protocols.TestSuites.Nrpc.DomainGuidType)(0)), ((Microsoft.Protocols.TestSuites.Nrpc.DsaGuidType)(0)), Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc);
                    this.Manager.Checkpoint("MS-NRPC_R103453");
                    this.Manager.Comment("reaching state \'S497\'");
                    this.Manager.Comment("checking step \'return DsrDeregisterDnsHostRecords/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp301, "return of DsrDeregisterDnsHostRecords, state S497");
                    this.Manager.Comment("reaching state \'S574\'");
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
        
        private void Test_LocateDc_DsrDeregisterDnsHostRecordsS62GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S63");
        }
        
        private void Test_LocateDc_DsrDeregisterDnsHostRecordsS62GetClientAccountTypeChecker(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S222");
        }
        
        private void Test_LocateDc_DsrDeregisterDnsHostRecordsS62GetClientAccountTypeChecker1(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S222");
        }
        
        private void Test_LocateDc_DsrDeregisterDnsHostRecordsS62GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S63");
        }
        
        private void Test_LocateDc_DsrDeregisterDnsHostRecordsS62GetClientAccountTypeChecker2(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S223");
        }
        
        private void Test_LocateDc_DsrDeregisterDnsHostRecordsS62GetClientAccountTypeChecker3(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S223");
        }
        
        private void Test_LocateDc_DsrDeregisterDnsHostRecordsS62GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S63");
        }
        
        private void Test_LocateDc_DsrDeregisterDnsHostRecordsS62GetClientAccountTypeChecker4(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S224");
        }
        
        private void Test_LocateDc_DsrDeregisterDnsHostRecordsS62GetClientAccountTypeChecker5(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S224");
        }
        #endregion
        
        #region Test Starting in S64
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_LocateDc_DsrDeregisterDnsHostRecordsS64() {
            this.Manager.BeginTest("Test_LocateDc_DsrDeregisterDnsHostRecordsS64");
            this.Manager.Comment("reaching state \'S64\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp304;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp304);
            this.Manager.AddReturn(GetPlatformInfo, null, temp304);
            this.Manager.Comment("reaching state \'S65\'");
            int temp317 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_LocateDc_DsrDeregisterDnsHostRecords.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_DsrDeregisterDnsHostRecordsS64GetPlatformChecker)), new ExpectedReturn(Test_LocateDc_DsrDeregisterDnsHostRecords.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_DsrDeregisterDnsHostRecordsS64GetPlatformChecker1)), new ExpectedReturn(Test_LocateDc_DsrDeregisterDnsHostRecords.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_DsrDeregisterDnsHostRecordsS64GetPlatformChecker2)));
            if ((temp317 == 0)) {
                this.Manager.Comment("reaching state \'S144\'");
                bool temp305;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp305);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp305);
                this.Manager.Comment("reaching state \'S225\'");
                int temp308 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_LocateDc_DsrDeregisterDnsHostRecords.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_LocateDc_DsrDeregisterDnsHostRecordsS64GetClientAccountTypeChecker)), new ExpectedReturn(Test_LocateDc_DsrDeregisterDnsHostRecords.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_LocateDc_DsrDeregisterDnsHostRecordsS64GetClientAccountTypeChecker1)));
                if ((temp308 == 0)) {
                    this.Manager.Comment("reaching state \'S359\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp306;
                    this.Manager.Comment("executing step \'call DsrDeregisterDnsHostRecords(PrimaryDc,FqdnFormatDomainName,I" +
                            "nvalidDomainGuid,DsaGuidOne,PrimaryDc)\'");
                    temp306 = this.INrpcServerAdapterInstance.DsrDeregisterDnsHostRecords(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.FqdnFormatDomainName, ((Microsoft.Protocols.TestSuites.Nrpc.DomainGuidType)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.DsaGuidType)(0)), Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc);
                    this.Manager.Checkpoint("MS-NRPC_R103453");
                    this.Manager.Comment("reaching state \'S498\'");
                    this.Manager.Comment("checking step \'return DsrDeregisterDnsHostRecords/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp306, "return of DsrDeregisterDnsHostRecords, state S498");
                    Test_LocateDc_DsrDeregisterDnsHostRecordsS511();
                    goto label84;
                }
                if ((temp308 == 1)) {
                    this.Manager.Comment("reaching state \'S360\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp307;
                    this.Manager.Comment("executing step \'call DsrDeregisterDnsHostRecords(PrimaryDc,FqdnFormatDomainName,I" +
                            "nvalidDomainGuid,DsaGuidOne,PrimaryDc)\'");
                    temp307 = this.INrpcServerAdapterInstance.DsrDeregisterDnsHostRecords(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.FqdnFormatDomainName, ((Microsoft.Protocols.TestSuites.Nrpc.DomainGuidType)(1)), ((Microsoft.Protocols.TestSuites.Nrpc.DsaGuidType)(0)), Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc);
                    this.Manager.Checkpoint("MS-NRPC_R103453");
                    this.Manager.Comment("reaching state \'S499\'");
                    this.Manager.Comment("checking step \'return DsrDeregisterDnsHostRecords/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp307, "return of DsrDeregisterDnsHostRecords, state S499");
                    Test_LocateDc_DsrDeregisterDnsHostRecordsS510();
                    goto label84;
                }
                throw new InvalidOperationException("never reached");
            label84:
;
                goto label87;
            }
            if ((temp317 == 1)) {
                this.Manager.Comment("reaching state \'S145\'");
                bool temp309;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp309);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp309);
                this.Manager.Comment("reaching state \'S226\'");
                int temp312 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_LocateDc_DsrDeregisterDnsHostRecords.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_LocateDc_DsrDeregisterDnsHostRecordsS64GetClientAccountTypeChecker2)), new ExpectedReturn(Test_LocateDc_DsrDeregisterDnsHostRecords.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_LocateDc_DsrDeregisterDnsHostRecordsS64GetClientAccountTypeChecker3)));
                if ((temp312 == 0)) {
                    this.Manager.Comment("reaching state \'S361\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp310;
                    this.Manager.Comment("executing step \'call DsrDeregisterDnsHostRecords(PrimaryDc,InvalidDomainName,Null" +
                            ",DsaGuidOne,PrimaryDc)\'");
                    temp310 = this.INrpcServerAdapterInstance.DsrDeregisterDnsHostRecords(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.InvalidDomainName, ((Microsoft.Protocols.TestSuites.Nrpc.DomainGuidType)(0)), ((Microsoft.Protocols.TestSuites.Nrpc.DsaGuidType)(0)), Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc);
                    this.Manager.Checkpoint("MS-NRPC_R103453");
                    this.Manager.Comment("reaching state \'S500\'");
                    this.Manager.Comment("checking step \'return DsrDeregisterDnsHostRecords/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp310, "return of DsrDeregisterDnsHostRecords, state S500");
                    this.Manager.Comment("reaching state \'S575\'");
                    goto label85;
                }
                if ((temp312 == 1)) {
                    this.Manager.Comment("reaching state \'S362\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp311;
                    this.Manager.Comment("executing step \'call DsrDeregisterDnsHostRecords(PrimaryDc,InvalidDomainName,Null" +
                            ",DsaGuidOne,PrimaryDc)\'");
                    temp311 = this.INrpcServerAdapterInstance.DsrDeregisterDnsHostRecords(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.InvalidDomainName, ((Microsoft.Protocols.TestSuites.Nrpc.DomainGuidType)(0)), ((Microsoft.Protocols.TestSuites.Nrpc.DsaGuidType)(0)), Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc);
                    this.Manager.Checkpoint("MS-NRPC_R103453");
                    this.Manager.Comment("reaching state \'S501\'");
                    this.Manager.Comment("checking step \'return DsrDeregisterDnsHostRecords/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp311, "return of DsrDeregisterDnsHostRecords, state S501");
                    this.Manager.Comment("reaching state \'S576\'");
                    goto label85;
                }
                throw new InvalidOperationException("never reached");
            label85:
;
                goto label87;
            }
            if ((temp317 == 2)) {
                this.Manager.Comment("reaching state \'S146\'");
                bool temp313;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp313);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp313);
                this.Manager.Comment("reaching state \'S227\'");
                int temp316 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_LocateDc_DsrDeregisterDnsHostRecords.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_LocateDc_DsrDeregisterDnsHostRecordsS64GetClientAccountTypeChecker4)), new ExpectedReturn(Test_LocateDc_DsrDeregisterDnsHostRecords.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_LocateDc_DsrDeregisterDnsHostRecordsS64GetClientAccountTypeChecker5)));
                if ((temp316 == 0)) {
                    this.Manager.Comment("reaching state \'S363\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp314;
                    this.Manager.Comment("executing step \'call DsrDeregisterDnsHostRecords(PrimaryDc,InvalidDomainName,Null" +
                            ",DsaGuidOne,PrimaryDc)\'");
                    temp314 = this.INrpcServerAdapterInstance.DsrDeregisterDnsHostRecords(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.InvalidDomainName, ((Microsoft.Protocols.TestSuites.Nrpc.DomainGuidType)(0)), ((Microsoft.Protocols.TestSuites.Nrpc.DsaGuidType)(0)), Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc);
                    this.Manager.Checkpoint("MS-NRPC_R103453");
                    this.Manager.Comment("reaching state \'S502\'");
                    this.Manager.Comment("checking step \'return DsrDeregisterDnsHostRecords/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp314, "return of DsrDeregisterDnsHostRecords, state S502");
                    this.Manager.Comment("reaching state \'S577\'");
                    goto label86;
                }
                if ((temp316 == 1)) {
                    this.Manager.Comment("reaching state \'S364\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp315;
                    this.Manager.Comment("executing step \'call DsrDeregisterDnsHostRecords(PrimaryDc,InvalidDomainName,Null" +
                            ",DsaGuidOne,PrimaryDc)\'");
                    temp315 = this.INrpcServerAdapterInstance.DsrDeregisterDnsHostRecords(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.InvalidDomainName, ((Microsoft.Protocols.TestSuites.Nrpc.DomainGuidType)(0)), ((Microsoft.Protocols.TestSuites.Nrpc.DsaGuidType)(0)), Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc);
                    this.Manager.Checkpoint("MS-NRPC_R103453");
                    this.Manager.Comment("reaching state \'S503\'");
                    this.Manager.Comment("checking step \'return DsrDeregisterDnsHostRecords/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp315, "return of DsrDeregisterDnsHostRecords, state S503");
                    this.Manager.Comment("reaching state \'S578\'");
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
        
        private void Test_LocateDc_DsrDeregisterDnsHostRecordsS64GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S65");
        }
        
        private void Test_LocateDc_DsrDeregisterDnsHostRecordsS64GetClientAccountTypeChecker(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S225");
        }
        
        private void Test_LocateDc_DsrDeregisterDnsHostRecordsS64GetClientAccountTypeChecker1(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S225");
        }
        
        private void Test_LocateDc_DsrDeregisterDnsHostRecordsS64GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S65");
        }
        
        private void Test_LocateDc_DsrDeregisterDnsHostRecordsS64GetClientAccountTypeChecker2(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S226");
        }
        
        private void Test_LocateDc_DsrDeregisterDnsHostRecordsS64GetClientAccountTypeChecker3(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S226");
        }
        
        private void Test_LocateDc_DsrDeregisterDnsHostRecordsS64GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S65");
        }
        
        private void Test_LocateDc_DsrDeregisterDnsHostRecordsS64GetClientAccountTypeChecker4(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S227");
        }
        
        private void Test_LocateDc_DsrDeregisterDnsHostRecordsS64GetClientAccountTypeChecker5(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S227");
        }
        #endregion
        
        #region Test Starting in S66
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_LocateDc_DsrDeregisterDnsHostRecordsS66() {
            this.Manager.BeginTest("Test_LocateDc_DsrDeregisterDnsHostRecordsS66");
            this.Manager.Comment("reaching state \'S66\'");
            Microsoft.Protocols.TestSuites.Nrpc.PlatformType temp318;
            this.Manager.Comment("executing step \'call GetPlatform(out _)\'");
            this.INrpcServerAdapterInstance.GetPlatform(out temp318);
            this.Manager.AddReturn(GetPlatformInfo, null, temp318);
            this.Manager.Comment("reaching state \'S67\'");
            int temp331 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_LocateDc_DsrDeregisterDnsHostRecords.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_DsrDeregisterDnsHostRecordsS66GetPlatformChecker)), new ExpectedReturn(Test_LocateDc_DsrDeregisterDnsHostRecords.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_DsrDeregisterDnsHostRecordsS66GetPlatformChecker1)), new ExpectedReturn(Test_LocateDc_DsrDeregisterDnsHostRecords.GetPlatformInfo, null, new GetPlatformDelegate1(this.Test_LocateDc_DsrDeregisterDnsHostRecordsS66GetPlatformChecker2)));
            if ((temp331 == 0)) {
                this.Manager.Comment("reaching state \'S147\'");
                bool temp319;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp319);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp319);
                this.Manager.Comment("reaching state \'S228\'");
                int temp322 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_LocateDc_DsrDeregisterDnsHostRecords.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_LocateDc_DsrDeregisterDnsHostRecordsS66GetClientAccountTypeChecker)), new ExpectedReturn(Test_LocateDc_DsrDeregisterDnsHostRecords.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_LocateDc_DsrDeregisterDnsHostRecordsS66GetClientAccountTypeChecker1)));
                if ((temp322 == 0)) {
                    this.Manager.Comment("reaching state \'S365\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp320;
                    this.Manager.Comment("executing step \'call DsrDeregisterDnsHostRecords(PrimaryDc,FqdnFormatDomainName,P" +
                            "rimaryDomainGuid,DsaGuidOne,PrimaryDc)\'");
                    temp320 = this.INrpcServerAdapterInstance.DsrDeregisterDnsHostRecords(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.FqdnFormatDomainName, Microsoft.Protocols.TestSuites.Nrpc.DomainGuidType.PrimaryDomainGuid, ((Microsoft.Protocols.TestSuites.Nrpc.DsaGuidType)(0)), Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc);
                    this.Manager.Checkpoint("MS-NRPC_R103453");
                    this.Manager.Comment("reaching state \'S504\'");
                    this.Manager.Comment("checking step \'return DsrDeregisterDnsHostRecords/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp320, "return of DsrDeregisterDnsHostRecords, state S504");
                    Test_LocateDc_DsrDeregisterDnsHostRecordsS511();
                    goto label88;
                }
                if ((temp322 == 1)) {
                    this.Manager.Comment("reaching state \'S366\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp321;
                    this.Manager.Comment("executing step \'call DsrDeregisterDnsHostRecords(PrimaryDc,FqdnFormatDomainName,P" +
                            "rimaryDomainGuid,DsaGuidOne,PrimaryDc)\'");
                    temp321 = this.INrpcServerAdapterInstance.DsrDeregisterDnsHostRecords(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.FqdnFormatDomainName, Microsoft.Protocols.TestSuites.Nrpc.DomainGuidType.PrimaryDomainGuid, ((Microsoft.Protocols.TestSuites.Nrpc.DsaGuidType)(0)), Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc);
                    this.Manager.Checkpoint("MS-NRPC_R103453");
                    this.Manager.Comment("reaching state \'S505\'");
                    this.Manager.Comment("checking step \'return DsrDeregisterDnsHostRecords/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp321, "return of DsrDeregisterDnsHostRecords, state S505");
                    Test_LocateDc_DsrDeregisterDnsHostRecordsS510();
                    goto label88;
                }
                throw new InvalidOperationException("never reached");
            label88:
;
                goto label91;
            }
            if ((temp331 == 1)) {
                this.Manager.Comment("reaching state \'S148\'");
                bool temp323;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp323);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp323);
                this.Manager.Comment("reaching state \'S229\'");
                int temp326 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_LocateDc_DsrDeregisterDnsHostRecords.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_LocateDc_DsrDeregisterDnsHostRecordsS66GetClientAccountTypeChecker2)), new ExpectedReturn(Test_LocateDc_DsrDeregisterDnsHostRecords.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_LocateDc_DsrDeregisterDnsHostRecordsS66GetClientAccountTypeChecker3)));
                if ((temp326 == 0)) {
                    this.Manager.Comment("reaching state \'S367\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp324;
                    this.Manager.Comment("executing step \'call DsrDeregisterDnsHostRecords(PrimaryDc,InvalidDomainName,Null" +
                            ",DsaGuidOne,PrimaryDc)\'");
                    temp324 = this.INrpcServerAdapterInstance.DsrDeregisterDnsHostRecords(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.InvalidDomainName, ((Microsoft.Protocols.TestSuites.Nrpc.DomainGuidType)(0)), ((Microsoft.Protocols.TestSuites.Nrpc.DsaGuidType)(0)), Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc);
                    this.Manager.Checkpoint("MS-NRPC_R103453");
                    this.Manager.Comment("reaching state \'S506\'");
                    this.Manager.Comment("checking step \'return DsrDeregisterDnsHostRecords/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp324, "return of DsrDeregisterDnsHostRecords, state S506");
                    this.Manager.Comment("reaching state \'S579\'");
                    goto label89;
                }
                if ((temp326 == 1)) {
                    this.Manager.Comment("reaching state \'S368\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp325;
                    this.Manager.Comment("executing step \'call DsrDeregisterDnsHostRecords(PrimaryDc,InvalidDomainName,Null" +
                            ",DsaGuidOne,PrimaryDc)\'");
                    temp325 = this.INrpcServerAdapterInstance.DsrDeregisterDnsHostRecords(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.InvalidDomainName, ((Microsoft.Protocols.TestSuites.Nrpc.DomainGuidType)(0)), ((Microsoft.Protocols.TestSuites.Nrpc.DsaGuidType)(0)), Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc);
                    this.Manager.Checkpoint("MS-NRPC_R103453");
                    this.Manager.Comment("reaching state \'S507\'");
                    this.Manager.Comment("checking step \'return DsrDeregisterDnsHostRecords/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp325, "return of DsrDeregisterDnsHostRecords, state S507");
                    this.Manager.Comment("reaching state \'S580\'");
                    goto label89;
                }
                throw new InvalidOperationException("never reached");
            label89:
;
                goto label91;
            }
            if ((temp331 == 2)) {
                this.Manager.Comment("reaching state \'S149\'");
                bool temp327;
                this.Manager.Comment("executing step \'call GetClientAccountType(out _)\'");
                this.INrpcServerAdapterInstance.GetClientAccountType(out temp327);
                this.Manager.AddReturn(GetClientAccountTypeInfo, null, temp327);
                this.Manager.Comment("reaching state \'S230\'");
                int temp330 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(Test_LocateDc_DsrDeregisterDnsHostRecords.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_LocateDc_DsrDeregisterDnsHostRecordsS66GetClientAccountTypeChecker4)), new ExpectedReturn(Test_LocateDc_DsrDeregisterDnsHostRecords.GetClientAccountTypeInfo, null, new GetClientAccountTypeDelegate1(this.Test_LocateDc_DsrDeregisterDnsHostRecordsS66GetClientAccountTypeChecker5)));
                if ((temp330 == 0)) {
                    this.Manager.Comment("reaching state \'S369\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp328;
                    this.Manager.Comment("executing step \'call DsrDeregisterDnsHostRecords(PrimaryDc,InvalidDomainName,Null" +
                            ",DsaGuidOne,PrimaryDc)\'");
                    temp328 = this.INrpcServerAdapterInstance.DsrDeregisterDnsHostRecords(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.InvalidDomainName, ((Microsoft.Protocols.TestSuites.Nrpc.DomainGuidType)(0)), ((Microsoft.Protocols.TestSuites.Nrpc.DsaGuidType)(0)), Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc);
                    this.Manager.Checkpoint("MS-NRPC_R103453");
                    this.Manager.Comment("reaching state \'S508\'");
                    this.Manager.Comment("checking step \'return DsrDeregisterDnsHostRecords/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp328, "return of DsrDeregisterDnsHostRecords, state S508");
                    this.Manager.Comment("reaching state \'S581\'");
                    goto label90;
                }
                if ((temp330 == 1)) {
                    this.Manager.Comment("reaching state \'S370\'");
                    Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp329;
                    this.Manager.Comment("executing step \'call DsrDeregisterDnsHostRecords(PrimaryDc,InvalidDomainName,Null" +
                            ",DsaGuidOne,PrimaryDc)\'");
                    temp329 = this.INrpcServerAdapterInstance.DsrDeregisterDnsHostRecords(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.InvalidDomainName, ((Microsoft.Protocols.TestSuites.Nrpc.DomainGuidType)(0)), ((Microsoft.Protocols.TestSuites.Nrpc.DsaGuidType)(0)), Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc);
                    this.Manager.Checkpoint("MS-NRPC_R103453");
                    this.Manager.Comment("reaching state \'S509\'");
                    this.Manager.Comment("checking step \'return DsrDeregisterDnsHostRecords/ERROR_SUCCESS\'");
                    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, ((Microsoft.Protocols.TestSuites.Nrpc.HRESULT)(0)), temp329, "return of DsrDeregisterDnsHostRecords, state S509");
                    this.Manager.Comment("reaching state \'S582\'");
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
        
        private void Test_LocateDc_DsrDeregisterDnsHostRecordsS66GetPlatformChecker(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008R2]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008R2, sutPlatform, "sutPlatform of GetPlatform, state S67");
        }
        
        private void Test_LocateDc_DsrDeregisterDnsHostRecordsS66GetClientAccountTypeChecker(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S228");
        }
        
        private void Test_LocateDc_DsrDeregisterDnsHostRecordsS66GetClientAccountTypeChecker1(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S228");
        }
        
        private void Test_LocateDc_DsrDeregisterDnsHostRecordsS66GetPlatformChecker1(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out NonWindows]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.NonWindows, sutPlatform, "sutPlatform of GetPlatform, state S67");
        }
        
        private void Test_LocateDc_DsrDeregisterDnsHostRecordsS66GetClientAccountTypeChecker2(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S229");
        }
        
        private void Test_LocateDc_DsrDeregisterDnsHostRecordsS66GetClientAccountTypeChecker3(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S229");
        }
        
        private void Test_LocateDc_DsrDeregisterDnsHostRecordsS66GetPlatformChecker2(Microsoft.Protocols.TestSuites.Nrpc.PlatformType sutPlatform) {
            this.Manager.Comment("checking step \'return GetPlatform/[out WindowsServer2008]\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.PlatformType>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.PlatformType.WindowsServer2008, sutPlatform, "sutPlatform of GetPlatform, state S67");
        }
        
        private void Test_LocateDc_DsrDeregisterDnsHostRecordsS66GetClientAccountTypeChecker4(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out True]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, isAdministrator, "isAdministrator of GetClientAccountType, state S230");
        }
        
        private void Test_LocateDc_DsrDeregisterDnsHostRecordsS66GetClientAccountTypeChecker5(bool isAdministrator) {
            this.Manager.Comment("checking step \'return GetClientAccountType/[out False]\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, isAdministrator, "isAdministrator of GetClientAccountType, state S230");
        }
        #endregion
        
        #region Test Starting in S68
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_LocateDc_DsrDeregisterDnsHostRecordsS68() {
            this.Manager.BeginTest("Test_LocateDc_DsrDeregisterDnsHostRecordsS68");
            this.Manager.Comment("reaching state \'S68\'");
            this.Manager.Comment("executing step \'call SwitchUserAccount(False)\'");
            this.INrpcServerAdapterInstance.SwitchUserAccount(false);
            this.Manager.Comment("reaching state \'S69\'");
            this.Manager.Comment("checking step \'return SwitchUserAccount\'");
            this.Manager.Comment("reaching state \'S150\'");
            Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp332;
            this.Manager.Comment("executing step \'call DsrDeregisterDnsHostRecords(PrimaryDc,FqdnFormatDomainName,T" +
                    "rustedDomainGuid,DsaGuidOne,PrimaryDc)\'");
            temp332 = this.INrpcServerAdapterInstance.DsrDeregisterDnsHostRecords(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.FqdnFormatDomainName, Microsoft.Protocols.TestSuites.Nrpc.DomainGuidType.TrustedDomainGuid, ((Microsoft.Protocols.TestSuites.Nrpc.DsaGuidType)(0)), Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc);
            this.Manager.Checkpoint("MS-NRPC_R103328");
            this.Manager.Checkpoint("MS-NRPC_R103454");
            this.Manager.Comment("reaching state \'S231\'");
            this.Manager.Comment("checking step \'return DsrDeregisterDnsHostRecords/ERROR_ACCESS_DENIED\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_ACCESS_DENIED, temp332, "return of DsrDeregisterDnsHostRecords, state S231");
            Test_LocateDc_DsrDeregisterDnsHostRecordsS238();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S8
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void Test_LocateDc_DsrDeregisterDnsHostRecordsS8() {
            this.Manager.BeginTest("Test_LocateDc_DsrDeregisterDnsHostRecordsS8");
            this.Manager.Comment("reaching state \'S8\'");
            this.Manager.Comment("executing step \'call SwitchUserAccount(False)\'");
            this.INrpcServerAdapterInstance.SwitchUserAccount(false);
            this.Manager.Comment("reaching state \'S9\'");
            this.Manager.Comment("checking step \'return SwitchUserAccount\'");
            this.Manager.Comment("reaching state \'S76\'");
            Microsoft.Protocols.TestSuites.Nrpc.HRESULT temp333;
            this.Manager.Comment("executing step \'call DsrDeregisterDnsHostRecords(PrimaryDc,InvalidDomainName,Trus" +
                    "tedDomainGuid,DsaGuidOne,PrimaryDc)\'");
            temp333 = this.INrpcServerAdapterInstance.DsrDeregisterDnsHostRecords(Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc, Microsoft.Protocols.TestSuites.Nrpc.DomainNameType.InvalidDomainName, Microsoft.Protocols.TestSuites.Nrpc.DomainGuidType.TrustedDomainGuid, ((Microsoft.Protocols.TestSuites.Nrpc.DsaGuidType)(0)), Microsoft.Protocols.TestSuites.Nrpc.ComputerType.PrimaryDc);
            this.Manager.Checkpoint("MS-NRPC_R103328");
            this.Manager.Checkpoint("MS-NRPC_R103454");
            this.Manager.Comment("reaching state \'S157\'");
            this.Manager.Comment("checking step \'return DsrDeregisterDnsHostRecords/ERROR_ACCESS_DENIED\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.Nrpc.HRESULT>(this.Manager, Microsoft.Protocols.TestSuites.Nrpc.HRESULT.ERROR_ACCESS_DENIED, temp333, "return of DsrDeregisterDnsHostRecords, state S157");
            Test_LocateDc_DsrDeregisterDnsHostRecordsS238();
            this.Manager.EndTest();
        }
        #endregion
    }
}
